using System;

namespace BuckRogers
{
	public enum BattleType
	{
		None = 0, 
		KillerSatellite = 1,
		Bombing,
		Normal,
	}
	/// <summary>
	/// Summary description for BattleInfo.
	/// </summary>
	public class BattleInfo
	{
		public BuckRogers.BattleType Type
		{
			get { return this.m_type; }
			set { this.m_type = value; }
		}
	
		public Territory Territory
		{
			get { return this.m_territory; }
			set { this.m_territory = value; }
		}

		public override string ToString()
		{
			//return base.ToString ();
			return m_territory.Name + " - " + m_type.ToString();
		}

	
		private BattleType m_type;
		private Territory m_territory;
		public BattleInfo()
		{
			m_type = BattleType.None;
			m_territory = Territory.NONE;
		}
	}
}
