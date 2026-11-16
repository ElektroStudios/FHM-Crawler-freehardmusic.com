' ***********************************************************************
' Author   : ElektroStudios
' Modified : 15-November-2026
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.Collections.ObjectModel

#End Region

#Region " PageCrawlEndEventArgs "

Namespace FHM

    ''' <summary>
    ''' Represents the event data of the <see cref="Crawler.PageCrawlEnd"/> event.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed NuGet packages:
    ''' <para></para>
    ''' <see href="https://www.nuget.org/packages/HtmlAgilityPack">HtmlAgilityPack</see>
    ''' </remarks>
    Public NotInheritable Class PageCrawlEndEventArgs : Inherits EventArgs

#Region " Properties "

        ''' <summary>
        ''' Gets the search query used.
        ''' </summary>
        '''
        ''' <value>
        ''' The search query used.
        ''' </value>
        Public ReadOnly Property SearchQuery As SearchQuery

        ''' <summary>
        ''' Gets the index of the search page crawled.
        ''' </summary>
        '''
        ''' <value>
        ''' The index of the search page crawled.
        ''' </value>
        Public ReadOnly Property SearchPage As Integer

        ''' <summary>
        ''' Gets a collection of <see cref="AlbumInfo"/> that contains the information of the albums that were crawled.
        ''' </summary>
        '''
        ''' <value>
        ''' A collection of <see cref="AlbumInfo"/> that contains the information of the albums that were crawled.
        ''' <para></para>
        ''' It can be empty (for example, when none of the albums of the page has download links).
        ''' </value>
        Public ReadOnly Property Albums As ReadOnlyCollection(Of AlbumInfo)

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="PageCrawlEndEventArgs"/> class.
        ''' </summary>
        '''
        ''' <param name="searchQuery">
        ''' The search query used.
        ''' </param>
        '''
        ''' <param name="searchPage">
        ''' The index of the search page crawled.
        ''' </param>
        '''
        ''' <param name="albums">
        ''' A collection of <see cref="AlbumInfo"/> that contains the information of the albums that were crawled.
        ''' <para></para>
        ''' The collection is copied, so later changes to it do not affect this instance.
        ''' </param>
        '''
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when <paramref name="searchQuery"/> or <paramref name="albums"/> is <see langword="Nothing"/>.
        ''' </exception>
        '''
        ''' <exception cref="ArgumentOutOfRangeException">
        ''' Thrown when <paramref name="searchPage"/> is negative.
        ''' </exception>
        Public Sub New(searchQuery As SearchQuery, searchPage As Integer, albums As IList(Of AlbumInfo))
            If searchQuery Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(searchQuery))
            End If
            If searchPage < 0 Then
                Throw New ArgumentOutOfRangeException(paramName:=NameOf(searchPage), actualValue:=searchPage, message:="The search page index cannot be negative.")
            End If
            If albums Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(albums))
            End If

            Me.SearchQuery = searchQuery
            Me.SearchPage = searchPage
            Me.Albums = New ReadOnlyCollection(Of AlbumInfo)(New List(Of AlbumInfo)(albums))
        End Sub

#End Region

    End Class

End Namespace

#End Region
