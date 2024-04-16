Imports System.Runtime.CompilerServices

''' <summary>
''' Provides method extensions for a <see cref="TabControl"/> control.
''' </summary>
<HideModuleName>
Public Module TabControlExtensions

    ''' <summary>
    ''' Collection used to store tab pages whose tab header need to remain disabled on a <see cref="TabControl"/>.
    ''' <para></para>
    ''' This collection depends on <see cref="TabControlExtensions.DisableOrEnableTabs_Internal"/> method.
    ''' </summary>
    Private disabledTabs As HashSet(Of TabPage)

    ''' <summary>
    ''' Collection used to store tab controls whose its <see cref="TabControl.Selecting"/> event
    ''' has been associated to <see cref="TabControlExtensions.disableTabPageHandler"/>.
    ''' <para></para>
    ''' This collection depends on <see cref="TabControlExtensions.DisableOrEnableTabs_Internal"/> method.
    ''' </summary>
    Private tabHandlerAddedControls As HashSet(Of TabControl)

    ''' <summary>
    ''' A <see cref="TabControlCancelEventHandler"/> delegate used for disabling tabs on a <see cref="TabControl"/>.
    ''' <para></para>
    ''' This handler depends on <see cref="TabControlExtensions.DisableOrEnableTabs_Internal"/> method.
    ''' </summary>
    Private tabDisablerHandler As TabControlCancelEventHandler

    ''' <summary>
    ''' Disables one or multiple <see cref="TabPage"/>, 
    ''' making the tabs unselectable in the source <see cref="TabControl"/>.
    ''' </summary>
    ''' 
    ''' <param name="tabControl">
    ''' The source <see cref="TabControl"/>.
    ''' </param>
    ''' 
    ''' <param name="tabPages">
    ''' An Array of <see cref="TabPage"/> to disable.
    ''' </param>
    <Extension>
    <DebuggerStepThrough>
    Public Sub DisableTabs(tabControl As TabControl, ParamArray tabPages As TabPage())
        TabControlExtensions.DisableOrEnableTabs_Internal(tabControl, enabled:=False, tabPages)
    End Sub

    ''' <summary>
    ''' Enables one or multiple <see cref="TabPage"/> that were previously 
    ''' disabled by a call to <see cref="TabControlExtensions.DisableTabPages"/> method, 
    ''' making the tabs selectable again in the source <see cref="TabControl"/>.
    ''' </summary>
    ''' 
    ''' <param name="tabControl">
    ''' The source <see cref="TabControl"/>.
    ''' </param>
    ''' 
    ''' <param name="tabPages">
    ''' An Array of <see cref="TabPage"/> to enable.
    ''' </param>
    <Extension>
    <DebuggerStepThrough>
    Public Sub EnableTabs(tabControl As TabControl, ParamArray tabPages As TabPage())
        TabControlExtensions.DisableOrEnableTabs_Internal(tabControl, enabled:=True, tabPages)
    End Sub

    ''' <summary>
    ''' *** FOR INTERNAL USE ONLY ***
    ''' <para></para>
    ''' Disables or enables one or multiple <see cref="TabPage"/>, 
    ''' denying or allowing their tab selection in the source <see cref="TabControl"/>.
    ''' </summary>
    ''' 
    ''' <param name="tabControl">
    ''' The source <see cref="TabControl"/>.
    ''' </param>
    ''' 
    ''' <param name="enabled">
    ''' If <see langword="False"/>, disables the tab pages and make them unselectable in the source <see cref="TabControl"/>; 
    ''' otherwise, enable the tab pages and allows to be selected in the source <see cref="TabControl"/>.
    ''' </param>
    ''' 
    ''' <param name="tabPages">
    ''' An Array of the tab pages to disable or enable.
    ''' </param>
    <DebuggerStepThrough>
    Private Sub DisableOrEnableTabs_Internal(tabControl As TabControl, enabled As Boolean, ParamArray tabPages As TabPage())
        If tabControl Is Nothing Then
            Throw New ArgumentNullException(paramName:=NameOf(tabControl))
        End If
        If tabPages Is Nothing Then
            Throw New ArgumentNullException(paramName:=NameOf(tabPages))
        End If

        ' Initialize collections.
        If TabControlExtensions.disabledTabs Is Nothing Then
            TabControlExtensions.disabledTabs = New HashSet(Of TabPage)
        End If
        If TabControlExtensions.tabHandlerAddedControls Is Nothing Then
            TabControlExtensions.tabHandlerAddedControls = New HashSet(Of TabControl)
        End If

        ' Initialize handler.
        If TabControlExtensions.tabDisablerHandler Is Nothing Then
            TabControlExtensions.tabDisablerHandler =
                Sub(sender As Object, e As TabControlCancelEventArgs)
                    If e.TabPageIndex < 0 Then
                        Exit Sub
                    End If

                    Select Case e.Action
                        Case TabControlAction.Selecting, TabControlAction.Selected
                            e.Cancel = TabControlExtensions.disabledTabs.Contains(e.TabPage)
                        Case Else
                            Exit Sub
                    End Select
                End Sub
        End If

        For Each tabPage As TabPage In tabPages
            If tabPage Is Nothing Then
                Throw New NullReferenceException($"{NameOf(tabPage)} object is null.")
            End If

            ' Disable or enable the tab page.
            tabPage.Enabled = enabled

            If Not enabled Then ' Disable the tab header.
                Dim success As Boolean = disabledTabs.Add(tabPage)
                If success AndAlso Not TabControlExtensions.tabHandlerAddedControls.Contains(tabControl) Then
                    AddHandler tabControl.Selecting, TabControlExtensions.tabDisablerHandler
                    TabControlExtensions.tabHandlerAddedControls.Add(tabControl)
                End If
            Else ' Enable the tab header.
                Dim success As Boolean = disabledTabs.Remove(tabPage)
                If success AndAlso TabControlExtensions.tabHandlerAddedControls.Contains(tabControl) AndAlso
                               Not TabControlExtensions.disabledTabs.Any() Then
                    RemoveHandler tabControl.Selecting, TabControlExtensions.tabDisablerHandler
                    TabControlExtensions.tabHandlerAddedControls.Remove(tabControl)
                End If
            End If
        Next tabPage

    End Sub

End Module
