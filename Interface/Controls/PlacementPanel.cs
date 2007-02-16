using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using BuckRogers.Interface;

namespace BuckRogers.Interface
{
	/// <summary>
	/// Summary description for PlacementPanel.
	/// </summary>
	public class PlacementPanel : System.Windows.Forms.UserControl
	{
		public event MoveModeChangedHandler MoveModeChanged;

		private GameController m_controller;

		private System.Windows.Forms.Label label2;
		private PlayerListBox m_lbPlayerOrder;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button m_btnPlaceUnits;
		private System.Windows.Forms.Button m_btnCancelPlacement;
		private System.Windows.Forms.ListView m_lvAvailableUnits;
		private bool m_playersSelectUnits;
		private int m_numPlayersFinishedChoosing;
		private Button m_btnChooseUnits;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PlacementPanel()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			m_btnCancelPlacement.Enabled = false;

			
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label2 = new System.Windows.Forms.Label();
			this.m_lbPlayerOrder = new BuckRogers.Interface.PlayerListBox();
			this.m_btnPlaceUnits = new System.Windows.Forms.Button();
			this.m_lvAvailableUnits = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.label1 = new System.Windows.Forms.Label();
			this.m_btnCancelPlacement = new System.Windows.Forms.Button();
			this.m_btnChooseUnits = new System.Windows.Forms.Button();
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
			// m_lbPlayerOrder
			// 
			this.m_lbPlayerOrder.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_lbPlayerOrder.Location = new System.Drawing.Point(92, 4);
			this.m_lbPlayerOrder.Name = "m_lbPlayerOrder";
			this.m_lbPlayerOrder.Size = new System.Drawing.Size(136, 95);
			this.m_lbPlayerOrder.TabIndex = 15;
			// 
			// m_btnPlaceUnits
			// 
			this.m_btnPlaceUnits.Location = new System.Drawing.Point(0, 120);
			this.m_btnPlaceUnits.Name = "m_btnPlaceUnits";
			this.m_btnPlaceUnits.Size = new System.Drawing.Size(75, 23);
			this.m_btnPlaceUnits.TabIndex = 17;
			this.m_btnPlaceUnits.Text = "Place Units";
			this.m_btnPlaceUnits.Click += new System.EventHandler(this.m_btnPlaceUnits_Click);
			// 
			// m_lvAvailableUnits
			// 
			this.m_lvAvailableUnits.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
			this.m_lvAvailableUnits.Location = new System.Drawing.Point(92, 184);
			this.m_lvAvailableUnits.Name = "m_lvAvailableUnits";
			this.m_lvAvailableUnits.Size = new System.Drawing.Size(136, 168);
			this.m_lvAvailableUnits.TabIndex = 18;
			this.m_lvAvailableUnits.UseCompatibleStateImageBehavior = false;
			this.m_lvAvailableUnits.View = System.Windows.Forms.View.Details;
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
			this.label1.Location = new System.Drawing.Point(0, 188);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(84, 16);
			this.label1.TabIndex = 19;
			this.label1.Text = "Units left:";
			// 
			// m_btnCancelPlacement
			// 
			this.m_btnCancelPlacement.Location = new System.Drawing.Point(80, 120);
			this.m_btnCancelPlacement.Name = "m_btnCancelPlacement";
			this.m_btnCancelPlacement.Size = new System.Drawing.Size(75, 23);
			this.m_btnCancelPlacement.TabIndex = 20;
			this.m_btnCancelPlacement.Text = "Cancel";
			this.m_btnCancelPlacement.Click += new System.EventHandler(this.m_btnCancelPlacement_Click);
			// 
			// m_btnChooseUnits
			// 
			this.m_btnChooseUnits.Location = new System.Drawing.Point(0, 149);
			this.m_btnChooseUnits.Name = "m_btnChooseUnits";
			this.m_btnChooseUnits.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.m_btnChooseUnits.Size = new System.Drawing.Size(75, 23);
			this.m_btnChooseUnits.TabIndex = 21;
			this.m_btnChooseUnits.Text = "Select Units";
			this.m_btnChooseUnits.UseVisualStyleBackColor = true;
			this.m_btnChooseUnits.Click += new System.EventHandler(this.m_btnChooseUnits_Click);
			// 
			// PlacementPanel
			// 
			this.Controls.Add(this.m_btnChooseUnits);
			this.Controls.Add(this.m_btnCancelPlacement);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.m_lvAvailableUnits);
			this.Controls.Add(this.m_btnPlaceUnits);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.m_lbPlayerOrder);
			this.Name = "PlacementPanel";
			this.Size = new System.Drawing.Size(240, 600);
			this.ResumeLayout(false);

		}
		#endregion

		private void m_btnPlaceUnits_Click(object sender, System.EventArgs e)
		{
			m_btnPlaceUnits.Enabled = false;
			m_btnCancelPlacement.Enabled = true;

			if(MoveModeChanged != null)
			{
				MoveModeEventArgs mmea = new MoveModeEventArgs();
				mmea.MoveMode = MoveMode.StartPlacement;

				MoveModeChanged(this, mmea);
			}

		}

		public void TerritoryClicked(Territory t, TerritoryEventArgs tcea)
		{
			// show placement form here

			if(t.Owner != m_controller.CurrentPlayer)
			{
				MessageBox.Show("Can't place units in a territory you don't own", "Placement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if(t.Units.Count >= 6 && !m_playersSelectUnits)
			{
				MessageBox.Show("Can only place 6 units in a single territory", "Placement",
								MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			PlacementForm pf = new PlacementForm();
			pf.SetupUnits(m_controller.CurrentPlayer, t);

			pf.ShowDialog();

			if(pf.DialogResult == DialogResult.OK)
			{
				UnitCollection uc = pf.SelectedUnits;

				m_controller.PlaceUnits(uc, t);
				

				if(m_controller.NextPlayer())
				{
					//m_lbPlayerOrder.SelectedIndex++;
					m_lbPlayerOrder.SelectedItem = m_controller.CurrentPlayer;
					RefreshAvailableUnits();					
				}
				else
				{
					RefreshPlayerOrder();
				}
				
				if(m_playersSelectUnits)
				{
					// TODO should probably be a better way to figure out if the player
					// has selected units or not
					//if(m_controller.CurrentPlayer.Units.Count <= 10)
					if(m_numPlayersFinishedChoosing < m_controller.Players.Length)
					{
						m_btnPlaceUnits.Enabled = false;
						m_btnCancelPlacement.Enabled = false;
						m_btnChooseUnits.Enabled = true;
						m_btnChooseUnits.Visible = true;
					}
					else
					{
						m_btnPlaceUnits.Enabled = true;
						m_btnCancelPlacement.Enabled = false;
						m_btnChooseUnits.Visible = false;
					}
				}
				else
				{
					m_btnPlaceUnits.Enabled = true;
					m_btnCancelPlacement.Enabled = false;
				}

				

				if(MoveModeChanged != null)
				{
					MoveModeEventArgs mmea = new MoveModeEventArgs();
					mmea.MoveMode = MoveMode.Finished;

					MoveModeChanged(this, mmea);
				}
			}
		}

		private void m_btnCancelPlacement_Click(object sender, System.EventArgs e)
		{
			m_btnCancelPlacement.Enabled = false;
			m_btnPlaceUnits.Enabled = true;

			if(MoveModeChanged != null)
			{
				MoveModeEventArgs mmea = new MoveModeEventArgs();
				mmea.MoveMode = MoveMode.Finished;

				MoveModeChanged(this, mmea);
			}
		}

		public void RefreshPlayerOrder()
		{
			m_lbPlayerOrder.Items.Clear();

			foreach(Player p in m_controller.PlayerOrder)
			{
				m_lbPlayerOrder.Items.Add(p);
			}

			m_lbPlayerOrder.SelectedIndex = 0;
			//m_lbPlayerOrder.SelectedItem = m_controller.CurrentPlayer;
		}

		public void RefreshAvailableUnits()
		{
			Player p = m_controller.CurrentPlayer;

			UnitCollection uc = p.Units.GetUnits(Territory.NONE);
			Hashtable ht = uc.GetUnitTypeCount();

			m_lvAvailableUnits.Items.Clear();

			foreach(DictionaryEntry de in ht)
			{
				UnitType ut = (UnitType)de.Key;
				int numUnits = (int)de.Value;

				ListViewItem lvi = new ListViewItem();
				lvi.Text = ut.ToString();
				/*
				if(ut == UnitType.Factory)
				{
					numUnits--;
					
					if(numUnits == 0)
					{
						continue;
					}
				}
				*/

				
				lvi.SubItems.Add(numUnits.ToString());

				m_lvAvailableUnits.Items.Add(lvi);
				
			}
	
		}

		public BuckRogers.GameController Controller
		{
			get { return this.m_controller; }
			set { this.m_controller = value; }
		}

		public void Initialize()
		{
			StartingScenarios settings = GameController.Options.SetupOptions;
			m_playersSelectUnits = ((settings & StartingScenarios.PickStartingUnits) == StartingScenarios.PickStartingUnits);

			if(m_playersSelectUnits)
			{
				m_btnChooseUnits.Visible = true;
				m_btnPlaceUnits.Enabled = false;
			}
			else
			{
				m_btnChooseUnits.Visible = false;
				m_btnPlaceUnits.Enabled = true;
			}

			RefreshPlayerOrder();
			RefreshAvailableUnits();
		}

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

			numUnits = 5;
			UnitSelectionForm usf = new UnitSelectionForm(numUnits);
			DialogResult dr = usf.ShowDialog();

			if(dr == DialogResult.Cancel)
			{
				return;
			}

			Player p = m_controller.CurrentPlayer;

			m_controller.CreateUnits(p, UnitType.Fighter, usf.TotalFighters);
			m_controller.CreateUnits(p, UnitType.Transport, usf.TotalTransports);
			m_controller.CreateUnits(p, UnitType.Trooper, usf.TotalTroopers);
			m_controller.CreateUnits(p, UnitType.Gennie, usf.TotalGennies);

			m_numPlayersFinishedChoosing++;

			RefreshAvailableUnits();

			m_btnChooseUnits.Enabled = false;
			m_btnPlaceUnits.Enabled = true;
			int i = 42;
			int q = i;
		}
	}
}
