using System;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for Action.
	/// </summary>
	public class Action
	{

		private Player m_owner;
		private UnitCollection m_units;
		private Territory m_startingTerritory;

		public Action()
		{
			m_owner = Player.NONE;
			m_units = new UnitCollection();
		}

		public BuckRogers.Player Owner
		{
			get { return this.m_owner; }
			set { this.m_owner = value; }
		}

		public BuckRogers.UnitCollection Units
		{
			get { return this.m_units; }
			set { this.m_units = value; }
		}

		public BuckRogers.Territory StartingTerritory
		{
			get { return this.m_startingTerritory; }
			set { this.m_startingTerritory = value; }
		}
	}
}
