Attribute VB_Name = "GDIPlusAPI"

Option Explicit
 
'vIstaswx GDI+ ����ģ��
'vIstaswx GDI+ Declare Module
 
'vIstaswx ������չ
'Extended by vIstaswx
 
'===========================================
'����޸ģ�2011/2/7
'Latest edit: 2011/2/7
'
'2011-2-7
'1.����GdipSetLinePresetBlend��4���������������Ĵ���
'
'2010-6-5:
'1.����ͼƬ�����Ż�
'2.InitGDIPlus(To) ����ʱ��ѡ��ʾ����Ի����˳�����
'  ֧���Զ������Ի������ݣ����ӷ���ֵ�������Ѿ���ʼ�����ж�
'3.TerminateGDIPlus(From) �����Ѿ��رյ��ж�
'4.ɾ��RtlMoveMemory(CopyMemory)�������޸�CLSIDFromString����ΪPrivate��
'===========================================
 
'��л��Ҷ����ģ����ڵ�bug��ָ��
 
 
'ת���뱣����Ȩ��лл
 
'�����������վ���鿴 vIstaswx VB6 GDI+ϵ�н̳� ����������
'Please via the following website to read the whole series
'of vIstaswx VB6 GDI+ Tutorials
'download by http://www.newxing.com
'http://vIstaswx.blogbus.com
 
'QQ     : 490241327
'BaiduHi: swx1995
 
'===================================================================================
'  ��������
'===================================================================================
 
'=================================
'== Structures                  ==
'=================================
 
'=================================
'Point Structure
Public Type POINTL
   x As Long
   y As Long
End Type
 
Public Type POINTF
   x As Single
   y As Single
End Type
 
'=================================
'Rectange Structure
Public Type RECTL
   Left As Long
   Top As Long
   Right As Long
   Bottom As Long
End Type
 
Public Type RECTF
   Left As Single
   Top As Single
   Right As Single
   Bottom As Single
End Type
 
'=================================
'Size Structure
Public Type SIZEL
   cx As Long
   cy As Long
End Type
 
Public Type SIZEF
   cx As Single
   cy As Single
End Type
 
'=================================
'Bitmap Structure
Public Type RGBQUAD
   rgbBlue As Byte
   rgbGreen As Byte
   rgbRed As Byte
   rgbReserved As Byte
End Type
 
Public Type BITMAPINFOHEADER
   biSize As Long
   biWidth As Long
   biHeight As Long
   biPlanes As Integer
   biBitCount As Integer
   biCompression As Long
   biSizeImage As Long
   biXPelsPerMeter As Long
   biYPelsPerMeter As Long
   biClrUsed As Long
   biClrImportant As Long
End Type
 
Public Type BITMAPINFO
   bmiHeader As BITMAPINFOHEADER
   bmiColors As RGBQUAD
End Type
 
 
Public Type BitmapData
   Width As Long
   Height As Long
   stride As Long
   PixelFormat As Long
   scan0 As Long
   Reserved As Long
End Type
 
'=================================
'Color Structure
Public Type COLORBYTES
   BlueByte As Byte
   GreenByte As Byte
   RedByte As Byte
   AlphaByte As Byte
End Type
 
Public Type COLORLONG
   longval As Long
End Type
 
Public Type ColorMap
   oldColor As Long
   newColor As Long
End Type
 
Public Type ColorMatrix
   m(0 To 4, 0 To 4) As Single
End Type
 
'=================================
'Path
Public Type PathData
   Count As Long
   POINTS As Long    ' Pointer to POINTF array
   Types As Long     ' Pointer to BYTE array
End Type
 
'=================================
'Encoder
Public Type Clsid
   data1 As Long
   data2 As Integer
   data3 As Integer
   data4(0 To 7) As Byte
End Type
 
Public Type EncoderParameter
   Guid As Clsid
   NumberOfValues As Long
   Type As EncoderParameterValueType
   Value As Long
End Type
 
Public Type EncoderParameters
   Count As Long
   Parameter As EncoderParameter
End Type
 
'=================================
'== Enums                       ==
'=================================
 
'=================================
'Pixel
Public Enum GpPixelFormat
    PixelFormat1bppIndexed = &H30101
    PixelFormat4bppIndexed = &H30402
    PixelFormat8bppIndexed = &H30803
    PixelFormat16bppGreyScale = &H101004
    PixelFormat16bppRGB555 = &H21005
    PixelFormat16bppRGB565 = &H21006
    PixelFormat16bppARGB1555 = &H61007
    PixelFormat24bppRGB = &H21808
    PixelFormat32bppRGB = &H22009
    PixelFormat32bppARGB = &H26200A
    PixelFormat32bppPARGB = &HE200B
    PixelFormat48bppRGB = &H10300C
    PixelFormat64bppARGB = &H34400D
    PixelFormat64bppPARGB = &H1C400E
End Enum
 
'=================================
'Unit
Public Enum GpUnit
    UnitWorld
    UnitDisplay
    UnitPixel
    UnitPoint
    UnitInch
    UnitDocument
    UnitMillimeter
End Enum
 
'=================================
'Path
Public Enum PathPointType
    PathPointTypeStart = 0
    PathPointTypeLine = 1
    PathPointTypeBezier = 3
    PathPointTypePathTypeMask = &H7
    PathPointTypePathDashMode = &H10
    PathPointTypePathMarker = &H20
    PathPointTypeCloseSubpath = &H80
    PathPointTypeBezier3 = 3
End Enum
 
'=================================
'Font / String
Public Enum GenericFontFamily
   GenericFontFamilySerif
   GenericFontFamilySansSerif
   GenericFontFamilyMonospace
End Enum
 
Public Enum FontStyle
   FontStyleRegular = 0
   FontStyleBold = 1
   FontStyleItalic = 2
   FontStyleBoldItalic = 3
   FontStyleUnderline = 4
   FontStyleStrikeout = 8
End Enum
 
Public Enum StringAlignment
   StringAlignmentNear = 0
   StringAlignmentCenter = 1
   StringAlignmentFar = 2
End Enum
 
'=================================
'Fill / Wrap
Public Enum FillMode
   FillModeAlternate
   FillModeWinding
End Enum
 
Public Enum WrapMode
   WrapModeTile
   WrapModeTileFlipX
   WrapModeTileFlipY
   WrapModeTileFlipXY
   WrapModeClamp
End Enum
 
Public Enum LinearGradientMode
   LinearGradientModeHorizontal
   LinearGradientModeVertical
   LinearGradientModeForwardDiagonal
   LinearGradientModeBackwardDiagonal
End Enum
 
'=================================
'Quality
Public Enum QualityMode
   QualityModeInvalid = -1
   QualityModeDefault = 0
   QualityModeLow = 1
   QualityModeHigh = 2
End Enum
 
Public Enum CompositingMode
   CompositingModeSourceOver
   CompositingModeSourceCopy
End Enum
 
Public Enum CompositingQuality
   CompositingQualityInvalid = QualityModeInvalid
   CompositingQualityDefault = QualityModeDefault
   CompositingQualityHighSpeed = QualityModeLow
   CompositingQualityHighQuality = QualityModeHigh
   CompositingQualityGammaCorrected
   CompositingQualityAssumeLinear
End Enum
 
Public Enum SmoothingMode
   SmoothingModeInvalid = QualityModeInvalid
   SmoothingModeDefault = QualityModeDefault
   SmoothingModeHighSpeed = QualityModeLow
   SmoothingModeHighQuality = QualityModeHigh
   SmoothingModeNone
   SmoothingModeAntiAlias
End Enum
 
Public Enum InterpolationMode
   InterpolationModeInvalid = QualityModeInvalid
   InterpolationModeDefault = QualityModeDefault
   InterpolationModeLowQuality = QualityModeLow
   InterpolationModeHighQuality = QualityModeHigh
   InterpolationModeBilinear
   InterpolationModeBicubic
   InterpolationModeNearestNeighbor
   InterpolationModeHighQualityBilinear
   InterpolationModeHighQualityBicubic
End Enum
 
Public Enum PixelOffsetMode
   PixelOffsetModeInvalid = QualityModeInvalid
   PixelOffsetModeDefault = QualityModeDefault
   PixelOffsetModeHighSpeed = QualityModeLow
   PixelOffsetModeHighQuality = QualityModeHigh
   PixelOffsetModeNone    ' No pixel offset
   PixelOffsetModeHalf     ' Offset by -0.5 -0.5 for fast anti-alias perf
End Enum
 
Public Enum TextRenderingHint
   TextRenderingHintSystemDefault = 0            ' Glyph with system default rendering hint
   TextRenderingHintSingleBitPerPixelGridFit     ' Glyph bitmap with hinting
   TextRenderingHintSingleBitPerPixel            ' Glyph bitmap without hinting
   TextRenderingHintAntiAliasGridFit             ' Glyph anti-alias bitmap with hinting
   TextRenderingHintAntiAlias                    ' Glyph anti-alias bitmap without hinting
   TextRenderingHintClearTypeGridFit              ' Glyph CT bitmap with hinting
End Enum
 
'=================================
'Color Matrix
Public Enum MatrixOrder
   MatrixOrderPrepend = 0
   MatrixOrderAppend = 1
End Enum
 
Public Enum ColorAdjustType
   ColorAdjustTypeDefault
   ColorAdjustTypeBitmap
   ColorAdjustTypeBrush
   ColorAdjustTypePen
   ColorAdjustTypeText
   ColorAdjustTypeCount
   ColorAdjustTypeAny
End Enum
 
Public Enum ColorMatrixFlags
   ColorMatrixFlagsDefault = 0
   ColorMatrixFlagsSkipGrays = 1
   ColorMatrixFlagsAltGray = 2
End Enum
 
Public Enum WarpMode
   WarpModePerspective     ' 0
   WarpModeBilinear        ' 1
End Enum
 
Public Enum CombineMode
   CombineModeReplace      ' 0
   CombineModeIntersect    ' 1
   CombineModeUnion        ' 2
   CombineModeXor          ' 3
   CombineModeExclude      ' 4
   CombineModeComplement   ' 5 (Exclude From)
End Enum
 
Public Enum ImageLockMode
   ImageLockModeRead = &H1
   ImageLockModeWrite = &H2
   ImageLockModeUserInputBuf = &H4
End Enum
 
Public Declare Function GdipGetDC Lib "gdiplus" (ByVal graphics As Long, hDc As Long) As GpStatus
Public Declare Function GdipReleaseDC Lib "gdiplus" (ByVal graphics As Long, ByVal hDc As Long) As GpStatus
 
'==================================================
 
Public Declare Function GdipCreateFromHDC Lib "gdiplus" (ByVal hDc As Long, graphics As Long) As GpStatus
Public Declare Function GdipCreateFromHWND Lib "gdiplus" (ByVal hwnd As Long, graphics As Long) As GpStatus
Public Declare Function GdipGetImageGraphicsContext Lib "gdiplus" (ByVal Image As Long, graphics As Long) As GpStatus
Public Declare Function GdipDeleteGraphics Lib "gdiplus" (ByVal graphics As Long) As GpStatus
 
Public Declare Function GdipGraphicsClear Lib "gdiplus" (ByVal graphics As Long, ByVal lColor As Long) As GpStatus
 
Public Declare Function GdipSetCompositingMode Lib "gdiplus" (ByVal graphics As Long, ByVal CompositingMd As CompositingMode) As GpStatus
Public Declare Function GdipGetCompositingMode Lib "gdiplus" (ByVal graphics As Long, CompositingMd As CompositingMode) As GpStatus
Public Declare Function GdipSetRenderingOrigin Lib "gdiplus" (ByVal graphics As Long, ByVal x As Long, ByVal y As Long) As GpStatus
Public Declare Function GdipGetRenderingOrigin Lib "gdiplus" (ByVal graphics As Long, x As Long, y As Long) As GpStatus
Public Declare Function GdipSetCompositingQuality Lib "gdiplus" (ByVal graphics As Long, ByVal CompositingQlty As CompositingQuality) As GpStatus
Public Declare Function GdipGetCompositingQuality Lib "gdiplus" (ByVal graphics As Long, CompositingQlty As CompositingQuality) As GpStatus
Public Declare Function GdipSetSmoothingMode Lib "gdiplus" (ByVal graphics As Long, ByVal SmoothingMd As SmoothingMode) As GpStatus
Public Declare Function GdipGetSmoothingMode Lib "gdiplus" (ByVal graphics As Long, SmoothingMd As SmoothingMode) As GpStatus
Public Declare Function GdipSetPixelOffsetMode Lib "gdiplus" (ByVal graphics As Long, ByVal PixOffsetMode As PixelOffsetMode) As GpStatus
Public Declare Function GdipGetPixelOffsetMode Lib "gdiplus" (ByVal graphics As Long, PixOffsetMode As PixelOffsetMode) As GpStatus
Public Declare Function GdipSetTextRenderingHint Lib "gdiplus" (ByVal graphics As Long, ByVal Mode As TextRenderingHint) As GpStatus
Public Declare Function GdipGetTextRenderingHint Lib "gdiplus" (ByVal graphics As Long, Mode As TextRenderingHint) As GpStatus
Public Declare Function GdipSetTextContrast Lib "gdiplus" (ByVal graphics As Long, ByVal contrast As Long) As GpStatus
Public Declare Function GdipGetTextContrast Lib "gdiplus" (ByVal graphics As Long, contrast As Long) As GpStatus
Public Declare Function GdipSetInterpolationMode Lib "gdiplus" (ByVal graphics As Long, ByVal interpolation As InterpolationMode) As GpStatus
Public Declare Function GdipGetInterpolationMode Lib "gdiplus" (ByVal graphics As Long, interpolation As InterpolationMode) As GpStatus
 
Public Declare Function GdipSetWorldTransform Lib "gdiplus" (ByVal graphics As Long, ByVal matrix As Long) As GpStatus
Public Declare Function GdipResetWorldTransform Lib "gdiplus" (ByVal graphics As Long) As GpStatus
Public Declare Function GdipMultiplyWorldTransform Lib "gdiplus" (ByVal graphics As Long, ByVal matrix As Long, ByVal order As MatrixOrder) As GpStatus
Public Declare Function GdipTranslateWorldTransform Lib "gdiplus" (ByVal graphics As Long, ByVal dx As Single, ByVal dy As Single, ByVal order As MatrixOrder) As GpStatus
Public Declare Function GdipScaleWorldTransform Lib "gdiplus" (ByVal graphics As Long, ByVal sx As Single, ByVal sy As Single, ByVal order As MatrixOrder) As GpStatus
Public Declare Function GdipRotateWorldTransform Lib "gdiplus" (ByVal graphics As Long, ByVal angle As Single, ByVal order As MatrixOrder) As GpStatus
Public Declare Function GdipGetWorldTransform Lib "gdiplus" (ByVal graphics As Long, ByVal matrix As Long) As GpStatus
Public Declare Function GdipResetPageTransform Lib "gdiplus" (ByVal graphics As Long) As GpStatus
Public Declare Function GdipGetPageUnit Lib "gdiplus" (ByVal graphics As Long, unit As GpUnit) As GpStatus
Public Declare Function GdipGetPageScale Lib "gdiplus" (ByVal graphics As Long, sScale As Single) As GpStatus
Public Declare Function GdipSetPageUnit Lib "gdiplus" (ByVal graphics As Long, ByVal unit As GpUnit) As GpStatus
Public Declare Function GdipSetPageScale Lib "gdiplus" (ByVal graphics As Long, ByVal sScale As Single) As GpStatus
Public Declare Function GdipGetDpiX Lib "gdiplus" (ByVal graphics As Long, dpi As Single) As GpStatus
Public Declare Function GdipGetDpiY Lib "gdiplus" (ByVal graphics As Long, dpi As Single) As GpStatus
Public Declare Function GdipTransformPoints Lib "gdiplus" (ByVal graphics As Long, ByVal destSpace As CoordinateSpace, ByVal srcSpace As CoordinateSpace, POINTS As POINTF, ByVal Count As Long) As GpStatus
Public Declare Function GdipTransformPointsI Lib "gdiplus" (ByVal graphics As Long, ByVal destSpace As CoordinateSpace, ByVal srcSpace As CoordinateSpace, POINTS As POINTL, ByVal Count As Long) As GpStatus
Public Declare Function GdipGetNearestColor Lib "gdiplus" (ByVal graphics As Long, argb As Long) As GpStatus
Public Declare Function GdipCreateHalftonePalette Lib "gdiplus" () As Long
 
Public Declare Function GdipSetClipGraphics Lib "gdiplus" (ByVal graphics As Long, ByVal srcgraphics As Long, ByVal CombineMd As CombineMode) As GpStatus
Public Declare Function GdipSetClipRect Lib "gdiplus" (ByVal graphics As Long, ByVal x As Single, ByVal y As Single, ByVal Width As Single, ByVal Height As Single, ByVal CombineMd As CombineMode) As GpStatus
Public Declare Function GdipSetClipRectI Lib "gdiplus" (ByVal graphics As Long, ByVal x As Long, ByVal y As Long, ByVal Width As Long, ByVal Height As Long, ByVal CombineMd As CombineMode) As GpStatus
Public Declare Function GdipSetClipPath Lib "gdiplus" (ByVal graphics As Long, ByVal Path As Long, ByVal CombineMd As CombineMode) As GpStatus
Public Declare Function GdipSetClipRegion Lib "gdiplus" (ByVal graphics As Long, ByVal region As Long, ByVal CombineMd As CombineMode) As GpStatus
Public Declare Function GdipSetClipHrgn Lib "gdiplus" (ByVal graphics As Long, ByVal hRgn As Long, ByVal CombineMd As CombineMode) As GpStatus
Public Declare Function GdipResetClip Lib "gdiplus" (ByVal graphics As Long) As GpStatus
 
Public Declare Function GdipTranslateClip Lib "gdiplus" (ByVal graphics As Long, ByVal dx As Single, ByVal dy As Single) As GpStatus
Public Declare Function GdipTranslateClipI Lib "gdiplus" (ByVal graphics As Long, ByVal dx As Long, ByVal dy As Long) As GpStatus
Public Declare Function GdipGetClip Lib "gdiplus" (ByVal graphics As Long, ByVal region As Long) As GpStatus
Public Declare Function GdipGetClipBounds Lib "gdiplus" (ByVal graphics As Long, rect As RECTF) As GpStatus
Public Declare Function GdipGetClipBoundsI Lib "gdiplus" (ByVal graphics As Long, rect As RECTL) As GpStatus
 
Public Declare Function GdipIsClipEmpty Lib "gdiplus" (ByVal graphics As Long, Result As Long) As GpStatus
Public Declare Function GdipGetVisibleClipBounds Lib "gdiplus" (ByVal graphics As Long, rect As RECTF) As GpStatus
Public Declare Function GdipGetVisibleClipBoundsI Lib "gdiplus" (ByVal graphics As Long, rect As RECTL) As GpStatus
Public Declare Function GdipIsVisibleClipEmpty Lib "gdiplus" (ByVal graphics As Long, Result As Long) As GpStatus
 
Public Declare Function GdipIsVisiblePoint Lib "gdiplus" (ByVal graphics As Long, ByVal x As Single, ByVal y As Single, Result As Long) As GpStatus
Public Declare Function GdipIsVisiblePointI Lib "gdiplus" (ByVal graphics As Long, ByVal x As Long, ByVal y As Long, Result As Long) As GpStatus
Public Declare Function GdipIsVisibleRect Lib "gdiplus" (ByVal graphics As Long, ByVal x As Single, ByVal y As Single, ByVal Width As Single, ByVal Height As Single, Result As Long) As GpStatus
Public Declare Function GdipIsVisibleRectI Lib "gdiplus" (ByVal graphics As Long, ByVal x As Long, ByVal y As Long, ByVal Width As Long, ByVal Height As Long, Result As Long) As GpStatus
 
Public Declare Function GdipSaveGraphics Lib "gdiplus" (ByVal graphics As Long, State As Long) As GpStatus
Public Declare Function GdipRestoreGraphics Lib "gdiplus" (ByVal graphics As Long, ByVal State As Long) As GpStatus
Public Declare Function GdipBeginContainer Lib "gdiplus" (ByVal graphics As Long, dstRect As RECTF, srcRect As RECTF, ByVal unit As GpUnit, State As Long) As GpStatus
Public Declare Function GdipBeginContainerI Lib "gdiplus" (ByVal graphics As Long, dstRect As RECTL, srcRect As RECTL, ByVal unit As GpUnit, State As Long) As GpStatus
Public Declare Function GdipBeginContainer2 Lib "gdiplus" (ByVal graphics As Long, State As Long) As GpStatus
Public Declare Function GdipEndContainer Lib "gdiplus" (ByVal graphics As Long, ByVal State As Long) As GpStatus
 
'==================================================
 
Public Declare Function GdipDrawLine Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, ByVal X1 As Single, ByVal Y1 As Single, ByVal X2 As Single, ByVal Y2 As Single) As GpStatus
Public Declare Function GdipDrawLineI Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, ByVal X1 As Long, ByVal Y1 As Long, ByVal X2 As Long, ByVal Y2 As Long) As GpStatus
Public Declare Function GdipDrawLines Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, POINTS As POINTF, ByVal Count As Long) As GpStatus
Public Declare Function GdipDrawLinesI Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, POINTS As POINTL, ByVal Count As Long) As GpStatus
 
'==================================================
 
Public Declare Function GdipDrawArc Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, ByVal x As Single, ByVal y As Single, ByVal Width As Single, ByVal Height As Single, ByVal startAngle As Single, ByVal sweepAngle As Single) As GpStatus
Public Declare Function GdipDrawArcI Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, ByVal x As Long, ByVal y As Long, ByVal Width As Long, ByVal Height As Long, ByVal startAngle As Single, ByVal sweepAngle As Single) As GpStatus
 
'==================================================
 
Public Declare Function GdipDrawBezier Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, ByVal X1 As Single, ByVal Y1 As Single, ByVal X2 As Single, ByVal Y2 As Single, ByVal X3 As Single, ByVal Y3 As Single, ByVal X4 As Single, ByVal Y4 As Single) As GpStatus
Public Declare Function GdipDrawBezierI Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, ByVal X1 As Long, ByVal Y1 As Long, ByVal X2 As Long, ByVal Y2 As Long, ByVal X3 As Long, ByVal Y3 As Long, ByVal X4 As Long, ByVal Y4 As Long) As GpStatus
Public Declare Function GdipDrawBeziers Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, POINTS As POINTF, ByVal Count As Long) As GpStatus
 
'==================================================
 
Public Declare Function GdipDrawBeziersI Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, POINTS As POINTL, ByVal Count As Long) As GpStatus
Public Declare Function GdipDrawRectangle Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, ByVal x As Single, ByVal y As Single, ByVal Width As Single, ByVal Height As Single) As GpStatus
Public Declare Function GdipDrawRectangleI Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, ByVal x As Long, ByVal y As Long, ByVal Width As Long, ByVal Height As Long) As GpStatus
Public Declare Function GdipDrawRectangles Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, RECTS As RECTF, ByVal Count As Long) As GpStatus
Public Declare Function GdipDrawRectanglesI Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, RECTS As RECTL, ByVal Count As Long) As GpStatus
 
Public Declare Function GdipFillRectangle Lib "gdiplus" (ByVal graphics As Long, ByVal brush As Long, ByVal x As Single, ByVal y As Single, ByVal Width As Single, ByVal Height As Single) As GpStatus
Public Declare Function GdipFillRectangleI Lib "gdiplus" (ByVal graphics As Long, ByVal brush As Long, ByVal x As Long, ByVal y As Long, ByVal Width As Long, ByVal Height As Long) As GpStatus
Public Declare Function GdipFillRectangles Lib "gdiplus" (ByVal graphics As Long, ByVal brush As Long, RECTS As RECTF, ByVal Count As Long) As GpStatus
Public Declare Function GdipFillRectanglesI Lib "gdiplus" (ByVal graphics As Long, ByVal brush As Long, RECTS As RECTL, ByVal Count As Long) As GpStatus
 
'==================================================
 
Public Declare Function GdipDrawEllipse Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, ByVal x As Single, ByVal y As Single, ByVal Width As Single, ByVal Height As Single) As GpStatus
Public Declare Function GdipDrawEllipseI Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, ByVal x As Long, ByVal y As Long, ByVal Width As Long, ByVal Height As Long) As GpStatus
 
Public Declare Function GdipFillEllipse Lib "gdiplus" (ByVal graphics As Long, ByVal brush As Long, ByVal x As Single, ByVal y As Single, ByVal Width As Single, ByVal Height As Single) As GpStatus
Public Declare Function GdipFillEllipseI Lib "gdiplus" (ByVal graphics As Long, ByVal brush As Long, ByVal x As Long, ByVal y As Long, ByVal Width As Long, ByVal Height As Long) As GpStatus
 
'==================================================
 
Public Declare Function GdipDrawPie Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, ByVal x As Single, ByVal y As Single, ByVal Width As Single, ByVal Height As Single, ByVal startAngle As Single, ByVal sweepAngle As Single) As GpStatus
Public Declare Function GdipDrawPieI Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, ByVal x As Long, ByVal y As Long, ByVal Width As Long, ByVal Height As Long, ByVal startAngle As Single, ByVal sweepAngle As Single) As GpStatus
 
Public Declare Function GdipFillPie Lib "gdiplus" (ByVal graphics As Long, ByVal brush As Long, ByVal x As Single, ByVal y As Single, ByVal Width As Single, ByVal Height As Single, ByVal startAngle As Single, ByVal sweepAngle As Single) As GpStatus
Public Declare Function GdipFillPieI Lib "gdiplus" (ByVal graphics As Long, ByVal brush As Long, ByVal x As Long, ByVal y As Long, ByVal Width As Long, ByVal Height As Long, ByVal startAngle As Single, ByVal sweepAngle As Single) As GpStatus
 
'==================================================
 
Public Declare Function GdipDrawPolygon Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, POINTS As POINTF, ByVal Count As Long) As GpStatus
Public Declare Function GdipDrawPolygonI Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, POINTS As POINTL, ByVal Count As Long) As GpStatus
 
Public Declare Function GdipFillPolygon Lib "gdiplus" (ByVal graphics As Long, ByVal brush As Long, POINTS As POINTF, ByVal Count As Long, ByVal FillMd As FillMode) As GpStatus
Public Declare Function GdipFillPolygonI Lib "gdiplus" (ByVal graphics As Long, ByVal brush As Long, POINTS As POINTL, ByVal Count As Long, ByVal FillMd As FillMode) As GpStatus
Public Declare Function GdipFillPolygon2 Lib "gdiplus" (ByVal graphics As Long, ByVal brush As Long, POINTS As POINTF, ByVal Count As Long) As GpStatus
Public Declare Function GdipFillPolygon2I Lib "gdiplus" (ByVal graphics As Long, ByVal brush As Long, POINTS As POINTL, ByVal Count As Long) As GpStatus
 
'==================================================
 
Public Declare Function GdipDrawPath Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, ByVal Path As Long) As GpStatus
 
Public Declare Function GdipFillPath Lib "gdiplus" (ByVal graphics As Long, ByVal brush As Long, ByVal Path As Long) As GpStatus
 
'==================================================
 
Public Declare Function GdipDrawCurve Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, POINTS As POINTF, ByVal Count As Long) As GpStatus
Public Declare Function GdipDrawCurveI Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, POINTS As POINTL, ByVal Count As Long) As GpStatus
Public Declare Function GdipDrawCurve2 Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, POINTS As POINTF, ByVal Count As Long, ByVal tension As Single) As GpStatus
Public Declare Function GdipDrawCurve2I Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, POINTS As POINTL, ByVal Count As Long, ByVal tension As Single) As GpStatus
Public Declare Function GdipDrawCurve3 Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, POINTS As POINTF, ByVal Count As Long, ByVal offset As Long, ByVal numberOfSegments As Long, ByVal tension As Single) As GpStatus
Public Declare Function GdipDrawCurve3I Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, POINTS As POINTL, ByVal Count As Long, ByVal offset As Long, ByVal numberOfSegments As Long, ByVal tension As Single) As GpStatus
 
Public Declare Function GdipDrawClosedCurve Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, POINTS As POINTF, ByVal Count As Long) As GpStatus
Public Declare Function GdipDrawClosedCurveI Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, POINTS As POINTL, ByVal Count As Long) As GpStatus
Public Declare Function GdipDrawClosedCurve2 Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, POINTS As POINTF, ByVal Count As Long, ByVal tension As Single) As GpStatus
Public Declare Function GdipDrawClosedCurve2I Lib "gdiplus" (ByVal graphics As Long, ByVal pen As Long, POINTS As POINTL, ByVal Count As Long, ByVal tension As Single) As GpStatus
 
Public Declare Function GdipFillClosedCurve Lib "gdiplus" (ByVal graphics As Long, ByVal brush As Long, POINTS As POINTF, ByVal Count As Long) As GpStatus
Public Declare Function GdipFillClosedCurveI Lib "gdiplus" (ByVal graphics As Long, ByVal brush As Long, POINTS As POINTL, ByVal Count As Long) As GpStatus
Public Declare Function GdipFillClosedCurve2 Lib "gdiplus" (ByVal graphics As Long, ByVal brush As Long, POINTS As POINTF, ByVal Count As Long, ByVal tension As Single, ByVal FillMd As FillMode) As GpStatus
Public Declare Function GdipFillClosedCurve2I Lib "gdiplus" (ByVal graphics As Long, ByVal brush As Long, POINTS As POINTL, ByVal Count As Long, ByVal tension As Single, ByVal FillMd As FillMode) As GpStatus
 
'==================================================
 
Public Declare Function GdipFillRegion Lib "gdiplus" (ByVal graphics As Long, ByVal brush As Long, ByVal region As Long) As GpStatus
 
'==================================================
 
Public Declare Function GdipDrawImage Lib "gdiplus" (ByVal graphics As Long, ByVal Image As Long, ByVal x As Single, ByVal y As Single) As GpStatus
Public Declare Function GdipDrawImageI Lib "gdiplus" (ByVal graphics As Long, ByVal Image As Long, ByVal x As Long, ByVal y As Long) As GpStatus
 
Public Declare Function GdipDrawImageRect Lib "gdiplus" (ByVal graphics As Long, ByVal Image As Long, ByVal x As Single, ByVal y As Single, ByVal Width As Single, ByVal Height As Single) As GpStatus
Public Declare Function GdipDrawImageRectI Lib "gdiplus" (ByVal graphics As Long, ByVal Image As Long, ByVal x As Long, ByVal y As Long, ByVal Width As Long, ByVal Height As Long) As GpStatus
Public Declare Function GdipDrawImagePoints Lib "gdiplus" (ByVal graphics As Long, ByVal Image As Long, dstpoints As POINTF, ByVal Count As Long) As GpStatus
Public Declare Function GdipDrawImagePointsI Lib "gdiplus" (ByVal graphics As Long, ByVal Image As Long, dstpoints As POINTL, ByVal Count As Long) As GpStatus
Public Declare Function GdipDrawImagePointRect Lib "gdiplus" (ByVal graphics As Long, ByVal Image As Long, ByVal x As Single, ByVal y As Single, ByVal SrcX As Single, ByVal SrcY As Single, ByVal srcwidth As Single, ByVal srcheight As Single, ByVal srcUnit As GpUnit) As GpStatus
Public Declare Function GdipDrawImagePointRectI Lib "gdiplus" (ByVal graphics As Long, ByVal Image As Long, ByVal x As Long, ByVal y As Long, ByVal SrcX As Long, ByVal SrcY As Long, ByVal srcwidth As Long, ByVal srcheight As Long, ByVal srcUnit As GpUnit) As GpStatus
Public Declare Function GdipDrawImageRectRect Lib "gdiplus" (ByVal graphics As Long, ByVal Image As Long, ByVal dstX As Single, ByVal dstY As Single, ByVal dstWidth As Long, ByVal dstHeight As Single, ByVal SrcX As Single, ByVal SrcY As Single, ByVal srcwidth As Single, ByVal srcheight As Single, ByVal srcUnit As GpUnit, Optional ByVal imageAttributes As Long = 0, Optional ByVal callback As Long = 0, Optional ByVal callbackData As Long = 0) As GpStatus
Public Declare Function GdipDrawImageRectRectI Lib "gdiplus" (ByVal graphics As Long, ByVal Image As Long, ByVal dstX As Long, ByVal dstY As Long, ByVal dstWidth As Long, ByVal dstHeight As Long, ByVal SrcX As Long, ByVal SrcY As Long, ByVal srcwidth As Long, ByVal srcheight As Long, ByVal srcUnit As GpUnit, Optional ByVal imageAttributes As Long = 0, Optional ByVal callback As Long = 0, Optional ByVal callbackData As Long = 0) As GpStatus
Public Declare Function GdipDrawImagePointsRect Lib "gdiplus" (ByVal graphics As Long, ByVal Image As Long, POINTS As POINTF, ByVal Count As Long, ByVal SrcX As Single, ByVal SrcY As Single, ByVal srcwidth As Single, ByVal srcheight As Single, ByVal srcUnit As GpUnit, Optional ByVal imageAttributes As Long = 0, Optional ByVal callback As Long = 0, Optional ByVal callbackData As Long = 0) As GpStatus
Public Declare Function GdipDrawImagePointsRectI Lib "gdiplus" (ByVal graphics As Long, ByVal Image As Long, POINTS As POINTL, ByVal Count As Long, ByVal SrcX As Long, ByVal SrcY As Long, ByVal srcwidth As Long, ByVal srcheight As Long, ByVal srcUnit As GpUnit, Optional ByVal imageAttributes As Long = 0, Optional ByVal callback As Long = 0, Optional ByVal callbackData As Long = 0) As GpStatus
 
Public Declare Function GdipGetImageDecoders Lib "gdiplus" (ByVal numDecoders As Long, ByVal Size As Long, decoders As Any) As GpStatus
Public Declare Function GdipGetImageEncodersSize Lib "gdiplus" (numEncoders As Long, Size As Long) As GpStatus
Public Declare Function GdipGetImageEncoders Lib "gdiplus" (ByVal numEncoders As Long, ByVal Size As Long, encoders As Any) As GpStatus
Public Declare Function GdipComment Lib "gdiplus" (ByVal graphics As Long, ByVal sizeData As Long, Data As Any) As GpStatus
 
Public Declare Function GdipLoadImageFromFile Lib "gdiplus" (ByVal filename As String, Image As Long) As GpStatus
Public Declare Function GdipLoadImageFromFileICM Lib "gdiplus" (ByVal filename As Long, Image As Long) As GpStatus
Public Declare Function GdipLoadImageFromStream Lib "gdiplus" (ByVal stream As Any, Image As Long) As GpStatus
Public Declare Function GdipLoadImageFromStreamICM Lib "gdiplus" (ByVal stream As Any, Image As Long) As GpStatus
Public Declare Function GdipDisposeImage Lib "gdiplus" (ByVal Image As Long) As GpStatus
Public Declare Function GdipCloneImage Lib "gdiplus" (ByVal Image As Long, cloneImage As Long) As GpStatus
 
Public Declare Function GdipSaveImageToFile Lib "gdiplus" (ByVal Image As Long, ByVal filename As Long, clsidEncoder As Clsid, encoderParams As Any) As GpStatus
Public Declare Function GdipSaveImageToStream Lib "gdiplus" (ByVal Image As Long, ByVal stream As Any, clsidEncoder As Clsid, encoderParams As Any) As GpStatus
 
Public Declare Function GdipSaveAdd Lib "gdiplus" (ByVal Image As Long, encoderParams As EncoderParameters) As GpStatus
Public Declare Function GdipSaveAddImage Lib "gdiplus" (ByVal Image As Long, ByVal newImage As Long, encoderParams As EncoderParameters) As GpStatus
 
Public Declare Function GdipGetImageBounds Lib "gdiplus" (ByVal Image As Long, srcRect As RECTF, srcUnit As GpUnit) As GpStatus
Public Declare Function GdipGetImageDimension Lib "gdiplus" (ByVal Image As Long, Width As Single, Height As Single) As GpStatus
Public Declare Function GdipGetImageType Lib "gdiplus" (ByVal Image As Long, iType As Image_Type) As GpStatus
Public Declare Function GdipGetImageWidth Lib "gdiplus" (ByVal Image As Long, Width As Long) As GpStatus
Public Declare Function GdipGetImageHeight Lib "gdiplus" (ByVal Image As Long, Height As Long) As GpStatus
Public Declare Function GdipGetImageHorizontalResolution Lib "gdiplus" (ByVal Image As Long, resolution As Single) As GpStatus
Public Declare Function GdipGetImageVerticalResolution Lib "gdiplus" (ByVal Image As Long, resolution As Single) As GpStatus
Public Declare Function GdipGetImageFlags Lib "gdiplus" (ByVal Image As Long, flags As Long) As GpStatus
Public Declare Function GdipGetImageRawFormat Lib "gdiplus" (ByVal Image As Long, Format As Clsid) As GpStatus
Public Declare Function GdipGetImagePixelFormat Lib "gdiplus" (ByVal Image As Long, PixelFormat As Long) As GpStatus
Public Declare Function GdipGetImageThumbnail Lib "gdiplus" (ByVal Image As Long, ByVal thumbWidth As Long, ByVal thumbHeight As Long, thumbImage As Long, Optional ByVal callback As Long = 0, Optional ByVal callbackData As Long = 0) As GpStatus
Public Declare Function GdipGetEncoderParameterListSize Lib "gdiplus" (ByVal Image As Long, clsidEncoder As Clsid, Size As Long) As GpStatus
Public Declare Function GdipGetEncoderParameterList Lib "gdiplus" (ByVal Image As Long, clsidEncoder As Clsid, ByVal Size As Long, Buffer As EncoderParameters) As GpStatus
 
Public Declare Function GdipImageGetFrameDimensionsCount Lib "gdiplus" (ByVal Image As Long, Count As Long) As GpStatus
Public Declare Function GdipImageGetFrameDimensionsList Lib "gdiplus" (ByVal Image As Long, dimensionIDs As Clsid, ByVal Count As Long) As GpStatus
Public Declare Function GdipImageGetFrameCount Lib "gdiplus" (ByVal Image As Long, dimensionID As Clsid, Count As Long) As GpStatus
Public Declare Function GdipImageSelectActiveFrame Lib "gdiplus" (ByVal Image As Long, dimensionID As Clsid, ByVal frameIndex As Long) As GpStatus
Public Declare Function GdipImageRotateFlip Lib "gdiplus" (ByVal Image As Long, ByVal rfType As RotateFlipType) As GpStatus
Public Declare Function GdipGetImagePalette Lib "gdiplus" (ByVal Image As Long, palette As ColorPalette, ByVal Size As Long) As GpStatus
Public Declare Function GdipSetImagePalette Lib "gdiplus" (ByVal Image As Long, palette As ColorPalette) As GpStatus
Public Declare Function GdipGetImagePaletteSize Lib "gdiplus" (ByVal Image As Long, Size As Long) As GpStatus
Public Declare Function GdipGetPropertyCount Lib "gdiplus" (ByVal Image As Long, numOfProperty As Long) As GpStatus
Public Declare Function GdipGetPropertyIdList Lib "gdiplus" (ByVal Image As Long, ByVal numOfProperty As Long, list As Long) As GpStatus
Public Declare Function GdipGetPropertyItemSize Lib "gdiplus" (ByVal Image As Long, ByVal propId As Long, Size As Long) As GpStatus
Public Declare Function GdipGetPropertyItem Lib "gdiplus" (ByVal Image As Long, ByVal propId As Long, ByVal propSize As Long, Buffer As PropertyItem) As GpStatus
Public Declare Function GdipGetPropertySize Lib "gdiplus" (ByVal Image As Long, totalBufferSize As Long, numProperties As Long) As GpStatus
Public Declare Function GdipGetAllPropertyItems Lib "gdiplus" (ByVal Image As Long, ByVal totalBufferSize As Long, ByVal numProperties As Long, allItems As PropertyItem) As GpStatus
Public Declare Function GdipRemovePropertyItem Lib "gdiplus" (ByVal Image As Long, ByVal propId As Long) As GpStatus
Public Declare Function GdipSetPropertyItem Lib "gdiplus" (ByVal Image As Long, item As PropertyItem) As GpStatus
Public Declare Function GdipImageForceValidation Lib "gdiplus" (ByVal Image As Long) As GpStatus
 
'==================================================
 
Public Declare Function GdipCreatePen1 Lib "gdiplus" (ByVal Color As Long, ByVal Width As Single, ByVal unit As GpUnit, pen As Long) As GpStatus
Public Declare Function GdipCreatePen2 Lib "gdiplus" (ByVal brush As Long, ByVal Width As Single, ByVal unit As GpUnit, pen As Long) As GpStatus
Public Declare Function GdipClonePen Lib "gdiplus" (ByVal pen As Long, clonepen As Long) As GpStatus
Public Declare Function GdipDeletePen Lib "gdiplus" (ByVal pen As Long) As GpStatus
 
Public Declare Function GdipSetPenWidth Lib "gdiplus" (ByVal pen As Long, ByVal Width As Single) As GpStatus
Public Declare Function GdipGetPenWidth Lib "gdiplus" (ByVal pen As Long, Width As Single) As GpStatus
Public Declare Function GdipSetPenUnit Lib "gdiplus" (ByVal pen As Long, ByVal unit As GpUnit) As GpStatus
Public Declare Function GdipGetPenUnit Lib "gdiplus" (ByVal pen As Long, unit As GpUnit) As GpStatus
 
Public Declare Function GdipSetPenLineCap Lib "gdiplus" Alias "GdipSetPenLineCap197819" (ByVal pen As Long, ByVal startCap As LineCap, ByVal endCap As LineCap, ByVal dcap As DashCap) As GpStatus
Public Declare Function GdipSetPenStartCap Lib "gdiplus" (ByVal pen As Long, ByVal startCap As LineCap) As GpStatus
Public Declare Function GdipSetPenEndCap Lib "gdiplus" (ByVal pen As Long, ByVal endCap As LineCap) As GpStatus
Public Declare Function GdipSetPenDashCap Lib "gdiplus" Alias "GdipSetPenDashCap197819" (ByVal pen As Long, ByVal dcap As DashCap) As GpStatus
Public Declare Function GdipGetPenStartCap Lib "gdiplus" (ByVal pen As Long, startCap As LineCap) As GpStatus
Public Declare Function GdipGetPenEndCap Lib "gdiplus" (ByVal pen As Long, endCap As LineCap) As GpStatus
Public Declare Function GdipGetPenDashCap Lib "gdiplus" Alias "GdipGetPenDashCap197819" (ByVal pen As Long, dcap As DashCap) As GpStatus
Public Declare Function GdipSetPenLineJoin Lib "gdiplus" (ByVal pen As Long, ByVal lnJoin As LineJoin) As GpStatus
Public Declare Function GdipGetPenLineJoin Lib "gdiplus" (ByVal pen As Long, lnJoin As LineJoin) As GpStatus
Public Declare Function GdipSetPenCustomStartCap Lib "gdiplus" (ByVal pen As Long, ByVal customCap As Long) As GpStatus
Public Declare Function GdipGetPenCustomStartCap Lib "gdiplus" (ByVal pen As Long, customCap As Long) As GpStatus
Public Declare Function GdipSetPenCustomEndCap Lib "gdiplus" (ByVal pen As Long, ByVal customCap As Long) As GpStatus
Public Declare Function GdipGetPenCustomEndCap Lib "gdiplus" (ByVal pen As Long, customCap As Long) As GpStatus
 
Public Declare Function GdipSetPenMiterLimit Lib "gdiplus" (ByVal pen As Long, ByVal miterLimit As Single) As GpStatus
Public Declare Function GdipGetPenMiterLimit Lib "gdiplus" (ByVal pen As Long, miterLimit As Single) As GpStatus
Public Declare Function GdipSetPenMode Lib "gdiplus" (ByVal pen As Long, ByVal penMode As PenAlignment) As GpStatus
Public Declare Function GdipGetPenMode Lib "gdiplus" (ByVal pen As Long, penMode As PenAlignment) As GpStatus
Public Declare Function GdipSetPenTransform Lib "gdiplus" (ByVal pen As Long, ByVal matrix As Long) As GpStatus
Public Declare Function GdipGetPenTransform Lib "gdiplus" (ByVal pen As Long, ByVal matrix As Long) As GpStatus
Public Declare Function GdipResetPenTransform Lib "gdiplus" (ByVal pen As Long) As GpStatus
Public Declare Function GdipMultiplyPenTransform Lib "gdiplus" (ByVal pen As Long, ByVal matrix As Long, ByVal order As MatrixOrder) As GpStatus
Public Declare Function GdipTranslatePenTransform Lib "gdiplus" (ByVal pen As Long, ByVal dx As Single, ByVal dy As Single, ByVal order As MatrixOrder) As GpStatus
Public Declare Function GdipScalePenTransform Lib "gdiplus" (ByVal pen As Long, ByVal sx As Single, ByVal sy As Single, ByVal order As MatrixOrder) As GpStatus
Public Declare Function GdipRotatePenTransform Lib "gdiplus" (ByVal pen As Long, ByVal angle As Single, ByVal order As MatrixOrder) As GpStatus
Public Declare Function GdipSetPenColor Lib "gdiplus" (ByVal pen As Long, ByVal argb As Long) As GpStatus
Public Declare Function GdipGetPenColor Lib "gdiplus" (ByVal pen As Long, argb As Long) As GpStatus
Public Declare Function GdipSetPenBrushFill Lib "gdiplus" (ByVal pen As Long, ByVal brush As Long) As GpStatus
Public Declare Function GdipGetPenBrushFill Lib "gdiplus" (ByVal pen As Long, brush As Long) As GpStatus
Public Declare Function GdipGetPenFillType Lib "gdiplus" (ByVal pen As Long, pType As PenType) As GpStatus
Public Declare Function GdipGetPenDashStyle Lib "gdiplus" (ByVal pen As Long, dStyle As DashStyle) As GpStatus
Public Declare Function GdipSetPenDashStyle Lib "gdiplus" (ByVal pen As Long, ByVal dStyle As DashStyle) As GpStatus
Public Declare Function GdipGetPenDashOffset Lib "gdiplus" (ByVal pen As Long, offset As Single) As GpStatus
Public Declare Function GdipSetPenDashOffset Lib "gdiplus" (ByVal pen As Long, ByVal offset As Single) As GpStatus
Public Declare Function GdipGetPenDashCount Lib "gdiplus" (ByVal pen As Long, Count As Long) As GpStatus
Public Declare Function GdipSetPenDashArray Lib "gdiplus" (ByVal pen As Long, dash As Single, ByVal Count As Long) As GpStatus
Public Declare Function GdipGetPenDashArray Lib "gdiplus" (ByVal pen As Long, dash As Single, ByVal Count As Long) As GpStatus
Public Declare Function GdipGetPenCompoundCount Lib "gdiplus" (ByVal pen As Long, Count As Long) As GpStatus
Public Declare Function GdipSetPenCompoundArray Lib "gdiplus" (ByVal pen As Long, dash As Single, ByVal Count As Long) As GpStatus
Public Declare Function GdipGetPenCompoundArray Lib "gdiplus" (ByVal pen As Long, dash As Single, ByVal Count As Long) As GpStatus
 
Public Declare Function GdipCreateCustomLineCap Lib "gdiplus" (ByVal FillPath As Long, ByVal StrokePath As Long, ByVal baseCap As LineCap, ByVal baseInset As Single, customCap As Long) As GpStatus
Public Declare Function GdipDeleteCustomLineCap Lib "gdiplus" (ByVal customCap As Long) As GpStatus
Public Declare Function GdipCloneCustomLineCap Lib "gdiplus" (ByVal customCap As Long, clonedCap As Long) As GpStatus
Public Declare Function GdipGetCustomLineCapType Lib "gdiplus" (ByVal customCap As Long, capType As CustomLineCapType) As GpStatus
Public Declare Function GdipSetCustomLineCapStrokeCaps Lib "gdiplus" (ByVal customCap As Long, ByVal startCap As LineCap, ByVal endCap As LineCap) As GpStatus
Public Declare Function GdipGetCustomLineCapStrokeCaps Lib "gdiplus" (ByVal customCap As Long, startCap As LineCap, endCap As LineCap) As GpStatus
Public Declare Function GdipSetCustomLineCapStrokeJoin Lib "gdiplus" (ByVal customCap As Long, ByVal lnJoin As LineJoin) As GpStatus
Public Declare Function GdipGetCustomLineCapStrokeJoin Lib "gdiplus" (ByVal customCap As Long, lnJoin As LineJoin) As GpStatus
Public Declare Function GdipSetCustomLineCapBaseCap Lib "gdiplus" (ByVal customCap As Long, ByVal baseCap As LineCap) As GpStatus
Public Declare Function GdipGetCustomLineCapBaseCap Lib "gdiplus" (ByVal customCap As Long, baseCap As LineCap) As GpStatus
Public Declare Function GdipSetCustomLineCapBaseInset Lib "gdiplus" (ByVal customCap As Long, ByVal inset As Single) As GpStatus
Public Declare Function GdipGetCustomLineCapBaseInset Lib "gdiplus" (ByVal customCap As Long, inset As Single) As GpStatus
Public Declare Function GdipSetCustomLineCapWidthScale Lib "gdiplus" (ByVal customCap As Long, ByVal widthScale As Single) As GpStatus
Public Declare Function GdipGetCustomLineCapWidthScale Lib "gdiplus" (ByVal customCap As Long, widthScale As Single) As GpStatus
 
Public Declare Function GdipCreateAdjustableArrowCap Lib "gdiplus" (ByVal Height As Single, ByVal Width As Single, ByVal isFilled As Long, cap As Long) As GpStatus
Public Declare Function GdipSetAdjustableArrowCapHeight Lib "gdiplus" (ByVal cap As Long, ByVal Height As Single) As GpStatus
Public Declare Function GdipGetAdjustableArrowCapHeight Lib "gdiplus" (ByVal cap As Long, Height As Single) As GpStatus
Public Declare Function GdipSetAdjustableArrowCapWidth Lib "gdiplus" (ByVal cap As Long, ByVal Width As Single) As GpStatus
Public Declare Function GdipGetAdjustableArrowCapWidth Lib "gdiplus" (ByVal cap As Long, Width As Single) As GpStatus
Public Declare Function GdipSetAdjustableArrowCapMiddleInset Lib "gdiplus" (ByVal cap As Long, ByVal middleInset As Single) As GpStatus
Public Declare Function GdipGetAdjustableArrowCapMiddleInset Lib "gdiplus" (ByVal cap As Long, middleInset As Single) As GpStatus
Public Declare Function GdipSetAdjustableArrowCapFillState Lib "gdiplus" (ByVal cap As Long, ByVal bFillState As Long) As GpStatus
Public Declare Function GdipGetAdjustableArrowCapFillState Lib "gdiplus" (ByVal cap As Long, bFillState As Long) As GpStatus
 
'==================================================
 
Public Declare Function GdipCreateBitmapFromFile Lib "gdiplus" (ByVal filename As Long, bitmap As Long) As GpStatus
Public Declare Function GdipCreateBitmapFromFileICM Lib "gdiplus" (ByVal filename As Long, bitmap As Long) As GpStatus
Public Declare Function GdipCreateBitmapFromStream Lib "gdiplus" (ByVal stream As Any, bitmap As Long) As GpStatus
Public Declare Function GdipCreateBitmapFromStreamICM Lib "gdiplus" (ByVal stream As Any, bitmap As Long) As GpStatus
Public Declare Function GdipCreateBitmapFromScan0 Lib "gdiplus" (ByVal Width As Long, ByVal Height As Long, ByVal stride As Long, ByVal PixelFormat As Long, scan0 As Any, bitmap As Long) As GpStatus
Public Declare Function GdipCreateBitmapFromGraphics Lib "gdiplus" (ByVal Width As Long, ByVal Height As Long, ByVal graphics As Long, bitmap As Long) As GpStatus
Public Declare Function GdipCreateBitmapFromGdiDib Lib "gdiplus" (gdiBitmapInfo As BITMAPINFO, ByVal gdiBitmapData As Long, bitmap As Long) As GpStatus
Public Declare Function GdipCreateBitmapFromHBITMAP Lib "gdiplus" (ByVal hbm As Long, ByVal hpal As Long, bitmap As Long) As GpStatus
Public Declare Function GdipCreateHBITMAPFromBitmap Lib "gdiplus" (ByVal bitmap As Long, hbmReturn As Long, ByVal Background As Long) As GpStatus
Public Declare Function GdipCreateBitmapFromHICON Lib "gdiplus" (ByVal hicon As Long, bitmap As Long) As GpStatus
Public Declare Function GdipCreateHICONFromBitmap Lib "gdiplus" (ByVal bitmap As Long, hbmReturn As Long) As GpStatus
Public Declare Function GdipCreateBitmapFromResource Lib "gdiplus" (ByVal hInstance As Long, ByVal lpBitmapName As Long, bitmap As Long) As GpStatus
 
Public Declare Function GdipCloneBitmapArea Lib "gdiplus" (ByVal x As Single, ByVal y As Single, ByVal Width As Single, ByVal Height As Single, ByVal PixelFormat As Long, ByVal srcBitmap As Long, dstBitmap As Long) As GpStatus
Public Declare Function GdipCloneBitmapAreaI Lib "gdiplus" (ByVal x As Long, ByVal y As Long, ByVal Width As Long, ByVal Height As Long, ByVal PixelFormat As Long, ByVal srcBitmap As Long, dstBitmap As Long) As GpStatus
 
Public Declare Function GdipBitmapLockBits Lib "gdiplus" (ByVal bitmap As Long, rect As RECTL, ByVal flags As ImageLockMode, ByVal PixelFormat As Long, lockedBitmapData As BitmapData) As GpStatus
Public Declare Function GdipBitmapUnlockBits Lib "gdiplus" (ByVal bitmap As Long, lockedBitmapData As BitmapData) As GpStatus
 
Public Declare Function GdipBitmapGetPixel Lib "gdiplus" (ByVal bitmap As Long, ByVal x As Long, ByVal y As Long, Color As Long) As GpStatus
Public Declare Function GdipBitmapSetPixel Lib "gdiplus" (ByVal bitmap As Long, ByVal x As Long, ByVal y As Long, ByVal Color As Long) As GpStatus
 
Public Declare Function GdipBitmapSetResolution Lib "gdiplus" (ByVal bitmap As Long, ByVal xdpi As Single, ByVal ydpi As Single) As GpStatus
 
Public Declare Function GdipCreateCachedBitmap Lib "gdiplus" (ByVal bitmap As Long, ByVal graphics As Long, cachedBitmap As Long) As GpStatus
Public Declare Function GdipDeleteCachedBitmap Lib "gdiplus" (ByVal cachedBitmap As Long) As GpStatus
Public Declare Function GdipDrawCachedBitmap Lib "gdiplus" (ByVal graphics As Long, ByVal cachedBitmap As Long, ByVal x As Long, ByVal y As Long) As GpStatus
 
'==================================================
 
Public Declare Function GdipCloneBrush Lib "gdiplus" (ByVal brush As Long, cloneBrush As Long) As GpStatus
Public Declare Function GdipDeleteBrush Lib "gdiplus" (ByVal brush As Long) As GpStatus
Public Declare Function GdipGetBrushType Lib "gdiplus" (ByVal brush As Long, brshType As BrushType) As GpStatus
Public Declare Function GdipCreateHatchBrush Lib "gdiplus" (ByVal Style As HatchStyle, ByVal forecolr As Long, ByVal backcolr As Long, brush As Long) As GpStatus
Public Declare Function GdipGetHatchStyle Lib "gdiplus" (ByVal brush As Long, Style As HatchStyle) As GpStatus
Public Declare Function GdipGetHatchForegroundColor Lib "gdiplus" (ByVal brush As Long, forecolr As Long) As GpStatus
Public Declare Function GdipGetHatchBackgroundColor Lib "gdiplus" (ByVal brush As Long, backcolr As Long) As GpStatus
Public Declare Function GdipCreateSolidFill Lib "gdiplus" (ByVal argb As Long, brush As Long) As GpStatus
Public Declare Function GdipSetSolidFillColor Lib "gdiplus" (ByVal brush As Long, ByVal argb As Long) As GpStatus
Public Declare Function GdipGetSolidFillColor Lib "gdiplus" (ByVal brush As Long, argb As Long) As GpStatus
Public Declare Function GdipCreateLineBrush Lib "gdiplus" (Point1 As POINTF, Point2 As POINTF, ByVal color1 As Long, ByVal color2 As Long, ByVal WrapMd As WrapMode, lineGradient As Long) As GpStatus
Public Declare Function GdipCreateLineBrushI Lib "gdiplus" (Point1 As POINTL, Point2 As POINTL, ByVal color1 As Long, ByVal color2 As Long, ByVal WrapMd As WrapMode, lineGradient As Long) As GpStatus
Public Declare Function GdipCreateLineBrushFromRect Lib "gdiplus" (rect As RECTF, ByVal color1 As Long, ByVal color2 As Long, ByVal Mode As LinearGradientMode, ByVal WrapMd As WrapMode, lineGradient As Long) As GpStatus
Public Declare Function GdipCreateLineBrushFromRectI Lib "gdiplus" (rect As RECTL, ByVal color1 As Long, ByVal color2 As Long, ByVal Mode As LinearGradientMode, ByVal WrapMd As WrapMode, lineGradient As Long) As GpStatus
Public Declare Function GdipCreateLineBrushFromRectWithAngle Lib "gdiplus" (rect As RECTF, ByVal color1 As Long, ByVal color2 As Long, ByVal angle As Single, ByVal isAngleScalable As Long, ByVal WrapMd As WrapMode, lineGradient As Long) As GpStatus
Public Declare Function GdipCreateLineBrushFromRectWithAngleI Lib "gdiplus" (rect As RECTL, ByVal color1 As Long, ByVal color2 As Long, ByVal angle As Single, ByVal isAngleScalable As Long, ByVal WrapMd As WrapMode, lineGradient As Long) As GpStatus
Public Declare Function GdipSetLineColors Lib "gdiplus" (ByVal brush As Long, ByVal color1 As Long, ByVal color2 As Long) As GpStatus
Public Declare Function GdipGetLineColors Lib "gdiplus" (ByVal brush As Long, lColors As Long) As GpStatus
Public Declare Function GdipGetLineRect Lib "gdiplus" (ByVal brush As Long, rect As RECTF) As GpStatus
Public Declare Function GdipGetLineRectI Lib "gdiplus" (ByVal brush As Long, rect As RECTL) As GpStatus
Public Declare Function GdipSetLineGammaCorrection Lib "gdiplus" (ByVal brush As Long, ByVal useGammaCorrection As Long) As GpStatus
Public Declare Function GdipGetLineGammaCorrection Lib "gdiplus" (ByVal brush As Long, useGammaCorrection As Long) As GpStatus
Public Declare Function GdipGetLineBlendCount Lib "gdiplus" (ByVal brush As Long, Count As Long) As GpStatus
Public Declare Function GdipGetLineBlend Lib "gdiplus" (ByVal brush As Long, blend As Any, positions As Any, ByVal Count As Long) As GpStatus
Public Declare Function GdipSetLineBlend Lib "gdiplus" (ByVal brush As Long, blend As Any, positions As Any, ByVal Count As Long) As GpStatus
Public Declare Function GdipGetLinePresetBlendCount Lib "gdiplus" (ByVal brush As Long, Count As Long) As GpStatus
Public Declare Function GdipGetLinePresetBlend Lib "gdiplus" (ByVal brush As Long, blend As Any, positions As Any, ByVal Count As Long) As GpStatus
Public Declare Function GdipSetLinePresetBlend Lib "gdiplus" (ByVal brush As Long, blend As Any, positions As Any, ByVal Count As Long) As GpStatus
Public Declare Function GdipSetLineSigmaBlend Lib "gdiplus" (ByVal brush As Long, ByVal Focus As Single, ByVal theScale As Single) As GpStatus
Public Declare Function GdipSetLineLinearBlend Lib "gdiplus" (ByVal brush As Long, ByVal Focus As Single, ByVal theScale As Single) As GpStatus
Public Declare Function GdipSetLineWrapMode Lib "gdiplus" (ByVal brush As Long, ByVal WrapMd As WrapMode) As GpStatus
Public Declare Function GdipGetLineWrapMode Lib "gdiplus" (ByVal brush As Long, WrapMd As WrapMode) As GpStatus
Public Declare Function GdipGetLineTransform Lib "gdiplus" (ByVal brush As Long, matrix As Long) As GpStatus
Public Declare Function GdipSetLineTransform Lib "gdiplus" (ByVal brush As Long, ByVal matrix As Long) As GpStatus
Public Declare Function GdipResetLineTransform Lib "gdiplus" (ByVal brush As Long) As GpStatus
Public Declare Function GdipMultiplyLineTransform Lib "gdiplus" (ByVal brush As Long, ByVal matrix As Long, ByVal order As MatrixOrder) As GpStatus
Public Declare Function GdipTranslateLineTransform Lib "gdiplus" (ByVal brush As Long, ByVal dx As Single, ByVal dy As Single, ByVal order As MatrixOrder) As GpStatus
Public Declare Function GdipScaleLineTransform Lib "gdiplus" (ByVal brush As Long, ByVal sx As Single, ByVal sy As Single, ByVal order As MatrixOrder) As GpStatus
Public Declare Function GdipRotateLineTransform Lib "gdiplus" (ByVal brush As Long, ByVal angle As Single, ByVal order As MatrixOrder) As GpStatus
Public Declare Function GdipCreateTexture Lib "gdiplus" (ByVal Image As Long, ByVal WrapMd As WrapMode, texture As Long) As GpStatus
Public Declare Function GdipCreateTexture2 Lib "gdiplus" (ByVal Image As Long, ByVal WrapMd As WrapMode, ByVal x As Single, ByVal y As Single, ByVal Width As Single, ByVal Height As Single, texture As Long) As GpStatus
Public Declare Function GdipCreateTextureIA Lib "gdiplus" (ByVal Image As Long, ByVal imageAttributes As Long, ByVal x As Single, ByVal y As Single, ByVal Width As Single, ByVal Height As Single, texture As Long) As GpStatus
Public Declare Function GdipCreateTexture2I Lib "gdiplus" (ByVal Image As Long, ByVal WrapMd As WrapMode, ByVal x As Long, ByVal y As Long, ByVal Width As Long, ByVal Height As Long, texture As Long) As GpStatus
Public Declare Function GdipCreateTextureIAI Lib "gdiplus" (ByVal Image As Long, ByVal imageAttributes As Long, ByVal x As Long, ByVal y As Long, ByVal Width As Long, ByVal Height As Long, texture As Long) As GpStatus
Public Declare Function GdipGetTextureTransform Lib "gdiplus" (ByVal brush As Long, ByVal matrix As Long) As GpStatus
Public Declare Function GdipSetTextureTransform Lib "gdiplus" (ByVal brush As Long, ByVal matrix As Long) As GpStatus
Public Declare Function GdipResetTextureTransform Lib "gdiplus" (ByVal brush As Long) As GpStatus
Public Declare Function GdipTranslateTextureTransform Lib "gdiplus" (ByVal brush As Long, ByVal dx As Single, ByVal dy As Single, ByVal order As MatrixOrder) As GpStatus
Public Declare Function GdipMultiplyTextureTransform Lib "gdiplus" (ByVal brush As Long, ByVal matrix As Long, ByVal order As MatrixOrder) As GpStatus
Public Declare Function GdipScaleTextureTransform Lib "gdiplus" (ByVal brush As Long, ByVal sx As Single, ByVal sy As Single, ByVal order As MatrixOrder) As GpStatus
Public Declare Function GdipRotateTextureTransform Lib "gdiplus" (ByVal brush As Long, ByVal angle As Single, ByVal order As MatrixOrder) As GpStatus
Public Declare Function GdipSetTextureWrapMode Lib "gdiplus" (ByVal brush As Long, ByVal WrapMd As WrapMode) As GpStatus
Public Declare Function GdipGetTextureWrapMode Lib "gdiplus" (ByVal brush As Long, WrapMd As WrapMode) As GpStatus
Public Declare Function GdipGetTextureImage Lib "gdiplus" (ByVal brush As Long, Image As Long) As GpStatus
Public Declare Function GdipCreatePathGradient Lib "gdiplus" (POINTS As POINTF, ByVal Count As Long, ByVal WrapMd As WrapMode, polyGradient As Long) As GpStatus
Public Declare Function GdipCreatePathGradientI Lib "gdiplus" (POINTS As POINTL, ByVal Count As Long, ByVal WrapMd As WrapMode, polyGradient As Long) As GpStatus
Public Declare Function GdipCreatePathGradientFromPath Lib "gdiplus" (ByVal Path As Long, polyGradient As Long) As GpStatus
Public Declare Function GdipGetPathGradientCenterColor Lib "gdiplus" (ByVal brush As Long, lColors As Long) As GpStatus
Public Declare Function GdipSetPathGradientCenterColor Lib "gdiplus" (ByVal brush As Long, ByVal lColors As Long) As GpStatus
Public Declare Function GdipGetPathGradientSurroundColorsWithCount Lib "gdiplus" (ByVal brush As Long, argb As Long, Count As Long) As GpStatus
Public Declare Function GdipSetPathGradientSurroundColorsWithCount Lib "gdiplus" (ByVal brush As Long, argb As Long, Count As Long) As GpStatus
Public Declare Function GdipGetPathGradientPath Lib "gdiplus" (ByVal brush As Long, ByVal Path As Long) As GpStatus
Public Declare Function GdipSetPathGradientPath Lib "gdiplus" (ByVal brush As Long, ByVal Path As Long) As GpStatus
Public Declare Function GdipGetPathGradientCenterPoint Lib "gdiplus" (ByVal brush As Long, POINTS As POINTF) As GpStatus
Public Declare Function GdipGetPathGradientCenterPointI Lib "gdiplus" (ByVal brush As Long, POINTS As POINTL) As GpStatus
Public Declare Function GdipSetPathGradientCenterPoint Lib "gdiplus" (ByVal brush As Long, POINTS As POINTF) As GpStatus
Public Declare Function GdipSetPathGradientCenterPointI Lib "gdiplus" (ByVal brush As Long, POINTS As POINTL) As GpStatus
Public Declare Function GdipGetPathGradientRect Lib "gdiplus" (ByVal brush As Long, rect As RECTF) As GpStatus
Public Declare Function GdipGetPathGradientRectI Lib "gdiplus" (ByVal brush As Long, rect As RECTL) As GpStatus
Public Declare Function GdipGetPathGradientPointCount Lib "gdiplus" (ByVal brush As Long, Count As Long) As GpStatus
Public Declare Function GdipGetPathGradientSurroundColorCount Lib "gdiplus" (ByVal brush As Long, Count As Long) As GpStatus
Public Declare Function GdipSetPathGradientGammaCorrection Lib "gdiplus" (ByVal brush As Long, ByVal useGammaCorrection As Long) As GpStatus
Public Declare Function GdipGetPathGradientGammaCorrection Lib "gdiplus" (ByVal brush As Long, useGammaCorrection As Long) As GpStatus
Public Declare Function GdipGetPathGradientBlendCount Lib "gdiplus" (ByVal brush As Long, Count As Long) As GpStatus
Public Declare Function GdipGetPathGradientBlend Lib "gdiplus" (ByVal brush As Long, blend As Single, positions As Single, ByVal Count As Long) As GpStatus
Public Declare Function GdipSetPathGradientBlend Lib "gdiplus" (ByVal brush As Long, blend As Single, positions As Single, ByVal Count As Long) As GpStatus
Public Declare Function GdipGetPathGradientPresetBlendCount Lib "gdiplus" (ByVal brush As Long, Count As Long) As GpStatus
Public Declare Function GdipGetPathGradientPresetBlend Lib "gdiplus" (ByVal brush As Long, blend As Long, positions As Single, ByVal Count As Long) As GpStatus
Public Declare Function GdipSetPathGradientPresetBlend Lib "gdiplus" (ByVal brush As Long, blend As Long, positions As Single, ByVal Count As Long) As GpStatus
Public Declare Function GdipSetPathGradientSigmaBlend Lib "gdiplus" (ByVal brush As Long, ByVal Focus As Single, ByVal sScale As Single) As GpStatus
Public Declare Function GdipSetPathGradientLinearBlend Lib "gdiplus" (ByVal brush As Long, ByVal Focus As Single, ByVal sScale As Single) As GpStatus
Public Declare Function GdipGetPathGradientWrapMode Lib "gdiplus" (ByVal brush As Long, WrapMd As WrapMode) As GpStatus
Public Declare Function GdipSetPathGradientWrapMode Lib "gdiplus" (ByVal brush As Long, ByVal WrapMd As WrapMode) As GpStatus
Public Declare Function GdipGetPathGradientTransform Lib "gdiplus" (ByVal brush As Long, ByVal matrix As Long) As GpStatus
Public Declare Function GdipSetPathGradientTransform Lib "gdiplus" (ByVal brush As Long, ByVal matrix As Long) As GpStatus
Public Declare Function GdipResetPathGradientTransform Lib "gdiplus" (ByVal brush As Long) As GpStatus
Public Declare Function GdipMultiplyPathGradientTransform Lib "gdiplus" (ByVal brush As Long, ByVal matrix As Long, ByVal order As MatrixOrder) As GpStatus
Public Declare Function GdipTranslatePathGradientTransform Lib "gdiplus" (ByVal brush As Long, ByVal dx As Single, ByVal dy As Single, ByVal order As MatrixOrder) As GpStatus
Public Declare Function GdipScalePathGradientTransform Lib "gdiplus" (ByVal brush As Long, ByVal sx As Single, ByVal sy As Single, ByVal order As MatrixOrder) As GpStatus
Public Declare Function GdipRotatePathGradientTransform Lib "gdiplus" (ByVal brush As Long, ByVal angle As Single, ByVal order As MatrixOrder) As GpStatus
Public Declare Function GdipGetPathGradientFocusScales Lib "gdiplus" (ByVal brush As Long, xScale As Single, yScale As Single) As GpStatus
Public Declare Function GdipSetPathGradientFocusScales Lib "gdiplus" (ByVal brush As Long, ByVal xScale As Single, ByVal yScale As Single) As GpStatus
Public Declare Function GdipCreatePath Lib "gdiplus" (ByVal brushmode As FillMode, Path As Long) As GpStatus
Public Declare Function GdipCreatePath2 Lib "gdiplus" (POINTS As POINTF, Types As Any, ByVal Count As Long, brushmode As FillMode, Path As Long) As GpStatus
Public Declare Function GdipCreatePath2I Lib "gdiplus" (POINTS As POINTL, Types As Any, ByVal Count As Long, brushmode As FillMode, Path As Long) As GpStatus
Public Declare Function GdipClonePath Lib "gdiplus" (ByVal Path As Long, clonePath As Long) As GpStatus
Public Declare Function GdipDeletePath Lib "gdiplus" (ByVal Path As Long) As GpStatus
Public Declare Function GdipResetPath Lib "gdiplus" (ByVal Path As Long) As GpStatus
Public Declare Function GdipGetPointCount Lib "gdiplus" (ByVal Path As Long, Count As Long) As GpStatus
Public Declare Function GdipGetPathTypes Lib "gdiplus" (ByVal Path As Long, Types As Any, ByVal Count As Long) As GpStatus
Public Declare Function GdipGetPathPoints Lib "gdiplus" (ByVal Path As Long, POINTS As POINTF, ByVal Count As Long) As GpStatus
Public Declare Function GdipGetPathPointsI Lib "gdiplus" (ByVal Path As Long, POINTS As POINTL, ByVal Count As Long) As GpStatus
Public Declare Function GdipGetPathFillMode Lib "gdiplus" (ByVal Path As Long, ByVal brushmode As FillMode) As GpStatus
Public Declare Function GdipSetPathFillMode Lib "gdiplus" (ByVal Path As Long, ByVal brushmode As FillMode) As GpStatus
Public Declare Function GdipGetPathData Lib "gdiplus" (ByVal Path As Long, pData As PathData) As GpStatus
Public Declare Function GdipStartPathFigure Lib "gdiplus" (ByVal Path As Long) As GpStatus
Public Declare Function GdipClosePathFigure Lib "gdiplus" (ByVal Path As Long) As GpStatus
Public Declare Function GdipClosePathFigures Lib "gdiplus" (ByVal Path As Long) As GpStatus
Public Declare Function GdipSetPathMarker Lib "gdiplus" (ByVal Path As Long) As GpStatus
Public Declare Function GdipClearPathMarkers Lib "gdiplus" (ByVal Path As Long) As GpStatus
Public Declare Function GdipReversePath Lib "gdiplus" (ByVal Path As Long) As GpStatus
Public Declare Function GdipGetPathLastPoint Lib "gdiplus" (ByVal Path As Long, lastPoint As POINTF) As GpStatus
Public Declare Function GdipAddPathLine Lib "gdiplus" (ByVal Path As Long, ByVal X1 As Single, ByVal Y1 As Single, ByVal X2 As Single, ByVal Y2 As Single) As GpStatus
Public Declare Function GdipAddPathLine2 Lib "gdiplus" (ByVal Path As Long, POINTS As POINTF, ByVal Count As Long) As GpStatus
Public Declare Function GdipAddPathArc Lib "gdiplus" (ByVal Path As Long, ByVal x As Single, ByVal y As Single, ByVal Width As Single, ByVal Height As Single, ByVal startAngle As Single, ByVal sweepAngle As Single) As GpStatus
Public Declare Function GdipAddPathBezier Lib "gdiplus" (ByVal Path As Long, ByVal X1 As Single, ByVal Y1 As Single, ByVal X2 As Single, ByVal Y2 As Single, ByVal X3 As Single, ByVal Y3 As Single, ByVal X4 As Single, ByVal Y4 As Single) As GpStatus
Public Declare Function GdipAddPathBeziers Lib "gdiplus" (ByVal Path As Long, POINTS As POINTF, ByVal Count As Long) As GpStatus
Public Declare Function GdipAddPathCurve Lib "gdiplus" (ByVal Path As Long, POINTS As POINTF, ByVal Count As Long) As GpStatus
Public Declare Function GdipAddPathCurve2 Lib "gdiplus" (ByVal Path As Long, POINTS As POINTF, ByVal Count As Long, ByVal tension As Single) As GpStatus
Public Declare Function GdipAddPathCurve3 Lib "gdiplus" (ByVal Path As Long, POINTS As POINTF, ByVal Count As Long, ByVal offset As Long, ByVal numberOfSegments As Long, ByVal tension As Single) As GpStatus
Public Declare Function GdipAddPathClosedCurve Lib "gdiplus" (ByVal Path As Long, POINTS As POINTF, ByVal Count As Long) As GpStatus
Public Declare Function GdipAddPathClosedCurve2 Lib "gdiplus" (ByVal Path As Long, POINTS As POINTF, ByVal Count As Long, ByVal tension As Single) As GpStatus
Public Declare Function GdipAddPathRectangle Lib "gdiplus" (ByVal Path As Long, ByVal x As Single, ByVal y As Single, ByVal Width As Single, ByVal Height As Single) As GpStatus
Public Declare Function GdipAddPathRectangles Lib "gdiplus" (ByVal Path As Long, rect As RECTF, ByVal Count As Long) As GpStatus
Public Declare Function GdipAddPathEllipse Lib "gdiplus" (ByVal Path As Long, ByVal x As Single, ByVal y As Single, ByVal Width As Single, ByVal Height As Single) As GpStatus
Public Declare Function GdipAddPathPie Lib "gdiplus" (ByVal Path As Long, ByVal x As Single, ByVal y As Single, ByVal Width As Single, ByVal Height As Single, ByVal startAngle As Single, ByVal sweepAngle As Single) As GpStatus
Public Declare Function GdipAddPathPolygon Lib "gdiplus" (ByVal Path As Long, POINTS As POINTF, ByVal Count As Long) As GpStatus
Public Declare Function GdipAddPathPath Lib "gdiplus" (ByVal Path As Long, ByVal addingPath As Long, ByVal bConnect As Long) As GpStatus
Public Declare Function GdipAddPathString Lib "gdiplus" (ByVal Path As Long, ByVal str As Long, ByVal Length As Long, ByVal family As Long, ByVal Style As Long, ByVal emSize As Single, layoutRect As RECTF, ByVal StringFormat As Long) As GpStatus
Public Declare Function GdipAddPathStringI Lib "gdiplus" (ByVal Path As Long, ByVal str As Long, ByVal Length As Long, ByVal family As Long, ByVal Style As Long, ByVal emSize As Single, layoutRect As RECTL, ByVal StringFormat As Long) As GpStatus
Public Declare Function GdipAddPathLineI Lib "gdiplus" (ByVal Path As Long, ByVal X1 As Long, ByVal Y1 As Long, ByVal X2 As Long, ByVal Y2 As Long) As GpStatus
Public Declare Function GdipAddPathLine2I Lib "gdiplus" (ByVal Path As Long, POINTS As POINTL, ByVal Count As Long) As GpStatus
Public Declare Function GdipAddPathArcI Lib "gdiplus" (ByVal Path As Long, ByVal x As Long, ByVal y As Long, ByVal Width As Long, ByVal Height As Long, ByVal startAngle As Single, ByVal sweepAngle As Single) As GpStatus
Public Declare Function GdipAddPathBezierI Lib "gdiplus" (ByVal Path As Long, ByVal X1 As Long, ByVal Y1 As Long, ByVal X2 As Long, ByVal Y2 As Long, ByVal X3 As Long, ByVal Y3 As Long, ByVal X4 As Long, ByVal Y4 As Long) As GpStatus
Public Declare Function GdipAddPathBeziersI Lib "gdiplus" (ByVal Path As Long, POINTS As POINTL, ByVal Count As Long) As GpStatus
Public Declare Function GdipAddPathCurveI Lib "gdiplus" (ByVal Path As Long, POINTS As POINTL, ByVal Count As Long) As GpStatus
Public Declare Function GdipAddPathCurve2I Lib "gdiplus" (ByVal Path As Long, POINTS As POINTL, ByVal Count As Long, ByVal tension As Long) As GpStatus
Public Declare Function GdipAddPathCurve3I Lib "gdiplus" (ByVal Path As Long, POINTS As POINTL, ByVal Count As Long, ByVal offset As Long, ByVal numberOfSegments As Long, ByVal tension As Single) As GpStatus
Public Declare Function GdipAddPathClosedCurveI Lib "gdiplus" (ByVal Path As Long, POINTS As POINTL, ByVal Count As Long) As GpStatus
Public Declare Function GdipAddPathClosedCurve2I Lib "gdiplus" (ByVal Path As Long, POINTS As POINTL, ByVal Count As Long, ByVal tension As Single) As GpStatus
Public Declare Function GdipAddPathRectangleI Lib "gdiplus" (ByVal Path As Long, ByVal x As Long, ByVal y As Long, ByVal Width As Long, ByVal Height As Long) As GpStatus
Public Declare Function GdipAddPathRectanglesI Lib "gdiplus" (ByVal Path As Long, RECTS As RECTL, ByVal Count As Long) As GpStatus
Public Declare Function GdipAddPathEllipseI Lib "gdiplus" (ByVal Path As Long, ByVal x As Long, ByVal y As Long, ByVal Width As Long, ByVal Height As Long) As GpStatus
Public Declare Function GdipAddPathPieI Lib "gdiplus" (ByVal Path As Long, ByVal x As Long, ByVal y As Long, ByVal Width As Long, ByVal Height As Long, ByVal startAngle As Single, ByVal sweepAngle As Single) As GpStatus
Public Declare Function GdipAddPathPolygonI Lib "gdiplus" (ByVal Path As Long, POINTS As POINTL, ByVal Count As Long) As GpStatus
Public Declare Function GdipFlattenPath Lib "gdiplus" (ByVal Path As Long, Optional ByVal matrix As Long = 0, Optional ByVal flatness As Single = 0.25) As GpStatus
Public Declare Function GdipWindingModeOutline Lib "gdiplus" (ByVal Path As Long, ByVal matrix As Long, ByVal flatness As Single) As GpStatus
Public Declare Function GdipWidenPath Lib "gdiplus" (ByVal NativePath As Long, ByVal pen As Long, ByVal matrix As Long, ByVal flatness As Single) As GpStatus
Public Declare Function GdipWarpPath Lib "gdiplus" (ByVal Path As Long, ByVal matrix As Long, POINTS As POINTF, ByVal Count As Long, ByVal SrcX As Single, ByVal SrcY As Single, ByVal srcwidth As Single, ByVal srcheight As Single, ByVal WarpMd As WarpMode, ByVal flatness As Single) As GpStatus
Public Declare Function GdipTransformPath Lib "gdiplus" (ByVal Path As Long, ByVal matrix As Long) As GpStatus
Public Declare Function GdipGetPathWorldBounds Lib "gdiplus" (ByVal Path As Long, bounds As RECTF, ByVal matrix As Long, ByVal pen As Long) As GpStatus
Public Declare Function GdipGetPathWorldBoundsI Lib "gdiplus" (ByVal Path As Long, bounds As RECTL, ByVal matrix As Long, ByVal pen As Long) As GpStatus
Public Declare Function GdipIsVisiblePathPoint Lib "gdiplus" (ByVal Path As Long, ByVal x As Single, ByVal y As Single, ByVal graphics As Long, Result As Long) As GpStatus
Public Declare Function GdipIsVisiblePathPointI Lib "gdiplus" (ByVal Path As Long, ByVal x As Long, ByVal y As Long, ByVal graphics As Long, Result As Long) As GpStatus
Public Declare Function GdipIsOutlineVisiblePathPoint Lib "gdiplus" (ByVal Path As Long, ByVal x As Single, ByVal y As Single, ByVal pen As Long, ByVal graphics As Long, Result As Long) As GpStatus
Public Declare Function GdipIsOutlineVisiblePathPointI Lib "gdiplus" (ByVal Path As Long, ByVal x As Long, ByVal y As Long, ByVal pen As Long, ByVal graphics As Long, Result As Long) As GpStatus
Public Declare Function GdipCreatePathIter Lib "gdiplus" (iterator As Long, ByVal Path As Long) As GpStatus
Public Declare Function GdipDeletePathIter Lib "gdiplus" (ByVal iterator As Long) As GpStatus
Public Declare Function GdipPathIterNextSubpath Lib "gdiplus" (ByVal iterator As Long, resultCount As Long, startIndex As Long, EndIndex As Long, isClosed As Long) As GpStatus
Public Declare Function GdipPathIterNextSubpathPath Lib "gdiplus" (ByVal iterator As Long, resultCount As Long, ByVal Path As Long, isClosed As Long) As GpStatus
Public Declare Function GdipPathIterNextPathType Lib "gdiplus" (ByVal iterator As Long, resultCount As Long, pathType As Any, startIndex As Long, EndIndex As Long) As GpStatus
Public Declare Function GdipPathIterNextMarker Lib "gdiplus" (ByVal iterator As Long, resultCount As Long, startIndex As Long, EndIndex As Long) As GpStatus
Public Declare Function GdipPathIterNextMarkerPath Lib "gdiplus" (ByVal iterator As Long, resultCount As Long, ByVal Path As Long) As GpStatus
Public Declare Function GdipPathIterGetCount Lib "gdiplus" (ByVal iterator As Long, Count As Long) As GpStatus
Public Declare Function GdipPathIterGetSubpathCount Lib "gdiplus" (ByVal iterator As Long, Count As Long) As GpStatus
Public Declare Function GdipPathIterIsValid Lib "gdiplus" (ByVal iterator As Long, valid As Long) As GpStatus
Public Declare Function GdipPathIterHasCurve Lib "gdiplus" (ByVal iterator As Long, hasCurve As Long) As GpStatus
Public Declare Function GdipPathIterRewind Lib "gdiplus" (ByVal iterator As Long) As GpStatus
Public Declare Function GdipPathIterEnumerate Lib "gdiplus" (ByVal iterator As Long, resultCount As Long, POINTS As POINTF, Types As Any, ByVal Count As Long) As GpStatus
Public Declare Function GdipPathIterCopyData Lib "gdiplus" (ByVal iterator As Long, resultCount As Long, POINTS As POINTF, Types As Any, ByVal startIndex As Long, ByVal EndIndex As Long) As GpStatus
Public Declare Function GdipCreateMatrix Lib "gdiplus" (matrix As Long) As GpStatus
Public Declare Function GdipCreateMatrix2 Lib "gdiplus" (ByVal m11 As Single, ByVal m12 As Single, ByVal m21 As Single, ByVal m22 As Single, ByVal dx As Single, ByVal dy As Single, matrix As Long) As GpStatus
Public Declare Function GdipCreateMatrix3 Lib "gdiplus" (rect As RECTF, dstplg As POINTF, matrix As Long) As GpStatus
Public Declare Function GdipCreateMatrix3I Lib "gdiplus" (rect As RECTL, dstplg As POINTL, matrix As Long) As GpStatus
Public Declare Function GdipCloneMatrix Lib "gdiplus" (ByVal matrix As Long, cloneMatrix As Long) As GpStatus
Public Declare Function GdipDeleteMatrix Lib "gdiplus" (ByVal matrix As Long) As GpStatus
Public Declare Function GdipSetMatrixElements Lib "gdiplus" (ByVal matrix As Long, ByVal m11 As Single, ByVal m12 As Single, ByVal m21 As Single, ByVal m22 As Single, ByVal dx As Single, ByVal dy As Single) As GpStatus
Public Declare Function GdipMultiplyMatrix Lib "gdiplus" (ByVal matrix As Long, ByVal matrix2 As Long, ByVal order As MatrixOrder) As GpStatus
Public Declare Function GdipTranslateMatrix Lib "gdiplus" (ByVal matrix As Long, ByVal offsetX As Single, ByVal offsetY As Single, ByVal order As MatrixOrder) As GpStatus
Public Declare Function GdipScaleMatrix Lib "gdiplus" (ByVal matrix As Long, ByVal scaleX As Single, ByVal scaleY As Single, ByVal order As MatrixOrder) As GpStatus
Public Declare Function GdipRotateMatrix Lib "gdiplus" (ByVal matrix As Long, ByVal angle As Single, ByVal order As MatrixOrder) As GpStatus
Public Declare Function GdipShearMatrix Lib "gdiplus" (ByVal matrix As Long, ByVal shearX As Single, ByVal shearY As Single, ByVal order As MatrixOrder) As GpStatus
Public Declare Function GdipInvertMatrix Lib "gdiplus" (ByVal matrix As Long) As GpStatus
Public Declare Function GdipTransformMatrixPoints Lib "gdiplus" (ByVal matrix As Long, pts As POINTF, ByVal Count As Long) As GpStatus
Public Declare Function GdipTransformMatrixPointsI Lib "gdiplus" (ByVal matrix As Long, pts As POINTL, ByVal Count As Long) As GpStatus
Public Declare Function GdipVectorTransformMatrixPoints Lib "gdiplus" (ByVal matrix As Long, pts As POINTF, ByVal Count As Long) As GpStatus
Public Declare Function GdipVectorTransformMatrixPointsI Lib "gdiplus" (ByVal matrix As Long, pts As POINTL, ByVal Count As Long) As GpStatus
Public Declare Function GdipGetMatrixElements Lib "gdiplus" (ByVal matrix As Long, matrixOut As Single) As GpStatus
Public Declare Function GdipIsMatrixInvertible Lib "gdiplus" (ByVal matrix As Long, Result As Long) As GpStatus
Public Declare Function GdipIsMatrixIdentity Lib "gdiplus" (ByVal matrix As Long, Result As Long) As GpStatus
Public Declare Function GdipIsMatrixEqual Lib "gdiplus" (ByVal matrix As Long, ByVal matrix2 As Long, Result As Long) As GpStatus
Public Declare Function GdipCreateRegion Lib "gdiplus" (region As Long) As GpStatus
Public Declare Function GdipCreateRegionRect Lib "gdiplus" (rect As RECTF, region As Long) As GpStatus
Public Declare Function GdipCreateRegionRectI Lib "gdiplus" (rect As RECTL, region As Long) As GpStatus
Public Declare Function GdipCreateRegionPath Lib "gdiplus" (ByVal Path As Long, region As Long) As GpStatus
Public Declare Function GdipCreateRegionRgnData Lib "gdiplus" (regionData As Any, ByVal Size As Long, region As Long) As GpStatus
Public Declare Function GdipCreateRegionHrgn Lib "gdiplus" (ByVal hRgn As Long, region As Long) As GpStatus
Public Declare Function GdipCloneRegion Lib "gdiplus" (ByVal region As Long, cloneRegion As Long) As GpStatus
Public Declare Function GdipDeleteRegion Lib "gdiplus" (ByVal region As Long) As GpStatus
Public Declare Function GdipSetInfinite Lib "gdiplus" (ByVal region As Long) As GpStatus
Public Declare Function GdipSetEmpty Lib "gdiplus" (ByVal region As Long) As GpStatus
Public Declare Function GdipCombineRegionRect Lib "gdiplus" (ByVal region As Long, rect As RECTF, ByVal CombineMd As CombineMode) As GpStatus
Public Declare Function GdipCombineRegionRectI Lib "gdiplus" (ByVal region As Long, rect As RECTL, ByVal CombineMd As CombineMode) As GpStatus
Public Declare Function GdipCombineRegionPath Lib "gdiplus" (ByVal region As Long, ByVal Path As Long, ByVal CombineMd As CombineMode) As GpStatus
Public Declare Function GdipCombineRegionRegion Lib "gdiplus" (ByVal region As Long, ByVal region2 As Long, ByVal CombineMd As CombineMode) As GpStatus
Public Declare Function GdipTranslateRegion Lib "gdiplus" (ByVal region As Long, ByVal dx As Single, ByVal dy As Single) As GpStatus
Public Declare Function GdipTranslateRegionI Lib "gdiplus" (ByVal region As Long, ByVal dx As Long, ByVal dy As Long) As GpStatus
Public Declare Function GdipTransformRegion Lib "gdiplus" (ByVal region As Long, ByVal matrix As Long) As GpStatus
Public Declare Function GdipGetRegionBounds Lib "gdiplus" (ByVal region As Long, ByVal graphics As Long, rect As RECTF) As GpStatus
Public Declare Function GdipGetRegionBoundsI Lib "gdiplus" (ByVal region As Long, ByVal graphics As Long, rect As RECTL) As GpStatus
Public Declare Function GdipGetRegionHRgn Lib "gdiplus" (ByVal region As Long, ByVal graphics As Long, hRgn As Long) As GpStatus
Public Declare Function GdipIsEmptyRegion Lib "gdiplus" (ByVal region As Long, ByVal graphics As Long, Result As Long) As GpStatus
Public Declare Function GdipIsInfiniteRegion Lib "gdiplus" (ByVal region As Long, ByVal graphics As Long, Result As Long) As GpStatus
Public Declare Function GdipIsEqualRegion Lib "gdiplus" (ByVal region As Long, ByVal region2 As Long, ByVal graphics As Long, Result As Long) As GpStatus
Public Declare Function GdipGetRegionDataSize Lib "gdiplus" (ByVal region As Long, BufferSize As Long) As GpStatus
Public Declare Function GdipGetRegionData Lib "gdiplus" (ByVal region As Long, Buffer As Any, ByVal BufferSize As Long, sizeFilled As Long) As GpStatus
Public Declare Function GdipIsVisibleRegionPoint Lib "gdiplus" (ByVal region As Long, ByVal x As Single, ByVal y As Single, ByVal graphics As Long, Result As Long) As GpStatus
Public Declare Function GdipIsVisibleRegionPointI Lib "gdiplus" (ByVal region As Long, ByVal x As Long, ByVal y As Long, ByVal graphics As Long, Result As Long) As GpStatus
Public Declare Function GdipIsVisibleRegionRect Lib "gdiplus" (ByVal region As Long, ByVal x As Single, ByVal y As Single, ByVal Width As Single, ByVal Height As Single, ByVal graphics As Long, Result As Long) As GpStatus
Public Declare Function GdipIsVisibleRegionRectI Lib "gdiplus" (ByVal region As Long, ByVal x As Long, ByVal y As Long, ByVal Width As Long, ByVal Height As Long, ByVal graphics As Long, Result As Long) As GpStatus
Public Declare Function GdipGetRegionScansCount Lib "gdiplus" (ByVal region As Long, Ucount As Long, ByVal matrix As Long) As GpStatus
Public Declare Function GdipGetRegionScans Lib "gdiplus" (ByVal region As Long, RECTS As RECTF, Count As Long, ByVal matrix As Long) As GpStatus
Public Declare Function GdipGetRegionScansI Lib "gdiplus" (ByVal region As Long, RECTS As RECTL, Count As Long, ByVal matrix As Long) As GpStatus
Public Declare Function GdipCreateImageAttributes Lib "gdiplus" (imageattr As Long) As GpStatus
Public Declare Function GdipCloneImageAttributes Lib "gdiplus" (ByVal imageattr As Long, cloneImageattr As Long) As GpStatus
Public Declare Function GdipDisposeImageAttributes Lib "gdiplus" (ByVal imageattr As Long) As GpStatus
Public Declare Function GdipSetImageAttributesToIdentity Lib "gdiplus" (ByVal imageattr As Long, ByVal ClrAdjType As ColorAdjustType) As GpStatus
Public Declare Function GdipResetImageAttributes Lib "gdiplus" (ByVal imageattr As Long, ByVal ClrAdjType As ColorAdjustType) As GpStatus
Public Declare Function GdipSetImageAttributesColorMatrix Lib "gdiplus" (ByVal imageattr As Long, ByVal ClrAdjType As ColorAdjustType, ByVal enableFlag As Long, colourMatrix As Any, grayMatrix As Any, ByVal flags As ColorMatrixFlags) As GpStatus
Public Declare Function GdipSetImageAttributesThreshold Lib "gdiplus" (ByVal imageattr As Long, ByVal ClrAdjType As ColorAdjustType, ByVal enableFlag As Long, ByVal threshold As Single) As GpStatus
Public Declare Function GdipSetImageAttributesGamma Lib "gdiplus" (ByVal imageattr As Long, ByVal ClrAdjType As ColorAdjustType, ByVal enableFlag As Long, ByVal gamma As Single) As GpStatus
Public Declare Function GdipSetImageAttributesNoOp Lib "gdiplus" (ByVal imageattr As Long, ByVal ClrAdjType As ColorAdjustType, ByVal enableFlag As Long) As GpStatus
Public Declare Function GdipSetImageAttributesColorKeys Lib "gdiplus" (ByVal imageattr As Long, ByVal ClrAdjType As ColorAdjustType, ByVal enableFlag As Long, ByVal colorLow As Long, ByVal colorHigh As Long) As GpStatus
Public Declare Function GdipSetImageAttributesOutputChannel Lib "gdiplus" (ByVal imageattr As Long, ByVal ClrAdjstType As ColorAdjustType, ByVal enableFlag As Long, ByVal channelFlags As ColorChannelFlags) As GpStatus
Public Declare Function GdipSetImageAttributesOutputChannelColorProfile Lib "gdiplus" (ByVal imageattr As Long, ByVal ClrAdjType As ColorAdjustType, ByVal enableFlag As Long, ByVal colorProfileFilename As Long) As GpStatus
Public Declare Function GdipSetImageAttributesRemapTable Lib "gdiplus" (ByVal imageattr As Long, ByVal ClrAdjType As ColorAdjustType, ByVal enableFlag As Long, ByVal mapSize As Long, map As Any) As GpStatus
Public Declare Function GdipSetImageAttributesWrapMode Lib "gdiplus" (ByVal imageattr As Long, ByVal wrap As WrapMode, ByVal argb As Long, ByVal bClamp As Long) As GpStatus
Public Declare Function GdipSetImageAttributesICMMode Lib "gdiplus" (ByVal imageattr As Long, ByVal bOn As Long) As GpStatus
Public Declare Function GdipGetImageAttributesAdjustedPalette Lib "gdiplus" (ByVal imageattr As Long, colorPal As ColorPalette, ByVal ClrAdjType As ColorAdjustType) As GpStatus
Public Declare Function GdipCreateFontFamilyFromName Lib "gdiplus" (ByVal name As Long, ByVal fontCollection As Long, fontFamily As Long) As GpStatus
Public Declare Function GdipDeleteFontFamily Lib "gdiplus" (ByVal fontFamily As Long) As GpStatus
Public Declare Function GdipCloneFontFamily Lib "gdiplus" (ByVal fontFamily As Long, clonedFontFamily As Long) As GpStatus
Public Declare Function GdipGetGenericFontFamilySansSerif Lib "gdiplus" (nativeFamily As Long) As GpStatus
Public Declare Function GdipGetGenericFontFamilySerif Lib "gdiplus" (nativeFamily As Long) As GpStatus
Public Declare Function GdipGetGenericFontFamilyMonospace Lib "gdiplus" (nativeFamily As Long) As GpStatus
Public Declare Function GdipGetFamilyName Lib "gdiplus" (ByVal family As Long, ByVal name As Long, ByVal language As Integer) As GpStatus
Public Declare Function GdipIsStyleAvailable Lib "gdiplus" (ByVal family As Long, ByVal Style As Long, IsStyleAvailable As Long) As GpStatus
Public Declare Function GdipFontCollectionEnumerable Lib "gdiplus" (ByVal fontCollection As Long, ByVal graphics As Long, numFound As Long) As GpStatus
Public Declare Function GdipFontCollectionEnumerate Lib "gdiplus" (ByVal fontCollection As Long, ByVal numSought As Long, gpFamilies As Long, ByVal numFound As Long, ByVal graphics As Long) As GpStatus
Public Declare Function GdipGetEmHeight Lib "gdiplus" (ByVal family As Long, ByVal Style As Long, EmHeight As Integer) As GpStatus
Public Declare Function GdipGetCellAscent Lib "gdiplus" (ByVal family As Long, ByVal Style As Long, CellAscent As Integer) As GpStatus
Public Declare Function GdipGetCellDescent Lib "gdiplus" (ByVal family As Long, ByVal Style As Long, CellDescent As Integer) As GpStatus
Public Declare Function GdipGetLineSpacing Lib "gdiplus" (ByVal family As Long, ByVal Style As Long, LineSpacing As Integer) As GpStatus
Public Declare Function GdipCreateFontFromDC Lib "gdiplus" (ByVal hDc As Long, createdfont As Long) As GpStatus
Public Declare Function GdipCreateFontFromLogfontA Lib "gdiplus" (ByVal hDc As Long, LOGFONT As LOGFONTA, createdfont As Long) As GpStatus
Public Declare Function GdipCreateFontFromLogfontW Lib "gdiplus" (ByVal hDc As Long, LOGFONT As LOGFONTW, createdfont As Long) As GpStatus
Public Declare Function GdipCreateFont Lib "gdiplus" (ByVal fontFamily As Long, ByVal emSize As Single, ByVal Style As Long, ByVal unit As GpUnit, createdfont As Long) As GpStatus
Public Declare Function GdipCloneFont Lib "gdiplus" (ByVal CurFont As Long, cloneFont As Long) As GpStatus
Public Declare Function GdipDeleteFont Lib "gdiplus" (ByVal CurFont As Long) As GpStatus
Public Declare Function GdipGetFamily Lib "gdiplus" (ByVal CurFont As Long, family As Long) As GpStatus
Public Declare Function GdipGetFontStyle Lib "gdiplus" (ByVal CurFont As Long, Style As Long) As GpStatus
Public Declare Function GdipGetFontSize Lib "gdiplus" (ByVal CurFont As Long, Size As Single) As GpStatus
Public Declare Function GdipGetFontUnit Lib "gdiplus" (ByVal CurFont As Long, unit As GpUnit) As GpStatus
Public Declare Function GdipGetFontHeight Lib "gdiplus" (ByVal CurFont As Long, ByVal graphics As Long, Height As Single) As GpStatus
Public Declare Function GdipGetFontHeightGivenDPI Lib "gdiplus" (ByVal CurFont As Long, ByVal dpi As Single, Height As Single) As GpStatus
Public Declare Function GdipGetLogFontA Lib "gdiplus" (ByVal CurFont As Long, ByVal graphics As Long, LOGFONT As LOGFONTA) As GpStatus
Public Declare Function GdipGetLogFontW Lib "gdiplus" (ByVal CurFont As Long, ByVal graphics As Long, LOGFONT As Any) As GpStatus
Public Declare Function GdipNewInstalledFontCollection Lib "gdiplus" (fontCollection As Long) As GpStatus
Public Declare Function GdipNewPrivateFontCollection Lib "gdiplus" (fontCollection As Long) As GpStatus
Public Declare Function GdipDeletePrivateFontCollection Lib "gdiplus" (fontCollection As Long) As GpStatus
Public Declare Function GdipGetFontCollectionFamilyCount Lib "gdiplus" (ByVal fontCollection As Long, numFound As Long) As GpStatus
Public Declare Function GdipGetFontCollectionFamilyList Lib "gdiplus" (ByVal fontCollection As Long, ByVal numSought As Long, gpFamilies As Long, numFound As Long) As GpStatus
Public Declare Function GdipPrivateAddFontFile Lib "gdiplus" (ByVal fontCollection As Long, ByVal filename As Long) As GpStatus
Public Declare Function GdipPrivateAddMemoryFont Lib "gdiplus" (ByVal fontCollection As Long, ByVal memory As Long, ByVal Length As Long) As GpStatus
Public Declare Function GdipDrawString Lib "gdiplus" (ByVal graphics As Long, ByVal str As Long, ByVal Length As Long, ByVal thefont As Long, layoutRect As RECTF, ByVal StringFormat As Long, ByVal brush As Long) As GpStatus
Public Declare Function GdipMeasureString Lib "gdiplus" (ByVal graphics As Long, ByVal str As Long, ByVal Length As Long, ByVal thefont As Long, layoutRect As RECTF, ByVal StringFormat As Long, boundingBox As RECTF, codepointsFitted As Long, linesFilled As Long) As GpStatus
Public Declare Function GdipMeasureCharacterRanges Lib "gdiplus" (ByVal graphics As Long, ByVal str As Long, ByVal Length As Long, ByVal thefont As Long, layoutRect As RECTF, ByVal StringFormat As Long, ByVal regionCount As Long, regions As Long) As GpStatus
Public Declare Function GdipDrawDriverString Lib "gdiplus" (ByVal graphics As Long, ByVal str As Long, ByVal Length As Long, ByVal thefont As Long, ByVal brush As Long, positions As POINTF, ByVal flags As Long, ByVal matrix As Long) As GpStatus
Public Declare Function GdipMeasureDriverString Lib "gdiplus" (ByVal graphics As Long, ByVal str As Long, ByVal Length As Long, ByVal thefont As Long, positions As POINTF, ByVal flags As Long, ByVal matrix As Long, boundingBox As RECTF) As GpStatus
Public Declare Function GdipCreateStringFormat Lib "gdiplus" (ByVal formatAttributes As Long, ByVal language As Integer, StringFormat As Long) As GpStatus
Public Declare Function GdipStringFormatGetGenericDefault Lib "gdiplus" (StringFormat As Long) As GpStatus
Public Declare Function GdipStringFormatGetGenericTypographic Lib "gdiplus" (StringFormat As Long) As GpStatus
Public Declare Function GdipDeleteStringFormat Lib "gdiplus" (ByVal StringFormat As Long) As GpStatus
Public Declare Function GdipCloneStringFormat Lib "gdiplus" (ByVal StringFormat As Long, newFormat As Long) As GpStatus
Public Declare Function GdipSetStringFormatFlags Lib "gdiplus" (ByVal StringFormat As Long, ByVal flags As StringFormatFlags) As GpStatus
Public Declare Function GdipGetStringFormatFlags Lib "gdiplus" (ByVal StringFormat As Long, flags As Long) As GpStatus
Public Declare Function GdipSetStringFormatAlign Lib "gdiplus" (ByVal StringFormat As Long, ByVal Align As StringAlignment) As GpStatus
Public Declare Function GdipGetStringFormatAlign Lib "gdiplus" (ByVal StringFormat As Long, Align As StringAlignment) As GpStatus
Public Declare Function GdipSetStringFormatLineAlign Lib "gdiplus" (ByVal StringFormat As Long, ByVal Align As StringAlignment) As GpStatus
Public Declare Function GdipGetStringFormatLineAlign Lib "gdiplus" (ByVal StringFormat As Long, Align As StringAlignment) As GpStatus
Public Declare Function GdipSetStringFormatTrimming Lib "gdiplus" (ByVal StringFormat As Long, ByVal trimming As StringTrimming) As GpStatus
Public Declare Function GdipGetStringFormatTrimming Lib "gdiplus" (ByVal StringFormat As Long, trimming As Long) As GpStatus
Public Declare Function GdipSetStringFormatHotkeyPrefix Lib "gdiplus" (ByVal StringFormat As Long, ByVal hkPrefix As HotkeyPrefix) As GpStatus
Public Declare Function GdipGetStringFormatHotkeyPrefix Lib "gdiplus" (ByVal StringFormat As Long, hkPrefix As HotkeyPrefix) As GpStatus
Public Declare Function GdipSetStringFormatTabStops Lib "gdiplus" (ByVal StringFormat As Long, ByVal firstTabOffset As Single, ByVal Count As Long, tabStops As Single) As GpStatus
Public Declare Function GdipGetStringFormatTabStops Lib "gdiplus" (ByVal StringFormat As Long, ByVal Count As Long, firstTabOffset As Single, tabStops As Single) As GpStatus
Public Declare Function GdipGetStringFormatTabStopCount Lib "gdiplus" (ByVal StringFormat As Long, Count As Long) As GpStatus
Public Declare Function GdipSetStringFormatDigitSubstitution Lib "gdiplus" (ByVal StringFormat As Long, ByVal language As Integer, ByVal substitute As StringDigitSubstitute) As GpStatus
Public Declare Function GdipGetStringFormatDigitSubstitution Lib "gdiplus" (ByVal StringFormat As Long, language As Integer, substitute As StringDigitSubstitute) As GpStatus
Public Declare Function GdipGetStringFormatMeasurableCharacterRangeCount Lib "gdiplus" (ByVal StringFormat As Long, Count As Long) As GpStatus
Public Declare Function GdipSetStringFormatMeasurableCharacterRanges Lib "gdiplus" (ByVal StringFormat As Long, ByVal rangeCount As Long, ranges As CharacterRange) As GpStatus
 
Public Declare Function TransparentBlt Lib "msimg32.dll" (ByVal hdcDest As Long, ByVal nXOriginDest As Long, ByVal nYOriginDest As Long, ByVal nWidthDest As Long, ByVal nHeightDest As Long, ByVal hdcSrc As Long, ByVal nXOriginSrc As Long, ByVal nYOriginSrc As Long, ByVal nWidthSrc As Long, ByVal nHeightSrc As Long, ByVal crTransparent As Long) As Boolean
'===================================================================================
'  ����ô���õĶ���
'===================================================================================
 
'=================================
'== Structures                  ==
'=================================
 
'=================================
'Log Font Structure
Public Type LOGFONTA
   lfHeight As Long
   lfWidth As Long
   lfEscapement As Long
   lfOrientation As Long
   lfWeight As Long
   lfItalic As Byte
   lfUnderline As Byte
   lfStrikeOut As Byte
   lfCharSet As Byte
   lfOutPrecision As Byte
   lfClipPrecision As Byte
   lfQuality As Byte
   lfPitchAndFamily As Byte
   lfFaceName(32) As Byte
End Type
 
Public Type LOGFONTW
   lfHeight As Long
   lfWidth As Long
   lfEscapement As Long
   lfOrientation As Long
   lfWeight As Long
   lfItalic As Byte
   lfUnderline As Byte
   lfStrikeOut As Byte
   lfCharSet As Byte
   lfOutPrecision As Byte
   lfClipPrecision As Byte
   lfQuality As Byte
   lfPitchAndFamily As Byte
   lfFaceName(32) As Byte
End Type
 
'=================================
'Image
Public Type ImageCodecInfo
   ClassID As Clsid
   FormatID As Clsid
   CodecName As Long
   DllName As Long
   FormatDescription As Long
   FilenameExtension As Long
   MimeType As Long
   flags As ImageCodecFlags
   Version As Long
   SigCount As Long
   SigSize As Long
   SigPattern As Long
   SigMask As Long
End Type
 
'=================================
'Colors
Public Type ColorPalette
   flags As PaletteFlags
   Count As Long
   Entries(0 To 255) As Long
End Type
 
'=================================
'Meta File
Public Type PWMFRect16
   Left As Integer
   Top As Integer
   Right As Integer
   Bottom As Integer
End Type
 
Public Type WmfPlaceableFileHeader
   key As Long                        ' GDIP_WMF_PLACEABLEKEY
   hMF As Integer                     ' Metafile HANDLE number (always 0)
   boundingBox As PWMFRect16          ' Coordinates in metafile units
   inch As Integer                    ' Number of metafile units per inch
   Reserved As Long                   ' Reserved (always 0)
   checksum As Integer                ' Checksum value for previous 10 WORDs
End Type
 
Public Type ENHMETAHEADER3
   iType As Long               ' Record type EMR_HEADER
   nSize As Long               ' Record size in bytes.  This may be greater
                               ' than the sizeof(ENHMETAHEADER).
   rclBounds As RECTL        ' Inclusive-inclusive bounds in device units
   rclFrame As RECTL         ' Inclusive-inclusive Picture Frame .01mm unit
   dSignature As Long          ' Signature.  Must be ENHMETA_SIGNATURE.
   nVersion As Long            ' Version number
   nBytes As Long              ' Size of the metafile in bytes
   nRecords As Long            ' Number of records in the metafile
   nHandles As Integer         ' Number of handles in the handle table
                               ' Handle index zero is reserved.
   sReserved As Integer        ' Reserved.  Must be zero.
   nDescription As Long        ' Number of chars in the unicode desc string
                               ' This is 0 if there is no description string
   offDescription As Long      ' Offset to the metafile description record.
                               ' This is 0 if there is no description string
   nPalEntries As Long         ' Number of entries in the metafile palette.
   szlDevice As SIZEL           ' Size of the reference device in pels
   szlMillimeters As SIZEL      ' Size of the reference device in millimeters
End Type
 
Public Type METAHEADER
   mtType As Integer
   mtHeaderSize As Integer
   mtVersion As Integer
   mtSize As Long
   mtNoObjects As Integer
   mtMaxRecord As Long
   mtNoParameters As Integer
End Type
 
Public Type METAFILEHEADER
   mType As MetafileType
   Size As Long                ' Size of the metafile (in bytes)
   Version As Long             ' EMF+, EMF, or WMF version
   EmfPlusFlags As Long
   DpiX As Single
   DpiY As Single
   x As Long                   ' Bounds in device units
   y As Long
   Width As Long
   Height As Long
 
   EmfHeader As ENHMETAHEADER3 ' NOTE: You'll have to use CopyMemory to view the METAHEADER type
   EmfPlusHeaderSize As Long   ' size of the EMF+ header in file
   LogicalDpiX As Long         ' Logical Dpi of reference Hdc
   LogicalDpiY As Long         ' usually valid only for EMF+
End Type
 
'=================================
'Other
Public Type PropertyItem
   propId As Long              ' ID of this property
   Length As Long              ' Length of the property value, in bytes
   Type As Integer             ' Type of the value, as one of TAG_TYPE_XXX
                               ' defined above
   Value As Long               ' property value
End Type
 
Public Type CharacterRange
   First As Long
   Length As Long
End Type
 
'=================================
'== Enums                       ==
'=================================
 
'=================================
'Image
Public Enum GpImageSaveFormat
    GpSaveBMP = 0
    GpSaveJPEG = 1
    GpSaveGIF = 2
    GpSavePNG = 3
    GpSaveTIFF = 4
End Enum
 
Public Enum GpImageFormatIdentifiers
    GpImageFormatUndefined = 0
    GpImageFormatMemoryBMP = 1
    GpImageFormatBMP = 2
    GpImageFormatEMF = 3
    GpImageFormatWMF = 4
    GpImageFormatJPEG = 5
    GpImageFormatPNG = 6
    GpImageFormatGIF = 7
    GpImageFormatTIFF = 8
    GpImageFormatEXIF = 9
    GpImageFormatIcon = 10
End Enum
 
Public Enum Image_Type
    ImageTypeUnknown = 0
    ImageTypeBitmap = 1
    ImageTypeMetafile = 2
End Enum
 
Public Enum Image_Property_Types
    PropertyTagTypeByte = 1
    PropertyTagTypeASCII = 2
    PropertyTagTypeShort = 3
    PropertyTagTypeLong = 4
    PropertyTagTypeRational = 5
    PropertyTagTypeUndefined = 7
    PropertyTagTypeSLONG = 9
    PropertyTagTypeSRational = 10
End Enum
 
Public Enum ImageCodecFlags
    ImageCodecFlagsEncoder = &H1
    ImageCodecFlagsDecoder = &H2
    ImageCodecFlagsSupportBitmap = &H4
    ImageCodecFlagsSupportVector = &H8
    ImageCodecFlagsSeekableEncode = &H10
    ImageCodecFlagsBlockingDecode = &H20
     
    ImageCodecFlagsBuiltin = &H10000
    ImageCodecFlagsSystem = &H20000
    ImageCodecFlagsUser = &H40000
End Enum
 
Public Enum Image_Property_ID_Tags
    PropertyTagExifIFD = &H8769
    PropertyTagGpsIFD = &H8825
 
    PropertyTagNewSubfileType = &HFE
    PropertyTagSubfileType = &HFF
    PropertyTagImageWidth = &H100
    PropertyTagImageHeight = &H101
    PropertyTagBitsPerSample = &H102
    PropertyTagCompression = &H103
    PropertyTagPhotometricInterp = &H106
    PropertyTagThreshHolding = &H107
    PropertyTagCellWidth = &H108
    PropertyTagCellHeight = &H109
    PropertyTagFillOrder = &H10A
    PropertyTagDocumentName = &H10D
    PropertyTagImageDescription = &H10E
    PropertyTagEquipMake = &H10F
    PropertyTagEquipModel = &H110
    PropertyTagStripOffsets = &H111
    PropertyTagOrientation = &H112
    PropertyTagSamplesPerPixel = &H115
    PropertyTagRowsPerStrip = &H116
    PropertyTagStripBytesCount = &H117
    PropertyTagMinSampleValue = &H118
    PropertyTagMaxSampleValue = &H119
    PropertyTagXResolution = &H11A            ' Image resolution in width direction
    PropertyTagYResolution = &H11B            ' Image resolution in height direction
    PropertyTagPlanarConfig = &H11C           ' Image data arrangement
    PropertyTagPageName = &H11D
    PropertyTagXPosition = &H11E
    PropertyTagYPosition = &H11F
    PropertyTagFreeOffset = &H120
    PropertyTagFreeByteCounts = &H121
    PropertyTagGrayResponseUnit = &H122
    PropertyTagGrayResponseCurve = &H123
    PropertyTagT4Option = &H124
    PropertyTagT6Option = &H125
    PropertyTagResolutionUnit = &H128         ' Unit of X and Y resolution
    PropertyTagPageNumber = &H129
    PropertyTagTransferFuncition = &H12D
    PropertyTagSoftwareUsed = &H131
    PropertyTagDateTime = &H132
    PropertyTagArtist = &H13B
    PropertyTagHostComputer = &H13C
    PropertyTagPredictor = &H13D
    PropertyTagWhitePoint = &H13E
    PropertyTagPrimaryChromaticities = &H13F
    PropertyTagColorMap = &H140
    PropertyTagHalftoneHints = &H141
    PropertyTagTileWidth = &H142
    PropertyTagTileLength = &H143
    PropertyTagTileOffset = &H144
    PropertyTagTileByteCounts = &H145
    PropertyTagInkSet = &H14C
    PropertyTagInkNames = &H14D
    PropertyTagNumberOfInks = &H14E
    PropertyTagDotRange = &H150
    PropertyTagTargetPrinter = &H151
    PropertyTagExtraSamples = &H152
    PropertyTagSampleFormat = &H153
    PropertyTagSMinSampleValue = &H154
    PropertyTagSMaxSampleValue = &H155
    PropertyTagTransferRange = &H156
 
    PropertyTagJPEGProc = &H200
    PropertyTagJPEGInterFormat = &H201
    PropertyTagJPEGInterLength = &H202
    PropertyTagJPEGRestartInterval = &H203
    PropertyTagJPEGLosslessPredictors = &H205
    PropertyTagJPEGPointTransforms = &H206
    PropertyTagJPEGQTables = &H207
    PropertyTagJPEGDCTables = &H208
    PropertyTagJPEGACTables = &H209
 
    PropertyTagYCbCrCoefficients = &H211
    PropertyTagYCbCrSubsampling = &H212
    PropertyTagYCbCrPositioning = &H213
    PropertyTagREFBlackWhite = &H214
 
    PropertyTagICCProfile = &H8773            ' This TAG is defined by ICC
                                                ' for embedded ICC in TIFF
    PropertyTagGamma = &H301
    PropertyTagICCProfileDescriptor = &H302
    PropertyTagSRGBRenderingIntent = &H303
 
    PropertyTagImageTitle = &H320
    PropertyTagCopyright = &H8298
 
    PropertyTagResolutionXUnit = &H5001
    PropertyTagResolutionYUnit = &H5002
    PropertyTagResolutionXLengthUnit = &H5003
    PropertyTagResolutionYLengthUnit = &H5004
    PropertyTagPrintFlags = &H5005
    PropertyTagPrintFlagsVersion = &H5006
    PropertyTagPrintFlagsCrop = &H5007
    PropertyTagPrintFlagsBleedWidth = &H5008
    PropertyTagPrintFlagsBleedWidthScale = &H5009
    PropertyTagHalftoneLPI = &H500A
    PropertyTagHalftoneLPIUnit = &H500B
    PropertyTagHalftoneDegree = &H500C
    PropertyTagHalftoneShape = &H500D
    PropertyTagHalftoneMisc = &H500E
    PropertyTagHalftoneScreen = &H500F
    PropertyTagJPEGQuality = &H5010
    PropertyTagGridSize = &H5011
    PropertyTagThumbnailFormat = &H5012            ' 1 = JPEG, 0 = RAW RGB
    PropertyTagThumbnailWidth = &H5013
    PropertyTagThumbnailHeight = &H5014
    PropertyTagThumbnailColorDepth = &H5015
    PropertyTagThumbnailPlanes = &H5016
    PropertyTagThumbnailRawBytes = &H5017
    PropertyTagThumbnailSize = &H5018
    PropertyTagThumbnailCompressedSize = &H5019
    PropertyTagColorTransferFunction = &H501A
    PropertyTagThumbnailData = &H501B
    PropertyTagThumbnailImageWidth = &H5020        ' Thumbnail width
    PropertyTagThumbnailImageHeight = &H5021       ' Thumbnail height
    PropertyTagThumbnailBitsPerSample = &H5022     ' Number of bits per
                                                     ' component
    PropertyTagThumbnailCompression = &H5023       ' Compression Scheme
    PropertyTagThumbnailPhotometricInterp = &H5024 ' Pixel composition
    PropertyTagThumbnailImageDescription = &H5025  ' Image Tile
    PropertyTagThumbnailEquipMake = &H5026         ' Manufacturer of Image
                                                     ' Input equipment
    PropertyTagThumbnailEquipModel = &H5027        ' Model of Image input
                                                     ' equipment
    PropertyTagThumbnailStripOffsets = &H5028      ' Image data location
    PropertyTagThumbnailOrientation = &H5029       ' Orientation of image
    PropertyTagThumbnailSamplesPerPixel = &H502A   ' Number of components
    PropertyTagThumbnailRowsPerStrip = &H502B      ' Number of rows per strip
    PropertyTagThumbnailStripBytesCount = &H502C   ' Bytes per compressed
                                                     ' strip
    PropertyTagThumbnailResolutionX = &H502D       ' Resolution in width
                                                     ' direction
    PropertyTagThumbnailResolutionY = &H502E       ' Resolution in height
                                                     ' direction
    PropertyTagThumbnailPlanarConfig = &H502F      ' Image data arrangement
    PropertyTagThumbnailResolutionUnit = &H5030    ' Unit of X and Y
                                                     ' Resolution
    PropertyTagThumbnailTransferFunction = &H5031  ' Transfer function
    PropertyTagThumbnailSoftwareUsed = &H5032      ' Software used
    PropertyTagThumbnailDateTime = &H5033          ' File change date and
                                                     ' time
    PropertyTagThumbnailArtist = &H5034            ' Person who created the
                                                     ' image
    PropertyTagThumbnailWhitePoint = &H5035        ' White point chromaticity
    PropertyTagThumbnailPrimaryChromaticities = &H5036
                                                     ' Chromaticities of
                                                     ' primaries
    PropertyTagThumbnailYCbCrCoefficients = &H5037 ' Color space transforma-
                                                     ' tion coefficients
    PropertyTagThumbnailYCbCrSubsampling = &H5038  ' Subsampling ratio of Y
                                                     ' to C
    PropertyTagThumbnailYCbCrPositioning = &H5039  ' Y and C position
    PropertyTagThumbnailRefBlackWhite = &H503A     ' Pair of black and white
                                                     ' reference values
    PropertyTagThumbnailCopyRight = &H503B         ' CopyRight holder
 
    PropertyTagLuminanceTable = &H5090
    PropertyTagChrominanceTable = &H5091
 
    PropertyTagFrameDelay = &H5100
    PropertyTagLoopCount = &H5101
 
    PropertyTagPixelUnit = &H5110          ' Unit specifier for pixel/unit
    PropertyTagPixelPerUnitX = &H5111      ' Pixels per unit in X
    PropertyTagPixelPerUnitY = &H5112      ' Pixels per unit in Y
    PropertyTagPaletteHistogram = &H5113   ' Palette histogram
 
    PropertyTagExifExposureTime = &H829A
    PropertyTagExifFNumber = &H829D
 
    PropertyTagExifExposureProg = &H8822
    PropertyTagExifSpectralSense = &H8824
    PropertyTagExifISOSpeed = &H8827
    PropertyTagExifOECF = &H8828
 
    PropertyTagExifVer = &H9000
    PropertyTagExifDTOrig = &H9003         ' Date & time of original
    PropertyTagExifDTDigitized = &H9004    ' Date & time of digital data generation
 
    PropertyTagExifCompConfig = &H9101
    PropertyTagExifCompBPP = &H9102
 
    PropertyTagExifShutterSpeed = &H9201
    PropertyTagExifAperture = &H9202
    PropertyTagExifBrightness = &H9203
    PropertyTagExifExposureBias = &H9204
    PropertyTagExifMaxAperture = &H9205
    PropertyTagExifSubjectDist = &H9206
    PropertyTagExifMeteringMode = &H9207
    PropertyTagExifLightSource = &H9208
    PropertyTagExifFlash = &H9209
    PropertyTagExifFocalLength = &H920A
    PropertyTagExifMakerNote = &H927C
    PropertyTagExifUserComment = &H9286
    PropertyTagExifDTSubsec = &H9290        ' Date & Time subseconds
    PropertyTagExifDTOrigSS = &H9291        ' Date & Time original subseconds
    PropertyTagExifDTDigSS = &H9292         ' Date & TIme digitized subseconds
 
    PropertyTagExifFPXVer = &HA000
    PropertyTagExifColorSpace = &HA001
    PropertyTagExifPixXDim = &HA002
    PropertyTagExifPixYDim = &HA003
    PropertyTagExifRelatedWav = &HA004      ' related sound file
    PropertyTagExifInterop = &HA005
    PropertyTagExifFlashEnergy = &HA20B
    PropertyTagExifSpatialFR = &HA20C       ' Spatial Frequency Response
    PropertyTagExifFocalXRes = &HA20E       ' Focal Plane X Resolution
    PropertyTagExifFocalYRes = &HA20F       ' Focal Plane Y Resolution
    PropertyTagExifFocalResUnit = &HA210    ' Focal Plane Resolution Unit
    PropertyTagExifSubjectLoc = &HA214
    PropertyTagExifExposureIndex = &HA215
    PropertyTagExifSensingMethod = &HA217
    PropertyTagExifFileSource = &HA300
    PropertyTagExifSceneType = &HA301
    PropertyTagExifCfaPattern = &HA302
 
    PropertyTagGpsVer = &H0
    PropertyTagGpsLatitudeRef = &H1
    PropertyTagGpsLatitude = &H2
    PropertyTagGpsLongitudeRef = &H3
    PropertyTagGpsLongitude = &H4
    PropertyTagGpsAltitudeRef = &H5
    PropertyTagGpsAltitude = &H6
    PropertyTagGpsGpsTime = &H7
    PropertyTagGpsGpsSatellites = &H8
    PropertyTagGpsGpsStatus = &H9
    PropertyTagGpsGpsMeasureMode = &HA
    PropertyTagGpsGpsDop = &HB              ' Measurement precision
    PropertyTagGpsSpeedRef = &HC
    PropertyTagGpsSpeed = &HD
    PropertyTagGpsTrackRef = &HE
    PropertyTagGpsTrack = &HF
    PropertyTagGpsImgDirRef = &H10
    PropertyTagGpsImgDir = &H11
    PropertyTagGpsMapDatum = &H12
    PropertyTagGpsDestLatRef = &H13
    PropertyTagGpsDestLat = &H14
    PropertyTagGpsDestLongRef = &H15
    PropertyTagGpsDestLong = &H16
    PropertyTagGpsDestBearRef = &H17
    PropertyTagGpsDestBear = &H18
    PropertyTagGpsDestDistRef = &H19
    PropertyTagGpsDestDist = &H1A
End Enum
 
'=================================
'Palette
Public Enum PaletteFlags
   PaletteFlagsHasAlpha = &H1
   PaletteFlagsGrayScale = &H2
   PaletteFlagsHalftone = &H4
End Enum
 
'=================================
'Rotate
Public Enum RotateFlipType
   RotateNoneFlipNone = 0
   Rotate90FlipNone = 1
   Rotate180FlipNone = 2
   Rotate270FlipNone = 3
 
   RotateNoneFlipX = 4
   Rotate90FlipX = 5
   Rotate180FlipX = 6
   Rotate270FlipX = 7
 
   RotateNoneFlipY = Rotate180FlipX
   Rotate90FlipY = Rotate270FlipX
   Rotate180FlipY = RotateNoneFlipX
   Rotate270FlipY = Rotate90FlipX
 
   RotateNoneFlipXY = Rotate180FlipNone
   Rotate90FlipXY = Rotate270FlipNone
   Rotate180FlipXY = RotateNoneFlipNone
   Rotate270FlipXY = Rotate90FlipNone
End Enum
 
'=================================
'Colors
Public Enum Colors
   AliceBlue = &HFFF0F8FF
   AntiqueWhite = &HFFFAEBD7
   Aqua = &HFF00FFFF
   Aquamarine = &HFF7FFFD4
   Azure = &HFFF0FFFF
   Beige = &HFFF5F5DC
   Bisque = &HFFFFE4C4
   Black = &HFF000000
   BlanchedAlmond = &HFFFFEBCD
   Blue = &HFF0000FF
   BlueViolet = &HFF8A2BE2
   Brown = &HFFA52A2A
   BurlyWood = &HFFDEB887
   CadetBlue = &HFF5F9EA0
   Chartreuse = &HFF7FFF00
   Chocolate = &HFFD2691E
   Coral = &HFFFF7F50
   CornflowerBlue = &HFF6495ED
   Cornsilk = &HFFFFF8DC
   Crimson = &HFFDC143C
   Cyan = &HFF00FFFF
   DarkBlue = &HFF00008B
   DarkCyan = &HFF008B8B
   DarkGoldenrod = &HFFB8860B
   DarkGray = &HFFA9A9A9
   DarkGreen = &HFF006400
   DarkKhaki = &HFFBDB76B
   DarkMagenta = &HFF8B008B
   DarkOliveGreen = &HFF556B2F
   DarkOrange = &HFFFF8C00
   DarkOrchid = &HFF9932CC
   DarkRed = &HFF8B0000
   DarkSalmon = &HFFE9967A
   DarkSeaGreen = &HFF8FBC8B
   DarkSlateBlue = &HFF483D8B
   DarkSlateGray = &HFF2F4F4F
   DarkTurquoise = &HFF00CED1
   DarkViolet = &HFF9400D3
   DeepPink = &HFFFF1493
   DeepSkyBlue = &HFF00BFFF
   DimGray = &HFF696969
   DodgerBlue = &HFF1E90FF
   Firebrick = &HFFB22222
   FloralWhite = &HFFFFFAF0
   ForestGreen = &HFF228B22
   Fuchsia = &HFFFF00FF
   Gainsboro = &HFFDCDCDC
   GhostWhite = &HFFF8F8FF
   Gold = &HFFFFD700
   Goldenrod = &HFFDAA520
   Gray = &HFF808080
   Green = &HFF008000
   GreenYellow = &HFFADFF2F
   Honeydew = &HFFF0FFF0
   HotPink = &HFFFF69B4
   IndianRed = &HFFCD5C5C
   Indigo = &HFF4B0082
   Ivory = &HFFFFFFF0
   Khaki = &HFFF0E68C
   Lavender = &HFFE6E6FA
   LavenderBlush = &HFFFFF0F5
   LawnGreen = &HFF7CFC00
   LemonChiffon = &HFFFFFACD
   LightBlue = &HFFADD8E6
   LightCoral = &HFFF08080
   LightCyan = &HFFE0FFFF
   LightGoldenrodYellow = &HFFFAFAD2
   LightGray = &HFFD3D3D3
   LightGreen = &HFF90EE90
   LightPink = &HFFFFB6C1
   LightSalmon = &HFFFFA07A
   LightSeaGreen = &HFF20B2AA
   LightSkyBlue = &HFF87CEFA
   LightSlateGray = &HFF778899
   LightSteelBlue = &HFFB0C4DE
   LightYellow = &HFFFFFFE0
   Lime = &HFF00FF00
   LimeGreen = &HFF32CD32
   Linen = &HFFFAF0E6
   Magenta = &HFFFF00FF
   Maroon = &HFF800000
   MediumAquamarine = &HFF66CDAA
   MediumBlue = &HFF0000CD
   MediumOrchid = &HFFBA55D3
   MediumPurple = &HFF9370DB
   MediumSeaGreen = &HFF3CB371
   MediumSlateBlue = &HFF7B68EE
   MediumSpringGreen = &HFF00FA9A
   MediumTurquoise = &HFF48D1CC
   MediumVioletRed = &HFFC71585
   MidnightBlue = &HFF191970
   MintCream = &HFFF5FFFA
   MistyRose = &HFFFFE4E1
   Moccasin = &HFFFFE4B5
   NavajoWhite = &HFFFFDEAD
   Navy = &HFF000080
   OldLace = &HFFFDF5E6
   Olive = &HFF808000
   OliveDrab = &HFF6B8E23
   Orange = &HFFFFA500
   OrangeRed = &HFFFF4500
   Orchid = &HFFDA70D6
   PaleGoldenrod = &HFFEEE8AA
   PaleGreen = &HFF98FB98
   PaleTurquoise = &HFFAFEEEE
   PaleVioletRed = &HFFDB7093
   PapayaWhip = &HFFFFEFD5
   PeachPuff = &HFFFFDAB9
   Peru = &HFFCD853F
   Pink = &HFFFFC0CB
   Plum = &HFFDDA0DD
   PowderBlue = &HFFB0E0E6
   Purple = &HFF800080
   Red = &HFFFF0000
   RosyBrown = &HFFBC8F8F
   RoyalBlue = &HFF4169E1
   SaddleBrown = &HFF8B4513
   Salmon = &HFFFA8072
   SandyBrown = &HFFF4A460
   SeaGreen = &HFF2E8B57
   SeaShell = &HFFFFF5EE
   Sienna = &HFFA0522D
   Silver = &HFFC0C0C0
   SkyBlue = &HFF87CEEB
   SlateBlue = &HFF6A5ACD
   SlateGray = &HFF708090
   Snow = &HFFFFFAFA
   SpringGreen = &HFF00FF7F
   SteelBlue = &HFF4682B4
   Tan = &HFFD2B48C
   Teal = &HFF008080
   Thistle = &HFFD8BFD8
   Tomato = &HFFFF6347
   TRANSPARENT = &HFFFFFF
   Turquoise = &HFF40E0D0
   Violet = &HFFEE82EE
   Wheat = &HFFF5DEB3
   White = &HFFFFFFFF
   WhiteSmoke = &HFFF5F5F5
   Yellow = &HFFFFFF00
   YellowGreen = &HFF9ACD32
End Enum
 
Public Enum ColorMode
    ColorModeARGB32 = 0
    ColorModeARGB64 = 1
End Enum
 
Public Enum ColorChannelFlags
   ColorChannelFlagsC = 0
   ColorChannelFlagsM
   ColorChannelFlagsY
   ColorChannelFlagsK
   ColorChannelFlagsLast
End Enum
 
Public Enum ColorShiftComponents
    AlphaShift = 24
    RedShift = 16
    GreenShift = 8
    BlueShift = 0
End Enum
 
Public Enum ColorMaskComponents
    AlphaMask = &HFF000000
    RedMask = &HFF0000
    GreenMask = &HFF00
    BlueMask = &HFF
End Enum
 
'=================================
'String
Public Enum StringFormatFlags
   StringFormatFlagsDirectionRightToLeft = &H1
   StringFormatFlagsDirectionVertical = &H2
   StringFormatFlagsNoFitBlackBox = &H4
   StringFormatFlagsDisplayFormatControl = &H20
   StringFormatFlagsNoFontFallback = &H400
   StringFormatFlagsMeasureTrailingSpaces = &H800
   StringFormatFlagsNoWrap = &H1000
   StringFormatFlagsLineLimit = &H2000
 
   StringFormatFlagsNoClip = &H4000
End Enum
 
Public Enum StringTrimming
   StringTrimmingNone = 0
   StringTrimmingCharacter = 1
   StringTrimmingWord = 2
   StringTrimmingEllipsisCharacter = 3
   StringTrimmingEllipsisWord = 4
   StringTrimmingEllipsisPath = 5
End Enum
 
Public Enum StringDigitSubstitute
   StringDigitSubstituteUser = 0
   StringDigitSubstituteNone = 1
   StringDigitSubstituteNational = 2
   StringDigitSubstituteTraditional = 3
End Enum
 
'=================================
'Pen / Brush
Public Enum HatchStyle
   HatchStyleHorizontal                   ' 0
   HatchStyleVertical                     ' 1
   HatchStyleForwardDiagonal              ' 2
   HatchStyleBackwardDiagonal             ' 3
   HatchStyleCross                        ' 4
   HatchStyleDiagonalCross                ' 5
   HatchStyle05Percent                    ' 6
   HatchStyle10Percent                    ' 7
   HatchStyle20Percent                    ' 8
   HatchStyle25Percent                    ' 9
   HatchStyle30Percent                    ' 10
   HatchStyle40Percent                    ' 11
   HatchStyle50Percent                    ' 12
   HatchStyle60Percent                    ' 13
   HatchStyle70Percent                    ' 14
   HatchStyle75Percent                    ' 15
   HatchStyle80Percent                    ' 16
   HatchStyle90Percent                    ' 17
   HatchStyleLightDownwardDiagonal        ' 18
   HatchStyleLightUpwardDiagonal          ' 19
   HatchStyleDarkDownwardDiagonal         ' 20
   HatchStyleDarkUpwardDiagonal           ' 21
   HatchStyleWideDownwardDiagonal         ' 22
   HatchStyleWideUpwardDiagonal           ' 23
   HatchStyleLightVertical                ' 24
   HatchStyleLightHorizontal              ' 25
   HatchStyleNarrowVertical               ' 26
   HatchStyleNarrowHorizontal             ' 27
   HatchStyleDarkVertical                 ' 28
   HatchStyleDarkHorizontal               ' 29
   HatchStyleDashedDownwardDiagonal       ' 30
   HatchStyleDashedUpwardDiagonal         ' 31
   HatchStyleDashedHorizontal             ' 32
   HatchStyleDashedVertical               ' 33
   HatchStyleSmallConfetti                ' 34
   HatchStyleLargeConfetti                ' 35
   HatchStyleZigZag                       ' 36
   HatchStyleWave                         ' 37
   HatchStyleDiagonalBrick                ' 38
   HatchStyleHorizontalBrick              ' 39
   HatchStyleWeave                        ' 40
   HatchStylePlaid                        ' 41
   HatchStyleDivot                        ' 42
   HatchStyleDottedGrid                   ' 43
   HatchStyleDottedDiamond                ' 44
   HatchStyleShingle                      ' 45
   HatchStyleTrellis                      ' 46
   HatchStyleSphere                       ' 47
   HatchStyleSmallGrid                    ' 48
   HatchStyleSmallCheckerBoard            ' 49
   HatchStyleLargeCheckerBoard            ' 50
   HatchStyleOutlinedDiamond              ' 51
   HatchStyleSolidDiamond                 ' 52
 
   HatchStyleTotal
   HatchStyleLargeGrid = HatchStyleCross  ' 4
 
   HatchStyleMin = HatchStyleHorizontal
   HatchStyleMax = HatchStyleTotal - 1
End Enum
 
Public Enum PenAlignment
    PenAlignmentCenter = 0
    PenAlignmentInset = 1
End Enum
 
Public Enum BrushType
   BrushTypeSolidColor = 0
   BrushTypeHatchFill = 1
   BrushTypeTextureFill = 2
   BrushTypePathGradient = 3
   BrushTypeLinearGradient = 4
End Enum
 
Public Enum DashStyle
   DashStyleSolid
   DashStyleDash
   DashStyleDot
   DashStyleDashDot
   DashStyleDashDotDot
   DashStyleCustom
End Enum
 
Public Enum DashCap
   DashCapFlat = 0
   DashCapRound = 2
   DashCapTriangle = 3
End Enum
 
Public Enum LineCap
   LineCapFlat = 0
   LineCapSquare = 1
   LineCapRound = 2
   LineCapTriangle = 3
 
   LineCapNoAnchor = &H10         ' corresponds to flat cap
   LineCapSquareAnchor = &H11     ' corresponds to square cap
   LineCapRoundAnchor = &H12      ' corresponds to round cap
   LineCapDiamondAnchor = &H13    ' corresponds to triangle cap
   LineCapArrowAnchor = &H14      ' no correspondence
 
   LineCapCustom = &HFF           ' custom cap
 
   LineCapAnchorMask = &HF0        ' mask to check for anchor or not.
End Enum
 
Public Enum CustomLineCapType
   CustomLineCapTypeDefault = 0
   CustomLineCapTypeAdjustableArrow = 1
End Enum
 
Public Enum LineJoin
   LineJoinMiter = 0
   LineJoinBevel = 1
   LineJoinRound = 2
   LineJoinMiterClipped = 3
End Enum
 
Public Enum PenType
   PenTypeSolidColor = BrushTypeSolidColor
   PenTypeHatchFill = BrushTypeHatchFill
   PenTypeTextureFill = BrushTypeTextureFill
   PenTypePathGradient = BrushTypePathGradient
   PenTypeLinearGradient = BrushTypeLinearGradient
   PenTypeUnknown = -1
End Enum
 
'=================================
'Meta File
Public Enum MetafileType
   MetafileTypeInvalid            ' Invalid metafile
   MetafileTypeWmf                ' Standard WMF
   MetafileTypeWmfPlaceable       ' Placeable WMF
   MetafileTypeEmf                ' EMF (not EMF+)
   MetafileTypeEmfPlusOnly        ' EMF+ without dual down-level records
   MetafileTypeEmfPlusDual         ' EMF+ with dual down-level records
End Enum
 
Public Enum EmfType
    EmfTypeEmfOnly = MetafileTypeEmf               ' no EMF+  only EMF
    EmfTypeEmfPlusOnly = MetafileTypeEmfPlusOnly   ' no EMF  only EMF+
    EmfTypeEmfPlusDual = MetafileTypeEmfPlusDual   ' both EMF+ and EMF
End Enum
 
Public Enum ObjectType
    ObjectTypeInvalid
    ObjectTypeBrush
    ObjectTypePen
    ObjectTypePath
    ObjectTypeRegion
    ObjectTypeImage
    ObjectTypeFont
    ObjectTypeStringFormat
    ObjectTypeImageAttributes
    ObjectTypeCustomLineCap
 
    ObjectTypeMax = ObjectTypeCustomLineCap
    ObjectTypeMin = ObjectTypeBrush
End Enum
 
Public Enum MetafileFrameUnit
   MetafileFrameUnitPixel = UnitPixel
   MetafileFrameUnitPoint = UnitPoint
   MetafileFrameUnitInch = UnitInch
   MetafileFrameUnitDocument = UnitDocument
   MetafileFrameUnitMillimeter = UnitMillimeter
   MetafileFrameUnitGdi                        ' GDI compatible .01 MM units
End Enum
 
' Coordinate space identifiers
Public Enum CoordinateSpace
   CoordinateSpaceWorld     ' 0
   CoordinateSpacePage      ' 1
   CoordinateSpaceDevice     ' 2
End Enum
 
Public Enum EmfPlusRecordType
   WmfRecordTypeSetBkColor = &H10201
   WmfRecordTypeSetBkMode = &H10102
   WmfRecordTypeSetMapMode = &H10103
   WmfRecordTypeSetROP2 = &H10104
   WmfRecordTypeSetRelAbs = &H10105
   WmfRecordTypeSetPolyFillMode = &H10106
   WmfRecordTypeSetStretchBltMode = &H10107
   WmfRecordTypeSetTextCharExtra = &H10108
   WmfRecordTypeSetTextColor = &H10209
   WmfRecordTypeSetTextJustification = &H1020A
   WmfRecordTypeSetWindowOrg = &H1020B
   WmfRecordTypeSetWindowExt = &H1020C
   WmfRecordTypeSetViewportOrg = &H1020D
   WmfRecordTypeSetViewportExt = &H1020E
   WmfRecordTypeOffsetWindowOrg = &H1020F
   WmfRecordTypeScaleWindowExt = &H10410
   WmfRecordTypeOffsetViewportOrg = &H10211
   WmfRecordTypeScaleViewportExt = &H10412
   WmfRecordTypeLineTo = &H10213
   WmfRecordTypeMoveTo = &H10214
   WmfRecordTypeExcludeClipRect = &H10415
   WmfRecordTypeIntersectClipRect = &H10416
   WmfRecordTypeArc = &H10817
   WmfRecordTypeEllipse = &H10418
   WmfRecordTypeFloodFill = &H10419
   WmfRecordTypePie = &H1081A
   WmfRecordTypeRectangle = &H1041B
   WmfRecordTypeRoundRect = &H1061C
   WmfRecordTypePatBlt = &H1061D
   WmfRecordTypeSaveDC = &H1001E
   WmfRecordTypeSetPixel = &H1041F
   WmfRecordTypeOffsetClipRgn = &H10220
   WmfRecordTypeTextOut = &H10521
   WmfRecordTypeBitBlt = &H10922
   WmfRecordTypeStretchBlt = &H10B23
   WmfRecordTypePolygon = &H10324
   WmfRecordTypePolyline = &H10325
   WmfRecordTypeEscape = &H10626
   WmfRecordTypeRestoreDC = &H10127
   WmfRecordTypeFillRegion = &H10228
   WmfRecordTypeFrameRegion = &H10429
   WmfRecordTypeInvertRegion = &H1012A
   WmfRecordTypePaintRegion = &H1012B
   WmfRecordTypeSelectClipRegion = &H1012C
   WmfRecordTypeSelectObject = &H1012D
   WmfRecordTypeSetTextAlign = &H1012E
   WmfRecordTypeDrawText = &H1062F
   WmfRecordTypeChord = &H10830
   WmfRecordTypeSetMapperFlags = &H10231
   WmfRecordTypeExtTextOut = &H10A32
   WmfRecordTypeSetDIBToDev = &H10D33
   WmfRecordTypeSelectPalette = &H10234
   WmfRecordTypeRealizePalette = &H10035
   WmfRecordTypeAnimatePalette = &H10436
   WmfRecordTypeSetPalEntries = &H10037
   WmfRecordTypePolyPolygon = &H10538
   WmfRecordTypeResizePalette = &H10139
   WmfRecordTypeDIBBitBlt = &H10940
   WmfRecordTypeDIBStretchBlt = &H10B41
   WmfRecordTypeDIBCreatePatternBrush = &H10142
   WmfRecordTypeStretchDIB = &H10F43
   WmfRecordTypeExtFloodFill = &H10548
   WmfRecordTypeSetLayout = &H10149
   WmfRecordTypeResetDC = &H1014C
   WmfRecordTypeStartDoc = &H1014D
   WmfRecordTypeStartPage = &H1004F
   WmfRecordTypeEndPage = &H10050
   WmfRecordTypeAbortDoc = &H10052
   WmfRecordTypeEndDoc = &H1005E
   WmfRecordTypeDeleteObject = &H101F0
   WmfRecordTypeCreatePalette = &H100F7
   WmfRecordTypeCreateBrush = &H100F8
   WmfRecordTypeCreatePatternBrush = &H101F9
   WmfRecordTypeCreatePenIndirect = &H102FA
   WmfRecordTypeCreateFontIndirect = &H102FB
   WmfRecordTypeCreateBrushIndirect = &H102FC
   WmfRecordTypeCreateBitmapIndirect = &H102FD
   WmfRecordTypeCreateBitmap = &H106FE
   WmfRecordTypeCreateRegion = &H106FF
   EmfRecordTypeHeader = 1
   EmfRecordTypePolyBezier = 2
   EmfRecordTypePolygon = 3
   EmfRecordTypePolyline = 4
   EmfRecordTypePolyBezierTo = 5
   EmfRecordTypePolyLineTo = 6
   EmfRecordTypePolyPolyline = 7
   EmfRecordTypePolyPolygon = 8
   EmfRecordTypeSetWindowExtEx = 9
   EmfRecordTypeSetWindowOrgEx = 10
   EmfRecordTypeSetViewportExtEx = 11
   EmfRecordTypeSetViewportOrgEx = 12
   EmfRecordTypeSetBrushOrgEx = 13
   EmfRecordTypeEOF = 14
   EmfRecordTypeSetPixelV = 15
   EmfRecordTypeSetMapperFlags = 16
   EmfRecordTypeSetMapMode = 17
   EmfRecordTypeSetBkMode = 18
   EmfRecordTypeSetPolyFillMode = 19
   EmfRecordTypeSetROP2 = 20
   EmfRecordTypeSetStretchBltMode = 21
   EmfRecordTypeSetTextAlign = 22
   EmfRecordTypeSetColorAdjustment = 23
   EmfRecordTypeSetTextColor = 24
   EmfRecordTypeSetBkColor = 25
   EmfRecordTypeOffsetClipRgn = 26
   EmfRecordTypeMoveToEx = 27
   EmfRecordTypeSetMetaRgn = 28
   EmfRecordTypeExcludeClipRect = 29
   EmfRecordTypeIntersectClipRect = 30
   EmfRecordTypeScaleViewportExtEx = 31
   EmfRecordTypeScaleWindowExtEx = 32
   EmfRecordTypeSaveDC = 33
   EmfRecordTypeRestoreDC = 34
   EmfRecordTypeSetWorldTransform = 35
   EmfRecordTypeModifyWorldTransform = 36
   EmfRecordTypeSelectObject = 37
   EmfRecordTypeCreatePen = 38
   EmfRecordTypeCreateBrushIndirect = 39
   EmfRecordTypeDeleteObject = 40
   EmfRecordTypeAngleArc = 41
   EmfRecordTypeEllipse = 42
   EmfRecordTypeRectangle = 43
   EmfRecordTypeRoundRect = 44
   EmfRecordTypeArc = 45
   EmfRecordTypeChord = 46
   EmfRecordTypePie = 47
   EmfRecordTypeSelectPalette = 48
   EmfRecordTypeCreatePalette = 49
   EmfRecordTypeSetPaletteEntries = 50
   EmfRecordTypeResizePalette = 51
   EmfRecordTypeRealizePalette = 52
   EmfRecordTypeExtFloodFill = 53
   EmfRecordTypeLineTo = 54
   EmfRecordTypeArcTo = 55
   EmfRecordTypePolyDraw = 56
   EmfRecordTypeSetArcDirection = 57
   EmfRecordTypeSetMiterLimit = 58
   EmfRecordTypeBeginPath = 59
   EmfRecordTypeEndPath = 60
   EmfRecordTypeCloseFigure = 61
   EmfRecordTypeFillPath = 62
   EmfRecordTypeStrokeAndFillPath = 63
   EmfRecordTypeStrokePath = 64
   EmfRecordTypeFlattenPath = 65
   EmfRecordTypeWidenPath = 66
   EmfRecordTypeSelectClipPath = 67
   EmfRecordTypeAbortPath = 68
   EmfRecordTypeReserved_069 = 69
   EmfRecordTypeGdiComment = 70
   EmfRecordTypeFillRgn = 71
   EmfRecordTypeFrameRgn = 72
   EmfRecordTypeInvertRgn = 73
   EmfRecordTypePaintRgn = 74
   EmfRecordTypeExtSelectClipRgn = 75
   EmfRecordTypeBitBlt = 76
   EmfRecordTypeStretchBlt = 77
   EmfRecordTypeMaskBlt = 78
   EmfRecordTypePlgBlt = 79
   EmfRecordTypeSetDIBitsToDevice = 80
   EmfRecordTypeStretchDIBits = 81
   EmfRecordTypeExtCreateFontIndirect = 82
   EmfRecordTypeExtTextOutA = 83
   EmfRecordTypeExtTextOutW = 84
   EmfRecordTypePolyBezier16 = 85
   EmfRecordTypePolygon16 = 86
   EmfRecordTypePolyline16 = 87
   EmfRecordTypePolyBezierTo16 = 88
   EmfRecordTypePolylineTo16 = 89
   EmfRecordTypePolyPolyline16 = 90
   EmfRecordTypePolyPolygon16 = 91
   EmfRecordTypePolyDraw16 = 92
   EmfRecordTypeCreateMonoBrush = 93
   EmfRecordTypeCreateDIBPatternBrushPt = 94
   EmfRecordTypeExtCreatePen = 95
   EmfRecordTypePolyTextOutA = 96
   EmfRecordTypePolyTextOutW = 97
   EmfRecordTypeSetICMMode = 98
   EmfRecordTypeCreateColorSpace = 99
   EmfRecordTypeSetColorSpace = 100
   EmfRecordTypeDeleteColorSpace = 101
   EmfRecordTypeGLSRecord = 102
   EmfRecordTypeGLSBoundedRecord = 103
   EmfRecordTypePixelFormat = 104
   EmfRecordTypeDrawEscape = 105
   EmfRecordTypeExtEscape = 106
   EmfRecordTypeStartDoc = 107
   EmfRecordTypeSmallTextOut = 108
   EmfRecordTypeForceUFIMapping = 109
   EmfRecordTypeNamedEscape = 110
   EmfRecordTypeColorCorrectPalette = 111
   EmfRecordTypeSetICMProfileA = 112
   EmfRecordTypeSetICMProfileW = 113
   EmfRecordTypeAlphaBlend = 114
   EmfRecordTypeSetLayout = 115
   EmfRecordTypeTransparentBlt = 116
   EmfRecordTypeReserved_117 = 117
   EmfRecordTypeGradientFill = 118
   EmfRecordTypeSetLinkedUFIs = 119
   EmfRecordTypeSetTextJustification = 120
   EmfRecordTypeColorMatchToTargetW = 121
   EmfRecordTypeCreateColorSpaceW = 122
   EmfRecordTypeMax = 122
   EmfRecordTypeMin = 1
 
   EmfPlusRecordTypeInvalid = 16384 '//GDIP_EMFPLUS_RECORD_BASE
   EmfPlusRecordTypeHeader = 16385
   EmfPlusRecordTypeEndOfFile = 16386
   EmfPlusRecordTypeComment = 16387
   EmfPlusRecordTypeGetDC = 16388
   EmfPlusRecordTypeMultiFormatStart = 16389
   EmfPlusRecordTypeMultiFormatSection = 16390
   EmfPlusRecordTypeMultiFormatEnd = 16391
 
   EmfPlusRecordTypeObject = 16392
 
   EmfPlusRecordTypeClear = 16393
   EmfPlusRecordTypeFillRects = 16394
   EmfPlusRecordTypeDrawRects = 16395
   EmfPlusRecordTypeFillPolygon = 16396
   EmfPlusRecordTypeDrawLines = 16397
   EmfPlusRecordTypeFillEllipse = 16398
   EmfPlusRecordTypeDrawEllipse = 16399
   EmfPlusRecordTypeFillPie = 16400
   EmfPlusRecordTypeDrawPie = 16401
   EmfPlusRecordTypeDrawArc = 16402
   EmfPlusRecordTypeFillRegion = 16403
   EmfPlusRecordTypeFillPath = 16404
   EmfPlusRecordTypeDrawPath = 16405
   EmfPlusRecordTypeFillClosedCurve = 16406
   EmfPlusRecordTypeDrawClosedCurve = 16407
   EmfPlusRecordTypeDrawCurve = 16408
   EmfPlusRecordTypeDrawBeziers = 16409
   EmfPlusRecordTypeDrawImage = 16410
   EmfPlusRecordTypeDrawImagePoints = 16411
   EmfPlusRecordTypeDrawString = 16412
 
   EmfPlusRecordTypeSetRenderingOrigin = 16413
   EmfPlusRecordTypeSetAntiAliasMode = 16414
   EmfPlusRecordTypeSetTextRenderingHint = 16415
   EmfPlusRecordTypeSetTextContrast = 16416
   EmfPlusRecordTypeSetInterpolationMode = 16417
   EmfPlusRecordTypeSetPixelOffsetMode = 16418
   EmfPlusRecordTypeSetCompositingMode = 16419
   EmfPlusRecordTypeSetCompositingQuality = 16420
   EmfPlusRecordTypeSave = 16421
   EmfPlusRecordTypeRestore = 16422
   EmfPlusRecordTypeBeginContainer = 16423
   EmfPlusRecordTypeBeginContainerNoParams = 16424
   EmfPlusRecordTypeEndContainer = 16425
   EmfPlusRecordTypeSetWorldTransform = 16426
   EmfPlusRecordTypeResetWorldTransform = 16427
   EmfPlusRecordTypeMultiplyWorldTransform = 16428
   EmfPlusRecordTypeTranslateWorldTransform = 16429
   EmfPlusRecordTypeScaleWorldTransform = 16430
   EmfPlusRecordTypeRotateWorldTransform = 16431
   EmfPlusRecordTypeSetPageTransform = 16432
   EmfPlusRecordTypeResetClip = 16433
   EmfPlusRecordTypeSetClipRect = 16434
   EmfPlusRecordTypeSetClipPath = 16435
   EmfPlusRecordTypeSetClipRegion = 16436
   EmfPlusRecordTypeOffsetClip = 16437
   EmfPlusRecordTypeDrawDriverString = 16438
   EmfPlusRecordTotal = 16439
   EmfPlusRecordTypeMax = 16438
   EmfPlusRecordTypeMin = 16385
End Enum
 
'=================================
'Other
Public Enum HotkeyPrefix
   HotkeyPrefixNone = 0
   HotkeyPrefixShow = 1
   HotkeyPrefixHide = 2
End Enum
 
 
Public Enum FlushIntention
   FlushIntentionFlush = 0         ' Flush all batched rendering operations
   FlushIntentionSync = 1          ' Flush all batched rendering operations
End Enum
 
Public Enum EncoderParameterValueType
   EncoderParameterValueTypeByte = 1              ' 8-bit unsigned int
   EncoderParameterValueTypeASCII = 2             ' 8-bit byte containing one 7-bit ASCII
                                                   ' code. NULL terminated.
   EncoderParameterValueTypeShort = 3             ' 16-bit unsigned int
   EncoderParameterValueTypeLong = 4              ' 32-bit unsigned int
   EncoderParameterValueTypeRational = 5          ' Two Longs. The first Long is the
                                                   ' numerator the second Long expresses the
                                                   ' denomintor.
   EncoderParameterValueTypeLongRange = 6         ' Two longs which specify a range of
                                                   ' integer values. The first Long specifies
                                                   ' the lower end and the second one
                                                   ' specifies the higher end. All values
                                                   ' are inclusive at both ends
   EncoderParameterValueTypeUndefined = 7         ' 8-bit byte that can take any value
                                                   ' depending on field definition
   EncoderParameterValueTypeRationalRange = 8      ' Two Rationals. The first Rational
                                                   ' specifies the lower end and the second
                                                   ' specifies the higher end. All values
                                                   ' are inclusive at both ends
End Enum
 
Public Enum EncoderValue
   EncoderValueColorTypeCMYK
   EncoderValueColorTypeYCCK
   EncoderValueCompressionLZW
   EncoderValueCompressionCCITT3
   EncoderValueCompressionCCITT4
   EncoderValueCompressionRle
   EncoderValueCompressionNone
   EncoderValueScanMethodInterlaced
   EncoderValueScanMethodNonInterlaced
   EncoderValueVersionGif87
   EncoderValueVersionGif89
   EncoderValueRenderProgressive
   EncoderValueRenderNonProgressive
   EncoderValueTransformRotate90
   EncoderValueTransformRotate180
   EncoderValueTransformRotate270
   EncoderValueTransformFlipHorizontal
   EncoderValueTransformFlipVertical
   EncoderValueMultiFrame
   EncoderValueLastFrame
   EncoderValueFlush
   EncoderValueFrameDimensionTime
   EncoderValueFrameDimensionResolution
   EncoderValueFrameDimensionPage
End Enum
 
Public Enum DebugEventLevel
    DebugEventLevelFatal
    DebugEventLevelWarning
End Enum
 
Public Declare Function GdipCreateFromHDC2 Lib "gdiplus" (ByVal hDc As Long, ByVal hDevice As Long, graphics As Long) As GpStatus
Public Declare Function GdipCreateFromHWNDICM Lib "gdiplus" (ByVal hwnd As Long, graphics As Long) As GpStatus
 
Public Declare Function GdipEnumerateMetafileDestPoint Lib "gdiplus" (ByVal graphics As Long, ByVal metafile As Long, destPoint As POINTF, lpEnumerateMetafileProc As Long, ByVal callbackData As Long, imageAttributes As Long) As GpStatus
Public Declare Function GdipEnumerateMetafileDestPointI Lib "gdiplus" (graphics As Long, ByVal metafile As Long, destPoint As POINTL, ByVal lpEnumerateMetafileProc As Long, ByVal callbackData As Long, ByVal imageAttributes As Long) As GpStatus
Public Declare Function GdipEnumerateMetafileDestRect Lib "gdiplus" (ByVal graphics As Long, ByVal metafile As Long, destRect As RECTF, lpEnumerateMetafileProc As Long, ByVal callbackData As Long, imageAttributes As Long) As GpStatus
Public Declare Function GdipEnumerateMetafileDestRectI Lib "gdiplus" (ByVal graphics As Long, ByVal metafile As Long, destRect As RECTL, lpEnumerateMetafileProc As Long, ByVal callbackData As Long, imageAttributes As Long) As GpStatus
Public Declare Function GdipEnumerateMetafileDestPoints Lib "gdiplus" (ByVal graphics As Long, ByVal metafile As Long, destPoint As POINTF, ByVal Count As Long, lpEnumerateMetafileProc As Long, ByVal callbackData As Long, imageAttributes As Long) As GpStatus
Public Declare Function GdipEnumerateMetafileDestPointsI Lib "gdiplus" (ByVal graphics As Long, ByVal metafile As Long, destPoint As POINTL, ByVal Count As Long, lpEnumerateMetafileProc As Long, ByVal callbackData As Long, imageAttributes As Long) As GpStatus
Public Declare Function GdipEnumerateMetafileSrcRectDestPoint Lib "gdiplus" (ByVal graphics As Long, ByVal metafile As Long, destPoint As POINTF, srcRect As RECTF, ByVal srcUnit As GpUnit, ByVal lpEnumerateMetafileProc As Long, ByVal callbackData As Long, ByVal imageAttributes As Long) As GpStatus
Public Declare Function GdipEnumerateMetafileSrcRectDestPointI Lib "gdiplus" (ByVal graphics As Long, ByVal metafile As Long, destPoint As POINTL, srcRect As RECTL, ByVal srcUnit As GpUnit, ByVal lpEnumerateMetafileProc As Long, ByVal callbackData As Long, ByVal imageAttributes As Long) As GpStatus
Public Declare Function GdipEnumerateMetafileSrcRectDestRect Lib "gdiplus" (ByVal graphics As Long, ByVal metafile As Long, destRect As RECTF, srcRect As RECTF, ByVal srcUnit As GpUnit, ByVal lpEnumerateMetafileProc As Long, ByVal callbackData As Long, ByVal imageAttributes As Long) As GpStatus
Public Declare Function GdipEnumerateMetafileSrcRectDestRectI Lib "gdiplus" (ByVal graphics As Long, ByVal metafile As Long, destRect As RECTL, srcRect As RECTL, ByVal srcUnit As GpUnit, ByVal lpEnumerateMetafileProc As Long, ByVal callbackData As Long, ByVal imageAttributes As Long) As GpStatus
Public Declare Function GdipEnumerateMetafileSrcRectDestPoints Lib "gdiplus" (ByVal graphics As Long, ByVal metafile As Long, destPoints As POINTF, ByVal Count As Long, srcRect As RECTF, ByVal srcUnit As GpUnit, ByVal lpEnumerateMetafileProc As Long, ByVal callbackData As Long, ByVal imageAttributes As Long) As GpStatus
Public Declare Function GdipEnumerateMetafileSrcRectDestPointsI Lib "gdiplus" (ByVal graphics As Long, ByVal metafile As Long, destPoints As POINTL, ByVal Count As Long, srcRect As RECTL, ByVal srcUnit As GpUnit, ByVal lpEnumerateMetafileProc As Long, ByVal callbackData As Long, ByVal imageAttributes As Long) As GpStatus
Public Declare Function GdipPlayMetafileRecord Lib "gdiplus" (ByVal metafile As Long, ByVal recordType As EmfPlusRecordType, ByVal flags As Long, ByVal dataSize As Long, byteData As Any) As GpStatus
 
Public Declare Function GdipGetMetafileHeaderFromWmf Lib "gdiplus" (ByVal hWmf As Long, WmfPlaceableFileHdr As WmfPlaceableFileHeader, Header As METAFILEHEADER) As GpStatus
Public Declare Function GdipGetMetafileHeaderFromEmf Lib "gdiplus" (ByVal hemf As Long, Header As METAFILEHEADER) As GpStatus
Public Declare Function GdipGetMetafileHeaderFromFile Lib "gdiplus" (ByVal filename As Long, Header As METAFILEHEADER) As GpStatus
Public Declare Function GdipGetMetafileHeaderFromStream Lib "gdiplus" (ByVal stream As Any, Header As METAFILEHEADER) As GpStatus
Public Declare Function GdipGetMetafileHeaderFromMetafile Lib "gdiplus" (ByVal metafile As Long, Header As METAFILEHEADER) As GpStatus
Public Declare Function GdipGetHemfFromMetafile Lib "gdiplus" (ByVal metafile As Long, hemf As Long) As GpStatus
Public Declare Function GdipCreateStreamOnFile Lib "gdiplus" (ByVal filename As Long, ByVal access As Long, stream As Any) As GpStatus
Public Declare Function GdipCreateMetafileFromWmf Lib "gdiplus" (ByVal hWmf As Long, ByVal bDeleteWmf As Long, WmfPlaceableFileHdr As WmfPlaceableFileHeader, ByVal metafile As Long) As GpStatus
Public Declare Function GdipCreateMetafileFromEmf Lib "gdiplus" (ByVal hemf As Long, ByVal bDeleteEmf As Long, metafile As Long) As GpStatus
Public Declare Function GdipCreateMetafileFromFile Lib "gdiplus" (ByVal file As Long, metafile As Long) As GpStatus
Public Declare Function GdipCreateMetafileFromWmfFile Lib "gdiplus" (ByVal file As Long, WmfPlaceableFileHdr As WmfPlaceableFileHeader, metafile As Long) As GpStatus
Public Declare Function GdipCreateMetafileFromStream Lib "gdiplus" (ByVal stream As Any, metafile As Long) As GpStatus
Public Declare Function GdipRecordMetafile Lib "gdiplus" (ByVal referenceHdc As Long, etype As EmfType, FrameRect As RECTF, ByVal frameUnit As MetafileFrameUnit, ByVal description As Long, metafile As Long) As GpStatus
Public Declare Function GdipRecordMetafileI Lib "gdiplus" (ByVal referenceHdc As Long, etype As EmfType, FrameRect As RECTL, ByVal frameUnit As MetafileFrameUnit, ByVal description As Long, metafile As Long) As GpStatus
Public Declare Function GdipRecordMetafileFileName Lib "gdiplus" (ByVal filename As Long, ByVal referenceHdc As Long, etype As EmfType, FrameRect As RECTF, ByVal frameUnit As MetafileFrameUnit, ByVal description As Long, metafile As Long) As GpStatus
Public Declare Function GdipRecordMetafileFileNameI Lib "gdiplus" (ByVal filename As Long, ByVal referenceHdc As Long, etype As EmfType, FrameRect As RECTL, ByVal frameUnit As MetafileFrameUnit, ByVal description As Long, metafile As Long) As GpStatus
Public Declare Function GdipRecordMetafileStream Lib "gdiplus" (ByVal stream As Any, ByVal referenceHdc As Long, etype As EmfType, FrameRect As RECTF, ByVal frameUnit As MetafileFrameUnit, ByVal description As Long, metafile As Long) As GpStatus
Public Declare Function GdipRecordMetafileStreamI Lib "gdiplus" (ByVal stream As Any, ByVal referenceHdc As Long, etype As EmfType, FrameRect As RECTL, ByVal frameUnit As MetafileFrameUnit, ByVal description As Long, metafile As Long) As GpStatus
Public Declare Function GdipSetMetafileDownLevelRasterizationLimit Lib "gdiplus" (ByVal metafile As Long, ByVal metafileRasterizationLimitDpi As Long) As GpStatus
Public Declare Function GdipGetMetafileDownLevelRasterizationLimit Lib "gdiplus" (ByVal metafile As Long, metafileRasterizationLimitDpi As Long) As GpStatus
Public Declare Function GdipGetImageDecodersSize Lib "gdiplus" (numDecoders As Long, Size As Long) As GpStatus
 
Public Declare Function GdipFlush Lib "gdiplus" (ByVal graphics As Long, ByVal intention As FlushIntention) As GpStatus
Public Declare Function GdipAlloc Lib "gdiplus" (ByVal Size As Long) As Long
Public Declare Sub GdipFree Lib "gdiplus" (ByVal ptr As Long)
 
'===================================================================================
'  �������� / ��������
'===================================================================================
 
Public Declare Function GdiplusStartup Lib "gdiplus" (Token As Long, inputbuf As GdiplusStartupInput, Optional ByVal outputbuf As Long = 0) As GpStatus
Public Declare Function GdiplusShutdown Lib "gdiplus" (ByVal Token As Long) As GpStatus
 
Public Type GdiplusStartupInput
   GdiplusVersion As Long
   DebugEventCallback As Long
   SuppressBackgroundThread As Long
   SuppressExternalCodecs As Long
End Type
 
Public Enum GpStatus
   Ok = 0
   GenericError = 1
   InvalidParameter = 2
   OutOfMemory = 3
   ObjectBusy = 4
   InsufficientBuffer = 5
   NotImplemented = 6
   Win32Error = 7
   WrongState = 8
   Aborted = 9
   FileNotFound = 10
   ValueOverflow = 11
   AccessDenied = 12
   UnknownImageFormat = 13
   FontFamilyNotFound = 14
   FontStyleNotFound = 15
   NotTrueTypeFont = 16
   UnsupportedGdiplusVersion = 17
   GdiplusNotInitialized = 18
   PropertyNotFound = 19
   PropertyNotSupported = 20
End Enum
 
Private Declare Function CLSIDFromString Lib "ole32.dll" (ByVal lpszProgID As Long, pCLSID As Clsid) As Long
 
Public Enum ImageType
    BMP
    EMF
    WMF
    JPG
    PNG
    GIF
    TIF
    ICO
End Enum
 
Public Const ImageEncoderSuffix       As String = "-1A04-11D3-9A73-0000F81EF32E}"
Public Const ImageEncoderBMP          As String = "{557CF400" & ImageEncoderSuffix
Public Const ImageEncoderJPG          As String = "{557CF401" & ImageEncoderSuffix
Public Const ImageEncoderGIF          As String = "{557CF402" & ImageEncoderSuffix
Public Const ImageEncoderEMF          As String = "{557CF403" & ImageEncoderSuffix
Public Const ImageEncoderWMF          As String = "{557CF404" & ImageEncoderSuffix
Public Const ImageEncoderTIF          As String = "{557CF405" & ImageEncoderSuffix
Public Const ImageEncoderPNG          As String = "{557CF406" & ImageEncoderSuffix
Public Const ImageEncoderICO          As String = "{557CF407" & ImageEncoderSuffix
Public Const EncoderCompression       As String = "{E09D739D-CCD4-44EE-8EBA-3FBF8BE4FC58}"
Public Const EncoderColorDepth        As String = "{66087055-AD66-4C7C-9A18-38A2310B8337}"
Public Const EncoderScanMethod        As String = "{3A4E2661-3109-4E56-8536-42C156E7DCFA}"
Public Const EncoderVersion           As String = "{24D18C76-814A-41A4-BF53-1C219CCCF797}"
Public Const EncoderRenderMethod      As String = "{6D42C53A-229A-4825-8BB7-5C99E2B9A8B8}"
Public Const EncoderQuality           As String = "{1D5BE4B5-FA4A-452D-9CDD-5DB35105E7EB}"
Public Const EncoderTransformation    As String = "{8D0EB2D1-A58E-4EA8-AA14-108074B7B6F9}"
Public Const EncoderLuminanceTable    As String = "{EDB33BCE-0266-4A77-B904-27216099E717}"
Public Const EncoderChrominanceTable  As String = "{F2E455DC-09B3-4316-8260-676ADA32481C}"
Public Const EncoderSaveFlag          As String = "{292266FC-AC40-47BF-8CFC-A85B89A655DE}"
 
Dim mToken As Long
 
Dim Pens() As Long, PenCount As Long
Dim Brushes() As Long, BrushCount As Long
Dim StrFormats() As Long, StrFormatCount As Long
Dim Matrixes() As Long, MatrixCount As Long



Public MainPen As Long, MainGrp As Long
Dim MainBush As Long
Dim MainDc As Long
Dim MainMarti As Long
Public Type ImgInfo
    Index As Long
    FlatIndex As Long
    w As Long
    h As Long
End Type
Dim MainFontFam As Long, MainStrFormat As Long, MainCurFont As Long, MainFont As Long
Public ImgInfo() As ImgInfo
Public Sub InitGDIPlus()
    Dim uInput As GdiplusStartupInput
    
    uInput.GdiplusVersion = 1
    If GdiplusStartup(mToken, uInput) <> Ok Then
        MsgBox "RuntimeError#0X1422FF16AC", vbCritical, "SystemWs"
        End
    End If
    GdipCreateFromHDC FrmMain.hDc, MainGrp
    GdipCreatePen1 &HFF000000, 5, UnitPixel, MainPen
    GdipCreateSolidFill &HFF000000, MainBush
    
    GdipCreateFontFamilyFromName StrPtr(MyFont), 0, MainFontFam
    GdipCreateStringFormat 0, 0, MainStrFormat
    GdipSetStringFormatAlign MainStrFormat, StringAlignmentCenter
    GdipCreateFont MainFontFam, FrmMain.FontSize, 0, UnitPoint, MainCurFont
    GdipCreateFontFromDC FrmMain.hDc, MainFont
    GdipSetTextRenderingHint MainGrp, TextRenderingHintSystemDefault
    MainMarti = NewMatrix(1, 0, 0, 1, 0, 0)
    ReDim ImgInfo(0)
    GdipSetSmoothingMode MainGrp, SmoothingModeAntiAlias '������ɷ���ݣ������ǲ�Ҫ���ˣ��������ٶ�
   
End Sub

Public Sub TerminateGDIPlus()
On Error Resume Next
GdipDeleteGraphics MainGrp
GdipDeleteBrush MainBush
Dim i As Long
For i = 1 To UBound(ImgInfo)
    UnloadImg i
Next
GdipDeleteStringFormat MainStrFormat
GdipDeleteFont MainCurFont
GdipDeleteFontFamily MainFontFam
GdiplusShutdown mToken

End Sub
Public Sub DwText(Infos As String, X1 As Single, Y1 As Single, Optional Aero As Integer = -1, Optional Colors As Long = vbGreen)
    Dim TempInfos()  As Byte
    TempInfos = Infos
    Dim Sc As String
    Sc = String$(1, 0)
    Dim InfoLength As Long
    InfoLength = (UBound(TempInfos) + 1) \ 2
    Dim Pf() As POINTF, TempBox As RECTF
    Dim i As Long
    ReDim Pf(InfoLength)
    
    If Aero = -1 Then
        GdipDrawDriverString MainGrp, VarPtr(TempInfos(0)), InfoLength, MainCurFont, MainBush, Pf(0), 1, MainMarti
    Else
        Dim TempBush As Long, TempColors As Long
        Dim TempFontFam As Long, TempStrFormat As Long, TempCurFont As Long, TempGrp As Long
        TempColors = GetRGB_VB2GDIP(Colors, Aero)
        GdipCreateSolidFill TempColors, TempBush
    
        Pf(0).x = X1
        Pf(0).y = Y1
        For i = 0 To InfoLength - 1
            CopyMemory ByVal StrPtr(Sc), TempInfos(i * 2), 2
            GdipMeasureDriverString MainGrp, StrPtr(Sc), 1, MainCurFont, Pf(0), 1, MainMarti, TempBox
            With Pf(i + 1)
                .x = Pf(i).x + TempBox.Right
                .y = Y1
            End With
        Next
        GdipDrawDriverString MainGrp, VarPtr(TempInfos(0)), InfoLength, MainCurFont, TempBush, Pf(0), 1, MainMarti
        GdipDeleteBrush TempBush
    End If
End Sub
Public Function NewMatrix(m11 As Single, m12 As Single, m21 As Single, m22 As Single, dx As Single, dy As Single) As Long
    Dim TempMatrixes As Long
     
    GdipCreateMatrix TempMatrixes
    GdipSetMatrixElements TempMatrixes, m11, m12, m21, m22, dx, dy
    NewMatrix = TempMatrixes
End Function
Public Function UnloadImg(ImgIndex As Long)
GdipDisposeImage ImgInfo(ImgIndex).Index
End Function
Public Sub DwEtrImg(Tar As Object, ImgIndex As Long, X1 As Single, Y1 As Single, Width As Single, Height As Single)
Dim TempGrp As Long
GdipCreateFromHDC Tar.hDc, TempGrp
GdipFillRectangle TempGrp, ImgInfo(ImgIndex).FlatIndex, X1, Y1, Width, Height
GdipDeleteGraphics TempGrp
End Sub
Public Function LoadImg(ImgPath As String, Optional Width As Single = -1) As Long
On Error GoTo Err:
Dim TempIndex As Long, img As Long
TempIndex = UBound(ImgInfo) + 1
ReDim Preserve ImgInfo(TempIndex)
With ImgInfo(TempIndex)
    GdipLoadImageFromFile StrConv(ImgPath, vbUnicode), .Index
    If .Index = 0 Then GoTo Err
    GdipGetImageWidth .Index, .w
    GdipGetImageHeight .Index, .h
    LoadImg = TempIndex
    GdipCreateTexture ImgInfo(TempIndex).Index, WrapModeTileFlipX, .FlatIndex
    Exit Function
Err:
    LoadImg = -1
End With

End Function
Public Sub DwImgFlat(ImgIndex As Long, X1 As Single, Y1 As Single, Width As Single, Height As Single)
GdipFillRectangle MainGrp, ImgInfo(ImgIndex).FlatIndex, X1, Y1, Width, Height
End Sub
Public Function DwImg(ImgIndex As Long, X1 As Single, Y1 As Single, Optional Width As Single = -1, Optional Height As Single = -1) As Single
If ImgIndex = -1 Then Exit Function
If Width = 0 Then Width = -1
If Height = 0 Then Height = -1
If Width = -1 And Height = -1 Then
    GdipDrawImageRectI MainGrp, ImgInfo(ImgIndex).Index, X1, Y1, ImgInfo(ImgIndex).w, ImgInfo(ImgIndex).h
    DwImg = ImgInfo(ImgIndex).w
ElseIf Height = -1 And Width > -1 Then
    DwImg = ImgInfo(ImgIndex).h * Width / ImgInfo(ImgIndex).w
    GdipDrawImageRectI MainGrp, ImgInfo(ImgIndex).Index, X1, Y1, Width, ImgInfo(ImgIndex).h * Width / ImgInfo(ImgIndex).w
ElseIf Width = -1 And Height > -1 Then
    DwImg = ImgInfo(ImgIndex).w * Height / ImgInfo(ImgIndex).h
    GdipDrawImageRectI MainGrp, ImgInfo(ImgIndex).Index, X1, Y1, ImgInfo(ImgIndex).w * Height / ImgInfo(ImgIndex).h, Height
Else
    GdipDrawImageRectI MainGrp, ImgInfo(ImgIndex).Index, X1, Y1, Width, Height
End If
End Function
Public Sub FillOval(X1 As Single, Y1 As Single, Width As Single, Height As Single, Optional Aero As Integer = -1, Optional Colors As Long = vbGreen)
    If Aero = -1 Then
        GdipFillEllipseI MainGrp, MainBush, X1, Y1, Width, Height
    Else
        Dim TempBush As Long, TempColors As Long
        TempColors = GetRGB_VB2GDIP(Colors, Aero)
        GdipCreateSolidFill TempColors, TempBush
        GdipFillEllipseI MainGrp, TempBush, X1, Y1, Width, Height
        GdipDeleteBrush TempBush
    End If
End Sub
Public Sub FillRect(X1 As Single, Y1 As Single, Width As Single, Height As Single, Optional Aero As Integer = -1, Optional Colors As Long = vbGreen)
    If Aero = -1 Then
        GdipFillRectangleI MainGrp, MainBush, X1, Y1, Width, Height
    Else
        Dim TempBush As Long, TempColors As Long
        TempColors = GetRGB_VB2GDIP(Colors, Aero)
        GdipCreateSolidFill TempColors, TempBush
        GdipFillRectangleI MainGrp, TempBush, X1, Y1, Width, Height
        GdipDeleteBrush TempBush
    End If
End Sub
Public Sub DwRect(X1 As Single, Y1 As Single, Width As Single, Height As Single, Optional w As Single = 2, Optional Aero As Integer = -1, Optional Colors As Long = vbGreen)
    If Aero = -1 Then
        GdipDrawRectangleI MainGrp, MainPen, X1, Y1, Width, Height
    Else
        Dim TempPen As Long, TempColors As Long
        TempColors = GetRGB_VB2GDIP(Colors, Aero)
        GdipCreatePen1 TempColors, w, UnitPixel, TempPen
        GdipDrawRectangle MainGrp, TempPen, X1, Y1, Width, Height
        GdipDeletePen TempPen
    End If
End Sub
Public Sub DwLine(X1 As Single, Y1 As Single, X2 As Single, Y2 As Single, Optional w As Single = 2, Optional Aero As Integer = -1, Optional Colors As Long = vbGreen)
    If Aero = -1 Then
        GdipDrawLineI MainGrp, MainPen, X1, Y1, X2, Y2
    Else
        Dim TempPen As Long, TempColors As Long
        TempColors = GetRGB_VB2GDIP(Colors, Aero)
        GdipCreatePen1 TempColors, w, UnitPixel, TempPen
        GdipDrawLineI MainGrp, TempPen, X1, Y1, X2, Y2
        GdipDeletePen TempPen
    End If
End Sub
Public Function GetPixSum(TwipNum As Single) As Single
GetPixSum = TwipNum / 15
End Function


Public Function ColorARGB(ByVal alpha As Byte, ByVal Red As Byte, ByVal Green As Byte, ByVal Blue As Byte) As Long
   Dim bytestruct As COLORBYTES
   Dim Result As COLORLONG
   
   With bytestruct
      .AlphaByte = alpha
      .RedByte = Red
      .GreenByte = Green
      .BlueByte = Blue
   End With
   
   LSet Result = bytestruct
   ColorARGB = Result.longval
End Function

Public Function ColorSetAlpha(ByVal lColor As Long, ByVal alpha As Byte) As Long
   Dim bytestruct As COLORBYTES
   Dim Result As COLORLONG
   
   Result.longval = lColor
   LSet bytestruct = Result

   bytestruct.AlphaByte = alpha

   LSet Result = bytestruct
   ColorSetAlpha = Result.longval
End Function

' Pass a GDI+ color to this function and get the VB compatible color
Public Function GetRGB_GDIP2VB(ByVal lColor As Long) As Long
   Dim argb As COLORBYTES
   CopyMemory argb, lColor, 4
   GetRGB_GDIP2VB = RGB(argb.RedByte, argb.GreenByte, argb.BlueByte)
End Function

' Pass a VB/standard color to this function and get the GDI+ compatible color
Public Function GetRGB_VB2GDIP(ByVal lColor As Long, Optional ByVal alpha As Byte = 255) As Long
   Dim rgbq As RGBQUAD
   CopyMemory rgbq, lColor, 4
   ' I must have done something wrong, but swapping Red and Blue works...
   GetRGB_VB2GDIP = ColorARGB(alpha, rgbq.rgbBlue, rgbq.rgbGreen, rgbq.rgbRed)
End Function




' Courtesy of: Dana Seaman
' Helper routine to convert a CLSID(aka GUID) string to a structure
Public Function DEFINE_GUID(ByVal sGuid As String) As Clsid
   ' Example ImageFormatBMP = {B96B3CAB-0728-11D3-9D7B-0000F81EF32E}
   Call CLSIDFromString(StrPtr(sGuid), DEFINE_GUID)
End Function

Public Function DeleteObjects()
    Dim i As Long
    For i = 1 To PenCount: GdipDeletePen Pens(i): Next
    For i = 1 To BrushCount: GdipDeleteBrush Brushes(i): Next
    For i = 1 To StrFormatCount: GdipDeleteStringFormat StrFormats(i): Next
    For i = 1 To MatrixCount: GdipDeleteMatrix Matrixes(i): Next
    PenCount = 0
    BrushCount = 0
    StrFormatCount = 0
    MatrixCount = 0
End Function
 
Public Function NewPen(Color As Long, Width As Single) As Long
    PenCount = PenCount + 1
    ReDim Preserve Pens(PenCount)
     
    GdipCreatePen1 Color, Width, UnitPixel, Pens(PenCount)
    NewPen = Pens(PenCount)
End Function
 
Public Function NewBrush(Color As Long) As Long
    BrushCount = BrushCount + 1
    ReDim Preserve Brushes(BrushCount)
     
    GdipCreateSolidFill Color, Brushes(BrushCount)
    NewBrush = Brushes(BrushCount)
End Function
 
Public Function NewStringFormat(Align As StringAlignment) As Long
    StrFormatCount = StrFormatCount + 1
    ReDim Preserve StrFormats(StrFormatCount)
     
    GdipCreateStringFormat 0, 0, StrFormats(StrFormatCount)
    GdipSetStringFormatAlign StrFormats(StrFormatCount), Align
    NewStringFormat = StrFormats(StrFormatCount)
End Function
 
 
Public Function NewRectF(Left As Single, Top As Single, Width As Single, Height As Single) As RECTF
    With NewRectF
        .Left = Left
        .Top = Top
        .Right = Width
        .Bottom = Height
    End With
End Function
 
Public Function NewRectL(Left As Single, Top As Long, Width As Long, Height As Long) As RECTL
    With NewRectL
        .Left = Left
        .Top = Top
        .Right = Width
        .Bottom = Height
    End With
End Function

Public Function GetImageEncoderClsid(ByVal ImageType As ImageType) As Clsid
    Select Case ImageType
        Case PNG: CLSIDFromString StrPtr(ImageEncoderPNG), GetImageEncoderClsid
        Case JPG: CLSIDFromString StrPtr(ImageEncoderJPG), GetImageEncoderClsid
        Case GIF: CLSIDFromString StrPtr(ImageEncoderGIF), GetImageEncoderClsid
        Case BMP: CLSIDFromString StrPtr(ImageEncoderBMP), GetImageEncoderClsid
        Case ICO: CLSIDFromString StrPtr(ImageEncoderICO), GetImageEncoderClsid
        Case EMF: CLSIDFromString StrPtr(ImageEncoderEMF), GetImageEncoderClsid
        Case WMF: CLSIDFromString StrPtr(ImageEncoderWMF), GetImageEncoderClsid
        Case TIF: CLSIDFromString StrPtr(ImageEncoderTIF), GetImageEncoderClsid
    End Select
End Function
 
Public Function SaveImageToPNG(ByVal Image As Long, ByVal Path As String) As GpStatus
    SaveImageToPNG = GdipSaveImageToFile(Image, StrPtr(Path), GetImageEncoderClsid(PNG), ByVal 0)
End Function
 
Public Function SaveImageToJPG(ByVal Image As Long, ByVal Path As String, ByVal Quality As Long) As GpStatus
    Dim Params As EncoderParameters
     
    Params.Count = 1
    CLSIDFromString StrPtr(EncoderQuality), Params.Parameter.Guid
    Params.Parameter.NumberOfValues = 1
    Params.Parameter.Type = 4
    Params.Parameter.Value = VarPtr(Quality)
     
    SaveImageToJPG = GdipSaveImageToFile(Image, StrPtr(Path), GetImageEncoderClsid(JPG), Params)
End Function
 
Public Function SaveImageToGIF(ByVal Image As Long, ByVal Path As String) As GpStatus
    SaveImageToGIF = GdipSaveImageToFile(Image, StrPtr(Path), GetImageEncoderClsid(GIF), ByVal 0)
End Function
 
Public Function SaveImageToBMP(ByVal Image As Long, ByVal Path As String) As GpStatus
    SaveImageToBMP = GdipSaveImageToFile(Image, StrPtr(Path), GetImageEncoderClsid(BMP), ByVal 0)
End Function
 
Public Function CreateBitmap(ByRef bitmap As Long, ByVal Width As Long, ByVal Height As Long, Optional ByVal PixelFormat As GpPixelFormat = PixelFormat32bppARGB) As GpStatus
    GdipCreateBitmapFromScan0 Width, Height, 0, PixelFormat, ByVal 0, bitmap
End Function
 
Public Function CreateBitmapWithGraphics(ByRef bitmap As Long, ByRef graphics As Long, ByVal Width As Long, ByVal Height As Long, Optional ByVal PixelFormat As GpPixelFormat = PixelFormat32bppARGB) As GpStatus
    GdipCreateBitmapFromScan0 Width, Height, 0, PixelFormat, ByVal 0, bitmap
    GdipGetImageGraphicsContext bitmap, graphics
End Function
