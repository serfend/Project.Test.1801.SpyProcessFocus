using DotNet4.Utilities.UtilReg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Time时间记录器.Util
{

	public class DataCore : SortedList<string,AppRecord>
	{
		public void Init(string name)
		{
			if (!this.ContainsKey(name))
			{
				this.Add(name, new AppRecord(name));
			}
		}
		public void AttachRelate(string name,string relateTo)
		{
			Init(name);
			this[name].Relate[relateTo].Times++;
		}
		public void AppRunning(string name) {
			Init(name);
			this[name].SumActiveTime++;
		}
		public void AppWasteTimeAttach(string name,int newWasteTime)
		{
			Init(name);
			this[name].SumWasteTime += newWasteTime;
		}
		public static int DayStamp(DateTime date)
		{
			TimeSpan dayNow = date - new DateTime(1970, 1, 1);
			return (int)dayNow.TotalDays;
		}
	}
	public class AppRecord
	{
		private string name;
		private SortedList<string,Relate> relate;//打开相关软件
		private int sumActiveTime;//启动总次数
		private int sumWasteTime;//软件总耗时
		private int[] daySumRunTime =new int[24];//每小时打开次数累积
		private Reg RegAppSetting;
		private Reg RegRelate;
		private Reg RegTodayAppSetting;
		public AppRecord(string name)
		{
			this.Name = name;
			RegAppSetting = Program.AppSetting.In("AppData").In(name);
			RegRelate = RegAppSetting.In("Relate");
			int relateAppNum = Convert.ToInt32(RegRelate.GetInfo("Count"));
			for(int i = 0; i < relateAppNum; i++)
			{
				var r = new Relate(RegRelate, i);
				Relate.Add(r.Name,r);
			}
			sumActiveTime = Convert.ToInt32(RegAppSetting.GetInfo("SumActiveTime"));
			sumWasteTime = Convert.ToInt32(RegAppSetting.GetInfo("SumWasteTime"));

			RegTodayAppSetting = RegAppSetting.In("Data").In(DataCore.DayStamp(DateTime.Now).ToString());
			for (int i = 0; i < DateTime.Now.Hour; i++)
			{
				daySumRunTime[i] = Convert.ToInt32(RegTodayAppSetting.GetInfo(i.ToString()));
			}

		}
		public int GetSumRunTime(int h)
		{
			return daySumRunTime[h];
		}
		public int SumActiveTime { get => sumActiveTime; set {
				var delta = sumActiveTime - value;
				daySumRunTime[DateTime.Now.Hour] += delta;
				RegTodayAppSetting.SetInfo(DateTime.Now.Hour.ToString(), daySumRunTime[DateTime.Now.Hour]);
				sumActiveTime = value;
				RegAppSetting.SetInfo("SumActiveTime",value);
			} }
		public int SumWasteTime { get => sumWasteTime; set
			{
				sumActiveTime = value;
				RegAppSetting.SetInfo("SumWasteTime", value);
			}
		}

		public SortedList<string,Relate> Relate { get => relate; set => relate = value; }
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
