#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Linq
Imports System.Net
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Web

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports FHM

Imports FHMCrawler.Tests.Infrastructure

#End Region

#Region " CrawlerFetchTests "

Namespace FHMTests

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Tests the asynchronous fetch workflow of the <see cref="Crawler"/> against an in-memory fake of the FHM website.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestClass>
    Public NotInheritable Class CrawlerFetchTests

#Region " GetAlbumCount "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The album count is read from the first search results page.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Async Function GetAlbumCountAsync_ReadsTheFirstSearchPage() As Task
            Dim server As New FakeWebServer()
            server.AddResponse(FakeWebServer.SearchPage(0), FhmSamples.SearchPage(37))
            Dim fhmCrawler As Crawler = CrawlerFetchTests.CreateCrawler(server)

            Dim albumCount As Integer = Await fhmCrawler.GetAlbumCountAsync().ConfigureAwait(False)

            Assert.AreEqual(37, albumCount)
            Assert.AreEqual(1, server.RequestedUris.Count)
            Assert.AreEqual(1, server.CountSearchPageRequests(0))
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The synchronous wrapper returns the same album count.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub GetAlbumCount_Synchronous_ReturnsTheAlbumCount()
            Dim server As New FakeWebServer()
            server.AddResponse(FakeWebServer.SearchPage(0), FhmSamples.SearchPage(5))
            Dim fhmCrawler As Crawler = CrawlerFetchTests.CreateCrawler(server)

            Assert.AreEqual(5, fhmCrawler.GetAlbumCount())
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The search request carries the current search filter.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Async Function GetAlbumCountAsync_UsesTheCurrentSearchFilter() As Task
            Dim server As New FakeWebServer()
            server.AddResponse(FakeWebServer.SearchPage(0), FhmSamples.SearchPage(1))
            Dim fhmCrawler As Crawler = CrawlerFetchTests.CreateCrawler(server)
            fhmCrawler.SearchFilter.Artist = "Accept"

            Await fhmCrawler.GetAlbumCountAsync().ConfigureAwait(False)

            Dim query As NameValueCollection = HttpUtility.ParseQueryString(server.RequestedUris.Single().Query)
            Assert.AreEqual("Accept", query("field_band"))
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A download delegate that returns a null string fails loudly instead of being parsed.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Async Function GetAlbumCountAsync_NullContent_ThrowsInvalidOperationException() As Task
            Dim server As New FakeWebServer()
            server.AddResponder(FakeWebServer.SearchPage(0), Function(address As Uri) Task.FromResult(Of String)(Nothing))
            Dim fhmCrawler As Crawler = CrawlerFetchTests.CreateCrawler(server)

            Await Assert.ThrowsExactlyAsync(Of InvalidOperationException)(Function() fhmCrawler.GetAlbumCountAsync()).ConfigureAwait(False)
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A download delegate that returns a null task fails loudly.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Async Function GetAlbumCountAsync_NullTask_ThrowsInvalidOperationException() As Task
            Dim fhmCrawler As New Crawler(New CookieContainer(), Function(address As Uri) Nothing)

            Await Assert.ThrowsExactlyAsync(Of InvalidOperationException)(Function() fhmCrawler.GetAlbumCountAsync()).ConfigureAwait(False)
        End Function

#End Region

#Region " FetchAlbumsAsync "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A search without results completes successfully without crawling any page.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Async Function FetchAlbumsAsync_NoResults_CompletesWithoutCrawlingPages() As Task
            Dim server As New FakeWebServer()
            server.AddResponse(FakeWebServer.SearchPage(0), FhmSamples.SearchPage(summary:=Nothing))
            Dim fhmCrawler As Crawler = CrawlerFetchTests.CreateCrawler(server)
            Dim recorder As New CrawlEventRecorder(fhmCrawler)

            Dim completed As Boolean = Await fhmCrawler.FetchAlbumsAsync().ConfigureAwait(False)

            Assert.IsTrue(completed)
            Assert.AreEqual(0, recorder.Sequence.Length)
            Assert.AreEqual(1, server.RequestedUris.Count)
            Assert.IsFalse(fhmCrawler.IsFetching)
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Every page is crawled in order, raising a begin/end event pair per page,
        ''' and the albums carry their parsed download links.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Async Function FetchAlbumsAsync_CrawlsEveryPageAndDeliversAlbums() As Task
            Dim server As New FakeWebServer()
            server.AddResponse(FakeWebServer.SearchPage(0), FhmSamples.SearchPage(12,
                                                                                  FhmSamples.AlbumTableWithId("dio", "holy diver", "1"),
                                                                                  FhmSamples.AlbumTableWithId("rainbow", "rising", "2")))
            server.AddResponse(FakeWebServer.SearchPage(1), FhmSamples.SearchPage(12,
                                                                                  FhmSamples.AlbumTableWithId("scorpions", "blackout", "3")))
            server.AddResponse(FakeWebServer.DownloadLinks("1"), FhmSamples.DownloadLinksXml("http://host/dio-1", "http://host/dio-2"))
            server.AddResponse(FakeWebServer.DownloadLinks("2"), FhmSamples.DownloadLinksXml("http://host/rainbow"))
            server.AddResponse(FakeWebServer.DownloadLinks("3"), FhmSamples.DownloadLinksXml())
            Dim fhmCrawler As Crawler = CrawlerFetchTests.CreateCrawler(server)
            Dim recorder As New CrawlEventRecorder(fhmCrawler)

            Dim completed As Boolean = Await fhmCrawler.FetchAlbumsAsync().ConfigureAwait(False)

            Assert.IsTrue(completed)
            CollectionAssert.AreEqual({"Begin:0", "End:0", "Begin:1", "End:1"}, recorder.Sequence)
            Assert.IsTrue(recorder.Senders.All(Function(sender As Object) sender Is fhmCrawler))

            Dim page0 As List(Of AlbumInfo) = recorder.EndPages(0).Albums
            Assert.AreEqual(2, page0.Count)
            Assert.AreEqual("Dio", page0(0).Artist)
            Assert.AreEqual("Holy Diver", page0(0).Album)
            CollectionAssert.AreEqual({"http://host/dio-1", "http://host/dio-2"}, page0(0).DounloadUrls)
            Assert.AreEqual("Rainbow", page0(1).Artist)
            CollectionAssert.AreEqual({"http://host/rainbow"}, page0(1).DounloadUrls)

            Dim page1 As List(Of AlbumInfo) = recorder.EndPages(1).Albums
            Assert.AreEqual(1, page1.Count)
            Assert.AreEqual("Scorpions", page1(0).Artist)
            Assert.AreEqual("Blackout", page1(0).Album)
            Assert.AreEqual(0, page1(0).DounloadUrls.Length)
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' An album count that is an exact multiple of the page size does not request an extra page.
        ''' <para></para>
        ''' Regression: the original code requested <c>(count \ 10) + 1</c> pages; the extra page had no albums
        ''' and crashed the crawler with a <see cref="NullReferenceException"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Async Function FetchAlbumsAsync_ExactMultipleOfPageSize_DoesNotRequestExtraPage() As Task
            Dim server As New FakeWebServer()
            server.AddResponse(FakeWebServer.SearchPage(0), FhmSamples.SearchPage(20))
            server.AddResponse(FakeWebServer.SearchPage(1), FhmSamples.SearchPage(20))
            Dim fhmCrawler As Crawler = CrawlerFetchTests.CreateCrawler(server)
            Dim recorder As New CrawlEventRecorder(fhmCrawler)

            Dim completed As Boolean = Await fhmCrawler.FetchAlbumsAsync().ConfigureAwait(False)

            Assert.IsTrue(completed)
            Assert.AreEqual(0, server.CountSearchPageRequests(2))
            CollectionAssert.AreEqual({"Begin:0", "End:0", "Begin:1", "End:1"}, recorder.Sequence)
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A page that lists no album completes its begin/end event pair with an empty album list.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Async Function FetchAlbumsAsync_PageWithoutAlbums_RaisesEndEventWithEmptyList() As Task
            Dim server As New FakeWebServer()
            server.AddResponse(FakeWebServer.SearchPage(0), FhmSamples.SearchPage(3))
            Dim fhmCrawler As Crawler = CrawlerFetchTests.CreateCrawler(server)
            Dim recorder As New CrawlEventRecorder(fhmCrawler)

            Dim completed As Boolean = Await fhmCrawler.FetchAlbumsAsync().ConfigureAwait(False)

            Assert.IsTrue(completed)
            CollectionAssert.AreEqual({"Begin:0", "End:0"}, recorder.Sequence)
            Assert.AreEqual(0, recorder.EndPages(0).Albums.Count)
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Albums whose download links cannot be retrieved (network error) or parsed (malformed XML,
        ''' unexpected document) are skipped, and the rest of the page is still delivered.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Async Function FetchAlbumsAsync_SkipsAlbumsWithBrokenDownloadLinks() As Task
            Dim server As New FakeWebServer()
            server.AddResponse(FakeWebServer.SearchPage(0), FhmSamples.SearchPage(4,
                                                                                  FhmSamples.AlbumTableWithId("Network", "Error", "1"),
                                                                                  FhmSamples.AlbumTableWithId("Malformed", "Xml", "2"),
                                                                                  FhmSamples.AlbumTableWithId("Unexpected", "Document", "3"),
                                                                                  FhmSamples.AlbumTableWithId("Valid", "Album", "4")))
            server.AddResponder(FakeWebServer.DownloadLinks("1"), Function(address As Uri) Task.FromException(Of String)(New WebException("The operation has timed out.")))
            server.AddResponse(FakeWebServer.DownloadLinks("2"), "<html><body>Internal Server Error")
            server.AddResponse(FakeWebServer.DownloadLinks("3"), "<error>Access denied</error>")
            server.AddResponse(FakeWebServer.DownloadLinks("4"), FhmSamples.DownloadLinksXml("http://host/valid"))
            Dim fhmCrawler As Crawler = CrawlerFetchTests.CreateCrawler(server)
            Dim recorder As New CrawlEventRecorder(fhmCrawler)

            Dim completed As Boolean = Await fhmCrawler.FetchAlbumsAsync().ConfigureAwait(False)

            Assert.IsTrue(completed)
            Dim albums As List(Of AlbumInfo) = recorder.EndPages.Single().Albums
            Assert.AreEqual(1, albums.Count)
            Assert.AreEqual("Valid", albums(0).Artist)
            CollectionAssert.AreEqual({"http://host/valid"}, albums(0).DounloadUrls)
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Unexpected errors while downloading the links of an album are not swallowed (fail-fast).
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Async Function FetchAlbumsAsync_UnexpectedAlbumError_Propagates() As Task
            Dim server As New FakeWebServer()
            server.AddResponse(FakeWebServer.SearchPage(0), FhmSamples.SearchPage(1, FhmSamples.AlbumTableWithId("A", "B", "1")))
            server.AddResponder(FakeWebServer.DownloadLinks("1"), Function(address As Uri) Task.FromException(Of String)(New NotSupportedException("Unexpected.")))
            Dim fhmCrawler As Crawler = CrawlerFetchTests.CreateCrawler(server)

            Await Assert.ThrowsExactlyAsync(Of NotSupportedException)(Function() fhmCrawler.FetchAlbumsAsync()).ConfigureAwait(False)
            Assert.IsFalse(fhmCrawler.IsFetching)
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A failed fetch operation (e.g. network error) does not leave the crawler marked as busy.
        ''' <para></para>
        ''' Regression: the busy flag was not reset on errors, so every later fetch failed with
        ''' "Another fetch operation is already running" until the application was restarted.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Async Function FetchAlbumsAsync_AfterNetworkFailure_CanFetchAgain() As Task
            Dim serverIsDown As Boolean = True
            Dim server As New FakeWebServer()
            server.AddResponder(FakeWebServer.SearchPage(0),
                                Function(address As Uri)
                                    If serverIsDown Then
                                        Return Task.FromException(Of String)(New WebException("The remote name could not be resolved."))
                                    End If
                                    Return Task.FromResult(FhmSamples.SearchPage(summary:=Nothing))
                                End Function)
            Dim fhmCrawler As Crawler = CrawlerFetchTests.CreateCrawler(server)

            Await Assert.ThrowsExactlyAsync(Of WebException)(Function() fhmCrawler.FetchAlbumsAsync()).ConfigureAwait(False)
            Assert.IsFalse(fhmCrawler.IsFetching)

            serverIsDown = False
            Dim completed As Boolean = Await fhmCrawler.FetchAlbumsAsync().ConfigureAwait(False)

            Assert.IsTrue(completed)
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Starting a fetch operation while another one is running is rejected.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Async Function FetchAlbumsAsync_WhileRunning_ThrowsInvalidOperationException() As Task
            Dim pendingPage As New TaskCompletionSource(Of String)(TaskCreationOptions.RunContinuationsAsynchronously)
            Dim server As New FakeWebServer()
            server.AddResponder(FakeWebServer.SearchPage(0), Function(address As Uri) pendingPage.Task)
            Dim fhmCrawler As Crawler = CrawlerFetchTests.CreateCrawler(server)

            Dim firstFetch As Task(Of Boolean) = fhmCrawler.FetchAlbumsAsync()
            Assert.IsTrue(fhmCrawler.IsFetching)

            Await Assert.ThrowsExactlyAsync(Of InvalidOperationException)(Function() fhmCrawler.FetchAlbumsAsync()).ConfigureAwait(False)

            pendingPage.SetResult(FhmSamples.SearchPage(summary:=Nothing))
            Dim completed As Boolean = Await firstFetch.ConfigureAwait(False)

            Assert.IsTrue(completed)
            Assert.IsFalse(fhmCrawler.IsFetching)
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The synchronous wrapper runs the whole fetch operation.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub FetchAlbums_Synchronous_RunsTheWholeOperation()
            Dim server As New FakeWebServer()
            server.AddResponse(FakeWebServer.SearchPage(0), FhmSamples.SearchPage(1, FhmSamples.AlbumTableWithId("A", "B", "1")))
            server.AddResponse(FakeWebServer.DownloadLinks("1"), FhmSamples.DownloadLinksXml("http://host/a"))
            Dim fhmCrawler As Crawler = CrawlerFetchTests.CreateCrawler(server)
            Dim recorder As New CrawlEventRecorder(fhmCrawler)

            fhmCrawler.FetchAlbums()

            CollectionAssert.AreEqual({"Begin:0", "End:0"}, recorder.Sequence)
            Assert.IsFalse(fhmCrawler.IsFetching)
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The events are raised on the <see cref="SynchronizationContext"/> captured when the fetch operation
        ''' started (e.g. the UI thread), even when the crawler continues on thread-pool threads.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Async Function FetchAlbumsAsync_RaisesEventsOnTheCapturedSynchronizationContext() As Task
            Dim pendingCount As New TaskCompletionSource(Of String)(TaskCreationOptions.RunContinuationsAsynchronously)
            Dim page0Requests As Integer = 0
            Dim html As String = FhmSamples.SearchPage(1, FhmSamples.AlbumTableWithId("A", "B", "1"))

            Dim server As New FakeWebServer()
            server.AddResponder(FakeWebServer.SearchPage(0),
                                Function(address As Uri)
                                    ' The first request (album count) stays pending until the test thread
                                    ' has restored its context, so the crawler resumes on a thread-pool thread.
                                    If Interlocked.Increment(page0Requests) = 1 Then
                                        Return pendingCount.Task
                                    End If
                                    Return Task.FromResult(html)
                                End Function)
            server.AddResponse(FakeWebServer.DownloadLinks("1"), FhmSamples.DownloadLinksXml("http://host/a"))

            Dim fhmCrawler As Crawler = CrawlerFetchTests.CreateCrawler(server)
            Dim context As New RecordingSynchronizationContext()
            Dim raisedInsideContext As New List(Of Boolean)
            AddHandler fhmCrawler.BeginPageCrawl, Sub(sender As Object, e As PageCrawlBeginEventArgs) raisedInsideContext.Add(context.IsInsideSend)
            AddHandler fhmCrawler.EndPageCrawl, Sub(sender As Object, e As PageCrawlEndEventArgs) raisedInsideContext.Add(context.IsInsideSend)

            Dim fetchTask As Task(Of Boolean)
            Dim previousContext As SynchronizationContext = SynchronizationContext.Current
            SynchronizationContext.SetSynchronizationContext(context)
            Try
                fetchTask = fhmCrawler.FetchAlbumsAsync()
            Finally
                SynchronizationContext.SetSynchronizationContext(previousContext)
            End Try

            pendingCount.SetResult(html)
            Dim completed As Boolean = Await fetchTask.ConfigureAwait(False)

            Assert.IsTrue(completed)
            Assert.AreEqual(2, context.SendCount)
            CollectionAssert.AreEqual({True, True}, raisedInsideContext)
        End Function

#End Region

#Region " CancelFetchAlbumsAsync "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A cancelled fetch operation returns <see langword="False"/> without crawling the pending page;
        ''' redundant cancellations throw an <see cref="OperationCanceledException"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Async Function CancelFetchAlbumsAsync_DuringFetch_StopsTheOperation() As Task
            Dim pendingPage As New TaskCompletionSource(Of String)(TaskCreationOptions.RunContinuationsAsynchronously)
            Dim page0Requests As Integer = 0
            Dim html As String = FhmSamples.SearchPage(12, FhmSamples.AlbumTableWithId("A", "B", "1"))

            Dim server As New FakeWebServer()
            server.AddResponder(FakeWebServer.SearchPage(0),
                                Function(address As Uri)
                                    ' 1st request: album count; 2nd request: first page (kept pending).
                                    If Interlocked.Increment(page0Requests) = 1 Then
                                        Return Task.FromResult(html)
                                    End If
                                    Return pendingPage.Task
                                End Function)
            Dim fhmCrawler As Crawler = CrawlerFetchTests.CreateCrawler(server)
            Dim recorder As New CrawlEventRecorder(fhmCrawler)

            Dim fetchTask As Task(Of Boolean) = fhmCrawler.FetchAlbumsAsync()
            Assert.IsTrue(fhmCrawler.IsFetching)

            fhmCrawler.CancelFetchAlbumsAsync()
            Assert.ThrowsExactly(Of OperationCanceledException)(Sub() fhmCrawler.CancelFetchAlbumsAsync())

            pendingPage.SetResult(html)
            Dim completed As Boolean = Await fetchTask.ConfigureAwait(False)

            Assert.IsFalse(completed)
            Assert.AreEqual(0, recorder.Sequence.Length)
            Assert.AreEqual(0, server.CountSearchPageRequests(1))
            Assert.IsFalse(fhmCrawler.IsFetching)
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A new fetch operation after a cancellation is not affected by the previous cancellation.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Async Function FetchAlbumsAsync_AfterCancellation_RunsNormally() As Task
            Dim pendingPage As New TaskCompletionSource(Of String)(TaskCreationOptions.RunContinuationsAsynchronously)
            Dim page0Requests As Integer = 0
            Dim html As String = FhmSamples.SearchPage(1, FhmSamples.AlbumTableWithId("A", "B", "1"))

            Dim server As New FakeWebServer()
            server.AddResponder(FakeWebServer.SearchPage(0),
                                Function(address As Uri)
                                    If Interlocked.Increment(page0Requests) = 2 Then
                                        Return pendingPage.Task
                                    End If
                                    Return Task.FromResult(html)
                                End Function)
            server.AddResponse(FakeWebServer.DownloadLinks("1"), FhmSamples.DownloadLinksXml("http://host/a"))
            Dim fhmCrawler As Crawler = CrawlerFetchTests.CreateCrawler(server)
            Dim recorder As New CrawlEventRecorder(fhmCrawler)

            Dim cancelledFetch As Task(Of Boolean) = fhmCrawler.FetchAlbumsAsync()
            fhmCrawler.CancelFetchAlbumsAsync()
            pendingPage.SetResult(html)
            Assert.IsFalse(Await cancelledFetch.ConfigureAwait(False))

            Dim completed As Boolean = Await fhmCrawler.FetchAlbumsAsync().ConfigureAwait(False)

            Assert.IsTrue(completed)
            CollectionAssert.AreEqual({"Begin:0", "End:0"}, recorder.Sequence)
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Cancelling while no fetch operation is running is rejected.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub CancelFetchAlbumsAsync_WhenIdle_ThrowsInvalidOperationException()
            Dim fhmCrawler As Crawler = CrawlerFetchTests.CreateCrawler(New FakeWebServer())

            Assert.ThrowsExactly(Of InvalidOperationException)(Sub() fhmCrawler.CancelFetchAlbumsAsync())
        End Sub

#End Region

#Region " Private Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Creates a crawler that downloads its content from the specified fake server.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="server">
        ''' The fake server.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The crawler.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Private Shared Function CreateCrawler(server As FakeWebServer) As Crawler
            Return New Crawler(New CookieContainer(), AddressOf server.DownloadStringAsync)
        End Function

#End Region

    End Class

End Namespace

#End Region
