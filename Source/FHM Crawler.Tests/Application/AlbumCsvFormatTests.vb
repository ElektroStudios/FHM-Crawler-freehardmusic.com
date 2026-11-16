#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Windows.Forms

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports DevCase.Core.Extensions.ListView

Imports FHM

Imports FHMCrawler.UI

#End Region

#Region " AlbumCsvFormatTests "

Namespace ApplicationTests

    ''' <summary>
    ''' Tests the <see cref="AlbumCsvFormat"/> class.
    ''' </summary>
    <TestClass>
    Public NotInheritable Class AlbumCsvFormatTests

#Region " SanitizeText "

        ''' <summary>
        ''' Plain values are returned unchanged.
        ''' </summary>
        <TestMethod>
        <DataRow("Saxon")>
        <DataRow("Crosby, Stills & Nash")>
        <DataRow("Guns N' Roses")>
        Public Sub SanitizeText_PlainValue_IsUnchanged(value As String)
            Assert.AreEqual(value, AlbumCsvFormat.SanitizeText(value))
        End Sub

        ''' <summary>
        ''' The delimiter is replaced with a comma.
        ''' </summary>
        <TestMethod>
        <DataRow("Live; Part 1", "Live, Part 1")>
        <DataRow(";Intro;", ",Intro,")>
        <DataRow("A;;B", "A,,B")>
        Public Sub SanitizeText_Delimiter_IsReplaced(value As String, expected As String)
            Dim result As String = AlbumCsvFormat.SanitizeText(value)

            Assert.AreEqual(expected, result)
            Assert.IsFalse(result.Contains(AlbumCsvFormat.Delimiter))
        End Sub

        ''' <summary>
        ''' Line breaks and tabs are replaced with a single white-space, and the result is trimmed.
        ''' </summary>
        <TestMethod>
        <DataRow("Line 1" & vbCrLf & "Line 2", "Line 1 Line 2")>
        <DataRow("Line 1" & vbLf & vbLf & "Line 2", "Line 1 Line 2")>
        <DataRow("A" & vbTab & " B", "A B")>
        <DataRow("  Padded  ", "Padded")>
        Public Sub SanitizeText_LineBreaks_AreReplaced(value As String, expected As String)
            Assert.AreEqual(expected, AlbumCsvFormat.SanitizeText(value))
        End Sub

        ''' <summary>
        ''' A null value is rejected.
        ''' </summary>
        <TestMethod>
        Public Sub SanitizeText_Null_Throws()
            Assert.ThrowsExactly(Of ArgumentNullException)(Function() AlbumCsvFormat.SanitizeText(Nothing))
        End Sub

#End Region

#Region " GetRowCells "

        ''' <summary>
        ''' The row contains the sanitized artist name, the sanitized album title and the download links.
        ''' </summary>
        <TestMethod>
        Public Sub GetRowCells_SanitizesArtistAndTitle()
            Dim album As New AlbumInfo("1", New Uri("http://freehardmusic.com/"), "Artist; Name", "Title;" & vbLf & "Part 2",
                                       {"http://host/a", "http://host/b"})

            Dim cells As String() = AlbumCsvFormat.GetRowCells(album)

            CollectionAssert.AreEqual({"Artist, Name", "Title, Part 2", "{http://host/a, http://host/b}"}, cells)
        End Sub

        ''' <summary>
        ''' An album without download links produces an empty download links cell.
        ''' </summary>
        <TestMethod>
        Public Sub GetRowCells_NoDownloadLinks_ProducesEmptyBraces()
            Dim album As New AlbumInfo("1", New Uri("http://freehardmusic.com/"), "A", "B", Array.Empty(Of String)())

            Assert.AreEqual("{}", AlbumCsvFormat.GetRowCells(album)(2))
        End Sub

        ''' <summary>
        ''' A null album is rejected.
        ''' </summary>
        <TestMethod>
        Public Sub GetRowCells_Null_Throws()
            Assert.ThrowsExactly(Of ArgumentNullException)(Function() AlbumCsvFormat.GetRowCells(Nothing))
        End Sub

#End Region

#Region " End-To-End "

        ''' <summary>
        ''' The CSV exported from the album rows always has exactly three fields per line:
        ''' the artist names and album titles never contain the delimiter.
        ''' </summary>
        <TestMethod>
        Public Sub ExportedCsv_ArtistAndTitleNeverContainTheDelimiter()
            Dim albums As AlbumInfo() = {
                New AlbumInfo("1", New Uri("http://freehardmusic.com/"), "Emerson; Lake & Palmer", "Works; Volume 1", {"http://host/a"}),
                New AlbumInfo("2", New Uri("http://freehardmusic.com/"), "Dio", "Holy Diver", {"http://host/b", "http://host/c"})
            }

            Using lv As New ListView() With {.View = View.Details}
                lv.Columns.AddRange({New ColumnHeader() With {.Text = "Artist"},
                                     New ColumnHeader() With {.Text = "Album"},
                                     New ColumnHeader() With {.Text = "Download Links"}})
                For Each album As AlbumInfo In albums
                    lv.Items.Add(New ListViewItem(AlbumCsvFormat.GetRowCells(album)))
                Next album

                Dim lines As String() = lv.ExportToCSV(delimiter:=AlbumCsvFormat.Delimiter).
                                           Split({Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)

                Assert.AreEqual(3, lines.Length)
                For Each line As String In lines
                    Assert.IsFalse(line.Contains(""""), $"Unexpected quoted field: {line}")
                    Assert.AreEqual(3, line.Split(AlbumCsvFormat.Delimiter).Length, $"Unexpected field count: {line}")
                Next line
                Assert.AreEqual("Emerson, Lake & Palmer;Works, Volume 1;{http://host/a}", lines(1))
            End Using
        End Sub

#End Region

    End Class

End Namespace

#End Region
