#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Windows.Forms

Imports Microsoft.VisualStudio.TestTools.UnitTesting

#End Region

#Region " TabControlExtensionsTests "

Namespace DevCaseTests

    ''' <summary>
    ''' Tests the <see cref="TabControlExtensions"/> module.
    ''' </summary>
    <TestClass>
    Public NotInheritable Class TabControlExtensionsTests

        ''' <summary>
        ''' Disabling a tab page disables only that page.
        ''' </summary>
        <TestMethod>
        Public Sub DisableTabs_DisablesOnlyTheSpecifiedPages()
            Using tc As New TabControl(), page1 As New TabPage("Search"), page2 As New TabPage("Browser")
                tc.TabPages.AddRange({page1, page2})

                tc.DisableTabs(page2)
                Try
                    Assert.IsTrue(page1.Enabled)
                    Assert.IsFalse(page2.Enabled)
                Finally
                    ' The module keeps the disabled pages in a static collection; leave it clean.
                    tc.EnableTabs(page2)
                End Try
            End Using
        End Sub

        ''' <summary>
        ''' A previously disabled tab page can be enabled again.
        ''' </summary>
        <TestMethod>
        Public Sub EnableTabs_ReEnablesDisabledPages()
            Using tc As New TabControl(), page1 As New TabPage("Search"), page2 As New TabPage("Browser")
                tc.TabPages.AddRange({page1, page2})

                tc.DisableTabs(page1, page2)
                tc.EnableTabs(page1, page2)

                Assert.IsTrue(page1.Enabled)
                Assert.IsTrue(page2.Enabled)
            End Using
        End Sub

        ''' <summary>
        ''' Redundant calls (disabling twice, enabling a page that is not disabled) are harmless.
        ''' </summary>
        <TestMethod>
        Public Sub DisableAndEnableTabs_RedundantCalls_AreHarmless()
            Using tc As New TabControl(), page As New TabPage("Search")
                tc.TabPages.Add(page)

                tc.EnableTabs(page)
                Assert.IsTrue(page.Enabled)

                tc.DisableTabs(page)
                tc.DisableTabs(page)
                Assert.IsFalse(page.Enabled)

                tc.EnableTabs(page)
                Assert.IsTrue(page.Enabled)
            End Using
        End Sub

        ''' <summary>
        ''' A null tab control or a null tab page array is rejected.
        ''' </summary>
        <TestMethod>
        Public Sub DisableAndEnableTabs_NullArguments_Throw()
            Using tc As New TabControl()
                Assert.ThrowsExactly(Of ArgumentNullException)(Sub() TabControlExtensions.DisableTabs(Nothing, New TabPage()))
                Assert.ThrowsExactly(Of ArgumentNullException)(Sub() TabControlExtensions.EnableTabs(Nothing, New TabPage()))
                Assert.ThrowsExactly(Of ArgumentNullException)(Sub() tc.DisableTabs(DirectCast(Nothing, TabPage())))
                Assert.ThrowsExactly(Of ArgumentNullException)(Sub() tc.EnableTabs(DirectCast(Nothing, TabPage())))
            End Using
        End Sub

    End Class

End Namespace

#End Region
