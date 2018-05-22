using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Time时间记录器.Util
{
	/// <summary>
	/// 管理所有进程情况
	/// </summary>
	public class ProcessGroup
	{
		public ProcessRecord this[int id]
		{
			get
			{
				foreach (var p in Process)
				{
					if (p.Id == id) return p;
				}
				return null;
			}
		}
		/// <summary>
		/// 初始化
		/// </summary>
		public ProcessGroup()
		{
			_last = new ProcessRecord();
			Process = new List<ProcessRecord>();
		}
		public ProcessRecord Last
		{
			get
			{
				return _last;
			}
		}


		private bool nowRunningStatus = true;
		public bool ModifiesRunningStatus()
		{
			NowRunningStatus = !NowRunningStatus;
			return NowRunningStatus;
		}


		public bool NowRunningStatus
		{
			get => nowRunningStatus; set
			{
				if (value != nowRunningStatus)
				{
					nowRunningStatus = value;
					this.RefreshTimeRecorder();

				}
			}
		}
		private void RefreshTimeRecorder()
		{
			if (NowRunningStatus)
			{//开始运行时更新所有开始时间
				this.Last.Begin("SystemPause");//重新开始计时
			}
			else
			{
				this.Last.End();//停止
			}
		}

		public List<ProcessRecord> Process { get => process; set => process = value; }
		/// <summary>
		/// 记录所有进程
		/// </summary>
		private List<ProcessRecord> process;
		/// <summary>
		/// 通过进程名称获取进程
		/// </summary>
		/// <param name="ProcessName">进程名称</param>
		/// <returns></returns>
		public ProcessRecord GetProcess(ProcessRecord process)
		{
			if (process == null) return new ProcessRecord();
			foreach (var p in Process)//在记录中寻找进程
			{
				if (p.RemarkName == process.RemarkName)
				{
					return p;
				}
			}
			Process.Add(process);
			return process;//不存在则创建新的进程
		}

		private ProcessRecord _last;
		/// <summary>
		/// 设置进程焦点时间
		/// </summary>
		/// <param name="ProcessName">进程名称</param>
		public ProcessRecord SetBegin(ProcessRecord process)
		{
			if (_last.Id == process.Id) return _last;
			if (!nowRunningStatus) return _last;
			var p = GetProcess(process);
			_last.End();
			p.Begin(_last.ProcessName);
			return p;
		}
		/// <summary>
		/// 设置进程焦点结束时间
		/// </summary>
		/// <param name="ProcessName"></param>
		public ProcessRecord SetEnd(ProcessRecord process)
		{
			var p = GetProcess(process);
			p.End();
			return p;
		}

		/// <summary>
		/// 以 进程名:进程时间输出
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder s = new StringBuilder();
			foreach (var p in Process)
			{
				s.Append(p.ProcessName).Append(":").Append(p.SumUsedTime()).Append("\n");
			}
			return s.ToString();
		}


		public IEnumerator<ProcessRecord> GetEnumerator()
		{
			return ((IEnumerable<ProcessRecord>)Process).GetEnumerator();
		}

	}
	/// <summary>
	/// 进程记录器
	/// </summary>
	public class ProcessRecord
	{
		/// <summary>
		/// 用于标识进程的ID
		/// </summary>
		private int id;
		private Reg ProcessSetting;//进程的存储数据
		private ApplicationInfomations appInfo;
		public string ProcessName { get; private set; }
		public DateTime StartTime { get; private set; }
		public string ModuleName { get; private set; }
		public string MainWindowTitle { get; private set; }
		public List<ProcessRecord> InteractApp;

		/// <summary>
		/// 用于记录进程的使用历史情况
		/// </summary>
		private List<RecordTime> record;
		/// <summary>
		/// 用于记录进程当前情况
		/// </summary>
		private RecordTime nowFocus;
		private int lastFocus, lastLostFocus;
		private Process parent;
		public ProcessRecord()
		{

		}
		public ProcessRecord(Process parent)
		{
			this.Id = parent.Id;
			this.ProcessName = parent.ProcessName;
			//this.StartTime = parent.StartTime;
			//this.ModuleName = parent.MainModule.ModuleName;
			this.MainWindowTitle = parent.MainWindowTitle;
			record = new List<RecordTime>();
			this.parent = parent;
			ProcessSetting = Program.AppSetting.In("Main").In("Data").In(ProcessName);
		}

		public int Id { get => id; set => id = value; }
		public int LastLostFocus { get => lastLostFocus; set => lastLostFocus = value; }
		public int LastFocus { get => lastFocus; set => lastFocus = value; }
		public ApplicationInfomations AppInfo { get => appInfo == null ? appInfo = new ApplicationInfomations(parent) : appInfo; set => appInfo = value; }
		private string remarkName;
		/// <summary>
		/// 进程的用户备注
		/// </summary>
		public string RemarkName
		{
			get
			{
				return remarkName ?? (remarkName = ProcessSetting.GetInfo("RemarkName", this.ProcessName));
			}
			set
			{
				ProcessSetting.SetInfo("RemarkName", value);
				remarkName = value;
			}
		}

		/// <summary>
		/// 设置进程的获取焦点的时间
		/// </summary>
		public void Begin(string switchFrom)
		{
			nowFocus = new RecordTime() { Begin = System.Environment.TickCount, SwitchFrom = switchFrom };
			LastFocus = nowFocus.Begin;
			record.Add(nowFocus);
		}
		private void UpdateRecord(RecordTime record)
		{

		}
		/// <summary>
		/// 设置进程的失去焦点的时间
		/// </summary>
		public void End()
		{
			if (nowFocus == null) return;
			nowFocus.End = System.Environment.TickCount;
			LastLostFocus = nowFocus.End;
			sumUsedTime = SumUsedTime(true);

		}

		private int sumUsedTime;
		/// <summary>
		/// 计算此进程的累积时间
		/// </summary>
		/// <returns>时间累积</returns>
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
			return new string[] { this.ProcessName + ":" + this.MainWindowTitle, this.RemarkName, SpyerProcess.GetMillToString(LastFocus), SpyerProcess.GetMillToString(LastLostFocus), SpyerProcess.GetMillToString(SumUsedTime()) };
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
	/// <summary>
	/// 进程时间对
	/// </summary>
	public class RecordTime
	{
		private int begin;
		private int end;
		private string switchFrom;
		public RecordTime()
		{

		}
		public RecordTime(int begin, int end)
		{
			this.begin = begin;
			this.end = end;
		}

		public int Begin { get => begin; set => begin = value; }
		public int End { get => end; set => end = value; }

		/// <summary>
		/// 获取进程存活时间
		/// </summary>
		public int AliveLength
		{
			get
			{
				if (End == 0 || Begin == 0) return 0;
				return End - Begin;
			}
		}

		public string SwitchFrom { get => switchFrom; set => switchFrom = value; }

		public override string ToString()
		{
			StringBuilder s = new StringBuilder(8);
			return s.Append(Begin).Append("-").Append(End).ToString();
		}
	}
}
