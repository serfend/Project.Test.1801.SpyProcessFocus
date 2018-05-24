using DotNet4.Utilities.UtilInput;
using DotNet4.Utilities.UtilReg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using 时间管理大师.Properties;
using 时间管理大师.Util;

namespace 时间管理大师.UI.Layout
{
	public class MenuPanel:Layout
	{
		public LeftLayout menu = new LeftLayout();

		public Bar.BtnCmd cmdHider;
		private Bar.BtnCmd cmdPauser;
		//private Bar.BtnCmd cmdGraphic;
		private Bar.BtnCmd cmdFlashEnable;
		private Bar.BtnCmd cmdAutoCurrentVersion;

		public void 更新饼图()
		{
			if (cmdHider.StateIsON)
			{
				Program.frmMain.ui.up.饼图.隐藏();
				var top = (float)(Program.frmMain.Height * 0.5);
				Program.frmMain.ui.up.Offset(0, 0, 0, -top);
				Program.frmMain.ui.center.Offset(0, -top, 0, top);
				Program.frmMain.ui.banner.Offset(0, -top, 0, 0);
			}
			else
			{
				Program.frmMain.ui.up.饼图.显示();
				Program.frmMain.ui.up.Offset(0, 0, 0, 0);
				Program.frmMain.ui.center.Offset(0, 0, 0, 0);
				Program.frmMain.ui.banner.Offset(0, 0, 0, 0);
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
				//ForeColor=Color.White,
				Parent = this
			};
			cmdPauser = new Bar.BtnCmd((x) => {
				Program.frmMain.BtnRunningCommand_Click();
			})
			{
				Text = "继续统计|暂停统计",
				Image = Resources.暂停,
				Font = this.Font,
				//ForeColor = Color.White,
				Parent = this
			};
			//cmdGraphic = new Bar.BtnCmd((x) => { })
			//{
			//	Text = "隐藏分析|分析交集",
			//	Image = Resources.图表,
			//	Font=this.Font,
			//	//ForeColor = Color.White,
			//	Parent = this
			//};

			cmdFlashEnable = new Bar.BtnCmd((x) => {
				Program.UsedFlash = !cmdFlashEnable.StateIsON;
			})
			{
				Text = "启用动画|关闭动画",
				//Image = Resources.图表,
				Font = this.Font,
				//ForeColor = Color.White,
				Parent = this,
				StateIsON=!Program.UsedFlash
			};
			cmdAutoCurrentVersion = new Bar.BtnCmd((x) => {
				Program.AutoCurrentVersion = !cmdAutoCurrentVersion.StateIsON;
			})
			{
				Text = "开机启动|取消启动",
				//Image = Resources.图表,
				Font = this.Font,
				//ForeColor = Color.White,
				Parent = this,
				StateIsON = !Program.AutoCurrentVersion
			};
			AppAboutLabel = new Bar.BtnCmd((x) => { })
			{
				Parent = this,
				Font = this.Font,
				Text = "开发团队\n李培培教员\nxx队xxx\nxx队xxx\nxx队xxx|时间掌控大师\nTimeMaster\n版本:1.0",
				AliasTextPos=false
			};

			SelectQueryDay = new Bar.BtnCmd[4];
			SelectQueryDay[0] = new Bar.BtnCmd((x) => { Program.QueryingDay = DataCore.DayStamp(DateTime.Now).ToString(); }) {Parent=this,Font=this.Font,Text="今天" };
			SelectQueryDay[1] = new Bar.BtnCmd((x) => { Program.QueryingDay = (DataCore.DayStamp(DateTime.Now)-1).ToString(); }) { Parent = this, Font = this.Font, Text = "昨天" };
			SelectQueryDay[2] = new Bar.BtnCmd((x) => {
				var date = InputBox.ShowInputBox("", "请按照格式输入日期", DateTime.Now.Date.ToShortDateString());
				Program.QueryingDay = DataCore.DayStamp(Convert.ToDateTime(date)).ToString();
			}) { Parent = this, Font = this.Font, Text = "其他.." };
			SelectQueryDay[3] = new Bar.BtnCmd((x) => { Program.QueryingDay = "SumDay"; }) { Parent = this, Font = this.Font, Text = "总计" };
			//菜单下方的控件将不显示左侧栏目
			menu.Parent = this;
			
		}
		public Bar.BtnCmd AppAboutLabel;
		public Bar.BtnCmd[] SelectQueryDay;
		protected override void OnResize(EventArgs e)
		{
			menu.DBounds = new System.Drawing.Rectangle(0,0,(int)(Parent.Width * 0.03),(int)(Parent.Height));
			cmdHider.SetLayoutPos(0, 0.2f, 1, 0.05f);
			cmdPauser.SetLayoutPos(0, 0.251f, 1, 0.05f);
			//cmdGraphic.SetLayoutPos(0, 0.302f, 1, 0.05f);
			cmdFlashEnable.SetLayoutPos(0, 0.302f, 1, 0.05f);
			cmdAutoCurrentVersion.SetLayoutPos(0, 0.404f, 1, 0.05f);

			AppAboutLabel.SetLayoutPos(0, 0.07f, 1, 0.05f);
			
			for(int i=0;i<4;i++)
			{
				if (SelectQueryDay[i] != null)
					SelectQueryDay[i].SetLayoutPos(0, 0.48f + 0.052f * i, 1, 0.05f);
			}
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
