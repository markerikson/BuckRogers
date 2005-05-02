using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using BuckRogers;

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
		private DataGridBoolColumn dgbc;
		private string[] m_playerNames;
		private GameOptions m_options;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox m_cbVictoryConditions;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.NumericUpDown m_nudNumTerritories;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.DataGrid dataGrid1;
		private System.Windows.Forms.NumericUpDown m_nudProductionMultiplier;
		private System.Windows.Forms.NumericUpDown m_nudProductionTurn;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private EeekSoft.WinForms.Controls.EnumEditor enumEditor1;
		private System.Windows.Forms.GroupBox groupBox1;
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

			enumEditor1.EnumType = typeof(StartingScenarios);
			enumEditor1.EnumValue = (long)StartingScenarios.Normal;
			enumEditor1.LableFormat = "{1}";
			

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

			m_cbNumPlayers.SelectedIndex = 0;

			m_options = new GameOptions();
			//m_props = new PropertyTable();

			m_nudProductionTurn.Enabled = false;
			m_nudProductionMultiplier.Enabled = false;


			DataGridTableStyle dgts=new DataGridTableStyle();			
			dgts.MappingName = m_options.OptionalRules.GetListName(null);

			dgbc=new DataGridBoolColumn();			
			dgbc.MappingName="Value";
			dgbc.AllowNull=false;
			dgbc.Width = 20;
			dgts.GridColumnStyles.Add(dgbc);			

			DataGridTextBoxColumn dgtbc=new DataGridTextBoxColumn();
			dgtbc.MappingName ="Description";
			dgtbc.HeaderText = "Option";
			dgtbc.ReadOnly = true;
			dgts.GridColumnStyles.Add(dgtbc);
			
			tabPage2.BindingContext = new BindingContext();			

			this.dataGrid1.TableStyles.Clear();
			this.dataGrid1.TableStyles.Add(dgts);
			
			dataGrid1.DataSource = m_options.OptionalRules;

			int numCols = dataGrid1.TableStyles[0].GridColumnStyles.Count;//( (DataTable) dataGrid1.DataSource ).Columns.Count; 
			//the fudge -4 is for the grid borders 
			int targetWidth = dataGrid1.ClientSize.Width -
				SystemInformation.VerticalScrollBarWidth - 4; 
			dataGrid1.TableStyles[0].RowHeaderWidth = 0;
			int runningWidthUsed = dataGrid1.TableStyles[0].RowHeaderWidth;//dataGrid1.TableStyles[ "customers" ].RowHeaderWidth; 
            
			for ( int i = 0; i < numCols - 1; ++i ) 
			{
				runningWidthUsed += 
					dataGrid1.TableStyles[0].GridColumnStyles[ i ].Width;
			}

			if ( runningWidthUsed < targetWidth ) 
			{
				dataGrid1.TableStyles[0].GridColumnStyles[ numCols - 1 ].Width = 
					targetWidth - runningWidthUsed; 
			}
			
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
			this.enumEditor1 = new EeekSoft.WinForms.Controls.EnumEditor();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.m_nudProductionTurn = new System.Windows.Forms.NumericUpDown();
			this.m_nudProductionMultiplier = new System.Windows.Forms.NumericUpDown();
			this.dataGrid1 = new System.Windows.Forms.DataGrid();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			((System.ComponentModel.ISupportInitialize)(this.m_nudNumTerritories)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_nudProductionTurn)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_nudProductionMultiplier)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
			this.groupBox1.SuspendLayout();
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
			this.m_cbNumPlayers.SelectedIndexChanged += new System.EventHandler(this.m_cbNumPlayers_SelectedIndexChanged);
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
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(4, 336);
			this.button1.Name = "button1";
			this.button1.TabIndex = 18;
			this.button1.Text = "OK";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button2.Location = new System.Drawing.Point(92, 336);
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
			this.m_nudNumTerritories.Location = new System.Drawing.Point(384, 44);
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
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(4, 8);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(504, 324);
			this.tabControl1.TabIndex = 24;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.groupBox1);
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
			this.tabPage1.Size = new System.Drawing.Size(496, 298);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Players";
			// 
			// enumEditor1
			// 
			this.enumEditor1.ControlSpacing = 24;
			this.enumEditor1.EditorType = EeekSoft.WinForms.Controls.EnumEditorType.Flags;
			this.enumEditor1.EnumType = null;
			this.enumEditor1.EnumValue = ((long)(0));
			this.enumEditor1.LableFormat = "{1}";
			this.enumEditor1.Location = new System.Drawing.Point(8, 24);
			this.enumEditor1.Name = "enumEditor1";
			this.enumEditor1.Size = new System.Drawing.Size(248, 52);
			this.enumEditor1.TabIndex = 24;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.label11);
			this.tabPage2.Controls.Add(this.label10);
			this.tabPage2.Controls.Add(this.m_nudProductionTurn);
			this.tabPage2.Controls.Add(this.m_nudProductionMultiplier);
			this.tabPage2.Controls.Add(this.dataGrid1);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(496, 298);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Game Options";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(48, 272);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(176, 16);
			this.label11.TabIndex = 29;
			this.label11.Text = "First turn for increased production";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(52, 248);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(124, 16);
			this.label10.TabIndex = 28;
			this.label10.Text = "Production multiplier";
			// 
			// m_nudProductionTurn
			// 
			this.m_nudProductionTurn.Location = new System.Drawing.Point(4, 268);
			this.m_nudProductionTurn.Maximum = new System.Decimal(new int[] {
																				25,
																				0,
																				0,
																				0});
			this.m_nudProductionTurn.Minimum = new System.Decimal(new int[] {
																				1,
																				0,
																				0,
																				0});
			this.m_nudProductionTurn.Name = "m_nudProductionTurn";
			this.m_nudProductionTurn.Size = new System.Drawing.Size(44, 20);
			this.m_nudProductionTurn.TabIndex = 27;
			this.m_nudProductionTurn.Value = new System.Decimal(new int[] {
																			  1,
																			  0,
																			  0,
																			  0});
			// 
			// m_nudProductionMultiplier
			// 
			this.m_nudProductionMultiplier.Location = new System.Drawing.Point(4, 244);
			this.m_nudProductionMultiplier.Maximum = new System.Decimal(new int[] {
																					  5,
																					  0,
																					  0,
																					  0});
			this.m_nudProductionMultiplier.Minimum = new System.Decimal(new int[] {
																					  1,
																					  0,
																					  0,
																					  0});
			this.m_nudProductionMultiplier.Name = "m_nudProductionMultiplier";
			this.m_nudProductionMultiplier.Size = new System.Drawing.Size(44, 20);
			this.m_nudProductionMultiplier.TabIndex = 26;
			this.m_nudProductionMultiplier.Value = new System.Decimal(new int[] {
																					1,
																					0,
																					0,
																					0});
			// 
			// dataGrid1
			// 
			this.dataGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.dataGrid1.DataMember = "";
			this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGrid1.Location = new System.Drawing.Point(4, 4);
			this.dataGrid1.Name = "dataGrid1";
			this.dataGrid1.Size = new System.Drawing.Size(468, 236);
			this.dataGrid1.TabIndex = 25;
			this.dataGrid1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dataGrid1_MouseUp);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.enumEditor1);
			this.groupBox1.Location = new System.Drawing.Point(228, 88);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(264, 84);
			this.groupBox1.TabIndex = 25;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Setup Options";
			// 
			// GameSetupForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(516, 362);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Name = "GameSetupForm";
			this.Text = "GameSetupForm";
			this.Load += new System.EventHandler(this.GameSetupForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.m_nudNumTerritories)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_nudProductionTurn)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_nudProductionMultiplier)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			GameSetupForm gsf = new GameSetupForm();
			Application.Run(gsf);

			GameOptions go = gsf.Options;
			bool shipSpeeds = go.OptionalRules["DifferentShipSpeeds"];
			if(gsf.DialogResult == DialogResult.OK)
			{
				BuckRogersForm brf = null;
				
				if(go.OptionalRules["UseTestingSetup"])
				{
					brf = new BuckRogersForm();
				}
				else
				{
					brf = new BuckRogersForm(go);
				}

				Application.Run(brf);
			}
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			int numPlayers = Int32.Parse((string)m_cbNumPlayers.SelectedItem);
			ArrayList al = new ArrayList();

			string errorMessage = "";
			for(int i = 0; i < numPlayers; i++)
			{
				string name = m_tbPlayerNames[i].Text;
				if(al.Contains(name))
				{
					errorMessage = "Can't have two players with the same name";
					break;
				}
				al.Add(name);
			}

			if(al.Contains(""))
			{
				errorMessage = "A player's name cannot be empty";
			}

			if(errorMessage != "")
			{
				MessageBox.Show(errorMessage, "Setup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			this.DialogResult = DialogResult.OK;
			
			m_playerNames = new string[numPlayers];

			for(int i = 0; i < numPlayers; i++)
			{
				m_playerNames[i] = m_tbPlayerNames[i].Text;
			}

			m_options.PlayerNames = m_playerNames;

			string conditionName = (string)m_cbVictoryConditions.SelectedItem;
			VictoryConditions vc = (VictoryConditions)Enum.Parse(typeof(VictoryConditions), conditionName);

			m_options.WinningConditions = vc;

			if(vc == VictoryConditions.NumberOfTerritories)
			{
				int numTerritories = (int)m_nudNumTerritories.Value;
				m_options.NumTerritoriesNeeded = numTerritories;
			}

			if(m_options.OptionalRules["IncreasedProduction"])
			{
				m_options.IncreasedProductionTurn = (int)m_nudProductionTurn.Value;
				m_options.ProductionMultiplier = (int)m_nudProductionMultiplier.Value;
			}

			m_options.SetupOptions = (StartingScenarios)enumEditor1.EnumValue;

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

		private void GameSetupForm_Load(object sender, System.EventArgs e)
		{
			tabPage2.BindingContext = tabPage2.BindingContext;
		}

		private void dataGrid1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			DataGrid.HitTestInfo hti = dataGrid1.HitTest( e.X, e.Y ); 
			try 
			{ 
				if ( hti.Type == DataGrid.HitTestType.Cell && 
					hti.Column ==  0) 
					dataGrid1[ hti.Row, hti.Column ] = ! (bool) dataGrid1[ hti.Row, hti.Column ]; 
			}                                                                                                    
			catch( Exception ex ) 
			{ 
				MessageBox.Show( ex.Message ); 
			} 

			bool increasedProduction = m_options.OptionalRules["IncreasedProduction"];
			m_nudProductionTurn.Enabled = increasedProduction;
			m_nudProductionMultiplier.Enabled = increasedProduction;

		
		}

		private void m_cbNumPlayers_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string sNum = (string)m_cbNumPlayers.SelectedItem;
			int numPlayers = Int32.Parse(sNum);

			for(int i = 5; i >= numPlayers; i--)
			{
				m_tbPlayerNames[i].Enabled = false;
			}

			for(int i = 0; i < numPlayers; i++)
			{
				m_tbPlayerNames[i].Enabled = true;
			}
		}


		public string[] PlayerNames
		{
			get
			{
				return m_playerNames;
			}
		}

		public BuckRogers.GameOptions Options
		{
			get { return this.m_options; }
			set { this.m_options = value; }
		}
	}
}
