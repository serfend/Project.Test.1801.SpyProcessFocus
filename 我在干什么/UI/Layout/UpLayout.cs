﻿using DotNet4.Utilities.UtilReg;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Windows.Forms;
using Time时间记录器.UI;
namespace Time时间记录器.UI.Layout
{
	public class UpLayout:Layout
	{
		
		public 时间分布饼图 饼图=new 时间分布饼图();
		public UpLayout()
		{
			BackColor = 饼图.BackColor = System.Drawing.Color.FromArgb(255, 195, 195, 195);
			饼图.Parent = this;
			
			饼图.Font = new System.Drawing.Font("微软雅黑", 10);

		}
		
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
		}

		protected override void OnResize(EventArgs e)
		{
			饼图.SetLayoutPos(0.01f,0,0.8f,1f);

			base.OnResize(e);
		}
	}
}
