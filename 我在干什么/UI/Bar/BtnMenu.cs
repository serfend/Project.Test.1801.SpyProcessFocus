using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Inst.UI.Bar
{
	class BtnMenu : Control
	{
		public BtnMenu(Action<Control> CallBack) : base(CallBack)
		{
			MovingSpeed = 0.3f;
			ForeColor = Color.White;
			BackColor = Color.FromArgb(255, 100, 100, 100);
		} 
		public override bool RefreshLayout()
		{
			//base.RefreshLayout();
			float lastSize = nowAngle;
			nowAngle = nowAngle * (1 - MovingSpeed) + targetAngle * MovingSpeed;
			
			if (Math.Abs(lastSize - nowAngle) > 0.01)
			{
				if (!Program.UsedFlash) nowAngle = targetAngle;
				this.Invalidate();
				return true;
			}
			return false;
		}
		protected override void OnMouseEnter(EventArgs e)
		{
			targetAngle += 90f;
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			targetAngle += 90f;
			targetAngle %= 3600;
		}

		protected override void OnForeColorChanged(EventArgs e)
		{
			foreBrush = new Pen(ForeColor,2);
			base.OnForeColorChanged(e);
		}
		protected override void OnBackColorChanged(EventArgs e)
		{
			bckBrush = new SolidBrush(BackColor);
			base.OnBackColorChanged(e);
		}
		private Pen foreBrush;
		private Brush bckBrush;
		float nowAngle = -270f;//画三根线 
		float targetAngle = 90f;
		protected override void OnPaint(PaintEventArgs e)
		{
			double beginAngle = (nowAngle - 45) / 180 * Math.PI;
			double endAngle = (nowAngle + 45) / 180 * Math.PI;
			if (foreBrush == null || bckBrush==null ) return;
			e.Graphics.DrawLine(foreBrush, (float)(Width * (0.5 + 0.4 * Math.Cos(beginAngle))), (float)(Height * (0.5 + 0.4 * Math.Sin(beginAngle))), (float)(Width * (0.5 + 0.4 * Math.Cos(endAngle))), (float)(Height * (0.5 + 0.4 * Math.Sin(endAngle))));
			e.Graphics.DrawLine(foreBrush, (float)(Width * (0.5 + 0.4 * Math.Cos(beginAngle))), (float)(Height * 0.5 ), (float)(Width * (0.5 + 0.4 * Math.Cos(endAngle))), (float)(Height * 0.5));
			e.Graphics.DrawLine(foreBrush, (float)(Width * (0.5 - 0.4 * Math.Cos(beginAngle))), (float)(Height * (0.5 - 0.4 * Math.Sin(beginAngle))), (float)(Width * (0.5 - 0.4 * Math.Cos(endAngle))), (float)(Height * (0.5 - 0.4 * Math.Sin(endAngle))));
		}
	}
}
