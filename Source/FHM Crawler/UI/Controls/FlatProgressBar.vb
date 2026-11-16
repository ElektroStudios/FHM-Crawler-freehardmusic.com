#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel

Imports FHMCrawler.UI.Theming

#End Region

#Region " FlatProgressBar "

Namespace FHMCrawler.UI.Controls

    ''' <summary>
    ''' A flat progress bar drawn with the colors of the current <see cref="ThemePalette"/>.
    ''' <para></para>
    ''' The bar color depends on its <see cref="FlatProgressBar.State"/>.
    ''' </summary>
    <DesignerCategory("Code")>
    Public Class FlatProgressBar : Inherits Control : Implements IThemeable

#Region " Private Fields "

        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The lower bound of the range.
        ''' </summary>
        Private minimumB As Integer = 0

        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The upper bound of the range.
        ''' </summary>
        Private maximumB As Integer = 100

        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The current position.
        ''' </summary>
        Private valueB As Integer = 0

        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The state of the progress bar.
        ''' </summary>
        Private stateB As ProgressBarState = ProgressBarState.Normal

        ''' <summary>
        ''' The bar color of the <see cref="ProgressBarState.Normal"/> state.
        ''' </summary>
        Private normalColor As Color = SystemColors.Highlight

        ''' <summary>
        ''' The bar color of the <see cref="ProgressBarState.Warning"/> state.
        ''' </summary>
        Private warningColor As Color = Color.FromArgb(255, 185, 0)

        ''' <summary>
        ''' The bar color of the <see cref="ProgressBarState.Error"/> state.
        ''' </summary>
        Private errorColor As Color = Color.FromArgb(232, 17, 35)

        ''' <summary>
        ''' The color of the empty part of the bar.
        ''' </summary>
        Private trackColor As Color = SystemColors.ControlLight

#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets or sets the lower bound of the range.
        ''' </summary>
        ''' <exception cref="ArgumentOutOfRangeException">
        ''' Thrown when the value is negative or greater than <see cref="FlatProgressBar.Maximum"/>.
        ''' </exception>
        <Category("Behavior")>
        <DefaultValue(0)>
        Public Property Minimum As Integer
            Get
                Return Me.minimumB
            End Get
            Set(value As Integer)
                If value < 0 OrElse value > Me.maximumB Then
                    Throw New ArgumentOutOfRangeException(paramName:=NameOf(value), actualValue:=value,
                                                          message:="The minimum must be between 0 and the maximum.")
                End If
                Me.minimumB = value
                Me.valueB = Math.Max(Me.valueB, value)
                Me.Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the upper bound of the range.
        ''' </summary>
        ''' <exception cref="ArgumentOutOfRangeException">
        ''' Thrown when the value is lower than <see cref="FlatProgressBar.Minimum"/>.
        ''' </exception>
        <Category("Behavior")>
        <DefaultValue(100)>
        Public Property Maximum As Integer
            Get
                Return Me.maximumB
            End Get
            Set(value As Integer)
                If value < Me.minimumB Then
                    Throw New ArgumentOutOfRangeException(paramName:=NameOf(value), actualValue:=value,
                                                          message:="The maximum cannot be lower than the minimum.")
                End If
                Me.maximumB = value
                Me.valueB = Math.Min(Me.valueB, value)
                Me.Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the current position.
        ''' </summary>
        ''' <exception cref="ArgumentOutOfRangeException">
        ''' Thrown when the value is not between <see cref="FlatProgressBar.Minimum"/> and <see cref="FlatProgressBar.Maximum"/>.
        ''' </exception>
        <Category("Behavior")>
        <DefaultValue(0)>
        Public Property Value As Integer
            Get
                Return Me.valueB
            End Get
            Set(value As Integer)
                If value < Me.minimumB OrElse value > Me.maximumB Then
                    Throw New ArgumentOutOfRangeException(paramName:=NameOf(value), actualValue:=value,
                                                          message:="The value must be between the minimum and the maximum.")
                End If
                Me.valueB = value
                Me.Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the state of the progress bar, which determines the bar color.
        ''' </summary>
        <Category("Appearance")>
        <DefaultValue(GetType(ProgressBarState), "Normal")>
        Public Property State As ProgressBarState
            Get
                Return Me.stateB
            End Get
            Set(value As ProgressBarState)
                If Not [Enum].IsDefined(GetType(ProgressBarState), value) Then
                    Throw New InvalidEnumArgumentException(NameOf(value), CInt(value), GetType(ProgressBarState))
                End If
                Me.stateB = value
                Me.Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Gets the default size of the control.
        ''' </summary>
        Protected Overrides ReadOnly Property DefaultSize As Size
            Get
                Return New Size(200, 6)
            End Get
        End Property

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="FlatProgressBar"/> class.
        ''' </summary>
        Public Sub New()
            Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                        ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
            Me.SetStyle(ControlStyles.Selectable, False)
            Me.TabStop = False
        End Sub

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Advances the current position by the specified amount, without exceeding <see cref="FlatProgressBar.Maximum"/>.
        ''' </summary>
        ''' <param name="amount">
        ''' The amount to advance.
        ''' </param>
        ''' <exception cref="ArgumentOutOfRangeException">
        ''' Thrown when <paramref name="amount"/> is negative.
        ''' </exception>
        Public Sub Increment(amount As Integer)
            If amount < 0 Then
                Throw New ArgumentOutOfRangeException(paramName:=NameOf(amount), actualValue:=amount, message:="The amount cannot be negative.")
            End If
            Me.Value = CInt(Math.Min(CLng(Me.valueB) + amount, Me.maximumB))
        End Sub

        ''' <summary>
        ''' Resets the progress bar: sets the range and the position, and the <see cref="ProgressBarState.Normal"/> state.
        ''' </summary>
        ''' <param name="maximum">
        ''' The upper bound of the range (the lower bound is zero).
        ''' </param>
        ''' <exception cref="ArgumentOutOfRangeException">
        ''' Thrown when <paramref name="maximum"/> is negative.
        ''' </exception>
        Public Sub Reset(maximum As Integer)
            If maximum < 0 Then
                Throw New ArgumentOutOfRangeException(paramName:=NameOf(maximum), actualValue:=maximum, message:="The maximum cannot be negative.")
            End If
            Me.minimumB = 0
            Me.maximumB = maximum
            Me.valueB = 0
            Me.stateB = ProgressBarState.Normal
            Me.Invalidate()
        End Sub

        ''' <summary>
        ''' Applies the specified palette to the progress bar.
        ''' </summary>
        ''' <param name="palette">
        ''' The palette to apply.
        ''' </param>
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when <paramref name="palette"/> is <see langword="Nothing"/>.
        ''' </exception>
        Public Sub ApplyTheme(palette As ThemePalette) Implements IThemeable.ApplyTheme
            If palette Is Nothing Then
                Throw New ArgumentNullException(paramName:=NameOf(palette))
            End If

            Me.normalColor = palette.AccentColor
            Me.warningColor = palette.WarningColor
            Me.errorColor = palette.ErrorColor
            Me.trackColor = palette.TrackColor
            Me.Invalidate()
        End Sub

#End Region

#Region " Friend Methods "

        ''' <summary>
        ''' Gets the fraction of the range that is completed, from 0 to 1.
        ''' </summary>
        ''' <returns>
        ''' The completed fraction (0 when the range is empty).
        ''' </returns>
        Friend Function GetFraction() As Double
            Dim range As Integer = Me.maximumB - Me.minimumB
            If range = 0 Then
                Return 0.0R
            End If
            Return CDbl(Me.valueB - Me.minimumB) / range
        End Function

        ''' <summary>
        ''' Gets the bar color of the current <see cref="FlatProgressBar.State"/>.
        ''' </summary>
        ''' <returns>
        ''' The bar color.
        ''' </returns>
        Friend Function GetBarColor() As Color
            Select Case Me.stateB
                Case ProgressBarState.Warning
                    Return Me.warningColor
                Case ProgressBarState.Error
                    Return Me.errorColor
                Case Else
                    Return Me.normalColor
            End Select
        End Function

#End Region

#Region " Event Invocators "

        ''' <summary>
        ''' Draws the progress bar.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            Dim bounds As Rectangle = Me.ClientRectangle
            Using trackBrush As New SolidBrush(Me.trackColor)
                e.Graphics.FillRectangle(trackBrush, bounds)
            End Using

            Dim barWidth As Integer = CInt(Math.Round(bounds.Width * Me.GetFraction()))
            If barWidth > 0 Then
                Using barBrush As New SolidBrush(Me.GetBarColor())
                    e.Graphics.FillRectangle(barBrush, bounds.X, bounds.Y, barWidth, bounds.Height)
                End Using
            End If
        End Sub

#End Region

    End Class

End Namespace

#End Region
