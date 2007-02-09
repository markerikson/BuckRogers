using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Text;

using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo.Util;

using BuckRogers;
using BuckRogers.Interface;

namespace BuckRogers.Interface
{
	public delegate void MoveModeChangedHandler(object sender, MoveModeEventArgs mmea);
	/// <summary>
	/// Summary description for MovePanel.
	/// </summary>
	public class MovePanel : System.Windows.Forms.UserControl
	{
		public event MoveModeChangedHandler MoveModeChanged;

		private ArrayList m_currentMoveTerritories;
		private UnitCollection m_unitsToMove;
		private Hashtable m_unitsToMoveCounts;
		private UnitCollection m_handSelectedUnits;
		private bool m_userChangedOriginalSelection;
		private bool m_unitTotalsWereChanged;
		private bool m_stupidCodeGuardFlag;

		private System.Windows.Forms.Button m_btnAddMove;
		private System.Windows.Forms.Button m_btnUndoMove;
		private System.Windows.Forms.Button m_btnRedoMove;

		private MoveListBox m_mlbMoves;
		private MoveListBox m_mlbTransports;
		private GameController m_controller;
		private MoveMode m_moveMode;
		private System.Windows.Forms.Button m_btnCancelMove;
		private System.Windows.Forms.Button m_btnEndMoves;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox m_lbCurrentMoves;
		private System.Windows.Forms.Button m_btnTransports;
		private System.Windows.Forms.Button m_btnAcceptMoves;
		private System.Windows.Forms.Button m_btnDoneTransports;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private PlayerListBox m_lbPlayerOrder;
		private System.Windows.Forms.Label label2;
		private MapControl m_map;


		public MapControl Map
		{
			get { return m_map; }
			set { m_map = value; }
		}

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MovePanel()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			this.Dock = DockStyle.Fill;

			m_mlbMoves = new MoveListBox();
			m_mlbTransports = new MoveListBox();

			int listboxY = m_lbCurrentMoves.Bottom + 8;
			int listboxHeight = ClientSize.Height - listboxY;


			this.DockPadding.Top = listboxY;
			//this.DockPadding.Right = 8;
			//this.DockPadding.Bottom = 4;

			tabControl1.Dock = DockStyle.Fill;

			m_mlbMoves.Parent = tabPage1;
			//m_mlbMoves.Bounds = new Rectangle(0, 0, this.ClientSize.Width - 8, listboxHeight);			
			m_mlbMoves.Dock = DockStyle.Fill;

			m_mlbTransports.Parent = tabPage2;
			m_mlbTransports.Dock = DockStyle.Fill;
			
			//m_mlbMoves.Anchor = AnchorStyles.Bottom;
			//m_mlbMoves.Dock = DockStyle.Bottom;

			/*
			m_mlbMoves.Items.Add("Title 1", "Testing some text\nwith several lines in it\nto see what happens");
			m_mlbMoves.Items.Add("Move #6", "From: American Regency\nTo: Urban Reservations\nUnits: 1 Gennie, 2 Troopers, 2 Fighters, 1 Killer Satellite, 15 Battlers, 3 Transports, and various other stuff\nYou know, that's a lot of stuff to move");

			for(int i = 10; i >= 0; i--)
			{
				m_mlbMoves.Items.Add("Item #" + i, "Line 1\nLine 2\nLine 3");
			}
			*/


			m_lbCurrentMoves.Items.Clear();
			m_currentMoveTerritories = new ArrayList();
			m_unitsToMove = new UnitCollection();
			m_handSelectedUnits = new UnitCollection();
			m_unitsToMoveCounts = new Hashtable();
			m_moveMode = MoveMode.Finished;

			m_btnCancelMove.Enabled = false;
			m_btnAcceptMoves.Enabled = false;
			m_btnUndoMove.Enabled = false;
			m_btnRedoMove.Enabled = false;
			m_btnDoneTransports.Enabled = false;

			m_userChangedOriginalSelection = false;

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
			this.m_btnAddMove = new System.Windows.Forms.Button();
			this.m_btnUndoMove = new System.Windows.Forms.Button();
			this.m_btnRedoMove = new System.Windows.Forms.Button();
			this.m_btnEndMoves = new System.Windows.Forms.Button();
			this.m_btnCancelMove = new System.Windows.Forms.Button();
			this.m_btnAcceptMoves = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.m_lbCurrentMoves = new System.Windows.Forms.ListBox();
			this.m_btnTransports = new System.Windows.Forms.Button();
			this.m_btnDoneTransports = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.m_lbPlayerOrder = new BuckRogers.Interface.PlayerListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.tabControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_btnAddMove
			// 
			this.m_btnAddMove.Location = new System.Drawing.Point(0, 104);
			this.m_btnAddMove.Name = "m_btnAddMove";
			this.m_btnAddMove.Size = new System.Drawing.Size(72, 23);
			this.m_btnAddMove.TabIndex = 0;
			this.m_btnAddMove.Text = "Add Move";
			this.m_btnAddMove.Click += new System.EventHandler(this.m_btnAddMove_Click);
			// 
			// m_btnUndoMove
			// 
			this.m_btnUndoMove.Location = new System.Drawing.Point(0, 160);
			this.m_btnUndoMove.Name = "m_btnUndoMove";
			this.m_btnUndoMove.Size = new System.Drawing.Size(72, 23);
			this.m_btnUndoMove.TabIndex = 2;
			this.m_btnUndoMove.Text = "Undo";
			this.m_btnUndoMove.Click += new System.EventHandler(this.m_btnUndoMove_Click);
			// 
			// m_btnRedoMove
			// 
			this.m_btnRedoMove.Location = new System.Drawing.Point(76, 160);
			this.m_btnRedoMove.Name = "m_btnRedoMove";
			this.m_btnRedoMove.Size = new System.Drawing.Size(72, 23);
			this.m_btnRedoMove.TabIndex = 3;
			this.m_btnRedoMove.Text = "Redo";
			this.m_btnRedoMove.Click += new System.EventHandler(this.m_btnRedoMove_Click);
			// 
			// m_btnEndMoves
			// 
			this.m_btnEndMoves.Location = new System.Drawing.Point(152, 160);
			this.m_btnEndMoves.Name = "m_btnEndMoves";
			this.m_btnEndMoves.Size = new System.Drawing.Size(72, 23);
			this.m_btnEndMoves.TabIndex = 4;
			this.m_btnEndMoves.Text = "End Turn";
			this.m_btnEndMoves.Click += new System.EventHandler(this.m_btnEndMoves_Click);
			// 
			// m_btnCancelMove
			// 
			this.m_btnCancelMove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_btnCancelMove.Location = new System.Drawing.Point(76, 104);
			this.m_btnCancelMove.Name = "m_btnCancelMove";
			this.m_btnCancelMove.Size = new System.Drawing.Size(72, 23);
			this.m_btnCancelMove.TabIndex = 6;
			this.m_btnCancelMove.Text = "Cancel";
			this.m_btnCancelMove.Click += new System.EventHandler(this.m_btnCancelMove_Click);
			// 
			// m_btnAcceptMoves
			// 
			this.m_btnAcceptMoves.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_btnAcceptMoves.Location = new System.Drawing.Point(152, 104);
			this.m_btnAcceptMoves.Name = "m_btnAcceptMoves";
			this.m_btnAcceptMoves.Size = new System.Drawing.Size(72, 23);
			this.m_btnAcceptMoves.TabIndex = 7;
			this.m_btnAcceptMoves.Text = "Accept";
			this.m_btnAcceptMoves.Click += new System.EventHandler(this.m_btnFinishMove_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 192);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 16);
			this.label1.TabIndex = 8;
			this.label1.Text = "Current move:";
			// 
			// m_lbCurrentMoves
			// 
			this.m_lbCurrentMoves.Items.AddRange(new object[] {
																  "One",
																  "Two",
																  "Three",
																  "Four",
																  "Five",
																  "Six"});
			this.m_lbCurrentMoves.Location = new System.Drawing.Point(84, 192);
			this.m_lbCurrentMoves.Name = "m_lbCurrentMoves";
			this.m_lbCurrentMoves.Size = new System.Drawing.Size(140, 82);
			this.m_lbCurrentMoves.TabIndex = 9;
			this.m_lbCurrentMoves.DoubleClick += new System.EventHandler(this.m_lbCurrentMoves_DoubleClick);
			// 
			// m_btnTransports
			// 
			this.m_btnTransports.Location = new System.Drawing.Point(0, 132);
			this.m_btnTransports.Name = "m_btnTransports";
			this.m_btnTransports.Size = new System.Drawing.Size(148, 23);
			this.m_btnTransports.TabIndex = 10;
			this.m_btnTransports.Text = "Load / Unload Transports";
			this.m_btnTransports.Click += new System.EventHandler(this.m_btnTransports_Click);
			// 
			// m_btnDoneTransports
			// 
			this.m_btnDoneTransports.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_btnDoneTransports.Location = new System.Drawing.Point(152, 132);
			this.m_btnDoneTransports.Name = "m_btnDoneTransports";
			this.m_btnDoneTransports.Size = new System.Drawing.Size(72, 23);
			this.m_btnDoneTransports.TabIndex = 11;
			this.m_btnDoneTransports.Text = "Done";
			this.m_btnDoneTransports.Click += new System.EventHandler(this.m_btnDoneTransports_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(0, 280);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(232, 316);
			this.tabControl1.TabIndex = 12;
			// 
			// tabPage1
			// 
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(224, 290);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Moves";
			// 
			// tabPage2
			// 
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(224, 290);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Transports";
			// 
			// m_lbPlayerOrder
			// 
			this.m_lbPlayerOrder.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_lbPlayerOrder.Location = new System.Drawing.Point(88, 4);
			this.m_lbPlayerOrder.Name = "m_lbPlayerOrder";
			this.m_lbPlayerOrder.Size = new System.Drawing.Size(136, 95);
			this.m_lbPlayerOrder.TabIndex = 13;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(0, 4);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(84, 16);
			this.label2.TabIndex = 14;
			this.label2.Text = "Player order:";
			// 
			// MovePanel
			// 
			this.Controls.Add(this.label2);
			this.Controls.Add(this.m_lbPlayerOrder);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.m_btnDoneTransports);
			this.Controls.Add(this.m_btnTransports);
			this.Controls.Add(this.m_lbCurrentMoves);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.m_btnAcceptMoves);
			this.Controls.Add(this.m_btnCancelMove);
			this.Controls.Add(this.m_btnEndMoves);
			this.Controls.Add(this.m_btnRedoMove);
			this.Controls.Add(this.m_btnUndoMove);
			this.Controls.Add(this.m_btnAddMove);
			this.Name = "MovePanel";
			this.Size = new System.Drawing.Size(240, 600);
			this.tabControl1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void m_btnAddMove_Click(object sender, System.EventArgs e)
		{
			m_btnCancelMove.Enabled = true;
			m_btnAcceptMoves.Enabled = true;

			m_btnAddMove.Enabled = false;
			m_btnUndoMove.Enabled = false;
			m_btnRedoMove.Enabled = false;
			m_btnEndMoves.Enabled = false;
			m_btnTransports.Enabled = false;

			m_currentMoveTerritories.Clear();

			if(MoveModeChanged != null)
			{
				MoveModeEventArgs mmea = new MoveModeEventArgs();
				mmea.MoveMode = MoveMode.StartMove;
				m_moveMode = MoveMode.StartMove;

				MoveModeChanged(this, mmea);
			}
		}


		public void TerritoryClicked(Territory t, TerritoryEventArgs tcea)
		{
			switch(m_moveMode)
			{
				case MoveMode.StartMove:
				{
					OnMoveCreationClick(t, tcea);
					break;
				}
				case MoveMode.StartTransport:
				{
					OnTransportLoadClick(t);
					break;
				}
			}
		}

		private void OnTransportLoadClick(Territory t)
		{
			TransportLoadForm tlf = new TransportLoadForm(m_controller);

			tlf.SetupUnits(t, m_controller.CurrentPlayer);
			tlf.ShowDialog();

			if (tlf.DialogResult != DialogResult.OK)
			{
				return;
			}

			ArrayList transferInfo = tlf.TransferInfo;

			m_mlbMoves.Refresh();
			m_mlbTransports.Refresh();

			return;
		}

		private void OnMoveCreationClick(Territory t, TerritoryEventArgs tcea)
		{
			m_unitTotalsWereChanged = false;
			m_stupidCodeGuardFlag = false;
			if (m_currentMoveTerritories.Count == 0)
			{
				if (t.Owner != m_controller.CurrentPlayer)
				{
					string message = String.Empty;
					UnitCollection uc = t.Units.GetUnits(m_controller.CurrentPlayer);

					if (uc.Count == 0)
					{
						message = "You don't have any units in that territory";
					}

					if (message != String.Empty)
					{
						MessageBox.Show(message, "Movement",
							MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return;
					}
				}

				m_unitsToMoveCounts.Clear();
			}

			PInputEventArgs e = (PInputEventArgs)tcea.Tag;

			if (m_currentMoveTerritories.Count == 0
				|| (m_currentMoveTerritories.Count == 1 && t == (m_currentMoveTerritories[0])))
			{
				PNode picked = e.PickedNode;

				PNodeList matchingNodes = new PNodeList();
				PointF point = e.Position;
				RectangleF pointRect = new RectangleF(point.X, point.Y, 1, 1);
				picked.FindIntersectingNodes(pointRect, matchingNodes);
				IconInfo clickedInfo = null;
				IconInfo movementInfo = null;

				if (matchingNodes.Count > 0)
				{
					PNode topNode = matchingNodes[0];

					if (topNode.Tag is IconInfo)
					{
						clickedInfo = (IconInfo)topNode.Tag;

						if (clickedInfo.Player != m_controller.CurrentPlayer)
						{
							return;
						}

						foreach (IconInfo existingInfo in m_map.MovementIcons)
						{
							if (existingInfo.Player == clickedInfo.Player
								&& existingInfo.Type == clickedInfo.Type)
							{
								movementInfo = existingInfo;
								break;
							}
						}
					}
				}

				if (e.Button == MouseButtons.Left)
				{
					Hashtable newUnitCounts = null;

					// territory was clicked, but no unit
					if (clickedInfo == null && m_unitsToMoveCounts.Count == 0)
					{
						if ((e.Modifiers & Keys.Shift) == Keys.Shift)
						{
							UnitCollection uc = t.Units.GetUnits(UnitType.None, m_controller.CurrentPlayer, t);
							UnitCollection unitsWithMoves = uc.GetUnitsWithMinMoves(1);

							if (unitsWithMoves.Count > 0)
							{
								newUnitCounts = unitsWithMoves.GetUnitTypeCount();
							}
						}
						else
						{
							// Show MoveUnitForm, get hashtable of results
							MoveUnitsForm muf = new MoveUnitsForm();

							muf.SetupUnits(t, m_controller.CurrentPlayer);

							muf.ShowDialog();

							if (muf.DialogResult != DialogResult.OK)
							{
								return;
							}

							m_handSelectedUnits.AddAllUnits(muf.SelectedUnits);
							newUnitCounts = muf.SelectedUnits.GetUnitTypeCount();

							m_userChangedOriginalSelection = false;
							m_stupidCodeGuardFlag = true;
						}
					}
					// unit was clicked, and none are currently shown
					else if (clickedInfo != null && movementInfo == null)
					{
						// Set newUnitCounts[unitType] to 1;
						UnitType ut = clickedInfo.Type;

						UnitCollection uc = t.Units.GetUnits(ut, m_controller.CurrentPlayer, t);
						UnitCollection unitsWithMoves = uc.GetUnitsWithMinMoves(1);

						if (unitsWithMoves.Count > 0)
						{
							if ((e.Modifiers & Keys.Shift) == Keys.Shift)
							{
								newUnitCounts = unitsWithMoves.GetUnitTypeCount();
							}
							else
							{
								int numUnitsToAdd = NumUnitsToAdd(e, unitsWithMoves.Count, 0);
								newUnitCounts = new Hashtable();
								newUnitCounts[ut] = numUnitsToAdd;//1;
							}
						}
						// no units with moves left
						else
						{
							return;
						}
					}
					else if (clickedInfo == null && m_unitsToMoveCounts.Count > 0)
					{
						// not trying to do anything here
						return;
					}

					if (newUnitCounts != null)
					{
						foreach (DictionaryEntry de in newUnitCounts)
						{
							Player p = m_controller.CurrentPlayer;
							UnitType ut = (UnitType)de.Key;
							int numUnits = (int)de.Value;

							// it's possible that we already had something displayed,
							// then the user shift-clicked
							if (!m_unitsToMoveCounts.ContainsKey(ut))
							{
								movementInfo = new IconInfo();
								movementInfo.Player = p;
								movementInfo.Type = ut;

								if (m_map.MovementIcons.Count == 0)
								{
									movementInfo.Location = e.Position;
								}
								else
								{
									IconInfo lastInfo = (IconInfo)m_map.MovementIcons[m_map.MovementIcons.Count - 1];
									PointF newLocation = lastInfo.Location;
									newLocation.X += 52;
									movementInfo.Location = newLocation;
								}

								m_map.IconManager.InitializeIcon(movementInfo);

								movementInfo.Label.TextBrush = Brushes.White;
								movementInfo.Label.ConstrainWidthToTextWidth = false;
								movementInfo.Label.Width = movementInfo.Icon.Width;
								movementInfo.Label.TextAlignment = StringAlignment.Center;

								movementInfo.Label.ScaleBy(0.5f);
								movementInfo.Icon.ScaleBy(0.5f);

								int numDisplayedIcons = m_map.MovementIcons.Count;

								m_map.Canvas.Camera.AddChild(movementInfo.Composite);
								m_map.MovementIcons.Add(movementInfo);

								m_unitsToMoveCounts[movementInfo.Type] = numUnits;
								m_unitTotalsWereChanged = true;
							}
							else
							{
								// should be guaranteed to find something
								foreach (IconInfo info in m_map.MovementIcons)
								{
									if (info.Player == p && info.Type == ut)
									{
										movementInfo = info;
										break;
									}
								}
							}

							movementInfo.Label.Text = numUnits.ToString();
						}

						m_map.UpdateMovementIcons(e);
					}
					else
					{
						int numCurrent = int.Parse(movementInfo.Label.Text);
						int numTotal = int.Parse(clickedInfo.Label.Text);

						if (numCurrent < numTotal)
						{
							int numUnitsToAdd = NumUnitsToAdd(e, numTotal, numCurrent);

							movementInfo.Label.Text = numUnitsToAdd.ToString();
							UnitType ut = movementInfo.Type;
							m_unitsToMoveCounts[ut] = numUnitsToAdd;
							m_unitTotalsWereChanged = true;
						}
						else
						{
							// full up already, don't do anything
							return;
						}
					}
				}
				// right mouse button
				else
				{
					if (m_map.MovementIcons.Count == 0)
					{
						return;
					}

					if ((e.Modifiers & Keys.Shift) == Keys.Shift)
					{
						ClearMovementIcons();
						m_unitsToMoveCounts.Clear();
						m_unitTotalsWereChanged = true;

						return;
					}

					if (movementInfo == null)
					{
						movementInfo = (IconInfo)m_map.MovementIcons[m_map.MovementIcons.Count - 1];
					}

					bool removeMovementIcon = false;
					if ((e.Modifiers & Keys.Control) == Keys.Control)
					{
						removeMovementIcon = true;
					}
					else
					{
						int numToRemove = 1;
						int numUnits = int.Parse(movementInfo.Label.Text);

						if ((e.Modifiers & Keys.Alt) == Keys.Alt)
						{
							int oneTenthDisplayed = numUnits / 10;
							numToRemove = Math.Max(1, oneTenthDisplayed);
						}

						numUnits -= numToRemove;

						if (numUnits <= 0)
						{
							removeMovementIcon = true;
						}
						else
						{
							movementInfo.Label.Text = numUnits.ToString();
							m_unitsToMoveCounts[movementInfo.Type] = numUnits;
							m_unitTotalsWereChanged = true;
						}
					}

					if (removeMovementIcon)
					{
						movementInfo.Composite.RemoveFromParent();
						m_map.MovementIcons.Remove(movementInfo);
						m_map.UpdateMovementIcons(e);

						m_unitsToMoveCounts.Remove(movementInfo.Type);
						m_unitTotalsWereChanged = true;
					}
				}

				int i = 42;
				int q = i;
			}

			bool addTerritory = true;

			if (e.Button == MouseButtons.Right)
			{
				addTerritory = false;
			}

			if ((m_currentMoveTerritories.Count > 0)
				&& (t == (Territory)m_currentMoveTerritories[m_currentMoveTerritories.Count - 1]))
			{
				addTerritory = false;
			}

			if (addTerritory)
			{
				m_currentMoveTerritories.Add(t);
				m_lbCurrentMoves.Items.Add(t.Name);
			}

			if(m_unitTotalsWereChanged && !m_stupidCodeGuardFlag)
			{
				m_userChangedOriginalSelection = true;
			}

			return;
		}

		private int NumUnitsToAdd(PInputEventArgs e, int numTotal, int numCurrent)
		{
			int numToAdd = 1;

			if ((e.Modifiers & Keys.Control) == Keys.Control)
			{
				numToAdd = numTotal - numCurrent;
			}
			else if ((e.Modifiers & Keys.Alt) == Keys.Alt)
			{
				numToAdd = 10;
			}
			/*
			else if ((e.Modifiers & Keys.Shift) == Keys.Shift)
			{
				// add EVERYTHING here
				MessageBox.Show("adding everything");
				return;
			}
			*/
			numCurrent += numToAdd;

			if (numCurrent > numTotal)
			{
				numCurrent = numTotal;
			}

			return numCurrent;
		}

		public void ClearMovementIcons()
		{
			foreach (IconInfo displayedInfo in m_map.MovementIcons)
			{
				displayedInfo.Composite.RemoveFromParent();
			}

			m_map.MovementIcons.Clear();
		}



		private void m_btnCancelMove_Click(object sender, System.EventArgs e)
		{
			m_lbCurrentMoves.Items.Clear();
			m_currentMoveTerritories.Clear();

			m_btnCancelMove.Enabled = false;
			m_btnAcceptMoves.Enabled = false;

			m_btnAddMove.Enabled = true;
			m_btnEndMoves.Enabled = true;
			m_btnTransports.Enabled = true;

			m_btnUndoMove.Enabled = m_controller.CanUndo;
			m_btnRedoMove.Enabled = m_controller.CanRedo;

			if(MoveModeChanged != null)
			{
				MoveModeEventArgs mmea = new MoveModeEventArgs();
				mmea.MoveMode = MoveMode.Finished;

				MoveModeChanged(this, mmea);
			}

			m_unitsToMoveCounts.Clear();
			ClearMovementIcons();

		}

		private void m_btnFinishMove_Click(object sender, System.EventArgs e)
		{
			

			if(m_currentMoveTerritories.Count < 2)
			{
				m_lbCurrentMoves.Items.Clear();
				m_currentMoveTerritories.Clear();

				if(m_currentMoveTerritories.Count == 0)
				{
					MessageBox.Show("No territories were selected, so no move was added");
				}
				else if(m_currentMoveTerritories.Count == 1)
				{
					MessageBox.Show("Only a starting territory was selected, so no move was added");
				}
				
				return;
			}

			try
			{
				if(m_handSelectedUnits.Count == 0 || m_userChangedOriginalSelection)
				{
					MoveUnitsForm muf = new MoveUnitsForm();

					Territory startingTerritory = (Territory)m_currentMoveTerritories[0];
					muf.SetupUnits(startingTerritory, m_controller.CurrentPlayer);

					bool userShouldConfirm = muf.PreSelectUnits(startingTerritory, m_controller.CurrentPlayer, m_unitsToMoveCounts);

					if (userShouldConfirm)
					{
						muf.ShowDialog();

						if (muf.DialogResult != DialogResult.OK)
						{
							return;
						}
					}

					m_handSelectedUnits.AddAllUnits(muf.SelectedUnits);
				}
				

				m_unitsToMove.Clear();
				m_unitsToMove.AddAllUnits(m_handSelectedUnits);
				//newUnitCounts = muf.SelectedUnits.GetUnitTypeCount();

				MoveAction ma = new MoveAction();
				ma.Owner = m_controller.CurrentPlayer;
				ma.StartingTerritory = (Territory)m_currentMoveTerritories[0];

				m_currentMoveTerritories.Remove(ma.StartingTerritory);

				foreach(Territory t in m_currentMoveTerritories)
				{
					ma.Territories.Add(t);
				}

				ma.Units.AddAllUnits(m_unitsToMove);

				m_controller.AddAction(ma);

				//AddActionToList(ma);

				
			}
			catch(ActionException aex)
			{
				MessageBox.Show(aex.Message);
			}

			m_btnUndoMove.Enabled = m_controller.CanUndo;
			m_btnRedoMove.Enabled = m_controller.CanRedo;

			m_lbCurrentMoves.Items.Clear();
			m_currentMoveTerritories.Clear();

			// doesn't matter if the move completed or if there was an exception - either
			// way, the move is over.
			if(MoveModeChanged != null)
			{
				MoveModeEventArgs mmea = new MoveModeEventArgs();
				mmea.MoveMode = MoveMode.Finished;

				MoveModeChanged(this, mmea);
			}

			m_btnCancelMove.Enabled = false;
			m_btnAcceptMoves.Enabled = false;

			m_btnAddMove.Enabled = true;
			m_btnTransports.Enabled = true;
			m_btnEndMoves.Enabled = true;

			m_unitsToMoveCounts.Clear();

			ClearMovementIcons();
		
		}

		private void m_btnUndoMove_Click(object sender, System.EventArgs e)
		{
			Action a = m_controller.UndoAction();

			if(a is MoveAction)
			{
				m_mlbMoves.Items.Remove(0);
				m_mlbMoves.Refresh();
			}
			else if(a is TransportAction)
			{
				m_mlbTransports.Items.Remove(0);
				m_mlbTransports.Refresh();
			}

			m_btnUndoMove.Enabled = m_controller.CanUndo;
			m_btnRedoMove.Enabled = m_controller.CanRedo;
		}

		private void m_btnRedoMove_Click(object sender, System.EventArgs e)
		{
			Action a = m_controller.RedoAction();

			//AddActionToList(a);

			m_btnUndoMove.Enabled = m_controller.CanUndo;
			m_btnRedoMove.Enabled = m_controller.CanRedo;
		}

		public void AddActionToList(Action a)
		{
			if(a is MoveAction)
			{
				MoveAction ma = (MoveAction)a;
				StringBuilder sb = new StringBuilder();
				sb.Append("From: " + ma.StartingTerritory.Name);
				foreach(Territory t in ma.Territories)
				{
					sb.Append("\n");
					sb.Append("To: ");
					sb.Append(t.Name);
				}

				sb.Append("\n");
				sb.Append("Units: ");

				Hashtable ht = ma.Units.GetUnitTypeCount();

				bool firstItem = true;
				foreach(UnitType ut in ht.Keys)
				{
					int numUnits = (int)ht[ut];
					
					if(!firstItem)
					{
						sb.Append(", ");
					}

					firstItem = false;

					sb.Append(numUnits);
					sb.Append(" ");
					sb.Append(ut.ToString());			
		
					if(numUnits > 1)
					{
						sb.Append("s");
					}
				}
					
				m_mlbMoves.Items.Insert(0, "Move #" + (m_controller.ActionsCount).ToString(),
					sb.ToString());
				m_mlbMoves.Refresh();


			}
			else if(a is TransportAction)
			{
				TransportAction ta = (TransportAction)a;
				StringBuilder sb = new StringBuilder();
				sb.Append("Territory: ");
				sb.Append(ta.StartingTerritory.Name);
				sb.Append("\n");
				if(ta.Load)
				{
					sb.Append("Loaded: ");
				}
				else
				{
					sb.Append("Unloaded: ");
				}
				
				sb.Append(ta.Units.Count);
				sb.Append(" ");
				sb.Append(ta.Units[0].Type.ToString());
				
				if(ta.Units.Count > 1)
				{
					sb.Append("s");
				}

				m_mlbTransports.Items.Insert(0, "Transportation #" + (m_controller.ActionsCount).ToString(),
												sb.ToString());
			}
		}

		private void m_btnEndMoves_Click(object sender, System.EventArgs e)
		{
            DialogResult dr = MessageBox.Show("Do you want to end your turn?", "End Turn?", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if(dr != DialogResult.Yes)
            {
                return;
            }


			m_controller.EndMovePhase();
			if(m_controller.NextPlayer())
			{
				m_lbPlayerOrder.SelectedItem = m_controller.CurrentPlayer;
				m_lbPlayerOrder.Refresh();
				
			}
			else
			{
				RefreshPlayerOrder();
			}

			m_btnUndoMove.Enabled = m_controller.CanUndo;
			m_btnRedoMove.Enabled = m_controller.CanRedo;

			m_mlbMoves.Items.Clear();
			m_mlbMoves.Refresh();
			m_mlbTransports.Items.Clear();
			m_mlbTransports.Refresh();
			m_unitsToMove.Clear();	
		}

		public void RefreshPlayerOrder()
		{
			m_lbPlayerOrder.Items.Clear();

			foreach(Player p in m_controller.PlayerOrder)
			{
				m_lbPlayerOrder.Items.Add(p);
			}

			//m_lbPlayerOrder.SelectedIndex = 0;
			m_lbPlayerOrder.SelectedItem = m_controller.CurrentPlayer;
		}

		public void DisableMovePanel()
		{
			m_mlbMoves.Items.Clear();
			m_mlbTransports.Items.Clear();

			m_btnAcceptMoves.Enabled = false;
			m_btnAddMove.Enabled = false;
			m_btnCancelMove.Enabled = false;
			m_btnDoneTransports.Enabled = false;
			m_btnEndMoves.Enabled = false;
			m_btnRedoMove.Enabled = false;
			m_btnTransports.Enabled = false;
			m_btnUndoMove.Enabled = false;
		}

		public void EnableMovePanel()
		{
			// Assume that we're in the middle of a normal turn.  Currently, the active
			// player's MoveActions from this turn are not saved, just the current position
			// of his units.  So, we don't need to worry about Undo/Redo.
			m_btnAddMove.Enabled = true;
			m_btnEndMoves.Enabled = true;
			m_btnTransports.Enabled = true;
		}

		private void m_btnTransports_Click(object sender, System.EventArgs e)
		{
			//m_btnCancelMove.Enabled = true;
			//m_btnAcceptMoves.Enabled = true;
			
			m_btnDoneTransports.Enabled = true;

			m_btnTransports.Enabled = false;
			m_btnAddMove.Enabled = false;
			m_btnAcceptMoves.Enabled = false;
			m_btnUndoMove.Enabled = m_controller.CanUndo;
			m_btnRedoMove.Enabled = m_controller.CanRedo;
			m_btnEndMoves.Enabled = false;

			m_currentMoveTerritories.Clear();

			if(MoveModeChanged != null)
			{
				MoveModeEventArgs mmea = new MoveModeEventArgs();
				mmea.MoveMode = MoveMode.StartTransport;
				m_moveMode = MoveMode.StartTransport;

				MoveModeChanged(this, mmea);
			}
		}

		private void m_btnDoneTransports_Click(object sender, System.EventArgs e)
		{
			m_btnAddMove.Enabled = true;
			m_btnTransports.Enabled = true;
			m_btnEndMoves.Enabled = true;

			m_btnDoneTransports.Enabled = false;

			if(MoveModeChanged != null)
			{
				MoveModeEventArgs mmea = new MoveModeEventArgs();
				mmea.MoveMode = MoveMode.Finished;
				m_moveMode = MoveMode.Finished;

				MoveModeChanged(this, mmea);
			}

		}

		public BuckRogers.GameController Controller
		{
			get { return this.m_controller; }
			set { this.m_controller = value; }
		}

		/*
		public GameController Controller
		{
			get { return this.m_controller; }
			set { this.m_controller = value; }
		}
		*/

		private void tlf_TransportInfo(object sender, TransportInfoEventArgs tiea)
		{
			char[] delimiters = {'%'};
			string[] eventInfo = tiea.Message.Split(delimiters);

			eventInfo[0] += (m_mlbTransports.Items.Count + 1).ToString();
			m_mlbTransports.Items.Add(eventInfo[0], eventInfo[1]);
			m_mlbTransports.Refresh();
		}

		private void m_lbCurrentMoves_DoubleClick(object sender, System.EventArgs e)
		{
			int index = m_lbCurrentMoves.SelectedIndex;
			if( index == m_lbCurrentMoves.Items.Count - 1
				&& index != 0)
			{
				m_lbCurrentMoves.Items.Remove(m_lbCurrentMoves.Items[index]);
				m_currentMoveTerritories.Remove(m_currentMoveTerritories[m_currentMoveTerritories.Count - 1]);
			}
		}
	}
	
}
