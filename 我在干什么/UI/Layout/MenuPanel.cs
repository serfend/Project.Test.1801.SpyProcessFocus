using DotNet4.Utilities.UtilReg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Time时间记录器.Properties;

namespace Time时间记录器.UI.Layout
{
	public class MenuPanel:Layout
	{
		public LeftLayout menu = new LeftLayout();

		public Bar.BtnCmd cmdHider;
		private Bar.BtnCmd cmdPauser;
		private Bar.BtnCmd cmdGraphic;
		private Bar.BtnCmd cmdFlashEnable;
		public void 更新饼图()
		{
			if (cmdHider.StateIsON)
			{
				Program.frmMain.ui.up.饼图.隐藏();
				var top = (float)(Program.frmMain.Height * 0.3);
				Program.frmMain.ui.up.Offset(0, 0, 0, -top);
				Program.frmMain.ui.center.Offset(0, -top, 0, top);
			}
			else
			{
				Program.frmMain.ui.up.饼图.显示();
				Program.frmMain.ui.up.Offset(0, 0, 0, 0);
				Program.frmMain.ui.center.Offset(0, 0, 0, 0);
			}
		}
		public MenuPanel()
		{
			this.BackColor = System.Drawing.Color.FromArgb(255, 50, 50, 50);
			Font = new System.Drawing.Font("微软雅黑", 12);
			
			//ShowUserInfoOfDay = new Bar.BtnNormal[3];
			//for (int i = 0; i < 3; i++)
			//{
			//	ShowUserInfoOfDay[i] = new Bar.BtnNormal((x) => { })
			//	{
			//		Text = "喵喵喵",
			//		Font = this.Font,
			//		Parent = this,
			//		deactiveColor = System.Drawing.Color.FromArgb(255, 91, 155, 213)
			//	};
			//}
			cmdHider = new Bar.BtnCmd((x) => {
				Program.frmMain.ui.ExpandMenu(false);
				更新饼图();

		})
			{
				Text = "显示图表|隐藏图表",
				Image = Resources.隐藏,
				Font = this.Font,
				Parent = this
			};
			cmdPauser = new Bar.BtnCmd((x) => {
				Program.frmMain.BtnRunningCommand_Click();
			})
			{
				Text = "继续统计|暂停统计",
				Image = Resources.暂停,
				Font = this.Font,
				Parent = this
			};
			cmdGraphic = new Bar.BtnCmd((x) => { })
			{
				Text = "隐藏分析|分析交集",
				Image = Resources.图表,
				Font=this.Font,
				Parent = this
			};
			cmdFlashEnable = new Bar.BtnCmd((x) => {
				Program.UsedFlash = !cmdFlashEnable.StateIsON;
			})
			{
				Text = "启用动画|关闭动画",
				Image = Resources.图表,
				Font = this.Font,
				Parent = this,
				StateIsON=!Program.UsedFlash
			};
			menu.Parent = this;
		}
		protected override void OnResize(EventArgs e)
		{
			menu.DBounds = new System.Drawing.Rectangle(0,0,(int)(Parent.Width * 0.03),(int)(Parent.Height));
			cmdHider.SetLayoutPos(0, 0.2f, 1, 0.05f);
			cmdPauser.SetLayoutPos(0, 0.251f, 1, 0.05f);
			cmdGraphic.SetLayoutPos(0, 0.302f, 1, 0.05f);
			cmdFlashEnable.SetLayoutPos(0, 0.353f, 1, 0.05f);
			//avatarNowSize = avatarNowSize * 0.8f + (menu.expandMenu ? 0.1f : 0.2f);

			//if (menu.expandMenu )
			//{
			//	for(int i=0;i<3;i++)
			//		ShowUserInfoOfDay[i].SetLayoutPos(0.51f, 0.05f*i+0.08f, 0.5f, 0.05f);
			//}
			base.OnResize(e);
		}
		//private float avatarNowSize = 1;
	}
}
