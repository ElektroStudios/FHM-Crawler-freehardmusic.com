Imports FHM

Imports ElektroKit.Core.Application.UserInterface.Types
Imports ElektroKit.Core.Application.UserInterface.Enums
Imports ElektroKit.Core.Extensions.ListView

Public Class Main

    Private WithEvents FHMCrawler As Crawler
    Private sorter As New ListViewColumnSorter
    Private albumCount As Integer

    Public Sub New()
        ' This call is required by the designer.
        MyClass.InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.FHMCrawler = New Crawler()
        Me.sorter = New ListViewColumnSorter() With {
            .Order = SortOrder.Ascending,
            .SortModifier = SortModifiers.SortByText
        }
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.ComboBox_Country.DataSource = DataSources.countries
        Me.ComboBox_Genre.DataSource = DataSources.genres
        Me.ComboBox_Year.DataSource = DataSources.years

        Me.ListView_Albums.ListViewItemSorter = Me.sorter
        Me.ListView_Albums.Sorting = SortOrder.Ascending
    End Sub

    Private Async Sub Button_FetchAlbums_Click(sender As Object, e As EventArgs) Handles Button_FetchAlbums.Click

        If (Me.GroupBox_ArtistSearch.Enabled) Then
            With Me.FHMCrawler.SearchFilter
                .Artist = Me.TextBoxArtist.Text
                .Country = "all"
                .Genre = "all"
                .Year = "all"
            End With

        Else
            With Me.FHMCrawler.SearchFilter
                .Artist = ""
                .Country = Me.ComboBox_Country.Text
                .Genre = Me.ComboBox_Genre.Text
                .Year = Me.ComboBox_Year.Text
            End With

        End If

        Me.DisableControls()

        Me.Label_AlbumCount.Text = "Searching albums..."
        Me.albumCount = Await Me.FHMCrawler.GetAlbumCountAsync()
        Me.ProgressBar1.Maximum = Me.albumCount
        Me.ProgressBar1.Step = 10 ' 10 albums per page.
        Me.Button_Cancel.Enabled = True
        Me.Label_AlbumCount.Text = String.Format("Fetching {0} albums...", Me.albumCount)

        Await Me.FHMCrawler.FetchAlbumsAsync()

        Me.EnableControls()

    End Sub

    Private Sub Button_Cancel_Click(sender As Object, e As EventArgs) Handles Button_Cancel.Click
        DirectCast(sender, Button).Enabled = False
        Me.Label_AlbumCount.Text = "Cancelling operation..."
        Me.FHMCrawler.CancelFetchAlbumsAsync()
    End Sub

    Private Sub DisableControls()
        Me.GroupBox_ArtistSearch.Enabled = False
        Me.GroupBox_CustomSearch.Enabled = False
        Me.Button_FetchAlbums.Enabled = False
        Me.Button_CopyAllAsCSV.Enabled = False
        Me.Button_CopyCheckedItems.Enabled = False
        Me.RadioButton_ArtistSearch.Enabled = False
        Me.RadioButton_CustomSearch.Enabled = False
        Me.Button_CheckAll.Enabled = False
        Me.Button_UncheckAll.Enabled = False

        Me.ProgressBar1.Value = 0
        Me.sorter.Order = SortOrder.None
        Me.ListView_Albums.BeginUpdate()
        Me.ListView_Albums.Items.Clear()
        Me.ListView_Albums.EndUpdate()
    End Sub

    Private Sub EnableControls()
        Me.Button_Cancel.Enabled = False
        Me.Label_AlbumCount.Text = String.Format("{0} albums fetched.", Me.ListView_Albums.Items.Count)
        Me.GroupBox_ArtistSearch.Enabled = Me.RadioButton_ArtistSearch.Checked
        Me.GroupBox_CustomSearch.Enabled = Me.RadioButton_CustomSearch.Checked
        Me.Button_FetchAlbums.Enabled = True
        Me.Button_CopyAllAsCSV.Enabled = True
        Me.Button_CopyCheckedItems.Enabled = True
        Me.RadioButton_ArtistSearch.Enabled = True
        Me.RadioButton_CustomSearch.Enabled = True
        Me.Button_CheckAll.Enabled = True
        Me.Button_UncheckAll.Enabled = True
    End Sub

    Private Sub RadioButton_ArtistSearch_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton_ArtistSearch.CheckedChanged
        Dim rb As RadioButton = DirectCast(sender, RadioButton)
        Me.GroupBox_ArtistSearch.Enabled = rb.Checked
        Me.GroupBox_CustomSearch.Enabled = Not rb.Checked
    End Sub

    Private Sub RadioButton_CustomSearch_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton_CustomSearch.CheckedChanged
        Dim rb As RadioButton = DirectCast(sender, RadioButton)
        Me.GroupBox_CustomSearch.Enabled = rb.Checked
        Me.GroupBox_ArtistSearch.Enabled = Not rb.Checked
    End Sub

    Private Sub ButtonSaveAllAsCSV_Click(sender As Object, e As EventArgs) Handles Button_CopyAllAsCSV.Click
        Clipboard.SetText(Me.ListView_Albums.ExportToCSV())
    End Sub

    Private Sub Button_CopyCheckedItems_Click(sender As Object, e As EventArgs) Handles Button_CopyCheckedItems.Click
        Me.ListView_Albums.CopyCheckedItems(separator:=",")
    End Sub

    Private Sub Button_CheckAll_Click(sender As Object, e As EventArgs) Handles Button_CheckAll.Click
        For Each lvItem As ListViewItem In Me.ListView_Albums.Items
            lvItem.Checked = True
        Next
    End Sub

    Private Sub Button_UncheckAll_Click(sender As Object, e As EventArgs) Handles Button_UncheckAll.Click
        For Each lvItem As ListViewItem In Me.ListView_Albums.CheckedItems
            lvItem.Checked = False
        Next
    End Sub

    Private Sub ListView_Albums_ColumnClick(ByVal sender As Object, ByVal e As ColumnClickEventArgs) _
    Handles ListView_Albums.ColumnClick

        Dim lv As ListView = DirectCast(sender, ListView)

        ' Dinamycaly sets the sort-modifier to sort the column by text, number, or date.
        Me.sorter.SortModifier = CType(lv.Columns(e.Column).Tag, SortModifiers)

        ' Determine whether clicked column is already the column that is being sorted.
        If (e.Column = Me.sorter.ColumnIndex) Then
            ' Reverse the current sort direction for this column.
            If (Me.sorter.Order = SortOrder.Ascending) Then
                Me.sorter.Order = SortOrder.Descending
            Else
                Me.sorter.Order = SortOrder.Ascending
            End If

        Else
            ' Set the column number that is to be sorted, default to ascending.
            Me.sorter.ColumnIndex = e.Column
            Me.sorter.Order = SortOrder.Ascending

        End If ' e.Column

        ' Perform the sort.
        lv.Sort()

    End Sub

    <DebuggerStepperBoundary>
    Private Sub FHMCrawler_BeginPageCrawl(ByVal sender As Object, e As PageCrawlBeginEventArgs) Handles FHMCrawler.BeginPageCrawl
#If Debug Then
        Debug.WriteLine("Begin crawling page index: {0}", e.PageIndex)
#End If
    End Sub

    <DebuggerStepperBoundary>
    Private Sub FHMCrawler_EndPageCrawl(ByVal sender As Object, e As PageCrawlEndEventArgs) Handles FHMCrawler.EndPageCrawl
#If Debug Then
        Debug.WriteLine("End crawling page index: {0}", e.PageIndex)
#End If
        Me.ListView_Albums.BeginUpdate()
        For Each albumInfo As AlbumInfo In e.Albums
            Dim lvItem As New ListViewItem({albumInfo.Artist, albumInfo.Album, String.Format("{{{0}}}", String.Join(", ", albumInfo.DounloadUrls))})
            Me.ListView_Albums.Items.Add(lvItem)
        Next albumInfo
        Me.ListView_Albums.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
        Me.ListView_Albums.EnsureVisible(Me.ListView_Albums.Items.Count - 1)
        Me.ListView_Albums.EndUpdate()

        Me.ProgressBar1.PerformStep()
    End Sub

End Class
