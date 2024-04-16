




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




#Region " Imports "

Imports DevCase.Core.Application.Enums

#End Region

#Region " Length Comparer "

Namespace DevCase.Core.Application.Types

    ''' ----------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Performs a comparison between the length of two <see cref="String"/> values.
    ''' </summary>
    ''' ----------------------------------------------------------------------------------------------------
    ''' <remarks>
    ''' <seealso cref="IComparer(Of String)"/>
    ''' </remarks>
    ''' ----------------------------------------------------------------------------------------------------
    Public NotInheritable Class StringLengthComparer : Implements IComparer(Of String)

#Region " Constructors "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes a new instance of the <see cref="StringLengthComparer"/> class.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        <DebuggerNonUserCode>
        Public Sub New()
        End Sub

#End Region

#Region " Public Methods "

        ''' ----------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Compares two <see cref="String"/> and returns a value 
        ''' indicating whether one has length less than, length equal to, or length greater than the other.
        ''' </summary>
        ''' ----------------------------------------------------------------------------------------------------
        ''' <param name="a">
        ''' The first <see cref="String"/> to compare.
        ''' </param>
        ''' 
        ''' <param name="b">
        ''' The second <see cref="String"/> to compare.
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
        Public Function Compare(a As String, b As String) As Integer Implements IComparer(Of String).Compare

            If (a Is Nothing) AndAlso (b Is Nothing) Then
                Return ComparerResult.Equals ' Length of A is equals to length of B.

            ElseIf (a Is Nothing) AndAlso (b IsNot Nothing) Then
                Return ComparerResult.LessThan ' Length of A is smaller than length of B.

            ElseIf (a IsNot Nothing) AndAlso (b Is Nothing) Then
                Return ComparerResult.GreaterThan ' Length of A is bigger than length of B.

            Else
                If (a.Length < b.Length) Then
                    Return ComparerResult.LessThan ' Length of A is smaller than length of B.

                ElseIf (a.Length > b.Length) Then
                    Return ComparerResult.GreaterThan ' Length of A is bigger than length of B.

                Else
                    Return DirectCast(a.CompareTo(b), ComparerResult)

                End If

            End If

        End Function

#End Region

    End Class

End Namespace

#End Region
