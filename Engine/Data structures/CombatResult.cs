using System;
using System.Collections;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for CombatResult.
	/// </summary>
	public class CombatResult
	{
		public System.Collections.ArrayList AttackResults
		{
			get { return this.m_attackResults; }
			set { this.m_attackResults = value; }
		}
	
		public UnitCollection UnusedAttackers
		{
			get { return this.m_unusedAttackers; }
			set { this.m_unusedAttackers = value; }
		}

		public UnitCollection UsedAttackers
		{
			get { return this.m_usedAttackers; }
			set { this.m_usedAttackers = value; }
		}
	
		public UnitCollection Survivors
		{
			get { return this.m_survivors; }
			set { this.m_survivors = value; }
		}
	
		public UnitCollection Casualties
		{
			get { return this.m_casualties; }
			set { this.m_casualties = value; }
		}
	
		private UnitCollection m_survivors;
		private UnitCollection m_casualties;
		private UnitCollection m_usedAttackers;
		private UnitCollection m_unusedAttackers;
		private ArrayList m_attackResults;

		public CombatResult()
		{
			m_survivors = new UnitCollection();
			m_casualties = new UnitCollection();
			m_usedAttackers = new UnitCollection();
			m_unusedAttackers = new UnitCollection();
			m_attackResults = new ArrayList();
		}
	}
}
