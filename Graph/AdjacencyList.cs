using System;
using System.Collections;


namespace skmDataStructures.Graph
{
	/// <summary>
	/// AdjacencyList maintains a list of neighbors for a particular <see cref="Node"/>.  It is derived from CollectionBase
	/// and provides a strongly-typed collection of <see cref="EdgeToNeighbor"/> instances.
	/// </summary>
	public class AdjacencyList : CollectionBase
	{
		/// <summary>
		/// Adds a new <see cref="EdgeToNeighbor"/> instance to the AdjacencyList.
		/// </summary>
		/// <param name="e">The <see cref="EdgeToNeighbor"/> instance to add.</param>
		protected internal virtual void Add(EdgeToNeighbor e)
		{
			base.InnerList.Add(e);
		}

		/// <summary>
		/// Returns a particular <see cref="EdgeToNeighbor"/> instance by index.
		/// </summary>
		public virtual EdgeToNeighbor this[int index]
		{
			get { return (EdgeToNeighbor) base.InnerList[index]; }
			set { base.InnerList[index] = value; }
		}

		public new IEnumerator GetEnumerator()
		{
			return new AdjacencyEnumerator(this);
		}

		private class AdjacencyEnumerator : IEnumerator
		{
			private int m_index;
			private AdjacencyList m_list;

			public AdjacencyEnumerator(AdjacencyList list)
			{
				m_list = list;
				Reset();
			}

			#region IEnumerator Members


			public void Reset()
			{
				m_index = -1;
			}

			public object Current
			{
				get
				{
					EdgeToNeighbor etn = m_list[m_index];
					return etn.Neighbor;
				}
			}

			public bool MoveNext()
			{
				m_index++;
				return (m_index < m_list.Count);
			}

			#endregion

		}

	}
}
