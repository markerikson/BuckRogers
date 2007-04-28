using System;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for TerritoryUnitsEventArgs.
	/// </summary>
	public class TerritoryUnitsEventArgs : EventArgs
	{
		private Player m_player;
		private Territory m_territory;
		private UnitCollection m_units;
		private bool m_added;

		public TerritoryUnitsEventArgs()
		{
		}

		public BuckRogers.Player Player
		{
			get { return this.m_player; }
			set { this.m_player = value; }
		}

		public BuckRogers.Territory Territory
		{
			get { return this.m_territory; }
			set { this.m_territory = value; }
		}

		public BuckRogers.UnitCollection Units
		{
			get { return this.m_units; }
			set { this.m_units = value; }
		}

		public bool Added
		{
			get { return this.m_added; }
			set { this.m_added = value; }
		}
	}
}
