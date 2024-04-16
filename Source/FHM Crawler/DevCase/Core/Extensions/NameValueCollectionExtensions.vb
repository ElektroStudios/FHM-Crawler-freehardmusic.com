#Region " Imports "

Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Web

#End Region

#Region " NameValueCollection Extensions "





' THIS OPEN-SOURCE APPLICATION IS POWERED BY DevCase FRAMEWORK, CREATED BY ELEKTRO STUDIOS.

' WHAT YOU SEE HERE IS FREE CUTTED CONTENT OF THIS FRAMEWORK.

' IF YOU LIKED THIS FREE APPLICATION, THEN PLEASE CONSIDER TO BUY DevCase FRAMEWORK FOR .NET AT:
' https://codecanyon.net/item/DevCase-class-library-for-net/19260282

' YOU CAN FIND THESE HELPER METHODS AND A MASSIVE AMOUNT MORE!, 
' +850 EXTENSION METHODS FOR ALL KIND OF TYPES, CUSTOM USER-CONTROLS, 
' EVERYTHING FOR THE NEWBIE And THE ADVANCED USER, FOR VB.NET AND C#. 

' DevCase is a utility framework containing new APIs and extensions to the core .NET Framework 
' to help complete your developer toolbox.
' It Is a set of general purpose classes provided as easy to consume packages.
' These utility classes and components provide productivity in day to day software development 
' mainly focused To WindowsForms. 

' UPDATES OF DevCase ARE MAINTAINED AND RELEASED EVERY MONTH.





Namespace DevCase.Core.Extensions.[NameValueCollection]

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Contains custom extension methods to use with an <see cref="Collections.Specialized.NameValueCollection"/>.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    <HideModuleName>
    Public Module NameValueCollectionExtensions

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Converts the name and values of a <see cref="Collections.Specialized.NameValueCollection"/> 
        ''' to a formatted web-request query string.
        ''' <para></para>
        ''' Consider this method to be the opposite of <see cref="Global.System.Web.HttpUtility.ParseQueryString"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim baseAddress As String = "http://www.google.com/search"
        ''' Dim params As New NameValueCollection()
        ''' params.Add("q", "Hello World")
        ''' params.Add("lr", "lang_en")
        ''' params.Add("ie", "utf-8")
        ''' 
        ''' Console.WriteLine(String.Format("{0}?{1}", baseAddress, params.ToQueryString()))
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source <see cref="Collections.Specialized.NameValueCollection"/>.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting web-request query string.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Function ToQueryString(sender As Collections.Specialized.NameValueCollection) As String

            Return NameValueCollectionExtensions.ToQueryString(sender, String.Empty)

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Converts the name and values of a <see cref="Collections.Specialized.NameValueCollection"/> 
        ''' to a formatted web-request query string.
        ''' <para></para>
        ''' Consider this method to be the opposite of <see cref="Global.System.Web.HttpUtility.ParseQueryString"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim params As New NameValueCollection()
        ''' params.Add("q", "Hello World")
        ''' params.Add("lr", "lang_en")
        ''' params.Add("ie", "utf-8")
        ''' 
        ''' Console.WriteLine(params.ToQueryString(baseAddress:=New Uri("http://www.google.com/search")))
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source <see cref="Collections.Specialized.NameValueCollection"/>.
        ''' </param>
        ''' 
        ''' <param name="baseAddress">
        ''' The base url address.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting web-request query string.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Function ToQueryString(sender As Collections.Specialized.NameValueCollection,
baseAddress As Uri) As String

            Return NameValueCollectionExtensions.ToQueryString(sender, baseAddress.AbsoluteUri)

        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Converts the name and values of a <see cref="Collections.Specialized.NameValueCollection"/> 
        ''' to a formatted web-request query string.
        ''' <para></para>
        ''' Consider this method to be the opposite of <see cref="Global.System.Web.HttpUtility.ParseQueryString"/>.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <example> This is a code example.
        ''' <code>
        ''' Dim params As New NameValueCollection()
        ''' params.Add("q", "Hello World")
        ''' params.Add("lr", "lang_en")
        ''' params.Add("ie", "utf-8")
        ''' 
        ''' Console.WriteLine(params.ToQueryString(baseAddress:="http://www.google.com/search"))
        ''' </code>
        ''' </example>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="sender">
        ''' The source <see cref="Collections.Specialized.NameValueCollection"/>.
        ''' </param>
        ''' 
        ''' <param name="baseAddress">
        ''' The base url address.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The resulting web-request query string.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Function ToQueryString(sender As Collections.Specialized.NameValueCollection,
baseAddress As String) As String

            Dim sb As New StringBuilder
            If Not String.IsNullOrWhiteSpace(baseAddress) Then
                sb.Append(baseAddress.TrimEnd({"?"c}))
                sb.Append("?")
            End If

            For Each key As String In sender.AllKeys
                sb.AppendFormat("{0}={1}&", key, HttpUtility.UrlEncode(sender(key)))
            Next

            Return sb.Remove((sb.Length - 1), 1).ToString() ' removes the last "&" char.

        End Function

    End Module

End Namespace

#End Region
