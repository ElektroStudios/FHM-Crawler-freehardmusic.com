' ***********************************************************************
' Author   : ElektroStudios
' Modified : 16-November-2026
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " SetWindowPosFlags "

' ReSharper disable once CheckNamespace

Namespace DevCase.Win32.Enums

    ''' <summary>
    ''' Specifies the window sizing and positioning flags.
    ''' <para></para>
    ''' For <see cref="NativeMethods.SetWindowPos"/> function.
    ''' </summary>
    '''
    ''' <remarks>
    ''' <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowpos"/>
    ''' </remarks>
    <Flags>
    Public Enum SetWindowPosFlags As UInteger

        ''' <summary>
        ''' Retains the current size (ignores the cx and cy parameters).
        ''' </summary>
        NoSize = 1UI << 0

        ''' <summary>
        ''' Retains the current position (ignores the X and Y parameters).
        ''' </summary>
        NoMove = 1UI << 1

        ''' <summary>
        ''' Retains the current Z order (ignores the hWndInsertAfter parameter).
        ''' </summary>
        NoZOrder = 1UI << 2

        ''' <summary>
        ''' Does not activate the window.
        ''' </summary>
        NoActivate = 1UI << 4

        ''' <summary>
        ''' Applies new frame styles and sends a WM_NCCALCSIZE message to the window,
        ''' forcing the non-client area (title bar and borders) to be recalculated and redrawn.
        ''' </summary>
        FrameChanged = 1UI << 5

    End Enum

End Namespace

#End Region
