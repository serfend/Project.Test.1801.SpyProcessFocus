using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using DotNet4.Utilities.UtilExcel;
using Inst.Util;
using System.Threading;
using System.Runtime.InteropServices;
using DotNet4.Utilities.UtilReg;

namespace Inst
{
	public partial class Form1 : Form
	{
		private BackgroundWorker _bckProcessRecord;
		public ProcessGroup _process;
		public UI.UI ui;
		public Form1()
		{
			InitializeComponent();
			_process = new ProcessGroup();
			ui = new UI.UI(this);

			InfoShow.DoubleClick += InfoShow_DoubleClick;
			InfoShow.Icon = this.Icon;
			InfoShow.BalloonTipClicked += Program.ResponseNoticeClick;
			Program.Running = true;

			var bound=RegUtil.GetFormPos(this);
			this.Load += (x, xx) => { SetBounds(bound[0], bound[1], bound[2], bound[3]); };
		}

		public void InfoShow_DoubleClick(object sender, EventArgs e)
		{
			this.ShowInTaskbar = true;
			this.Show();
			this.Activate();
			this.InfoShow.Visible = false;
			Program.Running = true;
		}

		public void FrmHide(object sender, EventArgs e)
		{
			this.ShowInTaskbar = false;
			this.Hide();
			this.InfoShow.Visible = true;
			var info = Program.Title + "已隐藏并持续在后台运行";
			Program.ShowNotice(5000, info, info + ",您可以在托盘中双击重新显示", ToolTipIcon.Info);
			Program.Running = false;
		}

		private void LstProcessRecorder_ModifyAppInfos(object sender, EventArgs e)
		{
			var it = sender as ListView;
			var nowSelect = it.SelectedItems[0];
			
		}
		private void Form1_Load(object sender, EventArgs e)
		{
			_bckProcessRecord = new BackgroundWorker() { WorkerReportsProgress=true,WorkerSupportsCancellation=true};
			_bckProcessRecord.DoWork += _bckProcessRecord_DoWork;
			_bckProcessRecord.ProgressChanged += _bckProcessRecord_ProgressChanged;
			_bckProcessRecord.RunWorkerAsync();
			this.OnResize(new EventArgs());
		}

		private ImageList imageListLargeIcon=new ImageList();
		private void _bckProcessRecord_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			
			ui.RefreshData(_process);
		}
		private int thisAppRuntime = 0;
		private int nextTipTime = 60;

		private Random rnd = new Random();
		private string lastRunAppName;
		private int lastRunAppTimeRecord;
		private void _bckProcessRecord_DoWork(object sender, DoWorkEventArgs e)
		{
			do
			{
				System.Threading.Thread.Sleep(500);
				var process = SpyerProcess.GetCurrentProcessFocus();
				if (process.ProcessName == "Idle") continue;
				var now =new ProcessRecord( process.ProcessName, process.MainWindowTitle, process);
				var nowTime = (int)(DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds;
				if (lastRunAppName == now.ProcessAliasName) {
					if (thisAppRuntime>0&&nowTime - thisAppRuntime > nextTipTime)
					{
						int h = nextTipTime / 3600;
						int m = (nextTipTime - h * 3600) / 60;
						int s = nextTipTime % 60;
						string timeStr = "";
						if (h > 1) timeStr = string.Format("{0}小时{1}分钟", h, m);
						else if (h == 1 || m > 9) timeStr = string.Format("{0}分钟{1}秒", h * 60 + m, s);
						else timeStr = nextTipTime+"秒";
						Program.ShowNotice(100000, "疲劳提醒", "您已经在"+now.ProcessAliasName+"上耗费了"+timeStr+"了哦",ToolTipIcon.Info,()=> {
							if (!Program.Running) Program.frmMain.InfoShow_DoubleClick(this,EventArgs.Empty);
							Program.frmMain.ui.center.apps.SetFocus(now.ProcessAliasName);//用户点击时聚焦到当前
						});
						if (nextTipTime < 7200) nextTipTime = nextTipTime * 2 + rnd.Next(250, 350);
						else {
							nextTipTime /= 3600;
							nextTipTime *= 3600;
							nextTipTime += 3600;
						}
					}
					continue;
				}
				var p = _process.SetBegin(now);
				if(Environment.TickCount- lastRunAppTimeRecord > 10000)//当前的App被关闭了10秒以上方可记录
				{
					lastRunAppName = now.ProcessAliasName;
					lastRunAppTimeRecord = Environment.TickCount;
					nextTipTime = 60;
					thisAppRuntime = nowTime;
				}

				//Console.WriteLine(now.ProcessName);
				
				_bckProcessRecord.ReportProgress(0);
			} while (!_bckProcessRecord.CancellationPending);

		}
		public void BtnOutPutToExcel_Click()
		{
			using (var xls = new ExcelBase())
			{
				xls.Open();
				xls.AddWorkSheet();
				int nowRowIndex = 0;
				foreach(var p in  _process)
				{
					xls.ExlWorkSheet.Cells[++nowRowIndex, 1] = "进程:"+ p.ProcessAliasName +":" + p.MainWindowTitle + "(" + p.RemarkName + ")";

					xls.ExlWorkSheet.Cells[nowRowIndex, 2] = "用户总用时:" +  SpyerProcess.GetMillToString(p.SumUsedTime());// +"ms";
					xls.ExlWorkSheet.Cells[++nowRowIndex, 1] = "焦点时间";
					xls.ExlWorkSheet.Cells[nowRowIndex, 2] = "失去焦点时间";
					xls.ExlWorkSheet.Cells[nowRowIndex, 3] = "间隔时间";
					foreach (var r in p)
					{
						xls.ExlWorkSheet.Cells[++nowRowIndex, 1] =SpyerProcess.GetMillToString(r.Begin);// r.Begin;
						xls.ExlWorkSheet.Cells[nowRowIndex, 2] = SpyerProcess.GetMillToString( r.End);
						xls.ExlWorkSheet.Cells[nowRowIndex, 3] = SpyerProcess.GetMillToString(r.AliveLength);
					}
					nowRowIndex++;
				}
				
				xls.SaveAs(Application.StartupPath +string.Format( @"\Inst-{0:D}.xls",DateTime.Today));
			}
		}
		
		public void BtnRunningCommand_Click()
		{
			var nowStatus = _process.ModifiesRunningStatus();
		}

		private void LstProcessRecorder_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		
		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{

			RegUtil.SetFormPos(this);
			InfoShow.Dispose();
		}

		private const int Guying_HTLEFT = 10;
		private const int Guying_HTRIGHT = 11;
		private const int Guying_HTTOP = 12;
		private const int Guying_HTTOPLEFT = 13;
		private const int Guying_HTTOPRIGHT = 14;
		private const int Guying_HTBOTTOM = 15;
		private const int Guying_HTBOTTOMLEFT = 0x10;
		private const int Guying_HTBOTTOMRIGHT = 17;
		private const int HTCAPTION = 0x2;
		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case WM_NCPAINT:                        // box shadow
					if (m_aeroEnabled)
					{
						var v = 2;
						DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
						MARGINS margins = new MARGINS()
						{
							bottomHeight = 1,
							leftWidth = 1,
							rightWidth = 1,
							topHeight = 1
						};
						DwmExtendFrameIntoClientArea(this.Handle, ref margins);

					}
					break;

				case 0x0084:
					base.WndProc(ref m);
					Point vPoint = new Point((int)m.LParam & 0xFFFF,
						(int)m.LParam >> 16 & 0xFFFF);
					vPoint = PointToClient(vPoint);
					if (vPoint.X <= 5)
						if (vPoint.Y <= 5)
							m.Result = (IntPtr)Guying_HTTOPLEFT;
						else if (vPoint.Y >= ClientSize.Height - 5)
							m.Result = (IntPtr)Guying_HTBOTTOMLEFT;
						else m.Result = (IntPtr)Guying_HTLEFT;
					else if (vPoint.X >= ClientSize.Width - 5)
						if (vPoint.Y <= 5)
							m.Result = (IntPtr)Guying_HTTOPRIGHT;
						else if (vPoint.Y >= ClientSize.Height - 5)
							m.Result = (IntPtr)Guying_HTBOTTOMRIGHT;
						else m.Result = (IntPtr)Guying_HTRIGHT;
					else if (vPoint.Y <= 5)
						m.Result = (IntPtr)Guying_HTTOP;
					else if (vPoint.Y >= ClientSize.Height - 5)
						m.Result = (IntPtr)Guying_HTBOTTOM;
					else
					{
						m.Result = (IntPtr)HTCAPTION;
					}
					break;
				case 0x0201:                //鼠标左键按下的消息   
					m.Msg = 0x00A1;         //更改消息为非客户区按下鼠标   
					m.LParam = IntPtr.Zero; //默认值   
					m.WParam = new IntPtr(2);//鼠标放在标题栏内   
					base.WndProc(ref m);
					break;
				default:
					base.WndProc(ref m);
					break;
			}
			//base.WndProc(ref m);
		}

		[DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
		private static extern IntPtr CreateRoundRectRgn
			(
				int nLeftRect, // x-coordinate of upper-left corner
				int nTopRect, // y-coordinate of upper-left corner
				int nRightRect, // x-coordinate of lower-right corner
				int nBottomRect, // y-coordinate of lower-right corner
				int nWidthEllipse, // height of ellipse
				int nHeightEllipse // width of ellipse
			 );

		[DllImport("dwmapi.dll")]
		public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

		[DllImport("dwmapi.dll")]
		public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

		[DllImport("dwmapi.dll")]
		public static extern int DwmIsCompositionEnabled(ref int pfEnabled);

		private bool m_aeroEnabled;                     // variables for box shadow
		private const int CS_DROPSHADOW = 0x00020000;
		private const int WM_NCPAINT = 0x0085;
		private const int WM_ACTIVATEAPP = 0x001C;

		public struct MARGINS                           // struct for box shadow
		{
			public int leftWidth;
			public int rightWidth;
			public int topHeight;
			public int bottomHeight;
		}

		private const int WM_NCHITTEST = 0x84;          // variables for dragging the form
		private const int HTCLIENT = 0x1;

		protected override CreateParams CreateParams
		{
			get
			{
				m_aeroEnabled = CheckAeroEnabled();

				CreateParams cp = base.CreateParams;
				if (!m_aeroEnabled)
					cp.ClassStyle |= CS_DROPSHADOW;

				return cp;
			}
		}

		private bool CheckAeroEnabled()
		{
			if (Environment.OSVersion.Version.Major >= 6)
			{
				int enabled = 0;
				DwmIsCompositionEnabled(ref enabled);
				return (enabled == 1) ? true : false;
			}
			return false;
		}
		

	}
}
