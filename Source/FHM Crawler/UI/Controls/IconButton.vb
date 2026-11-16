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

#Region " IconButton "

Namespace FHMCrawler.UI.Controls

    ''' <summary>
    ''' A flat button that only shows an icon, with a different image for the light and the dark themes.
    ''' </summary>
    <DesignerCategory("Code")>
    Public Class IconButton : Inherits Button : Implements IThemeable

#Region " Private Fields "

        ''' <summary>
        ''' The last applied palette.
        ''' </summary>
        Private palette As ThemePalette = ThemePalette.Light

        ''' <summary>
        ''' A value indicating whether the mouse is over the button.
        ''' </summary>
        Private isHovered As Boolean

        ''' <summary>
        ''' A value indicating whether the left mouse button is pressed over the button.
        ''' </summary>
        Private isPressed As Boolean

        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The icon shown with the light theme.
        ''' </summary>
        Private lightThemeImageB As Image

        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The icon shown with the dark theme.
        ''' </summary>
        Private darkThemeImageB As Image

        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The size, in pixels, at which the icon is drawn.
        ''' </summary>
        Private iconSizeB As Size = New Size(20, 20)

#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets or sets the icon shown with the light theme (a dark icon).
        ''' </summary>
        <Category("Appearance")>
        <DefaultValue(GetType(Image), Nothing)>
        Public Property LightThemeImage As Image
            Get
                Return Me.lightThemeImageB
            End Get
            Set(value As Image)
                Me.lightThemeImageB = value
                Me.Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the icon shown with the dark theme (a light icon).
        ''' </summary>
        <Category("Appearance")>
        <DefaultValue(GetType(Image), Nothing)>
        Public Property DarkThemeImage As Image
            Get
                Return Me.darkThemeImageB
            End Get
            Set(value As Image)
                Me.darkThemeImageB = value
                Me.Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the size, in pixels, at which the icon is drawn (centered in the button).
        ''' </summary>
        ''' <exception cref="ArgumentOutOfRangeException">
        ''' Thrown when the width or the height is not positive.
        ''' </exception>
        <Category("Appearance")>
        Public Property IconSize As Size
            Get
                Return Me.iconSizeB
            End Get
            Set(value As Size)
                If value.Width <= 0 OrElse value.Height <= 0 Then
                    Throw New ArgumentOutOfRangeException(paramName:=NameOf(value), actualValue:=value, message:="The icon size must be positive.")
                End If
                Me.iconSizeB = value
                Me.Invalidate()
            End Set
        End Property

        ''' <summary>
        ''' Gets the default size of the control.
        ''' </summary>
        Protected Overrides ReadOnly Property DefaultSize As Size
            Get
                Return New Size(32, 32)
            End Get
        End Property

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="IconButton"/> class.
        ''' </summary>
        Public Sub New()
            Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                        ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
            Me.FlatStyle = FlatStyle.Flat
            Me.UseVisualStyleBackColor = False
            Me.Cursor = Cursors.Hand
        End Sub

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Applies the specified palette to the button, and selects the icon of its theme.
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

            Me.palette = palette
            Me.Invalidate()
        End Sub

#End Region

#Region " Friend Methods "

        ''' <summary>
        ''' Gets the icon of the current theme.
        ''' </summary>
        ''' <returns>
        ''' The icon of the current theme, or <see langword="Nothing"/> if it was not set.
        ''' </returns>
        Friend Function GetCurrentImage() As Image
            Return If(Me.palette.Theme = AppTheme.Dark, Me.darkThemeImageB, Me.lightThemeImageB)
        End Function

        ''' <summary>
        ''' Gets the bounds of the icon, centered in the specified client bounds.
        ''' </summary>
        ''' <param name="clientBounds">
        ''' The client bounds of the button.
        ''' </param>
        ''' <param name="iconSize">
        ''' The size of the icon.
        ''' </param>
        ''' <returns>
        ''' The bounds of the icon.
        ''' </returns>
        Friend Shared Function GetIconBounds(clientBounds As Rectangle, iconSize As Size) As Rectangle
            Return New Rectangle(clientBounds.Left + ((clientBounds.Width - iconSize.Width) \ 2),
                                 clientBounds.Top + ((clientBounds.Height - iconSize.Height) \ 2),
                                 iconSize.Width, iconSize.Height)
        End Function

#End Region

#Region " Event Invocators "

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
            Me.isPressed = False
            Me.Invalidate()
        End Sub

        ''' <summary>
        ''' Raises the <see cref="Control.MouseDown"/> event.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            MyBase.OnMouseDown(e)
            If e.Button = MouseButtons.Left Then
                Me.isPressed = True
                Me.Invalidate()
            End If
        End Sub

        ''' <summary>
        ''' Raises the <see cref="Control.MouseUp"/> event.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
            MyBase.OnMouseUp(e)
            Me.isPressed = False
            Me.Invalidate()
        End Sub

        ''' <summary>
        ''' Draws the button.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            Dim g As Graphics = e.Graphics
            Dim bounds As Rectangle = Me.ClientRectangle

            Dim backColor As Color = Me.BackColor
            If Me.Enabled AndAlso Me.isPressed Then
                backColor = Me.palette.ButtonPressedBackColor
            ElseIf Me.Enabled AndAlso Me.isHovered Then
                backColor = Me.palette.ButtonHoverBackColor
            End If
            g.Clear(backColor)

            Dim image As Image = Me.GetCurrentImage()
            If image IsNot Nothing Then
                g.InterpolationMode = InterpolationMode.HighQualityBicubic
                g.PixelOffsetMode = PixelOffsetMode.HighQuality
                g.SmoothingMode = SmoothingMode.AntiAlias
                Dim iconBounds As Rectangle = IconButton.GetIconBounds(bounds, Me.iconSizeB)
                If Me.Enabled Then
                    g.DrawImage(image, iconBounds)
                Else
                    ControlPaint.DrawImageDisabled(g, image, iconBounds.X, iconBounds.Y, backColor)
                End If
            End If

            If Me.Focused AndAlso Me.ShowFocusCues Then
                ControlPaint.DrawFocusRectangle(g, Rectangle.Inflate(bounds, -2, -2), Me.palette.TextColor, backColor)
            End If
        End Sub

#End Region

    End Class

End Namespace

#End Region
