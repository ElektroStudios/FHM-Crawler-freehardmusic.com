' ***********************************************************************
' Author   : ElektroStudios
' Modified : 14-April-2024
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

#End Region

#Region " InternetGetCookieExFlags "

' ReSharper disable once CheckNamespace

Namespace DevCase.Win32.Enums

    ''' <summary>
    ''' Specifies the changes to be applied for the active desktop.
    ''' <para></para>
    ''' For <see cref="NativeMethods.InternetGetCookieEx"/> function.
    ''' </summary>
    '''
    ''' <remarks>
    ''' <see href="https://learn.microsoft.com/en-us/windows/win32/api/wininet/nf-wininet-internetgetcookieexa"/>
    ''' </remarks>
    Public Enum InternetGetCookieExFlags As Integer

        ''' <summary>
        ''' Enables the retrieval of cookies that are marked as "HTTPOnly". 
        ''' <para></para>
        ''' Do not use this flag if you expose a scriptable interface, because this has security implications. 
        ''' <para></para>
        ''' It is imperative that you use this flag only if you can guarantee that you 
        ''' will never expose the cookie to third-party code by way of an extensibility mechanism you provide.
        ''' </summary>
        Httponly = &H2000

        ''' <summary>
        ''' Retrieves only third-party cookies if policy explicitly allows all 
        ''' cookies for the specified URL to be retrieved
        ''' </summary>
        ThirdParty = &H10

        ''' <summary>
        ''' Retrieves only cookies that would be allowed if the specified URL were untrusted; 
        ''' that is, if it belonged to the URLZONE_UNTRUSTED zone
        ''' </summary>
        RestrictedZone = &H200

    End Enum

End Namespace

#End Region
