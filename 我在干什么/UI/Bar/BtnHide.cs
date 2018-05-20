using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Time时间记录器.UI.Bar
{
	public class BtnHide:Control
	{
		public BtnHide(Action<Control> CallBack =null):base(CallBack)
		{
			MovingSpeed = 0.05f;
			ForeColor = Color.LightGray;
		}
		public override bool RefreshLayout()
		{
			float lastSize = nowSize;
			nowSize = nowSize * (1 - MovingSpeed) + targetSize * MovingSpeed;
			if (Math.Abs(lastSize - nowSize) > 0.001)
			{
				this.Invalidate();
				return true;
			}
			return false;
		}
		protected override void OnMouseEnter(EventArgs e)
		{
			targetSize = 0;
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			targetSize = 1;
		}


		protected override void OnForeColorChanged(EventArgs e)
		{
			brush = new SolidBrush(ForeColor);
			base.OnForeColorChanged(e);
		}
		private Brush brush;
		float nowSize = 0f;//画一个方块 
		float targetSize = 1;
		protected override void OnPaint(PaintEventArgs e)
		{
			if (brush == null) return;
			e.Graphics.FillRectangle(brush,Width-nowSize*Width,0,Width*nowSize,Height);
		}
		
	}
}
