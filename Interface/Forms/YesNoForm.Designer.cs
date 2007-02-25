namespace BuckRogers.Interface
{
	partial class YesNoForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.m_btnYes = new System.Windows.Forms.Button();
			this.m_btnNo = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// m_btnYes
			// 
			this.m_btnYes.Location = new System.Drawing.Point(12, 76);
			this.m_btnYes.Name = "m_btnYes";
			this.m_btnYes.Size = new System.Drawing.Size(75, 23);
			this.m_btnYes.TabIndex = 0;
			this.m_btnYes.Text = "Yes";
			this.m_btnYes.UseVisualStyleBackColor = true;
			this.m_btnYes.Click += new System.EventHandler(this.m_btnYes_Click);
			// 
			// m_btnNo
			// 
			this.m_btnNo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_btnNo.Location = new System.Drawing.Point(93, 76);
			this.m_btnNo.Name = "m_btnNo";
			this.m_btnNo.Size = new System.Drawing.Size(75, 23);
			this.m_btnNo.TabIndex = 1;
			this.m_btnNo.Text = "No";
			this.m_btnNo.UseVisualStyleBackColor = true;
			this.m_btnNo.Click += new System.EventHandler(this.m_btnNo_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(12, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(48, 48);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(66, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(143, 61);
			this.label1.TabIndex = 3;
			this.label1.Text = "label1";
			// 
			// YesNoForm
			// 
			this.AcceptButton = this.m_btnYes;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.CancelButton = this.m_btnNo;
			this.ClientSize = new System.Drawing.Size(221, 107);
			this.ControlBox = false;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.m_btnNo);
			this.Controls.Add(this.m_btnYes);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "YesNoForm";
			this.ShowInTaskbar = false;
			this.Text = "YesNoForm";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button m_btnYes;
		private System.Windows.Forms.Button m_btnNo;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label1;
	}
}