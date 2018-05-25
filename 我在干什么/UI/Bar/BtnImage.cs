using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Inst.UI.Bar
{
	public class BtnImage:Control
	{
		public Image Image;
		public bool Center = false;
		public BtnImage(Action<Control> CallBack) : base(CallBack)
		{
			
		}

		public override bool RefreshLayout()
		{
			return false;
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			if(Image==null)return;
			float width = Height*Image.Width / Image.Height;
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.DrawImage(Image,new RectangleF(Center?(Width-width)*0.5f:(Width - width), 0,width,Height));
		}
	}
}
