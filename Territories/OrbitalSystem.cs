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
		public static OrbitalSystem NONE = new OrbitalSystem("NONE", null, null, 0, null, null, null, null);
		private OrbitalPath orbitalPath;
		private Hashtable ground;
		private Hashtable space;
		private int currentOrbitIndex;
		private bool m_HasKillerSatellite;
		
		private string m_name;
		
		private Territory farOrbit;

		/// <summary>
		/// Property FarOrbit; (Node)
		/// </summary>
		public Territory FarOrbit
		{
			get
			{
				return this.farOrbit;
			}
			set
			{
				this.farOrbit = value;
			}
		}	

		public bool HasKillerSatellite
		{
			get { return this.m_HasKillerSatellite; }
			set { this.m_HasKillerSatellite = value; }
		}

		
		/// <summary>
		/// Property Nodes (Hashtable)
		/// </summary>
		public Hashtable Ground
		{
			get
			{
				return this.ground;
			}
			set
			{
				this.ground = value;
			}
		}		

		public Hashtable Space
		{
			get
			{
				return this.space;
			}
			set
			{
				this.space = value;
			}
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

		public OrbitalSystem(string name, Graph graph, OrbitalPath op, int startingOrbitIndex, 
							string[] groundNames, string[] spaceNames, 
							Territory[] groundVertices, Territory[] spaceVertices)
		{
			if(name == "NONE")
			{
				return;
			}
			m_name = name;
			ground = new Hashtable();
			space = new Hashtable();
			this.OrbitalPath = op;

			for(int i = 0; i < groundNames.Length; i++)
			{
				Territory newNode = new Territory(groundNames[i], TerritoryType.Ground);
				newNode.System = this;
				ground.Add(groundNames[i], newNode);
				graph.AddNode(newNode);
				groundVertices[i] = newNode;
			}

			for(int i = 0; i < spaceNames.Length; i++)
			{
				Territory newNode = new Territory(spaceNames[i], TerritoryType.Space);
				newNode.System = this;
				space.Add(spaceNames[i], newNode);
				graph.AddNode(newNode);
				spaceVertices[i] = newNode;
			}

			currentOrbitIndex = startingOrbitIndex;


		}

		
	}
}
