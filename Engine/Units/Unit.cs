using System;
using System.Collections;
using System.Diagnostics;

namespace BuckRogers
{
	// This order is based on the combat table in the manual.
	// Anything before Factory should not be moved.
	public enum UnitType
	{
		Trooper = 0,
		Gennie,
		Fighter,
		Battler,
		Transport,
		KillerSatellite,
		Marker,
		Factory,
		Leader,
		None,
	}

	/// <summary>
	/// Summary description for Unit.
	/// </summary>
	[DebuggerDisplay("Unit: {Info}") ]
	public class Unit
	{
		public static Unit NONE = new Unit(Player.NONE, UnitType.None);

		private static int m_nextUnitID = 0;

		private static UnitCollection m_allUnits = new UnitCollection();

		private UnitType m_unitType;
		protected Player m_owner;
		private int m_movesLeft;
		private Territory m_currentTerritory;
		//private UnitCollection m_units;
		private bool m_transported;
		private Unit m_transportingUnit;
		private int m_id;


		public static UnitCollection AllUnits
		{
			get { return Unit.m_allUnits; }
			set { Unit.m_allUnits = value; }
		}

		public int ID
		{
			get
			{
				return this.m_id;
			}
			set
			{
				this.m_id = value;
			}
		}
			

		public Unit(Player owner, UnitType unitType)
		{
			this.Owner = owner;
			this.CurrentTerritory = Territory.NONE;
			m_unitType = unitType;
			MovesLeft = MaxMoves;

			m_id = Unit.m_nextUnitID;
			Unit.m_nextUnitID++;

			/*
			if(m_unitType == UnitType.Transport)
			{
				m_units = new UnitCollection();
			}
			*/
		}

		public static Unit CreateNewUnit(Player owner, UnitType type)
		{
			Unit u;
			switch(type)
			{
				case UnitType.Factory:
					u = new Factory(owner);
					break;
				case UnitType.Transport:
					u = new Transport(owner);
					break;
				default:
					u = new Unit(owner, type);
					break;
			}

			Unit.AllUnits.AddUnit(u);

			return u;
		}

		
		public static int GetCost(UnitType ut)
		{
			int cost = 0;
			switch(ut)
			{
				case UnitType.Trooper:
				case UnitType.Gennie:
					// half a turn = 1
					cost = 1;
					break;
				case UnitType.Fighter:
				case UnitType.Transport:
					cost = 2;
					break;
				case UnitType.Battler:
				case UnitType.KillerSatellite:
				case UnitType.Factory:
					cost = 4;
					break;
			}
			return cost;
		}

		public static ArrayList GetBuildableTypes()
		{
			ArrayList al = new ArrayList();
			al.Add(UnitType.Trooper);
			al.Add(UnitType.Gennie);
			al.Add(UnitType.Fighter);
			al.Add(UnitType.Battler);
			al.Add(UnitType.Transport);
			al.Add(UnitType.KillerSatellite);
			al.Add(UnitType.Factory);

			return al;
		}		

		public int MaxMoves
		{
			get
			{
				int maxMoves = 0;
				switch(m_unitType)
				{
					case UnitType.Battler:
					case UnitType.Leader:
					case UnitType.Fighter:
					case UnitType.Transport:
						maxMoves = 4;
						break;
					
					case UnitType.Factory:
					case UnitType.KillerSatellite:
					case UnitType.Marker:
						// no moves, stay at 0
						break;

					case UnitType.Gennie:
					case UnitType.Trooper:
						maxMoves = 1;
						break;

				}
				return maxMoves;
			}
		}

		public bool IsSpaceCapable
		{
			get
			{
				bool spaceCapable = false;
				switch(m_unitType)
				{
					case UnitType.Battler:
					case UnitType.Leader:
					case UnitType.Fighter:
					case UnitType.Transport:
					case UnitType.KillerSatellite:
						spaceCapable = true;
						break;

					case UnitType.Factory:
					case UnitType.Marker:
					case UnitType.Gennie:
					case UnitType.Trooper:
						// can't move or is a ground unit - stays false
						break;
				}
				return spaceCapable;
			}
		}

		public BuckRogers.Territory CurrentTerritory
		{
			get { return this.m_currentTerritory; }
			set 
			{ 
				if(this.m_currentTerritory != null)
				{
					m_currentTerritory.Units.RemoveUnit(this);
				}
				
				this.m_currentTerritory = value; 
				m_currentTerritory.Units.AddUnit(this);

				if(this.Type == UnitType.Factory)
				{
					Factory f = this as Factory;
					bool canProduce = true;
					if(m_currentTerritory == Territory.NONE)
					{
						canProduce = false;
					}
					else
					{
						UnitCollection factories = m_currentTerritory.Units.GetUnits(UnitType.Factory, m_owner, null);
						if(factories.Count > 1)
						{
							canProduce = false;
						}
					}
					if(!f.IsBlackMarket)
					{
						f.CanProduce = canProduce;
					}
					
				}
			}
		}

		public int MovesLeft
		{
			get { return this.m_movesLeft; }
			set { this.m_movesLeft = value; }
		}

		public virtual BuckRogers.Player Owner
		{
			get { return this.m_owner; }
			set 
			{ 
				
				if(m_owner != null && m_owner != Player.NONE)
				{
					m_owner.Units.RemoveUnit(this);
				}
				
				this.m_owner = value; 
				m_owner.Units.AddUnit(this);
			}
		}

		public BuckRogers.UnitType Type
		{
			get { return this.m_unitType; }
			set { this.m_unitType = value; }
		}

		public bool Transported
		{
			get { return this.m_transported; }
			set { this.m_transported = value; }
		}


		public string Info
		{
			get
			{
				return ID + " - " + Type.ToString() + " - " + Owner.Name + " - " + CurrentTerritory.Name;
			}
			//return base.ToString ();
			
		}

		public Unit TransportingUnit
		{
			get { return this.m_transportingUnit; }
			set 
			{ 
				Unit u = (Unit)value;
				if(u == Unit.NONE)
				{
					this.Transported = false;
					Transport t = (Transport)m_transportingUnit;
					t.Transportees.RemoveUnit(this);

					
				}
				else
				{
					this.Transported = true;
				}
				this.m_transportingUnit = value; 
			}
		}

		public virtual void Destroy()
		{
			if(m_transported)
			{
				TransportingUnit = Unit.NONE;
			}
			
			if(m_owner != null)
			{
				m_owner.Units.RemoveUnit(this);
			}
			
			if(m_currentTerritory != null)
			{
				m_currentTerritory.Units.RemoveUnit(this);
			}

			Unit.AllUnits.RemoveUnit(this);
			

			m_currentTerritory = null;
			m_owner = null;
		}

	}
}
