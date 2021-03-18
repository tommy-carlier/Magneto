Imports TC.Magneto
Imports TC.Magneto.Modules

<Constant("meaningOfLife", 42)> _
Public Class TestModule
    Inherits MagnetoModule

    Protected Overrides Sub StartCore()
        ' overriding StartCore is optional
        Console.WriteLine("Starting TestModule")
    End Sub

    Protected Overrides Sub StopCore()
        ' overriding StopCore is optional
        Console.WriteLine("Stopping TestModule")
    End Sub

    <[Function]("swap")> _
    Public Sub Swap(ByRef x As Long, ByRef y As Long)
        Dim tmp As Long = x
        x = y
        y = tmp
    End Sub

    <[Function]("double")> _
    Public Function [Double](ByVal x As Long) As Long
        Return x * 2
    End Function
End Class
