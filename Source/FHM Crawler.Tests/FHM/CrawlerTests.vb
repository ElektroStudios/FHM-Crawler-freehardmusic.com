#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Net
Imports System.Threading.Tasks

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports FHM

#End Region

#Region " CrawlerTests "

Namespace FHMTests

    ''' <summary>
    ''' Tests the construction and the state members of the <see cref="Crawler"/>.
    ''' </summary>
    <TestClass>
    Public NotInheritable Class CrawlerTests

        ''' <summary>
        ''' A new crawler exposes the provided cookies, a default search filter, and is idle.
        ''' </summary>
        <TestMethod>
        Public Sub Constructor_InitializesState()
            Dim cookies As New CookieContainer()

            Dim fhmCrawler As New Crawler(cookies)

            Assert.AreSame(cookies, fhmCrawler.Cookies)
            Assert.IsNotNull(fhmCrawler.SearchQuery)
            Assert.AreEqual("", fhmCrawler.SearchQuery.Artist)
            Assert.AreEqual("all", fhmCrawler.SearchQuery.Country)
            Assert.AreEqual("all", fhmCrawler.SearchQuery.Genre)
            Assert.AreEqual("all", fhmCrawler.SearchQuery.Year)
            Assert.IsFalse(fhmCrawler.IsFetching)
        End Sub

        ''' <summary>
        ''' Null cookies are rejected.
        ''' </summary>
        <TestMethod>
        Public Sub Constructor_NullCookies_Throws()
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub()
                                                 Dim unused As New Crawler(Nothing)
                                             End Sub)
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub()
                                                 Dim unused As New Crawler(Nothing, Function(address As Uri) Task.FromResult(""))
                                             End Sub)
        End Sub

        ''' <summary>
        ''' A null download delegate is rejected.
        ''' </summary>
        <TestMethod>
        Public Sub Constructor_NullDownloadDelegate_Throws()
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub()
                                                 Dim unused As New Crawler(New CookieContainer(), Nothing)
                                             End Sub)
        End Sub

        ''' <summary>
        ''' The cookies can be replaced, but not with a null container.
        ''' </summary>
        <TestMethod>
        Public Sub Cookies_CanBeReplacedButNotNulled()
            Dim fhmCrawler As New Crawler(New CookieContainer())
            Dim newCookies As New CookieContainer()

            fhmCrawler.Cookies = newCookies

            Assert.AreSame(newCookies, fhmCrawler.Cookies)
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub() fhmCrawler.Cookies = Nothing)
            Assert.AreSame(newCookies, fhmCrawler.Cookies)
        End Sub

        ''' <summary>
        ''' ResetSearchQuery restores the default values of an artist-name based search query.
        ''' </summary>
        <TestMethod>
        Public Sub ResetSearchQuery_FromArtistSearch_RestoresDefaults()
            Dim fhmCrawler As New Crawler(New CookieContainer())
            Dim query As SearchQuery = fhmCrawler.SearchQuery
            query.Artist = "Saxon"

            fhmCrawler.ResetSearchQuery()

            Assert.AreSame(query, fhmCrawler.SearchQuery)
            Assert.AreEqual("", query.Artist)
            Assert.AreEqual("all", query.Country)
            Assert.AreEqual("all", query.Genre)
            Assert.AreEqual("all", query.Year)
        End Sub

        ''' <summary>
        ''' ResetSearchQuery restores the default values of a custom search query.
        ''' </summary>
        <TestMethod>
        Public Sub ResetSearchQuery_FromCustomSearch_RestoresDefaults()
            Dim fhmCrawler As New Crawler(New CookieContainer())
            Dim query As SearchQuery = fhmCrawler.SearchQuery
            query.Country = "United Kingdom"
            query.Genre = "NWOBHM"
            query.Year = "1980"

            fhmCrawler.ResetSearchQuery()

            Assert.AreSame(query, fhmCrawler.SearchQuery)
            Assert.AreEqual("", query.Artist)
            Assert.AreEqual("all", query.Country)
            Assert.AreEqual("all", query.Genre)
            Assert.AreEqual("all", query.Year)
        End Sub

        ''' <summary>
        ''' Once disposed, the crawler rejects any operation.
        ''' </summary>
        <TestMethod>
        Public Async Function Dispose_ThenOperations_ThrowObjectDisposedException() As Task
            Dim fhmCrawler As New Crawler(New CookieContainer(), Function(address As Uri) Task.FromResult(""))

            fhmCrawler.Dispose()

            Assert.ThrowsExactly(Of ObjectDisposedException)(Function() fhmCrawler.GetAlbumCount())
            Assert.ThrowsExactly(Of ObjectDisposedException)(Sub() fhmCrawler.FetchAlbums())
            Assert.ThrowsExactly(Of ObjectDisposedException)(Sub() fhmCrawler.CancelFetchAlbumsAsync())
            Await Assert.ThrowsExactlyAsync(Of ObjectDisposedException)(Function() fhmCrawler.GetAlbumCountAsync()).ConfigureAwait(False)
            Await Assert.ThrowsExactlyAsync(Of ObjectDisposedException)(Function() fhmCrawler.FetchAlbumsAsync()).ConfigureAwait(False)
        End Function

        ''' <summary>
        ''' Disposing the crawler more than once is harmless.
        ''' </summary>
        <TestMethod>
        Public Sub Dispose_CalledTwice_IsHarmless()
            Dim fhmCrawler As New Crawler(New CookieContainer())

            fhmCrawler.Dispose()
            fhmCrawler.Dispose()

            Assert.IsFalse(fhmCrawler.IsFetching)
        End Sub

    End Class

End Namespace

#End Region
