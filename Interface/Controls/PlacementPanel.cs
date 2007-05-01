#define DEBUGUNITSELECTION

#region using directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using BuckRogers.Interface;
using BuckRogers.Networking;
using CommandManagement;

#endregion



namespace BuckRogers.Interface
{
	public class PlacementPanel : System.Windows.Forms.UserControl
	{
		#region private members
		//public event MoveModeChangedHandler MoveModeChanged;
		public event EventHandler<MoveModeEventArgs> MoveModeChanged;

		private System.ComponentModel.Container components = null;

		private GameController m_controller;
		private IconManager m_iconManager;
		private ClientSideGameManager m_csgm;

		private Hashtable m_playerImages;
		private Hashtable m_selectedUnitCounts;
		private UnitCollection m_ucPlacedUnits;

		private List<Player> m_playersFinishedChoosing;

		private System.Windows.Forms.Label label2;
		private PlayerListBox m_lbPlayerOrder;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListView m_lvAvailableUnits;
		private bool m_playersSelectUnits;
		private Button m_btnChooseUnits;
		private ListView m_lvUnitsPlaced;
		private Label label3;
		private Label label4;
		private Label m_lblPlacementTerritory;
		private bool m_clickOnEmptySpace;
		private Territory m_activeTerritory;
		private int m_idxSelectedUnit;

		private Keys[] m_unitKeys = { Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6 };

		#endregion

		#region properties
		public Keys[] UnitKeys
		{
			get { return m_unitKeys; }
			set { m_unitKeys = value; }
		}

		public IconManager IconManager
		{
			get { return m_iconManager; }
			set { m_iconManager = value; }
		}

		public BuckRogers.GameController GameController
		{
			get { return this.m_controller; }
			set { this.m_controller = value; }
		}

		internal ClientSideGameManager GameManager
		{
			get { return m_csgm; }
			set 
			{ 
				m_csgm = value;

				m_csgm.ClientUpdateMessage += new EventHandler<ClientUpdateEventArgs>(OnClientUpdateMessage);
			}
		}

		public List<Player> PlayersFinishedChoosing
		{
			get { return m_playersFinishedChoosing; }
			set { m_playersFinishedChoosing = value; }
		}

		#endregion

		#region P/Invokes

		[DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int SendMessage(IntPtr hwnd, int msg, int wParam, int lParam);

		#endregion

		#region constructor

		public PlacementPanel()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			m_selectedUnitCounts = new Hashtable();
			m_playersFinishedChoosing = new List<Player>();
			m_ucPlacedUnits = new UnitCollection();

			int LVM_SETICONSPACING = 4149;
			int width = 70;
			int height = 80;
			int iconSpacing = Utility.MakeLong(width, height);

			SendMessage(m_lvAvailableUnits.Handle, LVM_SETICONSPACING, 0, iconSpacing);
			SendMessage(m_lvUnitsPlaced.Handle, LVM_SETICONSPACING, 0, iconSpacing);

			ClientSideGameManager.CommandManager.RegisterCommandExecutor("System.Windows.Forms.Control", 
																			new ControlCommandExecutor());
			ClientSideGameManager.CommandManager.RegisterCommandExecutor("System.Windows.Forms.ListView", 
																			new ListViewCommandExecutor());

			Command selectUnitsCommand = new Command("PlacementSelectUnits", null,
							new Command.UpdateHandler(UpdateSelectUnitsCommand));
			ClientSideGameManager.CommandManager.Commands.Add(selectUnitsCommand);

			selectUnitsCommand.CommandInstances.Add(m_btnChooseUnits);
			selectUnitsCommand.CommandInstances.Add(m_lvAvailableUnits);

		}

		private void UpdateSelectUnitsCommand(Command command)
		{
			command.Enabled = ClientSideGameManager.IsLocalOrActive;
		}


		#endregion

		#region plumbing

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label2 = new System.Windows.Forms.Label();
			this.m_lvAvailableUnits = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.label1 = new System.Windows.Forms.Label();
			this.m_btnChooseUnits = new System.Windows.Forms.Button();
			this.m_lvUnitsPlaced = new System.Windows.Forms.ListView();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.m_lblPlacementTerritory = new System.Windows.Forms.Label();
			this.m_lbPlayerOrder = new BuckRogers.Interface.PlayerListBox();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(4, 4);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(84, 16);
			this.label2.TabIndex = 16;
			this.label2.Text = "Player order:";
			// 
			// m_lvAvailableUnits
			// 
			this.m_lvAvailableUnits.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
			this.m_lvAvailableUnits.HideSelection = false;
			this.m_lvAvailableUnits.Location = new System.Drawing.Point(6, 136);
			this.m_lvAvailableUnits.MultiSelect = false;
			this.m_lvAvailableUnits.Name = "m_lvAvailableUnits";
			this.m_lvAvailableUnits.Size = new System.Drawing.Size(221, 180);
			this.m_lvAvailableUnits.TabIndex = 18;
			this.m_lvAvailableUnits.UseCompatibleStateImageBehavior = false;
			this.m_lvAvailableUnits.KeyDown += new System.Windows.Forms.KeyEventHandler(this.m_lvAvailableUnits_KeyDown);
			this.m_lvAvailableUnits.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.m_lvAvailableUnits_ItemSelectionChanged);
			this.m_lvAvailableUnits.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_lvAvailableUnits_MouseDown);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Type";
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Count";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(6, 117);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(84, 16);
			this.label1.TabIndex = 19;
			this.label1.Text = "Units left:";
			// 
			// m_btnChooseUnits
			// 
			this.m_btnChooseUnits.Location = new System.Drawing.Point(3, 44);
			this.m_btnChooseUnits.Name = "m_btnChooseUnits";
			this.m_btnChooseUnits.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.m_btnChooseUnits.Size = new System.Drawing.Size(75, 23);
			this.m_btnChooseUnits.TabIndex = 21;
			this.m_btnChooseUnits.Text = "Select Units";
			this.m_btnChooseUnits.UseVisualStyleBackColor = true;
			this.m_btnChooseUnits.Click += new System.EventHandler(this.m_btnChooseUnits_Click);
			// 
			// m_lvUnitsPlaced
			// 
			this.m_lvUnitsPlaced.Location = new System.Drawing.Point(6, 412);
			this.m_lvUnitsPlaced.Name = "m_lvUnitsPlaced";
			this.m_lvUnitsPlaced.Size = new System.Drawing.Size(222, 100);
			this.m_lvUnitsPlaced.TabIndex = 22;
			this.m_lvUnitsPlaced.UseCompatibleStateImageBehavior = false;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(6, 390);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(86, 16);
			this.label3.TabIndex = 23;
			this.label3.Text = "Units placed:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(4, 329);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(128, 16);
			this.label4.TabIndex = 24;
			this.label4.Text = "Placement Territory:";
			// 
			// m_lblPlacementTerritory
			// 
			this.m_lblPlacementTerritory.AutoSize = true;
			this.m_lblPlacementTerritory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_lblPlacementTerritory.Location = new System.Drawing.Point(4, 345);
			this.m_lblPlacementTerritory.Name = "m_lblPlacementTerritory";
			this.m_lblPlacementTerritory.Size = new System.Drawing.Size(41, 16);
			this.m_lblPlacementTerritory.TabIndex = 25;
			this.m_lblPlacementTerritory.Text = "None";
			// 
			// m_lbPlayerOrder
			// 
			this.m_lbPlayerOrder.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_lbPlayerOrder.Location = new System.Drawing.Point(92, 4);
			this.m_lbPlayerOrder.Name = "m_lbPlayerOrder";
			this.m_lbPlayerOrder.ShowPlayerLocation = false;
			this.m_lbPlayerOrder.Size = new System.Drawing.Size(136, 95);
			this.m_lbPlayerOrder.TabIndex = 15;
			// 
			// PlacementPanel
			// 
			this.Controls.Add(this.m_lblPlacementTerritory);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.m_lvUnitsPlaced);
			this.Controls.Add(this.m_btnChooseUnits);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.m_lvAvailableUnits);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.m_lbPlayerOrder);
			this.Name = "PlacementPanel";
			this.Size = new System.Drawing.Size(240, 600);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		#endregion

		#region TerritoryClicked

		public void TerritoryClicked(Territory t, TerritoryEventArgs tcea)
		{
			string errorMessage = string.Empty;
			if(t.Owner != m_controller.CurrentPlayer)
			{
				errorMessage = "Can't place units in a territory you don't own";
			}

			if(t.Units.Count >= 6 && !m_playersSelectUnits && errorMessage == string.Empty)
			{
				errorMessage = "Can only place 6 units in a single territory";
			}

			if(m_lvAvailableUnits.SelectedIndices.Count == 0 && errorMessage == string.Empty)
			{
				errorMessage = "Must have a unit type selected in order to place units";
			}

			if(m_activeTerritory != null && t != m_activeTerritory && errorMessage == string.Empty)
			{
				errorMessage = "Can't place units in multiple territories in one turn";
			}			

			if(errorMessage != string.Empty)
			{
				MessageBox.Show(errorMessage, "Placement Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if(tcea.Button == MouseButtons.Left)
			{
				// User previously placed units, then decided he wasn't finished
				if (m_ucPlacedUnits.Count >= 3)
				{
					goto PlacementCompleted;
				}

				ListViewItem selectedItem = m_lvAvailableUnits.SelectedItems[0];
				UnitType ut = (UnitType)selectedItem.Tag;

				int totalUnitsPlaced = m_ucPlacedUnits.Count;
				int numThisTypePlaced = 0;

				if (m_selectedUnitCounts.ContainsKey(ut))
				{
					numThisTypePlaced = (int)m_selectedUnitCounts[ut];
				}

				// 1 by default - consider double-clicking a special case
				int numToAdd = 1;

				// TODO Maybe let players place more than 3 units at a time?
				if(tcea.DoubleClick)
				{
					numToAdd = 3 - totalUnitsPlaced;
				}

				Player p = m_controller.CurrentPlayer;
				UnitCollection uc = p.Units.GetUnits(ut, p, Territory.NONE, numToAdd);

				m_controller.PlaceUnits(uc, t);

				m_ucPlacedUnits.AddAllUnits(uc);

				foreach(Unit u in uc)
				{
					ListViewItem lvi = new ListViewItem();

					lvi.Text = ut.ToString();
					lvi.ImageKey = ut.ToString();
					lvi.Tag = ut;

					m_lvUnitsPlaced.Items.Add(lvi);
					numThisTypePlaced++;
				}

				m_selectedUnitCounts[ut] = numThisTypePlaced;

				m_activeTerritory = t;
				m_lblPlacementTerritory.Text = t.Name;

				UnitCollection remainingUnits = p.Units.GetUnits(ut, p, Territory.NONE, p.Units.Count);
				selectedItem.Text = string.Format("{0}: {1}", ut, remainingUnits.Count);				
			}
			else if(tcea.Button == MouseButtons.Right)
			{
				// Should never be a double-right-click here, since that's
				// handled by BuckRogersForm.  So, we're removing units.
				// Only question is, how many?

				UnitCollection unitsToRemove = new UnitCollection();

				// remove everything
				if( (tcea.Modifiers & Keys.Shift) == Keys.Shift)
				{
					foreach(DictionaryEntry de in m_selectedUnitCounts)
					{
						UnitType ut = (UnitType)de.Key;
						int numUnits = (int)de.Value;

						UnitCollection uc = m_activeTerritory.Units.GetUnits(ut, m_controller.CurrentPlayer, 
																			m_activeTerritory, numUnits);

						unitsToRemove.AddAllUnits(uc);

						
					}

					m_ucPlacedUnits.Clear();
					m_lvUnitsPlaced.Items.Clear();

				}
				// just remove the most recent
				else
				{
					Unit lastPlacedUnit = m_ucPlacedUnits[m_ucPlacedUnits.Count - 1];
					/*
					int index = m_lvUnitsPlaced.Items.Count;
					ListViewItem lvi = m_lvUnitsPlaced.Items[index - 1];

					UnitType ut = (UnitType)lvi.Tag;

					UnitCollection uc = m_activeTerritory.Units.GetUnits(ut, m_controller.CurrentPlayer,
																		m_activeTerritory, 1);
					*/

					unitsToRemove.AddUnit(lastPlacedUnit);
					m_ucPlacedUnits.RemoveUnit(lastPlacedUnit);

					int index = m_lvUnitsPlaced.Items.Count;
					m_lvUnitsPlaced.Items.Remove(m_lvUnitsPlaced.Items[index - 1]);
				}

				m_controller.RemoveUnits(unitsToRemove, m_activeTerritory);

				if(m_ucPlacedUnits.Count == 0)
				{
					m_activeTerritory = null;
					m_lblPlacementTerritory.Text = "None";
				}
			}
			else
			{
				// ignore middle-clicks
				return;
			}


		PlacementCompleted:
			// User has finished placing stuff

			UnitCollection totalRemainingUnits = Territory.NONE.Units.GetUnits(m_controller.CurrentPlayer);

			if (m_ucPlacedUnits.Count == 3 || totalRemainingUnits.Count == 0)
			{
				DialogResult dr = MessageBox.Show("Placement completed?", "Placement", 
													MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				if(dr == DialogResult.Yes)
				{
					m_lvUnitsPlaced.Items.Clear();
					
					
					m_lblPlacementTerritory.Text = "None";

					m_csgm.PlayerPlacedUnits(m_controller.CurrentPlayer, m_activeTerritory, m_ucPlacedUnits);

					m_ucPlacedUnits.Clear();
					m_activeTerritory = null;

					/*
					if (m_controller.NextPlayer())
					{
						
					}
					else
					{
						RefreshPlayerOrder();
					}
					*/
					
				}
			}
		}

		#endregion

		#region Updates

		public void RefreshPlayerOrder()
		{
			m_lbPlayerOrder.Items.Clear();

			foreach(Player p in m_controller.PlayerOrder)
			{
				m_lbPlayerOrder.Items.Add(p);
			}

			m_lbPlayerOrder.SelectedIndex = 0;
		}

		public void RefreshAvailableUnits()
		{
			Player p = m_controller.CurrentPlayer;
			ImageList il = (ImageList)m_playerImages[p];
			m_lvAvailableUnits.LargeImageList = il;
			m_lvUnitsPlaced.LargeImageList = il;

			UnitCollection uc = p.Units.GetUnits(Territory.NONE);
			Hashtable ht = uc.GetUnitTypeCount();

			m_lvAvailableUnits.Items.Clear();

			foreach(DictionaryEntry de in ht)
			{
				UnitType ut = (UnitType)de.Key;
				int numUnits = (int)de.Value;

				ListViewItem lvi = new ListViewItem();

				string text = string.Format("{0}: {1}", ut, numUnits); 
				lvi.Text = text;
				lvi.ImageKey = ut.ToString();
				lvi.SubItems.Add(numUnits.ToString());
				lvi.Tag = ut;

				m_lvAvailableUnits.Items.Add(lvi);				
			}

			m_lvAvailableUnits.Focus();

			m_idxSelectedUnit = 0;
			m_lvAvailableUnits.SelectedIndices.Add(m_idxSelectedUnit);
		}

		#endregion

		#region initialization

		public void Initialize()
		{
			m_playersSelectUnits = GameController.Options.OptionalRules["PickStartingUnits"];

			m_btnChooseUnits.Visible = m_playersSelectUnits;

			m_playerImages = new Hashtable();

			foreach(Player p in m_controller.Players)
			{
				Hashtable ht = m_iconManager.GetPlayerIcons(p);

				ImageList il = new ImageList();
				il.ImageSize = new Size(48, 48);

				foreach (UnitType ut in Enum.GetValues(typeof(UnitType)))
				{
					if (ht.ContainsKey(ut))
					{
						Bitmap b = (Bitmap)ht[ut];

						il.Images.Add(ut.ToString(), b);
					}
				}

				m_playerImages[p] = il;
			}			

			if(GameController.Options.IsNetworkGame)
			{
				m_lbPlayerOrder.ShowPlayerLocation = true;
			}

			RefreshPlayerOrder();
			RefreshAvailableUnits();

			m_csgm.ReadyToStartPlacement();

			/*
			if(!m_playersSelectUnits && !GameController.Options.IsNetworkGame)
			{
				if (MoveModeChanged != null)
				{
					MoveModeEventArgs mmea = new MoveModeEventArgs();
					mmea.MoveMode = MoveMode.StartPlacement;

					MoveModeChanged(this, mmea);
				}
			}
			*/
		
		}

		#endregion

		#region event handlers

		private void m_btnChooseUnits_Click(object sender, EventArgs e)
		{
			int numUnits = 15;

			if(m_controller.Players.Length < 4 
				&& !GameController.Options.OptionalRules["LimitedTwoPlayerSetup"])
			{
				if(m_controller.Players.Length == 2)
				{
					numUnits = 45;
				}
				else if(m_controller.Players.Length == 3)
				{
					numUnits = 30;
				}
			}

#if DEBUGUNITSELECTION
			numUnits = 3;
#endif

			UnitSelectionForm usf = new UnitSelectionForm(numUnits);
			DialogResult dr = usf.ShowDialog();

			if(dr == DialogResult.Cancel)
			{
				return;
			}

			m_csgm.PlayerChoseUnits(m_controller.CurrentPlayer, usf.TotalTroopers, usf.TotalFighters, 
									usf.TotalGennies, usf.TotalTransports);

			m_btnChooseUnits.Enabled = false;

			RefreshAvailableUnits();

			/*
			if (MoveModeChanged != null)
			{
				MoveModeEventArgs mmea = new MoveModeEventArgs();
				mmea.MoveMode = MoveMode.StartPlacement;

				MoveModeChanged(this, mmea);
			}
			*/
		}

		internal void KeyPressed(Keys keyCode)
		{
			// keyCode should be guaranteed to be D1..D6

			int index = Array.IndexOf(m_unitKeys, keyCode);

			if(m_lvAvailableUnits.Items.Count > index)
			{
				m_idxSelectedUnit = index;
				m_lvAvailableUnits.SelectedIndices.Clear();
				m_lvAvailableUnits.SelectedIndices.Add(m_idxSelectedUnit);
				
			}
		}

		private void m_lvAvailableUnits_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			if(!e.IsSelected && m_clickOnEmptySpace && !m_lvAvailableUnits.SelectedIndices.Contains(m_idxSelectedUnit))
			{
				m_lvAvailableUnits.SelectedIndices.Clear();
				m_lvAvailableUnits.SelectedIndices.Add(m_idxSelectedUnit);
			}
		}

		private void m_lvAvailableUnits_MouseDown(object sender, MouseEventArgs e)
		{
			ListViewItem item = m_lvAvailableUnits.GetItemAt(e.X, e.Y);

			m_clickOnEmptySpace = (item == null);
		}

		private void m_lvAvailableUnits_KeyDown(object sender, KeyEventArgs e)
		{
			e.Handled = true;
			e.SuppressKeyPress = true;

			if (Array.IndexOf(this.UnitKeys, e.KeyCode) != -1)
			{
				KeyPressed(e.KeyCode);
			}
		}

		#endregion

		#region OnClientUpdateMessage

		void OnClientUpdateMessage(object sender, ClientUpdateEventArgs e)
		{
			switch(e.MessageType)
			{
				case GameMessage.NextPlayer:
				{
					if(m_playersFinishedChoosing.Contains(m_controller.CurrentPlayer))
					{
						m_btnChooseUnits.Visible = false;
					}

					m_lbPlayerOrder.SelectedItem = m_controller.CurrentPlayer;
					RefreshAvailableUnits();

					goto case GameMessage.PlacementPhaseStarted;
				}
				case GameMessage.PlacementPhaseStarted:				
				{
					bool showChooseUnitsButton = (m_playersSelectUnits &&
													(m_playersFinishedChoosing.Count < m_controller.Players.Length));

					m_btnChooseUnits.Visible = showChooseUnitsButton;


					MoveModeEventArgs mmea = new MoveModeEventArgs();

					if(m_controller.CurrentPlayer.Location == PlayerLocation.Local)
					{
						mmea.MoveMode = MoveMode.StartPlacement;
					}
					else
					{
						mmea.MoveMode = MoveMode.None;
					}

					EventsHelper.Fire(MoveModeChanged, this, mmea);
					//MoveModeChanged(this, mmea);


					break;
				}
				case GameMessage.PlayerChoseUnits:
				{
					m_playersFinishedChoosing.Add(e.Players[0]);

					RefreshAvailableUnits();

					
					break;
				}
				case GameMessage.PlacementPhaseEnded:
				{
					m_csgm.ClientUpdateMessage -= new EventHandler<ClientUpdateEventArgs>(OnClientUpdateMessage);
					break;
				}
			}
		}

		#endregion
	}
}
