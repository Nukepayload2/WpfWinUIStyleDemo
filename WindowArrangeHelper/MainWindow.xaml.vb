Class MainWindow

    Private ReadOnly _viewModel As New ArrangeWindowViewModel

    Private Sub BtnPickWindow_PreviewMouseLeftButtonDown() Handles BtnPickWindow.PreviewMouseLeftButtonDown
        Dim picker As New WindowPicker
        picker.ShowDialog()

        If picker.TargetHwnd <> 0 Then
            Dim curDpi = VisualTreeHelper.GetDpi(Me)
            Dim pickedRect As RectInt32 = Nothing
            If GetWindowRect(picker.TargetHwnd, pickedRect) = 0 Then
                MsgBox("选择了窗口，但是不知道位置和大小，已取消。")
                Return
            End If

            _viewModel.Items.Add(New WindowTitleAndPosition With {
                                     .DpiXWhenCapturing = curDpi.PixelsPerInchX,
                                     .DpiYWhenCapturing = curDpi.PixelsPerInchY,
                                     .Height = pickedRect.Bottom - pickedRect.Y,
                                     .Width = pickedRect.Right - pickedRect.X,
                                     .Left = pickedRect.X,
                                     .Top = pickedRect.Y,
                                     .Title = picker.TargetTitle
                                 })
        End If
    End Sub

    Private _loaded As Boolean
    Private Async Sub MainWindow_Loaded() Handles Me.Loaded
        If _loaded Then Return
        _loaded = True

        IsEnabled = False
        Try
            Await _viewModel.LoadAsync()

            LstArrangeWindows.ItemsSource = _viewModel.Items
        Catch ex As Exception
            MsgBox(ex.Message, vbExclamation, "载入失败")
        Finally
            IsEnabled = True
        End Try
    End Sub

    Private Sub MainWindow_Closed() Handles Me.Closed
        Try
            _viewModel.Save()
        Catch ex As Exception
            MsgBox(ex.Message, vbExclamation, "保存失败")
        End Try
    End Sub

    Private Sub BtnRecoverWindowPos_Click() Handles BtnRecoverWindowPos.Click
        For Each wnd In _viewModel.Items
            Dim hWnd = FindWindow(Nothing, wnd.Title)
            If hWnd = Nothing Then Continue For

            SetWindowPos(hWnd, Nothing, wnd.Left, wnd.Top, 0, 0,
                         SWP_NOSIZE Or SWP_NOACTIVATE)
        Next
    End Sub
End Class
