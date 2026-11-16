#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Drawing.Drawing2D

Imports FHMCrawler.UI.Theming

#End Region

#Region " ThemedComboBox "

Namespace FHMCrawler.UI.Controls

    ''' <summary>
    ''' A drop-down list (<see cref="ComboBoxStyle.DropDownList"/>) drawn with the colors of the current <see cref="ThemePalette"/>.
    ''' <para></para>
    ''' The items are owner-drawn, and the border and the drop-down arrow are repainted over the native ones,
    ''' because the native flat combo box always draws a white border.
    ''' </summary>
    <DesignerCategory("Code")>
    Public Class ThemedComboBox : Inherits ComboBox : Implements IThemeable

#Region " Constants "

        ''' <summary>
        ''' The <c>WM_PAINT</c> window message.
        ''' </summary>
        Private Const WM_PAINT As Integer = &HF

        ''' <summary>
        ''' The horizontal space, in pixels, between the border and the text.
        ''' </summary>
        Private Const TextPadding As Integer = 6

#End Region

#Region " Private Fields "

        ''' <summary>
        ''' The last applied palette.
        ''' </summary>
        Private palette As ThemePalette = ThemePalette.Light

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="ThemedComboBox"/> class.
        ''' </summary>
        Public Sub New()
            Me.DrawMode = DrawMode.OwnerDrawFixed
            Me.DropDownStyle = ComboBoxStyle.DropDownList
            Me.FlatStyle = FlatStyle.Flat
            Me.ItemHeight = 20
            Me.IntegralHeight = False
            Me.MaxDropDownItems = 16
        End Sub

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Applies the specified palette to the combo box.
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
            Me.BackColor = palette.InputBackColor
            Me.ForeColor = palette.TextColor
            Me.Invalidate()
        End Sub

#End Region

#Region " Friend Methods "

        ''' <summary>
        ''' Gets the bounds of the drop-down arrow button.
        ''' </summary>
        ''' <param name="clientBounds">
        ''' The client bounds of the combo box.
        ''' </param>
        ''' <returns>
        ''' The bounds of the drop-down arrow button (inside the 1-pixel border).
        ''' </returns>
        Friend Shared Function GetArrowBounds(clientBounds As Rectangle) As Rectangle
            Dim width As Integer = SystemInformation.HorizontalScrollBarThumbWidth + 4
            Return New Rectangle(clientBounds.Right - width - 1, clientBounds.Top + 1, width, Math.Max(0, clientBounds.Height - 2))
        End Function

#End Region

#Region " Event Invocators "

        ''' <summary>
        ''' Draws an item of the drop-down list, or the selected item in the edit area.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
            Dim p As ThemePalette = Me.palette
            Dim isEditArea As Boolean = (e.State And DrawItemState.ComboBoxEdit) = DrawItemState.ComboBoxEdit
            Dim isSelected As Boolean = (e.State And DrawItemState.Selected) = DrawItemState.Selected

            Dim backColor As Color
            Dim textColor As Color
            If Not Me.Enabled Then
                backColor = p.InputBackColor
                textColor = p.DisabledTextColor
            ElseIf isSelected AndAlso Not isEditArea Then
                backColor = p.SelectionBackColor
                textColor = p.SelectionTextColor
            Else
                backColor = p.InputBackColor
                textColor = p.TextColor
            End If

            Using brush As New SolidBrush(backColor)
                e.Graphics.FillRectangle(brush, e.Bounds)
            End Using

            If e.Index >= 0 AndAlso e.Index < Me.Items.Count Then
                Dim text As String = Me.GetItemText(Me.Items(e.Index))
                Dim textBounds As Rectangle = Rectangle.FromLTRB(e.Bounds.Left + If(isEditArea, 2, ThemedComboBox.TextPadding),
                                                                 e.Bounds.Top, e.Bounds.Right - 2, e.Bounds.Bottom)
                TextRenderer.DrawText(e.Graphics, text, Me.Font, textBounds, textColor,
                                      TextFormatFlags.Left Or TextFormatFlags.VerticalCenter Or TextFormatFlags.SingleLine Or
                                      TextFormatFlags.EndEllipsis Or TextFormatFlags.NoPrefix)
            End If
        End Sub

        ''' <summary>
        ''' Raises the <see cref="Control.GotFocus"/> event and redraws the border.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnGotFocus(e As EventArgs)
            MyBase.OnGotFocus(e)
            Me.Invalidate()
        End Sub

        ''' <summary>
        ''' Raises the <see cref="Control.LostFocus"/> event and redraws the border.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnLostFocus(e As EventArgs)
            MyBase.OnLostFocus(e)
            Me.Invalidate()
        End Sub

        ''' <summary>
        ''' Processes the window messages; repaints the border and the drop-down arrow after the native painting.
        ''' </summary>
        ''' <param name="m">
        ''' The window message.
        ''' </param>
        Protected Overrides Sub WndProc(ByRef m As Message)
            MyBase.WndProc(m)

            If m.Msg = ThemedComboBox.WM_PAINT Then
                Using g As Graphics = Graphics.FromHwnd(Me.Handle)
                    Me.PaintChrome(g)
                End Using
            End If
        End Sub

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Paints the border, the inner ring and the drop-down arrow.
        ''' </summary>
        ''' <param name="g">
        ''' The graphics to paint on.
        ''' </param>
        Private Sub PaintChrome(g As Graphics)
            Dim p As ThemePalette = Me.palette
            Dim bounds As Rectangle = Me.ClientRectangle
            If bounds.Width <= 2 OrElse bounds.Height <= 2 Then
                Exit Sub
            End If

            ' Inner ring: covers the native inner border.
            Using innerPen As New Pen(p.InputBackColor)
                g.DrawRectangle(innerPen, 1, 1, bounds.Width - 3, bounds.Height - 3)
            End Using

            ' Drop-down arrow button.
            Dim arrowBounds As Rectangle = ThemedComboBox.GetArrowBounds(bounds)
            Using arrowBackBrush As New SolidBrush(p.InputBackColor)
                g.FillRectangle(arrowBackBrush, arrowBounds)
            End Using

            Dim chevronColor As Color = If(Me.Enabled, p.SecondaryTextColor, p.DisabledTextColor)
            Dim centerX As Integer = arrowBounds.Left + (arrowBounds.Width \ 2)
            Dim centerY As Integer = arrowBounds.Top + (arrowBounds.Height \ 2)
            g.SmoothingMode = SmoothingMode.AntiAlias
            Using chevronPen As New Pen(chevronColor, 1.6F)
                g.DrawLines(chevronPen, {New PointF(centerX - 4.0F, centerY - 2.0F),
                                         New PointF(centerX, centerY + 2.0F),
                                         New PointF(centerX + 4.0F, centerY - 2.0F)})
            End Using
            g.SmoothingMode = SmoothingMode.None

            ' Outer border.
            Dim borderColor As Color = If(Me.Focused OrElse Me.DroppedDown, p.AccentColor, p.BorderColor)
            Using borderPen As New Pen(borderColor)
                g.DrawRectangle(borderPen, 0, 0, bounds.Width - 1, bounds.Height - 1)
            End Using
        End Sub

#End Region

    End Class

End Namespace

#End Region
