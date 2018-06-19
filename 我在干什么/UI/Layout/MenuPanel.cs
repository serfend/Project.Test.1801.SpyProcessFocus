using DotNet4.Utilities.UtilInput;
using DotNet4.Utilities.UtilReg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Inst.Properties;
using Inst.Util;

namespace Inst.UI.Layout
{
	public class MenuPanel:Layout
	{
		public LeftLayout menu = new LeftLayout();

		public Bar.BtnCmd cmdDND;
		public Bar.BtnCmd cmdHider;
		public Bar.BtnCmd cmdPauser;
		//private Bar.BtnCmd cmdGraphic;
		public Bar.BtnCmd cmdFlashEnable;
		public Bar.BtnCmd cmdShowTomato;
		public Bar.BtnCmd cmdPauseTomato;
		public Bar.BtnCmd cmdAutoCurrentVersion;

		public Bar.BtnImage logo;

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
		private Button cmdUAC;
		public MenuPanel()
		{
			if (!Program.CheckAdminUAC())
			{
				cmdUAC = new Button
				{
					Location = new System.Drawing.Point(0, 300),
					Size = new System.Drawing.Size(123, 24),
					TabIndex = 0,
					Text = "权限未开启",
					UseVisualStyleBackColor = true,
					FlatStyle = FlatStyle.System,
					Parent = this,
				};
				cmdUAC.Click += (x, xx) => { Program.RunAsAdministrator(); };
				Program.SetControlUACFlag(cmdUAC);

			}
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
			cmdDND = new Bar.BtnCmd((x) => { Program.OnDND = !cmdDND.StateIsON; }) {
				Text = "开启免打扰|关闭免打扰",
				ImageActive = Resources.DNDon,
				ImageDeactive = Resources.DNDoff,
				Font=this.Font,
				Parent=this,
				StateIsON=!Program.OnDND
			};
			cmdHider = new Bar.BtnCmd((x) => {
				Program.frmMain.ui.ExpandMenu(false);
				Program.frmMain.ui.clock.Visible = false;
				Program.frmMain.ui.clock.隐藏();
				Program.frmMain.ui.menuPanel.cmdShowTomato.StateIsON = true;
				更新饼图();

		})
			{
				Text = "显示图表|隐藏图表",
				ImageDeactive = Resources.隐藏饼图,
				ImageActive=Resources.显示饼图,
				Font = this.Font,
				//ForeColor=Color.White,
				Parent = this
			};
			cmdPauser = new Bar.BtnCmd((x) => {
				Program.frmMain.BtnRunningCommand_Click();
			})
			{
				Text = "继续统计|暂停统计",
				ImageActive = Resources.开始统计,
				ImageDeactive=Resources.停止统计,
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
				ImageActive = Resources.打开动画,
				ImageDeactive=Resources.关闭动画,
				Font = this.Font,
				//ForeColor = Color.White,
				Parent = this,
				StateIsON=!Program.UsedFlash
			};
			cmdShowTomato = new Bar.BtnCmd((x) => {
				Program.frmMain.ui.HideAll(!cmdShowTomato.StateIsON);
				this.OnResize(EventArgs.Empty);
			})
			{
				Text = "打开番茄|关闭番茄",
				ImageActive = Resources.番茄,
				ImageDeactive=null,
				Font = this.Font,
				Parent = this,
				StateIsON = true
			};
			cmdPauseTomato = new Bar.BtnCmd((x) => {
				Program.frmMain.ui.clock.Pause =!Program.frmMain.ui.clock.Pause;
			})
			{
				Parent = this,
				Text = "暂停番茄|继续番茄",
				ImageActive = Resources.番茄暂停,
				ImageDeactive=null,
				StateIsON=true,
				Font=this.Font
			};
			cmdAutoCurrentVersion = new Bar.BtnCmd((x) => {
				if (!Program.CheckAdminUAC()) Program.RunAsAdministrator();
				Program.AutoCurrentVersion = !cmdAutoCurrentVersion.StateIsON;
			})
			{
				Text = "开机启动|取消启动",
				ImageActive = Resources.开机启动,
				ImageDeactive = Resources.停止开机启动,
				Font = this.Font,
				//ForeColor = Color.White,
				Parent = this,
				StateIsON = !Program.AutoCurrentVersion
			};
			
			SelectQueryDay = new Bar.BtnCmd[4];
			SelectQueryDay[0] = new Bar.BtnCmd((x) => {
				Program.QueryingDay = DataCore.DayStamp(DateTime.Now).ToString();
				SwitchButton(0);
			}) {Parent=this,Font=this.Font,Text="今天", ImageActive = Resources.单天 };
			SelectQueryDay[1] = new Bar.BtnCmd((x) => {
				Program.QueryingDay = (DataCore.DayStamp(DateTime.Now)-1).ToString();
				SwitchButton(1);
			}) { Parent = this, Font = this.Font, Text = "昨天", ImageActive = Resources.昨天 };
			SelectQueryDay[2] = new Bar.BtnCmd((x) => {
				var date = InputBox.ShowInputBox("", "请按照格式输入日期", DateTime.Now.Date.ToShortDateString());
				try
				{
					Program.QueryingDay = DataCore.DayStamp(Convert.ToDateTime(date)).ToString();
					SwitchButton(2);
				}
				catch (Exception ex)
				{
					
				}
				
			}) { Parent = this, Font = this.Font, Text = "其他..", ImageActive = Resources.日期 };
			SelectQueryDay[3] = new Bar.BtnCmd((x) => {
				Program.QueryingDay = "SumDay";
				SwitchButton(3);
			}) { Parent = this, Font = this.Font, Text = "总计", ImageActive = Resources.总计 };

			logo = new Bar.BtnImage((x) => { }) {
				Parent = this,
				Image = Resources.Logo,
				BackColor = this.BackColor,Center=true
			};
			//菜单下方的控件将不显示左侧栏目
			menu.Parent = this;
			SwitchButton(3);
		}
		private int nowFocusIndex = 0;
		public void SwitchButton(int newIndex)
		{
			SelectQueryDay[nowFocusIndex].DeactiveColor  = Color.FromArgb(255, 50, 50, 50);
			SelectQueryDay[nowFocusIndex].ActiveColor = Color.FromArgb(255, 124, 165, 199);
			nowFocusIndex = newIndex;
			SelectQueryDay[nowFocusIndex].DeactiveColor = Program.NowDateIsValid ? Color.FromArgb(255, 124, 165, 199):Color.FromArgb(255,139,139,139);
			SelectQueryDay[nowFocusIndex].ActiveColor = Color.FromArgb(255, 124, 165, 199);

		}

		public Bar.BtnCmd[] SelectQueryDay;
		protected override void OnResize(EventArgs e)
		{
			menu.DBounds = new System.Drawing.Rectangle(0,0,(int)(Parent.Width * 0.03),(int)(Parent.Height));
			float spaceForTomatoPos=cmdShowTomato.StateIsON?0: 0.05f;
			if (Program.frmMain.ui.menuPanel.menu.expandMenu)
			{
				logo.SetLayoutPos(0, 0.05f, 1, 0.2f);
				if (cmdUAC != null)
				{
					cmdUAC.Visible = true;
					cmdUAC.Left = (int)((Width - cmdUAC.Width));
					cmdUAC.Top = (int)(Height - cmdUAC.Height);
				}
			}
			else
			{
				if (cmdUAC != null)
				{
					cmdUAC.Visible = false;
				}
				logo.SetLayoutPos(1f, 0.05f, 1, 0.2f);
			}
			cmdDND.SetLayoutPos(0, 0.1f, 1, 0.052f);
			cmdHider.SetLayoutPos(0, 0.15f, 1, 0.052f);
			cmdPauser.SetLayoutPos(0, 0.2f, 1, 0.052f);
			//cmdGraphic.SetLayoutPos(0, 0.302f, 1, 0.052f);
			cmdFlashEnable.SetLayoutPos(0, 0.25f, 1, 0.052f);
			cmdShowTomato.SetLayoutPos(0, 0.3f, 1, 0.052f);
			cmdPauseTomato.SetLayoutPos(cmdShowTomato.StateIsON ? -1 : 0, 0.35f, 1, 0.052f);
			cmdAutoCurrentVersion.SetLayoutPos(0, 0.35f+spaceForTomatoPos, 1, 0.052f);


			for (int i = 0; i < 4; i++)
			{
				if (SelectQueryDay[i] != null)
					SelectQueryDay[i].SetLayoutPos(0, 0.43f + 0.05f * i + spaceForTomatoPos, 1, 0.052f);
			}
			//avatarNowSize = avatarNowSize * 0.8f + (menu.expandMenu ? 0.1f : 0.2f);

			//if (menu.expandMenu )7
			//{
			//	for(int i=0;i<3;i++)
			//		ShowUserInfoOfDay[i].SetLayoutPos(0.51f, 0.05f*i+0.08f, 0.5f, 0.05f);
			//}
			//base.OnResize(e);
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.DrawString("硬时头 Inst\n版本:1.0", Font, Brushes.White, Program.frmMain.Width*0.03f, Height*0.9f);
			//base.OnPaint(e);
		}
	}
}
