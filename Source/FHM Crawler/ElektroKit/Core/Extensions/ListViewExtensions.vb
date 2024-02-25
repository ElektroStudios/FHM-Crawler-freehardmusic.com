




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

Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Windows.Forms.ListViewItem
Imports WinForms = System.Windows.Forms

#End Region

#Region " ListView Extensions "

Namespace ElektroKit.Core.Extensions.[ListView]

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Contains custom extension methods to use with <see cref="ListView"/> control.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <HideModuleName>
    Public Module ListViewExtensions

#Region " Public Extension Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Exports the source <see cref="WinForms.ListView"/> to <c>CSV</c> table format.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim lv As New ListView
        ''' lv.Columns.Add("column1")
        ''' lv.Columns.Add("column2")
        ''' lv.Columns.Add("column3")
        ''' 
        ''' lv.Items.Add("Item1").SubItems.AddRange({"SubItem1", "SubItem2"})
        ''' lv.Items.Add("Item2").SubItems.Add("SubItem1")
        ''' lv.Items.Add("Item3").SubItems.Add("SubItem1")
        ''' 
        ''' Dim csv As String = lv.ExportToCSV(defaultValueIfEmpty:="N/A")
        ''' Console.WriteLine(csv)
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source <see cref="WinForms.ListView"/>.
        ''' </param>
        ''' 
        ''' <param name="defaultValueIfEmpty">
        ''' A default value to write in a <c>CSV</c> field 
        ''' if the corresponding <see cref="ListViewSubItem"/> value is <see langword="Nothing"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting <c>CSV</c> table string.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Function ExportToCSV(ByVal sender As WinForms.ListView,
                                    Optional ByVal defaultValueIfEmpty As String = "") As String

            Dim sb As New Text.StringBuilder()

            ' Add the column names to the CSV.
            For Each col As ColumnHeader In sender.Columns
                sb.Append(col.Text & ","c)
            Next col
            sb.Remove((sb.Length - 1), 1)
            sb.AppendLine()

            ' Add the item values to the CSV.
            For Each row As ListViewItem In sender.Items

                For Each cell As ListViewSubItem In row.SubItems

                    If Not String.IsNullOrEmpty(cell.Text) Then
                        sb.Append(cell.Text & ","c)
                    Else
                        sb.Append(defaultValueIfEmpty & ","c)
                    End If

                Next cell

                sb.Remove((sb.Length - 1), 1)

                If (row.SubItems.Count < sender.Columns.Count) Then
                    For count As Integer = 0 To ((sender.Columns.Count - row.SubItems.Count) - 1)
                        sb.Append(String.Format(",{0}", defaultValueIfEmpty))
                    Next
                End If

                sb.AppendLine()

            Next row

            Return sb.ToString()

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Copies all the text contained in the current checked items of <see cref="WinForms.ListView"/> to the clipboard.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source <see cref="ListView"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Sub CopyCheckedItems(ByVal sender As WinForms.ListView)

            ListViewExtensions.CopyCheckedItems(sender, "; ", -1)

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Copies all the text contained in the current checked items of <see cref="WinForms.ListView"/> to the clipboard.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source <see cref="ListView"/>.
        ''' </param>
        ''' 
        ''' <param name="separator">
        ''' The string used to separate the text of the subitems.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Sub CopyCheckedItems(ByVal sender As WinForms.ListView,
                                    ByVal separator As String)

            ListViewExtensions.CopyCheckedItems(sender, separator, -1)

        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Copies all the text contained in the current checked items of <see cref="WinForms.ListView"/> to the clipboard.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source <see cref="WinForms.ListView"/>.
        ''' </param>
        ''' 
        ''' <param name="separator">
        ''' The string used to separate the text of the subitems.
        ''' </param>
        ''' 
        ''' <param name="subItemIndex">
        ''' The index of the <see cref="ListViewSubItem"/> to copy.
        ''' <para></para>
        ''' If <paramref name="subItemIndex"/> is <c>-1</c>, all subitems are copied.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Sub CopyCheckedItems(ByVal sender As WinForms.ListView,
                                    ByVal separator As String,
                                    ByVal subItemIndex As Integer)

            If (sender.CheckedIndices.Count = 0) Then
                Exit Sub
            End If

            Dim sb As New StringBuilder

            If (subItemIndex = -1) Then
                For Each item As ListViewItem In sender.CheckedItems
                    sb.Append(String.Join(separator, From subItem As ListViewSubItem In item.SubItems.Cast(Of ListViewSubItem)()
                                                     Select subItem.Text))
                    sb.AppendLine()
                Next item

            Else
                For Each item As ListViewItem In sender.CheckedItems
                    sb.AppendFormat("{0}{1}", separator, item.SubItems(subItemIndex).Text)
                    sb.AppendLine()
                Next item

            End If

            Clipboard.SetText(sb.ToString())

        End Sub

#End Region

    End Module

End Namespace

#End Region
