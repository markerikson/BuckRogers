using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;

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
		private BattleController m_battleController;
		private int m_productionIndex;

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
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label m_labSeed;
		private System.Windows.Forms.ColumnHeader columnHeader23;
		private System.Windows.Forms.ColumnHeader columnHeader24;
		private System.Windows.Forms.Label m_labBattlesLeft;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.NumericUpDown m_udSkipBattles;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.ListBox m_lbProductionOrder;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.ListView m_lvFactories;
		private System.Windows.Forms.ColumnHeader columnHeader25;
		private System.Windows.Forms.ColumnHeader columnHeader26;
		private System.Windows.Forms.ColumnHeader columnHeader27;
		private System.Windows.Forms.ColumnHeader columnHeader28;
		private System.Windows.Forms.Button m_btnNextProduction;
		private System.Windows.Forms.ComboBox m_cbUnitTypes;
		private System.Windows.Forms.ComboBox m_cbNeighbors;
		private System.Windows.Forms.Button m_btnSetupProduction;
		private System.Windows.Forms.ColumnHeader columnHeader29;
		private System.Windows.Forms.Button m_btnFinishProduction;
		private System.Windows.Forms.Button m_btnProduce;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.TabPage tabPage3;
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

			foreach(UnitType ut in Unit.GetBuildableTypes())
			{
				m_cbUnitTypes.Items.Add(ut.ToString());
			}

			m_cbUnitTypes.SelectedIndex = 0;
            m_cbNumUnits.SelectedIndex = 0;

			m_lvAttackers.Items.Clear();
			m_lvDefenders.Items.Clear();
			m_lvAttUnused.Items.Clear();
			m_lvEnemyLive.Items.Clear();

			this.Init();
			this.SetupBattles();
			m_battleController.Battles = m_controller.Battles;
			m_battleController.UnitsToDisplay +=new DisplayUnitsHandler(DisplayUnits);

			Utility.Twister.Initialize(696);

			m_labSeed.Text = Utility.Twister.Seed.ToString();
			
			m_battleController.NextBattle();
			UpdateCombatInformation();

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
			this.columnHeader23 = new System.Windows.Forms.ColumnHeader();
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
			this.columnHeader24 = new System.Windows.Forms.ColumnHeader();
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
			this.columnHeader29 = new System.Windows.Forms.ColumnHeader();
			this.m_btnContinue = new System.Windows.Forms.Button();
			this.m_btnAttack = new System.Windows.Forms.Button();
			this.m_labBattleType = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.m_labLocation = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.m_btnNextPlayer = new System.Windows.Forms.Button();
			this.m_btnNextBattle = new System.Windows.Forms.Button();
			this.label13 = new System.Windows.Forms.Label();
			this.m_labSeed = new System.Windows.Forms.Label();
			this.m_labBattlesLeft = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.m_udSkipBattles = new System.Windows.Forms.NumericUpDown();
			this.label14 = new System.Windows.Forms.Label();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.label18 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.m_btnProduce = new System.Windows.Forms.Button();
			this.m_btnSetupProduction = new System.Windows.Forms.Button();
			this.m_cbNeighbors = new System.Windows.Forms.ComboBox();
			this.m_cbUnitTypes = new System.Windows.Forms.ComboBox();
			this.m_btnFinishProduction = new System.Windows.Forms.Button();
			this.m_btnNextProduction = new System.Windows.Forms.Button();
			this.m_lvFactories = new System.Windows.Forms.ListView();
			this.columnHeader25 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader26 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader27 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader28 = new System.Windows.Forms.ColumnHeader();
			this.label16 = new System.Windows.Forms.Label();
			this.m_lbProductionOrder = new System.Windows.Forms.ListBox();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			((System.ComponentModel.ISupportInitialize)(this.m_udSkipBattles)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
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
			this.label1.Location = new System.Drawing.Point(704, 432);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Current Player:";
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
			this.m_lbCurrentPlayer.Location = new System.Drawing.Point(704, 452);
			this.m_lbCurrentPlayer.Name = "m_lbCurrentPlayer";
			this.m_lbCurrentPlayer.Size = new System.Drawing.Size(156, 108);
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
			this.label3.Location = new System.Drawing.Point(528, 468);
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
			this.m_lvAttUsed.Location = new System.Drawing.Point(528, 488);
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
																							this.columnHeader7,
																							this.columnHeader23});
			this.m_lvEnemyLive.FullRowSelect = true;
			this.m_lvEnemyLive.HideSelection = false;
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
			// columnHeader23
			// 
			this.columnHeader23.Text = "Territory";
			this.columnHeader23.Width = 100;
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
			this.label5.Location = new System.Drawing.Point(624, 464);
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
			this.m_lvEnemyDead.Location = new System.Drawing.Point(624, 480);
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
			this.m_lvAttackers.HideSelection = false;
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
			this.m_cbNumUnits.Size = new System.Drawing.Size(116, 21);
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
																							this.columnHeader17,
																							this.columnHeader24});
			this.m_lvDefenders.FullRowSelect = true;
			this.m_lvDefenders.HideSelection = false;
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
			// columnHeader24
			// 
			this.columnHeader24.Text = "Territory";
			this.columnHeader24.Width = 100;
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
																						  this.columnHeader21,
																						  this.columnHeader29});
			this.m_lvResults.HideSelection = false;
			this.m_lvResults.Location = new System.Drawing.Point(580, 68);
			this.m_lvResults.Name = "m_lvResults";
			this.m_lvResults.Size = new System.Drawing.Size(352, 348);
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
			this.columnHeader21.Text = "Leader";
			// 
			// columnHeader29
			// 
			this.columnHeader29.Text = "Hit";
			this.columnHeader29.Width = 35;
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
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(428, 436);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(44, 12);
			this.label13.TabIndex = 32;
			this.label13.Text = "Seed:";
			// 
			// m_labSeed
			// 
			this.m_labSeed.Location = new System.Drawing.Point(476, 436);
			this.m_labSeed.Name = "m_labSeed";
			this.m_labSeed.Size = new System.Drawing.Size(44, 12);
			this.m_labSeed.TabIndex = 33;
			// 
			// m_labBattlesLeft
			// 
			this.m_labBattlesLeft.Location = new System.Drawing.Point(608, 436);
			this.m_labBattlesLeft.Name = "m_labBattlesLeft";
			this.m_labBattlesLeft.Size = new System.Drawing.Size(44, 16);
			this.m_labBattlesLeft.TabIndex = 35;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(544, 436);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(60, 16);
			this.label15.TabIndex = 34;
			this.label15.Text = "Battles left:";
			// 
			// m_udSkipBattles
			// 
			this.m_udSkipBattles.Location = new System.Drawing.Point(460, 468);
			this.m_udSkipBattles.Name = "m_udSkipBattles";
			this.m_udSkipBattles.Size = new System.Drawing.Size(36, 20);
			this.m_udSkipBattles.TabIndex = 36;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(380, 472);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(68, 16);
			this.label14.TabIndex = 37;
			this.label14.Text = "Skip battles:";
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(972, 660);
			this.tabControl1.TabIndex = 38;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.label14);
			this.tabPage1.Controls.Add(this.m_udSkipBattles);
			this.tabPage1.Controls.Add(this.m_labBattlesLeft);
			this.tabPage1.Controls.Add(this.label15);
			this.tabPage1.Controls.Add(this.m_labSeed);
			this.tabPage1.Controls.Add(this.label13);
			this.tabPage1.Controls.Add(this.m_btnNextBattle);
			this.tabPage1.Controls.Add(this.m_btnNextPlayer);
			this.tabPage1.Controls.Add(this.label10);
			this.tabPage1.Controls.Add(this.m_labLocation);
			this.tabPage1.Controls.Add(this.label12);
			this.tabPage1.Controls.Add(this.m_labBattleType);
			this.tabPage1.Controls.Add(this.label11);
			this.tabPage1.Controls.Add(this.m_btnAttack);
			this.tabPage1.Controls.Add(this.m_btnContinue);
			this.tabPage1.Controls.Add(this.m_lvResults);
			this.tabPage1.Controls.Add(this.m_labCurrentPlayer);
			this.tabPage1.Controls.Add(this.label9);
			this.tabPage1.Controls.Add(this.m_butRemDefenders);
			this.tabPage1.Controls.Add(this.m_butAddDefenders);
			this.tabPage1.Controls.Add(this.m_butRemAttackers);
			this.tabPage1.Controls.Add(this.m_butAddAttackers);
			this.tabPage1.Controls.Add(this.label8);
			this.tabPage1.Controls.Add(this.m_lvDefenders);
			this.tabPage1.Controls.Add(this.label7);
			this.tabPage1.Controls.Add(this.m_cbNumUnits);
			this.tabPage1.Controls.Add(this.label6);
			this.tabPage1.Controls.Add(this.m_lvAttackers);
			this.tabPage1.Controls.Add(this.label5);
			this.tabPage1.Controls.Add(this.m_lvEnemyDead);
			this.tabPage1.Controls.Add(this.label4);
			this.tabPage1.Controls.Add(this.m_lvEnemyLive);
			this.tabPage1.Controls.Add(this.label3);
			this.tabPage1.Controls.Add(this.m_lvAttUsed);
			this.tabPage1.Controls.Add(this.label2);
			this.tabPage1.Controls.Add(this.m_lbCurrentPlayer);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Controls.Add(this.m_lvAttUnused);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(964, 634);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "CombatTestForm";
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.label18);
			this.tabPage2.Controls.Add(this.label17);
			this.tabPage2.Controls.Add(this.m_btnProduce);
			this.tabPage2.Controls.Add(this.m_btnSetupProduction);
			this.tabPage2.Controls.Add(this.m_cbNeighbors);
			this.tabPage2.Controls.Add(this.m_cbUnitTypes);
			this.tabPage2.Controls.Add(this.m_btnFinishProduction);
			this.tabPage2.Controls.Add(this.m_btnNextProduction);
			this.tabPage2.Controls.Add(this.m_lvFactories);
			this.tabPage2.Controls.Add(this.label16);
			this.tabPage2.Controls.Add(this.m_lbProductionOrder);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(964, 634);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Production";
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(652, 84);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(100, 16);
			this.label18.TabIndex = 10;
			this.label18.Text = "Place unit in:";
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(652, 32);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(100, 16);
			this.label17.TabIndex = 9;
			this.label17.Text = "Unit to produce:";
			// 
			// m_btnProduce
			// 
			this.m_btnProduce.Location = new System.Drawing.Point(652, 132);
			this.m_btnProduce.Name = "m_btnProduce";
			this.m_btnProduce.TabIndex = 8;
			this.m_btnProduce.Text = "Produce";
			this.m_btnProduce.Click += new System.EventHandler(this.m_btnProduce_Click);
			// 
			// m_btnSetupProduction
			// 
			this.m_btnSetupProduction.Location = new System.Drawing.Point(12, 116);
			this.m_btnSetupProduction.Name = "m_btnSetupProduction";
			this.m_btnSetupProduction.TabIndex = 7;
			this.m_btnSetupProduction.Text = "Setup";
			this.m_btnSetupProduction.Click += new System.EventHandler(this.m_btnSetupProduction_Click);
			// 
			// m_cbNeighbors
			// 
			this.m_cbNeighbors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_cbNeighbors.Location = new System.Drawing.Point(652, 100);
			this.m_cbNeighbors.Name = "m_cbNeighbors";
			this.m_cbNeighbors.Size = new System.Drawing.Size(196, 21);
			this.m_cbNeighbors.TabIndex = 6;
			// 
			// m_cbUnitTypes
			// 
			this.m_cbUnitTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_cbUnitTypes.Location = new System.Drawing.Point(652, 48);
			this.m_cbUnitTypes.Name = "m_cbUnitTypes";
			this.m_cbUnitTypes.Size = new System.Drawing.Size(121, 21);
			this.m_cbUnitTypes.TabIndex = 5;
			// 
			// m_btnFinishProduction
			// 
			this.m_btnFinishProduction.Location = new System.Drawing.Point(12, 172);
			this.m_btnFinishProduction.Name = "m_btnFinishProduction";
			this.m_btnFinishProduction.TabIndex = 4;
			this.m_btnFinishProduction.Text = "Finish";
			this.m_btnFinishProduction.Click += new System.EventHandler(this.m_btnFinishProduction_Click);
			// 
			// m_btnNextProduction
			// 
			this.m_btnNextProduction.Location = new System.Drawing.Point(12, 144);
			this.m_btnNextProduction.Name = "m_btnNextProduction";
			this.m_btnNextProduction.Size = new System.Drawing.Size(76, 23);
			this.m_btnNextProduction.TabIndex = 3;
			this.m_btnNextProduction.Text = "Next player";
			this.m_btnNextProduction.Click += new System.EventHandler(this.m_btnNextProduction_Click);
			// 
			// m_lvFactories
			// 
			this.m_lvFactories.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							this.columnHeader25,
																							this.columnHeader26,
																							this.columnHeader27,
																							this.columnHeader28});
			this.m_lvFactories.FullRowSelect = true;
			this.m_lvFactories.HideSelection = false;
			this.m_lvFactories.Location = new System.Drawing.Point(180, 24);
			this.m_lvFactories.MultiSelect = false;
			this.m_lvFactories.Name = "m_lvFactories";
			this.m_lvFactories.Size = new System.Drawing.Size(424, 260);
			this.m_lvFactories.TabIndex = 2;
			this.m_lvFactories.View = System.Windows.Forms.View.Details;
			this.m_lvFactories.SelectedIndexChanged += new System.EventHandler(this.m_lvFactories_SelectedIndexChanged);
			// 
			// columnHeader25
			// 
			this.columnHeader25.Text = "Territory";
			this.columnHeader25.Width = 141;
			// 
			// columnHeader26
			// 
			this.columnHeader26.Text = "Producing";
			this.columnHeader26.Width = 70;
			// 
			// columnHeader27
			// 
			this.columnHeader27.Text = "Number";
			// 
			// columnHeader28
			// 
			this.columnHeader28.Text = "Destination";
			this.columnHeader28.Width = 120;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(4, 8);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(100, 16);
			this.label16.TabIndex = 1;
			this.label16.Text = "Current player:";
			// 
			// m_lbProductionOrder
			// 
			this.m_lbProductionOrder.Items.AddRange(new object[] {
																	 "Mark",
																	 "Chris",
																	 "Stu",
																	 "Hannah",
																	 "Jake",
																	 "Kathryn"});
			this.m_lbProductionOrder.Location = new System.Drawing.Point(4, 24);
			this.m_lbProductionOrder.Name = "m_lbProductionOrder";
			this.m_lbProductionOrder.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.m_lbProductionOrder.Size = new System.Drawing.Size(120, 82);
			this.m_lbProductionOrder.TabIndex = 0;
			// 
			// tabPage3
			// 
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(964, 634);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Movement";
			// 
			// CombatTestForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(992, 666);
			this.Controls.Add(this.tabControl1);
			this.Name = "CombatTestForm";
			this.Text = "Buck Rogers Combat Test Program";
			((System.ComponentModel.ISupportInitialize)(this.m_udSkipBattles)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
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
			MoveItems(m_lvAttUnused, m_lvAttackers, false);

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

		#region Battle setup code
		public void Init()
		{
			string[] players = {"Mark", "Chris", "Stu", "Hannah", "Jake", "Kathryn"};
			m_controller = new GameController(players);
			m_battleController = new BattleController(m_controller);

			m_controller.AssignTerritories();
			m_controller.CreateInitialUnits();

			// should set up Stu as the first player
			Utility.Twister.Initialize(8);
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
			uc = farside.Units.GetUnits(UnitType.Leader);
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


		#endregion

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
					

					m_lvResults.Items.Add(lvi);

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
			int battlesToSkip = Convert.ToInt32(m_udSkipBattles.Value);

			if(battlesToSkip >= (m_battleController.Battles.Count -1))
			{
				battlesToSkip = m_battleController.Battles.Count - 1;
			}
			for(int i = 0; i < battlesToSkip; i++)
			{
				m_battleController.Battles.Remove(0);
			}

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
				MessageBox.Show("Done with battles");
			}
			else
			{
				UpdateCombatInformation();
			}
		}

		#endregion

		
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

			foreach(Player p in m_battleController.BattleOrder)
			{
				m_lbCurrentPlayer.Items.Add(p.Name);
			}

			m_lbCurrentPlayer.SelectedIndex = 0;
			m_labCurrentPlayer.Text = m_battleController.CurrentPlayer.Name;

			EnableAttack();
		}

		private void m_btnNextProduction_Click(object sender, System.EventArgs e)
		{
			m_lvFactories.Items.Clear();
			NextProduction();
		}

		private void m_btnSetupProduction_Click(object sender, System.EventArgs e)
		{
			m_lbProductionOrder.Items.Clear();

			foreach(Player p in m_controller.PlayerOrder)
			{
				m_lbProductionOrder.Items.Add(p.Name);
			}

			m_productionIndex = -1;

			m_btnProduce.Enabled = false;
			m_btnNextProduction.Enabled = true;

			NextProduction();


		}

		private void NextProduction()
		{
			m_productionIndex++;

			if(m_productionIndex < m_lbProductionOrder.Items.Count)
			{
				m_lbProductionOrder.SelectedIndex = m_productionIndex;
				string playerName = (string)m_lbProductionOrder.SelectedItem;
				Player p = m_controller.GetPlayer(playerName);

				UnitCollection allFactories = p.Units.GetUnits(UnitType.Factory);

				UnitCollection usableFactories = new UnitCollection();

				foreach(Factory f in allFactories)
				{
					if(f.CanProduce)
					{
						usableFactories.AddUnit(f);
					}
				}

				foreach(Factory f in usableFactories)
				{
					ListViewItem lvi = new ListViewItem();

					lvi.Text = f.CurrentTerritory.Name;
					lvi.SubItems.Add(f.ProductionType.ToString());
					lvi.SubItems.Add(f.AmountProduced.ToString());
					lvi.SubItems.Add(f.DestinationTerritory.Name);

					m_lvFactories.Items.Add(lvi);
				}
			}
			else
			{
				m_btnNextProduction.Enabled = false;
				m_btnProduce.Enabled = true;
			}
			
		}

		private void m_lvFactories_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(m_lvFactories.SelectedIndices.Count > 0)
			{
				int idxUnused = m_lvFactories.SelectedIndices[0];
				ListViewItem lvi = m_lvFactories.Items[idxUnused];

				string territoryName = lvi.Text;
				Territory t = m_controller.Map[territoryName];
				string playerName = (string)m_lbProductionOrder.SelectedItem;
				Player p = m_controller.GetPlayer(playerName);

				ArrayList names = new ArrayList();
				//foreach(EdgeToNeighbor etn in t.Neighbors)
				foreach(Territory neighbor in t.Neighbors)
				{
					//Node neighbor = etn.Neighbor;
					if(   (neighbor.Owner == p) 
					   || (neighbor.Type == TerritoryType.Space) )
					{
						names.Add(neighbor.Key);
					}
					
				}

				string[] namearray = (string[])names.ToArray(typeof(string));
				Array.Sort(namearray);

				m_cbNeighbors.Items.Clear();

				m_cbNeighbors.Items.Add(territoryName);
				foreach(string Name in namearray)
				{
					m_cbNeighbors.Items.Add(Name);
				}

				m_cbNeighbors.SelectedIndex = 0;

				m_btnProduce.Enabled = true;
			}
			
		}

		private void m_btnProduce_Click(object sender, System.EventArgs e)
		{
			if(m_lvFactories.SelectedIndices.Count > 0)
			{
				int idxUnused = m_lvFactories.SelectedIndices[0];
				ListViewItem lvi = m_lvFactories.Items[idxUnused];

				string territoryName = lvi.Text;
				string typeName = (string)m_cbUnitTypes.SelectedItem;
				string destinationName = (string)m_cbNeighbors.SelectedItem;

				Territory t = m_controller.Map[territoryName];
				Territory destination = m_controller.Map[destinationName];
				UnitType ut = (UnitType)Enum.Parse(typeof(UnitType), typeName);
				UnitCollection factories = t.Units.GetUnits(UnitType.Factory);

				Factory f = (Factory)factories[0];

				ProductionInfo pi = new ProductionInfo();
				pi.Factory = f;
				pi.Type = ut;
				pi.DestinationTerritory = destination;

				try
				{
					if(m_controller.CheckProduction(pi))
					{
						f.StartProduction(ut, destination);

						lvi.SubItems[1].Text = typeName;
						lvi.SubItems[2].Text = f.AmountProduced.ToString();
						lvi.SubItems[3].Text = destinationName;
					}
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message, "Production Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
				
				


			}
		}

		private void m_btnFinishProduction_Click(object sender, System.EventArgs e)
		{
			m_controller.ExecuteProduction();
		}

		public BuckRogers.GameController Controller
		{
			get { return this.m_controller; }
			set { this.m_controller = value; }
		}

	}
}
