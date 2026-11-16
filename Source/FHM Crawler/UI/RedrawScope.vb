#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports DevCase.Win32.Enums

#End Region

#Region " RedrawScope "

Namespace FHMCrawler.UI

    ''' <summary>
    ''' Suspends the drawing of a control and of all its child controls (<c>WM_SETREDRAW</c>)
    ''' until the scope is disposed; then redraws them all at once.
    ''' <para></para>
    ''' This avoids the white flashing of the controls while many of them change at the same time
    ''' (for example, when a theme is applied or a group of controls is shown or hidden).
    ''' </summary>
    '''
    ''' <example> This is a code example.
    ''' <code language="VB.NET">
    ''' Using New RedrawScope(Me.Panel1)
    '''     ' Change the controls of Panel1 here.
    ''' End Using
    ''' </code>
    ''' </example>
    Friend NotInheritable Class RedrawScope : Implements IDisposable

#Region " Constants "

        ''' <summary>
        ''' The <c>WM_SETREDRAW</c> window message, that allows or prevents changes in a window from being redrawn.
        ''' </summary>
        Private Const WM_SETREDRAW As Integer = &HB

#End Region

#Region " Private Fields "

        ''' <summary>
        ''' The control whose drawing is suspended,
        ''' or <see langword="Nothing"/> when its handle did not exist yet (nothing was drawn, so nothing is suspended).
        ''' </summary>
        Private ReadOnly suspendedControl As Control

        ''' <summary>
        ''' Flag to detect redundant calls when disposing.
        ''' </summary>
        Private isDisposed As Boolean

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RedrawScope"/> class, and suspends the drawing of the specified control.
        ''' </summary>
        ''' <param name="ctrl">
        ''' The control whose drawing (and the drawing of its child controls) is suspended.
        ''' </param>
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when <paramref name="ctrl"/> is <see langword="Nothing"/>.
        ''' </exception>
        Friend Sub New(ctrl As Control)
            If ctrl Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(ctrl))
            End If

            If ctrl.IsHandleCreated Then
                NativeMethods.SendMessage(ctrl.Handle, RedrawScope.WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero)
                Me.suspendedControl = ctrl
            End If
        End Sub

#End Region

#Region " IDisposable Implementation "

        ''' <summary>
        ''' Resumes the drawing of the control and redraws it, with all its child controls, at once.
        ''' </summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            If Me.isDisposed Then
                Exit Sub
            End If
            Me.isDisposed = True

            If Me.suspendedControl IsNot Nothing AndAlso Me.suspendedControl.IsHandleCreated Then
                Dim handle As IntPtr = Me.suspendedControl.Handle
                NativeMethods.SendMessage(handle, RedrawScope.WM_SETREDRAW, New IntPtr(1), IntPtr.Zero)
                NativeMethods.RedrawWindow(handle, IntPtr.Zero, IntPtr.Zero,
                                           RedrawWindowFlags.Invalidate Or RedrawWindowFlags.Erase Or RedrawWindowFlags.Frame Or
                                           RedrawWindowFlags.AllChildren Or RedrawWindowFlags.UpdateNow)
            End If
        End Sub

#End Region

    End Class

End Namespace

#End Region
