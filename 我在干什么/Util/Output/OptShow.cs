using DotNet4.Utilities.UtilReg;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace Inst.Util.Output
{
	public struct Opt
	{
		public string infos;
		public string CmdInfo;
		public Color DColor;
		public Color UColor;
	}
	/// <summary>
	/// 用于调用外部的OptShow
	/// </summary>
	public class OptShows
	{
		public static Dictionary<string, Action> CallBack=new Dictionary<string, Action>();
		static OptShows()
		{
			var t = new Thread(() =>
			{
				while (true)
				{
					var cmd = reg.GetInfo("FrmReturn");
					if (cmd != null&&cmd.Length>0)
					{
						reg.SetInfo("FrmReturn", "");
						if (CallBack.ContainsKey(cmd))
						{
							CallBack[cmd].Invoke();
						}
					}
					Thread.Sleep(200);
				}
			})
			{ IsBackground=true};
			t.Start();
		}
		private static Reg reg = new Reg().In("TimeMasterForyy").In("Main").In("OptInfo");
		private Reg optReg;
		public void ShowOpt(Opt[] opt,string title,string content, string img=null)
		{
			NowOptIndex++;
			Title = title;
			Info = content;
			Images = img;
			OptNum = opt.Length;
			for(int i = 0; i < OptNum; i++)
			{
				var option= optReg.In((i+1).ToString());
				option.SetInfo("Cmd", opt[i].CmdInfo);
				option.SetInfo("DColor", opt[i].DColor.R + opt[i].DColor.G*256 + opt[i].DColor.B * 65536);
				option.SetInfo("UColor", opt[i].UColor.R  + opt[i].UColor.G * 256 + opt[i].UColor.B * 65536);
				option.SetInfo("Infos",opt[i].infos);
			}
			ShowOpt();
		}
		public void SetErr(string title,string errInfo)
		{
			var opts = new Opt[1];
			opts[0].infos = "确认";
			opts[0].UColor = Color.FromArgb(255,0,200,0);
			ShowOpt(opts, title, errInfo);
		}
		public void ShowOpt()
		{
			ComRegister.ShellExcute.ShellExcuteExe("SelOption.exe");
		}
		private int nowOptIndex = -1;
		public int NowOptIndex { get => nowOptIndex==-1?Convert.ToInt32(reg.GetInfo("NowOptId", "0")): nowOptIndex; set {
				nowOptIndex = value>8?0:value;
				reg.SetInfo("NowOptId", nowOptIndex);
				optReg = reg.In(nowOptIndex.ToString());
			} }
		public string Title {
			get => GetKeyValue("FrmTitle");
			set => SetKeyValue("FrmTitle", value);
		}
		public string Info
		{
			get => GetKeyValue("ContentInfo");
			set => SetKeyValue("ContentInfo", value);
		}
		public string Images
		{
			get => GetKeyValue("Images");
			set => SetKeyValue("Images", value??"");
		}
		public int OptNum
		{
			get {
				var i = GetKeyValue("FrmOptNum");
				return i==null?0: Convert.ToInt32(i);
			} 
			set => SetKeyValue("FrmOptNum", value.ToString());
		}
		public Opt this[int index]
		{
			get
			{
				if (!opts.ContainsKey(index))
				{
					opts[index] = new Opt();
				}
				return opts[index];
			}
			set => opts[index] = value; 
		}
		private Dictionary<int, Opt> opts=new Dictionary<int, Opt>();
		private Dictionary<string, string> dic=new Dictionary<string, string>();
		private string GetKeyValue(string key) {
			if (!dic.ContainsKey(key))  dic[key]= optReg.GetInfo(key); ;
			return dic[key];
		}
		private void SetKeyValue(string key, string value) { optReg.SetInfo(key, value); }
	}
}
