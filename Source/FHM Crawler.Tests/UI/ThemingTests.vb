#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Drawing

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports FHMCrawler.UI.Controls
Imports FHMCrawler.UI.Theming

#End Region

#Region " ThemingTests "

Namespace UITests

    ''' <summary>
    ''' Tests the <see cref="ThemePalette"/> and <see cref="ThemeManager"/> classes.
    ''' </summary>
    <TestClass>
    Public NotInheritable Class ThemingTests

#Region " ThemePalette "

        ''' <summary>
        ''' Each theme has its own palette.
        ''' </summary>
        <TestMethod>
        Public Sub FromTheme_ReturnsThePaletteOfTheTheme()
            Assert.AreEqual(AppTheme.Light, ThemePalette.FromTheme(AppTheme.Light).Theme)
            Assert.AreEqual(AppTheme.Dark, ThemePalette.FromTheme(AppTheme.Dark).Theme)
        End Sub

        ''' <summary>
        ''' An undefined theme is rejected.
        ''' </summary>
        <TestMethod>
        Public Sub FromTheme_UndefinedTheme_Throws()
            Assert.ThrowsExactly(Of InvalidEnumArgumentException)(Function() ThemePalette.FromTheme(CType(99, AppTheme)))
        End Sub

        ''' <summary>
        ''' The light theme uses the current Windows system colors.
        ''' </summary>
        <TestMethod>
        Public Sub LightPalette_UsesTheWindowsSystemColors()
            Dim palette As ThemePalette = ThemePalette.Light

            Assert.AreEqual(SystemColors.Control, palette.WindowBackColor)
            Assert.AreEqual(SystemColors.Window, palette.SurfaceBackColor)
            Assert.AreEqual(SystemColors.Window, palette.InputBackColor)
            Assert.AreEqual(SystemColors.ControlText, palette.TextColor)
            Assert.AreEqual(SystemColors.Highlight, palette.AccentColor)
            Assert.AreEqual(SystemColors.Highlight, palette.SelectionBackColor)
            Assert.AreEqual(SystemColors.HighlightText, palette.SelectionTextColor)
        End Sub

        ''' <summary>
        ''' The dark theme uses dark graphite surfaces (not pure black) and a wine red accent.
        ''' </summary>
        <TestMethod>
        Public Sub DarkPalette_IsGraphiteWithWineRedAccent()
            Dim palette As ThemePalette = ThemePalette.Dark

            For Each surface As Color In {palette.WindowBackColor, palette.SurfaceBackColor, palette.InputBackColor}
                Assert.IsTrue(surface.GetBrightness() < 0.2F, $"Not a dark color: {surface}.")
                Assert.AreNotEqual(Color.Black.ToArgb(), surface.ToArgb(), "Pure black is not used.")
            Next surface

            Dim accent As Color = palette.AccentColor
            Assert.IsTrue(accent.R > accent.G * 3 AndAlso accent.R > accent.B * 2, $"The accent is not a wine red: {accent}.")
            Assert.IsTrue(accent.GetBrightness() < 0.45F, $"The accent is too bright for a wine red: {accent}.")
        End Sub

        ''' <summary>
        ''' The texts of the dark palette have enough contrast against their backgrounds (WCAG AA, 4.5:1).
        ''' <para></para>
        ''' The light palette is not tested: it uses the Windows system colors, whose contrast depends on the system.
        ''' </summary>
        <TestMethod>
        Public Sub DarkPalette_TextsHaveEnoughContrast()
            Dim palette As ThemePalette = ThemePalette.Dark

            ThemingTests.AssertContrast(palette.TextColor, palette.WindowBackColor, 4.5)
            ThemingTests.AssertContrast(palette.TextColor, palette.SurfaceBackColor, 4.5)
            ThemingTests.AssertContrast(palette.TextColor, palette.InputBackColor, 4.5)
            ThemingTests.AssertContrast(palette.SelectionTextColor, palette.SelectionBackColor, 4.5)
            ThemingTests.AssertContrast(palette.AccentTextColor, palette.AccentColor, 4.5)
            ThemingTests.AssertContrast(palette.SecondaryTextColor, palette.SurfaceBackColor, 3.0)
        End Sub

#End Region

#Region " Settings "

        ''' <summary>
        ''' An empty setting (no theme chosen yet) returns the default theme.
        ''' </summary>
        <TestMethod>
        <DataRow(Nothing)>
        <DataRow("")>
        <DataRow("   ")>
        Public Sub ParseThemeSetting_Empty_ReturnsTheDefaultTheme(value As String)
            Assert.AreEqual(AppTheme.Dark, ThemeManager.ParseThemeSetting(value, AppTheme.Dark))
            Assert.AreEqual(AppTheme.Light, ThemeManager.ParseThemeSetting(value, AppTheme.Light))
        End Sub

        ''' <summary>
        ''' The theme names are parsed case-insensitively, ignoring surrounding white-spaces.
        ''' </summary>
        <TestMethod>
        <DataRow("Dark", AppTheme.Dark)>
        <DataRow("dark", AppTheme.Dark)>
        <DataRow(" LIGHT ", AppTheme.Light)>
        Public Sub ParseThemeSetting_ThemeName_ReturnsTheTheme(value As String, expected As AppTheme)
            Assert.AreEqual(expected, ThemeManager.ParseThemeSetting(value, AppTheme.Light))
        End Sub

        ''' <summary>
        ''' Invalid values (including numbers) are rejected instead of being silently ignored.
        ''' </summary>
        <TestMethod>
        <DataRow("Blue")>
        <DataRow("1")>
        <DataRow("Dark mode")>
        Public Sub ParseThemeSetting_InvalidValue_Throws(value As String)
            Assert.ThrowsExactly(Of FormatException)(Function() ThemeManager.ParseThemeSetting(value, AppTheme.Light))
        End Sub

        ''' <summary>
        ''' A theme is saved as its name, and the saved value is parsed back to the same theme.
        ''' </summary>
        <TestMethod>
        <DataRow(AppTheme.Light)>
        <DataRow(AppTheme.Dark)>
        Public Sub ToSettingValue_RoundTrips(theme As AppTheme)
            Dim value As String = ThemeManager.ToSettingValue(theme)

            Assert.AreEqual(theme.ToString(), value)
            Assert.AreEqual(theme, ThemeManager.ParseThemeSetting(value, If(theme = AppTheme.Dark, AppTheme.Light, AppTheme.Dark)))
        End Sub

        ''' <summary>
        ''' An undefined theme cannot be saved.
        ''' </summary>
        <TestMethod>
        Public Sub ToSettingValue_UndefinedTheme_Throws()
            Assert.ThrowsExactly(Of InvalidEnumArgumentException)(Function() ThemeManager.ToSettingValue(CType(5, AppTheme)))
        End Sub

        ''' <summary>
        ''' The Windows "AppsUseLightTheme" registry value: 0 is the dark theme; 1 or a missing value is the light theme.
        ''' </summary>
        <TestMethod>
        Public Sub ParseAppsUseLightThemeValue_ReturnsTheWindowsTheme()
            Assert.AreEqual(AppTheme.Dark, ThemeManager.ParseAppsUseLightThemeValue(0))
            Assert.AreEqual(AppTheme.Light, ThemeManager.ParseAppsUseLightThemeValue(1))
            Assert.AreEqual(AppTheme.Light, ThemeManager.ParseAppsUseLightThemeValue(Nothing))
        End Sub

        ''' <summary>
        ''' A registry value of an unexpected type is rejected.
        ''' </summary>
        <TestMethod>
        Public Sub ParseAppsUseLightThemeValue_UnexpectedType_Throws()
            Assert.ThrowsExactly(Of InvalidOperationException)(Function() ThemeManager.ParseAppsUseLightThemeValue("0"))
        End Sub

#End Region

#Region " Window Title Bar "

        ''' <summary>
        ''' The title bar cannot be changed without a window, nor before its handle is created.
        ''' </summary>
        <TestMethod>
        Public Sub SetTitleBarDarkMode_InvalidWindow_Throws()
            Assert.ThrowsExactly(Of ArgumentNullException)(Function() ThemeManager.SetTitleBarDarkMode(Nothing, True))

            Using window As New Form()
                Assert.ThrowsExactly(Of InvalidOperationException)(Function() ThemeManager.SetTitleBarDarkMode(window, True))
            End Using
        End Sub

#End Region

#Region " Controls "

        ''' <summary>
        ''' Null arguments are rejected.
        ''' </summary>
        <TestMethod>
        Public Sub ApplyToControls_NullArguments_Throw()
            Using ctrl As New Panel()
                Assert.ThrowsExactly(Of ArgumentNullException)(Sub() ThemeManager.ApplyToControls(Nothing, ThemePalette.Dark))
                Assert.ThrowsExactly(Of ArgumentNullException)(Sub() ThemeManager.ApplyToControls(ctrl, Nothing))
            End Using
        End Sub

        ''' <summary>
        ''' The palette is applied to every control of the tree, however deeply nested.
        ''' </summary>
        <TestMethod>
        Public Sub ApplyToControls_ThemesTheWholeControlTree()
            Dim palette As ThemePalette = ThemePalette.Dark

            Using root As New Panel()
                Dim current As Control = root
                For level As Integer = 1 To 50
                    Dim child As New Panel()
                    current.Controls.Add(child)
                    current = child
                Next level
                Dim deepButton As New Button()
                Dim deepText As New TextBox()
                Dim deepCombo As New ComboBox()
                Dim deepCard As New CardPanel()
                current.Controls.AddRange({deepButton, deepText, deepCombo, deepCard})

                ThemeManager.ApplyToControls(root, palette)

                Assert.AreEqual(FlatStyle.Flat, deepButton.FlatStyle)
                Assert.AreEqual(palette.ButtonBackColor, deepButton.BackColor)
                Assert.AreEqual(palette.BorderColor, deepButton.FlatAppearance.BorderColor)
                Assert.AreEqual(palette.InputBackColor, deepText.BackColor)
                Assert.AreEqual(palette.TextColor, deepText.ForeColor)
                Assert.AreEqual(palette.InputBackColor, deepCombo.BackColor)
                Assert.AreEqual(palette.SurfaceBackColor, deepCard.BackColor)
                Assert.AreEqual(palette.BorderColor, deepCard.BorderColor)
            End Using
        End Sub

        ''' <summary>
        ''' Switching from the dark theme to the light theme restores the light colors.
        ''' </summary>
        <TestMethod>
        Public Sub ApplyToControls_DarkThenLight_RestoresTheLightColors()
            Using root As New Panel(), btn As New ThemedButton(), txt As New ThemedTextBox()
                root.Controls.AddRange({btn, txt})

                ThemeManager.ApplyToControls(root, ThemePalette.Dark)
                ThemeManager.ApplyToControls(root, ThemePalette.Light)

                Assert.AreEqual(ThemePalette.Light.ButtonBackColor, btn.BackColor)
                Assert.AreEqual(ThemePalette.Light.InputBackColor, txt.TextBox.BackColor)
                Assert.AreEqual(ThemePalette.Light.TextColor, txt.TextBox.ForeColor)
            End Using
        End Sub

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Asserts that two colors have at least the specified WCAG contrast ratio.
        ''' </summary>
        ''' <param name="foreColor">
        ''' The text color.
        ''' </param>
        ''' <param name="backColor">
        ''' The background color.
        ''' </param>
        ''' <param name="minimumRatio">
        ''' The minimum contrast ratio.
        ''' </param>
        Private Shared Sub AssertContrast(foreColor As Color, backColor As Color, minimumRatio As Double)
            Dim l1 As Double = ThemingTests.GetRelativeLuminance(foreColor)
            Dim l2 As Double = ThemingTests.GetRelativeLuminance(backColor)
            Dim ratio As Double = (Math.Max(l1, l2) + 0.05) / (Math.Min(l1, l2) + 0.05)

            Assert.IsTrue(ratio >= minimumRatio, $"Contrast {ratio:0.00}:1 between {foreColor} and {backColor} is lower than {minimumRatio}:1.")
        End Sub

        ''' <summary>
        ''' Gets the WCAG relative luminance of a color.
        ''' </summary>
        ''' <param name="color">
        ''' The color.
        ''' </param>
        ''' <returns>
        ''' The relative luminance, from 0 (black) to 1 (white).
        ''' </returns>
        Private Shared Function GetRelativeLuminance(color As Color) As Double
            Dim channels As Double() = {color.R / 255.0, color.G / 255.0, color.B / 255.0}
            For index As Integer = 0 To channels.Length - 1
                Dim c As Double = channels(index)
                channels(index) = If(c <= 0.03928, c / 12.92, Math.Pow((c + 0.055) / 1.055, 2.4))
            Next index
            Return (0.2126 * channels(0)) + (0.7152 * channels(1)) + (0.0722 * channels(2))
        End Function

#End Region

    End Class

End Namespace

#End Region
