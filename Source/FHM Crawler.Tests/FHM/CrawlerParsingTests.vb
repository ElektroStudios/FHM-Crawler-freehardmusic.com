#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Xml

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports FHM

Imports FHMCrawler.Tests.Infrastructure

#End Region

#Region " CrawlerParsingTests "

Namespace FHMTests

    ''' <summary>
    ''' Tests the HTML and XML parsing functions of the <see cref="Crawler"/>.
    ''' </summary>
    <TestClass>
    Public NotInheritable Class CrawlerParsingTests

#Region " ParseAlbumCount "

        ''' <summary>
        ''' The album count is extracted from the search result summary, regardless of the surrounding white-spaces.
        ''' </summary>
        <TestMethod>
        Public Sub ParseAlbumCount_ReadsTheNumberFromTheSummary()
            Dim html As String = FhmSamples.SearchPage($"  Found{vbTab}{vbLf}  1234   entries  ")

            Assert.AreEqual(1234, Crawler.ParseAlbumCount(html))
        End Sub

        ''' <summary>
        ''' Only the first number of the summary is taken as the album count.
        ''' </summary>
        <TestMethod>
        Public Sub ParseAlbumCount_TakesTheFirstNumber()
            Dim html As String = FhmSamples.SearchPage("42 entries found in 3 categories")

            Assert.AreEqual(42, Crawler.ParseAlbumCount(html))
        End Sub

        ''' <summary>
        ''' A page without search result summary (the website omits it when there are no results) yields zero.
        ''' </summary>
        <TestMethod>
        Public Sub ParseAlbumCount_NoSummary_ReturnsZero()
            Assert.AreEqual(0, Crawler.ParseAlbumCount(FhmSamples.SearchPage(summary:=Nothing)))
            Assert.AreEqual(0, Crawler.ParseAlbumCount(String.Empty))
            Assert.AreEqual(0, Crawler.ParseAlbumCount("<html><body>Maintenance</body></html>"))
        End Sub

        ''' <summary>
        ''' A summary without any number means an unexpected markup change, so it fails loudly.
        ''' </summary>
        <TestMethod>
        Public Sub ParseAlbumCount_SummaryWithoutNumber_ThrowsFormatException()
            Dim html As String = FhmSamples.SearchPage("No entries found")

            Assert.ThrowsExactly(Of FormatException)(Sub() Crawler.ParseAlbumCount(html))
        End Sub

        ''' <summary>
        ''' A null HTML source code is rejected.
        ''' </summary>
        <TestMethod>
        Public Sub ParseAlbumCount_Null_Throws()
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub() Crawler.ParseAlbumCount(Nothing))
        End Sub

#End Region

#Region " ParseAlbumListings "

        ''' <summary>
        ''' Every album table of the page is parsed into a listing, in document order.
        ''' </summary>
        <TestMethod>
        Public Sub ParseAlbumListings_ParsesEveryAlbumInOrder()
            Dim html As String = FhmSamples.SearchPage(2,
                                                       FhmSamples.AlbumTableWithId("iron maiden", "the number of the beast", "101"),
                                                       FhmSamples.AlbumTableWithId("Metallica", "Master Of Puppets", "202"))

            Dim listings As List(Of AlbumListing) = Crawler.ParseAlbumListings(html)

            Assert.AreEqual(2, listings.Count)
            Assert.AreEqual("Iron Maiden", listings(0).Artist)
            Assert.AreEqual("The Number Of The Beast", listings(0).Title)
            Assert.AreEqual("101", listings(0).Sobi2Id)
            Assert.AreEqual("Metallica", listings(1).Artist)
            Assert.AreEqual("Master Of Puppets", listings(1).Title)
            Assert.AreEqual("202", listings(1).Sobi2Id)
        End Sub

        ''' <summary>
        ''' HTML entities in the artist name and in the album title are decoded.
        ''' </summary>
        <TestMethod>
        Public Sub ParseAlbumListings_DecodesHtmlEntities()
            Dim html As String = FhmSamples.SearchPage(1, FhmSamples.AlbumTableWithId("Guns N&#39; Roses", "Use Your Illusion I &amp; II", "7"))

            Dim listing As AlbumListing = Crawler.ParseAlbumListings(html).Single()

            Assert.AreEqual("Guns N' Roses", listing.Artist)
            Assert.AreEqual("Use Your Illusion I & II", listing.Title)
        End Sub

        ''' <summary>
        ''' A missing artist or title node falls back to "Unknown" (title-cased), as the original crawler did.
        ''' </summary>
        <TestMethod>
        Public Sub ParseAlbumListings_MissingArtistOrTitle_FallsBackToUnknown()
            Dim html As String = FhmSamples.SearchPage(2,
                                                       FhmSamples.AlbumTable(artist:=Nothing, title:="Some Album", href:=FhmSamples.DetailsHref("1")),
                                                       FhmSamples.AlbumTable(artist:="Some Artist", title:=Nothing, href:=FhmSamples.DetailsHref("2")))

            Dim listings As List(Of AlbumListing) = Crawler.ParseAlbumListings(html)

            Assert.AreEqual(2, listings.Count)
            Assert.AreEqual("Unknown", listings(0).Artist)
            Assert.AreEqual("Some Album", listings(0).Title)
            Assert.AreEqual("Some Artist", listings(1).Artist)
            Assert.AreEqual("Unknown", listings(1).Title)
        End Sub

        ''' <summary>
        ''' Albums without details link, or whose link has no album identifier, are skipped.
        ''' </summary>
        <TestMethod>
        Public Sub ParseAlbumListings_SkipsAlbumsWithoutUsableLink()
            Dim html As String = FhmSamples.SearchPage(4,
                                                       FhmSamples.AlbumTable("No Link", "Album", href:=Nothing),
                                                       FhmSamples.AlbumTable("No Id", "Album", "http://freehardmusic.com/index.php?option=com_sobi2&amp;Itemid=13"),
                                                       FhmSamples.AlbumTable("Empty Link", "Album", ""),
                                                       FhmSamples.AlbumTableWithId("Valid", "Album", "99"))

            Dim listings As List(Of AlbumListing) = Crawler.ParseAlbumListings(html)

            Assert.AreEqual(1, listings.Count)
            Assert.AreEqual("Valid", listings(0).Artist)
            Assert.AreEqual("99", listings(0).Sobi2Id)
        End Sub

        ''' <summary>
        ''' A page without album tables yields an empty list.
        ''' <para></para>
        ''' Regression: HtmlAgilityPack returns Nothing in that case, which used to throw a <see cref="NullReferenceException"/>.
        ''' </summary>
        <TestMethod>
        Public Sub ParseAlbumListings_PageWithoutAlbums_ReturnsEmptyList()
            Dim listings As List(Of AlbumListing) = Crawler.ParseAlbumListings(FhmSamples.SearchPage(0))

            Assert.IsNotNull(listings)
            Assert.AreEqual(0, listings.Count)
        End Sub

        ''' <summary>
        ''' A null HTML source code is rejected.
        ''' </summary>
        <TestMethod>
        Public Sub ParseAlbumListings_Null_Throws()
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub() Crawler.ParseAlbumListings(Nothing))
        End Sub

#End Region

#Region " TryExtractSobi2Id "

        ''' <summary>
        ''' The album identifier is extracted from absolute and relative links,
        ''' with or without HTML-encoded ampersands, regardless of the parameter position and case.
        ''' </summary>
        <TestMethod>
        <DataRow("http://freehardmusic.com/index.php?option=com_sobi2&amp;sobi2Task=sobi2Details&amp;sobi2Id=123&amp;Itemid=13", "123")>
        <DataRow("http://freehardmusic.com/index.php?option=com_sobi2&sobi2Task=sobi2Details&sobi2Id=456", "456")>
        <DataRow("/index.php?sobi2Id=789&option=com_sobi2", "789")>
        <DataRow("index.php?option=com_sobi2&amp;SOBI2ID=1011", "1011")>
        <DataRow("  http://freehardmusic.com/index.php?sobi2Id=%2012%20  ", "12")>
        Public Sub TryExtractSobi2Id_ValidLink_ReturnsTheId(href As String, expectedId As String)
            Dim sobi2Id As String = Nothing

            Dim success As Boolean = Crawler.TryExtractSobi2Id(href, sobi2Id)

            Assert.IsTrue(success)
            Assert.AreEqual(expectedId, sobi2Id)
        End Sub

        ''' <summary>
        ''' Links without album identifier are rejected.
        ''' </summary>
        <TestMethod>
        <DataRow("")>
        <DataRow("   ")>
        <DataRow("#")>
        <DataRow("http://freehardmusic.com/index.php?option=com_sobi2&amp;Itemid=13")>
        <DataRow("http://freehardmusic.com/index.php?sobi2Id=")>
        <DataRow("http://freehardmusic.com/index.php?sobi2Id=%20%20")>
        Public Sub TryExtractSobi2Id_LinkWithoutId_ReturnsFalse(href As String)
            Dim sobi2Id As String = "previous value"

            Dim success As Boolean = Crawler.TryExtractSobi2Id(href, sobi2Id)

            Assert.IsFalse(success)
            Assert.IsNull(sobi2Id)
        End Sub

        ''' <summary>
        ''' A null link is rejected without throwing.
        ''' </summary>
        <TestMethod>
        Public Sub TryExtractSobi2Id_Null_ReturnsFalse()
            Dim sobi2Id As String = Nothing

            Assert.IsFalse(Crawler.TryExtractSobi2Id(Nothing, sobi2Id))
            Assert.IsNull(sobi2Id)
        End Sub

#End Region

#Region " NormalizeText "

        ''' <summary>
        ''' The text is trimmed, HTML-decoded and title-cased; all-uppercase words (acronyms) are preserved.
        ''' </summary>
        <TestMethod>
        <DataRow("  black sabbath  ", "Black Sabbath")>
        <DataRow("DEEP purple", "DEEP Purple")>
        <DataRow("AC/DC", "AC/DC")>
        <DataRow("simon &amp; garfunkel", "Simon & Garfunkel")>
        <DataRow("unknown", "Unknown")>
        <DataRow("", "")>
        Public Sub NormalizeText_TrimsDecodesAndTitleCases(rawText As String, expected As String)
            Assert.AreEqual(expected, Crawler.NormalizeText(rawText))
        End Sub

        ''' <summary>
        ''' UTF-8 text that was decoded with the system ANSI code page (what WebClient does) is repaired.
        ''' </summary>
        <TestMethod>
        Public Sub NormalizeText_RepairsUtf8DecodedAsAnsi()
            Dim garbled As String = Encoding.Default.GetString(Encoding.UTF8.GetBytes("Motörhead"))

            Assert.AreEqual("Motörhead", Crawler.NormalizeText(garbled))
        End Sub

        ''' <summary>
        ''' A null text is rejected.
        ''' </summary>
        <TestMethod>
        Public Sub NormalizeText_Null_Throws()
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub() Crawler.NormalizeText(Nothing))
        End Sub

#End Region

#Region " ParseDownloadUrls "

        ''' <summary>
        ''' The download links are returned in slot order, trimmed, skipping the empty slots.
        ''' </summary>
        <TestMethod>
        Public Sub ParseDownloadUrls_ReturnsNonEmptySlotsInOrder()
            Dim xml As String = FhmSamples.DownloadLinksXml("http://host/a", "", "   ", "http://host/b   ", "  http://host/c")

            Dim urls As String() = Crawler.ParseDownloadUrls(xml)

            CollectionAssert.AreEqual({"http://host/a", "http://host/b", "http://host/c"}, urls)
        End Sub

        ''' <summary>
        ''' All the 13 slots are read, and elements outside the known slots are ignored.
        ''' </summary>
        <TestMethod>
        Public Sub ParseDownloadUrls_ReadsAllThirteenSlotsOnly()
            Dim values As String() = Enumerable.Range(1, 14).Select(Function(i As Integer) $"http://host/{i}").ToArray()
            Dim xml As String = FhmSamples.DownloadLinksXml(values)

            Dim urls As String() = Crawler.ParseDownloadUrls(xml)

            Assert.AreEqual(13, urls.Length)
            Assert.AreEqual("http://host/1", urls(0))
            Assert.AreEqual("http://host/13", urls(12))
        End Sub

        ''' <summary>
        ''' Missing slots are tolerated (not every album fills all of them).
        ''' </summary>
        <TestMethod>
        Public Sub ParseDownloadUrls_ToleratesMissingSlots()
            Dim xml As String = "<rev><msg5>http://host/five</msg5><other>x</other><msg13>http://host/thirteen</msg13></rev>"

            CollectionAssert.AreEqual({"http://host/five", "http://host/thirteen"}, Crawler.ParseDownloadUrls(xml))
            Assert.AreEqual(0, Crawler.ParseDownloadUrls("<rev/>").Length)
        End Sub

        ''' <summary>
        ''' A malformed XML document throws an <see cref="XmlException"/>.
        ''' </summary>
        <TestMethod>
        <DataRow("")>
        <DataRow("<html><body>Error</body>")>
        <DataRow("<rev><msg>unclosed</rev>")>
        Public Sub ParseDownloadUrls_MalformedXml_ThrowsXmlException(xml As String)
            Assert.ThrowsExactly(Of XmlException)(Sub() Crawler.ParseDownloadUrls(xml))
        End Sub

        ''' <summary>
        ''' A well-formed document with an unexpected root element throws a <see cref="FormatException"/>.
        ''' </summary>
        <TestMethod>
        Public Sub ParseDownloadUrls_UnexpectedRoot_ThrowsFormatException()
            Assert.ThrowsExactly(Of FormatException)(Sub() Crawler.ParseDownloadUrls("<error><msg>http://host/a</msg></error>"))
        End Sub

        ''' <summary>
        ''' A null XML document is rejected.
        ''' </summary>
        <TestMethod>
        Public Sub ParseDownloadUrls_Null_Throws()
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub() Crawler.ParseDownloadUrls(Nothing))
        End Sub

#End Region

    End Class

End Namespace

#End Region
