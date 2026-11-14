#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq

Imports Microsoft.VisualStudio.TestTools.UnitTesting

#End Region

#Region " DataSourcesTests "

Namespace ApplicationTests

    ''' <summary>
    ''' Tests the integrity of the <see cref="DataSources"/> module
    ''' (the values of the search filter combo boxes, sent as-is to the FHM website).
    ''' </summary>
    <TestClass>
    Public NotInheritable Class DataSourcesTests

#Region " Common Rules "

        ''' <summary>
        ''' Every list starts with the <c>"all"</c> value, which is the default search filter value.
        ''' </summary>
        <TestMethod>
        <DataRow(NameOf(DataSources.years))>
        <DataRow(NameOf(DataSources.genres))>
        <DataRow(NameOf(DataSources.countries))>
        Public Sub DataSource_StartsWithAll(listName As String)
            Dim values As String() = DataSourcesTests.GetDataSource(listName)

            Assert.AreEqual("all", values(0))
            Assert.AreEqual(1, values.Count(Function(value As String) value.Equals("all", StringComparison.OrdinalIgnoreCase)))
        End Sub

        ''' <summary>
        ''' No list contains empty values or values with leading or trailing white-spaces.
        ''' </summary>
        <TestMethod>
        <DataRow(NameOf(DataSources.years))>
        <DataRow(NameOf(DataSources.genres))>
        <DataRow(NameOf(DataSources.countries))>
        Public Sub DataSource_HasNoBlankOrUntrimmedValues(listName As String)
            Dim values As String() = DataSourcesTests.GetDataSource(listName)

            For Each value As String In values
                Assert.IsFalse(String.IsNullOrWhiteSpace(value), $"The '{listName}' list contains a blank value.")
                Assert.AreEqual(value.Trim(), value, $"The '{listName}' list contains an untrimmed value: '{value}'.")
            Next value
        End Sub

        ''' <summary>
        ''' No list contains duplicated values (case-insensitive).
        ''' </summary>
        <TestMethod>
        <DataRow(NameOf(DataSources.years))>
        <DataRow(NameOf(DataSources.genres))>
        <DataRow(NameOf(DataSources.countries))>
        Public Sub DataSource_HasNoDuplicates(listName As String)
            Dim values As String() = DataSourcesTests.GetDataSource(listName)

            Dim duplicates As String() = values.GroupBy(Function(value As String) value, StringComparer.OrdinalIgnoreCase).
                                                Where(Function(group As IGrouping(Of String, String)) group.Count() > 1).
                                                Select(Function(group As IGrouping(Of String, String)) group.Key).
                                                ToArray()

            Assert.AreEqual(0, duplicates.Length, $"Duplicated values in the '{listName}' list: {String.Join(", ", duplicates)}")
        End Sub

#End Region

#Region " Years "

        ''' <summary>
        ''' Every year (after <c>"all"</c>) is a four-digit number, sorted in ascending order.
        ''' </summary>
        <TestMethod>
        Public Sub Years_AreFourDigitNumbersInAscendingOrder()
            Dim years As New List(Of Integer)

            For Each value As String In DataSources.years.Skip(1)
                Dim year As Integer
                Assert.AreEqual(4, value.Length, $"Invalid year: '{value}'.")
                Assert.IsTrue(Integer.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, year), $"Invalid year: '{value}'.")
                years.Add(year)
            Next value

            CollectionAssert.AreEqual(years.OrderBy(Function(year As Integer) year).ToList(), years)
        End Sub

        ''' <summary>
        ''' Regression test (v1.2.0): the year list is no longer capped at 2018.
        ''' </summary>
        <TestMethod>
        Public Sub Years_IncludeRecentYears()
            CollectionAssert.IsSubsetOf({"2018", "2019", "2020", "2021", "2022", "2023", "2024", "2025"}, DataSources.years)
        End Sub

#End Region

#Region " Countries "

        ''' <summary>
        ''' The countries (after <c>"all"</c>) are sorted alphabetically, so they are easy to find in the combo box.
        ''' </summary>
        <TestMethod>
        Public Sub Countries_AreSortedAlphabetically()
            Dim countries As String() = DataSources.countries.Skip(1).ToArray()

            CollectionAssert.AreEqual(countries.OrderBy(Function(country As String) country, StringComparer.OrdinalIgnoreCase).ToArray(), countries)
        End Sub

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Gets a data source list by name.
        ''' </summary>
        ''' <param name="listName">
        ''' The name of the list (<c>years</c>, <c>genres</c> or <c>countries</c>).
        ''' </param>
        ''' <returns>
        ''' The list values.
        ''' </returns>
        ''' <exception cref="ArgumentException">
        ''' Thrown when <paramref name="listName"/> is not a known list name.
        ''' </exception>
        Private Shared Function GetDataSource(listName As String) As String()
            Select Case listName
                Case NameOf(DataSources.years)
                    Return DataSources.years
                Case NameOf(DataSources.genres)
                    Return DataSources.genres
                Case NameOf(DataSources.countries)
                    Return DataSources.countries
                Case Else
                    Throw New ArgumentException($"Unknown data source list: '{listName}'.", paramName:=NameOf(listName))
            End Select
        End Function

#End Region

    End Class

End Namespace

#End Region
