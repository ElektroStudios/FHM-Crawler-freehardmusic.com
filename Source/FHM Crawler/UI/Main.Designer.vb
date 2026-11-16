<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Main
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.TableLayoutPanel_Root = New System.Windows.Forms.TableLayoutPanel()
        Me.CardPanel_Header = New FHMCrawler.UI.Controls.CardPanel()
        Me.TableLayoutPanel_Header = New System.Windows.Forms.TableLayoutPanel()
        Me.Label_AppTitle = New System.Windows.Forms.Label()
        Me.Label_AppVersion = New System.Windows.Forms.Label()
        Me.FlowLayoutPanel_Navigation = New System.Windows.Forms.FlowLayoutPanel()
        Me.SegmentButton_NavSearch = New FHMCrawler.UI.Controls.SegmentButton()
        Me.SegmentButton_NavBrowser = New FHMCrawler.UI.Controls.SegmentButton()
        Me.IconButton_GitHub = New FHMCrawler.UI.Controls.IconButton()
        Me.Label_DarkTheme = New System.Windows.Forms.Label()
        Me.ToggleSwitch_DarkTheme = New FHMCrawler.UI.Controls.ToggleSwitch()
        Me.Panel_Content = New System.Windows.Forms.Panel()
        Me.Panel_SearchView = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel_SearchView = New System.Windows.Forms.TableLayoutPanel()
        Me.CardPanel_Search = New FHMCrawler.UI.Controls.CardPanel()
        Me.TableLayoutPanel_Search = New System.Windows.Forms.TableLayoutPanel()
        Me.Label_SearchTitle = New System.Windows.Forms.Label()
        Me.FlowLayoutPanel_SearchMode = New System.Windows.Forms.FlowLayoutPanel()
        Me.SegmentButton_ModeArtist = New FHMCrawler.UI.Controls.SegmentButton()
        Me.SegmentButton_ModeFilters = New FHMCrawler.UI.Controls.SegmentButton()
        Me.Panel_SearchInputs = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel_FilterInputs = New System.Windows.Forms.TableLayoutPanel()
        Me.Label_GenreCaption = New System.Windows.Forms.Label()
        Me.Label_CountryCaption = New System.Windows.Forms.Label()
        Me.Label_YearCaption = New System.Windows.Forms.Label()
        Me.ComboBox_Genre = New FHMCrawler.UI.Controls.ThemedComboBox()
        Me.ComboBox_Country = New FHMCrawler.UI.Controls.ThemedComboBox()
        Me.ComboBox_Year = New FHMCrawler.UI.Controls.ThemedComboBox()
        Me.TableLayoutPanel_ArtistInputs = New System.Windows.Forms.TableLayoutPanel()
        Me.Label_ArtistCaption = New System.Windows.Forms.Label()
        Me.TextBox_Artist = New FHMCrawler.UI.Controls.ThemedTextBox()
        Me.FlowLayoutPanel_SearchActions = New System.Windows.Forms.FlowLayoutPanel()
        Me.Button_FetchAlbums = New FHMCrawler.UI.Controls.ThemedButton()
        Me.Button_Cancel = New FHMCrawler.UI.Controls.ThemedButton()
        Me.CardPanel_Results = New FHMCrawler.UI.Controls.CardPanel()
        Me.TableLayoutPanel_Results = New System.Windows.Forms.TableLayoutPanel()
        Me.CardPanel_ResultsToolbar = New FHMCrawler.UI.Controls.CardPanel()
        Me.TableLayoutPanel_ResultsToolbar = New System.Windows.Forms.TableLayoutPanel()
        Me.Label_ResultsTitle = New System.Windows.Forms.Label()
        Me.Label_AlbumCount = New System.Windows.Forms.Label()
        Me.FlowLayoutPanel_ResultsActions = New System.Windows.Forms.FlowLayoutPanel()
        Me.Button_CheckAll = New FHMCrawler.UI.Controls.ThemedButton()
        Me.Button_UncheckAll = New FHMCrawler.UI.Controls.ThemedButton()
        Me.Button_CopyCheckedItems = New FHMCrawler.UI.Controls.ThemedButton()
        Me.Button_CopyAllAsCSV = New FHMCrawler.UI.Controls.ThemedButton()
        Me.ListView_Albums = New FHMCrawler.UI.Controls.ThemedListView()
        Me.ColumnHeader_Artist = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader_Album = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader_DownloadLinks = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Panel_BrowserView = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel_BrowserView = New System.Windows.Forms.TableLayoutPanel()
        Me.CardPanel_BrowserInfo = New FHMCrawler.UI.Controls.CardPanel()
        Me.TableLayoutPanel_BrowserInfo = New System.Windows.Forms.TableLayoutPanel()
        Me.Label_BrowserInfo = New System.Windows.Forms.Label()
        Me.Button_BrowserHome = New FHMCrawler.UI.Controls.ThemedButton()
        Me.CardPanel_Browser = New FHMCrawler.UI.Controls.CardPanel()
        Me.WebBrowser_Login = New System.Windows.Forms.WebBrowser()
        Me.CardPanel_StatusBar = New FHMCrawler.UI.Controls.CardPanel()
        Me.TableLayoutPanel_StatusBar = New System.Windows.Forms.TableLayoutPanel()
        Me.Label_Status = New System.Windows.Forms.Label()
        Me.ProgressBar_Fetch = New FHMCrawler.UI.Controls.FlatProgressBar()
        Me.ToolTip_Main = New System.Windows.Forms.ToolTip(Me.components)
        Me.TableLayoutPanel_Root.SuspendLayout()
        Me.CardPanel_Header.SuspendLayout()
        Me.TableLayoutPanel_Header.SuspendLayout()
        Me.FlowLayoutPanel_Navigation.SuspendLayout()
        Me.Panel_Content.SuspendLayout()
        Me.Panel_SearchView.SuspendLayout()
        Me.TableLayoutPanel_SearchView.SuspendLayout()
        Me.CardPanel_Search.SuspendLayout()
        Me.TableLayoutPanel_Search.SuspendLayout()
        Me.FlowLayoutPanel_SearchMode.SuspendLayout()
        Me.Panel_SearchInputs.SuspendLayout()
        Me.TableLayoutPanel_FilterInputs.SuspendLayout()
        Me.TableLayoutPanel_ArtistInputs.SuspendLayout()
        Me.FlowLayoutPanel_SearchActions.SuspendLayout()
        Me.CardPanel_Results.SuspendLayout()
        Me.TableLayoutPanel_Results.SuspendLayout()
        Me.CardPanel_ResultsToolbar.SuspendLayout()
        Me.TableLayoutPanel_ResultsToolbar.SuspendLayout()
        Me.FlowLayoutPanel_ResultsActions.SuspendLayout()
        Me.Panel_BrowserView.SuspendLayout()
        Me.TableLayoutPanel_BrowserView.SuspendLayout()
        Me.CardPanel_BrowserInfo.SuspendLayout()
        Me.TableLayoutPanel_BrowserInfo.SuspendLayout()
        Me.CardPanel_Browser.SuspendLayout()
        Me.CardPanel_StatusBar.SuspendLayout()
        Me.TableLayoutPanel_StatusBar.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel_Root
        '
        Me.TableLayoutPanel_Root.ColumnCount = 1
        Me.TableLayoutPanel_Root.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_Root.Controls.Add(Me.CardPanel_Header, 0, 0)
        Me.TableLayoutPanel_Root.Controls.Add(Me.Panel_Content, 0, 1)
        Me.TableLayoutPanel_Root.Controls.Add(Me.CardPanel_StatusBar, 0, 2)
        Me.TableLayoutPanel_Root.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel_Root.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel_Root.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel_Root.Name = "TableLayoutPanel_Root"
        Me.TableLayoutPanel_Root.RowCount = 3
        Me.TableLayoutPanel_Root.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56.0!))
        Me.TableLayoutPanel_Root.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_Root.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34.0!))
        Me.TableLayoutPanel_Root.Size = New System.Drawing.Size(1000, 680)
        Me.TableLayoutPanel_Root.TabIndex = 0
        '
        'CardPanel_Header
        '
        Me.CardPanel_Header.BorderSides = FHMCrawler.UI.Controls.BorderSides.Bottom
        Me.CardPanel_Header.Controls.Add(Me.TableLayoutPanel_Header)
        Me.CardPanel_Header.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CardPanel_Header.Location = New System.Drawing.Point(0, 0)
        Me.CardPanel_Header.Margin = New System.Windows.Forms.Padding(0)
        Me.CardPanel_Header.Name = "CardPanel_Header"
        Me.CardPanel_Header.Padding = New System.Windows.Forms.Padding(16, 0, 16, 1)
        Me.CardPanel_Header.Size = New System.Drawing.Size(1000, 56)
        Me.CardPanel_Header.TabIndex = 0
        '
        'TableLayoutPanel_Header
        '
        Me.TableLayoutPanel_Header.ColumnCount = 8
        Me.TableLayoutPanel_Header.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel_Header.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel_Header.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
        Me.TableLayoutPanel_Header.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel_Header.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_Header.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel_Header.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel_Header.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel_Header.Controls.Add(Me.Label_AppTitle, 0, 0)
        Me.TableLayoutPanel_Header.Controls.Add(Me.Label_AppVersion, 1, 0)
        Me.TableLayoutPanel_Header.Controls.Add(Me.FlowLayoutPanel_Navigation, 3, 0)
        Me.TableLayoutPanel_Header.Controls.Add(Me.IconButton_GitHub, 5, 0)
        Me.TableLayoutPanel_Header.Controls.Add(Me.Label_DarkTheme, 6, 0)
        Me.TableLayoutPanel_Header.Controls.Add(Me.ToggleSwitch_DarkTheme, 7, 0)
        Me.TableLayoutPanel_Header.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel_Header.Location = New System.Drawing.Point(16, 0)
        Me.TableLayoutPanel_Header.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel_Header.Name = "TableLayoutPanel_Header"
        Me.TableLayoutPanel_Header.RowCount = 1
        Me.TableLayoutPanel_Header.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_Header.Size = New System.Drawing.Size(968, 55)
        Me.TableLayoutPanel_Header.TabIndex = 0
        '
        'Label_AppTitle
        '
        Me.Label_AppTitle.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label_AppTitle.AutoSize = True
        Me.Label_AppTitle.Font = New System.Drawing.Font("Segoe UI Semibold", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_AppTitle.Location = New System.Drawing.Point(0, 14)
        Me.Label_AppTitle.Margin = New System.Windows.Forms.Padding(0)
        Me.Label_AppTitle.Name = "Label_AppTitle"
        Me.Label_AppTitle.Size = New System.Drawing.Size(119, 25)
        Me.Label_AppTitle.TabIndex = 0
        Me.Label_AppTitle.Text = "FHM Crawler"
        '
        'Label_AppVersion
        '
        Me.Label_AppVersion.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label_AppVersion.AutoSize = True
        Me.Label_AppVersion.Location = New System.Drawing.Point(125, 19)
        Me.Label_AppVersion.Margin = New System.Windows.Forms.Padding(6, 4, 0, 0)
        Me.Label_AppVersion.Name = "Label_AppVersion"
        Me.Label_AppVersion.Size = New System.Drawing.Size(40, 17)
        Me.Label_AppVersion.TabIndex = 1
        Me.Label_AppVersion.Text = "v0.0.0"
        '
        'FlowLayoutPanel_Navigation
        '
        Me.FlowLayoutPanel_Navigation.AutoSize = True
        Me.FlowLayoutPanel_Navigation.Controls.Add(Me.SegmentButton_NavSearch)
        Me.FlowLayoutPanel_Navigation.Controls.Add(Me.SegmentButton_NavBrowser)
        Me.FlowLayoutPanel_Navigation.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel_Navigation.Location = New System.Drawing.Point(193, 0)
        Me.FlowLayoutPanel_Navigation.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel_Navigation.Name = "FlowLayoutPanel_Navigation"
        Me.FlowLayoutPanel_Navigation.Size = New System.Drawing.Size(256, 55)
        Me.FlowLayoutPanel_Navigation.TabIndex = 2
        Me.FlowLayoutPanel_Navigation.WrapContents = False
        '
        'SegmentButton_NavSearch
        '
        Me.SegmentButton_NavSearch.Checked = True
        Me.SegmentButton_NavSearch.Location = New System.Drawing.Point(0, 0)
        Me.SegmentButton_NavSearch.Margin = New System.Windows.Forms.Padding(0)
        Me.SegmentButton_NavSearch.Name = "SegmentButton_NavSearch"
        Me.SegmentButton_NavSearch.Size = New System.Drawing.Size(96, 55)
        Me.SegmentButton_NavSearch.TabIndex = 0
        Me.SegmentButton_NavSearch.TabStop = True
        Me.SegmentButton_NavSearch.Text = "Search"
        '
        'SegmentButton_NavBrowser
        '
        Me.SegmentButton_NavBrowser.Location = New System.Drawing.Point(96, 0)
        Me.SegmentButton_NavBrowser.Margin = New System.Windows.Forms.Padding(0)
        Me.SegmentButton_NavBrowser.Name = "SegmentButton_NavBrowser"
        Me.SegmentButton_NavBrowser.Size = New System.Drawing.Size(160, 55)
        Me.SegmentButton_NavBrowser.TabIndex = 1
        Me.SegmentButton_NavBrowser.Text = "Browser (log in)"
        '
        'IconButton_GitHub
        '
        Me.IconButton_GitHub.AccessibleName = "GitHub repository"
        Me.IconButton_GitHub.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.IconButton_GitHub.Location = New System.Drawing.Point(792, 11)
        Me.IconButton_GitHub.Margin = New System.Windows.Forms.Padding(0, 0, 16, 0)
        Me.IconButton_GitHub.Name = "IconButton_GitHub"
        Me.IconButton_GitHub.Size = New System.Drawing.Size(32, 32)
        Me.IconButton_GitHub.TabIndex = 3
        Me.ToolTip_Main.SetToolTip(Me.IconButton_GitHub, "Open the GitHub repository of FHM Crawler")
        '
        'Label_DarkTheme
        '
        Me.Label_DarkTheme.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label_DarkTheme.AutoSize = True
        Me.Label_DarkTheme.Location = New System.Drawing.Point(840, 19)
        Me.Label_DarkTheme.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.Label_DarkTheme.Name = "Label_DarkTheme"
        Me.Label_DarkTheme.Size = New System.Drawing.Size(76, 17)
        Me.Label_DarkTheme.TabIndex = 4
        Me.Label_DarkTheme.Text = "Dark theme"
        '
        'ToggleSwitch_DarkTheme
        '
        Me.ToggleSwitch_DarkTheme.AccessibleName = "Dark theme"
        Me.ToggleSwitch_DarkTheme.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.ToggleSwitch_DarkTheme.Location = New System.Drawing.Point(924, 15)
        Me.ToggleSwitch_DarkTheme.Margin = New System.Windows.Forms.Padding(0)
        Me.ToggleSwitch_DarkTheme.Name = "ToggleSwitch_DarkTheme"
        Me.ToggleSwitch_DarkTheme.Size = New System.Drawing.Size(44, 24)
        Me.ToggleSwitch_DarkTheme.TabIndex = 5
        '
        'Panel_Content
        '
        Me.Panel_Content.Controls.Add(Me.Panel_SearchView)
        Me.Panel_Content.Controls.Add(Me.Panel_BrowserView)
        Me.Panel_Content.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel_Content.Location = New System.Drawing.Point(0, 56)
        Me.Panel_Content.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel_Content.Name = "Panel_Content"
        Me.Panel_Content.Size = New System.Drawing.Size(1000, 590)
        Me.Panel_Content.TabIndex = 1
        '
        'Panel_SearchView
        '
        Me.Panel_SearchView.Controls.Add(Me.TableLayoutPanel_SearchView)
        Me.Panel_SearchView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel_SearchView.Location = New System.Drawing.Point(0, 0)
        Me.Panel_SearchView.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel_SearchView.Name = "Panel_SearchView"
        Me.Panel_SearchView.Padding = New System.Windows.Forms.Padding(16, 16, 16, 12)
        Me.Panel_SearchView.Size = New System.Drawing.Size(1000, 590)
        Me.Panel_SearchView.TabIndex = 0
        '
        'TableLayoutPanel_SearchView
        '
        Me.TableLayoutPanel_SearchView.ColumnCount = 1
        Me.TableLayoutPanel_SearchView.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_SearchView.Controls.Add(Me.CardPanel_Search, 0, 0)
        Me.TableLayoutPanel_SearchView.Controls.Add(Me.CardPanel_Results, 0, 2)
        Me.TableLayoutPanel_SearchView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel_SearchView.Location = New System.Drawing.Point(16, 16)
        Me.TableLayoutPanel_SearchView.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel_SearchView.Name = "TableLayoutPanel_SearchView"
        Me.TableLayoutPanel_SearchView.RowCount = 3
        Me.TableLayoutPanel_SearchView.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 128.0!))
        Me.TableLayoutPanel_SearchView.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12.0!))
        Me.TableLayoutPanel_SearchView.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_SearchView.Size = New System.Drawing.Size(968, 562)
        Me.TableLayoutPanel_SearchView.TabIndex = 0
        '
        'CardPanel_Search
        '
        Me.CardPanel_Search.Controls.Add(Me.TableLayoutPanel_Search)
        Me.CardPanel_Search.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CardPanel_Search.Location = New System.Drawing.Point(0, 0)
        Me.CardPanel_Search.Margin = New System.Windows.Forms.Padding(0)
        Me.CardPanel_Search.Name = "CardPanel_Search"
        Me.CardPanel_Search.Padding = New System.Windows.Forms.Padding(16, 10, 16, 14)
        Me.CardPanel_Search.Size = New System.Drawing.Size(968, 128)
        Me.CardPanel_Search.TabIndex = 0
        '
        'TableLayoutPanel_Search
        '
        Me.TableLayoutPanel_Search.ColumnCount = 2
        Me.TableLayoutPanel_Search.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_Search.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel_Search.Controls.Add(Me.Label_SearchTitle, 0, 0)
        Me.TableLayoutPanel_Search.Controls.Add(Me.FlowLayoutPanel_SearchMode, 1, 0)
        Me.TableLayoutPanel_Search.Controls.Add(Me.Panel_SearchInputs, 0, 1)
        Me.TableLayoutPanel_Search.Controls.Add(Me.FlowLayoutPanel_SearchActions, 1, 1)
        Me.TableLayoutPanel_Search.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel_Search.Location = New System.Drawing.Point(16, 10)
        Me.TableLayoutPanel_Search.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel_Search.Name = "TableLayoutPanel_Search"
        Me.TableLayoutPanel_Search.RowCount = 2
        Me.TableLayoutPanel_Search.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42.0!))
        Me.TableLayoutPanel_Search.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_Search.Size = New System.Drawing.Size(936, 104)
        Me.TableLayoutPanel_Search.TabIndex = 0
        '
        'Label_SearchTitle
        '
        Me.Label_SearchTitle.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label_SearchTitle.AutoSize = True
        Me.Label_SearchTitle.Font = New System.Drawing.Font("Segoe UI Semibold", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_SearchTitle.Location = New System.Drawing.Point(0, 11)
        Me.Label_SearchTitle.Margin = New System.Windows.Forms.Padding(0)
        Me.Label_SearchTitle.Name = "Label_SearchTitle"
        Me.Label_SearchTitle.Size = New System.Drawing.Size(105, 20)
        Me.Label_SearchTitle.TabIndex = 0
        Me.Label_SearchTitle.Text = "Search albums"
        '
        'FlowLayoutPanel_SearchMode
        '
        Me.FlowLayoutPanel_SearchMode.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.FlowLayoutPanel_SearchMode.AutoSize = True
        Me.FlowLayoutPanel_SearchMode.Controls.Add(Me.SegmentButton_ModeArtist)
        Me.FlowLayoutPanel_SearchMode.Controls.Add(Me.SegmentButton_ModeFilters)
        Me.FlowLayoutPanel_SearchMode.Location = New System.Drawing.Point(660, 6)
        Me.FlowLayoutPanel_SearchMode.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel_SearchMode.Name = "FlowLayoutPanel_SearchMode"
        Me.FlowLayoutPanel_SearchMode.Size = New System.Drawing.Size(276, 30)
        Me.FlowLayoutPanel_SearchMode.TabIndex = 1
        Me.FlowLayoutPanel_SearchMode.WrapContents = False
        '
        'SegmentButton_ModeArtist
        '
        Me.SegmentButton_ModeArtist.Checked = True
        Me.SegmentButton_ModeArtist.Location = New System.Drawing.Point(0, 0)
        Me.SegmentButton_ModeArtist.Margin = New System.Windows.Forms.Padding(0)
        Me.SegmentButton_ModeArtist.Name = "SegmentButton_ModeArtist"
        Me.SegmentButton_ModeArtist.SegmentStyle = FHMCrawler.UI.Controls.SegmentButtonStyle.Filled
        Me.SegmentButton_ModeArtist.Size = New System.Drawing.Size(96, 30)
        Me.SegmentButton_ModeArtist.TabIndex = 0
        Me.SegmentButton_ModeArtist.TabStop = True
        Me.SegmentButton_ModeArtist.Text = "Artist"
        '
        'SegmentButton_ModeFilters
        '
        Me.SegmentButton_ModeFilters.Location = New System.Drawing.Point(96, 0)
        Me.SegmentButton_ModeFilters.Margin = New System.Windows.Forms.Padding(0)
        Me.SegmentButton_ModeFilters.Name = "SegmentButton_ModeFilters"
        Me.SegmentButton_ModeFilters.SegmentStyle = FHMCrawler.UI.Controls.SegmentButtonStyle.Filled
        Me.SegmentButton_ModeFilters.Size = New System.Drawing.Size(180, 30)
        Me.SegmentButton_ModeFilters.TabIndex = 1
        Me.SegmentButton_ModeFilters.Text = "Genre · Country · Year"
        '
        'Panel_SearchInputs
        '
        Me.Panel_SearchInputs.Controls.Add(Me.TableLayoutPanel_ArtistInputs)
        Me.Panel_SearchInputs.Controls.Add(Me.TableLayoutPanel_FilterInputs)
        Me.Panel_SearchInputs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel_SearchInputs.Location = New System.Drawing.Point(0, 42)
        Me.Panel_SearchInputs.Margin = New System.Windows.Forms.Padding(0, 0, 24, 0)
        Me.Panel_SearchInputs.Name = "Panel_SearchInputs"
        Me.Panel_SearchInputs.Size = New System.Drawing.Size(636, 62)
        Me.Panel_SearchInputs.TabIndex = 2
        '
        'TableLayoutPanel_FilterInputs
        '
        Me.TableLayoutPanel_FilterInputs.ColumnCount = 3
        Me.TableLayoutPanel_FilterInputs.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.TableLayoutPanel_FilterInputs.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.TableLayoutPanel_FilterInputs.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel_FilterInputs.Controls.Add(Me.Label_GenreCaption, 0, 0)
        Me.TableLayoutPanel_FilterInputs.Controls.Add(Me.Label_CountryCaption, 1, 0)
        Me.TableLayoutPanel_FilterInputs.Controls.Add(Me.Label_YearCaption, 2, 0)
        Me.TableLayoutPanel_FilterInputs.Controls.Add(Me.ComboBox_Genre, 0, 1)
        Me.TableLayoutPanel_FilterInputs.Controls.Add(Me.ComboBox_Country, 1, 1)
        Me.TableLayoutPanel_FilterInputs.Controls.Add(Me.ComboBox_Year, 2, 1)
        Me.TableLayoutPanel_FilterInputs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel_FilterInputs.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel_FilterInputs.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel_FilterInputs.Name = "TableLayoutPanel_FilterInputs"
        Me.TableLayoutPanel_FilterInputs.RowCount = 2
        Me.TableLayoutPanel_FilterInputs.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.TableLayoutPanel_FilterInputs.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_FilterInputs.Size = New System.Drawing.Size(636, 62)
        Me.TableLayoutPanel_FilterInputs.TabIndex = 1
        Me.TableLayoutPanel_FilterInputs.Visible = False
        '
        'Label_GenreCaption
        '
        Me.Label_GenreCaption.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label_GenreCaption.AutoSize = True
        Me.Label_GenreCaption.Location = New System.Drawing.Point(0, 3)
        Me.Label_GenreCaption.Margin = New System.Windows.Forms.Padding(0)
        Me.Label_GenreCaption.Name = "Label_GenreCaption"
        Me.Label_GenreCaption.Size = New System.Drawing.Size(43, 17)
        Me.Label_GenreCaption.TabIndex = 0
        Me.Label_GenreCaption.Text = "Genre"
        '
        'Label_CountryCaption
        '
        Me.Label_CountryCaption.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label_CountryCaption.AutoSize = True
        Me.Label_CountryCaption.Location = New System.Drawing.Point(254, 3)
        Me.Label_CountryCaption.Margin = New System.Windows.Forms.Padding(0)
        Me.Label_CountryCaption.Name = "Label_CountryCaption"
        Me.Label_CountryCaption.Size = New System.Drawing.Size(53, 17)
        Me.Label_CountryCaption.TabIndex = 2
        Me.Label_CountryCaption.Text = "Country"
        '
        'Label_YearCaption
        '
        Me.Label_YearCaption.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label_YearCaption.AutoSize = True
        Me.Label_YearCaption.Location = New System.Drawing.Point(508, 3)
        Me.Label_YearCaption.Margin = New System.Windows.Forms.Padding(0)
        Me.Label_YearCaption.Name = "Label_YearCaption"
        Me.Label_YearCaption.Size = New System.Drawing.Size(31, 17)
        Me.Label_YearCaption.TabIndex = 4
        Me.Label_YearCaption.Text = "Year"
        '
        'ComboBox_Genre
        '
        Me.ComboBox_Genre.Dock = System.Windows.Forms.DockStyle.Top
        Me.ComboBox_Genre.Location = New System.Drawing.Point(0, 26)
        Me.ComboBox_Genre.Margin = New System.Windows.Forms.Padding(0, 2, 12, 0)
        Me.ComboBox_Genre.Name = "ComboBox_Genre"
        Me.ComboBox_Genre.Size = New System.Drawing.Size(242, 26)
        Me.ComboBox_Genre.TabIndex = 1
        '
        'ComboBox_Country
        '
        Me.ComboBox_Country.Dock = System.Windows.Forms.DockStyle.Top
        Me.ComboBox_Country.Location = New System.Drawing.Point(254, 26)
        Me.ComboBox_Country.Margin = New System.Windows.Forms.Padding(0, 2, 12, 0)
        Me.ComboBox_Country.Name = "ComboBox_Country"
        Me.ComboBox_Country.Size = New System.Drawing.Size(242, 26)
        Me.ComboBox_Country.TabIndex = 3
        '
        'ComboBox_Year
        '
        Me.ComboBox_Year.Dock = System.Windows.Forms.DockStyle.Top
        Me.ComboBox_Year.Location = New System.Drawing.Point(508, 26)
        Me.ComboBox_Year.Margin = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.ComboBox_Year.Name = "ComboBox_Year"
        Me.ComboBox_Year.Size = New System.Drawing.Size(128, 26)
        Me.ComboBox_Year.TabIndex = 5
        '
        'TableLayoutPanel_ArtistInputs
        '
        Me.TableLayoutPanel_ArtistInputs.ColumnCount = 1
        Me.TableLayoutPanel_ArtistInputs.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_ArtistInputs.Controls.Add(Me.Label_ArtistCaption, 0, 0)
        Me.TableLayoutPanel_ArtistInputs.Controls.Add(Me.TextBox_Artist, 0, 1)
        Me.TableLayoutPanel_ArtistInputs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel_ArtistInputs.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel_ArtistInputs.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel_ArtistInputs.Name = "TableLayoutPanel_ArtistInputs"
        Me.TableLayoutPanel_ArtistInputs.RowCount = 2
        Me.TableLayoutPanel_ArtistInputs.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.TableLayoutPanel_ArtistInputs.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_ArtistInputs.Size = New System.Drawing.Size(636, 62)
        Me.TableLayoutPanel_ArtistInputs.TabIndex = 0
        '
        'Label_ArtistCaption
        '
        Me.Label_ArtistCaption.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label_ArtistCaption.AutoSize = True
        Me.Label_ArtistCaption.Location = New System.Drawing.Point(0, 3)
        Me.Label_ArtistCaption.Margin = New System.Windows.Forms.Padding(0)
        Me.Label_ArtistCaption.Name = "Label_ArtistCaption"
        Me.Label_ArtistCaption.Size = New System.Drawing.Size(79, 17)
        Me.Label_ArtistCaption.TabIndex = 0
        Me.Label_ArtistCaption.Text = "Artist name"
        '
        'TextBox_Artist
        '
        Me.TextBox_Artist.CueBanner = "Band or artist name, e.g. Iron Maiden"
        Me.TextBox_Artist.Dock = System.Windows.Forms.DockStyle.Top
        Me.TextBox_Artist.Location = New System.Drawing.Point(0, 24)
        Me.TextBox_Artist.Margin = New System.Windows.Forms.Padding(0)
        Me.TextBox_Artist.Name = "TextBox_Artist"
        Me.TextBox_Artist.Size = New System.Drawing.Size(636, 30)
        Me.TextBox_Artist.TabIndex = 1
        '
        'FlowLayoutPanel_SearchActions
        '
        Me.FlowLayoutPanel_SearchActions.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FlowLayoutPanel_SearchActions.AutoSize = True
        Me.FlowLayoutPanel_SearchActions.Controls.Add(Me.Button_FetchAlbums)
        Me.FlowLayoutPanel_SearchActions.Controls.Add(Me.Button_Cancel)
        Me.FlowLayoutPanel_SearchActions.Location = New System.Drawing.Point(698, 66)
        Me.FlowLayoutPanel_SearchActions.Margin = New System.Windows.Forms.Padding(0, 24, 0, 0)
        Me.FlowLayoutPanel_SearchActions.Name = "FlowLayoutPanel_SearchActions"
        Me.FlowLayoutPanel_SearchActions.Size = New System.Drawing.Size(238, 30)
        Me.FlowLayoutPanel_SearchActions.TabIndex = 3
        Me.FlowLayoutPanel_SearchActions.WrapContents = False
        '
        'Button_FetchAlbums
        '
        Me.Button_FetchAlbums.IsPrimary = True
        Me.Button_FetchAlbums.Location = New System.Drawing.Point(0, 0)
        Me.Button_FetchAlbums.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.Button_FetchAlbums.Name = "Button_FetchAlbums"
        Me.Button_FetchAlbums.Size = New System.Drawing.Size(134, 30)
        Me.Button_FetchAlbums.TabIndex = 0
        Me.Button_FetchAlbums.Text = "Fetch albums"
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Enabled = False
        Me.Button_Cancel.Location = New System.Drawing.Point(142, 0)
        Me.Button_Cancel.Margin = New System.Windows.Forms.Padding(0)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(96, 30)
        Me.Button_Cancel.TabIndex = 1
        Me.Button_Cancel.Text = "Cancel"
        '
        'CardPanel_Results
        '
        Me.CardPanel_Results.Controls.Add(Me.TableLayoutPanel_Results)
        Me.CardPanel_Results.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CardPanel_Results.Location = New System.Drawing.Point(0, 140)
        Me.CardPanel_Results.Margin = New System.Windows.Forms.Padding(0)
        Me.CardPanel_Results.Name = "CardPanel_Results"
        Me.CardPanel_Results.Padding = New System.Windows.Forms.Padding(1)
        Me.CardPanel_Results.Size = New System.Drawing.Size(968, 422)
        Me.CardPanel_Results.TabIndex = 1
        '
        'TableLayoutPanel_Results
        '
        Me.TableLayoutPanel_Results.ColumnCount = 1
        Me.TableLayoutPanel_Results.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_Results.Controls.Add(Me.CardPanel_ResultsToolbar, 0, 0)
        Me.TableLayoutPanel_Results.Controls.Add(Me.ListView_Albums, 0, 1)
        Me.TableLayoutPanel_Results.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel_Results.Location = New System.Drawing.Point(1, 1)
        Me.TableLayoutPanel_Results.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel_Results.Name = "TableLayoutPanel_Results"
        Me.TableLayoutPanel_Results.RowCount = 2
        Me.TableLayoutPanel_Results.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        Me.TableLayoutPanel_Results.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_Results.Size = New System.Drawing.Size(966, 420)
        Me.TableLayoutPanel_Results.TabIndex = 0
        '
        'CardPanel_ResultsToolbar
        '
        Me.CardPanel_ResultsToolbar.BorderSides = FHMCrawler.UI.Controls.BorderSides.Bottom
        Me.CardPanel_ResultsToolbar.Controls.Add(Me.TableLayoutPanel_ResultsToolbar)
        Me.CardPanel_ResultsToolbar.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CardPanel_ResultsToolbar.Location = New System.Drawing.Point(0, 0)
        Me.CardPanel_ResultsToolbar.Margin = New System.Windows.Forms.Padding(0)
        Me.CardPanel_ResultsToolbar.Name = "CardPanel_ResultsToolbar"
        Me.CardPanel_ResultsToolbar.Padding = New System.Windows.Forms.Padding(15, 0, 12, 1)
        Me.CardPanel_ResultsToolbar.Size = New System.Drawing.Size(966, 50)
        Me.CardPanel_ResultsToolbar.TabIndex = 0
        '
        'TableLayoutPanel_ResultsToolbar
        '
        Me.TableLayoutPanel_ResultsToolbar.ColumnCount = 4
        Me.TableLayoutPanel_ResultsToolbar.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel_ResultsToolbar.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel_ResultsToolbar.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_ResultsToolbar.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel_ResultsToolbar.Controls.Add(Me.Label_ResultsTitle, 0, 0)
        Me.TableLayoutPanel_ResultsToolbar.Controls.Add(Me.Label_AlbumCount, 1, 0)
        Me.TableLayoutPanel_ResultsToolbar.Controls.Add(Me.FlowLayoutPanel_ResultsActions, 3, 0)
        Me.TableLayoutPanel_ResultsToolbar.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel_ResultsToolbar.Location = New System.Drawing.Point(15, 0)
        Me.TableLayoutPanel_ResultsToolbar.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel_ResultsToolbar.Name = "TableLayoutPanel_ResultsToolbar"
        Me.TableLayoutPanel_ResultsToolbar.RowCount = 1
        Me.TableLayoutPanel_ResultsToolbar.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_ResultsToolbar.Size = New System.Drawing.Size(939, 49)
        Me.TableLayoutPanel_ResultsToolbar.TabIndex = 0
        '
        'Label_ResultsTitle
        '
        Me.Label_ResultsTitle.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label_ResultsTitle.AutoSize = True
        Me.Label_ResultsTitle.Font = New System.Drawing.Font("Segoe UI Semibold", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_ResultsTitle.Location = New System.Drawing.Point(0, 14)
        Me.Label_ResultsTitle.Margin = New System.Windows.Forms.Padding(0)
        Me.Label_ResultsTitle.Name = "Label_ResultsTitle"
        Me.Label_ResultsTitle.Size = New System.Drawing.Size(55, 20)
        Me.Label_ResultsTitle.TabIndex = 0
        Me.Label_ResultsTitle.Text = "Results"
        '
        'Label_AlbumCount
        '
        Me.Label_AlbumCount.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label_AlbumCount.AutoSize = True
        Me.Label_AlbumCount.Location = New System.Drawing.Point(65, 16)
        Me.Label_AlbumCount.Margin = New System.Windows.Forms.Padding(10, 2, 0, 0)
        Me.Label_AlbumCount.Name = "Label_AlbumCount"
        Me.Label_AlbumCount.Size = New System.Drawing.Size(56, 17)
        Me.Label_AlbumCount.TabIndex = 1
        Me.Label_AlbumCount.Text = "0 albums"
        '
        'FlowLayoutPanel_ResultsActions
        '
        Me.FlowLayoutPanel_ResultsActions.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.FlowLayoutPanel_ResultsActions.AutoSize = True
        Me.FlowLayoutPanel_ResultsActions.Controls.Add(Me.Button_CheckAll)
        Me.FlowLayoutPanel_ResultsActions.Controls.Add(Me.Button_UncheckAll)
        Me.FlowLayoutPanel_ResultsActions.Controls.Add(Me.Button_CopyCheckedItems)
        Me.FlowLayoutPanel_ResultsActions.Controls.Add(Me.Button_CopyAllAsCSV)
        Me.FlowLayoutPanel_ResultsActions.Location = New System.Drawing.Point(477, 10)
        Me.FlowLayoutPanel_ResultsActions.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel_ResultsActions.Name = "FlowLayoutPanel_ResultsActions"
        Me.FlowLayoutPanel_ResultsActions.Size = New System.Drawing.Size(462, 28)
        Me.FlowLayoutPanel_ResultsActions.TabIndex = 2
        Me.FlowLayoutPanel_ResultsActions.WrapContents = False
        '
        'Button_CheckAll
        '
        Me.Button_CheckAll.Enabled = False
        Me.Button_CheckAll.Location = New System.Drawing.Point(0, 0)
        Me.Button_CheckAll.Margin = New System.Windows.Forms.Padding(0, 0, 6, 0)
        Me.Button_CheckAll.Name = "Button_CheckAll"
        Me.Button_CheckAll.Size = New System.Drawing.Size(90, 28)
        Me.Button_CheckAll.TabIndex = 0
        Me.Button_CheckAll.Text = "Check all"
        '
        'Button_UncheckAll
        '
        Me.Button_UncheckAll.Enabled = False
        Me.Button_UncheckAll.Location = New System.Drawing.Point(96, 0)
        Me.Button_UncheckAll.Margin = New System.Windows.Forms.Padding(0, 0, 6, 0)
        Me.Button_UncheckAll.Name = "Button_UncheckAll"
        Me.Button_UncheckAll.Size = New System.Drawing.Size(100, 28)
        Me.Button_UncheckAll.TabIndex = 1
        Me.Button_UncheckAll.Text = "Uncheck all"
        '
        'Button_CopyCheckedItems
        '
        Me.Button_CopyCheckedItems.Enabled = False
        Me.Button_CopyCheckedItems.Location = New System.Drawing.Point(202, 0)
        Me.Button_CopyCheckedItems.Margin = New System.Windows.Forms.Padding(0, 0, 6, 0)
        Me.Button_CopyCheckedItems.Name = "Button_CopyCheckedItems"
        Me.Button_CopyCheckedItems.Size = New System.Drawing.Size(116, 28)
        Me.Button_CopyCheckedItems.TabIndex = 2
        Me.Button_CopyCheckedItems.Text = "Copy checked"
        '
        'Button_CopyAllAsCSV
        '
        Me.Button_CopyAllAsCSV.Enabled = False
        Me.Button_CopyAllAsCSV.Location = New System.Drawing.Point(324, 0)
        Me.Button_CopyAllAsCSV.Margin = New System.Windows.Forms.Padding(0)
        Me.Button_CopyAllAsCSV.Name = "Button_CopyAllAsCSV"
        Me.Button_CopyAllAsCSV.Size = New System.Drawing.Size(138, 28)
        Me.Button_CopyAllAsCSV.TabIndex = 3
        Me.Button_CopyAllAsCSV.Text = "Copy all as CSV"
        '
        'ListView_Albums
        '
        Me.ListView_Albums.CheckBoxes = True
        Me.ListView_Albums.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader_Artist, Me.ColumnHeader_Album, Me.ColumnHeader_DownloadLinks})
        Me.ListView_Albums.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView_Albums.EmptyText = "No albums yet. Choose the search criteria above and click ""Fetch albums""."
        Me.ListView_Albums.HideSelection = False
        Me.ListView_Albums.Location = New System.Drawing.Point(0, 50)
        Me.ListView_Albums.Margin = New System.Windows.Forms.Padding(0)
        Me.ListView_Albums.Name = "ListView_Albums"
        Me.ListView_Albums.Size = New System.Drawing.Size(966, 370)
        Me.ListView_Albums.TabIndex = 1
        Me.ListView_Albums.UseCompatibleStateImageBehavior = False
        '
        'ColumnHeader_Artist
        '
        Me.ColumnHeader_Artist.Tag = "0"
        Me.ColumnHeader_Artist.Text = "Artist"
        Me.ColumnHeader_Artist.Width = 220
        '
        'ColumnHeader_Album
        '
        Me.ColumnHeader_Album.Tag = "0"
        Me.ColumnHeader_Album.Text = "Album"
        Me.ColumnHeader_Album.Width = 300
        '
        'ColumnHeader_DownloadLinks
        '
        Me.ColumnHeader_DownloadLinks.Tag = "0"
        Me.ColumnHeader_DownloadLinks.Text = "Download links"
        Me.ColumnHeader_DownloadLinks.Width = 440
        '
        'Panel_BrowserView
        '
        Me.Panel_BrowserView.Controls.Add(Me.TableLayoutPanel_BrowserView)
        Me.Panel_BrowserView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel_BrowserView.Location = New System.Drawing.Point(0, 0)
        Me.Panel_BrowserView.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel_BrowserView.Name = "Panel_BrowserView"
        Me.Panel_BrowserView.Padding = New System.Windows.Forms.Padding(16, 16, 16, 12)
        Me.Panel_BrowserView.Size = New System.Drawing.Size(1000, 590)
        Me.Panel_BrowserView.TabIndex = 1
        Me.Panel_BrowserView.Visible = False
        '
        'TableLayoutPanel_BrowserView
        '
        Me.TableLayoutPanel_BrowserView.ColumnCount = 1
        Me.TableLayoutPanel_BrowserView.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_BrowserView.Controls.Add(Me.CardPanel_BrowserInfo, 0, 0)
        Me.TableLayoutPanel_BrowserView.Controls.Add(Me.CardPanel_Browser, 0, 2)
        Me.TableLayoutPanel_BrowserView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel_BrowserView.Location = New System.Drawing.Point(16, 16)
        Me.TableLayoutPanel_BrowserView.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel_BrowserView.Name = "TableLayoutPanel_BrowserView"
        Me.TableLayoutPanel_BrowserView.RowCount = 3
        Me.TableLayoutPanel_BrowserView.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56.0!))
        Me.TableLayoutPanel_BrowserView.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12.0!))
        Me.TableLayoutPanel_BrowserView.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_BrowserView.Size = New System.Drawing.Size(968, 562)
        Me.TableLayoutPanel_BrowserView.TabIndex = 0
        '
        'CardPanel_BrowserInfo
        '
        Me.CardPanel_BrowserInfo.Controls.Add(Me.TableLayoutPanel_BrowserInfo)
        Me.CardPanel_BrowserInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CardPanel_BrowserInfo.Location = New System.Drawing.Point(0, 0)
        Me.CardPanel_BrowserInfo.Margin = New System.Windows.Forms.Padding(0)
        Me.CardPanel_BrowserInfo.Name = "CardPanel_BrowserInfo"
        Me.CardPanel_BrowserInfo.Padding = New System.Windows.Forms.Padding(16, 1, 12, 1)
        Me.CardPanel_BrowserInfo.Size = New System.Drawing.Size(968, 56)
        Me.CardPanel_BrowserInfo.TabIndex = 0
        '
        'TableLayoutPanel_BrowserInfo
        '
        Me.TableLayoutPanel_BrowserInfo.ColumnCount = 2
        Me.TableLayoutPanel_BrowserInfo.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_BrowserInfo.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel_BrowserInfo.Controls.Add(Me.Label_BrowserInfo, 0, 0)
        Me.TableLayoutPanel_BrowserInfo.Controls.Add(Me.Button_BrowserHome, 1, 0)
        Me.TableLayoutPanel_BrowserInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel_BrowserInfo.Location = New System.Drawing.Point(16, 1)
        Me.TableLayoutPanel_BrowserInfo.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel_BrowserInfo.Name = "TableLayoutPanel_BrowserInfo"
        Me.TableLayoutPanel_BrowserInfo.RowCount = 1
        Me.TableLayoutPanel_BrowserInfo.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_BrowserInfo.Size = New System.Drawing.Size(940, 54)
        Me.TableLayoutPanel_BrowserInfo.TabIndex = 0
        '
        'Label_BrowserInfo
        '
        Me.Label_BrowserInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label_BrowserInfo.AutoSize = True
        Me.Label_BrowserInfo.Location = New System.Drawing.Point(0, 18)
        Me.Label_BrowserInfo.Margin = New System.Windows.Forms.Padding(0, 0, 12, 0)
        Me.Label_BrowserInfo.Name = "Label_BrowserInfo"
        Me.Label_BrowserInfo.Size = New System.Drawing.Size(778, 17)
        Me.Label_BrowserInfo.TabIndex = 0
        Me.Label_BrowserInfo.Text = "Log in to freehardmusic.com with your user account. The album search uses the session of this browser."
        '
        'Button_BrowserHome
        '
        Me.Button_BrowserHome.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Button_BrowserHome.Location = New System.Drawing.Point(790, 13)
        Me.Button_BrowserHome.Margin = New System.Windows.Forms.Padding(0)
        Me.Button_BrowserHome.Name = "Button_BrowserHome"
        Me.Button_BrowserHome.Size = New System.Drawing.Size(150, 28)
        Me.Button_BrowserHome.TabIndex = 1
        Me.Button_BrowserHome.Text = "Go to home page"
        '
        'CardPanel_Browser
        '
        Me.CardPanel_Browser.Controls.Add(Me.WebBrowser_Login)
        Me.CardPanel_Browser.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CardPanel_Browser.Location = New System.Drawing.Point(0, 68)
        Me.CardPanel_Browser.Margin = New System.Windows.Forms.Padding(0)
        Me.CardPanel_Browser.Name = "CardPanel_Browser"
        Me.CardPanel_Browser.Padding = New System.Windows.Forms.Padding(1)
        Me.CardPanel_Browser.Size = New System.Drawing.Size(968, 494)
        Me.CardPanel_Browser.TabIndex = 1
        '
        'WebBrowser_Login
        '
        Me.WebBrowser_Login.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WebBrowser_Login.Location = New System.Drawing.Point(1, 1)
        Me.WebBrowser_Login.Margin = New System.Windows.Forms.Padding(0)
        Me.WebBrowser_Login.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser_Login.Name = "WebBrowser_Login"
        Me.WebBrowser_Login.ScriptErrorsSuppressed = True
        Me.WebBrowser_Login.Size = New System.Drawing.Size(966, 492)
        Me.WebBrowser_Login.TabIndex = 0
        '
        'CardPanel_StatusBar
        '
        Me.CardPanel_StatusBar.BorderSides = FHMCrawler.UI.Controls.BorderSides.Top
        Me.CardPanel_StatusBar.Controls.Add(Me.TableLayoutPanel_StatusBar)
        Me.CardPanel_StatusBar.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CardPanel_StatusBar.Location = New System.Drawing.Point(0, 646)
        Me.CardPanel_StatusBar.Margin = New System.Windows.Forms.Padding(0)
        Me.CardPanel_StatusBar.Name = "CardPanel_StatusBar"
        Me.CardPanel_StatusBar.Padding = New System.Windows.Forms.Padding(16, 1, 16, 0)
        Me.CardPanel_StatusBar.Size = New System.Drawing.Size(1000, 34)
        Me.CardPanel_StatusBar.TabIndex = 2
        '
        'TableLayoutPanel_StatusBar
        '
        Me.TableLayoutPanel_StatusBar.ColumnCount = 2
        Me.TableLayoutPanel_StatusBar.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_StatusBar.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 240.0!))
        Me.TableLayoutPanel_StatusBar.Controls.Add(Me.Label_Status, 0, 0)
        Me.TableLayoutPanel_StatusBar.Controls.Add(Me.ProgressBar_Fetch, 1, 0)
        Me.TableLayoutPanel_StatusBar.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel_StatusBar.Location = New System.Drawing.Point(16, 1)
        Me.TableLayoutPanel_StatusBar.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel_StatusBar.Name = "TableLayoutPanel_StatusBar"
        Me.TableLayoutPanel_StatusBar.RowCount = 1
        Me.TableLayoutPanel_StatusBar.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_StatusBar.Size = New System.Drawing.Size(968, 33)
        Me.TableLayoutPanel_StatusBar.TabIndex = 0
        '
        'Label_Status
        '
        Me.Label_Status.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label_Status.AutoEllipsis = True
        Me.Label_Status.Location = New System.Drawing.Point(0, 8)
        Me.Label_Status.Margin = New System.Windows.Forms.Padding(0, 0, 16, 0)
        Me.Label_Status.Name = "Label_Status"
        Me.Label_Status.Size = New System.Drawing.Size(712, 17)
        Me.Label_Status.TabIndex = 0
        Me.Label_Status.Text = "Ready."
        '
        'ProgressBar_Fetch
        '
        Me.ProgressBar_Fetch.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar_Fetch.Location = New System.Drawing.Point(728, 13)
        Me.ProgressBar_Fetch.Margin = New System.Windows.Forms.Padding(0)
        Me.ProgressBar_Fetch.Name = "ProgressBar_Fetch"
        Me.ProgressBar_Fetch.Size = New System.Drawing.Size(240, 6)
        Me.ProgressBar_Fetch.TabIndex = 1
        '
        'Main
        '
        Me.AcceptButton = Me.Button_FetchAlbums
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1000, 680)
        Me.Controls.Add(Me.TableLayoutPanel_Root)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(900, 600)
        Me.Name = "Main"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FHM Crawler"
        Me.TableLayoutPanel_Root.ResumeLayout(False)
        Me.CardPanel_Header.ResumeLayout(False)
        Me.TableLayoutPanel_Header.ResumeLayout(False)
        Me.TableLayoutPanel_Header.PerformLayout()
        Me.FlowLayoutPanel_Navigation.ResumeLayout(False)
        Me.Panel_Content.ResumeLayout(False)
        Me.Panel_SearchView.ResumeLayout(False)
        Me.TableLayoutPanel_SearchView.ResumeLayout(False)
        Me.CardPanel_Search.ResumeLayout(False)
        Me.TableLayoutPanel_Search.ResumeLayout(False)
        Me.TableLayoutPanel_Search.PerformLayout()
        Me.FlowLayoutPanel_SearchMode.ResumeLayout(False)
        Me.Panel_SearchInputs.ResumeLayout(False)
        Me.TableLayoutPanel_FilterInputs.ResumeLayout(False)
        Me.TableLayoutPanel_FilterInputs.PerformLayout()
        Me.TableLayoutPanel_ArtistInputs.ResumeLayout(False)
        Me.TableLayoutPanel_ArtistInputs.PerformLayout()
        Me.FlowLayoutPanel_SearchActions.ResumeLayout(False)
        Me.CardPanel_Results.ResumeLayout(False)
        Me.TableLayoutPanel_Results.ResumeLayout(False)
        Me.CardPanel_ResultsToolbar.ResumeLayout(False)
        Me.TableLayoutPanel_ResultsToolbar.ResumeLayout(False)
        Me.TableLayoutPanel_ResultsToolbar.PerformLayout()
        Me.FlowLayoutPanel_ResultsActions.ResumeLayout(False)
        Me.Panel_BrowserView.ResumeLayout(False)
        Me.TableLayoutPanel_BrowserView.ResumeLayout(False)
        Me.CardPanel_BrowserInfo.ResumeLayout(False)
        Me.TableLayoutPanel_BrowserInfo.ResumeLayout(False)
        Me.TableLayoutPanel_BrowserInfo.PerformLayout()
        Me.CardPanel_Browser.ResumeLayout(False)
        Me.CardPanel_StatusBar.ResumeLayout(False)
        Me.TableLayoutPanel_StatusBar.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanel_Root As TableLayoutPanel
    Friend WithEvents CardPanel_Header As FHMCrawler.UI.Controls.CardPanel
    Friend WithEvents TableLayoutPanel_Header As TableLayoutPanel
    Friend WithEvents Label_AppTitle As Label
    Friend WithEvents Label_AppVersion As Label
    Friend WithEvents FlowLayoutPanel_Navigation As FlowLayoutPanel
    Friend WithEvents SegmentButton_NavSearch As FHMCrawler.UI.Controls.SegmentButton
    Friend WithEvents SegmentButton_NavBrowser As FHMCrawler.UI.Controls.SegmentButton
    Friend WithEvents IconButton_GitHub As FHMCrawler.UI.Controls.IconButton
    Friend WithEvents Label_DarkTheme As Label
    Friend WithEvents ToggleSwitch_DarkTheme As FHMCrawler.UI.Controls.ToggleSwitch
    Friend WithEvents Panel_Content As Panel
    Friend WithEvents Panel_SearchView As Panel
    Friend WithEvents TableLayoutPanel_SearchView As TableLayoutPanel
    Friend WithEvents CardPanel_Search As FHMCrawler.UI.Controls.CardPanel
    Friend WithEvents TableLayoutPanel_Search As TableLayoutPanel
    Friend WithEvents Label_SearchTitle As Label
    Friend WithEvents FlowLayoutPanel_SearchMode As FlowLayoutPanel
    Friend WithEvents SegmentButton_ModeArtist As FHMCrawler.UI.Controls.SegmentButton
    Friend WithEvents SegmentButton_ModeFilters As FHMCrawler.UI.Controls.SegmentButton
    Friend WithEvents Panel_SearchInputs As Panel
    Friend WithEvents TableLayoutPanel_FilterInputs As TableLayoutPanel
    Friend WithEvents Label_GenreCaption As Label
    Friend WithEvents Label_CountryCaption As Label
    Friend WithEvents Label_YearCaption As Label
    Friend WithEvents ComboBox_Genre As FHMCrawler.UI.Controls.ThemedComboBox
    Friend WithEvents ComboBox_Country As FHMCrawler.UI.Controls.ThemedComboBox
    Friend WithEvents ComboBox_Year As FHMCrawler.UI.Controls.ThemedComboBox
    Friend WithEvents TableLayoutPanel_ArtistInputs As TableLayoutPanel
    Friend WithEvents Label_ArtistCaption As Label
    Friend WithEvents TextBox_Artist As FHMCrawler.UI.Controls.ThemedTextBox
    Friend WithEvents FlowLayoutPanel_SearchActions As FlowLayoutPanel
    Friend WithEvents Button_FetchAlbums As FHMCrawler.UI.Controls.ThemedButton
    Friend WithEvents Button_Cancel As FHMCrawler.UI.Controls.ThemedButton
    Friend WithEvents CardPanel_Results As FHMCrawler.UI.Controls.CardPanel
    Friend WithEvents TableLayoutPanel_Results As TableLayoutPanel
    Friend WithEvents CardPanel_ResultsToolbar As FHMCrawler.UI.Controls.CardPanel
    Friend WithEvents TableLayoutPanel_ResultsToolbar As TableLayoutPanel
    Friend WithEvents Label_ResultsTitle As Label
    Friend WithEvents Label_AlbumCount As Label
    Friend WithEvents FlowLayoutPanel_ResultsActions As FlowLayoutPanel
    Friend WithEvents Button_CheckAll As FHMCrawler.UI.Controls.ThemedButton
    Friend WithEvents Button_UncheckAll As FHMCrawler.UI.Controls.ThemedButton
    Friend WithEvents Button_CopyCheckedItems As FHMCrawler.UI.Controls.ThemedButton
    Friend WithEvents Button_CopyAllAsCSV As FHMCrawler.UI.Controls.ThemedButton
    Friend WithEvents ListView_Albums As FHMCrawler.UI.Controls.ThemedListView
    Friend WithEvents ColumnHeader_Artist As ColumnHeader
    Friend WithEvents ColumnHeader_Album As ColumnHeader
    Friend WithEvents ColumnHeader_DownloadLinks As ColumnHeader
    Friend WithEvents Panel_BrowserView As Panel
    Friend WithEvents TableLayoutPanel_BrowserView As TableLayoutPanel
    Friend WithEvents CardPanel_BrowserInfo As FHMCrawler.UI.Controls.CardPanel
    Friend WithEvents TableLayoutPanel_BrowserInfo As TableLayoutPanel
    Friend WithEvents Label_BrowserInfo As Label
    Friend WithEvents Button_BrowserHome As FHMCrawler.UI.Controls.ThemedButton
    Friend WithEvents CardPanel_Browser As FHMCrawler.UI.Controls.CardPanel
    Friend WithEvents WebBrowser_Login As WebBrowser
    Friend WithEvents CardPanel_StatusBar As FHMCrawler.UI.Controls.CardPanel
    Friend WithEvents TableLayoutPanel_StatusBar As TableLayoutPanel
    Friend WithEvents Label_Status As Label
    Friend WithEvents ProgressBar_Fetch As FHMCrawler.UI.Controls.FlatProgressBar
    Friend WithEvents ToolTip_Main As ToolTip
End Class
