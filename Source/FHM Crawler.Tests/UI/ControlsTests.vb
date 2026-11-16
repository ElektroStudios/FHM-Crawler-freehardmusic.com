#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports FHMCrawler.UI.Controls
Imports FHMCrawler.UI.Theming

#End Region

#Region " ControlsTests "

Namespace UITests

    ''' <summary>
    ''' Tests the custom controls of the user interface.
    ''' </summary>
    <TestClass>
    Public NotInheritable Class ControlsTests

#Region " Common "

        ''' <summary>
        ''' Every themeable control rejects a null palette.
        ''' </summary>
        <TestMethod>
        Public Sub ApplyTheme_NullPalette_Throws()
            Dim controls As Control() = {New CardPanel(), New ToggleSwitch(), New FlatProgressBar(), New ThemedButton(),
                                         New SegmentButton(), New ThemedTextBox(), New ThemedComboBox(), New ThemedListView(), New IconButton()}
            Try
                For Each ctrl As Control In controls
                    Dim themeable As IThemeable = DirectCast(ctrl, IThemeable)
                    Assert.ThrowsExactly(Of ArgumentNullException)(Sub() themeable.ApplyTheme(Nothing), ctrl.GetType().Name)
                Next ctrl
            Finally
                For Each ctrl As Control In controls
                    ctrl.Dispose()
                Next ctrl
            End Try
        End Sub

#End Region

#Region " CardPanel "

        ''' <summary>
        ''' A card uses the surface color; a panel configured to use the window color uses the window color.
        ''' </summary>
        <TestMethod>
        Public Sub CardPanel_ApplyTheme_UsesSurfaceOrWindowColor()
            Dim palette As ThemePalette = ThemePalette.Dark
            Using card As New CardPanel(), windowPanel As New CardPanel() With {.UseSurfaceColor = False}
                card.ApplyTheme(palette)
                windowPanel.ApplyTheme(palette)

                Assert.AreEqual(palette.SurfaceBackColor, card.BackColor)
                Assert.AreEqual(palette.WindowBackColor, windowPanel.BackColor)
                Assert.AreEqual(palette.BorderColor, card.BorderColor)
                Assert.AreEqual(BorderSides.All, card.BorderSides)
            End Using
        End Sub

        ''' <summary>
        ''' The border sides are flags (bit-shifted values).
        ''' </summary>
        <TestMethod>
        Public Sub BorderSides_AreFlags()
            Assert.AreEqual(1, CInt(BorderSides.Left))
            Assert.AreEqual(2, CInt(BorderSides.Top))
            Assert.AreEqual(4, CInt(BorderSides.Right))
            Assert.AreEqual(8, CInt(BorderSides.Bottom))
            Assert.AreEqual(15, CInt(BorderSides.All))
        End Sub

#End Region

#Region " ToggleSwitch "

        ''' <summary>
        ''' The switch keeps the check box behavior: checking it raises <see cref="CheckBox.CheckedChanged"/>,
        ''' and without a window handle the thumb moves immediately (no animation).
        ''' </summary>
        <TestMethod>
        Public Sub ToggleSwitch_Checked_RaisesEventAndMovesTheThumb()
            Using toggle As New ToggleSwitch()
                Dim changes As Integer = 0
                AddHandler toggle.CheckedChanged, Sub(sender As Object, e As EventArgs) changes += 1

                toggle.Checked = True
                Assert.AreEqual(1, changes)
                Assert.AreEqual(1.0F, toggle.GetThumbPosition())

                toggle.Checked = False
                Assert.AreEqual(2, changes)
                Assert.AreEqual(0.0F, toggle.GetThumbPosition())
            End Using
        End Sub

        ''' <summary>
        ''' The switch uses the accent color for the "on" state and the track color for the "off" state.
        ''' </summary>
        <TestMethod>
        Public Sub ToggleSwitch_ApplyTheme_UsesAccentAndTrackColors()
            Dim palette As ThemePalette = ThemePalette.Dark
            Using toggle As New ToggleSwitch()
                toggle.ApplyTheme(palette)

                Assert.AreEqual(palette.AccentColor, toggle.OnColor)
                Assert.AreEqual(palette.AccentHoverColor, toggle.OnHoverColor)
                Assert.AreEqual(palette.TrackColor, toggle.OffColor)
            End Using
        End Sub

        ''' <summary>
        ''' Blending two colors interpolates every channel.
        ''' </summary>
        <TestMethod>
        Public Sub Blend_InterpolatesTheChannels()
            Dim black As Color = Color.FromArgb(0, 0, 0)
            Dim white As Color = Color.FromArgb(255, 255, 255)

            Assert.AreEqual(black.ToArgb(), ToggleSwitch.Blend(black, white, 0.0F).ToArgb())
            Assert.AreEqual(white.ToArgb(), ToggleSwitch.Blend(black, white, 1.0F).ToArgb())
            Assert.AreEqual(Color.FromArgb(128, 128, 128).ToArgb(), ToggleSwitch.Blend(black, white, 0.5F).ToArgb())
        End Sub

        ''' <summary>
        ''' A blend amount outside the 0 to 1 range is rejected.
        ''' </summary>
        <TestMethod>
        <DataRow(-0.1F)>
        <DataRow(1.1F)>
        Public Sub Blend_InvalidAmount_Throws(amount As Single)
            Assert.ThrowsExactly(Of ArgumentOutOfRangeException)(Function() ToggleSwitch.Blend(Color.Black, Color.White, amount))
        End Sub

#End Region

#Region " FlatProgressBar "

        ''' <summary>
        ''' Reset sets the range, clears the position and restores the normal state.
        ''' </summary>
        <TestMethod>
        Public Sub FlatProgressBar_Reset_RestoresTheInitialState()
            Using bar As New FlatProgressBar()
                bar.Value = 50
                bar.State = ProgressBarState.Error

                bar.Reset(maximum:=40)

                Assert.AreEqual(0, bar.Minimum)
                Assert.AreEqual(40, bar.Maximum)
                Assert.AreEqual(0, bar.Value)
                Assert.AreEqual(ProgressBarState.Normal, bar.State)
            End Using
        End Sub

        ''' <summary>
        ''' Increment advances the position without exceeding the maximum.
        ''' </summary>
        <TestMethod>
        Public Sub FlatProgressBar_Increment_ClampsToTheMaximum()
            Using bar As New FlatProgressBar()
                bar.Reset(maximum:=25)

                bar.Increment(10)
                Assert.AreEqual(10, bar.Value)
                bar.Increment(10)
                bar.Increment(10)
                Assert.AreEqual(25, bar.Value)
                Assert.AreEqual(1.0R, bar.GetFraction())
            End Using
        End Sub

        ''' <summary>
        ''' An empty range (no albums) has a zero fraction instead of a division by zero.
        ''' </summary>
        <TestMethod>
        Public Sub FlatProgressBar_EmptyRange_HasZeroFraction()
            Using bar As New FlatProgressBar()
                bar.Reset(maximum:=0)

                Assert.AreEqual(0.0R, bar.GetFraction())
                bar.Increment(10)
                Assert.AreEqual(0, bar.Value)
            End Using
        End Sub

        ''' <summary>
        ''' Invalid values are rejected.
        ''' </summary>
        <TestMethod>
        Public Sub FlatProgressBar_InvalidValues_Throw()
            Using bar As New FlatProgressBar()
                bar.Reset(maximum:=10)

                Assert.ThrowsExactly(Of ArgumentOutOfRangeException)(Sub() bar.Value = 11)
                Assert.ThrowsExactly(Of ArgumentOutOfRangeException)(Sub() bar.Value = -1)
                Assert.ThrowsExactly(Of ArgumentOutOfRangeException)(Sub() bar.Maximum = -1)
                Assert.ThrowsExactly(Of ArgumentOutOfRangeException)(Sub() bar.Minimum = 11)
                Assert.ThrowsExactly(Of ArgumentOutOfRangeException)(Sub() bar.Increment(-1))
                Assert.ThrowsExactly(Of ArgumentOutOfRangeException)(Sub() bar.Reset(-1))
                Assert.ThrowsExactly(Of InvalidEnumArgumentException)(Sub() bar.State = CType(9, ProgressBarState))
            End Using
        End Sub

        ''' <summary>
        ''' The bar color depends on the state: accent (normal), warning or error.
        ''' </summary>
        <TestMethod>
        Public Sub FlatProgressBar_State_SelectsTheBarColor()
            Dim palette As ThemePalette = ThemePalette.Dark
            Using bar As New FlatProgressBar()
                bar.ApplyTheme(palette)

                Assert.AreEqual(palette.AccentColor, bar.GetBarColor())
                bar.State = ProgressBarState.Warning
                Assert.AreEqual(palette.WarningColor, bar.GetBarColor())
                bar.State = ProgressBarState.Error
                Assert.AreEqual(palette.ErrorColor, bar.GetBarColor())
            End Using
        End Sub

#End Region

#Region " ThemedButton "

        ''' <summary>
        ''' A primary button uses the accent colors; a standard button uses the button colors.
        ''' </summary>
        <TestMethod>
        Public Sub ThemedButton_StateColors_DependOnTheButtonKind()
            Dim palette As ThemePalette = ThemePalette.Dark
            Using primary As New ThemedButton() With {.IsPrimary = True}, standard As New ThemedButton()
                primary.ApplyTheme(palette)
                standard.ApplyTheme(palette)

                Dim backColor As Color
                Dim borderColor As Color
                Dim textColor As Color

                primary.GetStateColors(backColor, borderColor, textColor)
                Assert.AreEqual(palette.AccentColor, backColor)
                Assert.AreEqual(palette.AccentTextColor, textColor)

                standard.GetStateColors(backColor, borderColor, textColor)
                Assert.AreEqual(palette.ButtonBackColor, backColor)
                Assert.AreEqual(palette.BorderColor, borderColor)
                Assert.AreEqual(palette.TextColor, textColor)
            End Using
        End Sub

        ''' <summary>
        ''' A disabled button (primary or not) uses the disabled text color and no accent.
        ''' </summary>
        <TestMethod>
        Public Sub ThemedButton_Disabled_UsesTheDisabledColors()
            Dim palette As ThemePalette = ThemePalette.Dark
            Using primary As New ThemedButton() With {.IsPrimary = True, .Enabled = False}
                primary.ApplyTheme(palette)

                Dim backColor As Color
                Dim borderColor As Color
                Dim textColor As Color
                primary.GetStateColors(backColor, borderColor, textColor)

                Assert.AreEqual(palette.ButtonBackColor, backColor)
                Assert.AreEqual(palette.DisabledTextColor, textColor)
            End Using
        End Sub

#End Region

#Region " SegmentButton "

        ''' <summary>
        ''' A navigation tab highlights the checked tab with the primary text color, and the others with the secondary one.
        ''' </summary>
        <TestMethod>
        Public Sub SegmentButton_Underline_StateColors()
            Dim palette As ThemePalette = ThemePalette.Dark
            Using tab As New SegmentButton()
                tab.ApplyTheme(palette)
                Dim backColor As Color
                Dim textColor As Color

                tab.GetStateColors(backColor, textColor)
                Assert.AreEqual(palette.SecondaryTextColor, textColor)

                tab.Checked = True
                tab.GetStateColors(backColor, textColor)
                Assert.AreEqual(palette.TextColor, textColor)

                tab.Enabled = False
                tab.GetStateColors(backColor, textColor)
                Assert.AreEqual(palette.DisabledTextColor, textColor)
            End Using
        End Sub

        ''' <summary>
        ''' A segment of a segmented control is filled with the accent color when it is checked.
        ''' </summary>
        <TestMethod>
        Public Sub SegmentButton_Filled_StateColors()
            Dim palette As ThemePalette = ThemePalette.Dark
            Using segment As New SegmentButton() With {.SegmentStyle = SegmentButtonStyle.Filled}
                segment.ApplyTheme(palette)
                Dim backColor As Color
                Dim textColor As Color

                segment.GetStateColors(backColor, textColor)
                Assert.AreEqual(palette.ButtonBackColor, backColor)

                segment.Checked = True
                segment.GetStateColors(backColor, textColor)
                Assert.AreEqual(palette.AccentColor, backColor)
                Assert.AreEqual(palette.AccentTextColor, textColor)
            End Using
        End Sub

        ''' <summary>
        ''' Segment buttons of the same container are mutually exclusive, as radio buttons.
        ''' </summary>
        <TestMethod>
        Public Sub SegmentButton_Siblings_AreMutuallyExclusive()
            Using container As New Panel(), first As New SegmentButton(), second As New SegmentButton()
                container.Controls.AddRange({first, second})

                first.Checked = True
                second.Checked = True

                Assert.IsFalse(first.Checked)
                Assert.IsTrue(second.Checked)
            End Using
        End Sub

        ''' <summary>
        ''' An undefined style is rejected.
        ''' </summary>
        <TestMethod>
        Public Sub SegmentButton_UndefinedStyle_Throws()
            Using segment As New SegmentButton()
                Assert.ThrowsExactly(Of InvalidEnumArgumentException)(Sub() segment.SegmentStyle = CType(7, SegmentButtonStyle))
            End Using
        End Sub

#End Region

#Region " ThemedTextBox "

        ''' <summary>
        ''' The text is stored in the hosted text box, and its changes raise <see cref="Control.TextChanged"/>.
        ''' </summary>
        <TestMethod>
        Public Sub ThemedTextBox_Text_IsForwardedToTheHostedTextBox()
            Using txt As New ThemedTextBox()
                Dim changes As Integer = 0
                AddHandler txt.TextChanged, Sub(sender As Object, e As EventArgs) changes += 1

                txt.Text = "Iron Maiden"
                Assert.AreEqual("Iron Maiden", txt.TextBox.Text)
                Assert.AreEqual(1, changes)

                txt.TextBox.Text = "Saxon"
                Assert.AreEqual("Saxon", txt.Text)
                Assert.AreEqual(2, changes)
            End Using
        End Sub

        ''' <summary>
        ''' The hosted text box has no native border, and it gets the input colors of the palette.
        ''' </summary>
        <TestMethod>
        Public Sub ThemedTextBox_ApplyTheme_ColorsTheHostedTextBox()
            Dim palette As ThemePalette = ThemePalette.Dark
            Using txt As New ThemedTextBox()
                txt.ApplyTheme(palette)

                Assert.AreEqual(BorderStyle.None, txt.TextBox.BorderStyle)
                Assert.AreEqual(palette.InputBackColor, txt.TextBox.BackColor)
                Assert.AreEqual(palette.TextColor, txt.TextBox.ForeColor)
            End Using
        End Sub

        ''' <summary>
        ''' The read-only and cue banner properties are kept; a null cue banner is rejected.
        ''' </summary>
        <TestMethod>
        Public Sub ThemedTextBox_Properties()
            Using txt As New ThemedTextBox()
                txt.ReadOnly = True
                txt.CueBanner = "Band or artist name"

                Assert.IsTrue(txt.TextBox.ReadOnly)
                Assert.AreEqual("Band or artist name", txt.CueBanner)
                Assert.ThrowsExactly(Of ArgumentNullException)(Sub() txt.CueBanner = Nothing)
            End Using
        End Sub

#End Region

#Region " ThemedComboBox "

        ''' <summary>
        ''' The combo box is an owner-drawn flat drop-down list.
        ''' </summary>
        <TestMethod>
        Public Sub ThemedComboBox_IsAnOwnerDrawnDropDownList()
            Using cb As New ThemedComboBox()
                Assert.AreEqual(ComboBoxStyle.DropDownList, cb.DropDownStyle)
                Assert.AreEqual(DrawMode.OwnerDrawFixed, cb.DrawMode)
                Assert.AreEqual(FlatStyle.Flat, cb.FlatStyle)
            End Using
        End Sub

        ''' <summary>
        ''' The drop-down arrow is placed at the right side, inside the border.
        ''' </summary>
        <TestMethod>
        Public Sub ThemedComboBox_GetArrowBounds_IsInsideTheRightBorder()
            Dim client As New Rectangle(0, 0, 200, 26)

            Dim arrow As Rectangle = ThemedComboBox.GetArrowBounds(client)

            Assert.AreEqual(client.Right - 1, arrow.Right)
            Assert.AreEqual(1, arrow.Top)
            Assert.AreEqual(24, arrow.Height)
            Assert.IsTrue(arrow.Width > 0)
        End Sub

#End Region

#Region " ThemedListView "

        ''' <summary>
        ''' The last column fills the available width, but never gets narrower than its content or the minimum width.
        ''' </summary>
        <TestMethod>
        <DataRow(800, 300, 200, 500)>
        <DataRow(800, 300, 650, 650)>
        <DataRow(400, 380, 50, 80)>
        Public Sub GetStretchedWidth_FillsTheAvailableWidth(available As Integer, others As Integer, content As Integer, expected As Integer)
            Assert.AreEqual(expected, ThemedListView.GetStretchedWidth(available, others, content))
        End Sub

        ''' <summary>
        ''' Resizing the columns to their content makes the last column fill the remaining width.
        ''' </summary>
        <TestMethod>
        Public Sub AutoSizeColumns_StretchesTheLastColumn()
            Using lv As New ThemedListView() With {.Width = 900, .Height = 300, .CheckBoxes = True}
                lv.Columns.AddRange({New ColumnHeader() With {.Text = "Artist"},
                                     New ColumnHeader() With {.Text = "Album"},
                                     New ColumnHeader() With {.Text = "Download links"}})
                lv.Items.Add(New ListViewItem({"Iron Maiden", "The Number Of The Beast", "{http://host/a}"}))

                lv.AutoSizeColumns()

                Dim artistTextWidth As Integer = TextRenderer.MeasureText("Iron Maiden", lv.Font).Width
                Assert.IsTrue(lv.Columns(0).Width > artistTextWidth, "The first column must fit its text and the check box.")
                Assert.AreEqual(lv.ClientSize.Width, lv.Columns(0).Width + lv.Columns(1).Width + lv.Columns(2).Width)
            End Using
        End Sub

        ''' <summary>
        ''' Rows alternate their background color, and selected rows use the selection color.
        ''' </summary>
        <TestMethod>
        Public Sub GetRowBackColor_AlternatesAndHighlightsTheSelection()
            Dim palette As ThemePalette = ThemePalette.Dark
            Using lv As New ThemedListView()
                lv.ApplyTheme(palette)
                Dim first As ListViewItem = lv.Items.Add("first")
                Dim second As ListViewItem = lv.Items.Add("second")

                Assert.AreEqual(palette.SurfaceBackColor, lv.GetRowBackColor(first))
                Assert.AreNotEqual(palette.SurfaceBackColor.ToArgb(), lv.GetRowBackColor(second).ToArgb())

                first.Selected = True
                Assert.AreEqual(palette.SelectionBackColor, lv.GetRowBackColor(first))
            End Using
        End Sub

        ''' <summary>
        ''' The list is an owner-drawn details view without native border; a null empty text is rejected.
        ''' </summary>
        <TestMethod>
        Public Sub ThemedListView_Defaults()
            Using lv As New ThemedListView()
                Assert.IsTrue(lv.OwnerDraw)
                Assert.AreEqual(View.Details, lv.View)
                Assert.IsTrue(lv.FullRowSelect)
                Assert.AreEqual(BorderStyle.None, lv.BorderStyle)
                Assert.AreEqual(-1, lv.SortColumnIndex)
                Assert.ThrowsExactly(Of ArgumentNullException)(Sub() lv.EmptyText = Nothing)
            End Using
        End Sub

        ''' <summary>
        ''' The available width accounts for the vertical scroll bar that the items need, not for the one currently shown:
        ''' while the drawing is suspended, the list does not show its scroll bar yet, and the columns would overflow
        ''' (showing a horizontal scroll bar) once it appears.
        ''' </summary>
        <TestMethod>
        <DataRow(800, False, False, 800)>
        <DataRow(800, False, True, 783)>
        <DataRow(783, True, True, 783)>
        <DataRow(783, True, False, 800)>
        Public Sub GetAvailableWidth_AccountsForTheNeededScrollBar(clientWidth As Integer, isVisible As Boolean, isNeeded As Boolean, expected As Integer)
            Assert.AreEqual(expected, ThemedListView.GetAvailableWidth(clientWidth, 17, isVisible, isNeeded))
        End Sub

        ''' <summary>
        ''' The available width is never negative, and negative arguments are rejected.
        ''' </summary>
        <TestMethod>
        Public Sub GetAvailableWidth_Limits()
            Assert.AreEqual(0, ThemedListView.GetAvailableWidth(10, 17, False, True))
            Assert.ThrowsExactly(Of ArgumentOutOfRangeException)(Function() ThemedListView.GetAvailableWidth(-1, 17, False, False))
            Assert.ThrowsExactly(Of ArgumentOutOfRangeException)(Function() ThemedListView.GetAvailableWidth(100, -1, False, False))
        End Sub

#End Region

#Region " ThemedListView Row Drawing "

        ''' <summary>
        ''' The cell bounds of a row follow the column widths, from the (scrolled) left side of the row.
        ''' <para></para>
        ''' The whole row is drawn with these bounds when Windows redraws the row under the mouse.
        ''' </summary>
        <TestMethod>
        <DataRow(0, 0, 120)>
        <DataRow(1, 120, 200)>
        <DataRow(2, 320, 400)>
        Public Sub GetCellBounds_FollowsTheColumnWidths(columnIndex As Integer, expectedLeft As Integer, expectedWidth As Integer)
            Dim row As New Rectangle(0, 40, 720, 19)

            Dim cell As Rectangle = ThemedListView.GetCellBounds(row, {120, 200, 400}, columnIndex)

            Assert.AreEqual(expectedLeft, cell.Left)
            Assert.AreEqual(expectedWidth, cell.Width)
            Assert.AreEqual(row.Top, cell.Top)
            Assert.AreEqual(row.Height, cell.Height)
        End Sub

        ''' <summary>
        ''' A horizontally scrolled row (negative left side) displaces every cell.
        ''' </summary>
        <TestMethod>
        Public Sub GetCellBounds_ScrolledRow_DisplacesTheCells()
            Dim row As New Rectangle(-50, 0, 720, 19)

            Assert.AreEqual(70, ThemedListView.GetCellBounds(row, {120, 200, 400}, 1).Left)
        End Sub

        ''' <summary>
        ''' Invalid arguments are rejected.
        ''' </summary>
        <TestMethod>
        Public Sub GetCellBounds_InvalidArguments_Throw()
            Dim row As New Rectangle(0, 0, 100, 19)

            Assert.ThrowsExactly(Of ArgumentNullException)(Function() ThemedListView.GetCellBounds(row, Nothing, 0))
            Assert.ThrowsExactly(Of ArgumentOutOfRangeException)(Function() ThemedListView.GetCellBounds(row, {100}, 1))
            Assert.ThrowsExactly(Of ArgumentOutOfRangeException)(Function() ThemedListView.GetCellBounds(row, {100}, -1))
        End Sub

#End Region

#Region " IconButton "

        ''' <summary>
        ''' The button shows the icon of the current theme.
        ''' </summary>
        <TestMethod>
        Public Sub IconButton_ShowsTheIconOfTheTheme()
            Using lightImage As New Bitmap(8, 8), darkImage As New Bitmap(8, 8),
                  btn As New IconButton() With {.LightThemeImage = lightImage, .DarkThemeImage = darkImage}

                btn.ApplyTheme(ThemePalette.Light)
                Assert.AreSame(lightImage, btn.GetCurrentImage())

                btn.ApplyTheme(ThemePalette.Dark)
                Assert.AreSame(darkImage, btn.GetCurrentImage())
            End Using
        End Sub

        ''' <summary>
        ''' The icon is centered in the button.
        ''' </summary>
        <TestMethod>
        Public Sub IconButton_GetIconBounds_CentersTheIcon()
            Dim bounds As Rectangle = IconButton.GetIconBounds(New Rectangle(0, 0, 32, 32), New Size(20, 20))

            Assert.AreEqual(New Rectangle(6, 6, 20, 20), bounds)
        End Sub

        ''' <summary>
        ''' A non-positive icon size is rejected.
        ''' </summary>
        <TestMethod>
        Public Sub IconButton_InvalidIconSize_Throws()
            Using btn As New IconButton()
                Assert.ThrowsExactly(Of ArgumentOutOfRangeException)(Sub() btn.IconSize = New Size(0, 20))
                Assert.ThrowsExactly(Of ArgumentOutOfRangeException)(Sub() btn.IconSize = New Size(20, -1))
            End Using
        End Sub

#End Region

#Region " RedrawScope "

        ''' <summary>
        ''' A null control is rejected.
        ''' </summary>
        <TestMethod>
        Public Sub RedrawScope_NullControl_Throws()
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub()
                                                               Using New FHMCrawler.UI.RedrawScope(Nothing)
                                                               End Using
                                                           End Sub)
        End Sub

        ''' <summary>
        ''' A control without window handle has nothing drawn yet: the scope does nothing, and does not create the handle.
        ''' </summary>
        <TestMethod>
        Public Sub RedrawScope_ControlWithoutHandle_DoesNothing()
            Using panel As New Panel()
                Using scope As New FHMCrawler.UI.RedrawScope(panel)
                    panel.BackColor = Color.Red
                    scope.Dispose()
                End Using

                Assert.IsFalse(panel.IsHandleCreated)
            End Using
        End Sub

#End Region

    End Class

End Namespace

#End Region
