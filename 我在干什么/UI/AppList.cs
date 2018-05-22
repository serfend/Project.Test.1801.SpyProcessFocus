using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Time时间记录器.Util;

namespace Time时间记录器.UI
{
	public class AppList:Control
	{
		private SortedList<string, App> list=new SortedList<string, App>();
		public AppList()
		{
			onResize = new OnResizeDelegate(OnResizeRaise);
		}
		public void Add(App app)
		{
			app.layoutParent = this;
			app.Parent = this;
			list.Add(app.ProcessName, app);
		}
		public bool Remove(string name)
		{
			foreach(var appC in this.Controls)
			{
				var app = (App)appC;
				if (app.Name == name)
				{
					Controls.Remove(app);
					list.Remove(app.Name);
					return true;
				}
			}
			return false;
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

						var lastAppBottom = Controls[Controls.Count - 1].Bottom;
						if (lastAppBottom < Height)
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
				lastY = e.Location.Y;
				this.OnResize(EventArgs.Empty);
			}
			base.OnMouseMove(e);
		}
		private bool mouseIsDown = false;
		private float nowY=0,targetY=0;

		public bool MouseIsDown { get => mouseIsDown; set => mouseIsDown = value; }
		public float NowY { get => nowY; set => nowY = value; }
		public float TargetY { get => targetY; set => targetY = value; }
		private void OnResizeRaise()
		{
			OnResize(EventArgs.Empty);
		}
		protected override void OnResize(EventArgs e)
		{
			int height = 100;
			int lastY =(int)nowY;
			for (int i = 0; i < Controls.Count; i++)
			{
				if (Controls[i] is App app)
				{
					app.DBounds=new System.Drawing.Rectangle(0,lastY ,Width, height);
					lastY += (int)(height*1.05);
				}
			}
		}
		public void RefreshData(ProcessGroup process)
		{
			for(int i=0;i< process.Process.Count; i++)
			{
				var p = process.Process[i];
				if (list.ContainsKey(p.RemarkName))
				{

				}
				else
				{
					App app = new App() {
						ProcessName = p.RemarkName,
					};
					app.TimeLine.TodayTime = 0;
					app.TimeLine.AvgTime = 0;
					app.TimeLine.SoftAvgTime = 0;

					Add(app);
					//app.Frequency
				}
			}
			
		}

	}
}
