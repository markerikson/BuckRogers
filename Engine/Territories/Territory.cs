using System;
using skmDataStructures.Graph;

namespace BuckRogers
{
	public enum TerritoryType
	{
		Space,
		Ground,
	}

	public delegate void TerritoryOwnerChangedHandler(object sender, TerritoryEventArgs tea);

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
			get { return this.m_owner; }
			set 
			{ 
				if(m_owner != null)
				{
					m_owner.Territories.Remove(this.Name);
				}
				m_owner = value; 
				m_owner.Territories[this.Name] = this;

				/*
				if(TerritoryOwnerChanged != null)
				{
					TerritoryEventArgs tea = new TerritoryEventArgs();
					tea.Name = this.Name;
					tea.Owner = m_owner;
				}
				*/
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
		private Player m_owner;
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
