Imports System.Runtime.InteropServices
Imports System.Text

Imports DevCase.Win32.Enums

Public Class NativeMethods

    ''' <summary>
    ''' Retrieves data stored in cookies associated with a specified URL. 
    ''' <para></para>
    ''' Unlike "NativeMethods.InternetGetCookie" function, 
    ''' <see cref="NativeMethods.InternetGetCookieEx"/> can be used to restrict data retrieved to a single cookie name or, 
    ''' by policy, associated with untrusted sites or third-party cookies.
    ''' </summary>
    '''
    ''' <remarks>
    ''' <see href="https://docs.microsoft.com/en-us/windows/desktop/api/wininet/nf-wininet-internetgetcookieexa"/>
    ''' </remarks>
    '''
    ''' <param name="urlName">
    ''' The URL for which cookies are to be retrieved.
    ''' </param>
    ''' 
    ''' <param name="cookieName">
    ''' Not implemented.
    ''' </param>
    ''' 
    ''' <param name="cookieData">
    ''' A buffer that receives the cookie data. This parameter can be NULL.
    ''' </param>
    ''' 
    ''' <param name="refCookieSize">
    ''' A variable that specifies the size of the <paramref name="cookieData"/> parameter buffer, in characters. 
    ''' <para></para>
    ''' If the function succeeds, the buffer receives the amount of data copied to the <paramref name="cookieData"/> buffer. 
    ''' <para></para>
    ''' If <paramref name="cookieData"/> is <see langword="Nothing"/>, 
    ''' this parameter receives a value that specifies the size of the buffer necessary to copy all the cookie data, 
    ''' expressed as a byte count.
    ''' </param>
    ''' 
    ''' <param name="flags">
    ''' A flag that controls how the function retrieves cookie data.
    ''' </param>
    ''' 
    ''' <param name="reserved">
    ''' Reserved for future use. Set to <see cref="IntPtr.Zero"/>.
    ''' </param>
    '''
    ''' <returns>
    ''' Returns <see langword="True"/> if successful, or <see langword="False"/> otherwise.
    ''' </returns>
    <DllImport("WinINet", SetLastError:=True, CharSet:=CharSet.Auto, BestFitMapping:=False, ThrowOnUnmappableChar:=True)>
    Public Shared Function InternetGetCookieEx(<[In]> urlName As String,
                                               <[In]> cookieName As String,
                                         <[Optional]> cookieData As StringBuilder,
                                    <[In], Out> ByRef refCookieSize As Integer,
                  <[In], MarshalAs(UnmanagedType.I4)> flags As InternetGetCookieExFlags,
                                               <[In]> reserved As IntPtr
    ) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

End Class
