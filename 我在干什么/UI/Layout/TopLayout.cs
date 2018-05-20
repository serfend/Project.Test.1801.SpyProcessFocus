using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Time时间记录器.UI.Layout
{
	public class TopLayout:Layout
	{
		private List<Bar.BtnNormal> btn=new List<Bar.BtnNormal>();
		public TopLayout()
		{

		}
		public Color deactiveColor = Color.FromArgb(255, 91, 155, 213);
		public Color activeColor = Color.FromArgb(255, 89, 255, 138);
		public void Add(Bar.BtnNormal btn)
		{
			btn.deactiveColor = deactiveColor;
			this.btn.Add(btn);
			btn.Parent = this;
			deactiveColor = Color.FromArgb(deactiveColor.A
				, (int)(deactiveColor.R * 0.95)
				, (int)(deactiveColor.G * 0.95)
				, (int)(deactiveColor.B * 0.95)
			);
		}
		protected override void OnResize(EventArgs e)
		{
			for(int i = 0; i < btn.Count; i++)
			{
				btn[i].SetLayoutPos(i * 0.08f, 0, 0.085f, 1);
			}
		}
	}
}
