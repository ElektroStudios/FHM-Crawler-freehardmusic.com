
Namespace FHM

    Public NotInheritable Class PageCrawlBeginEventArgs

        Public Property PageIndex As Integer

        Private Sub New()
        End Sub

        Public Sub New(pageIndex As Integer)
            Me.PageIndex = pageIndex
        End Sub

    End Class

End Namespace
