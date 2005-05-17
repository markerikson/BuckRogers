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
		//private bool m_unitHalfProduced;
		private Territory m_outputTerritory;
		private static int m_productionPerTurn = 2;
		private int m_partialProduction;
		private bool m_blackMarket;

		public Factory(Player owner)
			: base(owner, UnitType.Factory)
		{
			m_productionType = UnitType.None;
			m_canProduce = true;
			m_partialProduction = 0;
			m_outputTerritory = Territory.NONE;
			m_blackMarket = false;
			
		}

		public bool StartProduction(UnitType unitType)
		{
			return StartProduction(unitType, this.CurrentTerritory);
		}

		public bool StartProduction(UnitType unitType, Territory outputTerritory)
		{
			bool startedProduction = false;

			if(m_canProduce)
			{
				/*
				if(m_partialProduction == 0)
				{
					throw new Exception("Can't change production while a unit is partially produced");
				}
				*/

				m_productionType = unitType;
				m_outputTerritory = outputTerritory;
				startedProduction = true;
			}			

			return startedProduction;
		}

		public void ClearProduction()
		{
			m_productionType = UnitType.None;
			//m_unitHalfProduced = false;
			m_partialProduction = 0;
			m_outputTerritory = Territory.NONE;
		}

		public void ExecuteProduction()
		{
			if(m_productionType == UnitType.None || !m_canProduce)
			{
				return;
			}

			if(m_blackMarket)
			{
				switch(m_productionType)
				{
					case UnitType.Trooper:
					case UnitType.Fighter:
					{
						CreateUnit();
						m_partialProduction = 0;
						break;
					}
					case UnitType.Factory:
					{
						if(m_partialProduction > 0)
						{
							CreateUnit();
							m_partialProduction = 0;
							CanProduce = false;
						}
						else
						{
							m_partialProduction = 1;
						}
						break;
					}
				}
			}
			else
			{
				int unitCost = Unit.GetCost(m_productionType);

				int i = 0;
				for(i = m_productionPerTurn + m_partialProduction; (i - unitCost) >= 0; i -= unitCost)
				{
					CreateUnit();
				}

				m_partialProduction = i;
			}
		

			if(m_partialProduction == 0)
			{
				ClearProduction();
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

		public Territory DestinationTerritory
		{
			get
			{
				return m_outputTerritory;
			}
		}

		public float AmountProduced
		{
			get
			{
				if(m_productionType == UnitType.None)
				{
					return 0.0f;
				}

				if(m_blackMarket)
				{
					switch(m_productionType)
					{
						case UnitType.Trooper:
						case UnitType.Fighter:
							return 1.0f;

						case UnitType.Factory:
						{
							if(m_partialProduction > 0)
							{
								return 1.0f;
							}
							else
							{
								return 0.5f;
							}
						}

						default:
							return 0f;
					}
				}
				
				float amount = (m_productionPerTurn + m_partialProduction) / (float)Unit.GetCost(m_productionType);
				return amount;
			}
		}

		public bool IsBlackMarket
		{
			get { return this.m_blackMarket; }
			set { this.m_blackMarket = value; }
		}
	}
}
