
Namespace FHM

    Public NotInheritable Class SearchFilter

        Public Property Artist As String
        Public Property Genre As String
        Public Property Country As String
        Public Property Year As String

        Private Sub New()
        End Sub

        Public Sub New(ByVal artist As String)
            Me.Artist = artist
            Me.Genre = "all"
            Me.Country = "all"
            Me.Year = "all"
        End Sub

        Public Sub New(Optional ByVal genre As String = "all",
                       Optional ByVal country As String = "all",
                       Optional ByVal year As String = "all")

            Me.Artist = ""
            Me.Genre = genre
            Me.Country = country
            Me.Year = year
        End Sub

    End Class

End Namespace