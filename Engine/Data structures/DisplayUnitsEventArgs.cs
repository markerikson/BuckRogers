using System;

namespace BuckRogers
{
	public enum DisplayCategory
	{
		Attackers,
		Defenders,
		UnusedAttackers,
		SurvivingDefenders,
		DeadUnits,
		NonCombatUnits,
	}
	/// <summary>
	/// Summary description for DisplayUnitsEventArgs.
	/// </summary>
	public class DisplayUnitsEventArgs : EventArgs
	{
		private DisplayCategory m_category;
		private UnitCollection m_units;
		private Territory m_territory;
		private Player m_player;		

		
		public DisplayUnitsEventArgs()
		{
			
		}

		public Player Player
		{
			get { return m_player; }
			set { m_player = value; }
		}

		public BuckRogers.DisplayCategory Category
		{
			get { return this.m_category; }
			set { this.m_category = value; }
		}

		public BuckRogers.UnitCollection Units
		{
			get { return this.m_units; }
			set { this.m_units = value; }
		}

		public Territory Territory
		{
			get { return m_territory; }
			set { m_territory = value; }
		}
	}
}
