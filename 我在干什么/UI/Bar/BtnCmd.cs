using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Inst.UI.Bar
{
	public  class BtnCmd:Control
	{
		public bool AliasTextPos = true;//标签是否移位至图标右边
		private Color deactiveColor = Color.FromArgb(255, 50, 50, 50);
		private Color activeColor = Color.FromArgb(255, 124, 165, 199);
		public bool Center = false;
		public BtnCmd(Action<Control> CallBack):base(CallBack)
		{
			MovingSpeed = 0.2f;
			ForeColor = Color.FromArgb(255,240, 255, 240);
			BackColor = DeactiveColor;
		}
		public Image ImageActive,ImageDeactive;
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
					(int)(DeactiveColor.R * (1 - nowAngle) + ActiveColor.R * nowAngle),
					(int)(DeactiveColor.G * (1 - nowAngle) + ActiveColor.G * nowAngle),
					(int)(DeactiveColor.B * (1 - nowAngle) + ActiveColor.B * nowAngle));
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

		public Color DeactiveColor { get => deactiveColor; set { deactiveColor = value; nowAngle = 1-targetAngle; } }
		public Color ActiveColor { get => activeColor; set { activeColor = value; nowAngle = 1 - targetAngle; } }

		protected override void OnPaint(PaintEventArgs e)
		{
			var size = Parent.Controls[Parent.Controls.Count - 1].Width;

			var top = (int)((Height - size) * 0.5);
			var img = StateIsON ? ImageActive : ImageDeactive;
			if (img == null) img = StateIsON ? ImageDeactive : ImageActive;
			//TODO 启用默认图标
			if (img != null && (AliasTextPos || !expand)) e.Graphics.DrawImage(img, new RectangleF(0, top, size, size));//以menu为界限
			if (expand) {
				//TODO 不应用固定数值 ，此处需要优化
				
				showText = StateIsON ? StateText[0] : StateText[1];
				var strSize = e.Graphics.MeasureString(showText, Font);
				var strTop = (int)((Height - strSize.Height) * 0.5);
				e.Graphics.DrawString(showText, Font, foreBrush, (AliasTextPos ? size * 1.05f : (Center?(Width-strSize.Width)*0.5f:0) )+ nowAngle*Width*0.12f, strTop);
			}
			//base.OnPaint(e);
		}
	}
}
