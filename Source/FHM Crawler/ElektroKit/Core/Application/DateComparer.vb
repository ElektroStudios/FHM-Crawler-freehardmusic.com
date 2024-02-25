﻿




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

Imports ElektroKit.Core.Application.Enums

#End Region

#Region " Date Comparer "

Namespace ElektroKit.Core.Application.Types

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Performs a comparison between two <see cref="Date"/> instances.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <seealso cref="IComparer"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class DateComparer : Implements IComparer

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="DateComparer"/> class.
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
        Public Function Compare(ByVal a As Object, ByVal b As Object) As Integer Implements IComparer.Compare

            ' Null parsing.
            If (a Is Nothing) AndAlso (b Is Nothing) Then
                Return ComparerResult.Equals ' A is equals to B.

            ElseIf (a Is Nothing) AndAlso (b IsNot Nothing) Then
                Return ComparerResult.LessThan ' A is less than B.

            ElseIf (a IsNot Nothing) AndAlso (b Is Nothing) Then
                Return ComparerResult.GreaterThan ' A is greater than B.

            Else
                Dim dateA As DateTime
                Dim dateB As DateTime

                If (Date.TryParse(CStr(a), dateA)) AndAlso (Date.TryParse(CStr(b), dateB)) Then ' True And True.
                    Return DirectCast(dateA.CompareTo(dateB), ComparerResult)

                ElseIf (Date.TryParse(CStr(a), dateA)) AndAlso Not (Date.TryParse(CStr(b), dateB)) Then ' True And False.
                    Return ComparerResult.LessThan ' A is less than B.

                ElseIf Not (Date.TryParse(CStr(a), dateA)) AndAlso (Date.TryParse(CStr(b), dateB)) Then ' False And True.
                    Return ComparerResult.GreaterThan ' A is greater than B.

                Else ' False And False.
                    Return DirectCast(a.ToString().CompareTo(b.ToString()), ComparerResult)

                End If

            End If

        End Function

#End Region

    End Class

End Namespace

#End Region
