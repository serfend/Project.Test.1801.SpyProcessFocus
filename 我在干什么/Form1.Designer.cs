namespace 我在干什么
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
			this.LastFocus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SumUsedTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.LastLostFocus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// BtnOutPutToExcel
			// 
			this.BtnOutPutToExcel.Location = new System.Drawing.Point(423, 491);
			this.BtnOutPutToExcel.Name = "BtnOutPutToExcel";
			this.BtnOutPutToExcel.Size = new System.Drawing.Size(113, 30);
			this.BtnOutPutToExcel.TabIndex = 0;
			this.BtnOutPutToExcel.Text = "输出到Excel";
			this.BtnOutPutToExcel.UseVisualStyleBackColor = true;
			this.BtnOutPutToExcel.Click += new System.EventHandler(this.BtnOutPutToExcel_Click);
			// 
			// LstProcessRecorder
			// 
			this.LstProcessRecorder.AllowColumnReorder = true;
			this.LstProcessRecorder.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ProcessName,
            this.LastFocus,
            this.LastLostFocus,
            this.SumUsedTime});
			this.LstProcessRecorder.FullRowSelect = true;
			this.LstProcessRecorder.LabelWrap = false;
			this.LstProcessRecorder.Location = new System.Drawing.Point(12, 12);
			this.LstProcessRecorder.Name = "LstProcessRecorder";
			this.LstProcessRecorder.Size = new System.Drawing.Size(524, 473);
			this.LstProcessRecorder.TabIndex = 1;
			this.LstProcessRecorder.UseCompatibleStateImageBehavior = false;
			this.LstProcessRecorder.View = System.Windows.Forms.View.Details;
			// 
			// ProcessName
			// 
			this.ProcessName.Text = "名称";
			this.ProcessName.Width = 220;
			// 
			// LastFocus
			// 
			this.LastFocus.Text = "上次焦点";
			this.LastFocus.Width = 88;
			// 
			// SumUsedTime
			// 
			this.SumUsedTime.Text = "累积时间";
			this.SumUsedTime.Width = 123;
			// 
			// LastLostFocus
			// 
			this.LastLostFocus.Text = "上次失去焦点";
			this.LastLostFocus.Width = 87;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(543, 521);
			this.Controls.Add(this.LstProcessRecorder);
			this.Controls.Add(this.BtnOutPutToExcel);
			this.Name = "Form1";
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
	}
}

