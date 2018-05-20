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
		private Bar.BtnAvatar avatar;
		private Bar.BtnCmd cmdHider;
		private Bar.BtnCmd cmdPauser;
		private Bar.BtnCmd cmdGraphic;

		private Bar.BtnNormal[] ShowUserInfoOfDay;
		public MenuPanel()
		{
			this.BackColor = System.Drawing.Color.FromArgb(255, 50, 50, 50);
			Font = new System.Drawing.Font("微软雅黑", 18);
			var userAvatarFile = new Reg().In("Setting").In("Avatar");
			var userAvatarFilePath = userAvatarFile.GetInfo("Default", null);
			avatar = new Bar.BtnAvatar((x) =>
			{
				using (OpenFileDialog ofd = new OpenFileDialog())

				{
					ofd.Title = "更新头像";
					ofd.Filter = "JPG|*.jpg|BMP|*.bmp|Gif|*.gif|Png|*.png";
					ofd.CheckFileExists = true;
					ofd.CheckPathExists = true;
					ofd.Multiselect = false;
					if(ofd.ShowDialog() == DialogResult.OK)
					{
						avatar.Avatar = new Bitmap(ofd.FileName);
					}
					userAvatarFile.SetInfo("Default", ofd.FileName);
				}
				//TODO 可能可以显示下用户信息
			})
			{
				Avatar = userAvatarFilePath == null? Properties.Resources.defaultUserAvatar:new Bitmap(userAvatarFilePath),
				Parent=this,
				BackColor=this.BackColor
			};
			ShowUserInfoOfDay = new Bar.BtnNormal[3];
			for (int i = 0; i < 3; i++)
			{
				ShowUserInfoOfDay[i] = new Bar.BtnNormal((x) => { })
				{
					Text = "喵喵喵",
					Font = this.Font,
					Parent = this,
					deactiveColor = System.Drawing.Color.FromArgb(255, 91, 155, 213)
				};
			}
			cmdHider = new Bar.BtnCmd((x) => {
				if(cmdHider.StateIsON) Program.frmMain.ui.up.饼图.隐藏();else Program.frmMain.ui.up.饼图.显示();
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
			menu.Parent = this;
		}
		protected override void OnResize(EventArgs e)
		{
			menu.DBounds = new System.Drawing.Rectangle(0,0,(int)(Parent.Width * 0.05),(int)(Parent.Height));
			cmdHider.SetLayoutPos(0, 0.21f, 1, 0.1f);
			cmdPauser.SetLayoutPos(0, 0.32f, 1, 0.1f);
			cmdGraphic.SetLayoutPos(0, 0.43f, 1, 0.1f);

			avatarNowSize = avatarNowSize * 0.8f + (menu.expandMenu ? 0.1f : 0.2f);
			avatar.SetLayoutPos(0, 0.08f, avatarNowSize, 0);
			if (menu.expandMenu )
			{
				for(int i=0;i<3;i++)
					ShowUserInfoOfDay[i].SetLayoutPos(0.51f, 0.05f*i+0.08f, 0.5f, 0.05f);
			}
			base.OnResize(e);
		}
		private float avatarNowSize = 1;
	}
}
