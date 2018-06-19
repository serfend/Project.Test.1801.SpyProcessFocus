Attribute VB_Name = "Chri"

Public mMyFont As String, mInfoFont As String
Public Function GetUserName() As String
Dim Tem As String
GetUserName = GetInfo("main\User", "UserName", "Undefined User")
End Function
Public Function InfoFont() As String
On Error GoTo Err
If mInfoFont = "" Then
    mInfoFont = GetInfo("main\User", "InfoFont", "Î¢ÈíÑÅºÚ")
    FrmMain.FontName = mInfoFont
End If
    InfoFont = mInfoFont
Exit Function
Err:
    mInfoFont = "Î¢ÈíÑÅºÚ"
    InfoFont = mInfoFont
    
End Function
Public Function MyFont() As String
On Error GoTo Err
If mMyFont = "" Then
    mMyFont = GetInfo("main\User", "MainFont", "Î¢ÈíÑÅºÚ")
    FrmMain.FontName = mMyFont
End If
    MyFont = mMyFont
Exit Function
Err:
    mMyFont = "Î¢ÈíÑÅºÚ"
    MyFont = mMyFont
    
End Function
