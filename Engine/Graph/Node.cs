using System;
using System.Collections;

namespace skmDataStructures.Graph
{
	/// <summary>
	/// A Node is uniquely identified by its string Key.  A Node also has a Data property of type object
	/// that can be used to store any extra information associated with the Node.
	/// 
	/// The Node has a property of type AdjacencyList, which represents the node's neighbors.  To add a neighbor,
	/// the Node class exposes an AddDirected() method, which adds a directed edge with an (optional) weight to
	/// some other Node.  These methods are marked internal, and are called by the Graph class.
	/// </summary>
	public class Node
	{
		#region Private Member Variables
		// private member variables
		private string key;
		private object data;		
		private AdjacencyList neighbors;


		#endregion

		#region Constructors
		private Node() {}		// remove default constructor

		public Node(string key, object data) : this(key, data, null) {}

		public Node(string key, object data, AdjacencyList neighbors)
		{
			this.key = key;
			this.data = data;
			if (neighbors == null)
				this.neighbors = new AdjacencyList();
			else
				this.neighbors = neighbors;
		}
		#endregion

		#region Public Methods
		#region Add Methods
		/// <summary>
		/// Adds an unweighted, directed edge from this node to the passed-in Node n.
		/// </summary>
		protected internal virtual void AddDirected(Node n)
		{
			AddDirected(new EdgeToNeighbor(n));
		}

		/// <summary>
		/// Adds a weighted, directed edge from this node to the passed-in Node n.
		/// </summary>
		/// <param name="cost">The weight of the edge.</param>
		protected internal virtual void AddDirected(Node n, int cost)
		{
			AddDirected(new EdgeToNeighbor(n, cost));
		}

		/// <summary>
		/// Adds an edge based on the data in the passed-in EdgeToNeighbor instance.
		/// </summary>
		protected internal virtual void AddDirected(EdgeToNeighbor e)
		{
			neighbors.Add(e);
		}
		#endregion

		#region remove methods
		/// <summary>
		/// Removes an unweighted, directed edge from this node to the passed-in Node n.
		/// </summary>
		protected internal virtual void RemoveDirected(Node n)
		{
			int i = 0;
			for(i = 0; i < neighbors.Count; i++)
			{
				EdgeToNeighbor etn = neighbors[i];
				Node neighbor = etn.Neighbor;

				if(n == neighbor)
				{
					neighbors.RemoveAt(i);
					break;
				}
			}
		}

		#endregion

		public bool AdjacentTo(Node n)
		{
			bool adjacent = false;

			//foreach(EdgeToNeighbor etn in neighbors)
			foreach(Node neighbor in neighbors)
			{
				//Node neighbor = etn.Neighbor;
				if(n == neighbor)
				{
					adjacent = true;
					break;
				}
			}
			return adjacent;
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Returns the Node's Key.
		/// </summary>
		public virtual string Key
		{
			get
			{
				return key;
			}
		}

		/// <summary>
		/// Returns the Node's Data.
		/// </summary>
		public virtual object Data
		{
			get
			{
				return data;
			}
			set
			{
				data = value;
			}
		}

		/// <summary>
		/// Returns an AdjacencyList of the Node's neighbors.
		/// </summary>
		public virtual AdjacencyList Neighbors
		{
			get
			{
				return neighbors;
			}
		}
		#endregion
	}
}
