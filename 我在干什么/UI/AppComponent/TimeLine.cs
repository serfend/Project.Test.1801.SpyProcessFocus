using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Inst.UI.AppComponent
{
	public class TimeLine : Control
	{
		//本软件今日用时/本软件日平均用时/所有软件日平均最高用时 
		private int todayTime, avgTime;
		private int softAvgTime;
		private Color deActiveColor ;
		private Brush deActiveBrush;
		public TimeLine()
		{
			BackColor = System.Drawing.Color.LightGray;
			DeActiveColor = Color.FromArgb(255, 123, 191, 234);
		}

		public override bool RefreshLayout()
		{
			bool changed = anyChange;
			if (SoftAvgTime > 0)
			{
				if (changed)
				{
					todayPercent = (float)todayTime / (float)softAvgTime;
					if (todayPercent > 1) todayPercent = 1;
					 anyChange = false;
					this.Invalidate();
				}
			}

			return base.RefreshLayout()||changed;
		}
		public int TodayTime { get => todayTime; set { todayTime = value; anyChange = true; } }
		public int AvgTime { get => avgTime; set { avgTime = value; anyChange = true; } }
		public int SoftAvgTime { get => softAvgTime; set { softAvgTime = value; anyChange = true; } }

		public Color DeActiveColor
		{
			get => deActiveColor; set
			{
				deActiveColor = value;
				deActiveBrush = new SolidBrush(deActiveColor);
			}
		}

		private bool anyChange = false;
		private float todayPercent,avgPercent;
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.FillRectangle(deActiveBrush, 0, 0, todayPercent * Width, Height);//今日

			int time = TodayTime;
			int h = time / 3600;
			time -= h * 3600;
			int m =time/60;
			int s = time - m * 60;
			var str = string.Format("{0}h{1}m{2}s",h,m,s);
			var strSize = e.Graphics.MeasureString(str, Font);
			e.Graphics.DrawString(str,Font,System.Drawing.Brushes.Gray, (float)((Width - strSize.Width) * 0.5), (float)((Height - strSize.Height) * 0.5));
			
		}
	}
}
