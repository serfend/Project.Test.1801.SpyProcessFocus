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
using Time时间记录器.Util;
using System.Threading;


namespace Time时间记录器
{
	public partial class Form1 : Form
	{
		private BackgroundWorker _bckProcessRecord;
		private ProcessGroup _process;
		public UI.UI ui;
		public Form1()
		{
			InitializeComponent();
			_process = new ProcessGroup();
			LstProcessRecorder.SmallImageList = imageListLargeIcon;
			LstProcessRecorder.DoubleClick += LstProcessRecorder_ModifyAppInfos;
			ui = new UI.UI(this);


			this.DoubleClick += Form1_DoubleClick;
			InfoShow.DoubleClick += (x, xx) =>
			{
				this.ShowInTaskbar = true;
				this.Show();
				this.Activate();
				this.InfoShow.Visible = false;
				Program.Running = true;
			};
			InfoShow.Icon = this.Icon;
			Program.Running = true;
		}

		public void Form1_DoubleClick(object sender, EventArgs e)
		{
			this.ShowInTaskbar = false;
			this.Hide();
			this.InfoShow.Visible = true;
			Program.Running = false;
		}

		private void LstProcessRecorder_ModifyAppInfos(object sender, EventArgs e)
		{
			var it = sender as ListView;
			var nowSelect = it.SelectedItems[0];
			DotNet4.Utilities.UtilInput.InputBox.ShowInputBox("修改备注", "修改进程备注名称", nowSelect.SubItems[1].Text, (newName) =>
			{
				_process[Convert.ToInt32(nowSelect.Name)].RemarkName = newName;
				nowSelect.SubItems[1].Text = newName;
			});
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
			
			foreach (var p in _process.Process)
			{
				var item = LstProcessRecorder.Items[p.Id.ToString()];

				string[] ProcessInfo = p.GetItem();
				
				if (item == null)//否则新增一个项
				{
					var newItem = new ListViewItem(ProcessInfo) { Name = p.Id.ToString() };
					item = LstProcessRecorder.Items.Add(newItem);
					if (p.AppInfo.Icon != null)
					{
						var imageIndex= imageListLargeIcon.Images.Count ;
						imageListLargeIcon.Images.Add(newItem.Name, p.AppInfo.Icon);
						item.ImageIndex = imageIndex;
						
					}
					
					
				}
				else //如果原列表中已经有了这个项则直接修改
				{
					for(int i=0;i<ProcessInfo.Length;i++)
						item.SubItems[i].Text = ProcessInfo[i];
				}
			}
			ui.RefreshData(_process);
		}

		private void _bckProcessRecord_DoWork(object sender, DoWorkEventArgs e)
		{
			do
			{
				System.Threading.Thread.Sleep(500+ (int)(new Random().NextDouble() * 500));
				var now =new ProcessRecord( SpyerProcess.GetCurrentProcessFocus());
				
				if (_process.Last.Id == now.Id) continue;
				var p= _process.SetBegin(now);
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
					xls.ExlWorkSheet.Cells[++nowRowIndex, 1] = "进程:"+ p.ProcessName +":" + p.MainWindowTitle + "(" + p.RemarkName + ")";

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
				
				xls.SaveAs(Application.StartupPath +string.Format( @"\Time时间记录器-{0:D}.xls",DateTime.Today));
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
			InfoShow.Dispose();
		}

		private const int WM_NCHITTEST = 0x84;
		private const int HTCLIENT = 0x1;
		private const int HTCAPTION = 0x2;
		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case WM_NCHITTEST:
					base.WndProc(ref m);
					if ((int)m.Result == HTCLIENT)
						m.Result = (IntPtr)HTCAPTION;
					return;
			}
			base.WndProc(ref m);
		}
	}
}
