using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;

namespace BuckRogers.Interface
{
	/// <summary>
	/// Summary description for MoveUnitsForm.
	/// </summary>
	public class MoveUnitsForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button m_butRemAttackers;
		private System.Windows.Forms.Button m_butAddAttackers;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox m_cbNumUnits;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ColumnHeader columnHeader12;
		private System.Windows.Forms.ColumnHeader columnHeader13;
		private System.Windows.Forms.ColumnHeader columnHeader14;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ColumnHeader columnHeader11;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ListView m_lvSelectedUnits;
		private System.Windows.Forms.ListView m_lvAvailableUnits;
		private System.Windows.Forms.Button m_btnOK;
		private System.Windows.Forms.Button m_btnCancel;

		private Territory m_territory;
		private Player m_player;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.Button m_btnAddAll;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MoveUnitsForm()
		{
			InitializeComponent();

			this.Icon = InterfaceUtility.GetApplicationIcon();

			m_lvAvailableUnits.Items.Clear();
			m_cbNumUnits.SelectedIndex = 0;
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

		private int MoveItems(ListView origin, ListView destination, int index, int numToMove)
		{
			int idxUnused = -1;
			if(index < 0)
			{
				if(origin.SelectedIndices.Count == 0)
				{
					return 0;
				}
				else
				{
					idxUnused = origin.SelectedIndices[0];
				}
			}
			else
			{
				idxUnused = index;
			}

			 

			ListViewItem lvi = origin.Items[idxUnused];

			string type = lvi.SubItems[0].Text;
			int count = Int32.Parse(lvi.SubItems[1].Text);
			string numMovesLeft = lvi.SubItems[2].Text;
			string contents = lvi.SubItems[3].Text;

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
					string liMovesLeft = targetItem.SubItems[2].Text;
					string liContents = targetItem.SubItems[3].Text;

					if (liType == type && liMovesLeft == numMovesLeft && liContents == contents)
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
				lvi2.SubItems.Add(numMovesLeft);
				lvi2.SubItems.Add(contents);

				destination.Items.Add(lvi2);
			}

			return numToMove;
		}

		public void SetupUnits(Territory t, Player p)
		{
			UnitCollection uc = t.Units.GetUnits(p);
			m_player = p;
			m_territory = t;

			foreach(UnitType ut in uc.GetUnitTypeCount().Keys)
			{
				UnitCollection typeCollection = uc.GetUnits(ut);

				// Safe to do this because we know at least one unit of this type exists
				int maxMoves = typeCollection[0].MaxMoves;

				for(int i = maxMoves; i >= 0; i--)
				{
					UnitCollection moveUnits = typeCollection.GetUnitsWithMoves(i);
					if(moveUnits.Count == 0)
					{
						continue;
					}

					

					if(ut == UnitType.Transport)
					{

						UnitCollection empties = moveUnits.GetTransportsWithContents(UnitType.None, 0);
						UnitCollection factories = moveUnits.GetTransportsWithContents(UnitType.Factory, 1);
						//ArrayList troopers = new ArrayList();

						UnitCollection[] troopers = new UnitCollection[6];

						for(int j = 1; j < 6; j++)
						{
							troopers[j] = moveUnits.GetTransportsWithContents(UnitType.Trooper, j);
						}

						/*
						for(int j = 0; j < 6; j++)
						{
							troopers.Add(new ArrayList());
						}


						foreach(Transport tr in moveUnits)
						{
							if(tr.Transportees.Count == 0)
							{
								empties.AddUnit(tr);
							}
							else if(tr.Transportees.GetUnits(UnitType.Factory) > 0)
							{
								factories.AddUnit(tr);
							}
							else
							{
								ArrayList al = (ArrayList)troopers[tr.Transportees.Count];
								al.Add(tr);
							}
						}
						*/
						
						ListViewItem lvi = null;
						if(empties.Count > 0)
						{
							lvi = new ListViewItem();
							lvi.Text = ut.ToString();
							lvi.SubItems.Add(empties.Count.ToString());
							lvi.SubItems.Add(i.ToString());
							lvi.SubItems.Add(string.Empty);

							m_lvAvailableUnits.Items.Add(lvi);
						}

						if(factories.Count > 0)
						{
							lvi = new ListViewItem();
							lvi.Text = ut.ToString();
							lvi.SubItems.Add(factories.Count.ToString());
							lvi.SubItems.Add(i.ToString());
							lvi.SubItems.Add("1 Factory");

							m_lvAvailableUnits.Items.Add(lvi);
						}
						
						for(int j = 5; j > 0; j--)
						{
							if(troopers[j].Count > 0)
							{
								lvi = new ListViewItem();
								lvi.Text = ut.ToString();
								lvi.SubItems.Add(troopers[j].Count.ToString());
								lvi.SubItems.Add(i.ToString());
								StringBuilder sb = new StringBuilder();
								sb.Append(j);
								sb.Append(" Trooper");
								if(j > 1)
								{
									sb.Append("s");
								}
								lvi.SubItems.Add(sb.ToString());

								m_lvAvailableUnits.Items.Add(lvi);
							}
						}

					}
					else
					{
						ListViewItem lvi = new ListViewItem();

						lvi.Text = ut.ToString();
						lvi.SubItems.Add(moveUnits.Count.ToString());
						lvi.SubItems.Add(i.ToString());
						lvi.SubItems.Add(String.Empty);
						m_lvAvailableUnits.Items.Add(lvi);
					}

					
				}
			}
		}

		public bool PreSelectUnits(Territory t, Player p, Hashtable unitCounts)
		{
			bool userShouldConfirm = false;
			bool multipleTransportsEntries = false;

			int numTransportEntries = 0;

			foreach(ListViewItem lvi in m_lvAvailableUnits.Items)
			{
				if(lvi.Text == "Transport")
				{
					numTransportEntries++;
				}
			}

			if(numTransportEntries > 1)
			{
				multipleTransportsEntries = true;

				if(unitCounts.ContainsKey(UnitType.Transport))
				{
					userShouldConfirm = true;
				}
				
			}

			foreach(DictionaryEntry de in unitCounts)
			{
				UnitType ut = (UnitType)de.Key;
				int numUnits = (int)de.Value;

				UnitCollection playerUnits = t.Units.GetUnits(ut, p, t);

				bool sameMoves = true;
				int numMoves = playerUnits[0].MovesLeft;

				for(int i = 1; i < playerUnits.Count; i++)
				{
					Unit u = playerUnits[i];

					if(numMoves != u.MovesLeft)
					{
						sameMoves = false;
						break;
					}
				}

				if(sameMoves && !userShouldConfirm)
				{
					int index = -1;
					for(int i = 0; i < m_lvAvailableUnits.Items.Count; i++)
					{
						ListViewItem lvi = m_lvAvailableUnits.Items[i];

						if(lvi.Text == ut.ToString())
						{
							index = i;
							break;
						}
					}

					if(index >= 0)
					{
						m_lvAvailableUnits.Focus();
						m_lvAvailableUnits.Items[index].Selected = true;

						MoveItems(m_lvAvailableUnits, m_lvSelectedUnits, index, numUnits);	
					}
				}
				else
				{
					// caused by transports
					if(multipleTransportsEntries && ut == UnitType.Transport)
					{
						UnitCollection transports = playerUnits.GetUnits(UnitType.Transport);
						int unitsLeftToMove = numUnits;

						if(transports.Count == numUnits)
						{
							while(unitsLeftToMove > 0)
							{
								unitsLeftToMove = SelectAndMoveItems(UnitType.Transport, -1, unitsLeftToMove);
							}
						}
						// let's prioritize loaded transports first, then empties by moves left
						else
						{
							bool loadedTransports = true;
							for(int i = 0; i < numTransportEntries && unitsLeftToMove > 0; i++)
							{
								ListViewItem transportLVI = null;

								for(int j = 0; j < m_lvAvailableUnits.Items.Count && loadedTransports; j++)
								{
									ListViewItem lvi = m_lvAvailableUnits.Items[j];

									if(lvi.Text == "Transport"
										|| lvi.SubItems[3].Text != string.Empty)
									{
										transportLVI = lvi;
										break;
									}
								}

								if(transportLVI == null)
								{
									loadedTransports = false;

									int maxMoves = transports[0].MaxMoves;
									//int unitsLeftToMove = numUnits;

									for (int j = maxMoves; j >= 0 && unitsLeftToMove > 0; j--)
									{
										unitsLeftToMove = SelectAndMoveItems(ut, j, unitsLeftToMove);
									}
									
								}

								if(transportLVI != null)
								{
									int index = m_lvAvailableUnits.Items.IndexOf(transportLVI);
									int numAvailable = int.Parse(transportLVI.SubItems[1].Text);
									//int numToMove = Math.Min(numAvailable, unitsLeftToMove);

									unitsLeftToMove = SelectAndMoveItems(UnitType.Transport, -1, unitsLeftToMove);
								}
							}
						}
					}
					else
					{
						userShouldConfirm = true;

						int maxMoves = playerUnits[0].MaxMoves;
						int unitsLeftToMove = numUnits;

						for (int j = maxMoves; j >= 0 && unitsLeftToMove > 0; j--)
						{
							unitsLeftToMove = SelectAndMoveItems(ut, j, unitsLeftToMove);
						}
					}
					
				}
			}

			return userShouldConfirm;
		}

		private int SelectAndMoveItems(UnitType ut, int numMovesLeft, int unitsLeftToMove)
		{
			int index = -1;
			for (int i = 0; i < m_lvAvailableUnits.Items.Count; i++)
			{
				ListViewItem lvi = m_lvAvailableUnits.Items[i];

				if (lvi.Text == ut.ToString())
					
				{
					if(lvi.SubItems[2].Text == numMovesLeft.ToString()
						|| numMovesLeft == -1)
					index = i;
					break;
				}
			}

			if (index >= 0)
			{
				m_lvAvailableUnits.Focus();
				m_lvAvailableUnits.Items[index].Selected = true;

				int numAvailable = int.Parse(m_lvAvailableUnits.Items[index].SubItems[1].Text);
				int numToMove = Math.Min(unitsLeftToMove, numAvailable); //unitsLeftToMove - numAvailable;

				if (numToMove <= 0)
				{
					numToMove = unitsLeftToMove;
				}
				int unitsMoved = MoveItems(m_lvAvailableUnits, m_lvSelectedUnits, index, numToMove);
				unitsLeftToMove -= unitsMoved;
			}

			return unitsLeftToMove;
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
			this.m_butRemAttackers = new System.Windows.Forms.Button();
			this.m_butAddAttackers = new System.Windows.Forms.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.m_cbNumUnits = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.m_lvSelectedUnits = new System.Windows.Forms.ListView();
			this.columnHeader12 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader13 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader14 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.label2 = new System.Windows.Forms.Label();
			this.m_lvAvailableUnits = new System.Windows.Forms.ListView();
			this.columnHeader11 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.m_btnOK = new System.Windows.Forms.Button();
			this.m_btnCancel = new System.Windows.Forms.Button();
			this.m_btnAddAll = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// m_butRemAttackers
			// 
			this.m_butRemAttackers.Location = new System.Drawing.Point(272, 92);
			this.m_butRemAttackers.Name = "m_butRemAttackers";
			this.m_butRemAttackers.TabIndex = 25;
			this.m_butRemAttackers.Text = "<< Remove";
			this.m_butRemAttackers.Click += new System.EventHandler(this.m_butRemAttackers_Click);
			// 
			// m_butAddAttackers
			// 
			this.m_butAddAttackers.Location = new System.Drawing.Point(272, 60);
			this.m_butAddAttackers.Name = "m_butAddAttackers";
			this.m_butAddAttackers.TabIndex = 24;
			this.m_butAddAttackers.Text = "Add >>";
			this.m_butAddAttackers.Click += new System.EventHandler(this.m_butAddAttackers_Click);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(252, 124);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(116, 16);
			this.label7.TabIndex = 23;
			this.label7.Text = "Units to add / remove:";
			//this.label7.Click += new System.EventHandler(this.label7_Click);
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
			this.m_cbNumUnits.Location = new System.Drawing.Point(252, 140);
			this.m_cbNumUnits.Name = "m_cbNumUnits";
			this.m_cbNumUnits.Size = new System.Drawing.Size(116, 21);
			this.m_cbNumUnits.TabIndex = 22;
			//this.m_cbNumUnits.SelectedIndexChanged += new System.EventHandler(this.m_cbNumUnits_SelectedIndexChanged);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(380, 8);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(100, 16);
			this.label6.TabIndex = 21;
			this.label6.Text = "Selected units:";
			// 
			// m_lvSelectedUnits
			// 
			this.m_lvSelectedUnits.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																								this.columnHeader12,
																								this.columnHeader13,
																								this.columnHeader14,
																								this.columnHeader4});
			this.m_lvSelectedUnits.FullRowSelect = true;
			this.m_lvSelectedUnits.HideSelection = false;
			this.m_lvSelectedUnits.Location = new System.Drawing.Point(380, 24);
			this.m_lvSelectedUnits.MultiSelect = false;
			this.m_lvSelectedUnits.Name = "m_lvSelectedUnits";
			this.m_lvSelectedUnits.Size = new System.Drawing.Size(236, 136);
			this.m_lvSelectedUnits.TabIndex = 20;
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
			// columnHeader14
			// 
			this.columnHeader14.Text = "Moves";
			this.columnHeader14.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader14.Width = 47;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Contents";
			this.columnHeader4.Width = 64;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(4, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 16);
			this.label2.TabIndex = 19;
			this.label2.Text = "Available units:";
			// 
			// m_lvAvailableUnits
			// 
			this.m_lvAvailableUnits.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																								 this.columnHeader11,
																								 this.columnHeader1,
																								 this.columnHeader2,
																								 this.columnHeader3});
			this.m_lvAvailableUnits.FullRowSelect = true;
			this.m_lvAvailableUnits.HideSelection = false;
			this.m_lvAvailableUnits.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
																							   listViewItem1,
																							   listViewItem2,
																							   listViewItem3,
																							   listViewItem4,
																							   listViewItem5});
			this.m_lvAvailableUnits.Location = new System.Drawing.Point(4, 24);
			this.m_lvAvailableUnits.MultiSelect = false;
			this.m_lvAvailableUnits.Name = "m_lvAvailableUnits";
			this.m_lvAvailableUnits.Size = new System.Drawing.Size(236, 136);
			this.m_lvAvailableUnits.TabIndex = 18;
			this.m_lvAvailableUnits.View = System.Windows.Forms.View.Details;
			//this.m_lvAvailableUnits.SelectedIndexChanged += new System.EventHandler(this.m_lvAvailableUnits_SelectedIndexChanged);
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
			// columnHeader2
			// 
			this.columnHeader2.Text = "Moves";
			this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader2.Width = 47;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Contents";
			this.columnHeader3.Width = 64;
			// 
			// m_btnOK
			// 
			this.m_btnOK.Location = new System.Drawing.Point(4, 176);
			this.m_btnOK.Name = "m_btnOK";
			this.m_btnOK.TabIndex = 26;
			this.m_btnOK.Text = "OK";
			this.m_btnOK.Click += new System.EventHandler(this.m_btnOK_Click);
			// 
			// m_btnCancel
			// 
			this.m_btnCancel.Location = new System.Drawing.Point(88, 176);
			this.m_btnCancel.Name = "m_btnCancel";
			this.m_btnCancel.TabIndex = 27;
			this.m_btnCancel.Text = "Cancel";
			this.m_btnCancel.Click += new System.EventHandler(this.m_btnCancel_Click);
			// 
			// m_btnAddAll
			// 
			this.m_btnAddAll.Location = new System.Drawing.Point(272, 28);
			this.m_btnAddAll.Name = "m_btnAddAll";
			this.m_btnAddAll.TabIndex = 28;
			this.m_btnAddAll.Text = "Add All >>>";
			this.m_btnAddAll.Click += new System.EventHandler(this.m_btnAddAll_Click);
			// 
			// MoveUnitsForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(620, 206);
			this.Controls.Add(this.m_btnAddAll);
			this.Controls.Add(this.m_btnCancel);
			this.Controls.Add(this.m_btnOK);
			this.Controls.Add(this.m_butRemAttackers);
			this.Controls.Add(this.m_butAddAttackers);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.m_cbNumUnits);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.m_lvSelectedUnits);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.m_lvAvailableUnits);
			this.Name = "MoveUnitsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Move Units";
			this.ResumeLayout(false);

		}
		#endregion

		private void m_btnOK_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		private void m_btnCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void m_butAddAttackers_Click(object sender, System.EventArgs e)
		{
			int numUnits = Int32.Parse((string)m_cbNumUnits.SelectedItem);
			MoveItems(m_lvAvailableUnits, m_lvSelectedUnits, -1, numUnits);
		}

		private void m_butRemAttackers_Click(object sender, System.EventArgs e)
		{
			int numUnits = Int32.Parse((string)m_cbNumUnits.SelectedItem);
			MoveItems(m_lvSelectedUnits, m_lvAvailableUnits, -1, numUnits);
		}

		/*
		private void m_lvAvailableUnits_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}

		private void m_cbNumUnits_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}

		private void label7_Click(object sender, System.EventArgs e)
		{
		
		}
		*/

		private void m_btnAddAll_Click(object sender, System.EventArgs e)
		{
			ArrayList temp = new ArrayList();

			while(m_lvAvailableUnits.Items.Count > 0)
			{
				ListViewItem lvi = m_lvAvailableUnits.Items[0];
				m_lvAvailableUnits.Items.Remove(lvi);

				if(lvi.SubItems[2].Text == "0")
				{
					temp.Add(lvi);
				}
				else
				{
					m_lvSelectedUnits.Items.Add(lvi);
				}
			}

			foreach(ListViewItem lvi in temp)
			{
				m_lvAvailableUnits.Items.Add(lvi);
			}
		}

		public UnitCollection SelectedUnits
		{
			get
			{
				UnitCollection uc = new UnitCollection();

				UnitCollection availableUnits = new UnitCollection();
				availableUnits.AddAllUnits(m_territory.Units);

				foreach(ListViewItem lvi in m_lvSelectedUnits.Items)
				{
					UnitType ut = (UnitType)Enum.Parse(typeof(UnitType), lvi.SubItems[0].Text);
					int numUnits = Int32.Parse(lvi.SubItems[1].Text);
					int numMoves = Int32.Parse(lvi.SubItems[2].Text);

					UnitCollection typeMatches = availableUnits.GetUnits(ut, m_player, null);
					UnitCollection movesMatches = typeMatches.GetUnitsWithMoves(numMoves);
					UnitCollection neededUnits = movesMatches.GetUnits(numUnits);

					string contents = lvi.SubItems[3].Text;
					if(ut == UnitType.Transport && contents != string.Empty)
					{
						if(contents.Contains("Factory"))
						{
							neededUnits = neededUnits.GetTransportsWithContents(UnitType.Factory, 1);
						}
						else
						{
							string sNumTroopers = contents.Substring(0, 1);
							int numTroopers = int.Parse(sNumTroopers);

							neededUnits = neededUnits.GetTransportsWithContents(UnitType.Trooper, numTroopers);
						}
						
					}

					uc.AddAllUnits(neededUnits);
					availableUnits.RemoveAllUnits(neededUnits);

				}

				return uc;
			}
		}
	}
}
