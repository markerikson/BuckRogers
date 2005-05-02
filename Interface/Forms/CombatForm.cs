using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;

namespace BuckRogers.Interface
{
	/// <summary>
	/// Summary description for CombatForm.
	/// </summary>
	public class CombatForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label m_labLocation;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label m_labBattleType;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label m_labCurrentPlayer;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Button m_butRemAttackers;
		private System.Windows.Forms.Button m_butAddAttackers;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ListView m_lvAttackers;
		private System.Windows.Forms.ColumnHeader columnHeader12;
		private System.Windows.Forms.ColumnHeader columnHeader13;
		private System.Windows.Forms.ColumnHeader columnHeader14;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListView m_lvAttUnused;
		private System.Windows.Forms.ColumnHeader columnHeader11;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox m_cbNumUnits;
		private System.Windows.Forms.ColumnHeader columnHeader18;
		private System.Windows.Forms.ColumnHeader columnHeader19;
		private System.Windows.Forms.ColumnHeader columnHeader22;
		private System.Windows.Forms.ColumnHeader columnHeader20;
		private System.Windows.Forms.ColumnHeader columnHeader21;
		private System.Windows.Forms.ColumnHeader columnHeader29;
		private System.Windows.Forms.ColumnHeader columnHeader15;
		private System.Windows.Forms.ColumnHeader columnHeader16;
		private System.Windows.Forms.ColumnHeader columnHeader17;
		private System.Windows.Forms.ColumnHeader columnHeader24;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ColumnHeader columnHeader8;
		private System.Windows.Forms.ColumnHeader columnHeader9;
		private System.Windows.Forms.ColumnHeader columnHeader10;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.ColumnHeader columnHeader7;
		private System.Windows.Forms.ColumnHeader columnHeader23;
		private System.Windows.Forms.ColumnHeader columnHeader25;
		private System.Windows.Forms.ColumnHeader columnHeader26;
		private System.Windows.Forms.ColumnHeader columnHeader27;
		private System.Windows.Forms.ColumnHeader columnHeader28;
		private System.Windows.Forms.ColumnHeader columnHeader30;
		private System.Windows.Forms.ColumnHeader columnHeader31;
		private System.Windows.Forms.Button m_butRemDefenders;
		private System.Windows.Forms.Button m_butAddDefenders;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ListView m_lvDefenders;
		private System.Windows.Forms.ColumnHeader columnHeader32;
		private System.Windows.Forms.ColumnHeader columnHeader33;
		private System.Windows.Forms.ColumnHeader columnHeader34;
		private System.Windows.Forms.ColumnHeader columnHeader35;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ListView m_lvEnemyLive;
		private System.Windows.Forms.ColumnHeader columnHeader36;
		private System.Windows.Forms.ColumnHeader columnHeader37;
		private System.Windows.Forms.ColumnHeader columnHeader38;
		private System.Windows.Forms.ColumnHeader columnHeader39;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.ListView m_lvResults;
		private System.Windows.Forms.ColumnHeader columnHeader40;
		private System.Windows.Forms.ColumnHeader columnHeader41;
		private System.Windows.Forms.ColumnHeader columnHeader42;
		private System.Windows.Forms.ColumnHeader columnHeader43;
		private System.Windows.Forms.ColumnHeader columnHeader44;
		private System.Windows.Forms.ColumnHeader columnHeader45;
		private System.Windows.Forms.Button m_btnNextBattle;
		private System.Windows.Forms.Button m_btnNextPlayer;
		private System.Windows.Forms.Button m_btnAttack;
		private System.Windows.Forms.Button m_btnContinue;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private BattleController m_battleController;
		private System.Windows.Forms.Label m_labBattlesLeft;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.ListBox m_lbCurrentPlayer;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListView m_lvCasualties;
		private System.Windows.Forms.ColumnHeader columnHeader46;
		private System.Windows.Forms.ColumnHeader columnHeader47;
		private System.Windows.Forms.ColumnHeader columnHeader48;
		private System.Windows.Forms.Label label3;
		private GameController m_controller;

		public CombatForm(GameController gc, BattleController bc)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			m_cbNumUnits.SelectedIndex = 5;

			m_lvAttackers.Items.Clear();
			m_lvDefenders.Items.Clear();
			m_lvAttUnused.Items.Clear();
			m_lvEnemyLive.Items.Clear();

			m_battleController = bc;
			m_controller = gc;

			m_battleController.UnitsToDisplay += new DisplayUnitsHandler(DisplayUnits);
			m_battleController.StatusUpdate += new StatusUpdateHandler(OnBattleControllerStatusUpdate);

			
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
																													 "Mark",
																													 "Trooper",
																													 "15"}, -1);
			System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
																													 "Mark",
																													 "Gennie",
																													 "5"}, -1);
			System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
																													 "Mark",
																													 "Fighter",
																													 "10"}, -1);
			System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
																													 "Mark",
																													 "Transport",
																													 "5"}, -1);
			System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
																													 "Mark",
																													 "Marker",
																													 "1"}, -1);
			System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] {
																													 "Jake",
																													 "Fighter",
																													 "1"}, -1);
			System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem(new string[] {
																													 "Jake",
																													 "Transport",
																													 "100"}, -1);
			System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem(new string[] {
																													 "Stu",
																													 "Trooper",
																													 "8"}, -1);
			System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem(new string[] {
																													 "Mark",
																													 "Trooper",
																													 "15"}, -1);
			System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem(new string[] {
																													  "Mark",
																													  "Gennie",
																													  "5"}, -1);
			System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem(new string[] {
																													  "Mark",
																													  "Fighter",
																													  "10"}, -1);
			System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem(new string[] {
																													  "Mark",
																													  "Transport",
																													  "5"}, -1);
			System.Windows.Forms.ListViewItem listViewItem13 = new System.Windows.Forms.ListViewItem(new string[] {
																													  "Mark",
																													  "Marker",
																													  "1"}, -1);
			this.m_labLocation = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.m_labBattleType = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.m_labCurrentPlayer = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.m_butRemAttackers = new System.Windows.Forms.Button();
			this.m_butAddAttackers = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.m_lvAttackers = new System.Windows.Forms.ListView();
			this.columnHeader12 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader13 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader14 = new System.Windows.Forms.ColumnHeader();
			this.label2 = new System.Windows.Forms.Label();
			this.m_lvAttUnused = new System.Windows.Forms.ListView();
			this.columnHeader11 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.label7 = new System.Windows.Forms.Label();
			this.m_cbNumUnits = new System.Windows.Forms.ComboBox();
			this.columnHeader18 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader19 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader22 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader20 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader21 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader29 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader15 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader16 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader17 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader24 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader23 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader25 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader26 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader27 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader28 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader30 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader31 = new System.Windows.Forms.ColumnHeader();
			this.m_butRemDefenders = new System.Windows.Forms.Button();
			this.m_butAddDefenders = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.m_lvDefenders = new System.Windows.Forms.ListView();
			this.columnHeader32 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader33 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader34 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader35 = new System.Windows.Forms.ColumnHeader();
			this.label4 = new System.Windows.Forms.Label();
			this.m_lvEnemyLive = new System.Windows.Forms.ListView();
			this.columnHeader36 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader37 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader38 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader39 = new System.Windows.Forms.ColumnHeader();
			this.label10 = new System.Windows.Forms.Label();
			this.m_lvResults = new System.Windows.Forms.ListView();
			this.columnHeader40 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader41 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader42 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader43 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader44 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader45 = new System.Windows.Forms.ColumnHeader();
			this.m_btnNextBattle = new System.Windows.Forms.Button();
			this.m_btnNextPlayer = new System.Windows.Forms.Button();
			this.m_btnAttack = new System.Windows.Forms.Button();
			this.m_btnContinue = new System.Windows.Forms.Button();
			this.m_labBattlesLeft = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.m_lbCurrentPlayer = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.m_lvCasualties = new System.Windows.Forms.ListView();
			this.columnHeader46 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader47 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader48 = new System.Windows.Forms.ColumnHeader();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_labLocation
			// 
			this.m_labLocation.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_labLocation.Location = new System.Drawing.Point(316, 4);
			this.m_labLocation.Name = "m_labLocation";
			this.m_labLocation.Size = new System.Drawing.Size(208, 23);
			this.m_labLocation.TabIndex = 34;
			this.m_labLocation.Text = "Current Player:";
			// 
			// label12
			// 
			this.label12.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label12.Location = new System.Drawing.Point(240, 4);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(76, 23);
			this.label12.TabIndex = 33;
			this.label12.Text = "Location:";
			// 
			// m_labBattleType
			// 
			this.m_labBattleType.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_labBattleType.Location = new System.Drawing.Point(100, 4);
			this.m_labBattleType.Name = "m_labBattleType";
			this.m_labBattleType.Size = new System.Drawing.Size(128, 23);
			this.m_labBattleType.TabIndex = 32;
			this.m_labBattleType.Text = "Current Player:";
			// 
			// label11
			// 
			this.label11.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label11.Location = new System.Drawing.Point(4, 4);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(96, 23);
			this.label11.TabIndex = 31;
			this.label11.Text = "Battle Type:";
			// 
			// m_labCurrentPlayer
			// 
			this.m_labCurrentPlayer.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_labCurrentPlayer.Location = new System.Drawing.Point(656, 4);
			this.m_labCurrentPlayer.Name = "m_labCurrentPlayer";
			this.m_labCurrentPlayer.Size = new System.Drawing.Size(128, 23);
			this.m_labCurrentPlayer.TabIndex = 30;
			this.m_labCurrentPlayer.Text = "Current Player:";
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label9.Location = new System.Drawing.Point(536, 4);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(120, 23);
			this.label9.TabIndex = 29;
			this.label9.Text = "Current Player:";
			// 
			// m_butRemAttackers
			// 
			this.m_butRemAttackers.Location = new System.Drawing.Point(204, 116);
			this.m_butRemAttackers.Name = "m_butRemAttackers";
			this.m_butRemAttackers.TabIndex = 40;
			this.m_butRemAttackers.Text = "<< Remove";
			this.m_butRemAttackers.Click += new System.EventHandler(this.m_butRemAttackers_Click);
			// 
			// m_butAddAttackers
			// 
			this.m_butAddAttackers.Location = new System.Drawing.Point(204, 84);
			this.m_butAddAttackers.Name = "m_butAddAttackers";
			this.m_butAddAttackers.TabIndex = 39;
			this.m_butAddAttackers.Text = "Add >>";
			this.m_butAddAttackers.Click += new System.EventHandler(this.m_butAddAttackers_Click);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(288, 36);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(100, 16);
			this.label6.TabIndex = 38;
			this.label6.Text = "Attacking units:";
			// 
			// m_lvAttackers
			// 
			this.m_lvAttackers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							this.columnHeader12,
																							this.columnHeader13,
																							this.columnHeader14});
			this.m_lvAttackers.FullRowSelect = true;
			this.m_lvAttackers.HideSelection = false;
			this.m_lvAttackers.Location = new System.Drawing.Point(288, 52);
			this.m_lvAttackers.MultiSelect = false;
			this.m_lvAttackers.Name = "m_lvAttackers";
			this.m_lvAttackers.Size = new System.Drawing.Size(192, 136);
			this.m_lvAttackers.TabIndex = 37;
			this.m_lvAttackers.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader12
			// 
			this.columnHeader12.Text = "Player";
			// 
			// columnHeader13
			// 
			this.columnHeader13.Text = "Type";
			// 
			// columnHeader14
			// 
			this.columnHeader14.Text = "Count";
			this.columnHeader14.Width = 45;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(4, 36);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 16);
			this.label2.TabIndex = 36;
			this.label2.Text = "Unused Units:";
			// 
			// m_lvAttUnused
			// 
			this.m_lvAttUnused.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							this.columnHeader11,
																							this.columnHeader1,
																							this.columnHeader2});
			this.m_lvAttUnused.FullRowSelect = true;
			this.m_lvAttUnused.HideSelection = false;
			this.m_lvAttUnused.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
																						  listViewItem1,
																						  listViewItem2,
																						  listViewItem3,
																						  listViewItem4,
																						  listViewItem5});
			this.m_lvAttUnused.Location = new System.Drawing.Point(4, 52);
			this.m_lvAttUnused.MultiSelect = false;
			this.m_lvAttUnused.Name = "m_lvAttUnused";
			this.m_lvAttUnused.Size = new System.Drawing.Size(192, 136);
			this.m_lvAttUnused.TabIndex = 35;
			this.m_lvAttUnused.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader11
			// 
			this.columnHeader11.Text = "Player";
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Type";
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Count";
			this.columnHeader2.Width = 45;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(204, 188);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(72, 16);
			this.label7.TabIndex = 42;
			this.label7.Text = "Units to shift:";
			// 
			// m_cbNumUnits
			// 
			this.m_cbNumUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_cbNumUnits.Items.AddRange(new object[] {
															  "1",
															  "5",
															  "10",
															  "25",
															  "100",
															  "1000"});
			this.m_cbNumUnits.Location = new System.Drawing.Point(204, 204);
			this.m_cbNumUnits.Name = "m_cbNumUnits";
			this.m_cbNumUnits.Size = new System.Drawing.Size(76, 21);
			this.m_cbNumUnits.TabIndex = 41;
			// 
			// columnHeader18
			// 
			this.columnHeader18.Text = "Attacker";
			// 
			// columnHeader19
			// 
			this.columnHeader19.Text = "Defender";
			// 
			// columnHeader22
			// 
			this.columnHeader22.Text = "Owner";
			this.columnHeader22.Width = 80;
			// 
			// columnHeader20
			// 
			this.columnHeader20.Text = "Roll";
			this.columnHeader20.Width = 35;
			// 
			// columnHeader21
			// 
			this.columnHeader21.Text = "Leader";
			// 
			// columnHeader29
			// 
			this.columnHeader29.Text = "Hit";
			this.columnHeader29.Width = 35;
			// 
			// columnHeader15
			// 
			this.columnHeader15.Text = "Player";
			this.columnHeader15.Width = 80;
			// 
			// columnHeader16
			// 
			this.columnHeader16.Text = "Type";
			// 
			// columnHeader17
			// 
			this.columnHeader17.Text = "Count";
			this.columnHeader17.Width = 45;
			// 
			// columnHeader24
			// 
			this.columnHeader24.Text = "Territory";
			this.columnHeader24.Width = 100;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Player";
			this.columnHeader3.Width = 80;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Type";
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Count";
			this.columnHeader5.Width = 45;
			// 
			// columnHeader8
			// 
			this.columnHeader8.Text = "Player";
			this.columnHeader8.Width = 80;
			// 
			// columnHeader9
			// 
			this.columnHeader9.Text = "Type";
			// 
			// columnHeader10
			// 
			this.columnHeader10.Text = "Count";
			this.columnHeader10.Width = 44;
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "Player";
			this.columnHeader6.Width = 80;
			// 
			// columnHeader7
			// 
			this.columnHeader7.Text = "Type";
			// 
			// columnHeader23
			// 
			this.columnHeader23.Text = "Count";
			this.columnHeader23.Width = 45;
			// 
			// columnHeader25
			// 
			this.columnHeader25.Text = "Territory";
			this.columnHeader25.Width = 100;
			// 
			// columnHeader26
			// 
			this.columnHeader26.Text = "Type";
			// 
			// columnHeader27
			// 
			this.columnHeader27.Text = "Count";
			this.columnHeader27.Width = 50;
			// 
			// columnHeader28
			// 
			this.columnHeader28.Text = "Player";
			this.columnHeader28.Width = 80;
			// 
			// columnHeader30
			// 
			this.columnHeader30.Text = "Type";
			// 
			// columnHeader31
			// 
			this.columnHeader31.Text = "Count";
			this.columnHeader31.Width = 45;
			// 
			// m_butRemDefenders
			// 
			this.m_butRemDefenders.Location = new System.Drawing.Point(204, 304);
			this.m_butRemDefenders.Name = "m_butRemDefenders";
			this.m_butRemDefenders.TabIndex = 48;
			this.m_butRemDefenders.Text = "<< Remove";
			this.m_butRemDefenders.Click += new System.EventHandler(this.m_butRemDefenders_Click);
			// 
			// m_butAddDefenders
			// 
			this.m_butAddDefenders.Location = new System.Drawing.Point(204, 272);
			this.m_butAddDefenders.Name = "m_butAddDefenders";
			this.m_butAddDefenders.TabIndex = 47;
			this.m_butAddDefenders.Text = "Add >>";
			this.m_butAddDefenders.Click += new System.EventHandler(this.m_butAddDefenders_Click);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(292, 200);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(100, 16);
			this.label8.TabIndex = 46;
			this.label8.Text = "Defending units:";
			// 
			// m_lvDefenders
			// 
			this.m_lvDefenders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							this.columnHeader32,
																							this.columnHeader33,
																							this.columnHeader34,
																							this.columnHeader35});
			this.m_lvDefenders.FullRowSelect = true;
			this.m_lvDefenders.HideSelection = false;
			this.m_lvDefenders.Location = new System.Drawing.Point(288, 216);
			this.m_lvDefenders.MultiSelect = false;
			this.m_lvDefenders.Name = "m_lvDefenders";
			this.m_lvDefenders.Size = new System.Drawing.Size(192, 180);
			this.m_lvDefenders.TabIndex = 45;
			this.m_lvDefenders.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader32
			// 
			this.columnHeader32.Text = "Player";
			// 
			// columnHeader33
			// 
			this.columnHeader33.Text = "Type";
			// 
			// columnHeader34
			// 
			this.columnHeader34.Text = "Count";
			this.columnHeader34.Width = 45;
			// 
			// columnHeader35
			// 
			this.columnHeader35.Text = "Territory";
			this.columnHeader35.Width = 100;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(4, 200);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(124, 16);
			this.label4.TabIndex = 44;
			this.label4.Text = "Enemy Units (Live):";
			// 
			// m_lvEnemyLive
			// 
			this.m_lvEnemyLive.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							this.columnHeader36,
																							this.columnHeader37,
																							this.columnHeader38,
																							this.columnHeader39});
			this.m_lvEnemyLive.FullRowSelect = true;
			this.m_lvEnemyLive.HideSelection = false;
			this.m_lvEnemyLive.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
																						  listViewItem6,
																						  listViewItem7,
																						  listViewItem8});
			this.m_lvEnemyLive.Location = new System.Drawing.Point(4, 216);
			this.m_lvEnemyLive.MultiSelect = false;
			this.m_lvEnemyLive.Name = "m_lvEnemyLive";
			this.m_lvEnemyLive.Size = new System.Drawing.Size(192, 180);
			this.m_lvEnemyLive.TabIndex = 43;
			this.m_lvEnemyLive.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader36
			// 
			this.columnHeader36.Text = "Player";
			// 
			// columnHeader37
			// 
			this.columnHeader37.Text = "Type";
			// 
			// columnHeader38
			// 
			this.columnHeader38.Text = "Count";
			this.columnHeader38.Width = 45;
			// 
			// columnHeader39
			// 
			this.columnHeader39.Text = "Territory";
			this.columnHeader39.Width = 100;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(492, 36);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(100, 16);
			this.label10.TabIndex = 50;
			this.label10.Text = "Combat results:";
			// 
			// m_lvResults
			// 
			this.m_lvResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						  this.columnHeader40,
																						  this.columnHeader41,
																						  this.columnHeader42,
																						  this.columnHeader43,
																						  this.columnHeader44,
																						  this.columnHeader45});
			this.m_lvResults.HideSelection = false;
			this.m_lvResults.Location = new System.Drawing.Point(492, 52);
			this.m_lvResults.Name = "m_lvResults";
			this.m_lvResults.Size = new System.Drawing.Size(316, 176);
			this.m_lvResults.TabIndex = 49;
			this.m_lvResults.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader40
			// 
			this.columnHeader40.Text = "Attacker";
			// 
			// columnHeader41
			// 
			this.columnHeader41.Text = "Defender";
			// 
			// columnHeader42
			// 
			this.columnHeader42.Text = "Owner";
			// 
			// columnHeader43
			// 
			this.columnHeader43.Text = "Roll";
			this.columnHeader43.Width = 35;
			// 
			// columnHeader44
			// 
			this.columnHeader44.Text = "Leader";
			this.columnHeader44.Width = 48;
			// 
			// columnHeader45
			// 
			this.columnHeader45.Text = "Hit";
			this.columnHeader45.Width = 35;
			// 
			// m_btnNextBattle
			// 
			this.m_btnNextBattle.Location = new System.Drawing.Point(264, 404);
			this.m_btnNextBattle.Name = "m_btnNextBattle";
			this.m_btnNextBattle.TabIndex = 54;
			this.m_btnNextBattle.Text = "Next battle";
			this.m_btnNextBattle.Click += new System.EventHandler(this.m_btnNextBattle_Click);
			// 
			// m_btnNextPlayer
			// 
			this.m_btnNextPlayer.Location = new System.Drawing.Point(176, 404);
			this.m_btnNextPlayer.Name = "m_btnNextPlayer";
			this.m_btnNextPlayer.TabIndex = 53;
			this.m_btnNextPlayer.Text = "Next player";
			this.m_btnNextPlayer.Click += new System.EventHandler(this.m_btnNextPlayer_Click);
			// 
			// m_btnAttack
			// 
			this.m_btnAttack.Location = new System.Drawing.Point(4, 404);
			this.m_btnAttack.Name = "m_btnAttack";
			this.m_btnAttack.TabIndex = 52;
			this.m_btnAttack.Text = "Attack";
			this.m_btnAttack.Click += new System.EventHandler(this.m_btnAttack_Click);
			// 
			// m_btnContinue
			// 
			this.m_btnContinue.Location = new System.Drawing.Point(88, 404);
			this.m_btnContinue.Name = "m_btnContinue";
			this.m_btnContinue.TabIndex = 51;
			this.m_btnContinue.Text = "Continue";
			this.m_btnContinue.Click += new System.EventHandler(this.m_btnContinue_Click);
			// 
			// m_labBattlesLeft
			// 
			this.m_labBattlesLeft.Location = new System.Drawing.Point(752, 360);
			this.m_labBattlesLeft.Name = "m_labBattlesLeft";
			this.m_labBattlesLeft.Size = new System.Drawing.Size(44, 16);
			this.m_labBattlesLeft.TabIndex = 56;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(688, 360);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(60, 16);
			this.label15.TabIndex = 55;
			this.label15.Text = "Battles left:";
			// 
			// m_lbCurrentPlayer
			// 
			this.m_lbCurrentPlayer.Items.AddRange(new object[] {
																   "Mark",
																   "Chris",
																   "Stu",
																   "Hannah",
																   "Jake",
																   "Kathryn",
																   "An extremely long name"});
			this.m_lbCurrentPlayer.Location = new System.Drawing.Point(688, 252);
			this.m_lbCurrentPlayer.Name = "m_lbCurrentPlayer";
			this.m_lbCurrentPlayer.Size = new System.Drawing.Size(120, 95);
			this.m_lbCurrentPlayer.TabIndex = 58;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(688, 236);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 16);
			this.label1.TabIndex = 57;
			this.label1.Text = "Current Player:";
			// 
			// m_lvCasualties
			// 
			this.m_lvCasualties.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							 this.columnHeader46,
																							 this.columnHeader47,
																							 this.columnHeader48});
			this.m_lvCasualties.FullRowSelect = true;
			this.m_lvCasualties.HideSelection = false;
			this.m_lvCasualties.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
																						   listViewItem9,
																						   listViewItem10,
																						   listViewItem11,
																						   listViewItem12,
																						   listViewItem13});
			this.m_lvCasualties.Location = new System.Drawing.Point(492, 252);
			this.m_lvCasualties.MultiSelect = false;
			this.m_lvCasualties.Name = "m_lvCasualties";
			this.m_lvCasualties.Size = new System.Drawing.Size(192, 144);
			this.m_lvCasualties.TabIndex = 59;
			this.m_lvCasualties.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader46
			// 
			this.columnHeader46.Text = "Player";
			// 
			// columnHeader47
			// 
			this.columnHeader47.Text = "Type";
			// 
			// columnHeader48
			// 
			this.columnHeader48.Text = "Count";
			this.columnHeader48.Width = 45;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(492, 236);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 16);
			this.label3.TabIndex = 60;
			this.label3.Text = "Casualties:";
			// 
			// CombatForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(816, 566);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.m_lvCasualties);
			this.Controls.Add(this.m_lbCurrentPlayer);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.m_labBattlesLeft);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.m_btnNextBattle);
			this.Controls.Add(this.m_btnNextPlayer);
			this.Controls.Add(this.m_btnAttack);
			this.Controls.Add(this.m_btnContinue);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.m_lvResults);
			this.Controls.Add(this.m_butRemDefenders);
			this.Controls.Add(this.m_butAddDefenders);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.m_lvDefenders);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.m_lvEnemyLive);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.m_cbNumUnits);
			this.Controls.Add(this.m_butRemAttackers);
			this.Controls.Add(this.m_butAddAttackers);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.m_lvAttackers);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.m_lvAttUnused);
			this.Controls.Add(this.m_labLocation);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.m_labBattleType);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.m_labCurrentPlayer);
			this.Controls.Add(this.label9);
			this.Name = "CombatForm";
			this.Text = "CombatForm";
			this.ResumeLayout(false);

		}
		#endregion

		public void BeginCombat()
		{
			m_battleController.Battles = m_controller.Battles;

			m_battleController.NextBattle();
			UpdateCombatInformation();
		}

		public void DisplayUnits(object sender, DisplayUnitsEventArgs duea)
		{
			ListView lv = null;
			bool includeTerritory = false;
			switch(duea.Category)
			{
				case DisplayCategory.Attackers:
				{
					lv = m_lvAttackers;
					includeTerritory = false;
					break;
				}
				case DisplayCategory.Defenders:
				{
					lv = m_lvDefenders;
					includeTerritory = true;
					break;
				}
				case DisplayCategory.SurvivingDefenders:
				{
					lv = m_lvEnemyLive;
					includeTerritory = true;
					break;
				}
				case DisplayCategory.UnusedAttackers:
				{
					lv = m_lvAttUnused;
					includeTerritory = false;
					break;
				}

			}
			AddUnitsToListView(duea.Units, lv, includeTerritory);
		}

		private void AddUnitsToListView(UnitCollection uc, ListView lv, bool includeTerritory)
		{
			foreach(Player p in uc.GetPlayersWithUnits())
			{
				UnitCollection playerUnits = uc.GetUnits(p);
				UnitCollection combatUnits = new UnitCollection();
				if(m_battleController.CurrentBattle.Type == BattleType.Bombing)
				{
					combatUnits.AddAllUnits(playerUnits);
				}
				else
				{
					combatUnits.AddAllUnits(uc.GetCombatUnits());
				}
				

				ArrayList territories = combatUnits.GetUnitTerritories();

				// Theoretically, the only time this should matter is if it's a bombing
				// battle, since all other times there's only one territory
				foreach(Territory t in territories)
				{
					UnitCollection unitsInTerritory = playerUnits.GetUnits(t);
					foreach(UnitType ut in unitsInTerritory.GetUnitTypeCount().Keys)
					{
						UnitCollection subgroup = unitsInTerritory.GetUnits(ut);
						

						ListViewItem lvi = new ListViewItem();
						lvi.Text = p.Name;
						lvi.SubItems.Add(ut.ToString());
						lvi.SubItems.Add(subgroup.Count.ToString());

						if(includeTerritory)
						{
							lvi.SubItems.Add(t.Name);
						}
					
						lv.Items.Add(lvi);
					}
				}
				
			}			
		}

		private void UpdateCombatInformation()
		{
			m_labBattleType.Text = m_battleController.CurrentBattle.Type.ToString();				
			m_labLocation.Text = m_battleController.CurrentBattle.Territory.Name;
			m_labBattlesLeft.Text = m_battleController.Battles.Count.ToString();

			m_btnContinue.Enabled = false;
			m_btnNextPlayer.Enabled = false;
			m_btnNextBattle.Enabled = false;
			m_btnAttack.Enabled = false;

			m_lbCurrentPlayer.Items.Clear();
			m_lvCasualties.Items.Clear();

			foreach(Player p in m_battleController.BattleOrder)
			{
				m_lbCurrentPlayer.Items.Add(p.Name);
			}

			m_lbCurrentPlayer.SelectedIndex = 0;
			m_labCurrentPlayer.Text = m_battleController.CurrentPlayer.Name;

			EnableAttack();
		}

		private void EnableAttack()
		{
			// TODO What if only a leader is in the territory?
			if( (m_lvAttackers.Items.Count > 0) &&
				(m_lvDefenders.Items.Count > 0))
			{
				m_btnAttack.Enabled = true;
			}
			else
			{
				m_btnAttack.Enabled = false;
			}
		}

		private void m_butAddAttackers_Click(object sender, System.EventArgs e)
		{
			MoveItems(m_lvAttUnused, m_lvAttackers, false);

			EnableAttack();

		}

		private void m_butRemAttackers_Click(object sender, System.EventArgs e)
		{
			MoveItems(m_lvAttackers, m_lvAttUnused, false);

			EnableAttack();
		}

		private void m_butAddDefenders_Click(object sender, System.EventArgs e)
		{
			MoveItems(m_lvEnemyLive, m_lvDefenders, true);

			EnableAttack();
		}

		private void m_butRemDefenders_Click(object sender, System.EventArgs e)
		{
			MoveItems(m_lvDefenders, m_lvEnemyLive, true);

			EnableAttack();
		}

		private void MoveItems(ListView origin, ListView destination, bool includeTerritory)
		{
			if(origin.SelectedIndices.Count == 0)
			{
				return;
			}

			int idxUnused = origin.SelectedIndices[0];

			ListViewItem lvi = origin.Items[idxUnused];

			string name = lvi.SubItems[0].Text;
			string type = lvi.SubItems[1].Text;
			int count = Int32.Parse(lvi.SubItems[2].Text);

			string territory = "";
			if(includeTerritory)
			{
				territory = lvi.SubItems[3].Text;
			}

			string sNumToMove = (string)m_cbNumUnits.SelectedItem;
			int numToMove = Int32.Parse(sNumToMove);

			if(numToMove > count)
			{
				numToMove = count;
			}

			int numLeft = count - numToMove;

			if(numLeft > 0)
			{
				origin.Items[idxUnused].SubItems[2].Text = numLeft.ToString();
				origin.Items[idxUnused].Selected = true;
				origin.Focus();
			}
			else
			{
				origin.Items.Remove(lvi);
			}
			
			bool addNewItem = true;
			if(destination.Items.Count > 0)
			{
				ListViewItem lastItem = destination.Items[destination.Items.Count - 1];
			
				string liName = lastItem.SubItems[0].Text;
				string liType = lastItem.SubItems[1].Text;

				string liTerritory = "";

				if(includeTerritory)
				{
					liTerritory = lastItem.SubItems[3].Text;
				}

				if(liName == name && liType == type)
				{
					if(!includeTerritory || (includeTerritory && territory == liTerritory))
					{
						addNewItem = false;
						int numCurrent = Int32.Parse(lastItem.SubItems[2].Text);
						numCurrent += numToMove;

						lastItem.SubItems[2].Text = numCurrent.ToString();
					}
				}
			}
			
			if(addNewItem)
			{
				ListViewItem lvi2 = new ListViewItem();
				lvi2.Text = name;
				lvi2.SubItems.Add(type);
				lvi2.SubItems.Add(numToMove.ToString());

				if(includeTerritory)
				{
					lvi2.SubItems.Add(territory);
				}

				destination.Items.Add(lvi2);
			}
		}

		#region Combat button handlers
		private void m_btnAttack_Click(object sender, System.EventArgs e)
		{
			CombatResult cr = null;
			m_lvResults.Items.Clear();

			m_btnAttack.Enabled = false;
			m_btnContinue.Enabled = false;
			m_btnNextBattle.Enabled = false;
			m_btnNextPlayer.Enabled = false;
			
			switch(m_battleController.CurrentBattle.Type)
			{
				case BattleType.KillerSatellite:
				{
					cr = m_battleController.DoKillerSatelliteCombat(m_battleController.CurrentBattle);
					break;
				}
				case BattleType.Bombing:
				{
					CombatInfo ci = SetUpCombat();
					ci.Type = BattleType.Bombing;

					UnitCollection leaders = m_battleController.CurrentBattle.Territory.Units.GetUnits(UnitType.Leader);
					ci.AttackingLeader = (leaders.GetUnits(m_battleController.CurrentPlayer).Count > 0);

					cr = m_battleController.DoBombingCombat(ci);
					break;
				}
				case BattleType.Normal:
				{
					CombatInfo ci = null;
					try
					{
						ci = SetUpCombat();
						ci.Type = BattleType.Normal;

						UnitCollection leaders = m_battleController.CurrentBattle.Territory.Units.GetUnits(UnitType.Leader);
						ci.AttackingLeader = (leaders.GetUnits(m_battleController.CurrentPlayer).Count > 0);

						cr = m_battleController.ExecuteCombat(ci);
					
					}
					catch(Exception ex)
					{
						//m_battleController.CurrentUnused.AddAllUnits(ci.Attackers);
						MessageBox.Show(ex.Message);
						m_btnAttack.Enabled = true;
						return;
					}
					
					break;
				}
			}

			
			if(cr != null)
			{
				ArrayList casualties = new ArrayList();

				foreach(AttackResult ar in cr.AttackResults)
				{
					ListViewItem lvi = new ListViewItem();
					lvi.Text = ar.Attacker.UnitType.ToString();
					lvi.SubItems.Add(ar.Defender.UnitType.ToString());
					lvi.SubItems.Add(ar.Defender.Owner.Name);
					lvi.SubItems.Add(ar.Roll.ToString());
					string leader = ar.Leader ? "Yes" : "No";
					lvi.SubItems.Add(leader);
					string hitResult = ar.Hit ? "Yes" : "No";
					lvi.SubItems.Add(hitResult);
					
					if(ar.Hit)
					{
						casualties.Add(ar);
					}

					m_lvResults.Items.Add(lvi);

				}

				foreach(AttackResult ar in casualties)
				{
					ListViewItem matchingItem = null;

					for(int i = 0; i < m_lvCasualties.Items.Count; i++)
					{
						ListViewItem lvi = m_lvCasualties.Items[i];

						if(lvi.Text != ar.Defender.Owner.Name)
						{
							continue;
						}

						if(lvi.SubItems[1].Text != ar.Defender.UnitType.ToString())
						{
							continue;
						}

						matchingItem = lvi;
						break;
					}

					if(matchingItem != null)
					{
						int numCurrent = Int32.Parse(matchingItem.SubItems[2].Text);
						numCurrent++;
						matchingItem.SubItems[2].Text = numCurrent.ToString();
					}
					else
					{
						matchingItem = new ListViewItem();
						matchingItem.Text = ar.Defender.Owner.Name;
						matchingItem.SubItems.Add(ar.Defender.UnitType.ToString());
						matchingItem.SubItems.Add("1");

						m_lvCasualties.Items.Add(matchingItem);
					}
				}

				m_battleController.LastResult = cr;
			}

			m_btnContinue.Enabled = true;
		}

		private void m_btnContinue_Click(object sender, System.EventArgs e)
		{
			m_btnContinue.Enabled = false;

			m_lvAttackers.Items.Clear();
			m_lvDefenders.Items.Clear();
			m_lvResults.Items.Clear();

			m_battleController.TurnResult.Casualties.AddAllUnits(m_battleController.LastResult.Casualties);
			
			foreach(Player p in m_battleController.LastResult.Casualties.GetPlayersWithUnits())
			{
				UnitCollection uc = (UnitCollection)m_battleController.SurvivingUnits[p];
				uc.RemoveAllUnits(m_battleController.LastResult.Casualties.GetUnits(p));
			}

			m_battleController.CurrentUnused.AddAllUnits(m_battleController.LastResult.UnusedAttackers);
			m_battleController.CurrentUnused.RemoveAllUnits(m_battleController.LastResult.UsedAttackers);

			
			if(m_battleController.CurrentUnused.Count == 0)
			{
				m_btnNextPlayer.Enabled = true;
			}
			else
			{
				m_lvAttUnused.Items.Clear();
				AddUnitsToListView(m_battleController.CurrentUnused, m_lvAttUnused, false);
			}

			m_lvAttUnused.Items.Clear();
			m_lvEnemyLive.Items.Clear();
			m_battleController.DisplayUnits();

			if(m_lvEnemyLive.Items.Count == 0)
			{
				m_btnNextPlayer.Enabled = true;
			}
		}

		private void m_btnNextPlayer_Click(object sender, System.EventArgs e)
		{
			m_btnNextPlayer.Enabled = false;

			switch(m_battleController.CurrentBattle.Type)
			{
					// only one turn / player for these types
				case BattleType.KillerSatellite:
				case BattleType.Bombing:
				{
					m_battleController.NextTurn();
					m_battleController.DisplaySurvivingEnemies();
					m_btnNextBattle.Enabled = true;
					break;
				}
				case BattleType.Normal:
				{
					int index = m_battleController.BattleOrder.IndexOf(m_battleController.CurrentPlayer);

					bool anotherTurn = false;

					if( (index == (m_battleController.BattleOrder.Count - 1)))
					{
						anotherTurn = m_battleController.NextTurn();
						if(anotherTurn)
						{
							m_battleController.CurrentPlayer = (Player)m_battleController.BattleOrder[0];
							m_battleController.UpdateUnusedUnits();
							m_labCurrentPlayer.Text = m_battleController.CurrentPlayer.Name;
							m_lbCurrentPlayer.SelectedIndex = m_battleController.BattleOrder.IndexOf(m_battleController.CurrentPlayer);
						}
						else
						{
							m_btnNextBattle.Enabled = true;
						}
					}
					else
					{
						m_battleController.CurrentPlayer = (Player)m_battleController.BattleOrder[index + 1];
						m_battleController.UpdateUnusedUnits();
					}

					m_lvAttUnused.Items.Clear();
					m_lvEnemyLive.Items.Clear();
					m_battleController.DisplayUnits();
					break;
				}
			}
		}

		private void m_btnNextBattle_Click(object sender, System.EventArgs e)
		{
			/*
			int battlesToSkip = Convert.ToInt32(m_udSkipBattles.Value);

			if(battlesToSkip >= (m_battleController.Battles.Count -1))
			{
				battlesToSkip = m_battleController.Battles.Count - 1;
			}
			for(int i = 0; i < battlesToSkip; i++)
			{
				m_battleController.Battles.Remove(0);
			}
			*/

			m_lvAttUnused.Items.Clear();
			m_lvEnemyLive.Items.Clear();
			m_lvAttackers.Items.Clear();
			m_lvEnemyLive.Items.Clear();

			CombatResult cr = m_battleController.BattleResult;

			StringBuilder sb = new StringBuilder();

			foreach(Player p in cr.Casualties.GetPlayersWithUnits())
			{
				UnitCollection playerUnits = cr.Casualties.GetUnits(p);

				foreach(UnitType ut in playerUnits.GetUnitTypeCount().Keys)
				{
					UnitCollection types = playerUnits.GetUnits(ut);

					sb.Append(p.Name);
					sb.Append(" - ");
					sb.Append(ut.ToString());
					sb.Append(" - ");
					sb.Append(types.Count.ToString());
					sb.Append("\r\n");
				}
			}

			//MessageBox.Show(sb.ToString(), "Combat Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
			//MessageBox.Show()
			if(!m_battleController.NextBattle())
			{
				m_battleController.CombatComplete();
				MessageBox.Show("Combat complete");
				this.DialogResult = DialogResult.OK;
			}
			else
			{
				UpdateCombatInformation();
			}
		}

		#endregion

		private bool OnBattleControllerStatusUpdate(object sender, StatusUpdateEventArgs suea)
		{
			bool result = true;

			switch(suea.StatusInfo)
			{
				case StatusInfo.FactoryConquered:
				{
					string message = suea.Territory.Name + " has been conquered and a factory is about to be captured.  Sabotage the factory?";
					DialogResult dr = MessageBox.Show(message, "Sabotage Factory?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

					result = (dr == DialogResult.Yes);
					break;
				}
				case StatusInfo.SabotageResult:
				{
					string message = String.Empty;

					if(suea.Result)
					{
						message = "Sabotage successful!  The factory was destroyed";
					}
					else
					{
						message = "Sabotage failed.  The factory is still intact.";
					}

					MessageBox.Show(message, "Sabotage Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
					break;
				}
			}

			return result;
		}
		
		private CombatInfo SetUpCombat()
		{
			CombatInfo ci = new CombatInfo();

			UnitCollection attackers = GetListedUnits(m_lvAttackers, true);
			ci.Attackers.AddAllUnits(attackers);

			try
			{
				UnitCollection defenders = GetListedUnits(m_lvDefenders, false);
				ci.Defenders.AddAllUnits(defenders);
			}
			catch(Exception ex)
			{
				m_battleController.CurrentUnused.AddAllUnits(attackers);
				throw ex;
			}

			return ci;
		}

		private UnitCollection GetListedUnits(ListView lv, bool attacking)
		{
			UnitCollection allMatches = new UnitCollection();
			for(int i = 0; i < lv.Items.Count; i++)
			{
				ListViewItem lvi = lv.Items[i];
				Player p = m_controller.GetPlayer(lvi.Text);
				UnitType ut = (UnitType)Enum.Parse(typeof(UnitType), lvi.SubItems[1].Text);
				int numUnits = Int32.Parse(lvi.SubItems[2].Text);
				string territory = "";

				if(!attacking)
				{
					territory = lvi.SubItems[3].Text;
				}

				UnitCollection matches = null;

				// Theoretically should be at least this many units available, anyway,
				// since the list of units was based on 
				if(attacking)
				{
					matches = m_battleController.CurrentUnused.GetUnits(ut, numUnits);
					m_battleController.CurrentUnused.RemoveAllUnits(matches);
				}
				else
				{
					UnitCollection playerUnits = ((UnitCollection)m_battleController.SurvivingUnits[p]);
				
					if(ut == UnitType.Transport)
					{
						int numTransports = playerUnits.GetUnits(UnitType.Transport).Count;
						UnitCollection otherUnits = playerUnits.GetNonMatchingUnits(UnitType.Transport);
						UnitCollection otherUnitsAdded = allMatches.GetUnits(p).GetNonMatchingUnits(UnitType.Transport);

						if(otherUnits.Count != otherUnitsAdded.Count)
						{

							throw new Exception("Can't attack transports if other units are still alive");
						}
					}
				
					UnitCollection matchesType = playerUnits.GetUnits(ut);
					matchesType.RemoveAllUnits(allMatches);
				
					Territory t = m_controller.Map[territory];
					matches = matchesType.GetUnits(t, numUnits);				
				}

				allMatches.AddAllUnits(matches);
			}
			
			return allMatches;
		}
	}
}