using System;
using skmDataStructures.Graph;

namespace BuckRogers
{
	public enum TerritoryType
	{
		Space,
		Ground,
	}

	/// <summary>
	/// Summary description for Territory.
	/// </summary>
	public class Territory : skmDataStructures.Graph.Node
	{
		public BuckRogers.TerritoryType Type
		{
			get { return this.m_type; }
			//set { this.m_type = value; }
		}

		public BuckRogers.UnitCollection Units
		{
			get { return this.m_units; }
			set { this.m_units = value; }
		}

		public BuckRogers.Player Owner
		{
			get { return this.owner; }
			set 
			{ 
				if(this.owner != null)
				{
					owner.Territories.Remove(this.Name);
				}
				this.owner = value; 
				owner.Territories[this.Name] = this;
			}
		}

		public string Name
		{
			get
			{
				return this.Key;
			}
		}

		public BuckRogers.OrbitalSystem System
		{
			get { return this.m_system; }
			set { this.m_system = value; }
		}

		

		private TerritoryType m_type;
		private Player owner;
		private UnitCollection m_units;
		private OrbitalSystem m_system;
		private OrbitalPath m_orbit;

		public static Territory NONE = new Territory("None", TerritoryType.Space);



		public Territory(string name, TerritoryType type)
			: base(name, null)
		{
			this.Owner = Player.NONE;
			m_units = new UnitCollection();
			this.m_type = type;
			m_system = OrbitalSystem.NONE;
			m_orbit = OrbitalPath.NONE;
		}

		public BuckRogers.OrbitalPath Orbit
		{
			get { return this.m_orbit; }
			set { this.m_orbit = value; }
		}

		
	}



	
}
