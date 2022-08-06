Imports System.Windows.Interop
Imports System.Windows.Threading

Public Class WindowPicker

    Public Const CenterOffset = 25
    Public ReadOnly Property TargetHwnd As IntPtr
    Public ReadOnly Property TargetTitle As String

    Private Sub Me_MouseLeftDown() Handles Me.PreviewMouseLeftButtonDown
        ForceDragMoveTimer.Stop()
        DragMove()
    End Sub

    Private Sub Me_PreviewMouseMove(sender As Object, e As MouseEventArgs) Handles Me.PreviewMouseMove
        If e.LeftButton = MouseButtonState.Pressed Then
            ForceDragMoveTimer.Stop()
            DragMove()
        End If
    End Sub

    Private Sub Me_MouseLeftUp(sender As Object, e As MouseButtonEventArgs) Handles Me.PreviewMouseLeftButtonUp
        Hide()

        Dim screenPos As PointInt32
        If GetCursorPos(screenPos) <> 0 Then
            _TargetHwnd = WindowFromPoint(
            New PointInt32 With {.X = screenPos.X, .Y = screenPos.Y})

            If _TargetHwnd <> Nothing Then
                Dim title As New String(ControlChars.NullChar, 1000)
                Dim titleLength = GetWindowText(_TargetHwnd, title, 999)
                _TargetTitle = title.Substring(0, titleLength)
            End If
        End If

        Close()
    End Sub

    WithEvents ForceDragMoveTimer As New DispatcherTimer With {
        .Interval = TimeSpan.FromMilliseconds(100)
    }

    Private _loaded As Boolean
    Private Sub WindowPicker_Loaded() Handles Me.Loaded
        If _loaded Then Return
        _loaded = True

        Dim hWnd = New WindowInteropHelper(Me).Handle
        Dim oldLong = GetWindowLong(hWnd, GWL_STYLE)
        SetWindowLong(hWnd, GWL_STYLE,
                      RemoveFlag(oldLong, WS_MINIMIZEBOX Or WS_MAXIMIZEBOX))

        MoveToCursor()
        ForceDragMoveTimer.Start()
    End Sub

    Private Sub MoveToCursor()
        Dim screenPos As PointInt32
        Dim scale = VisualTreeHelper.GetDpi(Me)
        If GetCursorPos(screenPos) <> 0 Then
            Left = screenPos.X / scale.DpiScaleX - CenterOffset
            Top = screenPos.Y / scale.DpiScaleY - CenterOffset
        End If
    End Sub

    Private Sub ForceDragMoveTimer_Tick() Handles ForceDragMoveTimer.Tick
        MoveToCursor()
    End Sub
End Class
