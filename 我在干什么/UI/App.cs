using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Time时间记录器.UI.AppComponent;
using Time时间记录器.Util;

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
		public int Index;
		public App()
		{
			BackColor = Color.White;
			ForeColor = Color.Black;
			Font = new Font("微软雅黑", 10);
			logo = new Logo((x) => { })
			{
				Parent = this
			};
			title = new Bar.BtnNormal((x) => {
				var p = Program.frmMain._process[name];
				DotNet4.Utilities.UtilInput.InputBox.ShowInputBox("修改备注", "修改进程备注名称", p.RemarkName, (newName) =>
				{
					p.RemarkName = newName;
				});
			})
			{
				Parent = this,
				deactiveColor = Color.FromArgb(200, 91, 155, 213)
			};
			TimeLine = new TimeLine()
			{
				TodayTime = 0,
				AvgTime = 0,
				SoftAvgTime = 0,
				Parent = this
			};
			frequency = new UseFrequency() {  Parent=this};
		}
		protected override void OnResize(EventArgs e)
		{
			logo.DBounds = new Rectangle(10, 35, 90, 90);
			title.DBounds = new Rectangle(10, 5, 90, 30);
			TimeLine.DBounds = new Rectangle(100, 5, Width - 110, 30);
			base.OnResize(e);
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

		public string ProcessName { get => name; set {
				name = value;
				title.Text = Program.frmMain._process[name].RemarkName;
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
