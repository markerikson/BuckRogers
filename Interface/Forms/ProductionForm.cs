using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace BuckRogers.Interface
{
	/// <summary>
	/// Summary description for ProductionForm.
	/// </summary>
	public class ProductionForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button m_btnFinishProduction;
		private System.Windows.Forms.Button m_btnNextProduction;
		private System.Windows.Forms.Label label16;
		private UnclickableListBox m_lbProductionOrder;
		private System.Windows.Forms.ListView m_lvFactories;
		private System.Windows.Forms.ColumnHeader columnHeader25;
		private System.Windows.Forms.ColumnHeader columnHeader26;
		private System.Windows.Forms.ColumnHeader columnHeader27;
		private System.Windows.Forms.ColumnHeader columnHeader28;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Button m_btnProduce;
		private System.Windows.Forms.ComboBox m_cbNeighbors;
		private System.Windows.Forms.ComboBox m_cbUnitTypes;
		private ColumnHeader columnHeader1;
		private System.Windows.Forms.Button m_btnDismantleFactory;

		private int m_productionIndex;
		private GameController m_controller;
		private UnitCollection m_factories;
		private ListViewColumnSorter m_sorter;
		
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ProductionForm(GameController gc)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.Icon = InterfaceUtility.GetApplicationIcon();

			m_sorter = new ListViewColumnSorter();
			m_lvFactories.ListViewItemSorter = m_sorter;

			m_controller = gc;

			foreach(UnitType ut in Unit.GetBuildableTypes())
			{
				m_cbUnitTypes.Items.Add(ut.ToString());
			}
			m_cbUnitTypes.SelectedIndex = 0;
			m_btnFinishProduction.Enabled = false;

			m_factories = new UnitCollection();

			
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
			this.m_btnFinishProduction = new System.Windows.Forms.Button();
			this.m_btnNextProduction = new System.Windows.Forms.Button();
			this.label16 = new System.Windows.Forms.Label();
			this.m_lbProductionOrder = new BuckRogers.Interface.UnclickableListBox();
			this.m_lvFactories = new System.Windows.Forms.ListView();
			this.columnHeader25 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader26 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader27 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader28 = new System.Windows.Forms.ColumnHeader();
			this.label18 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.m_btnProduce = new System.Windows.Forms.Button();
			this.m_cbNeighbors = new System.Windows.Forms.ComboBox();
			this.m_cbUnitTypes = new System.Windows.Forms.ComboBox();
			this.m_btnDismantleFactory = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// m_btnFinishProduction
			// 
			this.m_btnFinishProduction.Location = new System.Drawing.Point(4, 168);
			this.m_btnFinishProduction.Name = "m_btnFinishProduction";
			this.m_btnFinishProduction.Size = new System.Drawing.Size(75, 23);
			this.m_btnFinishProduction.TabIndex = 11;
			this.m_btnFinishProduction.Text = "Finish";
			this.m_btnFinishProduction.Click += new System.EventHandler(this.m_btnFinishProduction_Click);
			// 
			// m_btnNextProduction
			// 
			this.m_btnNextProduction.Location = new System.Drawing.Point(4, 140);
			this.m_btnNextProduction.Name = "m_btnNextProduction";
			this.m_btnNextProduction.Size = new System.Drawing.Size(76, 23);
			this.m_btnNextProduction.TabIndex = 10;
			this.m_btnNextProduction.Text = "Next player";
			this.m_btnNextProduction.Click += new System.EventHandler(this.m_btnNextProduction_Click);
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(4, 4);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(100, 16);
			this.label16.TabIndex = 9;
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
			this.m_lbProductionOrder.Location = new System.Drawing.Point(4, 20);
			this.m_lbProductionOrder.Name = "m_lbProductionOrder";
			this.m_lbProductionOrder.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.m_lbProductionOrder.Size = new System.Drawing.Size(120, 82);
			this.m_lbProductionOrder.TabIndex = 8;
			// 
			// m_lvFactories
			// 
			this.m_lvFactories.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader25,
            this.columnHeader1,
            this.columnHeader26,
            this.columnHeader27,
            this.columnHeader28});
			this.m_lvFactories.FullRowSelect = true;
			this.m_lvFactories.HideSelection = false;
			this.m_lvFactories.Location = new System.Drawing.Point(128, 4);
			this.m_lvFactories.MultiSelect = false;
			this.m_lvFactories.Name = "m_lvFactories";
			this.m_lvFactories.Size = new System.Drawing.Size(474, 188);
			this.m_lvFactories.TabIndex = 13;
			this.m_lvFactories.UseCompatibleStateImageBehavior = false;
			this.m_lvFactories.View = System.Windows.Forms.View.Details;
			this.m_lvFactories.SelectedIndexChanged += new System.EventHandler(this.m_lvFactories_SelectedIndexChanged);
			this.m_lvFactories.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.m_lvFactories_ColumnClick);
			// 
			// columnHeader25
			// 
			this.columnHeader25.Text = "Territory";
			this.columnHeader25.Width = 129;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Planet";
			this.columnHeader1.Width = 56;
			// 
			// columnHeader26
			// 
			this.columnHeader26.Text = "Producing";
			this.columnHeader26.Width = 70;
			// 
			// columnHeader27
			// 
			this.columnHeader27.Text = "Number";
			this.columnHeader27.Width = 52;
			// 
			// columnHeader28
			// 
			this.columnHeader28.Text = "Destination";
			this.columnHeader28.Width = 133;
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(608, 56);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(100, 16);
			this.label18.TabIndex = 18;
			this.label18.Text = "Place unit in:";
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(608, 4);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(100, 16);
			this.label17.TabIndex = 17;
			this.label17.Text = "Unit to produce:";
			// 
			// m_btnProduce
			// 
			this.m_btnProduce.Location = new System.Drawing.Point(608, 104);
			this.m_btnProduce.Name = "m_btnProduce";
			this.m_btnProduce.Size = new System.Drawing.Size(75, 23);
			this.m_btnProduce.TabIndex = 16;
			this.m_btnProduce.Text = "Produce";
			this.m_btnProduce.Click += new System.EventHandler(this.m_btnProduce_Click);
			// 
			// m_cbNeighbors
			// 
			this.m_cbNeighbors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_cbNeighbors.Location = new System.Drawing.Point(608, 72);
			this.m_cbNeighbors.Name = "m_cbNeighbors";
			this.m_cbNeighbors.Size = new System.Drawing.Size(153, 21);
			this.m_cbNeighbors.TabIndex = 15;
			// 
			// m_cbUnitTypes
			// 
			this.m_cbUnitTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_cbUnitTypes.Location = new System.Drawing.Point(608, 20);
			this.m_cbUnitTypes.Name = "m_cbUnitTypes";
			this.m_cbUnitTypes.Size = new System.Drawing.Size(153, 21);
			this.m_cbUnitTypes.TabIndex = 14;
			this.m_cbUnitTypes.SelectedIndexChanged += new System.EventHandler(this.m_cbUnitTypes_SelectedIndexChanged);
			// 
			// m_btnDismantleFactory
			// 
			this.m_btnDismantleFactory.Location = new System.Drawing.Point(608, 168);
			this.m_btnDismantleFactory.Name = "m_btnDismantleFactory";
			this.m_btnDismantleFactory.Size = new System.Drawing.Size(75, 23);
			this.m_btnDismantleFactory.TabIndex = 19;
			this.m_btnDismantleFactory.Text = "Dismantle";
			this.m_btnDismantleFactory.Click += new System.EventHandler(this.m_btnDismantleFactory_Click);
			// 
			// ProductionForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(768, 202);
			this.ControlBox = false;
			this.Controls.Add(this.m_btnDismantleFactory);
			this.Controls.Add(this.label18);
			this.Controls.Add(this.label17);
			this.Controls.Add(this.m_btnProduce);
			this.Controls.Add(this.m_cbNeighbors);
			this.Controls.Add(this.m_cbUnitTypes);
			this.Controls.Add(this.m_lvFactories);
			this.Controls.Add(this.m_btnFinishProduction);
			this.Controls.Add(this.m_btnNextProduction);
			this.Controls.Add(this.label16);
			this.Controls.Add(this.m_lbProductionOrder);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "ProductionForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Production";
			this.ResumeLayout(false);

		}


		#endregion


		private void m_cbUnitTypes_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(m_lvFactories.SelectedIndices.Count > 0)
			{
				FindValidProductionTerritories();
			}
		}


		// TODO Black Market production
		
		private void m_btnFinishProduction_Click(object sender, System.EventArgs e)
		{
			AddProduction();
			m_controller.ExecuteProduction();
			
			//this.DialogResult = DialogResult.OK;
			this.Hide();
			m_controller.CheckNextPhase();
		}

		private void m_btnNextProduction_Click(object sender, System.EventArgs e)
		{
			
			NextProduction();
		}

		private void m_btnProduce_Click(object sender, System.EventArgs e)
		{
			if(m_lvFactories.SelectedIndices.Count > 0)
			{
				int idxSelectedFactory = m_lvFactories.SelectedIndices[0];
				ListViewItem lvi = m_lvFactories.Items[idxSelectedFactory];

				string territoryName = lvi.Text;
				string typeName = (string)m_cbUnitTypes.SelectedItem;
				string destinationName = (string)m_cbNeighbors.SelectedItem;

				string playerName = (string)m_lbProductionOrder.SelectedItem;
				Player p = m_controller.GetPlayer(playerName);

				Territory t = (Territory)m_controller.Map[territoryName];
				Territory destination = m_controller.Map[destinationName];
				UnitType ut = (UnitType)Enum.Parse(typeof(UnitType), typeName);
				UnitCollection factories = t.Units.GetUnits(UnitType.Factory, p, null);

				// TODO This spot worries me... what if there's more than one factory?
				Factory f = (Factory)factories[0];

				ProductionInfo pi = new ProductionInfo();
				pi.Factory = f;
				pi.Type = ut;
				pi.DestinationTerritory = destination;

				bool setProduction = false;

			ValidateProduction:
				try
				{
					setProduction = m_controller.CheckProduction(pi);
					
				}
				catch(Exception ex)
				{
					if(pi.Factory.UnitHalfProduced)
					{
						DialogResult dr = MessageBox.Show("Really cancel the current production?", "Change Production?",
														MessageBoxButtons.YesNo, MessageBoxIcon.Question);

						if(dr == DialogResult.Yes)
						{
							pi.Factory.ClearProduction();
							goto ValidateProduction;
						}
					}
					else
					{
						MessageBox.Show(ex.Message, "Production Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
					
				}
				finally
				{
					if(setProduction)
					{
						f.StartProduction(ut, destination);

						lvi.SubItems[2].Text = typeName;
						lvi.SubItems[3].Text = f.AmountProduced.ToString();
						lvi.SubItems[4].Text = destinationName;

						m_lvFactories.SelectedIndices.Remove(idxSelectedFactory);

						if(idxSelectedFactory < m_lvFactories.Items.Count - 1)
						{
							m_lvFactories.SelectedIndices.Add(idxSelectedFactory + 1);
						}
					}
				}
			}
		}

		private void m_lvFactories_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(m_lvFactories.SelectedIndices.Count > 0)
			{
				FindValidProductionTerritories();				

				m_cbNeighbors.SelectedIndex = 0;

				m_btnProduce.Enabled = true;
				m_btnDismantleFactory.Enabled = true;
			}
			
		}

		private void FindValidProductionTerritories()
		{
			int idxUnused = m_lvFactories.SelectedIndices[0];
			ListViewItem lvi = m_lvFactories.Items[idxUnused];

			string territoryName = lvi.SubItems[0].Text;
			Territory t = m_controller.Map[territoryName];

			if(t == null)
			{
				return;
			}

			Factory f = (Factory)m_factories.GetUnits(t)[0];//(Factory)m_factories[idxUnused];


			string playerName = (string)m_lbProductionOrder.SelectedItem;
			Player p = m_controller.GetPlayer(playerName);

			string unitTypeName = (string)m_cbUnitTypes.SelectedItem;
			UnitType unitType = (UnitType)Enum.Parse(typeof(UnitType), unitTypeName);


			ArrayList names = new ArrayList();

			if (f.IsBlackMarket)
			{
				foreach (DictionaryEntry de in p.Territories)
				{
					Territory playerTerritory = (Territory)de.Value;
					names.Add(playerTerritory.Name);
				}
			}
			else
			{
				switch(unitType)
				{
					case UnitType.Trooper:
					case UnitType.Gennie:
					case UnitType.Fighter:
					case UnitType.Transport:
					{
						names.Add(f.CurrentTerritory.Name);
						break;
					}
					case UnitType.Battler:
					case UnitType.KillerSatellite:
					{
						foreach (Territory neighbor in t.Neighbors)
						{
							if (neighbor.Type == TerritoryType.Space)
							{
								names.Add(neighbor.Key);
							}
						}
						break;
					}
					case UnitType.Factory:
					{
						names.Add(f.CurrentTerritory.Name);

						foreach (Territory neighbor in t.Neighbors)
						{
							if ((neighbor.Owner == p))
							{
								names.Add(neighbor.Key);
							}
						}
						break;
					}
				}
			}

			string[] namearray = (string[])names.ToArray(typeof(string));
			Array.Sort(namearray);

			m_cbNeighbors.Items.Clear();

			/*
			if (territoryName != "Black Market")
			{
				m_cbNeighbors.Items.Add(territoryName);
			}
			*/

			foreach (string Name in namearray)
			{
				m_cbNeighbors.Items.Add(Name);
			}

			m_cbNeighbors.SelectedIndex = 0;
		}


		public void SetupProduction()
		{
			m_lbProductionOrder.Items.Clear();
			m_lvFactories.Items.Clear();
			
			m_btnFinishProduction.Enabled = false;

			foreach(Player p in m_controller.PlayerOrder)
			{
				if(!p.Disabled)
				{
					m_lbProductionOrder.Items.Add(p.Name);
				}
				
			}

			if(m_lbProductionOrder.Items.Count > 0)
			{
				m_productionIndex = -1;

				m_btnProduce.Enabled = false;
				m_btnNextProduction.Enabled = true;

				NextProduction();
			}
			else
			{
				string error = "No players are able to produce!";
				MessageBox.Show(error, "Production", MessageBoxButtons.OK, MessageBoxIcon.Warning);

				m_btnFinishProduction.Enabled = true;
				m_btnNextProduction.Enabled = false;
			}			
		}

		private void AddProduction()
		{
			
			m_lvFactories.Items.Clear();

			string playerName = (string)m_lbProductionOrder.SelectedItem;
			Player p = m_controller.GetPlayer(playerName);

			m_controller.CheckBlackMarket(p);
			UnitCollection allFactories = p.Units.GetUnits(UnitType.Factory);

			UnitCollection usableFactories = new UnitCollection();
			m_factories = new UnitCollection();

			foreach(Factory f in allFactories)
			{
				if(f.Transported)
				{
					continue;
				}

				if(f.CanProduce)
				{
					usableFactories.AddUnit(f);
				}
			}

			foreach(Factory f in usableFactories)
			{
				ListViewItem lvi = new ListViewItem();

				lvi.Text = f.CurrentTerritory.Name;
				lvi.SubItems.Add(f.CurrentTerritory.System.Name);
				lvi.SubItems.Add(f.ProductionType.ToString());
				lvi.SubItems.Add(f.AmountProduced.ToString());
				lvi.SubItems.Add(f.DestinationTerritory.Name);

				m_lvFactories.Items.Add(lvi);

				m_factories.AddUnit(f);
			}

			if(usableFactories.Count > 0)
			{
				m_lvFactories.Items[0].Selected = true;
			}
			else
			{
				m_cbNeighbors.Items.Clear();
			}
		}
		private void NextProduction()
		{
			bool anyNonProducingFactories = false;

			for (int i = 0; i < m_factories.Count; i++)
			{
				Factory f = (Factory)m_factories[i];

				if(f.ProductionType == UnitType.None)
				{
					anyNonProducingFactories = true;
					break;
				}
			}

			if(anyNonProducingFactories)
			{
				string message = "Some of your factories aren't producing anything.  Are you sure you want to ignore them?";
				DialogResult dr = MessageBox.Show(message, "Unused Factories", 
												MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				if(dr == DialogResult.No)
				{
					return;
				}
			}

			m_lvFactories.Items.Clear();
			m_productionIndex++;

			if(m_productionIndex < m_lbProductionOrder.Items.Count)
			{
				m_lbProductionOrder.SelectedIndex = m_productionIndex;
				AddProduction();
			}
			
			if(m_productionIndex == (m_lbProductionOrder.Items.Count - 1))
			{
				m_btnNextProduction.Enabled = false;
				//m_btnProduce.Enabled = true;
				m_btnFinishProduction.Enabled = true;
			}

			m_sorter.SortColumn = 1;
			m_sorter.Order = SortOrder.Ascending;
			m_lvFactories.Sort();
			
		}

		private void m_btnDismantleFactory_Click(object sender, System.EventArgs e)
		{
			if(m_lvFactories.SelectedIndices.Count > 0)
			{
				int idxUnused = m_lvFactories.SelectedIndices[0];
				ListViewItem lvi = m_lvFactories.Items[idxUnused];

				string territoryName = lvi.Text;
				string typeName = (string)m_cbUnitTypes.SelectedItem;
				string destinationName = (string)m_cbNeighbors.SelectedItem;


				if(territoryName == "Black Market")
				{
					MessageBox.Show("Can't dismantle the Black Market", "Production", MessageBoxButtons.OK,
									MessageBoxIcon.Information);
					return;
				}

				DialogResult dr = MessageBox.Show("Are you REALLY sure you want to dismantle the factory in " + territoryName + "?",
								"Dismantle Factory?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

				if(dr != DialogResult.Yes)
				{
					return;
				}

				Territory t = m_controller.Map[territoryName];
				Territory destination = m_controller.Map[destinationName];
				UnitType ut = (UnitType)Enum.Parse(typeof(UnitType), typeName);
				UnitCollection factories = t.Units.GetUnits(UnitType.Factory);

				// TODO This spot worries me... what if there's more than one factory?
				Factory f = (Factory)factories[0];

				f.Destroy();

				m_lvFactories.Items.Remove(lvi);
				m_factories.RemoveUnit(f);

				string playerName = (string)m_lbProductionOrder.SelectedItem;
				Player p = m_controller.GetPlayer(playerName);

				if(m_controller.CheckBlackMarket(p))
				{
					AddProduction();
				}				
			}
		}

		private void m_lvFactories_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			ListView myListView = (ListView)sender;

			// Determine if clicked column is already the column that is being sorted.
			if (e.Column == m_sorter.SortColumn)
			{
				// Reverse the current sort direction for this column.
				if (m_sorter.Order == SortOrder.Ascending)
				{
					m_sorter.Order = SortOrder.Descending;
				}
				else
				{
					m_sorter.Order = SortOrder.Ascending;
				}
			}
			else
			{
				// Set the column number that is to be sorted; default to ascending.
				m_sorter.SortColumn = e.Column;
				m_sorter.Order = SortOrder.Ascending;
			}

			// Perform the sort with these new sort options.
			myListView.Sort();
		}
	}
}
