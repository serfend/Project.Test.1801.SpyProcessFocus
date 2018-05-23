using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 时间管理大师.UI.AppComponent
{
	public class UseFrequency:Control
	{
		//TODO 在此处绘制软件一日使用频率表
		//每4小时一个大点，1小时一个小点
		//以平滑曲线连接
		private int[] Count = new int[24];
		private int maxCount=-1, minCount=-1;
		public void SetCount(int h,int value)
		{
			Count[h] = value;
			if (maxCount == -1) maxCount=minCount = value;
			else
			{
				if (maxCount < value) maxCount = value;
				if (minCount > value) minCount = value;
			}
		}
		private Color pointColor;
		private Color lineColor;
		public UseFrequency()
		{
			BackColor = Color.AliceBlue;
			PointColor = Color.FromArgb(75, 145, 209);
			LineColor = Color.FromArgb(75, 145, 209);
			//var rnd = new Random();
			//for (int i = 0; i < 24; i++) SetCount(i, rnd.Next(0, 50));
		}
		public Color PointColor { get => pointColor; set {
				pointColor = value;
				PointBrush = new SolidBrush(pointColor);
			} }
		public Color LineColor { get => lineColor; set {
				lineColor = value;
				LineBrush = new Pen(lineColor, 2);
			} }
		private SolidBrush PointBrush;
		private Pen LineBrush;

		private Point[] points=new Point[24];
		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			int l = (int)(minCount * 0.9),h=(int)(maxCount*1.1);
			float w = (float)Width / 25f;
			for (int i = 0; i < 24; i++)
			{
				points[i] = new Point(15 + (int)(w * i), ConvertValueToTop(Count[i], l, h));
				e.Graphics.FillEllipse(PointBrush,points[i].X-2,points[i].Y-2,5,5);
				if (i % 4 == 0) e.Graphics.DrawString(i.ToString(), Font, Brushes.Gray, (float)points[i].X, (float)(Height-15));
			}
			
			e.Graphics.DrawCurve(LineBrush, points);
			base.OnPaint(e);
		}
		private int ConvertValueToTop(int value,int minCount,int maxCount)
		{
			return (int)(140f * (1 -(float)(value-minCount)/(maxCount-minCount)));
		}

	}
}
