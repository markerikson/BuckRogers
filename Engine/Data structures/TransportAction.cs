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
	
		private bool m_load;
		private int m_maxTransfer;
		private Transport m_transport;
		private UnitType m_unitType;
		public TransportAction()
		{
			m_maxTransfer = 0;
		}

	}
}
