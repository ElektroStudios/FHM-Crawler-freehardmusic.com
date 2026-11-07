#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports FHM

#End Region

#Region " ModelTests "

Namespace FHMTests

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Tests the data types of the FHM namespace: <see cref="SearchFilter"/>, <see cref="AlbumInfo"/>,
    ''' <see cref="AlbumListing"/>, <see cref="PageCrawlBeginEventArgs"/> and <see cref="PageCrawlEndEventArgs"/>.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestClass>
    Public NotInheritable Class ModelTests

#Region " SearchFilter "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The parameterless construction searches everything.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub SearchFilter_Default_SearchesEverything()
            Dim filter As New SearchFilter()

            Assert.AreEqual("", filter.Artist)
            Assert.AreEqual("all", filter.Genre)
            Assert.AreEqual("all", filter.Country)
            Assert.AreEqual("all", filter.Year)
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The artist constructor searches the artist in all genres, countries and years.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub SearchFilter_Artist_SearchesTheArtistEverywhere()
            Dim filter As New SearchFilter("Judas Priest")

            Assert.AreEqual("Judas Priest", filter.Artist)
            Assert.AreEqual("all", filter.Genre)
            Assert.AreEqual("all", filter.Country)
            Assert.AreEqual("all", filter.Year)
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The custom search constructor sets the specified criteria and leaves the artist empty.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub SearchFilter_CustomSearch_SetsTheCriteria()
            Dim filter As New SearchFilter(genre:="Thrash metal", country:="Germany", year:="1985")

            Assert.AreEqual("", filter.Artist)
            Assert.AreEqual("Thrash metal", filter.Genre)
            Assert.AreEqual("Germany", filter.Country)
            Assert.AreEqual("1985", filter.Year)
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The omitted criteria of the custom search constructor default to "all".
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub SearchFilter_CustomSearch_OmittedCriteriaDefaultToAll()
            Dim filter As New SearchFilter(year:="2001")

            Assert.AreEqual("all", filter.Genre)
            Assert.AreEqual("all", filter.Country)
            Assert.AreEqual("2001", filter.Year)
        End Sub

#End Region

#Region " AlbumInfo "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The album information keeps the provided values.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub AlbumInfo_KeepsValues()
            Dim urls As String() = {"http://host/a", "http://host/b"}

            Dim album As New AlbumInfo("Whitesnake", "Slip Of The Tongue", urls)

            Assert.AreEqual("Whitesnake", album.Artist)
            Assert.AreEqual("Slip Of The Tongue", album.Album)
            Assert.AreSame(urls, album.DounloadUrls)
        End Sub

#End Region

#Region " AlbumListing "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The album listing keeps the provided values.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub AlbumListing_KeepsValues()
            Dim listing As New AlbumListing("Europe", "The Final Countdown", "1986")

            Assert.AreEqual("Europe", listing.Artist)
            Assert.AreEqual("The Final Countdown", listing.Title)
            Assert.AreEqual("1986", listing.Sobi2Id)
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Invalid album listing values are rejected.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub AlbumListing_InvalidValues_Throw()
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub()
                                                 Dim unused As New AlbumListing(Nothing, "Title", "1")
                                             End Sub)
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub()
                                                 Dim unused As New AlbumListing("Artist", Nothing, "1")
                                             End Sub)
            Assert.ThrowsExactly(Of ArgumentException)(Sub()
                                                 Dim unused As New AlbumListing("Artist", "Title", Nothing)
                                             End Sub)
            Assert.ThrowsExactly(Of ArgumentException)(Sub()
                                                 Dim unused As New AlbumListing("Artist", "Title", " ")
                                             End Sub)
        End Sub

#End Region

#Region " Event Args "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The begin event data keeps the page index and derives from <see cref="EventArgs"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub PageCrawlBeginEventArgs_KeepsPageIndex()
            Dim e As New PageCrawlBeginEventArgs(3)

            Assert.AreEqual(3, e.PageIndex)
            Assert.IsInstanceOfType(Of EventArgs)(e)
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The end event data keeps the page index and the albums, and derives from <see cref="EventArgs"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub PageCrawlEndEventArgs_KeepsPageIndexAndAlbums()
            Dim albums As New List(Of AlbumInfo) From {New AlbumInfo("A", "B", Array.Empty(Of String)())}

            Dim e As New PageCrawlEndEventArgs(7, albums)

            Assert.AreEqual(7, e.PageIndex)
            Assert.AreSame(albums, e.Albums)
            Assert.IsInstanceOfType(Of EventArgs)(e)
        End Sub

#End Region

    End Class

End Namespace

#End Region
