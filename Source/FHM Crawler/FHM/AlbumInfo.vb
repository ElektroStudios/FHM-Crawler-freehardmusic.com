
Namespace FHM

    Public NotInheritable Class AlbumInfo

        Public Property Artist As String
        Public Property Album As String
        ' Public Property Country As String
        ' Public Property Genre As String
        ' Public Property Year As String
        Public Property DounloadUrls As String()

        Private Sub New()
        End Sub

        Public Sub New(artist As String, album As String, dounloadUrls As String())
            Me.Artist = artist
            Me.Album = album
            Me.DounloadUrls = dounloadUrls
        End Sub

    End Class

End Namespace