using System;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for Factory.
	/// </summary>
	public class Factory : Unit
	{
		//private Unit m_producing;
		private UnitType m_productionType;
		private bool m_canProduce;
		private bool m_unitHalfProduced;
		private Territory m_outputTerritory;
		private static int m_productionPerTurn = 2;
		private int m_partialProduction;

		public Factory(Player owner)
			: base(owner, UnitType.Factory)
		{
			m_productionType = UnitType.None;
			m_canProduce = true;
			m_partialProduction = 0;
			
		}

		// TODO Base production off of unit costs (needed for double production)
		public bool StartProduction(UnitType unitType)
		{
			return StartProduction(unitType, this.CurrentTerritory);
		}

		public bool StartProduction(UnitType unitType, Territory outputTerritory)
		{
			bool startedProduction = true;

			if(m_canProduce)
			{

				switch(unitType)
				{
					case UnitType.KillerSatellite:
					case UnitType.Battler:
					{
						// TODO Return a bool or throw an exception, not both
						if(outputTerritory == this.CurrentTerritory)
						{
							startedProduction = false;
							throw new Exception("Can't produce a satellite or battler and put it on the ground");
						}
						break;
					}
				}
				if(startedProduction)
				{
					m_productionType = unitType;
					m_unitHalfProduced = false;
					m_outputTerritory = outputTerritory;
				}
			}			

			return startedProduction;
		}

		public void CancelProduction()
		{
			m_productionType = UnitType.None;
			m_unitHalfProduced = false;
			m_partialProduction = 0;
		}

		public void ExecuteProduction()
		{
			if(m_productionType == UnitType.None || !m_canProduce)
			{
				return;
			}

			int unitCost = Unit.GetCost(m_productionType);

			int i = 0;
			for(i = m_productionPerTurn + m_partialProduction; (i - unitCost) >= 0; i -= unitCost)
			{
                CreateUnit();
			}

			m_partialProduction = i;

			if(m_partialProduction == 0)
			{
				m_productionType = UnitType.None;
			}
			
		}

		private void CreateUnit()
		{
			Unit unit;
			if(m_productionType == UnitType.Factory)
			{
				unit = new Factory(m_owner);

				if(m_outputTerritory == this.CurrentTerritory)
				{
					((Factory)unit).CanProduce = false;
				}
			}
			else
			{
				unit = Unit.CreateNewUnit(this.Owner, m_productionType);
			}
			unit.CurrentTerritory = m_outputTerritory;
			//m_owner.Units.AddUnit(unit);
			
		}

		public bool CanProduce
		{
			get { return this.m_canProduce; }
			set { this.m_canProduce = value; }
		}

		public BuckRogers.UnitType ProductionType
		{
			get { return this.m_productionType; }
			//set { this.m_productionType = value; }
		}

		public bool UnitHalfProduced
		{
			get { return m_partialProduction > 0; }//return this.m_unitHalfProduced; }
			//set { this.m_unitHalfProduced = value; }
		}

		public static int ProductionPerTurn
		{
			get { return m_productionPerTurn; }
			set { m_productionPerTurn = value; }
		}
	}
}
