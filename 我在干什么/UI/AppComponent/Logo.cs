using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Inst.UI.AppComponent
{
	class Logo : Control
	{
		public Icon Icon;
		public string appPath;
		public Logo(Action<Control> CallBack) : base(CallBack)
		{

		}
		protected override void OnCallBacking()
		{
			if (appPath != null)
			{
				System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe")
				{
					Arguments = "/e,/select," + appPath
				};
				System.Diagnostics.Process.Start(psi);
			}
			base.OnCallBacking();
		}
		public override bool RefreshLayout()
		{
			return false;
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			if(Icon == null)base.OnPaint(e);
			else
			{
				e.Graphics.DrawIcon(Icon,new Rectangle(0,0,Width,Height));
			}
		}
	}
}
