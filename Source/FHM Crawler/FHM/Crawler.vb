#Region " Imports "

Imports System.Collections.Specialized
Imports System.Globalization
Imports System.Net
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Web

Imports HtmlDocument = HtmlAgilityPack.HtmlDocument
Imports HtmlNode = HtmlAgilityPack.HtmlNode
Imports HtmlNodeCollection = HtmlAgilityPack.HtmlNodeCollection

Imports DevCase.Core.Extensions.NameValueCollection

#End Region

Namespace FHM

    ' Un simple crawler para recolectar las urls de descarga de los álbumes de Rock de la página http://freehardmusic.com/.

    Public NotInheritable Class Crawler

#Region " Private Fields "

        Private ReadOnly uriBase As New Uri("http://freehardmusic.com/")
        Private ReadOnly uriIndex As New Uri(Me.uriBase, "/index.php")
        Private ReadOnly uriIndex2 As New Uri(Me.uriBase, "/index2.php")

        Private cancelToken As CancellationToken
        Private cancelTokenSrc As CancellationTokenSource
        Private isFetching As Boolean
        Private ReadOnly client As CookieAwareWebClient

#End Region

#Region " Properties "

        Public ReadOnly Property SearchFilter As SearchFilter
        Public Property Cookies As CookieContainer

#End Region

#Region " Events "

        Public Event BeginPageCrawl As EventHandler(Of PageCrawlBeginEventArgs)
        Public Event EndPageCrawl As EventHandler(Of PageCrawlEndEventArgs)

#End Region

#Region " Constructors "

        Public Sub New(cookies As CookieContainer)
            Me.Cookies = cookies

            Me.client = New CookieAwareWebClient With {
                .Cookies = Me.Cookies
            }

            Me.SearchFilter = New SearchFilter()
            Me.cancelTokenSrc = New CancellationTokenSource()
            Me.cancelToken = Me.cancelTokenSrc.Token
        End Sub

#End Region

#Region " Public Methods "

        Public Function GetAlbumCount() As Integer
            Dim t As Task(Of Integer) = Task.Run(Of Integer)(AddressOf Me.GetAlbumCountAsync)
            t.Wait()

            Return t.Result
        End Function

        Public Async Function GetAlbumCountAsync() As Task(Of Integer)
            Dim searchQueary As String = Me.GetSearchQueary(pageindex:=0)

            Dim uriSearch As New Uri(searchQueary)
            Dim htmlSourceCode As String = Await Me.client.DownloadStringTaskAsync(uriSearch)

            Dim htmldoc As New HtmlDocument
            htmldoc.LoadHtml(htmlSourceCode)

            Dim xPathResultString As String = "//div[@id='content']/div[@id='content_container']/div[@class='floatbox']/table[1]/tr[2]/td"

            Dim node As HtmlNode = htmldoc.DocumentNode.SelectSingleNode(xPathResultString)
            If node Is Nothing Then
                Return 0
            End If

            Dim text As String = node.InnerText
            text = Regex.Replace(text, "\n", "", RegexOptions.None)    ' Remove new lines.
            text = Regex.Replace(text, "\t", " "c, RegexOptions.None)  ' Replace tabs for white-spaces.
            text = Regex.Replace(text, "\s+", " "c, RegexOptions.None) ' Replace duplicated white-spaces.

            Dim albumCount As Integer = CInt(Regex.Match(text, "\d+", RegexOptions.None).Value)
            Return albumCount

        End Function

        Public Sub FetchAlbums()
            Dim t As Task = Task.Run(AddressOf Me.FetchAlbumsAsync)
            t.Wait()
        End Sub

        Public Async Function FetchAlbumsAsync() As Task(Of Boolean)
            If (Me.isFetching) Then
                Throw New Exception("Another fetch operation is already running in background.")
            End If
            Me.isFetching = True

            Me.cancelTokenSrc.Dispose()
            Me.cancelTokenSrc = New CancellationTokenSource()
            Me.cancelToken = Me.cancelTokenSrc.Token

            Dim albumCount As Integer = Await Me.GetAlbumCountAsync()
            If (albumCount = 0) Then
                Me.isFetching = False
                Return True
            End If

            Dim maxPages As Integer = ((albumCount \ 10) + 1) ' 10 albums per page.
            For i As Integer = 0 To (maxPages - 1)
                Dim searchQueary As String = Me.GetSearchQueary(pageindex:=i)
                Dim uriSearch As New Uri(searchQueary)
                Dim htmlSourceCode As String = Await Me.client.DownloadStringTaskAsync(uriSearch)

                If (Me.cancelToken.IsCancellationRequested) Then
                    Me.isFetching = False
                    Return False
                End If

                RaiseEvent BeginPageCrawl(Me, New PageCrawlBeginEventArgs(i))
                Await Me.ParseHtmlSourceCode(i, htmlSourceCode)
            Next i

            Me.isFetching = False
            Return True
        End Function

        Public Sub CancelFetchAlbumsAsync()
            If Not (Me.isFetching) Then
                Throw New Exception("No fetch operation is running.")
            End If

            If (Me.cancelToken.IsCancellationRequested) Then
                ' Handle redundant cancellation calls to CancelFetchAlbums()...
                Me.cancelToken.ThrowIfCancellationRequested()
            End If

            Me.cancelTokenSrc.Cancel()
        End Sub

        Public Sub ResetSearchFilters()
            Me.SearchFilter.Artist = ""
            Me.SearchFilter.Country = "all"
            Me.SearchFilter.Genre = "all"
            Me.SearchFilter.Year = "all"
        End Sub

#End Region

#Region " Private Methods "

        Private Function GetSearchQueary(pageindex As Integer) As String
            Dim searchParams As New NameValueCollection From {
                    {"field_band", Me.SearchFilter.Artist},
                    {"field_country", Me.SearchFilter.Country},
                    {"field_genre", Me.SearchFilter.Genre},
                    {"field_year", Me.SearchFilter.Year},
                    {"field_format", "0"},
                    {"option", "com_sobi2"},
                    {"search", "Search"},
                    {"searchphrase", "exact"},
                    {"sobi2Search", ""},
                    {"sobi2Task", "axSearch"},
                    {"SobiCatSelected_0", "0"},
                    {"sobiCid", "0"},
                    {"reset", "2"},
                    {"Itemid", "13"},
                    {"SobiSearchPage", CStr(pageindex)}
                }

            Return searchParams.ToQueryString(Me.uriIndex)
        End Function

        Private Async Function ParseHtmlSourceCode(pageindex As Integer, htmlSourceCode As String) As Task(Of Boolean)

            Dim albums As New List(Of AlbumInfo)

            Dim xPathTable As String = "//table[@class='vicard']"
            Dim xPathArtist As String = ".//tr/td/span[@class='sobi2Listing_field_band']"
            Dim xPathTitle As String = ".//tr/td/p[@class='sobi2ItemTitle']/a[@title]"
            Dim xPathUrl As String = ".//table[@class='vicard2']/tr/td/a[@href]"

            Dim htmldoc As New HtmlDocument
            ' Try
            htmldoc.LoadHtml(htmlSourceCode)
            ' Catch ex As Exception
            '     Return False
            ' End Try

            Dim nodes As HtmlNodeCollection = htmldoc.DocumentNode.SelectNodes(xPathTable)
            If (nodes.Count = 0) Then
                Return False
            End If

            For Each node As HtmlNode In nodes
                Dim artist As String
                Dim title As String
                Dim albumUrl As String

                Try
                    artist = node.SelectSingleNode(xPathArtist).InnerText.Trim(" "c)
                Catch ex As Exception
                    artist = "unknown"
                End Try
                artist = Encoding.UTF8.GetString(Encoding.Default.GetBytes(artist))
                artist = HttpUtility.HtmlDecode(artist)
                artist = New CultureInfo("en-US").TextInfo.ToTitleCase(artist)

                Try
                    title = node.SelectSingleNode(xPathTitle).GetAttributeValue("title", "").Trim(" "c)
                Catch ex As Exception
                    title = "unknown"
                End Try
                title = Encoding.UTF8.GetString(Encoding.Default.GetBytes(title))
                title = HttpUtility.HtmlDecode(title)
                title = New CultureInfo("en-US").TextInfo.ToTitleCase(title)

                Try
                    albumUrl = node.SelectSingleNode(xPathUrl).GetAttributeValue("href", "").Trim(" "c)
                Catch ex As Exception
                    Continue For
                End Try
                albumUrl = HttpUtility.UrlDecode(albumUrl)

                Dim downloadUrlParams As New NameValueCollection From {
                        {"sobiid", HttpUtility.ParseQueryString(New Uri(albumUrl).Query)("amp;sobi2Id")},
                        {"sobi2Task", "addSRev"},
                        {"no_html", "1"},
                        {"option", "com_sobi2"},
                        {"rvote", "1"}
                    }

                Dim downloadUrls As List(Of String)
                Try
                    htmlSourceCode = Await Me.client.DownloadStringTaskAsync(New Uri(downloadUrlParams.ToQueryString(Me.uriIndex2)))

                    Dim xDoc As XDocument = XDocument.Parse(htmlSourceCode)
                    Dim elements As IEnumerable(Of XElement) = xDoc.<rev>
                    downloadUrls = New List(Of String) From {
                        elements.<msg>.Value,
                        elements.<msg2>.Value,
                        elements.<msg3>.Value,
                        elements.<msg4>.Value,
                        elements.<msg5>.Value,
                        elements.<msg6>.Value,
                        elements.<msg7>.Value,
                        elements.<msg8>.Value,
                        elements.<msg9>.Value,
                        elements.<msg10>.Value,
                        elements.<msg11>.Value,
                        elements.<msg12>.Value,
                        elements.<msg13>.Value
                    }
                Catch ex As Exception
                    Continue For
                End Try

                downloadUrls = (From item As String In downloadUrls
                                Where Not String.IsNullOrWhiteSpace(item)
                                Select item.TrimEnd(" "c)
                               ).ToList()

                albums.Add(New AlbumInfo(artist, title, downloadUrls.ToArray()))
                downloadUrls.Clear()
            Next node

            RaiseEvent EndPageCrawl(Me, New PageCrawlEndEventArgs(pageindex, albums))
            albums.Clear()

            Return True
        End Function

#End Region

    End Class

End Namespace
