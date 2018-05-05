using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing.Drawing2D;

namespace Time时间记录器.Util
{
	public class UI
	{
		private Graphics g;
		private List<Brush> brushs=new List<Brush>();
		private Rectangle pos;
		private Rectangle boundPos;
		public UI(Graphics g)
		{
			this.g = g;
			InitBrush();
			Pos = new Rectangle(new Point(20, 20), new Size(200, 200));
			var t = new Task(()=> {
				while (g != null)
				{
					this.OnPaint();
					Thread.Sleep(10);
				}
			});
			t.Start();
			this.Show();
		}

		private bool showOut = false;
		public void Show()
		{
			showOut = true;
			ModifyState(true);
		}
		public void Hide()
		{
			showOut = false;
			ModifyState(false);
		}
		private void ModifyState(bool show)
		{
			
			var t = new Task(() => {
				float targetAngle = (show ? 360f : 0f);
				while ( Math.Abs(nowMaxAngle - targetAngle) >0.1)
				{
					if (showOut != show) return;
					nowMaxAngle = nowMaxAngle * 0.9f + targetAngle * 0.1f+(show ? 0.2f:-0.2f);
					Thread.Sleep(50);
				}
				nowMaxAngle = targetAngle;
			});
			t.Start();
			
		}
		private float nowMaxAngle = 0;//用于出现消失动画
		public void OnPaint()
		{
			try
			{
				BufferedGraphicsContext GraphicsContext = BufferedGraphicsManager.Current;
				BufferedGraphics myBuffer = GraphicsContext.Allocate(g, boundPos);


				Graphics tmpG = myBuffer.Graphics;
				tmpG.PixelOffsetMode = PixelOffsetMode.HighQuality;
				tmpG.SmoothingMode = SmoothingMode.HighQuality;
				tmpG.Clear(Color.White);
				float nowAngle = 0;
				for (int i = 0; i < Data.Count; i++)
				{
					var data = Data.Values[i];
					if (data == null) continue;
					data.nowValue = data.nowValue * 0.8f + data.targetValue * 0.2f;//更新权值
				}
				for (int i = 0; i < Data.Count; i++)
				{
					var data = Data.Values[i];
					if (data == null) continue;
					this.DrawPie(tmpG, data.colorIndex, Data.Keys[i], nowAngle,  data.nowValue);
					nowAngle += data.nowValue;
				}
				myBuffer.Render();
				tmpG.Dispose();
				myBuffer.Dispose();
			}
			catch (Exception)
			{

				
			}
			
		}
		private SortedList<string, ProcessData> Data=new SortedList<string, ProcessData>();//原始数据 进程备注+<时间,目标权值,当前权值>

		public Rectangle Pos { get => pos; set {
				pos = value;
				boundPos = new Rectangle(pos.X - pos.Width, pos.Y - pos.Height,pos.X+2*pos.Width,pos.Y+pos.Height*2);
			}  }

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
					target = new ProcessData() { colorIndex=nowColorIndex++};
					if (nowColorIndex == 10) nowColorIndex = 0;
					Data.Add(p.RemarkName, target);
				}
				target.time = p.SumUsedTime();
				sumTime += target.time;
			}
			sumTime += 10;//防0
			foreach (var data in Data)//重新规划
			{
				data.Value.targetValue =100f* data.Value.time / sumTime;
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
		private Font defaultFont = new Font("微软雅黑", 10);
		private Brush defaultBrush=new SolidBrush(Color.Black);
		private void DrawPie(Graphics g, int colorIndex,string name,float startAngle,float weight)
		{
			g.FillPie(brushs[colorIndex], Pos, startAngle*nowMaxAngle/100, weight * nowMaxAngle / 100);


			var pos = new Point((int)((Pos.X + Pos.Width) * 0.5),(int)( (Pos.Y + Pos.Height) * 0.5));
			var angle = (startAngle*2+weight)*0.5 * 0.01 * nowMaxAngle;
			var radian =(double)(angle * Math.PI/180);
			if (weight* nowMaxAngle*0.01 > 36)//仅显示大于36度（10%）的
			{
				var str = string.Format("{0}\n{1}%", name, Math.Round(weight, 1));
				var strSize = g.MeasureString(str, defaultFont);
				g.DrawString(str, defaultFont, defaultBrush, new PointF(pos.X - strSize.Width * 0.5f + Pos.Width * (float)Math.Cos(radian) * 0.4f, pos.Y + Pos.Height * (float)Math.Sin(radian) * 0.4f));
			}

		}
	}
}
