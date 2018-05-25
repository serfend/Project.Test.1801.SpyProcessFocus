using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Inst.UI.Bar
{
	class BtnAvatar:Control
	{
		private Image avatar;
		public BtnAvatar(Action<Control> CallBack):base(CallBack)
		{
			MovingSpeed = 0.05f;
			ForeColor = Color.Gray;
		}

		protected override void OnResize(EventArgs e)
		{
			this.Invalidate();
			base.OnResize(e);
		}

		protected override void OnForeColorChanged(EventArgs e)
		{
			foreBrush = new SolidBrush(ForeColor);
			base.OnForeColorChanged(e);
		}

		private Brush foreBrush;
		private Bitmap avatarCutting;
		public Image Avatar { get => avatar; set {
				avatar = value;
				int size = value.Width;
				if (value != null)
				{
					avatarCutting = new Bitmap(size, size);
					using (Graphics g = Graphics.FromImage(avatarCutting))
					{
						using (TextureBrush br = new TextureBrush(value, System.Drawing.Drawing2D.WrapMode.Clamp))
						{
							g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
							g.FillEllipse(br, new Rectangle(0, 0, size, size));
						}
					}
				}
				else avatarCutting = null;



			} }

		/// <summary>
		/// 在圈里画头像 置顶居中
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{

			var size = Width;
			var top = (int)((Height - size) * 0.5);
			//e.Graphics.DrawImage(avatar, new RectangleF(0, top, size, size));//以menu为界限
			if (avatarCutting == null)
			{
				e.Graphics.FillEllipse(foreBrush, 0, 0, Bounds.Width, Bounds.Width);
			}
			else
			{
				e.Graphics.DrawImage(avatarCutting, 0, 0, Bounds.Width, Bounds.Width);
			}
			
		}
	}
}
