using DotNet4.Utilities.UtilReg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using 时间管理大师.Util;

namespace 时间管理大师
{
	static class Program
	{
		public static Form1 frmMain;
		public static Reg AppSetting = new Reg().In("TimeMasterForyy");
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main()
		{
			System.Threading.Mutex newMutex = new System.Threading.Mutex(true, "仅一次", out bool Exist);
			if (Exist)
				{
					newMutex.ReleaseMutex();
				}
				else
				{
				MessageBox.Show("？？？", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}

			QueryingDay = DataCore.DayStamp(DateTime.Now);
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
				Console.WriteLine("ExitProgram()"+ex.Message);
				
			}
		}
		public static void ShowNotice(int time,string title,string info,ToolTipIcon icon = ToolTipIcon.Info)
		{
			frmMain.InfoShow.ShowBalloonTip(time,title,info,icon);
		}

		public static string Title = "时间管理大师";
		public static bool Running { set; get; }
		public static bool UsedFlash { set; get; }
		public static int QueryingDay { get;  set; }

		public static DataCore ProcessData=new DataCore();
	}
}
