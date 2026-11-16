' ***********************************************************************
' Author   : ElektroStudios
' Modified : 15-November-2026
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " PageCrawlBeginEventArgs "

Namespace FHM

    ''' <summary>
    ''' Represents the event data of the <see cref="Crawler.PageCrawlBegin"/> event.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed NuGet packages:
    ''' <para></para>
    ''' <see href="https://www.nuget.org/packages/HtmlAgilityPack">HtmlAgilityPack</see>
    ''' </remarks>
    Public NotInheritable Class PageCrawlBeginEventArgs : Inherits EventArgs

#Region " Properties "

        ''' <summary>
        ''' Gets the search query used.
        ''' </summary>
        '''
        ''' <value>
        ''' The search query used.
        ''' </value>
        Public ReadOnly Property SearchQuery As SearchQuery

        ''' <summary>
        ''' Gets the index of the search page being crawled.
        ''' </summary>
        '''
        ''' <value>
        ''' The index of the search page being crawled.
        ''' </value>
        Public ReadOnly Property SearchPage As Integer

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="PageCrawlBeginEventArgs"/> class.
        ''' </summary>
        '''
        ''' <param name="searchQuery">
        ''' The search query used.
        ''' </param>
        '''
        ''' <param name="searchPage">
        ''' The index of the search page.
        ''' </param>
        '''
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when <paramref name="searchQuery"/> is <see langword="Nothing"/>.
        ''' </exception>
        '''
        ''' <exception cref="ArgumentOutOfRangeException">
        ''' Thrown when <paramref name="searchPage"/> is negative.
        ''' </exception>
        Public Sub New(searchQuery As SearchQuery, searchPage As Integer)
            If searchQuery Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(searchQuery))
            End If
            If searchPage < 0 Then
                Throw New ArgumentOutOfRangeException(paramName:=NameOf(searchPage), actualValue:=searchPage, message:="The search page index cannot be negative.")
            End If

            Me.SearchQuery = searchQuery
            Me.SearchPage = searchPage
        End Sub

#End Region

    End Class

End Namespace

#End Region
