#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " ProgressBarState "

Namespace FHMCrawler.UI.Controls

    ''' <summary>
    ''' Specifies the state of a <see cref="FlatProgressBar"/>, which determines the color of its bar.
    ''' <para></para>
    ''' The state must not change while the operation runs; only when it finishes.
    ''' </summary>
    Public Enum ProgressBarState As Integer

        ''' <summary>
        ''' The operation is running, or it finished successfully (accent color).
        ''' </summary>
        Normal = 0

        ''' <summary>
        ''' The operation finished with warnings, for example when it was cancelled (warning color).
        ''' </summary>
        Warning = 1

        ''' <summary>
        ''' The operation finished with errors (error color).
        ''' </summary>
        [Error] = 2

    End Enum

End Namespace

#End Region
