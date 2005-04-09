using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using BuckRogers;

namespace BuckRogers.Interface
{
	/// <summary>
	/// Summary description for MovePanel.
	/// </summary>
	public class MovePanel : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Button m_btnAddMove;
		private System.Windows.Forms.Button m_btnUndoMove;
		private System.Windows.Forms.Button m_btnRedoMove;

		private MoveListBox m_listbox;
		private GameController m_controller;
		private System.Windows.Forms.Button m_btnDone;
		private System.Windows.Forms.Label m_labMoves;

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

			int listboxY = m_labMoves.Bottom + 8;
			int listboxHeight = ClientSize.Height - listboxY;
			m_listbox.Bounds = new Rectangle(0, listboxY, this.ClientSize.Width - 8, listboxHeight);

			m_listbox.Parent = this;

			this.DockPadding.Top = listboxY;
			this.DockPadding.Right = 8;
			this.DockPadding.Bottom = 4;

			m_listbox.Dock = DockStyle.Fill;

			m_listbox.Items.Add("Title 1", "Testing some text\nwith several lines in it\nto see what happens");
			m_listbox.Items.Add("Move #6", "From: American Regency\nTo: Urban Reservations\nUnits: 1 Gennie, 2 Troopers, 2 Fighters, 1 Killer Satellite, 15 Battlers, 3 Transports, and various other stuff\nYou know, that's a lot of stuff to move");

			for(int i = 10; i >= 0; i--)
			{
				m_listbox.Items.Add("Item #" + i, "Line 1\nLine 2\nLine 3");
			}
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
			this.m_btnDone = new System.Windows.Forms.Button();
			this.m_labMoves = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_btnAddMove
			// 
			this.m_btnAddMove.Location = new System.Drawing.Point(0, 4);
			this.m_btnAddMove.Name = "m_btnAddMove";
			this.m_btnAddMove.Size = new System.Drawing.Size(72, 23);
			this.m_btnAddMove.TabIndex = 0;
			this.m_btnAddMove.Text = "Add Move";
			// 
			// m_btnUndoMove
			// 
			this.m_btnUndoMove.Location = new System.Drawing.Point(80, 4);
			this.m_btnUndoMove.Name = "m_btnUndoMove";
			this.m_btnUndoMove.Size = new System.Drawing.Size(72, 23);
			this.m_btnUndoMove.TabIndex = 2;
			this.m_btnUndoMove.Text = "Undo Move";
			// 
			// m_btnRedoMove
			// 
			this.m_btnRedoMove.Location = new System.Drawing.Point(160, 4);
			this.m_btnRedoMove.Name = "m_btnRedoMove";
			this.m_btnRedoMove.Size = new System.Drawing.Size(72, 23);
			this.m_btnRedoMove.TabIndex = 3;
			this.m_btnRedoMove.Text = "Redo Move";
			// 
			// m_btnDone
			// 
			this.m_btnDone.Location = new System.Drawing.Point(80, 32);
			this.m_btnDone.Name = "m_btnDone";
			this.m_btnDone.TabIndex = 4;
			this.m_btnDone.Text = "Done";
			// 
			// m_labMoves
			// 
			this.m_labMoves.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_labMoves.Location = new System.Drawing.Point(4, 64);
			this.m_labMoves.Name = "m_labMoves";
			this.m_labMoves.Size = new System.Drawing.Size(100, 20);
			this.m_labMoves.TabIndex = 5;
			this.m_labMoves.Text = "Moves:";
			// 
			// MovePanel
			// 
			this.Controls.Add(this.m_labMoves);
			this.Controls.Add(this.m_btnDone);
			this.Controls.Add(this.m_btnRedoMove);
			this.Controls.Add(this.m_btnUndoMove);
			this.Controls.Add(this.m_btnAddMove);
			this.Name = "MovePanel";
			this.Size = new System.Drawing.Size(240, 400);
			this.ResumeLayout(false);

		}
		#endregion

		/*
		public GameController Controller
		{
			get { return this.m_controller; }
			set { this.m_controller = value; }
		}
		*/
	}
	
}
