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
		public TopLayout top = new TopLayout();
		public UpLayout up=new UpLayout();
		public CenterLayout center = new CenterLayout();
		public MenuPanel menuPanel = new MenuPanel();
		public TitleLayout title = new TitleLayout();
		public Form frm;
		public void ExpandMenu(bool expand)
		{
			float x = expand ? 0.2f*frm.Width : 0;
			float y = expand ? 0.1f*frm.Height : 0;
			title.Offset(x, 0, 0, 0);
			top.Offset(x, 0, 0, 0);
			up.Offset(x, 0, 0, 0);
			center.Offset(x, 0, 0, 0);
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
			top.Parent = frm;
			var font = new Font("微软雅黑", 10);
			top.Add(new Bar.BtnNormal(Program.ShowDataAnalysis()) { Text = "今日", Font = font });
			top.Add(new Bar.BtnNormal(Program.ShowDataAnalysis()) { Text = "本周", Font = font });
			top.Add(new Bar.BtnNormal(Program.ShowDataAnalysis()) { Text = "上周", Font = font });
			top.Add(new Bar.BtnNormal(Program.ShowDataAnalysis()) { Text = "本月", Font = font });
			top.Add(new Bar.BtnNormal(Program.ShowDataAnalysis()) { Text = "上月", Font = font });
			top.Add(new Bar.BtnNormal(Program.ShowDataAnalysis()) { Text = "本季度", Font = font });
			top.Add(new Bar.BtnNormal(Program.ShowDataAnalysis()) { Text = "上季度", Font = font });
			top.Add(new Bar.BtnNormal(Program.ShowDataAnalysis()) { Text = "年度", Font = font });

			up.Parent = frm;
			center.Parent = frm;
			menuPanel.Parent = frm;
			title.Parent = frm;
			frm.Resize += Frm_Resize;
		}

		private void Frm_Resize(object sender, EventArgs e)
		{
			bar.SetLayoutPos(0.85f, 0, 0.15f, 0.03f);
			top.SetLayoutPos(0.46f, 0.03f, 0.54f, 0.02f);
			up.SetLayoutPos(0.05f, 0.05f, 0.95f, 0.3f);
			center.SetLayoutPos(0.05f, 0.31f, 0.95f, 0.69f);
			menuPanel.SetLayoutPos(0, 0, 0.05f, 1);
			title.SetLayoutPos(0.05f, 0, 0.4f, 0.05f);
		}

		internal void RefreshData(ProcessGroup process)
		{
			up.饼图.RefreshData(process);
		}
	}
}
