#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Runtime.InteropServices

Imports Microsoft.Win32

Imports DevCase.Win32.Enums

#End Region

#Region " ThemeManager "

Namespace FHMCrawler.UI.Theming

    ''' <summary>
    ''' Applies the application themes to windows and controls,
    ''' and converts the themes to and from their user settings value.
    ''' </summary>
    Public NotInheritable Class ThemeManager

#Region " Constants "

        ''' <summary>
        ''' The registry key that contains the Windows personalization settings of the current user.
        ''' </summary>
        Friend Const PersonalizeRegistryKey As String = "Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"

        ''' <summary>
        ''' The registry value that indicates whether applications must use the light theme (1) or the dark theme (0).
        ''' </summary>
        Friend Const AppsUseLightThemeRegistryValue As String = "AppsUseLightTheme"

        ''' <summary>
        ''' The <c>WM_NCACTIVATE</c> window message, sent to a window when its non-client area
        ''' (title bar) needs to be changed to indicate an active or inactive state.
        ''' </summary>
        Private Const WM_NCACTIVATE As Integer = &H86

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Prevents a default instance of the <see cref="ThemeManager"/> class from being created.
        ''' </summary>
        Private Sub New()
        End Sub

#End Region

#Region " Settings "

        ''' <summary>
        ''' Converts a theme user setting value into an <see cref="AppTheme"/>.
        ''' </summary>
        ''' <param name="value">
        ''' The setting value: <c>"Light"</c>, <c>"Dark"</c> (case-insensitive),
        ''' or an empty string when the user has not chosen a theme yet.
        ''' </param>
        ''' <param name="defaultTheme">
        ''' The theme to return when <paramref name="value"/> is empty.
        ''' </param>
        ''' <returns>
        ''' The theme.
        ''' </returns>
        ''' <exception cref="FormatException">
        ''' Thrown when <paramref name="value"/> is not a valid theme name.
        ''' </exception>
        Public Shared Function ParseThemeSetting(value As String, defaultTheme As AppTheme) As AppTheme
            If String.IsNullOrWhiteSpace(value) Then
                Return defaultTheme
            End If

            Dim trimmedValue As String = value.Trim()
            For Each theme As AppTheme In [Enum].GetValues(GetType(AppTheme))
                If trimmedValue.Equals(theme.ToString(), StringComparison.OrdinalIgnoreCase) Then
                    Return theme
                End If
            Next theme

            Throw New FormatException($"The theme setting value '{value}' is not valid. Valid values are: {String.Join(", ", [Enum].GetNames(GetType(AppTheme)))}.")
        End Function

        ''' <summary>
        ''' Converts an <see cref="AppTheme"/> into its user setting value.
        ''' </summary>
        ''' <param name="theme">
        ''' The theme.
        ''' </param>
        ''' <returns>
        ''' The setting value (the theme name).
        ''' </returns>
        ''' <exception cref="InvalidEnumArgumentException">
        ''' Thrown when <paramref name="theme"/> is not a valid <see cref="AppTheme"/> value.
        ''' </exception>
        Public Shared Function ToSettingValue(theme As AppTheme) As String
            If Not [Enum].IsDefined(GetType(AppTheme), theme) Then
                Throw New InvalidEnumArgumentException(NameOf(theme), CInt(theme), GetType(AppTheme))
            End If
            Return theme.ToString()
        End Function

        ''' <summary>
        ''' Gets the application theme chosen by the current user in the Windows personalization settings.
        ''' </summary>
        ''' <returns>
        ''' <see cref="AppTheme.Dark"/> if Windows is set to use the dark app mode;
        ''' otherwise, <see cref="AppTheme.Light"/> (also on Windows versions without app modes).
        ''' </returns>
        Public Shared Function GetWindowsAppTheme() As AppTheme
            Using key As RegistryKey = Registry.CurrentUser.OpenSubKey(ThemeManager.PersonalizeRegistryKey, writable:=False)
                ' The key and the value do not exist on Windows versions without app modes (prior to Windows 10).
                Dim value As Object = If(key Is Nothing, Nothing, key.GetValue(ThemeManager.AppsUseLightThemeRegistryValue))
                Return ThemeManager.ParseAppsUseLightThemeValue(value)
            End Using
        End Function

        ''' <summary>
        ''' Converts the <c>AppsUseLightTheme</c> Windows registry value into an <see cref="AppTheme"/>.
        ''' </summary>
        ''' <param name="value">
        ''' The registry value, or <see langword="Nothing"/> when it does not exist.
        ''' </param>
        ''' <returns>
        ''' <see cref="AppTheme.Dark"/> if the value is zero; otherwise, <see cref="AppTheme.Light"/>.
        ''' </returns>
        ''' <exception cref="InvalidOperationException">
        ''' Thrown when <paramref name="value"/> is not a DWORD (<see cref="Integer"/>) value.
        ''' </exception>
        Friend Shared Function ParseAppsUseLightThemeValue(value As Object) As AppTheme
            If value Is Nothing Then
                Return AppTheme.Light
            End If

            If TypeOf value IsNot Integer Then
                Throw New InvalidOperationException($"The '{ThemeManager.AppsUseLightThemeRegistryValue}' registry value has an unexpected type: {value.GetType().FullName}.")
            End If

            Return If(DirectCast(value, Integer) = 0, AppTheme.Dark, AppTheme.Light)
        End Function

#End Region

#Region " Window Title Bar "

        ''' <summary>
        ''' Enables or disables the dark mode of the title bar of the specified window,
        ''' by calling the <c>DwmSetWindowAttribute</c> function.
        ''' <para></para>
        ''' This method must be called once the window handle has been created
        ''' (from <see cref="Control.OnHandleCreated"/>, and later whenever the theme changes);
        ''' calling it from the <see cref="Form.Load"/> event has no effect.
        ''' </summary>
        ''' <param name="window">
        ''' The window.
        ''' </param>
        ''' <param name="enabled">
        ''' <see langword="True"/> to draw the title bar in dark colors; <see langword="False"/> to draw it in light colors.
        ''' </param>
        ''' <returns>
        ''' <see langword="True"/> if the attribute was set;
        ''' <see langword="False"/> if the current Windows version does not support dark title bars (prior to Windows 10 1809).
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when <paramref name="window"/> is <see langword="Nothing"/>.
        ''' </exception>
        ''' <exception cref="InvalidOperationException">
        ''' Thrown when the window handle has not been created yet.
        ''' </exception>
        Public Shared Function SetTitleBarDarkMode(window As Form, enabled As Boolean) As Boolean
            If window Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(window))
            End If
            If Not window.IsHandleCreated Then
                Throw New InvalidOperationException("The window handle has not been created yet.")
            End If

            Dim value As Integer = If(enabled, 1, 0)
            Dim valueSize As Integer = Marshal.SizeOf(GetType(Integer))

            ' DWMWA_USE_IMMERSIVE_DARK_MODE (Windows 10 version 2004 and later).
            Dim hResult As Integer = NativeMethods.DwmSetWindowAttribute(window.Handle, DwmWindowAttribute.UseImmersiveDarkMode, value, valueSize)
            If hResult <> 0 Then
                ' DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 (Windows 10 versions 1809 to 1909).
                hResult = NativeMethods.DwmSetWindowAttribute(window.Handle, DwmWindowAttribute.UseImmersiveDarkModeBefore20H1, value, valueSize)
            End If
            If hResult <> 0 Then
                Return False
            End If

            If window.Visible Then
                ThemeManager.RefreshNonClientArea(window)
            End If

            Return True
        End Function

        ''' <summary>
        ''' Forces the non-client area (title bar and borders) of a visible window to be redrawn.
        ''' <para></para>
        ''' Windows 11 redraws the title bar as soon as the DWM attribute changes, but Windows 10 does not.
        ''' </summary>
        ''' <param name="window">
        ''' The window.
        ''' </param>
        Private Shared Sub RefreshNonClientArea(window As Form)
            NativeMethods.SetWindowPos(window.Handle, IntPtr.Zero, 0, 0, 0, 0,
                                       SetWindowPosFlags.NoMove Or SetWindowPosFlags.NoSize Or
                                       SetWindowPosFlags.NoZOrder Or SetWindowPosFlags.NoActivate Or
                                       SetWindowPosFlags.FrameChanged)

            ' Toggling the caption activation state repaints the title bar with the new colors.
            Dim isActive As Boolean = (Form.ActiveForm Is window)
            NativeMethods.SendMessage(window.Handle, ThemeManager.WM_NCACTIVATE, New IntPtr(If(isActive, 0, 1)), IntPtr.Zero)
            NativeMethods.SendMessage(window.Handle, ThemeManager.WM_NCACTIVATE, New IntPtr(If(isActive, 1, 0)), IntPtr.Zero)
        End Sub

#End Region

#Region " Controls "

        ''' <summary>
        ''' Applies the specified palette to a control and to all of its child controls.
        ''' <para></para>
        ''' Controls that implement <see cref="IThemeable"/> theme themselves;
        ''' standard buttons, text boxes and combo boxes receive the palette colors;
        ''' any other control (labels, panels...) inherits the colors of its parent (ambient properties).
        ''' </summary>
        ''' <param name="root">
        ''' The root control.
        ''' </param>
        ''' <param name="palette">
        ''' The palette to apply.
        ''' </param>
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when <paramref name="root"/> or <paramref name="palette"/> is <see langword="Nothing"/>.
        ''' </exception>
        Public Shared Sub ApplyToControls(root As Control, palette As ThemePalette)
            If root Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(root))
            End If
            If palette Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(palette))
            End If

            ' Iterative depth-first traversal of the control tree.
            Dim pending As New Stack(Of Control)
            pending.Push(root)

            While pending.Count > 0
                Dim ctrl As Control = pending.Pop()
                ThemeManager.ApplyToControl(ctrl, palette)

                For Each child As Control In ctrl.Controls
                    pending.Push(child)
                Next child
            End While
        End Sub

        ''' <summary>
        ''' Applies the specified palette to a single control (not to its child controls).
        ''' </summary>
        ''' <param name="ctrl">
        ''' The control.
        ''' </param>
        ''' <param name="palette">
        ''' The palette to apply.
        ''' </param>
        Private Shared Sub ApplyToControl(ctrl As Control, palette As ThemePalette)
            Dim themeable As IThemeable = TryCast(ctrl, IThemeable)
            If themeable IsNot Nothing Then
                themeable.ApplyTheme(palette)
                Exit Sub
            End If

            Dim btn As Button = TryCast(ctrl, Button)
            If btn IsNot Nothing Then
                btn.FlatStyle = FlatStyle.Flat
                btn.UseVisualStyleBackColor = False
                btn.BackColor = palette.ButtonBackColor
                btn.ForeColor = palette.TextColor
                btn.FlatAppearance.BorderColor = palette.BorderColor
                btn.FlatAppearance.MouseOverBackColor = palette.ButtonHoverBackColor
                btn.FlatAppearance.MouseDownBackColor = palette.ButtonPressedBackColor
                Exit Sub
            End If

            Dim txt As TextBoxBase = TryCast(ctrl, TextBoxBase)
            If txt IsNot Nothing Then
                txt.BackColor = palette.InputBackColor
                txt.ForeColor = palette.TextColor
                Exit Sub
            End If

            Dim cb As ComboBox = TryCast(ctrl, ComboBox)
            If cb IsNot Nothing Then
                cb.BackColor = palette.InputBackColor
                cb.ForeColor = palette.TextColor
            End If
        End Sub

#End Region

    End Class

End Namespace

#End Region
