<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
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
        Me.GroupBox_ArtistSearch.SuspendLayout()
        Me.GroupBox_CustomSearch.SuspendLayout()
        Me.SuspendLayout()
        '
        'ComboBox_Genre
        '
        Me.ComboBox_Genre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Genre.FormattingEnabled = True
        Me.ComboBox_Genre.Location = New System.Drawing.Point(55, 19)
        Me.ComboBox_Genre.Name = "ComboBox_Genre"
        Me.ComboBox_Genre.Size = New System.Drawing.Size(217, 21)
        Me.ComboBox_Genre.TabIndex = 0
        '
        'ComboBox_Year
        '
        Me.ComboBox_Year.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Year.FormattingEnabled = True
        Me.ComboBox_Year.Location = New System.Drawing.Point(55, 73)
        Me.ComboBox_Year.Name = "ComboBox_Year"
        Me.ComboBox_Year.Size = New System.Drawing.Size(217, 21)
        Me.ComboBox_Year.TabIndex = 1
        '
        'ComboBox_Country
        '
        Me.ComboBox_Country.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Country.FormattingEnabled = True
        Me.ComboBox_Country.Location = New System.Drawing.Point(55, 46)
        Me.ComboBox_Country.Name = "ComboBox_Country"
        Me.ComboBox_Country.Size = New System.Drawing.Size(217, 21)
        Me.ComboBox_Country.TabIndex = 2
        '
        'TextBoxArtist
        '
        Me.TextBoxArtist.Location = New System.Drawing.Point(55, 16)
        Me.TextBoxArtist.Name = "TextBoxArtist"
        Me.TextBoxArtist.Size = New System.Drawing.Size(211, 20)
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
        Me.ListView_Albums.Location = New System.Drawing.Point(12, 158)
        Me.ListView_Albums.Name = "ListView_Albums"
        Me.ListView_Albums.Size = New System.Drawing.Size(588, 298)
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
        Me.Button_FetchAlbums.Location = New System.Drawing.Point(12, 462)
        Me.Button_FetchAlbums.Name = "Button_FetchAlbums"
        Me.Button_FetchAlbums.Size = New System.Drawing.Size(83, 29)
        Me.Button_FetchAlbums.TabIndex = 5
        Me.Button_FetchAlbums.Text = "Fetch Albums"
        Me.Button_FetchAlbums.UseVisualStyleBackColor = True
        '
        'Label_Artist
        '
        Me.Label_Artist.AutoSize = True
        Me.Label_Artist.Location = New System.Drawing.Point(6, 19)
        Me.Label_Artist.Name = "Label_Artist"
        Me.Label_Artist.Size = New System.Drawing.Size(30, 13)
        Me.Label_Artist.TabIndex = 6
        Me.Label_Artist.Text = "Artist"
        '
        'Label_Country
        '
        Me.Label_Country.AutoSize = True
        Me.Label_Country.Location = New System.Drawing.Point(6, 49)
        Me.Label_Country.Name = "Label_Country"
        Me.Label_Country.Size = New System.Drawing.Size(43, 13)
        Me.Label_Country.TabIndex = 7
        Me.Label_Country.Text = "Country"
        '
        'Label_Gernre
        '
        Me.Label_Gernre.AutoSize = True
        Me.Label_Gernre.Location = New System.Drawing.Point(6, 22)
        Me.Label_Gernre.Name = "Label_Gernre"
        Me.Label_Gernre.Size = New System.Drawing.Size(36, 13)
        Me.Label_Gernre.TabIndex = 8
        Me.Label_Gernre.Text = "Genre"
        '
        'Label_Year
        '
        Me.Label_Year.AutoSize = True
        Me.Label_Year.Location = New System.Drawing.Point(6, 76)
        Me.Label_Year.Name = "Label_Year"
        Me.Label_Year.Size = New System.Drawing.Size(29, 13)
        Me.Label_Year.TabIndex = 9
        Me.Label_Year.Text = "Year"
        '
        'GroupBox_ArtistSearch
        '
        Me.GroupBox_ArtistSearch.Controls.Add(Me.Label_Artist)
        Me.GroupBox_ArtistSearch.Controls.Add(Me.TextBoxArtist)
        Me.GroupBox_ArtistSearch.Location = New System.Drawing.Point(12, 46)
        Me.GroupBox_ArtistSearch.Name = "GroupBox_ArtistSearch"
        Me.GroupBox_ArtistSearch.Size = New System.Drawing.Size(277, 106)
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
        Me.GroupBox_CustomSearch.Location = New System.Drawing.Point(326, 46)
        Me.GroupBox_CustomSearch.Name = "GroupBox_CustomSearch"
        Me.GroupBox_CustomSearch.Size = New System.Drawing.Size(277, 106)
        Me.GroupBox_CustomSearch.TabIndex = 11
        Me.GroupBox_CustomSearch.TabStop = False
        '
        'RadioButton_ArtistSearch
        '
        Me.RadioButton_ArtistSearch.AutoSize = True
        Me.RadioButton_ArtistSearch.Checked = True
        Me.RadioButton_ArtistSearch.Location = New System.Drawing.Point(12, 23)
        Me.RadioButton_ArtistSearch.Name = "RadioButton_ArtistSearch"
        Me.RadioButton_ArtistSearch.Size = New System.Drawing.Size(83, 17)
        Me.RadioButton_ArtistSearch.TabIndex = 12
        Me.RadioButton_ArtistSearch.TabStop = True
        Me.RadioButton_ArtistSearch.Text = "Artist search"
        Me.RadioButton_ArtistSearch.UseVisualStyleBackColor = True
        '
        'RadioButton_CustomSearch
        '
        Me.RadioButton_CustomSearch.AutoSize = True
        Me.RadioButton_CustomSearch.Location = New System.Drawing.Point(326, 23)
        Me.RadioButton_CustomSearch.Name = "RadioButton_CustomSearch"
        Me.RadioButton_CustomSearch.Size = New System.Drawing.Size(95, 17)
        Me.RadioButton_CustomSearch.TabIndex = 13
        Me.RadioButton_CustomSearch.Text = "Custom search"
        Me.RadioButton_CustomSearch.UseVisualStyleBackColor = True
        '
        'Button_CopyAllAsCSV
        '
        Me.Button_CopyAllAsCSV.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button_CopyAllAsCSV.Enabled = False
        Me.Button_CopyAllAsCSV.Location = New System.Drawing.Point(190, 497)
        Me.Button_CopyAllAsCSV.Name = "Button_CopyAllAsCSV"
        Me.Button_CopyAllAsCSV.Size = New System.Drawing.Size(130, 29)
        Me.Button_CopyAllAsCSV.TabIndex = 14
        Me.Button_CopyAllAsCSV.Text = "Copy All Items As CSV"
        Me.Button_CopyAllAsCSV.UseVisualStyleBackColor = True
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar1.Location = New System.Drawing.Point(190, 462)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(266, 29)
        Me.ProgressBar1.TabIndex = 15
        '
        'Label_AlbumCount
        '
        Me.Label_AlbumCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label_AlbumCount.AutoSize = True
        Me.Label_AlbumCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.Label_AlbumCount.Location = New System.Drawing.Point(462, 474)
        Me.Label_AlbumCount.Name = "Label_AlbumCount"
        Me.Label_AlbumCount.Size = New System.Drawing.Size(20, 17)
        Me.Label_AlbumCount.TabIndex = 16
        Me.Label_AlbumCount.Text = "..."
        '
        'Button_CopyCheckedItems
        '
        Me.Button_CopyCheckedItems.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button_CopyCheckedItems.Enabled = False
        Me.Button_CopyCheckedItems.Location = New System.Drawing.Point(326, 497)
        Me.Button_CopyCheckedItems.Name = "Button_CopyCheckedItems"
        Me.Button_CopyCheckedItems.Size = New System.Drawing.Size(130, 29)
        Me.Button_CopyCheckedItems.TabIndex = 17
        Me.Button_CopyCheckedItems.Text = "Copy Checked Items"
        Me.Button_CopyCheckedItems.UseVisualStyleBackColor = True
        '
        'Button_CheckAll
        '
        Me.Button_CheckAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button_CheckAll.Enabled = False
        Me.Button_CheckAll.Location = New System.Drawing.Point(12, 497)
        Me.Button_CheckAll.Name = "Button_CheckAll"
        Me.Button_CheckAll.Size = New System.Drawing.Size(83, 29)
        Me.Button_CheckAll.TabIndex = 19
        Me.Button_CheckAll.Text = "Check All"
        Me.Button_CheckAll.UseVisualStyleBackColor = True
        '
        'Button_UncheckAll
        '
        Me.Button_UncheckAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button_UncheckAll.Enabled = False
        Me.Button_UncheckAll.Location = New System.Drawing.Point(101, 497)
        Me.Button_UncheckAll.Name = "Button_UncheckAll"
        Me.Button_UncheckAll.Size = New System.Drawing.Size(83, 29)
        Me.Button_UncheckAll.TabIndex = 20
        Me.Button_UncheckAll.Text = "Uncheck All"
        Me.Button_UncheckAll.UseVisualStyleBackColor = True
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button_Cancel.Enabled = False
        Me.Button_Cancel.Location = New System.Drawing.Point(101, 462)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(83, 29)
        Me.Button_Cancel.TabIndex = 21
        Me.Button_Cancel.Text = "CANCEL"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(612, 532)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.Button_UncheckAll)
        Me.Controls.Add(Me.Button_CheckAll)
        Me.Controls.Add(Me.Button_CopyCheckedItems)
        Me.Controls.Add(Me.Label_AlbumCount)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.Button_CopyAllAsCSV)
        Me.Controls.Add(Me.RadioButton_CustomSearch)
        Me.Controls.Add(Me.GroupBox_CustomSearch)
        Me.Controls.Add(Me.RadioButton_ArtistSearch)
        Me.Controls.Add(Me.GroupBox_ArtistSearch)
        Me.Controls.Add(Me.Button_FetchAlbums)
        Me.Controls.Add(Me.ListView_Albums)
        Me.Name = "Main"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FHM Crawler - Demo by Elektro"
        Me.GroupBox_ArtistSearch.ResumeLayout(False)
        Me.GroupBox_ArtistSearch.PerformLayout()
        Me.GroupBox_CustomSearch.ResumeLayout(False)
        Me.GroupBox_CustomSearch.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

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
End Class
