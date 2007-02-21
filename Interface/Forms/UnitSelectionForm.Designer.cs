namespace BuckRogers.Interface
{
	partial class UnitSelectionForm
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.m_labUnitsLeft = new System.Windows.Forms.Label();
			this.m_nudFighters = new System.Windows.Forms.NumericUpDown();
			this.m_nudTransports = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.m_nudTroopers = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.m_nudGennies = new System.Windows.Forms.NumericUpDown();
			this.label6 = new System.Windows.Forms.Label();
			this.m_btnOK = new System.Windows.Forms.Button();
			this.m_btnCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.m_nudFighters)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_nudTransports)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_nudTroopers)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_nudGennies)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 54);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(71, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Fighters:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(12, 9);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(123, 20);
			this.label2.TabIndex = 1;
			this.label2.Text = "Units remaining:";
			// 
			// m_labUnitsLeft
			// 
			this.m_labUnitsLeft.AutoSize = true;
			this.m_labUnitsLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_labUnitsLeft.Location = new System.Drawing.Point(141, 9);
			this.m_labUnitsLeft.Name = "m_labUnitsLeft";
			this.m_labUnitsLeft.Size = new System.Drawing.Size(27, 20);
			this.m_labUnitsLeft.TabIndex = 2;
			this.m_labUnitsLeft.Text = "17";
			// 
			// m_nudFighters
			// 
			this.m_nudFighters.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_nudFighters.Location = new System.Drawing.Point(125, 52);
			this.m_nudFighters.Maximum = new decimal(new int[] {
            17,
            0,
            0,
            0});
			this.m_nudFighters.Name = "m_nudFighters";
			this.m_nudFighters.Size = new System.Drawing.Size(43, 26);
			this.m_nudFighters.TabIndex = 3;
			this.m_nudFighters.ValueChanged += new System.EventHandler(this.ItemValueChanged);
			// 
			// m_nudTransports
			// 
			this.m_nudTransports.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_nudTransports.Location = new System.Drawing.Point(125, 84);
			this.m_nudTransports.Maximum = new decimal(new int[] {
            17,
            0,
            0,
            0});
			this.m_nudTransports.Name = "m_nudTransports";
			this.m_nudTransports.Size = new System.Drawing.Size(43, 26);
			this.m_nudTransports.TabIndex = 5;
			this.m_nudTransports.ValueChanged += new System.EventHandler(this.ItemValueChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(12, 86);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(89, 20);
			this.label4.TabIndex = 4;
			this.label4.Text = "Transports:";
			// 
			// m_nudTroopers
			// 
			this.m_nudTroopers.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_nudTroopers.Location = new System.Drawing.Point(125, 116);
			this.m_nudTroopers.Maximum = new decimal(new int[] {
            17,
            0,
            0,
            0});
			this.m_nudTroopers.Name = "m_nudTroopers";
			this.m_nudTroopers.Size = new System.Drawing.Size(43, 26);
			this.m_nudTroopers.TabIndex = 7;
			this.m_nudTroopers.ValueChanged += new System.EventHandler(this.ItemValueChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(12, 118);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(76, 20);
			this.label5.TabIndex = 6;
			this.label5.Text = "Troopers:";
			// 
			// m_nudGennies
			// 
			this.m_nudGennies.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_nudGennies.Location = new System.Drawing.Point(125, 148);
			this.m_nudGennies.Maximum = new decimal(new int[] {
            17,
            0,
            0,
            0});
			this.m_nudGennies.Name = "m_nudGennies";
			this.m_nudGennies.Size = new System.Drawing.Size(43, 26);
			this.m_nudGennies.TabIndex = 9;
			this.m_nudGennies.ValueChanged += new System.EventHandler(this.ItemValueChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(12, 150);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(73, 20);
			this.label6.TabIndex = 8;
			this.label6.Text = "Gennies:";
			// 
			// m_btnOK
			// 
			this.m_btnOK.Location = new System.Drawing.Point(12, 201);
			this.m_btnOK.Name = "m_btnOK";
			this.m_btnOK.Size = new System.Drawing.Size(75, 23);
			this.m_btnOK.TabIndex = 10;
			this.m_btnOK.Text = "OK";
			this.m_btnOK.UseVisualStyleBackColor = true;
			this.m_btnOK.Click += new System.EventHandler(this.m_btnOK_Click);
			// 
			// m_btnCancel
			// 
			this.m_btnCancel.Location = new System.Drawing.Point(93, 201);
			this.m_btnCancel.Name = "m_btnCancel";
			this.m_btnCancel.Size = new System.Drawing.Size(75, 23);
			this.m_btnCancel.TabIndex = 11;
			this.m_btnCancel.Text = "Cancel";
			this.m_btnCancel.UseVisualStyleBackColor = true;
			this.m_btnCancel.Click += new System.EventHandler(this.m_btnCancel_Click);
			// 
			// UnitSelectionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(192, 238);
			this.Controls.Add(this.m_btnCancel);
			this.Controls.Add(this.m_btnOK);
			this.Controls.Add(this.m_nudGennies);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.m_nudTroopers);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.m_nudTransports);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.m_nudFighters);
			this.Controls.Add(this.m_labUnitsLeft);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "UnitSelectionForm";
			this.Text = "Unit Selection";
			((System.ComponentModel.ISupportInitialize)(this.m_nudFighters)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_nudTransports)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_nudTroopers)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_nudGennies)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label m_labUnitsLeft;
		private System.Windows.Forms.NumericUpDown m_nudFighters;
		private System.Windows.Forms.NumericUpDown m_nudTransports;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown m_nudTroopers;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown m_nudGennies;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button m_btnOK;
		private System.Windows.Forms.Button m_btnCancel;
	}
}