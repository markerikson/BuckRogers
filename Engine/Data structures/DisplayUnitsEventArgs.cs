using System;

namespace BuckRogers
{
	public enum DisplayCategory
	{
		Attackers,
		Defenders,
		UnusedAttackers,
		SurvivingDefenders,
	}
	/// <summary>
	/// Summary description for DisplayUnitsEventArgs.
	/// </summary>
	public class DisplayUnitsEventArgs : EventArgs
	{
		private DisplayCategory m_category;
		private UnitCollection m_units;
		public DisplayUnitsEventArgs()
		{
			
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
	}
}
