#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Diagnostics
Imports System.Net

Imports FHM

Imports DevCase.Core.Application.UserInterface.Enums
Imports DevCase.Core.Application.UserInterface.Types
Imports DevCase.Core.Extensions.ListView

Imports FHMCrawler.UI
Imports FHMCrawler.UI.Controls
Imports FHMCrawler.UI.Theming

#End Region

#Region " Main "

''' <summary>
''' The main window of the application.
''' </summary>
Public Class Main : Inherits System.Windows.Forms.Form

#Region " Constants "

    ''' <summary>
    ''' The address of the FHM website home page.
    ''' </summary>
    Private Const HomePageAddress As String = "http://freehardmusic.com/"

    ''' <summary>
    ''' The address of the GitHub repository of the application.
    ''' </summary>
    Private Const RepositoryAddress As String = "https://github.com/ElektroStudios/FHM-Crawler-freehardmusic.com"

#End Region

#Region " Private Fields "

    ''' <summary>
    ''' The crawler. It is created on the first search, with the session cookies of the browser.
    ''' </summary>
    Private WithEvents albumCrawler As Crawler

    ''' <summary>
    ''' The sorter of the album list.
    ''' </summary>
    Private ReadOnly sorter As ListViewColumnSorter

    ''' <summary>
    ''' The amount of albums found by the current search.
    ''' </summary>
    Private albumCount As Integer

    ''' <summary>
    ''' The session cookies of the FHM website, taken from the browser.
    ''' </summary>
    Private cookies As CookieContainer

    ''' <summary>
    ''' The current visual theme.
    ''' </summary>
    Private currentTheme As AppTheme

    ''' <summary>
    ''' A value indicating whether the theme switch is being initialized
    ''' (its change must not be saved to the user settings).
    ''' </summary>
    Private isInitializingTheme As Boolean

#End Region

#Region " Constructors "

    ''' <summary>
    ''' Initializes a new instance of the <see cref="Main"/> class.
    ''' </summary>
    Public Sub New()
        ' This call is required by the designer.
        MyClass.InitializeComponent()

        Me.sorter = New ListViewColumnSorter() With {
            .Order = SortOrder.Ascending,
            .SortModifier = SortModifiers.SortByText
        }

        Me.IconButton_GitHub.LightThemeImage = My.Resources.GitHub_Invertocat_Black
        Me.IconButton_GitHub.DarkThemeImage = My.Resources.GitHub_Invertocat_White

        ' The theme saved in the user settings; on the first run, the Windows app theme.
        Me.currentTheme = ThemeManager.ParseThemeSetting(My.Settings.Theme, ThemeManager.GetWindowsAppTheme())

        Me.isInitializingTheme = True
        Try
            Me.ToggleSwitch_DarkTheme.Checked = (Me.currentTheme = AppTheme.Dark)
        Finally
            Me.isInitializingTheme = False
        End Try

        Me.ApplyTheme(Me.currentTheme)
    End Sub

#End Region

#Region " Event Invocators "

    ''' <summary>
    ''' Raises the <see cref="Control.HandleCreated"/> event and applies the title bar colors of the current theme.
    ''' <para></para>
    ''' The title bar must be set here: from the <see cref="Form.Load"/> event the DWM attribute has no effect.
    ''' </summary>
    ''' <param name="e">
    ''' The event data.
    ''' </param>
    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        ThemeManager.SetTitleBarDarkMode(Me, Me.currentTheme = AppTheme.Dark)
    End Sub

#End Region

#Region " Event Handlers "

    ''' <summary>
    ''' Handles the <see cref="Form.Load"/> event of the <see cref="Main"/> form.
    ''' </summary>
    ''' <param name="sender">
    ''' The source of the event.
    ''' </param>
    ''' <param name="e">
    ''' The event data.
    ''' </param>
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim version As String = My.Application.Info.Version.ToString(fieldCount:=3)
        Me.Text = $"{My.Application.Info.Title} {version}"
        Me.Label_AppTitle.Text = My.Application.Info.Title
        Me.Label_AppVersion.Text = $"v{version}"

        Me.ComboBox_Country.DataSource = DataSources.countries
        Me.ComboBox_Genre.DataSource = DataSources.genres
        Me.ComboBox_Year.DataSource = DataSources.years

        Me.ListView_Albums.ListViewItemSorter = Me.sorter
        Me.ListView_Albums.Sorting = SortOrder.Ascending

        ' Create the handles of the hidden input controls now, so they are not created (and drawn blank)
        ' the first time that their search mode is selected.
        For Each ctrl As Control In New Control() {Me.TableLayoutPanel_FilterInputs, Me.ComboBox_Genre, Me.ComboBox_Country, Me.ComboBox_Year,
                                                   Me.TableLayoutPanel_ArtistInputs, Me.TextBox_Artist}
            Main.EnsureHandle(ctrl)
        Next ctrl

        Me.UpdateSearchMode()
        Me.UpdateAlbumCountLabel()
    End Sub

    ''' <summary>
    ''' Handles the <see cref="Form.Shown"/> event of the <see cref="Main"/> form.
    ''' </summary>
    ''' <param name="sender">
    ''' The source of the event.
    ''' </param>
    ''' <param name="e">
    ''' The event data.
    ''' </param>
    Private Sub Main_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Me.WebBrowser_Login.Navigate(Main.HomePageAddress)
    End Sub

    ''' <summary>
    ''' Handles the <see cref="Form.FormClosed"/> event of the <see cref="Main"/> form.
    ''' </summary>
    ''' <param name="sender">
    ''' The source of the event.
    ''' </param>
    ''' <param name="e">
    ''' The event data.
    ''' </param>
    Private Sub Main_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        ' The crawler is created on the first search.
        If Me.albumCrawler IsNot Nothing Then
            Me.albumCrawler.Dispose()
            Me.albumCrawler = Nothing
        End If
    End Sub

    ''' <summary>
    ''' Handles the <see cref="CheckBox.CheckedChanged"/> event of the dark theme switch:
    ''' applies the selected theme and saves it to the user settings.
    ''' </summary>
    ''' <param name="sender">
    ''' The source of the event.
    ''' </param>
    ''' <param name="e">
    ''' The event data.
    ''' </param>
    Private Sub ToggleSwitch_DarkTheme_CheckedChanged(sender As Object, e As EventArgs) Handles ToggleSwitch_DarkTheme.CheckedChanged
        If Me.isInitializingTheme Then
            Exit Sub
        End If

        Me.currentTheme = If(Me.ToggleSwitch_DarkTheme.Checked, AppTheme.Dark, AppTheme.Light)
        Me.ApplyTheme(Me.currentTheme)

        My.Settings.Theme = ThemeManager.ToSettingValue(Me.currentTheme)
        My.Settings.Save()
    End Sub

    ''' <summary>
    ''' Handles the click on the "Dark theme" label: toggles the theme switch.
    ''' </summary>
    ''' <param name="sender">
    ''' The source of the event.
    ''' </param>
    ''' <param name="e">
    ''' The event data.
    ''' </param>
    Private Sub Label_DarkTheme_Click(sender As Object, e As EventArgs) Handles Label_DarkTheme.Click
        Me.ToggleSwitch_DarkTheme.Checked = Not Me.ToggleSwitch_DarkTheme.Checked
    End Sub

    ''' <summary>
    ''' Handles the <see cref="RadioButton.CheckedChanged"/> event of the navigation buttons: shows the selected view.
    ''' </summary>
    ''' <param name="sender">
    ''' The source of the event.
    ''' </param>
    ''' <param name="e">
    ''' The event data.
    ''' </param>
    Private Sub SegmentButton_Navigation_CheckedChanged(sender As Object, e As EventArgs) Handles SegmentButton_NavSearch.CheckedChanged,
                                                                                                    SegmentButton_NavBrowser.CheckedChanged
        Dim showBrowser As Boolean = Me.SegmentButton_NavBrowser.Checked
        Using New RedrawScope(Me.Panel_Content)
            Me.Panel_BrowserView.Visible = showBrowser
            Me.Panel_SearchView.Visible = Not showBrowser
        End Using
    End Sub

    ''' <summary>
    ''' Handles the <see cref="RadioButton.CheckedChanged"/> event of the search mode buttons: shows the inputs of the selected mode.
    ''' </summary>
    ''' <param name="sender">
    ''' The source of the event.
    ''' </param>
    ''' <param name="e">
    ''' The event data.
    ''' </param>
    Private Sub SegmentButton_SearchMode_CheckedChanged(sender As Object, e As EventArgs) Handles SegmentButton_ModeArtist.CheckedChanged,
                                                                                                  SegmentButton_ModeFilters.CheckedChanged
        Me.UpdateSearchMode()
    End Sub

    ''' <summary>
    ''' Handles the <see cref="Control.Click"/> event of the GitHub button: opens the repository of the application in the default web browser.
    ''' </summary>
    ''' <param name="sender">
    ''' The source of the event.
    ''' </param>
    ''' <param name="e">
    ''' The event data.
    ''' </param>
    Private Sub IconButton_GitHub_Click(sender As Object, e As EventArgs) Handles IconButton_GitHub.Click
        Using Process.Start(New ProcessStartInfo(Main.RepositoryAddress) With {.UseShellExecute = True})
        End Using
    End Sub

    ''' <summary>
    ''' Handles the <see cref="Control.Click"/> event of the "Go to home page" button.
    ''' </summary>
    ''' <param name="sender">
    ''' The source of the event.
    ''' </param>
    ''' <param name="e">
    ''' The event data.
    ''' </param>
    Private Sub Button_BrowserHome_Click(sender As Object, e As EventArgs) Handles Button_BrowserHome.Click
        Me.WebBrowser_Login.Navigate(Main.HomePageAddress)
    End Sub

    ''' <summary>
    ''' Handles the <see cref="Control.Click"/> event of the "Fetch albums" button:
    ''' validates the browser session and fetches the albums of the current search.
    ''' </summary>
    ''' <param name="sender">
    ''' The source of the event.
    ''' </param>
    ''' <param name="e">
    ''' The event data.
    ''' </param>
    Private Async Sub Button_FetchAlbums_Click(sender As Object, e As EventArgs) Handles Button_FetchAlbums.Click

        Dim browserViewName As String = Me.SegmentButton_NavBrowser.Text

        Try
            If Me.SegmentButton_ModeArtist.Checked AndAlso String.IsNullOrWhiteSpace(Me.TextBox_Artist.Text) Then
                MessageBox.Show(Me, "Type the name of the artist to search.",
                                My.Application.Info.Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.TextBox_Artist.Focus()
                Exit Sub
            End If

            Dim currentUri As Uri = Me.WebBrowser_Login.Url

            If currentUri Is Nothing Then
                MessageBox.Show(Me, "Browser has no valid URL.",
                                My.Application.Info.Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            Dim rawCookies As String = String.Empty
            If Me.WebBrowser_Login.Document IsNot Nothing AndAlso Not String.IsNullOrEmpty(Me.WebBrowser_Login.Document.Cookie) Then
                rawCookies = Me.WebBrowser_Login.Document.Cookie
            End If

            If String.IsNullOrWhiteSpace(rawCookies) Then
                MessageBox.Show(Me, $"Session cookies for FHM website not found in the browser DOM.{Environment.NewLine}{Environment.NewLine}Please select the '{browserViewName}' view to log in, then try again.",
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
            Next pair

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
                    MessageBox.Show(Me, $"Session cookies for FHM website have expired.{Environment.NewLine}{Environment.NewLine}Please select the '{browserViewName}' view to log in, then try again.",
                                    My.Application.Info.Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If

                If c.Name.Length = 32 Then
                    sessionFound = True
                End If
            Next c

            If Not sessionFound Then
                MessageBox.Show(Me, $"Authentication session cookies not found.{Environment.NewLine}{Environment.NewLine}Please select the '{browserViewName}' view to log in, then try again.",
                                My.Application.Info.Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            If Me.albumCrawler Is Nothing Then
                Me.albumCrawler = New Crawler(Me.cookies)
            Else
                ' Use the current browser session, which may have changed since the previous search.
                Me.albumCrawler.Cookies = Me.cookies
            End If

            ' The search query rejects mixing an artist name with country, genre or year criteria,
            ' so the criteria of the previous search must be cleared first.
            Me.albumCrawler.ResetSearchQuery()
            If Me.SegmentButton_ModeArtist.Checked Then
                Me.albumCrawler.SearchQuery.Artist = Me.TextBox_Artist.Text.Trim()
            Else
                With Me.albumCrawler.SearchQuery
                    .Country = Me.ComboBox_Country.Text
                    .Genre = Me.ComboBox_Genre.Text
                    .Year = Me.ComboBox_Year.Text
                End With
            End If

            Me.SetSearchControlsEnabled(False)
            Me.ClearResults()

            Me.Label_AlbumCount.Text = "Searching..."
            Me.Label_Status.Text = "Searching albums. The FHM website is very slow, please wait..."

            Me.albumCount = Await Me.albumCrawler.GetAlbumCountAsync()

            Me.ProgressBar_Fetch.Reset(Me.albumCount)
            Me.Button_Cancel.Enabled = True
            Me.Label_Status.Text = $"Fetching {Me.albumCount} albums..."

            Dim completed As Boolean = Await Me.albumCrawler.FetchAlbumsAsync()

            If completed Then
                Me.ProgressBar_Fetch.Value = Me.ProgressBar_Fetch.Maximum
                Me.Label_Status.Text = $"Done. {Me.ListView_Albums.Items.Count} of {Me.albumCount} albums have download links."
            Else
                Me.ProgressBar_Fetch.State = ProgressBarState.Warning
                Me.Label_Status.Text = "The search was cancelled."
            End If

        Catch ex As Exception
            Me.ProgressBar_Fetch.State = ProgressBarState.Error
            Me.Label_Status.Text = "The search failed."
            MessageBox.Show(Me, $"An unexpected error occurred during the album fetching process:{Environment.NewLine}{Environment.NewLine}{ex.Message}",
                            My.Application.Info.Title, MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            Me.SetSearchControlsEnabled(True)
            Me.UpdateAlbumCountLabel()

        End Try

    End Sub

    ''' <summary>
    ''' Handles the <see cref="Control.Click"/> event of the "Cancel" button.
    ''' </summary>
    ''' <param name="sender">
    ''' The source of the event.
    ''' </param>
    ''' <param name="e">
    ''' The event data.
    ''' </param>
    Private Sub Button_Cancel_Click(sender As Object, e As EventArgs) Handles Button_Cancel.Click
        Me.Button_Cancel.Enabled = False
        Me.Label_Status.Text = "Cancelling the search..."
        Me.albumCrawler.CancelFetchAlbumsAsync()
    End Sub

    ''' <summary>
    ''' Handles the <see cref="Control.Click"/> event of the "Copy all as CSV" button.
    ''' </summary>
    ''' <param name="sender">
    ''' The source of the event.
    ''' </param>
    ''' <param name="e">
    ''' The event data.
    ''' </param>
    Private Sub Button_CopyAllAsCSV_Click(sender As Object, e As EventArgs) Handles Button_CopyAllAsCSV.Click
        Clipboard.SetText(Me.ListView_Albums.ExportToCSV(delimiter:=AlbumCsvFormat.Delimiter))
        Me.Label_Status.Text = $"{Me.ListView_Albums.Items.Count} albums copied to the clipboard as CSV."
    End Sub

    ''' <summary>
    ''' Handles the <see cref="Control.Click"/> event of the "Copy checked" button.
    ''' </summary>
    ''' <param name="sender">
    ''' The source of the event.
    ''' </param>
    ''' <param name="e">
    ''' The event data.
    ''' </param>
    Private Sub Button_CopyCheckedItems_Click(sender As Object, e As EventArgs) Handles Button_CopyCheckedItems.Click
        Me.ListView_Albums.CopyCheckedItems(separator:=",")
        Me.Label_Status.Text = $"{Me.ListView_Albums.CheckedItems.Count} checked albums copied to the clipboard."
    End Sub

    ''' <summary>
    ''' Handles the <see cref="Control.Click"/> event of the "Check all" button.
    ''' </summary>
    ''' <param name="sender">
    ''' The source of the event.
    ''' </param>
    ''' <param name="e">
    ''' The event data.
    ''' </param>
    Private Sub Button_CheckAll_Click(sender As Object, e As EventArgs) Handles Button_CheckAll.Click
        Me.ListView_Albums.BeginUpdate()
        For Each lvItem As ListViewItem In Me.ListView_Albums.Items
            lvItem.Checked = True
        Next lvItem
        Me.ListView_Albums.EndUpdate()
    End Sub

    ''' <summary>
    ''' Handles the <see cref="Control.Click"/> event of the "Uncheck all" button.
    ''' </summary>
    ''' <param name="sender">
    ''' The source of the event.
    ''' </param>
    ''' <param name="e">
    ''' The event data.
    ''' </param>
    Private Sub Button_UncheckAll_Click(sender As Object, e As EventArgs) Handles Button_UncheckAll.Click
        Me.ListView_Albums.BeginUpdate()
        For Each lvItem As ListViewItem In Me.ListView_Albums.CheckedItems
            lvItem.Checked = False
        Next lvItem
        Me.ListView_Albums.EndUpdate()
    End Sub

    ''' <summary>
    ''' Handles the <see cref="ListView.ColumnClick"/> event of the album list: sorts the list by the clicked column.
    ''' </summary>
    ''' <param name="sender">
    ''' The source of the event.
    ''' </param>
    ''' <param name="e">
    ''' The event data.
    ''' </param>
    Private Sub ListView_Albums_ColumnClick(sender As Object, e As ColumnClickEventArgs) Handles ListView_Albums.ColumnClick

        ' Dynamically sets the sort modifier to sort the column by text, number, or date.
        Me.sorter.SortModifier = CType(Me.ListView_Albums.Columns(e.Column).Tag, SortModifiers)

        ' Determine whether clicked column is already the column that is being sorted.
        If (e.Column = Me.sorter.ColumnIndex) Then
            ' Reverse the current sort direction for this column.
            Me.sorter.Order = If(Me.sorter.Order = SortOrder.Ascending, SortOrder.Descending, SortOrder.Ascending)

        Else
            ' Set the column number that is to be sorted, default to ascending.
            Me.sorter.ColumnIndex = e.Column
            Me.sorter.Order = SortOrder.Ascending

        End If

        ' Perform the sort, and show the sort glyph in the column header.
        Me.ListView_Albums.Sort()
        Me.ListView_Albums.SortColumnIndex = e.Column
        Me.ListView_Albums.SortOrder = Me.sorter.Order

    End Sub

    ''' <summary>
    ''' Handles the <see cref="Crawler.PageCrawlBegin"/> event.
    ''' </summary>
    ''' <param name="sender">
    ''' The source of the event.
    ''' </param>
    ''' <param name="e">
    ''' The event data.
    ''' </param>
    <DebuggerStepperBoundary>
    Private Sub AlbumCrawler_PageCrawlBegin(sender As Object, e As PageCrawlBeginEventArgs) Handles albumCrawler.PageCrawlBegin
        Dim pageCount As Integer = Crawler.GetPageCount(Me.albumCount)
        Me.Label_Status.Text = $"Crawling page {e.SearchPage + 1} of {pageCount}..."
#If DEBUG Then
        Debug.WriteLine($"Begin crawling page index: {e.SearchPage}")
#End If
    End Sub

    ''' <summary>
    ''' Handles the <see cref="Crawler.PageCrawlEnd"/> event: adds the crawled albums to the list.
    ''' </summary>
    ''' <param name="sender">
    ''' The source of the event.
    ''' </param>
    ''' <param name="e">
    ''' The event data.
    ''' </param>
    <DebuggerStepperBoundary>
    Private Sub AlbumCrawler_PageCrawlEnd(sender As Object, e As PageCrawlEndEventArgs) Handles albumCrawler.PageCrawlEnd
#If DEBUG Then
        Debug.WriteLine($"End crawling page index: {e.SearchPage}")
#End If

        Me.ListView_Albums.BeginUpdate()
        For Each albumInfo As AlbumInfo In e.Albums
            Me.ListView_Albums.Items.Add(New ListViewItem(AlbumCsvFormat.GetRowCells(albumInfo)))
        Next albumInfo
        Me.ListView_Albums.AutoSizeColumns()
        ' A page can yield no albums (e.g. all its albums lacked download links); EnsureVisible(-1) would throw.
        If Me.ListView_Albums.Items.Count > 0 Then
            Me.ListView_Albums.EnsureVisible(Me.ListView_Albums.Items.Count - 1)
        End If
        Me.ListView_Albums.EndUpdate()

        Me.ProgressBar_Fetch.Increment(Crawler.AlbumsPerPage)
        Me.UpdateAlbumCountLabel()
    End Sub

#End Region

#Region " Private Methods "

    ''' <summary>
    ''' Applies the specified theme to the window, its controls and its title bar.
    ''' </summary>
    ''' <param name="theme">
    ''' The theme to apply.
    ''' </param>
    Private Sub ApplyTheme(theme As AppTheme)
        Dim palette As ThemePalette = ThemePalette.FromTheme(theme)

        ' The drawing is suspended until every control has its new colors, then everything is redrawn at once;
        ' otherwise every control repaints itself as soon as its colors change, flashing.
        Using New RedrawScope(Me.TableLayoutPanel_Root)
            Me.SuspendLayout()
            Try
                Me.BackColor = palette.WindowBackColor
                Me.ForeColor = palette.TextColor
                ThemeManager.ApplyToControls(Me, palette)

                ' Secondary texts.
                For Each lbl As Label In {Me.Label_AppVersion, Me.Label_DarkTheme, Me.Label_AlbumCount, Me.Label_Status, Me.Label_BrowserInfo,
                                          Me.Label_ArtistCaption, Me.Label_GenreCaption, Me.Label_CountryCaption, Me.Label_YearCaption}
                    lbl.ForeColor = palette.SecondaryTextColor
                Next lbl
            Finally
                Me.ResumeLayout()
            End Try
        End Using

        If Me.IsHandleCreated Then
            ThemeManager.SetTitleBarDarkMode(Me, theme = AppTheme.Dark)
        End If
    End Sub

    ''' <summary>
    ''' Shows the inputs of the selected search mode.
    ''' </summary>
    Private Sub UpdateSearchMode()
        Dim isArtistSearch As Boolean = Me.SegmentButton_ModeArtist.Checked
        Using New RedrawScope(Me.Panel_SearchInputs)
            Me.TableLayoutPanel_ArtistInputs.Visible = isArtistSearch
            Me.TableLayoutPanel_FilterInputs.Visible = Not isArtistSearch
        End Using
    End Sub

    ''' <summary>
    ''' Creates the window handle of a control (and of its parents) if it does not exist yet,
    ''' even when the control is hidden.
    ''' </summary>
    ''' <param name="ctrl">
    ''' The control.
    ''' </param>
    Private Shared Sub EnsureHandle(ctrl As Control)
        If Not ctrl.IsHandleCreated AndAlso ctrl.Handle = IntPtr.Zero Then
            Throw New InvalidOperationException($"The window handle of '{ctrl.Name}' could not be created.")
        End If
    End Sub

    ''' <summary>
    ''' Updates the label that shows the amount of albums in the list.
    ''' </summary>
    Private Sub UpdateAlbumCountLabel()
        Dim count As Integer = Me.ListView_Albums.Items.Count
        Me.Label_AlbumCount.Text = If(count = 1, "1 album", $"{count} albums")

        Dim hasItems As Boolean = count > 0 AndAlso Not Me.Button_Cancel.Enabled
        Me.Button_CheckAll.Enabled = hasItems
        Me.Button_UncheckAll.Enabled = hasItems
        Me.Button_CopyCheckedItems.Enabled = hasItems
        Me.Button_CopyAllAsCSV.Enabled = hasItems
    End Sub

    ''' <summary>
    ''' Removes the albums of the previous search from the list.
    ''' </summary>
    Private Sub ClearResults()
        Me.sorter.Order = SortOrder.None
        Me.ListView_Albums.SortColumnIndex = -1
        Me.ListView_Albums.SortOrder = SortOrder.None

        Me.ListView_Albums.BeginUpdate()
        Me.ListView_Albums.Items.Clear()
        Me.ListView_Albums.AutoSizeColumns()
        Me.ListView_Albums.EndUpdate()

        Me.ProgressBar_Fetch.Reset(maximum:=0)
        Me.UpdateAlbumCountLabel()
    End Sub

    ''' <summary>
    ''' Enables or disables the controls that must not be used while a search is running.
    ''' </summary>
    ''' <param name="enabled">
    ''' <see langword="True"/> to enable the controls (no search is running); otherwise, <see langword="False"/>.
    ''' </param>
    Private Sub SetSearchControlsEnabled(enabled As Boolean)
        Me.SegmentButton_NavBrowser.Enabled = enabled
        Me.SegmentButton_ModeArtist.Enabled = enabled
        Me.SegmentButton_ModeFilters.Enabled = enabled
        ' Read-only instead of disabled: a disabled text box ignores its background color.
        Me.TextBox_Artist.ReadOnly = Not enabled
        Me.ComboBox_Genre.Enabled = enabled
        Me.ComboBox_Country.Enabled = enabled
        Me.ComboBox_Year.Enabled = enabled
        Me.Button_FetchAlbums.Enabled = enabled

        If enabled Then
            Me.Button_Cancel.Enabled = False
        End If
    End Sub

#End Region

End Class

#End Region
