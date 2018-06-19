Attribute VB_Name = "RegProApi"
Private Declare Function RegOpenKey Lib "advapi32.dll" Alias "RegOpenKeyA" _
(ByVal hkey As Long, ByVal lpSubKey As String, phkResult As Long) As Long

Public Declare Function RegCreateKey Lib "advapi32.dll" Alias "RegCreateKeyA" _
(ByVal hkey As Long, ByVal lpSubKey As String, phkResult As Long) As Long

Public Declare Function RegSetValueEx Lib "advapi32.dll" Alias "RegSetValueExA" _
(ByVal hkey As Long, ByVal lpValueName As String, ByVal Reserved As Long, _
ByVal dwType As Long, lpData As Any, ByVal cbData As Long) As Long

Public Declare Function RegQueryValueEx Lib "advapi32.dll" Alias "RegQueryValueExA" _
(ByVal hkey As Long, ByVal lpValueName As String, ByVal lpReserved As Long, _
lpType As Long, lpData As Any, lpcbData As Long) As Long
Public Declare Function RegCloseKey Lib "advapi32.dll" (ByVal hkey As Long) As Long

Public Declare Function RegDeleteValue Lib "advapi32.dll" Alias "RegDeleteValueA" _
(ByVal hkey As Long, ByVal lpValueName As String) As Long
Private Declare Function SHDeleteKey Lib "shlwapi.dll" Alias "SHDeleteKeyA" (ByVal hkey As Long, ByVal pszSubKey As String) As Long

Private Const HKEY_CURRENT_USER = &H80000001
Private Const REG_DWORD = 4    ' 32-bit number
Private Const REG_SZ = 1

Public Const MainPath As String = "Software\Serfend\TimeMasterForyy\"
Public Function SetInfo(ByVal Info As String, ByVal Path As String, ByVal KeyName As String) As Long
Dim mPath As String, hkey As Long
mPath = MainPath & Path
RegCreateKey HKEY_CURRENT_USER, mPath, hkey
Info = Info & Chr(0)
RegSetValueEx hkey, KeyName, 0, REG_SZ, ByVal Info, LenB(Info) * 2
RegCloseKey hkey
End Function

Public Function ReadKey(ByVal Names As String, hkey As Long) As String
Dim TempInfo As String, lenData As Long
lenData = 10 ^ 5
TempInfo = String(1024, vbNullChar)
RegQueryValueEx hkey, Names, 0, REG_SZ, ByVal TempInfo, lenData
If lenData = 0 Then
    ReadKey = ""
Else
    'RegQueryValueEx hKey, Names, 0, REG_SZ, ByVal TempInfo, lenData
    ReadKey = Left(TempInfo, lenData / 4 - 1)
End If
End Function
Public Sub CloseKey(ByVal hkey As Long)
RegCloseKey hkey
End Sub

Public Function GetInfo(ByVal Path As String, ByVal KeyName As String, Optional ByVal Default As String, Optional Lengths As Long = 1024) As String
Dim mPath As String, hkey As Long, lenData As Long
Dim TempInfo As String
TempInfo = String(Lengths, vbNullChar)
mPath = MainPath & Path
RegCreateKey HKEY_CURRENT_USER, mPath, hkey
lenData = Lengths * 4
RegQueryValueEx hkey, KeyName, 0, REG_SZ, ByVal TempInfo, lenData

If lenData = 0 Then
    If GetInfo = "" And Default <> "" Then
        GetInfo = Default
        SetInfo GetInfo, Path, KeyName
    End If
Else
    'RegQueryValueEx hKey, KeyName, 0, REG_SZ, ByVal TempInfo, lenData
    
    'Dim TempNum As Long
    'TempNum = InStr(1, TempInfo, Chr(0))
    'GetInfo = Left(TempInfo, TempNum - 1)
    If lenData = Lengths * 4 Then
        GetInfo = Default
        RegSetValueEx hkey, KeyName, 0, REG_SZ, ByVal GetInfo, LenB(GetInfo) * 2
    Else
        GetInfo = Left(TempInfo, InStr(1, TempInfo, Chr(0)) - 1) 'Left(TempInfo, lenData / 4 - 1) '
    End If
End If
RegCloseKey hkey


End Function
Public Function DelInfo(ByVal Path As String, KeyName As String) As Long
Dim hkey As Long
RegOpenKey HKEY_CURRENT_USER, MainPath & Path, hkey
RegDeleteValue hkey, KeyName
RegCloseKey hkey
End Function
Public Function DelSub(ByVal Path As String, KeyName As String) As Long
RegOpenKey HKEY_CURRENT_USER, MainPath & Path, hkey
SHDeleteKey hkey, KeyName
RegCloseKey hkey
End Function
Public Function GetXmlHttp(ByVal GetUrl As String) As Byte()        '取网页源码函数
On Error GoTo errHD
    Dim XmlHttp As Object
    Set XmlHttp = CreateObject("MSXML2.XMLHTTP")         '动态创建XMLHTTP对象，优点是不需要在工程里引用。缺点是写代码的时候不会有代码提示
    XmlHttp.Open "GET", GetUrl, True            'GET
    XmlHttp.send
    Do Until XmlHttp.readyState = 4             '等待数据返回、准备完毕
        DoEvents
    Loop
    GetXmlHttp = XmlHttp.responseText
    Set XmlHttp = Nothing
    Exit Function
errHD:
End Function
Public Sub ShellOut(ExeNames As String)
On Error GoTo Err:
Shell App.Path & "\" & ExeNames & ".exe", vbNormalFocus
Exit Sub
Err:
SetErr "[" & Hex(Err.Number) & "]$" & Err.description & "$$ShellOut:" & ExeNames

End Sub

