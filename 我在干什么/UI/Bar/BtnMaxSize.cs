﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Inst.UI.Bar
{
	class BtnMaxSize:Control
	{
		public BtnMaxSize(Action<Control> CallBack) : base(CallBack)
		{
			MovingSpeed = 0.05f;
			ForeColor = Color.PaleVioletRed;
		}
		public override bool RefreshLayout()
		{
			float lastAngle = nowAngle;
			nowAngle = nowAngle * (1 - MovingSpeed) + (active ? ActiveAngle : deActiveAngle) * MovingSpeed;
			nowLen = nowLen * (1 - MovingSpeed) + (active ? ActiveLen : deActiveLen) * MovingSpeed;
			if (Math.Abs(lastAngle - nowAngle) > 0.1)
			{
				if (!Program.UsedFlash)
				{
					nowAngle = active ? ActiveAngle : deActiveAngle;
					nowLen = active ? ActiveLen : deActiveLen;
				}
				this.Invalidate();
				return true;
			}
			return false;
		}
		private bool active = false;
		protected override void OnMouseEnter(EventArgs e)
		{
			active = true;
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			active = false;
		}
		protected override void OnForeColorChanged(EventArgs e)
		{
			pen = new Pen(ForeColor, 6);

			base.OnForeColorChanged(e);
		}
		private Pen pen;

		private float nowAngle = 0;//0-90度的叉叉
		private float nowLen = 0.5f;
		private float deActiveLen = 0.5f;
		private float ActiveLen = 0.8f;
		private float deActiveAngle = 45;
		private float ActiveAngle = 135 + 180;
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
		}
	}
}
