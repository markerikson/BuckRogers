using System;
using System.Collections;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for Player.
	/// </summary>
	/// 
	
	

	public class Player
	{
		public Hashtable Territories
		{
			get { return this.m_territories; }
			set { this.m_territories = value; }
		}
	
		public string Name
		{
			get { return this.m_name; }
			set { this.m_name = value; }
		}
	
		public BuckRogers.UnitCollection Units
		{
			get { return this.m_units; }
			set { this.m_units = value; }
		}

		public static Player NONE = new Player("None");
	
		private UnitCollection m_units;
		private string m_name;
		private Hashtable m_territories;

		public Player(string name)
		{
			//
			// TODO: Add constructor logic here
			//

			m_name = name;
			m_units = new UnitCollection();
			m_territories = new Hashtable();
		}
	}
}
