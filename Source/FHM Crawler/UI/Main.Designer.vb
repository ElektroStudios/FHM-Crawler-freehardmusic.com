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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.ComboBox_Genre = New System.Windows.Forms.ComboBox()
        Me.ComboBox_Year = New System.Windows.Forms.ComboBox()
        Me.ComboBox_Country = New System.Windows.Forms.ComboBox()
        Me.TextBoxArtist = New System.Windows.Forms.TextBox()
        Me.ListView_Albums = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Button_FetchAlbums = New System.Windows.Forms.Button()
        Me.Label_Artist = New System.Windows.Forms.Label()
        Me.Label_Country = New System.Windows.Forms.Label()
        Me.Label_Gernre = New System.Windows.Forms.Label()
        Me.Label_Year = New System.Windows.Forms.Label()
        Me.GroupBox_ArtistSearch = New System.Windows.Forms.GroupBox()
        Me.GroupBox_CustomSearch = New System.Windows.Forms.GroupBox()
        Me.RadioButton_ArtistSearch = New System.Windows.Forms.RadioButton()
        Me.RadioButton_CustomSearch = New System.Windows.Forms.RadioButton()
        Me.Button_CopyAllAsCSV = New System.Windows.Forms.Button()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.Label_AlbumCount = New System.Windows.Forms.Label()
        Me.Button_CopyCheckedItems = New System.Windows.Forms.Button()
        Me.Button_CheckAll = New System.Windows.Forms.Button()
        Me.Button_UncheckAll = New System.Windows.Forms.Button()
        Me.Button_Cancel = New System.Windows.Forms.Button()
        Me.WebBrowser1 = New System.Windows.Forms.WebBrowser()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPageCrawler = New System.Windows.Forms.TabPage()
        Me.Label_Debug = New System.Windows.Forms.Label()
        Me.TabPageBrowser = New System.Windows.Forms.TabPage()
        Me.GroupBox_ArtistSearch.SuspendLayout()
        Me.GroupBox_CustomSearch.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPageCrawler.SuspendLayout()
        Me.TabPageBrowser.SuspendLayout()
        Me.SuspendLayout()
        '
        'ComboBox_Genre
        '
        Me.ComboBox_Genre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Genre.FormattingEnabled = True
        Me.ComboBox_Genre.Location = New System.Drawing.Point(64, 25)
        Me.ComboBox_Genre.Margin = New System.Windows.Forms.Padding(4)
        Me.ComboBox_Genre.Name = "ComboBox_Genre"
        Me.ComboBox_Genre.Size = New System.Drawing.Size(252, 25)
        Me.ComboBox_Genre.TabIndex = 0
        '
        'ComboBox_Year
        '
        Me.ComboBox_Year.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Year.FormattingEnabled = True
        Me.ComboBox_Year.Location = New System.Drawing.Point(64, 95)
        Me.ComboBox_Year.Margin = New System.Windows.Forms.Padding(4)
        Me.ComboBox_Year.Name = "ComboBox_Year"
        Me.ComboBox_Year.Size = New System.Drawing.Size(252, 25)
        Me.ComboBox_Year.TabIndex = 1
        '
        'ComboBox_Country
        '
        Me.ComboBox_Country.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Country.FormattingEnabled = True
        Me.ComboBox_Country.Location = New System.Drawing.Point(64, 60)
        Me.ComboBox_Country.Margin = New System.Windows.Forms.Padding(4)
        Me.ComboBox_Country.Name = "ComboBox_Country"
        Me.ComboBox_Country.Size = New System.Drawing.Size(252, 25)
        Me.ComboBox_Country.TabIndex = 2
        '
        'TextBoxArtist
        '
        Me.TextBoxArtist.Location = New System.Drawing.Point(64, 21)
        Me.TextBoxArtist.Margin = New System.Windows.Forms.Padding(4)
        Me.TextBoxArtist.Name = "TextBoxArtist"
        Me.TextBoxArtist.Size = New System.Drawing.Size(245, 25)
        Me.TextBoxArtist.TabIndex = 3
        '
        'ListView_Albums
        '
        Me.ListView_Albums.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView_Albums.CheckBoxes = True
        Me.ListView_Albums.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.ListView_Albums.FullRowSelect = True
        Me.ListView_Albums.HideSelection = False
        Me.ListView_Albums.Location = New System.Drawing.Point(10, 175)
        Me.ListView_Albums.Margin = New System.Windows.Forms.Padding(4)
        Me.ListView_Albums.Name = "ListView_Albums"
        Me.ListView_Albums.Size = New System.Drawing.Size(685, 322)
        Me.ListView_Albums.TabIndex = 4
        Me.ListView_Albums.UseCompatibleStateImageBehavior = False
        Me.ListView_Albums.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Tag = "0"
        Me.ColumnHeader1.Text = "Artist"
        Me.ColumnHeader1.Width = 127
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Tag = "0"
        Me.ColumnHeader2.Text = "Album"
        Me.ColumnHeader2.Width = 169
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Tag = "0"
        Me.ColumnHeader3.Text = "Download Link(s)"
        Me.ColumnHeader3.Width = 155
        '
        'Button_FetchAlbums
        '
        Me.Button_FetchAlbums.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button_FetchAlbums.Location = New System.Drawing.Point(10, 505)
        Me.Button_FetchAlbums.Margin = New System.Windows.Forms.Padding(4)
        Me.Button_FetchAlbums.Name = "Button_FetchAlbums"
        Me.Button_FetchAlbums.Size = New System.Drawing.Size(97, 38)
        Me.Button_FetchAlbums.TabIndex = 5
        Me.Button_FetchAlbums.Text = "Fetch Albums"
        Me.Button_FetchAlbums.UseVisualStyleBackColor = True
        '
        'Label_Artist
        '
        Me.Label_Artist.AutoSize = True
        Me.Label_Artist.Location = New System.Drawing.Point(7, 25)
        Me.Label_Artist.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label_Artist.Name = "Label_Artist"
        Me.Label_Artist.Size = New System.Drawing.Size(38, 17)
        Me.Label_Artist.TabIndex = 6
        Me.Label_Artist.Text = "Artist"
        '
        'Label_Country
        '
        Me.Label_Country.AutoSize = True
        Me.Label_Country.Location = New System.Drawing.Point(7, 64)
        Me.Label_Country.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label_Country.Name = "Label_Country"
        Me.Label_Country.Size = New System.Drawing.Size(53, 17)
        Me.Label_Country.TabIndex = 7
        Me.Label_Country.Text = "Country"
        '
        'Label_Gernre
        '
        Me.Label_Gernre.AutoSize = True
        Me.Label_Gernre.Location = New System.Drawing.Point(7, 29)
        Me.Label_Gernre.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label_Gernre.Name = "Label_Gernre"
        Me.Label_Gernre.Size = New System.Drawing.Size(43, 17)
        Me.Label_Gernre.TabIndex = 8
        Me.Label_Gernre.Text = "Genre"
        '
        'Label_Year
        '
        Me.Label_Year.AutoSize = True
        Me.Label_Year.Location = New System.Drawing.Point(7, 99)
        Me.Label_Year.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label_Year.Name = "Label_Year"
        Me.Label_Year.Size = New System.Drawing.Size(33, 17)
        Me.Label_Year.TabIndex = 9
        Me.Label_Year.Text = "Year"
        '
        'GroupBox_ArtistSearch
        '
        Me.GroupBox_ArtistSearch.Controls.Add(Me.Label_Artist)
        Me.GroupBox_ArtistSearch.Controls.Add(Me.TextBoxArtist)
        Me.GroupBox_ArtistSearch.Location = New System.Drawing.Point(7, 27)
        Me.GroupBox_ArtistSearch.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox_ArtistSearch.Name = "GroupBox_ArtistSearch"
        Me.GroupBox_ArtistSearch.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox_ArtistSearch.Size = New System.Drawing.Size(323, 140)
        Me.GroupBox_ArtistSearch.TabIndex = 10
        Me.GroupBox_ArtistSearch.TabStop = False
        '
        'GroupBox_CustomSearch
        '
        Me.GroupBox_CustomSearch.Controls.Add(Me.Label_Gernre)
        Me.GroupBox_CustomSearch.Controls.Add(Me.ComboBox_Genre)
        Me.GroupBox_CustomSearch.Controls.Add(Me.Label_Year)
        Me.GroupBox_CustomSearch.Controls.Add(Me.ComboBox_Country)
        Me.GroupBox_CustomSearch.Controls.Add(Me.Label_Country)
        Me.GroupBox_CustomSearch.Controls.Add(Me.ComboBox_Year)
        Me.GroupBox_CustomSearch.Location = New System.Drawing.Point(373, 27)
        Me.GroupBox_CustomSearch.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox_CustomSearch.Name = "GroupBox_CustomSearch"
        Me.GroupBox_CustomSearch.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox_CustomSearch.Size = New System.Drawing.Size(323, 140)
        Me.GroupBox_CustomSearch.TabIndex = 11
        Me.GroupBox_CustomSearch.TabStop = False
        '
        'RadioButton_ArtistSearch
        '
        Me.RadioButton_ArtistSearch.AutoSize = True
        Me.RadioButton_ArtistSearch.Checked = True
        Me.RadioButton_ArtistSearch.Location = New System.Drawing.Point(7, 7)
        Me.RadioButton_ArtistSearch.Margin = New System.Windows.Forms.Padding(4)
        Me.RadioButton_ArtistSearch.Name = "RadioButton_ArtistSearch"
        Me.RadioButton_ArtistSearch.Size = New System.Drawing.Size(98, 21)
        Me.RadioButton_ArtistSearch.TabIndex = 12
        Me.RadioButton_ArtistSearch.TabStop = True
        Me.RadioButton_ArtistSearch.Text = "Artist search"
        Me.RadioButton_ArtistSearch.UseVisualStyleBackColor = True
        '
        'RadioButton_CustomSearch
        '
        Me.RadioButton_CustomSearch.AutoSize = True
        Me.RadioButton_CustomSearch.Location = New System.Drawing.Point(373, 7)
        Me.RadioButton_CustomSearch.Margin = New System.Windows.Forms.Padding(4)
        Me.RadioButton_CustomSearch.Name = "RadioButton_CustomSearch"
        Me.RadioButton_CustomSearch.Size = New System.Drawing.Size(112, 21)
        Me.RadioButton_CustomSearch.TabIndex = 13
        Me.RadioButton_CustomSearch.Text = "Custom search"
        Me.RadioButton_CustomSearch.UseVisualStyleBackColor = True
        '
        'Button_CopyAllAsCSV
        '
        Me.Button_CopyAllAsCSV.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button_CopyAllAsCSV.Enabled = False
        Me.Button_CopyAllAsCSV.Location = New System.Drawing.Point(218, 551)
        Me.Button_CopyAllAsCSV.Margin = New System.Windows.Forms.Padding(4)
        Me.Button_CopyAllAsCSV.Name = "Button_CopyAllAsCSV"
        Me.Button_CopyAllAsCSV.Size = New System.Drawing.Size(152, 38)
        Me.Button_CopyAllAsCSV.TabIndex = 14
        Me.Button_CopyAllAsCSV.Text = "Copy All Items As CSV"
        Me.Button_CopyAllAsCSV.UseVisualStyleBackColor = True
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar1.Location = New System.Drawing.Point(218, 505)
        Me.ProgressBar1.Margin = New System.Windows.Forms.Padding(4)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(310, 38)
        Me.ProgressBar1.TabIndex = 15
        '
        'Label_AlbumCount
        '
        Me.Label_AlbumCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label_AlbumCount.AutoEllipsis = True
        Me.Label_AlbumCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.Label_AlbumCount.Location = New System.Drawing.Point(535, 505)
        Me.Label_AlbumCount.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label_AlbumCount.Name = "Label_AlbumCount"
        Me.Label_AlbumCount.Size = New System.Drawing.Size(160, 46)
        Me.Label_AlbumCount.TabIndex = 16
        Me.Label_AlbumCount.Text = "..."
        '
        'Button_CopyCheckedItems
        '
        Me.Button_CopyCheckedItems.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button_CopyCheckedItems.Enabled = False
        Me.Button_CopyCheckedItems.Location = New System.Drawing.Point(376, 551)
        Me.Button_CopyCheckedItems.Margin = New System.Windows.Forms.Padding(4)
        Me.Button_CopyCheckedItems.Name = "Button_CopyCheckedItems"
        Me.Button_CopyCheckedItems.Size = New System.Drawing.Size(152, 38)
        Me.Button_CopyCheckedItems.TabIndex = 17
        Me.Button_CopyCheckedItems.Text = "Copy Checked Items"
        Me.Button_CopyCheckedItems.UseVisualStyleBackColor = True
        '
        'Button_CheckAll
        '
        Me.Button_CheckAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button_CheckAll.Enabled = False
        Me.Button_CheckAll.Location = New System.Drawing.Point(10, 551)
        Me.Button_CheckAll.Margin = New System.Windows.Forms.Padding(4)
        Me.Button_CheckAll.Name = "Button_CheckAll"
        Me.Button_CheckAll.Size = New System.Drawing.Size(97, 38)
        Me.Button_CheckAll.TabIndex = 19
        Me.Button_CheckAll.Text = "Check All"
        Me.Button_CheckAll.UseVisualStyleBackColor = True
        '
        'Button_UncheckAll
        '
        Me.Button_UncheckAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button_UncheckAll.Enabled = False
        Me.Button_UncheckAll.Location = New System.Drawing.Point(114, 551)
        Me.Button_UncheckAll.Margin = New System.Windows.Forms.Padding(4)
        Me.Button_UncheckAll.Name = "Button_UncheckAll"
        Me.Button_UncheckAll.Size = New System.Drawing.Size(97, 38)
        Me.Button_UncheckAll.TabIndex = 20
        Me.Button_UncheckAll.Text = "Uncheck All"
        Me.Button_UncheckAll.UseVisualStyleBackColor = True
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button_Cancel.Enabled = False
        Me.Button_Cancel.Location = New System.Drawing.Point(114, 505)
        Me.Button_Cancel.Margin = New System.Windows.Forms.Padding(4)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(97, 38)
        Me.Button_Cancel.TabIndex = 21
        Me.Button_Cancel.Text = "CANCEL"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'WebBrowser1
        '
        Me.WebBrowser1.AllowWebBrowserDrop = False
        Me.WebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WebBrowser1.Location = New System.Drawing.Point(3, 3)
        Me.WebBrowser1.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.ScriptErrorsSuppressed = True
        Me.WebBrowser1.Size = New System.Drawing.Size(700, 590)
        Me.WebBrowser1.TabIndex = 24
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPageCrawler)
        Me.TabControl1.Controls.Add(Me.TabPageBrowser)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(714, 626)
        Me.TabControl1.TabIndex = 10
        '
        'TabPageCrawler
        '
        Me.TabPageCrawler.Controls.Add(Me.Label_Debug)
        Me.TabPageCrawler.Controls.Add(Me.Button_Cancel)
        Me.TabPageCrawler.Controls.Add(Me.GroupBox_ArtistSearch)
        Me.TabPageCrawler.Controls.Add(Me.Button_UncheckAll)
        Me.TabPageCrawler.Controls.Add(Me.RadioButton_ArtistSearch)
        Me.TabPageCrawler.Controls.Add(Me.Button_CheckAll)
        Me.TabPageCrawler.Controls.Add(Me.GroupBox_CustomSearch)
        Me.TabPageCrawler.Controls.Add(Me.Button_CopyCheckedItems)
        Me.TabPageCrawler.Controls.Add(Me.RadioButton_CustomSearch)
        Me.TabPageCrawler.Controls.Add(Me.Label_AlbumCount)
        Me.TabPageCrawler.Controls.Add(Me.ListView_Albums)
        Me.TabPageCrawler.Controls.Add(Me.ProgressBar1)
        Me.TabPageCrawler.Controls.Add(Me.Button_FetchAlbums)
        Me.TabPageCrawler.Controls.Add(Me.Button_CopyAllAsCSV)
        Me.TabPageCrawler.Location = New System.Drawing.Point(4, 26)
        Me.TabPageCrawler.Name = "TabPageCrawler"
        Me.TabPageCrawler.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageCrawler.Size = New System.Drawing.Size(706, 596)
        Me.TabPageCrawler.TabIndex = 0
        Me.TabPageCrawler.Text = "Crawler"
        Me.TabPageCrawler.UseVisualStyleBackColor = True
        '
        'Label_Debug
        '
        Me.Label_Debug.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label_Debug.AutoEllipsis = True
        Me.Label_Debug.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.Label_Debug.Location = New System.Drawing.Point(535, 551)
        Me.Label_Debug.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label_Debug.Name = "Label_Debug"
        Me.Label_Debug.Size = New System.Drawing.Size(160, 38)
        Me.Label_Debug.TabIndex = 22
        Me.Label_Debug.Text = "..."
        '
        'TabPageBrowser
        '
        Me.TabPageBrowser.Controls.Add(Me.WebBrowser1)
        Me.TabPageBrowser.Location = New System.Drawing.Point(4, 26)
        Me.TabPageBrowser.Name = "TabPageBrowser"
        Me.TabPageBrowser.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageBrowser.Size = New System.Drawing.Size(706, 596)
        Me.TabPageBrowser.TabIndex = 1
        Me.TabPageBrowser.Text = "Browser (log in)"
        Me.TabPageBrowser.UseVisualStyleBackColor = True
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(714, 626)
        Me.Controls.Add(Me.TabControl1)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "Main"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FHM Crawler - Demo by Elektro"
        Me.GroupBox_ArtistSearch.ResumeLayout(False)
        Me.GroupBox_ArtistSearch.PerformLayout()
        Me.GroupBox_CustomSearch.ResumeLayout(False)
        Me.GroupBox_CustomSearch.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPageCrawler.ResumeLayout(False)
        Me.TabPageCrawler.PerformLayout()
        Me.TabPageBrowser.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ComboBox_Genre As ComboBox
    Friend WithEvents ComboBox_Year As ComboBox
    Friend WithEvents ComboBox_Country As ComboBox
    Friend WithEvents TextBoxArtist As TextBox
    Friend WithEvents ListView_Albums As ListView
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents Button_FetchAlbums As Button
    Friend WithEvents Label_Artist As Label
    Friend WithEvents Label_Country As Label
    Friend WithEvents Label_Gernre As Label
    Friend WithEvents Label_Year As Label
    Friend WithEvents GroupBox_ArtistSearch As GroupBox
    Friend WithEvents GroupBox_CustomSearch As GroupBox
    Friend WithEvents RadioButton_ArtistSearch As RadioButton
    Friend WithEvents RadioButton_CustomSearch As RadioButton
    Friend WithEvents Button_CopyAllAsCSV As Button
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents Label_AlbumCount As Label
    Friend WithEvents Button_CopyCheckedItems As Button
    Friend WithEvents Button_CheckAll As Button
    Friend WithEvents Button_UncheckAll As Button
    Friend WithEvents Button_Cancel As Button
    Friend WithEvents WebBrowser1 As WebBrowser
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPageCrawler As TabPage
    Friend WithEvents TabPageBrowser As TabPage
    Friend WithEvents Label_Debug As Label
End Class
