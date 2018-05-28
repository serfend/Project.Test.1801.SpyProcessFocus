using DotNet4.Utilities.UtilReg;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Inst.Util
{
	public class ProcessRecord
	{

		private Reg ProcessSetting;//进程的存储数据
		private ApplicationInfomations appInfo;
		public string FilePath { get; set; }
		public string ProcessAliasName { get;  set; }
		public DateTime StartTime { get;  set; }
		public string ModuleName { get;  set; }
		public string MainWindowTitle { get;  set; }
		public List<ProcessRecord> InteractApp;
		private List<RecordTime> record;
		private RecordTime nowFocus;
		private int lastFocus, lastLostFocus;
		public Process parent;

		public ProcessRecord(string ProcessName, string MainWindowTitle,Process parent)
		{
			this.parent = parent;
			this.MainWindowTitle = MainWindowTitle;
			this.ProcessAliasName = (ProcessName == "ApplicationFrameHost" ? MainWindowTitle : ProcessName);
			//Console.WriteLine(this.ProcessName+":"+ProcessName + ","+ MainWindowTitle);
			if (parent != null)
			{
				try
				{
					this.FilePath = parent.MainModule.FileName;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
			
			record = new List<RecordTime>();
			ProcessSetting = Program.AppSetting.In("Main").In("Data").In(ProcessName);
		}


		public int LastLostFocus { get => lastLostFocus; set => lastLostFocus = value; }
		public int LastFocus { get => lastFocus; set => lastFocus = value; }
		public ApplicationInfomations AppInfo { get => appInfo == null ? appInfo = new ApplicationInfomations(parent) : appInfo; set => appInfo = value; }
		private string remarkName;
		public string RemarkName
		{
			get
			{
				return remarkName ?? (remarkName = ProcessSetting.GetInfo("RemarkName", this.ProcessAliasName));
			}
			set
			{
				ProcessSetting.SetInfo("RemarkName", value);
				remarkName = value;
			}
		}
		public bool AnyDataRefresh;
		public void Begin(string switchFrom)
		{
			switchFrom = switchFrom ?? "#null#";
			nowFocus = new RecordTime() { Begin = System.Environment.TickCount, SwitchFrom = switchFrom };
			LastFocus = nowFocus.Begin;
			record.Add(nowFocus);
			Program.ProcessData.AppRunning(this.ProcessAliasName);
			Program.ProcessData.AppAttachRelate(this.ProcessAliasName, switchFrom);
			AnyDataRefresh = true;
		}
		public void End()
		{
			if (nowFocus == null) return;
			nowFocus.End = System.Environment.TickCount;
			LastLostFocus = nowFocus.End;
			sumUsedTime = SumUsedTime(true);
			Program.ProcessData.AppWasteTimeAttach(this.ProcessAliasName, nowFocus.End - nowFocus.Begin);
			AnyDataRefresh = true;
		}

		private int sumUsedTime;
		public int SumUsedTime(bool reCaculate = false)
		{
			if (!reCaculate) return sumUsedTime;
			int result = 0;
			foreach (var r in record)
			{
				result += r.AliveLength;
			}
			return result;
		}

		public string[] GetItem()
		{
			return new string[] { this.ProcessAliasName + ":" + this.MainWindowTitle, this.RemarkName, SpyerProcess.GetMillToString(LastFocus), SpyerProcess.GetMillToString(LastLostFocus), SpyerProcess.GetMillToString(SumUsedTime()) };
		}
		public IEnumerator<RecordTime> GetEnumerator()
		{
			return record.GetEnumerator();
		}
		public override string ToString()
		{
			StringBuilder s = new StringBuilder(32);
			foreach (var r in record)
			{
				s.Append(r.ToString()).Append("\n");
			}
			return s.ToString();
		}

	}

}
