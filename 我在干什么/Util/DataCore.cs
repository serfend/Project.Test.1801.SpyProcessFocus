using DotNet4.Utilities.UtilReg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Time时间记录器.Util
{
	/// <summary>
	/// 用于获取软件的工作记录
	/// </summary>
	public static class DataCore
	{
		

	}
	public class AppRecord
	{
		private string name;
		private List<Relate> relate;//打开相关软件
		private int sumActiveTime;//启动总次数
		private int sumWasteTime;//软件总耗时
		private int[] daySumRunTime =new int[24];//每小时打开次数累积
		private Reg RegAppSetting;
		private Reg RegRelate;
		public AppRecord(string name)
		{
			this.Name = name;
			RegAppSetting = Program.AppSetting.In("AppData").In(name);
			RegRelate = RegAppSetting.In("Relate");
			int relateAppNum = Convert.ToInt32(RegRelate.GetInfo("Count"));
			for(int i = 0; i < relateAppNum; i++)
			{
				Relate.Add(new Relate(RegRelate, i));
			}
			sumActiveTime = Convert.ToInt32(RegAppSetting.GetInfo("SumActiveTime"));
			sumWasteTime = Convert.ToInt32(RegAppSetting.GetInfo("SumWasteTime"));
		}

		public int SumActiveTime { get => sumActiveTime; set {
				sumActiveTime = value;
				RegAppSetting.SetInfo("SumActiveTime",value);
			} }
		public int SumWasteTime { get => sumWasteTime; set
			{
				sumActiveTime = value;
				RegAppSetting.SetInfo("SumWasteTime", value);
			}
		}

		public List<Relate> Relate { get => relate; set => relate = value; }
		public string Name { get => name; set => name = value; }

		public string Setting(string name,string newValue = null)
		{
			if (newValue == null)
			{
				return RegAppSetting.GetInfo(name);
			}
			else
			{
				RegAppSetting.SetInfo(name, newValue);
				return newValue;
			}
		}

		public DayRecord GetDayRecord(int day)
		{
			return new DayRecord(Name,day);
		}
	}
	public class DayRecord
	{
		public int activeCount;//总打开次数
		public int activeTime;//总使用时长（秒）
		public int[] hourActiveCount;
		public int[] hourActiveTime;
		public DayRecord(string name,int day)
		{
			hourActiveCount = new int[24];
			hourActiveTime = new int[24];
		}
		
	}
	public class Relate
	{
		private int index;
		private string name;
		private int times;
		private Reg target;
		public string Name { get => name; set {
				name = value;
				target.SetInfo(index + ".Name",value);
			} }
		public int Times { get => times; set
			{
				times = value;
				target.SetInfo(index + ".Times", value);
			}
		}

		public Relate(Reg target,int index)
		{
			this.index = index;
			this.target = target;
			name = target.GetInfo(index + ".Name");
			times = Convert.ToInt32(target.GetInfo(index + ".Times"));
		}
	}
}
