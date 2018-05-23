using DotNet4.Utilities.UtilInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using 时间管理大师.UI.AppComponent;
using 时间管理大师.UI.Bar;
using 时间管理大师.Util;

namespace 时间管理大师.UI
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
		public AppComponent.AppRelate RelateApp;

		public AppComponent.UseFrequency frequency;
		public AppList layoutParent;
		public int RawIndex;
		public int Index;
		public App(string name)
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
			ProcessName = name;
			TimeLine = new TimeLine()
			{
				TodayTime = 0,
				AvgTime = 0,
				SoftAvgTime = 0,
				Parent = this
			};
			sumUsedTime = new Bar.BtnNormal((x) => { }) {Parent=this,Text="总耗时" };
			sumUsedCount = new Bar.BtnNormal((x) => { }) { Parent = this,Text="总激活" };
			SumUsedTimeLine = new TimeLine() { Parent=this};
			SumUsedCountLine = new Bar.BtnNormal((x)=> { }) { Parent=this};
			frequency = new UseFrequency() {  Parent=this};

			relateAppLabel = new BtnNormal((x) =>
			{
				InputBox.ShowInputBox("修改", "修改显示相关应用的数量", RelateApp.ShowIconNum.ToString(), (ans) =>
				{
					RelateApp.ShowIconNum = Convert.ToInt32(ans);
				});
			})
			{ Parent = this, Text = "相关应用" };
			RelateApp = new AppRelate((x)=> { },name) {
				Parent=this,
				
			};
		}

		protected override void OnResize(EventArgs e)
		{
			logo.DBounds = new Rectangle(10, 35, 90, 90);
			title.DBounds = new Rectangle(10, 5, 90, 30);
			sumUsedTime.DBounds = new Rectangle(101, 37, 50, 30);
			SumUsedTimeLine.DBounds = new Rectangle(151, 37,Width-661, 30);
			sumUsedCount.DBounds = new Rectangle(101, 69, 50, 30);
			relateAppLabel.DBounds = new Rectangle(101,101,50,24);
			RelateApp.DBounds = new Rectangle(151, 101,Width-661, 24);
			SumUsedCountLine.DBounds = new Rectangle(151, 69, 60, 30);
			TimeLine.DBounds = new Rectangle(100, 5, Width - 110, 30);
			frequency.DBounds = new Rectangle(Width - 500, 40, 490, 160);
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
		public TimeLine SumUsedTimeLine { get => sumUsedTimeLine; set => sumUsedTimeLine = value; }
		public BtnNormal SumUsedCountLine { get => sumUsedCountLine; set => sumUsedCountLine = value; }

		protected override void OnPaint(PaintEventArgs e)
		{
			//软件实例
			//程序logo  使用时间条
			//程序名称                                                  
			//展开后展示最近使用（略）        频率图表                展开/收回 按钮
			base.OnPaint(e);
			//e.Graphics.FillRectangle(Brushes.Aquamarine,0, 0, Bounds.Width,Bounds.Height);
		}
	}
}
