#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " IThemeable "

Namespace FHMCrawler.UI.Theming

    ''' <summary>
    ''' Represents a control that knows how to apply a <see cref="ThemePalette"/> to itself.
    ''' <para></para>
    ''' <see cref="ThemeManager.ApplyToControls"/> calls <see cref="IThemeable.ApplyTheme"/>
    ''' instead of applying its generic rules to the controls that implement this interface.
    ''' </summary>
    Public Interface IThemeable

        ''' <summary>
        ''' Applies the specified palette to the control.
        ''' </summary>
        ''' <param name="palette">
        ''' The palette to apply.
        ''' </param>
        Sub ApplyTheme(palette As ThemePalette)

    End Interface

End Namespace

#End Region
