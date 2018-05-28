using DotNet4.Utilities.UtilReg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Inst.UI.Layout
{
	public class BannerLayout:Layout
	{
		private Bar.BtnAvatar avatar;
		private Bar.BtnImage banner;
		public BannerLayout()
		{
			var userAvatarFile = Program.AppSetting.In("Setting").In("Avatar");
			var userAvatarFilePath = userAvatarFile.GetInfo("Default", null);
			var userBannerImageFilePath = userAvatarFile.GetInfo("Banner", null);
			avatar = new Bar.BtnAvatar((x) =>
			{
				var path = SetImageNew(userAvatarFile, "Default", "更换头像");
				avatar.Avatar = path == null ? Properties.Resources.defaultAvatar : new Bitmap(path);
				//TODO 可能可以显示下用户信息
			})
			{
				Avatar = userAvatarFilePath == null ? Properties.Resources.defaultAvatar : new Bitmap(userAvatarFilePath),
				Parent = this,
				BackColor = this.BackColor
			};
			banner = new Bar.BtnImage((x) => {
				var path= SetImageNew(userAvatarFile, "Banner");
				banner.Image = path == null ? GetRandomBanner (): new Bitmap(path);
			}) {
				Image = userBannerImageFilePath == null ? GetRandomBanner (): new Bitmap(userBannerImageFilePath),
				Parent = this,
				BackColor = this.BackColor
			};
		}
		private Image GetRandomBanner()
		{
			var bannerIndex = Program.AppSetting.In("Setting").In("Banner");
			var index = (Convert.ToInt32(bannerIndex.GetInfo("lastIndex", "1")) + 1)%3;
			bannerIndex.SetInfo("lastIndex", index);
			switch (index+1)
			{
				case 1:return Properties.Resources.banner1;
				case 2: return Properties.Resources.banner2;
				case 3: return Properties.Resources.banner3;
			}
			return null;
		}
		protected override void OnResize(EventArgs e)
		{
			avatar.SetLayoutPos(0, 0, 0, 1);
			banner.SetLayoutPos(0, 0, 1, 1);
			base.OnResize(e);
		}

		private string SetImageNew(Reg reg,string name,string title="更换图片")
		{
			using (OpenFileDialog ofd = new OpenFileDialog())

			{
				ofd.Title = title;
				ofd.Filter = "文件|*.*";
				ofd.CheckFileExists = true;
				ofd.CheckPathExists = true;
				ofd.Multiselect = false;
				reg.SetInfo(name, ofd.FileName);
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					return ofd.FileName;
				}
				else return null;
			}

		}
	}
}
