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

    ''' <summary>
    ''' Sets the value of a Desktop Window Manager (DWM) non-client rendering attribute for a window.
    ''' </summary>
    '''
    ''' <remarks>
    ''' <see href="https://learn.microsoft.com/en-us/windows/win32/api/dwmapi/nf-dwmapi-dwmsetwindowattribute"/>
    ''' </remarks>
    '''
    ''' <param name="hWnd">
    ''' The handle to the window for which the attribute value is to be set.
    ''' </param>
    '''
    ''' <param name="attribute">
    ''' The attribute to set.
    ''' </param>
    '''
    ''' <param name="refValue">
    ''' The attribute value to set (a BOOL value, for the attributes declared in <see cref="DwmWindowAttribute"/>).
    ''' </param>
    '''
    ''' <param name="valueSize">
    ''' The size, in bytes, of the attribute value.
    ''' </param>
    '''
    ''' <returns>
    ''' Returns <c>S_OK</c> (zero) if successful; otherwise, an HRESULT error code.
    ''' </returns>
    <DllImport("dwmapi.dll", PreserveSig:=True)>
    Public Shared Function DwmSetWindowAttribute(hWnd As IntPtr,
                                                 attribute As DwmWindowAttribute,
                                                 ByRef refValue As Integer,
                                                 valueSize As Integer
    ) As Integer
    End Function

    ''' <summary>
    ''' Causes a window to use a different set of visual style information than its class normally uses.
    ''' </summary>
    '''
    ''' <remarks>
    ''' <see href="https://learn.microsoft.com/en-us/windows/win32/api/uxtheme/nf-uxtheme-setwindowtheme"/>
    ''' </remarks>
    '''
    ''' <param name="hWnd">
    ''' The handle to the window whose visual style information is to be changed.
    ''' </param>
    '''
    ''' <param name="subAppName">
    ''' The application name to use in place of the calling application's name.
    ''' <para></para>
    ''' If this parameter and <paramref name="subIdList"/> are <see langword="Nothing"/>,
    ''' the previously applied visual style information is removed.
    ''' </param>
    '''
    ''' <param name="subIdList">
    ''' A semicolon-separated list of class identifier (CLSID) names to use in place of the actual list passed by the window's class.
    ''' </param>
    '''
    ''' <returns>
    ''' Returns <c>S_OK</c> (zero) if successful; otherwise, an HRESULT error code.
    ''' </returns>
    <DllImport("uxtheme.dll", CharSet:=CharSet.Unicode, ExactSpelling:=True)>
    Public Shared Function SetWindowTheme(hWnd As IntPtr,
                                          subAppName As String,
                                          subIdList As String
    ) As Integer
    End Function

    ''' <summary>
    ''' Sends the specified message to a window, with a string as the <c>lParam</c> parameter.
    ''' </summary>
    '''
    ''' <remarks>
    ''' <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendmessagew"/>
    ''' </remarks>
    '''
    ''' <param name="hWnd">
    ''' The handle to the window whose window procedure will receive the message.
    ''' </param>
    '''
    ''' <param name="msg">
    ''' The message to be sent.
    ''' </param>
    '''
    ''' <param name="wParam">
    ''' Additional message-specific information.
    ''' </param>
    '''
    ''' <param name="lParam">
    ''' Additional message-specific information (a string).
    ''' </param>
    '''
    ''' <returns>
    ''' The result of the message processing; it depends on the message sent.
    ''' </returns>
    <DllImport("user32.dll", EntryPoint:="SendMessageW", CharSet:=CharSet.Unicode, ExactSpelling:=True)>
    Public Shared Function SendMessage(hWnd As IntPtr,
                                       msg As Integer,
                                       wParam As IntPtr,
                                       lParam As String
    ) As IntPtr
    End Function

    ''' <summary>
    ''' Sends the specified message to a window.
    ''' </summary>
    '''
    ''' <remarks>
    ''' <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendmessagew"/>
    ''' </remarks>
    '''
    ''' <param name="hWnd">
    ''' The handle to the window whose window procedure will receive the message.
    ''' </param>
    '''
    ''' <param name="msg">
    ''' The message to be sent.
    ''' </param>
    '''
    ''' <param name="wParam">
    ''' Additional message-specific information.
    ''' </param>
    '''
    ''' <param name="lParam">
    ''' Additional message-specific information.
    ''' </param>
    '''
    ''' <returns>
    ''' The result of the message processing; it depends on the message sent.
    ''' </returns>
    <DllImport("user32.dll", EntryPoint:="SendMessageW", CharSet:=CharSet.Unicode, ExactSpelling:=True)>
    Public Shared Function SendMessage(hWnd As IntPtr,
                                       msg As Integer,
                                       wParam As IntPtr,
                                       lParam As IntPtr
    ) As IntPtr
    End Function

    ''' <summary>
    ''' Adds a rectangle to the update region of the specified window, so that it is redrawn.
    ''' </summary>
    '''
    ''' <remarks>
    ''' <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-invalidaterect"/>
    ''' </remarks>
    '''
    ''' <param name="hWnd">
    ''' A handle to the window whose update region has changed.
    ''' </param>
    '''
    ''' <param name="rect">
    ''' A pointer to the rectangle to add, or <see cref="IntPtr.Zero"/> to add the entire client area.
    ''' </param>
    '''
    ''' <param name="erase">
    ''' <see langword="True"/> to erase the background within the update region.
    ''' </param>
    '''
    ''' <returns>
    ''' Returns <see langword="True"/> if successful, or <see langword="False"/> otherwise.
    ''' </returns>
    <DllImport("user32.dll", ExactSpelling:=True)>
    Public Shared Function InvalidateRect(hWnd As IntPtr,
                                          rect As IntPtr,
                                          <MarshalAs(UnmanagedType.Bool)> [erase] As Boolean
    ) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    ''' <summary>
    ''' Updates the specified rectangle or region in the client area of the specified window.
    ''' </summary>
    '''
    ''' <remarks>
    ''' <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-redrawwindow"/>
    ''' </remarks>
    '''
    ''' <param name="hWnd">
    ''' A handle to the window to be redrawn.
    ''' </param>
    '''
    ''' <param name="updateRect">
    ''' A pointer to the update rectangle, or <see cref="IntPtr.Zero"/> to update the entire client area.
    ''' </param>
    '''
    ''' <param name="updateRegion">
    ''' A handle to the update region, or <see cref="IntPtr.Zero"/> to update the entire client area.
    ''' </param>
    '''
    ''' <param name="flags">
    ''' The redraw flags.
    ''' </param>
    '''
    ''' <returns>
    ''' Returns <see langword="True"/> if successful, or <see langword="False"/> otherwise.
    ''' </returns>
    <DllImport("user32.dll", ExactSpelling:=True)>
    Public Shared Function RedrawWindow(hWnd As IntPtr,
                                        updateRect As IntPtr,
                                        updateRegion As IntPtr,
                                        flags As RedrawWindowFlags
    ) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    ''' <summary>
    ''' Changes the size, position, and Z order of a child, pop-up, or top-level window.
    ''' </summary>
    '''
    ''' <remarks>
    ''' <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowpos"/>
    ''' </remarks>
    '''
    ''' <param name="hWnd">
    ''' A handle to the window.
    ''' </param>
    '''
    ''' <param name="hWndInsertAfter">
    ''' A handle to the window to precede the positioned window in the Z order.
    ''' </param>
    '''
    ''' <param name="x">
    ''' The new position of the left side of the window, in client coordinates.
    ''' </param>
    '''
    ''' <param name="y">
    ''' The new position of the top of the window, in client coordinates.
    ''' </param>
    '''
    ''' <param name="cx">
    ''' The new width of the window, in pixels.
    ''' </param>
    '''
    ''' <param name="cy">
    ''' The new height of the window, in pixels.
    ''' </param>
    '''
    ''' <param name="flags">
    ''' The window sizing and positioning flags.
    ''' </param>
    '''
    ''' <returns>
    ''' Returns <see langword="True"/> if successful, or <see langword="False"/> otherwise.
    ''' </returns>
    <DllImport("user32.dll", SetLastError:=True, ExactSpelling:=True)>
    Public Shared Function SetWindowPos(hWnd As IntPtr,
                                        hWndInsertAfter As IntPtr,
                                        x As Integer,
                                        y As Integer,
                                        cx As Integer,
                                        cy As Integer,
                                        flags As SetWindowPosFlags
    ) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    ''' <summary>
    ''' Retrieves information about the specified window, such as its window styles.
    ''' </summary>
    '''
    ''' <remarks>
    ''' <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowlongw"/>
    ''' <para></para>
    ''' The 32-bit version is valid in 64-bit processes for the window styles (<c>GWL_STYLE</c>), which are 32-bit values.
    ''' </remarks>
    '''
    ''' <param name="hWnd">
    ''' A handle to the window.
    ''' </param>
    '''
    ''' <param name="index">
    ''' The zero-based offset to the value to be retrieved (e.g. <c>GWL_STYLE</c>).
    ''' </param>
    '''
    ''' <returns>
    ''' The requested value, or zero if the function fails.
    ''' </returns>
    <DllImport("user32.dll", EntryPoint:="GetWindowLongW", ExactSpelling:=True, SetLastError:=True)>
    Public Shared Function GetWindowLong(hWnd As IntPtr,
                                         index As Integer
    ) As Integer
    End Function

End Class
