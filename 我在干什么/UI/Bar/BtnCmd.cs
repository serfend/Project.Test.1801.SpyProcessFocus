using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Time时间记录器.UI.Bar
{
	class BtnCmd:Control
	{
		private Color deactiveColor= Color.FromArgb(255, 189, 215, 238);
		private Color activeColor = Color.FromArgb(255, 89, 255, 138);
		public BtnCmd(Action<Control> CallBack):base(CallBack)
		{
			MovingSpeed = 0.05f;
			ForeColor = Color.Black;
			BackColor = deactiveColor;
		}
		public Image Image;
		private bool expand = false;
		private string[] StateText;
		private string showText="";
		public bool StateIsON=false;
		protected override void OnCallBacking()
		{
			StateIsON = !StateIsON;
			base.OnCallBacking();
			this.Invalidate();
		}
		protected override void OnTextChanged(EventArgs e)
		{
			StateText = Text.Split('|');
			if (StateText.Length < 2) StateText = new string[2] { Text, Text };
			base.OnTextChanged(e);
		}
		public void Expand(bool expand=false)
		{
			this.expand = expand;
		}
		public override bool RefreshLayout()
		{
			base.RefreshLayout();
			float lastSize = nowAngle;
			nowAngle = nowAngle * (1 - MovingSpeed) + targetAngle * MovingSpeed;
			if (Math.Abs(lastSize - nowAngle) > 0.005)
			{
				if (!Program.UsedFlash) nowAngle = targetAngle;
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
			foreBrush = new SolidBrush(ForeColor);
			base.OnForeColorChanged(e);
		}
		protected override void OnBackColorChanged(EventArgs e)
		{
			bckBrush = new SolidBrush(BackColor);
			base.OnBackColorChanged(e);
		}
		private Brush foreBrush;
		private Brush bckBrush;
		float nowAngle = 1;//可拓展的图标 
		float targetAngle = 0;
		protected override void OnPaint(PaintEventArgs e)
		{
			if (Image == null) return;//防止遇到智障

			//TODO 不应用固定数值 ，此处需要优化
			var size = Parent.Controls[4].Width;

			var top = (int)((Height - size) * 0.5);
			e.Graphics.DrawImage(Image,new RectangleF(0,top,size,size));//以menu为界限
			showText = StateIsON ? StateText[0] : StateText[1];
			var strTop = (int)((Height - e.Graphics.MeasureString(showText, Font).Height)*0.5);
			if (expand) {
				e.Graphics.DrawString(showText, Font, foreBrush, size * 1.2f+ nowAngle*Width*0.2f, strTop);
			}
			base.OnPaint(e);
		}
	}
}
