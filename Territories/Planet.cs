using System;
using System.Collections;
using skmDataStructures.Graph;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for Planet.
	/// </summary>
	public class Planet : OrbitalSystem
	{
		//private string[] names;

		private Territory nearOrbit;
		

		

		/// <summary>
		/// Property NearOrbit (Node)
		/// </summary>
		public Territory NearOrbit
		{
			get
			{
				return this.nearOrbit;
			}
			set
			{
				this.nearOrbit = value;
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
