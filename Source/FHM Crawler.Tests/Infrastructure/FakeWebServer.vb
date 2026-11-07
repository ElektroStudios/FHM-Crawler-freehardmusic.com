#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Globalization
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Web

#End Region

#Region " FakeWebServer "

Namespace Infrastructure

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' An in-memory replacement of the FHM website, used to feed the crawler without network access.
    ''' <para></para>
    ''' Responses are matched against the requested <see cref="Uri"/> in registration order;
    ''' an unmatched request fails the test with an <see cref="InvalidOperationException"/>.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Friend NotInheritable Class FakeWebServer

#Region " Private Fields "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The registered responders (request predicate and response factory).
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private ReadOnly responders As New List(Of KeyValuePair(Of Func(Of Uri, Boolean), Func(Of Uri, Task(Of String))))

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The requested addresses, in request order.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private ReadOnly requests As New List(Of Uri)

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The object used to synchronize the access to the collections.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private ReadOnly syncRoot As New Object()

#End Region

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a snapshot of the requested addresses, in request order.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Friend ReadOnly Property RequestedUris As IReadOnlyList(Of Uri)
            Get
                SyncLock Me.syncRoot
                    Return Me.requests.ToArray()
                End SyncLock
            End Get
        End Property

#End Region

#Region " Friend Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Registers a fixed response for the requests that match the specified predicate.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="predicate">
        ''' The request predicate.
        ''' </param>
        '''
        ''' <param name="content">
        ''' The response content.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        Friend Sub AddResponse(predicate As Func(Of Uri, Boolean), content As String)
            Me.AddResponder(predicate, Function(address As Uri) Task.FromResult(content))
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Registers a response factory for the requests that match the specified predicate.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="predicate">
        ''' The request predicate.
        ''' </param>
        '''
        ''' <param name="responder">
        ''' The response factory.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        Friend Sub AddResponder(predicate As Func(Of Uri, Boolean), responder As Func(Of Uri, Task(Of String)))
            If predicate Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(predicate))
            End If
            If responder Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(responder))
            End If

            SyncLock Me.syncRoot
                Me.responders.Add(New KeyValuePair(Of Func(Of Uri, Boolean), Func(Of Uri, Task(Of String)))(predicate, responder))
            End SyncLock
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Downloads the content of the specified address. Pass this method to the crawler test constructor.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="address">
        ''' The requested address.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The registered response.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <exception cref="InvalidOperationException">
        ''' Thrown when no registered responder matches the request.
        ''' </exception>
        ''' ----------------------------------------------------------------------------------------------------
        Friend Function DownloadStringAsync(address As Uri) As Task(Of String)
            Dim responder As Func(Of Uri, Task(Of String)) = Nothing

            SyncLock Me.syncRoot
                Me.requests.Add(address)
                For Each pair As KeyValuePair(Of Func(Of Uri, Boolean), Func(Of Uri, Task(Of String))) In Me.responders
                    If pair.Key.Invoke(address) Then
                        responder = pair.Value
                        Exit For
                    End If
                Next pair
            End SyncLock

            If responder Is Nothing Then
                Throw New InvalidOperationException($"Unexpected request: {address}")
            End If

            Return responder.Invoke(address)
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Counts the requested search results pages with the specified page index.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="pageIndex">
        ''' The page index.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The amount of requests.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Friend Function CountSearchPageRequests(pageIndex As Integer) As Integer
            Return Enumerable.Count(Me.RequestedUris, Function(address As Uri) FakeWebServer.IsSearchPage(address, pageIndex))
        End Function

#End Region

#Region " Request Predicates "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a predicate that matches the search results page with the specified page index.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="pageIndex">
        ''' The page index.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The predicate.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Friend Shared Function SearchPage(pageIndex As Integer) As Func(Of Uri, Boolean)
            Return Function(address As Uri) FakeWebServer.IsSearchPage(address, pageIndex)
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a predicate that matches the download links document of the specified album.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sobi2Id">
        ''' The album identifier.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The predicate.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Friend Shared Function DownloadLinks(sobi2Id As String) As Func(Of Uri, Boolean)
            Return Function(address As Uri)
                       If Not address.AbsolutePath.Equals("/index2.php", StringComparison.OrdinalIgnoreCase) Then
                           Return False
                       End If
                       Dim query As NameValueCollection = HttpUtility.ParseQueryString(address.Query)
                       Return String.Equals(query("sobiid"), sobi2Id, StringComparison.Ordinal)
                   End Function
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Determines whether the specified address is the search results page with the specified page index.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="address">
        ''' The address.
        ''' </param>
        '''
        ''' <param name="pageIndex">
        ''' The page index.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see langword="True"/> if it is the search results page; otherwise, <see langword="False"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Friend Shared Function IsSearchPage(address As Uri, pageIndex As Integer) As Boolean
            If Not address.AbsolutePath.Equals("/index.php", StringComparison.OrdinalIgnoreCase) Then
                Return False
            End If
            Dim query As NameValueCollection = HttpUtility.ParseQueryString(address.Query)
            Return String.Equals(query("SobiSearchPage"), pageIndex.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal)
        End Function

#End Region

    End Class

End Namespace

#End Region
