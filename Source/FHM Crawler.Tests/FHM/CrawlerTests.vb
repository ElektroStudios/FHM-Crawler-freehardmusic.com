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
            Assert.IsNotNull(fhmCrawler.SearchFilter)
            Assert.AreEqual("", fhmCrawler.SearchFilter.Artist)
            Assert.AreEqual("all", fhmCrawler.SearchFilter.Country)
            Assert.AreEqual("all", fhmCrawler.SearchFilter.Genre)
            Assert.AreEqual("all", fhmCrawler.SearchFilter.Year)
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
        ''' Resetting the search filters restores the default values (the instance is kept).
        ''' </summary>
        <TestMethod>
        Public Sub ResetSearchFilters_RestoresDefaults()
            Dim fhmCrawler As New Crawler(New CookieContainer())
            Dim filter As SearchFilter = fhmCrawler.SearchFilter
            filter.Artist = "Saxon"
            filter.Country = "United Kingdom"
            filter.Genre = "NWOBHM"
            filter.Year = "1980"

            fhmCrawler.ResetSearchFilters()

            Assert.AreSame(filter, fhmCrawler.SearchFilter)
            Assert.AreEqual("", filter.Artist)
            Assert.AreEqual("all", filter.Country)
            Assert.AreEqual("all", filter.Genre)
            Assert.AreEqual("all", filter.Year)
        End Sub

    End Class

End Namespace

#End Region
