using System;
using System.Collections;
using skmDataStructures.Graph;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for Asteroid.
	/// </summary>
	public class Asteroid : OrbitalSystem
	{
		public Asteroid(string name, Graph graph, OrbitalPath op, int startingOrbitIndex, 
			string[] groundNames, string[] spaceNames, 
			Territory[] groundVertices, Territory[] spaceVertices)
			: base(name, graph, op, startingOrbitIndex, groundNames, spaceNames, groundVertices, spaceVertices)
		{
			if(spaceNames.Length > 1)
			{
				for(int i = 0; i < spaceNames.Length; i++)
				{
					// ugly hack, but whatever
					if(spaceNames[i].EndsWith("Orbit"))
					{
						this.NearOrbit = spaceVertices[i];
						break;
					}
				}
			}	
		}
	}
}
