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

		public Factory(Player owner)
			: base(owner, UnitType.Factory)
		{
			//
			// TODO: Add constructor logic here
			//
			m_productionType = UnitType.None;
			m_canProduce = true;
			
		}

		public void StartProduction(UnitType unitType)
		{
			StartProduction(unitType, this.CurrentTerritory);
		}

		public void StartProduction(UnitType unitType, Territory outputTerritory)
		{
			if(m_canProduce)
			{
				m_productionType = unitType;
				m_unitHalfProduced = false;
				m_outputTerritory = outputTerritory;
			}			
		}

		public void CancelProduction()
		{
			m_productionType = UnitType.None;
			m_unitHalfProduced = false;
		}

		public void ExecuteProduction()
		{
			if(m_productionType == UnitType.None || !m_canProduce)
			{
				return;
			}
			
			switch(m_productionType)
			{
				case UnitType.Trooper:
				case UnitType.Gennie:
					CreateUnit();
					CreateUnit();
					break;
				case UnitType.Fighter:
				case UnitType.Transport:
					CreateUnit();
					break;
				case UnitType.Battler:
				case UnitType.KillerSatellite:	
				case UnitType.Factory:
					if(m_unitHalfProduced)
					{
						CreateUnit();
					}
					else
					{
						m_unitHalfProduced = true;
					}
					break;
			}
		}

		private void CreateUnit()
		{
			Unit unit;
			if(m_productionType == UnitType.Factory)
			{
				unit = new Factory(m_owner);
			}
			else
			{
				unit = Unit.CreateNewUnit(this.Owner, m_productionType);
			}
			unit.CurrentTerritory = m_outputTerritory;
			m_owner.Units.AddUnit(unit);
			m_productionType = UnitType.None;
		}
	}
}
