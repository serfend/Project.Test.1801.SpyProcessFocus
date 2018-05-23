using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 时间管理大师.Util
{
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
