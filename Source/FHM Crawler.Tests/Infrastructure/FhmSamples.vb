#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq
Imports System.Text
Imports System.Web

#End Region

#Region " FhmSamples "

Namespace Infrastructure

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Builds HTML and XML documents that mimic the markup of the FHM website,
    ''' following the XPath expressions used by the crawler.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Friend Module FhmSamples

#Region " Search Results Pages "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Builds a search results page.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="summary">
        ''' The text of the search result summary (e.g. <c>"Found 12 entries"</c>),
        ''' or <see langword="Nothing"/> to omit the summary (as the website does when there are no results).
        ''' </param>
        '''
        ''' <param name="albumTables">
        ''' The album tables (see <see cref="FhmSamples.AlbumTable"/>).
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The HTML source code.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Friend Function SearchPage(summary As String, ParamArray albumTables As String()) As String
            Dim sb As New StringBuilder()
            sb.AppendLine("<html><head><title>FHM</title></head><body>")
            sb.AppendLine("<div id='content'><div id='content_container'><div class='floatbox'>")
            If summary IsNot Nothing Then
                sb.AppendLine("<table><tr><td>Search</td></tr><tr><td>")
                sb.AppendLine($"{vbTab}{summary}{vbLf}{vbTab}")
                sb.AppendLine("</td></tr></table>")
            End If
            For Each albumTable As String In albumTables
                sb.AppendLine(albumTable)
            Next albumTable
            sb.AppendLine("</div></div></div>")
            sb.AppendLine("</body></html>")
            Return sb.ToString()
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Builds a search results page whose summary reports the specified album count.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="albumCount">
        ''' The album count.
        ''' </param>
        '''
        ''' <param name="albumTables">
        ''' The album tables (see <see cref="FhmSamples.AlbumTable"/>).
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The HTML source code.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Friend Function SearchPage(albumCount As Integer, ParamArray albumTables As String()) As String
            Return FhmSamples.SearchPage($"Found {albumCount.ToString(CultureInfo.InvariantCulture)} entries", albumTables)
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Builds the table of an album listed in a search results page.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="artist">
        ''' The raw (HTML) artist name, or <see langword="Nothing"/> to omit the artist node.
        ''' </param>
        '''
        ''' <param name="title">
        ''' The raw (HTML attribute) album title, or <see langword="Nothing"/> to omit the title node.
        ''' </param>
        '''
        ''' <param name="href">
        ''' The raw (HTML attribute) details link, or <see langword="Nothing"/> to omit the link node.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The HTML source code of the album table.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Friend Function AlbumTable(artist As String, title As String, href As String) As String
            Dim sb As New StringBuilder()
            sb.AppendLine("<table class='vicard'><tr><td>")
            If title IsNot Nothing Then
                sb.AppendLine($"<p class='sobi2ItemTitle'><a title=""{title}"" href=""#"">{title}</a></p>")
            End If
            If artist IsNot Nothing Then
                sb.AppendLine($"<span class='sobi2Listing_field_band'>{artist}</span>")
            End If
            If href IsNot Nothing Then
                sb.AppendLine($"<table class='vicard2'><tr><td><a href=""{href}"">Details</a></td></tr></table>")
            End If
            sb.AppendLine("</td></tr></table>")
            Return sb.ToString()
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Builds the table of an album with a well-formed details link for the specified album identifier.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="artist">
        ''' The raw (HTML) artist name.
        ''' </param>
        '''
        ''' <param name="title">
        ''' The raw (HTML attribute) album title.
        ''' </param>
        '''
        ''' <param name="sobi2Id">
        ''' The album identifier.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The HTML source code of the album table.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Friend Function AlbumTableWithId(artist As String, title As String, sobi2Id As String) As String
            Return FhmSamples.AlbumTable(artist, title, FhmSamples.DetailsHref(sobi2Id))
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Builds an absolute album details link with HTML-encoded ampersands, as the FHM website renders it.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sobi2Id">
        ''' The album identifier.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The details link.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Friend Function DetailsHref(sobi2Id As String) As String
            Return $"http://freehardmusic.com/index.php?option=com_sobi2&amp;sobi2Task=sobi2Details&amp;catid=0&amp;sobi2Id={sobi2Id}&amp;Itemid=13"
        End Function

#End Region

#Region " Download Links Documents "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Builds a download links document (root <c>&lt;rev&gt;</c>) that fills the first slots
        ''' (<c>&lt;msg&gt;</c>, <c>&lt;msg2&gt;</c>, ...) with the specified values.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="values">
        ''' The slot values.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The XML document.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Friend Function DownloadLinksXml(ParamArray values As String()) As String
            Dim sb As New StringBuilder()
            sb.Append("<?xml version=""1.0"" encoding=""utf-8""?><rev>")
            For index As Integer = 0 To (values.Length - 1)
                Dim elementName As String = If(index = 0, "msg", $"msg{(index + 1).ToString(CultureInfo.InvariantCulture)}")
                sb.Append($"<{elementName}>{HttpUtility.HtmlEncode(values(index))}</{elementName}>")
            Next index
            sb.Append("</rev>")
            Return sb.ToString()
        End Function

#End Region

    End Module

End Namespace

#End Region
