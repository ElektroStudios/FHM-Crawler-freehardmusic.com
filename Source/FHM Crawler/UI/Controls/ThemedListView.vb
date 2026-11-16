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

#Region " ThemedListView "

Namespace FHMCrawler.UI.Controls

    ''' <summary>
    ''' A details-view <see cref="ListView"/> whose column headers, rows and check boxes are drawn
    ''' with the colors of the current <see cref="ThemePalette"/>.
    ''' <para></para>
    ''' It also shows a sort glyph in the sorted column header, stretches its last column to fill the available width,
    ''' and shows a message (<see cref="ThemedListView.EmptyText"/>) while it has no items.
    ''' </summary>
    <DesignerCategory("Code")>
    Public Class ThemedListView : Inherits ListView : Implements IThemeable

#Region " Constants "

        ''' <summary>
        ''' The <c>WM_PAINT</c> window message.
        ''' </summary>
        Private Const WM_PAINT As Integer = &HF

        ''' <summary>
        ''' The <c>LVM_GETHEADER</c> message, that gets the handle of the header control of a list view.
        ''' </summary>
        Private Const LVM_GETHEADER As Integer = &H101F

        ''' <summary>
        ''' The <c>LVM_GETCOUNTPERPAGE</c> message, that gets the number of rows that fit vertically in the visible area.
        ''' </summary>
        Private Const LVM_GETCOUNTPERPAGE As Integer = &H1028

        ''' <summary>
        ''' The <c>GWL_STYLE</c> index, to retrieve the window styles with <c>GetWindowLong</c>.
        ''' </summary>
        Private Const GWL_STYLE As Integer = -16

        ''' <summary>
        ''' The <c>WS_VSCROLL</c> window style: the window shows its vertical scroll bar.
        ''' </summary>
        Private Const WS_VSCROLL As Integer = &H200000

        ''' <summary>
        ''' The horizontal space, in pixels, between the cell bounds and the text.
        ''' </summary>
        Private Const CellPadding As Integer = 6

        ''' <summary>
        ''' The size, in pixels (at 96 DPI), of the drawn check boxes.
        ''' </summary>
        Private Const CheckBoxSize As Integer = 14

        ''' <summary>
        ''' The minimum width, in pixels, of the stretched last column.
        ''' </summary>
        Private Const MinimumLastColumnWidth As Integer = 80

#End Region

#Region " Private Fields "

        ''' <summary>
        ''' The last applied palette.
        ''' </summary>
        Private palette As ThemePalette = ThemePalette.Light

        ''' <summary>
        ''' The width needed to show the widest content of the last column, measured by <see cref="ThemedListView.AutoSizeColumns"/>.
        ''' </summary>
        Private lastColumnContentWidth As Integer

        ''' <summary>
        ''' A value indicating whether the last column is being stretched (prevents re-entrant stretching).
        ''' </summary>
        Private isStretching As Boolean

        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The message shown while the list has no items.
        ''' </summary>
        Private emptyTextB As String = ""

        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The index of the column that shows the sort glyph.
        ''' </summary>
        Private sortColumnIndexB As Integer = -1

        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The sort order shown by the sort glyph.
        ''' </summary>
        Private sortOrderB As SortOrder = SortOrder.None

#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets or sets the message shown while the list has no items.
        ''' </summary>
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when the value is <see langword="Nothing"/>.
        ''' </exception>
        <Category("Appearance")>
        <DefaultValue("")>
        Public Property EmptyText As String
            Get
                Return Me.emptyTextB
            End Get
            Set(value As String)
                If value Is Nothing Then
                    Throw New ArgumentNullException(paramName:=NameOf(value))
                End If
                Me.emptyTextB = value
                Me.Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the index of the column header that shows the sort glyph (-1 for none).
        ''' </summary>
        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property SortColumnIndex As Integer
            Get
                Return Me.sortColumnIndexB
            End Get
            Set(value As Integer)
                Me.sortColumnIndexB = value
                Me.InvalidateHeader()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the sort order shown by the sort glyph.
        ''' </summary>
        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property SortOrder As SortOrder
            Get
                Return Me.sortOrderB
            End Get
            Set(value As SortOrder)
                Me.sortOrderB = value
                Me.InvalidateHeader()
            End Set
        End Property

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="ThemedListView"/> class.
        ''' </summary>
        Public Sub New()
            Me.OwnerDraw = True
            Me.DoubleBuffered = True
            Me.View = View.Details
            Me.FullRowSelect = True
            Me.HideSelection = False
            Me.BorderStyle = BorderStyle.None
        End Sub

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Applies the specified palette to the list.
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
            Me.BackColor = palette.SurfaceBackColor
            Me.ForeColor = palette.TextColor
            If Me.IsHandleCreated Then
                Me.ApplyWindowTheme()
            End If
            Me.Invalidate()
            Me.InvalidateHeader()
        End Sub

        ''' <summary>
        ''' Resizes every column to fit its content and header,
        ''' and stretches the last column to fill the available width.
        ''' </summary>
        Public Sub AutoSizeColumns()
            If Me.Columns.Count = 0 Then
                Exit Sub
            End If

            ' The widths are measured here instead of using the native auto-resize,
            ' because the native measurement ignores the owner-drawn check boxes and paddings.
            Dim widths As Integer() = New Integer(Me.Columns.Count - 1) {}
            For index As Integer = 0 To Me.Columns.Count - 1
                widths(index) = Me.MeasureHeaderWidth(index)
            Next index

            Dim checkBoxWidth As Integer = If(Me.CheckBoxes, 16 + (ThemedListView.CellPadding * 2), 0)
            For Each item As ListViewItem In Me.Items
                For index As Integer = 0 To Math.Min(item.SubItems.Count, Me.Columns.Count) - 1
                    Dim textWidth As Integer = TextRenderer.MeasureText(item.SubItems(index).Text, Me.Font).Width + (ThemedListView.CellPadding * 3)
                    If index = 0 Then
                        textWidth += checkBoxWidth
                    End If
                    widths(index) = Math.Max(widths(index), textWidth)
                Next index
            Next item

            Dim lastIndex As Integer = Me.Columns.Count - 1
            Me.isStretching = True
            Try
                For index As Integer = 0 To lastIndex - 1
                    Me.Columns(index).Width = widths(index)
                Next index
            Finally
                Me.isStretching = False
            End Try

            Me.lastColumnContentWidth = widths(lastIndex)
            Me.StretchLastColumn()
        End Sub

#End Region

#Region " Friend Methods "

        ''' <summary>
        ''' Gets the width of the last column so that it fills the available width,
        ''' but never narrower than its content or <see cref="ThemedListView.MinimumLastColumnWidth"/>.
        ''' </summary>
        ''' <param name="availableWidth">
        ''' The client width of the list.
        ''' </param>
        ''' <param name="otherColumnsWidth">
        ''' The total width of the other columns.
        ''' </param>
        ''' <param name="contentWidth">
        ''' The width needed to show the widest content of the last column.
        ''' </param>
        ''' <returns>
        ''' The width of the last column.
        ''' </returns>
        Friend Shared Function GetStretchedWidth(availableWidth As Integer, otherColumnsWidth As Integer, contentWidth As Integer) As Integer
            Dim fillWidth As Integer = availableWidth - otherColumnsWidth
            Return Math.Max(ThemedListView.MinimumLastColumnWidth, Math.Max(fillWidth, contentWidth))
        End Function

        ''' <summary>
        ''' Gets the width available for the columns once the vertical scroll bar is shown or hidden as the items require.
        ''' <para></para>
        ''' The client width alone is not reliable: while the drawing is suspended (<see cref="ListView.BeginUpdate"/>),
        ''' the list does not show or hide its vertical scroll bar until the drawing is resumed, so the client width
        ''' still includes (or excludes) a scroll bar that is about to disappear (or appear).
        ''' </summary>
        ''' <param name="clientWidth">
        ''' The current client width of the list.
        ''' </param>
        ''' <param name="verticalScrollBarWidth">
        ''' The width of a vertical scroll bar.
        ''' </param>
        ''' <param name="isVerticalScrollBarVisible">
        ''' <see langword="True"/> if the vertical scroll bar is currently shown (so it is excluded from <paramref name="clientWidth"/>).
        ''' </param>
        ''' <param name="isVerticalScrollBarNeeded">
        ''' <see langword="True"/> if the items do not fit vertically, so the vertical scroll bar will be shown.
        ''' </param>
        ''' <returns>
        ''' The available width, in pixels.
        ''' </returns>
        ''' <exception cref="ArgumentOutOfRangeException">
        ''' Thrown when <paramref name="clientWidth"/> or <paramref name="verticalScrollBarWidth"/> is negative.
        ''' </exception>
        Friend Shared Function GetAvailableWidth(clientWidth As Integer, verticalScrollBarWidth As Integer,
                                                 isVerticalScrollBarVisible As Boolean, isVerticalScrollBarNeeded As Boolean) As Integer
            If clientWidth < 0 Then
                Throw New ArgumentOutOfRangeException(NameOf(clientWidth), clientWidth, "The client width cannot be negative.")
            End If
            If verticalScrollBarWidth < 0 Then
                Throw New ArgumentOutOfRangeException(NameOf(verticalScrollBarWidth), verticalScrollBarWidth, "The scroll bar width cannot be negative.")
            End If

            Dim width As Integer = clientWidth
            If isVerticalScrollBarVisible Then
                width += verticalScrollBarWidth
            End If
            If isVerticalScrollBarNeeded Then
                width -= verticalScrollBarWidth
            End If
            Return Math.Max(0, width)
        End Function

        ''' <summary>
        ''' Gets the bounds of a cell of a row in details view, from the row bounds and the column widths.
        ''' </summary>
        ''' <param name="rowBounds">
        ''' The bounds of the row (its left side is already displaced by the horizontal scroll).
        ''' </param>
        ''' <param name="columnWidths">
        ''' The widths of the columns, in display order.
        ''' </param>
        ''' <param name="columnIndex">
        ''' The index of the column of the cell.
        ''' </param>
        ''' <returns>
        ''' The bounds of the cell.
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when <paramref name="columnWidths"/> is <see langword="Nothing"/>.
        ''' </exception>
        ''' <exception cref="ArgumentOutOfRangeException">
        ''' Thrown when <paramref name="columnIndex"/> is not a valid column index.
        ''' </exception>
        Friend Shared Function GetCellBounds(rowBounds As Rectangle, columnWidths As Integer(), columnIndex As Integer) As Rectangle
            If columnWidths Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(columnWidths))
            End If
            If columnIndex < 0 OrElse columnIndex >= columnWidths.Length Then
                Throw New ArgumentOutOfRangeException(paramName:=NameOf(columnIndex), actualValue:=columnIndex, message:="Invalid column index.")
            End If

            Dim left As Integer = rowBounds.Left
            For index As Integer = 0 To columnIndex - 1
                left += columnWidths(index)
            Next index
            Return New Rectangle(left, rowBounds.Top, columnWidths(columnIndex), rowBounds.Height)
        End Function

        ''' <summary>
        ''' Gets the background color of the specified row.
        ''' </summary>
        ''' <param name="item">
        ''' The item of the row.
        ''' </param>
        ''' <returns>
        ''' The selection color for selected items; otherwise, alternating subtle row colors.
        ''' </returns>
        Friend Function GetRowBackColor(item As ListViewItem) As Color
            If item.Selected Then
                Return Me.palette.SelectionBackColor
            End If
            If (item.Index Mod 2) = 1 Then
                Return ToggleSwitch.Blend(Me.BackColor, Me.palette.TextColor, 0.04F)
            End If
            Return Me.BackColor
        End Function

#End Region

#Region " Event Invocators "

        ''' <summary>
        ''' Raises the <see cref="Control.HandleCreated"/> event and applies the native visual style (scroll bars).
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnHandleCreated(e As EventArgs)
            MyBase.OnHandleCreated(e)
            Me.ApplyWindowTheme()
        End Sub

        ''' <summary>
        ''' Draws a column header.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnDrawColumnHeader(e As DrawListViewColumnHeaderEventArgs)
            Dim p As ThemePalette = Me.palette
            Dim g As Graphics = e.Graphics
            Dim bounds As Rectangle = e.Bounds

            Using backBrush As New SolidBrush(p.HeaderBackColor)
                g.FillRectangle(backBrush, bounds)
            End Using

            Dim isSorted As Boolean = (e.ColumnIndex = Me.sortColumnIndexB) AndAlso (Me.sortOrderB <> SortOrder.None)
            Dim glyphWidth As Integer = If(isSorted, 14, 0)
            Dim textBounds As Rectangle = Rectangle.FromLTRB(bounds.Left + ThemedListView.CellPadding, bounds.Top,
                                                             bounds.Right - ThemedListView.CellPadding - glyphWidth, bounds.Bottom)
            TextRenderer.DrawText(g, e.Header.Text, Me.Font, textBounds, p.TextColor,
                                  TextFormatFlags.Left Or TextFormatFlags.VerticalCenter Or TextFormatFlags.SingleLine Or
                                  TextFormatFlags.EndEllipsis Or TextFormatFlags.NoPrefix)

            If isSorted Then
                ThemedListView.DrawSortGlyph(g, bounds, Me.sortOrderB, p.SecondaryTextColor)
            End If

            Using borderPen As New Pen(p.BorderColor)
                g.DrawLine(borderPen, bounds.Right - 1, bounds.Top + 4, bounds.Right - 1, bounds.Bottom - 5)
                g.DrawLine(borderPen, bounds.Left, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1)
            End Using
        End Sub

        ''' <summary>
        ''' Draws a whole row: its background, its check box and the text of every cell.
        ''' <para></para>
        ''' The whole row is drawn here (not only its background) because, when the mouse moves over an item
        ''' of an owner-drawn list with full row selection, Windows redraws the row raising only
        ''' <see cref="ListView.DrawItem"/>, without <see cref="ListView.DrawSubItem"/>; drawing only the background
        ''' here would erase the texts of the row under the mouse.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnDrawItem(e As DrawListViewItemEventArgs)
            e.DrawDefault = False
            Dim g As Graphics = e.Graphics

            Using backBrush As New SolidBrush(Me.GetRowBackColor(e.Item))
                g.FillRectangle(backBrush, e.Bounds)
            End Using

            If Me.View <> View.Details OrElse Me.Columns.Count = 0 Then
                Me.DrawCellContent(g, e.Item, 0, e.Bounds)
                Exit Sub
            End If

            Dim widths As Integer() = New Integer(Me.Columns.Count - 1) {}
            For index As Integer = 0 To Me.Columns.Count - 1
                widths(index) = Me.Columns(index).Width
            Next index

            For index As Integer = 0 To Me.Columns.Count - 1
                Me.DrawCellContent(g, e.Item, index, ThemedListView.GetCellBounds(e.Bounds, widths, index))
            Next index
        End Sub

        ''' <summary>
        ''' Draws a cell: its background, its check box (first column) and its text.
        ''' <para></para>
        ''' The result is identical to the cell drawn by <see cref="ThemedListView.OnDrawItem"/>.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnDrawSubItem(e As DrawListViewSubItemEventArgs)
            e.DrawDefault = False

            Using backBrush As New SolidBrush(Me.GetRowBackColor(e.Item))
                e.Graphics.FillRectangle(backBrush, e.Bounds)
            End Using
            Me.DrawCellContent(e.Graphics, e.Item, e.ColumnIndex, e.Bounds)
        End Sub

        ''' <summary>
        ''' Raises the <see cref="ListView.ColumnWidthChanged"/> event, redraws the list and stretches the last column.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnColumnWidthChanged(e As ColumnWidthChangedEventArgs)
            MyBase.OnColumnWidthChanged(e)
            Me.Invalidate()
            If Not Me.isStretching AndAlso e.ColumnIndex <> Me.Columns.Count - 1 Then
                Me.StretchLastColumn()
            End If
        End Sub

        ''' <summary>
        ''' Raises the <see cref="Control.Resize"/> event and stretches the last column.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnResize(e As EventArgs)
            MyBase.OnResize(e)
            Me.StretchLastColumn()
        End Sub

        ''' <summary>
        ''' Processes the window messages; draws the <see cref="ThemedListView.EmptyText"/> while the list has no items.
        ''' </summary>
        ''' <param name="m">
        ''' The window message.
        ''' </param>
        Protected Overrides Sub WndProc(ByRef m As Message)
            MyBase.WndProc(m)

            If m.Msg = ThemedListView.WM_PAINT AndAlso Me.Items.Count = 0 AndAlso Me.emptyTextB.Length <> 0 Then
                Using g As Graphics = Graphics.FromHwnd(Me.Handle)
                    Dim textBounds As Rectangle = Rectangle.Inflate(Me.ClientRectangle, -24, -24)
                    TextRenderer.DrawText(g, Me.emptyTextB, Me.Font, textBounds, Me.palette.SecondaryTextColor,
                                          TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter Or
                                          TextFormatFlags.WordBreak Or TextFormatFlags.NoPrefix)
                End Using
            End If
        End Sub

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Draws the content of a cell (the check box of the first column, and the text), without its background.
        ''' </summary>
        ''' <param name="g">
        ''' The graphics to draw on.
        ''' </param>
        ''' <param name="item">
        ''' The item of the row.
        ''' </param>
        ''' <param name="columnIndex">
        ''' The index of the column of the cell.
        ''' </param>
        ''' <param name="bounds">
        ''' The bounds of the cell.
        ''' </param>
        Private Sub DrawCellContent(g As Graphics, item As ListViewItem, columnIndex As Integer, bounds As Rectangle)
            Dim p As ThemePalette = Me.palette

            Dim textLeft As Integer = bounds.Left + ThemedListView.CellPadding
            If columnIndex = 0 AndAlso Me.CheckBoxes Then
                Dim checkBounds As Rectangle = Me.GetCheckBoxBounds(item, bounds, g.DpiX)
                ThemedListView.DrawCheckBox(g, checkBounds, item.Checked, p)
                textLeft = checkBounds.Right + ThemedListView.CellPadding
            End If

            If columnIndex >= item.SubItems.Count Then
                Exit Sub
            End If

            Dim textColor As Color = If(item.Selected, p.SelectionTextColor, p.TextColor)
            Dim textBounds As Rectangle = Rectangle.FromLTRB(textLeft, bounds.Top, bounds.Right - ThemedListView.CellPadding, bounds.Bottom)
            If textBounds.Width > 0 Then
                TextRenderer.DrawText(g, item.SubItems(columnIndex).Text, Me.Font, textBounds, textColor,
                                      TextFormatFlags.Left Or TextFormatFlags.VerticalCenter Or TextFormatFlags.SingleLine Or
                                      TextFormatFlags.EndEllipsis Or TextFormatFlags.NoPrefix)
            End If
        End Sub

        ''' <summary>
        ''' Applies the native visual style that matches the palette (dark or light scroll bars).
        ''' </summary>
        Private Sub ApplyWindowTheme()
            Dim subAppName As String = If(Me.palette.Theme = AppTheme.Dark, "DarkMode_Explorer", "Explorer")
            NativeMethods.SetWindowTheme(Me.Handle, subAppName, Nothing)
        End Sub

        ''' <summary>
        ''' Redraws the column headers.
        ''' </summary>
        Private Sub InvalidateHeader()
            If Not Me.IsHandleCreated Then
                Exit Sub
            End If

            Dim headerHandle As IntPtr = NativeMethods.SendMessage(Me.Handle, ThemedListView.LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero)
            If headerHandle <> IntPtr.Zero Then
                NativeMethods.InvalidateRect(headerHandle, IntPtr.Zero, True)
            End If
        End Sub

        ''' <summary>
        ''' Stretches the last column to fill the available width.
        ''' </summary>
        Private Sub StretchLastColumn()
            If Me.isStretching OrElse Me.Columns.Count = 0 Then
                Exit Sub
            End If

            Dim lastIndex As Integer = Me.Columns.Count - 1
            Dim otherColumnsWidth As Integer = 0
            For index As Integer = 0 To lastIndex - 1
                otherColumnsWidth += Me.Columns(index).Width
            Next index

            Dim width As Integer = ThemedListView.GetStretchedWidth(Me.GetColumnsAvailableWidth(), otherColumnsWidth, Me.lastColumnContentWidth)
            If Me.Columns(lastIndex).Width <> width Then
                Me.isStretching = True
                Try
                    Me.Columns(lastIndex).Width = width
                Finally
                    Me.isStretching = False
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Gets the width available for the columns, taking into account whether the vertical scroll bar
        ''' will be shown when the drawing is resumed (see <see cref="ThemedListView.GetAvailableWidth"/>).
        ''' </summary>
        ''' <returns>
        ''' The available width, in pixels.
        ''' </returns>
        Private Function GetColumnsAvailableWidth() As Integer
            If Not Me.IsHandleCreated Then
                Return Me.ClientSize.Width
            End If

            Dim style As Integer = NativeMethods.GetWindowLong(Me.Handle, ThemedListView.GWL_STYLE)
            Dim isVisible As Boolean = (style And ThemedListView.WS_VSCROLL) <> 0
            Dim rowsPerPage As Integer = NativeMethods.SendMessage(Me.Handle, ThemedListView.LVM_GETCOUNTPERPAGE, IntPtr.Zero, IntPtr.Zero).ToInt32()
            Dim isNeeded As Boolean = Me.Items.Count > rowsPerPage

            Return ThemedListView.GetAvailableWidth(Me.ClientSize.Width, SystemInformation.VerticalScrollBarWidth, isVisible, isNeeded)
        End Function

        ''' <summary>
        ''' Measures the width needed to show the text and the sort glyph of a column header.
        ''' </summary>
        ''' <param name="columnIndex">
        ''' The column index.
        ''' </param>
        ''' <returns>
        ''' The width, in pixels.
        ''' </returns>
        Private Function MeasureHeaderWidth(columnIndex As Integer) As Integer
            Return TextRenderer.MeasureText(Me.Columns(columnIndex).Text, Me.Font).Width + (ThemedListView.CellPadding * 2) + 18
        End Function

        ''' <summary>
        ''' Gets the bounds of the check box of an item, aligned with the native check box (state image) area,
        ''' so that the native mouse handling toggles the item when the drawn check box is clicked.
        ''' </summary>
        ''' <param name="item">
        ''' The item.
        ''' </param>
        ''' <param name="cellBounds">
        ''' The bounds of the first cell of the item.
        ''' </param>
        ''' <param name="dpi">
        ''' The horizontal resolution of the drawing surface, in dots per inch.
        ''' </param>
        ''' <returns>
        ''' The bounds of the check box.
        ''' </returns>
        Private Function GetCheckBoxBounds(item As ListViewItem, cellBounds As Rectangle, dpi As Single) As Rectangle
            Dim scale As Double = dpi / 96.0R
            Dim size As Integer = CInt(Math.Round(ThemedListView.CheckBoxSize * scale))
            Dim stateImageWidth As Integer = CInt(Math.Round(16 * scale))

            ' The native state image is placed just before the icon area.
            Dim iconBounds As Rectangle = item.GetBounds(ItemBoundsPortion.Icon)
            Dim stateLeft As Integer = iconBounds.Left - stateImageWidth
            If stateLeft < cellBounds.Left Then
                stateLeft = cellBounds.Left + 2
            End If

            Dim x As Integer = stateLeft + ((stateImageWidth - size) \ 2)
            Dim y As Integer = cellBounds.Top + ((cellBounds.Height - size) \ 2)
            Return New Rectangle(x, y, size, size)
        End Function

        ''' <summary>
        ''' Draws a flat check box.
        ''' </summary>
        ''' <param name="g">
        ''' The graphics to draw on.
        ''' </param>
        ''' <param name="bounds">
        ''' The bounds of the check box.
        ''' </param>
        ''' <param name="isChecked">
        ''' A value indicating whether the check box is checked.
        ''' </param>
        ''' <param name="p">
        ''' The palette.
        ''' </param>
        Private Shared Sub DrawCheckBox(g As Graphics, bounds As Rectangle, isChecked As Boolean, p As ThemePalette)
            Dim boxBounds As New Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1)

            If isChecked Then
                Using fillBrush As New SolidBrush(p.AccentColor)
                    g.FillRectangle(fillBrush, boxBounds)
                End Using

                Dim oldMode As SmoothingMode = g.SmoothingMode
                g.SmoothingMode = SmoothingMode.AntiAlias
                Using checkPen As New Pen(p.AccentTextColor, Math.Max(1.6F, bounds.Width / 8.0F))
                    g.DrawLines(checkPen, {New PointF(boxBounds.Left + (boxBounds.Width * 0.22F), boxBounds.Top + (boxBounds.Height * 0.52F)),
                                           New PointF(boxBounds.Left + (boxBounds.Width * 0.42F), boxBounds.Top + (boxBounds.Height * 0.72F)),
                                           New PointF(boxBounds.Left + (boxBounds.Width * 0.78F), boxBounds.Top + (boxBounds.Height * 0.3F))})
                End Using
                g.SmoothingMode = oldMode
            Else
                Using fillBrush As New SolidBrush(p.InputBackColor)
                    g.FillRectangle(fillBrush, boxBounds)
                End Using
                Using borderPen As New Pen(p.SecondaryTextColor)
                    g.DrawRectangle(borderPen, boxBounds)
                End Using
            End If
        End Sub

        ''' <summary>
        ''' Draws the sort glyph (a small triangle) at the right side of a column header.
        ''' </summary>
        ''' <param name="g">
        ''' The graphics to draw on.
        ''' </param>
        ''' <param name="headerBounds">
        ''' The bounds of the column header.
        ''' </param>
        ''' <param name="order">
        ''' The sort order.
        ''' </param>
        ''' <param name="color">
        ''' The glyph color.
        ''' </param>
        Private Shared Sub DrawSortGlyph(g As Graphics, headerBounds As Rectangle, order As SortOrder, color As Color)
            Dim centerX As Single = headerBounds.Right - ThemedListView.CellPadding - 6.0F
            Dim centerY As Single = headerBounds.Top + (headerBounds.Height / 2.0F)
            Dim points As PointF()
            If order = SortOrder.Ascending Then
                points = {New PointF(centerX - 4.0F, centerY + 2.0F), New PointF(centerX + 4.0F, centerY + 2.0F), New PointF(centerX, centerY - 2.5F)}
            Else
                points = {New PointF(centerX - 4.0F, centerY - 2.0F), New PointF(centerX + 4.0F, centerY - 2.0F), New PointF(centerX, centerY + 2.5F)}
            End If

            Dim oldMode As SmoothingMode = g.SmoothingMode
            g.SmoothingMode = SmoothingMode.AntiAlias
            Using brush As New SolidBrush(color)
                g.FillPolygon(brush, points)
            End Using
            g.SmoothingMode = oldMode
        End Sub

#End Region

    End Class

End Namespace

#End Region
