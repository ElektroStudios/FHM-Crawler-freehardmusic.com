#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports Microsoft.VisualStudio.TestTools.UnitTesting

#End Region

' Some tests touch process-wide state (the TabControlExtensions module keeps static collections,
' and other tests swap the current SynchronizationContext), so they must not run in parallel.
<Assembly: DoNotParallelize>
