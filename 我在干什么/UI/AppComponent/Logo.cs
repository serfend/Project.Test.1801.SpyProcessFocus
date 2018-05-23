using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 时间管理大师.UI.AppComponent
{
	class Logo : Control
	{
		public Icon Icon;
		public Logo(Action<Control> CallBack) : base(CallBack)
		{

		}

		public override bool RefreshLayout()
		{
			return false;
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			if(Icon == null)base.OnPaint(e);
			else
			{
				e.Graphics.DrawIcon(Icon,new Rectangle(0,0,Width,Height));
			}
		}
	}
}
