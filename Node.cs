using System;
using System.Collections;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for Node.
	/// </summary>
	public class Node
	{
		
		private ArrayList connections;

		private string text;
		private string name;
		
		/// <summary>
		/// Property Name (string)
		/// </summary>
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}		
		/// <summary>
		/// Property Text (string)
		/// </summary>
		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
			}
		}		
		/// <summary>
		/// Property Connections (ArrayList)
		/// </summary>
		public ArrayList Connections
		{
			get
			{
				return this.connections;
			}
			set
			{
				this.connections = value;
			}
		}

		public Node(string newName)
		{
			name = newName;
			//
			// TODO: Add constructor logic here
			//
			connections = new ArrayList();
			text = "Not connected";
		}

		public bool ConnectedTo(Node checking)
		{
			
			
			bool connected = connections.Contains(checking);

			if(connected)
			{
				this.text = "Connected";
			}

			return connected;
			//connections.
			/*
			for (int i = 0; i < connections.Count; ++i) 
			{
				Node stored = (Node)connections[i];
				if(checking.Equals(stored))
				{
					connected = true;
				}
			}
			*/
			//return connected;
		}

		public void ConnectTo(Node newConnection)
		{
			connections.Add(newConnection);
		}

	}
}
