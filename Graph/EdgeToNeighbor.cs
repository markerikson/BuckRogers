using System;

namespace skmDataStructures.Graph
{
	/// <summary>
	/// EdgeToNeighbor represents an edge eminating from one <see cref="Node"/> to its neighbor.  The EdgeToNeighbor
	/// class, then, contains a reference to the neighbor and the weight of the edge.
	/// </summary>
	public class EdgeToNeighbor
	{
		#region Private Member Variables
		// private member variables
		private int cost;		
		private Node neighbor;
		#endregion

		#region Constructors
		private EdgeToNeighbor() {}

		public EdgeToNeighbor(Node neighbor) : this(neighbor, 0) {}

		public EdgeToNeighbor(Node neighbor, int cost)
		{
			this.cost = cost;
			this.neighbor = neighbor;
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// The weight of the edge.
		/// </summary>
		/// <remarks>A value of 0 would indicate that there is no weight, and is the value used when an unweighted
		/// edge is added via the <see cref="Graph"/> class.</remarks>
		public virtual int Cost
		{
			get
			{
				return cost;
			}
		}

		/// <summary>
		/// The neighbor the edge is leading to.
		/// </summary>
		public virtual Node Neighbor
		{
			get
			{
				return neighbor;
			}
		}
		#endregion
	}
}
