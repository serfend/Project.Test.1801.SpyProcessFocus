using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Time时间记录器
{
	static class Program
	{
		public static Form1 frmMain;
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			frmMain = new Form1();
			Application.Run(frmMain);
		}

		internal static void HideProgram()
		{
			frmMain.Form1_DoubleClick(new object(),new EventArgs());
		}

		public static void ExitProgram()
		{
			try
			{
				Environment.Exit(0);
			}
			catch (Exception)
			{

				
			}
		}

		public static Action<Control> ShowDataAnalysis()
		{
			return (x) =>
			{
				MessageBox.Show("假装" + x.Text + "的信息展示ing");
			};
		}

		public static string Title = "时间统计";
		public static bool Running { set; get; }
	}
}
