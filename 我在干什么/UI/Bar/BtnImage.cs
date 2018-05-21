using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Time时间记录器.UI.Bar
{
	class BtnImage:Control
	{
		public Image Image;
		public BtnImage(Action<Control> CallBack) : base(CallBack)
		{
			
		}

		public override bool RefreshLayout()
		{
			return false;
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if(Image==null)return;
			float width = Height*Image.  Width / Image.Height;
			e.Graphics.DrawImage(Image,new RectangleF(Width-width,0,width,Height));
		}
	}
}
