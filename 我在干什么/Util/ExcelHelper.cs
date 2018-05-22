using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop;
using System;
using System.Reflection;

namespace DotNet4.Utilities.UtilExcel
{
	public class ExcelBase:IDisposable
	{
		#region " Private Variable Definition "

		private Application exlApp;
		private _Workbook exlWorkBook;
		private _Worksheet exlWorkSheet;
		private int sheetNumber = 1;

		#endregion

		#region " Public Property and Constant Definition "

		/// <summary>
		/// Excel单元格边框的线条的粗细枚举
		/// </summary>
		public enum ExcelBorderWeight
		{
			/// <summary>
			/// 极细的线条
			/// </summary>
			Hairline =Microsoft.Office.Interop.Excel.XlBorderWeight.xlHairline,
			/// <summary>
			/// 中等的线条
			/// </summary>
			Medium =Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
			/// <summary>
			/// 粗线条
			/// </summary>
			Thick =Microsoft.Office.Interop.Excel.XlBorderWeight.xlThick,
			/// <summary>
			/// 细线条
			/// </summary>
			Thin =Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin
		}

		/// <summary>
		/// Excel单元格边框枚举
		/// </summary>
		public enum ExcelBordersIndex
		{
			/// <summary>
			/// 主对角线从
			/// </summary>
			DiagonalDown =Microsoft.Office.Interop.Excel.XlBordersIndex.xlDiagonalDown,
			/// <summary>
			/// 辅对角线
			/// </summary>
			DiagonUp =Microsoft.Office.Interop.Excel.XlBordersIndex.xlDiagonalUp,
			/// <summary>
			///底边框
			/// </summary>
			EdgeBottom =Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom,
			/// <summary>
			/// 左边框
			/// </summary>
			EdgeLeft =Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft,
			/// <summary>
			/// 右边框
			/// </summary>
			EdgeRight =Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight,
			/// <summary>
			/// 顶边框
			/// </summary>
			EdgeTop =Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop,
			/// <summary>
			/// 边框内水平横线
			/// </summary>
			InsideHorizontal =Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal,
			/// <summary>
			/// 边框内垂直竖线
			/// </summary>
			InsideVertical =Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical
		}

		/// <summary>
		/// Excel单元格的竖直方法对齐枚举
		/// </summary>
		public enum ExcelVerticalAlignment
		{
			/// <summary>
			/// 居中
			/// </summary>
			Center =Microsoft.Office.Interop.Excel.Constants.xlCenter,
			/// <summary>
			/// 靠上
			/// </summary>
			Top =Microsoft.Office.Interop.Excel.Constants.xlTop,
			/// <summary>
			/// 靠下
			/// </summary>
			Bottom =Microsoft.Office.Interop.Excel.Constants.xlBottom,
			/// <summary>
			/// 两端对齐
			/// </summary>
			Justify =Microsoft.Office.Interop.Excel.Constants.xlJustify,
			/// <summary>
			/// 分散对齐
			/// </summary>
			Distributed =Microsoft.Office.Interop.Excel.Constants.xlDistributed

		};

		/// <summary>
		/// Excel 水平方向对齐枚举
		/// </summary>
		public enum ExcelHorizontalAlignment
		{
			/// <summary>
			///常规
			/// </summary>
			General =Microsoft.Office.Interop.Excel.Constants.xlGeneral,
			/// <summary>
			/// 靠左
			/// </summary>
			Left =Microsoft.Office.Interop.Excel.Constants.xlLeft,
			/// <summary>
			/// 居中
			/// </summary>
			Center =Microsoft.Office.Interop.Excel.Constants.xlCenter,
			/// <summary>
			/// 靠右
			/// </summary>
			Right =Microsoft.Office.Interop.Excel.Constants.xlRight,
			/// <summary>
			/// 填充
			/// </summary>
			Fill =Microsoft.Office.Interop.Excel.Constants.xlFill,
			/// <summary>
			/// 两端对齐
			/// </summary>
			Justify =Microsoft.Office.Interop.Excel.Constants.xlJustify,
			/// <summary>
			/// 跨列居中
			/// </summary>
			CenterAcrossSelection =Microsoft.Office.Interop.Excel.Constants.xlCenterAcrossSelection,
			/// <summary>
			/// 分散对齐
			/// </summary>
			Distributed =Microsoft.Office.Interop.Excel.Constants.xlDistributed

		}


		/// <summary>
		/// Excel边框线条的枚举
		/// </summary>
		public enum ExcelStyleLine
		{
			/// <summary>
			/// 没有线条
			/// </summary>
			StyleNone =Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone,
			/// <summary>
			/// 连续的细线
			/// </summary>
			Continious =Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
			/// <summary>
			/// 点状线
			/// </summary>
			Dot =Microsoft.Office.Interop.Excel.XlLineStyle.xlDot,
			/// <summary>
			/// 双条线
			/// </summary>
			Double =Microsoft.Office.Interop.Excel.XlLineStyle.xlDouble,
		}

		/// <summary>
		/// 排序的玫举
		/// </summary>
		public enum ExcelSortOrder
		{
			/// <summary>
			/// 升序
			/// </summary>
			Ascending =Microsoft.Office.Interop.Excel.XlSortOrder.xlAscending,
			/// <summary>
			/// 降序
			/// </summary>
			Descending = Microsoft.Office.Interop.Excel.XlSortOrder.xlDescending,
		}



		#endregion

		#region " Construction Method "

		/// <summary>
		/// 默认初始化
		/// </summary>
		public ExcelBase()
		{
			//实例化Excel对象。
			ExlApp = new Microsoft.Office.Interop.Excel.Application();
		}


		#endregion

		#region " Open and dispose method definition "

		/// <summary>
		/// 打开一个Excel文件
		/// </summary>
		public void Open()
		{
			//Get a new WorkSheet
			ExlWorkBook = (Workbook)ExlApp.Workbooks.Add(Missing.Value);
			ExlWorkSheet = (Worksheet)ExlWorkBook.ActiveSheet;
		}

		/// <summary>
		/// 打开已经存在的Excel文件模版
		/// </summary>
		/// <param name="XLTPath">已经存在的文件模版的完整路径</param>
		public void Open(string XLTPath)
		{
			
			if (System.IO.File.Exists(XLTPath))
			{
				ExlWorkBook = (Workbook)ExlApp.Workbooks.Add(XLTPath);
				ExlWorkSheet = (Worksheet)ExlWorkBook.ActiveSheet;
			}
			else
			{
				throw new System.IO.FileNotFoundException(string.Format("{0}不存在，请重新确定文件名", XLTPath));
			}
		}

		/// <summary>
		/// 保存Excel文件
		/// </summary>
		/// <param name="fileName">保存的文件名</param>
		public void SaveAs(string fileName)
		{
			if (ExlWorkSheet == null)
			{
				throw new Exception("未初始化的表格");
			}
			try
			{
				ExlWorkSheet.SaveAs(fileName, Missing.Value, Missing.Value, Missing.Value, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value);
			}
			catch (Exception ex)
			{

				Console.WriteLine("ExcelSaveAs()"+ex.Message);
			}

		}

		#endregion

		#region " Print and PrintPreview method definition "

		/// <summary>
		/// 打印Excel文件,可以设置是否是打印前预览打印的Excel文件
		/// </summary>
		/// <param name="IsPrintPreview">打印前是否预览 , true:打印前预览false:直接打印,不预览 </param>
		public void Print(bool IsPrintPreview)
		{
			bool flag = ExlApp.Visible;
			if (ExlApp.Visible)
			{
				ExlApp.Visible = true;
			}
			ExlWorkSheet.PrintOut(Missing.Value, Missing.Value, Missing.Value, IsPrintPreview, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
			ExlApp.Visible = flag;
		}

		/// <summary>
		/// 打印Excel文件,可以设置是否打印预览，以及打印的份数
		/// </summary>
		/// <param name="IsPrintPreview">打印前是否预览 , true:打印前预览false:直接打印,不预览</param>
		/// <param name="iCopy">打印的份数</param>
		public void Print(bool IsPrintPreview, int iCopy)
		{
			if (iCopy < 1)
			{
				iCopy = 1;
			}
			ExlWorkSheet.PrintOut(Missing.Value, Missing.Value, Missing.Value, IsPrintPreview, iCopy, Missing.Value, Missing.Value, Missing.Value);
		}

		/// <summary>
		/// 打印预览Excel文件
		/// </summary>
		public void PrintPreview()
		{
			ExlWorkSheet.PrintPreview(Missing.Value);
		}

		#endregion

		#region " Detail control excel method "

		public bool Visiable
		{
			set => ExlApp.Visible=value;get => ExlApp.Visible;
		}

		/// <summary>
		/// 工作簿的名称
		/// </summary>
		public string WorkSheetName
		{
			get
			{
				return ExlWorkSheet?.Name;
			}
			set
			{
				if (ExlWorkSheet == null) return;
				ExlWorkSheet.Name = value;
			}
		}

		public Application ExlApp { get => exlApp; set => exlApp = value; }
		public _Workbook ExlWorkBook { get => exlWorkBook; set => exlWorkBook = value; }
		public _Worksheet ExlWorkSheet { get => exlWorkSheet; set => exlWorkSheet = value; }

		/// <summary>
		///返回指定单元格的内容
		/// </summary>
		/// <param name="iRow">定位的行</param>
		/// <param name="iCol">定位的列</param>
		/// <returns>返回指定单元格的内容</returns>
		public string GetCellText(int iRow, int iCol)
		{
			Range sRange = GetRange(iRow, iCol, iRow, iCol);
			string returnText = (string)sRange.Text;
			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;
			return returnText;
		}

		/// <summary>
		///返回指定单元格的内容
		/// </summary>
		/// <param name="iRow">定位的行</param>
		/// <param name="iCol">定位的列</param>
		/// <returns>返回指定单元格的内容</returns>
		public string GetCellText(int startRow, int startCol, int startRow2, int startCol2)
		{
			Range sRange = GetRange(startRow, startCol, startRow2, startCol2);
			string returnText = (string)sRange.Text;
			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;
			return returnText;
		}


		/// <summary>
		/// 设置指定范围单元格的内容,通过单元格，比如从"A1" 到 "B3"
		/// </summary>
		/// <param name="startCell">开始的单元格,比如"A1"</param>
		/// <param name="endCell">结束的单元格,比如"B2"</param>机动车统计表.xlt
		/// <param name="text">要设置的内容，可以使用Excel的公式</param>
		public void SetCellText(string startCell, string endCell, string text)
		{
			Range sRange = ExlWorkSheet.get_Range(startCell, endCell);
			//这里没有用value属性，而用Formula属性，因为考虑到可以扩展，可以利用公式
			sRange.Cells.Formula = text;
			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;
		}

		/// <summary>
		/// 设置指定范围单元格的内容,通过单元格，比如从"A1" 到 "B3"
		/// </summary>
		/// <param name="startCell">开始的单元格,比如"A1"</param>
		/// <param name="endCell">结束的单元格,比如"B2"</param>机动车统计表.xlt
		/// <param name="text">要设置的内容，可以使用Excel的公式</param>
		public void SetCellText(string startCell, string endCell, int text)
		{
			Range sRange = ExlWorkSheet.get_Range(startCell, endCell);
			//这里没有用value属性，而用Formula属性，因为考虑到可以扩展，可以利用公式
			sRange.Cells.Formula = text.ToString();
			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;
		}

		/// <summary>
		/// 设置指定范围的单元格的内容,通过行列来定位。如第1行第2列内容
		/// </summary>
		/// <param name="iRow">开始的行</param>
		/// <param name="iCol">开始的列</param>
		///<param name="text">要设置的文本，可以使用Excel的公式</param>
		public void SetCellText(int iRow, int iCol, string text)
		{
			Range sRange = this.GetRange(iRow, iCol, iRow, iCol);
			sRange.Cells.Formula = text;
			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;
		}


		/// <summary>
		/// 设置指定范围的单元格的内容,通过行列来定位。如第1行第2列内容
		/// </summary>
		/// <param name="iRow">开始的行</param>
		/// <param name="iCol">开始的列</param>
		///<param name="text">要设置的文本，可以使用Excel的公式</param>
		public void SetCellTextNOZero(int iRow, int iCol, string text)
		{
			string txt = "";
			try
			{
				if (System.Convert.ToInt32(text) == 0)
				{
					txt = "";
				}
				else
				{
					txt = text;
				}
			}
			catch
			{
				txt = text;
			}


			Range sRange = this.GetRange(iRow, iCol, iRow, iCol);
			sRange.Cells.Formula = txt;
			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;

		}

		/// <summary>
		/// 设置指定范围的单元格的内容,通过行列来定位。如第1行第2列内容
		/// </summary>
		/// <param name="iRow">开始的行</param>
		/// <param name="iCol">开始的列</param>
		///<param name="text">要设置的文本，可以使用Excel的公式</param>
		public void SetCellText(int iRow, int iCol, int text)
		{
			Range sRange = this.GetRange(iRow, iCol, iRow, iCol);
			sRange.Cells.Formula = text;
			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;
		}
		/// <summary>
		/// 设置指定单元格的内容,比如设置"A1"单元格的内容
		/// </summary>
		/// <param name="cell">指定的单元格</param>
		/// <param name="text">要设置的内容，可以使用Excel的公式，如sum(A1:A7)--合计A1到A7数值</param>
		public void SetCellText(string cell, string text)
		{
			Range sRange = GetRange(cell);
			sRange.Cells.Formula = text;
			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;
		}

		/// <summary>
		/// 设置指定单元格的内容
		/// </summary>
		/// <param name="cell">指定的单元格</param>
		/// <param name="num">要设置的内容</param>
		public void SetCellText(string cell, Int32 num)
		{
			Range sRange = GetRange(cell);
			sRange.Cells.Formula = num.ToString();
			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;
		}

		/// <summary>
		/// 设置指定单元格的内容，可以指定格式
		/// </summary>
		/// <param name="cell">要指定的单元格</param>
		/// <param name="textValue">要填写的内容</param>
		/// <param name="StringFormat">要显示的格式</param>
		///<param name="FontName">设置单元格的字体</param>
		/// <param name="FontSize">设置单元格的字体大小</param>
		public void SetCellTextByFormat(string cell, string textValue, string StringFormat, string FontName, string FontSize)
		{
			Range sRange = GetRange(cell);
			sRange.Select();
			if (StringFormat != "")
			{
				sRange.NumberFormatLocal = StringFormat;
			}
			if (FontName != "")
			{
				sRange.Font.Name = FontName;
			}
			if (FontSize != "")
			{
				sRange.Font.Size = FontSize;
			}
			sRange.Cells.Formula = textValue;


			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;
		}

		/// <summary>
		/// 设置指定单元格的内容，可以指定格式
		/// </summary>
		/// <param name="cell">要指定的单元格</param>
		/// <param name="textValue">要填写的内容</param>
		/// <param name="StringFormat">要显示的格式</param>
		///<param name="FontName">设置单元格的字体</param>
		/// <param name="FontSize">设置单元格的字体大小</param>
		/// <param name="colorIndex">设置单元格的颜色，我查了MSDN但是没有颜色代码的说明,Excel中一共有56种颜色的代码，常用的几个是
		/// 1-黑色 2-白色 3-红色 4-草绿色 5-蓝色 6-黄色 7-紫色 ，如果想看仔细的颜色，就依次从 1 循环到 56 把颜色打印出来看看</param>
		public void SetCellTextByFormat(string cell, string textValue, string StringFormat, string FontName, string FontSize, int colorIndex)
		{
			Range sRange = GetRange(cell);
			sRange.Select();
			if (StringFormat != "")
			{
				sRange.Cells.NumberFormatLocal = StringFormat;
			}
			if (FontName != "")
			{
				sRange.Font.Name = FontName;
			}
			if (FontSize != "")
			{
				sRange.Font.Size = FontSize;
			}
			if (colorIndex != 0)
			{
				sRange.Font.ColorIndex = colorIndex;
			}
			sRange.Cells.Formula = textValue;

			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;
		}

		/// <summary>
		/// 设置指定单元格的内容，可以指定格式
		/// </summary>
		/// <param name="cell">要指定的单元格</param>
		/// <param name="textValue">要填写的内容</param>
		/// <param name="StringFormat">要显示的格式</param>
		/// <param name="FontName">设置单元格的字体</param>
		/// <param name="FontSize">设置单元格的字体大小</param>
		/// <param name="colorIndex">设置单元格的颜色，我查了MSDN但是没有颜色代码的说明,Excel中一共有56种颜色的代码，常用的几个是
		/// 1-黑色 2-白色 3-红色 4-草绿色 5-蓝色 6-黄色 7-紫色 ，如果想看仔细的颜色，就依次从 1 循环到 56 把颜色打印出来看看</param>
		/// <param name="Bold">设置单元格的字体是否粗体</param>
		public void SetCellTextByFormat(string cell, string textValue, string StringFormat, string FontName, string FontSize, int colorIndex, bool Bold)
		{
			Range sRange = GetRange(cell);
			sRange.Select();
			if (StringFormat != "")
			{
				sRange.Cells.NumberFormatLocal = StringFormat;
			}
			if (FontName != "")
			{
				sRange.Font.Name = FontName;
			}
			if (FontSize != "")
			{
				sRange.Font.Size = FontSize;
			}
			if (colorIndex != 0)
			{
				sRange.Font.ColorIndex = colorIndex;
			}
			sRange.Font.Bold = Bold;
			sRange.Cells.Formula = textValue;

			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;
		}

		/// <summary>
		/// 设置指定单元格的内容，可以指定格式
		/// </summary>
		/// <param name="cell">要指定的单元格</param>
		/// <param name="textValue">要填写的内容</param>
		/// <param name="StringFormat">要显示的格式</param>
		/// <param name="FontName">设置单元格的字体</param>
		/// <param name="FontSize">设置单元格的字体大小</param>
		/// <param name="colorIndex">设置单元格的颜色，我查了MSDN但是没有颜色代码的说明,Excel中一共有56种颜色的代码，常用的几个是
		/// 1-黑色 2-白色 3-红色 4-草绿色 5-蓝色 6-黄色 7-紫色 ，如果想看仔细的颜色，就依次从 1 循环到 56 把颜色打印出来看看</param>
		/// <param name="Bold">设置单元格的字体是否粗体</param>
		/// <param name="BcolorIndex">设置单元格背景颜色</param>
		public void SetCellTextByFormat(string cell, string textValue, string StringFormat, string FontName, string FontSize, int colorIndex, bool Bold, int BcolorIndex)
		{
			Range sRange = GetRange(cell);
			sRange.Select();
			if (StringFormat != "")
			{
				sRange.Cells.NumberFormatLocal = StringFormat;
			}
			if (FontName != "")
			{
				sRange.Font.Name = FontName;
			}
			if (FontSize != "")
			{
				sRange.Font.Size = FontSize;
			}
			if (colorIndex != 0)
			{
				sRange.Font.ColorIndex = colorIndex;
			}
			sRange.Font.Bold = Bold;
			sRange.Cells.Formula = textValue;
			sRange.Interior.ColorIndex = BcolorIndex;

			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;
		}


		/// <summary>
		/// 设置单元格的内容（指定单元格的格式化字符串）
		/// </summary>
		/// <param name="cell">指定的单元格</param>
		/// <param name="textValue">内容</param>
		/// <param name="stringFormat">格式化字符串</param>
		public void setCellText(string cell, string textValue, string stringFormat)
		{
			Range sRange = GetRange(cell);
			sRange.Select();
			if (stringFormat != "")
			{
				sRange.Cells.NumberFormatLocal = stringFormat;
			}
			sRange.Cells.Formula = textValue;
			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;
		}
		/// <summary>
		/// 得到指定单元格的内容
		/// </summary>
		/// <param name="cell">指定的单元格比如 A1,A2</param>
		/// <returns>返回指定的内容</returns>
		public object GetCellText(string cell)
		{
			object returnValue;
			Range sRange = GetRange(cell);
			returnValue = sRange.Cells.Text;
			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;
			return returnValue;
		}

		/// <summary>
		/// 设置指定单元格的内容，比如设置"A1"单元格的内容
		/// </summary>
		/// <param name="cell">指定的单元格</param>
		/// <param name="text">要设置的内容，使用Excel里面的R1C1这样的格式(不知道是不是画蛇添足，因为Excel里的Macro中是这样使用的)</param>
		public void SetCellTextR1C1(string cell, string text)
		{
			Range sRange = GetRange(cell);
			sRange.Cells.FormulaR1C1 = text;
			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;
		}

		/// <summary>
		///设置单元格的单元格格式
		/// </summary>
		/// <param name="cell">要设定的单元格的坐标</param>
		/// <param name="formatString">单元格的格式化字符   
		/// 常规："G/通用格式"
		/// 数值："[红色]-0.00"(表示是2位小数，如果是负数的话则用红色表示)
		/// 货币："￥#,##0.000;[红色]￥-#,##0.000"(￥是货币符号，可以用$,也可以不填写，0.000代表三位小数位；[红色]表示如果是负数的话，用红色表示)
		/// 日期：@"yyyy"年"m"月"d"日";@"    (表示用年月日了表示)        @"[DBNum1][$-804]yyyy"年"m"月"d"日";@"(表示用汉字表示年月日)
		/// 百分比："0.000%;[红色]-0.000%"(表示小数位为3位,红色表示如果是负数的话则用红色表示)
		/// 文本："@"(表示是文本格式)
		/// 特殊："[DBNum1][$-804]G/通用格式"(能将数字转换成中文小写，如1234转换成一千二百三十四)        "[DBNum2][$-804]G/通用格式"(能将数字转换成中文大写，如1234转换成 壹仟贰佰叁拾肆)
		/// 自定义：输入自定义的格式化字符串
		/// </param>
		public void SetCellFormat(string cell, string formatString)
		{
			Range sRange = GetRange(cell);
			sRange.Select();
			sRange.NumberFormatLocal = formatString;
			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;
		}

		/// <summary>
		/// 设置指定范围的单元格格式
		/// </summary>
		/// <param name="startCell">开始的单元格</param>
		/// <param name="endCell">结束的单元格</param>
		/// <param name="formatString">单元格的格式化字符   
		/// 常规："G/通用格式"
		/// 数值："[红色]-0.00"(表示是2位小数，如果是负数的话则用红色表示)
		/// 货币："￥#,##0.000;[红色]￥-#,##0.000"(￥是货币符号，可以用$,也可以不填写，0.000代表三位小数位；[红色]表示如果是负数的话，用红色表示)
		/// 日期：@"yyyy"年"m"月"d"日";@"    (表示用年月日了表示)        @"[DBNum1][$-804]yyyy"年"m"月"d"日";@"(表示用汉字表示年月日)
		/// 百分比："0.000%;[红色]-0.000%"(表示小数位为3位,红色表示如果是负数的话则用红色表示)
		/// 文本："@"(表示是文本格式)
		/// 特殊："[DBNum1][$-804]G/通用格式"(能将数字转换成中文小写，如1234转换成一千二百三十四)        "[DBNum2][$-804]G/通用格式"(能将数字转换成中文大写，如1234转换成 壹仟贰佰叁拾肆)
		/// 自定义：输入自定义的格式化字符串</param>
		public void SetAreaCellFormat(string startCell, string endCell, string formatString)
		{
			Range sRange = GetRange(startCell, endCell);
			sRange.Select();
			sRange.NumberFormatLocal = formatString;
			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;
		}

		/// <summary>
		/// 设置某一列，某几列的列宽为自动适应大小，比如要设置第1列为自动适应大小SetColumnAutoFit("A","A")
		/// </summary>
		/// <param name="startColumn">开始的列</param>
		/// <param name="endColumn">结束的列</param>
		///
		public void SetColumnAutoFit(string startColumn, string endColumn)
		{
			Range sRange = (Range)ExlWorkSheet.Columns[String.Format("{0}:{1}", startColumn, endColumn), Missing.Value];
			sRange.Select();
			sRange.EntireColumn.AutoFit();
			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;
		}
		#endregion

		#region " Get excel range method definition "

		/// <summary>
		/// 根据行列的定位，返回选定的单元格。因为Range 是通过Cell来定位的，而Cell需要2个参数定位，所以需要四个参数。               
		/// </summary>
		/// <param name="iStartRow">定位开始Range的Cell的行</param>
		/// <param name="iStartCol">定位开始Range的Cell的列</param>
		/// <param name="iEndRow">定位结束Range的Cell的行</param>
		/// <param name="iEndCol">定位结束Range的Cell的列</param>
		/// <returns>返回指定范围的Range</returns>
		public Range GetRange(int iStartRow, int iStartCol, int iEndRow, int iEndCol)
		{
			return ExlWorkSheet.get_Range(ExlApp.Cells[iStartRow, iStartCol], ExlApp.Cells[iEndRow, iEndCol]);
		}

		/// <summary>
		/// 返回指定的单元格
		/// </summary>
		/// <param name="cell">指定的单元格</param>
		/// <returns>返回指定的单元格</returns>
		public Range GetRange(string cell)
		{
			return ExlWorkSheet.get_Range(cell, Missing.Value);
		}

		/// <summary>
		/// 返回一个单元格的范围
		/// </summary>
		/// <param name="startCell">开始的单元格坐标</param>
		/// <param name="endCell">结束的单元格坐标</param>
		/// <returns>返回指定的单元格范围</returns>
		public Range GetRange(string startCell, string endCell)
		{
			return ExlWorkSheet.get_Range(startCell, endCell);
		}

		/// <summary>
		/// 增加一个工作簿
		/// </summary>
		public void AddWorkSheet()
		{
			if (this.sheetNumber <= 3)
			{
				ExlApp.ActiveWorkbook.Sheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
				ExlWorkSheet = (Worksheet)ExlWorkBook.ActiveSheet;
				ExlWorkSheet.Select(Missing.Value);
			}
			else
			{
				sheetNumber++;
				ExlApp.ActiveWorkbook.Sheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
				ExlWorkSheet = (Worksheet)ExlWorkBook.ActiveSheet;
				ExlWorkSheet.Select(Missing.Value);
			}
			//exlWorkBook.ActiveSheet;

		}


		#endregion

		#region " Excel range style method definition "
		/// <summary>
		/// 设置单元格的垂直方向对齐方式
		/// </summary>
		/// <param name="cell">指定的单元格</param>
		/// <param name="cellAlignment">垂直方向的对齐方式</param>
		public void SetCellVerticalAlignment(string cell, ExcelVerticalAlignment cellAlignment)
		{
			Range sRange = GetRange(cell);
			sRange.Select();
			sRange.VerticalAlignment = cellAlignment;
			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;
		}

		/// <summary>
		/// 设定指定范围的单元格的垂直对齐方式
		/// </summary>
		/// <param name="startCell">开始的单元格的坐标</param>
		/// <param name="endCell">结束单元格的坐标</param>
		/// <param name="cellAlignment">对齐方式</param>
		public void SetCellAreaVerticalAlignment(string startCell, string endCell, ExcelVerticalAlignment cellAlignment)
		{
			Range sRange = GetRange(startCell, endCell);
			sRange.Select();
			sRange.VerticalAlignment = cellAlignment;
			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;
		}


		/// <summary>
		/// 设置指定范围的单元格的水平方向的对齐方式
		/// </summary>
		/// <param name="cell">指定的单元格</param>
		/// <param name="cellAlignment">水平方向的对齐方式</param>
		public void SetCellHorizontalAlignment(string cell, ExcelHorizontalAlignment cellAlignment)
		{
			Range sRange = GetRange(cell);
			sRange.Select();
			sRange.HorizontalAlignment = cellAlignment;
			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;
		}

		/// <summary>
		/// 设定指定范围的单元格的水平对齐方式
		/// </summary>
		/// <param name="startCell">开始的单元格的坐标</param>
		/// <param name="endCell">结束单元格的坐标</param>
		/// <param name="cellAlignment">对齐方式</param>
		public void SetCellAreaHorizontalAlignment(string startCell, string endCell, ExcelHorizontalAlignment cellAlignment)
		{
			Range sRange = GetRange(startCell, endCell);
			sRange.Select();
			sRange.HorizontalAlignment = cellAlignment;
			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;
		}

		/// <summary>
		/// 设置指定单元格的边框，这里只能设置单个单元格的边框
		/// </summary>
		/// <param name="cell">要设定的单元格</param>
		public void SetCellBorder(string cell)
		{
			Range sRange = GetRange(cell);


			//上边框
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeTop].LineStyle = ExcelStyleLine.Continious;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeTop].Weight = ExcelBorderWeight.Thin;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeTop].ColorIndex =Microsoft.Office.Interop.Excel.Constants.xlAutomatic;
			//底边框
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeBottom].LineStyle = ExcelStyleLine.Continious;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeBottom].Weight = ExcelBorderWeight.Thin;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeBottom].ColorIndex =Microsoft.Office.Interop.Excel.Constants.xlAutomatic;
			//右边框
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeRight].LineStyle = ExcelStyleLine.Continious;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeRight].Weight = ExcelBorderWeight.Thin;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeRight].ColorIndex =Microsoft.Office.Interop.Excel.Constants.xlAutomatic;
			//左边框
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeLeft].LineStyle = ExcelStyleLine.Continious;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeLeft].Weight = ExcelBorderWeight.Thin;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeLeft].ColorIndex =Microsoft.Office.Interop.Excel.Constants.xlAutomatic;

			//释放资源
			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;
		}

		/// <summary>
		/// 设置指定范围的Excel单元格的边框，包括外边框，内边框
		/// </summary>
		/// <param name="startCell">开始的单元格坐标</param>
		/// <param name="endCell">结束的单元格坐标</param>
		public void SetAreaBorder1(string startCell, String endCell)
		{
			Range sRange = GetRange(startCell, endCell);
			//上边框
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeTop].LineStyle = ExcelStyleLine.Continious;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeTop].Weight = ExcelBorderWeight.Thin;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeTop].ColorIndex =Microsoft.Office.Interop.Excel.Constants.xlAutomatic;
			//底边框
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeBottom].LineStyle = ExcelStyleLine.Continious;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeBottom].Weight = ExcelBorderWeight.Thin;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeBottom].ColorIndex =Microsoft.Office.Interop.Excel.Constants.xlAutomatic;
			//右边框
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeRight].LineStyle = ExcelStyleLine.Continious;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeRight].Weight = ExcelBorderWeight.Thin;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeRight].ColorIndex =Microsoft.Office.Interop.Excel.Constants.xlAutomatic;
			//左边框
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeLeft].LineStyle = ExcelStyleLine.Continious;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeLeft].Weight = ExcelBorderWeight.Thin;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeLeft].ColorIndex =Microsoft.Office.Interop.Excel.Constants.xlAutomatic;

			//范围内竖直竖线
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.InsideVertical].LineStyle = ExcelStyleLine.Continious;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.InsideVertical].Weight = ExcelBorderWeight.Thin;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.InsideVertical].ColorIndex =Microsoft.Office.Interop.Excel.Constants.xlAutomatic;


			//释放资源
			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;
		}
		/// <summary>
		/// 设置指定范围的Excel单元格的边框，包括外边框，内边框
		/// </summary>
		/// <param name="startCell">开始的单元格坐标</param>
		/// <param name="endCell">结束的单元格坐标</param>
		public void SetAreaBorder(string startCell, String endCell)
		{
			Range sRange = GetRange(startCell, endCell);
			//上边框
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeTop].LineStyle = ExcelStyleLine.Continious;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeTop].Weight = ExcelBorderWeight.Thin;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeTop].ColorIndex =Microsoft.Office.Interop.Excel.Constants.xlAutomatic;
			//底边框
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeBottom].LineStyle = ExcelStyleLine.Continious;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeBottom].Weight = ExcelBorderWeight.Thin;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeBottom].ColorIndex =Microsoft.Office.Interop.Excel.Constants.xlAutomatic;
			//右边框
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeRight].LineStyle = ExcelStyleLine.Continious;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeRight].Weight = ExcelBorderWeight.Thin;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeRight].ColorIndex =Microsoft.Office.Interop.Excel.Constants.xlAutomatic;
			//左边框
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeLeft].LineStyle = ExcelStyleLine.Continious;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeLeft].Weight = ExcelBorderWeight.Thin;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.EdgeLeft].ColorIndex =Microsoft.Office.Interop.Excel.Constants.xlAutomatic;
			//范围内水平横线
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.InsideHorizontal].LineStyle = ExcelStyleLine.Continious;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.InsideHorizontal].Weight = ExcelBorderWeight.Thin;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.InsideHorizontal].ColorIndex =Microsoft.Office.Interop.Excel.Constants.xlAutomatic;

			//范围内竖直竖线
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.InsideVertical].LineStyle = ExcelStyleLine.Continious;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.InsideVertical].Weight = ExcelBorderWeight.Thin;
			sRange.Borders[(Microsoft.Office.Interop.Excel.XlBordersIndex)ExcelBordersIndex.InsideVertical].ColorIndex =Microsoft.Office.Interop.Excel.Constants.xlAutomatic;


			//释放资源
			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;
		}

		/// <summary>
		/// 设置单元格的颜色
		/// </summary>
		/// <param name="cell">定位改单元格</param>
		/// <param name="colorIndex">颜色的代码，我查了MSDN但是没有颜色代码的说明,Excel中一共有56种颜色的代码，常用的几个是
		/// 1-黑色 2-白色 3-红色 4-草绿色 5-蓝色 6-黄色 7-紫色 ，如果想看仔细的颜色，就依次从 1 循环到 56 把颜色打印出来看看</param>
		public void SetCellBackGroundColor(string cell, int colorIndex)
		{
			Range sRange = GetRange(cell);
			sRange.Select();
			sRange.Font.ColorIndex = colorIndex;
			//释放资源
			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;
		}

		/// <summary>
		/// 设置指定单元格范围的颜色
		/// </summary>
		/// <param name="startCell">开始的单元格</param>
		/// <param name="endCell">结束的单元格</param>
		/// <param name="colorIndex">颜色的代码，我查了MSDN但是没有颜色代码的说明,Excel中一共有56种颜色的代码，常用的几个是
		/// 1-黑色 2-白色 3-红色 4-草绿色 5-蓝色 6-黄色 7-紫色 ，如果想看仔细的颜色，就依次从 1 循环到 56 把颜色打印出来看看</param>
		public void SetAreaCellBackGroundColor(string startCell, string endCell, int colorIndex)
		{
			Range sRange = GetRange(startCell, endCell);
			sRange.Select();
			sRange.Font.ColorIndex = colorIndex;
			System.Runtime.InteropServices.Marshal.ReleaseComObject(sRange);
			sRange = null;

		}

		/// <summary>
		/// 合并单元格
		/// <param name="startCell">开始的单元格</param>
		/// <param name="endCell">结束的单元格</param>
		/// </summary>
		public void SetMergeCells(string startCell, string endCell)
		{
			Range sRange = GetRange(startCell, endCell);
			sRange.MergeCells = true;
		}
		/// <summary>
		/// 合并单元格
		/// <param name="iStartRow">定位开始Range的Cell的行(A1=1,1)</param>
		/// <param name="iStartCol">定位开始Range的Cell的列(A1=1,1)</param>
		/// <param name="iEndRow">定位结束Range的Cell的行(A1=1,1)</param>
		/// <param name="iEndCol">定位结束Range的Cell的列(A1=1,1)</param>
		/// </summary>
		public void SetMergeCells(int iStartRow, int iStartCol, int iEndRow, int iEndCol)
		{
			Range sRange = GetRange(iStartRow, iStartCol, iEndRow, iEndCol);
			sRange.MergeCells = true;
		}

		/// <summary>
		/// 设置单元格背景颜色
		/// <param name="startCell">开始的单元格</param>
		/// <param name="endCell">结束的单元格</param>
		/// <param name="ColorIndex">颜色的代码，我查了MSDN但是没有颜色代码的说明,Excel中一共有56种颜色的代码，常用的几个是
		/// 1-黑色 2-白色 3-红色 4-草绿色 5-蓝色 6-黄色 7-紫色 ，如果想看仔细的颜色，就依次从 1 循环到 56 把颜色打印出来看看</param>
		/// </summary>
		public void SetInteriorColor(string startCell, string endCell, int ColorIndex)
		{
			Range sRange = GetRange(startCell, endCell);
			sRange.Interior.ColorIndex = ColorIndex;
		}

		/// <summary>
		/// 设置单元格背景颜色
		/// <param name="iStartRow">定位开始Range的Cell的行(A1=1,1)</param>
		/// <param name="iStartCol">定位开始Range的Cell的列(A1=1,1)</param>
		/// <param name="iEndRow">定位结束Range的Cell的行(A1=1,1)</param>
		/// <param name="iEndCol">定位结束Range的Cell的列(A1=1,1)</param>
		/// <param name="ColorIndex">颜色的代码，我查了MSDN但是没有颜色代码的说明,Excel中一共有56种颜色的代码，常用的几个是
		/// 1-黑色 2-白色 3-红色 4-草绿色 5-蓝色 6-黄色 7-紫色 ，如果想看仔细的颜色，就依次从 1 循环到 56 把颜色打印出来看看</param>
		/// </summary>
		public void SetInteriorColor(int iStartRow, int iStartCol, int iEndRow, int iEndCol, int ColorIndex)
		{
			Range sRange = GetRange(iStartRow, iStartCol, iEndRow, iEndCol);
			sRange.Interior.ColorIndex = ColorIndex;
		}

		#region IDisposable Support
		private bool disposedValue = false; // 要检测冗余调用

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					if (ExlApp != null)
					{
						ExlApp.Quit();
					}
					if (ExlWorkBook != null)
					{
						System.Runtime.InteropServices.Marshal.ReleaseComObject(ExlWorkBook);
						ExlWorkBook = null;
					}
					System.Runtime.InteropServices.Marshal.ReleaseComObject(ExlWorkSheet);
					ExlWorkSheet = null;
					System.Runtime.InteropServices.Marshal.ReleaseComObject(ExlApp);
					ExlApp = null;
					GC.SuppressFinalize(this);
				}


				disposedValue = true;
			}
		}


		// ~ExcelBase() {
		//   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
		//   Dispose(false);
		// }

		// 添加此代码以正确实现可处置模式。
		public void Dispose()
		{
			// 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
			Dispose(true);
		}
		#endregion
		#endregion

	}
}
