




' THIS OPEN-SOURCE APPLICATION IS POWERED BY ELEKTROKIT FRAMEWORK, CREATED BY ELEKTRO STUDIOS.

' WHAT YOU SEE HERE IS FREE CUTTED CONTENT OF THIS FRAMEWORK.

' IF YOU LIKED THIS FREE APPLICATION, THEN PLEASE CONSIDER TO BUY ELEKTROKIT FRAMEWORK FOR .NET AT:
' https://codecanyon.net/item/elektrokit-class-library-for-net/19260282

' YOU CAN FIND THESE HELPER METHODS AND A MASSIVE AMOUNT MORE!, 
' +850 EXTENSION METHODS FOR ALL KIND OF TYPES, CUSTOM USER-CONTROLS, 
' EVERYTHING FOR THE NEWBIE And THE ADVANCED USER, FOR VB.NET AND C#. 

' ElektroKit is a utility framework containing new APIs and extensions to the core .NET Framework 
' to help complete your developer toolbox.
' It Is a set of general purpose classes provided as easy to consume packages.
' These utility classes and components provide productivity in day to day software development 
' mainly focused To WindowsForms. 

' UPDATES OF ELEKTROKIT ARE MAINTAINED AND RELEASED EVERY MONTH.




#Region " Imports "

Imports ElektroKit.Core.Application.Enums
Imports ElektroKit.Core.Application.Types
Imports ElektroKit.Core.Application.UserInterface.Enums

#End Region

#Region " ListView's Column Sorter "

Namespace ElektroKit.Core.Application.UserInterface.Types

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Performs a sorting operation in a <see cref="ListView"/>.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <example> This is a code example.
    ''' <code>
    ''' Public Class Form1 : Inherits Form
    ''' 
    '''     Friend WithEvents MyListView As New ListView
    '''     Private sorter As New ListViewColumnSorter
    ''' 
    '''     Public Sub New()
    ''' 
    '''         MyClass.InitializeComponent()
    ''' 
    '''         With Me.MyListView
    ''' 
    '''             ' Set the sorter, our ListViewColumnSorter.
    '''             .ListViewItemSorter = sorter
    ''' 
    '''             ' The initial direction for the sorting.
    '''             .Sorting = SortOrder.Ascending
    ''' 
    '''             ' Set the initial sort-modifier.
    '''             sorter.SortModifier = SortModifiers.SortByText
    ''' 
    '''             ' Add some columns.
    '''             .Columns.Add("Text").Tag = SortModifiers.SortByText
    '''             .Columns.Add("Numbers").Tag = SortModifiers.SortByNumber
    '''             .Columns.Add("Dates").Tag = SortModifiers.SortByDate
    ''' 
    '''             ' Adjust the column sizes.
    '''             For Each col As ColumnHeader In Me.MyListView.Columns
    '''                 col.Width = 100
    '''             Next
    ''' 
    '''             ' Add some items.
    '''             .Items.Add("hello").SubItems.AddRange({"2", "11/11/2000"})
    '''             .Items.Add("yeehaa!").SubItems.AddRange({"1", "9/9/1999"})
    '''             .Items.Add("El3ktr0").SubItems.AddRange({"100", "21/08/2014"})
    '''             .Items.Add("wow").SubItems.AddRange({"10", "11-11-2000"})
    ''' 
    '''             ' Styling things.
    '''             .Dock = DockStyle.Fill
    '''             .View = View.Details
    '''             .FullRowSelect = True
    '''         End With
    ''' 
    '''         With Me ' Styling things.
    '''             .Size = New Size(400, 200)
    '''             .FormBorderStyle =WinForms.FormBorderStyle.FixedSingle
    '''             .MaximizeBox = False
    '''             .StartPosition = FormStartPosition.CenterScreen
    '''             .Text = "ListViewColumnSorter TestForm"
    '''         End With
    ''' 
    '''         Me.Controls.Add(Me.MyListView)
    ''' 
    '''     End Sub
    ''' 
    '''     ''' ----------------------------------------------------------------------------------------------------
    '''     ''' &lt;summary&gt;
    '''     ''' Handles the &lt;see cref="ListView.ColumnClick"/&gt; event of the &lt;see cref="MyListView"/&gt; control.
    '''     ''' &lt;/summary&gt;
    '''     ''' ----------------------------------------------------------------------------------------------------
    '''     ''' &lt;param name="sender"&gt;
    '''     ''' The source of the event.
    '''     ''' &lt;/param&gt;
    '''     ''' 
    '''     ''' &lt;param name="e"&gt;
    '''     ''' The &lt;see cref="ColumnClickEventArgs"/&gt; instance containing the event data.
    '''     ''' &lt;/param&gt;
    '''     ''' ----------------------------------------------------------------------------------------------------
    '''     Private Sub MyListView_ColumnClick(ByVal sender As Object, ByVal e As ColumnClickEventArgs) _
    '''     Handles MyListView.ColumnClick
    ''' 
    '''         Dim lv As ListView = DirectCast(sender, ListView)
    ''' 
    '''         ' Dinamycaly sets the sort-modifier to sort the column by text, number, or date.
    '''         sorter.SortModifier = DirectCast(lv.Columns(e.Column).Tag, SortModifiers)
    ''' 
    '''         ' Determine whether clicked column is already the column that is being sorted.
    '''         If (e.Column = sorter.ColumnIndex) Then
    ''' 
    '''             ' Reverse the current sort direction for this column.
    '''             If (sorter.Order = SortOrder.Ascending) Then
    '''                 sorter.Order = SortOrder.Descending
    ''' 
    '''             Else
    '''                 sorter.Order = SortOrder.Ascending
    ''' 
    '''             End If
    ''' 
    '''         Else
    '''             ' Set the column number that is to be sorted, default to ascending.
    '''             sorter.ColumnIndex = e.Column
    '''             sorter.Order = SortOrder.Ascending
    ''' 
    '''         End If ' e.Column
    ''' 
    '''         ' Perform the sort.
    '''         lv.Sort()
    ''' 
    '''     End Sub
    ''' 
    ''' End Class
    ''' </code>
    ''' </example>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class ListViewColumnSorter : Implements IComparer

#Region " Private Fields "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The comparer instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private comparer As IComparer

#End Region

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the index of the column to which to apply the sorting operation (default index is <c>0</c>).
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The index of the column to which to apply the sorting operation (default index is <c>0</c>).
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Property ColumnIndex As Integer
            <DebuggerStepThrough>
            Get
                Return Me.columnIndexB
            End Get
            <DebuggerStepThrough>
            Set(ByVal value As Integer)
                Me.columnIndexB = value
            End Set
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing field )
        ''' The index of the column to which to apply the sorting operation (default index is <c>0</c>).
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private columnIndexB As Integer

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the order of sorting to apply.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The order of sorting to apply.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Property Order As SortOrder
            <DebuggerStepThrough>
            Get
                Return Me.orderB
            End Get
            <DebuggerStepThrough>
            Set(ByVal value As SortOrder)
                Me.orderB = value
            End Set
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing field )
        ''' The order of sorting to apply.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private orderB As SortOrder

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the sort modifier.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <value>
        ''' The sort modifier.
        ''' </value>
        ''' ----------------------------------------------------------------------------------------------------
        Public Property SortModifier As SortModifiers
            <DebuggerStepThrough>
            Get
                Return Me.sortModifierB
            End Get
            <DebuggerStepThrough>
            Set(ByVal value As SortModifiers)
                Me.sortModifierB = value
            End Set
        End Property
        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' ( Backing field )
        ''' The sort modifier.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private sortModifierB As SortModifiers

#End Region

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="ListViewColumnSorter"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerNonUserCode>
        Public Sub New()

            Me.comparer = New TextComparer
            Me.columnIndexB = 0
            Me.orderB = SortOrder.None
            Me.sortModifierB = SortModifiers.SortByText

        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="a">
        ''' The first object to compare.
        ''' </param>
        ''' 
        ''' <param name="b">
        ''' The second object to compare.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' A signed integer that indicates the relative values of <paramref name="a"/> and <paramref name="b"/>.
        ''' <para></para>
        ''' 0: <paramref name="a"/> equals <paramref name="b"/>.
        ''' <para></para>
        ''' Less than 0: <paramref name="a"/> is less than <paramref name="b"/>.
        ''' <para></para>
        ''' Greater than 0: <paramref name="a"/> is greater than <paramref name="b"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Function Compare(ByVal a As Object, ByVal b As Object) As Integer Implements IComparer.Compare

            Dim compareResult As ComparerResult = ComparerResult.Equals
            Dim lvItemA As ListViewItem
            Dim lvItemB As ListViewItem

            ' Cast the objects to be compared
            lvItemA = DirectCast(a, ListViewItem)
            lvItemB = DirectCast(b, ListViewItem)

            Dim strA As String = If(Not lvItemA.SubItems.Count <= Me.columnIndexB,
                                    lvItemA.SubItems(Me.columnIndexB).Text,
                                    Nothing)

            Dim strB As String = If(Not lvItemB.SubItems.Count <= Me.columnIndexB,
                                    lvItemB.SubItems(Me.columnIndexB).Text,
                                    Nothing)

            Dim listViewMain As ListView = lvItemA.ListView

            ' Calculate correct return value based on object comparison
            If listViewMain.Sorting <> SortOrder.Ascending AndAlso listViewMain.Sorting <> SortOrder.Descending Then

                ' Return '0' to indicate they are equal
                Return ComparerResult.Equals

            End If

            If Me.sortModifierB.Equals(SortModifiers.SortByText) Then

                ' Compare the two items
                If lvItemA.SubItems.Count <= Me.columnIndexB AndAlso lvItemB.SubItems.Count <= Me.columnIndexB Then
                    compareResult = DirectCast(Me.comparer.Compare(Nothing, Nothing), ComparerResult)

                ElseIf lvItemA.SubItems.Count <= Me.columnIndexB AndAlso lvItemB.SubItems.Count > Me.columnIndexB Then
                    compareResult = DirectCast(Me.comparer.Compare(Nothing, strB), ComparerResult)

                ElseIf lvItemA.SubItems.Count > Me.columnIndexB AndAlso lvItemB.SubItems.Count <= Me.columnIndexB Then
                    compareResult = DirectCast(Me.comparer.Compare(strA, Nothing), ComparerResult)

                Else
                    compareResult = DirectCast(Me.comparer.Compare(strA, strB), ComparerResult)

                End If

            Else ' Me.sortModifierB IsNot SortModifiers.SortByText.

                Select Case Me.sortModifierB

                    Case SortModifiers.SortByNumber
                        If Me.comparer.GetType <> GetType(NumericComparer) Then
                            Me.comparer = New NumericComparer
                        End If

                    Case SortModifiers.SortByDate
                        If Me.comparer.GetType <> GetType(DateComparer) Then
                            Me.comparer = New DateComparer
                        End If

                    Case Else
                        If Me.comparer.GetType <> GetType(TextComparer) Then
                            Me.comparer = New TextComparer
                        End If

                End Select

                compareResult = DirectCast(Me.comparer.Compare(strA, strB), ComparerResult)

            End If ' Me.sortModifierB.Equals(...)

            ' Calculate the proper return value based on object comparison.
            If Me.orderB = SortOrder.Ascending Then
                ' Ascending sort is selected, return normal result of the comparison operation.
                Return compareResult

            ElseIf Me.orderB = SortOrder.Descending Then
                ' Descending sort is selected, return negative result of the comparison operation.
                Return -CInt(compareResult)

            Else
                ' Return '0' to indicate they are equal.
                Return 0

            End If ' Me.orderB = ...

        End Function

#End Region

    End Class

End Namespace

#End Region
