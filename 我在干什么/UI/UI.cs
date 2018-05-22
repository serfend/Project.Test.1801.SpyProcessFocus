using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Time时间记录器.UI.Layout;
namespace Time时间记录器.UI
{
	public class UI
	{
		public BarLayout bar = new BarLayout();
		//public TopLayout top = new TopLayout();
		public BannerLayout banner = new BannerLayout();
		public UpLayout up=new UpLayout();
		public CenterLayout center = new CenterLayout();
		public MenuPanel menuPanel = new MenuPanel();
		//public TitleLayout title = new TitleLayout();
		public Form frm;
		public void ExpandMenu(bool expand)
		{
			Program.frmMain.ui.menuPanel.menu.expandMenu = expand;
			float x = expand ? 0.1f*frm.Width : 0;
			float y = expand ? 0.1f*frm.Height : 0;
			//title.Offset(x, 0, 0, 0);
			//top.Offset(x, 0, 0, 0);
			banner.Offset(x, 0, 0, 0);
			up.Offset(x, 0, 0, up.TargetOffset.Height);
			center.Offset(x, center.TargetOffset.Y, 0, center.TargetOffset.Height);
			menuPanel.Offset(0, 0, x, 0);
			foreach(var ctl in menuPanel.Controls)
			{
				if(ctl is Bar.BtnCmd cmd)
				{
					cmd.Expand(expand);
					cmd.Offset(0, y, 0, 0);
				}
			}
		}
		public UI( Form frm)
		{
			this.frm = frm;
			bar.Parent = frm;
			banner.Parent = frm;
			up.Parent = frm;
			center.Parent = frm;
			menuPanel.Parent = frm;
			//title.Parent = frm;
			frm.Resize += Frm_Resize;
		}

		private void Frm_Resize(object sender, EventArgs e)
		{
			menuPanel.更新饼图();
			ExpandMenu(Program.frmMain.ui.menuPanel.menu.expandMenu);
			bar.SetLayoutPos(0.85f, 0, 0.15f, 0.03f);
			//top.SetLayoutPos(0.46f, 0.03f, 0.54f, 0.02f);
			banner.SetLayoutPos(0.05f, 0.05f, 0.93f, 0.2f);
			up.SetLayoutPos(0.05f, 0.25f, 0.93f, 0.3f);
			center.SetLayoutPos(0.05f, 0.56f, 0.93f, 0.44f);
			menuPanel.SetLayoutPos(0, 0, 0.03f, 1);
			//title.SetLayoutPos(0.05f, 0, 0.4f, 0.05f);
		}

		internal void RefreshData(ProcessGroup process)
		{
			up.饼图.RefreshData(process);
			center.apps.RefreshData(process);
		}
	}
}
