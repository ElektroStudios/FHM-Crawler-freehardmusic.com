#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel

Imports FHMCrawler.UI.Theming

#End Region

#Region " ThemedButton "

Namespace FHMCrawler.UI.Controls

    ''' <summary>
    ''' A flat button fully drawn with the colors of the current <see cref="ThemePalette"/>
    ''' (including its disabled state, which the native flat button draws with embossed text).
    ''' <para></para>
    ''' A primary button (<see cref="ThemedButton.IsPrimary"/>) is filled with the accent color.
    ''' </summary>
    <DesignerCategory("Code")>
    Public Class ThemedButton : Inherits Button : Implements IThemeable

#Region " Private Fields "

        ''' <summary>
        ''' The last applied palette.
        ''' </summary>
        Private palette As ThemePalette = ThemePalette.Light

        ''' <summary>
        ''' A value indicating whether the mouse is over the button.
        ''' </summary>
        Private isHovered As Boolean

        ''' <summary>
        ''' A value indicating whether the left mouse button is pressed over the button.
        ''' </summary>
        Private isPressed As Boolean

        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' A value indicating whether this is a primary button.
        ''' </summary>
        Private isPrimaryB As Boolean

#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets or sets a value indicating whether this is a primary button (filled with the accent color).
        ''' </summary>
        <Category("Appearance")>
        <DefaultValue(False)>
        Public Property IsPrimary As Boolean
            Get
                Return Me.isPrimaryB
            End Get
            Set(value As Boolean)
                Me.isPrimaryB = value
                Me.Invalidate()
            End Set
        End Property

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="ThemedButton"/> class.
        ''' </summary>
        Public Sub New()
            Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                        ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
            Me.FlatStyle = FlatStyle.Flat
            Me.UseVisualStyleBackColor = False
        End Sub

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Applies the specified palette to the button.
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

            Me.palette = palette
            Me.BackColor = palette.ButtonBackColor
            Me.ForeColor = palette.TextColor
            Me.Invalidate()
        End Sub

#End Region

#Region " Friend Methods "

        ''' <summary>
        ''' Gets the background, border and text colors of the button for its current state.
        ''' </summary>
        ''' <param name="refBackColor">
        ''' When this method returns, contains the background color.
        ''' </param>
        ''' <param name="refBorderColor">
        ''' When this method returns, contains the border color.
        ''' </param>
        ''' <param name="refTextColor">
        ''' When this method returns, contains the text color.
        ''' </param>
        Friend Sub GetStateColors(ByRef refBackColor As Color, ByRef refBorderColor As Color, ByRef refTextColor As Color)
            Dim p As ThemePalette = Me.palette

            If Not Me.Enabled Then
                refBackColor = p.ButtonBackColor
                refBorderColor = p.BorderColor
                refTextColor = p.DisabledTextColor

            ElseIf Me.isPrimaryB Then
                refBackColor = If(Me.isPressed, p.AccentPressedColor, If(Me.isHovered, p.AccentHoverColor, p.AccentColor))
                refBorderColor = refBackColor
                refTextColor = p.AccentTextColor

            Else
                refBackColor = If(Me.isPressed, p.ButtonPressedBackColor, If(Me.isHovered, p.ButtonHoverBackColor, p.ButtonBackColor))
                refBorderColor = If(Me.isHovered OrElse Me.isPressed, p.AccentColor, p.BorderColor)
                refTextColor = p.TextColor
            End If
        End Sub

#End Region

#Region " Event Invocators "

        ''' <summary>
        ''' Raises the <see cref="Control.MouseEnter"/> event.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnMouseEnter(e As EventArgs)
            MyBase.OnMouseEnter(e)
            Me.isHovered = True
            Me.Invalidate()
        End Sub

        ''' <summary>
        ''' Raises the <see cref="Control.MouseLeave"/> event.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnMouseLeave(e As EventArgs)
            MyBase.OnMouseLeave(e)
            Me.isHovered = False
            Me.isPressed = False
            Me.Invalidate()
        End Sub

        ''' <summary>
        ''' Raises the <see cref="Control.MouseDown"/> event.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            MyBase.OnMouseDown(e)
            If e.Button = MouseButtons.Left Then
                Me.isPressed = True
                Me.Invalidate()
            End If
        End Sub

        ''' <summary>
        ''' Raises the <see cref="Control.MouseUp"/> event.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
            MyBase.OnMouseUp(e)
            Me.isPressed = False
            Me.Invalidate()
        End Sub

        ''' <summary>
        ''' Raises the <see cref="Control.EnabledChanged"/> event and redraws the button.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnEnabledChanged(e As EventArgs)
            MyBase.OnEnabledChanged(e)
            If Not Me.Enabled Then
                Me.isHovered = False
                Me.isPressed = False
            End If
            Me.Invalidate()
        End Sub

        ''' <summary>
        ''' Draws the button.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            Dim backColor As Color
            Dim borderColor As Color
            Dim textColor As Color
            Me.GetStateColors(backColor, borderColor, textColor)

            Dim g As Graphics = e.Graphics
            Dim bounds As Rectangle = Me.ClientRectangle
            g.Clear(backColor)

            Using pen As New Pen(borderColor)
                g.DrawRectangle(pen, 0, 0, bounds.Width - 1, bounds.Height - 1)
            End Using

            Dim flags As TextFormatFlags = TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter Or
                                           TextFormatFlags.SingleLine Or TextFormatFlags.EndEllipsis
            If Not Me.ShowKeyboardCues Then
                flags = flags Or TextFormatFlags.HidePrefix
            End If
            TextRenderer.DrawText(g, Me.Text, Me.Font, bounds, textColor, flags)

            If Me.Focused AndAlso Me.ShowFocusCues Then
                ControlPaint.DrawFocusRectangle(g, Rectangle.Inflate(bounds, -3, -3), textColor, backColor)
            End If
        End Sub

#End Region

    End Class

End Namespace

#End Region
