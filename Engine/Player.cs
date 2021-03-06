using System;
using System.Collections;
using System.Drawing;
using System.Diagnostics;

namespace BuckRogers
{
	public enum PlayerType
	{
		Human,
		AI,
	}

	public enum PlayerLocation
	{
		Local,
		Remote,
	}



	[DebuggerDisplay("Player: {m_name}")]
	public class Player
	{
		public static Player NONE = new Player("None");
	
		private UnitCollection m_units;
		private string m_name;
		private Hashtable m_territories;
		private bool m_disabled;
		private Color m_color;
		private int m_turnDisabled;
		private PlayerType m_type;
		private int m_id;
		private PlayerLocation m_location;

		
		public int ID
		{
			get { return m_id; }
			set { m_id = value; }
		}

		public PlayerType Type
		{
			get { return m_type; }
			set { m_type = value; }
		}

		public PlayerLocation Location
		{
			get { return m_location; }
			set { m_location = value; }
		}

		public bool IsLocal
		{
			get
			{
				return m_location == PlayerLocation.Local;
			}
		}

		public Player(string name, Color color)
		{
			Init(name, color);
		}

		public Player(string name)
		{
			Init(name, Color.White);
		}

		private void Init(string name, Color color)
		{
			m_name = name;
			m_units = new UnitCollection();
			m_territories = new Hashtable();
			m_disabled = false;
			m_color = color;
			m_type = PlayerType.Human;
			m_location = PlayerLocation.Local;
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
