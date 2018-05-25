using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inst.Util
{
	/// <summary>
	/// 管理所有进程情况
	/// </summary>
	public class ProcessGroup
	{
		public ProcessRecord this[string name]
		{
			get
			{
				foreach (var p in Process)
				{
					if (p.ProcessName == name) return p;
				}
				return null;
			}
		}
		public ProcessGroup()
		{
			_last = new ProcessRecord("#null#", "#null#",null);
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
				this.Last.Begin("#SystemPause#");//重新开始计时
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
			if (process == null) return new ProcessRecord("#null#", "#null#",null);
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
		public ProcessRecord SetBegin(ProcessRecord process)
		{
			if (_last.RemarkName == process.RemarkName) return _last;
			if (!nowRunningStatus) return _last;
			var p = GetProcess(process);
			_last.End();
			p.Begin(_last.ProcessName);
			_last = p;
			return p;
		}
		public ProcessRecord SetEnd(ProcessRecord process)
		{
			var p = GetProcess(process);
			p.End();
			return p;
		}

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

}
