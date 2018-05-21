namespace Time时间记录器
{
	partial class Form1
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.LstProcessRecorder = new System.Windows.Forms.ListView();
			this.ProcessName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.RemarkName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.LastFocus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.LastLostFocus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SumUsedTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.InfoShow = new System.Windows.Forms.NotifyIcon(this.components);
			this.SuspendLayout();
			// 
			// LstProcessRecorder
			// 
			this.LstProcessRecorder.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ProcessName,
            this.RemarkName,
            this.LastFocus,
            this.LastLostFocus,
            this.SumUsedTime});
			this.LstProcessRecorder.FullRowSelect = true;
			this.LstProcessRecorder.LabelWrap = false;
			this.LstProcessRecorder.Location = new System.Drawing.Point(566, 314);
			this.LstProcessRecorder.Name = "LstProcessRecorder";
			this.LstProcessRecorder.Size = new System.Drawing.Size(304, 238);
			this.LstProcessRecorder.TabIndex = 1;
			this.LstProcessRecorder.UseCompatibleStateImageBehavior = false;
			this.LstProcessRecorder.View = System.Windows.Forms.View.Details;
			this.LstProcessRecorder.SelectedIndexChanged += new System.EventHandler(this.LstProcessRecorder_SelectedIndexChanged);
			// 
			// ProcessName
			// 
			this.ProcessName.Text = "名称";
			this.ProcessName.Width = 158;
			// 
			// RemarkName
			// 
			this.RemarkName.Text = "备注";
			this.RemarkName.Width = 120;
			// 
			// LastFocus
			// 
			this.LastFocus.Text = "开始时间";
			this.LastFocus.Width = 160;
			// 
			// LastLostFocus
			// 
			this.LastLostFocus.Text = "结束时间";
			this.LastLostFocus.Width = 160;
			// 
			// SumUsedTime
			// 
			this.SumUsedTime.Text = "累积时间";
			this.SumUsedTime.Width = 160;
			// 
			// InfoShow
			// 
			this.InfoShow.Text = "serfendInfoShow";
			this.InfoShow.Visible = true;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(901, 564);
			this.Controls.Add(this.LstProcessRecorder);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Form1";
			this.Text = "Time时间记录器";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.ListView LstProcessRecorder;
		private System.Windows.Forms.ColumnHeader ProcessName;
		private System.Windows.Forms.ColumnHeader LastFocus;
		private System.Windows.Forms.ColumnHeader LastLostFocus;
		private System.Windows.Forms.ColumnHeader SumUsedTime;
		private System.Windows.Forms.ColumnHeader RemarkName;

		public System.Windows.Forms.NotifyIcon InfoShow;

	}
}

