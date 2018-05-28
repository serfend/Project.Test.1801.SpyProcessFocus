using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Inst.Properties;

namespace Inst.UI.Layout
{
	public class LeftLayout:Layout
	{
		public bool expandMenu = false;//是否展开菜单栏
		private Bar.BtnMenu BtnMenu;
		
		public Action<Control> DoExpandMenu() {
			return (x) => {
				expandMenu = !expandMenu;
				Program.frmMain.ui.clock.Visible = false;
				Program.frmMain.ui.clock.隐藏();
				Program.frmMain.ui.ExpandMenu(expandMenu);
				
				Program.frmMain.ui.menuPanel.cmdShowTomato.StateIsON = true;
			};
		}

		public LeftLayout()
		{
			BackColor = Color.FromArgb(255, 50, 50, 50);
			BtnMenu = new Bar.BtnMenu(DoExpandMenu())
			{
				Parent = this
			};
			
		}

		protected override void OnResize(EventArgs e)
		{
			BtnMenu.SetLayoutPos(0, 0, 1, 0);
		}
	}
}
