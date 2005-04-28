using System;
using System.Collections;
using System.Windows.Forms;
using skmDataStructures.Graph;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for OrbitalSystem.
	/// </summary>
	public class OrbitalSystem
	{
		
	
		public static OrbitalSystem NONE = new OrbitalSystem("None", null, null, 0, null, null, null, null);
		private OrbitalPath orbitalPath;
		private Hashtable m_ground;
		private Hashtable m_space;
		private int currentOrbitIndex;
		private bool m_HasKillerSatellite;
		private Hashlist m_surface;
		private Player m_owner;
		
		private string m_name;
		
		private Territory m_nearOrbit;

		/// <summary>
		/// Property FarOrbit; (Node)
		/// </summary>
		public Territory NearOrbit
		{
			get
			{
				return this.m_nearOrbit;
			}
			set
			{
				this.m_nearOrbit = value;
			}
		}	

		public bool HasKillerSatellite
		{
			get 
			{ 
				//return this.m_HasKillerSatellite; 
				return (this.NearOrbit.Units.GetUnits(UnitType.KillerSatellite).Count == 1);
			}
			set { this.m_HasKillerSatellite = value; }
		}

		
		/// <summary>
		/// Property Nodes (Hashtable)
		/// </summary>
		public Hashtable Ground
		{
			get
			{
				return this.m_ground;
			}
			set
			{
				this.m_ground = value;
			}
		}		

		public Hashtable Space
		{
			get
			{
				return this.m_space;
			}
			set
			{
				this.m_space = value;
			}
		}

		public BuckRogers.Hashlist Surface
		{
			get { return this.m_surface; }
			set { this.m_surface = value; }
		}

		public BuckRogers.OrbitalPath OrbitalPath
		{
			get { return this.orbitalPath; }
			set { this.orbitalPath = value; }
		}

		public int CurrentOrbitIndex
		{
			get { return this.currentOrbitIndex; }
			set { this.currentOrbitIndex = value; }
		}

		public string Name
		{
			get { return this.m_name; }
			set { this.m_name = value; }
		}	

		public bool IsControlled
		{
			get
			{
				return CheckControl();
			}
		}

		public BuckRogers.Player Owner
		{
			get 
			{ 
				CheckControl();
				return this.m_owner; 
			}
			set { this.m_owner = value; }
		}

		public OrbitalSystem(string name, Graph graph, OrbitalPath op, int startingOrbitIndex, 
							string[] groundNames, string[] spaceNames, 
							Territory[] groundVertices, Territory[] spaceVertices)
		{
			if(name == "None")
			{
				return;
			}
			m_name = name;
			m_ground = new Hashtable();
			m_space = new Hashtable();
			m_surface = new Hashlist();
			this.Owner = Player.NONE;
			this.OrbitalPath = op;

			for(int i = 0; i < groundNames.Length; i++)
			{
				string territoryName = groundNames[i];
				Territory newNode = new Territory(territoryName, TerritoryType.Ground);
				newNode.System = this;
				m_ground.Add(territoryName, newNode);
				graph.AddNode(newNode);
				groundVertices[i] = newNode;
			}

			for(int i = 0; i < spaceNames.Length; i++)
			{
				string territoryName = spaceNames[i];
				Territory newNode = new Territory(territoryName, TerritoryType.Space);
				newNode.System = this;
				m_space.Add(territoryName, newNode);
				graph.AddNode(newNode);
				spaceVertices[i] = newNode;
				if(territoryName.StartsWith("Near "))
				{
					this.NearOrbit = newNode;
				}
			}

			currentOrbitIndex = startingOrbitIndex;


		}

		public void CalculateSurfaceAreas()
		{
			foreach(Territory t in m_ground.Values)
			//foreach(object o in m_ground.Values)
			{
				//object b = o;
				
				if(t.Neighbors.Count > 1)
				{
					m_surface.Add(t.Name, t);
				}
				
				
			}
		}	

		public virtual bool CheckControl()
		{
			// TODO Implement partial planetary control
			bool controlled = true;

			Player first = ((Territory)m_surface[0]).Owner;

			foreach(Territory t in m_surface)
			{
				// skip over moons and satellites
				if(t.Neighbors.Count == 1)
				{
					continue;
				}
				if(t.Owner != first)
				{
					controlled = false;
				}
			}

			if(controlled)
			{
				this.Owner = first;
			}

			return controlled;
		}
	}
}
