#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Linq
Imports System.Windows.Forms

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports DevCase.Core.Application.UserInterface.Enums
Imports DevCase.Core.Application.UserInterface.Types

#End Region

#Region " ListViewColumnSorterTests "

Namespace DevCaseTests

    ''' <summary>
    ''' Tests the <see cref="ListViewColumnSorter"/> class.
    ''' </summary>
    <TestClass>
    Public NotInheritable Class ListViewColumnSorterTests

#Region " Tests "

        ''' <summary>
        ''' A new sorter sorts the first column by text, with no order.
        ''' </summary>
        <TestMethod>
        Public Sub Constructor_InitializesDefaults()
            Dim sorter As New ListViewColumnSorter()

            Assert.AreEqual(0, sorter.ColumnIndex)
            Assert.AreEqual(SortOrder.None, sorter.Order)
            Assert.AreEqual(SortModifiers.SortByText, sorter.SortModifier)
        End Sub

        ''' <summary>
        ''' Text columns are compared case-insensitively, and the descending order reverses the result.
        ''' </summary>
        <TestMethod>
        Public Sub Compare_Text_AscendingAndDescending()
            Dim items As ListViewItem() = Nothing
            Using lv As ListView = ListViewColumnSorterTests.CreateListView(items, {"beta"}, {"ALPHA"})
                Dim sorter As New ListViewColumnSorter() With {.Order = SortOrder.Ascending}

                Assert.IsTrue(sorter.Compare(items(0), items(1)) > 0)

                sorter.Order = SortOrder.Descending
                Assert.IsTrue(sorter.Compare(items(0), items(1)) < 0)
            End Using
        End Sub

        ''' <summary>
        ''' The sorter compares the configured column.
        ''' </summary>
        <TestMethod>
        Public Sub Compare_UsesTheConfiguredColumn()
            Dim items As ListViewItem() = Nothing
            Using lv As ListView = ListViewColumnSorterTests.CreateListView(items, {"a", "z"}, {"b", "y"})
                Dim sorter As New ListViewColumnSorter() With {.Order = SortOrder.Ascending, .ColumnIndex = 1}

                Assert.IsTrue(sorter.Compare(items(0), items(1)) > 0)
            End Using
        End Sub

        ''' <summary>
        ''' Numeric columns are compared by value.
        ''' </summary>
        <TestMethod>
        Public Sub Compare_Number_ComparesByValue()
            Dim items As ListViewItem() = Nothing
            Using lv As ListView = ListViewColumnSorterTests.CreateListView(items, {"10"}, {"9"})
                Dim sorter As New ListViewColumnSorter() With {.Order = SortOrder.Ascending, .SortModifier = SortModifiers.SortByNumber}

                Assert.IsTrue(sorter.Compare(items(0), items(1)) > 0)
            End Using
        End Sub

        ''' <summary>
        ''' Date columns are compared chronologically.
        ''' </summary>
        <TestMethod>
        Public Sub Compare_Date_ComparesChronologically()
            Dim items As ListViewItem() = Nothing
            Using lv As ListView = ListViewColumnSorterTests.CreateListView(items, {"2001-01-01"}, {"1999-12-31"})
                Dim sorter As New ListViewColumnSorter() With {.Order = SortOrder.Ascending, .SortModifier = SortModifiers.SortByDate}

                Assert.IsTrue(sorter.Compare(items(0), items(1)) > 0)
            End Using
        End Sub

        ''' <summary>
        ''' Switching back to text sorting after a numeric sort compares the values as text again.
        ''' <para></para>
        ''' Regression: the text branch kept using the previously selected comparer (e.g. the numeric one).
        ''' </summary>
        <TestMethod>
        Public Sub Compare_SwitchingBackToText_UsesTextComparer()
            Dim items As ListViewItem() = Nothing
            Using lv As ListView = ListViewColumnSorterTests.CreateListView(items, {"10"}, {"9"})
                Dim sorter As New ListViewColumnSorter() With {.Order = SortOrder.Ascending, .SortModifier = SortModifiers.SortByNumber}
                Assert.IsTrue(sorter.Compare(items(0), items(1)) > 0, "Numeric: 10 > 9")

                sorter.SortModifier = SortModifiers.SortByText

                Assert.IsTrue(sorter.Compare(items(0), items(1)) < 0, "Text: ""10"" < ""9""")
            End Using
        End Sub

        ''' <summary>
        ''' An item without the sorted sub-item is sorted first.
        ''' </summary>
        <TestMethod>
        Public Sub Compare_MissingSubItem_IsSortedFirst()
            Dim items As ListViewItem() = Nothing
            Using lv As ListView = ListViewColumnSorterTests.CreateListView(items, {"x"}, {"y", "value"})
                Dim sorter As New ListViewColumnSorter() With {.Order = SortOrder.Ascending, .ColumnIndex = 1}

                Assert.IsTrue(sorter.Compare(items(0), items(1)) < 0)
                Assert.IsTrue(sorter.Compare(items(1), items(0)) > 0)
                Assert.AreEqual(0, sorter.Compare(items(0), items(0)))
            End Using
        End Sub

        ''' <summary>
        ''' Items are considered equal when the sorter has no order or the list view is not sorted.
        ''' </summary>
        <TestMethod>
        Public Sub Compare_NoOrder_ReturnsZero()
            Dim items As ListViewItem() = Nothing
            Using lv As ListView = ListViewColumnSorterTests.CreateListView(items, {"a"}, {"b"})
                Dim sorter As New ListViewColumnSorter() With {.Order = SortOrder.None}
                Assert.AreEqual(0, sorter.Compare(items(0), items(1)))

                sorter.Order = SortOrder.Ascending
                lv.Sorting = SortOrder.None
                Assert.AreEqual(0, sorter.Compare(items(0), items(1)))
            End Using
        End Sub

        ''' <summary>
        ''' End-to-end: the sorter reorders the items of a real list view.
        ''' </summary>
        <STATestMethod>
        Public Sub Sort_ListView_ReordersItems()
            Using lv As ListView = ListViewColumnSorterTests.CreateListView(Nothing, {"Saxon"}, {"accept"}, {"Dio"})
                Dim handle As IntPtr = lv.Handle ' Sorting is performed by the native control.
                Assert.AreNotEqual(IntPtr.Zero, handle)

                Dim sorter As New ListViewColumnSorter() With {.Order = SortOrder.Ascending}
                lv.ListViewItemSorter = sorter
                lv.Sort()
                CollectionAssert.AreEqual({"accept", "Dio", "Saxon"}, lv.Items.Cast(Of ListViewItem)().Select(Function(item As ListViewItem) item.Text).ToArray())

                sorter.Order = SortOrder.Descending
                lv.Sort()
                CollectionAssert.AreEqual({"Saxon", "Dio", "accept"}, lv.Items.Cast(Of ListViewItem)().Select(Function(item As ListViewItem) item.Text).ToArray())
            End Using
        End Sub

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Creates an ascending-sorted list view with two columns and the specified rows.
        ''' </summary>
        ''' <param name="refItems">
        ''' When this method returns, contains the created items, in the same order as <paramref name="rows"/>.
        ''' </param>
        '''
        ''' <param name="rows">
        ''' The rows (item text followed by the sub-item texts).
        ''' </param>
        ''' <returns>
        ''' The list view.
        ''' </returns>
        Private Shared Function CreateListView(ByRef refItems As ListViewItem(), ParamArray rows As String()()) As ListView
            Dim lv As New ListView() With {.View = View.Details}
            lv.Columns.Add("Column 1")
            lv.Columns.Add("Column 2")

            ' Keep the item references: the list view may reorder its items once it is sorted.
            refItems = rows.Select(Function(row As String()) New ListViewItem(row)).ToArray()
            lv.Items.AddRange(refItems)
            lv.Sorting = SortOrder.Ascending
            Return lv
        End Function

#End Region

    End Class

End Namespace

#End Region
