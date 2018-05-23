using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using 时间管理大师.Properties;

namespace 时间管理大师.UI.Layout
{
	public class LeftLayout:Layout
	{
		public bool expandMenu = false;//是否展开菜单栏
		private Bar.BtnMenu BtnMenu;
		
		public Action<Control> DoExpandMenu() {
			return (x) => {
				expandMenu = !expandMenu;
				Program.frmMain.ui.ExpandMenu(expandMenu);
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
