Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Text.Json

Public Class ArrangeWindowViewModel
    Public ReadOnly Property Items As New ObservableCollection(Of WindowTitleAndPosition)

    Const ConfigFileName = "ArrangeWindowConfig.json"

    Public Async Function LoadAsync() As Task
        If Not File.Exists(ConfigFileName) Then Return

        Items.Clear()

        Using jsonStrm = File.OpenRead(ConfigFileName)
            Dim data = Await JsonSerializer.DeserializeAsync(Of WindowTitleAndPosition())(jsonStrm)
            For Each item In data
                Items.Add(item)
            Next
        End Using
    End Function

    Public Sub Save()
        Using jsonStrm = File.Create(ConfigFileName)
            JsonSerializer.Serialize(jsonStrm, Items)
        End Using
    End Sub
End Class
