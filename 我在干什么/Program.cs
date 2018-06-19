using DotNet4.Utilities.UtilReg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Inst.Util;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using Inst.Util.Output;

namespace Inst
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
			
			//MessageBox.Show("version:"+Environment.OSVersion.Version.Major);
			if (CheckMutiProcess()) { return; };
			ProgramName = "Inst";
			QueryingDay = "SumDay";
			NowDateIsValid = true;

			Program.UsedFlash = Program.AppSetting.In("Setting").GetInfo("UsedFlash", "1") == "1";
			

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			frmMain = new Form1();
			Application.Run(frmMain);
		}

		internal static void HideProgram()
		{
			frmMain.FrmHide(new object(),new EventArgs());
		}

		public static void ExitProgram()
		{
			try
			{
				
				Program.AppSetting.In("Setting").SetInfo("UsedFlash", UsedFlash?"1":"0");
				frmMain.Invoke((EventHandler)delegate {
					frmMain.InfoShow.Visible = false;
					frmMain.Close();
				});
				
				newMutex.ReleaseMutex();
				Application.Exit();
			}
			catch (Exception ex)
			{
				Console.WriteLine("ExitProgram()"+ex.Message);
				
			}
		}
		public static void ShowNotice(int time,string title,string info,ToolTipIcon icon = ToolTipIcon.Info,Action CallBack=null,bool showNoticeInSystem=true)
		{
			if (OnDND) return;
			if (Environment.OSVersion.Version.Major >= 10)
			{
				if (showNoticeInSystem)
				{
					frmMain.InfoShow.ShowBalloonTip(time, title, info, icon);
					nowCallBack = CallBack;
				}
				
			}
			try
			{
				Program.frmMain.Invoke((EventHandler)delegate
				{//务必给主线程去调用
					var f = new InfoShower() { Title = title, Info = info, ExistTime = time,ToolTip=icon };
					f.CallBack = CallBack;
					InfoShower.ShowOnce(f);
				});
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

		}
		private static Action nowCallBack = null;
		public static void ResponseNoticeClick(object sender, EventArgs e)
		{
			nowCallBack?.Invoke();
		}

		public static string Title = "Inst";
		public static bool Running { set; get; }
		public static bool UsedFlash { set; get; }
		public static bool OnDND { set {
				AppSetting.In("Setting").SetInfo("DND", value?"1":"0");
			} get {
				return AppSetting.In("Setting").GetInfo("DND", "0")=="1";
			}
		}
		private static string queryingDay;
		public static string QueryingDay { get=>queryingDay;  set {
				if (value == queryingDay) return;
				NowDateIsValid = DataCore.CheckDateValid(value);
				queryingDay = value;
				if (frmMain!=null)
				{
					
					foreach (var p in frmMain._process)
					{
						p.AnyDataRefresh = true;
					}
					frmMain.ui.center.apps.RefreshData(frmMain._process);
				}
				
			} }
		public static bool NowDateIsValid { get; set; }
		public static bool AutoCurrentVersion { get
			{
				if (CheckAdminUAC())
					return RegUtil.SetRunCurrentVersion(false, true);
				else return false;
			}  set {
				if (CheckAdminUAC())
				{
					ShowNotice(30000, "开机启动", "已"+(value ? "开启" : "关闭" )+ "开机自动启动\n点击此处取消设置", ToolTipIcon.Warning, () => { AutoCurrentVersion = !value; });
					RegUtil.SetRunCurrentVersion(value);
				}
			} }

		public static string ProgramName { get;  set; }

		public static DataCore ProcessData=new DataCore();
		public static bool CheckIfWinVistaAbove()
		{
			return  (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 6);
		}

		internal static void RunAsAdministrator()
		{
			ProcessStartInfo psi = new ProcessStartInfo();
			psi.FileName = Application.ExecutablePath;
			psi.Verb = "runas";

			try
			{
				AppSetting.SetInfo("AdminMutiProcessOnce", "1");
				Process.Start(psi);
				Application.Exit();
			}
			catch (Exception eee)
			{
				CheckMutiProcess();
				MessageBox.Show(eee.Message,"请求权限失败");
			}
		}
		public static System.Threading.Mutex newMutex;
		public static bool CheckMutiProcess()
		{
			newMutex = new System.Threading.Mutex(true, "serfendInst", out bool Exist);
			if (Exist)
			{
				newMutex.ReleaseMutex();
			}
			else
			{

				if (AppSetting.GetInfo("AdminMutiProcessOnce") == "1") {
					AppSetting.SetInfo("AdminMutiProcessOnce", "0");
					return false;
				}
				
				MessageBox.Show("已正在运行,请勿多次开启", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return true;
				
			}
			return false;
		}

		private static int UACnow=0;
		public static bool CheckAdminUAC()
		{
			if (Environment.OSVersion.Version.Major >= 6)
			{
				if (UACnow != 0)
				{
					return UACnow == 1;
				}
				WindowsIdentity identity = WindowsIdentity.GetCurrent();
				WindowsPrincipal principal = new WindowsPrincipal(identity);
				bool UAC = principal.IsInRole(WindowsBuiltInRole.Administrator);
				UACnow = UAC ? 1 : -1;
				return UAC;
			}
			else return true;

		}

		[DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
		 public static extern int SendMessage(IntPtr hWnd, UInt32 Msg, int wParam, IntPtr lParam); 
		 public const UInt32 BCM_SETSHIELD = 0x160C;
		public static void SetControlUACFlag(Control ctl)
		{
			SendMessage(ctl.Handle, BCM_SETSHIELD, 0, (IntPtr)1);
		}
	}
}
