using DotNet4.Utilities.UtilInput;
using DotNet4.Utilities.UtilReg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inst.UI.Layout
{
	public class TomatoClock:Control
	{
		private bool pause = false;
		private int restTime=0x3fff;
		private int workTime= 0x3fff;
		public float targetNowTime=0,nowTime=0;
		private bool IsStageOne=true;
		private Brush brushNow,brushPause, brushTotal;
		private Color bckColor;
		public float brushWidth = 0.05f;
		private Reg TomatoSetting;
		public override bool RefreshLayout()
		{
			brushPause = new SolidBrush(Color.FromArgb(255,255,255,0));
			TomatoSetting =Program.AppSetting.In("Setting").In("Tomato");
			restTime = Convert.ToInt32(TomatoSetting.GetInfo("restTime", "300"));
			workTime = Convert.ToInt32(TomatoSetting.GetInfo("workTime", "1500"));
			nowTime = nowTime * 0.95f + targetNowTime * 0.05f;
			if(Math.Abs(nowTime - targetNowTime)>0.1)this.Invalidate();
			return base.RefreshLayout();
		}
		public TomatoClock(Action<Control> CallBack) : base(CallBack)
		{
			nowRoundIndex = 1;
			lastBeginTime = Environment.TickCount;
			MovingSpeed = 0.3f;
			BgColor = Color.FromArgb(100,155,155,155);
			ForeColor = Color.FromArgb(255, 155, 225, 155);
			var t = new Thread((x) =>
			{
				while (true)
				{
					if (pause)
					{
						Thread.Sleep(200);
					}else
					if (RefreshData())
						Thread.Sleep(500);
					else Thread.Sleep(50);
				}
			})
			{ IsBackground=true};
			t.Start();
		}
		protected override void OnCallBacking()
		{
			Pause = !Pause;
			if(Pause)
			if (MessageBox.Show(Program.frmMain, "重新设置番茄","重置确认",MessageBoxButtons.OKCancel,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2) ==DialogResult.OK) {

					try
					{
						RestTime = Convert.ToInt32(InputBox.ShowInputBox("重置", "休息时间", RestTime.ToString()));
						WorkTime = Convert.ToInt32(InputBox.ShowInputBox("重置", "工作时间", WorkTime.ToString()));
						pauseTimeLeft = targetNowTime = (float)Convert.ToDouble(InputBox.ShowInputBox("重置", "当前时间", targetNowTime.ToString()));
						nowRoundIndex = 1;
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
					}
				Pause = true;
			}
			
			base.OnCallBacking();
		}
		public Color BgColor
		{
			set {
				bckColor = value;
				brushTotal = new SolidBrush(bckColor);
			}
			get => bckColor;
		}

		public int RestTime { get => restTime; set  {
				restTime = value;
				TomatoSetting.SetInfo("restTime",value);
			} }
		public int WorkTime { get => workTime; set
			{
				workTime = value;
				TomatoSetting.SetInfo("workTime", value);
			} }

		public bool Pause { get => pause; set {
				if (pause)
				{
					lastBeginTime = Environment.TickCount + (int)((pauseTimeLeft-(IsStageOne?workTime:restTime)) *1000);
					pause = false;
				}
				else
				{
					pauseTimeLeft = targetNowTime;
					pause = true;
				}
				this.Invalidate();
			} }

		public override void 隐藏()
		{
			ShowOut = false;
			this.ModifyState(false);
		}
		public override void 显示()
		{
			ShowOut = true;
			this.ModifyState(true);
		}


		private void ModifyState(bool show)
		{
			var t = new Task(() =>
			{
				float targetAngle = (show ? 360f : 0f);
				while (Math.Abs(nowMaxAngle - targetAngle) > 0.05f)
				{
					if (ShowOut != show) return;
					if (!Program.UsedFlash) nowMaxAngle = targetAngle;
					else
						nowMaxAngle = nowMaxAngle * (1 - MovingSpeed) + targetAngle * MovingSpeed;
					this.Invalidate();

					Thread.Sleep(50);
				}
				nowMaxAngle = targetAngle;
			});
			t.Start();
		}
		private float nowMaxAngle = 360f;//用于出现消失动画
		protected override void OnForeColorChanged(EventArgs e)
		{
			brushNow =new SolidBrush(ForeColor);
			base.OnForeColorChanged(e);
		}
		private float pauseTimeLeft;
		public bool RefreshData() {
			//数据刷新
			var d = (Environment.TickCount - lastBeginTime) / 1000f;
			targetNowTime = (IsStageOne ? WorkTime : RestTime)- d;
			if (targetNowTime <= 0)
			{
				IsStageOne = !IsStageOne;
				if (IsStageOne) nowRoundIndex++;
				Program.ShowNotice(10000,"时间到啦", GetDisplayInfo(),ToolTipIcon.Warning,()=> {
					if (!Program.Running)
					{
						Program.frmMain.InfoShow_DoubleClick(this,EventArgs.Empty);
					}
					if (!Program.frmMain.ui.clock.Visible)
					{
						Program.frmMain.ui.HideAll(true);
						Program.frmMain.ui.menuPanel.Height++;
					}
				});
				lastBeginTime =Environment.TickCount;
				targetNowTime = 0;
			}
			return targetNowTime > 10;
		}
		private int lastBeginTime = 0;
		private int nowRoundIndex;
		protected override void OnPaint(PaintEventArgs e)
		{
			var x = Width * brushWidth;
			var y = Height * brushWidth;
			var nowValueAngle = nowTime / (float)(IsStageOne?WorkTime: RestTime);
			nowValueAngle = nowValueAngle * nowMaxAngle;
			var nowAngleStart = nowMaxAngle ;
			GraphicsPath path = new GraphicsPath();
			path.AddPie(0, 0, Width, Width, -90, nowAngleStart);
			path.AddPie(x, y, Width - 2 * x, Height - 2 * y, -90, nowAngleStart);
			e.Graphics.FillPath(brushTotal, path);
			path = new GraphicsPath();
			path.AddPie(0, 0, Width , Height , -90, nowValueAngle);
			path.AddPie(x, y, Width-2*x, Height-2*y, -90, nowValueAngle);
			e.Graphics.FillPath(pause? brushPause:brushNow, path);
			var nowValue = nowTime * nowMaxAngle / 360;
			var strInfo = (nowValue > 10 ? Math.Ceiling(nowValue).ToString() : string.Format("{0:0.0}", nowValue)) + "/" + Math.Ceiling((IsStageOne? WorkTime:RestTime) * nowMaxAngle / 360);
			var strInfo2 = GetDisplayInfo();
			var strSize = e.Graphics.MeasureString(strInfo, Font);
			x = 0.5f * (Width - strSize.Width);
			y = 0.3f * (Height - strSize.Height);
			e.Graphics.DrawString(strInfo,Font,Brushes.Black,x,y);
			strSize = e.Graphics.MeasureString(strInfo2, Font);
			x = 0.5f * (Width - strSize.Width);
			y = 0.3f * (Height - strSize.Height);
			e.Graphics.DrawString(strInfo2, Font, Brushes.Black, x, y+strSize.Height);
		}
		private string GetDisplayInfo()=> "第" + nowRoundIndex + "轮 " + (IsStageOne ? "工作" : "休息");
	}
}
