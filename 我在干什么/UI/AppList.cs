﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Inst.Util;

namespace Inst.UI
{
	public class AppList:Control
	{
		private List<App> list=new List<App>();
		public AppList()
		{
			onResize = new OnResizeDelegate(OnResizeRaise);
		}
		public void Add(App app)
		{
			app.layoutParent = this;
			app.Parent = this;
			app.Index=app.RawIndex = list.Count;
			list.Add( app);
		}
		public override bool RefreshLayout()
		{
			if (!mouseIsDown)
			{
				if (targetY >10)
					targetY = 0;
				else
				{
					if(Controls.Count > 0)
					{

						var lastAppBottom = Controls[lastAppIndex].Bottom;
						if (lastAppBottom < Bottom)
						{
							var firstAppTop = Controls[0].Top;
							if(firstAppTop < 0)
								targetY += (int)(0.01 * Math.Pow((Height - lastAppBottom), 1.2)) + 1;
						}
					}
					
				}
			}
				if (Math.Abs(nowY - targetY) > 0.01)
			{
				nowY = nowY * 0.9f + targetY * 0.1f;
				this.Invoke(onResize);
			}
			return base.RefreshLayout();
		}
		private delegate void OnResizeDelegate();
		private OnResizeDelegate onResize;
		protected override void OnMouseDown(MouseEventArgs e)
		{
			MouseIsDown = true;
			lastY = e.Location.Y;
			base.OnMouseDown(e);
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			MouseIsDown = false;
			base.OnMouseUp(e);
		}
		private int lastY;
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (MouseIsDown)
			{
				TargetY += (e.Location.Y - lastY)*5;
				this.OnResize(EventArgs.Empty);
			}
			base.OnMouseMove(e);
		}
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			TargetY += e.Delta;
			base.OnMouseWheel(e);
		}

		private bool mouseIsDown = false;
		private float nowY=0,targetY=0;

		public bool MouseIsDown { get => mouseIsDown; set => mouseIsDown = value; }
		public float NowY { get => nowY; set => nowY = value; }
		public float TargetY { get => targetY; set => targetY = value; }

		public int lastHdlFocusRawIndex;
		private void OnResizeRaise()
		{
			OnResize(EventArgs.Empty);
		}
		protected override void OnResize(EventArgs e)
		{
			int height = 200;
			int lastY =(int)nowY;
			for (int i = 0; i < Controls.Count; i++)
			{
				if (Controls[i] is App app)
				{
					app.DBounds=new System.Drawing.Rectangle(0,
					lastY+(int)(height*1.05*app.Index) ,Width, height);
					//lastY += (int)(height*1.05);
				}
			}
		}
		public void RefreshData(ProcessGroup process)
		{
			int maxValue=0;
			bool anyRefresh = false;
			for(int i=0;i< process.Process.Count; i++)
			{
				var p = process.Process[i];
				
				var app = list.Find((x) => x.ProcessName == p.ProcessAliasName);
				if (app!=null) { 
					var data=Program.ProcessData[p.ProcessAliasName];
					app.TimeLine.TodayTime = (Program.QueryingDay == "SumDay" ? data.SumWasteTime : data.GetDayWasteTime(Program.QueryingDay))/1000;

					if (app.TimeLine.TodayTime > maxValue) maxValue = app.TimeLine.TodayTime;
					if (!p.AnyDataRefresh) continue;
					p.AnyDataRefresh = false;
					
					app.SumUsedTimeLine.TodayTime = data.SumWasteTime/1000;
					app.SumUsedTimeLine.SoftAvgTime = 86400;
					app.SumUsedCountLine.Text = data.SumActiveTime.ToString();
					app.RelateApp.RefreshData();
					for (int h = 0; h < 24; h++)
					{
						int count = data.GetDayRunTime(Program.QueryingDay, h);
						app.frequency.SetCount(h,count);
					}
					app.frequency.Invalidate();
					
				}
				else
				{
					App newApp = new App(p.ProcessAliasName) {
						Font = this.Font,
					};
					newApp.Logo.Icon = p.AppInfo.Icon;
					newApp.Logo.appPath = p.FilePath;
					newApp.Logo.BackColor = p.AppInfo.IconMainColor;
					newApp.TimeLine.TodayTime = 0;
					newApp.TimeLine.AvgTime = 50;
					newApp.TimeLine.SoftAvgTime = 100;

					Add(newApp);
					anyRefresh = true;
					//app.Frequency
				}
			}
			SortControls(maxValue);
			if (anyRefresh) this.OnResizeRaise();
		}

		internal void SetFocus(string processAliasName)
		{
			//TODO 切换到目标位置
		}

		private int lastAppIndex;
		private void SortControls(int maxValue)
		{
		;
			int maxIndex = 0;
			bool anyRefresh = false;
			for(int i=0; i < list.Count; i++)
			{
				var app = ((App)(Controls[i]));
				for (int j = 0; j < list.Count; j++)
				{
					var app2 = ((App)Controls[j]);
					if (i != j)
					{
						if (AppValue(i) > AppValue(j) && AppRank(i) > AppRank(j)) {
							app.Index ^= app2.Index;
							app2.Index ^= app.Index;
							app.Index ^= app2.Index;
							anyRefresh = true;
						}
					}
				}
				app.TimeLine.SoftAvgTime = maxValue;
				
				if(maxIndex< app.Index)
				{
					maxIndex = app.Index;
					lastAppIndex = i;
				}
			}
			if (anyRefresh) this.OnResizeRaise();
		}

		private int AppValue(int index)
		{
			return ((App)Controls[index]).TimeLine.TodayTime;
		}
		private int AppRank(int index)
		{
			return ((App)Controls[index]).Index;
		}

	}
}
