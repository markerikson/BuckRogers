using System;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for TerritoryClickEventArgs.
	/// </summary>
	public class TerritoryEventArgs : System.EventArgs
	{
		private string m_territoryName;
		private Player m_owner;

		public TerritoryEventArgs()
		{

		}

		public TerritoryEventArgs(string s)
		{
			m_territoryName = s;
		}

		public string Name
		{
			get { return this.m_territoryName; }
			set { this.m_territoryName = value; }
		}

		public BuckRogers.Player Owner
		{
			get { return this.m_owner; }
			set { this.m_owner = value; }
		}

		
	}
}
