
Namespace FHM

    Public NotInheritable Class PageCrawlEndEventArgs : Inherits EventArgs

        Public Property PageIndex As Integer

        Public Property Albums As List(Of AlbumInfo)

        Private Sub New()
        End Sub

        Public Sub New(ByVal pageIndex As Integer, ByVal albums As List(Of AlbumInfo))
            Me.PageIndex = pageIndex
            Me.Albums = albums
        End Sub

    End Class

End Namespace
