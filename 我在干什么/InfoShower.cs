using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Inst
{
	public partial class InfoShower : Form
	{
		public InfoShower()
		{

			InitializeComponent();
			existTime = 5000;
			this.Visible = false;
			this.Click += OnClickToHide;
			this.DoubleBuffered = true;
			this.TopMost = true;
			TopMost = false;//取消
		}

		private void OnClickToHide(object sender, EventArgs e)
		{
			CallBack?.Invoke();
			this.existTime = 0;
		}
		public Action CallBack;
		static InfoShower()
		{
			titleFont = new Font("微软雅黑", 18);
			infoFont = new Font("微软雅黑", 12);
			
		}
		private static Font titleFont ;
		private static Font infoFont;
		private string title;
		private string info;
		private int existTime;
		public ToolTipIcon ToolTip = ToolTipIcon.None;
		private Color titleColor = Color.CornflowerBlue;
		private Color infoColor = Color.White;
		private static List<InfoShower> list=new List<InfoShower>();
		public static void ShowOnce(InfoShower newInfo)
		{
			newInfo.FormClosed += (x,xx) => {
				list.Remove(newInfo);
				ReQueueInfo();
			};
			
			list.Add(newInfo);
			ReQueueInfo();
			newInfo.ShowDirect();

		}
		private int showOutStamp;
		private int targetY;//当前纵坐标
		public void ShowDirect()
		{
			this.Visible = true;
			ShowOutStamp = Environment.TickCount;
			this.Top = TargetY;
			var targetX = Screen.PrimaryScreen.Bounds.Right - this.Width;

			
			var showOutFlash = new Thread(() =>
			{
				try
				{
					this.Invoke((EventHandler)delegate
					{
						this.Left = Screen.PrimaryScreen.Bounds.Right;
					});
					while (Math.Abs(this.Left - targetX) > 5 || Environment.TickCount - ShowOutStamp <= ExistTime)
					{
						this.Invalidate();
						Thread.Sleep(50);
						this.Invoke((EventHandler)delegate
						{
							this.Left = (int)(targetX * 0.2 + this.Left * 0.8) - 1;
							this.Top = (int)(TargetY * 0.2 + this.Top * 0.8) - 1;
							this.Opacity = this.Opacity * 0.8 + 20;
						});
					}
					this.Invoke((EventHandler)delegate
					{
						this.Left = targetX;
					});

					TargetY = (int)(Screen.PrimaryScreen.Bounds.Height * 0.02);
					while (Math.Abs(this.Top - TargetY) > 1)
					{
						Thread.Sleep(50);
						this.Invoke((EventHandler)delegate
						{
							this.Top = (int)(TargetY * 0.2 + this.Top * 0.8) - 1;
							this.Opacity = this.Opacity * 0.8;
						});
					}
					this.Invoke((EventHandler)delegate
					{
						this.Hide();
						this.Close();
					});
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			})
			{ IsBackground=true};
			showOutFlash.Start();
		}
		private static void ReQueueInfo()
		{
			int beginY =(int)(Screen.PrimaryScreen.Bounds.Height * 0.05);
			int endY = (int)(Screen.PrimaryScreen.Bounds.Height * 0.8);
			var screenHeight = Screen.PrimaryScreen.Bounds.Height;
			int infoCapacity = (endY- beginY ) / 125;
			if(infoCapacity< list.Count)
			{
				for(int i = 0; i < list.Count - infoCapacity;i++)
				{
					list[i].ExistTime = 0;
					list.Remove(list[i]);
				}
				for(int i = list.Count - infoCapacity; i < list.Count; i++)
				{
					list[i].TargetY = 125 * (i- list.Count + infoCapacity) + beginY;
				}
			}else
			for (int i = 0; i < list.Count; i++)
			{
				list[i].TargetY = 125 * i+beginY;
			}
		}
		public string Title { get => title; set => title = value; }
		public string Info { get => info; set => info = value; }
		public int ExistTime { get => existTime; set => existTime = value; }
		public int ShowOutStamp { get => showOutStamp; set => showOutStamp = value; }
		public int TargetY { get => targetY; set => targetY = value; }
		public Color TitleColor { get => titleColor; set
			{
				titleColor = value;
				titleBrush = new SolidBrush(titleColor);
			} }
		public Color InfoColor { get => infoColor; set
			{
				infoColor = value;
				infoBrush = new SolidBrush(infoColor);
			}
		}
		private Brush titleBrush=Brushes.CornflowerBlue;
		private Brush infoBrush = Brushes.White;
		protected override void OnPaint(PaintEventArgs e)
		{
			var strSize = e.Graphics.MeasureString(title, titleFont);
			e.Graphics.DrawString(title, titleFont, titleBrush, Width*0.05f, 5);
			e.Graphics.DrawString(info, infoFont, infoBrush, new RectangleF(Width*0.05f,5+strSize.Height,Width*0.95f,Height-5));
		}
	}
}
