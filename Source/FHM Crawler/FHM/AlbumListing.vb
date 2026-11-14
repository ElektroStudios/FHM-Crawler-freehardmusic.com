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
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when <paramref name="artist"/> or <paramref name="title"/> is <see langword="Nothing"/>.
        ''' </exception>
        ''' 
        ''' <exception cref="ArgumentException">
        ''' Thrown when <paramref name="sobi2Id"/> is <see langword="Nothing"/>, empty or white-space.
        ''' </exception>
        Friend Sub New(artist As String, title As String, sobi2Id As String)
            If artist Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(artist))
            End If
            If title Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(title))
            End If
            If String.IsNullOrWhiteSpace(sobi2Id) Then
                Throw New ArgumentException("The album identifier cannot be null, empty or white-space.", paramName:=NameOf(sobi2Id))
            End If

            Me.Artist = artist
            Me.Title = title
            Me.Sobi2Id = sobi2Id
        End Sub

#End Region

    End Class

End Namespace

#End Region
