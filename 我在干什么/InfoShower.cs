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
		}
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
		public void ShowOnce()
		{
			var t = new Thread(()=> {

			});
			t.Start();
		}
		public string Title { get => title; set => title = value; }
		public string Info { get => info; set => info = value; }
		public int ExistTime { get => existTime; set => existTime = value; }
		protected override void OnPaint(PaintEventArgs e)
		{
			var strSize = e.Graphics.MeasureString(title, titleFont);
			e.Graphics.DrawString(title, titleFont, Brushes.CornflowerBlue, Width*0.2f, 5);
			e.Graphics.DrawString(info, infoFont, Brushes.White,new RectangleF(Width*0.2f,5+strSize.Height,Width*0.8f,Height-5));
		}
	}
}
