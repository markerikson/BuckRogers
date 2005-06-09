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
		public Hashlist OrbitalPaths
		{
			get { return this.orbitalPaths; }
			set { this.orbitalPaths = value; }
		}

		public Hashlist Planets
		{
			get { return this.m_planets; }
			set { this.m_planets = value; }
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

		private Hashlist m_planets;
		private Hashlist orbitalPaths;

		
		public GameMap()
		{
			meo = new OrbitalPath("Mercury Orbit", 2);
			vo = new OrbitalPath("Venus Orbit", 4);
			eo = new OrbitalPath("Earth Orbit", 8);
			mao = new OrbitalPath("Mars Orbit", 16);
			ao = new OrbitalPath("Asteroid Orbit", 32);
			teo = new OrbitalPath("Trans-Earth Orbit", 16);
			tmo = new OrbitalPath("Trans-Mars Orbit", 32);

			m_planets = new Hashlist();
			orbitalPaths = new Hashlist();
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
			m_planets.Add(mercury.Name, mercury);
			meo.Planets.Add(mercury);

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
			//m_graph.AddUndirectedEdge(mnsv[0], mngv[4]);
			//m_graph.AddUndirectedEdge(mnsv[0], mngv[5]);
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
			m_planets.Add(venus.Name, venus);
			vo.Planets.Add(venus);

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
			m_planets.Add(earth.Name, earth);
			m_planets.Add(moon.Name, moon);
			eo.Planets.Add(earth);
			
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
			m_planets.Add(mars.Name, mars);
			mao.Planets.Add(mars);

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
			string[] asteroidNames = new string[]{"Ceres", "Aurora", "Hygeia", "Juno", 
													 "Vesta", "Fortuna", "Thule", 
													 "Psyche", "Pallas"};

			int[] asteroidLocations = {0, 2, 5, 11, 13, 16, 19, 24, 28};
			Territory[] asteroidOrbits = new Territory[asteroidNames.Length];

			for(int i = 0; i < asteroidNames.Length; i++)
			{
				string[] ang = new string[]{asteroidNames[i]};
				string[] ans = new string[]{asteroidNames[i] + " Orbit"};
				Territory[] angv = new Territory[ang.Length];
				Territory[] ansv = new Territory[ans.Length];
				Asteroid asteroid = new Asteroid(//ans[0], 
												asteroidNames[i],
												m_graph, ao, asteroidLocations[i], ang, ans, angv, ansv);	
				asteroidOrbits[i] = ansv[0];
				asteroid.NearOrbit = ansv[0];
				m_graph.AddUndirectedEdge(angv[0], ansv[0]);
				m_planets.Add(asteroidNames[i], asteroid);
				ao.Planets.Add(asteroid);
			}
			
			#endregion

			InitOrbit(meo, "Mercury", true);
			InitOrbit(vo, "Venus", true);
			InitOrbit(eo, "Earth", true);
			InitOrbit(mao, "Mars", true);
			InitOrbit(teo, "Trans-Earth", false);
			InitOrbit(tmo, "Trans-Mars", false);
			InitOrbit(ao, "Asteroid", true);

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

			for(int i = 0; i < asteroidOrbits.Length; i++)
			{
				m_graph.AddUndirectedEdge(asteroidOrbits[i], ao[asteroidLocations[i]]);
			}
			
			#endregion


			Territory blackMarket = new Territory("Black Market", TerritoryType.Ground);
			m_graph.AddNode(blackMarket);

			m_graph.AddNode(Territory.NONE);
			
			foreach(OrbitalSystem os in m_planets)
			{
				os.CalculateSurfaceAreas();
			}

			string[] satelliteNames = {"Mariposas", "Hielo", "L-4 Colony", "L-5 Colony", "Deimos"};

			for(int i = 0; i < satelliteNames.Length; i++)
			{
				Territory t = this[satelliteNames[i]];
				t.IsSatellite = true;
			}
		}

		private void InitOrbit(OrbitalPath orbit, string orbitName, bool connectOrbit)
		{
			for(int i = 0; i < orbit.Length; i++)
			{
				orbit[i] = new Territory(orbitName + " Orbit: " + i.ToString(), TerritoryType.Space);
				orbit[i].Orbit = orbit;
				orbit[i].IsSolarTerritory = true;
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

		public string GetPlanetTag(string orbitName, int orbitIndex)
		{
			OrbitalPath op = (OrbitalPath)orbitalPaths[orbitName];

			string tag = null;

			for(int i = 0; i < op.Planets.Count; i++)
			{
				OrbitalSystem os = (OrbitalSystem)op.Planets[i];
				if(os.CurrentOrbitIndex == orbitIndex)
				{
					tag = os.Name;
				}
			}

			return tag;

		}

		public void AdvancePlanets()
		{
			foreach(string s in m_planets.Keys)
			{
				if(s == "Moon")
				{
					continue;
				}

				OrbitalSystem os = (OrbitalSystem)m_planets[s];
				OrbitalPath op = os.OrbitalPath;
				Node currentSpaceLocation = op[os.CurrentOrbitIndex];

				Territory outerOrbit;
				if(os is Planet)
				{
					outerOrbit = ((Planet)os).FarOrbit;
				}
				else if(os is Asteroid)
				{
					outerOrbit = os.NearOrbit;
				}
				else
				{
					throw new Exception("Unknown OrbitalSystem type in AdvancePlanets()");
				}
				m_graph.RemoveUndirectedEdge(currentSpaceLocation, outerOrbit);
				int newIndex = op.NextOrbitalNodeIndex(os.CurrentOrbitIndex);
				Node newSpaceLocation = op[newIndex];
				m_graph.AddUndirectedEdge(newSpaceLocation, outerOrbit);
				os.CurrentOrbitIndex = newIndex;
			}

		}
	}
}
