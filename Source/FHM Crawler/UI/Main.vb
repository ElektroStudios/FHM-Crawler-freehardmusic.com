Imports System.Net

Imports FHM

Imports DevCase.Core.Application.UserInterface.Types
Imports DevCase.Core.Application.UserInterface.Enums
Imports DevCase.Core.Extensions.ListView

Imports System.Diagnostics

Public Class Main : Inherits System.Windows.Forms.Form

    Private WithEvents FHMCrawler As Crawler
    Private sorter As New ListViewColumnSorter
    Private albumCount As Integer
    Private isLoggedIn As Boolean
    Private cookies As CookieContainer

    Public Sub New()
        ' This call is required by the designer.
        MyClass.InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.sorter = New ListViewColumnSorter() With {
            .Order = SortOrder.Ascending,
            .SortModifier = SortModifiers.SortByText
        }
    End Sub

#Region " Event Handlers "

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = $"{My.Application.Info.Title} {My.Application.Info.Version.ToString(fieldCount:=3)} — by {My.Application.Info.CompanyName}"

        Me.ComboBox_Country.DataSource = DataSources.countries
        Me.ComboBox_Genre.DataSource = DataSources.genres
        Me.ComboBox_Year.DataSource = DataSources.years

        Me.ListView_Albums.ListViewItemSorter = Me.sorter
        Me.ListView_Albums.Sorting = SortOrder.Ascending
    End Sub

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Me.MinimumSize = Me.Size
        Me.WebBrowser1.Navigate("http://freehardmusic.com/")
    End Sub

    Private Async Sub Button_FetchAlbums_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button_FetchAlbums.Click

        Try
            If Me.RadioButton_ArtistSearch.Checked AndAlso String.IsNullOrWhiteSpace(Me.TextBoxArtist.Text) Then
                MessageBox.Show(Me, "Artist TextBox is empty.",
                                My.Application.Info.Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            Dim currentUri As Uri = Me.WebBrowser1.Url

            If currentUri Is Nothing Then
                MessageBox.Show(Me, "Browser has no valid URL.",
                                My.Application.Info.Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            Dim rawCookies As String = String.Empty
            If Me.WebBrowser1.Document IsNot Nothing AndAlso Not String.IsNullOrEmpty(Me.WebBrowser1.Document.Cookie) Then
                rawCookies = Me.WebBrowser1.Document.Cookie
            End If

            If String.IsNullOrWhiteSpace(rawCookies) Then
                MessageBox.Show(Me, $"Session cookies for FHM website not found in the browser DOM.{Environment.NewLine}{Environment.NewLine}Please select the '{Me.TabPageBrowser.Text}' tab to log in, then try again.",
                                My.Application.Info.Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            Me.cookies = New CookieContainer()
            Dim cookiePairs As String() = rawCookies.Split(";"c)
            Dim extractedUserName As String = String.Empty

            For Each pair As String In cookiePairs
                Dim parts As String() = pair.Trim().Split(New Char() {"="c}, 2)

                If parts.Length = 2 Then
                    Dim cookieName As String = parts(0).Trim()
                    Dim cookieValue As String = parts(1).Trim()

                    Dim currentCookie As New Cookie(cookieName, cookieValue) With {
                        .Domain = currentUri.Host
                    }
                    Me.cookies.Add(currentCookie)

                    If cookieName.Equals("referrerid", StringComparison.OrdinalIgnoreCase) Then
                        extractedUserName = If(cookieValue.StartsWith("RS-", StringComparison.OrdinalIgnoreCase), cookieValue.Substring(3).ToLower(), cookieValue.ToLower())
                    End If
                End If
            Next

            If String.IsNullOrWhiteSpace(extractedUserName) Then
                MessageBox.Show(Me, $"Could not dynamically extract the username from the active session.{Environment.NewLine}{Environment.NewLine}Please ensure you are fully logged in.",
                                My.Application.Info.Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            Dim usernameCookie As New Cookie("jalUserName", extractedUserName) With {
                .Domain = currentUri.Host
            }
            Me.cookies.Add(usernameCookie)

            Dim sessionFound As Boolean = False
            Dim cookieCollection As CookieCollection = Me.cookies.GetCookies(currentUri)

            For Each c As Cookie In cookieCollection
                If c.Expired Then
                    MessageBox.Show(Me, $"Session cookies for FHM website have expired.{Environment.NewLine}{Environment.NewLine}Please select the '{Me.TabPageBrowser.Text}' tab to log in, then try again.",
                                    My.Application.Info.Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If

                If c.Name.Length = 32 Then
                    sessionFound = True
                End If
            Next c

            If Not sessionFound Then
                MessageBox.Show(Me, $"Authentication session cookies not found.{Environment.NewLine}{Environment.NewLine}Please select the '{Me.TabPageBrowser.Text}' tab to log in, then try again.",
                                My.Application.Info.Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            If Me.FHMCrawler Is Nothing Then
                Me.FHMCrawler = New Crawler(Me.cookies)
            End If

            If Me.GroupBox_ArtistSearch.Enabled Then
                With Me.FHMCrawler.SearchFilter
                    .Artist = Me.TextBoxArtist.Text
                    .Country = "all"
                    .Genre = "all"
                    .Year = "all"
                End With
            Else
                With Me.FHMCrawler.SearchFilter
                    .Artist = String.Empty
                    .Country = Me.ComboBox_Country.Text
                    .Genre = Me.ComboBox_Genre.Text
                    .Year = Me.ComboBox_Year.Text
                End With
            End If

            Me.DisableControls()

            Me.Label_AlbumCount.Text = "Searching albums..."
            Me.Label_Debug.Text = "FHM website is very slow. Please wait..."

            Me.albumCount = Await Me.FHMCrawler.GetAlbumCountAsync()

            Me.ProgressBar1.Maximum = Me.albumCount
            Me.ProgressBar1.Step = 10
            Me.Button_Cancel.Enabled = True
            Me.Label_AlbumCount.Text = $"Fetching {Me.albumCount} albums..."

            Await Me.FHMCrawler.FetchAlbumsAsync()

            Me.EnableControls()

        Catch ex As Exception
            MessageBox.Show(Me, $"An unexpected error occurred during the album fetching process:{Environment.NewLine}{Environment.NewLine}{ex.Message}",
                            My.Application.Info.Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.EnableControls()
        End Try

    End Sub

    Private Sub Button_Cancel_Click(sender As Object, e As EventArgs) Handles Button_Cancel.Click
        DirectCast(sender, Button).Enabled = False
        Me.Label_AlbumCount.Text = "Cancelling operation..."
        Me.FHMCrawler.CancelFetchAlbumsAsync()
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

    Private Sub ListView_Albums_ColumnClick(sender As Object, e As ColumnClickEventArgs) _
    Handles ListView_Albums.ColumnClick

        Dim lv As ListView = DirectCast(sender, ListView)

        ' Dinamycaly sets the sort-modifier to sort the column by text, number, or date.
        Me.sorter.SortModifier = CType(lv.Columns(e.Column).Tag, SortModifiers)

        ' Determine whether clicked column is already the column that is being sorted.
        If (e.Column = Me.sorter.ColumnIndex) Then
            ' Reverse the current sort direction for this column.
            Me.sorter.Order = If(Me.sorter.Order = SortOrder.Ascending, SortOrder.Descending, SortOrder.Ascending)

        Else
            ' Set the column number that is to be sorted, default to ascending.
            Me.sorter.ColumnIndex = e.Column
            Me.sorter.Order = SortOrder.Ascending

        End If ' e.Column

        ' Perform the sort.
        lv.Sort()

    End Sub

    <DebuggerStepperBoundary>
    Private Sub FHMCrawler_BeginPageCrawl(sender As Object, e As PageCrawlBeginEventArgs) Handles FHMCrawler.BeginPageCrawl
        Dim msg As String = $"Begin crawling page index: {e.PageIndex}"
        Me.Label_Debug.Text = msg
#If DEBUG Then
        Debug.WriteLine(msg)
#End If
    End Sub

    <DebuggerStepperBoundary>
    Private Sub FHMCrawler_EndPageCrawl(sender As Object, e As PageCrawlEndEventArgs) Handles FHMCrawler.EndPageCrawl
        Dim msg As String = $"End crawling page index: {e.PageIndex}"
        Me.Label_Debug.Text = msg
#If DEBUG Then
        Debug.WriteLine(msg)
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

#End Region

#Region " Private Methods "

    Private Sub DisableControls()
        TabControlExtensions.DisableTabs(Me.TabControl1, Me.TabPageBrowser)

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
        TabControlExtensions.EnableTabs(Me.TabControl1, Me.TabPageBrowser)

        Me.Button_Cancel.Enabled = False
        Me.Label_AlbumCount.Text = String.Format("{0} albums fetched.", Me.ListView_Albums.Items.Count)
        Me.Label_Debug.Text = "..."
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

#End Region

End Class
