using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using skmDataStructures.Graph;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.ListBox listBox2;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.ComboBox comboBox2;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader player;
		private System.Windows.Forms.ColumnHeader roll;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.ListView listView2;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ListBox listBox3;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.ListView listView3;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.Button button6;
		private ListViewColumnSorter lvwColumnSorter;
		private Button button7;
		private GameMap gm;

		public Form1()
		{
			gm = new GameMap();
			
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			lvwColumnSorter = new ListViewColumnSorter();
			listView3.ListViewItemSorter = lvwColumnSorter;

			ArrayList names = new ArrayList();
			foreach(Node node in gm.m_graph.Nodes)
			{
				names.Add(node.Key);
			}

			string[] namearray = (string[])names.ToArray(typeof(string));
			Array.Sort(namearray);

			listBox1.Items.Clear();
			comboBox1.Items.Clear();
			comboBox2.Items.Clear();
			foreach(string Name in namearray)
			{
				listBox1.Items.Add(Name);
				comboBox1.Items.Add(Name);
				comboBox2.Items.Add(Name);
			}
			comboBox1.SelectedIndex = 0;
			comboBox2.SelectedIndex = 0;

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
			this.button1 = new System.Windows.Forms.Button();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.listBox2 = new System.Windows.Forms.ListBox();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.comboBox2 = new System.Windows.Forms.ComboBox();
			this.listView1 = new System.Windows.Forms.ListView();
			this.player = new System.Windows.Forms.ColumnHeader();
			this.roll = new System.Windows.Forms.ColumnHeader();
			this.button5 = new System.Windows.Forms.Button();
			this.listView2 = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.listBox3 = new System.Windows.Forms.ListBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.listView3 = new System.Windows.Forms.ListView();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.button6 = new System.Windows.Forms.Button();
			this.button7 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(8, 232);
			this.button1.Name = "button1";
			this.button1.TabIndex = 0;
			this.button1.Text = "Urb Neighbors";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// listBox1
			// 
			this.listBox1.Location = new System.Drawing.Point(8, 8);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(176, 173);
			this.listBox1.TabIndex = 1;
			this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// listBox2
			// 
			this.listBox2.Location = new System.Drawing.Point(200, 8);
			this.listBox2.Name = "listBox2";
			this.listBox2.Size = new System.Drawing.Size(176, 173);
			this.listBox2.TabIndex = 2;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(96, 232);
			this.button2.Name = "button2";
			this.button2.TabIndex = 3;
			this.button2.Text = "Earth Orbit";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(184, 232);
			this.button3.Name = "button3";
			this.button3.TabIndex = 4;
			this.button3.Text = "Advance Planets";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(272, 232);
			this.button4.Name = "button4";
			this.button4.TabIndex = 5;
			this.button4.Text = "Shortest Path";
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// comboBox1
			// 
			this.comboBox1.Location = new System.Drawing.Point(8, 192);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(176, 21);
			this.comboBox1.TabIndex = 6;
			this.comboBox1.Text = "comboBox1";
			// 
			// comboBox2
			// 
			this.comboBox2.Location = new System.Drawing.Point(200, 192);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(176, 21);
			this.comboBox2.TabIndex = 7;
			this.comboBox2.Text = "comboBox2";
			// 
			// listView1
			// 
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.player,
            this.roll});
			this.listView1.Location = new System.Drawing.Point(8, 272);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(128, 176);
			this.listView1.TabIndex = 8;
			this.listView1.View = System.Windows.Forms.View.Details;
			// 
			// player
			// 
			this.player.Text = "Player";
			this.player.Width = 81;
			// 
			// roll
			// 
			this.roll.Text = "Roll";
			this.roll.Width = 42;
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(120, 456);
			this.button5.Name = "button5";
			this.button5.TabIndex = 9;
			this.button5.Text = "Initiative";
			this.button5.Click += new System.EventHandler(this.button5_Click);
			// 
			// listView2
			// 
			this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
			this.listView2.Location = new System.Drawing.Point(144, 272);
			this.listView2.Name = "listView2";
			this.listView2.Size = new System.Drawing.Size(128, 176);
			this.listView2.TabIndex = 10;
			this.listView2.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Player";
			this.columnHeader1.Width = 73;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Roll";
			this.columnHeader2.Width = 50;
			// 
			// listBox3
			// 
			this.listBox3.Location = new System.Drawing.Point(280, 272);
			this.listBox3.Name = "listBox3";
			this.listBox3.Size = new System.Drawing.Size(96, 173);
			this.listBox3.TabIndex = 11;
			// 
			// checkBox1
			// 
			this.checkBox1.Location = new System.Drawing.Point(16, 456);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.TabIndex = 12;
			this.checkBox1.Text = "Check parity";
			// 
			// listView3
			// 
			this.listView3.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
			this.listView3.Location = new System.Drawing.Point(416, 8);
			this.listView3.Name = "listView3";
			this.listView3.Size = new System.Drawing.Size(328, 440);
			this.listView3.TabIndex = 13;
			this.listView3.View = System.Windows.Forms.View.Details;
			this.listView3.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView3_ColumnClick);
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Player";
			this.columnHeader3.Width = 69;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Type";
			this.columnHeader4.Width = 82;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Territory";
			this.columnHeader5.Width = 165;
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(408, 464);
			this.button6.Name = "button6";
			this.button6.TabIndex = 14;
			this.button6.Text = "Play";
			this.button6.Click += new System.EventHandler(this.button6_Click);
			// 
			// button7
			// 
			this.button7.Location = new System.Drawing.Point(516, 464);
			this.button7.Name = "button7";
			this.button7.TabIndex = 15;
			this.button7.Text = "Init";
			this.button7.Click += new System.EventHandler(this.button7_Click);
			// 
			// Form1
			// 
			this.ClientSize = new System.Drawing.Size(832, 558);
			this.Controls.Add(this.button7);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.listView3);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.listBox3);
			this.Controls.Add(this.listView2);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.listView1);
			this.Controls.Add(this.comboBox2);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.listBox2);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.button1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			/*
			Graph g = new Graph();
			Node testnode = new Node("Blah blah", null);
			Node[] nodes = new Node[10];

			g.AddNode(testnode);


			
			for(int i = 0; i < nodes.Length; i++)
			{
				nodes[i] = new Node("Node " + i.ToString(), null);
				//testnode.ConnectTo(nodes[i]);
				g.AddNode(nodes[i]);
				g.AddUndirectedEdge(testnode, nodes[i]);
			}
			*/
			Node urbres = gm.m_graph.Nodes["Urban Reservations"];
			StringBuilder sb = new StringBuilder();
			//foreach(EdgeToNeighbor en in urbres.Neighbors)
			foreach(Node neighbor in urbres.Neighbors)
			{
				//Node neighbor = en.Neighbor;
				sb.Append(neighbor.Key);
				sb.Append("\n");
			}
			MessageBox.Show(sb.ToString());
			/*
			if(farside.AdjacentTo(tranq))//nodes[4].AdjacentTo(nodes[3]))//testnode.ConnectedTo(nodes[3]))
			{
				MessageBox.Show(farside.Key + " is adjacent to " + tranq.Key);
			}
			else
			{
				MessageBox.Show(farside.Key + " is NOT adjacent to " + tranq.Key);
			}
			*/
			
		}
		/*
		[STAThread]
		public static void Main(String[] args)
		{
			//Application.Run(new Form1());
		}
		*/

		private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string item = (string)listBox1.SelectedItem;
			listBox2.Items.Clear();

			Node node = gm.m_graph.Nodes[item];

			ArrayList names = new ArrayList();
			//foreach(EdgeToNeighbor etn in node.Neighbors)
			foreach(Node neighbor in node.Neighbors)
			{
				//Node neighbor = etn.Neighbor;
				names.Add(neighbor.Key);
			}

			string[] namearray = (string[])names.ToArray(typeof(string));
			Array.Sort(namearray);

			foreach(string Name in namearray)
			{
				listBox2.Items.Add(Name);
			}
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			Node eo3 = gm.m_graph.Nodes["Earth Orbit: 3"];
			StringBuilder sb = new StringBuilder();

			Node currentNode = eo3;
			OrbitalPath eo = (OrbitalPath)gm.OrbitalPaths["Earth Orbit"];
			for(int i = 0; i < 9; i++)
			{
				int currentNodeIndex = eo.NextOrbitalNodeIndex(currentNode);
				currentNode = eo[currentNodeIndex];
				sb.Append(currentNode.Key);
				sb.Append("\n");
			}
			MessageBox.Show(sb.ToString());
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			gm.AdvancePlanets();
		}

		private void button4_Click(object sender, System.EventArgs e)
		{
			string name1 = (string)comboBox1.Text;
			string name2 = (string)comboBox2.Text;

			Node node1 = gm.m_graph.Nodes[name1];
			Node node2 = gm.m_graph.Nodes[name2];
			ArrayList al = gm.m_graph.ShortestPath(node1, node2);

			StringBuilder sb = new StringBuilder();
			foreach(Node n in al)
			{
				sb.Append(n.Key);
				sb.Append("\n");
			}
			MessageBox.Show(sb.ToString());
		}

		private void button5_Click(object sender, System.EventArgs e)
		{
			string[] players = {"Mark", "Chris", "Stu", "Hannah", "Jake", "Kathryn"};
			GameController gc = new GameController(players);
			gc.RollForInitiative(checkBox1.Checked);

			listView1.Items.Clear();
			listView2.Items.Clear();
			foreach(TurnRoll roll in gc.Rolls)
			{
				string[] items = {roll.Player.Name, roll.Roll.ToString()};
				ListViewItem lvi = new ListViewItem(items);
				listView1.Items.Add(lvi);
			}

			foreach(TurnRoll tr in gc.m_rollResults)
			{
				string[] items = {tr.Player.Name, tr.Roll.ToString()};
				ListViewItem lvi = new ListViewItem(items);
				listView2.Items.Add(lvi);
			}

			listBox3.Items.Clear();
			foreach(Player p in gc.PlayerOrder)
			{
				listBox3.Items.Add(p.Name);
			}



		}

		private void button6_Click(object sender, System.EventArgs e)
		{
			string[] players = {"Mark", "Chris", "Stu", "Hannah", "Jake", "Kathryn"};
			GameController gc = new GameController(players);


			gc.PlayGame();

			listView2.Items.Clear();
			listView3.Items.Clear();
			foreach(Player p in gc.Players)
			{
				//foreach(Territory t in p.Territories.Values)
				//foreach(Unit u in p.Units)
				for(int i = 0; i < p.Units.Count; i++)
				{
					Unit u = p.Units[i];
					string[] items = {p.Name, u.UnitType.ToString(), u.CurrentTerritory.Name};
					ListViewItem lvi = new ListViewItem(items);
					listView3.Items.Add(lvi);

				}
			}
		}

		private void listView3_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			// Determine if clicked column is already the column that is being sorted.
			if ( e.Column == lvwColumnSorter.SortColumn )
			{
				// Reverse the current sort direction for this column.
				if (lvwColumnSorter.Order == SortOrder.Ascending)
				{
					lvwColumnSorter.Order = SortOrder.Descending;
				}
				else
				{
					lvwColumnSorter.Order = SortOrder.Ascending;
				}
			}
			else
			{
				// Set the column number that is to be sorted; default to ascending.
				lvwColumnSorter.SortColumn = e.Column;
				lvwColumnSorter.Order = SortOrder.Ascending;
			}

			// Perform the sort with these new sort options.
			this.listView3.Sort();
		}

		private void button7_Click(object sender, EventArgs e)
		{
			ControllerTest ct = new ControllerTest();
			ct.Init();
		}
	}

	/// <summary>
	/// This class is an implementation of the 'IComparer' interface.
	/// </summary>
	public class ListViewColumnSorter : IComparer
	{
		/// <summary>
		/// Specifies the column to be sorted
		/// </summary>
		private int ColumnToSort;
		/// <summary>
		/// Specifies the order in which to sort (i.e. 'Ascending').
		/// </summary>
		private SortOrder OrderOfSort;
		/// <summary>
		/// Case insensitive comparer object
		/// </summary>
		private CaseInsensitiveComparer ObjectCompare;

		/// <summary>
		/// Class constructor.  Initializes various elements
		/// </summary>
		public ListViewColumnSorter()
		{
			// Initialize the column to '0'
			ColumnToSort = 0;

			// Initialize the sort order to 'none'
			OrderOfSort = SortOrder.None;

			// Initialize the CaseInsensitiveComparer object
			ObjectCompare = new CaseInsensitiveComparer();
		}

		/// <summary>
		/// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
		/// </summary>
		/// <param name="x">First object to be compared</param>
		/// <param name="y">Second object to be compared</param>
		/// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
		public int Compare(object x, object y)
		{
			int compareResult;
			ListViewItem listviewX, listviewY;

			// Cast the objects to be compared to ListViewItem objects
			listviewX = (ListViewItem)x;
			listviewY = (ListViewItem)y;

			// Compare the two items
			compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text,listviewY.SubItems[ColumnToSort].Text);

			// Calculate correct return value based on object comparison
			if (OrderOfSort == SortOrder.Ascending)
			{
				// Ascending sort is selected, return normal result of compare operation
				return compareResult;
			}
			else if (OrderOfSort == SortOrder.Descending)
			{
				// Descending sort is selected, return negative result of compare operation
				return (-compareResult);
			}
			else
			{
				// Return '0' to indicate they are equal
				return 0;
			}
		}

		/// <summary>
		/// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
		/// </summary>
		public int SortColumn
		{
			set
			{
				ColumnToSort = value;
			}
			get
			{
				return ColumnToSort;
			}
		}

		/// <summary>
		/// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
		/// </summary>
		public SortOrder Order
		{
			set
			{
				OrderOfSort = value;
			}
			get
			{
				return OrderOfSort;
			}
		}

	}
}
