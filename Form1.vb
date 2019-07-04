Public Class MouseJiggler
    Dim CurrentMousePosition As Point
    Dim Sequence As Int16 = 1
    Dim drag As Boolean
    Dim mousex As Integer
    Dim mousey As Integer
    Dim startminimised As Integer
    '---------------------------------------------------------------------------
    '-----                   Software Mouse Jiggler                       ------
    '-----  Free Opensource alternative to a hardware Mouse Jiggler       ------
    '-----    Written by James Franklin for MouseJigglers.com.au          ------
    '-----                 Last Updated July/2019                         ------
    '---------------------------------------------------------------------------
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()

    End Sub

    Private Sub btnEnable_Click(sender As Object, e As EventArgs) Handles btnEnable.Click

        Select Case btnEnable.Text
            Case "Disable"
                StopMovement()

            Case "Enable"
                StartMovement()

        End Select

    End Sub

    Private Sub StartMovement()
        btnEnable.Text = "Disable"
        lblStatus.Text = "On"
        StartToolStripMenuItem.Enabled = False
        StopToolStripMenuItem.Enabled = True
        tmr_movement.Enabled = True
    End Sub

    Private Sub StopMovement()
        btnEnable.Text = "Enable"
        lblStatus.Text = "Off"
        StopToolStripMenuItem.Enabled = False
        StartToolStripMenuItem.Enabled = True
        tmr_movement.Enabled = False

    End Sub



    Private Sub tmr_movement_Tick(sender As Object, e As EventArgs) Handles tmr_movement.Tick

        tmr_movement.Interval = Int((20000 * Rnd()) + 10000)

        CurrentMousePosition = Cursor.Position
        Select Case Sequence
            Case 1
                Cursor.Position = New Point(CurrentMousePosition.X + 1, CurrentMousePosition.Y)
                Sequence = 2
            Case 2
                Cursor.Position = New Point(CurrentMousePosition.X, CurrentMousePosition.Y + 1)
                Sequence = 3
            Case 3
                Cursor.Position = New Point(CurrentMousePosition.X - 100, CurrentMousePosition.Y)
                Sequence = 4
            Case 4
                Cursor.Position = New Point(CurrentMousePosition.X, CurrentMousePosition.Y - 1)
                Sequence = 1
        End Select

    End Sub

    Private Sub MouseJiggler_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        drag = True
        mousex = Windows.Forms.Cursor.Position.X - Me.Left
        mousey = Windows.Forms.Cursor.Position.Y - Me.Top
    End Sub

    Private Sub MouseJiggler_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        If drag Then
            Me.Top = Windows.Forms.Cursor.Position.Y - mousey
            Me.Left = Windows.Forms.Cursor.Position.X - mousex
        End If
    End Sub

    Private Sub MouseJiggler_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
        drag = False
        SaveSetting(AppName:="Mouse Jiggler", Section:="Config", Key:="DefaultLocationX", Setting:=Me.Location.X)
        SaveSetting(AppName:="Mouse Jiggler", Section:="Config", Key:="DefaultLocationY", Setting:=Me.Location.Y)
    End Sub

    Private Sub btnMinimise_Click(sender As Object, e As EventArgs) Handles btnMinimise.Click


        If startminimised = -1 Then
            Dim result As Integer = MessageBox.Show("Do you want Mouse Jiggler to start minimised in future", "Mouse Jiggler", MessageBoxButtons.YesNo)
            If result = DialogResult.No Then
                startminimised = 0
            ElseIf result = DialogResult.Yes Then
                startminimised = 1
            End If

            SaveSetting(AppName:="Mouse Jiggler", Section:="Config", Key:="StartMinimised", Setting:=startminimised)
        End If

        Me.WindowState = FormWindowState.Minimized
        NotifyIcon.Visible = True
    End Sub

    Private Sub MouseJiggler_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim Startx As Int16
        Dim starty As Int16
        startminimised = GetSetting("Mouse Jiggler", "Config", "StartMinimised", -1)

        Startx = GetSetting("Mouse Jiggler", "Config", "DefaultLocationX", 500)
        starty = GetSetting("Mouse Jiggler", "Config", "DefaultLocationY", 500)

        Me.Location = New Point(Startx, starty)


        StartToolStripMenuItem.Enabled = False
        StopToolStripMenuItem.Enabled = True

        If startminimised = 1 Then
            Me.WindowState = FormWindowState.Minimized
            YesToolStripMenuItem.Enabled = False
        Else
            Me.WindowState = FormWindowState.Normal
            NoToolStripMenuItem.Enabled = False
        End If

    End Sub

    Private Sub NotifyIcon_DoubleClick(sender As Object, e As EventArgs) Handles NotifyIcon.DoubleClick
        Me.WindowState = FormWindowState.Normal
    End Sub

    Private Sub YesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles YesToolStripMenuItem.Click
        SaveSetting(AppName:="Mouse Jiggler", Section:="Config", Key:="StartMinimised", Setting:=1)
        NoToolStripMenuItem.Enabled = True
        YesToolStripMenuItem.Enabled = False
    End Sub

    Private Sub NoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NoToolStripMenuItem.Click
        SaveSetting(AppName:="Mouse Jiggler", Section:="Config", Key:="StartMinimised", Setting:=0)


        NoToolStripMenuItem.Enabled = False
        YesToolStripMenuItem.Enabled = True
    End Sub

    Private Sub StartToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StartToolStripMenuItem.Click
        StartMovement()
    End Sub

    Private Sub StopToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StopToolStripMenuItem.Click
        StopMovement()
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        MsgBox("Freeware: Developed by www.mousejigglers.com.au  ")
    End Sub

    Private Sub ToolStripMenuItemExit_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemExit.Click
        End
    End Sub
End Class
