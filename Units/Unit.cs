using System;

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
	public class Unit
	{
		public static Unit NONE = new Unit(Player.NONE, UnitType.None);
		

		private UnitType m_unitType;
		protected Player m_owner;
		private int m_movesLeft;
		private Territory m_currentTerritory;
		//private UnitCollection m_units;
		private bool m_transported;
		private Unit m_transportingUnit;

		
			

		public Unit(Player owner, UnitType unitType)
		{
			this.Owner = owner;
			this.CurrentTerritory = Territory.NONE;
			m_unitType = unitType;
			MovesLeft = MaxMoves;

			/*
			if(m_unitType == UnitType.Transport)
			{
				m_units = new UnitCollection();
			}
			*/
		}

		public static Unit CreateNewUnit(Player owner, UnitType type)
		{
			switch(type)
			{
				case UnitType.Factory:
					return new Factory(owner);
				case UnitType.Transport:
					return new Transport(owner);
				default:
					return new Unit(owner, type);
			}
		}

		/*
		public int Cost
		{
			get
			{
				int cost = 1;
				switch(m_unitType)
				{
					case UnitType.Trooper:
					case UnitType.Gennie:
						// half a turn = 1
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
		}
		*/

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
				this.m_owner = value; 
				m_owner.Units.AddUnit(this);
			}
		}

		public BuckRogers.UnitType UnitType
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
				return UnitType.ToString() + " - " + Owner.Name + " - " + CurrentTerritory.Name;
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

	}
}
