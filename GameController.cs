using System;
using System.Collections;
using CenterSpace.Free;
using skmDataStructures.Graph;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for GameController.
	/// </summary>
	public class GameController
	{
		
		
		#region Properties
		public CenterSpace.Free.MersenneTwister Twister
		{
			get { return this.m_twister; }
			set { this.m_twister = value; }
		}

		public BuckRogers.GameMap Map
		{
			get { return this.m_map; }
			set { this.m_map = value; }
		}
	
		public BuckRogers.Player[] Players
		{
			get { return this.m_players; }
			set { this.m_players = value; }
		}
	
		public int Number
		{
			get { return this.m_turnNumber; }
			set { this.m_turnNumber = value; }
		}

		public System.Collections.ArrayList PlayerOrder
		{
			get { return this.m_currentPlayerOrder; }
			set { this.m_currentPlayerOrder = value; }
		}

		public BuckRogers.Hashlist Battles
		{
			get { return this.m_battles; }
			set { this.m_battles = value; }
		}

		#endregion
	
		private GameMap m_map;
		// Player list assumes players in clockwise order
		private Player[] m_players;
		private ArrayList m_currentPlayerOrder;
		public ArrayList m_rollResults;
		private MersenneTwister m_twister;
		private ArrayList m_rollList;
		private ArrayList m_checkedActions;
		private ArrayList m_undoneActions;
		private int m_turnNumber;
		private Hashlist m_battles;
		private int[,] m_combatTable;
		private const int NOTPOSSIBLE = 99;

		// TODO Change this into a property or something		
		public TurnRoll[] Rolls;


		

		public void PlayGame()
		{
			string[] players = {"Mark", "Chris"};
			
			//GameController gc = new GameController(players);

			this.AssignTerritories();
			this.CreateInitialUnits();

			
			this.RollForInitiative(false);

			//bool playersHaveUnits = true;

			// 18 units, distributed 3 at a time
			for(int j = 0; j < 6; j++)
			{
				foreach(Player p in this.PlayerOrder)
				{
					ICollection ic = p.Territories.Keys;
					Territory terr = Territory.NONE;


					//for(int i = 0; i < ic.Count; i++)
					foreach(string s in ic)
					{
						terr = (Territory)p.Territories[s];
						if(terr.Units.Count < 6)
						{
							break;
						}
					}

					UnitCollection units = Territory.NONE.Units.GetUnits(p);
					Utility.RandomizeList(units);

					if(units.Count >= 3)
					{
						for(int i = 0; i < 3; i++)
						{
							Unit unit = (Unit)units[i];
							unit.CurrentTerritory = terr;
						}
					}
				}
			}			

			/*
			MoveAction move1 = new MoveAction();
			Territory t = this.Map["Elysium"];
			move1.Owner = t.Owner;
			UnitCollection u = new UnitCollection();
			*/
			
		}

		public GameController(string[] playerNames)
		{
			m_map = new GameMap();
			m_players = new Player[playerNames.Length];

			for(int i = 0; i < playerNames.Length; i++)
			{
				m_players[i] = new Player(playerNames[i]);
			}

			m_twister = new MersenneTwister();
			m_checkedActions = new ArrayList();
			m_undoneActions = new ArrayList();

			m_combatTable = new int[,]	{	{6, 8, 7, NOTPOSSIBLE, 6, NOTPOSSIBLE, 3}, // Trooper
											{5, 6, 6, NOTPOSSIBLE, 5, NOTPOSSIBLE, 2}, // Gennie
											{7, 7, 6, 8, 3, 7, 3}, // Fighter
											{7, 7, 4, 6, 4, 6, NOTPOSSIBLE}, // Battler
											{9, 10, 8, 10, 6, 10, 9}, // Transport
											{NOTPOSSIBLE, NOTPOSSIBLE, 6, 7, 5, NOTPOSSIBLE, NOTPOSSIBLE}, // Killer Satellite
											{8, 9, 9, NOTPOSSIBLE, 7, NOTPOSSIBLE, NOTPOSSIBLE}, // Control marker
										};

			m_turnNumber = 0;			
		}

		public Player GetPlayer(string name)
		{
			Player result = Player.NONE;

			foreach(Player p in m_players)
			{
				if(p.Name == name)
				{
					result = p;
					break;
				}
			}

			return result;
		}


		public int RollD10()
		{
			return m_twister.Next(1, 10);
		}

		#region Setup functions

		public void AssignTerritories()
		{

			ArrayList ground = new ArrayList();
			foreach(Territory t in m_map.Graph.Nodes)
			{
				if(t.Type == TerritoryType.Ground)
				{
					ground.Add(t);
				}
			}
			
			foreach(Player p in m_players)
			{
				// TODO Technically supposed to be 6 territories apiece
				for(int i = 0; i < 7; i++)
				{
					int idx = m_twister.Next(ground.Count - 1);
					Territory t = (Territory)ground[idx];
					t.Owner = p;
					ground.Remove(t);
				}

			}
		}

		public void CreateInitialUnits()
		{
			foreach(Player p in m_players)
			{
				Unit u = Unit.CreateNewUnit(p, UnitType.Leader);

				for(int i = 0; i < 8; i++)
				{
					u = Unit.CreateNewUnit(p, UnitType.Trooper);
				}

				for(int i = 0; i < 4; i++)
				{
					u = Unit.CreateNewUnit(p, UnitType.Fighter);
				}

				for(int i = 0; i < 2; i++)
				{
					u = Unit.CreateNewUnit(p, UnitType.Factory);
				}

				for(int i = 0; i < 2; i++)
				{
					u = Unit.CreateNewUnit(p, UnitType.Gennie);
				}

				u = Unit.CreateNewUnit(p, UnitType.Transport);
			}
		}

		#endregion

		#region Initiative functions

		public void RollForInitiative()
		{
			RollForInitiative(true);
		}

		public void RollForInitiative(bool checkRollParity)
		{
			m_currentPlayerOrder = new ArrayList();
			m_rollResults = new ArrayList();
			m_rollList = new ArrayList();
			
			ArrayList players = new ArrayList(m_players);
			RollAndCheckForTies(players);

			Rolls = (TurnRoll[])m_rollList.ToArray(typeof(TurnRoll));

			TurnRoll topRoll = (TurnRoll)m_rollResults[0];
			m_currentPlayerOrder.Add(topRoll.Player);
			int playerIndex = Array.IndexOf(m_players, topRoll.Player);

			if(checkRollParity && ( (topRoll.Roll % 2) == 1))
			{
				for(int i = playerIndex - 1; i >= 0; i--)
				{
					m_currentPlayerOrder.Add(m_players[i]);
				}

				for(int i = m_players.Length - 1; i > playerIndex; i--)
				{
					m_currentPlayerOrder.Add(m_players[i]);
				}
			}
			else
			{
				for(int i = playerIndex + 1; i < m_players.Length; i++)
				{
					m_currentPlayerOrder.Add(m_players[i]);
				}

				for(int i = 0; i < playerIndex; i++)
				{
					m_currentPlayerOrder.Add(m_players[i]);
				}
			}
		}

		private void RollAndCheckForTies(ArrayList players)
		{
			TurnRoll[] rolls = new TurnRoll[players.Count];
			for(int i = 0; i < players.Count; i++)
			{
				rolls[i] = new TurnRoll();
				rolls[i].Roll = RollD10();
				rolls[i].Player = (Player)players[i];				
			}

			Array.Sort(rolls);
			Array.Reverse(rolls);

			for(int i = 0; i < rolls.Length; i++)
			{
				m_rollList.Add(rolls[i]);
			}

			int numTopRolls = 1;

			for(int i = 1; i < rolls.Length; i++)
			{
				if(rolls[i].Roll == rolls[0].Roll)
				{
					numTopRolls++;
				}
				else
				{
					break;
				}
			}

			if(numTopRolls > 1)
			{
				for(int i = numTopRolls; i < rolls.Length; i++)
				{
					m_rollResults.Add(rolls[i]);
				}

				ArrayList morePlayers = new ArrayList();
				for(int i = 0; i < numTopRolls; i++)
				{
					morePlayers.Add(rolls[i].Player);
				}
				RollAndCheckForTies(morePlayers);
			}
			else
			{
				for(int i = 0; i < rolls.Length; i++)
				{
					m_rollResults.Insert(i, rolls[i]);
				}
			}
		}

		#endregion

		#region Transport functions

		//public void LoadTransport(Transport transport, UnitCollection units, UnitType type)
		public void LoadTransport(TransportAction ta)
		{
			Transport transport = ta.Transport;
			UnitCollection units = ta.Units;
			UnitType type = ta.UnitType;

			int numCurrent = transport.Transportees.Count;

			switch(type)
			{
				case UnitType.Trooper:
				{
					UnitCollection troopers = units.GetUnits(UnitType.Trooper);
					int numNew = troopers.Count;
					int numTotal = numCurrent + numNew;
					int numFactories = units.GetUnits(UnitType.Factory).Count;

					if(numCurrent > 5 || numTotal > 5 || numFactories > 0)
					{
						throw new ActionException("Can't load this many units on this transport");
					}

					foreach(Unit u in troopers)
					{
						u.Transported = true;
						u.TransportingUnit = transport;
						u.CurrentTerritory = Territory.NONE;
					}

					transport.Transportees.AddAllUnits(troopers);
					break;
				}
				case UnitType.Factory:
				{
					UnitCollection factories = units.GetUnits(UnitType.Factory);

					if(factories.Count > 1)
					{
						throw new ActionException("Can't load more than one factory onto a transport");
					}

					if(numCurrent > 0)
					{
						throw new ActionException("Can't load a factory onto a transport with units");
					}

					Factory factory = factories[0] as Factory;

					factory.CurrentTerritory = Territory.NONE;
					factory.Transported = true;
					factory.TransportingUnit = transport;

					transport.Transportees.AddUnit(factory);
					break;
				}
			}
			
		}

		//public void UnloadTransport(Transport tr, int max)
		public void UnloadTransport(TransportAction ta)
		{
			Transport tr = ta.Transport;
			int max = ta.MaxTransfer;
			Territory t = tr.CurrentTerritory;

			if(t.Type != TerritoryType.Ground)
			{
				throw new ActionException("Transports can only be unloaded in ground territories");
			}

			if(tr.Transportees.Count == 0)
			{
				throw new ActionException("Can't unload an empty transport");
			}

			int numToUnload = max;
			if(tr.Transportees.Count < numToUnload)
			{
				numToUnload = tr.Transportees.Count;
			}

			ta.Units = new UnitCollection();

			// we've got at least one unit, and they're guaranteed to be all the same type
			ta.UnitType = tr.Transportees[0].UnitType;

			for(int i = 0; i < numToUnload; i++)
			{
				// Transportees are automatically removed when the transporting unit is set to NONE,
				// so the index needs to stay at zero
				Unit u = tr.Transportees[0];
				u.TransportingUnit = Unit.NONE;

				// Removes the unit from the old territory, assigns the unit to the new territory, and 
				// adds the unit to the new territory's unit collection
				u.CurrentTerritory = t;

				// need to make sure we've got the right units for undoing the move
				ta.Units.AddUnit(u);
			}

		}

		#endregion 

		#region Action functions

		public void AddAction(Action action)
		{
			if(action is MoveAction)
			{
				MoveAction move = (MoveAction)action;

				bool alreadyInEnemyTerritory = false;
				Territory previousTerritory = move.StartingTerritory;

				for(int i = 0; i < move.Territories.Count; i++)
				{
					if(alreadyInEnemyTerritory)
					{
						throw new ActionException("Can't move units in and out of a territory with enemy units");
					}

					Territory t = (Territory)move.Territories[i];

					if(!t.AdjacentTo(previousTerritory))
					{
						throw new ActionException("Can't move between non-adjacent territories (" 
							+ previousTerritory.Name + " -> " + t.Name + ")");
					}

					if(t.Type == TerritoryType.Ground)//t.Owner != move.Owner)
					{
						ArrayList players = t.Units.GetPlayersWithUnits();
						//Console.WriteLine("Players: " + players.Count);

						if(players.Count > 0)
						{
							alreadyInEnemyTerritory = true;
						}						
					
					}

					if(t.Type == TerritoryType.Space && t.Units.GetUnits(UnitType.KillerSatellite).Count > 0)
					{
						alreadyInEnemyTerritory = true;
					}
			
					for(int j = 0; j < move.Units.Count; j++)
					{
						Unit u = move.Units[j];
						//Console.WriteLine(u.UnitType.ToString());
						//Console.WriteLine(u.MaxMoves);

						if(u.MaxMoves == 0)
						{
							throw new ActionException("Can't move an immobile unit");
						}

						if(t.Type == TerritoryType.Space && !u.IsSpaceCapable)
						{
							throw new ActionException("Can't move a ground unit into space");
						}

						if(u.UnitType == UnitType.Battler && t.Type == TerritoryType.Ground )
						{
							throw new ActionException("Can't move a battler onto the ground");
						}

						if(u.MovesLeft == 0)
						{
							throw new ActionException("Unit has no moves left.  Unit: " + u.Info);
						}

						u.CurrentTerritory = t;
						u.MovesLeft--;

					}

					previousTerritory = t;
				}
			}
			else if(action is TransportAction)
			{
				TransportAction ta = (TransportAction)action;
				if(ta.Load)
				{
					//LoadTransport(ta.Transport, ta.Units, ta.UnitType);
					LoadTransport(ta);
				}
				else
				{
					//UnloadTransport(ta.Transport, ta.MaxTransfer);
					UnloadTransport(ta);
				}
					
			}
			else
			{
				throw new ActionException("Unknown Action type in ExecuteActions");
			}
			m_checkedActions.Add(action);
			
		}

		public void UndoAction()
		{
			if(m_checkedActions.Count == 0)
			{
				throw new ActionException("No actions to undo");
			}

			Action action = (Action)m_checkedActions[m_checkedActions.Count - 1];

			if(action is MoveAction)
			{
				MoveAction ma = (MoveAction)action;

				for(int i = ma.Territories.Count - 1; i >= 0; i--)
				{
					Territory t = (Territory)ma.Territories[i];
					foreach(Unit u in ma.Units)
					{
						u.CurrentTerritory = t;
						u.MovesLeft++;
					}
				}

				foreach(Unit u in ma.Units)
				{
					u.CurrentTerritory = ma.StartingTerritory;
					u.MovesLeft++;
				}
			}
			else if(action is TransportAction)
			{
				TransportAction ta = (TransportAction)action;

				if(ta.Load)
				{
					UnloadTransport(ta);
				}
				else
				{
					LoadTransport(ta);
				}
			}
			else
			{
				throw new ActionException("Unknown Action type in UndoAction");
			}

			m_checkedActions.Remove(action);
			m_undoneActions.Add(action);

		}

		public void RedoAction()
		{
			if(m_undoneActions.Count == 0)
			{
				throw new ActionException("No undone actions to redo");
			}

			Action a = (Action)m_undoneActions[m_undoneActions.Count - 1];
			AddAction(a);
			m_undoneActions.Remove(a);

		}

		// Ends the current player's move phase
		public void EndMovePhase()
		{
			foreach(Action a in m_checkedActions)
			{
				if(a is MoveAction)
				{
					MoveAction ma = (MoveAction)a;

					foreach(Unit u in ma.Units)
					{
						u.MovesLeft = u.MaxMoves;
					}
				}
			}

			m_checkedActions.Clear();

			// TODO Change the active player

			// TODO Raise an event here for end of all movement?

		}

		#endregion

		public Hashlist FindBattles()
		{
			/* Two possible methods for finding battles.
			 * 
			 * 1) Brute force.  Search every single territory for multiple units.  Ugly and time-consuming, but will work.
			 * 2) Scan through the players and use the units list to calculate it somehow.  Don't know an algorithm
			 *    yet, but should be shorter.
			 *    
			 *    Hah!  Solution: Scan through each player's units and check if the territory has multiple players in
			 *    it.  Guaranteed to only search territories that already have units.  
			 *
			 */
			string[] searchOrder = {"Mercury", "Mercury Orbit", "Venus", "Venus Orbit", "Earth", "Moon", "Earth Orbit",
									"Trans-Earth Orbit", "Mars", "Trans-Mars Orbit", "Ceres", "Aurora", "Hygeia", "Juno", 
									   "Vesta", "Fortuna", "Thule", "Psyche", "Pallas", "Asteroid Orbit"};


			Hashtable planets = new Hashtable();
			for(int i = 0; i < searchOrder.Length; i++)
			{
				planets[searchOrder[i]] =  new Hashlist();
			}

			foreach(OrbitalSystem os in m_map.Planets)
			{
				if(os.HasKillerSatellite)
				{
					if(os.NearOrbit.Units.HasUnitsFromMultiplePlayers  
						// TODO Remove this comment && os.IsControlled)
						)
					{
						Hashlist hl = (Hashlist)planets[os.Name];
						// first time around, no need to check for contains
						BattleInfo bi = new BattleInfo();
						bi.Territory = os.NearOrbit;
						bi.Player = os.Owner;
						bi.Type = BattleType.KillerSatellite;
						hl.Add(bi.ToString(), bi);
					}
				}				
			}

			foreach(Player p in m_players)
			{
				UnitCollection battlers = p.Units.GetUnits(UnitType.Battler);
				if(battlers.Count == 0)
				{
					continue;
				}

				ArrayList battlerTerritories = battlers.GetUnitTerritories();
				foreach(Territory t in battlerTerritories)
				{
					if(CheckForBombing(t, p))
					{
						BattleInfo bi = new BattleInfo();
						bi.Territory =t;
						bi.Type = BattleType.Bombing;
						bi.Player = p;

						Hashlist hl = (Hashlist)planets[t.System.Name];
						hl.Add(bi.ToString(), bi);
					}
				}
			}

			for(int i = 0; i < m_players.Length; i++)
			{
				Player player = m_players[i];
				foreach(Unit u in player.Units)
				{
					if(u.CurrentTerritory.Units.HasUnitsFromMultiplePlayers)
					{
						string name;
						//if(u.CurrentTerritory.Type == TerritoryType.Ground)
						
						if(u.CurrentTerritory.System != OrbitalSystem.NONE)
						{
							name = u.CurrentTerritory.System.Name;
						}
						else
						{
							name = u.CurrentTerritory.Orbit	.Name;
						}

						// Add to the appropriate planet/orbit's list of battles
						Hashlist planetBattleList = (Hashlist)planets[name];

						BattleInfo bi = new BattleInfo();
						bi.Type = BattleType.Normal;
						bi.Territory = u.CurrentTerritory;
						//Console.WriteLine("Name: " + name + ", Unit: " + u.Info);
						if(!planetBattleList.ContainsKey(bi.ToString()))
						{
							
							planetBattleList.Add(bi.ToString(), bi);
						}
						
					}
				}
			}

			// Add the battles from each planet in the appropriate order
			m_battles = new Hashlist();
			for(int i = 0; i < searchOrder.Length; i++)
			{
				Hashlist planet = (Hashlist)planets[searchOrder[i]];

				if(planet.Count > 0)
				{
					foreach(BattleInfo bi in planet)
					{

						m_battles.Add(bi.ToString(), bi);
					}
				}
			}

			return m_battles;
			
		}

		private ArrayList GetSurfaceTerritories(Territory t)
		{
			ArrayList surfaceTerritories = new ArrayList();
			// TODO I really hate having to use EdgeToNeighbor here... can I have Neighbors return a Node?
			foreach(EdgeToNeighbor etn in t.Neighbors)
			{
				Territory neighbor = (Territory)etn.Neighbor;
				if(neighbor.Type == TerritoryType.Ground)
				{
					surfaceTerritories.Add(neighbor);
				}
			}

			return surfaceTerritories;
		}

		public bool CheckForBombing(Territory t)
		{
			return CheckForBombing(t, Player.NONE);
		}

		public bool CheckForBombing(Territory t, Player player)
		{
			bool addBombing = false;
			UnitCollection battlers; 
			UnitCollection surfaceUnits = new UnitCollection();

			if(t.Type != TerritoryType.Space)
			{
				throw new Exception("Can't bomb from a non-Space territory");
			}

			battlers = t.Units.GetUnits(UnitType.Battler);

			if(player != Player.NONE)
			{
				battlers = battlers.GetUnits(player);
			}

			

			foreach(Territory surface in GetSurfaceTerritories(t))
			{
				surfaceUnits.AddAllUnits(surface.Units);
			}			

			if(battlers.Count > 0)
			{
				foreach(Unit u in battlers)
				{
					UnitCollection otherPlayersUnits = surfaceUnits.GetNonMatchingUnits(u.Owner);
					if(otherPlayersUnits.Count > 0)
					{
						addBombing = true;
						break;
					}
				}
			}

			return addBombing;
		}

		public UnitCollection GetBombingTargets(Territory t, Player p)
		{
			ArrayList surfaceTerritories = new ArrayList();
			UnitCollection nearbyUnits = new UnitCollection();


			foreach(Territory surface in GetSurfaceTerritories(t))
			{
				UnitCollection otherUnits = surface.Units.GetNonMatchingUnits(p);
				
				nearbyUnits.AddAllUnits(otherUnits);
			}			

			UnitCollection troopers = nearbyUnits.GetUnits(UnitType.Trooper);
			UnitCollection gennies = nearbyUnits.GetUnits(UnitType.Factory);
			UnitCollection fighters = nearbyUnits.GetUnits(UnitType.Fighter);
			UnitCollection factories = nearbyUnits.GetUnits(UnitType.Factory);

			UnitCollection targets = new UnitCollection();
			targets.AddAllUnits(troopers);
			targets.AddAllUnits(gennies);
			targets.AddAllUnits(fighters);
			targets.AddAllUnits(factories);
			return targets;
		}

		public CombatResult DoKillerSatelliteCombat(BattleInfo bi)
		{
			CombatResult cr = new CombatResult();

			UnitCollection units = bi.Territory.Units;
			UnitCollection satellites = units.GetUnits(UnitType.KillerSatellite);
			Unit satellite = satellites[0];

			UnitCollection targets = units.GetNonMatchingUnits(satellite.Owner);
			UnitCollection leaders = units.GetUnits(UnitType.Leader);
			UnitCollection attackerLeaders = leaders.GetUnits(satellite.Owner);

			cr = new CombatResult();
			UnitCollection individualTarget = new UnitCollection();
			CombatInfo ci;

			// Since ExecuteCombat() ends when all attacking units have fired, and there's
			// only one attacking unit, we need to do a separate combat for each
			// defending unit
			for(int i = 0; i < targets.Count; i++)
			{
				ci = new CombatInfo();
				individualTarget.Clear();
				individualTarget.AddUnit(targets[i]);
				ci.Defenders.AddAllUnits(individualTarget);
				ci.Attackers.AddAllUnits(satellites);
				ci.AttackingLeader = !attackerLeaders.Empty;
						
				CombatResult individualCR = ExecuteCombat(ci);

				cr.AttackResults.AddRange(individualCR.AttackResults);
				if(!individualCR.Casualties.Empty)
				{
					cr.Casualties.AddAllUnits(individualCR.Casualties);
				}

				if(!individualCR.Survivors.Empty)
				{
					cr.Survivors.AddAllUnits(individualCR.Survivors);
				}

			}

			cr.UsedAttackers.AddAllUnits(satellites);

			return cr;
		}

		public CombatResult DoBombingCombat(CombatInfo ci)
		{
			//CombatResult cr = new CombatResult();

			

			return ExecuteCombat(ci);


			//return cr;
		}


		public CombatResult DoCombat(BattleInfo bi)
		{
			CombatResult cr = null;
			switch(bi.Type)
			{
				case BattleType.KillerSatellite:
				{
					UnitCollection units = bi.Territory.Units;
					UnitCollection satellites = units.GetUnits(UnitType.KillerSatellite);
					Unit satellite = satellites[0];

					UnitCollection targets = units.GetNonMatchingUnits(satellite.Owner);
					UnitCollection leaders = units.GetUnits(UnitType.Leader);
					UnitCollection attackerLeaders = leaders.GetUnits(satellite.Owner);

					cr = new CombatResult();
					UnitCollection individualTarget = new UnitCollection();
					CombatInfo ci;

					// Since ExecuteCombat() ends when all attacking units have fired, and there's
					// only one attacking unit, we need to do a separate combat for each
					// defending unit
					for(int i = 0; i < targets.Count; i++)
					{
						ci = new CombatInfo();
						individualTarget.Clear();
						individualTarget.AddUnit(targets[i]);
						ci.Defenders.AddAllUnits(individualTarget);
						ci.Attackers.AddAllUnits(satellites);
						ci.AttackingLeader = !attackerLeaders.Empty;
						
						CombatResult individualCR = ExecuteCombat(ci);

						if(!individualCR.Casualties.Empty)
						{
							cr.Casualties.AddAllUnits(individualCR.Casualties);
						}

						if(!individualCR.Survivors.Empty)
						{
							cr.Survivors.AddAllUnits(individualCR.Survivors);
						}

					}

					cr.UsedAttackers.AddAllUnits(satellites);

					break;
				}
				case BattleType.Bombing:
				{
					// Need to make sure this is still valid, since it's possible that a 
					// killer satellite wiped out all battlers in the territory

					break;
				}
			}

			return cr;
		}

		public CombatResult ExecuteCombat(CombatInfo ci)
		{
			CombatResult cr = new CombatResult();

			// it's basically a while loop - no incrementing of i, since this
			// needs to go until all attackers are done or all defenders are destroyed, 
			// and one unit will be removed from ci.Attackers each time through
			//for(int i = 0; i < ci.Attackers.Count;)
			while(ci.Attackers.Count > 0)
			{
				Unit attacker = (Unit)ci.Attackers[0];
				Unit defender = (Unit)ci.Defenders[0];

				int toHit = NOTPOSSIBLE;
				if(!(ci.Type == BattleType.Bombing))
				{
					toHit = m_combatTable[(int)attacker.UnitType, (int)defender.UnitType];
				}
				else
				{
					toHit = 7;
				}
				
				if(toHit == NOTPOSSIBLE)
				{
					throw new Exception("Can't attack a " + defender.UnitType + " with a " + attacker.UnitType);
				}

				int roll = RollD10();

				if(ci.AttackingLeader)
				{
					roll += 2;
				}

				if(roll > 10)
				{
					roll = 10;
				}

				ci.Attackers.RemoveUnit(attacker);
				cr.UsedAttackers.AddUnit(attacker);

				bool attackHit = (roll >= toHit);
				AttackResult ar = new AttackResult();
				ar.Attacker = attacker;
				ar.Defender = defender;
				ar.Roll = roll;
				ar.Hit = attackHit;
				cr.AttackResults.Add(ar);

				Console.WriteLine("To hit: " + toHit + ", roll: " + roll);
				if(attackHit)
				{
					ci.Defenders.RemoveUnit(defender);
					cr.Casualties.AddUnit(defender);

					if(ci.Defenders.Count == 0)
					{
						break;
					}
				}

				// TODO Maybe raise an event here that can pass the attack details back to the interface?

				
			}

			cr.UnusedAttackers.AddAllUnits(ci.Attackers);
			cr.Survivors.AddAllUnits(ci.Defenders);
			
			return cr;
		}
	}
}
