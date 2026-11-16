#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " AppTheme "

Namespace FHMCrawler.UI.Theming

    ''' <summary>
    ''' Specifies the visual theme of the application.
    ''' </summary>
    Public Enum AppTheme As Integer

        ''' <summary>
        ''' Light theme, which uses the default Windows colors and control styles.
        ''' </summary>
        Light = 0

        ''' <summary>
        ''' Dark theme, which uses custom dark colors, flat control styles and a dark window title bar.
        ''' </summary>
        Dark = 1

    End Enum

End Namespace

#End Region
