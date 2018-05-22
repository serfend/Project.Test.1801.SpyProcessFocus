using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Time时间记录器.UI.AppComponent
{
	class TimeLine : Control
	{
		//本软件今日用时/本软件日平均用时/所有软件日平均最高用时 
		private int todayTime, avgTime;
		private int softAvgTime;
		private Bar.BtnNormal today, avg;
		public TimeLine()
		{
			today = new Bar.BtnNormal((x) => { })
			{
				Parent = this,
			};
			avg = new Bar.BtnNormal((x) => { })
			{
				Parent = this,
			};
		}
		public override bool RefreshLayout()
		{
			if (SoftAvgTime > 0)
			{
				today.Width = Width * TodayTime / SoftAvgTime;
				avg.Width = Width * AvgTime / SoftAvgTime;

			}

			return base.RefreshLayout();
		}
		public int TodayTime { get => todayTime; set => todayTime = value; }
		public int AvgTime { get => avgTime; set => avgTime = value; }
		public int SoftAvgTime { get => softAvgTime; set => softAvgTime = value; }

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			int time = TodayTime;
			int h = time / 86400;
			time -= h * 3600;
			int m =time/60;
			int s = time - m * 60;
			var str = string.Format("{0}h{1}m{2}s",h,m,s);
			var strSize = e.Graphics.MeasureString(str, Font);
			e.Graphics.DrawString(str,Font,System.Drawing.Brushes.Gray, (float)((Width - strSize.Width) * 0.5), (float)((Height - strSize.Height) * 0.5));
			
		}
	}
}
