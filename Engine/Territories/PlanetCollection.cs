using System;
using System.Collections;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for PlanetCollection.
	/// </summary>
	public class PlanetCollection
	{
		private Hashtable planets;

		public PlanetCollection()
		{
			planets = new Hashtable();
			//
			// TODO: Add constructor logic here
			//
		}

		public void Add(String s, Planet p)
		{
			planets.Add(s, p);
		}

		public Planet this[string planetName]
		{
			get
			{
				return (Planet)planets[planetName];
			}
			//return ((Planet)(planets.Pop()));
		}

		public bool Contains(Planet Planet)
		{
			return planets.Contains(Planet);
		}

		public int Count
		{
			get { return planets.Count; }
		}

		public IEnumerator GetEnumerator()
		{
			return planets.GetEnumerator();
		}
	}
}
