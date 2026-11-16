' ***********************************************************************
' Author   : ElektroStudios
' Modified : 16-November-2026
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " DwmWindowAttribute "

' ReSharper disable once CheckNamespace

Namespace DevCase.Win32.Enums

    ''' <summary>
    ''' Specifies a Desktop Window Manager (DWM) window attribute.
    ''' <para></para>
    ''' For <see cref="NativeMethods.DwmSetWindowAttribute"/> function.
    ''' </summary>
    '''
    ''' <remarks>
    ''' <see href="https://learn.microsoft.com/en-us/windows/win32/api/dwmapi/ne-dwmapi-dwmwindowattribute"/>
    ''' </remarks>
    Public Enum DwmWindowAttribute As Integer

        ''' <summary>
        ''' Undocumented value of <see cref="DwmWindowAttribute.UseImmersiveDarkMode"/>
        ''' used by Windows 10 builds older than 20H1 (from 1809 to 1909).
        ''' </summary>
        UseImmersiveDarkModeBefore20H1 = 19

        ''' <summary>
        ''' Allows the window frame (title bar) to be drawn in dark mode colors when the attribute value is <see langword="True"/>.
        ''' <para></para>
        ''' Supported starting with Windows 11 build 22000 and Windows 10 20H1.
        ''' </summary>
        UseImmersiveDarkMode = 20

    End Enum

End Namespace

#End Region
