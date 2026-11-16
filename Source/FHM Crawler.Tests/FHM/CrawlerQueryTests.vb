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

    ''' <summary>
    ''' Tests the addresses built by the <see cref="Crawler"/> (search pages, download links and pagination).
    ''' </summary>
    <TestClass>
    Public NotInheritable Class CrawlerQueryTests

#Region " SearchQuery.Uri "

        ''' <summary>
        ''' The search query targets the FHM search page.
        ''' </summary>
        <TestMethod>
        Public Sub SearchQueryUri_TargetsTheSearchPage()
            Dim address As Uri = New SearchQuery().Uri(searchPage:=0)

            StringAssert.StartsWith(address.AbsoluteUri, "http://freehardmusic.com/index.php?")
        End Sub

        ''' <summary>
        ''' The search query contains the parameters sent by the FHM search form, in the same order.
        ''' </summary>
        <TestMethod>
        Public Sub SearchQueryUri_ContainsExpectedParametersInOrder()
            Dim query As NameValueCollection = CrawlerQueryTests.ParseQuery(New SearchQuery().Uri(searchPage:=0))

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

        ''' <summary>
        ''' The search query carries the custom search criteria.
        ''' </summary>
        <TestMethod>
        Public Sub SearchQueryUri_CarriesCustomSearchCriteria()
            Dim searchQuery As New SearchQuery(genre:="Heavy metal", country:="Spain", year:="1986")

            Dim query As NameValueCollection = CrawlerQueryTests.ParseQuery(searchQuery.Uri(searchPage:=0))

            Assert.AreEqual("", query("field_band"))
            Assert.AreEqual("Spain", query("field_country"))
            Assert.AreEqual("Heavy metal", query("field_genre"))
            Assert.AreEqual("1986", query("field_year"))
        End Sub

        ''' <summary>
        ''' The search query carries the artist name (non-ASCII characters included).
        ''' </summary>
        <TestMethod>
        Public Sub SearchQueryUri_CarriesArtistName()
            Dim query As NameValueCollection = CrawlerQueryTests.ParseQuery(New SearchQuery("Barón Rojo").Uri(searchPage:=0))

            Assert.AreEqual("Barón Rojo", query("field_band"))
            Assert.AreEqual("all", query("field_country"))
            Assert.AreEqual("all", query("field_genre"))
            Assert.AreEqual("all", query("field_year"))
        End Sub

        ''' <summary>
        ''' Reserved URL characters in the artist name are encoded, so they do not break the query string.
        ''' </summary>
        <TestMethod>
        Public Sub SearchQueryUri_EncodesReservedCharacters()
            Dim address As Uri = New SearchQuery("Guns & Roses?=#").Uri(searchPage:=0)
            Dim query As NameValueCollection = CrawlerQueryTests.ParseQuery(address)

            Assert.AreEqual("Guns & Roses?=#", query("field_band"))
            Assert.AreEqual(15, query.Count)
            StringAssert.Contains(address.OriginalString, "field_band=Guns+%26+Roses%3f%3d%23")
        End Sub

        ''' <summary>
        ''' The search query carries the search page index.
        ''' </summary>
        <TestMethod>
        <DataRow(0)>
        <DataRow(1)>
        <DataRow(57)>
        Public Sub SearchQueryUri_CarriesSearchPage(searchPage As Integer)
            Dim query As NameValueCollection = CrawlerQueryTests.ParseQuery(New SearchQuery().Uri(searchPage))

            Assert.AreEqual(searchPage.ToString(CultureInfo.InvariantCulture), query("SobiSearchPage"))
        End Sub

        ''' <summary>
        ''' The parameterless Uri property represents the first search page.
        ''' </summary>
        <TestMethod>
        Public Sub SearchQueryUri_Default_IsTheFirstSearchPage()
            Dim searchQuery As New SearchQuery("Accept")

            Assert.AreEqual(searchQuery.Uri(searchPage:=0), searchQuery.Uri)
        End Sub

        ''' <summary>
        ''' A negative search page index is rejected.
        ''' </summary>
        <TestMethod>
        Public Sub SearchQueryUri_NegativeSearchPage_Throws()
            Assert.ThrowsExactly(Of ArgumentOutOfRangeException)(Function() New SearchQuery().Uri(searchPage:=-1))
        End Sub

#End Region

#Region " BuildDownloadUrlsQuery "

        ''' <summary>
        ''' The download links query targets the FHM XML endpoint with the expected parameters.
        ''' </summary>
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

        ''' <summary>
        ''' An empty or white-space album identifier is rejected.
        ''' </summary>
        <TestMethod>
        <DataRow("")>
        <DataRow("   ")>
        Public Sub BuildDownloadUrlsQuery_BlankId_Throws(sobi2Id As String)
            Assert.ThrowsExactly(Of ArgumentException)(Sub() Crawler.BuildDownloadUrlsQuery(sobi2Id))
        End Sub

        ''' <summary>
        ''' A null album identifier is rejected.
        ''' </summary>
        <TestMethod>
        Public Sub BuildDownloadUrlsQuery_NullId_Throws()
            Assert.ThrowsExactly(Of ArgumentException)(Sub() Crawler.BuildDownloadUrlsQuery(Nothing))
        End Sub

#End Region

#Region " GetPageCount "

        ''' <summary>
        ''' The page count is the album count divided by the page size, rounded up.
        ''' <para></para>
        ''' Regression: exact multiples of the page size used to request an extra (empty) page, which crashed the crawler.
        ''' </summary>
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

        ''' <summary>
        ''' A negative album count is rejected.
        ''' </summary>
        <TestMethod>
        Public Sub GetPageCount_NegativeCount_Throws()
            Assert.ThrowsExactly(Of ArgumentOutOfRangeException)(Sub() Crawler.GetPageCount(-1))
        End Sub

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Parses the query string of an absolute address.
        ''' </summary>
        ''' <param name="address">
        ''' The absolute address.
        ''' </param>
        ''' <returns>
        ''' The parsed (decoded) query parameters.
        ''' </returns>
        Private Shared Function ParseQuery(address As String) As NameValueCollection
            Return CrawlerQueryTests.ParseQuery(New Uri(address))
        End Function

        ''' <summary>
        ''' Parses the query string parameters of an address.
        ''' </summary>
        '''
        ''' <param name="address">
        ''' The address.
        ''' </param>
        '''
        ''' <returns>
        ''' The decoded query string parameters, in order.
        ''' </returns>
        Private Shared Function ParseQuery(address As Uri) As NameValueCollection
            Return HttpUtility.ParseQueryString(address.Query)
        End Function

#End Region

    End Class

End Namespace

#End Region
