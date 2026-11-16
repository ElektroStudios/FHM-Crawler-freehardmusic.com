#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " BorderSides "

Namespace FHMCrawler.UI.Controls

    ''' <summary>
    ''' Specifies the sides of a control that draw a border.
    ''' </summary>
    <Flags>
    Public Enum BorderSides As Integer

        ''' <summary>
        ''' No border.
        ''' </summary>
        None = 0

        ''' <summary>
        ''' Left border.
        ''' </summary>
        Left = 1 << 0

        ''' <summary>
        ''' Top border.
        ''' </summary>
        Top = 1 << 1

        ''' <summary>
        ''' Right border.
        ''' </summary>
        Right = 1 << 2

        ''' <summary>
        ''' Bottom border.
        ''' </summary>
        Bottom = 1 << 3

        ''' <summary>
        ''' Border on every side.
        ''' </summary>
        All = BorderSides.Left Or BorderSides.Top Or BorderSides.Right Or BorderSides.Bottom

    End Enum

End Namespace

#End Region
