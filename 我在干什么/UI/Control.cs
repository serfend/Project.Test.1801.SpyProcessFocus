using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace 时间管理大师.UI
{
	public class Control : System.Windows.Forms.Control
	{
		private Action<Control> CallBack;
		public Control(Action<Control> CallBack=null):this()
		{
			this.CallBack = CallBack;
			
		}
		public Control()
		{
			refreshBounds = new RefreshBounds(更新);
			this.DoubleBuffered = true;
			var t = new Thread(() =>
			{
				while (true)
				{
					if (Program.Running) this.RefreshLayout();
					Thread.Sleep(50);
					
				}
			})
			{ IsBackground = true };
			t.Start();
			this.MouseEnter += Control_MouseEnter; ;
			this.MouseLeave += Control_MouseLeave;
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			if(callbackWaitting)
			if (e.X > 0 && e.X < Width && e.Y > 0 && e.Y < Height)
			{
				this.OnCallBacking();
			}
			callbackWaitting = false;
		}
		private bool callbackWaitting = false;
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.X > 0 && e.X < Width && e.Y > 0 && e.Y < Height) callbackWaitting=true;
				base.OnMouseDown(e);
		}
		protected virtual void OnCallBacking()
		{
			CallBack?.Invoke(this);
		}
		private Rectangle dBounds;
		public Rectangle DBounds
		{
			get => dBounds;
			set
			{
				dBounds = value;
				//SetBounds((int)(DBounds.X - DBounds.Width * 0.1), (int)(DBounds.Y - DBounds.Height * 0.1), (int)(DBounds.Width * 1.2), (int)(DBounds.Height * 1.2));
				SetBounds((int)(DBounds.X + nowOffset.X),
						(int)(DBounds.Y+nowOffset.Y), 
						(int)(DBounds.Width+nowOffset.Width),
						(int)(DBounds.Height+nowOffset.Height));
				this.Invalidate();
			}
		}
		/// <summary>
		/// 以父控件的宽和高来计算子控件Pos
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		public virtual void SetLayoutPos(float x,float y,float w,float h) {
			if (Parent == null) return ;
			

			x = Parent.Width * x;
			y = Parent.Height * y;
			w = Parent.Width * w;
			h = Parent.Height * h;
			if (w == 0) w = h;
			if (h == 0) h = w;
			var ret = new Rectangle((int)x, (int)y, (int)w, (int)h);
			ShowPos = ret;
			DBounds = ret;
		}
		private RectangleF targetOffset = new RectangleF();
		private RectangleF nowOffset = new RectangleF();
		public void Offset(float x, float y, float w, float h)
		{
			TargetOffset = new RectangleF(x, y, w, h);
		}

			

		private void Control_MouseEnter(object sender, EventArgs e)
		{
			onFocus = true;
			this.Invalidate();
		}

		private void Control_MouseLeave(object sender, EventArgs e)
		{
			onFocus = false;
			this.Invalidate();
		}

		private bool onFocus = true;
		protected override void OnPaint(PaintEventArgs e)
		{
			//if (onFocus)
			//	e.Graphics.FillRectangle(Brushes.DeepSkyBlue, e.ClipRectangle);
		}
		protected override void OnResize(EventArgs e)
		{
			this.Invalidate();
			this.ShowPos = DBounds;
			base.OnResize(e);
		}
		
		private Rectangle showPos, hidePos;
		protected Rectangle ShowPos { get => showPos; set => showPos = value; }
		protected Rectangle HidePos { get => hidePos; set => hidePos = value; }

		protected enum MovingStyle
		{
			none,vertical,horizon,both
		}
		protected MovingStyle movingStyle=MovingStyle.both;
		public float MovingSpeed=0.2f;
		public virtual void 显示()
		{
			base.Show();
		}
		public virtual void 隐藏()
		{
			base.Hide();
		}
		

		private bool showOut = true;
		public bool ShowOut { get => showOut; set => showOut = value; }
		private delegate void RefreshBounds();
		RefreshBounds refreshBounds;
		protected virtual void 更新()
		{
			if (this.InvokeRequired)
			{
				this.Invoke(refreshBounds);
			}
			else
			{
				var newPos = showOut ? this.ShowPos : this.HidePos;
				DBounds = new Rectangle((int)(DBounds.X * MovingSpeed + newPos.X * (1 - MovingSpeed)),(int)(DBounds.Y * MovingSpeed + newPos.Y * (1 - MovingSpeed)), (int)(DBounds.Width * MovingSpeed + newPos.Width * (1 - MovingSpeed)), (int)(DBounds.Height * MovingSpeed + newPos.Height * (1 - MovingSpeed)));
				
			}
		}
		public virtual bool RefreshLayout()
		{
			float offsetMoving = Math.Abs(nowOffset.X - TargetOffset.X) +
				Math.Abs(nowOffset.Y - TargetOffset.Y) +
				Math.Abs(nowOffset.Width - TargetOffset.Width) +
				Math.Abs(nowOffset.Height - TargetOffset.Height);
			if (offsetMoving > 0.1)
			{
				if (!Program.UsedFlash) nowOffset = TargetOffset;
				nowOffset.X = nowOffset.X * (1 - MovingSpeed) + TargetOffset.X * MovingSpeed;
				nowOffset.Y = nowOffset.Y * (1 - MovingSpeed) + TargetOffset.Y * MovingSpeed;
				nowOffset.Width = nowOffset.Width * (1 - MovingSpeed) + TargetOffset.Width * MovingSpeed;
				nowOffset.Height = nowOffset.Height * (1 - MovingSpeed) + TargetOffset.Height * MovingSpeed;
				更新();
			}

			bool anyRefresh = false;
			for(int i=0;i< Controls.Count;i++)
			{
				var ctl = Controls[i];
				if (ctl is Control nCtl)
				{
					anyRefresh |= nCtl.RefreshLayout();
				}
			}
			return anyRefresh;
		}


		#region 搞事情
			private Color bckColor=Color.White;
			public override Color BackColor { get => bckColor; set
			{
				bckColor = value;
				this.OnBackColorChanged(EventArgs.Empty);
			} }

		public RectangleF TargetOffset { get => targetOffset; set => targetOffset = value; }
		#endregion
	}
}
