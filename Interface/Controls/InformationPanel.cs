using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace BuckRogers.Interface
{
	/// <summary>
	/// Summary description for InformationPanel.
	/// </summary>
	public class InformationPanel : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.ColumnHeader columnHeader7;
		private System.Windows.Forms.ColumnHeader columnHeader8;
		private System.Windows.Forms.ListView m_lvTotalUnits;
		private System.Windows.Forms.ListView m_lvUnitLocations;
		private System.Windows.Forms.ColumnHeader columnHeader9;
		private System.Windows.Forms.ColumnHeader columnHeader10;
		private System.Windows.Forms.ColumnHeader columnHeader11;
		private System.Windows.Forms.Label label3;
		private GameController m_controller;
		private System.Windows.Forms.ListView m_lvTerritories;
		private ListViewColumnSorter[] m_lvcs;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public InformationPanel()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			m_lvcs = new ListViewColumnSorter[3];
			for(int i = 0; i < 3; i++)
			{
				m_lvcs[i] = new ListViewColumnSorter();
			}

			m_lvTerritories.ListViewItemSorter = m_lvcs[0];
			m_lvTotalUnits.ListViewItemSorter = m_lvcs[1];
			m_lvUnitLocations.ListViewItemSorter = m_lvcs[2];
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
			this.m_lvTotalUnits = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.m_lvUnitLocations = new System.Windows.Forms.ListView();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.m_lvTerritories = new System.Windows.Forms.ListView();
			this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader11 = new System.Windows.Forms.ColumnHeader();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_lvTotalUnits
			// 
			this.m_lvTotalUnits.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							 this.columnHeader1,
																							 this.columnHeader2,
																							 this.columnHeader3});
			this.m_lvTotalUnits.Location = new System.Drawing.Point(0, 20);
			this.m_lvTotalUnits.Name = "m_lvTotalUnits";
			this.m_lvTotalUnits.Size = new System.Drawing.Size(228, 176);
			this.m_lvTotalUnits.TabIndex = 0;
			this.m_lvTotalUnits.View = System.Windows.Forms.View.Details;
			this.m_lvTotalUnits.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ListViewColumn_Click);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Player";
			this.columnHeader1.Width = 80;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Type";
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Count";
			this.columnHeader3.Width = 52;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Total Units:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(0, 204);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(136, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Player Units:";
			// 
			// m_lvUnitLocations
			// 
			this.m_lvUnitLocations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																								this.columnHeader4,
																								this.columnHeader8,
																								this.columnHeader7,
																								this.columnHeader5,
																								this.columnHeader6});
			this.m_lvUnitLocations.Location = new System.Drawing.Point(0, 220);
			this.m_lvUnitLocations.Name = "m_lvUnitLocations";
			this.m_lvUnitLocations.Size = new System.Drawing.Size(228, 176);
			this.m_lvUnitLocations.TabIndex = 4;
			this.m_lvUnitLocations.View = System.Windows.Forms.View.Details;
			this.m_lvUnitLocations.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ListViewColumn_Click);
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Player";
			// 
			// columnHeader8
			// 
			this.columnHeader8.Text = "Location";
			// 
			// columnHeader7
			// 
			this.columnHeader7.Text = "Territory";
			this.columnHeader7.Width = 100;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Type";
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "Count";
			this.columnHeader6.Width = 47;
			// 
			// m_lvTerritories
			// 
			this.m_lvTerritories.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							  this.columnHeader9,
																							  this.columnHeader10,
																							  this.columnHeader11});
			this.m_lvTerritories.Location = new System.Drawing.Point(0, 420);
			this.m_lvTerritories.Name = "m_lvTerritories";
			this.m_lvTerritories.Size = new System.Drawing.Size(228, 176);
			this.m_lvTerritories.TabIndex = 5;
			this.m_lvTerritories.View = System.Windows.Forms.View.Details;
			this.m_lvTerritories.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ListViewColumn_Click);
			// 
			// columnHeader9
			// 
			this.columnHeader9.Text = "Player";
			this.columnHeader9.Width = 54;
			// 
			// columnHeader10
			// 
			this.columnHeader10.Text = "Location";
			this.columnHeader10.Width = 56;
			// 
			// columnHeader11
			// 
			this.columnHeader11.Text = "Territory";
			this.columnHeader11.Width = 96;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(0, 404);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 16);
			this.label3.TabIndex = 6;
			this.label3.Text = "Territories:";
			// 
			// InformationPanel
			// 
			this.Controls.Add(this.label3);
			this.Controls.Add(this.m_lvTerritories);
			this.Controls.Add(this.m_lvUnitLocations);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.m_lvTotalUnits);
			this.Name = "InformationPanel";
			this.Size = new System.Drawing.Size(236, 600);
			this.ResumeLayout(false);

		}
		#endregion


		public void RefreshTerritoryInfo()
		{
			m_lvTerritories.Items.Clear();

			foreach(Player p in m_controller.Players)
			{
				foreach(Territory t in p.Territories.Values)
				{
					ListViewItem lvi = new ListViewItem();
					lvi.Text = p.Name;
					lvi.SubItems.Add(t.System.Name);
					lvi.SubItems.Add(t.Name);

					m_lvTerritories.Items.Add(lvi);
				}
			}
		}

		public void RefreshAllInfo()
		{
			RefreshUnitLocations();
			RefreshTerritoryInfo();
			RefreshUnitCounts();
		}

		public void RefreshUnitLocations()
		{
			m_lvUnitLocations.Items.Clear();

			foreach(Player p in m_controller.Players)
			{
				ArrayList territories = p.Units.GetUnitTerritories();

				foreach(Territory t in territories)
				{
					UnitCollection ucTerritory = t.Units.GetUnits(p);
					Hashtable ht = ucTerritory.GetUnitTypeCount();

					foreach(UnitType ut in ht.Keys)
					{
						ListViewItem lvi = new ListViewItem();
						lvi.Text = p.Name;
						lvi.SubItems.Add(t.System.Name);
						lvi.SubItems.Add(t.Name);
						lvi.SubItems.Add(ut.ToString());

						int numUnits = (int)ht[ut];
						lvi.SubItems.Add(numUnits.ToString());

						m_lvUnitLocations.Items.Add(lvi);

					}					
				}
			}
		}

		public void UpdateUnitInfo(object sender, TerritoryUnitsEventArgs tuea)
		{
			ArrayList players = tuea.Units.GetPlayersWithUnits();

			foreach(Player p in players)
			{
				UnitCollection uc = tuea.Units.GetUnits(p);
				Hashtable ht = uc.GetUnitTypeCount();

				foreach(UnitType ut in ht.Keys)
				{
					ListViewItem lvi = null;

					foreach(ListViewItem lvi2 in m_lvUnitLocations.Items)
					{
						if(lvi2.Text == p.Name 
							&& lvi2.SubItems[2].Text == tuea.Territory.Name
							&& lvi2.SubItems[3].Text == ut.ToString())
						{
							lvi = lvi2;
							break;
						}
					}

					if(tuea.Added)
					{
						int numStartingUnits = 0;
						
						if(lvi != null)
						{
							numStartingUnits = Int32.Parse(lvi.SubItems[4].Text);
						}
						else
						{
							lvi = new ListViewItem();
							lvi.Text = p.Name;
							lvi.SubItems.Add(tuea.Territory.System.Name);
							lvi.SubItems.Add(tuea.Territory.Name);
							lvi.SubItems.Add(ut.ToString());
							lvi.SubItems.Add("0");
							m_lvUnitLocations.Items.Add(lvi);
						}

						int numUnitsChanged = (int)ht[ut];
						int numResultingUnits = numStartingUnits + numUnitsChanged;

						lvi.SubItems[4].Text = numResultingUnits.ToString();
					}
					else
					{
						if(lvi == null)
						{
							continue;
						}

						int numStartingUnits = Int32.Parse(lvi.SubItems[4].Text);
						int numUnitsChanged = (int)ht[ut];

						int numResultingUnits = 0;
						numResultingUnits = numStartingUnits - numUnitsChanged;

						if(numResultingUnits <= 0)
						{
							m_lvUnitLocations.Items.Remove(lvi);
						}
						else
						{
							lvi.SubItems[4].Text = numResultingUnits.ToString();
						}

					}					
				}
			}
		}

		public void RefreshUnitCounts()
		{
			m_lvTotalUnits.Items.Clear();

			foreach(Player p in m_controller.Players)
			{
				Hashtable ht = p.Units.GetUnitTypeCount();

				foreach(UnitType ut in ht.Keys)
				{
					ListViewItem lvi = new ListViewItem();
					lvi.Text = p.Name;
					lvi.SubItems.Add(ut.ToString());

					int numUnits = (int)ht[ut];
					lvi.SubItems.Add(numUnits.ToString());

					m_lvTotalUnits.Items.Add(lvi);
					
				}
			}
		}

		public BuckRogers.GameController Controller
		{
			get { return this.m_controller; }
			set { this.m_controller = value; }
		}


		private void ListViewColumn_Click(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			ListView lv = (ListView)sender;

			ListViewColumnSorter lvcs = (ListViewColumnSorter)lv.ListViewItemSorter;

			if ( e.Column == lvcs.SortColumn )
			{
				// Reverse the current sort direction for this column.
				if (lvcs.Order == SortOrder.Ascending)
				{
					lvcs.Order = SortOrder.Descending;
				}
				else
				{
					lvcs.Order = SortOrder.Ascending;
				}
			}
			else
			{
				// Set the column number that is to be sorted; default to ascending.
				lvcs.SortColumn = e.Column;
				lvcs.Order = SortOrder.Ascending;
			}

			lv.Sort();
		}

	}
}
