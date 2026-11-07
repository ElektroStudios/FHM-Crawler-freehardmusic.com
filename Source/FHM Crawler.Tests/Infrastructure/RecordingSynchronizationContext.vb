#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Threading

#End Region

#Region " RecordingSynchronizationContext "

Namespace Infrastructure

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' A <see cref="SynchronizationContext"/> that records the synchronous dispatches (<see cref="SynchronizationContext.Send"/>),
    ''' used to verify that the crawler raises its events on the context captured when the fetch operation started.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Friend NotInheritable Class RecordingSynchronizationContext : Inherits SynchronizationContext

#Region " Private Fields "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Indicates whether the current thread is executing a callback dispatched through <see cref="RecordingSynchronizationContext.Send"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <ThreadStatic>
        Private Shared insideSend As Boolean

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Backing field of the <see cref="RecordingSynchronizationContext.SendCount"/> property.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Private sendCountValue As Integer

#End Region

#Region " Properties "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the amount of callbacks dispatched through <see cref="RecordingSynchronizationContext.Send"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Friend ReadOnly Property SendCount As Integer
            Get
                Return Volatile.Read(Me.sendCountValue)
            End Get
        End Property

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a value indicating whether the current thread is executing a callback
        ''' dispatched through <see cref="RecordingSynchronizationContext.Send"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        Friend ReadOnly Property IsInsideSend As Boolean
            Get
                Return RecordingSynchronizationContext.insideSend
            End Get
        End Property

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Dispatches a synchronous message, recording it.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="d">
        ''' The callback to invoke.
        ''' </param>
        '''
        ''' <param name="state">
        ''' The object passed to the callback.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Sub Send(d As SendOrPostCallback, state As Object)
            Interlocked.Increment(Me.sendCountValue)
            RecordingSynchronizationContext.insideSend = True
            Try
                d.Invoke(state)
            Finally
                RecordingSynchronizationContext.insideSend = False
            End Try
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Creates a copy of the synchronization context.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The same instance, so all the dispatches are recorded in one place.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        Public Overrides Function CreateCopy() As SynchronizationContext
            Return Me
        End Function

#End Region

    End Class

End Namespace

#End Region
