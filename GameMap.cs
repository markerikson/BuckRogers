using System;
using System.Collections;
using skmDataStructures.Graph;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for GameMap.
	/// </summary>
	public class GameMap
	{
		public System.Collections.Hashtable OrbitalPaths
		{
			get { return this.orbitalPaths; }
			set { this.orbitalPaths = value; }
		}

		public System.Collections.Hashtable Planets
		{
			get { return this.planets; }
			set { this.planets = value; }
		}

		public skmDataStructures.Graph.Graph Graph
		{
			get { return this.m_graph; }
			set { this.m_graph = value; }
		}

		public Territory this[string name]
		{
			get
			{
				return (Territory)m_graph.Nodes[name];
			}

		}
	
		private OrbitalPath eo;
		private OrbitalPath mao;
		private OrbitalPath vo;
		private OrbitalPath meo;
		private OrbitalPath ao;
		private OrbitalPath teo;
		private OrbitalPath tmo;

		public Graph m_graph;

		private Hashtable planets;
		private Hashtable orbitalPaths;

		
		public GameMap()
		{
			//
			// TODO: Add constructor logic here
			//
			meo = new OrbitalPath("Mercury Orbit", 2);
			vo = new OrbitalPath("Venus Orbit", 4);
			eo = new OrbitalPath("Earth Orbit", 8);
			mao = new OrbitalPath("Mars Orbit", 16);
			ao = new OrbitalPath("Asteroid Orbit", 32);
			teo = new OrbitalPath("Trans-Earth Orbit", 16);
			tmo = new OrbitalPath("Trans-Mars Orbit", 32);

			planets = new Hashtable();
			orbitalPaths = new Hashtable();
			orbitalPaths.Add("Mercury Orbit", meo);
			orbitalPaths.Add("Venus Orbit", vo);
			orbitalPaths.Add("Earth Orbit", eo);
			orbitalPaths.Add("Mars Orbit", mao);
			orbitalPaths.Add("Trans-Earth Orbit", teo);
			orbitalPaths.Add("Trans-Mars Orbit", tmo);
			orbitalPaths.Add("Asteroid Orbit", ao);

			m_graph = new Graph();

			InitMap();
			
		}

		private void InitMap()
		{
			#region Earth/Moon creation
			string[] eng = new string[]{"Independent Arcologies", 
										   "American Regency", 
										   "Eurasian Regency",
										   "Urban Reservations", 
										   "African Regency", 
										   "Australian Development Facility", 
										   "Antarctic Testing Zone",
										   "L-4 Colony",
										   "L-5 Colony"};

			string[] ens = new String[]{"Near Earth Orbit",
										   "Far Earth/Moon Orbit"
										   };

			string[] mong = new string[]{"Moscoviense",
											"Tranquility",
											"Farside",
											"Tycho"};
			string[] mons = new string[]{"Near Moon Orbit"};

			Territory[] engv = new Territory[eng.Length];
			Territory[] ensv = new Territory[ens.Length];
			Territory[] mongv = new Territory[mong.Length];
			Territory[] monsv = new Territory[mons.Length];

			
			Planet earth = new Planet("Earth", m_graph, eo, 0, eng, ens, engv, ensv);
			Planet moon = new Planet("Moon", m_graph, eo, 0, mong, mons, mongv, monsv);
			planets.Add(earth.Name, earth);
			planets.Add(moon.Name, moon);
			
			// connect the Earth territories
			
			m_graph.AddUndirectedEdge(engv[0], engv[1]);
			m_graph.AddUndirectedEdge(engv[0], engv[2]);
			m_graph.AddUndirectedEdge(engv[1], engv[2]);
			m_graph.AddUndirectedEdge(engv[1], engv[3]);
			m_graph.AddUndirectedEdge(engv[2], engv[3]);
			m_graph.AddUndirectedEdge(engv[2], engv[4]);
			m_graph.AddUndirectedEdge(engv[3], engv[4]);
			m_graph.AddUndirectedEdge(engv[3], engv[5]);
			m_graph.AddUndirectedEdge(engv[3], engv[6]);
			m_graph.AddUndirectedEdge(engv[4], engv[6]);
			m_graph.AddUndirectedEdge(engv[5], engv[6]);

			// connect the Moon territories
			m_graph.AddUndirectedEdge(mongv[0], mongv[1]);
			m_graph.AddUndirectedEdge(mongv[0], mongv[2]);
			m_graph.AddUndirectedEdge(mongv[1], mongv[2]);
			m_graph.AddUndirectedEdge(mongv[1], mongv[3]);
			m_graph.AddUndirectedEdge(mongv[2], mongv[3]);

			// connect the Earth territories to Near Earth Orbit
			for(int i = 0; i <= 6; i++)
			{
				m_graph.AddUndirectedEdge(engv[i], ensv[0]);
			}

			// connect the Moon territories to Near Moon Orbit
			for(int i = 0; i < mongv.Length; i++)
			{
				m_graph.AddUndirectedEdge(mongv[i], monsv[0]);
			}

			// Connect the Earth/Moon orbits and the colonies
			m_graph.AddUndirectedEdge(ensv[0], ensv[1]);
			m_graph.AddUndirectedEdge(ensv[0], monsv[0]);			
			m_graph.AddUndirectedEdge(monsv[0], ensv[1]);
			m_graph.AddUndirectedEdge(ensv[1], engv[7]);
			m_graph.AddUndirectedEdge(ensv[1], engv[8]);
			
			#endregion
			
			#region Mercury creation
			string[] mng = new string[]{"The Warrens", 
										   "Bach", 
										   "Tolstoi", 
										   "Sobkou Plains",
										   "Mariposas",
										   "Hielo"};

			string[] mns = new string[]{"Near Mercury Orbit",
										   "Far Mercury Orbit"};

			Territory[] mngv = new Territory[mng.Length];
			Territory[] mnsv = new Territory[mns.Length];

			Planet mercury = new Planet("Mercury", m_graph, meo, 0, mng, mns, mngv, mnsv);
			planets.Add(mercury.Name, mercury);

			m_graph.AddUndirectedEdge(mngv[0], mngv[1]);
			m_graph.AddUndirectedEdge(mngv[0], mngv[2]);
			m_graph.AddUndirectedEdge(mngv[1], mngv[2]);
			m_graph.AddUndirectedEdge(mngv[1], mngv[3]);
			m_graph.AddUndirectedEdge(mngv[2], mngv[3]);

			for(int i = 0; i < mngv.Length; i++)
			{
				m_graph.AddUndirectedEdge(mngv[i], mnsv[0]);
			}

			m_graph.AddUndirectedEdge(mnsv[0], mnsv[1]);
			m_graph.AddUndirectedEdge(mnsv[0], mngv[4]);
			m_graph.AddUndirectedEdge(mnsv[0], mngv[5]);
			#endregion
			
			#region Venus creation
			string[] vng = new string[]{"Aerostates", 
										   "Mt. Maxwell", 
										   "Lowlanders",
										   "Elysium",
										   "Beta Regio",
										   "Aphrodite", 
										   "Wreckers"};

			string[] vns = new string[]{"Near Venus Orbit", "Far Venus Orbit"};

			Territory[] vngv = new Territory[vng.Length];
			Territory[] vnsv = new Territory[vns.Length];
		
		
			Planet venus = new Planet("Venus", m_graph, vo, 2, vng, vns, vngv, vnsv);
			planets.Add(venus.Name, venus);

			m_graph.AddUndirectedEdge(vngv[0], vngv[1]);
			m_graph.AddUndirectedEdge(vngv[0], vngv[2]);
			m_graph.AddUndirectedEdge(vngv[1], vngv[2]);
			m_graph.AddUndirectedEdge(vngv[1], vngv[3]);
			m_graph.AddUndirectedEdge(vngv[2], vngv[3]);
			m_graph.AddUndirectedEdge(vngv[2], vngv[4]);
			m_graph.AddUndirectedEdge(vngv[2], vngv[5]);
			m_graph.AddUndirectedEdge(vngv[3], vngv[5]);
			m_graph.AddUndirectedEdge(vngv[3], vngv[6]);
			m_graph.AddUndirectedEdge(vngv[4], vngv[5]);
			m_graph.AddUndirectedEdge(vngv[5], vngv[6]);

			for(int i = 0; i < vngv.Length; i++)
			{
				m_graph.AddUndirectedEdge(vngv[i], vnsv[0]);
			}

			m_graph.AddUndirectedEdge(vnsv[0], vnsv[1]);

			#endregion
			
			#region Mars creation
			string[] mang = new string[]{"Boreal Sea",
											"Coprates Chasm", 
											"Ram HQ",
											"Arcadia", 
											"Pavonis", 
											"Space Elevator",
											"Deimos"};

			string[] mans = new string[]{"Near Mars Orbit", 
											"Far Mars Orbit"};

			Territory[] mangv = new Territory[mang.Length];
			Territory[] mansv = new Territory[mans.Length];

			Planet mars = new Planet("Mars", m_graph, mao, 9, mang, mans, mangv, mansv);
			planets.Add(mars.Name, mars);

			m_graph.AddUndirectedEdge(mangv[0], mangv[1]);
			m_graph.AddUndirectedEdge(mangv[0], mangv[2]);
			m_graph.AddUndirectedEdge(mangv[0], mangv[3]);
			m_graph.AddUndirectedEdge(mangv[0], mangv[4]);
			m_graph.AddUndirectedEdge(mangv[1], mangv[2]);
			m_graph.AddUndirectedEdge(mangv[2], mangv[3]);
			m_graph.AddUndirectedEdge(mangv[3], mangv[4]);
			m_graph.AddUndirectedEdge(mangv[4], mangv[5]);

			// don't include Deimos in the near orbit connections
			for(int i = 0; i < mangv.Length - 1; i++)
			{
				m_graph.AddUndirectedEdge(mangv[i], mansv[0]);
			}

			m_graph.AddUndirectedEdge(mangv[5], mansv[1]);
			m_graph.AddUndirectedEdge(mansv[0], mansv[1]);
			m_graph.AddUndirectedEdge(mansv[1], mangv[6]);
			
			#endregion
			
			#region Asteroid creation
			string[] asteroidNames = new string[]{"Aurora", "Hygeia", "Juno", 
													 "Vesta", "Fortuna", "Thule", 
													 "Psyche", "Pallas", "Ceres"};

			int[] asteroidLocations = {2, 5, 11, 13, 16, 19, 24, 28, 0};
			Territory[] asteroidOrbits = new Territory[asteroidNames.Length];

			for(int i = 0; i < asteroidNames.Length; i++)
			{
				string[] ang = new string[]{asteroidNames[i]};
				string[] ans = new string[]{asteroidNames[i] + " Orbit"};
				Territory[] angv = new Territory[ang.Length];
				Territory[] ansv = new Territory[ans.Length];
				Asteroid asteroid = new Asteroid(ans[0], m_graph, ao, asteroidLocations[i], ang, ans, angv, ansv);	
				asteroidOrbits[i] = ansv[0];
				asteroid.FarOrbit = ansv[0];
				m_graph.AddUndirectedEdge(angv[0], ansv[0]);
				planets.Add(asteroidNames[i], asteroid);
			}
			
			#endregion

			InitOrbit(meo, "Mercury", true);
			InitOrbit(vo, "Venus", true);
			InitOrbit(eo, "Earth", true);
			InitOrbit(mao, "Mars", true);
			InitOrbit(ao, "Asteroid", true);
			InitOrbit(teo, "Trans-Earth", false);
			InitOrbit(tmo, "Trans-Mars", false);

			#region Inter-Orbit links

			// Mercury - Venus orbit links
			m_graph.AddUndirectedEdge(meo[0], vo[0]);
			m_graph.AddUndirectedEdge(meo[0], vo[3]);
			m_graph.AddUndirectedEdge(meo[1], vo[1]);
			m_graph.AddUndirectedEdge(meo[1], vo[2]);

			// Venus - Earth orbit links
			m_graph.AddUndirectedEdge(vo[0], eo[1]);
			m_graph.AddUndirectedEdge(vo[0], eo[2]);
			m_graph.AddUndirectedEdge(vo[1], eo[3]);
			m_graph.AddUndirectedEdge(vo[1], eo[4]);
			m_graph.AddUndirectedEdge(vo[2], eo[5]);
			m_graph.AddUndirectedEdge(vo[2], eo[6]);
			m_graph.AddUndirectedEdge(vo[3], eo[7]);
			m_graph.AddUndirectedEdge(vo[3], eo[0]);

			// Earth - Trans-Earth orbit links
			int eoIdx = 0;
			for(int i = 0; i < 16; i++)
			{
				m_graph.AddUndirectedEdge(eo[eoIdx], teo[i]);
				if(i % 2 == 1)
				{
					eoIdx++;
				}
			}

			// Trans-Earth - Mars orbit links
			for(int i = 0; i < 16; i++)
			{
				m_graph.AddUndirectedEdge(teo[i], mao[i]);
			}


			// Mars - Trans-Mars orbit links

			int maoIdx = 0;
			for(int i = 0; i < 32; i++)
			{
				m_graph.AddUndirectedEdge(mao[maoIdx], tmo[i]);
				if(i % 2 == 1)
				{
					maoIdx++;
				}
			}

			// Trans-Mars - Asteroid orbit links
			for(int i = 0; i < 32; i++)
			{
				m_graph.AddUndirectedEdge(tmo[i], ao[i]);
			}

			
			// Add far orbit - space links
			m_graph.AddUndirectedEdge(mercury.FarOrbit, meo[0]);
			m_graph.AddUndirectedEdge(venus.FarOrbit, vo[2]);
			m_graph.AddUndirectedEdge(earth.FarOrbit, eo[0]);
			m_graph.AddUndirectedEdge(mars.FarOrbit, mao[9]);

			m_graph.AddUndirectedEdge(asteroidOrbits[0], ao[2]);
			m_graph.AddUndirectedEdge(asteroidOrbits[1], ao[5]);
			m_graph.AddUndirectedEdge(asteroidOrbits[2], ao[11]);
			m_graph.AddUndirectedEdge(asteroidOrbits[3], ao[13]);
			m_graph.AddUndirectedEdge(asteroidOrbits[4], ao[16]);
			m_graph.AddUndirectedEdge(asteroidOrbits[5], ao[19]);
			m_graph.AddUndirectedEdge(asteroidOrbits[6], ao[24]);
			m_graph.AddUndirectedEdge(asteroidOrbits[7], ao[28]);
			m_graph.AddUndirectedEdge(asteroidOrbits[8], ao[0]);
			
			#endregion

		}

		private void InitOrbit(OrbitalPath orbit, string orbitName, bool connectOrbit)
		{
			for(int i = 0; i < orbit.Length; i++)
			{
				orbit[i] = new Territory(orbitName + " Orbit: " + i.ToString(), TerritoryType.Space);
				orbit[i].Orbit = orbit;
				m_graph.AddNode(orbit[i]);
			}

			if(connectOrbit)
			{
				for(int i = 0; i < orbit.Length - 1; i++)
				{
					m_graph.AddUndirectedEdge(orbit[i], orbit[i + 1]);
				}

				m_graph.AddUndirectedEdge(orbit[orbit.Length - 1], orbit[0]);
			}
			

		}

		public void AdvancePlanets()
		{
			foreach(string s in planets.Keys)
			{
				if(s == "Moon")
				{
					continue;
				}

				OrbitalSystem os = (OrbitalSystem)planets[s];
				OrbitalPath op = os.OrbitalPath;
				Node currentSpaceLocation = op[os.CurrentOrbitIndex];
				m_graph.RemoveUndirectedEdge(currentSpaceLocation, os.FarOrbit);
				int newIndex = op.NextOrbitalNodeIndex(os.CurrentOrbitIndex);
				Node newSpaceLocation = op[newIndex];
				m_graph.AddUndirectedEdge(newSpaceLocation, os.FarOrbit);
				os.CurrentOrbitIndex = newIndex;
			}

		}
	}
}
