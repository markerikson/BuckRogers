using System;
using System.Collections;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for Move.
	/// </summary>
	public class MoveAction : Action
	{
		
	
		public ArrayList Territories
		{
			get { return this.m_territories; }
			set { this.m_territories = value; }
		}

		public ArrayList m_originalMovesLeft;
		public Hashtable m_conqueredTerritories;

		
	
		private ArrayList m_territories;
		

		public MoveAction()
		{
			m_territories = new ArrayList();
			m_conqueredTerritories = new Hashtable();
			m_originalMovesLeft	 = new ArrayList();
		}

		public System.Collections.ArrayList OriginalMovesLeft
		{
			get { return this.m_originalMovesLeft; }
			set { this.m_originalMovesLeft = value; }
		}

		public System.Collections.Hashtable ConqueredTerritories
		{
			get { return this.m_conqueredTerritories; }
			set { this.m_conqueredTerritories = value; }
		}

		
	}
}
