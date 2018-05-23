using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 时间管理大师.UI.AppComponent
{
	public class AppRelate:Control
	{
		private string name;
		private SortedList<string, Util.Relate> relate;
		private string[] relateIndex=new string[99];
		private int showIconNum = 3;
		public AppRelate(Action<Control> CallBack,string name) : base(CallBack)
		{
			//BackColor = Color.Black;
			this.name = name;
			relate = Program.ProcessData[name].Relate;
			var numStr = Program.ProcessData[name].Setting("RelateIconShowNum");
			ShowIconNum = Convert.ToInt32(numStr==""?"3": numStr);
		}

		public int ShowIconNum { get => showIconNum; set {
				showIconNum = value;
				Program.ProcessData[name].Setting("RelateIconShowNum",value.ToString());
			} }

		public void RefreshData()
		{
			int nowFillNum = 0;
			foreach(var app in relate)
			{
				relateIndex[nowFillNum++] = app.Value.Name;
				if (nowFillNum == showIconNum) break;
			}
			if (showIconNum >= relate.Count) return;
			foreach (var app in relate)
			{
				for(int i=nowFillNum - 1; i > 0; i--)
				{
					if (relate[relateIndex[i]].Times < app.Value.Times)
					{
						if (i < nowFillNum - 1)
							relateIndex[i + 1] = relateIndex[i];
						relateIndex[i] = app.Key;
					}
				}
			}
		}
		public override bool RefreshLayout()
		{
			return false;
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			int count = 0;
			for(int i = 0; count < ShowIconNum&& relateIndex[i] != null; i++)
			{
				if (relateIndex[i] != "")
				{
					var name = relate[relateIndex[i]].Name;
					var app = Program.frmMain._process[name];
					if (app != null && app.AppInfo.Icon != null) { e.Graphics.DrawIcon(app.AppInfo.Icon, new Rectangle((count++) * 30, 0, 24, 24)); }
				}
					
			}
			//base.OnPaint(e);
		}
	}
}
