using System;
using System.Windows.Forms;
using BuckRogers.Networking;
using BuckRogers.Interface;

namespace BuckRogers.Interface
{
	partial class GameLobbyForm
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
			this.m_txtAddress = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.m_txtPort = new System.Windows.Forms.TextBox();
			this.m_btnConnect = new System.Windows.Forms.Button();
			this.m_btnDisconnect = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.m_lbMessages = new System.Windows.Forms.ListBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.label7 = new System.Windows.Forms.Label();
			this.m_lblConnectionStatus = new System.Windows.Forms.Label();
			this.m_btnAddPlayer = new System.Windows.Forms.Button();
			this.m_btnRemovePlayer = new System.Windows.Forms.Button();
			this.m_txtGameSettings = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tabControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_txtAddress
			// 
			this.m_txtAddress.Location = new System.Drawing.Point(12, 25);
			this.m_txtAddress.Name = "m_txtAddress";
			this.m_txtAddress.Size = new System.Drawing.Size(129, 20);
			this.m_txtAddress.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Address:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(147, 9);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(29, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Port:";
			// 
			// m_txtPort
			// 
			this.m_txtPort.Location = new System.Drawing.Point(147, 25);
			this.m_txtPort.Name = "m_txtPort";
			this.m_txtPort.Size = new System.Drawing.Size(39, 20);
			this.m_txtPort.TabIndex = 2;
			// 
			// m_btnConnect
			// 
			this.m_btnConnect.Location = new System.Drawing.Point(12, 51);
			this.m_btnConnect.Name = "m_btnConnect";
			this.m_btnConnect.Size = new System.Drawing.Size(75, 23);
			this.m_btnConnect.TabIndex = 4;
			this.m_btnConnect.Text = "Connect";
			this.m_btnConnect.UseVisualStyleBackColor = true;
			this.m_btnConnect.Click += new System.EventHandler(this.m_btnConnect_Click);
			// 
			// m_btnDisconnect
			// 
			this.m_btnDisconnect.Enabled = false;
			this.m_btnDisconnect.Location = new System.Drawing.Point(111, 51);
			this.m_btnDisconnect.Name = "m_btnDisconnect";
			this.m_btnDisconnect.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.m_btnDisconnect.Size = new System.Drawing.Size(75, 23);
			this.m_btnDisconnect.TabIndex = 5;
			this.m_btnDisconnect.Text = "Disconnect";
			this.m_btnDisconnect.UseVisualStyleBackColor = true;
			this.m_btnDisconnect.Click += new System.EventHandler(this.m_btnDisconnect_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(739, 18);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(58, 13);
			this.label4.TabIndex = 15;
			this.label4.Text = "Messages:";
			// 
			// m_lbMessages
			// 
			this.m_lbMessages.FormattingEnabled = true;
			this.m_lbMessages.HorizontalScrollbar = true;
			this.m_lbMessages.Location = new System.Drawing.Point(742, 34);
			this.m_lbMessages.Name = "m_lbMessages";
			this.m_lbMessages.Size = new System.Drawing.Size(174, 277);
			this.m_lbMessages.TabIndex = 14;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(192, 12);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(548, 304);
			this.tabControl1.TabIndex = 26;
			// 
			// tabPage1
			// 
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(540, 278);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "tabPage1";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(540, 278);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(12, 83);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(62, 13);
			this.label7.TabIndex = 27;
			this.label7.Text = "Connected:";
			// 
			// m_lblConnectionStatus
			// 
			this.m_lblConnectionStatus.AutoSize = true;
			this.m_lblConnectionStatus.Location = new System.Drawing.Point(80, 83);
			this.m_lblConnectionStatus.Name = "m_lblConnectionStatus";
			this.m_lblConnectionStatus.Size = new System.Drawing.Size(21, 13);
			this.m_lblConnectionStatus.TabIndex = 28;
			this.m_lblConnectionStatus.Text = "No";
			// 
			// m_btnAddPlayer
			// 
			this.m_btnAddPlayer.Location = new System.Drawing.Point(12, 106);
			this.m_btnAddPlayer.Name = "m_btnAddPlayer";
			this.m_btnAddPlayer.Size = new System.Drawing.Size(75, 23);
			this.m_btnAddPlayer.TabIndex = 29;
			this.m_btnAddPlayer.Text = "Add Player";
			this.m_btnAddPlayer.UseVisualStyleBackColor = true;
			this.m_btnAddPlayer.Click += new System.EventHandler(this.m_btnAddPlayer_Click);
			// 
			// m_btnRemovePlayer
			// 
			this.m_btnRemovePlayer.Location = new System.Drawing.Point(93, 106);
			this.m_btnRemovePlayer.Name = "m_btnRemovePlayer";
			this.m_btnRemovePlayer.Size = new System.Drawing.Size(93, 23);
			this.m_btnRemovePlayer.TabIndex = 30;
			this.m_btnRemovePlayer.Text = "Remove Player";
			this.m_btnRemovePlayer.UseVisualStyleBackColor = true;
			this.m_btnRemovePlayer.Click += new System.EventHandler(this.m_btnRemovePlayer_Click);
			// 
			// m_txtGameSettings
			// 
			this.m_txtGameSettings.Location = new System.Drawing.Point(12, 175);
			this.m_txtGameSettings.Multiline = true;
			this.m_txtGameSettings.Name = "m_txtGameSettings";
			this.m_txtGameSettings.ReadOnly = true;
			this.m_txtGameSettings.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.m_txtGameSettings.Size = new System.Drawing.Size(174, 136);
			this.m_txtGameSettings.TabIndex = 31;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 159);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(79, 13);
			this.label3.TabIndex = 32;
			this.label3.Text = "Game Settings:";
			// 
			// ClientForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(919, 354);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.m_txtGameSettings);
			this.Controls.Add(this.m_btnRemovePlayer);
			this.Controls.Add(this.m_btnAddPlayer);
			this.Controls.Add(this.m_lblConnectionStatus);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.m_lbMessages);
			this.Controls.Add(this.m_btnDisconnect);
			this.Controls.Add(this.m_btnConnect);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.m_txtPort);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.m_txtAddress);
			this.Name = "ClientForm";
			this.Text = "Client";
			this.tabControl1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox m_txtAddress;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox m_txtPort;
		private System.Windows.Forms.Button m_btnConnect;
		private System.Windows.Forms.Button m_btnDisconnect;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ListBox m_lbMessages;
		private TabControl tabControl1;
		private TabPage tabPage1;
		private TabPage tabPage2;
		private Label label7;
		private Label m_lblConnectionStatus;
		private Button m_btnAddPlayer;
		private Button m_btnRemovePlayer;
		private TextBox m_txtGameSettings;
		private Label label3;
	}
}

