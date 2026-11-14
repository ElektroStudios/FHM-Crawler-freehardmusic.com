#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Globalization
Imports System.Net
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Web
Imports System.Xml
Imports System.Xml.Linq

Imports HtmlDocument = HtmlAgilityPack.HtmlDocument
Imports HtmlNode = HtmlAgilityPack.HtmlNode
Imports HtmlNodeCollection = HtmlAgilityPack.HtmlNodeCollection

Imports DevCase.Core.Extensions.NameValueCollection

#End Region

#Region " Crawler "

Namespace FHM

    ''' <summary>
    ''' A simple crawler that collects the download urls of the Rock albums published on the
    ''' <c>http://freehardmusic.com/</c> website.
    ''' </summary>
    Public NotInheritable Class Crawler

#Region " Constants "

        ''' <summary>
        ''' The amount of albums that the FHM website lists in every search results page.
        ''' </summary>
        Friend Const AlbumsPerPage As Integer = 10

        ''' <summary>
        ''' The fallback text used when the artist or the title of an album cannot be found in the HTML.
        ''' </summary>
        Friend Const UnknownFieldValue As String = "unknown"

        ''' <summary>
        ''' XPath expression that selects the node containing the search result summary (the album count).
        ''' </summary>
        Private Const XPathAlbumCount As String = "//div[@id='content']/div[@id='content_container']/div[@class='floatbox']/table[1]/tr[2]/td"

        ''' <summary>
        ''' XPath expression that selects the table of every album listed in a search results page.
        ''' </summary>
        Private Const XPathAlbumTable As String = "//table[@class='vicard']"

        ''' <summary>
        ''' XPath expression (relative to an album table) that selects the artist name node.
        ''' </summary>
        Private Const XPathAlbumArtist As String = ".//tr/td/span[@class='sobi2Listing_field_band']"

        ''' <summary>
        ''' XPath expression (relative to an album table) that selects the album title node.
        ''' </summary>
        Private Const XPathAlbumTitle As String = ".//tr/td/p[@class='sobi2ItemTitle']/a[@title]"

        ''' <summary>
        ''' XPath expression (relative to an album table) that selects the album details link node.
        ''' </summary>
        Private Const XPathAlbumUrl As String = ".//table[@class='vicard2']/tr/td/a[@href]"

#End Region

#Region " Private Fields "

        ''' <summary>
        ''' The base address of the FHM website.
        ''' </summary>
        Friend Shared ReadOnly UriBase As New Uri("http://freehardmusic.com/")

        ''' <summary>
        ''' The address of the FHM search page.
        ''' </summary>
        Friend Shared ReadOnly UriIndex As New Uri(Crawler.UriBase, "/index.php")

        ''' <summary>
        ''' The address of the FHM page that returns the download links of an album (as XML).
        ''' </summary>
        Friend Shared ReadOnly UriIndex2 As New Uri(Crawler.UriBase, "/index2.php")

        ''' <summary>
        ''' The names of the XML elements that contain the download links of an album, in order.
        ''' </summary>
        Private Shared ReadOnly DownloadUrlElementNames As String() = {
            "msg", "msg2", "msg3", "msg4", "msg5", "msg6", "msg7",
            "msg8", "msg9", "msg10", "msg11", "msg12", "msg13"
        }

        ''' <summary>
        ''' The culture used to convert artist names and album titles to title case.
        ''' </summary>
        Private Shared ReadOnly TitleCaseCulture As CultureInfo = CultureInfo.GetCultureInfo("en-US")

        ''' <summary>
        ''' The web client used to perform the HTTP requests.
        ''' </summary>
        Private ReadOnly client As CookieAwareWebClient

        ''' <summary>
        ''' The delegate that downloads the content of an <see cref="Uri"/> as a string.
        ''' <para></para>
        ''' By default it points to <see cref="WebClient.DownloadStringTaskAsync(Uri)"/>;
        ''' unit tests replace it to avoid network access.
        ''' </summary>
        Private ReadOnly downloadStringAsync As Func(Of Uri, Task(Of String))

        ''' <summary>
        ''' Backing field of the <see cref="Crawler.Cookies"/> property.
        ''' </summary>
        Private cookiesValue As CookieContainer

        ''' <summary>
        ''' The source of the cancellation token of the current fetch operation.
        ''' </summary>
        Private cancelTokenSrc As CancellationTokenSource

        ''' <summary>
        ''' The cancellation token of the current fetch operation.
        ''' </summary>
        Private cancelToken As CancellationToken

        ''' <summary>
        ''' A flag (<c>0</c> or <c>1</c>) that indicates whether a fetch operation is running.
        ''' <para></para>
        ''' It is an <see cref="Integer"/> to allow atomic updates through <see cref="Interlocked"/>.
        ''' </summary>
        Private fetchingFlag As Integer

        ''' <summary>
        ''' The <see cref="SynchronizationContext"/> captured when the current fetch operation started.
        ''' <para></para>
        ''' The <see cref="Crawler.BeginPageCrawl"/> and <see cref="Crawler.EndPageCrawl"/> events are raised on it,
        ''' so UI event handlers keep running on the UI thread even though the crawler uses <c>ConfigureAwait(False)</c>.
        ''' </summary>
        Private eventContext As SynchronizationContext

#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets the search filter used by the crawler.
        ''' </summary>
        Public ReadOnly Property SearchFilter As SearchFilter

        ''' <summary>
        ''' Gets or sets the session cookies sent to the FHM website.
        ''' </summary>
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when the value is <see langword="Nothing"/>.
        ''' </exception>
        Public Property Cookies As CookieContainer
            Get
                Return Me.cookiesValue
            End Get
            Set(value As CookieContainer)
                If value Is Nothing Then
                    Throw New ArgumentNullException(paramName:=NameOf(value))
                End If
                Me.cookiesValue = value
                Me.client.Cookies = value
            End Set
        End Property

        ''' <summary>
        ''' Gets a value indicating whether a fetch operation is currently running.
        ''' </summary>
        Public ReadOnly Property IsFetching As Boolean
            Get
                Return Volatile.Read(Me.fetchingFlag) = 1
            End Get
        End Property

#End Region

#Region " Events "

        ''' <summary>
        ''' Occurs when the crawler begins parsing a search results page.
        ''' </summary>
        Public Event BeginPageCrawl As EventHandler(Of PageCrawlBeginEventArgs)

        ''' <summary>
        ''' Occurs when the crawler finishes parsing a search results page.
        ''' </summary>
        Public Event EndPageCrawl As EventHandler(Of PageCrawlEndEventArgs)

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="Crawler"/> class.
        ''' </summary>
        ''' <param name="cookies">
        ''' The session cookies sent to the FHM website.
        ''' </param>
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when <paramref name="cookies"/> is <see langword="Nothing"/>.
        ''' </exception>
        Public Sub New(cookies As CookieContainer)
            If cookies Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(cookies))
            End If

            Me.client = New CookieAwareWebClient With {
                .Cookies = cookies
            }
            Me.cookiesValue = cookies
            Me.downloadStringAsync = Function(address As Uri) Me.client.DownloadStringTaskAsync(address)

            Me.SearchFilter = New SearchFilter()
            Me.cancelTokenSrc = New CancellationTokenSource()
            Me.cancelToken = Me.cancelTokenSrc.Token
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="Crawler"/> class
        ''' that downloads the web content through the specified delegate.
        ''' <para></para>
        ''' This constructor exists for unit testing purposes.
        ''' </summary>
        ''' <param name="cookies">
        ''' The session cookies sent to the FHM website.
        ''' </param>
        '''
        ''' <param name="downloadStringAsync">
        ''' The delegate that downloads the content of an <see cref="Uri"/> as a string.
        ''' </param>
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when <paramref name="cookies"/> or <paramref name="downloadStringAsync"/> is <see langword="Nothing"/>.
        ''' </exception>
        Friend Sub New(cookies As CookieContainer, downloadStringAsync As Func(Of Uri, Task(Of String)))
            Me.New(cookies)

            If downloadStringAsync Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(downloadStringAsync))
            End If
            Me.downloadStringAsync = downloadStringAsync
        End Sub

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Gets the amount of albums that match the current <see cref="Crawler.SearchFilter"/>.
        ''' </summary>
        ''' <returns>
        ''' The amount of albums that match the current search filter.
        ''' </returns>
        Public Function GetAlbumCount() As Integer
            Dim t As Task(Of Integer) = Task.Run(Of Integer)(AddressOf Me.GetAlbumCountAsync)
            t.Wait()

            Return t.Result
        End Function

        ''' <summary>
        ''' Asynchronously gets the amount of albums that match the current <see cref="Crawler.SearchFilter"/>.
        ''' </summary>
        ''' <returns>
        ''' The amount of albums that match the current search filter.
        ''' </returns>
        Public Async Function GetAlbumCountAsync() As Task(Of Integer)
            Dim uriSearch As New Uri(Crawler.BuildSearchQuery(Me.SearchFilter, pageIndex:=0))
            Dim htmlSourceCode As String = Await Me.DownloadAsync(uriSearch).ConfigureAwait(False)

            Return Crawler.ParseAlbumCount(htmlSourceCode)
        End Function

        ''' <summary>
        ''' Fetches all the albums that match the current <see cref="Crawler.SearchFilter"/>.
        ''' </summary>
        Public Sub FetchAlbums()
            Dim t As Task = Task.Run(AddressOf Me.FetchAlbumsAsync)
            t.Wait()
        End Sub

        ''' <summary>
        ''' Asynchronously fetches all the albums that match the current <see cref="Crawler.SearchFilter"/>.
        ''' <para></para>
        ''' The albums of every page are delivered through the <see cref="Crawler.EndPageCrawl"/> event.
        ''' </summary>
        ''' <returns>
        ''' <see langword="True"/> if the operation completed;
        ''' <see langword="False"/> if it was cancelled by a call to <see cref="Crawler.CancelFetchAlbumsAsync"/>.
        ''' </returns>
        ''' <exception cref="InvalidOperationException">
        ''' Thrown when another fetch operation is already running.
        ''' </exception>
        Public Async Function FetchAlbumsAsync() As Task(Of Boolean)
            If Interlocked.CompareExchange(Me.fetchingFlag, 1, 0) <> 0 Then
                Throw New InvalidOperationException("Another fetch operation is already running in background.")
            End If

            ' The flag must be reset whatever happens (network errors included);
            ' otherwise every subsequent fetch operation would be rejected until the application is restarted.
            Try
                Me.cancelTokenSrc.Dispose()
                Me.cancelTokenSrc = New CancellationTokenSource()
                Me.cancelToken = Me.cancelTokenSrc.Token
                Me.eventContext = SynchronizationContext.Current

                Dim albumCount As Integer = Await Me.GetAlbumCountAsync().ConfigureAwait(False)
                Dim pageCount As Integer = Crawler.GetPageCount(albumCount)

                For pageIndex As Integer = 0 To (pageCount - 1)
                    Dim currentPageIndex As Integer = pageIndex
                    Dim uriSearch As New Uri(Crawler.BuildSearchQuery(Me.SearchFilter, currentPageIndex))
                    Dim htmlSourceCode As String = Await Me.DownloadAsync(uriSearch).ConfigureAwait(False)

                    If Me.cancelToken.IsCancellationRequested Then
                        Return False
                    End If

                    Me.InvokeOnEventContext(Sub() RaiseEvent BeginPageCrawl(Me, New PageCrawlBeginEventArgs(currentPageIndex)))

                    Dim albums As List(Of AlbumInfo) = Await Me.FetchPageAlbumsAsync(htmlSourceCode).ConfigureAwait(False)

                    Me.InvokeOnEventContext(Sub() RaiseEvent EndPageCrawl(Me, New PageCrawlEndEventArgs(currentPageIndex, albums)))
                Next pageIndex

                Return True

            Finally
                Me.eventContext = Nothing
                Interlocked.Exchange(Me.fetchingFlag, 0)

            End Try
        End Function

        ''' <summary>
        ''' Requests the cancellation of the running fetch operation.
        ''' </summary>
        ''' <exception cref="InvalidOperationException">
        ''' Thrown when no fetch operation is running.
        ''' </exception>
        '''
        ''' <exception cref="OperationCanceledException">
        ''' Thrown when the cancellation of the running fetch operation was already requested.
        ''' </exception>
        Public Sub CancelFetchAlbumsAsync()
            If Not Me.IsFetching Then
                Throw New InvalidOperationException("No fetch operation is running.")
            End If

            ' Handle redundant cancellation calls.
            Me.cancelToken.ThrowIfCancellationRequested()

            Me.cancelTokenSrc.Cancel()
        End Sub

        ''' <summary>
        ''' Resets the <see cref="Crawler.SearchFilter"/> to its default values.
        ''' </summary>
        Public Sub ResetSearchFilters()
            Me.SearchFilter.Artist = ""
            Me.SearchFilter.Country = "all"
            Me.SearchFilter.Genre = "all"
            Me.SearchFilter.Year = "all"
        End Sub

#End Region

#Region " Friend Methods (pure functions, unit tested) "

        ''' <summary>
        ''' Builds the address of a search results page for the specified search filter.
        ''' </summary>
        ''' <param name="filter">
        ''' The search filter.
        ''' </param>
        '''
        ''' <param name="pageIndex">
        ''' The zero-based index of the search results page.
        ''' </param>
        ''' <returns>
        ''' The absolute address of the search results page.
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when <paramref name="filter"/> is <see langword="Nothing"/>.
        ''' </exception>
        '''
        ''' <exception cref="ArgumentOutOfRangeException">
        ''' Thrown when <paramref name="pageIndex"/> is negative.
        ''' </exception>
        Friend Shared Function BuildSearchQuery(filter As SearchFilter, pageIndex As Integer) As String
            If filter Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(filter))
            End If
            If pageIndex < 0 Then
                Throw New ArgumentOutOfRangeException(paramName:=NameOf(pageIndex), actualValue:=pageIndex, message:="The page index cannot be negative.")
            End If

            Dim searchParams As New NameValueCollection From {
                {"field_band", filter.Artist},
                {"field_country", filter.Country},
                {"field_genre", filter.Genre},
                {"field_year", filter.Year},
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
                {"SobiSearchPage", pageIndex.ToString(CultureInfo.InvariantCulture)}
            }

            Return searchParams.ToQueryString(Crawler.UriIndex)
        End Function

        ''' <summary>
        ''' Builds the address of the XML document that contains the download links of an album.
        ''' </summary>
        ''' <param name="sobi2Id">
        ''' The FHM identifier of the album (the <c>sobi2Id</c> query parameter of its details page).
        ''' </param>
        ''' <returns>
        ''' The absolute address of the XML document.
        ''' </returns>
        ''' <exception cref="ArgumentException">
        ''' Thrown when <paramref name="sobi2Id"/> is <see langword="Nothing"/>, empty or white-space.
        ''' </exception>
        Friend Shared Function BuildDownloadUrlsQuery(sobi2Id As String) As String
            If String.IsNullOrWhiteSpace(sobi2Id) Then
                Throw New ArgumentException("The album identifier cannot be null, empty or white-space.", paramName:=NameOf(sobi2Id))
            End If

            Dim downloadUrlParams As New NameValueCollection From {
                {"sobiid", sobi2Id},
                {"sobi2Task", "addSRev"},
                {"no_html", "1"},
                {"option", "com_sobi2"},
                {"rvote", "1"}
            }

            Return downloadUrlParams.ToQueryString(Crawler.UriIndex2)
        End Function

        ''' <summary>
        ''' Gets the amount of search results pages needed to list the specified amount of albums.
        ''' </summary>
        ''' <param name="albumCount">
        ''' The amount of albums.
        ''' </param>
        ''' <returns>
        ''' The amount of search results pages.
        ''' </returns>
        ''' <exception cref="ArgumentOutOfRangeException">
        ''' Thrown when <paramref name="albumCount"/> is negative.
        ''' </exception>
        Friend Shared Function GetPageCount(albumCount As Integer) As Integer
            If albumCount < 0 Then
                Throw New ArgumentOutOfRangeException(paramName:=NameOf(albumCount), actualValue:=albumCount, message:="The album count cannot be negative.")
            End If

            Dim fullPages As Integer = albumCount \ Crawler.AlbumsPerPage
            Dim hasPartialPage As Boolean = (albumCount Mod Crawler.AlbumsPerPage) <> 0

            Return If(hasPartialPage, fullPages + 1, fullPages)
        End Function

        ''' <summary>
        ''' Parses the album count from the HTML source code of a search results page.
        ''' </summary>
        ''' <param name="htmlSourceCode">
        ''' The HTML source code of the search results page.
        ''' </param>
        ''' <returns>
        ''' The album count, or <c>0</c> if the page does not contain the search result summary.
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when <paramref name="htmlSourceCode"/> is <see langword="Nothing"/>.
        ''' </exception>
        '''
        ''' <exception cref="FormatException">
        ''' Thrown when the search result summary does not contain any number.
        ''' </exception>
        Friend Shared Function ParseAlbumCount(htmlSourceCode As String) As Integer
            If htmlSourceCode Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(htmlSourceCode))
            End If

            Dim htmldoc As New HtmlDocument()
            htmldoc.LoadHtml(htmlSourceCode)

            ' The website omits the search result summary when the search yields no results.
            Dim node As HtmlNode = htmldoc.DocumentNode.SelectSingleNode(Crawler.XPathAlbumCount)
            If node Is Nothing Then
                Return 0
            End If

            Dim text As String = Regex.Replace(node.InnerText, "\s+", " ", RegexOptions.None).Trim()
            Dim match As Match = Regex.Match(text, "\d+", RegexOptions.None)
            If Not match.Success Then
                Throw New FormatException($"The album count could not be found in the search result summary: '{text}'.")
            End If

            Return Integer.Parse(match.Value, NumberStyles.None, CultureInfo.InvariantCulture)
        End Function

        ''' <summary>
        ''' Parses the album listings from the HTML source code of a search results page.
        ''' <para></para>
        ''' Listings without a valid details link are skipped.
        ''' </summary>
        ''' <param name="htmlSourceCode">
        ''' The HTML source code of the search results page.
        ''' </param>
        ''' <returns>
        ''' The album listings found in the page (an empty list if the page does not list any album).
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when <paramref name="htmlSourceCode"/> is <see langword="Nothing"/>.
        ''' </exception>
        Friend Shared Function ParseAlbumListings(htmlSourceCode As String) As List(Of AlbumListing)
            If htmlSourceCode Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(htmlSourceCode))
            End If

            Dim htmldoc As New HtmlDocument()
            htmldoc.LoadHtml(htmlSourceCode)

            Dim listings As New List(Of AlbumListing)

            ' HtmlAgilityPack returns Nothing (not an empty collection) when no node matches the XPath expression.
            Dim nodes As HtmlNodeCollection = htmldoc.DocumentNode.SelectNodes(Crawler.XPathAlbumTable)
            If nodes Is Nothing Then
                Return listings
            End If

            For Each node As HtmlNode In nodes
                Dim urlNode As HtmlNode = node.SelectSingleNode(Crawler.XPathAlbumUrl)
                If urlNode Is Nothing Then
                    Continue For
                End If

                Dim sobi2Id As String = Nothing
                If Not Crawler.TryExtractSobi2Id(urlNode.GetAttributeValue("href", ""), sobi2Id) Then
                    Continue For
                End If

                Dim artistNode As HtmlNode = node.SelectSingleNode(Crawler.XPathAlbumArtist)
                Dim rawArtist As String = If(artistNode Is Nothing, Crawler.UnknownFieldValue, artistNode.InnerText)

                Dim titleNode As HtmlNode = node.SelectSingleNode(Crawler.XPathAlbumTitle)
                Dim rawTitle As String = If(titleNode Is Nothing, Crawler.UnknownFieldValue, titleNode.GetAttributeValue("title", ""))

                listings.Add(New AlbumListing(Crawler.NormalizeText(rawArtist), Crawler.NormalizeText(rawTitle), sobi2Id))
            Next node

            Return listings
        End Function

        ''' <summary>
        ''' Tries to extract the FHM album identifier (the <c>sobi2Id</c> query parameter) from an album details link.
        ''' </summary>
        ''' <param name="href">
        ''' The <c>href</c> attribute value of the album details link.
        ''' It can be absolute or relative, with or without HTML-encoded ampersands (<c>&amp;amp;</c>).
        ''' </param>
        '''
        ''' <param name="refSobi2Id">
        ''' When this method returns <see langword="True"/>, contains the album identifier.
        ''' </param>
        ''' <returns>
        ''' <see langword="True"/> if the album identifier was found; otherwise, <see langword="False"/>.
        ''' </returns>
        Friend Shared Function TryExtractSobi2Id(href As String, ByRef refSobi2Id As String) As Boolean
            refSobi2Id = Nothing
            If String.IsNullOrWhiteSpace(href) Then
                Return False
            End If

            Dim decodedHref As String = HttpUtility.HtmlDecode(href.Trim())
            Dim albumUri As Uri = Nothing
            If Not Uri.TryCreate(Crawler.UriBase, decodedHref, albumUri) Then
                Return False
            End If

            Dim sobi2Id As String = HttpUtility.ParseQueryString(albumUri.Query)("sobi2Id")
            If String.IsNullOrWhiteSpace(sobi2Id) Then
                Return False
            End If

            refSobi2Id = sobi2Id.Trim()
            Return True
        End Function

        ''' <summary>
        ''' Normalizes an artist name or an album title scraped from the FHM website:
        ''' trims it, fixes the UTF-8 text that was decoded with the system ANSI code page,
        ''' decodes HTML entities and converts it to title case.
        ''' </summary>
        ''' <param name="rawText">
        ''' The raw text.
        ''' </param>
        ''' <returns>
        ''' The normalized text.
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when <paramref name="rawText"/> is <see langword="Nothing"/>.
        ''' </exception>
        Friend Shared Function NormalizeText(rawText As String) As String
            If rawText Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(rawText))
            End If

            ' WebClient decodes the (UTF-8) responses using the system ANSI code page; this reverts it.
            Dim text As String = Encoding.UTF8.GetString(Encoding.Default.GetBytes(rawText.Trim()))
            text = HttpUtility.HtmlDecode(text)
            text = Crawler.TitleCaseCulture.TextInfo.ToTitleCase(text)

            Return text
        End Function

        ''' <summary>
        ''' Parses the download links of an album from the XML document returned by the FHM website.
        ''' </summary>
        ''' <param name="xml">
        ''' The XML document (root element <c>&lt;rev&gt;</c>, links in <c>&lt;msg&gt;</c>, <c>&lt;msg2&gt;</c> ... <c>&lt;msg13&gt;</c>).
        ''' </param>
        ''' <returns>
        ''' The download links, in document order, without empty values.
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when <paramref name="xml"/> is <see langword="Nothing"/>.
        ''' </exception>
        '''
        ''' <exception cref="XmlException">
        ''' Thrown when <paramref name="xml"/> is not a well-formed XML document.
        ''' </exception>
        '''
        ''' <exception cref="FormatException">
        ''' Thrown when the root element of the XML document is not <c>&lt;rev&gt;</c>.
        ''' </exception>
        Friend Shared Function ParseDownloadUrls(xml As String) As String()
            If xml Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(xml))
            End If

            Dim xDoc As XDocument = XDocument.Parse(xml)
            Dim root As XElement = xDoc.Root
            If Not root.Name.LocalName.Equals("rev", StringComparison.Ordinal) Then
                Throw New FormatException($"Unexpected download links document: the root element is '{root.Name.LocalName}' instead of 'rev'.")
            End If

            Dim downloadUrls As New List(Of String)
            For Each elementName As String In Crawler.DownloadUrlElementNames
                ' Not every album fills all the download link slots.
                Dim element As XElement = root.Element(elementName)
                If element Is Nothing Then
                    Continue For
                End If

                Dim value As String = element.Value.Trim()
                If value.Length <> 0 Then
                    downloadUrls.Add(value)
                End If
            Next elementName

            Return downloadUrls.ToArray()
        End Function

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Downloads the content of the specified <see cref="Uri"/> as a string.
        ''' </summary>
        ''' <param name="address">
        ''' The address to download.
        ''' </param>
        ''' <returns>
        ''' The downloaded content.
        ''' </returns>
        ''' <exception cref="InvalidOperationException">
        ''' Thrown when the download delegate returns a null task or a null string.
        ''' </exception>
        Private Async Function DownloadAsync(address As Uri) As Task(Of String)
            Dim downloadTask As Task(Of String) = Me.downloadStringAsync.Invoke(address)
            If downloadTask Is Nothing Then
                Throw New InvalidOperationException($"The download delegate returned a null task for '{address}'.")
            End If

            Dim content As String = Await downloadTask.ConfigureAwait(False)
            If content Is Nothing Then
                Throw New InvalidOperationException($"The download of '{address}' returned no content.")
            End If

            Return content
        End Function

        ''' <summary>
        ''' Parses the album listings of a search results page and downloads the download links of every album.
        ''' <para></para>
        ''' Albums whose download links cannot be retrieved or parsed are skipped.
        ''' </summary>
        ''' <param name="htmlSourceCode">
        ''' The HTML source code of the search results page.
        ''' </param>
        ''' <returns>
        ''' The albums of the page.
        ''' </returns>
        Private Async Function FetchPageAlbumsAsync(htmlSourceCode As String) As Task(Of List(Of AlbumInfo))
            Dim listings As List(Of AlbumListing) = Crawler.ParseAlbumListings(htmlSourceCode)
            Dim albums As New List(Of AlbumInfo)(listings.Count)

            For Each listing As AlbumListing In listings
                Dim downloadUrls As String()
                Try
                    Dim uriDownloadUrls As New Uri(Crawler.BuildDownloadUrlsQuery(listing.Sobi2Id))
                    Dim xml As String = Await Me.DownloadAsync(uriDownloadUrls).ConfigureAwait(False)
                    downloadUrls = Crawler.ParseDownloadUrls(xml)

                Catch ex As WebException
                    Continue For

                Catch ex As XmlException
                    Continue For

                Catch ex As FormatException
                    Continue For

                End Try

                albums.Add(New AlbumInfo(listing.Artist, listing.Title, downloadUrls))
            Next listing

            Return albums
        End Function

        ''' <summary>
        ''' Invokes the specified action on the <see cref="SynchronizationContext"/> captured when the
        ''' current fetch operation started, or directly if there was no context (or it is the current one).
        ''' </summary>
        ''' <param name="action">
        ''' The action to invoke.
        ''' </param>
        Private Sub InvokeOnEventContext(action As Action)
            Dim context As SynchronizationContext = Me.eventContext
            If context Is Nothing OrElse context Is SynchronizationContext.Current Then
                action.Invoke()
            Else
                context.Send(Sub(state As Object) action.Invoke(), Nothing)
            End If
        End Sub

#End Region

    End Class

End Namespace

#End Region
