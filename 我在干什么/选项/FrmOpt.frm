VERSION 5.00
Begin VB.Form FrmMain 
   Appearance      =   0  'Flat
   AutoRedraw      =   -1  'True
   BackColor       =   &H00000000&
   BorderStyle     =   0  'None
   ClientHeight    =   2865
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   5250
   BeginProperty Font 
      Name            =   "ËÎÌå"
      Size            =   15.75
      Charset         =   134
      Weight          =   400
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2865
   ScaleWidth      =   5250
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  '´°¿ÚÈ±Ê¡
   Begin VB.Timer MainMover 
      Index           =   5
      Interval        =   66
      Left            =   0
      Top             =   0
   End
   Begin VB.Timer MainMover 
      Index           =   4
      Interval        =   66
      Left            =   0
      Top             =   0
   End
   Begin VB.Timer MainMover 
      Index           =   3
      Interval        =   66
      Left            =   0
      Top             =   0
   End
   Begin VB.Timer MainMover 
      Index           =   2
      Interval        =   66
      Left            =   0
      Top             =   0
   End
   Begin VB.Timer MainMover 
      Index           =   1
      Interval        =   66
      Left            =   0
      Top             =   0
   End
   Begin VB.Timer MainMover 
      Index           =   0
      Interval        =   66
      Left            =   0
      Top             =   0
   End
End
Attribute VB_Name = "FrmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Dim MyId As Long, FrmTitle As String, FrmInfos As String, FrmOptNum As Long, FrmOptInfo() As String
Dim FrmM As Boolean, FrmPos As Pos, PrFocus As Long, PrKeyClose As Boolean
Dim OptCmd() As Mycube, OptDownColor() As Long, OptUpColor() As Long
Dim Titles As Mycube, ImagesPath As String, ImagesIndex As Long, ImgViewX As Single, _
    ImgViewY As Single, ImgHeight As Single, ImgMouseDown As Boolean, ImgPrX As Single, ImgPrY As Single
Dim FrmMouseDown As Boolean, MousePrY As Single, SViewY As Single, ContentY As Single
Dim Inied As Integer, IniedOpt As Boolean, FrmInfosRul() As String, FrmInfoNum As Long, ClosingMe As Integer
Private Const MyFont = "Î¢ÈíÑÅºÚ"

Dim TimerCount As Long
Private Sub Form_DblClick()
        DelInfo "Main\OptInfo", "FrmReturn"
        ClosingMe = 1
End Sub
Private Sub Form_KeyDown(KeyCode As Integer, Shift As Integer)
If KeyCode = 27 Then
    PrKeyClose = True
    Titles.Bacolors = vbRed
ElseIf KeyCode = 13 Then
    Form_MouseDown 0, 0, 1, FrmPos.Ah - 1
Else
    Form_MouseUp 0, 0, -1, FrmPos.Ah + 1
End If
End Sub
Private Sub Form_KeyUp(KeyCode As Integer, Shift As Integer)
If KeyCode = 27 And PrKeyClose Then Form_DblClick
PrKeyClose = False
If KeyCode = 13 Then
    Form_MouseUp 0, 0, 1, FrmPos.Ah - 1
End If
Titles.Bacolors = vbWhite
End Sub

Private Sub Form_Load()
Me.FontName = MyFont

MyId = Val(GetInfo("Main\OptInfo", "NowOptId"))
FrmTitle = GetInfo("Main\OptInfo\" & MyId, "FrmTitle")
FrmInfos = SciInfo(GetInfo("Main\OptInfo\" & MyId, "ContentInfo"))
FrmOptNum = Val(GetInfo("Main\OptInfo\" & MyId, "FrmOptNum"))

With FrmPos
    .x = Screen.Width / 2
    .y = Screen.Height * 0.38
    .h = Screen.Height * 0.24
    .Ax = Screen.Width * 0.35
    .Aw = Screen.Width * 0.3
    .Ay = .y
    .Ah = .h
    Move .x, .y, .w, .h
End With
ImgViewY = 0.3 * FrmPos.Ah
Dim i As Long
ReDim OptCmd(FrmOptNum): ReDim OptDownColor(FrmOptNum): ReDim OptUpColor(FrmOptNum): ReDim FrmOptInfo(FrmOptNum)
For i = 1 To FrmOptNum
    With OptCmd(i)
        .Infos = GetInfo("Main\optinfo\" & MyId & "\" & i, "Infos")
        FrmOptInfo(i) = GetInfo("Main\optinfo\" & MyId & "\" & i, "Cmd")
        OptDownColor(i) = Val(GetInfo("Main\optinfo\" & MyId & "\" & i, "DColor"))
        OptUpColor(i) = Val(GetInfo("Main\optinfo\" & MyId & "\" & i, "UColor"))
        If OptDownColor(i) = 0 Then OptDownColor(i) = vbWhite
        If OptUpColor(i) = 0 Then OptUpColor(i) = vbWhite
        .Bacolors = OptUpColor(i)
        .aAero = 255
    End With
Next

SetOntop Me.hwnd
setFRM Me, 225
End Sub
Private Function SciInfo(Infos As String) As String
Dim i As Long, j As Long, Temp As String
Dim Atemp
Atemp = Split(Infos, "$")
FrmInfoNum = UBound(Atemp) + 1
ReDim FrmInfosRul(FrmInfoNum)
For i = 1 To Len(Infos)
    Temp = Mid(Infos, i, 1)
    Select Case Temp
        Case "$": j = j + 1
        Case Else: FrmInfosRul(j) = FrmInfosRul(j) & Temp
    End Select
Next
End Function
Private Sub Form_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
Dim i As Long
If y < Me.Height * 0.2 Then
    If x > Me.Width * 0.95 Then
        If FrmOptNum = 0 Then
            DelInfo "Main\OptInfo", "FrmReturn"
            ClosingMe = 1
        End If
    Else
        MoveWindow Me.hwnd
    End If
Else
    If x > ImgViewX And x < ImgViewX + FrmPos.Aw * 0.3 And y > ImgViewY And y < ImgViewY + ImgHeight Then
        ImgMouseDown = True
        ImgPrX = x
        ImgPrY = y
    ElseIf y < Me.Height * 0.8 Then
        FrmMouseDown = True
        MousePrY = y
    End If
End If
i = FindFocus(x, y)
If i > 0 Then
    With OptCmd(i)
        .Bacolors = OptDownColor(i)
        PrFocus = i
    End With
Else
    
End If
End Sub

Private Function FindFocus(x As Single, y As Single) As Long
Dim i As Long
For i = 1 To FrmOptNum
    With OptCmd(i)
        If .x < x And .x + .w > x And .y < y And .y + .h > y Then FindFocus = i: Exit Function
    End With
Next
End Function
Private Sub ReFreshOpt()
    Dim i As Long
    If IniedOpt Then
        For i = 1 To FrmOptNum
            With OptCmd(i)
                .Ah = Me.Height * 0.2
                .Aw = Me.Width / FrmOptNum
                .Ax = Me.Width * (i - 1) / FrmOptNum
                .Ay = Me.Height * 0.8
                .w = .Aw
            End With
        Next
        With Titles
            .Ah = Me.Height * 0.2
            .Aw = Me.Width
            .w = .Aw
            .aAero = 255
        End With
        Exit Sub
    End If
    IniedOpt = True
    For i = 1 To FrmOptNum
        With OptCmd(i)
            .Ah = Me.Height * 0.2
            .Aw = Me.Width / FrmOptNum
            .Ax = Me.Width * (i - 1) / FrmOptNum
            .Ay = Me.Height * 0.8
            .w = .Aw
            .x = .Ax
            .y = Me.Height
        End With
    Next
    With Titles
        .Ah = Me.Height * 0.2
        .Aw = Me.Width
        .w = .Aw
        .Infos = FrmTitle
        .Bacolors = vbWhite
        .aAero = 255
    End With
End Sub

Private Sub Form_MouseMove(Button As Integer, Shift As Integer, x As Single, y As Single)
If FrmMouseDown Then
    SViewY = SViewY + y - MousePrY
    MousePrY = y
ElseIf ImgMouseDown Then
    ImgViewX = ImgViewX + x - ImgPrX
    ImgViewY = ImgViewY + y - ImgPrY
    ImgPrX = x: ImgPrY = y
End If
End Sub

Private Sub Form_MouseUp(Button As Integer, Shift As Integer, x As Single, y As Single)
If FrmMouseDown Then
    FrmMouseDown = False
    If SViewY < ContentY Then SViewY = ContentY
    If SViewY > 0 Then SViewY = 0
ElseIf ImgMouseDown Then
    ImgMouseDown = False
End If
    If FindFocus(x, y) = PrFocus And PrFocus <> 0 Then
        SetInfo CStr(FrmOptInfo(PrFocus)), "Main\OptInfo", "FrmReturn"
        ClosingMe = 1
        Exit Sub
    Else
        With OptCmd(PrFocus)
            .Bacolors = OptUpColor(PrFocus)
            .Ah = Me.Height * 0.2
            PrFocus = 0
        End With
    End If
End Sub

Private Sub Form_Unload(Cancel As Integer)
TerminateGDIPlus
End Sub
Private Sub GetCmdInfo()
    Dim cmdInfo As String
    cmdInfo = GetInfo("Main\OptInfo", "nowCmd")
    If cmdInfo <> "" Then
        SetInfo "", "Main\OptInfo", "nowCmd"
        Select Case cmdInfo
        
        End Select
    End If
End Sub
Private Sub MainMover_Timer(Index As Integer)
Static Moving As Boolean
Static checkCmdRefreshTime As Long
checkCmdRefreshTime = checkCmdRefreshTime + 1
If checkCmdRefreshTime = 10 Then
    checkCmdRefreshTime = 0
    GetCmdInfo
End If
While Moving
    DoEvents
Wend
Moving = True

With FrmPos
    If FrmM = False Then FrmM = OMoving(Me, .Ax, .Ay, .Aw, .Ah, 10)
End With
If ClosingMe > 0 Then
    If ClosingMe = 2 Then
        If FrmM = True Then Unload Me: Exit Sub
        Moving = False
        Exit Sub
    End If
    FrmPos.Ax = Me.Left + FrmPos.Aw / 2
    FrmPos.Ay = Me.Top
    FrmPos.Aw = 0
    
    FrmM = False
    ClosingMe = 2
End If
Dim i As Long
FillRect 0, 0, GetPixSum(FrmPos.Aw), GetPixSum(FrmPos.Ah), 255, Me.BackColor

If Inied = 2 Then
    With Titles
        CcolorEditEx Me, Titles, , , SViewY
    End With
    Dim TempHeight As Single
    TempHeight = TextHeight(FrmInfos)
    Dim MeY As Single
    MeY = (FrmPos.Ah - TextHeight(FrmInfos)) * 0.3
    Dim NowFColor As Long, NowFAero As Integer
    NowFAero = 255: NowFColor = RGB(200, 200, 200)
    For i = 0 To FrmInfoNum - 1
        MeY = MeY + TempHeight
        If FrmInfosRul(i) <> "" Then
            DwMutlText FrmInfosRul(i), NowFColor, NowFAero, MeY + SViewY
        End If
    Next
    ContentY = MeY - FrmPos.Ah
End If

For i = 1 To FrmOptNum
    With OptCmd(i)
        CcolorEditEx Me, OptCmd(i)
    End With
Next

If FrmM = True And Inied = 0 Then
    With FrmPos
        .Ay = Screen.Height * 0.3
        .Ah = Screen.Height * 0.4
    End With
    FrmM = False
    Inied = 1
End If
If Inied = 1 And FrmM = True Then
    ReFreshOpt
    InitGDIPlus
    ImagesPath = App.Path & "\images\" & GetInfo("Main\OptInfo\" & MyId, "Images") & ".png"
    ImagesIndex = LoadImg(ImagesPath)
    Inied = 2
End If

If ImagesIndex > 0 Then ImgHeight = 15 * DwImg(ImagesIndex, GetPixSum(ImgViewX), GetPixSum(ImgViewY), GetPixSum(FrmPos.Aw * 0.3))
TimerCount = TimerCount + 1
If TimerCount > 20 Then
    TimerCount = 0
    'If GetInfo("Main\Setting", "Closed") = "1" Then ClosingMe = 1
End If
Moving = False
End Sub

Private Sub DwMutlText(IniInfo As String, nFColor As Long, nFAero As Integer, ByVal PosiY As Single)
Dim i As Long, j As Long, Temp As Long, TempOutPut As String, PosiX As Single
Dim TempInfo() As String, SumOutPutInfo As String
TempInfo = Split(IniInfo, "#")
For i = 0 To UBound(TempInfo) Step 2
    SumOutPutInfo = SumOutPutInfo & TempInfo(i)
Next
PosiX = (FrmPos.Aw - TextWidth(SumOutPutInfo)) / 2
For i = 0 To UBound(TempInfo) Step 2
    TempOutPut = TempInfo(i)
    If TempOutPut <> "" Then
        DwText TempOutPut, GetPixSum(PosiX), GetPixSum(PosiY), 255, nFColor
        PosiX = PosiX + TextWidth(TempOutPut)
    End If
    If i + 1 < UBound(TempInfo) Then
        nFColor = Val(TempInfo(i + 1))
    End If
Next
End Sub
