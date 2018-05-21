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
		private Color deactiveColor=Color.FromArgb(255,100,100,100), activeColor=Color.FromArgb(255,200,200,200);
		public BtnHide(Action<Control> CallBack =null):base(CallBack)
		{
			MovingSpeed = 0.05f;
			ForeColor = Color.Gray;
		}
		public override bool RefreshLayout()
		{
			float lastSize = nowSize;
			nowSize = nowSize * (1 - MovingSpeed) + targetSize * MovingSpeed;
			if (Math.Abs(lastSize - nowSize) > 0.001)
			{
				if (!Program.UsedFlash) nowSize = targetSize;
				BackColor = Color.FromArgb(255, (int)(deactiveColor.R * nowSize + activeColor.R * (1 - nowSize)), (int)(deactiveColor.G * nowSize + activeColor.G * (1 - nowSize)), (int)(deactiveColor.B * nowSize + activeColor.B * (1 - nowSize)));
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
			this.brush = new SolidBrush(ForeColor);
			base.OnForeColorChanged(e);
		}
		private Brush brush;
		float nowSize = 0f;//画一个方块 
		float targetSize = 1;
		protected override void OnPaint(PaintEventArgs e)
		{
			var strSize = e.Graphics.MeasureString(Text, Font);
			e.Graphics.DrawString(Text, Font, brush, (int)((Width - strSize.Width) * 0.5), (int)((Height - strSize.Height) * 0.5));
			base.OnPaint(e);
		}
		
	}
}
