namespace BuckRogers.Interface
{
	partial class ClientLobbyPanel
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.m_txtName = new System.Windows.Forms.TextBox();
			this.m_btnSendAll = new System.Windows.Forms.Button();
			this.m_btnSendPlayer = new System.Windows.Forms.Button();
			this.m_txtMessage = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.m_chkPlayerConnected = new System.Windows.Forms.CheckBox();
			this.m_txtMessagesDisplay = new System.Windows.Forms.TextBox();
			this.m_cbColorPicker = new Azuria.Controls.ColorPicker.ColorPicker();
			this.m_lbOtherClients = new BuckRogers.Interface.PlayerListBox();
			this.SuspendLayout();
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(0, 98);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(34, 13);
			this.label6.TabIndex = 42;
			this.label6.Text = "Color:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(0, 47);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(38, 13);
			this.label5.TabIndex = 39;
			this.label5.Text = "Name:";
			// 
			// m_txtName
			// 
			this.m_txtName.Location = new System.Drawing.Point(3, 63);
			this.m_txtName.Name = "m_txtName";
			this.m_txtName.Size = new System.Drawing.Size(158, 20);
			this.m_txtName.TabIndex = 38;
			// 
			// m_btnSendAll
			// 
			this.m_btnSendAll.Enabled = false;
			this.m_btnSendAll.Location = new System.Drawing.Point(463, 30);
			this.m_btnSendAll.Name = "m_btnSendAll";
			this.m_btnSendAll.Size = new System.Drawing.Size(75, 23);
			this.m_btnSendAll.TabIndex = 37;
			this.m_btnSendAll.Text = "Send All";
			this.m_btnSendAll.UseVisualStyleBackColor = true;
			this.m_btnSendAll.Click += new System.EventHandler(this.m_btnSendAll_Click);
			// 
			// m_btnSendPlayer
			// 
			this.m_btnSendPlayer.Enabled = false;
			this.m_btnSendPlayer.Location = new System.Drawing.Point(463, 3);
			this.m_btnSendPlayer.Name = "m_btnSendPlayer";
			this.m_btnSendPlayer.Size = new System.Drawing.Size(75, 23);
			this.m_btnSendPlayer.TabIndex = 36;
			this.m_btnSendPlayer.Text = "Send Player";
			this.m_btnSendPlayer.UseVisualStyleBackColor = true;
			this.m_btnSendPlayer.Click += new System.EventHandler(this.m_btnSendPlayer_Click);
			// 
			// m_txtMessage
			// 
			this.m_txtMessage.Location = new System.Drawing.Point(193, 3);
			this.m_txtMessage.Multiline = true;
			this.m_txtMessage.Name = "m_txtMessage";
			this.m_txtMessage.Size = new System.Drawing.Size(264, 50);
			this.m_txtMessage.TabIndex = 35;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(0, 161);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(73, 13);
			this.label3.TabIndex = 34;
			this.label3.Text = "Other Players:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(190, 70);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(58, 13);
			this.label4.TabIndex = 33;
			this.label4.Text = "Messages:";
			// 
			// m_chkPlayerConnected
			// 
			this.m_chkPlayerConnected.Appearance = System.Windows.Forms.Appearance.Button;
			this.m_chkPlayerConnected.Enabled = false;
			this.m_chkPlayerConnected.Location = new System.Drawing.Point(3, 3);
			this.m_chkPlayerConnected.Name = "m_chkPlayerConnected";
			this.m_chkPlayerConnected.Size = new System.Drawing.Size(161, 24);
			this.m_chkPlayerConnected.TabIndex = 43;
			this.m_chkPlayerConnected.Text = "Player Connected";
			this.m_chkPlayerConnected.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.m_chkPlayerConnected.UseVisualStyleBackColor = true;
			this.m_chkPlayerConnected.CheckedChanged += new System.EventHandler(this.OnPlayerConnectedClicked);
			// 
			// m_txtMessagesDisplay
			// 
			this.m_txtMessagesDisplay.Location = new System.Drawing.Point(193, 86);
			this.m_txtMessagesDisplay.MaxLength = 65535;
			this.m_txtMessagesDisplay.Multiline = true;
			this.m_txtMessagesDisplay.Name = "m_txtMessagesDisplay";
			this.m_txtMessagesDisplay.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.m_txtMessagesDisplay.Size = new System.Drawing.Size(345, 186);
			this.m_txtMessagesDisplay.TabIndex = 44;
			// 
			// m_cbColorPicker
			// 
			this.m_cbColorPicker.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_cbColorPicker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_cbColorPicker.FormattingEnabled = true;
			this.m_cbColorPicker.Items = null;
			this.m_cbColorPicker.Location = new System.Drawing.Point(3, 114);
			this.m_cbColorPicker.Name = "m_cbColorPicker";
			this.m_cbColorPicker.Size = new System.Drawing.Size(161, 21);
			this.m_cbColorPicker.TabIndex = 41;
			this.m_cbColorPicker.SelectedIndexChanged += new System.EventHandler(this.m_cbColorPicker_SelectedIndexChanged);
			// 
			// m_lbOtherClients
			// 
			this.m_lbOtherClients.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_lbOtherClients.FormattingEnabled = true;
			this.m_lbOtherClients.Location = new System.Drawing.Point(3, 177);
			this.m_lbOtherClients.Name = "m_lbOtherClients";
			this.m_lbOtherClients.Size = new System.Drawing.Size(161, 95);
			this.m_lbOtherClients.TabIndex = 40;
			// 
			// ClientLobbyPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.m_txtMessagesDisplay);
			this.Controls.Add(this.m_chkPlayerConnected);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.m_cbColorPicker);
			this.Controls.Add(this.m_lbOtherClients);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.m_txtName);
			this.Controls.Add(this.m_btnSendAll);
			this.Controls.Add(this.m_btnSendPlayer);
			this.Controls.Add(this.m_txtMessage);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label4);
			this.Name = "ClientLobbyPanel";
			this.Size = new System.Drawing.Size(542, 275);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label6;
		private Azuria.Controls.ColorPicker.ColorPicker m_cbColorPicker;
		private BuckRogers.Interface.PlayerListBox m_lbOtherClients;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox m_txtName;
		private System.Windows.Forms.Button m_btnSendAll;
		private System.Windows.Forms.Button m_btnSendPlayer;
		private System.Windows.Forms.TextBox m_txtMessage;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox m_chkPlayerConnected;
		private System.Windows.Forms.TextBox m_txtMessagesDisplay;
	}
}
