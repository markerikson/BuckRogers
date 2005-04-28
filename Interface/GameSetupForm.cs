using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace BuckRogers.Interface
{
	/// <summary>
	/// Summary description for GameSetupForm.
	/// </summary>
	public class GameSetupForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox m_tbPlayer1;
		private System.Windows.Forms.TextBox m_tbPlayer2;
		private System.Windows.Forms.TextBox m_tbPlayer3;
		private System.Windows.Forms.TextBox m_tbPlayer4;
		private System.Windows.Forms.TextBox m_tbPlayer5;
		private System.Windows.Forms.TextBox m_tbPlayer6;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.ComboBox m_cbNumPlayers;
		private TextBox[] m_tbPlayerNames;
		private string[] m_playerNames;
		private GameOptions m_options;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox m_cbVictoryConditions;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.NumericUpDown m_nudNumTerritories;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.CheckBox m_chkPlanetFactory;
		private System.Windows.Forms.CheckBox m_chkTransportedBuilds;
		private System.Windows.Forms.CheckBox m_chkDeployAnywhere;
		private System.Windows.Forms.CheckBox m_chkCombineProduction;
		private System.Windows.Forms.CheckBox m_chkFactoryDefense;
		private System.Windows.Forms.CheckBox m_chkSlingshot;
		private System.Windows.Forms.CheckBox m_chkMergeOrbits;
		private System.Windows.Forms.CheckBox m_chkPartialPlanets;
		private System.Windows.Forms.CheckBox m_chkLimitFactories;
		private System.Windows.Forms.CheckBox m_chkCombatRetreats;
		private System.Windows.Forms.CheckBox m_chkMarkersFight;
		private System.Windows.Forms.CheckBox m_chkPassingFire;
		private System.Windows.Forms.CheckBox m_chkTroopSpeeds;
		private System.Windows.Forms.CheckBox m_chkShipSpeeds;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GameSetupForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			m_cbNumPlayers.SelectedIndex = 0;

			m_cbVictoryConditions.Items.Clear();

			foreach(string condition in Enum.GetNames(typeof(VictoryConditions)))
			{
				m_cbVictoryConditions.Items.Add(condition);
			}

			m_cbVictoryConditions.SelectedIndex = 0;
			

			m_tbPlayerNames = new TextBox[6];
			m_tbPlayerNames[0] = m_tbPlayer1;
			m_tbPlayerNames[1] = m_tbPlayer2;
			m_tbPlayerNames[2] = m_tbPlayer3;
			m_tbPlayerNames[3] = m_tbPlayer4;
			m_tbPlayerNames[4] = m_tbPlayer5;
			m_tbPlayerNames[5] = m_tbPlayer6;

			m_options = new GameOptions();
			
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.m_tbPlayer1 = new System.Windows.Forms.TextBox();
			this.m_tbPlayer2 = new System.Windows.Forms.TextBox();
			this.m_tbPlayer3 = new System.Windows.Forms.TextBox();
			this.m_tbPlayer4 = new System.Windows.Forms.TextBox();
			this.m_tbPlayer5 = new System.Windows.Forms.TextBox();
			this.m_tbPlayer6 = new System.Windows.Forms.TextBox();
			this.m_cbNumPlayers = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.m_cbVictoryConditions = new System.Windows.Forms.ComboBox();
			this.m_nudNumTerritories = new System.Windows.Forms.NumericUpDown();
			this.label9 = new System.Windows.Forms.Label();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.m_chkPlanetFactory = new System.Windows.Forms.CheckBox();
			this.m_chkTransportedBuilds = new System.Windows.Forms.CheckBox();
			this.m_chkDeployAnywhere = new System.Windows.Forms.CheckBox();
			this.m_chkCombineProduction = new System.Windows.Forms.CheckBox();
			this.m_chkFactoryDefense = new System.Windows.Forms.CheckBox();
			this.m_chkSlingshot = new System.Windows.Forms.CheckBox();
			this.m_chkMergeOrbits = new System.Windows.Forms.CheckBox();
			this.m_chkPartialPlanets = new System.Windows.Forms.CheckBox();
			this.m_chkLimitFactories = new System.Windows.Forms.CheckBox();
			this.m_chkCombatRetreats = new System.Windows.Forms.CheckBox();
			this.m_chkMarkersFight = new System.Windows.Forms.CheckBox();
			this.m_chkPassingFire = new System.Windows.Forms.CheckBox();
			this.m_chkTroopSpeeds = new System.Windows.Forms.CheckBox();
			this.m_chkShipSpeeds = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.m_nudNumTerritories)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(4, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(60, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Player 1:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(4, 60);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(60, 16);
			this.label2.TabIndex = 2;
			this.label2.Text = "Player 2:";
			// 
			// m_tbPlayer1
			// 
			this.m_tbPlayer1.Location = new System.Drawing.Point(80, 36);
			this.m_tbPlayer1.Name = "m_tbPlayer1";
			this.m_tbPlayer1.TabIndex = 6;
			this.m_tbPlayer1.Text = "";
			// 
			// m_tbPlayer2
			// 
			this.m_tbPlayer2.Location = new System.Drawing.Point(80, 60);
			this.m_tbPlayer2.Name = "m_tbPlayer2";
			this.m_tbPlayer2.TabIndex = 7;
			this.m_tbPlayer2.Text = "";
			// 
			// m_tbPlayer3
			// 
			this.m_tbPlayer3.Location = new System.Drawing.Point(80, 84);
			this.m_tbPlayer3.Name = "m_tbPlayer3";
			this.m_tbPlayer3.TabIndex = 8;
			this.m_tbPlayer3.Text = "";
			// 
			// m_tbPlayer4
			// 
			this.m_tbPlayer4.Location = new System.Drawing.Point(80, 108);
			this.m_tbPlayer4.Name = "m_tbPlayer4";
			this.m_tbPlayer4.TabIndex = 9;
			this.m_tbPlayer4.Text = "";
			// 
			// m_tbPlayer5
			// 
			this.m_tbPlayer5.Location = new System.Drawing.Point(80, 132);
			this.m_tbPlayer5.Name = "m_tbPlayer5";
			this.m_tbPlayer5.TabIndex = 10;
			this.m_tbPlayer5.Text = "";
			// 
			// m_tbPlayer6
			// 
			this.m_tbPlayer6.Location = new System.Drawing.Point(80, 156);
			this.m_tbPlayer6.Name = "m_tbPlayer6";
			this.m_tbPlayer6.TabIndex = 11;
			this.m_tbPlayer6.Text = "";
			// 
			// m_cbNumPlayers
			// 
			this.m_cbNumPlayers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_cbNumPlayers.Items.AddRange(new object[] {
																"2",
																"3",
																"4",
																"5",
																"6"});
			this.m_cbNumPlayers.Location = new System.Drawing.Point(104, 8);
			this.m_cbNumPlayers.Name = "m_cbNumPlayers";
			this.m_cbNumPlayers.Size = new System.Drawing.Size(76, 21);
			this.m_cbNumPlayers.TabIndex = 12;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(4, 12);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 16);
			this.label3.TabIndex = 13;
			this.label3.Text = "Number of players:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(4, 108);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(60, 16);
			this.label4.TabIndex = 15;
			this.label4.Text = "Player 4:";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(4, 84);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(60, 16);
			this.label5.TabIndex = 14;
			this.label5.Text = "Player 3:";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(4, 156);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(60, 16);
			this.label6.TabIndex = 17;
			this.label6.Text = "Player 6:";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(4, 132);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(60, 16);
			this.label7.TabIndex = 16;
			this.label7.Text = "Player 5:";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(4, 324);
			this.button1.Name = "button1";
			this.button1.TabIndex = 18;
			this.button1.Text = "OK";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(92, 324);
			this.button2.Name = "button2";
			this.button2.TabIndex = 19;
			this.button2.Text = "Cancel";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(228, 12);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(100, 16);
			this.label8.TabIndex = 20;
			this.label8.Text = "Victory Conditions:";
			// 
			// m_cbVictoryConditions
			// 
			this.m_cbVictoryConditions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_cbVictoryConditions.Location = new System.Drawing.Point(332, 8);
			this.m_cbVictoryConditions.Name = "m_cbVictoryConditions";
			this.m_cbVictoryConditions.Size = new System.Drawing.Size(124, 21);
			this.m_cbVictoryConditions.TabIndex = 21;
			this.m_cbVictoryConditions.SelectedIndexChanged += new System.EventHandler(this.m_cbVictoryConditions_SelectedIndexChanged);
			// 
			// m_nudNumTerritories
			// 
			this.m_nudNumTerritories.Location = new System.Drawing.Point(388, 44);
			this.m_nudNumTerritories.Maximum = new System.Decimal(new int[] {
																				42,
																				0,
																				0,
																				0});
			this.m_nudNumTerritories.Minimum = new System.Decimal(new int[] {
																				15,
																				0,
																				0,
																				0});
			this.m_nudNumTerritories.Name = "m_nudNumTerritories";
			this.m_nudNumTerritories.Size = new System.Drawing.Size(72, 20);
			this.m_nudNumTerritories.TabIndex = 22;
			this.m_nudNumTerritories.Value = new System.Decimal(new int[] {
																			  15,
																			  0,
																			  0,
																			  0});
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(228, 44);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(144, 16);
			this.label9.TabIndex = 23;
			this.label9.Text = "Number of territories to win:";
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(4, 8);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(504, 264);
			this.tabControl1.TabIndex = 24;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.m_tbPlayer3);
			this.tabPage1.Controls.Add(this.m_tbPlayer6);
			this.tabPage1.Controls.Add(this.m_tbPlayer4);
			this.tabPage1.Controls.Add(this.m_tbPlayer1);
			this.tabPage1.Controls.Add(this.label2);
			this.tabPage1.Controls.Add(this.m_cbNumPlayers);
			this.tabPage1.Controls.Add(this.label4);
			this.tabPage1.Controls.Add(this.label7);
			this.tabPage1.Controls.Add(this.label5);
			this.tabPage1.Controls.Add(this.label6);
			this.tabPage1.Controls.Add(this.m_tbPlayer5);
			this.tabPage1.Controls.Add(this.m_tbPlayer2);
			this.tabPage1.Controls.Add(this.label3);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Controls.Add(this.label9);
			this.tabPage1.Controls.Add(this.m_cbVictoryConditions);
			this.tabPage1.Controls.Add(this.label8);
			this.tabPage1.Controls.Add(this.m_nudNumTerritories);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(496, 238);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Players";
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.m_chkPlanetFactory);
			this.tabPage2.Controls.Add(this.m_chkTransportedBuilds);
			this.tabPage2.Controls.Add(this.m_chkDeployAnywhere);
			this.tabPage2.Controls.Add(this.m_chkCombineProduction);
			this.tabPage2.Controls.Add(this.m_chkFactoryDefense);
			this.tabPage2.Controls.Add(this.m_chkSlingshot);
			this.tabPage2.Controls.Add(this.m_chkMergeOrbits);
			this.tabPage2.Controls.Add(this.m_chkPartialPlanets);
			this.tabPage2.Controls.Add(this.m_chkLimitFactories);
			this.tabPage2.Controls.Add(this.m_chkCombatRetreats);
			this.tabPage2.Controls.Add(this.m_chkMarkersFight);
			this.tabPage2.Controls.Add(this.m_chkPassingFire);
			this.tabPage2.Controls.Add(this.m_chkTroopSpeeds);
			this.tabPage2.Controls.Add(this.m_chkShipSpeeds);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(496, 238);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Game Options";
			// 
			// m_chkPlanetFactory
			// 
			this.m_chkPlanetFactory.Location = new System.Drawing.Point(252, 124);
			this.m_chkPlanetFactory.Name = "m_chkPlanetFactory";
			this.m_chkPlanetFactory.Size = new System.Drawing.Size(212, 20);
			this.m_chkPlanetFactory.TabIndex = 13;
			this.m_chkPlanetFactory.Text = "First owner of a planet gets a Factory";
			// 
			// m_chkTransportedBuilds
			// 
			this.m_chkTransportedBuilds.Location = new System.Drawing.Point(252, 100);
			this.m_chkTransportedBuilds.Name = "m_chkTransportedBuilds";
			this.m_chkTransportedBuilds.Size = new System.Drawing.Size(228, 20);
			this.m_chkTransportedBuilds.TabIndex = 12;
			this.m_chkTransportedBuilds.Text = "Transported Factories can build fighters";
			// 
			// m_chkDeployAnywhere
			// 
			this.m_chkDeployAnywhere.Location = new System.Drawing.Point(252, 76);
			this.m_chkDeployAnywhere.Name = "m_chkDeployAnywhere";
			this.m_chkDeployAnywhere.Size = new System.Drawing.Size(196, 20);
			this.m_chkDeployAnywhere.TabIndex = 11;
			this.m_chkDeployAnywhere.Text = "New units deployed anywhere";
			// 
			// m_chkCombineProduction
			// 
			this.m_chkCombineProduction.Location = new System.Drawing.Point(252, 52);
			this.m_chkCombineProduction.Name = "m_chkCombineProduction";
			this.m_chkCombineProduction.Size = new System.Drawing.Size(240, 20);
			this.m_chkCombineProduction.TabIndex = 10;
			this.m_chkCombineProduction.Text = "Adjacent Factories can combine production";
			// 
			// m_chkFactoryDefense
			// 
			this.m_chkFactoryDefense.Location = new System.Drawing.Point(252, 28);
			this.m_chkFactoryDefense.Name = "m_chkFactoryDefense";
			this.m_chkFactoryDefense.Size = new System.Drawing.Size(176, 20);
			this.m_chkFactoryDefense.TabIndex = 9;
			this.m_chkFactoryDefense.Text = "Factories add to defense";
			// 
			// m_chkSlingshot
			// 
			this.m_chkSlingshot.Location = new System.Drawing.Point(252, 4);
			this.m_chkSlingshot.Name = "m_chkSlingshot";
			this.m_chkSlingshot.Size = new System.Drawing.Size(176, 20);
			this.m_chkSlingshot.TabIndex = 8;
			this.m_chkSlingshot.Text = "Slingshot around planets";
			// 
			// m_chkMergeOrbits
			// 
			this.m_chkMergeOrbits.Location = new System.Drawing.Point(8, 176);
			this.m_chkMergeOrbits.Name = "m_chkMergeOrbits";
			this.m_chkMergeOrbits.Size = new System.Drawing.Size(200, 20);
			this.m_chkMergeOrbits.TabIndex = 7;
			this.m_chkMergeOrbits.Text = "Merge Far Orbits and space zones";
			// 
			// m_chkPartialPlanets
			// 
			this.m_chkPartialPlanets.Location = new System.Drawing.Point(8, 152);
			this.m_chkPartialPlanets.Name = "m_chkPartialPlanets";
			this.m_chkPartialPlanets.Size = new System.Drawing.Size(176, 20);
			this.m_chkPartialPlanets.TabIndex = 6;
			this.m_chkPartialPlanets.Text = "Partial planet control";
			// 
			// m_chkLimitFactories
			// 
			this.m_chkLimitFactories.Location = new System.Drawing.Point(8, 128);
			this.m_chkLimitFactories.Name = "m_chkLimitFactories";
			this.m_chkLimitFactories.Size = new System.Drawing.Size(176, 20);
			this.m_chkLimitFactories.TabIndex = 5;
			this.m_chkLimitFactories.Text = "Limited factories per planet";
			// 
			// m_chkCombatRetreats
			// 
			this.m_chkCombatRetreats.Location = new System.Drawing.Point(8, 104);
			this.m_chkCombatRetreats.Name = "m_chkCombatRetreats";
			this.m_chkCombatRetreats.Size = new System.Drawing.Size(176, 20);
			this.m_chkCombatRetreats.TabIndex = 4;
			this.m_chkCombatRetreats.Text = "Combat retreats allowed";
			// 
			// m_chkMarkersFight
			// 
			this.m_chkMarkersFight.Location = new System.Drawing.Point(8, 80);
			this.m_chkMarkersFight.Name = "m_chkMarkersFight";
			this.m_chkMarkersFight.Size = new System.Drawing.Size(176, 20);
			this.m_chkMarkersFight.TabIndex = 3;
			this.m_chkMarkersFight.Text = "Control markers fight";
			// 
			// m_chkPassingFire
			// 
			this.m_chkPassingFire.Location = new System.Drawing.Point(8, 56);
			this.m_chkPassingFire.Name = "m_chkPassingFire";
			this.m_chkPassingFire.Size = new System.Drawing.Size(176, 20);
			this.m_chkPassingFire.TabIndex = 2;
			this.m_chkPassingFire.Text = "Passing fire";
			// 
			// m_chkTroopSpeeds
			// 
			this.m_chkTroopSpeeds.Location = new System.Drawing.Point(8, 32);
			this.m_chkTroopSpeeds.Name = "m_chkTroopSpeeds";
			this.m_chkTroopSpeeds.Size = new System.Drawing.Size(176, 20);
			this.m_chkTroopSpeeds.TabIndex = 1;
			this.m_chkTroopSpeeds.Text = "Different ground unit speeds";
			// 
			// m_chkShipSpeeds
			// 
			this.m_chkShipSpeeds.Location = new System.Drawing.Point(8, 8);
			this.m_chkShipSpeeds.Name = "m_chkShipSpeeds";
			this.m_chkShipSpeeds.Size = new System.Drawing.Size(176, 20);
			this.m_chkShipSpeeds.TabIndex = 0;
			this.m_chkShipSpeeds.Text = "Different spaceship speeds";
			// 
			// GameSetupForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(512, 370);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Name = "GameSetupForm";
			this.Text = "GameSetupForm";
			((System.ComponentModel.ISupportInitialize)(this.m_nudNumTerritories)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		[STAThread]
		public static void Main(string[] args)
		{
			GameSetupForm gsf = new GameSetupForm();
			Application.Run(gsf);

			if(gsf.DialogResult == DialogResult.OK)
			{

				BuckRogersForm brf = new BuckRogersForm();
				Application.Run(new BuckRogersForm());
			}
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			int numPlayers = Int32.Parse((string)m_cbNumPlayers.SelectedItem);
			m_playerNames = new string[numPlayers];

			for(int i = 0; i < numPlayers; i++)
			{
				m_playerNames[i] = m_tbPlayerNames[i].Text;
			}

			m_options.BoardWideProduction = m_chkDeployAnywhere.Checked;
			m_options.CombatRetreat = m_chkCombatRetreats.Checked;
			m_options.CombinedProduction = m_chkCombineProduction.Checked;
			m_options.ControlMarkersFight = m_chkMarkersFight.Checked;
			m_options.DifferentShipSpeeds = m_chkShipSpeeds.Checked;
			m_options.DifferentTroopSpeeds = m_chkTroopSpeeds.Checked;
			m_options.FactoryDefenses = m_chkFactoryDefense.Checked;
			m_options.FactoryLimits = m_chkLimitFactories.Checked;
			m_options.MergeFarOrbits = m_chkMergeOrbits.Checked;
			m_options.PartialPlanetControl = m_chkPartialPlanets.Checked;
			m_options.PassingFire = m_chkPassingFire.Checked;
			m_options.SlingshotEffect = m_chkSlingshot.Checked;
			m_options.TransportedFactoryProduction = m_chkTransportedBuilds.Checked;

			string conditionName = (string)m_cbVictoryConditions.SelectedItem;
			VictoryConditions vc = (VictoryConditions)Enum.Parse(typeof(VictoryConditions), conditionName);

			m_options.WinningConditions = vc;

			if(vc == VictoryConditions.NumberOfTerritories)
			{
				int numTerritories = (int)m_nudNumTerritories.Value;
				m_options.NumTerritoriesNeeded = numTerritories;
			}

			this.Close();
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void m_cbVictoryConditions_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string conditionName = (string)m_cbVictoryConditions.SelectedItem;

			VictoryConditions vc = (VictoryConditions)Enum.Parse(typeof(VictoryConditions), conditionName);

			if(vc == VictoryConditions.NumberOfTerritories)
			{
				m_nudNumTerritories.Enabled = true;
			}
			else
			{
				m_nudNumTerritories.Enabled = false;
			}
		}

		public string[] PlayerNames
		{
			get
			{
				return m_playerNames;
			}
		}
	}
}
