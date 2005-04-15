using System;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for TransportAction.
	/// </summary>
	public class TransportAction : Action
	{
		public int MaxTransfer
		{
			get 
			{ 
				if(m_maxTransfer == 0)
				{
					return m_transport.Transportees.Count;
				}
				return this.m_maxTransfer; 
			}
			set { this.m_maxTransfer = value; }
		}
	
		public bool Load
		{
			get { return this.m_load; }
			set { this.m_load = value; }
		}
	
		public BuckRogers.UnitType UnitType
		{
			get { return this.m_unitType; }
			set { this.m_unitType = value; }
		}
	
		public BuckRogers.Transport Transport
		{
			get { return this.m_transport; }
			set { this.m_transport = value; }
		}

		public int Moves
		{
			get { return this.m_moves; }
			set { this.m_moves = value; }
		}

		public bool MatchMoves
		{
			get { return this.m_matchMoves; }
			set { this.m_matchMoves = value; }
		}
	
		private bool m_load;
		private bool m_matchMoves;
		private int m_maxTransfer;
		private int m_moves;
		private Transport m_transport;
		private UnitType m_unitType;
		public TransportAction()
		{
			m_maxTransfer = 0;
		}

	}
}
