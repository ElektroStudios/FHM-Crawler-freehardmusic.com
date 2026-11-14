#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.Globalization

Imports FHM

#End Region

#Region " CrawlEventRecorder "

Namespace Infrastructure

    ''' <summary>
    ''' Records the events raised by a <see cref="Crawler"/>.
    ''' </summary>
    Friend NotInheritable Class CrawlEventRecorder

#Region " Private Fields "

        ''' <summary>
        ''' The object used to synchronize the access to the collections.
        ''' </summary>
        Private ReadOnly syncRoot As New Object()

        ''' <summary>
        ''' The recorded events, as text (e.g. <c>"Begin:0"</c>, <c>"End:0"</c>), in order.
        ''' </summary>
        Private ReadOnly sequenceList As New List(Of String)

        ''' <summary>
        ''' The recorded <see cref="Crawler.EndPageCrawl"/> event data, in order.
        ''' </summary>
        Private ReadOnly endPagesList As New List(Of PageCrawlEndEventArgs)

        ''' <summary>
        ''' The senders of all the recorded events.
        ''' </summary>
        Private ReadOnly sendersList As New List(Of Object)

#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets a snapshot of the recorded events, as text (e.g. <c>"Begin:0"</c>, <c>"End:0"</c>), in order.
        ''' </summary>
        Friend ReadOnly Property Sequence As String()
            Get
                SyncLock Me.syncRoot
                    Return Me.sequenceList.ToArray()
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Gets a snapshot of the recorded <see cref="Crawler.EndPageCrawl"/> event data, in order.
        ''' </summary>
        Friend ReadOnly Property EndPages As PageCrawlEndEventArgs()
            Get
                SyncLock Me.syncRoot
                    Return Me.endPagesList.ToArray()
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Gets a snapshot of the senders of all the recorded events.
        ''' </summary>
        Friend ReadOnly Property Senders As Object()
            Get
                SyncLock Me.syncRoot
                    Return Me.sendersList.ToArray()
                End SyncLock
            End Get
        End Property

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="CrawlEventRecorder"/> class
        ''' and subscribes to the events of the specified crawler.
        ''' </summary>
        ''' <param name="fhmCrawler">
        ''' The crawler to observe.
        ''' </param>
        Friend Sub New(fhmCrawler As Crawler)
            If fhmCrawler Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(fhmCrawler))
            End If

            AddHandler fhmCrawler.BeginPageCrawl, AddressOf Me.OnBeginPageCrawl
            AddHandler fhmCrawler.EndPageCrawl, AddressOf Me.OnEndPageCrawl
        End Sub

#End Region

#Region " Event Handlers "

        ''' <summary>
        ''' Handles the <see cref="Crawler.BeginPageCrawl"/> event.
        ''' </summary>
        ''' <param name="sender">
        ''' The source of the event.
        ''' </param>
        '''
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Private Sub OnBeginPageCrawl(sender As Object, e As PageCrawlBeginEventArgs)
            SyncLock Me.syncRoot
                Me.sendersList.Add(sender)
                Me.sequenceList.Add($"Begin:{e.PageIndex.ToString(CultureInfo.InvariantCulture)}")
            End SyncLock
        End Sub

        ''' <summary>
        ''' Handles the <see cref="Crawler.EndPageCrawl"/> event.
        ''' </summary>
        ''' <param name="sender">
        ''' The source of the event.
        ''' </param>
        '''
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Private Sub OnEndPageCrawl(sender As Object, e As PageCrawlEndEventArgs)
            SyncLock Me.syncRoot
                Me.sendersList.Add(sender)
                Me.sequenceList.Add($"End:{e.PageIndex.ToString(CultureInfo.InvariantCulture)}")
                Me.endPagesList.Add(e)
            End SyncLock
        End Sub

#End Region

    End Class

End Namespace

#End Region
