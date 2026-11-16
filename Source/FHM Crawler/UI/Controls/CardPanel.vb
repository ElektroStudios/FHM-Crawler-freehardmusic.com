#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel

Imports FHMCrawler.UI.Theming

#End Region

#Region " CardPanel "

Namespace FHMCrawler.UI.Controls

    ''' <summary>
    ''' A panel that draws a flat surface with a 1-pixel border on the specified sides,
    ''' using the colors of the current <see cref="ThemePalette"/>.
    ''' <para></para>
    ''' It is used for the header, the cards and the status bar of the application.
    ''' </summary>
    <DesignerCategory("Code")>
    Public Class CardPanel : Inherits Panel : Implements IThemeable

#Region " Private Fields "

        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The sides that draw a border.
        ''' </summary>
        Private borderSidesB As BorderSides = BorderSides.All

        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The border color.
        ''' </summary>
        Private borderColorB As Color = SystemColors.ControlDark

        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' A value indicating whether the panel uses the surface color (<see langword="True"/>)
        ''' or the window color (<see langword="False"/>) as background.
        ''' </summary>
        Private useSurfaceColorB As Boolean = True

#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets or sets the sides that draw a border.
        ''' </summary>
        <Category("Appearance")>
        <DefaultValue(GetType(BorderSides), "All")>
        Public Property BorderSides As BorderSides
            Get
                Return Me.borderSidesB
            End Get
            Set(value As BorderSides)
                Me.borderSidesB = value
                Me.Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the border color.
        ''' </summary>
        <Category("Appearance")>
        Public Property BorderColor As Color
            Get
                Return Me.borderColorB
            End Get
            Set(value As Color)
                Me.borderColorB = value
                Me.Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a value indicating whether the panel uses the surface color of the palette
        ''' (<see langword="True"/>) or the window color (<see langword="False"/>) as background
        ''' when a theme is applied.
        ''' </summary>
        <Category("Appearance")>
        <DefaultValue(True)>
        Public Property UseSurfaceColor As Boolean
            Get
                Return Me.useSurfaceColorB
            End Get
            Set(value As Boolean)
                Me.useSurfaceColorB = value
            End Set
        End Property

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="CardPanel"/> class.
        ''' </summary>
        Public Sub New()
            Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                        ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
        End Sub

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Applies the specified palette to the panel.
        ''' </summary>
        ''' <param name="palette">
        ''' The palette to apply.
        ''' </param>
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when <paramref name="palette"/> is <see langword="Nothing"/>.
        ''' </exception>
        Public Sub ApplyTheme(palette As ThemePalette) Implements IThemeable.ApplyTheme
            If palette Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(palette))
            End If

            Me.BackColor = If(Me.useSurfaceColorB, palette.SurfaceBackColor, palette.WindowBackColor)
            Me.ForeColor = palette.TextColor
            Me.BorderColor = palette.BorderColor
        End Sub

#End Region

#Region " Event Invocators "

        ''' <summary>
        ''' Raises the <see cref="Control.Paint"/> event and draws the borders.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)

            Dim bounds As Rectangle = Me.ClientRectangle
            If bounds.Width <= 0 OrElse bounds.Height <= 0 Then
                Exit Sub
            End If

            Using pen As New Pen(Me.borderColorB)
                If (Me.borderSidesB And BorderSides.Left) <> 0 Then
                    e.Graphics.DrawLine(pen, bounds.Left, bounds.Top, bounds.Left, bounds.Bottom - 1)
                End If
                If (Me.borderSidesB And BorderSides.Top) <> 0 Then
                    e.Graphics.DrawLine(pen, bounds.Left, bounds.Top, bounds.Right - 1, bounds.Top)
                End If
                If (Me.borderSidesB And BorderSides.Right) <> 0 Then
                    e.Graphics.DrawLine(pen, bounds.Right - 1, bounds.Top, bounds.Right - 1, bounds.Bottom - 1)
                End If
                If (Me.borderSidesB And BorderSides.Bottom) <> 0 Then
                    e.Graphics.DrawLine(pen, bounds.Left, bounds.Bottom - 1, bounds.Right - 1, bounds.Bottom - 1)
                End If
            End Using
        End Sub

#End Region

    End Class

End Namespace

#End Region
