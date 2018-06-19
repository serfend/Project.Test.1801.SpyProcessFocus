Attribute VB_Name = "MathSub"
Option Explicit
Public Declare Function GetTickCount Lib "kernel32" () As Long
Public Function GetColors() As Long
GetColors = RGB(Rnd * 100 + 100, Rnd * 155 + 100, Rnd * 100 + 100)
End Function
Public Function GetColor(Index As Long) As Long
GetColor = RGB(128 + Index ^ 1.5 Mod 128, 128 + Index ^ 2 Mod 128, 128 + Index ^ 2.5 Mod 128)
End Function
'用消耗堆栈的二分法从小到大排序，这是VB6使用最方便、最高效、稳定的排序方法之一
Public Sub QuickSortStoB(ByRef arrValue() As Long, ByVal lngLx As Long, ByVal lngRx As Long)
    Dim TempValue    As Long
    Dim NumI           As Long
    Dim NumJ          As Long

    NumI = lngLx
    NumJ = lngRx
    Do
        While arrValue(NumI) <= arrValue(NumJ) And NumI < NumJ
        NumI = NumI + 1
        Wend
        If NumI < NumJ Then
            TempValue = arrValue(NumI)
            arrValue(NumI) = arrValue(NumJ)
            arrValue(NumJ) = TempValue
        End If
        While arrValue(NumI) <= arrValue(NumJ) And NumI < NumJ
        NumJ = NumJ - 1
        Wend
        If NumI < NumJ Then
            TempValue = arrValue(NumI)
            arrValue(NumI) = arrValue(NumJ)
            arrValue(NumJ) = TempValue
        End If
    Loop Until NumI = NumJ
    NumI = NumI - 1
    NumJ = NumJ + 1

    If NumI > lngLx Then
        QuickSortStoB arrValue, lngLx, NumI
    End If
    If NumJ < lngRx Then
        QuickSortStoB arrValue, NumJ, lngRx
    End If
End Sub
Public Sub SwapeL(A1 As Long, A2 As Long)
Dim Temp As Long
Temp = A1
A1 = A2
A2 = Temp
End Sub
Public Sub SwapeS(A1 As Single, A2 As Single)
Dim Temp As Single
Temp = A1
A1 = A2
A2 = Temp
End Sub
Public Sub SwapeD(A1 As Double, A2 As Double)
Dim Temp As Double
Temp = A1
A1 = A2
A2 = Temp
End Sub
Public Sub SwapeC(A1 As String, A2 As String)
Dim Temp As String
Temp = A1
A1 = A2
A2 = Temp
End Sub
Public Function TimeSci(Time0 As Double) As String
'Debug.Print Time0
If Time0 < 0 Then
    TimeSci = 0
ElseIf Time0 < 0.9 Then
    If Time0 = 0 Then
        TimeSci = "0.0"
    Else
        TimeSci = 0 & Round(Time0, 1)
    End If
ElseIf Time0 < 10 Then
    If Time0 * 10 Mod 10 = 0 Then
        TimeSci = Round(Time0, 1) & ".0"
    Else
        TimeSci = Round(Time0, 1)
    End If
Else
    TimeSci = Round(Time0, 0)
End If
End Function
Public Function SpaceGet(Text0 As String, SumChar As Long) As String
Dim i As Long, Tem As String, Lens As Long, sL As Long
Lens = Len(Text0)
sL = SumChar
If SumChar < Lens Then SpaceGet = False: SpaceGet = "": Exit Function
For i = 1 To Lens
    If Asc(Mid(Text0, i, 1)) >= 45 And Asc(Mid(Text0, i, 1)) <= 122 Then
        
    Else
        sL = sL - 1
    End If
Next
    SpaceGet = Space(sL - Lens)
End Function

