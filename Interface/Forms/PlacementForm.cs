using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace BuckRogers.Interface
{
	/// <summary>
	/// Summary description for PlacementForm.
	/// </summary>
	public class PlacementForm : System.Windows.Forms.Form
	{
		private Player m_player;
		private UnitCollection m_availableUnits;

		private System.Windows.Forms.Button m_btnCancel;
		private System.Windows.Forms.Button m_btnOK;
		private System.Windows.Forms.Button m_butRemAttackers;
		private System.Windows.Forms.Button m_butAddAttackers;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ListView m_lvSelectedUnits;
		private System.Windows.Forms.ColumnHeader columnHeader12;
		private System.Windows.Forms.ColumnHeader columnHeader13;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListView m_lvAvailableUnits;
		private System.Windows.Forms.ColumnHeader columnHeader11;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ListView m_lvCurrentUnits;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PlacementForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			m_lvAvailableUnits.Items.Clear();
			m_lvSelectedUnits.Items.Clear();
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
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
																													 "Trooper",
																													 "15",
																													 "3"}, -1);
			System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
																													 "Fighter",
																													 "3",
																													 "1",
																													 ""}, -1);
			System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
																													 "Transport",
																													 "1",
																													 "4",
																													 "1 Trooper"}, -1);
			System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
																													 "Transport",
																													 "1",
																													 "4",
																													 "1 Factory"}, -1);
			System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
																													 "Transport",
																													 "1",
																													 "4",
																													 "2 Troopers"}, -1);
			this.m_btnCancel = new System.Windows.Forms.Button();
			this.m_btnOK = new System.Windows.Forms.Button();
			this.m_butRemAttackers = new System.Windows.Forms.Button();
			this.m_butAddAttackers = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.m_lvSelectedUnits = new System.Windows.Forms.ListView();
			this.columnHeader12 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader13 = new System.Windows.Forms.ColumnHeader();
			this.label2 = new System.Windows.Forms.Label();
			this.m_lvAvailableUnits = new System.Windows.Forms.ListView();
			this.columnHeader11 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.label1 = new System.Windows.Forms.Label();
			this.m_lvCurrentUnits = new System.Windows.Forms.ListView();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// m_btnCancel
			// 
			this.m_btnCancel.Location = new System.Drawing.Point(88, 172);
			this.m_btnCancel.Name = "m_btnCancel";
			this.m_btnCancel.TabIndex = 35;
			this.m_btnCancel.Text = "Cancel";
			this.m_btnCancel.Click += new System.EventHandler(this.m_btnCancel_Click);
			// 
			// m_btnOK
			// 
			this.m_btnOK.Location = new System.Drawing.Point(4, 172);
			this.m_btnOK.Name = "m_btnOK";
			this.m_btnOK.TabIndex = 34;
			this.m_btnOK.Text = "OK";
			this.m_btnOK.Click += new System.EventHandler(this.m_btnOK_Click);
			// 
			// m_butRemAttackers
			// 
			this.m_butRemAttackers.Location = new System.Drawing.Point(140, 92);
			this.m_butRemAttackers.Name = "m_butRemAttackers";
			this.m_butRemAttackers.TabIndex = 33;
			this.m_butRemAttackers.Text = "<< Remove";
			this.m_butRemAttackers.Click += new System.EventHandler(this.m_butRemAttackers_Click);
			// 
			// m_butAddAttackers
			// 
			this.m_butAddAttackers.Location = new System.Drawing.Point(140, 60);
			this.m_butAddAttackers.Name = "m_butAddAttackers";
			this.m_butAddAttackers.TabIndex = 32;
			this.m_butAddAttackers.Text = "Add >>";
			this.m_butAddAttackers.Click += new System.EventHandler(this.m_butAddAttackers_Click);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(224, 4);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(100, 16);
			this.label6.TabIndex = 31;
			this.label6.Text = "Selected units:";
			// 
			// m_lvSelectedUnits
			// 
			this.m_lvSelectedUnits.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																								this.columnHeader12,
																								this.columnHeader13});
			this.m_lvSelectedUnits.FullRowSelect = true;
			this.m_lvSelectedUnits.HideSelection = false;
			this.m_lvSelectedUnits.Location = new System.Drawing.Point(224, 20);
			this.m_lvSelectedUnits.MultiSelect = false;
			this.m_lvSelectedUnits.Name = "m_lvSelectedUnits";
			this.m_lvSelectedUnits.Size = new System.Drawing.Size(128, 136);
			this.m_lvSelectedUnits.TabIndex = 30;
			this.m_lvSelectedUnits.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader12
			// 
			this.columnHeader12.Text = "Type";
			// 
			// columnHeader13
			// 
			this.columnHeader13.Text = "Count";
			this.columnHeader13.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader13.Width = 43;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(4, 4);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 16);
			this.label2.TabIndex = 29;
			this.label2.Text = "Available units:";
			// 
			// m_lvAvailableUnits
			// 
			this.m_lvAvailableUnits.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																								 this.columnHeader11,
																								 this.columnHeader1});
			this.m_lvAvailableUnits.FullRowSelect = true;
			this.m_lvAvailableUnits.HideSelection = false;
			this.m_lvAvailableUnits.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
																							   listViewItem1,
																							   listViewItem2,
																							   listViewItem3,
																							   listViewItem4,
																							   listViewItem5});
			this.m_lvAvailableUnits.Location = new System.Drawing.Point(4, 20);
			this.m_lvAvailableUnits.MultiSelect = false;
			this.m_lvAvailableUnits.Name = "m_lvAvailableUnits";
			this.m_lvAvailableUnits.Size = new System.Drawing.Size(128, 136);
			this.m_lvAvailableUnits.TabIndex = 28;
			this.m_lvAvailableUnits.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader11
			// 
			this.columnHeader11.Text = "Type";
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Count";
			this.columnHeader1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader1.Width = 43;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(368, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 16);
			this.label1.TabIndex = 37;
			this.label1.Text = "Current units:";
			// 
			// m_lvCurrentUnits
			// 
			this.m_lvCurrentUnits.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							   this.columnHeader2,
																							   this.columnHeader3});
			this.m_lvCurrentUnits.FullRowSelect = true;
			this.m_lvCurrentUnits.HideSelection = false;
			this.m_lvCurrentUnits.Location = new System.Drawing.Point(368, 20);
			this.m_lvCurrentUnits.MultiSelect = false;
			this.m_lvCurrentUnits.Name = "m_lvCurrentUnits";
			this.m_lvCurrentUnits.Size = new System.Drawing.Size(128, 136);
			this.m_lvCurrentUnits.TabIndex = 36;
			this.m_lvCurrentUnits.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Type";
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Count";
			this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader3.Width = 43;
			// 
			// PlacementForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(532, 198);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.m_lvCurrentUnits);
			this.Controls.Add(this.m_btnCancel);
			this.Controls.Add(this.m_btnOK);
			this.Controls.Add(this.m_butRemAttackers);
			this.Controls.Add(this.m_butAddAttackers);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.m_lvSelectedUnits);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.m_lvAvailableUnits);
			this.Name = "PlacementForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Place Units";
			this.Load += new System.EventHandler(this.PlacementForm_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void m_btnOK_Click(object sender, System.EventArgs e)
		{
			int numPlaced = 0;

			foreach(ListViewItem lvi in m_lvSelectedUnits.Items)
			{
				int numUnits = Int32.Parse(lvi.SubItems[1].Text);
				numPlaced += numUnits;
			}

			if(numPlaced != 3)
			{
				bool playersSelectUnits = GameController.Options.OptionalRules["PickStartingUnits"];//((GameController.Options.SetupOptions & StartingScenarios.PickStartingUnits) == StartingScenarios.PickStartingUnits);

				string errorMessage = string.Empty;

				if(!playersSelectUnits)
				{
					errorMessage = "You must place exactly three units";
				}
				else
				{
					// can never place more than three
					if(numPlaced > 3)
					{
						errorMessage = "You cannot place more than three units at a time.";
					}
					// must have chosen less than three
					else
					{
						int numUnitsLeft = m_availableUnits.Count;

						if(numPlaced != numUnitsLeft)
						{
							if(numUnitsLeft >= 3)
							{
								errorMessage = "You must place exactly three units.";
							}
							else
							{
								errorMessage = "You must place all your remaining units.";
							}							
						}
					}
				}
				
				if(errorMessage	!= string.Empty)
				{
					MessageBox.Show(errorMessage, "Placement Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				
			}			

			this.DialogResult = DialogResult.OK;
		}

		private void m_btnCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		public void SetupUnits(Player p, Territory t)
		{
			m_player = p;

			m_availableUnits = p.Units.GetUnits(Territory.NONE);

			Hashtable unitCount = m_availableUnits.GetUnitTypeCount();
			foreach(UnitType ut in unitCount.Keys)
			{
				UnitCollection typeCollection = m_availableUnits.GetUnits(ut);

				ListViewItem lvi = new ListViewItem();
				lvi.Text = ut.ToString();
				int numUnits = (int)unitCount[ut];
				lvi.SubItems.Add(numUnits.ToString());

				m_lvAvailableUnits.Items.Add(lvi);
			}

			UnitCollection existingUnits = t.Units.GetUnits(p);

			unitCount = existingUnits.GetUnitTypeCount();
			foreach(UnitType ut in unitCount.Keys)
			{
				UnitCollection typeCollection = existingUnits.GetUnits(ut);

				ListViewItem lvi = new ListViewItem();
				lvi.Text = ut.ToString();
				int numUnits = (int)unitCount[ut];
				lvi.SubItems.Add(numUnits.ToString());

				m_lvCurrentUnits.Items.Add(lvi);
			}
			
		}

		private void MoveItems(ListView origin, ListView destination, int numToMove)
		{
			if(origin.SelectedIndices.Count == 0)
			{
				return;
			}

			int idxUnused = origin.SelectedIndices[0];

			ListViewItem lvi = origin.Items[idxUnused];

			string type = lvi.SubItems[0].Text;
			int count = Int32.Parse(lvi.SubItems[1].Text);

			if(numToMove > count)
			{
				numToMove = count;
			}

			int numLeft = count - numToMove;

			if(numLeft > 0)
			{
				origin.Items[idxUnused].SubItems[1].Text = numLeft.ToString();
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
				for(int i = 0; i < destination.Items.Count; i++)
				{
					ListViewItem targetItem = destination.Items[i];

					//string liName = lastItem.SubItems[0].Text;
					string liType = targetItem.SubItems[0].Text;
					string liCount = targetItem.SubItems[1].Text;

					if (liType == type)
					{
						addNewItem = false;
						int numCurrent = Int32.Parse(targetItem.SubItems[1].Text);
						numCurrent += numToMove;

						targetItem.SubItems[1].Text = numCurrent.ToString();
						break;
					}
				}
				
			}
			
			if(addNewItem)
			{
				ListViewItem lvi2 = new ListViewItem();
				lvi2.Text = type;
				lvi2.SubItems.Add(numToMove.ToString());

				destination.Items.Add(lvi2);
			}
		}

		private void m_butAddAttackers_Click(object sender, System.EventArgs e)
		{
			MoveItems(m_lvAvailableUnits, m_lvSelectedUnits, 1);
		}

		private void m_butRemAttackers_Click(object sender, System.EventArgs e)
		{
			MoveItems(m_lvSelectedUnits, m_lvAvailableUnits, 1);
		}

		private void PlacementForm_Load(object sender, System.EventArgs e)
		{
		
		}

		public UnitCollection SelectedUnits
		{
			get
			{
				UnitCollection uc = new UnitCollection();


				foreach(ListViewItem lvi in m_lvSelectedUnits.Items)
				{
					UnitType ut = (UnitType)Enum.Parse(typeof(UnitType), lvi.SubItems[0].Text);
					int numUnits = Int32.Parse(lvi.SubItems[1].Text);

					UnitCollection typeMatches = m_availableUnits.GetUnits(ut, m_player, null);
					UnitCollection neededUnits = typeMatches.GetUnits(numUnits);

					uc.AddAllUnits(neededUnits);
					m_availableUnits.RemoveAllUnits(neededUnits);

				}

				return uc;
			}
		}

	}
}
