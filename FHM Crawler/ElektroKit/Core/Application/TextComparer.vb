




' THIS OPEN-SOURCE APPLICATION IS POWERED BY ELEKTROKIT FRAMEWORK, CREATED BY ELEKTRO STUDIOS.

' WHAT YOU SEE HERE IS FREE CUTTED CONTENT OF THIS FRAMEWORK.

' IF YOU LIKED THIS FREE APPLICATION, THEN PLEASE CONSIDER TO BUY ELEKTROKIT FRAMEWORK FOR .NET AT:
' https://codecanyon.net/item/elektrokit-class-library-for-net/19260282

' YOU CAN FIND THESE HELPER METHODS AND A MASSIVE AMOUNT MORE!, 
' +850 EXTENSION METHODS FOR ALL KIND OF TYPES, CUSTOM USER-CONTROLS, 
' EVERYTHING FOR THE NEWBIE And THE ADVANCED USER, FOR VB.NET AND C#. 

' ElektroKit is a utility framework containing new APIs and extensions to the core .NET Framework 
' to help complete your developer toolbox.
' It Is a set of general purpose classes provided as easy to consume packages.
' These utility classes and components provide productivity in day to day software development 
' mainly focused To WindowsForms. 

' UPDATES OF ELEKTROKIT ARE MAINTAINED AND RELEASED EVERY MONTH.




#Region " Imports "

Imports System.ComponentModel

Imports ElektroKit.Core.Application.Enums

#End Region

#Region " Text Comparer "

Namespace ElektroKit.Core.Application.Types

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Performs a comparison between two string values.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class TextComparer : Inherits CaseInsensitiveComparer

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="TextComparer"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerNonUserCode>
        Public Sub New()
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="a">
        ''' The first object to compare.
        ''' </param>
        ''' 
        ''' <param name="b">
        ''' The second object to compare.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' A signed integer that indicates the relative values of <paramref name="a"/> and <paramref name="b"/>.
        ''' <para></para>
        ''' 0: <paramref name="a"/> equals <paramref name="b"/>. 
        ''' <para></para>
        ''' Less than 0: <paramref name="a"/> is less than <paramref name="b"/>. 
        ''' <para></para>
        ''' Greater than 0: <paramref name="a"/> is greater than <paramref name="b"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerStepThrough>
        Public Shadows Function Compare(ByVal a As Object, ByVal b As Object) As Integer

            ' Null parsing.
            If a Is Nothing AndAlso b Is Nothing Then
                Return ComparerResult.Equals ' A is equals to B.

            ElseIf a Is Nothing AndAlso b IsNot Nothing Then
                Return ComparerResult.LessThan ' A is less than B.

            ElseIf a IsNot Nothing AndAlso b Is Nothing Then
                Return ComparerResult.GreaterThan ' A is greater than B.

            Else
                If (TypeOf a Is String) AndAlso (TypeOf b Is String) Then ' True And True.
                    Return DirectCast(MyBase.Compare(a, b), ComparerResult)

                ElseIf (TypeOf a Is String) AndAlso Not (TypeOf b Is String) Then ' True And False.
                    Return ComparerResult.GreaterThan ' A is greater than B.

                ElseIf Not (TypeOf a Is String) AndAlso (TypeOf b Is String) Then ' False And True.
                    Return ComparerResult.LessThan ' A is less than B.

                Else ' False And False.
                    Return ComparerResult.Equals

                End If

            End If

        End Function

#End Region

#Region " Hidden Base Members "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Serves as a hash function for a particular type.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <EditorBrowsable(EditorBrowsableState.Never)>
        <DebuggerNonUserCode>
        Public Shadows Function GetHashCode() As Integer
            Return MyBase.GetHashCode()
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the <see cref="Type"/> of the current instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' The exact runtime type of the current instance.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <EditorBrowsable(EditorBrowsableState.Never)>
        <DebuggerNonUserCode>
        Public Shadows Function [GetType]() As Type
            Return MyBase.GetType()
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Determines whether the specified <see cref="Object"/> is equal to this instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="obj">
        ''' Another object to compare to.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see langword="True"/> if the specified <see cref="Object"/> is equal to this instance; 
        ''' otherwise, <see langword="False"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <EditorBrowsable(EditorBrowsableState.Never)>
        <DebuggerNonUserCode>
        Public Shadows Function Equals(ByVal obj As Object) As Boolean
            Return MyBase.Equals(obj)
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Determines whether the specified <see cref="Object"/> instances are considered equal.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="objA">
        ''' The first object to compare.
        ''' </param>
        ''' 
        ''' <param name="objB">
        ''' The second object to compare.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see langword="True"/> if the objects are considered equal; otherwise, <see langword="False"/>.
        ''' <para></para>
        ''' If both <paramref name="objA"/> and <paramref name="objB"/> are <see langword="Nothing"/>, 
        ''' the method returns <see langword="True"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <EditorBrowsable(EditorBrowsableState.Never)>
        <DebuggerNonUserCode>
        Public Shared Shadows Function Equals(ByVal objA As Object, ByVal objB As Object) As Boolean
            Return Object.Equals(objA, objB)
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Determines whether the specified <see cref="Object"/> instances are the same instance.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="objA">
        ''' The first object to compare.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="objB">
        ''' The second object to compare.
        ''' </param>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' <see langword="True"/> if <paramref name="objA"/> is the same instance as <paramref name="objB"/> 
        ''' or if both are <see langword="Nothing"/>; otherwise, <see langword="False"/>.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <EditorBrowsable(EditorBrowsableState.Never)>
        <DebuggerNonUserCode>
        Public Shared Shadows Function ReferenceEquals(ByVal objA As Object, ByVal objB As Object) As Boolean
            Return Object.ReferenceEquals(objA, objB)
        End Function

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Returns a String that represents the current object.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <returns>
        ''' A string that represents the current object.
        ''' </returns>
        ''' ----------------------------------------------------------------------------------------------------
        <EditorBrowsable(EditorBrowsableState.Never)>
        <DebuggerNonUserCode>
        Public Overrides Function ToString() As String
            Return MyBase.ToString()
        End Function

#End Region

    End Class

End Namespace

#End Region
