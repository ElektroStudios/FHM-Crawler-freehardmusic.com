#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Specialized

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports DevCase.Core.Extensions.NameValueCollection

#End Region

#Region " NameValueCollectionExtensionsTests "

Namespace DevCaseTests

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Tests the <see cref="NameValueCollectionExtensions"/> module.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <TestClass>
    Public NotInheritable Class NameValueCollectionExtensionsTests

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The parameters are joined in insertion order with <c>&amp;</c>, without trailing separator.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub ToQueryString_JoinsParametersInOrder()
            Dim params As New NameValueCollection From {{"b", "2"}, {"a", "1"}, {"c", "3"}}

            Assert.AreEqual("b=2&a=1&c=3", params.ToQueryString())
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The values are URL-encoded (spaces become <c>+</c>).
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub ToQueryString_EncodesValues()
            Dim params As New NameValueCollection From {{"q", "Hello World"}, {"lr", "lang_en"}}

            Assert.AreEqual("q=Hello+World&lr=lang_en", params.ToQueryString())
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Null and empty values produce empty parameter values.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub ToQueryString_NullOrEmptyValues_ProduceEmptyValues()
            Dim params As New NameValueCollection From {{"a", Nothing}, {"b", ""}}

            Assert.AreEqual("a=&b=", params.ToQueryString())
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The base address (string overload) is prepended with a single <c>?</c> separator.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        <DataRow("http://www.google.com/search")>
        <DataRow("http://www.google.com/search?")>
        <DataRow("http://www.google.com/search??")>
        Public Sub ToQueryString_StringBaseAddress_UsesSingleSeparator(baseAddress As String)
            Dim params As New NameValueCollection From {{"q", "metal"}}

            Assert.AreEqual("http://www.google.com/search?q=metal", params.ToQueryString(baseAddress))
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' The base address (<see cref="Uri"/> overload) is prepended using its absolute form.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub ToQueryString_UriBaseAddress_UsesAbsoluteUri()
            Dim params As New NameValueCollection From {{"q", "rock"}}

            Assert.AreEqual("http://freehardmusic.com/index.php?q=rock", params.ToQueryString(New Uri("http://freehardmusic.com/index.php")))
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' An empty collection with base address yields the bare base address.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub ToQueryString_EmptyCollectionWithBaseAddress_ReturnsBaseAddress()
            Assert.AreEqual("http://host/page", New NameValueCollection().ToQueryString("http://host/page"))
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' An empty collection without base address yields an empty string.
        ''' <para></para>
        ''' Regression: it used to throw an <see cref="ArgumentOutOfRangeException"/> (StringBuilder.Remove(-1, 1)).
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub ToQueryString_EmptyCollectionWithoutBaseAddress_ReturnsEmptyString()
            Assert.AreEqual("", New NameValueCollection().ToQueryString())
            Assert.AreEqual("", New NameValueCollection().ToQueryString(baseAddress:="   "))
        End Sub

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' A null collection is rejected.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <TestMethod>
        Public Sub ToQueryString_NullCollection_Throws()
            Dim params As NameValueCollection = Nothing

            Assert.ThrowsExactly(Of ArgumentNullException)(Sub() params.ToQueryString())
        End Sub

    End Class

End Namespace

#End Region
