Imports System.Runtime.InteropServices

Module NativeMethods
    Declare Unicode Function GetWindowText Lib "user32" Alias "GetWindowTextW" (
         hwnd As IntPtr, lpString As String, cch As Integer) As Integer
    Declare Function WindowFromPoint Lib "user32" (
         pt As PointInt32) As IntPtr
    Declare Function GetCursorPos Lib "user32" (ByRef lpPoint As PointInt32) As Integer

    Private Declare Unicode Function SetWindowLong32 Lib "user32" Alias "SetWindowLongW" (
        hWnd As IntPtr, nIndex As Integer, dwNewLong As Integer) As Integer

    Private Declare Unicode Function SetWindowLongPtr64 Lib "user32" Alias "SetWindowLongPtrW" (
        hWnd As IntPtr, nIndex As Integer, dwNewLong As IntPtr) As IntPtr

    Public Function SetWindowLong(hWnd As IntPtr, nIndex As Integer, dwNewLong As IntPtr) As IntPtr
        If IntPtr.Size = 8 Then
            Return SetWindowLongPtr64(hWnd, nIndex, dwNewLong)
        Else
            Return New IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32))
        End If
    End Function

    Private Declare Unicode Function GetWindowLong32 Lib "user32" Alias "GetWindowLongW" (
        hWnd As IntPtr, nIndex As Integer) As IntPtr

    Private Declare Unicode Function GetWindowLongPtr64 Lib "user32" Alias "GetWindowLongPtrW" (
        hWnd As IntPtr, nIndex As Integer) As IntPtr

    Public Function GetWindowLong(hWnd As IntPtr, nIndex As Integer) As IntPtr
        If IntPtr.Size = 8 Then
            Return GetWindowLongPtr64(hWnd, nIndex)
        Else
            Return GetWindowLong32(hWnd, nIndex)
        End If
    End Function

    Declare Function GetWindowRect Lib "user32" (
        hWnd As IntPtr, ByRef lpRect As RectInt32) As Integer

    Declare Unicode Function FindWindow Lib "user32" Alias "FindWindowW" (
        <MarshalAs(UnmanagedType.LPWStr)>
        lpClassName As String,
        <MarshalAs(UnmanagedType.LPWStr)>
        lpWindowName As String) As IntPtr

    Declare Function SetWindowPos Lib "user32.dll" (
        hWnd As IntPtr, hWndInsertAfter As IntPtr,
        X As Integer, Y As Integer, cx As Integer, cy As Integer,
        uFlags As Integer) As Integer

End Module

Structure PointInt32
    Dim X, Y As Integer
End Structure

Structure RectInt32
    Dim X, Y, Right, Bottom As Integer
End Structure

Module NativeValues
    Public Const GWL_STYLE = -16
    Public Const WS_MAXIMIZEBOX = &H10000
    Public Const WS_MINIMIZEBOX = &H20000
    Public Const SWP_NOSIZE = 1
    Public Const SWP_NOACTIVATE = &H10

    Function RemoveFlag(srcValue As IntPtr, targetValue As IntPtr) As IntPtr
        Return srcValue.ToInt64 And (Not targetValue.ToInt64)
    End Function
End Module
