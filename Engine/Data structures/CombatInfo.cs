using System;
using System.Collections;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for CombatInfo.
	/// </summary>
	public class CombatInfo
	{
		public BuckRogers.BattleType Type
		{
			get { return this.m_type; }
			set { this.m_type = value; }
		}
	
		public UnitCollection Attackers
		{
			get { return this.m_attackers; }
			set { this.m_attackers = value; }
		}

		public bool AttackingLeader
		{
			get { return this.m_attackingLeader; }
			set { this.m_attackingLeader = value; }
		}

		public UnitCollection Defenders
		{
			get { return this.m_defenders; }
			set { this.m_defenders = value; }
		}
	
		private BattleType m_type;
		private UnitCollection m_attackers;
		private UnitCollection m_defenders;
		private bool m_attackingLeader;


		public CombatInfo()
		{
			m_attackers = new UnitCollection();
			m_defenders = new UnitCollection();
			m_attackingLeader = false;
			m_type = BattleType.None;
		}
	}
}
