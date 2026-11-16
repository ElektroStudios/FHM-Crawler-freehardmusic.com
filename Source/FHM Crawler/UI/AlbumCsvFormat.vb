#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Text

Imports FHM

#End Region

#Region " AlbumCsvFormat "

Namespace FHMCrawler.UI

    ''' <summary>
    ''' Defines the <c>CSV</c> format of the album list exported by the application,
    ''' and builds the album list rows so that they can be exported without breaking the <c>CSV</c> columns.
    ''' </summary>
    Friend NotInheritable Class AlbumCsvFormat

#Region " Constants "

        ''' <summary>
        ''' The <c>CSV</c> field delimiter.
        ''' </summary>
        Friend Const Delimiter As Char = ";"c

        ''' <summary>
        ''' The character that replaces the <see cref="AlbumCsvFormat.Delimiter"/>
        ''' in artist names and album titles.
        ''' </summary>
        Friend Const DelimiterReplacement As Char = ","c

        ''' <summary>
        ''' The separator of the download links in the download links column.
        ''' </summary>
        Friend Const DownloadLinkSeparator As String = ", "

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Prevents a default instance of the <see cref="AlbumCsvFormat"/> class from being created.
        ''' </summary>
        Private Sub New()
        End Sub

#End Region

#Region " Friend Methods "

        ''' <summary>
        ''' Sanitizes an artist name or an album title, so that it never contains
        ''' the <see cref="AlbumCsvFormat.Delimiter"/> nor line breaks.
        ''' <para></para>
        ''' The delimiter is replaced with <see cref="AlbumCsvFormat.DelimiterReplacement"/>,
        ''' line breaks and tabs are replaced with a white-space, and consecutive white-spaces are collapsed.
        ''' </summary>
        '''
        ''' <param name="value">
        ''' The artist name or the album title.
        ''' </param>
        '''
        ''' <returns>
        ''' The sanitized value.
        ''' </returns>
        '''
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when <paramref name="value"/> is <see langword="Nothing"/>.
        ''' </exception>
        Friend Shared Function SanitizeText(value As String) As String
            If value Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(value))
            End If

            Dim sb As New StringBuilder(value.Length)
            Dim previousIsWhiteSpace As Boolean = False

            For Each c As Char In value
                Dim current As Char = c
                If current = AlbumCsvFormat.Delimiter Then
                    current = AlbumCsvFormat.DelimiterReplacement
                ElseIf current = ControlChars.Cr OrElse current = ControlChars.Lf OrElse current = ControlChars.Tab Then
                    current = " "c
                End If

                Dim isWhiteSpace As Boolean = (current = " "c)
                If isWhiteSpace AndAlso previousIsWhiteSpace Then
                    Continue For
                End If

                sb.Append(current)
                previousIsWhiteSpace = isWhiteSpace
            Next c

            Return sb.ToString().Trim()
        End Function

        ''' <summary>
        ''' Gets the cells of the album list row of the specified album:
        ''' artist name, album title and download links.
        ''' <para></para>
        ''' The artist name and the album title are sanitized with <see cref="AlbumCsvFormat.SanitizeText"/>.
        ''' </summary>
        '''
        ''' <param name="album">
        ''' The album.
        ''' </param>
        '''
        ''' <returns>
        ''' The cells of the row.
        ''' </returns>
        '''
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when <paramref name="album"/> is <see langword="Nothing"/>.
        ''' </exception>
        Friend Shared Function GetRowCells(album As AlbumInfo) As String()
            If album Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(album))
            End If

            Return {
                AlbumCsvFormat.SanitizeText(album.Artist),
                AlbumCsvFormat.SanitizeText(album.Title),
                $"{{{String.Join(AlbumCsvFormat.DownloadLinkSeparator, album.DownloadLinks)}}}"
            }
        End Function

#End Region

    End Class

End Namespace

#End Region
