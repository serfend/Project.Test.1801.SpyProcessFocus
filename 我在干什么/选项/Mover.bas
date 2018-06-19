Attribute VB_Name = "Mover"
Option Explicit
Private FrmBarShowOut As Boolean
Private Declare Function GetWindowLong Lib "user32.dll" Alias "GetWindowLongA" (ByVal hwnd As Long, ByVal nIndex As Long) As Long
Private Declare Function SetLayeredWindowAttributes Lib "user32.dll" (ByVal hwnd As Long, ByVal crKey As Long, ByVal bAlpha As Byte, ByVal dwFlags As Long) As Long
Private Declare Function SetWindowLong Lib "user32.dll" Alias "SetWindowLongA" (ByVal hwnd As Long, ByVal nIndex As Long, ByVal dwNewLong As Long) As Long
Private Declare Function SetWindowPos Lib "user32" (ByVal hwnd As Long, ByVal hWndInsertAfter As Long, ByVal x As Long, ByVal y As Long, ByVal cx As Long, ByVal cy As Long, ByVal wFlags As Long) As Long

Private Declare Function ReleaseCapture Lib "user32" () As Long
Private Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hwnd As Long, ByVal wMsg As Long, ByVal wParam As Long, lParam As Any) As Long
Private Const WM_NCLBUTTONDOWN = &HA1
Private Const HTCAPTION = 2

Private Const LWA_ALPHA = &H2
Private Const LWA_COLORKEY = &H1
Private Const GWL_ExsTyle = -20
Private Const WS_EX_LAYERED = &H80000
Public Type Pos
    x As Single
    y As Single
    w As Single
    h As Single
    Ax As Single
    Ay As Single
    Aw As Single
    Ah As Single
End Type
Public Type Mycube
    x As Single
    y As Single
    w As Single
    h As Single
    Ax As Single
    Ay As Single
    Aw As Single
    Ah As Single
    Bcolors As Long
    Fcolors As Long
    Bacolors As Long
    Facolors As Long
    Aero As Single
    aAero As Single
    FontAero As Single
    aFontAero As Single
    BAero As Single
    aBAero As Single
    Infos As String
    Tag As String
    ID As Long
    Stabled As Boolean
    TempColor As Long
End Type
Public Type FrmBar
    BarRect As Mycube
    CloseBar As Mycube
    MinestBar As Mycube
End Type
Private Const HWND_TopMost& = -1
' 将窗口置于列表顶部，并位于任何最顶部窗口的前面
Private Const SWP_NoSize& = &H1
' 保持窗口大小
Private Const SWP_NoMove& = &H2
' 保持窗口位置
Public Sub MoveWindow(hwnd As Long)
    ReleaseCapture
    SendMessage hwnd, WM_NCLBUTTONDOWN, HTCAPTION, 0&
End Sub
Public Sub SetOntop(hwnd As Long, Optional DeOntop As Integer = 0)
If DeOntop = 1 Then
    SetWindowPos hwnd, 0, 0, 0, 0, 0, SWP_NoMove Or SWP_NoSize
Else
    SetWindowPos hwnd, HWND_TopMost, 0, 0, 0, 0, SWP_NoMove Or SWP_NoSize
End If
End Sub
Public Sub setFRM(Frm As Form, ByVal limpid As Long) ' 设置窗体透明度
On Error Resume Next
     Call SetWindowLong(Frm.hwnd, GWL_ExsTyle, GetWindowLong(Frm.hwnd, GWL_ExsTyle) Or WS_EX_LAYERED)
     Call SetLayeredWindowAttributes(Frm.hwnd, 0, limpid, LWA_ALPHA)     'limpid在0--255之间
End Sub
Public Sub SetFrmAlphaColor(FrmHwnd As Long, Optional Colors As Long = 0)
    Dim rtn As Long
    rtn = GetWindowLong(FrmHwnd, GWL_ExsTyle)
If Colors = -1 Then
    SetLayeredWindowAttributes FrmHwnd, 0, 255, LWA_ALPHA
Else
    SetWindowLong FrmHwnd, GWL_ExsTyle, rtn Or WS_EX_LAYERED
    SetLayeredWindowAttributes FrmHwnd, Colors, 0, LWA_COLORKEY
End If
End Sub
Public Function GetNewPos(Pos As Single, AimPos As Single, Optional Ways As Integer = 0, Optional speeds As Single = 0.2, Optional Scales As Single = 50) As Single
Dim TempNum As Single
If Ways = 0 Then
    'TempNum = (AimPos - Pos) / speeds
    'If Abs(TempNum) < Scales Then
    '    TempNum = Scales * Sgn(TempNum)
    'End If
    GetNewPos = Pos * (1 - speeds) + AimPos * speeds
Else
    If Abs(AimPos - Pos) < speeds Then
        GetNewPos = AimPos
    Else
        GetNewPos = Pos + speeds * Sgn((AimPos - Pos))
    End If
End If
End Function
Public Function IniFrmBar(Posi As Pos, FrmBar As FrmBar, Optional ShowClose As Boolean = False, Optional ShowMinest As Boolean = False) As Long
    With FrmBar
        With .BarRect
            .w = Posi.w
            .Aw = .w
            .Ah = Posi.h * 0.05
            .Bacolors = RGB(100, 100, 200)
            .aAero = 255
            .Ay = -.Ah
        End With
        If ShowClose Then
            With .CloseBar
                .w = Posi.h * 0.05
                .Aw = .w
                .Ah = .w
                .Ax = Posi.w * 0.95
                .Infos = "关闭"
                .Bacolors = RGB(150, 0, 0)
                .aAero = 255
                .Ay = -.Ah
            End With
        End If
        If ShowMinest Then
            With .MinestBar
                .w = Posi.w * 0.05
                .Ah = .w
                .Aw = .w
                .Ax = Posi.w * 0.9
                .Infos = "-"
                .Bacolors = RGB(200, 200, 200)
                .aAero = 255
                .Ay = -.Ah
            End With
        End If
    End With
End Function
Public Sub ShowFrmBar(FrmBar As FrmBar, Optional ShowOut As Boolean = True)
If FrmBarShowOut = ShowOut Then Exit Sub
If ShowOut = False Then
    With FrmBar
        .BarRect.Ay = -.BarRect.Ah
        .CloseBar.Ay = -.CloseBar.Ah
        .MinestBar.Ay = -.MinestBar.Ah
    End With
    FrmBarShowOut = False
Else
    With FrmBar
        .BarRect.Ay = 0
        .CloseBar.Ay = 0
        .MinestBar.Ay = 0
    End With
    FrmBarShowOut = True
End If
End Sub
Public Function OMoving(TargetObject As Object, Ax As Single, Ay As Single, Aw As Single, Ah As Single, Optional Scales As Single = 50, Optional speeds As Single = 0.2) As Boolean
Dim NowT As Integer, Aim As Single, TAim As String, Tac As String, FT(3) As Boolean, i As Long
With TargetObject
        If Abs(.Left - Ax) < Scales Then
            .Left = Ax
            FT(0) = True
        Else
            .Left = GetNewPos(.Left, Ax, , speeds, Scales)
        End If

        If Abs(.Top - Ay) < Scales Then
            .Top = Ay
            FT(1) = True
        Else
            .Top = GetNewPos(.Top, Ay, , speeds, Scales)
        End If

        If Abs(.Width - Aw) < Scales Then
            .Width = Aw
            FT(2) = True
        Else
            .Width = GetNewPos(.Width, Aw, , speeds, Scales)
        End If

        If Abs(.Height - Ah) < Scales Then
            .Height = Ah
            FT(3) = True
        Else
            .Height = GetNewPos(.Height, Ah, , speeds, Scales)
        End If

End With
For i = 0 To 3
    If FT(i) = False Then OMoving = False: Exit Function
Next
OMoving = True
End Function
Public Function CMoving(Frm As Form, TargetObject As Mycube, Ax As Single, Ay As Single, Aw As Single, Ah As Single, Optional Scales As Single = 50, Optional speeds As Single = 0.2, Optional FillFull As Boolean = True) As Boolean
Dim NowT As Integer, Aim As Single, TAim As String, Tac As String, FT(3) As Boolean, i As Long
With TargetObject
        If Abs(.x - Ax) < Scales Then
            .x = Ax
        Else
            .x = GetNewPos(.x, Ax, , speeds, Scales)
        End If
        If Abs(.y - Ay) < Scales Then
            .y = Ay
        Else
            .y = GetNewPos(.y, Ay, , speeds, Scales)
        End If
        If Abs(.w - Aw) < Scales Then
            .w = Aw
        Else
            .w = GetNewPos(.w, Aw, , speeds, Scales)
        End If
        If Abs(.h - Ah) < Scales Then
            .h = Ah
        Else
            .h = GetNewPos(.h, Ah, , speeds, Scales)
        End If
        If .x + .w < 0 Or .y + .h < 0 Or .x > Frm.Width Or .y > Frm.Height Then
            .Aero = 0
            Exit Function
        End If
        If .aAero = -1 Then
            .BAero = .aBAero * 0.1 + .BAero * 0.9
            .FontAero = .aFontAero * 0.1 + .FontAero * 0.9
            If .BAero > 1 Then
                If FillFull Then
                    FillRect GetPixSum(.x), GetPixSum(.y), GetPixSum(.w), GetPixSum(.h), CInt(.BAero), .Bcolors
                Else
                    DwRect GetPixSum(.x), GetPixSum(.y), GetPixSum(.w), GetPixSum(.h), 1, CInt(.BAero), .Bcolors
                End If
            End If
            If .Infos <> "" Then
                If .FontAero > 1 Then DwText .Infos, GetPixSum(.x + (.w - Frm.TextWidth(.Infos)) / 2), GetPixSum(.y + (Frm.TextHeight(.Infos) / 2 + .h) / 2), CInt(.FontAero), .Fcolors
            End If
        Else
            .Aero = .aAero * 0.1 + .Aero * 0.9
            If .Aero < 1 Then Exit Function
                If FillFull Then
                    FillRect GetPixSum(.x), GetPixSum(.y), GetPixSum(.w), GetPixSum(.h), CInt(.Aero), .Bcolors
                Else
                    DwRect GetPixSum(.x), GetPixSum(.y), GetPixSum(.w), GetPixSum(.h), 1, CInt(.Aero), .Bcolors
                End If
            If .Infos <> "" Then DwText .Infos, GetPixSum(.x + (.w - Frm.TextWidth(.Infos)) / 2), GetPixSum(.y + (Frm.TextHeight(.Infos) / 2 + .h) / 2), CInt(.Aero), .Fcolors
        End If
End With

End Function
Public Function CMovingOval(Frm As Form, TargetObject As Mycube, Ax As Single, Ay As Single, Aw As Single, Ah As Single, Optional Scales As Single = 50, Optional speeds As Single = 0.2) As Boolean
Dim NowT As Integer, Aim As Single, TAim As String, Tac As String, FT(3) As Boolean, i As Long
With TargetObject
        If Abs(.x - Ax) < Scales Then
            .x = Ax
            FT(0) = True
        Else
            .x = GetNewPos(.x, Ax, , speeds, Scales)
        End If
        If Abs(.y - Ay) < Scales Then
            .y = Ay
            FT(1) = True
        Else
            .y = GetNewPos(.y, Ay, , speeds, Scales)
        End If
        If Abs(.w - Aw) < Scales Then
            .w = Aw
            FT(2) = True
        Else
            .w = GetNewPos(.w, Aw, , speeds, Scales)
        End If
        If Abs(.h - Ah) < Scales Then
            .h = Ah
            FT(3) = True
        Else
            .h = GetNewPos(.h, Ah, , speeds, Scales)
        End If
        If .x + .w < 0 Or .y + .h < 0 Or .x > Frm.Width Or .y > Frm.Height Then
            .Aero = 0
            Exit Function
        End If
        If .aAero = -1 Then
            .BAero = .aBAero * 0.1 + .BAero * 0.9
            .FontAero = .aFontAero * 0.1 + .FontAero * 0.9
            If .BAero > 1 Then FillOval GetPixSum(.x), GetPixSum(.y), GetPixSum(.w), GetPixSum(.h), CInt(.BAero), .Bcolors
            If .Infos <> "" Then
                If .FontAero > 1 Then DwText .Infos, GetPixSum(.x + (.w - Frm.TextWidth(.Infos)) / 2), GetPixSum(.y + (Frm.TextHeight(.Infos) / 2 + .h) / 2), CInt(.FontAero), .Fcolors
            End If
        Else
            .Aero = .aAero * 0.1 + .Aero * 0.9
            If .Aero < 1 Then Exit Function
            FillOval GetPixSum(.x), GetPixSum(.y), GetPixSum(.w), GetPixSum(.h), CInt(.Aero), .Bcolors
            If .Infos <> "" Then DwText .Infos, GetPixSum(.x + (.w - Frm.TextWidth(.Infos)) / 2), GetPixSum(.y + (Frm.TextHeight(.Infos) / 2 + .h) / 2), CInt(.Aero), .Fcolors
        End If
End With
For i = 0 To 3
    If FT(i) = False Then CMovingOval = False: Exit Function
Next
CMovingOval = True
End Function
Public Function CcolorEdit(Frm As Form, Tar As Mycube, Optional speeds As Single = 0.2) As Boolean
With Tar
    Dim c(3) As Integer, D(3) As Integer
    c(1) = .Bcolors Mod 256
    c(2) = (.Bcolors Mod 65536) / 255
    c(3) = .Bcolors / 65536
    D(1) = .Bacolors Mod 256
    D(2) = (.Bacolors Mod 65536) / 255
    D(3) = .Bacolors / 65536
    .Bcolors = RGB(c(1) * (1 - speeds) + D(1) * speeds, c(2) * (1 - speeds) + D(2) * speeds, c(3) * (1 - speeds) + D(3) * speeds)
    
    c(1) = .Fcolors Mod 256
    c(2) = (.Fcolors Mod 65536) / 255
    c(3) = .Fcolors / 65536
    D(1) = .Facolors Mod 256
    D(2) = (.Facolors Mod 65536) / 255
    D(3) = .Facolors / 65536
    .Fcolors = RGB(c(1) * (1 - speeds) + D(1) * speeds, c(2) * (1 - speeds) + D(2) * speeds, c(3) * (1 - speeds) + D(3) * speeds)
    CcolorEdit = CMoving(Frm, Tar, .Ax, .Ay, .Aw, .Ah, , speeds)
    'Debug.Print .Ax & "," & .Ay & "   " & .Aw & "," & .Ah
End With
End Function
Public Function CcolorEditEx(Frm As Form, Tar As Mycube, Optional speeds As Single = 0.2, Optional SecViewX As Single = 0, Optional SecViewY As Single = 0, Optional FillFull As Boolean = True) As Boolean
With Tar
    Dim c(3) As Integer, D(3) As Integer
    c(1) = .Bcolors Mod 256
    c(2) = (.Bcolors Mod 65536) / 255
    c(3) = .Bcolors / 65536
    D(1) = .Bacolors Mod 256
    D(2) = (.Bacolors Mod 65536) / 255
    D(3) = .Bacolors / 65536
    .Bcolors = RGB(c(1) * (1 - speeds) + D(1) * speeds, c(2) * (1 - speeds) + D(2) * speeds, c(3) * (1 - speeds) + D(3) * speeds)
    
    c(1) = .Fcolors Mod 256
    c(2) = (.Fcolors Mod 65536) / 255
    c(3) = .Fcolors / 65536
    D(1) = .Facolors Mod 256
    D(2) = (.Facolors Mod 65536) / 255
    D(3) = .Facolors / 65536
    .Fcolors = RGB(c(1) * (1 - speeds) + D(1) * speeds, c(2) * (1 - speeds) + D(2) * speeds, c(3) * (1 - speeds) + D(3) * speeds)
    CcolorEditEx = CMoving(Frm, Tar, .Ax + SecViewX, .Ay + SecViewY, .Aw, .Ah, , speeds, FillFull)
    'Debug.Print .Ax & "," & .Ay & "   " & .Aw & "," & .Ah
End With
End Function
Public Function CcolorEditOvalEx(Frm As Form, Tar As Mycube, Optional speeds As Single = 0.2, Optional SecViewX As Single = 0, Optional SecViewY As Single = 0) As Boolean
With Tar
    Dim c(3) As Integer, D(3) As Integer
    c(1) = .Bcolors Mod 256
    c(2) = (.Bcolors Mod 65536) / 255
    c(3) = .Bcolors / 65536
    D(1) = .Bacolors Mod 256
    D(2) = (.Bacolors Mod 65536) / 255
    D(3) = .Bacolors / 65536
    .Bcolors = RGB(c(1) * (1 - speeds) + D(1) * speeds, c(2) * (1 - speeds) + D(2) * speeds, c(3) * (1 - speeds) + D(3) * speeds)
    
    c(1) = .Fcolors Mod 256
    c(2) = (.Fcolors Mod 65536) / 255
    c(3) = .Fcolors / 65536
    D(1) = .Facolors Mod 256
    D(2) = (.Facolors Mod 65536) / 255
    D(3) = .Facolors / 65536
    .Fcolors = RGB(c(1) * (1 - speeds) + D(1) * speeds, c(2) * (1 - speeds) + D(2) * speeds, c(3) * (1 - speeds) + D(3) * speeds)
    CcolorEditOvalEx = CMovingOval(Frm, Tar, .Ax + SecViewX, .Ay + SecViewY, .Aw, .Ah, , speeds)
    'Debug.Print .Ax & "," & .Ay & "   " & .Aw & "," & .Ah
End With
End Function
Public Sub ColorEditEx(Color As Long, aColor As Long, Optional speeds As Single = 0.2)
    Dim c(3) As Integer, D(3) As Integer
    c(1) = Color Mod 256
    c(2) = (Color Mod 65536) / 255
    c(3) = Color / 65536
    D(1) = aColor Mod 256
    D(2) = (aColor Mod 65536) / 255
    D(3) = aColor / 65536
    Color = RGB(c(1) * (1 - speeds) + D(1) * speeds, c(2) * (1 - speeds) + D(2) * speeds, c(3) * (1 - speeds) + D(3) * speeds)
End Sub
