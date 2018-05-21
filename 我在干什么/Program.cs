using DotNet4.Utilities.UtilReg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Time时间记录器
{
	static class Program
	{
		public static Form1 frmMain;
		public static Reg AppSetting = new Reg().In("TimeCounter");
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main()
		{

				bool Exist;//定义一个bool变量，用来表示是否已经运行
						   //创建Mutex互斥对象
				System.Threading.Mutex newMutex = new System.Threading.Mutex(true, "仅一次", out Exist);
				if (Exist)//如果没有运行
				{
					newMutex.ReleaseMutex();//运行新窗体
				}
				else
				{
					MessageBox.Show("？？？", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);//弹出提示信息
					return;//关闭当前窗体
				}


			Program.UsedFlash = Program.AppSetting.In("Setting").GetInfo("UsedFlash", "1") == "1";
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
				
				Program.AppSetting.In("Setting").SetInfo("UsedFlash", UsedFlash?"1":"0");
				frmMain.InfoShow.Visible = false;
				frmMain.Close();
				Environment.Exit(0);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				
			}
		}
		public static void ShowNotice(int time,string title,string info,ToolTipIcon icon = ToolTipIcon.Info)
		{
			frmMain.InfoShow.ShowBalloonTip(time,title,info,icon);
		}

		public static string Title = "时间统计";
		public static bool Running { set; get; }
		public static bool UsedFlash { set; get; }
	}
}
