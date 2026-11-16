#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.Collections.ObjectModel

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports FHM

#End Region

#Region " ModelTests "

Namespace FHMTests

    ''' <summary>
    ''' Tests the crawler data model: <see cref="SearchQuery"/>, <see cref="AlbumInfo"/>,
    ''' <see cref="AlbumListing"/>, <see cref="PageCrawlBeginEventArgs"/> and <see cref="PageCrawlEndEventArgs"/>.
    ''' </summary>
    <TestClass>
    Public NotInheritable Class ModelTests

#Region " SearchQuery "

        ''' <summary>
        ''' A default search query searches everything.
        ''' </summary>
        <TestMethod>
        Public Sub SearchQuery_Default_SearchesEverything()
            Dim query As New SearchQuery()

            Assert.AreEqual("", query.Artist)
            Assert.AreEqual("all", query.Genre)
            Assert.AreEqual("all", query.Country)
            Assert.AreEqual("all", query.Year)
        End Sub

        ''' <summary>
        ''' An artist-name based search sets the other criteria to "all".
        ''' </summary>
        <TestMethod>
        Public Sub SearchQuery_Artist_SearchesTheArtistEverywhere()
            Dim query As New SearchQuery("Judas Priest")

            Assert.AreEqual("Judas Priest", query.Artist)
            Assert.AreEqual("all", query.Genre)
            Assert.AreEqual("all", query.Country)
            Assert.AreEqual("all", query.Year)
        End Sub

        ''' <summary>
        ''' A custom search sets the specified criteria and no artist name.
        ''' </summary>
        <TestMethod>
        Public Sub SearchQuery_CustomSearch_SetsTheCriteria()
            Dim query As New SearchQuery(genre:="Thrash metal", country:="Germany", year:="1985")

            Assert.AreEqual("", query.Artist)
            Assert.AreEqual("Thrash metal", query.Genre)
            Assert.AreEqual("Germany", query.Country)
            Assert.AreEqual("1985", query.Year)
        End Sub

        ''' <summary>
        ''' The criteria omitted in a custom search default to "all".
        ''' </summary>
        <TestMethod>
        Public Sub SearchQuery_CustomSearch_OmittedCriteriaDefaultToAll()
            Dim query As New SearchQuery(year:="2001")

            Assert.AreEqual("all", query.Genre)
            Assert.AreEqual("all", query.Country)
            Assert.AreEqual("2001", query.Year)
        End Sub

        ''' <summary>
        ''' An artist name cannot be set while a country, genre or year criteria is set.
        ''' </summary>
        <TestMethod>
        <DataRow("Germany", "all", "all")>
        <DataRow("all", "Thrash metal", "all")>
        <DataRow("all", "all", "1985")>
        Public Sub SearchQuery_ArtistWithCustomCriteria_Throws(country As String, genre As String, year As String)
            Dim query As New SearchQuery(genre:=genre, country:=country, year:=year)

            Assert.ThrowsExactly(Of ArgumentException)(Sub() query.Artist = "Kreator")
            Assert.AreEqual("", query.Artist)
        End Sub

        ''' <summary>
        ''' A country, genre or year criteria cannot be set while an artist name is set.
        ''' </summary>
        <TestMethod>
        Public Sub SearchQuery_CustomCriteriaWithArtist_Throws()
            Dim query As New SearchQuery("Kreator")

            Assert.ThrowsExactly(Of ArgumentException)(Sub() query.Country = "Germany")
            Assert.ThrowsExactly(Of ArgumentException)(Sub() query.Genre = "Thrash metal")
            Assert.ThrowsExactly(Of ArgumentException)(Sub() query.Year = "1985")
            Assert.AreEqual("all", query.Country)
            Assert.AreEqual("all", query.Genre)
            Assert.AreEqual("all", query.Year)
        End Sub

        ''' <summary>
        ''' Setting a criteria to "all" (case-insensitive) is always allowed, even in an artist-name based search.
        ''' </summary>
        <TestMethod>
        Public Sub SearchQuery_AllCriteriaWithArtist_IsAllowed()
            Dim query As New SearchQuery("Kreator")

            query.Country = "ALL"
            query.Genre = "all"
            query.Year = "All"

            Assert.AreEqual("Kreator", query.Artist)
        End Sub

        ''' <summary>
        ''' Clearing the artist name is always allowed, even in a custom search.
        ''' </summary>
        <TestMethod>
        Public Sub SearchQuery_ClearingArtistWithCustomCriteria_IsAllowed()
            Dim query As New SearchQuery(country:="Germany")

            query.Artist = ""

            Assert.AreEqual("", query.Artist)
            Assert.AreEqual("Germany", query.Country)
        End Sub

        ''' <summary>
        ''' Null values are rejected by the constructors and the properties.
        ''' </summary>
        <TestMethod>
        Public Sub SearchQuery_NullValues_Throw()
            Dim query As New SearchQuery()

            Assert.ThrowsExactly(Of ArgumentNullException)(Sub() query.Artist = Nothing)
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub() query.Country = Nothing)
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub() query.Genre = Nothing)
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub() query.Year = Nothing)
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub()
                                                               Dim unused As New SearchQuery(artist:=Nothing)
                                                           End Sub)
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub()
                                                               Dim unused As New SearchQuery(genre:=Nothing)
                                                           End Sub)
        End Sub

        ''' <summary>
        ''' Reset restores the default values of an artist-name based search.
        ''' </summary>
        <TestMethod>
        Public Sub SearchQuery_Reset_FromArtistSearch_RestoresDefaults()
            Dim query As New SearchQuery("Saxon")

            query.Reset()

            Assert.AreEqual("", query.Artist)
            Assert.AreEqual("all", query.Country)
            Assert.AreEqual("all", query.Genre)
            Assert.AreEqual("all", query.Year)
        End Sub

        ''' <summary>
        ''' Regression: Reset restores the default values of a custom search.
        ''' <para></para>
        ''' The original DevCase implementation set the artist name first, and the Artist property setter
        ''' threw an <see cref="ArgumentException"/> because the custom criteria were not "all" yet.
        ''' </summary>
        <TestMethod>
        Public Sub SearchQuery_Reset_FromCustomSearch_RestoresDefaults()
            Dim query As New SearchQuery(genre:="NWOBHM", country:="United Kingdom", year:="1980")

            query.Reset()

            Assert.AreEqual("", query.Artist)
            Assert.AreEqual("all", query.Country)
            Assert.AreEqual("all", query.Genre)
            Assert.AreEqual("all", query.Year)
        End Sub

        ''' <summary>
        ''' ToString returns the search criteria as a query string.
        ''' </summary>
        <TestMethod>
        Public Sub SearchQuery_ToString_ReturnsTheSearchCriteria()
            Dim query As New SearchQuery(genre:="Heavy metal", country:="Spain")

            Assert.AreEqual("field_band=&field_country=Spain&field_genre=Heavy+metal&field_year=all", query.ToString())
        End Sub

#End Region

#Region " AlbumInfo "

        ''' <summary>
        ''' The constructor keeps the specified values.
        ''' </summary>
        <TestMethod>
        Public Sub AlbumInfo_KeepsValues()
            Dim albumUri As New Uri("http://freehardmusic.com/index.php?sobi2Id=42")
            Dim urls As String() = {"http://host/a", "http://host/b"}

            Dim album As New AlbumInfo("42", albumUri, "Whitesnake", "Slip Of The Tongue", urls)

            Assert.AreEqual("42", album.Id)
            Assert.AreSame(albumUri, album.Uri)
            Assert.AreEqual("Whitesnake", album.Artist)
            Assert.AreEqual("Slip Of The Tongue", album.Title)
            CollectionAssert.AreEqual(urls, album.DownloadLinks)
        End Sub

        ''' <summary>
        ''' The download links are a read-only copy: changes to the source collection do not affect the album.
        ''' </summary>
        <TestMethod>
        Public Sub AlbumInfo_DownloadLinks_IsAReadOnlyCopy()
            Dim urls As New List(Of String) From {"http://host/a"}

            Dim album As New AlbumInfo("1", New Uri("http://freehardmusic.com/"), "A", "B", urls)
            urls.Add("http://host/b")

            Assert.IsInstanceOfType(Of ReadOnlyCollection(Of String))(album.DownloadLinks)
            CollectionAssert.AreEqual({"http://host/a"}, album.DownloadLinks)
        End Sub

        ''' <summary>
        ''' Invalid values are rejected.
        ''' </summary>
        <TestMethod>
        Public Sub AlbumInfo_InvalidValues_Throw()
            Dim albumUri As New Uri("http://freehardmusic.com/")
            Dim urls As String() = Array.Empty(Of String)()

            Assert.ThrowsExactly(Of ArgumentException)(Sub()
                                                           Dim unused As New AlbumInfo(" ", albumUri, "A", "B", urls)
                                                       End Sub)
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub()
                                                               Dim unused As New AlbumInfo("1", Nothing, "A", "B", urls)
                                                           End Sub)
            Assert.ThrowsExactly(Of ArgumentException)(Sub()
                                                           Dim unused As New AlbumInfo("1", New Uri("/index.php", UriKind.Relative), "A", "B", urls)
                                                       End Sub)
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub()
                                                               Dim unused As New AlbumInfo("1", albumUri, Nothing, "B", urls)
                                                           End Sub)
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub()
                                                               Dim unused As New AlbumInfo("1", albumUri, "A", Nothing, urls)
                                                           End Sub)
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub()
                                                               Dim unused As New AlbumInfo("1", albumUri, "A", "B", Nothing)
                                                           End Sub)
        End Sub

#End Region

#Region " AlbumListing "

        ''' <summary>
        ''' The constructor keeps the specified values.
        ''' </summary>
        <TestMethod>
        Public Sub AlbumListing_KeepsValues()
            Dim albumUri As New Uri("http://freehardmusic.com/index.php?sobi2Id=1986")

            Dim listing As New AlbumListing("Europe", "The Final Countdown", "1986", albumUri)

            Assert.AreEqual("Europe", listing.Artist)
            Assert.AreEqual("The Final Countdown", listing.Title)
            Assert.AreEqual("1986", listing.Sobi2Id)
            Assert.AreSame(albumUri, listing.AlbumUri)
        End Sub

        ''' <summary>
        ''' Invalid values are rejected.
        ''' </summary>
        <TestMethod>
        Public Sub AlbumListing_InvalidValues_Throw()
            Dim albumUri As New Uri("http://freehardmusic.com/")

            Assert.ThrowsExactly(Of ArgumentNullException)(Sub()
                                                               Dim unused As New AlbumListing(Nothing, "Title", "1", albumUri)
                                                           End Sub)
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub()
                                                               Dim unused As New AlbumListing("Artist", Nothing, "1", albumUri)
                                                           End Sub)
            Assert.ThrowsExactly(Of ArgumentException)(Sub()
                                                           Dim unused As New AlbumListing("Artist", "Title", Nothing, albumUri)
                                                       End Sub)
            Assert.ThrowsExactly(Of ArgumentException)(Sub()
                                                           Dim unused As New AlbumListing("Artist", "Title", " ", albumUri)
                                                       End Sub)
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub()
                                                               Dim unused As New AlbumListing("Artist", "Title", "1", Nothing)
                                                           End Sub)
            Assert.ThrowsExactly(Of ArgumentException)(Sub()
                                                           Dim unused As New AlbumListing("Artist", "Title", "1", New Uri("/a", UriKind.Relative))
                                                       End Sub)
        End Sub

#End Region

#Region " Event Args "

        ''' <summary>
        ''' The begin event data keeps the search query and the search page.
        ''' </summary>
        <TestMethod>
        Public Sub PageCrawlBeginEventArgs_KeepsValues()
            Dim query As New SearchQuery("Dio")

            Dim e As New PageCrawlBeginEventArgs(query, 3)

            Assert.AreSame(query, e.SearchQuery)
            Assert.AreEqual(3, e.SearchPage)
            Assert.IsInstanceOfType(Of EventArgs)(e)
        End Sub

        ''' <summary>
        ''' The end event data keeps the search query, the search page and a read-only copy of the albums.
        ''' </summary>
        <TestMethod>
        Public Sub PageCrawlEndEventArgs_KeepsValues()
            Dim query As New SearchQuery("Dio")
            Dim album As New AlbumInfo("1", New Uri("http://freehardmusic.com/"), "A", "B", Array.Empty(Of String)())
            Dim albums As New List(Of AlbumInfo) From {album}

            Dim e As New PageCrawlEndEventArgs(query, 7, albums)
            albums.Clear()

            Assert.AreSame(query, e.SearchQuery)
            Assert.AreEqual(7, e.SearchPage)
            Assert.AreEqual(1, e.Albums.Count)
            Assert.AreSame(album, e.Albums(0))
            Assert.IsInstanceOfType(Of EventArgs)(e)
        End Sub

        ''' <summary>
        ''' Invalid event data values are rejected.
        ''' </summary>
        <TestMethod>
        Public Sub PageCrawlEventArgs_InvalidValues_Throw()
            Dim query As New SearchQuery()
            Dim albums As New List(Of AlbumInfo)

            Assert.ThrowsExactly(Of ArgumentNullException)(Sub()
                                                               Dim unused As New PageCrawlBeginEventArgs(Nothing, 0)
                                                           End Sub)
            Assert.ThrowsExactly(Of ArgumentOutOfRangeException)(Sub()
                                                                     Dim unused As New PageCrawlBeginEventArgs(query, -1)
                                                                 End Sub)
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub()
                                                               Dim unused As New PageCrawlEndEventArgs(Nothing, 0, albums)
                                                           End Sub)
            Assert.ThrowsExactly(Of ArgumentOutOfRangeException)(Sub()
                                                                     Dim unused As New PageCrawlEndEventArgs(query, -1, albums)
                                                                 End Sub)
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub()
                                                               Dim unused As New PageCrawlEndEventArgs(query, 0, Nothing)
                                                           End Sub)
        End Sub

#End Region

    End Class

End Namespace

#End Region
