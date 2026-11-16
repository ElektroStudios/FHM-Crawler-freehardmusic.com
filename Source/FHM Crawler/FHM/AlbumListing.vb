#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " AlbumListing "

Namespace FHM

    ''' <summary>
    ''' Represents an album as it is listed in a search results page of the FHM website,
    ''' before its download links are retrieved.
    ''' </summary>
    Friend NotInheritable Class AlbumListing

#Region " Properties "

        ''' <summary>
        ''' Gets the (normalized) artist name.
        ''' </summary>
        Friend ReadOnly Property Artist As String

        ''' <summary>
        ''' Gets the (normalized) album title.
        ''' </summary>
        Friend ReadOnly Property Title As String

        ''' <summary>
        ''' Gets the FHM identifier of the album (the <c>sobi2Id</c> query parameter of its details page).
        ''' </summary>
        Friend ReadOnly Property Sobi2Id As String

        ''' <summary>
        ''' Gets the absolute <see cref="Uri"/> of the album details page.
        ''' </summary>
        Friend ReadOnly Property AlbumUri As Uri

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="AlbumListing"/> class.
        ''' </summary>
        ''' <param name="artist">
        ''' The artist name.
        ''' </param>
        ''' 
        ''' <param name="title">
        ''' The album title.
        ''' </param>
        ''' 
        ''' <param name="sobi2Id">
        ''' The FHM identifier of the album.
        ''' </param>
        '''
        ''' <param name="albumUri">
        ''' The absolute <see cref="Uri"/> of the album details page.
        ''' </param>
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when <paramref name="artist"/>, <paramref name="title"/> or <paramref name="albumUri"/> is <see langword="Nothing"/>.
        ''' </exception>
        '''
        ''' <exception cref="ArgumentException">
        ''' Thrown when <paramref name="sobi2Id"/> is <see langword="Nothing"/>, empty or white-space,
        ''' or when <paramref name="albumUri"/> is not absolute.
        ''' </exception>
        Friend Sub New(artist As String, title As String, sobi2Id As String, albumUri As Uri)
            If artist Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(artist))
            End If
            If title Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(title))
            End If
            If String.IsNullOrWhiteSpace(sobi2Id) Then
                Throw New ArgumentException("The album identifier cannot be null, empty or white-space.", paramName:=NameOf(sobi2Id))
            End If
            If albumUri Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(albumUri))
            End If
            If Not albumUri.IsAbsoluteUri Then
                Throw New ArgumentException("The album Uri must be absolute.", paramName:=NameOf(albumUri))
            End If

            Me.Artist = artist
            Me.Title = title
            Me.Sobi2Id = sobi2Id
            Me.AlbumUri = albumUri
        End Sub

#End Region

    End Class

End Namespace

#End Region
