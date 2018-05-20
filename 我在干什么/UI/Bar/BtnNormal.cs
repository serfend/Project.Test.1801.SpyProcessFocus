using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Time时间记录器.UI.Bar
{
	public class BtnNormal : Control
	{
		public Color deactiveColor = Color.FromArgb(255, 189, 215, 238);
		public Color activeColor = Color.FromArgb(255, 89, 255, 138);
		public BtnNormal(Action<Control> CallBack) : base(CallBack)
		{
			MovingSpeed = 0.05f;
			ForeColor = Color.Black;
			BackColor = deactiveColor;
		}
		private bool expand = false;
		public void Expand(bool expand = false)
		{
			this.expand = expand;

		}
		public override bool RefreshLayout()
		{
			float lastSize = nowAngle;
			nowAngle = nowAngle * (1 - MovingSpeed) + targetAngle * MovingSpeed;
			if (Math.Abs(lastSize - nowAngle) > 0.005)
			{
				BackColor = Color.FromArgb(
					(int)(deactiveColor.R * (1 - nowAngle) + activeColor.R * nowAngle),
					(int)(deactiveColor.G * (1 - nowAngle) + activeColor.G * nowAngle),
					(int)(deactiveColor.B * (1 - nowAngle) + activeColor.B * nowAngle));
				this.Invalidate();
				return true;
			}
			return false;
		}
		protected override void OnMouseEnter(EventArgs e)
		{
			targetAngle = 1;
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			targetAngle = 0;
		}

		protected override void OnForeColorChanged(EventArgs e)
		{
			foreBrush = new SolidBrush(BackColor);
			base.OnForeColorChanged(e);
		}
		protected override void OnBackColorChanged(EventArgs e)
		{
			bckBrush = new SolidBrush(BackColor);
			base.OnBackColorChanged(e);
		}
		private Brush foreBrush;
		private Brush bckBrush;
		float nowAngle = 1;//可拓展的 带阴影的图标 
		float targetAngle = 0;
		protected override void OnPaint(PaintEventArgs e)
		{
			var strSize = e.Graphics.MeasureString(Text, Font);
			e.Graphics.DrawString(Text, Font, foreBrush,(Width - strSize.Width)*0.5f, (Height - strSize.Height) * 0.5f);

		}
	}
}
