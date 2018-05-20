using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Time时间记录器.UI
{
	public class 时间分布饼图:Control
	{
		private List<Brush> brushs = new List<Brush>();

		public 时间分布饼图()
		{
			InitBrush();
		}
		private bool layoutUpdate = false;
		public override bool RefreshLayout()
		{
			float sumChange = 0;
			foreach (var d in Data)
			{
				float change = d.Value.nowValue;
				d.Value.nowValue = MovingSpeed * d.Value.targetValue + (1 - MovingSpeed) * d.Value.nowValue;
				change -= d.Value.nowValue;
				sumChange += Math.Abs(change);
			}
			if (sumChange > 0.01 || layoutUpdate)
			{
				layoutUpdate = false;
				this.Invalidate();
			}
			return sumChange > 0.01 || base.RefreshLayout();
		}
		protected override void OnResize(EventArgs e)
		{
			this.ShowPos = DBounds;
			this.HidePos = new Rectangle((int)((DBounds.X + DBounds.Width * 0.5)), (int)(0.5 * (DBounds.Y + DBounds.Height*0.5)),0,0);
			layoutUpdate = true;
		}
		public override void 隐藏()
		{
			ShowOut = false;
			this.ModifyState(false);
		}
		public override void 显示()
		{
			ShowOut = true;
			this.ModifyState(true);
		}
		

		private void ModifyState(bool show)
		{
			var t = new Task(() => {		
				float targetAngle = (show ? 360f : 0f);
				while (Math.Abs(nowMaxAngle - targetAngle) > 0.5)
				{
					if (ShowOut != show) return;
					nowMaxAngle = nowMaxAngle * (1 - MovingSpeed) + targetAngle * MovingSpeed;
					this.Invalidate();
					Thread.Sleep(50);
				}
				nowMaxAngle = targetAngle;
				
			});
			t.Start();
		}
		private float nowMaxAngle = 360f;//用于出现消失动画
		protected override void OnPaint(PaintEventArgs e)
		{
			//base.OnPaint(e)
			try
			{
				float lastValue = 0f;
				foreach (var p in this.Data)
				{
					this.DrawPie(e.Graphics, p.Value.colorIndex, p.Key, lastValue, p.Value.nowValue);
					lastValue += p.Value.nowValue;
				}
			}
			catch (Exception)
			{

				throw;
			};
			
		}
		private SortedList<string, ProcessData> Data = new SortedList<string, ProcessData>();//原始数据 进程备注+<时间,目标权值,当前权值>
		public void RefreshData(ProcessGroup process)
		{

			int sumTime = 0;
			foreach (var p in process.Process)//刷新
			{
				ProcessData target = null;
				if (Data.ContainsKey(p.RemarkName))
				{
					target = Data[p.RemarkName];
				}
				else
				{
					target = new ProcessData() { colorIndex = nowColorIndex++ };
					if (nowColorIndex == 10) nowColorIndex = 0;
					Data.Add(p.RemarkName, target);
				}
				target.time = p.SumUsedTime();
				sumTime += target.time;
			}
			if (sumTime == 0) sumTime = 10;
			foreach (var data in Data)//重新规划
			{
				data.Value.targetValue = 100f * data.Value.time / sumTime;
			}
		}
		private class ProcessData
		{
			public int time;
			public float targetValue;
			public float nowValue;
			public int colorIndex;
		}
		private int nowColorIndex;
		private void InitBrush()
		{
			var rnd = new Random();
			for (int i = 0; i < 10; i++)
			{
				var brush = new SolidBrush(Color.FromArgb(rnd.Next(100, 255), rnd.Next(100, 255), rnd.Next(100, 255)));//创建随机画笔
				brushs.Add(brush);
			}
		}

		private Brush defaultBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
		private void DrawPie(Graphics g, int colorIndex, string name, float startAngle, float weight)
		{
			g.FillPie(brushs[colorIndex], new Rectangle(0,0,(int)(Width ), (int)(Height)), startAngle * nowMaxAngle / 100, weight * nowMaxAngle / 100);

			var pos = new Point((int)( Width * 0.5 ), (int)( Height * 0.5));
			var angle = (startAngle * 2 + weight) * 0.5 * 0.01 * nowMaxAngle;
			var radian = (double)(angle * Math.PI / 180);
			if (weight * nowMaxAngle * 0.01 > 36)//仅显示大于36度（10%）的
			{
				var str = string.Format("{0}\n{1}%", name, Math.Round(weight, 1));
				var strSize = g.MeasureString(str, Font);
				g.DrawString(str, Font, defaultBrush, new PointF(pos.X + Width * (float)Math.Cos(radian) * 0.3f-(float)strSize.Width*0.5f, pos.Y + Height*(float)Math.Sin(radian) * 0.3f-0.5f*(float)strSize.Height));
			}
		}
	}
}
