#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Specialized
Imports System.Globalization
Imports System.Web

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports FHM

#End Region

#Region " CrawlerQueryTests "

Namespace FHMTests

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Tests the addresses built by the <see cref="Crawler"/> (search pages, download links and pagination).
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestClass>
    Public NotInheritable Class CrawlerQueryTests

#Region " BuildSearchQuery "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The search query targets the FHM search page.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub BuildSearchQuery_TargetsTheSearchPage()
            Dim query As String = Crawler.BuildSearchQuery(New SearchFilter(), pageIndex:=0)

            StringAssert.StartsWith(query, "http://freehardmusic.com/index.php?")
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The search query contains all the parameters expected by the website, in the original order.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub BuildSearchQuery_ContainsExpectedParametersInOrder()
            Dim query As NameValueCollection = CrawlerQueryTests.ParseQuery(Crawler.BuildSearchQuery(New SearchFilter(), pageIndex:=0))

            Dim expectedKeys As String() = {
                "field_band", "field_country", "field_genre", "field_year", "field_format",
                "option", "search", "searchphrase", "sobi2Search", "sobi2Task",
                "SobiCatSelected_0", "sobiCid", "reset", "Itemid", "SobiSearchPage"
            }
            CollectionAssert.AreEqual(expectedKeys, query.AllKeys)

            Assert.AreEqual("0", query("field_format"))
            Assert.AreEqual("com_sobi2", query("option"))
            Assert.AreEqual("Search", query("search"))
            Assert.AreEqual("exact", query("searchphrase"))
            Assert.AreEqual("", query("sobi2Search"))
            Assert.AreEqual("axSearch", query("sobi2Task"))
            Assert.AreEqual("0", query("SobiCatSelected_0"))
            Assert.AreEqual("0", query("sobiCid"))
            Assert.AreEqual("2", query("reset"))
            Assert.AreEqual("13", query("Itemid"))
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The search query carries the values of the search filter.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub BuildSearchQuery_CarriesSearchFilterValues()
            Dim filter As New SearchFilter(genre:="Heavy metal", country:="Spain", year:="1986")
            filter.Artist = "Barón Rojo"

            Dim query As NameValueCollection = CrawlerQueryTests.ParseQuery(Crawler.BuildSearchQuery(filter, pageIndex:=0))

            Assert.AreEqual("Barón Rojo", query("field_band"))
            Assert.AreEqual("Spain", query("field_country"))
            Assert.AreEqual("Heavy metal", query("field_genre"))
            Assert.AreEqual("1986", query("field_year"))
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Reserved URL characters in the filter values are encoded, so they cannot break the query string.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub BuildSearchQuery_EncodesReservedCharacters()
            Dim filter As New SearchFilter("Guns & Roses?=#")

            Dim rawQuery As String = Crawler.BuildSearchQuery(filter, pageIndex:=0)
            Dim query As NameValueCollection = CrawlerQueryTests.ParseQuery(rawQuery)

            Assert.AreEqual("Guns & Roses?=#", query("field_band"))
            Assert.AreEqual(15, query.Count)
            StringAssert.Contains(rawQuery, "field_band=Guns+%26+Roses%3f%3d%23")
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The search query carries the requested (zero-based) page index.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        <DataRow(0)>
        <DataRow(1)>
        <DataRow(57)>
        Public Sub BuildSearchQuery_CarriesPageIndex(pageIndex As Integer)
            Dim query As NameValueCollection = CrawlerQueryTests.ParseQuery(Crawler.BuildSearchQuery(New SearchFilter(), pageIndex))

            Assert.AreEqual(pageIndex.ToString(CultureInfo.InvariantCulture), query("SobiSearchPage"))
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A null search filter is rejected.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub BuildSearchQuery_NullFilter_Throws()
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub() Crawler.BuildSearchQuery(Nothing, pageIndex:=0))
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A negative page index is rejected.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub BuildSearchQuery_NegativePageIndex_Throws()
            Assert.ThrowsExactly(Of ArgumentOutOfRangeException)(Sub() Crawler.BuildSearchQuery(New SearchFilter(), pageIndex:=-1))
        End Sub

#End Region

#Region " BuildDownloadUrlsQuery "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The download links query targets the FHM XML endpoint with the expected parameters.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub BuildDownloadUrlsQuery_TargetsTheXmlEndpointWithExpectedParameters()
            Dim rawQuery As String = Crawler.BuildDownloadUrlsQuery("4321")
            Dim query As NameValueCollection = CrawlerQueryTests.ParseQuery(rawQuery)

            StringAssert.StartsWith(rawQuery, "http://freehardmusic.com/index2.php?")
            CollectionAssert.AreEqual({"sobiid", "sobi2Task", "no_html", "option", "rvote"}, query.AllKeys)
            Assert.AreEqual("4321", query("sobiid"))
            Assert.AreEqual("addSRev", query("sobi2Task"))
            Assert.AreEqual("1", query("no_html"))
            Assert.AreEqual("com_sobi2", query("option"))
            Assert.AreEqual("1", query("rvote"))
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' An empty or white-space album identifier is rejected.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        <DataRow("")>
        <DataRow("   ")>
        Public Sub BuildDownloadUrlsQuery_BlankId_Throws(sobi2Id As String)
            Assert.ThrowsExactly(Of ArgumentException)(Sub() Crawler.BuildDownloadUrlsQuery(sobi2Id))
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A null album identifier is rejected.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub BuildDownloadUrlsQuery_NullId_Throws()
            Assert.ThrowsExactly(Of ArgumentException)(Sub() Crawler.BuildDownloadUrlsQuery(Nothing))
        End Sub

#End Region

#Region " GetPageCount "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The page count is the album count divided by the page size, rounded up.
        ''' <para></para>
        ''' Regression: exact multiples of the page size used to request an extra (empty) page, which crashed the crawler.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        <DataRow(0, 0)>
        <DataRow(1, 1)>
        <DataRow(9, 1)>
        <DataRow(10, 1)>
        <DataRow(11, 2)>
        <DataRow(20, 2)>
        <DataRow(21, 3)>
        <DataRow(1000, 100)>
        <DataRow(Integer.MaxValue, 214748365)>
        Public Sub GetPageCount_RoundsUpToWholePages(albumCount As Integer, expectedPageCount As Integer)
            Assert.AreEqual(expectedPageCount, Crawler.GetPageCount(albumCount))
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A negative album count is rejected.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub GetPageCount_NegativeCount_Throws()
            Assert.ThrowsExactly(Of ArgumentOutOfRangeException)(Sub() Crawler.GetPageCount(-1))
        End Sub

#End Region

#Region " Private Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Parses the query string of an absolute address.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="address">
        ''' The absolute address.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The parsed (decoded) query parameters.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Private Shared Function ParseQuery(address As String) As NameValueCollection
            Return HttpUtility.ParseQueryString(New Uri(address).Query)
        End Function

#End Region

    End Class

End Namespace

#End Region
