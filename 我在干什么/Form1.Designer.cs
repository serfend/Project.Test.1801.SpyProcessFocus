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
			this.BtnOutPutToExcel = new System.Windows.Forms.Button();
			this.LstProcessRecorder = new System.Windows.Forms.ListView();
			this.ProcessName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.RemarkName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.LastFocus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.LastLostFocus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SumUsedTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.BtnRunningCommand = new System.Windows.Forms.Button();
			this.BtnShowStatus = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// BtnOutPutToExcel
			// 
			this.BtnOutPutToExcel.Location = new System.Drawing.Point(771, 691);
			this.BtnOutPutToExcel.Name = "BtnOutPutToExcel";
			this.BtnOutPutToExcel.Size = new System.Drawing.Size(113, 30);
			this.BtnOutPutToExcel.TabIndex = 0;
			this.BtnOutPutToExcel.Text = "输出到Excel";
			this.BtnOutPutToExcel.UseVisualStyleBackColor = true;
			this.BtnOutPutToExcel.Click += new System.EventHandler(this.BtnOutPutToExcel_Click);
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
			this.LstProcessRecorder.Location = new System.Drawing.Point(30, 232);
			this.LstProcessRecorder.Name = "LstProcessRecorder";
			this.LstProcessRecorder.Size = new System.Drawing.Size(854, 453);
			this.LstProcessRecorder.TabIndex = 1;
			this.LstProcessRecorder.UseCompatibleStateImageBehavior = false;
			this.LstProcessRecorder.View = System.Windows.Forms.View.Details;
			this.LstProcessRecorder.SelectedIndexChanged += new System.EventHandler(this.LstProcessRecorder_SelectedIndexChanged);
			// 
			// ProcessName
			// 
			this.ProcessName.Text = "名称";
			this.ProcessName.Width = 240;
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
			// BtnRunningCommand
			// 
			this.BtnRunningCommand.Location = new System.Drawing.Point(652, 691);
			this.BtnRunningCommand.Name = "BtnRunningCommand";
			this.BtnRunningCommand.Size = new System.Drawing.Size(113, 30);
			this.BtnRunningCommand.TabIndex = 2;
			this.BtnRunningCommand.Text = "暂停";
			this.BtnRunningCommand.UseVisualStyleBackColor = true;
			this.BtnRunningCommand.Click += new System.EventHandler(this.BtnRunningCommand_Click);
			// 
			// BtnShowStatus
			// 
			this.BtnShowStatus.Location = new System.Drawing.Point(533, 691);
			this.BtnShowStatus.Name = "BtnShowStatus";
			this.BtnShowStatus.Size = new System.Drawing.Size(113, 30);
			this.BtnShowStatus.TabIndex = 3;
			this.BtnShowStatus.Text = "饼图";
			this.BtnShowStatus.UseVisualStyleBackColor = true;
			this.BtnShowStatus.Click += new System.EventHandler(this.BtnShowStatus_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(896, 733);
			this.Controls.Add(this.BtnShowStatus);
			this.Controls.Add(this.BtnRunningCommand);
			this.Controls.Add(this.LstProcessRecorder);
			this.Controls.Add(this.BtnOutPutToExcel);
			this.DoubleBuffered = true;
			this.Name = "Form1";
			this.Text = "Time时间记录器";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button BtnOutPutToExcel;
		private System.Windows.Forms.ListView LstProcessRecorder;
		private System.Windows.Forms.ColumnHeader ProcessName;
		private System.Windows.Forms.ColumnHeader LastFocus;
		private System.Windows.Forms.ColumnHeader LastLostFocus;
		private System.Windows.Forms.ColumnHeader SumUsedTime;
		private System.Windows.Forms.Button BtnRunningCommand;
		private System.Windows.Forms.ColumnHeader RemarkName;
		private System.Windows.Forms.Button BtnShowStatus;
	}
}

