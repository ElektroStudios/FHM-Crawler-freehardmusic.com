Imports DevCase.Win32.Enums

Imports System.Net
Imports System.Text

Namespace DevCase.Core.Net

    Public NotInheritable Class InternetUtil

    Private Sub New()
    End Sub

        ''' <summary>
        ''' Retrieves the Internet Explorer cookies (stored in the operating system) associated with the provided <see cref="Uri"/>.
        ''' </summary>
        ''' 
        ''' <remarks>
        ''' Original C# code: <see href="https://stackoverflow.com/a/15052664/1248295"/>
        ''' </remarks>
        ''' 
        ''' <param name="uri">
        ''' The <see cref="Uri"/> for which to retrieve the cookies.
        ''' </param>
        ''' 
        ''' <returns>
        ''' A <see cref="CookieContainer"/> containing the cookies associated with the provided <see cref="Uri"/>, 
        ''' or <see langword="Nothing"/> if no cookies were found.
        ''' </returns>
        Public Shared Function GetCookieContainer(uri As Uri) As CookieContainer
            Dim cookies As CookieContainer = Nothing
            Dim datasize As Integer = 8192 * 16 ' Size of the buffer to receive cookie data.
            Dim cookieData As New StringBuilder(datasize)

            If Not NativeMethods.InternetGetCookieEx(uri.ToString(), Nothing, cookieData, datasize, InternetGetCookieExFlags.Httponly, IntPtr.Zero) Then
                If datasize < 0 Then
                    Return Nothing
                End If

                cookieData = New StringBuilder(datasize)
                If Not NativeMethods.InternetGetCookieEx(uri.ToString(), Nothing, cookieData, datasize, InternetGetCookieExFlags.Httponly, IntPtr.Zero) Then
                    Return Nothing
                End If
            End If

            If cookieData.Length > 0 Then
                cookies = New CookieContainer()
                cookies.SetCookies(uri, cookieData.ToString().Replace(";", ","))
            End If

            Return cookies
        End Function

    End Class

End Namespace