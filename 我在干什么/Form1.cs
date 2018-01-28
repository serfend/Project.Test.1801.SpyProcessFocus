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
namespace 我在干什么
{
	public partial class Form1 : Form
	{
		private BackgroundWorker _bckProcessRecord;
		private ProcessGroup _process;
		public Form1()
		{
			InitializeComponent();
			_process = new ProcessGroup();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			_bckProcessRecord = new BackgroundWorker() { WorkerReportsProgress=true,WorkerSupportsCancellation=true};
			_bckProcessRecord.DoWork += _bckProcessRecord_DoWork;
			_bckProcessRecord.ProgressChanged += _bckProcessRecord_ProgressChanged;
			_bckProcessRecord.RunWorkerAsync();
		}

		private void _bckProcessRecord_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			
			foreach (var p in _process.Process)
			{
				var item = LstProcessRecorder.Items[p.Name];

				string[] ProcessInfo = p.GetItem();
				
				if (item == null)//否则新增一个项
				{
					var newItem = new ListViewItem(ProcessInfo) { Name = p.Name };
					item = LstProcessRecorder.Items.Add(newItem);
				}
				else //如果原列表中已经有了这个项则直接修改
				{
					for(int i=0;i<ProcessInfo.Length;i++)
						item.SubItems[i].Text = ProcessInfo[i];
				}
			}
			Console.WriteLine(_process);
		}

		private void _bckProcessRecord_DoWork(object sender, DoWorkEventArgs e)
		{
			do
			{
				System.Threading.Thread.Sleep(500+ (int)(new Random().NextDouble() * 500));
				Process now = SpyerProcess.GetCurrentProcessFocus();
				if (_process.Last.Name == now.ProcessName) continue;
				var p= _process.SetBegin(now.ProcessName);
				_bckProcessRecord.ReportProgress(0);
			} while (!_bckProcessRecord.CancellationPending);

		}

		private void BtnOutPutToExcel_Click(object sender, EventArgs e)
		{
			using (var xls = new ExcelBase())
			{
				xls.Open();
				xls.AddWorkSheet();
				int nowRowIndex = 0;
				foreach(var p in  _process)
				{
					xls.ExlWorkSheet.Cells[++nowRowIndex, 1] = "进程:"+ p.Name;

					xls.ExlWorkSheet.Cells[nowRowIndex, 2] = "用户总用时:" + p.SumUsedTime() +"ms";
					xls.ExlWorkSheet.Cells[++nowRowIndex, 1] = "焦点时间";
					xls.ExlWorkSheet.Cells[nowRowIndex, 2] = "失去焦点时间";
					xls.ExlWorkSheet.Cells[nowRowIndex, 3] = "间隔时间";
					foreach (var r in p)
					{
						xls.ExlWorkSheet.Cells[++nowRowIndex, 1] = r.Begin;
						xls.ExlWorkSheet.Cells[nowRowIndex, 2] = r.End;
						xls.ExlWorkSheet.Cells[nowRowIndex, 3] = r.AliveLength;
					}
					nowRowIndex++;
				}
				
				xls.SaveAs(Application.StartupPath +string.Format( @"\我在干什么-{0:D}.xls",DateTime.Today));
			}

			

		}
	}
}
