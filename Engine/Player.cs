using System;
using System.Collections;
using System.Drawing;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for Player.
	/// </summary>
	/// 

	public class Player
	{
		public static Player NONE = new Player("None");
	
		private UnitCollection m_units;
		private string m_name;
		private Hashtable m_territories;
		private bool m_disabled;
		private Color m_color;
		private int m_turnDisabled;

		public Player(string name)
		{
			m_name = name;
			m_units = new UnitCollection();
			m_territories = new Hashtable();
			m_disabled = false;
		}

		public bool Disabled
		{
			get { return this.m_disabled; }
			set { this.m_disabled = value; }
		}

		public int TurnDisabled
		{
			get { return this.m_turnDisabled; }
			set { this.m_turnDisabled = value; }
		}

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

		public Color Color
		{
			get { return this.m_color; }
			set { this.m_color = value; }
		}
	}
}
