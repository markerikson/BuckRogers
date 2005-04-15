using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;

namespace BuckRogers.Interface
{
	public delegate void TransportInfoEventHandler(object sender, TransportInfoEventArgs tiea);
	/// <summary>
	/// Summary description for TransportLoadForm.
	/// </summary>
	public class TransportLoadForm : System.Windows.Forms.Form
	{
		public event TransportInfoEventHandler TransportInfo;

		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ListView m_lvUnits;
		private System.Windows.Forms.TreeView m_tvTransports;
		private System.Windows.Forms.Button m_btnLoadOne;
		private System.Windows.Forms.Button m_btnUnloadOne;
		private System.Windows.Forms.Button m_btnUnloadAll;

		private UnitCollection m_transports;
		private UnitCollection m_troopers;
		private UnitCollection m_factories;
		private Territory m_territory;
		private Player m_player;
		private GameController m_controller;
		private ArrayList m_transferInfo;

		private System.Windows.Forms.Button m_btnLoadMax;
		private System.Windows.Forms.Button m_btnCancel;
		private System.Windows.Forms.Button m_btnOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TransportLoadForm(GameController gc)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			m_controller = gc;
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
																													 "7",
																													 "1"}, -1);
			System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
																													 "Trooper",
																													 "4",
																													 "0"}, -1);
			System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
																													 "Factory",
																													 "1",
																													 "0",
																													 "None"}, -1);
			System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
																													 "Factory",
																													 "1",
																													 "0",
																													 "Battler"}, -1);
			this.m_lvUnits = new System.Windows.Forms.ListView();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.m_tvTransports = new System.Windows.Forms.TreeView();
			this.m_btnLoadOne = new System.Windows.Forms.Button();
			this.m_btnLoadMax = new System.Windows.Forms.Button();
			this.m_btnUnloadOne = new System.Windows.Forms.Button();
			this.m_btnUnloadAll = new System.Windows.Forms.Button();
			this.m_btnCancel = new System.Windows.Forms.Button();
			this.m_btnOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// m_lvUnits
			// 
			this.m_lvUnits.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.columnHeader4,
																						this.columnHeader6,
																						this.columnHeader5,
																						this.columnHeader1});
			this.m_lvUnits.HideSelection = false;
			this.m_lvUnits.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
																					  listViewItem1,
																					  listViewItem2,
																					  listViewItem3,
																					  listViewItem4});
			this.m_lvUnits.Location = new System.Drawing.Point(284, 28);
			this.m_lvUnits.Name = "m_lvUnits";
			this.m_lvUnits.Size = new System.Drawing.Size(224, 168);
			this.m_lvUnits.TabIndex = 1;
			this.m_lvUnits.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Type";
			this.columnHeader4.Width = 50;
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "Count";
			this.columnHeader6.Width = 43;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Moves";
			this.columnHeader5.Width = 47;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Producing";
			this.columnHeader1.Width = 63;
			// 
			// m_tvTransports
			// 
			this.m_tvTransports.HideSelection = false;
			this.m_tvTransports.ImageIndex = -1;
			this.m_tvTransports.Location = new System.Drawing.Point(4, 32);
			this.m_tvTransports.Name = "m_tvTransports";
			this.m_tvTransports.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
																					   new System.Windows.Forms.TreeNode("Transport #1 (Moves: 3)", new System.Windows.Forms.TreeNode[] {
																																															new System.Windows.Forms.TreeNode("Trooper (Moves: 1)"),
																																															new System.Windows.Forms.TreeNode("Trooper (Moves: 1)"),
																																															new System.Windows.Forms.TreeNode("Trooper (Moves: 0)")}),
																					   new System.Windows.Forms.TreeNode("Transport #2 (Moves: 4)", new System.Windows.Forms.TreeNode[] {
																																															new System.Windows.Forms.TreeNode("Factory")})});
			this.m_tvTransports.SelectedImageIndex = -1;
			this.m_tvTransports.Size = new System.Drawing.Size(172, 164);
			this.m_tvTransports.TabIndex = 2;
			this.m_tvTransports.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.m_tvTransports_AfterSelect);
			// 
			// m_btnLoadOne
			// 
			this.m_btnLoadOne.Location = new System.Drawing.Point(192, 56);
			this.m_btnLoadOne.Name = "m_btnLoadOne";
			this.m_btnLoadOne.Size = new System.Drawing.Size(80, 23);
			this.m_btnLoadOne.TabIndex = 3;
			this.m_btnLoadOne.Text = "< Load 1";
			this.m_btnLoadOne.Click += new System.EventHandler(this.m_btnLoadOne_Click);
			// 
			// m_btnLoadMax
			// 
			this.m_btnLoadMax.Location = new System.Drawing.Point(192, 84);
			this.m_btnLoadMax.Name = "m_btnLoadMax";
			this.m_btnLoadMax.Size = new System.Drawing.Size(80, 23);
			this.m_btnLoadMax.TabIndex = 4;
			this.m_btnLoadMax.Text = "<< Load Max";
			this.m_btnLoadMax.Click += new System.EventHandler(this.m_btnLoadMax_Click);
			// 
			// m_btnUnloadOne
			// 
			this.m_btnUnloadOne.Location = new System.Drawing.Point(192, 120);
			this.m_btnUnloadOne.Name = "m_btnUnloadOne";
			this.m_btnUnloadOne.Size = new System.Drawing.Size(80, 23);
			this.m_btnUnloadOne.TabIndex = 5;
			this.m_btnUnloadOne.Text = "Unload 1 >";
			this.m_btnUnloadOne.Click += new System.EventHandler(this.m_btnUnloadOne_Click);
			// 
			// m_btnUnloadAll
			// 
			this.m_btnUnloadAll.Location = new System.Drawing.Point(192, 148);
			this.m_btnUnloadAll.Name = "m_btnUnloadAll";
			this.m_btnUnloadAll.Size = new System.Drawing.Size(80, 23);
			this.m_btnUnloadAll.TabIndex = 6;
			this.m_btnUnloadAll.Text = "Unload All >>";
			this.m_btnUnloadAll.Click += new System.EventHandler(this.m_btnUnloadAll_Click);
			// 
			// m_btnCancel
			// 
			this.m_btnCancel.Location = new System.Drawing.Point(88, 240);
			this.m_btnCancel.Name = "m_btnCancel";
			this.m_btnCancel.TabIndex = 29;
			this.m_btnCancel.Text = "Cancel";
			this.m_btnCancel.Click += new System.EventHandler(this.m_btnCancel_Click);
			// 
			// m_btnOK
			// 
			this.m_btnOK.Location = new System.Drawing.Point(4, 240);
			this.m_btnOK.Name = "m_btnOK";
			this.m_btnOK.TabIndex = 28;
			this.m_btnOK.Text = "OK";
			this.m_btnOK.Click += new System.EventHandler(this.m_btnOK_Click);
			// 
			// TransportLoadForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(508, 266);
			this.Controls.Add(this.m_btnCancel);
			this.Controls.Add(this.m_btnOK);
			this.Controls.Add(this.m_btnUnloadAll);
			this.Controls.Add(this.m_btnUnloadOne);
			this.Controls.Add(this.m_btnLoadMax);
			this.Controls.Add(this.m_btnLoadOne);
			this.Controls.Add(this.m_tvTransports);
			this.Controls.Add(this.m_lvUnits);
			this.Name = "TransportLoadForm";
			this.Text = "TransportLoadForm";
			this.ResumeLayout(false);

		}
		#endregion

		public void SetupUnits(Territory t, Player p)
		{
			m_tvTransports.Nodes.Clear();
			m_lvUnits.Items.Clear();

			m_territory = t;
			m_player = p;

			m_transferInfo = new ArrayList();
			m_transports = t.Units.GetUnits(UnitType.Transport, p, null);
			m_troopers = t.Units.GetUnits(UnitType.Trooper, p, null);
			m_factories = t.Units.GetUnits(UnitType.Factory, p, null);

			for(int i = 0; i < m_transports.Count; i++)
			{
				Transport tr = (Transport)m_transports[i];
				//m_transferInfo.Add(new ArrayList());

				m_tvTransports.Nodes.Add("Transport #" + (i + 1));

				TreeNode transportNode = m_tvTransports.Nodes[i];
				foreach(Unit u in tr.Transportees)
				{
					AddUnit(transportNode, u);
				}
			}

			UnitCollection troopers0 = m_troopers.GetUnitsWithMoves(0);
			UnitCollection troopers1 = m_troopers.GetUnitsWithMoves(1);
			
			if(troopers0.Count > 0)
			{
				ListViewItem lvit0 = new ListViewItem();
				lvit0.Text = "Trooper";
				lvit0.SubItems.Add(troopers0.Count.ToString());
				lvit0.SubItems.Add("0");
				m_lvUnits.Items.Add(lvit0);
			}

			if(troopers1.Count > 0)
			{
				ListViewItem lvit1 = new ListViewItem();
				lvit1.Text = "Trooper";
				lvit1.SubItems.Add(troopers1.Count.ToString());
				lvit1.SubItems.Add("1");
				m_lvUnits.Items.Add(lvit1);
			}

			if(m_factories.Count > 0)
			{
				foreach(Factory f in m_factories)
				{
					ListViewItem lvif = new ListViewItem();
					lvif.Text = "Factory";
					lvif.SubItems.Add("1");
					lvif.SubItems.Add("0");

					string production = String.Empty;
					if(f.UnitHalfProduced)
					{
						production = f.ProductionType.ToString();
					}

					lvif.SubItems.Add(production);

					m_lvUnits.Items.Add(lvif);
				}
			}
			
		}

		private void AddUnit(TreeNode node, Unit u)
		{
			string text = "";
			switch(u.UnitType)
			{
				case UnitType.Trooper:
				{
					text = "Trooper (Moves: " + u.MovesLeft + ")";
					break;
				}
				case UnitType.Factory:
				{
					text = "Factory";
					break;
				}
			}

			node.Nodes.Add(text);
		}

		private void m_btnLoadOne_Click(object sender, System.EventArgs e)
		{
			TreeNode transport = m_tvTransports.SelectedNode;

			if(transport == null || transport.Parent != null)
			{
				MessageBox.Show("Units must be added to a transport");
				return;
			}

			if(m_lvUnits.SelectedIndices.Count == 0)
			{
				return;
			}

			int idxUnit = m_lvUnits.SelectedIndices[0];
			ListViewItem lvi = m_lvUnits.Items[idxUnit];

			int numTransportees = transport.Nodes.Count;
			int numMoves = Int32.Parse(lvi.SubItems[2].Text);
			UnitCollection containerCollection = null;

			string nodeText = String.Empty;
			switch(lvi.Text)
			{
				case "Trooper":
				{
					if(numTransportees >= 5)
					{
						MessageBox.Show("Can't load more than 5 troopers into a transport");
						return;
					}

					nodeText = "Trooper (Moves: " + lvi.SubItems[2].Text + ")";
					containerCollection = m_troopers;
					
					break;
				}
				case "Factory":
				{
					if(numTransportees > 0)
					{
						MessageBox.Show("Can't load a factory into a transport that is already carrying units");
						return;
					}
					nodeText = "Factory";
					containerCollection = m_factories;
					break;
					
				}
			}

			UnitCollection transportees = containerCollection.GetUnits(m_player, 1).GetUnitsWithMoves(numMoves);
			
			try
			{
				TransportAction ta = new TransportAction();
				ta.Owner = m_player;
				ta.StartingTerritory = m_territory;

				int idxTransport = m_tvTransports.Nodes.IndexOf(transport);
				ta.Transport = (Transport)m_transports[idxTransport];
				ta.Units = transportees;
				ta.Load = true;
				ta.MaxTransfer = 1;

				m_controller.AddAction(ta);
				m_transferInfo.Add(ta);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
				return;
			}

			containerCollection.RemoveAllUnits(transportees);

			/*
			StringBuilder sb = new StringBuilder();
			sb.Append("Transportation #");
			sb.Append(m_transferInfo.Count + 1);
			sb.Append("%");
			sb.Append("Territory: ");
			sb.Append(m_territory.Name);
			sb.Append("\n");
			sb.Append("Loaded: 1 ");
			sb.Append(lvi.Text);

			m_transferInfo.Add(sb.ToString());
			*/

			transport.Nodes.Add(nodeText);

			int numUnits = Int32.Parse(lvi.SubItems[1].Text);
			if(numUnits == 1)
			{
				m_lvUnits.Items.Remove(lvi);
			}
			else
			{
				numUnits--;
				lvi.SubItems[1].Text = numUnits.ToString();
			}
		}

		private void m_btnLoadMax_Click(object sender, System.EventArgs e)
		{
			TreeNode transport = m_tvTransports.SelectedNode;

			if(transport == null || transport.Parent != null)
			{
				MessageBox.Show("Units must be added to a transport");
				return;
			}

			if(m_lvUnits.SelectedIndices.Count == 0)
			{
				return;
			}

			int idxUnit = m_lvUnits.SelectedIndices[0];
			ListViewItem lvi = m_lvUnits.Items[idxUnit];

			int numTransportees = transport.Nodes.Count;

			StringBuilder sb = new StringBuilder();

			string nodeText = String.Empty;
			switch(lvi.Text)
			{
				case "Trooper":
				{
					if(numTransportees >= 5)
					{
						MessageBox.Show("Can't load more than 5 troopers into a transport");
						return;
					}

					nodeText = "Trooper (Moves: " + lvi.SubItems[2].Text + ")";

					int numUnits = Int32.Parse(lvi.SubItems[1].Text);
					int numMoves = Int32.Parse(lvi.SubItems[2].Text);
					int numEmptySpaces = 5 - numTransportees;
					int numToTransfer = Math.Min(numEmptySpaces, numUnits);


					//UnitCollection m_
					UnitCollection transportees = m_troopers.GetUnits(m_player, numToTransfer).GetUnitsWithMoves(numMoves);
			
					try
					{
						TransportAction ta = new TransportAction();
						ta.Owner = m_player;
						ta.StartingTerritory = m_territory;

						int idxTransport = m_tvTransports.Nodes.IndexOf(transport);
						ta.Transport = (Transport)m_transports[idxTransport];
						ta.Units = transportees;
						ta.Load = true;
						ta.MaxTransfer = numToTransfer;

						m_controller.AddAction(ta);
						
						m_transferInfo.Add(ta);
					}
					catch(Exception ex)
					{
						MessageBox.Show(ex.Message);
						return;
					}

					m_troopers.RemoveAllUnits(transportees);

					
/*
					sb.Append("Transportation #");
					sb.Append(m_transferInfo.Count + 1);
					sb.Append("%");
					sb.Append("Territory: ");
					sb.Append(m_territory.Name);
					sb.Append("\n");
					sb.Append("Loaded: ");
					sb.Append(numToTransfer);
					sb.Append(" ");
					sb.Append(lvi.Text);
					
					if(numToTransfer > 1)
					{
						sb.Append("s");
					}

					m_transferInfo.Add(sb.ToString());
*/
					for(int i = 0; i < numToTransfer; i++)
					{
						transport.Nodes.Add(nodeText);
						numUnits--;
					}



					if(numUnits == 0)
					{
						m_lvUnits.Items.Remove(lvi);
					}
					else
					{
						lvi.SubItems[1].Text = numUnits.ToString();
					}
					break;
				}
				case "Factory":
				{
					if(numTransportees > 0)
					{
						MessageBox.Show("Can't load a factory into a transport that is already carrying units");
						return;
					}
					nodeText = "Factory";

					transport.Nodes.Add(nodeText);

					int numUnits = Int32.Parse(lvi.SubItems[1].Text);
					if(numUnits == 1)
					{
						m_lvUnits.Items.Remove(lvi);
					}
					break;
				}
			}

		}


		private void m_tvTransports_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			//m_btnUnloadOne
		}

		private void m_btnUnloadOne_Click(object sender, System.EventArgs e)
		{
			TreeNode unit = m_tvTransports.SelectedNode;

			if(unit == null || unit.Parent == null)
			{
				MessageBox.Show("Must select a unit to remove");
				return;
			}

			int moves = 0;

			if(unit.Text.StartsWith("Tr"))
			{
				moves = Int32.Parse(unit.Text.Substring(16, 1));				
			}

			try
			{
				TransportAction ta = new TransportAction();
				ta.Owner = m_player;
				ta.StartingTerritory = m_territory;

				int idxTransport = m_tvTransports.Nodes.IndexOf(unit.Parent);
				ta.Transport = (Transport)m_transports[idxTransport];
				//ta.Units = transportees;
				ta.Load = false;
				ta.MaxTransfer = 1;
				ta.Moves = moves;

				m_controller.AddAction(ta);
						
				m_transferInfo.Add(ta);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
				return;
			}
			
			UnloadUnit(unit, moves);

			unit.Remove();
		}

		private void UnloadUnit(TreeNode unit, int moves)
		{
			string title = unit.Text;

			string type = String.Empty;
			//string moves = String.Empty;
			string count = "1";
			string production = String.Empty;

			if(title.StartsWith("Tr"))
			{
				type = "Trooper";
				//moves = title.Substring(16, 1);				
			}
			else if(title.StartsWith("Fa"))
			{
				type = "Factory";
				//moves = "0";
			}

			bool addNew = false;
			if(m_lvUnits.Items.Count == 0 || type == "Factory")
			{
				addNew = true;
			}			
			else
			{
				ListViewItem lvi = m_lvUnits.Items[m_lvUnits.Items.Count - 1];
				
				if(lvi.Text == type && lvi.SubItems[2].Text == moves.ToString())
				{
					string sNumUnits = lvi.SubItems[1].Text;
					int numUnits = Int32.Parse(sNumUnits);
					numUnits++;
					lvi.SubItems[1].Text = numUnits.ToString();
				}
				else
				{
					addNew = true;
				}
			}

			if(addNew)
			{
				ListViewItem lvi = new ListViewItem();
				lvi.Text = type;
				lvi.SubItems.Add(count);
				lvi.SubItems.Add(moves.ToString());				
				lvi.SubItems.Add(production);

				m_lvUnits.Items.Add(lvi);
			}
		}

		private void m_btnUnloadAll_Click(object sender, System.EventArgs e)
		{
			TreeNode transport = m_tvTransports.SelectedNode;

			if(transport == null || transport.Parent != null)
			{
				MessageBox.Show("Units must be added to a transport");
				return;
			}

			int moves = 0;

			try
			{
				TransportAction ta = new TransportAction();
				ta.Owner = m_player;
				ta.StartingTerritory = m_territory;

				int idxTransport = m_tvTransports.Nodes.IndexOf(transport);
				ta.Transport = (Transport)m_transports[idxTransport];
				//ta.Units = transportees;
				ta.Load = false;
				ta.MaxTransfer = 5;
				ta.Moves = moves;
				ta.MatchMoves = false;

				m_controller.AddAction(ta);
						
				m_transferInfo.Add(ta);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
				return;
			}

			foreach(TreeNode node in transport.Nodes)
			{
				if(node.Text.StartsWith("Tr"))
				{
					moves = Int32.Parse(node.Text.Substring(16, 1));				
				}
				UnloadUnit(node, moves);
			}

			transport.Nodes.Clear();

		}

		private void m_btnCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void m_btnOK_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		public System.Collections.ArrayList TransferInfo
		{
			get { return this.m_transferInfo; }
			set { this.m_transferInfo = value; }
		}		
	}
}
