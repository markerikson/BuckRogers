using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for CombatTestForm.
	/// </summary>
	public class CombatTestForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ListView m_lvAttUnused;
		private System.Windows.Forms.ListBox m_lbCurrentPlayer;
		private System.Windows.Forms.ListView m_lvAttUsed;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.ColumnHeader columnHeader7;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ColumnHeader columnHeader8;
		private System.Windows.Forms.ColumnHeader columnHeader9;
		private System.Windows.Forms.ColumnHeader columnHeader10;
		private System.Windows.Forms.ListView m_lvEnemyLive;
		private System.Windows.Forms.ListView m_lvEnemyDead;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ListView m_lvAttackers;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ListView m_lvDefenders;
		private System.Windows.Forms.ColumnHeader columnHeader11;
		private System.Windows.Forms.ColumnHeader columnHeader12;
		private System.Windows.Forms.ColumnHeader columnHeader13;
		private System.Windows.Forms.ColumnHeader columnHeader14;
		private System.Windows.Forms.ColumnHeader columnHeader15;
		private System.Windows.Forms.ColumnHeader columnHeader16;
		private System.Windows.Forms.ColumnHeader columnHeader17;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox m_cbNumUnits;
		private System.Windows.Forms.Button m_butAddAttackers;
		private System.Windows.Forms.Button m_butRemAttackers;
		private System.Windows.Forms.Button m_butRemDefenders;
		private System.Windows.Forms.Button m_butAddDefenders;
		private System.Windows.Forms.Label m_labCurrentPlayer;

		private GameController m_controller;
		private Hashlist m_battles;
		private BattleInfo m_currentBattle;
		private CombatResult m_lastResult;
		private CombatResult m_turnResult;
		private CombatResult m_cumulativeResult;
		private ArrayList m_playerOrder;
		private Hashtable m_survivingUnits;
		private UnitCollection m_currentUnused;
		private Player m_currentPlayer;

		private System.Windows.Forms.ColumnHeader columnHeader18;
		private System.Windows.Forms.ColumnHeader columnHeader19;
		private System.Windows.Forms.ColumnHeader columnHeader20;
		private System.Windows.Forms.ColumnHeader columnHeader21;
		private System.Windows.Forms.ColumnHeader columnHeader22;
		private System.Windows.Forms.Button m_btnAttack;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label m_labBattleType;
		private System.Windows.Forms.Label m_labLocation;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.ListView m_lvResults;
		private System.Windows.Forms.Button m_btnContinue;
		private System.Windows.Forms.Button m_btnNextPlayer;
		private System.Windows.Forms.Button m_btnNextBattle;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CombatTestForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			m_cbNumUnits.SelectedIndex = 0;
			//m_cbNumUnits.DropDownStyle = ComboBoxStyle.DropDownList;

			this.Init();
			this.SetupBattles();
			this.Battles = m_controller.Battles;

			m_controller.Twister.Initialize(42);
			
			NextBattle();

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

		[STAThread]
		public static void Main(String[] args)
		{
			Application.Run(new CombatTestForm());
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
																													 "Trooper",
																													 "15"}, -1);
			System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem(new string[] {
																													 "Gennie",
																													 "5"}, -1);
			System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem(new string[] {
																													 "Fighter",
																													 "10"}, -1);
			System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem(new string[] {
																													 "Transport",
																													 "5"}, -1);
			System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem(new string[] {
																													  "Marker",
																													  "1"}, -1);
			System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem(new string[] {
																													  "Jake",
																													  "Fighter",
																													  "1"}, -1);
			System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem(new string[] {
																													  "Jake",
																													  "Transport",
																													  "100"}, -1);
			System.Windows.Forms.ListViewItem listViewItem13 = new System.Windows.Forms.ListViewItem(new string[] {
																													  "Stu",
																													  "Trooper",
																													  "8"}, -1);
			this.m_lvAttUnused = new System.Windows.Forms.ListView();
			this.columnHeader11 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.label1 = new System.Windows.Forms.Label();
			this.m_lbCurrentPlayer = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.m_lvAttUsed = new System.Windows.Forms.ListView();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.m_lvEnemyLive = new System.Windows.Forms.ListView();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.m_lvEnemyDead = new System.Windows.Forms.ListView();
			this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
			this.m_lvAttackers = new System.Windows.Forms.ListView();
			this.columnHeader12 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader13 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader14 = new System.Windows.Forms.ColumnHeader();
			this.label6 = new System.Windows.Forms.Label();
			this.m_cbNumUnits = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.m_lvDefenders = new System.Windows.Forms.ListView();
			this.columnHeader15 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader16 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader17 = new System.Windows.Forms.ColumnHeader();
			this.m_butAddAttackers = new System.Windows.Forms.Button();
			this.m_butRemAttackers = new System.Windows.Forms.Button();
			this.m_butRemDefenders = new System.Windows.Forms.Button();
			this.m_butAddDefenders = new System.Windows.Forms.Button();
			this.label9 = new System.Windows.Forms.Label();
			this.m_labCurrentPlayer = new System.Windows.Forms.Label();
			this.m_lvResults = new System.Windows.Forms.ListView();
			this.columnHeader18 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader19 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader22 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader20 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader21 = new System.Windows.Forms.ColumnHeader();
			this.m_btnContinue = new System.Windows.Forms.Button();
			this.m_btnAttack = new System.Windows.Forms.Button();
			this.m_labBattleType = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.m_labLocation = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.m_btnNextPlayer = new System.Windows.Forms.Button();
			this.m_btnNextBattle = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// m_lvAttUnused
			// 
			this.m_lvAttUnused.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							this.columnHeader11,
																							this.columnHeader1,
																							this.columnHeader2});
			this.m_lvAttUnused.FullRowSelect = true;
			this.m_lvAttUnused.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
																						  listViewItem1,
																						  listViewItem2,
																						  listViewItem3,
																						  listViewItem4,
																						  listViewItem5});
			this.m_lvAttUnused.Location = new System.Drawing.Point(4, 68);
			this.m_lvAttUnused.MultiSelect = false;
			this.m_lvAttUnused.Name = "m_lvAttUnused";
			this.m_lvAttUnused.Size = new System.Drawing.Size(212, 136);
			this.m_lvAttUnused.TabIndex = 0;
			this.m_lvAttUnused.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader11
			// 
			this.columnHeader11.Text = "Player";
			this.columnHeader11.Width = 80;
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
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(256, 524);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(84, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Current Player:";
			// 
			// m_lbCurrentPlayer
			// 
			this.m_lbCurrentPlayer.ItemHeight = 14;
			this.m_lbCurrentPlayer.Items.AddRange(new object[] {
																   "Mark",
																   "Chris",
																   "Stu",
																   "Hannah",
																   "Jake",
																   "Kathryn",
																   "An extremely long name"});
			this.m_lbCurrentPlayer.Location = new System.Drawing.Point(256, 544);
			this.m_lbCurrentPlayer.Name = "m_lbCurrentPlayer";
			this.m_lbCurrentPlayer.Size = new System.Drawing.Size(52, 18);
			this.m_lbCurrentPlayer.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(4, 52);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Unused Units:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(92, 532);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 16);
			this.label3.TabIndex = 5;
			this.label3.Text = "Used Units";
			// 
			// m_lvAttUsed
			// 
			this.m_lvAttUsed.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						  this.columnHeader3,
																						  this.columnHeader4});
			this.m_lvAttUsed.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
																						listViewItem6,
																						listViewItem7,
																						listViewItem8,
																						listViewItem9,
																						listViewItem10});
			this.m_lvAttUsed.Location = new System.Drawing.Point(92, 552);
			this.m_lvAttUsed.Name = "m_lvAttUsed";
			this.m_lvAttUsed.Size = new System.Drawing.Size(52, 36);
			this.m_lvAttUsed.TabIndex = 4;
			this.m_lvAttUsed.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Type";
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Count";
			this.columnHeader4.Width = 50;
			// 
			// m_lvEnemyLive
			// 
			this.m_lvEnemyLive.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							this.columnHeader5,
																							this.columnHeader6,
																							this.columnHeader7});
			this.m_lvEnemyLive.FullRowSelect = true;
			this.m_lvEnemyLive.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
																						  listViewItem11,
																						  listViewItem12,
																						  listViewItem13});
			this.m_lvEnemyLive.Location = new System.Drawing.Point(4, 236);
			this.m_lvEnemyLive.MultiSelect = false;
			this.m_lvEnemyLive.Name = "m_lvEnemyLive";
			this.m_lvEnemyLive.Size = new System.Drawing.Size(212, 180);
			this.m_lvEnemyLive.TabIndex = 6;
			this.m_lvEnemyLive.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Player";
			this.columnHeader5.Width = 80;
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "Type";
			// 
			// columnHeader7
			// 
			this.columnHeader7.Text = "Count";
			this.columnHeader7.Width = 45;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(4, 220);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(124, 16);
			this.label4.TabIndex = 7;
			this.label4.Text = "Enemy Units (Live):";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(188, 528);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(48, 16);
			this.label5.TabIndex = 9;
			this.label5.Text = "Enemy Units (Dead):";
			// 
			// m_lvEnemyDead
			// 
			this.m_lvEnemyDead.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							this.columnHeader8,
																							this.columnHeader9,
																							this.columnHeader10});
			this.m_lvEnemyDead.Location = new System.Drawing.Point(188, 544);
			this.m_lvEnemyDead.Name = "m_lvEnemyDead";
			this.m_lvEnemyDead.Size = new System.Drawing.Size(52, 44);
			this.m_lvEnemyDead.TabIndex = 8;
			this.m_lvEnemyDead.View = System.Windows.Forms.View.Details;
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
			// m_lvAttackers
			// 
			this.m_lvAttackers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							this.columnHeader12,
																							this.columnHeader13,
																							this.columnHeader14});
			this.m_lvAttackers.FullRowSelect = true;
			this.m_lvAttackers.Location = new System.Drawing.Point(356, 68);
			this.m_lvAttackers.MultiSelect = false;
			this.m_lvAttackers.Name = "m_lvAttackers";
			this.m_lvAttackers.Size = new System.Drawing.Size(212, 136);
			this.m_lvAttackers.TabIndex = 10;
			this.m_lvAttackers.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader12
			// 
			this.columnHeader12.Text = "Player";
			this.columnHeader12.Width = 80;
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
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(360, 52);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(100, 16);
			this.label6.TabIndex = 11;
			this.label6.Text = "Attacking units:";
			// 
			// m_cbNumUnits
			// 
			this.m_cbNumUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_cbNumUnits.Items.AddRange(new object[] {
															  "1",
															  "5",
															  "10",
															  "25",
															  "100"});
			this.m_cbNumUnits.Location = new System.Drawing.Point(228, 208);
			this.m_cbNumUnits.Name = "m_cbNumUnits";
			this.m_cbNumUnits.Size = new System.Drawing.Size(116, 22);
			this.m_cbNumUnits.TabIndex = 12;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(228, 192);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(116, 16);
			this.label7.TabIndex = 13;
			this.label7.Text = "Units to add / remove:";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(356, 220);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(100, 16);
			this.label8.TabIndex = 15;
			this.label8.Text = "Defending units:";
			// 
			// m_lvDefenders
			// 
			this.m_lvDefenders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							this.columnHeader15,
																							this.columnHeader16,
																							this.columnHeader17});
			this.m_lvDefenders.FullRowSelect = true;
			this.m_lvDefenders.Location = new System.Drawing.Point(356, 236);
			this.m_lvDefenders.MultiSelect = false;
			this.m_lvDefenders.Name = "m_lvDefenders";
			this.m_lvDefenders.Size = new System.Drawing.Size(212, 180);
			this.m_lvDefenders.TabIndex = 14;
			this.m_lvDefenders.View = System.Windows.Forms.View.Details;
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
			// m_butAddAttackers
			// 
			this.m_butAddAttackers.Location = new System.Drawing.Point(248, 100);
			this.m_butAddAttackers.Name = "m_butAddAttackers";
			this.m_butAddAttackers.TabIndex = 16;
			this.m_butAddAttackers.Text = "Add >>";
			this.m_butAddAttackers.Click += new System.EventHandler(this.m_butAddAttackers_Click);
			// 
			// m_butRemAttackers
			// 
			this.m_butRemAttackers.Location = new System.Drawing.Point(248, 132);
			this.m_butRemAttackers.Name = "m_butRemAttackers";
			this.m_butRemAttackers.TabIndex = 17;
			this.m_butRemAttackers.Text = "<< Remove";
			this.m_butRemAttackers.Click += new System.EventHandler(this.m_butRemAttackers_Click);
			// 
			// m_butRemDefenders
			// 
			this.m_butRemDefenders.Location = new System.Drawing.Point(248, 324);
			this.m_butRemDefenders.Name = "m_butRemDefenders";
			this.m_butRemDefenders.TabIndex = 19;
			this.m_butRemDefenders.Text = "<< Remove";
			this.m_butRemDefenders.Click += new System.EventHandler(this.m_butRemDefenders_Click);
			// 
			// m_butAddDefenders
			// 
			this.m_butAddDefenders.Location = new System.Drawing.Point(248, 292);
			this.m_butAddDefenders.Name = "m_butAddDefenders";
			this.m_butAddDefenders.TabIndex = 18;
			this.m_butAddDefenders.Text = "Add >>";
			this.m_butAddDefenders.Click += new System.EventHandler(this.m_butAddDefenders_Click);
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label9.Location = new System.Drawing.Point(576, 4);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(120, 23);
			this.label9.TabIndex = 20;
			this.label9.Text = "Current Player:";
			// 
			// m_labCurrentPlayer
			// 
			this.m_labCurrentPlayer.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_labCurrentPlayer.Location = new System.Drawing.Point(704, 4);
			this.m_labCurrentPlayer.Name = "m_labCurrentPlayer";
			this.m_labCurrentPlayer.Size = new System.Drawing.Size(128, 23);
			this.m_labCurrentPlayer.TabIndex = 21;
			this.m_labCurrentPlayer.Text = "Current Player:";
			// 
			// m_lvResults
			// 
			this.m_lvResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						  this.columnHeader18,
																						  this.columnHeader19,
																						  this.columnHeader22,
																						  this.columnHeader20,
																						  this.columnHeader21});
			this.m_lvResults.Location = new System.Drawing.Point(580, 68);
			this.m_lvResults.Name = "m_lvResults";
			this.m_lvResults.Size = new System.Drawing.Size(292, 348);
			this.m_lvResults.TabIndex = 22;
			this.m_lvResults.View = System.Windows.Forms.View.Details;
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
			this.columnHeader21.Text = "Hit";
			this.columnHeader21.Width = 35;
			// 
			// m_btnContinue
			// 
			this.m_btnContinue.Location = new System.Drawing.Point(120, 432);
			this.m_btnContinue.Name = "m_btnContinue";
			this.m_btnContinue.TabIndex = 23;
			this.m_btnContinue.Text = "Continue";
			this.m_btnContinue.Click += new System.EventHandler(this.m_btnContinue_Click);
			// 
			// m_btnAttack
			// 
			this.m_btnAttack.Location = new System.Drawing.Point(36, 432);
			this.m_btnAttack.Name = "m_btnAttack";
			this.m_btnAttack.TabIndex = 24;
			this.m_btnAttack.Text = "Attack";
			this.m_btnAttack.Click += new System.EventHandler(this.m_btnAttack_Click);
			// 
			// m_labBattleType
			// 
			this.m_labBattleType.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_labBattleType.Location = new System.Drawing.Point(108, 4);
			this.m_labBattleType.Name = "m_labBattleType";
			this.m_labBattleType.Size = new System.Drawing.Size(128, 23);
			this.m_labBattleType.TabIndex = 26;
			this.m_labBattleType.Text = "Current Player:";
			// 
			// label11
			// 
			this.label11.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label11.Location = new System.Drawing.Point(4, 4);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(96, 23);
			this.label11.TabIndex = 25;
			this.label11.Text = "Battle Type:";
			// 
			// m_labLocation
			// 
			this.m_labLocation.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_labLocation.Location = new System.Drawing.Point(344, 4);
			this.m_labLocation.Name = "m_labLocation";
			this.m_labLocation.Size = new System.Drawing.Size(208, 23);
			this.m_labLocation.TabIndex = 28;
			this.m_labLocation.Text = "Current Player:";
			// 
			// label12
			// 
			this.label12.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label12.Location = new System.Drawing.Point(256, 4);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(76, 23);
			this.label12.TabIndex = 27;
			this.label12.Text = "Location:";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(580, 52);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(100, 16);
			this.label10.TabIndex = 29;
			this.label10.Text = "Combat results:";
			// 
			// m_btnNextPlayer
			// 
			this.m_btnNextPlayer.Location = new System.Drawing.Point(208, 432);
			this.m_btnNextPlayer.Name = "m_btnNextPlayer";
			this.m_btnNextPlayer.TabIndex = 30;
			this.m_btnNextPlayer.Text = "Next player";
			this.m_btnNextPlayer.Click += new System.EventHandler(this.m_btnNextPlayer_Click);
			// 
			// m_btnNextBattle
			// 
			this.m_btnNextBattle.Location = new System.Drawing.Point(296, 432);
			this.m_btnNextBattle.Name = "m_btnNextBattle";
			this.m_btnNextBattle.TabIndex = 31;
			this.m_btnNextBattle.Text = "Next battle";
			this.m_btnNextBattle.Click += new System.EventHandler(this.m_btnNextBattle_Click);
			// 
			// CombatTestForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(992, 666);
			this.Controls.Add(this.m_btnNextBattle);
			this.Controls.Add(this.m_btnNextPlayer);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.m_labLocation);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.m_labBattleType);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.m_btnAttack);
			this.Controls.Add(this.m_btnContinue);
			this.Controls.Add(this.m_lvResults);
			this.Controls.Add(this.m_labCurrentPlayer);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.m_butRemDefenders);
			this.Controls.Add(this.m_butAddDefenders);
			this.Controls.Add(this.m_butRemAttackers);
			this.Controls.Add(this.m_butAddAttackers);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.m_lvDefenders);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.m_cbNumUnits);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.m_lvAttackers);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.m_lvEnemyDead);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.m_lvEnemyLive);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.m_lvAttUsed);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.m_lbCurrentPlayer);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.m_lvAttUnused);
			this.Name = "CombatTestForm";
			this.Text = "CombatTestForm";
			this.ResumeLayout(false);

		}
		#endregion


		private void EnableAttack()
		{
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
			MoveItems(m_lvAttUnused, m_lvAttackers);

			EnableAttack();

		}

		private void MoveItems(ListView origin, ListView destination)
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

				if(liName == name && liType == type)
				{
					addNewItem = false;
					int numCurrent = Int32.Parse(lastItem.SubItems[2].Text);
					numCurrent += numToMove;

					lastItem.SubItems[2].Text = numCurrent.ToString();

				}
			}
			
			if(addNewItem)
			{
				ListViewItem lvi2 = new ListViewItem();
				lvi2.Text = name;
				lvi2.SubItems.Add(type);
				lvi2.SubItems.Add(numToMove.ToString());

				destination.Items.Add(lvi2);
			}

			
		}

		private void m_butRemAttackers_Click(object sender, System.EventArgs e)
		{
			MoveItems(m_lvAttackers, m_lvAttUnused);

			EnableAttack();
		}

		private void m_butAddDefenders_Click(object sender, System.EventArgs e)
		{
			MoveItems(m_lvEnemyLive, m_lvDefenders);

			EnableAttack();
		}

		private void m_butRemDefenders_Click(object sender, System.EventArgs e)
		{
			MoveItems(m_lvDefenders, m_lvEnemyLive);

			EnableAttack();
		}

		public void Init()
		{
			string[] players = {"Mark", "Chris", "Stu", "Hannah", "Jake", "Kathryn"};
			m_controller = new GameController(players);

			m_controller.AssignTerritories();
			m_controller.CreateInitialUnits();

			m_controller.RollForInitiative(false);

			string[,] territories = { {"Deimos", "Psyche", "Urban Reservations", "Space Elevator", 
										  "Thule", "Aurora", "Tolstoi"},
										{
											"Mt. Maxwell", "Moscoviense", "Tycho", "Aerostates", "Mariposas", 
											"African Regency", "Boreal Sea"},
										{
											"Pallas", "Eurasian Regency", "Fortuna", "Coprates Chasm", "Hielo", 
											"Sobkou Plains", "The Warrens"},
										{
											"Lowlanders", "L-4 Colony", "Hygeia", "Wreckers", "Beta Regio", 
											"L-5 Colony", "Arcadia"},
										{
											"Juno", "Independent Arcologies", "Tranquility", "Vesta", "American Regency", 
											"Ceres", "Pavonis"},
										{
											"Ram HQ", "Bach", "Elysium", "Antarctic Testing Zone", "Aphrodite", 
											"Australian Development Facility", "Farside"},
			};

			ArrayList al = new ArrayList();
			for (int i = 0; i < players.Length; i++)
			{
				for (int j = 0; j < territories.GetLength(1); j++)
				{
					Territory t = m_controller.Map[territories[i, j]];
					t.Owner = m_controller.Players[i];
					al.Add(t);
				}
			}

			#region Mark's unit setup

			Player mark = m_controller.Players[0];

			UnitCollection units = mark.Units;
			
			Territory thule = m_controller.Map["Thule"];
			Territory elevator = m_controller.Map["Space Elevator"];
			Territory deimos = m_controller.Map["Deimos"];

			UnitCollection uc = units.GetUnits(UnitType.Fighter);

			for (int i = 0; i < 4; i++)
			{
				uc[i].CurrentTerritory = deimos;
			}

			uc = units.GetUnits(UnitType.Leader);
			uc[0].CurrentTerritory = thule;

			uc = mark.Units.GetUnits(UnitType.Transport);
			uc[0].CurrentTerritory = thule;

			uc = units.GetUnits(UnitType.Factory);
			uc[0].CurrentTerritory = deimos;
			uc[1].CurrentTerritory = elevator;

			uc = units.GetUnits(UnitType.Gennie);
			uc[0].CurrentTerritory = elevator;
			uc[1].CurrentTerritory = elevator;


			uc = units.GetUnits(UnitType.Trooper);
			uc[0].CurrentTerritory = deimos;

			for(int i = 1; i < 4; i++)
			{
				uc[i].CurrentTerritory = elevator;
			}

			for(int i = 4; i < 8; i++)
			{
				uc[i].CurrentTerritory = thule;
			}

			#endregion

			#region Chris's unit setup

			Territory africa = m_controller.Map["African Regency"];
			Territory mosco = m_controller.Map["Moscoviense"];
			Territory tycho = m_controller.Map["Tycho"];

			Player chris = m_controller.Players[1];
			units = chris.Units;

			uc = units.GetUnits(UnitType.Leader);
			uc[0].CurrentTerritory = africa;

			uc = units.GetUnits(UnitType.Gennie);
			uc[0].CurrentTerritory = mosco;
			uc[1].CurrentTerritory = tycho;

			uc = units.GetUnits(UnitType.Factory);
			uc[0].CurrentTerritory = mosco;
			uc[1].CurrentTerritory = tycho;

			uc = units.GetUnits(UnitType.Transport);
			uc[0].CurrentTerritory = africa;

			uc = units.GetUnits(UnitType.Trooper);

			for(int i = 0; i < 4; i++)
			{
				uc[i].CurrentTerritory = tycho;
			}

			for(int i = 4; i < 8; i++)
			{
				uc[i].CurrentTerritory = mosco;
			}

			uc = units.GetUnits(UnitType.Fighter);

			for(int i = 0; i < 4; i++)
			{
				uc[i].CurrentTerritory = africa;
			}

			#endregion

			#region Stu's unit setup

			Territory hielo = m_controller.Map["Hielo"];
			Territory warrens = m_controller.Map["The Warrens"];
			Territory sobkou = m_controller.Map["Sobkou Plains"];

			Player stu = m_controller.Players[2];
			units = stu.Units;

			uc = units.GetUnits(UnitType.Leader);
			uc[0].CurrentTerritory = hielo;

			uc = units.GetUnits(UnitType.Gennie);
			uc[0].CurrentTerritory = warrens;
			uc[1].CurrentTerritory = sobkou;

			uc = units.GetUnits(UnitType.Factory);
			uc[0].CurrentTerritory = warrens;
			uc[1].CurrentTerritory = sobkou;

			uc = units.GetUnits(UnitType.Transport);
			uc[0].CurrentTerritory = hielo;

			uc = units.GetUnits(UnitType.Trooper);

			for(int i = 0; i < 4; i++)
			{
				uc[i].CurrentTerritory = sobkou;
			}

			for(int i = 4; i < 8; i++)
			{
				uc[i].CurrentTerritory = warrens;
			}

			uc = units.GetUnits(UnitType.Fighter);

			for(int i = 0; i < 4; i++)
			{
				uc[i].CurrentTerritory = hielo;
			}

			#endregion

			#region Hannah's unit setup

			Territory wreckers = m_controller.Map["Wreckers"];
			Territory beta = m_controller.Map["Beta Regio"];
			Territory lowlanders = m_controller.Map["Lowlanders"];

			Player hannah = m_controller.Players[3];
			units = hannah.Units;

			uc = units.GetUnits(UnitType.Leader);
			uc[0].CurrentTerritory = wreckers;

			uc = units.GetUnits(UnitType.Gennie);
			uc[0].CurrentTerritory = beta;
			uc[1].CurrentTerritory = lowlanders;

			uc = units.GetUnits(UnitType.Factory);
			uc[0].CurrentTerritory = beta;
			uc[1].CurrentTerritory = lowlanders;

			uc = units.GetUnits(UnitType.Transport);
			uc[0].CurrentTerritory = wreckers;

			uc = units.GetUnits(UnitType.Trooper);

			for(int i = 0; i < 4; i++)
			{
				uc[i].CurrentTerritory = lowlanders;
			}

			for(int i = 4; i < 8; i++)
			{
				uc[i].CurrentTerritory = beta;
			}

			uc = units.GetUnits(UnitType.Fighter);

			for(int i = 0; i < 4; i++)
			{
				uc[i].CurrentTerritory = wreckers;
			}

			#endregion

			#region Jake's unit setup

			Territory tranquility = m_controller.Map["Tranquility"];
			Territory arcologies = m_controller.Map["Independent Arcologies"];
			Territory america = m_controller.Map["American Regency"];

			Player jake = m_controller.Players[4];
			units = jake.Units;

			uc = units.GetUnits(UnitType.Leader);
			uc[0].CurrentTerritory = tranquility;

			uc = units.GetUnits(UnitType.Gennie);
			uc[0].CurrentTerritory = arcologies;
			uc[1].CurrentTerritory = america;

			uc = units.GetUnits(UnitType.Factory);
			uc[0].CurrentTerritory = arcologies;
			uc[1].CurrentTerritory = america;

			uc = units.GetUnits(UnitType.Transport);
			uc[0].CurrentTerritory = tranquility;

			uc = units.GetUnits(UnitType.Trooper);

			for(int i = 0; i < 4; i++)
			{
				uc[i].CurrentTerritory = america;
			}

			for(int i = 4; i < 8; i++)
			{
				uc[i].CurrentTerritory = arcologies;
			}

			uc = units.GetUnits(UnitType.Fighter);

			for(int i = 0; i < 4; i++)
			{
				uc[i].CurrentTerritory = tranquility;
			}

			#endregion

			#region Kathryn's unit setup

			Territory farside = m_controller.Map["Farside"];
			Territory antarctica = m_controller.Map["Antarctic Testing Zone"];
			Territory australia = m_controller.Map["Australian Development Facility"];

			Player kathryn = m_controller.Players[5];
			units = kathryn.Units;

			uc = units.GetUnits(UnitType.Leader);
			uc[0].CurrentTerritory = farside;

			uc = units.GetUnits(UnitType.Gennie);
			uc[0].CurrentTerritory = antarctica;
			uc[1].CurrentTerritory = australia;

			uc = units.GetUnits(UnitType.Factory);
			uc[0].CurrentTerritory = antarctica;
			uc[1].CurrentTerritory = australia;

			uc = units.GetUnits(UnitType.Transport);
			uc[0].CurrentTerritory = farside;

			uc = units.GetUnits(UnitType.Trooper);

			for(int i = 0; i < 4; i++)
			{
				uc[i].CurrentTerritory = australia;
			}

			for(int i = 4; i < 8; i++)
			{
				uc[i].CurrentTerritory = antarctica;
			}

			uc = units.GetUnits(UnitType.Fighter);

			for(int i = 0; i < 4; i++)
			{
				uc[i].CurrentTerritory = farside;
			}

			#endregion


			

		}


		public void SetupBattles()
		{
			Territory africa = m_controller.Map["African Regency"];
			Territory antarctica = m_controller.Map["Antarctic Testing Zone"];
			UnitCollection uc = africa.Units.GetUnits(UnitType.Fighter);
			
			MoveAction ma = new MoveAction();
			ma.Owner = africa.Owner;
			ma.StartingTerritory = africa;
			ma.Territories.Add(antarctica);
			ma.Units.AddAllUnits(uc);

			m_controller.AddAction(ma);


			Territory farside = m_controller.Map["Farside"];
			uc = farside.Units.GetUnits(UnitType.Fighter);

			ma = new MoveAction();
			ma.Owner = farside.Owner;
			ma.StartingTerritory = farside;
			ma.Units.AddAllUnits(uc);
			ma.Territories.Add(m_controller.Map["Near Moon Orbit"]);
			ma.Territories.Add(m_controller.Map["Near Earth Orbit"]);
			Territory arcologies = m_controller.Map["Independent Arcologies"];
			ma.Territories.Add(arcologies);

			m_controller.AddAction(ma);

			Territory tranquility = m_controller.Map["Tranquility"];
			uc = tranquility.Units;

			ma = new MoveAction();
			ma.Owner = tranquility.Owner;
			ma.StartingTerritory = tranquility;
			ma.Units.AddAllUnits(uc);
			Territory moscoviense = m_controller.Map["Moscoviense"];
			ma.Territories.Add(moscoviense);

			m_controller.AddAction(ma);

			Territory mercuryOrbit1 = m_controller.Map["Mercury Orbit: 1"];
			Territory transMarsOrbit15 = m_controller.Map["Trans-Mars Orbit: 15"];

			UnitCollection uc2 = m_controller.Map["Deimos"].Units.GetUnits(UnitType.Fighter);
			UnitCollection uc3 = m_controller.Map["Hielo"].Units.GetUnits(UnitType.Fighter);

			uc2[0].CurrentTerritory = mercuryOrbit1;
			uc2[1].CurrentTerritory = transMarsOrbit15;

			uc3[0].CurrentTerritory = mercuryOrbit1;
			uc3[1].CurrentTerritory = transMarsOrbit15;

			Player stu = m_controller.GetPlayer("Stu");
			Planet mercury = (Planet)m_controller.Map.Planets["Mercury"];
			foreach(Territory t in mercury.Surface)
			{
				t.Owner = stu;
			}
			mercury.CheckControl();
			Unit satellite = Unit.CreateNewUnit(stu, UnitType.KillerSatellite);
			satellite.CurrentTerritory = mercury.NearOrbit;
			
			Player mark = m_controller.GetPlayer("Mark");
			for(int i = 0; i < 5; i++)
			{
				Unit battler = Unit.CreateNewUnit(mark, UnitType.Battler);
				battler.CurrentTerritory = mercury.NearOrbit;
			}
			
			for(int i = 0; i < 5; i++)
			{
				Unit fighter = Unit.CreateNewUnit(mark, UnitType.Fighter);
				fighter.CurrentTerritory = mercury.NearOrbit;
			}

			Unit battler2 = Unit.CreateNewUnit(mark, UnitType.Battler);
			Planet venus = (Planet)m_controller.Map.Planets["Venus"];
			battler2.CurrentTerritory = venus.NearOrbit;

			Unit battler3 = Unit.CreateNewUnit(mark, UnitType.Battler);
			Planet earth = (Planet)m_controller.Map.Planets["Earth"];
			battler3.CurrentTerritory = earth.NearOrbit;

			Player chris = m_controller.GetPlayer("Chris");
			Unit battler4 = Unit.CreateNewUnit(chris, UnitType.Battler);
			Planet mars = (Planet)m_controller.Map.Planets["Mars"];
			battler4.CurrentTerritory = mars.FarOrbit;

			Unit battler5 = Unit.CreateNewUnit(chris, UnitType.Battler);
			battler5.CurrentTerritory = earth.NearOrbit;

			Hashlist battles = m_controller.FindBattles();


			Territory[] battleSites = {mercury.NearOrbit, mercury.NearOrbit, mercury.NearOrbit, mercuryOrbit1,
										  venus.NearOrbit, earth.NearOrbit, earth.NearOrbit, earth.NearOrbit, antarctica, 
										  arcologies, moscoviense,  mars.FarOrbit, transMarsOrbit15};
			BattleType[] battleTypes = {BattleType.KillerSatellite, BattleType.Bombing, BattleType.Normal, BattleType.Normal,
										   BattleType.Bombing, BattleType.Bombing, BattleType.Bombing, BattleType.Normal, 
										   BattleType.Normal, 
										   BattleType.Normal, BattleType.Normal, BattleType.Bombing, BattleType.Normal};
			//foreach(BattleInfo bi in battles)
			for(int i = 0; i < battles.Count; i++)
			{
				BattleInfo bi = (BattleInfo)battles[i];
				//Console.WriteLine(bi.ToString());
				//Assert.AreEqual(battleSites[i], bi.Territory);
				//Assert.AreEqual(battleTypes[i], bi.Type);
			}
		}


		private bool NextBattle()
		{
			if(m_battles != null && m_battles.Count > 0)
			{
				m_currentBattle = (BattleInfo)m_battles[0];
				m_battles.Remove(m_currentBattle.ToString());

				m_labBattleType.Text = m_currentBattle.Type.ToString();
				
				m_labLocation.Text = m_currentBattle.Territory.Name;

				m_btnContinue.Enabled = false;
				m_btnNextPlayer.Enabled = false;
				m_btnNextBattle.Enabled = false;
				m_btnAttack.Enabled = false;

				m_lvAttUsed.Items.Clear();
				m_lvAttUnused.Items.Clear();
				m_lvAttackers.Items.Clear();
				m_lvDefenders.Items.Clear();
				m_lvEnemyLive.Items.Clear();
				m_lvEnemyDead.Items.Clear();

				m_cumulativeResult = new CombatResult();
				m_turnResult = new CombatResult();
				
				m_survivingUnits = new Hashtable();

				CheckPlayerOrder();

				m_currentPlayer = (Player)m_playerOrder[0];
				m_labCurrentPlayer.Text = m_currentPlayer.Name;
				//m_currentUnused = (UnitCollection)m_survivingUnits[m_currentPlayer];
				m_currentUnused = new UnitCollection();
				
				
				Territory t = m_currentBattle.Territory;

				switch(m_currentBattle.Type)
				{
					case BattleType.KillerSatellite:
					{
						UnitCollection satellites = t.Units.GetUnits(UnitType.KillerSatellite);
						m_currentUnused.AddAllUnits(satellites);
						UnitCollection defenders = t.Units.GetOtherPlayersUnits(m_currentBattle.Player);

						UnitCollection uc = null;

						uc = (UnitCollection)m_survivingUnits[m_currentPlayer];
						uc.AddAllUnits(satellites);

						foreach(Player p in defenders.GetPlayersWithUnits())
						{
							uc = (UnitCollection)m_survivingUnits[p];
							uc.AddAllUnits(defenders.GetUnits(p));
						}

						AddUnitsToListView(satellites, m_lvAttackers);
						AddUnitsToListView(defenders, m_lvDefenders);

						EnableAttack();
						
						//m_currentPlayer = satellites[0].Owner;
						//m_playerOrder.Clear();
						//m_playerOrder.Add(m_currentPlayer);

						break;
					}
					case BattleType.Bombing:
					{
						// Bombing attacks only have one player
						//m_currentPlayer = m_currentBattle.Player;
						//m_playerOrder.Clear();
						//m_playerOrder.Add(m_currentPlayer);

						UnitCollection attackers = t.Units.GetUnits(m_currentPlayer).GetUnits(UnitType.Battler);
						//UnitCollection defenders = t.Units.GetOtherPlayersUnits(m_currentPlayer).GetCombatUnits();
						UnitCollection defenders = m_controller.GetBombingTargets(t, m_currentPlayer);

						m_currentUnused.AddAllUnits(attackers);

						UnitCollection uc = null;

						uc = (UnitCollection)m_survivingUnits[m_currentPlayer];
						uc.AddAllUnits(attackers);

						foreach(Player p in defenders.GetPlayersWithUnits())
						{
							uc = (UnitCollection)m_survivingUnits[p];
							UnitCollection playerUnits = defenders.GetUnits(p);
							uc.AddAllUnits(playerUnits);
						}

						AddUnitsToListView(attackers, m_lvAttUnused);
						AddUnitsToListView(defenders, m_lvEnemyLive);
						break;
					}
					case BattleType.Normal:
					{
						foreach(Player p in m_playerOrder)
						{
							UnitCollection uc = (UnitCollection)m_survivingUnits[p];
							UnitCollection playerUnits = t.Units.GetUnits(p);
							uc.AddAllUnits(playerUnits.GetCombatUnits());
						}

						UpdateUnusedUnits();
						DisplayUnits();
						break;
					}
				}
				return true;
			}

			return false;
		}

		private void CheckPlayerOrder()
		{
			m_playerOrder = new ArrayList();
			ArrayList al;
			
			if(m_currentBattle.Type == BattleType.Bombing)
			{
				al = m_controller.GetBombingTargets(m_currentBattle.Territory, m_currentBattle.Player).GetPlayersWithUnits();
				al.Add(m_currentBattle.Player);
			}
			else
			{
				al = m_currentBattle.Territory.Units.GetPlayersWithUnits();
			}
						
			//CombatResult finalResult = new CombatResult();
			//CombatResult turnResult = new CombatResult();

			foreach(Player p in m_controller.PlayerOrder)
			{
				if(al.Contains(p))
				{
					m_playerOrder.Add(p);
					m_survivingUnits[p] = new UnitCollection();
				}
			}

			switch(m_currentBattle.Type)
			{
				case BattleType.KillerSatellite:
				case BattleType.Bombing:
				{
					Player p = m_currentBattle.Player;
					m_playerOrder.Remove(p);
					m_playerOrder.Insert(0, p);
					break;
				}
			}
		}

		private void AddUnitsToListView(UnitCollection uc, ListView lv)
		{
			foreach(Player p in uc.GetPlayersWithUnits())
			{
				UnitCollection playerUnits = uc.GetUnits(p);
				UnitCollection combatUnits = new UnitCollection();
				if(m_currentBattle.Type == BattleType.Bombing)
				{
					combatUnits.AddAllUnits(playerUnits);
				}
				else
				{
					combatUnits.AddAllUnits(uc.GetCombatUnits());
				}
				

				//playerUnits.
				foreach(UnitType ut in combatUnits.GetUnitTypeCount().Keys)//Enum.GetValues(typeof(UnitType)))
				{
					
					UnitCollection subgroup = playerUnits.GetUnits(ut);

					ListViewItem lvi = new ListViewItem();
					lvi.Text = p.Name;
					lvi.SubItems.Add(ut.ToString());
					lvi.SubItems.Add(subgroup.Count.ToString());
					
					lv.Items.Add(lvi);

				}
			}
			
		}

		private void m_btnAttack_Click(object sender, System.EventArgs e)
		{
			CombatResult cr = null;
			m_lvResults.Items.Clear();
			

			m_btnAttack.Enabled = false;
			m_btnContinue.Enabled = false;
			m_btnNextBattle.Enabled = false;
			m_btnNextPlayer.Enabled = false;
			
			switch(m_currentBattle.Type)
			{
				case BattleType.KillerSatellite:
				{
					cr = m_controller.DoKillerSatelliteCombat(m_currentBattle);
					break;
				}
				case BattleType.Bombing:
				{
					CombatInfo ci = SetUpCombat();
					ci.Type = BattleType.Bombing;
					cr = m_controller.DoBombingCombat(ci);
					break;
				}
				case BattleType.Normal:
				{
					CombatInfo ci = SetUpCombat();
					ci.Type = BattleType.Normal;
					cr = m_controller.ExecuteCombat(ci);
					break;
				}
			}

			if(cr != null)
			{
				foreach(AttackResult ar in cr.AttackResults)
				{
					ListViewItem lvi = new ListViewItem();
					lvi.Text = ar.Attacker.UnitType.ToString();
					lvi.SubItems.Add(ar.Defender.UnitType.ToString());
					lvi.SubItems.Add(ar.Defender.Owner.Name);
					lvi.SubItems.Add(ar.Roll.ToString());
					string hitResult = ar.Hit ? "Yes" : "No";
					lvi.SubItems.Add(hitResult);

					m_lvResults.Items.Add(lvi);

				}

				m_lastResult = cr;
			}



			m_btnContinue.Enabled = true;
		}

		private CombatInfo SetUpCombat()
		{
			CombatInfo ci = new CombatInfo();

			foreach(ListViewItem lvi in m_lvAttackers.Items)
			{
				UnitCollection uc = GetListedUnits(lvi, true);
				ci.Attackers.AddAllUnits(uc);
			}

			foreach(ListViewItem lvi in m_lvDefenders.Items)
			{
				UnitCollection uc = GetListedUnits(lvi, false);
				ci.Defenders.AddAllUnits(uc);
			}

			return ci;
		}

		private UnitCollection GetListedUnits(ListViewItem lvi, bool attacking)
		{
			Player p = m_controller.GetPlayer(lvi.Text);
			UnitType ut = (UnitType)Enum.Parse(typeof(UnitType), lvi.SubItems[1].Text);
			int numUnits = Int32.Parse(lvi.SubItems[2].Text);

			// TODO This won't work... defenders' casualties aren't removed until the end of the turn,
			//      so a "dead" unit would still be in the territory
			//UnitCollection playerUnits = ((UnitCollection)m_currentUnused[p]);//.GetUnits(p);
			//UnitCollection matchingTypes = playerUnits.GetUnits(ut);

			UnitCollection matches = null;
			// Theoretically should be at least this many units available, anyway
			if(attacking)
			{
				matches = m_currentUnused.GetUnits(ut, numUnits);
				m_currentUnused.RemoveAllUnits(matches);
			}
			else
			{
				
				UnitCollection playerUnits = ((UnitCollection)m_survivingUnits[p]);//.GetUnits(p);

				// TODO Figure out how to check for transports only targetable last
				/*
				if(ut == UnitType.Transport)
				{
					if(playerUnits.GetUnitTypeCount().Count > 1)
					{

					}
				}
				*/

				matches = playerUnits.GetUnits(ut);
			}
			//return matchingTypes.GetUnits(ut, numUnits);
			return matches;

		}

		private void m_btnContinue_Click(object sender, System.EventArgs e)
		{
			m_btnContinue.Enabled = false;

			m_lvAttackers.Items.Clear();
			m_lvDefenders.Items.Clear();
			m_lvResults.Items.Clear();

			m_turnResult.Casualties.AddAllUnits(m_lastResult.Casualties);
			
			foreach(Player p in m_lastResult.Casualties.GetPlayersWithUnits())
			{
				UnitCollection uc = (UnitCollection)m_survivingUnits[p];
				uc.RemoveAllUnits(m_lastResult.Casualties.GetUnits(p));
			}

			m_currentUnused.AddAllUnits(m_lastResult.UnusedAttackers);
			m_currentUnused.RemoveAllUnits(m_lastResult.UsedAttackers);

			
			if(m_currentUnused.Count == 0)
			{
				/*
				if(m_currentBattle.Type == BattleType.KillerSatellite)
				{
					m_btnNextBattle.Enabled = true;
				}
				else
				{
					m_btnNextPlayer.Enabled = true;
				}
				*/
				m_btnNextPlayer.Enabled = true;
			}
			else
			{
				m_lvAttUnused.Items.Clear();
				AddUnitsToListView(m_currentUnused, m_lvAttUnused);


			}

			DisplayUnits();
			if(m_lvEnemyLive.Items.Count == 0)
			{
				m_btnNextPlayer.Enabled = true;
			}
		}

		private void m_btnNextPlayer_Click(object sender, System.EventArgs e)
		{
			m_btnNextPlayer.Enabled = false;
			

			//m_currentUnused

			//bool displayUnits = false;

			switch(m_currentBattle.Type)
			{
				// only one turn / player for these types
				case BattleType.KillerSatellite:
				case BattleType.Bombing:
				{
					
					NextTurn();
					//DisplayUnits();
					DisplaySurvivingEnemies();
					m_btnNextBattle.Enabled = true;
					break;
				}
				case BattleType.Normal:
				{
					int index = m_playerOrder.IndexOf(m_currentPlayer);

					bool anotherTurn = false;

					bool allOtherUnitsDead = true;
					foreach(Player p in m_playerOrder)
					{
						if(p == m_currentPlayer)
						{
							continue;
						}

						UnitCollection uc = (UnitCollection)m_survivingUnits[p];
						if(uc.Count > 0)
						{
							allOtherUnitsDead = false;
						}
					}
					if( (index == (m_playerOrder.Count - 1))
						)//|| allOtherUnitsDead)//(m_lvEnemyLive.Items.Count == 0))
					{
						anotherTurn = NextTurn();
						if(anotherTurn)
						{
							m_currentPlayer = (Player)m_playerOrder[0];
							UpdateUnusedUnits();
						}
						else
						{
							m_btnNextBattle.Enabled = true;
						}
					}
					else
					{
						m_currentPlayer = (Player)m_playerOrder[index + 1];
						UpdateUnusedUnits();
					}

					DisplayUnits();
					break;
				}
			}
		}

		private void UpdateUnusedUnits()
		{
			m_currentUnused.Clear();
			UnitCollection survivors = (UnitCollection)m_survivingUnits[m_currentPlayer];
			// if there's anything here, it's been killed this turn and can still shoot
			UnitCollection casualties = m_turnResult.Casualties.GetUnits(m_currentPlayer);

			m_currentUnused.AddAllUnits(survivors);
			m_currentUnused.AddAllUnits(casualties);
		}
		
		// Returns true if there is another turn after this one
		private bool NextTurn()
		{
			foreach(Unit u in m_turnResult.Casualties)
			{

				u.Destroy();
			}

			if( (m_currentBattle.Type == BattleType.KillerSatellite)
				|| (m_currentBattle.Type == BattleType.Bombing))
			{
				m_playerOrder.Clear();
				return false;
			}
			else
			{
				Territory t = m_currentBattle.Territory;
				ArrayList playersToRemove = new ArrayList();
				foreach(Player p in m_playerOrder)
				{
					UnitCollection uc = (UnitCollection)m_survivingUnits[p];//t.Units.GetUnits(p);
					//UnitCollection combatUnits = uc.GetCombatUnits();
					if(uc.Count == 0)
					{
						playersToRemove.Add(p);
					}				
				}

				foreach(Player p in playersToRemove)
				{
					m_playerOrder.Remove(p);
				}

				// TODO Could just clear out everything instead...
				m_turnResult = new CombatResult();

				// only one player left?
				return (m_playerOrder.Count != 1);
			}
			
		}

		private void DisplayUnits()
		{
			DisplayUnits(false);
		}

		private void DisplayUnits(bool updateUnused)
		{
			m_lvAttUnused.Items.Clear();
			m_lvEnemyLive.Items.Clear();

			if(updateUnused)
			{
				
			}
			

            AddUnitsToListView(m_currentUnused, m_lvAttUnused);

			DisplaySurvivingEnemies();
			
		}

		private void DisplaySurvivingEnemies()
		{
			foreach(Player p in m_playerOrder)
			{
				if(p == m_currentPlayer)
				{
					continue;
				}

				UnitCollection enemySurvivors = (UnitCollection)m_survivingUnits[p];
				AddUnitsToListView(enemySurvivors, m_lvEnemyLive);

			}
		}

		private void m_btnNextBattle_Click(object sender, System.EventArgs e)
		{
			if(!NextBattle())
			{
				MessageBox.Show("Done with battles");
			}
		}

		public BuckRogers.GameController Controller
		{
			get { return this.m_controller; }
			set { this.m_controller = value; }
		}

		public Hashlist Battles
		{
			get { return this.m_battles; }
			set { this.m_battles = value; }
		}


	}
}
