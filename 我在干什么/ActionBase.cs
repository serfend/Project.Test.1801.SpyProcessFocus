using Inst.Util.Output;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Inst
{
	public static class ActionBase
	{
		public static Action ShowTomato { get => () =>
		{
			Program.frmMain.Invoke((EventHandler)delegate
			{
				if (!Program.Running)
				{
					Program.frmMain.InfoShow_DoubleClick(new object(), EventArgs.Empty);
				}
				if (!Program.frmMain.ui.clock.Visible)
				{
					Program.frmMain.ui.HideAll(true);
					Program.frmMain.ui.menuPanel.Height++;
				}
			});

		};
		} 
		public static Action ExitInst { get => () =>
		{
			var opts = new Opt[2];
			opts[1].infos = "取消";
			opts[0].infos = "退出";
			opts[0].CmdInfo = "ExitInst";
			opts[1].UColor = Color.Gray;
			opts[0].UColor = Color.DarkRed;
			opts[0].DColor = Color.Red;
			new OptShow().ShowOpt(opts,"Inst正在被关闭", "确定要退出吗?");
		};
		}
	}
}
