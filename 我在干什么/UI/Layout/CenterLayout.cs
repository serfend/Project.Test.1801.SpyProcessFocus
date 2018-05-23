using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 时间管理大师.UI.Layout
{
	public class CenterLayout:Layout
	{
		public AppList apps = new AppList();
		public CenterLayout()
		{
			BackColor = System.Drawing.Color.Black;
			apps.Parent = this;
			
		}
		protected override void OnResize(EventArgs e)
		{
			apps.SetLayoutPos(0, 0, 1, 1);
		}

	}
}
