using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 时间管理大师.UI.Layout
{

	public class TitleLayout:Layout
	{
		private Bar.BtnTitle title = new Bar.BtnTitle((x)=> {
			MessageBox.Show("23333");
		});
		public TitleLayout()
		{
			title.Parent = this;
		}
		protected override void OnResize(EventArgs e)
		{
			title.SetLayoutPos(0, 0, 1, 1);
		}
	}
}
