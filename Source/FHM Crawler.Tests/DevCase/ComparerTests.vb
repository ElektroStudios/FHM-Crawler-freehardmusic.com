#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections
Imports System.Collections.Generic

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports DevCase.Core.Application.Enums
Imports DevCase.Core.Application.Types

#End Region

#Region " ComparerTests "

Namespace DevCaseTests

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Tests the comparers used to sort the album list: <see cref="TextComparer"/>, <see cref="NumericComparer"/>,
    ''' <see cref="DateComparer"/> and <see cref="StringLengthComparer"/>.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestClass>
    Public NotInheritable Class ComparerTests

#Region " TextComparer "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Strings are compared case-insensitively.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub TextComparer_ComparesCaseInsensitively()
            Dim comparer As IComparer = New TextComparer()

            Assert.AreEqual(0, comparer.Compare("Metallica", "METALLICA"))
            Assert.IsTrue(comparer.Compare("Accept", "anthrax") < 0)
            Assert.IsTrue(comparer.Compare("Saxon", "accept") > 0)
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Null values are sorted before any value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub TextComparer_SortsNullFirst()
            Dim comparer As IComparer = New TextComparer()

            Assert.AreEqual(ComparerResult.Equals, DirectCast(comparer.Compare(Nothing, Nothing), ComparerResult))
            Assert.AreEqual(ComparerResult.LessThan, DirectCast(comparer.Compare(Nothing, "a"), ComparerResult))
            Assert.AreEqual(ComparerResult.GreaterThan, DirectCast(comparer.Compare("a", Nothing), ComparerResult))
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Through the <see cref="IComparer"/> interface, non-string values follow the custom rules
        ''' (strings are greater than non-strings; two non-strings are equal) instead of throwing.
        ''' <para></para>
        ''' Regression: the custom method was a <c>Shadows</c> method, so interface calls used the base
        ''' <see cref="CaseInsensitiveComparer"/>, which throws for values of different types.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub TextComparer_ThroughInterface_UsesCustomRulesForNonStrings()
            Dim comparer As IComparer = New TextComparer()

            Assert.AreEqual(ComparerResult.GreaterThan, DirectCast(comparer.Compare("a", 5), ComparerResult))
            Assert.AreEqual(ComparerResult.LessThan, DirectCast(comparer.Compare(5, "a"), ComparerResult))
            Assert.AreEqual(ComparerResult.Equals, DirectCast(comparer.Compare(5, New Object()), ComparerResult))
        End Sub

#End Region

#Region " NumericComparer "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Numeric strings are compared by value, not lexically.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub NumericComparer_ComparesByValue()
            Dim comparer As IComparer = New NumericComparer()

            Assert.AreEqual(ComparerResult.LessThan, DirectCast(comparer.Compare("9", "10"), ComparerResult))
            Assert.AreEqual(ComparerResult.GreaterThan, DirectCast(comparer.Compare("100", "20"), ComparerResult))
            Assert.AreEqual(ComparerResult.Equals, DirectCast(comparer.Compare("007", "7"), ComparerResult))
            Assert.AreEqual(ComparerResult.LessThan, DirectCast(comparer.Compare("-5", "3"), ComparerResult))
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Numbers are sorted after non-numeric text; two non-numeric texts are compared as text.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub NumericComparer_MixedValues()
            Dim comparer As IComparer = New NumericComparer()

            Assert.AreEqual(ComparerResult.GreaterThan, DirectCast(comparer.Compare("1", "abc"), ComparerResult))
            Assert.AreEqual(ComparerResult.LessThan, DirectCast(comparer.Compare("abc", "1"), ComparerResult))
            Assert.AreEqual(ComparerResult.LessThan, DirectCast(comparer.Compare("abc", "abd"), ComparerResult))
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Null values are sorted before any value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub NumericComparer_SortsNullFirst()
            Dim comparer As IComparer = New NumericComparer()

            Assert.AreEqual(ComparerResult.Equals, DirectCast(comparer.Compare(Nothing, Nothing), ComparerResult))
            Assert.AreEqual(ComparerResult.LessThan, DirectCast(comparer.Compare(Nothing, "1"), ComparerResult))
            Assert.AreEqual(ComparerResult.GreaterThan, DirectCast(comparer.Compare("1", Nothing), ComparerResult))
        End Sub

#End Region

#Region " DateComparer "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Date strings are compared chronologically.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub DateComparer_ComparesChronologically()
            Dim comparer As IComparer = New DateComparer()

            Assert.AreEqual(ComparerResult.LessThan, DirectCast(comparer.Compare("1999-12-31", "2000-01-01"), ComparerResult))
            Assert.AreEqual(ComparerResult.GreaterThan, DirectCast(comparer.Compare("2024-06-01", "2023-06-01"), ComparerResult))
            Assert.AreEqual(ComparerResult.Equals, DirectCast(comparer.Compare("2020-02-29", "2020-02-29"), ComparerResult))
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Dates are sorted before non-date text; two non-date texts are compared as text.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub DateComparer_MixedValues()
            Dim comparer As IComparer = New DateComparer()

            Assert.AreEqual(ComparerResult.LessThan, DirectCast(comparer.Compare("2000-01-01", "not a date"), ComparerResult))
            Assert.AreEqual(ComparerResult.GreaterThan, DirectCast(comparer.Compare("not a date", "2000-01-01"), ComparerResult))
            Assert.AreEqual(ComparerResult.LessThan, DirectCast(comparer.Compare("abc", "abd"), ComparerResult))
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Null values are sorted before any value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub DateComparer_SortsNullFirst()
            Dim comparer As IComparer = New DateComparer()

            Assert.AreEqual(ComparerResult.Equals, DirectCast(comparer.Compare(Nothing, Nothing), ComparerResult))
            Assert.AreEqual(ComparerResult.LessThan, DirectCast(comparer.Compare(Nothing, "2000-01-01"), ComparerResult))
            Assert.AreEqual(ComparerResult.GreaterThan, DirectCast(comparer.Compare("2000-01-01", Nothing), ComparerResult))
        End Sub

#End Region

#Region " StringLengthComparer "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Strings are compared by length first, then by value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub StringLengthComparer_ComparesByLengthThenValue()
            Dim comparer As IComparer(Of String) = New StringLengthComparer()

            Assert.AreEqual(ComparerResult.LessThan, DirectCast(comparer.Compare("zz", "aaa"), ComparerResult))
            Assert.AreEqual(ComparerResult.GreaterThan, DirectCast(comparer.Compare("aaaa", "zzz"), ComparerResult))
            Assert.AreEqual(ComparerResult.LessThan, DirectCast(comparer.Compare("abc", "abd"), ComparerResult))
            Assert.AreEqual(ComparerResult.Equals, DirectCast(comparer.Compare("same", "same"), ComparerResult))
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Null values are sorted before any value.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub StringLengthComparer_SortsNullFirst()
            Dim comparer As IComparer(Of String) = New StringLengthComparer()

            Assert.AreEqual(ComparerResult.Equals, DirectCast(comparer.Compare(Nothing, Nothing), ComparerResult))
            Assert.AreEqual(ComparerResult.LessThan, DirectCast(comparer.Compare(Nothing, ""), ComparerResult))
            Assert.AreEqual(ComparerResult.GreaterThan, DirectCast(comparer.Compare("", Nothing), ComparerResult))
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The comparer sorts a list by length.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub StringLengthComparer_SortsList()
            Dim items As New List(Of String) From {"Rainbow", "Dio", "Kiss", "Ozzy", "Accept"}

            items.Sort(New StringLengthComparer())

            CollectionAssert.AreEqual({"Dio", "Kiss", "Ozzy", "Accept", "Rainbow"}, items)
        End Sub

#End Region

    End Class

End Namespace

#End Region
