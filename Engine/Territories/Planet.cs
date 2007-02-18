using System;
using System.Collections;
using skmDataStructures.Graph;
using System.Diagnostics;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for Planet.
	/// </summary>
	[DebuggerDisplay("Planet: {Name}")]
	public class Planet : OrbitalSystem
	{
		//private string[] names;

		private Territory m_farOrbit;
		

		

		/// <summary>
		/// Property NearOrbit (Node)
		/// </summary>
		public Territory FarOrbit
		{
			get
			{
				return this.m_farOrbit;
			}
			set
			{
				this.m_farOrbit = value;
			}
		}
		/*
		/// <summary>
		/// Property Names (string[])
		/// </summary>
		public string[] Names
		{
			get
			{
				
				return this.names;
			}
			set
			{
				this.names = value;
			}
		}
		*/

		

			
		
		public Planet(string name, Graph graph, OrbitalPath op, int startingOrbitIndex, 
						string[] groundNames, string[] spaceNames, 
						Territory[] groundVertices, Territory[] spaceVertices)
			: base(name, graph, op, startingOrbitIndex, groundNames, spaceNames, groundVertices, spaceVertices)
		{
			if(spaceNames.Length > 1)
			{
				for(int i = 0; i < spaceNames.Length; i++)
				{
					// ugly hack, but whatever
					if(spaceNames[i].StartsWith("Far "))
					{
						this.FarOrbit = spaceVertices[i];
						break;
					}
				}
			}	
		}
	}
}
