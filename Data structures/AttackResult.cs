using System;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for AttackResult.
	/// </summary>
	public class AttackResult
	{
		public bool Hit
		{
			get { return this.m_hit; }
			set { this.m_hit = value; }
		}
	
		public int Roll
		{
			get { return this.m_roll; }
			set { this.m_roll = value; }
		}
	
		public BuckRogers.Unit Defender
		{
			get { return this.m_defender; }
			set { this.m_defender = value; }
		}
	
		public BuckRogers.Unit Attacker
		{
			get { return this.m_attacker; }
			set { this.m_attacker = value; }
		}
	
		private Unit m_attacker;
		private Unit m_defender;
		private int m_roll;
		private bool m_hit;

		public AttackResult()
		{
			m_defender = Unit.NONE;
			m_attacker = Unit.NONE;
			m_roll = 0;
			m_hit = false;
			
		}
	}
}
