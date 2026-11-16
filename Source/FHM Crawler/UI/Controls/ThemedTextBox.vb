#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel

Imports FHMCrawler.UI.Theming

#End Region

#Region " ThemedTextBox "

Namespace FHMCrawler.UI.Controls

    ''' <summary>
    ''' A single-line text box with a flat 1-pixel border, drawn with the colors of the current <see cref="ThemePalette"/>.
    ''' <para></para>
    ''' It hosts a borderless <see cref="TextBox"/>, because the native text box border cannot be recolored.
    ''' </summary>
    <DesignerCategory("Code")>
    <DefaultEvent(NameOf(Control.TextChanged))>
    <DefaultProperty(NameOf(Control.Text))>
    Public Class ThemedTextBox : Inherits Control : Implements IThemeable

#Region " Constants "

        ''' <summary>
        ''' The <c>EM_SETCUEBANNER</c> message, that sets the textual cue (placeholder) of an edit control.
        ''' </summary>
        Private Const EM_SETCUEBANNER As Integer = &H1501

        ''' <summary>
        ''' The horizontal space, in pixels, between the border and the text.
        ''' </summary>
        Private Const HorizontalPadding As Integer = 8

#End Region

#Region " Private Fields "

        ''' <summary>
        ''' The hosted text box.
        ''' </summary>
        Private ReadOnly innerTextBox As TextBox

        ''' <summary>
        ''' ( Backing Field )
        ''' <para></para>
        ''' The textual cue (placeholder) shown while the text box is empty.
        ''' </summary>
        Private cueBannerB As String = ""

        ''' <summary>
        ''' The border color.
        ''' </summary>
        Private borderColor As Color = SystemColors.ControlDark

        ''' <summary>
        ''' The border color while the text box has the focus.
        ''' </summary>
        Private focusBorderColor As Color = SystemColors.Highlight

#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets or sets the text of the text box.
        ''' </summary>
        <Browsable(True)>
        <EditorBrowsable(EditorBrowsableState.Always)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
        Public Overrides Property Text As String
            Get
                Return Me.innerTextBox.Text
            End Get
            Set(value As String)
                Me.innerTextBox.Text = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the textual cue (placeholder) shown while the text box is empty.
        ''' </summary>
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when the value is <see langword="Nothing"/>.
        ''' </exception>
        <Category("Appearance")>
        <DefaultValue("")>
        Public Property CueBanner As String
            Get
                Return Me.cueBannerB
            End Get
            Set(value As String)
                If value Is Nothing Then
                    Throw New ArgumentNullException(paramName:=NameOf(value))
                End If
                Me.cueBannerB = value
                Me.UpdateCueBanner()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a value indicating whether the text is read-only.
        ''' </summary>
        <Category("Behavior")>
        <DefaultValue(False)>
        Public Property [ReadOnly] As Boolean
            Get
                Return Me.innerTextBox.ReadOnly
            End Get
            Set(value As Boolean)
                Me.innerTextBox.ReadOnly = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the maximum number of characters that can be typed.
        ''' </summary>
        <Category("Behavior")>
        <DefaultValue(32767)>
        Public Property MaxLength As Integer
            Get
                Return Me.innerTextBox.MaxLength
            End Get
            Set(value As Integer)
                Me.innerTextBox.MaxLength = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the hosted text box.
        ''' </summary>
        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public ReadOnly Property TextBox As TextBox
            Get
                Return Me.innerTextBox
            End Get
        End Property

        ''' <summary>
        ''' Gets the default size of the control.
        ''' </summary>
        Protected Overrides ReadOnly Property DefaultSize As Size
            Get
                Return New Size(200, 30)
            End Get
        End Property

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="ThemedTextBox"/> class.
        ''' </summary>
        Public Sub New()
            Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                        ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
            ' The hosted text box receives the focus, not this container.
            Me.SetStyle(ControlStyles.Selectable, False)

            Me.innerTextBox = New TextBox With {
                .BorderStyle = BorderStyle.None,
                .Multiline = False
            }
            AddHandler Me.innerTextBox.TextChanged, AddressOf Me.InnerTextBox_TextChanged
            AddHandler Me.innerTextBox.GotFocus, AddressOf Me.InnerTextBox_FocusChanged
            AddHandler Me.innerTextBox.LostFocus, AddressOf Me.InnerTextBox_FocusChanged
            AddHandler Me.innerTextBox.HandleCreated, AddressOf Me.InnerTextBox_HandleCreated
            Me.Controls.Add(Me.innerTextBox)
        End Sub

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Applies the specified palette to the text box.
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

            Me.BackColor = palette.InputBackColor
            Me.ForeColor = palette.TextColor
            Me.innerTextBox.BackColor = palette.InputBackColor
            Me.innerTextBox.ForeColor = palette.TextColor
            Me.borderColor = palette.BorderColor
            Me.focusBorderColor = palette.AccentColor
            Me.Invalidate()
        End Sub

        ''' <summary>
        ''' Selects all the text of the text box.
        ''' </summary>
        Public Sub SelectAll()
            Me.innerTextBox.SelectAll()
        End Sub

#End Region

#Region " Event Invocators "

        ''' <summary>
        ''' Raises the <see cref="Control.Layout"/> event and places the hosted text box.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnLayout(e As LayoutEventArgs)
            MyBase.OnLayout(e)

            Dim width As Integer = Math.Max(0, Me.ClientSize.Width - (ThemedTextBox.HorizontalPadding * 2))
            Dim y As Integer = Math.Max(1, (Me.ClientSize.Height - Me.innerTextBox.PreferredHeight) \ 2)
            Me.innerTextBox.SetBounds(ThemedTextBox.HorizontalPadding, y, width, Me.innerTextBox.PreferredHeight)
        End Sub

        ''' <summary>
        ''' Raises the <see cref="Control.FontChanged"/> event and propagates the font to the hosted text box.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnFontChanged(e As EventArgs)
            MyBase.OnFontChanged(e)
            Me.innerTextBox.Font = Me.Font
            Me.PerformLayout()
        End Sub

        ''' <summary>
        ''' Raises the <see cref="Control.MouseDown"/> event and gives the focus to the hosted text box.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            MyBase.OnMouseDown(e)
            Me.innerTextBox.Focus()
        End Sub

        ''' <summary>
        ''' Raises the <see cref="Control.Enter"/> event and gives the focus to the hosted text box.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnEnter(e As EventArgs)
            MyBase.OnEnter(e)
            Me.innerTextBox.Focus()
        End Sub

        ''' <summary>
        ''' Draws the background and the border.
        ''' </summary>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            e.Graphics.Clear(Me.BackColor)

            Dim color As Color = If(Me.innerTextBox.Focused, Me.focusBorderColor, Me.borderColor)
            Using pen As New Pen(color)
                e.Graphics.DrawRectangle(pen, 0, 0, Me.ClientSize.Width - 1, Me.ClientSize.Height - 1)
            End Using
        End Sub

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Sends the textual cue (placeholder) to the hosted text box.
        ''' </summary>
        Private Sub UpdateCueBanner()
            If Me.innerTextBox.IsHandleCreated Then
                ' wParam = 1: the cue is also shown while the text box has the focus.
                NativeMethods.SendMessage(Me.innerTextBox.Handle, ThemedTextBox.EM_SETCUEBANNER, New IntPtr(1), Me.cueBannerB)
            End If
        End Sub

        ''' <summary>
        ''' Raises the <see cref="Control.TextChanged"/> event of this control when the hosted text box text changes.
        ''' </summary>
        ''' <param name="sender">
        ''' The source of the event.
        ''' </param>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Private Sub InnerTextBox_TextChanged(sender As Object, e As EventArgs)
            Me.OnTextChanged(e)
        End Sub

        ''' <summary>
        ''' Redraws the border when the hosted text box gets or loses the focus.
        ''' </summary>
        ''' <param name="sender">
        ''' The source of the event.
        ''' </param>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Private Sub InnerTextBox_FocusChanged(sender As Object, e As EventArgs)
            Me.Invalidate()
        End Sub

        ''' <summary>
        ''' Sends the textual cue to the hosted text box once its handle exists.
        ''' </summary>
        ''' <param name="sender">
        ''' The source of the event.
        ''' </param>
        ''' <param name="e">
        ''' The event data.
        ''' </param>
        Private Sub InnerTextBox_HandleCreated(sender As Object, e As EventArgs)
            Me.UpdateCueBanner()
        End Sub

#End Region

    End Class

End Namespace

#End Region
