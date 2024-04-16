Imports System.Net

Public Class CookieAwareWebClient
    Inherits WebClient

    Public Property Cookies() As New CookieContainer()

    Protected Overrides Function GetWebRequest(address As Uri) As WebRequest
        Dim request As WebRequest = MyBase.GetWebRequest(address)
        If TypeOf request Is HttpWebRequest Then
            Dim httpRequest As HttpWebRequest = DirectCast(request, HttpWebRequest)
            httpRequest.CookieContainer = Me.Cookies
        End If
        Return request
    End Function
End Class