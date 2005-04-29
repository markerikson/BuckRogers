using System;
using System.Collections;
using CenterSpace.Free;
using skmDataStructures.Graph;
using System.Drawing;

namespace BuckRogers
{
	public enum GamePhase
	{
		Setup,
		Movement,
		Combat,
		Production,
	}
	public delegate bool StatusUpdateHandler(object sender, StatusUpdateEventArgs suea);
	/// <summary>
	/// Summary description for GameController.
	/// </summary>
	public class GameController
	{
		public event TerritoryOwnerChangedHandler TerritoryOwnerChanged;
		public event StatusUpdateHandler StatusUpdate;
		
		#region Properties
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

		public bool CanUndo
		{
			get
			{
				return m_checkedActions.Count != 0;
			}
		}

		public bool CanRedo
		{
			get
			{
				return m_undoneActions.Count != 0;
			}
		}

		public int TurnNumber
		{
			get
			{
				return m_turnNumber;
			}
		}

		public Player CurrentPlayer
		{
			get
			{
				return (Player)m_currentPlayerOrder[m_idxCurrentPlayer];
			}
		}

		public BuckRogers.TurnRoll[] Rolls
		{
			get { return this.m_rolls; }
			set { this.m_rolls = value; }
		}

		public BuckRogers.GamePhase CurrentPhase
		{
			get { return this.m_phase; }
			set { this.m_phase = value; }
		}

		public static BuckRogers.GameOptions Options
		{
			get { return m_options; }
			set { m_options = value; }
		}
		#endregion
	
		private GameMap m_map;
		private Color[] m_playerColors = {Color.CornflowerBlue, Color.Teal, Color.Yellow, 
											Color.Violet, Color.Tan, Color.MediumVioletRed};
		// Player list assumes players in clockwise order
		private Player[] m_players;
		private ArrayList m_currentPlayerOrder;
		public ArrayList m_rollResults;
		private ArrayList m_rollList;
		private ArrayList m_checkedActions;
		private ArrayList m_undoneActions;
		private int m_turnNumber;
		private int m_idxCurrentPlayer;
		private bool m_redoingAction;
		private GamePhase m_phase;
		private static GameOptions m_options = new GameOptions();

		private Hashlist m_battles;	
		public TurnRoll[] m_rolls;


		// TODO Start implementing optional rules

		public GameController()
		{
			Init();
		}

		public GameController(string[] playerNames)
		{
			m_options = new GameOptions();
			Init();
			SetPlayers(playerNames);
		}

		public GameController(string[] playerNames, GameOptions options)
		{
			m_options = options;
			Init();
			SetPlayers(playerNames);
		}

		private void Init()
		{
			m_map = new GameMap();

			m_checkedActions = new ArrayList();
			m_undoneActions = new ArrayList();
			m_turnNumber = 0;	

			m_redoingAction = false;
		}

		public void SetPlayers(string[] playerNames)
		{
			SetPlayers(playerNames, null);
		}

		public void SetPlayers(string[] playerNames, Color[] colors)
		{
			m_players = new Player[playerNames.Length];

			for(int i = 0; i < playerNames.Length; i++)
			{
				m_players[i] = new Player(playerNames[i]);
				if(colors == null)
				{
					m_players[i].Color = m_playerColors[i];
				}
				else
				{
					m_players[i].Color = colors[i];
				}
				
			}

			m_phase = GamePhase.Setup;
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
					int idx = Utility.Twister.Next(ground.Count - 1);
					Territory t = (Territory)ground[idx];
					t.Owner = p;
					ground.Remove(t);

					if(TerritoryOwnerChanged != null)
					{
						TerritoryEventArgs tea = new TerritoryEventArgs();
						tea.Name = t.Name;
						tea.Owner = p;

						TerritoryOwnerChanged(this, tea);
					}
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

		// Handles randomization of player order using rolls
		public void RollForInitiative(bool checkRollParity)
		{
			m_currentPlayerOrder = new ArrayList();
			m_rollResults = new ArrayList();
			m_rollList = new ArrayList();
			
			ArrayList players = new ArrayList(m_players);
			RollAndCheckForTies(players);

			m_rolls = (TurnRoll[])m_rollList.ToArray(typeof(TurnRoll));

			TurnRoll topRoll = (TurnRoll)m_rollResults[0];
			m_currentPlayerOrder.Add(topRoll.Player);
			int playerIndex = Array.IndexOf(m_players, topRoll.Player);

			// If it's an odd number, go to the right (counter-clockwise)
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
			// if it's even (or it's the setup turn), go to the left (clockwise)
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

		// Rolls dice and 
		private void RollAndCheckForTies(ArrayList players)
		{
			TurnRoll[] rolls = new TurnRoll[players.Count];
			for(int i = 0; i < players.Count; i++)
			{
				rolls[i] = new TurnRoll();
				rolls[i].Roll = Utility.RollD10();
				rolls[i].Player = (Player)players[i];				
			}

			// Sorts lowest to highest
			Array.Sort(rolls);
			// reverse so that it's highest to lowest
			Array.Reverse(rolls);

			for(int i = 0; i < rolls.Length; i++)
			{
				m_rollList.Add(rolls[i]);
			}

			int numTopRolls = 1;

			// check for top roll ties
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

			// we've got ties - add all the non-tied players to the results, then 
			// recurse and try again with just the tied players
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
			// no ties - put the current players at the start of the list
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

			UnitCollection unitsToUnload = null;
			
			if(ta.MatchMoves)
			{
				tr.Transportees.GetUnitsWithMoves(ta.Moves);
			}
			else
			{
				unitsToUnload = tr.Transportees;
			}

			for(int i = 0; i < numToUnload; i++)
			{
				// Transportees are automatically removed when the transporting unit is set to NONE,
				// so the index needs to stay at zero
				Unit u = unitsToUnload[0];
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

				string exceptionString = String.Empty;

				foreach(Unit u in move.Units)
				{
					move.OriginalMovesLeft.Add(u.MovesLeft);
				}

				// TODO Fix issues with undoing moves that turn out to be invalid while half done

				for(int i = 0; i < move.Territories.Count; i++)
				{
					if(alreadyInEnemyTerritory)
					{
						exceptionString = "Can't move units in and out of a territory with enemy units";
						goto MoveError;
					}

					Territory t = (Territory)move.Territories[i];

					if(!t.AdjacentTo(previousTerritory))
					{
						exceptionString = "Can't move between non-adjacent territories (" 
							+ previousTerritory.Name + " -> " + t.Name + ")";
						goto MoveError;
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
							//throw new ActionException("Can't move an immobile unit");
							exceptionString = "Can't move an immobile unit";
							goto MoveError;
						}

						if(t.Type == TerritoryType.Space && !u.IsSpaceCapable)
						{
							//throw new ActionException("Can't move a ground unit into space");
							exceptionString = "Can't move a ground unit into space";
							goto MoveError;
						}

						if(u.UnitType == UnitType.Battler && t.Type == TerritoryType.Ground )
						{
							//throw new ActionException("Can't move a battler onto the ground");
							exceptionString = "Can't move a battler onto the ground";
							goto MoveError;
							
						}

						if(u.MovesLeft == 0)
						{
							//throw new ActionException("Unit has no moves left.  Unit: " + u.Info);
							exceptionString = "Unit has no moves left.  Unit: " + u.Info;
							goto MoveError;
						}

						u.CurrentTerritory = t;
						u.MovesLeft--;

					}

					if(t.Type == TerritoryType.Ground 
						&& t.Owner != move.Owner 
						&& t.Units.GetPlayerUnitCounts().Count == 1)
					{
						move.ConqueredTerritories[t] = t.Owner;
					}

					previousTerritory = t;
				}

			MoveError:
				if(exceptionString != String.Empty)
				{
					for(int i = 0; i < move.OriginalMovesLeft.Count; i++)
					{
						move.Units[i].MovesLeft = (int)move.OriginalMovesLeft[i];
						move.Units[i].CurrentTerritory = move.StartingTerritory;
					}

					throw new ActionException(exceptionString);
				}

				foreach(Territory conquered in move.ConqueredTerritories.Keys)
				{
					conquered.Owner = move.Owner;

					if(TerritoryOwnerChanged != null)
					{
						TerritoryEventArgs tea = new TerritoryEventArgs();
						tea.Name = conquered.Name;
						tea.Owner = move.Owner;

						TerritoryOwnerChanged(this, tea);
					}
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

			if(!m_redoingAction)
			{
				m_undoneActions.Clear();
			}
			
			
		}

		public Action UndoAction()
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

				//foreach(Unit u in ma.Units)
				for(int i = 0; i < ma.Units.Count; i++)
				{
					Unit u = ma.Units[i];
					u.CurrentTerritory = ma.StartingTerritory;
					//u.MovesLeft = (int)ma.OriginalMovesLeft[i];
				}

				foreach(Territory t in ma.ConqueredTerritories.Keys)
				{
					Player originalOwner = (Player)ma.ConqueredTerritories[t];
					t.Owner = originalOwner;

					if(TerritoryOwnerChanged != null)
					{
						TerritoryEventArgs tea = new TerritoryEventArgs();
						tea.Name = t.Name;
						tea.Owner = originalOwner;

						TerritoryOwnerChanged(this, tea);
					}
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

			return action;
		}

		public Action RedoAction()
		{
			if(m_undoneActions.Count == 0)
			{
				throw new ActionException("No undone actions to redo");
			}

			Action a = (Action)m_undoneActions[m_undoneActions.Count - 1];
			m_redoingAction = true;
			AddAction(a);
			m_redoingAction = false;
			m_undoneActions.Remove(a);

			return a;

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

		}

		#endregion

		// TODO Move this to BattleController?
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

			// Since we're adding them to an ordered list, we don't care that the Hashtable returns them unordered
			foreach(OrbitalSystem os in m_map.Planets)
			{
				if(os.HasKillerSatellite)
				{
					if(os.NearOrbit.Units.HasUnitsFromMultiplePlayers  
						&& os.IsControlled)
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

			// Same with players - we don't care what order we search them in
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
					if(u.Transported)
					{
						continue;
					}
					if(u.CurrentTerritory.Units.HasUnitsFromMultiplePlayers)
					{
						string name;

						
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

						// need to use ToString() because there can be more than one battle in a territory
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

			foreach(Territory neighbor in t.Neighbors)
			{
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

		public bool CheckProduction(ProductionInfo pi)//(Factory f, UnitType ut, Territory t)
		{
			bool validProduction = true;

			if(!pi.Factory.CanProduce)
			{
				throw new Exception("Factory is currently not able to produce");
			}

			if(pi.Factory.UnitHalfProduced)
			{
				throw new Exception("Can't change production while a unit is half-produced");
			}

			//if(pi.DestinationTerritory != null)
			//{
			OrbitalSystem os = pi.DestinationTerritory.System;
			Territory orbit = os.NearOrbit;

			switch(pi.Type)
			{
				case UnitType.KillerSatellite:
				{
					UnitCollection otherUnits = orbit.Units.GetNonMatchingUnits(pi.Factory.Owner);

					if(!(os.Owner == pi.Factory.Owner))
					{
						throw new Exception("Can't start a Killer Satellite if you don't own the planet");
					}

					
					if(otherUnits.Count > 0)
					{
						throw new Exception("Can't start a Killer Satellite if enemy units are in Near Orbit");
					}

					if(pi.DestinationTerritory != orbit)
					{
						throw new Exception("Must place a Killer Satellite in Near Orbit");
					}
					
					goto case UnitType.Battler;
				}
				case UnitType.Battler:
				{
					if(pi.DestinationTerritory.Type != TerritoryType.Space)
					{
						throw new Exception("Must deploy Battlers into space");
					}
					break;
				}
				case UnitType.Trooper:
				case UnitType.Gennie:
				case UnitType.Fighter:
				case UnitType.Transport:
				{
					if(pi.DestinationTerritory != pi.Factory.CurrentTerritory)
					{
						throw new Exception("Must build this unit in the same territory as the factory");
					}
					break;
				}
			}
			//}

			return validProduction;
		}

		public void ExecuteProduction()
		{
			foreach(Player p in m_currentPlayerOrder)
			{
				if(!p.Disabled)
				{
					UnitCollection factories = p.Units.GetUnits(UnitType.Factory);

					foreach(Factory f in factories)
					{
						if(f.CanProduce)
						{
							f.ExecuteProduction();
						}
					}
				}
			}
		}

		// TODO Check for dead players and remove them
		// TODO Uncontrolled Killer Satellites become the property of the new planetary owner

		public void NextTurn()
		{
			m_checkedActions.Clear();
			m_undoneActions.Clear();

			m_map.AdvancePlanets();
			RollForInitiative();

			m_idxCurrentPlayer = 0;
			m_turnNumber++;

			m_phase = GamePhase.Movement;
		}

		public bool NextPlayer()
		{
			m_undoneActions.Clear();
			m_checkedActions.Clear();
			
			bool morePlayers = true;
            if(m_idxCurrentPlayer == m_currentPlayerOrder.Count - 1)
			{
				m_idxCurrentPlayer = 0;
				morePlayers = false;
			}
			else
			{
				m_idxCurrentPlayer++;
			}

			if(!morePlayers)
			{
				NextPhase();
			}
			

			if(StatusUpdate != null)
			{
				StatusUpdateEventArgs suea = new StatusUpdateEventArgs();
				suea.Player = (Player)m_currentPlayerOrder[m_idxCurrentPlayer];

				
				suea.StatusInfo = morePlayers ? StatusInfo.NextPlayer : StatusInfo.NextPhase;
				StatusUpdate(this, suea);
			}

			return morePlayers;
		}

		public void NextPhase()
		{
			switch(m_phase)
			{
				case GamePhase.Setup:
				case GamePhase.Production:
				{
					m_phase = GamePhase.Movement;
					break;
				}
				case GamePhase.Movement:
				{
					m_phase = GamePhase.Combat;
					break;
				}
				case GamePhase.Combat:
				{
					m_phase = GamePhase.Production;
					break;
				}
			}
		}

		

	}
}
