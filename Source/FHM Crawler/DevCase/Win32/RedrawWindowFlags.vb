' ***********************************************************************
' Author   : ElektroStudios
' Modified : 16-November-2026
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " RedrawWindowFlags "

' ReSharper disable once CheckNamespace

Namespace DevCase.Win32.Enums

    ''' <summary>
    ''' Specifies the redraw flags of the <see cref="NativeMethods.RedrawWindow"/> function.
    ''' </summary>
    '''
    ''' <remarks>
    ''' <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-redrawwindow"/>
    ''' </remarks>
    <Flags>
    Public Enum RedrawWindowFlags As UInteger

        ''' <summary>
        ''' Invalidates the update region of the window.
        ''' </summary>
        Invalidate = 1UI << 0

        ''' <summary>
        ''' Causes the window to receive a WM_ERASEBKGND message when the window is repainted.
        ''' </summary>
        [Erase] = 1UI << 2

        ''' <summary>
        ''' Includes child windows, if any, in the repainting operation.
        ''' </summary>
        AllChildren = 1UI << 7

        ''' <summary>
        ''' Causes the affected windows to receive WM_ERASEBKGND and WM_PAINT messages before the function returns.
        ''' </summary>
        UpdateNow = 1UI << 8

        ''' <summary>
        ''' Causes any part of the non-client area of the window that intersects the update region to receive a WM_NCPAINT message.
        ''' </summary>
        Frame = 1UI << 10

    End Enum

End Namespace

#End Region
