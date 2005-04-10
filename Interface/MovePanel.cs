using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Text;

using BuckRogers;

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

		private System.Windows.Forms.Button m_btnAddMove;
		private System.Windows.Forms.Button m_btnUndoMove;
		private System.Windows.Forms.Button m_btnRedoMove;

		private MoveListBox m_listbox;
		private GameController m_controller;
		private System.Windows.Forms.Label m_labMoves;
		private System.Windows.Forms.Button m_btnCancelMove;
		private System.Windows.Forms.Button m_btnEndMoves;
		private System.Windows.Forms.Button m_btnFinishMove;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox m_lbCurrentMoves;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MovePanel()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			this.Dock = DockStyle.Fill;

			m_listbox = new MoveListBox();

			int listboxY = m_labMoves.Bottom + 4;//m_labMoves.Bottom + 8;
			int listboxHeight = ClientSize.Height - listboxY;


			this.DockPadding.Top = listboxY;
			this.DockPadding.Right = 8;
			this.DockPadding.Bottom = 4;

			
			m_listbox.Bounds = new Rectangle(0, 0, this.ClientSize.Width - 4, listboxHeight);
			m_listbox.Parent = this;
			m_listbox.Dock = DockStyle.Fill;
			
			//m_listbox.Anchor = AnchorStyles.Bottom;
			//m_listbox.Dock = DockStyle.Bottom;

			/*
			m_listbox.Items.Add("Title 1", "Testing some text\nwith several lines in it\nto see what happens");
			m_listbox.Items.Add("Move #6", "From: American Regency\nTo: Urban Reservations\nUnits: 1 Gennie, 2 Troopers, 2 Fighters, 1 Killer Satellite, 15 Battlers, 3 Transports, and various other stuff\nYou know, that's a lot of stuff to move");

			for(int i = 10; i >= 0; i--)
			{
				m_listbox.Items.Add("Item #" + i, "Line 1\nLine 2\nLine 3");
			}
			*/


			m_lbCurrentMoves.Items.Clear();
			m_currentMoveTerritories = new ArrayList();

			m_btnCancelMove.Enabled = false;
			m_btnFinishMove.Enabled = false;
			m_btnUndoMove.Enabled = false;
			m_btnRedoMove.Enabled = false;
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
			this.m_labMoves = new System.Windows.Forms.Label();
			this.m_btnCancelMove = new System.Windows.Forms.Button();
			this.m_btnFinishMove = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.m_lbCurrentMoves = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// m_btnAddMove
			// 
			this.m_btnAddMove.Location = new System.Drawing.Point(0, 4);
			this.m_btnAddMove.Name = "m_btnAddMove";
			this.m_btnAddMove.Size = new System.Drawing.Size(72, 23);
			this.m_btnAddMove.TabIndex = 0;
			this.m_btnAddMove.Text = "Add Move";
			this.m_btnAddMove.Click += new System.EventHandler(this.m_btnAddMove_Click);
			// 
			// m_btnUndoMove
			// 
			this.m_btnUndoMove.Location = new System.Drawing.Point(0, 32);
			this.m_btnUndoMove.Name = "m_btnUndoMove";
			this.m_btnUndoMove.Size = new System.Drawing.Size(72, 23);
			this.m_btnUndoMove.TabIndex = 2;
			this.m_btnUndoMove.Text = "Undo Move";
			this.m_btnUndoMove.Click += new System.EventHandler(this.m_btnUndoMove_Click);
			// 
			// m_btnRedoMove
			// 
			this.m_btnRedoMove.Location = new System.Drawing.Point(76, 32);
			this.m_btnRedoMove.Name = "m_btnRedoMove";
			this.m_btnRedoMove.Size = new System.Drawing.Size(72, 23);
			this.m_btnRedoMove.TabIndex = 3;
			this.m_btnRedoMove.Text = "Redo Move";
			this.m_btnRedoMove.Click += new System.EventHandler(this.m_btnRedoMove_Click);
			// 
			// m_btnEndMoves
			// 
			this.m_btnEndMoves.Location = new System.Drawing.Point(152, 32);
			this.m_btnEndMoves.Name = "m_btnEndMoves";
			this.m_btnEndMoves.TabIndex = 4;
			this.m_btnEndMoves.Text = "End Moves";
			// 
			// m_labMoves
			// 
			this.m_labMoves.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_labMoves.Location = new System.Drawing.Point(4, 152);
			this.m_labMoves.Name = "m_labMoves";
			this.m_labMoves.Size = new System.Drawing.Size(56, 20);
			this.m_labMoves.TabIndex = 5;
			this.m_labMoves.Text = "Moves:";
			// 
			// m_btnCancelMove
			// 
			this.m_btnCancelMove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_btnCancelMove.Location = new System.Drawing.Point(76, 4);
			this.m_btnCancelMove.Name = "m_btnCancelMove";
			this.m_btnCancelMove.Size = new System.Drawing.Size(72, 23);
			this.m_btnCancelMove.TabIndex = 6;
			this.m_btnCancelMove.Text = "Cancel";
			this.m_btnCancelMove.Click += new System.EventHandler(this.m_btnCancelMove_Click);
			// 
			// m_btnFinishMove
			// 
			this.m_btnFinishMove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_btnFinishMove.Location = new System.Drawing.Point(152, 4);
			this.m_btnFinishMove.Name = "m_btnFinishMove";
			this.m_btnFinishMove.Size = new System.Drawing.Size(72, 23);
			this.m_btnFinishMove.TabIndex = 7;
			this.m_btnFinishMove.Text = "Accept";
			this.m_btnFinishMove.Click += new System.EventHandler(this.m_btnFinishMove_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(4, 64);
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
			this.m_lbCurrentMoves.Location = new System.Drawing.Point(92, 64);
			this.m_lbCurrentMoves.Name = "m_lbCurrentMoves";
			this.m_lbCurrentMoves.Size = new System.Drawing.Size(136, 82);
			this.m_lbCurrentMoves.TabIndex = 9;
			// 
			// MovePanel
			// 
			this.Controls.Add(this.m_lbCurrentMoves);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.m_btnFinishMove);
			this.Controls.Add(this.m_btnCancelMove);
			this.Controls.Add(this.m_labMoves);
			this.Controls.Add(this.m_btnEndMoves);
			this.Controls.Add(this.m_btnRedoMove);
			this.Controls.Add(this.m_btnUndoMove);
			this.Controls.Add(this.m_btnAddMove);
			this.Name = "MovePanel";
			this.Size = new System.Drawing.Size(240, 400);
			this.ResumeLayout(false);

		}
		#endregion

		private void m_btnAddMove_Click(object sender, System.EventArgs e)
		{
			m_btnCancelMove.Enabled = true;
			m_btnFinishMove.Enabled = true;

			m_btnAddMove.Enabled = false;
			m_btnUndoMove.Enabled = false;
			m_btnRedoMove.Enabled = false;
			m_btnEndMoves.Enabled = false;

			m_currentMoveTerritories.Clear();

			if(MoveModeChanged != null)
			{
				MoveModeEventArgs mmea = new MoveModeEventArgs();
				mmea.MoveMode = MoveMode.Started;

				MoveModeChanged(this, mmea);
			}
		}


		public void TerritoryClicked(Territory t)
		{

			if(m_currentMoveTerritories.Count == 0)
			{
				MoveUnitsForm muf = new MoveUnitsForm();

				muf.SetupUnits(t, m_controller.CurrentPlayer);

				muf.ShowDialog();

				if(muf.DialogResult != DialogResult.OK)
				{
					return;
				}

				m_unitsToMove = muf.SelectedUnits;

			}
			m_currentMoveTerritories.Add(t);

			m_lbCurrentMoves.Items.Add(t.Name);
		}

		private void m_btnCancelMove_Click(object sender, System.EventArgs e)
		{
			m_lbCurrentMoves.Items.Clear();
			m_currentMoveTerritories.Clear();

			m_btnCancelMove.Enabled = false;
			m_btnFinishMove.Enabled = false;

			m_btnAddMove.Enabled = true;
			m_btnEndMoves.Enabled = true;

			m_btnUndoMove.Enabled = m_controller.CanUndo;
			m_btnRedoMove.Enabled = m_controller.CanRedo;

			if(MoveModeChanged != null)
			{
				MoveModeEventArgs mmea = new MoveModeEventArgs();
				mmea.MoveMode = MoveMode.Finished;

				MoveModeChanged(this, mmea);
			}

		}

		private void m_btnFinishMove_Click(object sender, System.EventArgs e)
		{
			

			m_btnCancelMove.Enabled = false;
			m_btnFinishMove.Enabled = false;

			m_btnAddMove.Enabled = true;
			m_btnEndMoves.Enabled = true;

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

				AddActionToList(ma);

				if(MoveModeChanged != null)
				{
					MoveModeEventArgs mmea = new MoveModeEventArgs();
					mmea.MoveMode = MoveMode.Finished;

					MoveModeChanged(this, mmea);
				}
			}
			catch(ActionException aex)
			{
				MessageBox.Show(aex.Message);
			}

			m_btnUndoMove.Enabled = m_controller.CanUndo;
			m_btnRedoMove.Enabled = m_controller.CanRedo;

			m_lbCurrentMoves.Items.Clear();
			m_currentMoveTerritories.Clear();
		
		}

		private void m_btnUndoMove_Click(object sender, System.EventArgs e)
		{
			m_controller.UndoAction();

			m_listbox.Items.Remove(0);
			m_listbox.Refresh();

			m_btnUndoMove.Enabled = m_controller.CanUndo;
			m_btnRedoMove.Enabled = m_controller.CanRedo;
		}

		private void m_btnRedoMove_Click(object sender, System.EventArgs e)
		{
			Action a = m_controller.RedoAction();

			AddActionToList(a);

			m_btnUndoMove.Enabled = m_controller.CanUndo;
			m_btnRedoMove.Enabled = m_controller.CanRedo;
		}

		private void AddActionToList(Action a)
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
					
				m_listbox.Items.Insert(0, "Move #" + (m_listbox.Items.Count + 1).ToString(),
					sb.ToString());
				m_listbox.Refresh();


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
	}
	
}
