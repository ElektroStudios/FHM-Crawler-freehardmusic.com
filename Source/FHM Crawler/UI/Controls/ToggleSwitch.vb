#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Drawing.Drawing2D

Imports FHMCrawler.UI.Theming

#End Region

#Region " ToggleSwitch "

Namespace FHMCrawler.UI.Controls

    ''' <summary>
    ''' An on/off switch: a <see cref="CheckBox"/> drawn as a rounded track with a sliding thumb.
    ''' <para></para>
    ''' It keeps the <see cref="CheckBox"/> behavior (<see cref="CheckBox.Checked"/>,
    ''' <see cref="CheckBox.CheckedChanged"/>, keyboard and accessibility support).
    ''' </summary>
    <DesignerCategory("Code")>
    <DefaultEvent(NameOf(CheckBox.CheckedChanged))>
    Public Class ToggleSwitch : Inherits CheckBox : Implements IThemeable

#Region " Constants "

        ''' <summary>
        ''' The amount that the thumb position advances in every animation step (from 0 to 1).
        ''' </summary>
        Private Const AnimationStep As Single = 0.25F

#End Region

#Region " Private Fields "

        ''' <summary>
        ''' The timer that animates the thumb.
        ''' </summary>
        Private ReadOnly animationTimer As Timer

        ''' <summary>
        ''' The current thumb position: 0 is the "off" position, 1 is the "on" position.
        ''' </summary>
        Private thumbPosition As Single

        ''' <summary>
        ''' A value indicating whether the mouse is over the control.
        ''' </summary>
        Private isHovered As Boolean

        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The track color in the "on" state.
        ''' </summary>
        Private onColorB As Color = SystemColors.Highlight

        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The track color in the "on" state when the mouse is over the control.
        ''' </summary>
        Private onHoverColorB As Color = SystemColors.HotTrack

        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The track color in the "off" state.
        ''' </summary>
        Private offColorB As Color = SystemColors.ControlDark

        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The thumb color.
        ''' </summary>
        Private thumbColorB As Color = Color.White

        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The color of the focus border.
        ''' </summary>
        Private focusColorB As Color = SystemColors.ControlText

#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets or sets the track color in the "on" state.
        ''' </summary>
        <Category("Appearance")>
        Public Property OnColor As Color
            Get
                Return Me.onColorB
            End Get
            Set(value As Color)
                Me.onColorB = value
                Me.Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the track color in the "on" state when the mouse is over the control.
        ''' </summary>
        <Category("Appearance")>
        Public Property OnHoverColor As Color
            Get
                Return Me.onHoverColorB
            End Get
            Set(value As Color)
                Me.onHoverColorB = value
                Me.Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the track color in the "off" state.
        ''' </summary>
        <Category("Appearance")>
        Public Property OffColor As Color
            Get
                Return Me.offColorB
            End Get
            Set(value As Color)
                Me.offColorB = value
                Me.Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the thumb color.
        ''' </summary>
        <Category("Appearance")>
        Public Property ThumbColor As Color
            Get
                Return Me.thumbColorB
            End Get
            Set(value As Color)
                Me.thumbColorB = value
                Me.Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the color of the border drawn when the control has the keyboard focus.
        ''' </summary>
        <Category("Appearance")>
        Public Property FocusColor As Color
            Get
                Return Me.focusColorB
            End Get
            Set(value As Color)
                Me.focusColorB = value
                Me.Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Gets the default size of the control.
        ''' </summary>
        Protected Overrides ReadOnly Property DefaultSize As Size
            Get
                Return New Size(44, 22)
            End Get
        End Property

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="ToggleSwitch"/> class.
        ''' </summary>
        Public Sub New()
            Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                        ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
            Me.AutoSize = False
            Me.Appearance = Appearance.Normal
            Me.Cursor = Cursors.Hand

            Me.animationTimer = New Timer With {.Interval = 15}
            AddHandler Me.animationTimer.Tick, AddressOf Me.AnimationTimer_Tick
        End Sub

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Applies the specified palette to the switch.
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

            Me.OnColor = palette.AccentColor
            Me.OnHoverColor = palette.AccentHoverColor
            Me.OffColor = palette.TrackColor
            Me.ThumbColor = Color.White
            Me.FocusColor = palette.TextColor
        End Sub

#End Region

#Region " Event Invocators "

        ''' <summary>
        ''' Raises the <see cref="CheckBox.CheckedChanged"/> event and starts the thumb animation.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnCheckedChanged(e As EventArgs)
            MyBase.OnCheckedChanged(e)

            If Me.IsHandleCreated AndAlso Me.Visible Then
                Me.animationTimer.Start()
            Else
                Me.thumbPosition = If(Me.Checked, 1.0F, 0.0F)
                Me.Invalidate()
            End If
        End Sub

        ''' <summary>
        ''' Raises the <see cref="Control.HandleCreated"/> event and places the thumb without animation.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnHandleCreated(e As EventArgs)
            MyBase.OnHandleCreated(e)
            Me.thumbPosition = If(Me.Checked, 1.0F, 0.0F)
        End Sub

        ''' <summary>
        ''' Raises the <see cref="Control.MouseEnter"/> event.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnMouseEnter(e As EventArgs)
            MyBase.OnMouseEnter(e)
            Me.isHovered = True
            Me.Invalidate()
        End Sub

        ''' <summary>
        ''' Raises the <see cref="Control.MouseLeave"/> event.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnMouseLeave(e As EventArgs)
            MyBase.OnMouseLeave(e)
            Me.isHovered = False
            Me.Invalidate()
        End Sub

        ''' <summary>
        ''' Draws the switch.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            Dim g As Graphics = e.Graphics
            g.Clear(Me.BackColor)
            g.SmoothingMode = SmoothingMode.AntiAlias

            Dim track As Rectangle = Me.GetTrackBounds()
            If track.Width <= 0 OrElse track.Height <= 0 Then
                Exit Sub
            End If

            ' Track color: blend between the "off" and "on" colors following the thumb position.
            Dim onColor As Color = If(Me.isHovered, Me.onHoverColorB, Me.onColorB)
            Dim trackColor As Color = ToggleSwitch.Blend(Me.offColorB, onColor, Me.thumbPosition)
            If Not Me.Enabled Then
                trackColor = ToggleSwitch.Blend(trackColor, Me.BackColor, 0.5F)
            End If

            Using path As GraphicsPath = ToggleSwitch.CreatePillPath(track),
                  trackBrush As New SolidBrush(trackColor)
                g.FillPath(trackBrush, path)
            End Using

            ' Thumb.
            Dim thumbMargin As Integer = 3
            Dim thumbSize As Integer = track.Height - (thumbMargin * 2)
            Dim thumbTravel As Integer = track.Width - thumbSize - (thumbMargin * 2)
            Dim thumbX As Single = track.X + thumbMargin + (thumbTravel * Me.thumbPosition)
            Using thumbBrush As New SolidBrush(Me.thumbColorB)
                g.FillEllipse(thumbBrush, thumbX, CSng(track.Y + thumbMargin), CSng(thumbSize), CSng(thumbSize))
            End Using

            ' Focus border (only when the focus was set with the keyboard).
            If Me.Focused AndAlso Me.ShowFocusCues Then
                Dim focusBounds As Rectangle = Rectangle.Inflate(track, 2, 2)
                Using path As GraphicsPath = ToggleSwitch.CreatePillPath(focusBounds),
                      focusPen As New Pen(Me.focusColorB) With {.DashStyle = DashStyle.Dot}
                    g.DrawPath(focusPen, path)
                End Using
            End If
        End Sub

        ''' <summary>
        ''' Releases the resources used by the control.
        ''' </summary>
        ''' <param name="disposing">
        ''' <see langword="True"/> to release both managed and unmanaged resources.
        ''' </param>
        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing Then
                Me.animationTimer.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

#End Region

#Region " Friend Methods "

        ''' <summary>
        ''' Gets the current thumb position: 0 is the "off" position, 1 is the "on" position.
        ''' </summary>
        ''' <returns>
        ''' The thumb position.
        ''' </returns>
        Friend Function GetThumbPosition() As Single
            Return Me.thumbPosition
        End Function

        ''' <summary>
        ''' Blends two colors.
        ''' </summary>
        ''' <param name="color1">
        ''' The first color.
        ''' </param>
        ''' <param name="color2">
        ''' The second color.
        ''' </param>
        ''' <param name="amount">
        ''' The amount of the second color, from 0 (only <paramref name="color1"/>) to 1 (only <paramref name="color2"/>).
        ''' </param>
        ''' <returns>
        ''' The blended color.
        ''' </returns>
        ''' <exception cref="ArgumentOutOfRangeException">
        ''' Thrown when <paramref name="amount"/> is not between 0 and 1.
        ''' </exception>
        Friend Shared Function Blend(color1 As Color, color2 As Color, amount As Single) As Color
            If amount < 0.0F OrElse amount > 1.0F Then
                Throw New ArgumentOutOfRangeException(paramName:=NameOf(amount), actualValue:=amount, message:="The amount must be between 0 and 1.")
            End If

            Return Color.FromArgb(CInt(Math.Round(color1.R + ((CInt(color2.R) - color1.R) * amount))),
                                  CInt(Math.Round(color1.G + ((CInt(color2.G) - color1.G) * amount))),
                                  CInt(Math.Round(color1.B + ((CInt(color2.B) - color1.B) * amount))))
        End Function

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Gets the bounds of the track, centered in the control and leaving room for the focus border.
        ''' </summary>
        ''' <returns>
        ''' The bounds of the track.
        ''' </returns>
        Private Function GetTrackBounds() As Rectangle
            Dim height As Integer = Math.Min(Me.ClientSize.Height - 4, (Me.ClientSize.Width - 4) \ 2)
            Dim width As Integer = height * 2
            Return New Rectangle((Me.ClientSize.Width - width) \ 2, (Me.ClientSize.Height - height) \ 2, width, height)
        End Function

        ''' <summary>
        ''' Creates a pill-shaped (fully rounded rectangle) path.
        ''' </summary>
        ''' <param name="bounds">
        ''' The bounds of the pill.
        ''' </param>
        ''' <returns>
        ''' The path.
        ''' </returns>
        Private Shared Function CreatePillPath(bounds As Rectangle) As GraphicsPath
            Dim diameter As Integer = bounds.Height
            Dim path As New GraphicsPath()
            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 90, 180)
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 180)
            path.CloseFigure()
            Return path
        End Function

        ''' <summary>
        ''' Moves the thumb one animation step towards its target position.
        ''' </summary>
        ''' <param name="sender">
        ''' The source of the event.
        ''' </param>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Private Sub AnimationTimer_Tick(sender As Object, e As EventArgs)
            Dim target As Single = If(Me.Checked, 1.0F, 0.0F)
            If Me.thumbPosition < target Then
                Me.thumbPosition = Math.Min(target, Me.thumbPosition + ToggleSwitch.AnimationStep)
            Else
                Me.thumbPosition = Math.Max(target, Me.thumbPosition - ToggleSwitch.AnimationStep)
            End If

            If Me.thumbPosition = target Then
                Me.animationTimer.Stop()
            End If
            Me.Invalidate()
        End Sub

#End Region

    End Class

End Namespace

#End Region
