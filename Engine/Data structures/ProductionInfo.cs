using System;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for ProductionInfo.
	/// </summary>
	public class ProductionInfo
	{
		private Factory m_factory;
		private UnitType m_type;
		private Territory m_outputTerritory;

		public ProductionInfo()
		{
		}

		public BuckRogers.Factory Factory
		{
			get { return this.m_factory; }
			set { this.m_factory = value; }
		}

		public BuckRogers.Territory DestinationTerritory
		{
			get { return this.m_outputTerritory; }
			set { this.m_outputTerritory = value; }
		}

		public BuckRogers.UnitType Type
		{
			get { return this.m_type; }
			set { this.m_type = value; }
		}
	}
}
