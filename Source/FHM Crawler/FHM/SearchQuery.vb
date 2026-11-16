' ***********************************************************************
' Author   : ElektroStudios
' Modified : 15-November-2026
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Specialized
Imports System.Diagnostics
Imports System.Globalization

Imports DevCase.Core.Extensions.NameValueCollection

#End Region

#Region " SearchQuery "

Namespace FHM

    ''' <summary>
    ''' Represents a search query of the http://freehardmusic.com/ website,
    ''' that is managed by the <see cref="Crawler.FetchAlbums()"/>
    ''' and <see cref="Crawler.FetchAlbumsAsync()"/> methods.
    ''' <para></para>
    ''' Note that a search query can be performed in two different ways:
    ''' <para></para>
    ''' 1. An artist-name based search (<see cref="SearchQuery.Artist"/>).
    ''' <para></para>
    ''' 2. A non-artist name based search. That is, a custom search based on country (<see cref="SearchQuery.Country"/>),
    ''' genre (<see cref="SearchQuery.Genre"/>) or year criterias (<see cref="SearchQuery.Year"/>);
    ''' this kind of search can combine the three mentioned criterias, but not the artist name (<see cref="SearchQuery.Artist"/>).
    ''' <para></para>
    ''' <para></para>
    ''' So, for an artist-name based search,
    ''' the value of <see cref="SearchQuery.Country"/>, <see cref="SearchQuery.Genre"/>
    ''' and <see cref="SearchQuery.Year"/> properties will always be set to "all".
    ''' <para></para>
    ''' <para></para>
    ''' And for a country, genre or year based search,
    ''' the value of <see cref="SearchQuery.Artist"/> property will always be set to an empty string.
    ''' </summary>
    Public NotInheritable Class SearchQuery

#Region " Constants "

        ''' <summary>
        ''' The value that matches any country, genre or year.
        ''' </summary>
        Public Const AnyValue As String = "all"

#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets or sets the artist name.
        ''' </summary>
        '''
        ''' <value>
        ''' The artist name.
        ''' <para></para>
        ''' An empty string means that this is not an artist-name based search.
        ''' </value>
        '''
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when the value is <see langword="Nothing"/>.
        ''' </exception>
        '''
        ''' <exception cref="ArgumentException">
        ''' Thrown when the value is not empty, and the value of <see cref="SearchQuery.Country"/>,
        ''' <see cref="SearchQuery.Genre"/> or <see cref="SearchQuery.Year"/> property is not "all".
        ''' </exception>
        Public Property Artist As String
            Get
                Return Me.artistB
            End Get
            <DebuggerStepThrough>
            Set(value As String)
                If value Is Nothing Then
                    Throw New ArgumentNullException(paramName:=NameOf(value))
                End If

                ' Clearing the artist name is always allowed: it does not turn this into an artist-name based search.
                If value.Length <> 0 AndAlso
                   (Not SearchQuery.IsAnyValue(Me.countryB) OrElse
                    Not SearchQuery.IsAnyValue(Me.genreB) OrElse
                    Not SearchQuery.IsAnyValue(Me.yearB)) Then

                    Throw New ArgumentException("To perform an artist-name based search, you must set the value of Country, Genre and Year properties to ""all"" before setting the Artist property.", paramName:=NameOf(value))
                End If
                Me.artistB = value
            End Set
        End Property

        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The artist name.
        ''' </summary>
        Private artistB As String

        ''' <summary>
        ''' Gets or sets the country of the band/artist.
        ''' </summary>
        '''
        ''' <value>
        ''' The country of the band/artist.
        ''' </value>
        '''
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when the value is <see langword="Nothing"/>.
        ''' </exception>
        '''
        ''' <exception cref="ArgumentException">
        ''' Thrown when the value is not "all" and the value of <see cref="SearchQuery.Artist"/> property is not an empty string.
        ''' </exception>
        Public Property Country As String
            Get
                Return Me.countryB
            End Get
            <DebuggerStepThrough>
            Set(value As String)
                SearchQuery.ValidateCustomCriteria(value, Me.artistB, "country")
                Me.countryB = value
            End Set
        End Property

        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The country of the band/artist.
        ''' </summary>
        Private countryB As String

        ''' <summary>
        ''' Gets or sets the music genre.
        ''' </summary>
        '''
        ''' <value>
        ''' The music genre.
        ''' </value>
        '''
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when the value is <see langword="Nothing"/>.
        ''' </exception>
        '''
        ''' <exception cref="ArgumentException">
        ''' Thrown when the value is not "all" and the value of <see cref="SearchQuery.Artist"/> property is not an empty string.
        ''' </exception>
        Public Property Genre As String
            Get
                Return Me.genreB
            End Get
            <DebuggerStepThrough>
            Set(value As String)
                SearchQuery.ValidateCustomCriteria(value, Me.artistB, "genre")
                Me.genreB = value
            End Set
        End Property

        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The music genre.
        ''' </summary>
        Private genreB As String

        ''' <summary>
        ''' Gets or sets the year that the album has been released.
        ''' </summary>
        '''
        ''' <value>
        ''' The year that the album has been released.
        ''' </value>
        '''
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when the value is <see langword="Nothing"/>.
        ''' </exception>
        '''
        ''' <exception cref="ArgumentException">
        ''' Thrown when the value is not "all" and the value of <see cref="SearchQuery.Artist"/> property is not an empty string.
        ''' </exception>
        Public Property Year As String
            Get
                Return Me.yearB
            End Get
            <DebuggerStepThrough>
            Set(value As String)
                SearchQuery.ValidateCustomCriteria(value, Me.artistB, "year")
                Me.yearB = value
            End Set
        End Property

        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The year that the album has been released.
        ''' </summary>
        Private yearB As String

        ''' <summary>
        ''' Gets the <see cref="Uri"/> that represents this search query (the first search page).
        ''' </summary>
        '''
        ''' <value>
        ''' The <see cref="Uri"/> that represents this search query.
        ''' </value>
        Public ReadOnly Property Uri As Uri
            Get
                Return Me.Uri(searchPage:=0)
            End Get
        End Property

        ''' <summary>
        ''' Gets the <see cref="Uri"/> that represents this search query.
        ''' </summary>
        '''
        ''' <param name="searchPage">
        ''' The zero-based index of the search page parameter.
        ''' </param>
        '''
        ''' <value>
        ''' The <see cref="Uri"/> that represents this search query.
        ''' </value>
        '''
        ''' <exception cref="ArgumentOutOfRangeException">
        ''' Thrown when <paramref name="searchPage"/> is negative.
        ''' </exception>
        Public ReadOnly Property Uri(searchPage As Integer) As Uri
            <DebuggerStepThrough>
            Get
                If searchPage < 0 Then
                    Throw New ArgumentOutOfRangeException(paramName:=NameOf(searchPage), actualValue:=searchPage, message:="Positive integer value is required.")
                End If

                ' The parameters (and their order) that the FHM website sends from its search form.
                Dim params As New NameValueCollection From {
                    {"field_band", Me.Artist},
                    {"field_country", Me.Country},
                    {"field_genre", Me.Genre},
                    {"field_year", Me.Year},
                    {"field_format", "0"},
                    {"option", "com_sobi2"},
                    {"search", "Search"},
                    {"searchphrase", "exact"},
                    {"sobi2Search", ""},
                    {"sobi2Task", "axSearch"},
                    {"SobiCatSelected_0", "0"},
                    {"sobiCid", "0"},
                    {"reset", "2"},
                    {"Itemid", "13"},
                    {"SobiSearchPage", searchPage.ToString(CultureInfo.InvariantCulture)}
                }

                Return New Uri(params.ToQueryString(Crawler.UriIndex))
            End Get
        End Property

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="SearchQuery"/> class
        ''' for an artist-name based search.
        ''' </summary>
        '''
        ''' <param name="artist">
        ''' The artist name.
        ''' </param>
        '''
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when <paramref name="artist"/> is <see langword="Nothing"/>.
        ''' </exception>
        Public Sub New(artist As String)
            If artist Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(artist))
            End If

            Me.artistB = artist
            Me.genreB = SearchQuery.AnyValue
            Me.countryB = SearchQuery.AnyValue
            Me.yearB = SearchQuery.AnyValue
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="SearchQuery"/> class
        ''' for a country, genre or year based search.
        ''' </summary>
        '''
        ''' <param name="genre">
        ''' The music genre. Default value is: "all"
        ''' </param>
        '''
        ''' <param name="country">
        ''' The country of the band/artist. Default value is: "all"
        ''' </param>
        '''
        ''' <param name="year">
        ''' The year that the album has been released. Default value is: "all"
        ''' </param>
        '''
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when <paramref name="genre"/>, <paramref name="country"/> or <paramref name="year"/> is <see langword="Nothing"/>.
        ''' </exception>
        Public Sub New(Optional genre As String = SearchQuery.AnyValue,
                       Optional country As String = SearchQuery.AnyValue,
                       Optional year As String = SearchQuery.AnyValue)

            If genre Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(genre))
            End If
            If country Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(country))
            End If
            If year Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(year))
            End If

            Me.artistB = ""
            Me.genreB = genre
            Me.countryB = country
            Me.yearB = year
        End Sub

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Resets the current search query to its default values.
        ''' </summary>
        <DebuggerStepThrough>
        Public Sub Reset()
            ' The artist name is cleared first; the custom criteria can always be set to "all".
            Me.Artist = ""
            Me.Country = SearchQuery.AnyValue
            Me.Genre = SearchQuery.AnyValue
            Me.Year = SearchQuery.AnyValue
        End Sub

        ''' <summary>
        ''' Returns a <see cref="String"/> that represents the search query.
        ''' </summary>
        '''
        ''' <returns>
        ''' A <see cref="String"/> that represents the search query.
        ''' </returns>
        Public Overrides Function ToString() As String

            Dim params As New NameValueCollection From {
                {"field_band", Me.Artist},
                {"field_country", Me.Country},
                {"field_genre", Me.Genre},
                {"field_year", Me.Year}
            }

            Return params.ToQueryString()

        End Function

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Determines whether the specified criteria value matches any country, genre or year ("all").
        ''' </summary>
        '''
        ''' <param name="value">
        ''' The criteria value.
        ''' </param>
        '''
        ''' <returns>
        ''' <see langword="True"/> if the value is "all" (case-insensitive); otherwise, <see langword="False"/>.
        ''' </returns>
        Private Shared Function IsAnyValue(value As String) As Boolean
            Return value.Equals(SearchQuery.AnyValue, StringComparison.OrdinalIgnoreCase)
        End Function

        ''' <summary>
        ''' Validates the value of a country, genre or year criteria.
        ''' </summary>
        '''
        ''' <param name="value">
        ''' The criteria value.
        ''' </param>
        '''
        ''' <param name="artist">
        ''' The current artist name.
        ''' </param>
        '''
        ''' <param name="criteriaName">
        ''' The name of the criteria, for the exception message ("country", "genre" or "year").
        ''' </param>
        '''
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when <paramref name="value"/> is <see langword="Nothing"/>.
        ''' </exception>
        '''
        ''' <exception cref="ArgumentException">
        ''' Thrown when <paramref name="value"/> is not "all" and <paramref name="artist"/> is not an empty string.
        ''' </exception>
        <DebuggerStepThrough>
        Private Shared Sub ValidateCustomCriteria(value As String, artist As String, criteriaName As String)
            If value Is Nothing Then
                Throw New ArgumentNullException(paramName:="value")
            End If

            If Not SearchQuery.IsAnyValue(value) AndAlso artist.Length <> 0 Then
                Throw New ArgumentException($"To perform a {criteriaName} based search, you must set the value of Artist property to an empty string.", paramName:="value")
            End If
        End Sub

#End Region

    End Class

End Namespace

#End Region
