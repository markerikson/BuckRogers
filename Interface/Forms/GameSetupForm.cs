using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using BuckRogers;
using BuckRogers.Networking;

namespace BuckRogers.Interface
{
	/// <summary>
	/// Summary description for GameSetupForm.
	/// </summary>
	public class GameSetupForm : System.Windows.Forms.Form
	{
		#region private members
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
		private System.Windows.Forms.Button m_btnOK;
		private System.Windows.Forms.Button m_btnCancel;
		private System.Windows.Forms.ComboBox m_cbNumPlayers;
		private TextBox[] m_tbPlayerNames;
		private string[] m_playerNames;
		private string m_loadFileName;
		private GameOptions m_options;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox m_cbVictoryConditions;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.NumericUpDown m_nudNumTerritories;
		private System.Windows.Forms.Button m_btnLoadGame;
		private RangeLimitedUpDown m_nudNodeValue;
		private TreeView m_tvOptions;
		private bool m_upDownDisplayed;
		private TreeNode m_selectedNode;
		private Hashtable m_optionNodes;
		private Label label10;
		private Button m_btnNetworkClient;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		public GameSetupForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.Icon = InterfaceUtility.GetApplicationIcon();

			this.StartPosition = FormStartPosition.CenterScreen;


			m_nudNodeValue = new RangeLimitedUpDown();
			m_optionNodes = new Hashtable();

			m_loadFileName = String.Empty;			

			m_cbVictoryConditions.Items.Clear();

			foreach(VictoryConditions vc in Enum.GetValues(typeof(VictoryConditions)))
			{
				string description = Utility.GetDescriptionOf(vc);
				m_cbVictoryConditions.Items.Add(description);
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

			m_tvOptions.CheckBoxes = true;

			for (int i = 0; i < m_options.Categories.Count; i++ )
			{
				OptionCategory category = (OptionCategory)m_options.Categories[i];

				TreeNode categoryNode = new TreeNode(category.Name);
				m_tvOptions.Nodes.Add(categoryNode);

				foreach (GameOption option in category.Options)
				{
					TreeNode optionNode = new TreeNode(option.Description);
					optionNode.Tag = option.Name;
					m_optionNodes[option.Name] = optionNode;

					categoryNode.Nodes.Add(optionNode);
				}
			}

			m_tvOptions.ExpandAll();

			for (int i = 0; i < m_tvOptions.Nodes.Count; i++)
			{
				TreeNode categoryNode = m_tvOptions.Nodes[i];
				TreeNode_SetStateImageIndex(categoryNode, 0);
			}

			// HACK Dunno why, but the first node needs this done a second time
			TreeNode_SetStateImageIndex(m_tvOptions.Nodes[0], 0);
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
			this.m_btnOK = new System.Windows.Forms.Button();
			this.m_btnCancel = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.m_cbVictoryConditions = new System.Windows.Forms.ComboBox();
			this.m_nudNumTerritories = new System.Windows.Forms.NumericUpDown();
			this.label9 = new System.Windows.Forms.Label();
			this.m_tvOptions = new System.Windows.Forms.TreeView();
			this.m_btnLoadGame = new System.Windows.Forms.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.m_btnNetworkClient = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.m_nudNumTerritories)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(15, 44);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(60, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Player 1:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(15, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(60, 16);
			this.label2.TabIndex = 2;
			this.label2.Text = "Player 2:";
			// 
			// m_tbPlayer1
			// 
			this.m_tbPlayer1.Location = new System.Drawing.Point(91, 40);
			this.m_tbPlayer1.Name = "m_tbPlayer1";
			this.m_tbPlayer1.Size = new System.Drawing.Size(100, 20);
			this.m_tbPlayer1.TabIndex = 6;
			// 
			// m_tbPlayer2
			// 
			this.m_tbPlayer2.Location = new System.Drawing.Point(91, 64);
			this.m_tbPlayer2.Name = "m_tbPlayer2";
			this.m_tbPlayer2.Size = new System.Drawing.Size(100, 20);
			this.m_tbPlayer2.TabIndex = 7;
			// 
			// m_tbPlayer3
			// 
			this.m_tbPlayer3.Location = new System.Drawing.Point(91, 88);
			this.m_tbPlayer3.Name = "m_tbPlayer3";
			this.m_tbPlayer3.Size = new System.Drawing.Size(100, 20);
			this.m_tbPlayer3.TabIndex = 8;
			// 
			// m_tbPlayer4
			// 
			this.m_tbPlayer4.Location = new System.Drawing.Point(91, 112);
			this.m_tbPlayer4.Name = "m_tbPlayer4";
			this.m_tbPlayer4.Size = new System.Drawing.Size(100, 20);
			this.m_tbPlayer4.TabIndex = 9;
			// 
			// m_tbPlayer5
			// 
			this.m_tbPlayer5.Location = new System.Drawing.Point(91, 136);
			this.m_tbPlayer5.Name = "m_tbPlayer5";
			this.m_tbPlayer5.Size = new System.Drawing.Size(100, 20);
			this.m_tbPlayer5.TabIndex = 10;
			// 
			// m_tbPlayer6
			// 
			this.m_tbPlayer6.Location = new System.Drawing.Point(91, 160);
			this.m_tbPlayer6.Name = "m_tbPlayer6";
			this.m_tbPlayer6.Size = new System.Drawing.Size(100, 20);
			this.m_tbPlayer6.TabIndex = 11;
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
			this.m_cbNumPlayers.Location = new System.Drawing.Point(115, 12);
			this.m_cbNumPlayers.Name = "m_cbNumPlayers";
			this.m_cbNumPlayers.Size = new System.Drawing.Size(76, 21);
			this.m_cbNumPlayers.TabIndex = 12;
			this.m_cbNumPlayers.SelectedIndexChanged += new System.EventHandler(this.m_cbNumPlayers_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(15, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 16);
			this.label3.TabIndex = 13;
			this.label3.Text = "Number of players:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(15, 112);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(60, 16);
			this.label4.TabIndex = 15;
			this.label4.Text = "Player 4:";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(15, 88);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(60, 16);
			this.label5.TabIndex = 14;
			this.label5.Text = "Player 3:";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(15, 160);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(60, 16);
			this.label6.TabIndex = 17;
			this.label6.Text = "Player 6:";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(15, 136);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(60, 16);
			this.label7.TabIndex = 16;
			this.label7.Text = "Player 5:";
			// 
			// m_btnOK
			// 
			this.m_btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.m_btnOK.Location = new System.Drawing.Point(4, 292);
			this.m_btnOK.Name = "m_btnOK";
			this.m_btnOK.Size = new System.Drawing.Size(75, 23);
			this.m_btnOK.TabIndex = 18;
			this.m_btnOK.Text = "OK";
			this.m_btnOK.Click += new System.EventHandler(this.button1_Click);
			// 
			// m_btnCancel
			// 
			this.m_btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.m_btnCancel.Location = new System.Drawing.Point(92, 292);
			this.m_btnCancel.Name = "m_btnCancel";
			this.m_btnCancel.Size = new System.Drawing.Size(75, 23);
			this.m_btnCancel.TabIndex = 19;
			this.m_btnCancel.Text = "Cancel";
			this.m_btnCancel.Click += new System.EventHandler(this.button2_Click);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(15, 194);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(100, 16);
			this.label8.TabIndex = 20;
			this.label8.Text = "Victory Conditions:";
			// 
			// m_cbVictoryConditions
			// 
			this.m_cbVictoryConditions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_cbVictoryConditions.Location = new System.Drawing.Point(18, 213);
			this.m_cbVictoryConditions.Name = "m_cbVictoryConditions";
			this.m_cbVictoryConditions.Size = new System.Drawing.Size(173, 21);
			this.m_cbVictoryConditions.TabIndex = 21;
			this.m_cbVictoryConditions.SelectedIndexChanged += new System.EventHandler(this.m_cbVictoryConditions_SelectedIndexChanged);
			// 
			// m_nudNumTerritories
			// 
			this.m_nudNumTerritories.Location = new System.Drawing.Point(18, 266);
			this.m_nudNumTerritories.Maximum = new decimal(new int[] {
            42,
            0,
            0,
            0});
			this.m_nudNumTerritories.Minimum = new decimal(new int[] {
            15,
            0,
            0,
            0});
			this.m_nudNumTerritories.Name = "m_nudNumTerritories";
			this.m_nudNumTerritories.Size = new System.Drawing.Size(72, 20);
			this.m_nudNumTerritories.TabIndex = 22;
			this.m_nudNumTerritories.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(15, 247);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(144, 16);
			this.label9.TabIndex = 23;
			this.label9.Text = "Number of territories to win:";
			// 
			// m_tvOptions
			// 
			this.m_tvOptions.CheckBoxes = true;
			this.m_tvOptions.Location = new System.Drawing.Point(221, 40);
			this.m_tvOptions.Name = "m_tvOptions";
			this.m_tvOptions.ShowPlusMinus = false;
			this.m_tvOptions.ShowRootLines = false;
			this.m_tvOptions.Size = new System.Drawing.Size(384, 246);
			this.m_tvOptions.TabIndex = 32;
			this.m_tvOptions.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
			this.m_tvOptions.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
			this.m_tvOptions.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeCollapse);
			this.m_tvOptions.Click += new System.EventHandler(this.treeView1_Click);
			// 
			// m_btnLoadGame
			// 
			this.m_btnLoadGame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_btnLoadGame.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.m_btnLoadGame.Location = new System.Drawing.Point(331, 292);
			this.m_btnLoadGame.Name = "m_btnLoadGame";
			this.m_btnLoadGame.Size = new System.Drawing.Size(75, 23);
			this.m_btnLoadGame.TabIndex = 25;
			this.m_btnLoadGame.Text = "Load Game";
			this.m_btnLoadGame.Click += new System.EventHandler(this.m_btnLoadGame_Click);
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(218, 16);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(75, 13);
			this.label10.TabIndex = 33;
			this.label10.Text = "Game options:";
			// 
			// m_btnNetworkClient
			// 
			this.m_btnNetworkClient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_btnNetworkClient.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.m_btnNetworkClient.Location = new System.Drawing.Point(412, 292);
			this.m_btnNetworkClient.Name = "m_btnNetworkClient";
			this.m_btnNetworkClient.Size = new System.Drawing.Size(144, 23);
			this.m_btnNetworkClient.TabIndex = 34;
			this.m_btnNetworkClient.Text = "Connect to Network Game";
			this.m_btnNetworkClient.Click += new System.EventHandler(this.m_btnNetworkClient_Click);
			// 
			// GameSetupForm
			// 
			this.AcceptButton = this.m_btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(617, 318);
			this.Controls.Add(this.m_btnNetworkClient);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.m_tvOptions);
			this.Controls.Add(this.m_btnLoadGame);
			this.Controls.Add(this.m_tbPlayer3);
			this.Controls.Add(this.m_tbPlayer6);
			this.Controls.Add(this.m_btnCancel);
			this.Controls.Add(this.m_tbPlayer4);
			this.Controls.Add(this.m_btnOK);
			this.Controls.Add(this.m_tbPlayer1);
			this.Controls.Add(this.m_nudNumTerritories);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.m_cbNumPlayers);
			this.Controls.Add(this.m_cbVictoryConditions);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.m_tbPlayer2);
			this.Controls.Add(this.m_tbPlayer5);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "GameSetupForm";
			this.Text = "Buck Rogers Game Setup";
			this.Load += new System.EventHandler(this.GameSetupForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.m_nudNumTerritories)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			GameSetupForm gsf = new GameSetupForm();
			Application.Run(gsf);

			GameOptions go = gsf.Options;
			if(gsf.DialogResult == DialogResult.OK)
			{
				if(go.IsNetworkGame)
				{
					GameLobbyForm glf = new GameLobbyForm();

					Application.Run(glf);

					BuckRogersClient client = glf.GameClient;
					if (client.GameStarted)
					{
						BuckRogersForm brf = null;

						brf = new BuckRogersForm(client, go);

						Application.Run(brf);
					}
				}
				else
				{
					BuckRogersForm brf = null;

					brf = new BuckRogersForm(go, gsf.LoadFileName);

					Application.Run(brf);
				}
				
			}
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			int numPlayers = Int32.Parse((string)m_cbNumPlayers.SelectedItem);
			ArrayList al = new ArrayList();

			string errorMessage = "";

            // If we're testing, we don't care about the players
			if(!Options.OptionalRules["UseTestingSetup"])
			{
				for(int i = 0; i < numPlayers; i++)
				{
					string name = m_tbPlayerNames[i].Text;
					if(al.Contains(name))
					{
						errorMessage = "Can't have two players with the same name";
						goto ErrorMessage;
					}
					al.Add(name);
                    
				}

				if(al.Contains(string.Empty))
				{
					errorMessage = "A player's name cannot be empty";
                    goto ErrorMessage;
				}
			}

            // Need to know the victory condition before we can test other stuff
			string conditionDescription = (string)m_cbVictoryConditions.SelectedItem;
			VictoryConditions vc = (VictoryConditions)Utility.GetEnumFromDescription(conditionDescription, typeof(VictoryConditions));

            m_options.WinningConditions = vc;

            if (vc == VictoryConditions.NumberOfTerritories)
            {
                int numTerritories = (int)m_nudNumTerritories.Value;
                m_options.NumTerritoriesNeeded = numTerritories;
            }

            if (Options.OptionalRules["AllTerritoriesOwned"] 
                && vc == VictoryConditions.NumberOfTerritories)
            {
                if( (numPlayers == 2 && m_options.NumTerritoriesNeeded < 25)
                    || (numPlayers == 3 && m_options.NumTerritoriesNeeded < 20))
                {
                    errorMessage = "Please increase the number of territories needed to win, add more players, or turn off the \"All Territories Owned\" option.";
                    goto ErrorMessage;
                }
            }

        ErrorMessage:
            if (errorMessage != "")
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



			if(m_options.OptionalRules["IncreasedProduction"])
			{
				GameOption optionProduction = (GameOption)m_options.OptionalRules.Get("IncreasedProduction");
				OptionValue turnValue = (OptionValue)optionProduction.Values["startingTurn"];
				OptionValue multiplierValue = (OptionValue)optionProduction.Values["multiplier"];

				m_options.IncreasedProductionTurn = turnValue.Value;
				m_options.ProductionMultiplier = multiplierValue.Value;
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
			string conditionDescription = (string)m_cbVictoryConditions.SelectedItem;
			VictoryConditions vc = (VictoryConditions)Utility.GetEnumFromDescription(conditionDescription, typeof(VictoryConditions));
			
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
		
		/*
		private void m_chklbOptions_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			GameOption option = (GameOption)m_options.OptionalRules[e.Index];

			option.Value = (e.NewValue == CheckState.Checked);

			bool increasedProduction = m_options.OptionalRules["IncreasedProduction"];
		}
		*/

		private void m_btnLoadGame_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Buck Rogers save games (*.xml)|*.xml";

			if(ofd.ShowDialog() == DialogResult.OK)
			{
				m_loadFileName = ofd.FileName;
				this.DialogResult = DialogResult.OK;
				this.Close();
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

		public string LoadFileName
		{
			get { return this.m_loadFileName; }
			set { this.m_loadFileName = value; }
		}

		private void button3_Click(object sender, EventArgs e)
		{
			NumberEntryForm nef = new NumberEntryForm();
			nef.Initialize("This is a really, really long string of text, used to test out the label auto-size functionality in the NumberEntryForm",
							1, 50, 10);

			nef.ShowDialog();
		}

		[DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int SendMessage(IntPtr hwnd, int msg, IntPtr wParam, ref TVITEM lParam);
		private void TreeNode_SetStateImageIndex(TreeNode node, int index)
		{
			TVITEM tvi = new TVITEM();
			tvi.hItem = node.Handle;
			tvi.mask = TVIF_STATE;
			tvi.stateMask = TVIS_STATEIMAGEMASK;
			tvi.state = index << 12;
			SendMessage(node.TreeView.Handle, TVM_SETITEM, IntPtr.Zero, ref tvi);
		}

		public const int TVIF_STATE = 8;
		public const int TVIS_STATEIMAGEMASK = 61440;
		public const int TV_FIRST = 4352;
		public const int TVM_SETITEM = TV_FIRST + 63;
		[StructLayout(LayoutKind.Sequential)]
		public struct TVITEM
		{
			public int mask;
			public IntPtr hItem;
			public int state;
			public int stateMask;
			[MarshalAs(UnmanagedType.LPTStr)]
			public string lpszText;
			public int cchTextMax;
			public int iImage;
			public int iSelectedImage;
			public int cChildren;
			public IntPtr lParam;
		}

		private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if(e.Node.Level < 2)
			{
				return;
			}

			m_selectedNode = e.Node;

			string nodeText = e.Node.Text;
			string ruleName = (string)m_selectedNode.Parent.Tag;
			string valueName = (string)m_selectedNode.Tag;
			GameOption option = (GameOption)m_options.OptionalRules.Get(ruleName);
			OptionValue ov = (OptionValue)option.Values[valueName];

			string[] delimiters = {":"};
			string[] textHalves = nodeText.Split(delimiters, StringSplitOptions.None);
			string labelText = textHalves[0];
			string valueText = textHalves[1].Trim();
			int value = int.Parse(valueText);

			Size size = TextRenderer.MeasureText(labelText, e.Node.NodeFont);

			Point numberLocation = new Point();
			numberLocation.X = e.Node.Bounds.X + size.Width;
			numberLocation.Y = e.Node.Bounds.Y;
			
			m_nudNodeValue.Value = value;
			m_nudNodeValue.Minimum = ov.Min;
			m_nudNodeValue.Maximum = ov.Max;
			
			m_nudNodeValue.Height = e.Node.Bounds.Height;	
			m_nudNodeValue.Location = numberLocation;			
			m_nudNodeValue.Width = 40;

			m_tvOptions.Controls.Add(m_nudNodeValue);
			m_nudNodeValue.Show();

			m_upDownDisplayed = true;
		}

		private void treeView1_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
		{
			e.Cancel = true;
		}

		private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
		{
			if(!(e.Node.Tag is string))
			{
				return;
			}

			string optionName = (string)e.Node.Tag;
			GameOption option = (GameOption)m_options.OptionalRules.Get(optionName);

			option.Value = e.Node.Checked;

			if(option.Values.Count > 0)
			{
				if(e.Node.Checked)
				{
					foreach(OptionValue ov in option.Values)
					{
						TreeNode valueNode = new TreeNode();

						string text = string.Format("{0}: {1}", ov.Description, ov.Start);
						valueNode.Text = text;
						valueNode.Tag = ov.Name;

						e.Node.Nodes.Add(valueNode);

						TreeNode_SetStateImageIndex(valueNode, 0);

					}

					m_tvOptions.ExpandAll();
				}
				else
				{
					e.Node.Nodes.Clear();
				}
			}

			

			if(e.Node.Checked)
			{
				string[] delimiters = { "," };
				string[] exclusions = option.Excludes.Split(delimiters, StringSplitOptions.None);

				foreach (string exclusion in exclusions)
				{
					TreeNode optionNode = (TreeNode)m_optionNodes[exclusion];

					if(optionNode == null)
					{
						continue;
					}

					if (optionNode.Checked)
					{
						optionNode.Checked = false;
					}

				}
			}
			
		}

		private void treeView1_Click(object sender, EventArgs e)
		{
			if(m_upDownDisplayed)
			{
				m_nudNodeValue.Hide();
				m_tvOptions.Controls.Remove(m_nudNodeValue);

				string nodeText = m_selectedNode.Text;
				string[] delimiters = { ":" };
				string[] textHalves = nodeText.Split(delimiters, StringSplitOptions.None);
				string labelText = textHalves[0];

				string text = string.Format("{0}: {1}", labelText, m_nudNodeValue.Value);
				m_selectedNode.Text = text;

				string ruleName = (string)m_selectedNode.Parent.Tag;
				string valueName = (string)m_selectedNode.Tag;
				GameOption option = (GameOption)m_options.OptionalRules.Get(ruleName);
				OptionValue ov = (OptionValue)option.Values[valueName];
				ov.Value = (int)m_nudNodeValue.Value;

				m_selectedNode = null;
				m_upDownDisplayed = false;
			}
		}

		private void m_btnNetworkClient_Click(object sender, EventArgs e)
		{
			m_options = new GameOptions();
			m_options.IsNetworkGame = true;

			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
