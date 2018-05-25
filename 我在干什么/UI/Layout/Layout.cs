using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Inst.UI.Layout
{
	public class Layout: Control
	{
		
		private double fps = 0.1;
		public double FPS { set {
				fps = value > 120 ? 120 : value;
			} get => fps;
		}
		public Layout()
		{
			////this.SetBounds(0, 0, 300, 300);
			//var t = new Thread(() =>
			//{
			//	while (true)
			//	{
			//		if (Program.Running)
			//		{
			//			if (base.RefreshLayout())
			//				this.Invalidate();
			//			Thread.Sleep((int)(10000 / FPS));
			//		}
			//		else
			//		{
			//			Thread.Sleep(20000);
			//		}
			//	}
			//})
			//{ IsBackground = true };
			////t.Start();
		}

	}
}
