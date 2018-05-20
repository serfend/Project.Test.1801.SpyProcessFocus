using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Time时间记录器.UI;
namespace Time时间记录器.UI.Layout
{
	public class UpLayout:Layout
	{
		public 时间分布饼图 饼图=new 时间分布饼图();
		public UpLayout()
		{
			饼图.Parent = this;
			饼图.Font = new System.Drawing.Font("微软雅黑", 12);
		}



		protected override void OnResize(EventArgs e)
		{
			饼图.SetLayoutPos(0.1f,0,0,1f);
			base.OnResize(e);
		}
	}
}
