using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Time时间记录器.UI.Bar
{
	class BtnTitle : Control
	{
		public BtnTitle(Action<Control> CallBack):base(CallBack)
		{
			MovingSpeed = 0.05f;
			ForeColor = Color.Black;
			BackColor = Color.FromArgb( 91, 155, 213);
			Font = new Font("微软雅黑", 24);
		}
		public override bool RefreshLayout()
		{
			float lastSize = nowSize;
			nowSize = nowSize * (1 - MovingSpeed) + targetSize * MovingSpeed;
			if (Math.Abs(lastSize - nowSize) > 0.001)
			{
				if (!Program.UsedFlash) nowSize = targetSize;
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
			strBrush = new SolidBrush(ForeColor);
			base.OnForeColorChanged(e);
		}
		protected override void OnBackColorChanged(EventArgs e)
		{
			bckBrush = new SolidBrush(BackColor);
			base.OnBackColorChanged(e);
		}
		private Brush strBrush,bckBrush;
		float nowSize = 0f;//画一个方块 
		float targetSize = 1;
		protected override void OnPaint(PaintEventArgs e)
		{
			if (bckBrush == null || strBrush==null) return;
			e.Graphics.FillRectangle(bckBrush, 0, 0, Width , Height);
			var str = Program.Title;
			var strSize = e.Graphics.MeasureString(str, Font);
			e.Graphics.DrawString(str, Font, strBrush, Left+nowSize*Width*0.5f,(Height-strSize.Height)*0.5f);
		}
	}
}
