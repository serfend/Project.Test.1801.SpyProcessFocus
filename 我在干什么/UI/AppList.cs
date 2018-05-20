using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Time时间记录器.UI
{
	class AppList:Control
	{
		public AppList()
		{
			
		}
		public void Add(App app)
		{
			this.Add(app);
		}
		public bool Remove(string name)
		{
			foreach(var appC in this.Controls)
			{
				var app = (App)appC;
				if (app.Name == name)
				{
					Controls.Remove(app);
					return true;
				}
			}
			return false;
		}
		public override bool RefreshLayout()
		{
			return base.RefreshLayout();
		}
	}
}
