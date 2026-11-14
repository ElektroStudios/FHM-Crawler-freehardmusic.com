#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Windows.Forms

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports DevCase.Core.Extensions.ListView

#End Region

#Region " ListViewExtensionsTests "

Namespace DevCaseTests

    ''' <summary>
    ''' Tests the <see cref="ListViewExtensions"/> module (CSV export).
    ''' </summary>
    <TestClass>
    Public NotInheritable Class ListViewExtensionsTests

#Region " ExportToCSV "

        ''' <summary>
        ''' The first line contains the column names and every item produces one line, in order.
        ''' </summary>
        <TestMethod>
        Public Sub ExportToCSV_WritesHeaderAndRowsInOrder()
            Using lv As ListView = ListViewExtensionsTests.CreateListView({"Artist", "Title"},
                                                                          {"Saxon", "Wheels of Steel"},
                                                                          {"Dio", "Holy Diver"})

                Dim expected As String = String.Join(Environment.NewLine, "Artist,Title", "Saxon,Wheels of Steel", "Dio,Holy Diver", "")

                Assert.AreEqual(expected, lv.ExportToCSV())
            End Using
        End Sub

        ''' <summary>
        ''' A list view without items produces only the header line.
        ''' </summary>
        <TestMethod>
        Public Sub ExportToCSV_NoItems_WritesOnlyTheHeader()
            Using lv As ListView = ListViewExtensionsTests.CreateListView({"Artist", "Title"})

                Assert.AreEqual($"Artist,Title{Environment.NewLine}", lv.ExportToCSV())
            End Using
        End Sub

        ''' <summary>
        ''' Empty sub-items are replaced with the specified default value.
        ''' </summary>
        <TestMethod>
        Public Sub ExportToCSV_EmptySubItem_UsesTheDefaultValue()
            Using lv As ListView = ListViewExtensionsTests.CreateListView({"Artist", "Title", "Year"},
                                                                          {"Saxon", "", "1980"})

                Dim lines As String() = lv.ExportToCSV(defaultValueIfEmpty:="N/A").Split({Environment.NewLine}, StringSplitOptions.None)

                Assert.AreEqual("Saxon,N/A,1980", lines(1))
            End Using
        End Sub

        ''' <summary>
        ''' Items with fewer sub-items than columns are padded with the default value,
        ''' so every line has the same amount of fields as the header.
        ''' </summary>
        <TestMethod>
        Public Sub ExportToCSV_MissingSubItems_ArePaddedWithTheDefaultValue()
            Using lv As ListView = ListViewExtensionsTests.CreateListView({"Artist", "Title", "Year"},
                                                                          {"Saxon"})

                Dim lines As String() = lv.ExportToCSV(defaultValueIfEmpty:="-").Split({Environment.NewLine}, StringSplitOptions.None)

                Assert.AreEqual("Saxon,-,-", lines(1))
            End Using
        End Sub

        ''' <summary>
        ''' Values containing commas, double quotes or line breaks are escaped (RFC 4180),
        ''' so they do not break the column layout of the CSV.
        ''' </summary>
        <TestMethod>
        Public Sub ExportToCSV_SpecialCharacters_AreEscaped()
            Using lv As ListView = ListViewExtensionsTests.CreateListView({"Artist, Band", "Title"},
                                                                          {"Crosby, Stills & Nash", "Say ""Hello"""})

                Dim lines As String() = lv.ExportToCSV().Split({Environment.NewLine}, StringSplitOptions.None)

                Assert.AreEqual("""Artist, Band"",Title", lines(0))
                Assert.AreEqual("""Crosby, Stills & Nash"",""Say """"Hello""""""", lines(1))
            End Using
        End Sub

        ''' <summary>
        ''' A null list view is rejected.
        ''' </summary>
        <TestMethod>
        Public Sub ExportToCSV_NullListView_Throws()
            Assert.ThrowsExactly(Of ArgumentNullException)(Sub() ListViewExtensions.ExportToCSV(Nothing))
        End Sub

#End Region

#Region " EscapeCsvField "

        ''' <summary>
        ''' Plain values are returned unchanged.
        ''' </summary>
        <TestMethod>
        <DataRow("Saxon", "Saxon")>
        <DataRow("Wheels of Steel", "Wheels of Steel")>
        <DataRow("AC/DC", "AC/DC")>
        Public Sub EscapeCsvField_PlainValue_IsUnchanged(value As String, expected As String)
            Assert.AreEqual(expected, ListViewExtensions.EscapeCsvField(value))
        End Sub

        ''' <summary>
        ''' Values with separators, double quotes or line breaks are quoted, and inner double quotes are doubled.
        ''' </summary>
        <TestMethod>
        <DataRow("a,b", """a,b""")>
        <DataRow("say ""hi""", """say """"hi""""""")>
        <DataRow("line1" & vbCrLf & "line2", """line1" & vbCrLf & "line2""")>
        <DataRow("line1" & vbLf & "line2", """line1" & vbLf & "line2""")>
        Public Sub EscapeCsvField_SpecialValue_IsQuoted(value As String, expected As String)
            Assert.AreEqual(expected, ListViewExtensions.EscapeCsvField(value))
        End Sub

        ''' <summary>
        ''' Null and empty values produce an empty field.
        ''' </summary>
        <TestMethod>
        <DataRow(Nothing)>
        <DataRow("")>
        Public Sub EscapeCsvField_NullOrEmpty_ReturnsEmptyString(value As String)
            Assert.AreEqual(String.Empty, ListViewExtensions.EscapeCsvField(value))
        End Sub

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Creates a <see cref="ListView"/> in details view with the specified columns and rows.
        ''' </summary>
        ''' <param name="columns">
        ''' The column names.
        ''' </param>
        '''
        ''' <param name="rows">
        ''' The rows; every row contains the text of its sub-items.
        ''' </param>
        ''' <returns>
        ''' The list view.
        ''' </returns>
        Private Shared Function CreateListView(columns As String(), ParamArray rows As String()()) As ListView
            Dim lv As New ListView() With {.View = View.Details}
            For Each column As String In columns
                lv.Columns.Add(column)
            Next column
            For Each row As String() In rows
                lv.Items.Add(New ListViewItem(row))
            Next row
            Return lv
        End Function

#End Region

    End Class

End Namespace

#End Region
