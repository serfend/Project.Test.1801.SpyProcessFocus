using DotNet4.Utilities.UtilInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Inst.UI.AppComponent;
using Inst.UI.Bar;
using Inst.Util;
using Inst.Util.Image;

namespace Inst.UI
{
	public class App:Control
	{
		private string name;
		private AppComponent.Logo logo;
		private AppComponent.TimeLine timeLine;
		private Bar.BtnNormal title;

		private Bar.BtnNormal sumUsedTime;
		private AppComponent.TimeLine sumUsedTimeLine;
		private Bar.BtnNormal sumUsedCount;
		private Bar.BtnNormal sumUsedCountLine;

		private Bar.BtnNormal relateAppLabel;

		public bool AppIsActive = false;
		public AppComponent.AppRelate RelateApp;

		public AppComponent.UseFrequency frequency;
		public int RawIndex;
		public int Index;
		public App(string name)
		{
			pen = new Pen(boardColor, 1);
			BackColor = Color.White;
			ForeColor = Color.Black;
			Font = new Font("微软雅黑", 10,FontStyle.Bold);
			logo = new Logo((x) => { })
			{
				Parent = this
			};
			title = new Bar.BtnNormal((x) => {
				var p = Program.frmMain._process[name];
				DotNet4.Utilities.UtilInput.InputBox.ShowInputBox("修改备注", "修改进程备注名称", p.RemarkName, (newName) =>
				{
					p.RemarkName = newName;
					ProcessName = ProcessName;
				});
			})
			{
				Parent = this,
				deactiveColor = Color.FromArgb(200, 91, 155, 213),
				Font=this.Font
			};
			ProcessName = name;
			TimeLine = new TimeLine()
			{
				TodayTime = 0,
				AvgTime = 0,
				SoftAvgTime = 0,
				Parent = this,
				Font = this.Font
			};
			sumUsedTime = new Bar.BtnNormal((x) => { }) {Parent=this,Text="总耗时",Font=this.Font};
			sumUsedCount = new Bar.BtnNormal((x) => { }) { Parent = this,Text= "活跃度", Font = this.Font };
			SumUsedTimeLine = new TimeLine() { Parent=this, Font = this.Font };
			SumUsedCountLine = new Bar.BtnNormal((x)=> { }) { Parent=this, Font = this.Font };
			frequency = new UseFrequency() {  Parent=this, Font = this.Font };

			relateAppLabel = new BtnNormal((x) =>
			{
				InputBox.ShowInputBox("修改", "修改显示相关应用的数量", RelateApp.ShowIconNum.ToString(), (ans) =>
				{
					RelateApp.ShowIconNum = Convert.ToInt32(ans);
				});
			})
			{ Parent = this, Text = "相关应用", Font = this.Font };
			RelateApp = new AppRelate((x)=> { },name) {
				Parent=this,
				Font = this.Font
			};
		}

		protected override void OnResize(EventArgs e)
		{
			logo.DBounds = new Rectangle(10, 40, 90, 90);
			title.DBounds = new Rectangle(10, 10, 90, 30);
			sumUsedTime.DBounds = new Rectangle(101, 42, 50, 30);
			SumUsedTimeLine.DBounds = new Rectangle(151, 42,Width-661, 30);
			sumUsedCount.DBounds = new Rectangle(101, 74, 50, 30);
			relateAppLabel.DBounds = new Rectangle(10,135,90,24);
			RelateApp.DBounds = new Rectangle(105, 135,Width-661, 24);
			SumUsedCountLine.DBounds = new Rectangle(151, 74, 60, 30);
			TimeLine.DBounds = new Rectangle(100, 10, Width - 110, 30);
			frequency.DBounds = new Rectangle(Width - 500, 42, 490, 150);
			base.OnResize(e);
		}
		private int lastY;
		public AppList layoutParent;
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			layoutParent.TargetY += e.Delta;
			base.OnMouseWheel(e);
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (layoutParent.MouseIsDown)
			{
				layoutParent.TargetY +=(e.Y  - layoutParent.lastY)*2;
				layoutParent.lastY = e.Y;
			}
			base.OnMouseMove(e);
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			layoutParent.MouseIsDown = false;
			base.OnMouseUp(e);
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			lastY = e.Y;
			layoutParent.MouseIsDown = true;
			base.OnMouseDown(e);
		}

		public string ProcessName { get => name; set {
				name = value;
				title.Text = Program.frmMain._process[name].RemarkName;
				title.Invalidate();
			} }
		internal Logo Logo { get => logo; set => logo = value; }
		internal TimeLine TimeLine { get => timeLine; set => timeLine = value; }
		public UseFrequency Frequency { get => frequency; set => frequency = value; }
		public TimeLine SumUsedTimeLine { get => sumUsedTimeLine; set => sumUsedTimeLine = value; }
		public BtnNormal SumUsedCountLine { get => sumUsedCountLine; set => sumUsedCountLine = value; }
		
		private Color boardColor = Color.FromArgb(0, 0, 0, 0);
		private Color boardActiveColor = Color.FromArgb(0, 0, 0, 255);
		private Pen pen;
		protected override void OnPaint(PaintEventArgs e)
		{
			//软件实例
			//程序logo  使用时间条
			//程序名称                                                  
			//展开后展示最近使用（略）        频率图表                展开/收回 按钮
			//base.OnPaint(e);
			int shadowWidth = 10;
			for (int i = 0; i < shadowWidth; i++)
			{
				pen.Color = Color.FromArgb((255 / shadowWidth / shadowWidth) * i, AppIsActive?boardActiveColor :boardColor);
				GDIShadow.DrawRoundRectangle(e.Graphics, pen, new Rectangle(i, i, Width - (2 * i) - 1, Height - (2 * i) - 1), 8);
			}
			//e.Graphics.FillRectangle(Brushes.Aquamarine,0, 0, Bounds.Width,Bounds.Height);
		}
	}
}
