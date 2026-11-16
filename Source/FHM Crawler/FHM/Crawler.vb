' ***********************************************************************
' Author   : ElektroStudios
' Modified : 15-November-2026
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Usage Example "

' Imports System.Net
' Imports System.Text
' Imports System.Threading
'
' Imports FHM
'
' Public Module Module1
'
'     ' The FHM website requires a logged-in session:
'     ' the container must hold the session cookies of a logged-in FHM user.
'     Private ReadOnly SessionCookies As New CookieContainer()
'
'     Private WithEvents FHMCrawler As New Crawler(SessionCookies)
'     Private ReadOnly mre As New ManualResetEvent(initialState:=False)
'
'     Public Sub Main()
'         FHMCrawler.SearchQuery.Artist = "Linkin Park"
'
'         Console.WriteLine($"Search Params: {FHMCrawler.SearchQuery}")
'         Console.WriteLine()
'         Console.WriteLine($"Absolute Uri: {FHMCrawler.SearchQuery.Uri.AbsoluteUri}")
'         Console.WriteLine()
'         Console.WriteLine("Retrieving Album count...")
'         Dim albumCount As Integer = FHMCrawler.GetAlbumCount()
'         Console.WriteLine($"Album Count: {albumCount}")
'         Console.WriteLine()
'         Console.WriteLine("Begin crawling, please wait...")
'         Fetch()
'         mre.WaitOne()
'         FHMCrawler.Dispose()
'         Console.WriteLine("Done!. Press any key to exit...")
'         Console.ReadKey()
'     End Sub
'
'     Public Async Sub Fetch()
'         Dim success As Boolean = Await FHMCrawler.FetchAlbumsAsync()
'         mre.Set()
'     End Sub
'
'     <DebuggerStepperBoundary>
'     Private Sub FHMCrawler_PageCrawlBegin(sender As Object, e As PageCrawlBeginEventArgs) Handles FHMCrawler.PageCrawlBegin
'         Console.WriteLine($"[+] Begin crawling page with index: {e.SearchPage}")
'         Console.WriteLine()
'     End Sub
'
'     <DebuggerStepperBoundary>
'     Private Sub FHMCrawler_PageCrawlEnd(sender As Object, e As PageCrawlEndEventArgs) Handles FHMCrawler.PageCrawlEnd
'         For Each albumInfo As AlbumInfo In e.Albums
'             Dim sb As New StringBuilder()
'             sb.AppendLine($"Artist Name.....: {albumInfo.Artist}")
'             sb.AppendLine($"Album Title.....: {albumInfo.Title}")
'             sb.AppendLine($"Album Id........: {albumInfo.Id}")
'             sb.AppendLine($"Album Url.......: {albumInfo.Uri.AbsoluteUri}")
'             sb.AppendLine($"Download Link(s): {{ {String.Join(", ", albumInfo.DownloadLinks)} }}")
'             Console.WriteLine(sb.ToString())
'         Next albumInfo
'         Console.WriteLine($"[+] End crawling page with index: {e.SearchPage}")
'         Console.WriteLine()
'     End Sub
'
' End Module

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
    ''' A crawler that searchs and collect albums (its download links) from the http://freehardmusic.com/ website.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed NuGet packages:
    ''' <para></para>
    ''' <see href="https://www.nuget.org/packages/HtmlAgilityPack">HtmlAgilityPack</see>
    ''' </remarks>
    '''
    ''' <example> This is a code example.
    ''' <code language="VB.NET">
    ''' Imports System.Net
    ''' Imports System.Text
    ''' Imports System.Threading
    '''
    ''' Imports FHM
    '''
    ''' Public Module Module1
    '''
    '''     ' The FHM website requires a logged-in session:
    '''     ' the container must hold the session cookies of a logged-in FHM user.
    '''     Private ReadOnly SessionCookies As New CookieContainer()
    '''
    '''     Private WithEvents FHMCrawler As New Crawler(SessionCookies)
    '''     Private ReadOnly mre As New ManualResetEvent(initialState:=False)
    '''
    '''     Public Sub Main()
    '''         FHMCrawler.SearchQuery.Artist = "Linkin Park"
    '''
    '''         Console.WriteLine($"Search Params: {FHMCrawler.SearchQuery}")
    '''         Console.WriteLine()
    '''         Console.WriteLine($"Absolute Uri: {FHMCrawler.SearchQuery.Uri.AbsoluteUri}")
    '''         Console.WriteLine()
    '''         Console.WriteLine("Retrieving Album count...")
    '''         Dim albumCount As Integer = FHMCrawler.GetAlbumCount()
    '''         Console.WriteLine($"Album Count: {albumCount}")
    '''         Console.WriteLine()
    '''         Console.WriteLine("Begin crawling, please wait...")
    '''         Fetch()
    '''         mre.WaitOne()
    '''         FHMCrawler.Dispose()
    '''         Console.WriteLine("Done!. Press any key to exit...")
    '''         Console.ReadKey()
    '''     End Sub
    '''
    '''     Public Async Sub Fetch()
    '''         Dim success As Boolean = Await FHMCrawler.FetchAlbumsAsync()
    '''         mre.Set()
    '''     End Sub
    '''
    '''     Private Sub FHMCrawler_PageCrawlBegin(sender As Object, e As PageCrawlBeginEventArgs) Handles FHMCrawler.PageCrawlBegin
    '''         Console.WriteLine($"[+] Begin crawling page with index: {e.SearchPage}")
    '''         Console.WriteLine()
    '''     End Sub
    '''
    '''     Private Sub FHMCrawler_PageCrawlEnd(sender As Object, e As PageCrawlEndEventArgs) Handles FHMCrawler.PageCrawlEnd
    '''         For Each albumInfo As AlbumInfo In e.Albums
    '''             Dim sb As New StringBuilder()
    '''             sb.AppendLine($"Artist Name.....: {albumInfo.Artist}")
    '''             sb.AppendLine($"Album Title.....: {albumInfo.Title}")
    '''             sb.AppendLine($"Album Id........: {albumInfo.Id}")
    '''             sb.AppendLine($"Album Url.......: {albumInfo.Uri.AbsoluteUri}")
    '''             sb.AppendLine($"Download Link(s): {{ {String.Join(", ", albumInfo.DownloadLinks)} }}")
    '''             Console.WriteLine(sb.ToString())
    '''         Next albumInfo
    '''         Console.WriteLine($"[+] End crawling page with index: {e.SearchPage}")
    '''         Console.WriteLine()
    '''     End Sub
    '''
    ''' End Module
    ''' </code>
    ''' </example>
    Public Class Crawler : Implements IDisposable

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
        ''' The <see cref="Uri"/> that points to "http://freehardmusic.com/".
        ''' </summary>
        Friend Shared ReadOnly UriBase As New Uri("http://freehardmusic.com/")

        ''' <summary>
        ''' The <see cref="Uri"/> that points to "http://freehardmusic.com/index.php" (the search page).
        ''' </summary>
        Friend Shared ReadOnly UriIndex As New Uri(Crawler.UriBase, "/index.php")

        ''' <summary>
        ''' The <see cref="Uri"/> that points to "http://freehardmusic.com/index2.php"
        ''' (the page that returns the download links of an album, as XML).
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
        ''' The <see cref="CancellationTokenSource"/> instance that signals to <see cref="Crawler.cancelToken"/>.
        ''' </summary>
        Private cancelTokenSrc As CancellationTokenSource

        ''' <summary>
        ''' The <see cref="CancellationToken"/> instance that cancels a pending fetch operation
        ''' started by a call of <see cref="Crawler.FetchAlbumsAsync()"/>.
        ''' </summary>
        Private cancelToken As CancellationToken

        ''' <summary>
        ''' Flag (<c>0</c> or <c>1</c>) that determines whether this <see cref="Crawler"/> is busy in a pending fetch operation.
        ''' <para></para>
        ''' It is an <see cref="Integer"/> to allow atomic updates through <see cref="Interlocked"/>.
        ''' </summary>
        Private fetchingFlag As Integer

        ''' <summary>
        ''' The <see cref="SynchronizationContext"/> captured when the current fetch operation started.
        ''' <para></para>
        ''' The <see cref="Crawler.PageCrawlBegin"/> and <see cref="Crawler.PageCrawlEnd"/> events are raised on it,
        ''' so UI event handlers keep running on the UI thread even though the crawler uses <c>ConfigureAwait(False)</c>.
        ''' </summary>
        Private eventContext As SynchronizationContext

#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets the search query.
        ''' </summary>
        '''
        ''' <value>
        ''' The search query.
        ''' </value>
        Public ReadOnly Property SearchQuery As SearchQuery

        ''' <summary>
        ''' Gets or sets the session cookies sent to the FHM website.
        ''' </summary>
        '''
        ''' <value>
        ''' The session cookies sent to the FHM website.
        ''' </value>
        '''
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
        ''' Gets a value that determines whether this <see cref="Crawler"/> is busy in a pending fetch operation.
        ''' </summary>
        '''
        ''' <value>
        ''' <see langword="True"/> if a fetch operation is running; otherwise, <see langword="False"/>.
        ''' </value>
        Public ReadOnly Property IsFetching As Boolean
            Get
                Return Volatile.Read(Me.fetchingFlag) = 1
            End Get
        End Property

#End Region

#Region " Events "

        ''' <summary>
        ''' Occurs when a page is about to be crawled.
        ''' </summary>
        Public Event PageCrawlBegin As EventHandler(Of PageCrawlBeginEventArgs)

        ''' <summary>
        ''' Occurs when a page is crawled.
        ''' </summary>
        Public Event PageCrawlEnd As EventHandler(Of PageCrawlEndEventArgs)

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="Crawler"/> class.
        ''' </summary>
        '''
        ''' <param name="cookies">
        ''' The session cookies sent to the FHM website (the FHM website requires a logged-in session).
        ''' </param>
        '''
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

            Me.SearchQuery = New SearchQuery()
            Me.cancelTokenSrc = New CancellationTokenSource()
            Me.cancelToken = Me.cancelTokenSrc.Token
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="Crawler"/> class
        ''' that downloads the web content through the specified delegate.
        ''' <para></para>
        ''' This constructor exists for unit testing purposes.
        ''' </summary>
        '''
        ''' <param name="cookies">
        ''' The session cookies sent to the FHM website.
        ''' </param>
        '''
        ''' <param name="downloadStringAsync">
        ''' The delegate that downloads the content of an <see cref="Uri"/> as a string.
        ''' </param>
        '''
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
        ''' Gets the count of the albums found using the current search query.
        ''' </summary>
        '''
        ''' <returns>
        ''' The count of the albums found using the current search query.
        ''' </returns>
        '''
        ''' <exception cref="ObjectDisposedException">
        ''' Thrown when this instance has been disposed.
        ''' </exception>
        Public Overridable Function GetAlbumCount() As Integer
            Me.ThrowIfDisposed()

            ' Runs on a thread-pool thread to avoid deadlocks when called from a UI thread;
            ' GetResult() throws the original exception instead of an AggregateException.
            Return Task.Run(Of Integer)(AddressOf Me.GetAlbumCountAsync).GetAwaiter().GetResult()
        End Function

        ''' <summary>
        ''' Asynchronously gets the count of the albums found using the current search query.
        ''' </summary>
        '''
        ''' <returns>
        ''' The count of the albums found using the current search query.
        ''' </returns>
        '''
        ''' <exception cref="ObjectDisposedException">
        ''' Thrown when this instance has been disposed.
        ''' </exception>
        Public Overridable Async Function GetAlbumCountAsync() As Task(Of Integer)
            Me.ThrowIfDisposed()

            Dim htmlSourceCode As String = Await Me.DownloadAsync(Me.SearchQuery.Uri(searchPage:=0)).ConfigureAwait(False)
            Return Crawler.ParseAlbumCount(htmlSourceCode)
        End Function

        ''' <summary>
        ''' Fetch any album found using the current search query.
        ''' </summary>
        '''
        ''' <exception cref="ObjectDisposedException">
        ''' Thrown when this instance has been disposed.
        ''' </exception>
        '''
        ''' <exception cref="InvalidOperationException">
        ''' Thrown when another fetch operation is already running.
        ''' </exception>
        Public Overridable Sub FetchAlbums()
            Me.ThrowIfDisposed()

            ' Runs on a thread-pool thread to avoid deadlocks when called from a UI thread;
            ' GetResult() throws the original exception instead of an AggregateException.
            Task.Run(Of Boolean)(AddressOf Me.FetchAlbumsAsync).GetAwaiter().GetResult()
        End Sub

        ''' <summary>
        ''' Asynchronously fetch any album found using the current search query.
        ''' <para></para>
        ''' The albums of every search page are delivered through the <see cref="Crawler.PageCrawlEnd"/> event.
        ''' </summary>
        '''
        ''' <returns>
        ''' Returns <see langword="False"/> if the fetch operation was canceled by a call to
        ''' <see cref="Crawler.CancelFetchAlbumsAsync()"/> method; otherwise, <see langword="True"/>.
        ''' </returns>
        '''
        ''' <exception cref="ObjectDisposedException">
        ''' Thrown when this instance has been disposed.
        ''' </exception>
        '''
        ''' <exception cref="InvalidOperationException">
        ''' Thrown when another fetch operation is already running in background.
        ''' </exception>
        Public Overridable Async Function FetchAlbumsAsync() As Task(Of Boolean)
            Me.ThrowIfDisposed()

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

                For searchPage As Integer = 0 To (pageCount - 1)
                    Dim currentSearchPage As Integer = searchPage
                    Dim htmlSourceCode As String = Await Me.DownloadAsync(Me.SearchQuery.Uri(currentSearchPage)).ConfigureAwait(False)

                    If Me.cancelToken.IsCancellationRequested Then
                        Return False
                    End If

                    Me.InvokeOnEventContext(Sub() Me.OnPageCrawlBegin(New PageCrawlBeginEventArgs(Me.SearchQuery, currentSearchPage)))
                    Dim albums As List(Of AlbumInfo) = Await Me.FetchPageAlbumsAsync(htmlSourceCode).ConfigureAwait(False)
                    Me.InvokeOnEventContext(Sub() Me.OnPageCrawlEnd(New PageCrawlEndEventArgs(Me.SearchQuery, currentSearchPage, albums)))
                Next searchPage

                Return True

            Finally
                Me.eventContext = Nothing
                Interlocked.Exchange(Me.fetchingFlag, 0)

            End Try
        End Function

        ''' <summary>
        ''' Aborts a pending fetch operation started by a call to <see cref="Crawler.FetchAlbumsAsync()"/> function.
        ''' </summary>
        '''
        ''' <exception cref="ObjectDisposedException">
        ''' Thrown when this instance has been disposed.
        ''' </exception>
        '''
        ''' <exception cref="InvalidOperationException">
        ''' No fetch operation is running.
        ''' </exception>
        '''
        ''' <exception cref="OperationCanceledException">
        ''' The cancellation of the running fetch operation was already requested.
        ''' </exception>
        Public Sub CancelFetchAlbumsAsync()
            Me.ThrowIfDisposed()

            If Not Me.IsFetching Then
                Throw New InvalidOperationException("No fetch operation is running.")
            End If

            ' Handle redundant cancellation calls to CancelFetchAlbumsAsync()...
            Me.cancelToken.ThrowIfCancellationRequested()

            Me.cancelTokenSrc.Cancel()
        End Sub

        ''' <summary>
        ''' Resets the current search query (<see cref="Crawler.SearchQuery"/>) to its default values.
        ''' </summary>
        Public Sub ResetSearchQuery()
            Me.SearchQuery.Reset()
        End Sub

#End Region

#Region " Event-Invocators "

        ''' <summary>
        ''' Raises the <see cref="Crawler.PageCrawlBegin"/> event.
        ''' </summary>
        '''
        ''' <param name="e">
        ''' The <see cref="PageCrawlBeginEventArgs"/> instance containing the event data.
        ''' </param>
        Protected Overridable Sub OnPageCrawlBegin(e As PageCrawlBeginEventArgs)
            If Me.PageCrawlBeginEvent IsNot Nothing Then
                RaiseEvent PageCrawlBegin(Me, e)
            End If
        End Sub

        ''' <summary>
        ''' Raises the <see cref="Crawler.PageCrawlEnd"/> event.
        ''' </summary>
        '''
        ''' <param name="e">
        ''' The <see cref="PageCrawlEndEventArgs"/> instance containing the event data.
        ''' </param>
        Protected Overridable Sub OnPageCrawlEnd(e As PageCrawlEndEventArgs)
            If Me.PageCrawlEndEvent IsNot Nothing Then
                RaiseEvent PageCrawlEnd(Me, e)
            End If
        End Sub

#End Region

#Region " Friend Methods (pure functions, unit tested) "

        ''' <summary>
        ''' Builds the address of the XML document that contains the download links of an album.
        ''' </summary>
        '''
        ''' <param name="albumId">
        ''' The album identifier (that is used in the 'sobiid' and 'sobi2id' parameters).
        ''' </param>
        '''
        ''' <returns>
        ''' The absolute address of the XML document.
        ''' </returns>
        '''
        ''' <exception cref="ArgumentException">
        ''' Thrown when <paramref name="albumId"/> is <see langword="Nothing"/>, empty or white-space.
        ''' </exception>
        Friend Shared Function BuildDownloadUrlsQuery(albumId As String) As String
            If String.IsNullOrWhiteSpace(albumId) Then
                Throw New ArgumentException("The album identifier cannot be null, empty or white-space.", paramName:=NameOf(albumId))
            End If

            Dim downloadUrlParams As New NameValueCollection From {
                {"sobiid", albumId},
                {"sobi2Task", "addSRev"},
                {"no_html", "1"},
                {"option", "com_sobi2"},
                {"rvote", "1"}
            }
            Return downloadUrlParams.ToQueryString(Crawler.UriIndex2)
        End Function

        ''' <summary>
        ''' Gets the amount of search pages needed to list the specified amount of albums.
        ''' </summary>
        '''
        ''' <param name="albumCount">
        ''' The amount of albums.
        ''' </param>
        '''
        ''' <returns>
        ''' The amount of search pages.
        ''' </returns>
        '''
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
        ''' Parses the album count from the HTML source code of a search page.
        ''' </summary>
        '''
        ''' <param name="htmlSourceCode">
        ''' The HTML source code of the search page.
        ''' </param>
        '''
        ''' <returns>
        ''' The album count, or <c>0</c> if the page does not contain the search result summary.
        ''' </returns>
        '''
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

            ' Remove new lines, and replace tabs and duplicated white-spaces for a single white-space.
            Dim text As String = Regex.Replace(node.InnerText, "\s+", " ", RegexOptions.None, TimeSpan.FromSeconds(30)).Trim()
            Dim match As Match = Regex.Match(text, "\d+", RegexOptions.None, TimeSpan.FromSeconds(30))
            If Not match.Success Then
                Throw New FormatException($"The album count could not be found in the search result summary: '{text}'.")
            End If

            Return Integer.Parse(match.Value, NumberStyles.None, CultureInfo.InvariantCulture)
        End Function

        ''' <summary>
        ''' Parses the album listings from the HTML source code of a search page.
        ''' <para></para>
        ''' Listings without a valid album link are skipped.
        ''' </summary>
        '''
        ''' <param name="htmlSourceCode">
        ''' The HTML source code of the search page.
        ''' </param>
        '''
        ''' <returns>
        ''' The album listings found in the page (an empty list if the page does not list any album).
        ''' </returns>
        '''
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

                Dim albumId As String = Nothing
                Dim albumUri As Uri = Nothing
                If Not Crawler.TryParseAlbumLink(urlNode.GetAttributeValue("href", ""), albumId, albumUri) Then
                    Continue For
                End If

                Dim artistNode As HtmlNode = node.SelectSingleNode(Crawler.XPathAlbumArtist)
                Dim rawArtist As String = If(artistNode Is Nothing, Crawler.UnknownFieldValue, artistNode.InnerText)

                Dim titleNode As HtmlNode = node.SelectSingleNode(Crawler.XPathAlbumTitle)
                Dim rawTitle As String = If(titleNode Is Nothing, Crawler.UnknownFieldValue, titleNode.GetAttributeValue("title", ""))

                listings.Add(New AlbumListing(Crawler.NormalizeText(rawArtist), Crawler.NormalizeText(rawTitle), albumId, albumUri))
            Next node

            Return listings
        End Function

        ''' <summary>
        ''' Tries to parse an album link: its absolute <see cref="Uri"/>
        ''' and the album identifier (the <c>sobi2Id</c> query parameter).
        ''' </summary>
        '''
        ''' <param name="href">
        ''' The <c>href</c> attribute value of the album link.
        ''' It can be absolute or relative, with or without HTML-encoded ampersands (<c>&amp;amp;</c>).
        ''' </param>
        '''
        ''' <param name="refAlbumId">
        ''' When this method returns <see langword="True"/>, contains the album identifier.
        ''' </param>
        '''
        ''' <param name="refAlbumUri">
        ''' When this method returns <see langword="True"/>, contains the absolute album <see cref="Uri"/>.
        ''' </param>
        '''
        ''' <returns>
        ''' <see langword="True"/> if the album link is valid and contains the album identifier;
        ''' otherwise, <see langword="False"/>.
        ''' </returns>
        Friend Shared Function TryParseAlbumLink(href As String, ByRef refAlbumId As String, ByRef refAlbumUri As Uri) As Boolean
            refAlbumId = Nothing
            refAlbumUri = Nothing

            If String.IsNullOrWhiteSpace(href) Then
                Return False
            End If

            Dim decodedHref As String = HttpUtility.HtmlDecode(href.Trim())
            Dim albumUri As Uri = Nothing
            If Not Uri.TryCreate(Crawler.UriBase, decodedHref, albumUri) Then
                Return False
            End If

            Dim albumId As String = HttpUtility.ParseQueryString(albumUri.Query)("sobi2Id")
            If String.IsNullOrWhiteSpace(albumId) Then
                Return False
            End If

            refAlbumId = albumId.Trim()
            refAlbumUri = albumUri
            Return True
        End Function

        ''' <summary>
        ''' Normalizes an artist name or an album title scraped from the FHM website:
        ''' trims it, fixes the UTF-8 text that was decoded with the system ANSI code page,
        ''' decodes HTML entities and converts it to title case.
        ''' </summary>
        '''
        ''' <param name="rawText">
        ''' The raw text.
        ''' </param>
        '''
        ''' <returns>
        ''' The normalized text.
        ''' </returns>
        '''
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
        '''
        ''' <param name="xml">
        ''' The XML document (root element <c>&lt;rev&gt;</c>, links in <c>&lt;msg&gt;</c>, <c>&lt;msg2&gt;</c> ... <c>&lt;msg13&gt;</c>).
        ''' </param>
        '''
        ''' <returns>
        ''' The download links, in document order, without empty values.
        ''' </returns>
        '''
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
        '''
        ''' <param name="address">
        ''' The address to download.
        ''' </param>
        '''
        ''' <returns>
        ''' The downloaded content.
        ''' </returns>
        '''
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
        ''' Parses the albums listed in the HTML source code of a search page,
        ''' and retrieves the download links of every album.
        ''' <para></para>
        ''' Albums whose download links cannot be retrieved (network error or malformed response) are skipped.
        ''' </summary>
        '''
        ''' <param name="htmlSourceCode">
        ''' The HTML source code of the search page.
        ''' </param>
        '''
        ''' <returns>
        ''' The crawled albums.
        ''' </returns>
        Private Async Function FetchPageAlbumsAsync(htmlSourceCode As String) As Task(Of List(Of AlbumInfo))
            Dim listings As List(Of AlbumListing) = Crawler.ParseAlbumListings(htmlSourceCode)
            Dim albums As New List(Of AlbumInfo)(listings.Count)

            For Each listing As AlbumListing In listings
                Dim downloadLinks As String()
                Try
                    Dim uriDownloadLinks As New Uri(Crawler.BuildDownloadUrlsQuery(listing.Sobi2Id))
                    Dim xml As String = Await Me.DownloadAsync(uriDownloadLinks).ConfigureAwait(False)
                    downloadLinks = Crawler.ParseDownloadUrls(xml)

                Catch ex As WebException
                    Continue For

                Catch ex As XmlException
                    Continue For

                Catch ex As FormatException
                    Continue For

                End Try

                albums.Add(New AlbumInfo(listing.Sobi2Id, listing.AlbumUri, listing.Artist, listing.Title, downloadLinks))
            Next listing

            Return albums
        End Function

        ''' <summary>
        ''' Runs the specified action on the <see cref="SynchronizationContext"/> captured
        ''' when the current fetch operation started (or directly, when there is none).
        ''' </summary>
        '''
        ''' <param name="action">
        ''' The action to run.
        ''' </param>
        Private Sub InvokeOnEventContext(action As Action)
            Dim context As SynchronizationContext = Me.eventContext
            If context Is Nothing OrElse context Is SynchronizationContext.Current Then
                action.Invoke()
            Else
                context.Send(Sub(state As Object) action.Invoke(), Nothing)
            End If
        End Sub

        ''' <summary>
        ''' Throws an <see cref="ObjectDisposedException"/> if this instance has been disposed.
        ''' </summary>
        '''
        ''' <exception cref="ObjectDisposedException">
        ''' Thrown when this instance has been disposed.
        ''' </exception>
        Private Sub ThrowIfDisposed()
            If Me.isDisposed Then
                Throw New ObjectDisposedException(Me.GetType().FullName)
            End If
        End Sub

#End Region

#Region " IDisposable Implementation "

        ''' <summary>
        ''' Flag to detect redundant calls when disposing.
        ''' </summary>
        Private isDisposed As Boolean = False

        ''' <summary>
        ''' Releases all the resources used by this instance.
        ''' <para></para>
        ''' A running fetch operation is cancelled.
        ''' </summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            Me.Dispose(isDisposing:=True)
            GC.SuppressFinalize(Me)
        End Sub

        ''' <summary>
        ''' Releases unmanaged and, optionally, managed resources.
        ''' </summary>
        '''
        ''' <param name="isDisposing">
        ''' <see langword="True"/> to release both managed and unmanaged resources;
        ''' <see langword="False"/> to release only unmanaged resources.
        ''' </param>
        Protected Overridable Sub Dispose(isDisposing As Boolean)
            If (Not Me.isDisposed) AndAlso isDisposing Then
                If Me.IsFetching AndAlso Not Me.cancelToken.IsCancellationRequested Then
                    Me.cancelTokenSrc.Cancel()
                End If
                Me.cancelTokenSrc.Dispose()
                Me.client.Dispose()
            End If

            Me.isDisposed = True
        End Sub

#End Region

    End Class

End Namespace

#End Region
