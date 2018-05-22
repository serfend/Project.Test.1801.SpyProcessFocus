using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Time时间记录器.UI.AppComponent;

namespace Time时间记录器.UI
{
	public class App:Control
	{
		private string name;
		private AppComponent.Logo logo;
		private AppComponent.TimeLine timeLine;
		private Bar.BtnNormal title;
		private AppComponent.UseFrequency frequency;
		public AppList layoutParent;
		public App()
		{
			BackColor = Color.BlueViolet;
			TimeLine = new TimeLine()
			{
				TodayTime = 0,
				AvgTime = 0,
				SoftAvgTime = 0,
			};
			frequency = new UseFrequency() { };
			title = new Bar.BtnNormal((x) => { })
			{

			};
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			layoutParent.MouseIsDown = true;
			lastY = e.Location.Y;
			base.OnMouseDown(e);
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			layoutParent.MouseIsDown = false;
			base.OnMouseUp(e);
		}
		private int lastY;
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (layoutParent.MouseIsDown)
			{
				layoutParent.TargetY += (e.Location.Y - lastY) * 5;
				lastY = e.Location.Y;
				this.OnResize(EventArgs.Empty);
			}
			base.OnMouseMove(e);
		}
		private bool mouseIsDown = false;

		public string ProcessName { get => name; set {
				//title.Text = value;
				name = value;
			} }
		internal Logo Logo { get => logo; set => logo = value; }
		internal TimeLine TimeLine { get => timeLine; set => timeLine = value; }
		public UseFrequency Frequency { get => frequency; set => frequency = value; }

		protected override void OnPaint(PaintEventArgs e)
		{
			//TODO 软件实例
			//程序logo  使用时间条
			//程序名称                                                  
			//展开后展示最近使用（略）        频率图表                展开/收回 按钮
			base.OnPaint(e);
			//e.Graphics.FillRectangle(Brushes.Aquamarine,0, 0, Bounds.Width,Bounds.Height);
		}
	}
}
