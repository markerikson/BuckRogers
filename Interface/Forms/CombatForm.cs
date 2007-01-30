using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;

using CommandManagement;

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
		private System.Windows.Forms.Label m_lblHasLeader;
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
		private System.Windows.Forms.Button m_btnAddAllAttackers;
		private System.Windows.Forms.Button m_btnAddAllDefenders;
		private System.Windows.Forms.Button m_btnRemoveAttackers;
		private System.Windows.Forms.Button m_btnAddAttackers;
		private System.Windows.Forms.Button m_btnAddDefenders;
		private System.Windows.Forms.Button m_btnRemoveDefenders;
		private Label m_lblHasLeaderText;
		private CommandManager m_command;

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

			m_command = new CommandManager();

			m_command.Commands.Add(new Command("Attack", new Command.ExecuteHandler(DoAttackCommand), 
									new Command.UpdateHandler(UpdateAttackCommand)));

			m_command.Commands.Add(new Command("Continue", new Command.ExecuteHandler(DoContinueCommand), 
				new Command.UpdateHandler(UpdateContinueCommand)));

			m_command.Commands.Add(new Command("NextPlayer", new Command.ExecuteHandler(DoNextPlayerCommand), 
				new Command.UpdateHandler(UpdateNextPlayerCommand)));

			m_command.Commands.Add(new Command("NextBattle", new Command.ExecuteHandler(DoNextBattleCommand), 
				new Command.UpdateHandler(UpdateNextBattleCommand)));

			Command.UpdateHandler addRemoveHandler = new Command.UpdateHandler(UpdateAddRemoveCommands);
			m_command.Commands.Add(new Command("AddAttackers", new Command.ExecuteHandler(DoAddAttackersCommand), 
				addRemoveHandler));

			m_command.Commands.Add(new Command("RemoveAttackers", new Command.ExecuteHandler(DoRemoveAttackersCommand), 
				addRemoveHandler));

			m_command.Commands.Add(new Command("AddDefenders", new Command.ExecuteHandler(DoAddDefendersCommand), 
				addRemoveHandler));

			m_command.Commands.Add(new Command("RemoveDefenders", new Command.ExecuteHandler(DoRemoveDefendersCommand), 
				addRemoveHandler));

			m_command.Commands.Add(new Command("AddAllAttackers", new Command.ExecuteHandler(DoAddAllAttackersCommand), 
				addRemoveHandler));

			m_command.Commands.Add(new Command("AddAllDefenders", new Command.ExecuteHandler(DoAddAllDefendersCommand), 
				addRemoveHandler));


			m_command.Commands["Attack"].CommandInstances.Add(
				new Object[]{m_btnAttack});

			m_command.Commands["Continue"].CommandInstances.Add(
				new Object[]{m_btnContinue});

			m_command.Commands["NextPlayer"].CommandInstances.Add(
				new Object[]{m_btnNextPlayer});

			m_command.Commands["NextBattle"].CommandInstances.Add(
				new Object[]{m_btnNextBattle});

			m_command.Commands["AddAttackers"].CommandInstances.Add(
				new Object[]{m_btnAddAttackers});

			m_command.Commands["RemoveAttackers"].CommandInstances.Add(
				new Object[]{m_btnRemoveAttackers});

			m_command.Commands["AddDefenders"].CommandInstances.Add(
				new Object[]{m_btnAddDefenders});
			
			m_command.Commands["RemoveDefenders"].CommandInstances.Add(
				new Object[]{m_btnRemoveDefenders});
			
			m_command.Commands["AddAllAttackers"].CommandInstances.Add(
				new Object[]{m_btnAddAllAttackers});

			m_command.Commands["AddAllDefenders"].CommandInstances.Add(
				new Object[]{m_btnAddAllDefenders});

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
			System.Windows.Forms.ListViewItem listViewItem14 = new System.Windows.Forms.ListViewItem(new string[] {
            "Mark",
            "Trooper",
            "15"}, -1);
			System.Windows.Forms.ListViewItem listViewItem15 = new System.Windows.Forms.ListViewItem(new string[] {
            "Mark",
            "Gennie",
            "5"}, -1);
			System.Windows.Forms.ListViewItem listViewItem16 = new System.Windows.Forms.ListViewItem(new string[] {
            "Mark",
            "Fighter",
            "10"}, -1);
			System.Windows.Forms.ListViewItem listViewItem17 = new System.Windows.Forms.ListViewItem(new string[] {
            "Mark",
            "Transport",
            "5"}, -1);
			System.Windows.Forms.ListViewItem listViewItem18 = new System.Windows.Forms.ListViewItem(new string[] {
            "Mark",
            "Marker",
            "1"}, -1);
			System.Windows.Forms.ListViewItem listViewItem19 = new System.Windows.Forms.ListViewItem(new string[] {
            "Jake",
            "Fighter",
            "1"}, -1);
			System.Windows.Forms.ListViewItem listViewItem20 = new System.Windows.Forms.ListViewItem(new string[] {
            "Jake",
            "Transport",
            "100"}, -1);
			System.Windows.Forms.ListViewItem listViewItem21 = new System.Windows.Forms.ListViewItem(new string[] {
            "Stu",
            "Trooper",
            "8"}, -1);
			System.Windows.Forms.ListViewItem listViewItem22 = new System.Windows.Forms.ListViewItem(new string[] {
            "Mark",
            "Trooper",
            "15"}, -1);
			System.Windows.Forms.ListViewItem listViewItem23 = new System.Windows.Forms.ListViewItem(new string[] {
            "Mark",
            "Gennie",
            "5"}, -1);
			System.Windows.Forms.ListViewItem listViewItem24 = new System.Windows.Forms.ListViewItem(new string[] {
            "Mark",
            "Fighter",
            "10"}, -1);
			System.Windows.Forms.ListViewItem listViewItem25 = new System.Windows.Forms.ListViewItem(new string[] {
            "Mark",
            "Transport",
            "5"}, -1);
			System.Windows.Forms.ListViewItem listViewItem26 = new System.Windows.Forms.ListViewItem(new string[] {
            "Mark",
            "Marker",
            "1"}, -1);
			this.m_labLocation = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.m_labBattleType = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.m_lblHasLeader = new System.Windows.Forms.Label();
			this.m_lblHasLeaderText = new System.Windows.Forms.Label();
			this.m_btnRemoveAttackers = new System.Windows.Forms.Button();
			this.m_btnAddAttackers = new System.Windows.Forms.Button();
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
			this.m_btnRemoveDefenders = new System.Windows.Forms.Button();
			this.m_btnAddDefenders = new System.Windows.Forms.Button();
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
			this.m_btnAddAllAttackers = new System.Windows.Forms.Button();
			this.m_btnAddAllDefenders = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// m_labLocation
			// 
			this.m_labLocation.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_labLocation.Location = new System.Drawing.Point(316, 4);
			this.m_labLocation.Name = "m_labLocation";
			this.m_labLocation.Size = new System.Drawing.Size(208, 23);
			this.m_labLocation.TabIndex = 34;
			this.m_labLocation.Text = "American Regency";
			// 
			// label12
			// 
			this.label12.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label12.Location = new System.Drawing.Point(240, 4);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(76, 23);
			this.label12.TabIndex = 33;
			this.label12.Text = "Location:";
			// 
			// m_labBattleType
			// 
			this.m_labBattleType.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_labBattleType.Location = new System.Drawing.Point(100, 4);
			this.m_labBattleType.Name = "m_labBattleType";
			this.m_labBattleType.Size = new System.Drawing.Size(128, 23);
			this.m_labBattleType.TabIndex = 32;
			this.m_labBattleType.Text = "Normal";
			// 
			// label11
			// 
			this.label11.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label11.Location = new System.Drawing.Point(4, 4);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(96, 23);
			this.label11.TabIndex = 31;
			this.label11.Text = "Battle Type:";
			// 
			// m_lblHasLeader
			// 
			this.m_lblHasLeader.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_lblHasLeader.Location = new System.Drawing.Point(668, 4);
			this.m_lblHasLeader.Name = "m_lblHasLeader";
			this.m_lblHasLeader.Size = new System.Drawing.Size(128, 23);
			this.m_lblHasLeader.TabIndex = 30;
			this.m_lblHasLeader.Text = "Current Player:";
			// 
			// m_lblHasLeaderText
			// 
			this.m_lblHasLeaderText.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_lblHasLeaderText.Location = new System.Drawing.Point(536, 4);
			this.m_lblHasLeaderText.Name = "m_lblHasLeaderText";
			this.m_lblHasLeaderText.Size = new System.Drawing.Size(126, 23);
			this.m_lblHasLeaderText.TabIndex = 29;
			this.m_lblHasLeaderText.Text = "Leader Present:";
			// 
			// m_btnRemoveAttackers
			// 
			this.m_btnRemoveAttackers.Location = new System.Drawing.Point(204, 140);
			this.m_btnRemoveAttackers.Name = "m_btnRemoveAttackers";
			this.m_btnRemoveAttackers.Size = new System.Drawing.Size(75, 23);
			this.m_btnRemoveAttackers.TabIndex = 40;
			this.m_btnRemoveAttackers.Text = "<< Remove";
			// 
			// m_btnAddAttackers
			// 
			this.m_btnAddAttackers.Location = new System.Drawing.Point(204, 108);
			this.m_btnAddAttackers.Name = "m_btnAddAttackers";
			this.m_btnAddAttackers.Size = new System.Drawing.Size(75, 23);
			this.m_btnAddAttackers.TabIndex = 39;
			this.m_btnAddAttackers.Text = "Add >>";
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
            listViewItem14,
            listViewItem15,
            listViewItem16,
            listViewItem17,
            listViewItem18});
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
			// m_btnRemoveDefenders
			// 
			this.m_btnRemoveDefenders.Location = new System.Drawing.Point(204, 328);
			this.m_btnRemoveDefenders.Name = "m_btnRemoveDefenders";
			this.m_btnRemoveDefenders.Size = new System.Drawing.Size(75, 23);
			this.m_btnRemoveDefenders.TabIndex = 48;
			this.m_btnRemoveDefenders.Text = "<< Remove";
			// 
			// m_btnAddDefenders
			// 
			this.m_btnAddDefenders.Location = new System.Drawing.Point(204, 296);
			this.m_btnAddDefenders.Name = "m_btnAddDefenders";
			this.m_btnAddDefenders.Size = new System.Drawing.Size(75, 23);
			this.m_btnAddDefenders.TabIndex = 47;
			this.m_btnAddDefenders.Text = "Add >>";
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
            listViewItem19,
            listViewItem20,
            listViewItem21});
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
			this.m_btnNextBattle.Size = new System.Drawing.Size(75, 23);
			this.m_btnNextBattle.TabIndex = 54;
			this.m_btnNextBattle.Text = "Next battle";
			// 
			// m_btnNextPlayer
			// 
			this.m_btnNextPlayer.Location = new System.Drawing.Point(176, 404);
			this.m_btnNextPlayer.Name = "m_btnNextPlayer";
			this.m_btnNextPlayer.Size = new System.Drawing.Size(75, 23);
			this.m_btnNextPlayer.TabIndex = 53;
			this.m_btnNextPlayer.Text = "Next player";
			// 
			// m_btnAttack
			// 
			this.m_btnAttack.Location = new System.Drawing.Point(4, 404);
			this.m_btnAttack.Name = "m_btnAttack";
			this.m_btnAttack.Size = new System.Drawing.Size(75, 23);
			this.m_btnAttack.TabIndex = 52;
			this.m_btnAttack.Text = "Attack";
			// 
			// m_btnContinue
			// 
			this.m_btnContinue.Location = new System.Drawing.Point(88, 404);
			this.m_btnContinue.Name = "m_btnContinue";
			this.m_btnContinue.Size = new System.Drawing.Size(75, 23);
			this.m_btnContinue.TabIndex = 51;
			this.m_btnContinue.Text = "Continue";
			// 
			// m_labBattlesLeft
			// 
			this.m_labBattlesLeft.Location = new System.Drawing.Point(752, 360);
			this.m_labBattlesLeft.Name = "m_labBattlesLeft";
			this.m_labBattlesLeft.Size = new System.Drawing.Size(44, 16);
			this.m_labBattlesLeft.TabIndex = 56;
			this.m_labBattlesLeft.Visible = false;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(688, 360);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(60, 16);
			this.label15.TabIndex = 55;
			this.label15.Text = "Battles left:";
			this.label15.Visible = false;
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
            listViewItem22,
            listViewItem23,
            listViewItem24,
            listViewItem25,
            listViewItem26});
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
			// m_btnAddAllAttackers
			// 
			this.m_btnAddAllAttackers.Location = new System.Drawing.Point(204, 76);
			this.m_btnAddAllAttackers.Name = "m_btnAddAllAttackers";
			this.m_btnAddAllAttackers.Size = new System.Drawing.Size(75, 23);
			this.m_btnAddAllAttackers.TabIndex = 61;
			this.m_btnAddAllAttackers.Text = "Add All >>>";
			// 
			// m_btnAddAllDefenders
			// 
			this.m_btnAddAllDefenders.Location = new System.Drawing.Point(204, 264);
			this.m_btnAddAllDefenders.Name = "m_btnAddAllDefenders";
			this.m_btnAddAllDefenders.Size = new System.Drawing.Size(75, 23);
			this.m_btnAddAllDefenders.TabIndex = 62;
			this.m_btnAddAllDefenders.Text = "Add All >>>";
			// 
			// CombatForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(816, 434);
			this.ControlBox = false;
			this.Controls.Add(this.m_btnAddAllDefenders);
			this.Controls.Add(this.m_btnAddAllAttackers);
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
			this.Controls.Add(this.m_btnRemoveDefenders);
			this.Controls.Add(this.m_btnAddDefenders);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.m_lvDefenders);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.m_lvEnemyLive);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.m_cbNumUnits);
			this.Controls.Add(this.m_btnRemoveAttackers);
			this.Controls.Add(this.m_btnAddAttackers);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.m_lvAttackers);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.m_lvAttUnused);
			this.Controls.Add(this.m_labLocation);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.m_labBattleType);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.m_lblHasLeader);
			this.Controls.Add(this.m_lblHasLeaderText);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Name = "CombatForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Combat";
			this.ResumeLayout(false);

		}
		#endregion

		public void BeginCombat()
		{
			m_battleController.Battles = m_controller.Battles;

			m_battleController.LogNewTurn();
			m_battleController.NextBattle();
			//if(m_battleController.NextBattle())
			//{
				UpdateCombatInformation();
			//}
			
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
			lv.Items.Clear();
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
			ResetDisplay();
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
			if(m_battleController.CurrentPlayer != null)
			{
				//m_lbl.Text = m_battleController.CurrentPlayer.Name;
			}

			ShowLeaderPresent();
			

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

		public void DoAddAttackersCommand(Command cmd)
		{
			MoveItems(m_lvAttUnused, m_lvAttackers, false);
		}

		public void DoRemoveAttackersCommand(Command cmd)
		{
			MoveItems(m_lvAttackers, m_lvAttUnused, false);
		}

		public void DoAddDefendersCommand(Command cmd)
		{
			MoveItems(m_lvEnemyLive, m_lvDefenders, true);
		}

		public void DoRemoveDefendersCommand(Command cmd)
		{
			MoveItems(m_lvDefenders, m_lvEnemyLive, true);
		}

		public void DoAddAllAttackersCommand(Command cmd)
		{
			ArrayList al = new ArrayList();
			while(m_lvAttUnused.Items.Count > 0)
			{
				ListViewItem lvi = m_lvAttUnused.Items[0];

				if(lvi.SubItems[1].Text == "Transport")
				{
					m_lvAttUnused.Items.Remove(lvi);
					al.Add(lvi);
				}
				else
				{
					lvi.Selected = true;
					MoveItems(m_lvAttUnused, m_lvAttackers, false);
				}
			}

			foreach(ListViewItem lvi in al)
			{
				m_lvAttUnused.Items.Add(lvi);
				lvi.Selected = true;
				MoveItems(m_lvAttUnused, m_lvAttackers, false);
			}
		}

		public void DoAddAllDefendersCommand(Command cmd)
		{
			// TODO Save Transports until last
			ArrayList al = new ArrayList();
			while(m_lvEnemyLive.Items.Count > 0)
			{
				ListViewItem lvi = m_lvEnemyLive.Items[0];

				if(lvi.SubItems[1].Text == "Transport")
				{
					m_lvEnemyLive.Items.Remove(lvi);
					al.Add(lvi);
				}
				else
				{
					lvi.Selected = true;
					MoveItems(m_lvEnemyLive, m_lvDefenders, true);
				}
			}

			foreach(ListViewItem lvi in al)
			{
				m_lvEnemyLive.Items.Add(lvi);
				lvi.Selected = true;
				MoveItems(m_lvEnemyLive, m_lvDefenders, true);
			}
		}

		public void UpdateAddRemoveCommands(Command cmd)
		{
			cmd.Enabled = (m_battleController.Status == BattleStatus.Setup);
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
                for (int i = 0; i < destination.Items.Count; i++)
                {
                    //ListViewItem lastItem = destination.Items[destination.Items.Count - 1];
                    ListViewItem lastItem = destination.Items[i];

                    string liName = lastItem.SubItems[0].Text;
                    string liType = lastItem.SubItems[1].Text;

                    string liTerritory = "";

                    if (includeTerritory)
                    {
                        liTerritory = lastItem.SubItems[3].Text;
                    }

                    if (liName == name && liType == type)
                    {
                        if (!includeTerritory || (includeTerritory && territory == liTerritory))
                        {
                            addNewItem = false;
                            int numCurrent = Int32.Parse(lastItem.SubItems[2].Text);
                            numCurrent += numToMove;

                            lastItem.SubItems[2].Text = numCurrent.ToString();

                            break;
                        }
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
		public void DoAttackCommand(Command cmd)
		{
			DoAttack();
		}

		public void UpdateAttackCommand(Command cmd)
		{
			bool rightStatus = m_battleController.Status == BattleStatus.Setup;
			bool haveAttackers = (m_lvAttackers.Items.Count > 0);
			bool haveDefenders = (m_lvDefenders.Items.Count > 0);
			cmd.Enabled = rightStatus && haveAttackers && haveDefenders;
		}

		private void DoAttack()
		{
			CombatResult cr = null;
			m_lvResults.Items.Clear();

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
					lvi.Text = ar.Attacker.Type.ToString();
					lvi.SubItems.Add(ar.Defender.Type.ToString());
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

						if(lvi.SubItems[1].Text != ar.Defender.Type.ToString())
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
						matchingItem.SubItems.Add(ar.Defender.Type.ToString());
						matchingItem.SubItems.Add("1");

						m_lvCasualties.Items.Add(matchingItem);
					}
				}

				m_battleController.LastResult = cr;
			}

			m_battleController.ProcessAttackResults();

			m_lvAttackers.Items.Clear();
			m_lvDefenders.Items.Clear();
		}

		public void DoContinueCommand(Command cmd)
		{
			DoContinue();
		}

		public void UpdateContinueCommand(Command cmd)
		{
			cmd.Enabled = (m_battleController.Status == BattleStatus.AttackComplete);
		}

		private void DoContinue()
		{
			ResetDisplay();
			m_battleController.NextRound();
		}

		public void DoNextPlayerCommand(Command cmd)
		{
			ResetDisplay();
			m_battleController.NextPlayer();
			m_lbCurrentPlayer.SelectedItem = m_battleController.CurrentPlayer.Name;

			ShowLeaderPresent();
		}

		private void ShowLeaderPresent()
		{
			
			Hashtable playerUnitCounts = m_battleController.CurrentBattle.Territory.Units.GetUnitTypeCount(m_battleController.CurrentPlayer);
			int numLeaders = (int)playerUnitCounts[UnitType.Leader];

			string hasLeaders = (numLeaders > 0 ? "Yes" : "No");
			m_lblHasLeader.Text = hasLeaders;
		}
		
		public void UpdateNextPlayerCommand(Command cmd)
		{
			cmd.Enabled = (m_battleController.Status == BattleStatus.RoundComplete);
		}

		public void DoNextBattleCommand(Command cmd)
		{
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

		public void UpdateNextBattleCommand(Command cmd)
		{
			cmd.Enabled = (m_battleController.Status == BattleStatus.BattleComplete);
		}

		private void ResetDisplay()
		{
			m_lvAttackers.Items.Clear();
			m_lvDefenders.Items.Clear();
			m_lvResults.Items.Clear();
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
				case StatusInfo.LeaderKilled:
				{
					string message = suea.Player.Name + "'s leader has been killed!";
					MessageBox.Show(message, "Leader Killed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
					UnitCollection playerUnits = m_battleController.SurvivingUnits.GetUnits(p);//((UnitCollection)m_battleController.SurvivingUnits[p]);
				
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
