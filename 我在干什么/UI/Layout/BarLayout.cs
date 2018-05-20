using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Time时间记录器.UI.Layout
{

	public class BarLayout : Layout
	{
		public Bar.BtnExit Exit = new Bar.BtnExit((x) => {
			Program.ExitProgram();

		});

		public Bar.BtnHide Hider = new Bar.BtnHide((x)=> {
			Program.HideProgram();
		});
		public BarLayout()
		{
			Exit.Parent = this;
			Hider.Parent = this;
		}
		protected override void OnResize(EventArgs e)
		{
			Exit.SetLayoutPos(0.8f, 0, 0, 1);
			Hider.SetLayoutPos(0, 0, 0.8f, 1);
			base.OnResize(e);
		}
	}
}
