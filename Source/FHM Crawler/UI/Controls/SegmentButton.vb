#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel

Imports FHMCrawler.UI.Theming

#End Region

#Region " SegmentButton "

Namespace FHMCrawler.UI.Controls

    ''' <summary>
    ''' A <see cref="RadioButton"/> drawn as a navigation tab or as a segment of a segmented control,
    ''' with the colors of the current <see cref="ThemePalette"/>.
    ''' <para></para>
    ''' The sibling segment buttons of the same container are mutually exclusive, as any radio button.
    ''' </summary>
    <DesignerCategory("Code")>
    <DefaultEvent(NameOf(RadioButton.CheckedChanged))>
    Public Class SegmentButton : Inherits RadioButton : Implements IThemeable

#Region " Constants "

        ''' <summary>
        ''' The height, in pixels, of the accent bar of the <see cref="SegmentButtonStyle.Underline"/> style.
        ''' </summary>
        Private Const UnderlineHeight As Integer = 3

#End Region

#Region " Private Fields "

        ''' <summary>
        ''' The last applied palette.
        ''' </summary>
        Private palette As ThemePalette = ThemePalette.Light

        ''' <summary>
        ''' A value indicating whether the mouse is over the control.
        ''' </summary>
        Private isHovered As Boolean

        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The style of the button.
        ''' </summary>
        Private segmentStyleB As SegmentButtonStyle = SegmentButtonStyle.Underline

#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets or sets how the button indicates that it is checked.
        ''' </summary>
        <Category("Appearance")>
        <DefaultValue(GetType(SegmentButtonStyle), "Underline")>
        Public Property SegmentStyle As SegmentButtonStyle
            Get
                Return Me.segmentStyleB
            End Get
            Set(value As SegmentButtonStyle)
                If Not [Enum].IsDefined(GetType(SegmentButtonStyle), value) Then
                    Throw New InvalidEnumArgumentException(NameOf(value), CInt(value), GetType(SegmentButtonStyle))
                End If
                Me.segmentStyleB = value
                Me.Invalidate()
            End Set
        End Property

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="SegmentButton"/> class.
        ''' </summary>
        Public Sub New()
            Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                        ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
            Me.Appearance = Appearance.Button
            Me.AutoSize = False
            Me.TextAlign = ContentAlignment.MiddleCenter
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
            Me.Invalidate()
        End Sub

#End Region

#Region " Friend Methods "

        ''' <summary>
        ''' Gets the background and text colors of the button for its current state.
        ''' </summary>
        ''' <param name="refBackColor">
        ''' When this method returns, contains the background color.
        ''' </param>
        ''' <param name="refTextColor">
        ''' When this method returns, contains the text color.
        ''' </param>
        Friend Sub GetStateColors(ByRef refBackColor As Color, ByRef refTextColor As Color)
            Dim p As ThemePalette = Me.palette

            If Me.segmentStyleB = SegmentButtonStyle.Filled Then
                If Me.Checked Then
                    refBackColor = If(Me.Enabled, p.AccentColor, p.ButtonPressedBackColor)
                    refTextColor = If(Me.Enabled, p.AccentTextColor, p.DisabledTextColor)
                Else
                    refBackColor = If(Me.isHovered AndAlso Me.Enabled, p.ButtonHoverBackColor, p.ButtonBackColor)
                    refTextColor = If(Me.Enabled, p.TextColor, p.DisabledTextColor)
                End If
            Else
                refBackColor = Me.BackColor
                If Not Me.Enabled Then
                    refTextColor = p.DisabledTextColor
                ElseIf Me.Checked OrElse Me.isHovered Then
                    refTextColor = p.TextColor
                Else
                    refTextColor = p.SecondaryTextColor
                End If
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
            Me.Invalidate()
        End Sub

        ''' <summary>
        ''' Raises the <see cref="RadioButton.CheckedChanged"/> event and redraws the button.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnCheckedChanged(e As EventArgs)
            MyBase.OnCheckedChanged(e)
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
            Dim textColor As Color
            Me.GetStateColors(backColor, textColor)

            Dim g As Graphics = e.Graphics
            Dim bounds As Rectangle = Me.ClientRectangle
            g.Clear(backColor)

            If Me.segmentStyleB = SegmentButtonStyle.Filled Then
                Dim borderColor As Color = If(Me.Checked AndAlso Me.Enabled, Me.palette.AccentColor, Me.palette.BorderColor)
                Using pen As New Pen(borderColor)
                    g.DrawRectangle(pen, 0, 0, bounds.Width - 1, bounds.Height - 1)
                End Using

            ElseIf Me.Checked Then
                Dim barColor As Color = If(Me.Enabled, Me.palette.AccentColor, Me.palette.DisabledTextColor)
                Using brush As New SolidBrush(barColor)
                    g.FillRectangle(brush, 0, bounds.Height - SegmentButton.UnderlineHeight, bounds.Width, SegmentButton.UnderlineHeight)
                End Using
            End If

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
