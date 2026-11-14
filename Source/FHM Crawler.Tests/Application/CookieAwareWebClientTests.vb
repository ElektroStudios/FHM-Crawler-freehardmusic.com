#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Net

Imports Microsoft.VisualStudio.TestTools.UnitTesting

#End Region

#Region " CookieAwareWebClientTests "

Namespace ApplicationTests

    ''' <summary>
    ''' Tests the <see cref="CookieAwareWebClient"/> class.
    ''' </summary>
    <TestClass>
    Public NotInheritable Class CookieAwareWebClientTests

#Region " Tests "

        ''' <summary>
        ''' A new client has an empty cookie container ready to use.
        ''' </summary>
        <TestMethod>
        Public Sub Constructor_CreatesAnEmptyCookieContainer()
            Using client As New CookieAwareWebClient()
                Assert.IsNotNull(client.Cookies)
                Assert.AreEqual(0, client.Cookies.Count)
            End Using
        End Sub

        ''' <summary>
        ''' The HTTP requests created by the client use its cookie container,
        ''' so the session cookies are sent to the website.
        ''' </summary>
        <TestMethod>
        Public Sub GetWebRequest_HttpAddress_AttachesTheCookieContainer()
            Dim cookies As New CookieContainer()
            cookies.Add(New Cookie("session", "abc123", "/", "freehardmusic.com"))

            Using client As New TestableCookieAwareWebClient() With {.Cookies = cookies}
                Dim request As WebRequest = client.CreateRequest(New Uri("http://freehardmusic.com/index.php"))

                Assert.IsInstanceOfType(Of HttpWebRequest)(request)
                Assert.AreSame(cookies, DirectCast(request, HttpWebRequest).CookieContainer)
            End Using
        End Sub

        ''' <summary>
        ''' Replacing the cookie container affects the requests created afterwards.
        ''' </summary>
        <TestMethod>
        Public Sub GetWebRequest_AfterReplacingCookies_UsesTheNewContainer()
            Using client As New TestableCookieAwareWebClient()
                Dim newCookies As New CookieContainer()
                client.Cookies = newCookies

                Dim request As HttpWebRequest = DirectCast(client.CreateRequest(New Uri("http://freehardmusic.com/")), HttpWebRequest)

                Assert.AreSame(newCookies, request.CookieContainer)
            End Using
        End Sub

        ''' <summary>
        ''' Non-HTTP requests (which have no cookie container) are created without errors.
        ''' </summary>
        <TestMethod>
        Public Sub GetWebRequest_FileAddress_IsCreatedWithoutCookies()
            Using client As New TestableCookieAwareWebClient()
                Dim request As WebRequest = client.CreateRequest(New Uri("file:///C:/Temp/file.txt"))

                Assert.IsNotInstanceOfType(Of HttpWebRequest)(request)
            End Using
        End Sub

#End Region

#Region " Test Doubles "

        ''' <summary>
        ''' A <see cref="CookieAwareWebClient"/> that exposes its protected <c>GetWebRequest</c> method.
        ''' </summary>
        Private NotInheritable Class TestableCookieAwareWebClient : Inherits CookieAwareWebClient

            ''' <summary>
            ''' Creates the <see cref="WebRequest"/> that the client would use for the specified address.
            ''' </summary>
            ''' <param name="address">
            ''' The address.
            ''' </param>
            ''' <returns>
            ''' The web request (no connection is opened).
            ''' </returns>
            Friend Function CreateRequest(address As Uri) As WebRequest
                Return Me.GetWebRequest(address)
            End Function

        End Class

#End Region

    End Class

End Namespace

#End Region
