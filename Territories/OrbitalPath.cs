using System;
using System.Collections;
using skmDataStructures.Graph;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for OrbitalPath.
	/// </summary>
	public class OrbitalPath
	{
		public Territory[] Territories
		{
			get { return this.m_territories; }
			set { this.m_territories = value; }
		}

		public Territory this[int index]
		{
			get
			{
				return m_territories[index];
			}
			set
			{
				m_territories[index] = value;
			}
		}

		public int Length
		{
			get
			{
				return m_territories.Length;
			}
		}
		
	
		private Territory[] m_territories;

		public OrbitalPath(int size)
		{
			//
			// TODO: Add constructor logic here
			//
			m_territories = new Territory[size];
		}

		public int NextOrbitalNodeIndex(Node current)
		{
			bool found = false;
			int i;
			for(i = 0; i < m_territories.Length; i++)
			{
				if(m_territories[i] == current)
				{
					found = true;
					break;
				}
			}

			if(found)
			{
				return NextOrbitalNodeIndex(i);
			}
			return 0;
		}

		public int NextOrbitalNodeIndex(int currentIndex)
		{
			if(currentIndex == (m_territories.Length - 1))
			{
				return 0;
			}
			currentIndex++;
			return currentIndex;
		}
	}
}
