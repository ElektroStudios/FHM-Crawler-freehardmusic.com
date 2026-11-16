#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel

#End Region

#Region " ThemePalette "

Namespace FHMCrawler.UI.Theming

    ''' <summary>
    ''' Defines the colors of an <see cref="AppTheme"/>.
    ''' </summary>
    <ImmutableObject(True)>
    Public NotInheritable Class ThemePalette

#Region " Properties "

        ''' <summary>
        ''' Gets the theme that this palette represents.
        ''' </summary>
        Public ReadOnly Property Theme As AppTheme

        ''' <summary>
        ''' Gets the background color of the window.
        ''' </summary>
        Public ReadOnly Property WindowBackColor As Color

        ''' <summary>
        ''' Gets the background color of the surfaces placed on the window (header, cards and status bar).
        ''' </summary>
        Public ReadOnly Property SurfaceBackColor As Color

        ''' <summary>
        ''' Gets the background color of the input controls (text boxes, combo boxes and lists).
        ''' </summary>
        Public ReadOnly Property InputBackColor As Color

        ''' <summary>
        ''' Gets the color of borders and separators.
        ''' </summary>
        Public ReadOnly Property BorderColor As Color

        ''' <summary>
        ''' Gets the color of the primary text.
        ''' </summary>
        Public ReadOnly Property TextColor As Color

        ''' <summary>
        ''' Gets the color of the secondary text (captions, hints and status messages).
        ''' </summary>
        Public ReadOnly Property SecondaryTextColor As Color

        ''' <summary>
        ''' Gets the color of the text of disabled controls.
        ''' </summary>
        Public ReadOnly Property DisabledTextColor As Color

        ''' <summary>
        ''' Gets the accent color (primary buttons, checked items, switches, focus borders and progress).
        ''' </summary>
        Public ReadOnly Property AccentColor As Color

        ''' <summary>
        ''' Gets the accent color when the mouse is over an accent-colored control.
        ''' </summary>
        Public ReadOnly Property AccentHoverColor As Color

        ''' <summary>
        ''' Gets the accent color while an accent-colored control is pressed.
        ''' </summary>
        Public ReadOnly Property AccentPressedColor As Color

        ''' <summary>
        ''' Gets the color of the text drawn over the <see cref="ThemePalette.AccentColor"/>.
        ''' </summary>
        Public ReadOnly Property AccentTextColor As Color

        ''' <summary>
        ''' Gets the background color of buttons.
        ''' </summary>
        Public ReadOnly Property ButtonBackColor As Color

        ''' <summary>
        ''' Gets the background color of buttons when the mouse is over them.
        ''' </summary>
        Public ReadOnly Property ButtonHoverBackColor As Color

        ''' <summary>
        ''' Gets the background color of buttons while they are pressed.
        ''' </summary>
        Public ReadOnly Property ButtonPressedBackColor As Color

        ''' <summary>
        ''' Gets the background color of the list column headers.
        ''' </summary>
        Public ReadOnly Property HeaderBackColor As Color

        ''' <summary>
        ''' Gets the background color of the selected list items.
        ''' </summary>
        Public ReadOnly Property SelectionBackColor As Color

        ''' <summary>
        ''' Gets the color of the text of the selected list items.
        ''' </summary>
        Public ReadOnly Property SelectionTextColor As Color

        ''' <summary>
        ''' Gets the color of the track of switches and progress bars.
        ''' </summary>
        Public ReadOnly Property TrackColor As Color

        ''' <summary>
        ''' Gets the color that indicates that an operation finished with errors.
        ''' </summary>
        Public ReadOnly Property ErrorColor As Color

        ''' <summary>
        ''' Gets the color that indicates that an operation finished with warnings (for example, when it was cancelled).
        ''' </summary>
        Public ReadOnly Property WarningColor As Color

#End Region

#Region " Shared Properties "

        ''' <summary>
        ''' Gets the palette of the <see cref="AppTheme.Light"/> theme,
        ''' which uses the current Windows system colors and the Windows 10 button colors.
        ''' </summary>
        Public Shared ReadOnly Property Light As ThemePalette
            Get
                ' Built on every call, because the Windows system colors can change at runtime.
                Return New ThemePalette(theme:=AppTheme.Light,
                                        windowBackColor:=SystemColors.Control,
                                        surfaceBackColor:=SystemColors.Window,
                                        inputBackColor:=SystemColors.Window,
                                        borderColor:=SystemColors.ControlDark,
                                        textColor:=SystemColors.ControlText,
                                        secondaryTextColor:=SystemColors.GrayText,
                                        disabledTextColor:=SystemColors.GrayText,
                                        accentColor:=SystemColors.Highlight,
                                        accentHoverColor:=SystemColors.HotTrack,
                                        accentPressedColor:=ControlPaint.Dark(SystemColors.Highlight, 0.1F),
                                        accentTextColor:=SystemColors.HighlightText,
                                        buttonBackColor:=Color.FromArgb(225, 225, 225),
                                        buttonHoverBackColor:=Color.FromArgb(229, 241, 251),
                                        buttonPressedBackColor:=Color.FromArgb(204, 228, 247),
                                        headerBackColor:=SystemColors.Window,
                                        selectionBackColor:=SystemColors.Highlight,
                                        selectionTextColor:=SystemColors.HighlightText,
                                        trackColor:=SystemColors.ControlDark,
                                        errorColor:=Color.FromArgb(232, 17, 35),
                                        warningColor:=Color.FromArgb(255, 185, 0))
            End Get
        End Property

        ''' <summary>
        ''' Gets the palette of the <see cref="AppTheme.Dark"/> theme: graphite surfaces and a wine red accent.
        ''' </summary>
        Public Shared ReadOnly Property Dark As ThemePalette =
            New ThemePalette(theme:=AppTheme.Dark,
                             windowBackColor:=Color.FromArgb(27, 27, 29),
                             surfaceBackColor:=Color.FromArgb(36, 36, 39),
                             inputBackColor:=Color.FromArgb(44, 44, 48),
                             borderColor:=Color.FromArgb(62, 62, 68),
                             textColor:=Color.FromArgb(236, 236, 236),
                             secondaryTextColor:=Color.FromArgb(166, 166, 173),
                             disabledTextColor:=Color.FromArgb(110, 110, 117),
                             accentColor:=Color.FromArgb(155, 35, 53),
                             accentHoverColor:=Color.FromArgb(176, 42, 64),
                             accentPressedColor:=Color.FromArgb(126, 28, 43),
                             accentTextColor:=Color.White,
                             buttonBackColor:=Color.FromArgb(50, 50, 55),
                             buttonHoverBackColor:=Color.FromArgb(62, 62, 68),
                             buttonPressedBackColor:=Color.FromArgb(74, 74, 81),
                             headerBackColor:=Color.FromArgb(42, 42, 46),
                             selectionBackColor:=Color.FromArgb(92, 30, 43),
                             selectionTextColor:=Color.White,
                             trackColor:=Color.FromArgb(74, 74, 81),
                             errorColor:=Color.FromArgb(229, 72, 77),
                             warningColor:=Color.FromArgb(224, 161, 0))

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="ThemePalette"/> class.
        ''' </summary>
        Private Sub New(theme As AppTheme,
                        windowBackColor As Color,
                        surfaceBackColor As Color,
                        inputBackColor As Color,
                        borderColor As Color,
                        textColor As Color,
                        secondaryTextColor As Color,
                        disabledTextColor As Color,
                        accentColor As Color,
                        accentHoverColor As Color,
                        accentPressedColor As Color,
                        accentTextColor As Color,
                        buttonBackColor As Color,
                        buttonHoverBackColor As Color,
                        buttonPressedBackColor As Color,
                        headerBackColor As Color,
                        selectionBackColor As Color,
                        selectionTextColor As Color,
                        trackColor As Color,
                        errorColor As Color,
                        warningColor As Color)

            Me.Theme = theme
            Me.WindowBackColor = windowBackColor
            Me.SurfaceBackColor = surfaceBackColor
            Me.InputBackColor = inputBackColor
            Me.BorderColor = borderColor
            Me.TextColor = textColor
            Me.SecondaryTextColor = secondaryTextColor
            Me.DisabledTextColor = disabledTextColor
            Me.AccentColor = accentColor
            Me.AccentHoverColor = accentHoverColor
            Me.AccentPressedColor = accentPressedColor
            Me.AccentTextColor = accentTextColor
            Me.ButtonBackColor = buttonBackColor
            Me.ButtonHoverBackColor = buttonHoverBackColor
            Me.ButtonPressedBackColor = buttonPressedBackColor
            Me.HeaderBackColor = headerBackColor
            Me.SelectionBackColor = selectionBackColor
            Me.SelectionTextColor = selectionTextColor
            Me.TrackColor = trackColor
            Me.ErrorColor = errorColor
            Me.WarningColor = warningColor
        End Sub

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Gets the palette of the specified theme.
        ''' </summary>
        ''' <param name="theme">
        ''' The theme.
        ''' </param>
        ''' <returns>
        ''' The palette of the theme.
        ''' </returns>
        ''' <exception cref="InvalidEnumArgumentException">
        ''' Thrown when <paramref name="theme"/> is not a valid <see cref="AppTheme"/> value.
        ''' </exception>
        Public Shared Function FromTheme(theme As AppTheme) As ThemePalette
            Select Case theme
                Case AppTheme.Light
                    Return ThemePalette.Light
                Case AppTheme.Dark
                    Return ThemePalette.Dark
                Case Else
                    Throw New InvalidEnumArgumentException(NameOf(theme), CInt(theme), GetType(AppTheme))
            End Select
        End Function

#End Region

    End Class

End Namespace

#End Region
