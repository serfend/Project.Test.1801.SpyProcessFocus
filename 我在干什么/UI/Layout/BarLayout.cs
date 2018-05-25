using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Inst.UI.Layout
{

	public class BarLayout : Layout
	{
		public Bar.BtnExit Exit = new Bar.BtnExit((x) => {
			Program.ExitProgram();

		});

		public Bar.BtnHide Hider = new Bar.BtnHide((x) =>
		{
			Program.HideProgram();
		})
		{ Text="隐藏"};
		//public Bar.BtnMaxSize MaxSize = new Bar.BtnMaxSize((x) => {
		//	Program.ExitProgram();

		//});
		public BarLayout()
		{
			Font = new System.Drawing.Font("微软雅黑", 8);
			Exit.Parent = this;
			//MaxSize.Parent = this;
			Hider.Parent = this;
			Hider.Font = this.Font;
		}
		protected override void OnResize(EventArgs e)
		{
			Exit.SetLayoutPos(0.8f, 0, 0, 1);
			Hider.SetLayoutPos(0.6f, 0, 0.2f, 1);
			//MaxSize.SetLayoutPos(0.45f, 0, 0.15f, 1);
			base.OnResize(e);
		}
	}
}
