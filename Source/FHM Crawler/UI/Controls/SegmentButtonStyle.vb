#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " SegmentButtonStyle "

Namespace FHMCrawler.UI.Controls

    ''' <summary>
    ''' Specifies how a <see cref="SegmentButton"/> indicates that it is checked.
    ''' </summary>
    Public Enum SegmentButtonStyle As Integer

        ''' <summary>
        ''' Navigation tab: the checked button shows an accent bar under its text.
        ''' </summary>
        Underline = 0

        ''' <summary>
        ''' Segmented control: the checked button is filled with the accent color.
        ''' </summary>
        Filled = 1

    End Enum

End Namespace

#End Region
