#region using directives
using System;
using System.Collections;
using System.Collections.Generic;
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
using BuckRogers.Networking;

using CommandManagement;

#endregion

namespace BuckRogers.Interface
{
	//public delegate void MoveModeChangedHandler(object sender, MoveModeEventArgs mmea);

	public class MovePanel : System.Windows.Forms.UserControl
	{
		#region private members
		//public event MoveModeChangedHandler MoveModeChanged;
		public event EventHandler<MoveModeEventArgs> MoveModeChanged;

		private MoveMode m_moveMode;

		private GameController m_controller;
		private ClientSideGameManager m_csgm;

		private List<Territory> m_currentMoveTerritories;
		private UnitCollection m_unitsToMove;
		private UnitCollection m_handSelectedUnits;
		private Hashtable m_unitsToMoveCounts;
		
		private MoveListBox m_mlbMoves;
		private MoveListBox m_mlbTransports;
		private PlayerListBox m_lbPlayerOrder;
		private MapControl m_map;
		private MoveUnitsForm m_muf;
		
		private bool m_userChangedOriginalSelection;
		private bool m_unitTotalsWereChanged;
		private bool m_stupidCodeGuardFlag;
		private bool m_showingTransportDialog;
		private bool m_enabled;

		private System.Windows.Forms.Button m_btnUndoMove;
		private System.Windows.Forms.Button m_btnRedoMove;
		private System.Windows.Forms.CheckBox m_chkLoadTransports;
		private System.Windows.Forms.Button m_btnRemoveTerritory;
		private System.Windows.Forms.Button m_btnCancelMove;
		private System.Windows.Forms.Button m_btnEndMoves;
		private System.Windows.Forms.Button m_btnAcceptMoves;
		
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox m_lbCurrentMoves;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;	
		private System.Windows.Forms.Label label2;		

		private System.ComponentModel.Container components = null;

		#endregion

		#region properties
		public MapControl Map
		{
			get { return m_map; }
			set
			{
				m_map = value;
			}
		}

		public BuckRogers.GameController Controller
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

		public List<Territory> CurrentMoveTerritories
		{
			get { return m_currentMoveTerritories; }
			set { m_currentMoveTerritories = value; }
		}

		public MoveMode MoveMode
		{
			get { return m_moveMode; }
			set { m_moveMode = value; }
		}

		public bool PanelEnabled
		{
			get
			{
				return m_enabled;
			}
			set
			{
				m_enabled = value;

				if(!m_enabled)
				{
					m_mlbMoves.Items.Clear();
					m_mlbTransports.Items.Clear();
				}
			}
		}
		#endregion

		#region Constructor and initialization
		public MovePanel()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			this.Dock = DockStyle.Fill;

			m_mlbMoves = new MoveListBox();
			m_mlbTransports = new MoveListBox();
			m_muf = new MoveUnitsForm();

			int listboxY = m_lbCurrentMoves.Bottom + 8;
			int listboxHeight = ClientSize.Height - listboxY;

			this.DockPadding.Top = listboxY;

			tabControl1.Dock = DockStyle.Fill;

			m_mlbMoves.Parent = tabPage1;		
			m_mlbMoves.Dock = DockStyle.Fill;

			m_mlbTransports.Parent = tabPage2;
			m_mlbTransports.Dock = DockStyle.Fill;

			m_lbCurrentMoves.Items.Clear();
			m_currentMoveTerritories = new List<Territory>();
			m_unitsToMove = new UnitCollection();
			m_handSelectedUnits = new UnitCollection();
			m_unitsToMoveCounts = new Hashtable();
			m_moveMode = MoveMode.Finished;

			/*
			m_btnCancelMove.Enabled = false;
			m_btnAcceptMoves.Enabled = false;
			m_btnUndoMove.Enabled = false;
			m_btnRedoMove.Enabled = false;
			*/

			m_userChangedOriginalSelection = false;

			

			ClientSideGameManager.CommandManager.RegisterCommandExecutor("System.Windows.Forms.CheckBox", 
																			new CheckBoxCommandExecutor());


			Command endTurnCommand = new Command("MovementTransportOrEndTurn", null,
							new Command.UpdateHandler(UpdateEndTurnCommand));
			ClientSideGameManager.CommandManager.Commands.Add(endTurnCommand);

			endTurnCommand.CommandInstances.Add(m_btnEndMoves);
			endTurnCommand.CommandInstances.Add(m_chkLoadTransports);

			Command moveActiveCommand = new Command("MovementMoveActive", null,
							new Command.UpdateHandler(UpdateMoveActiveCommand));
			ClientSideGameManager.CommandManager.Commands.Add(moveActiveCommand);

			moveActiveCommand.CommandInstances.Add(m_btnRemoveTerritory);
			moveActiveCommand.CommandInstances.Add(m_btnAcceptMoves);
			moveActiveCommand.CommandInstances.Add(m_btnCancelMove);


			Command undoCommand = new Command("MovementUndoMove", null,
							new Command.UpdateHandler(UpdateUndoCommand));
			ClientSideGameManager.CommandManager.Commands.Add(undoCommand);

			undoCommand.CommandInstances.Add(m_btnUndoMove);

			Command redoCommand = new Command("MovementRedoMove", null,
							new Command.UpdateHandler(UpdateRedoCommand));
			ClientSideGameManager.CommandManager.Commands.Add(redoCommand);

			redoCommand.CommandInstances.Add(m_btnRedoMove);

			/*
			Command acceptCancelCommand = new Command("MovementAcceptOrCancelMove", null,
							new Command.UpdateHandler(UpdateAcceptCancelCommand));
			ClientSideGameManager.CommandManager.Commands.Add(acceptCancelCommand);

			acceptCancelCommand.CommandInstances.Add(m_btnAcceptMoves);
			acceptCancelCommand.CommandInstances.Add(m_btnCancelMove);
			*/
		}

		#endregion

		#region plumbing
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
			this.m_btnUndoMove = new System.Windows.Forms.Button();
			this.m_btnRedoMove = new System.Windows.Forms.Button();
			this.m_btnEndMoves = new System.Windows.Forms.Button();
			this.m_btnCancelMove = new System.Windows.Forms.Button();
			this.m_btnAcceptMoves = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.m_lbCurrentMoves = new System.Windows.Forms.ListBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.label2 = new System.Windows.Forms.Label();
			this.m_chkLoadTransports = new System.Windows.Forms.CheckBox();
			this.m_btnRemoveTerritory = new System.Windows.Forms.Button();
			this.m_lbPlayerOrder = new BuckRogers.Interface.PlayerListBox();
			this.tabControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_btnUndoMove
			// 
			this.m_btnUndoMove.Location = new System.Drawing.Point(0, 163);
			this.m_btnUndoMove.Name = "m_btnUndoMove";
			this.m_btnUndoMove.Size = new System.Drawing.Size(72, 23);
			this.m_btnUndoMove.TabIndex = 2;
			this.m_btnUndoMove.Text = "Undo Move";
			this.m_btnUndoMove.Click += new System.EventHandler(this.m_btnUndoMove_Click);
			// 
			// m_btnRedoMove
			// 
			this.m_btnRedoMove.Location = new System.Drawing.Point(76, 163);
			this.m_btnRedoMove.Name = "m_btnRedoMove";
			this.m_btnRedoMove.Size = new System.Drawing.Size(72, 23);
			this.m_btnRedoMove.TabIndex = 3;
			this.m_btnRedoMove.Text = "Redo Move";
			this.m_btnRedoMove.Click += new System.EventHandler(this.m_btnRedoMove_Click);
			// 
			// m_btnEndMoves
			// 
			this.m_btnEndMoves.Location = new System.Drawing.Point(152, 163);
			this.m_btnEndMoves.Name = "m_btnEndMoves";
			this.m_btnEndMoves.Size = new System.Drawing.Size(72, 23);
			this.m_btnEndMoves.TabIndex = 4;
			this.m_btnEndMoves.Text = "End Turn";
			this.m_btnEndMoves.Click += new System.EventHandler(this.m_btnEndMoves_Click);
			// 
			// m_btnCancelMove
			// 
			this.m_btnCancelMove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_btnCancelMove.Location = new System.Drawing.Point(0, 105);
			this.m_btnCancelMove.Name = "m_btnCancelMove";
			this.m_btnCancelMove.Size = new System.Drawing.Size(105, 23);
			this.m_btnCancelMove.TabIndex = 6;
			this.m_btnCancelMove.Text = "Cancel Move";
			this.m_btnCancelMove.Click += new System.EventHandler(this.m_btnCancelMove_Click);
			// 
			// m_btnAcceptMoves
			// 
			this.m_btnAcceptMoves.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_btnAcceptMoves.Location = new System.Drawing.Point(119, 105);
			this.m_btnAcceptMoves.Name = "m_btnAcceptMoves";
			this.m_btnAcceptMoves.Size = new System.Drawing.Size(105, 23);
			this.m_btnAcceptMoves.TabIndex = 7;
			this.m_btnAcceptMoves.Text = "Accept Move";
			this.m_btnAcceptMoves.Click += new System.EventHandler(this.m_btnFinishMove_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 193);
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
			this.m_lbCurrentMoves.Location = new System.Drawing.Point(105, 193);
			this.m_lbCurrentMoves.Name = "m_lbCurrentMoves";
			this.m_lbCurrentMoves.Size = new System.Drawing.Size(120, 82);
			this.m_lbCurrentMoves.TabIndex = 9;
			this.m_lbCurrentMoves.DoubleClick += new System.EventHandler(this.m_lbCurrentMoves_DoubleClick);
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
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(0, 4);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(84, 16);
			this.label2.TabIndex = 14;
			this.label2.Text = "Player order:";
			// 
			// m_chkLoadTransports
			// 
			this.m_chkLoadTransports.Appearance = System.Windows.Forms.Appearance.Button;
			this.m_chkLoadTransports.Location = new System.Drawing.Point(0, 134);
			this.m_chkLoadTransports.Name = "m_chkLoadTransports";
			this.m_chkLoadTransports.Size = new System.Drawing.Size(224, 24);
			this.m_chkLoadTransports.TabIndex = 16;
			this.m_chkLoadTransports.Text = "Load / Unload Transports";
			this.m_chkLoadTransports.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.m_chkLoadTransports.UseVisualStyleBackColor = true;
			this.m_chkLoadTransports.CheckedChanged += new System.EventHandler(this.m_chkLoadTransports_CheckedChanged);
			// 
			// m_btnRemoveTerritory
			// 
			this.m_btnRemoveTerritory.Location = new System.Drawing.Point(0, 212);
			this.m_btnRemoveTerritory.Name = "m_btnRemoveTerritory";
			this.m_btnRemoveTerritory.Size = new System.Drawing.Size(80, 36);
			this.m_btnRemoveTerritory.TabIndex = 18;
			this.m_btnRemoveTerritory.Text = "Remove Territory";
			this.m_btnRemoveTerritory.UseVisualStyleBackColor = true;
			this.m_btnRemoveTerritory.Click += new System.EventHandler(this.m_btnRemoveTerritory_Click);
			// 
			// m_lbPlayerOrder
			// 
			this.m_lbPlayerOrder.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.m_lbPlayerOrder.Location = new System.Drawing.Point(88, 4);
			this.m_lbPlayerOrder.Name = "m_lbPlayerOrder";
			this.m_lbPlayerOrder.Size = new System.Drawing.Size(136, 95);
			this.m_lbPlayerOrder.TabIndex = 13;
			// 
			// MovePanel
			// 
			this.Controls.Add(this.m_chkLoadTransports);
			this.Controls.Add(this.m_btnRemoveTerritory);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.m_lbPlayerOrder);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.m_lbCurrentMoves);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.m_btnAcceptMoves);
			this.Controls.Add(this.m_btnCancelMove);
			this.Controls.Add(this.m_btnEndMoves);
			this.Controls.Add(this.m_btnRedoMove);
			this.Controls.Add(this.m_btnUndoMove);
			this.Name = "MovePanel";
			this.Size = new System.Drawing.Size(240, 600);
			this.tabControl1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#endregion

		#region command handlers
		private void UpdateEndTurnCommand(Command command)
		{
			bool noActiveMove = (m_currentMoveTerritories.Count == 0);
			bool inMovePhase = (m_controller.CurrentPhase == GamePhase.Movement);
			command.Enabled = m_enabled && ClientSideGameManager.IsLocalOrActive 
								&& inMovePhase && noActiveMove;
		}

		private void UpdateMoveActiveCommand(Command command)
		{
			bool activeMove = (m_currentMoveTerritories.Count > 0);
			bool inMovePhase = (m_controller.CurrentPhase == GamePhase.Movement);
			command.Enabled = m_enabled && ClientSideGameManager.IsLocalOrActive 
								&& inMovePhase && activeMove;
		}

		private void UpdateUndoCommand(Command command)
		{
			bool noActiveMove = (m_currentMoveTerritories.Count == 0);
			bool inMovePhase = (m_controller.CurrentPhase == GamePhase.Movement);
			command.Enabled = m_enabled && ClientSideGameManager.IsLocalOrActive 
								&& inMovePhase && noActiveMove && m_controller.CanUndo;
		}

		private void UpdateRedoCommand(Command command)
		{
			bool noActiveMove = (m_currentMoveTerritories.Count == 0);
			bool inMovePhase = (m_controller.CurrentPhase == GamePhase.Movement);
			command.Enabled = m_enabled && ClientSideGameManager.IsLocalOrActive 
								&& inMovePhase && noActiveMove && m_controller.CanRedo;
		}

		#endregion

		#region event handlers

		/*
		private void m_btnAddMove_Click(object sender, System.EventArgs e)
		{
			m_btnCancelMove.Enabled = true;
			m_btnAcceptMoves.Enabled = true;

			m_btnUndoMove.Enabled = false;
			m_btnRedoMove.Enabled = false;
			m_btnEndMoves.Enabled = false;

			m_currentMoveTerritories.Clear();

			if(MoveModeChanged != null)
			{
				MoveModeEventArgs mmea = new MoveModeEventArgs();
				mmea.MoveMode = MoveMode.StartMove;
				m_moveMode = MoveMode.StartMove;

				MoveModeChanged(this, mmea);
			}
		}
		*/

		private void m_btnCancelMove_Click(object sender, System.EventArgs e)
		{
			CancelMove();
		}

		private void m_btnFinishMove_Click(object sender, System.EventArgs e)
		{
			FinalizeCurrentMove();
		}

		private void m_btnUndoMove_Click(object sender, System.EventArgs e)
		{
			m_csgm.PlayerUndidMove();
			//m_btnUndoMove.Enabled = m_controller.CanUndo;
			//m_btnRedoMove.Enabled = m_controller.CanRedo;
		}

		private void m_btnRedoMove_Click(object sender, System.EventArgs e)
		{
			m_csgm.PlayerRedidMove();
			//m_btnUndoMove.Enabled = m_controller.CanUndo;
			//m_btnRedoMove.Enabled = m_controller.CanRedo;
		}

		private void m_btnEndMoves_Click(object sender, System.EventArgs e)
		{
			DialogResult dr = MessageBox.Show("Do you want to end your turn?", "End Turn?", MessageBoxButtons.YesNo,
				MessageBoxIcon.Question);

			if (dr != DialogResult.Yes)
			{
				return;
			}

			m_currentMoveTerritories.Clear();

			m_mlbMoves.Items.Clear();
			m_mlbMoves.Refresh();
			m_mlbTransports.Items.Clear();
			m_mlbTransports.Refresh();
			m_unitsToMove.Clear();

			m_csgm.PlayerFinishedMoving();
			/*
			if (MoveModeChanged != null)
			{
				MoveModeEventArgs mmea = new MoveModeEventArgs();
				mmea.MoveMode = MoveMode.Finished;
				m_moveMode = MoveMode.Finished;

				MoveModeChanged(this, mmea);
			}
			*/

			/*
			m_controller.FinalizeCurrentPlayerMoves();
			if (m_controller.NextPlayer())
			{
				m_lbPlayerOrder.SelectedItem = m_controller.CurrentPlayer;
				m_lbPlayerOrder.Refresh();

			}
			else
			{
				RefreshPlayerOrder();
			}	
			*/
		}

		private void m_lbCurrentMoves_DoubleClick(object sender, System.EventArgs e)
		{
			RemoveTerritoryFromMove();
		}


		private void m_btnRemoveTerritory_Click(object sender, EventArgs e)
		{
			RemoveTerritoryFromMove();
		}

		private void m_chkLoadTransports_CheckedChanged(object sender, EventArgs e)
		{
			//if (MoveModeChanged != null)
			//{
			MoveModeEventArgs mmea = new MoveModeEventArgs();

			if (m_chkLoadTransports.Checked)
			{
				mmea.MoveMode = MoveMode.StartTransport;
				m_moveMode = MoveMode.StartTransport;
			}
			else
			{
				mmea.MoveMode = MoveMode.StartMove;
				m_moveMode = MoveMode.StartMove;
			}
			EventsHelper.Fire(MoveModeChanged, this, mmea);
				//MoveModeChanged(this, mmea);
			//}
		}	


		#endregion

		#region external event handlers

		void OnClientUpdateMessage(object sender, ClientUpdateEventArgs e)
		{
			switch (e.MessageType)
			{
				case GameMessage.PlacementPhaseEnded:
				{
					m_chkLoadTransports.Location = new Point(0, 134);
					break;
				}
				case GameMessage.MovementPhaseStarted:
				{
					// need to have this at a late stage, since GC.Options isn't
					// set by the time MP's constructor is called
					if (GameController.Options.IsNetworkGame)
					{
						m_lbPlayerOrder.ShowPlayerLocation = true;
					}


					goto case GameMessage.NextPlayer;
				}
				case GameMessage.NextPlayer:
				{
					if(m_controller.CurrentPhase != GamePhase.Movement)
					{
						return;
					}

					m_currentMoveTerritories.Clear();

					RefreshPlayerOrder();

					if(ClientSideGameManager.IsLocalOrActive)
					{
						m_enabled = true;
						m_moveMode = MoveMode.StartMove;
					}
					else
					{
						m_enabled = false;
						m_moveMode = MoveMode.None;
					}

					//if (MoveModeChanged != null)
					//{
						MoveModeEventArgs mmea = new MoveModeEventArgs();
						mmea.MoveMode = m_moveMode;

						EventsHelper.Fire(MoveModeChanged, this, mmea);
						//MoveModeChanged(this, mmea);
					//}
					break;
				}
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

		internal void KeyPressed(Keys keyCode)
		{
			if (keyCode == Keys.Enter)
			{
				FinalizeCurrentMove();
			}
			else if (keyCode == Keys.T)
			{
				// Switch the transport state, if we're currently able to
				if (m_chkLoadTransports.Enabled && !m_showingTransportDialog)
				{
					m_chkLoadTransports.Checked = !m_chkLoadTransports.Checked;
				}
			}

		}

		#endregion

		#region primary transport / movement functions

		private void OnTransportLoadClick(Territory t)
		{
			m_showingTransportDialog = true;
			TransportLoadForm tlf = new TransportLoadForm(m_controller);

			tlf.SetupUnits(t, m_controller.CurrentPlayer);
			tlf.ShowDialog();

			m_showingTransportDialog = false;

			if (tlf.DialogResult != DialogResult.OK)
			{
				// actions were undone and disappeared, so effectively
				// nothing happened
				return;
			}

			List<TransportAction> actions = new List<TransportAction>();

			while(tlf.TransferInfo.Count > 0)
			{
				TransportAction ta = (TransportAction)tlf.TransferInfo.Pop();
				actions.Add(ta);
			}

			actions.Reverse();

			m_csgm.PlayerTransportedUnits(actions);
			

			m_mlbMoves.Refresh();
			m_mlbTransports.Refresh();

			return;
		}

		private void OnMoveCreationClick(Territory t, TerritoryEventArgs tcea)
		{
			m_unitTotalsWereChanged = false;
			m_stupidCodeGuardFlag = false;

			// If no move has been started, make sure the player picked
			// a territory where he has units.  
			if (m_currentMoveTerritories.Count == 0)
			{
				if (t.Owner != m_controller.CurrentPlayer)
				{
					string message = String.Empty;
					UnitCollection uc = t.Units.GetUnits(m_controller.CurrentPlayer);

					// If there's no units, bail out
					if (uc.Count == 0)
					{
						MessageBox.Show("You don't have any units in that territory", "Movement",
							MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return;
					}
				}

				// Either the player owns the territory, or he at least has units there.  
				// Make sure we're ready to start counting units.
				m_unitsToMoveCounts.Clear();
			}

			PInputEventArgs e = (PInputEventArgs)tcea.Tag;

			// If this is the first click of the move, or if only the starting territory
			// has been clicked, then we can try to add units
			if (m_currentMoveTerritories.Count == 0
				|| (m_currentMoveTerritories.Count == 1 && t == m_currentMoveTerritories[0]) )
			{
				PNode picked = e.PickedNode;

				// check to see if the player clicked a unit icon
				PNodeList matchingNodes = new PNodeList();
				PointF point = e.Position;
				RectangleF pointRect = new RectangleF(point.X, point.Y, 1, 1);
				picked.FindIntersectingNodes(pointRect, matchingNodes);
				IconInfo clickedInfo = null;
				IconInfo movementInfo = null;

				// something beneath the mouse
				if (matchingNodes.Count > 0)
				{
					PNode topNode = matchingNodes[0];

					if (topNode.Tag is IconInfo)
					{
						// Must have clicked a unit icon
						clickedInfo = (IconInfo)topNode.Tag;

						if (clickedInfo.Player != m_controller.CurrentPlayer)
						{
							return;
						}

						// Find if there's a matching floating movement icon.
						// This comes into play later.
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
					AddUnitsToMove(t, e, clickedInfo, ref movementInfo);

					if (m_currentMoveTerritories.Count == 0)
					{
						m_currentMoveTerritories.Add(t);
						m_lbCurrentMoves.Items.Add(t.Name);

						//m_chkLoadTransports.Enabled = false;
						//m_btnEndMoves.Enabled = false;
					}
				}
				// right mouse button
				else if (e.Button == MouseButtons.Right)
				{
					RemoveUnitsFromMove(e, ref movementInfo);					
				}
				// middle mouse button - ignore
				else
				{
					return;
				}
			}
			// Not the first territory of the move
			else
			{
				// Double-right-clicks are handled by the main form and will never get here.
				// We only care about a single-right-click if it's on the starting territory
				// so we can subtract units, and we don't care about the middle button.
				// So, if it's not a left-click, we can bail out.

				/*
				if(e.Button != MouseButtons.Left)
				{
					return;
				}
				*/

				if(e.Button == MouseButtons.Left)
				{
					ArrayList territoriesToAdd = new ArrayList();
					// Find the shortest path from our starting position to here and add the move
					if (tcea.DoubleClick)
					{
						Territory lastListedTerritory = (Territory)m_currentMoveTerritories[m_currentMoveTerritories.Count - 1];
						ArrayList al = m_controller.Map.Graph.ShortestPath(lastListedTerritory, t);

						territoriesToAdd.AddRange(al);
					}
					// add this one territory to the move
					else
					{
						territoriesToAdd.Add(t);
					}

					MoveAction ma = CreateAndValidateMove(territoriesToAdd, tcea.DoubleClick);

					if (ma.Validated)
					{
						FinalizeOrUpdateMove(tcea.DoubleClick, territoriesToAdd, ma);
					}
				}
				else if(e.Button == MouseButtons.Middle)
				{
					if(!m_map.PathArrowsDisplayed)
					{
						ArrayList al = new ArrayList();
						Territory startingTerritory = (Territory)m_currentMoveTerritories[0];

						foreach(Territory terr in m_currentMoveTerritories)
						{
							al.Add(terr.Name);
						}

						m_map.ShowPathArrows(al);
					}
					else
					{
						m_map.ClearPathArrows();
					}					
				}				
			}

			// Perhaps the user used the MoveUnitsForm to select his units, then
			// clicked on a unit icon or SHIFT-clicked.  In that case, we're going to
			// pop up another MoveUnitsForm when he finishes.  This lets us know
			// that needs to happen.  "stupidCodeGuardFlag" keeps this from happening
			// unless the MoveUnitsForm was used in the first place.
			if(m_unitTotalsWereChanged && !m_stupidCodeGuardFlag)
			{
				m_userChangedOriginalSelection = true;
			}

			return;
		}

		#endregion

		#region secondary movement functions

		private void FinalizeOrUpdateMove(bool finalizeMove, ArrayList territoriesToAdd, MoveAction ma)
		{
			if (ma.Validated)
			{
				if (finalizeMove)
				{
					m_controller.AddAction(ma);

					m_csgm.PlayerMovedUnits(ma);

					//m_btnUndoMove.Enabled = m_controller.CanUndo;
					//m_btnRedoMove.Enabled = m_controller.CanRedo;

					m_handSelectedUnits.Clear();
					m_currentMoveTerritories.Clear();
					m_lbCurrentMoves.Items.Clear();
					

					//m_btnCancelMove.Enabled = false;
					//m_btnAcceptMoves.Enabled = false;
					
					//m_btnEndMoves.Enabled = true;
					//m_chkLoadTransports.Enabled = true;

					m_unitsToMoveCounts.Clear();

					ClearMovementIcons();
					m_map.ClearPathArrows();
				}
				else
				{
					foreach (Territory terr in territoriesToAdd)
					{
						m_currentMoveTerritories.Add(terr);
						m_lbCurrentMoves.Items.Add(terr.Name);
					}

					//m_btnCancelMove.Enabled = true;
					//m_btnAcceptMoves.Enabled = true;

					m_btnAcceptMoves.Focus();
				}
			}
		}

		private MoveAction CreateAndValidateMove(ArrayList territoriesToAdd, bool finalizingMove)
		{
			MoveAction ma = new MoveAction();
			ma.Owner = m_controller.CurrentPlayer;
			ma.StartingTerritory = (Territory)m_currentMoveTerritories[0];

			for (int i = 1; i < m_currentMoveTerritories.Count; i++)
			{
				ma.Territories.Add(m_currentMoveTerritories[i]);
			}

			ma.Territories.AddRange(territoriesToAdd);

			if (m_handSelectedUnits.Count == 0 || m_userChangedOriginalSelection)
			{
				m_muf.SetupUnits(ma.StartingTerritory, m_controller.CurrentPlayer);
				bool userShouldConfirm = m_muf.PreSelectUnits(ma.StartingTerritory, m_controller.CurrentPlayer, m_unitsToMoveCounts);

				if(userShouldConfirm && finalizingMove)
				{
					m_muf.ShowDialog();

					if (m_muf.DialogResult != DialogResult.OK)
					{
						return ma;
					}
				}

				ma.Units.AddAllUnits(m_muf.SelectedUnits);
			}
			else
			{
				ma.Units.AddAllUnits(m_handSelectedUnits);
			}

			if (!m_controller.ValidateAction(ma))
			{
				MessageBox.Show(m_controller.ValidationMessage, "Illegal Move",
								   MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}

			return ma;
		}

		private void RemoveUnitsFromMove(PInputEventArgs e, ref IconInfo movementInfo)
		{
			// If there's no icons displayed, don't do anything.
			if (m_map.MovementIcons.Count == 0)
			{
				return;
			}

			// If the user SHIFT-right-clicked, then remove all the units.
			if ((e.Modifiers & Keys.Shift) == Keys.Shift)
			{
				//ResetMovementInfo();
				//m_unitTotalsWereChanged = true;
				CancelMove();

				return;
			}

			// If the player right-clicked the territory, but didn't click on an icon, 
			// then we're going to decrement the last icon displayed
			if (movementInfo == null)
			{
				movementInfo = (IconInfo)m_map.MovementIcons[m_map.MovementIcons.Count - 1];
			}

			bool removeMovementIcon = false;

			// If the user CTRL-clicked, just remove it
			if ((e.Modifiers & Keys.Control) == Keys.Control)
			{
				removeMovementIcon = true;
			}
			else
			{
				int numToRemove = 1;
				int numUnits = int.Parse(movementInfo.Label.Text);

				// If the user ALT-clicked, remove a max of 10
				// TODO Maybe remove 1/10th of the units (and correspondingly add 1/10th elsewhere?)
				if ((e.Modifiers & Keys.Alt) == Keys.Alt)
				{
					//int oneTenthDisplayed = numUnits / 10;
					//numToRemove = Math.Max(1, oneTenthDisplayed);
					numToRemove = Math.Min(numUnits, 10);
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

				if(m_map.MovementIcons.Count == 0)
				{
					CancelMove();
				}
				else
				{
					m_unitTotalsWereChanged = true;
				}				
			}

			return;
		}

		private void AddUnitsToMove(Territory t, PInputEventArgs e, IconInfo clickedInfo, ref IconInfo movementInfo)
		{
			Hashtable newUnitCounts = null;

			// If the user SHIFT-clicked, then grab the counts of all 
			// units in the territory
			if ((e.Modifiers & Keys.Shift) == Keys.Shift)
			{
				UnitCollection uc = t.Units.GetUnits(UnitType.None, m_controller.CurrentPlayer, t);
				UnitCollection unitsWithMoves = uc.GetUnitsWithMinMoves(1);

				if (unitsWithMoves.Count > 0)
				{
					newUnitCounts = unitsWithMoves.GetUnitTypeCount();
				}
			}

			// territory itself was clicked, and no units have been
			// added so far
			if (clickedInfo == null && m_unitsToMoveCounts.Count == 0 && newUnitCounts == null)
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
			// unit icon was clicked, and none of that type have been added yet
			else if (clickedInfo != null && movementInfo == null && newUnitCounts == null)
			{
				// Set newUnitCounts[unitType] to 1;
				UnitType ut = clickedInfo.Type;

				UnitCollection uc = t.Units.GetUnits(ut, m_controller.CurrentPlayer, t);
				UnitCollection unitsWithMoves = uc.GetUnitsWithMinMoves(1);

				if (unitsWithMoves.Count > 0)
				{

					int numUnitsToAdd = NumUnitsToAdd(e, unitsWithMoves.Count, 0);
					newUnitCounts = new Hashtable();
					newUnitCounts[ut] = numUnitsToAdd;
				}
				// no units with moves left
				else
				{
					return;
				}
			}
			// The user directly clicked this territory, but has previously
			// selected units to move.  We're going to ignore this click.
			else if (clickedInfo == null && m_unitsToMoveCounts.Count > 0 && newUnitCounts == null)
			{
				// not trying to do anything here
				return;
			}


			// If newUnitCounts is not null, then the player has selected units
			// by either using the MoveUnitsForm or by shift-clicking.  
			// We need to add all those units.
			if (newUnitCounts != null)
			{
				foreach (DictionaryEntry de in newUnitCounts)
				{
					Player p = m_controller.CurrentPlayer;
					UnitType ut = (UnitType)de.Key;
					int numUnits = (int)de.Value;

					// it's possible that we already had this type displayed,
					// then the user shift-clicked.  Find it so we can update it.						
					if (m_unitsToMoveCounts.ContainsKey(ut))
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
					// If this type was not displayed previously, we need to add
					// a floating movement icon for it.
					else
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

					// Since the number of units in the territory isn't changed 
					// until the user finishes the move, this is guaranteed to never
					// be more than the number available in the territory.
					movementInfo.Label.Text = numUnits.ToString();
				}

				m_map.UpdateMovementIcons(e);
			}
			// otherwise, the player has clicked on a unit icon.  We need to figure
			// out how many units to add based on that icon's text.
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

			numCurrent += numToAdd;

			if (numCurrent > numTotal)
			{
				numCurrent = numTotal;
			}

			return numCurrent;
		}

		private void FinalizeCurrentMove()
		{
			if (m_currentMoveTerritories.Count > 1)
			{
				ArrayList dummy = new ArrayList();
				MoveAction ma = CreateAndValidateMove(dummy, true);

				FinalizeOrUpdateMove(true, dummy, ma);
			}
		}

		#endregion

		#region movement state functions 

		public void BeginMovement()
		{
			m_currentMoveTerritories.Clear();
			

			/*
			if (MoveModeChanged != null)
			{
				MoveModeEventArgs mmea = new MoveModeEventArgs();
				mmea.MoveMode = MoveMode.StartMove;
				m_moveMode = MoveMode.StartMove;

				MoveModeChanged(this, mmea);
			}
			*/
		}

		private void RemoveTerritoryFromMove()
		{
			if (m_lbCurrentMoves.Items.Count > 0)
			{
				int index = m_lbCurrentMoves.Items.Count - 1;

				m_lbCurrentMoves.Items.RemoveAt(index);
				m_currentMoveTerritories.RemoveAt(index);
			}

			if (m_lbCurrentMoves.Items.Count == 0)
			{
				CancelMove();
			}
		}

		public void ResetMovementInfo()
		{
			m_handSelectedUnits.Clear();
			m_unitsToMoveCounts.Clear();
			ClearMovementIcons();
		}

		public void ClearMovementIcons()
		{
			foreach (IconInfo displayedInfo in m_map.MovementIcons)
			{
				displayedInfo.Composite.RemoveFromParent();
			}

			m_map.MovementIcons.Clear();
		}

		public void CancelMove()
		{
			m_lbCurrentMoves.Items.Clear();
			m_currentMoveTerritories.Clear();

			//m_btnCancelMove.Enabled = false;
			//m_btnAcceptMoves.Enabled = false;
			//m_btnEndMoves.Enabled = true;

			//m_chkLoadTransports.Enabled = true;

			//m_btnUndoMove.Enabled = m_controller.CanUndo;
			//m_btnRedoMove.Enabled = m_controller.CanRedo;

			ResetMovementInfo();
		}

		#endregion

		#region action list functions

		public void RemoveActionFromList(object sender, StatusUpdateEventArgs suea)//Action a)
		{
			Action a = suea.Action;

			if (a is MoveAction)
			{
				m_mlbMoves.Items.Remove(0);
				m_mlbMoves.Refresh();
			}
			else if (a is TransportAction)
			{
				m_mlbTransports.Items.Remove(0);
				m_mlbTransports.Refresh();
			}

			//m_btnUndoMove.Enabled = m_controller.CanUndo;
			//m_btnRedoMove.Enabled = m_controller.CanRedo;
		}

		public void AddActionToList(object sender, StatusUpdateEventArgs suea)//Action a)
		{
			Action a = suea.Action;

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

		#endregion

		#region panel state functions

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
			

			//m_btnAcceptMoves.Enabled = false;
			//m_btnCancelMove.Enabled = false;
			//m_btnEndMoves.Enabled = false;
			//m_btnRedoMove.Enabled = false;
			//m_btnUndoMove.Enabled = false;
		}

		public void EnableMovePanel()
		{
			// Assume that we're in the middle of a normal turn.  Currently, the active
			// player's MoveActions from this turn are not saved, just the current position
			// of his units.  So, we don't need to worry about Undo/Redo.
			//m_btnEndMoves.Enabled = true;
			//m_chkLoadTransports.Enabled = true;
		}

		#endregion
	}
	
}
