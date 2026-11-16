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

#Region " AlbumInfo "

Namespace FHM

    ''' <summary>
    ''' Represents the information of an album crawled with <see cref="Crawler"/>.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed NuGet packages:
    ''' <para></para>
    ''' <see href="https://www.nuget.org/packages/HtmlAgilityPack">HtmlAgilityPack</see>
    ''' </remarks>
    Public NotInheritable Class AlbumInfo

#Region " Properties "

        ''' <summary>
        ''' Gets the album identifier (that is used in the 'sobiid' and 'sobi2id' parameters).
        ''' </summary>
        '''
        ''' <value>
        ''' The album identifier (that is used in the 'sobiid' and 'sobi2id' parameters).
        ''' </value>
        Public ReadOnly Property Id As String

        ''' <summary>
        ''' Gets the album <see cref="Uri"/>.
        ''' </summary>
        '''
        ''' <value>
        ''' The album <see cref="Uri"/>.
        ''' </value>
        Public ReadOnly Property Uri As Uri

        ''' <summary>
        ''' Gets the artist name.
        ''' </summary>
        '''
        ''' <value>
        ''' The artist name.
        ''' </value>
        Public ReadOnly Property Artist As String

        ''' <summary>
        ''' Gets the album title.
        ''' </summary>
        '''
        ''' <value>
        ''' The album title.
        ''' </value>
        Public ReadOnly Property Title As String

        ''' <summary>
        ''' Gets the urls to download the album. It can be a single url, or multiple of them.
        ''' </summary>
        '''
        ''' <value>
        ''' The urls to download the album. It can be a single url, or multiple of them.
        ''' </value>
        Public ReadOnly Property DownloadLinks As ReadOnlyCollection(Of String)

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="AlbumInfo"/> class.
        ''' </summary>
        '''
        ''' <param name="id">
        ''' The album identifier.
        ''' </param>
        '''
        ''' <param name="uri">
        ''' The album <see cref="Uri"/>.
        ''' </param>
        '''
        ''' <param name="artist">
        ''' The artist name.
        ''' </param>
        '''
        ''' <param name="title">
        ''' The album title.
        ''' </param>
        '''
        ''' <param name="downloadLinks">
        ''' The urls to download the album. It can be a single url, or multiple of them.
        ''' <para></para>
        ''' The collection is copied, so later changes to it do not affect this instance.
        ''' </param>
        '''
        ''' <exception cref="ArgumentException">
        ''' Thrown when <paramref name="id"/> is <see langword="Nothing"/>, empty or white-space,
        ''' or when <paramref name="uri"/> is not an absolute <see cref="System.Uri"/>.
        ''' </exception>
        '''
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when <paramref name="uri"/>, <paramref name="artist"/>, <paramref name="title"/>
        ''' or <paramref name="downloadLinks"/> is <see langword="Nothing"/>.
        ''' </exception>
        Public Sub New(id As String, uri As Uri,
                       artist As String, title As String,
                       downloadLinks As IList(Of String))

            If String.IsNullOrWhiteSpace(id) Then
                Throw New ArgumentException("The album identifier cannot be null, empty or white-space.", paramName:=NameOf(id))
            End If
            If uri Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(uri))
            End If
            If Not uri.IsAbsoluteUri Then
                Throw New ArgumentException("The album Uri must be absolute.", paramName:=NameOf(uri))
            End If
            If artist Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(artist))
            End If
            If title Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(title))
            End If
            If downloadLinks Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(downloadLinks))
            End If

            Me.Id = id
            Me.Uri = uri
            Me.Artist = artist
            Me.Title = title
            Me.DownloadLinks = New ReadOnlyCollection(Of String)(New List(Of String)(downloadLinks))
        End Sub

#End Region

    End Class

End Namespace

#End Region
