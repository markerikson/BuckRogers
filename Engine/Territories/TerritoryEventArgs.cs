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
		private System.Windows.Forms.MouseButtons m_button;

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

		public System.Windows.Forms.MouseButtons Button
		{
			get { return this.m_button; }
			set { this.m_button = value; }
		}

		
	}
}
