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
		public void AppAttachRelate(string name,string relateTo)
		{
			Init(name);
			var app = this[name];
			if (!app.Relate.ContainsKey(relateTo))
			{
				int nowCount = app.Relate.Count;
				app.RegRelateTo.SetInfo(nowCount + ".Name", relateTo);
				app.Relate.Add(relateTo, new Relate(app.RegRelateTo, nowCount));
				app.RegRelateTo.SetInfo("Count", nowCount+1);
				
			}
			app.Relate[relateTo].Times++;
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
		private int sumActiveCount;//启动总次数
		private int sumWasteTime;//软件总耗时
		private int todayWasteTime;//今日耗时
		private int[] daySumRunCount =new int[24];//每小时打开次数累积
		private Reg RegAppSetting;
		private Reg RegRelate;
		private Reg RegTodayAppSetting;
		private Reg RegSumAppHourRuntime;
		private int[] sumAppHourRunTime=new int[24];//全局 每小时打开次数累积
		public AppRecord(string name)
		{
			Relate = new SortedList<string, Relate>();
			this.Name = name;
			RegAppSetting = Program.AppSetting.In("AppData").In(name);
			RegRelateTo = RegAppSetting.In("Relate");
			int relateAppNum = Convert.ToInt32(RegRelateTo.GetInfo("Count","0"));
			for(int i = 0; i < relateAppNum; i++)
			{
				var r = new Relate(RegRelateTo, i);
				Relate.Add(r.Name,r);
			}
			sumActiveCount = Convert.ToInt32(RegAppSetting.GetInfo("SumActiveTime","0"));
			sumWasteTime = Convert.ToInt32(RegAppSetting.GetInfo("SumWasteTime","0"));

			RegTodayAppSetting = InitDaySetting(DataCore.DayStamp(DateTime.Now).ToString());
			RegTodayAppSetting.SetInfo("Main", "1");
			for (int i = 0; i < DateTime.Now.Hour; i++)
			{
				daySumRunCount[i] = GetDayRunTime(RegTodayAppSetting, i);
			}
			todayWasteTime= Convert.ToInt32( RegTodayAppSetting.GetInfo("WasteTime", "0"));
			RegSumAppHourRuntime = RegAppSetting.In("Data").In("SumDay");
		}
		public Reg InitDaySetting(string day="SumDay")
		{
			if (day == "SumDay") return RegSumAppHourRuntime;
			return RegAppSetting.In("Data").In(day);
		}
		public int GetDayRunTime(Reg day,int h)
		{
			return Convert.ToInt32(day.GetInfo(h.ToString(),"0"));
		}
		public bool CheckDayData(Reg day)
		{
			return day.GetInfo("Main") != "";
		}
		public int SumActiveTime { get => sumActiveCount; set {
				var delta = sumActiveCount - value;
				daySumRunCount[DateTime.Now.Hour] += delta;
				RegTodayAppSetting.SetInfo(DateTime.Now.Hour.ToString(), daySumRunCount[DateTime.Now.Hour]);

				sumActiveCount = value;
				RegAppSetting.SetInfo("SumActiveTime",value);
			} }
		public int SumWasteTime { get => sumWasteTime; set
			{
				int delta = value - sumWasteTime;
				sumWasteTime = value;
				TodayWasteTime += delta;
				RegAppSetting.SetInfo("SumWasteTime", value);
			}
		}

		public SortedList<string,Relate> Relate { get => relate; set => relate = value; }
		public string Name { get => name; set => name = value; }
		public int TodayWasteTime { get => todayWasteTime; private set {
				todayWasteTime = value;
				RegTodayAppSetting.SetInfo("WasteTime",value);
			} }

		public Reg RegRelateTo { get => RegRelate; set => RegRelate = value; }

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
			times = Convert.ToInt32(target.GetInfo(index + ".Times","0"));
		}
	}
}
