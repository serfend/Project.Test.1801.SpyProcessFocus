using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Time时间记录器.Util;

namespace Time时间记录器.UI
{
	public class 时间分布饼图:Control
	{
		private List<Brush> brushs = new List<Brush>();

		public 时间分布饼图()
		{
			
		}

		private bool layoutUpdate = false;
		public override bool RefreshLayout()
		{
			float sumChange = 0;
			for(int i = 0; i < Data.Count; i++)
			{
				var d = Data.Values[i];
				float change = d.nowValue;
				d.nowValue = MovingSpeed * d.targetValue + (1 - MovingSpeed) * d.nowValue;
				change -= d.nowValue;
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
					if (!Program.UsedFlash) nowMaxAngle = targetAngle;else
					nowMaxAngle = nowMaxAngle * (1 - MovingSpeed) + targetAngle * MovingSpeed;
					var nowColor = 255 - 55* nowMaxAngle / 360;
					Parent.BackColor = Color.FromArgb(255, (int)nowColor, (int)nowColor, (int)nowColor);
					this.BackColor = Parent.BackColor;
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
			base.OnPaint(e);
			if (nowMaxAngle < 0.2) return;
			try
			{
				e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
				float lastValue = 0f;
				
				foreach (var p in this.Data)
				{
					this.DrawPie(e.Graphics, p.Value.ColorIndex, p.Key, lastValue, p.Value.nowValue);
					this.DrawInfo(e.Graphics,p.Value.Rank, p.Value.ColorIndex, p.Key, p.Value.nowValue);
					lastValue += p.Value.nowValue;
				}
			}
			catch (Exception)
			{

			};
			
		}



		private SortedList<string, ProcessData> Data = new SortedList<string, ProcessData>();//原始数据 进程备注+<时间,目标权值,当前权值>
		private int nowNum = 0;
		public void RefreshData(ProcessGroup process)
		{
			try
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
						brushs.Add(new SolidBrush(p.AppInfo.IconMainColor));
						target = new ProcessData() { ColorIndex = brushs.Count - 1, Rank = nowNum++ };
						Data.Add(p.RemarkName, target);
					}
					target.time = Program.ProcessData[p.ProcessName].TodayWasteTime;
					sumTime += target.time;
				}
				if (sumTime == 0) sumTime = 10;
				foreach (var data in Data)//重新规划
				{
					data.Value.targetValue = 100f * data.Value.time / sumTime;
				}
				ProcessData.RefreshRank(ref this.Data);
			}
			catch (Exception)
			{
				
			}
		}
		private class ProcessData
		{
			public int time;
			public float targetValue;
			public float nowValue;
			private int colorIndex;
			public int Rank { get; internal set; }
			public int ColorIndex { get => colorIndex; set => colorIndex = value; }

			public static bool Cmp(ProcessData a,ProcessData b)
			{
				return a.targetValue > b.targetValue;
			}
			public static void RefreshRank(ref SortedList<string,ProcessData>data)
			{
				foreach(var a in data)
				{
					foreach(var b in data)
					{
						if (a.Key == b.Key) continue;
						if (Cmp(a.Value, b.Value)&&a.Value.Rank>b.Value.Rank)
						{
							a.Value.Rank ^= b.Value.Rank;
							b.Value.Rank ^= a.Value.Rank;
							a.Value.Rank ^= b.Value.Rank;
						}
					}
				}
			}
		}


		private Brush defaultBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
		private void DrawInfo(Graphics g,int infoIndex, int colorIndex, string name, float weight)
		{
			int maxItemNum = 10;//最多个数
			int columItemShowNum = 5;//每栏5个
			if (infoIndex >= maxItemNum) return;
			var left = (int)(Height * 1.1);
			
			var size = (int)(Height * 0.05);
			
			var strSize = g.MeasureString("好", Font);
			var sigleHeight = Height * (1/ (double)columItemShowNum);
			var topCenter = (float)((sigleHeight - strSize.Height) * 0.5 + sigleHeight * ((infoIndex )% columItemShowNum));
			topCenter += (nowMaxAngle/360-1)*Height;//图例动画
			var leftBegin = (int)(left  + (left*1.5) * (int)((infoIndex ) / columItemShowNum ));//换行
			var rect = new Rectangle((int)(leftBegin), (int)(topCenter+size*0.25), size*2, size*2);

			g.FillRectangle(brushs[colorIndex], rect);
			g.DrawString(name + "(" + Math.Round(weight , 2) + "%)",Font,defaultBrush, leftBegin+(int)(left*0.1), topCenter);
		}
		private void DrawPie(Graphics g, int colorIndex, string name, float startAngle, float weight)
		{
			
			g.FillPie(brushs[colorIndex], new Rectangle(0,0,(int)(Height), (int)(Height)), startAngle * nowMaxAngle / 100, weight * nowMaxAngle / 100);

			//启用在图例中标识
			//var pos = new Point((int)( Width * 0.5 ), (int)( Height * 0.5));
			//var angle = (startAngle * 2 + weight) * 0.5 * 0.01 * nowMaxAngle;
			//var radian = (double)(angle * Math.PI / 180);
			//if (weight * nowMaxAngle * 0.01 > 36)//仅显示大于36度（10%）的
			//{
			//	var str = string.Format("{0}\n{1}%", name, Math.Round(weight, 1));
			//	var strSize = g.MeasureString(str, Font);
			//	g.DrawString(str, Font, defaultBrush, new PointF(pos.X + Width * (float)Math.Cos(radian) * 0.3f-(float)strSize.Width*0.5f, pos.Y + Height*(float)Math.Sin(radian) * 0.3f-0.5f*(float)strSize.Height));
			//}
		}
	}
}
