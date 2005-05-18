using System;
using System.Collections;
using CenterSpace.Free;
using skmDataStructures.Graph;
using System.Drawing;
using System.Text;
using System.Xml;

namespace BuckRogers
{
	public enum GamePhase
	{
		Setup,
		Movement,
		Combat,
		Production,
		EndTurn,
		GameOver,
	}
	public delegate bool StatusUpdateHandler(object sender, StatusUpdateEventArgs suea);
	public delegate void DisplayActionHandler(Action a);
	public delegate void TerritoryUnitsChangedHandler(object sender, TerritoryUnitsEventArgs tuea);
	public delegate void PlayersCreatedHandler();
	/// <summary>
	/// Summary description for GameController.
	/// </summary>
	public class GameController
	{
		public event TerritoryOwnerChangedHandler TerritoryOwnerChanged;
		public event StatusUpdateHandler StatusUpdate;
		public event DisplayActionHandler ActionAdded;
		public event TerritoryUnitsChangedHandler TerritoryUnitsChanged;
		public TerritoryUpdateHandler UpdateTerritory;
		//public event PlayersCreatedHandler PlayersCreated;
		
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

		public int ActionsCount
		{
			get
			{
				// by the time this is called, the action is verified but 
				// hasn't actually been added to the list
				return m_checkedActions.Count;
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
		private Color[] m_playerColors = {Color.CornflowerBlue, Color.Yellow, Color.Teal,  
											Color.Violet, Color.Tan, Color.MediumVioletRed};
		// Player list assumes players in clockwise order
		private Player[] m_players;
		private Player m_winner;
		private ArrayList m_currentPlayerOrder;
		public ArrayList m_rollResults;
		private ArrayList m_rollList;
		private ArrayList m_checkedActions;
		private ArrayList m_undoneActions;
		private Hashtable m_alteredTerritories;
		private int m_turnNumber;
		private int m_idxCurrentPlayer;
		private bool m_redoingAction;
		private GamePhase m_phase;
		private static GameOptions m_options = new GameOptions();
		private XmlDocument m_gamelog;
		private XmlElement m_rootNode;
		private XmlElement m_xeTurns;
		private XmlElement m_xeCurrentTurn;
		private XmlElement m_xeCurrentMovement;
		private XmlElement m_xeCurrentPlayer;

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

		public GameController(GameOptions options)
		{
			m_options = options;
			Init();
			SetPlayers(m_options.PlayerNames);
		}

		private void Init()
		{
			m_map = new GameMap();

			m_checkedActions = new ArrayList();
			m_undoneActions = new ArrayList();
			m_alteredTerritories = new Hashtable();
			m_winner = Player.NONE;
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

		public void InitGamelog()
		{
			m_gamelog = new XmlDocument();
			m_rootNode = m_gamelog.CreateElement("Game");
			m_gamelog.AppendChild(m_rootNode);

			XmlElement setup = m_gamelog.CreateElement("Setup");
			XmlElement options = m_gamelog.CreateElement("Options");
			XmlElement seed = m_gamelog.CreateElement("Seed");
			XmlElement players = m_gamelog.CreateElement("Players");
			XmlElement assignment = m_gamelog.CreateElement("Assignment");
			seed.InnerText = Utility.Twister.Seed.ToString();

			m_rootNode.AppendChild(setup);
			setup.AppendChild(options);
			setup.AppendChild(seed);
			setup.AppendChild(players);
			setup.AppendChild(assignment);

			foreach(GameOption option in m_options.OptionalRules)
			{
				if(option.Value)
				{
					XmlElement xeOption = m_gamelog.CreateElement("Option");
					XmlAttribute name = m_gamelog.CreateAttribute("name");
					name.Value = option.Name;
					xeOption.Attributes.Append(name);
					options.AppendChild(xeOption);
				}
			}

			foreach(Player p in m_players)
			{
				XmlElement xePlayerInfo = m_gamelog.CreateElement("Player");
				XmlAttribute name = m_gamelog.CreateAttribute("name");
				name.Value = p.Name;
				XmlAttribute color = m_gamelog.CreateAttribute("color");
				color.Value = p.Color.Name;
				xePlayerInfo.Attributes.Append(name);
				xePlayerInfo.Attributes.Append(color);

				players.AppendChild(xePlayerInfo);

				XmlElement xePlayerTerritories = m_gamelog.CreateElement("Player");
				XmlAttribute pname = m_gamelog.CreateAttribute("name");
				pname.Value = p.Name;
				xePlayerTerritories.Attributes.Append(pname);

				foreach(Territory t in p.Territories.Values)
				{
					XmlElement xeTerritory = m_gamelog.CreateElement("Territory");
					XmlAttribute tname = m_gamelog.CreateAttribute("name");
					tname.Value = t.Name;
					xeTerritory.Attributes.Append(tname);

					xePlayerTerritories.AppendChild(xeTerritory);
				}

				assignment.AppendChild(xePlayerTerritories);
			}

			m_gamelog.Save(@"gamelog.xml");
		}

		public void LogInitialPlacements()
		{
			XmlNode setup = m_rootNode.GetElementsByTagName("Setup")[0];

			XmlElement placement = m_gamelog.CreateElement("Placement");

			foreach(Player p in m_currentPlayerOrder)
			{
				XmlElement xePlayerUnits = m_gamelog.CreateElement("Player");
				XmlAttribute pname = m_gamelog.CreateAttribute("name");
				pname.Value = p.Name;
				xePlayerUnits.Attributes.Append(pname);

				foreach(Unit u in p.Units)
				{
					XmlElement xeUnit = m_gamelog.CreateElement("Unit");
					XmlAttribute tname = m_gamelog.CreateAttribute("type");
					XmlAttribute territory = m_gamelog.CreateAttribute("territory");
					tname.Value = u.Type.ToString();
					territory.Value = u.CurrentTerritory.Name;
					xeUnit.Attributes.Append(tname);
					xeUnit.Attributes.Append(territory);

					xePlayerUnits.AppendChild(xeUnit);
				}

				placement.AppendChild(xePlayerUnits);
			}

			setup.AppendChild(placement);

			m_xeTurns = m_gamelog.CreateElement("Turns");
			m_rootNode.AppendChild(m_xeTurns);
			m_gamelog.Save("gamelog.xml");
		}

		public void LogNextPlayer()
		{
			m_xeCurrentPlayer = m_gamelog.CreateElement("Player");
			XmlAttribute name = m_gamelog.CreateAttribute("name");
			name.Value = CurrentPlayer.Name;
			m_xeCurrentPlayer.Attributes.Append(name);

			if(m_phase == GamePhase.Movement)
			{
				m_xeCurrentMovement.AppendChild(m_xeCurrentPlayer);
			}
			else if(m_phase == GamePhase.Production)
			{

			}
			
		}

		public void SaveLog()
		{
			m_gamelog.Save("gamelog.xml");
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
				int numTerritories = 6;

				if(!m_options.OptionalRules["LimitedTwoPlayerSetup"])
				{
					if(m_players.Length == 2)
					{
						numTerritories = 12;
					}
					else if(m_players.Length == 3)
					{
						numTerritories = 9;
					}
				}
				

				for(int i = 0; i < numTerritories; i++)
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
			int numPlayersModifier = 1;

			if(!m_options.OptionalRules["LimitedTwoPlayerSetup"])
			{
				if(m_players.Length == 3)
				{
					numPlayersModifier = 2;
				}
				else if(m_players.Length == 2)
				{
					numPlayersModifier = 3;
				}
			}
			
			foreach(Player p in m_players)
			{
				Unit u = null;

				for(int i = 0; i < 8 * numPlayersModifier; i++)
				{
					u = Unit.CreateNewUnit(p, UnitType.Trooper);
				}

				for(int i = 0; i < 4 * numPlayersModifier; i++)
				{
					u = Unit.CreateNewUnit(p, UnitType.Fighter);
				}

				for(int i = 0; i < 2 * numPlayersModifier; i++)
				{
					u = Unit.CreateNewUnit(p, UnitType.Factory);
				}

				for(int i = 0; i < 2 * numPlayersModifier; i++)
				{
					u = Unit.CreateNewUnit(p, UnitType.Gennie);
				}

				for(int i = 0; i < numPlayersModifier; i++)
				{
					u = Unit.CreateNewUnit(p, UnitType.Transport);
					u = Unit.CreateNewUnit(p, UnitType.Leader);
				}

				Factory blackMarket = (Factory)Unit.CreateNewUnit(p, UnitType.Factory);
				blackMarket.IsBlackMarket = true;
				blackMarket.CanProduce = false;
				blackMarket.CurrentTerritory = m_map["Black Market"];
				
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
			
			ArrayList players = new ArrayList();//m_players);

			foreach(Player p in m_players)
			{
				if(!p.Disabled)
				{
					players.Add(p);
				}
			}
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
					Player p = m_players[i];
					if(m_players[i].Disabled)
					{
						continue;
					}
					m_currentPlayerOrder.Add(m_players[i]);
				}

				for(int i = m_players.Length - 1; i > playerIndex; i--)
				{
					Player p = m_players[i];
					if(m_players[i].Disabled)
					{
						continue;
					}
					m_currentPlayerOrder.Add(m_players[i]);
				}
			}
			// if it's even (or it's the setup turn), go to the left (clockwise)
			else
			{
				for(int i = playerIndex + 1; i < m_players.Length; i++)
				{
					Player p = m_players[i];
					if(m_players[i].Disabled)
					{
						continue;
					}
					m_currentPlayerOrder.Add(m_players[i]);
				}

				for(int i = 0; i < playerIndex; i++)
				{
					Player p = m_players[i];
					if(m_players[i].Disabled)
					{
						continue;
					}
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

					Territory startingTerritory = factory.CurrentTerritory;
					factory.CurrentTerritory = Territory.NONE;
					factory.Transported = true;
					factory.TransportingUnit = transport;

					transport.Transportees.AddUnit(factory);

					UnitCollection remainingFactories = startingTerritory.Units.GetUnits(UnitType.Factory);

					if(remainingFactories.Count == 1)
					{
						Factory otherFactory = (Factory)remainingFactories[0];
						otherFactory.CanProduce = true;
					}
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
			ta.UnitType = tr.Transportees[0].Type;

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

						if(u.Type == UnitType.Battler && t.Type == TerritoryType.Ground )
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
						bool conquerTerritory = true;

						Hashtable unitCount = t.Units.GetPlayerUnitCounts();
						unitCount.Remove(UnitType.Leader);
						
						// can't conquer a territory with just a Leader
						if(unitCount.Count == 0)
						{
							conquerTerritory = false;
						}

						if(m_options.OptionalRules["ConquerWithGround"])
						{
							if(!(t.System is Asteroid))
							{
								conquerTerritory = unitCount.ContainsKey(UnitType.Trooper) || unitCount.ContainsKey(UnitType.Gennie);
							}
						}
						if(conquerTerritory)
						{
							move.ConqueredTerritories[t] = t.Owner;
						}
						
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

				// needed here just to make sure the move gets listed before a possible transport action
				m_checkedActions.Add(action);
				if(ActionAdded != null)
				{
					ActionAdded(action);
				}

				XmlElement xeAction = m_gamelog.CreateElement("Action");
				XmlAttribute xaMoveType = m_gamelog.CreateAttribute("type");
				xaMoveType.Value = "Movement";
				xeAction.Attributes.Append(xaMoveType);
				XmlAttribute xaStart = m_gamelog.CreateAttribute("territory");
				xaStart.Value = move.StartingTerritory.Name;
				xeAction.Attributes.Append(xaStart);

				XmlElement xeTerritories = m_gamelog.CreateElement("Territories");
				xeAction.AppendChild(xeTerritories);

				foreach(Territory t in move.Territories)
				{
					XmlElement xeTerritory = m_gamelog.CreateElement("Territory");
					XmlAttribute xaTerritoryName = m_gamelog.CreateAttribute("name");
					xaTerritoryName.Value = t.Name;
					xeTerritory.Attributes.Append(xaTerritoryName);

					xeTerritories.AppendChild(xeTerritory);
				}

				XmlElement xeUnits = m_gamelog.CreateElement("Units");
				xeAction.AppendChild(xeUnits);

				foreach(Unit u in move.Units)
				{
					XmlElement xeUnit = m_gamelog.CreateElement("Unit");
					XmlAttribute xaUnitType = m_gamelog.CreateAttribute("type");
					xaUnitType.Value = u.Type.ToString();
					XmlAttribute xaUnitID = m_gamelog.CreateAttribute("id");
					xaUnitID.Value = u.ID.ToString();
					xeUnit.Attributes.Append(xaUnitType);
					xeUnit.Attributes.Append(xaUnitID);
					
					
					xeUnits.AppendChild(xeUnit);
				}

				m_xeCurrentPlayer.AppendChild(xeAction);

				if(TerritoryUnitsChanged != null)
				{
					TerritoryUnitsEventArgs tuea = new TerritoryUnitsEventArgs();
					tuea.Units = move.Units;
					tuea.Territory = move.StartingTerritory;
					tuea.Added = false;
					tuea.Player = move.Owner;

					m_alteredTerritories[move.StartingTerritory] = null;

					TerritoryUnitsChanged(this, tuea);

					Territory finalTerritory = (Territory)move.Territories[move.Territories.Count - 1];
					tuea.Territory = finalTerritory;
					tuea.Added = true;

					m_alteredTerritories[finalTerritory] = null;


					TerritoryUnitsChanged(this, tuea);
				}

				
				Territory destination = (Territory)move.Territories[move.Territories.Count - 1];
				if(destination.Type == TerritoryType.Ground)
				{					
					bool unloadTransports = false;

					UnitCollection transports = move.Units.GetUnits(UnitType.Transport);
					if(transports.Count > 0)
					{
						bool askToUnload = false;

						foreach(Transport tr in transports)
						{
							if(tr.Transportees.Count > 0)
							{
								askToUnload = true;
								break;
							}
						}
						if(askToUnload)
						{
							if(StatusUpdate != null)
							{
								StatusUpdateEventArgs suea = new StatusUpdateEventArgs();
								suea.Territory = destination;//(Player)m_currentPlayerOrder[m_idxCurrentPlayer];
				
								suea.StatusInfo = StatusInfo.TransportLanded;
								unloadTransports = StatusUpdate(this, suea);
							}	
						}									
					}

					if(unloadTransports)
					{
						foreach(Transport tr in transports)
						{
							TransportAction ta = new TransportAction();
							ta.Load = false;
							ta.Owner = tr.Owner;
							ta.MaxTransfer = tr.Transportees.Count;
							ta.StartingTerritory = destination;
							ta.Transport = tr;
							ta.UnitType = tr.Transportees[0].Type;

							AddAction(ta);
						}
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

				m_checkedActions.Add(action);
				m_alteredTerritories[action.StartingTerritory] = null;
				if(ActionAdded != null)
				{
					ActionAdded(action); 
				}

				XmlElement xeAction = m_gamelog.CreateElement("Action");
				XmlAttribute xaType = m_gamelog.CreateAttribute("type");
				xaType.Value = "Transport";
				XmlAttribute xaTerritory = m_gamelog.CreateAttribute("territory");
				xaTerritory.Value = ta.StartingTerritory.Name;
				XmlAttribute xaLoad = m_gamelog.CreateAttribute("load");
				xaLoad.Value = ta.Load.ToString();

				xeAction.Attributes.Append(xaType);
				xeAction.Attributes.Append(xaTerritory);
				xeAction.Attributes.Append(xaLoad);

				XmlElement xeUnits = m_gamelog.CreateElement("Units");
				
				foreach(Unit u in ta.Units)
				{
					XmlElement xeUnit = m_gamelog.CreateElement("Unit");
					XmlAttribute xaUnitType = m_gamelog.CreateAttribute("type");
					xaUnitType.Value = u.Type.ToString();
					XmlAttribute xaMoves = m_gamelog.CreateAttribute("moves");
					xaMoves.Value = u.MovesLeft.ToString();
					XmlAttribute xaUnitID = m_gamelog.CreateAttribute("id");
					xaUnitID.Value = u.ID.ToString();
					
					xeUnit.Attributes.Append(xaUnitType);
					xeUnit.Attributes.Append(xaMoves);
					xeUnit.Attributes.Append(xaUnitID);

					xeUnits.AppendChild(xeUnit);
				}

				xeAction.AppendChild(xeUnits);
				
				m_xeCurrentPlayer.AppendChild(xeAction);


				if(TerritoryUnitsChanged != null)
				{
					TerritoryUnitsEventArgs tuea = new TerritoryUnitsEventArgs();
					tuea.Units = ta.Units;
					tuea.Territory = ta.StartingTerritory;
					tuea.Added = !ta.Load;
					tuea.Player = ta.Owner;

					TerritoryUnitsChanged(this, tuea);
				}

					
			}
			else
			{
				throw new ActionException("Unknown Action type in ExecuteActions");
			}

			


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

					if(u.CurrentTerritory.Name == "Black Market")
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
						// Altered battles should be taken care of by the battle controller
						//m_alteredTerritories[bi.Territory] = null;

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

		public bool CheckBlackMarket(Player p)
		{
			UnitCollection factories = p.Units.GetUnits(UnitType.Factory);

			if(factories.Count == 1)
			{
				Factory f = (Factory)factories[0];

				if(f.IsBlackMarket)
				{
					f.CanProduce = true;

					return true;
				}
			}
			return false;
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

					UnitCollection factories = pi.Factory.Owner.Units.GetUnits(UnitType.Factory);

					foreach(Factory f in factories)
					{
						if(f == pi.Factory)
						{
							continue;
						}

						if(f.ProductionType == UnitType.KillerSatellite 
							&& f.DestinationTerritory == pi.DestinationTerritory)
						{
							throw new Exception("Can't build two Killer Satellites in the same place");
						}
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
					if(pi.DestinationTerritory != pi.Factory.CurrentTerritory && !pi.Factory.IsBlackMarket)
					{
						throw new Exception("Must build this unit in the same territory as the factory");
					}
					break;
				}
			}
			
			if(pi.DestinationTerritory.Type == TerritoryType.Space 
				&& pi.Type != UnitType.Battler
				&& pi.Type != UnitType.KillerSatellite)
			{
				throw new Exception("Can't build this in space");
			}

			if(pi.Factory.IsBlackMarket)
			{
				if(pi.Type != UnitType.Trooper 
					&& pi.Type != UnitType.Fighter
					&& pi.Type != UnitType.Factory)
				{
					throw new Exception("Can only build Troopers, Fighters, or Factories on the Black Market");
				}
			}


			return validProduction;
		}

		public void ExecuteProduction()
		{
			XmlElement xeProduction = m_gamelog.CreateElement("Production");

			foreach(Player p in m_currentPlayerOrder)
			{
				if(!p.Disabled)
				{
					XmlElement xePlayer = m_gamelog.CreateElement("Player");
					XmlAttribute xaName = m_gamelog.CreateAttribute("name");
					xaName.Value = p.Name;
					xePlayer.Attributes.Append(xaName);

					UnitCollection factories = p.Units.GetUnits(UnitType.Factory);

					foreach(Factory f in factories)
					{
						if(f.CanProduce && f.ProductionType != UnitType.None)
						{
							XmlElement xeUnit = m_gamelog.CreateElement("Unit");
							XmlAttribute xaType = m_gamelog.CreateAttribute("type");
							XmlAttribute xaNumber = m_gamelog.CreateAttribute("number");
							XmlAttribute xaLocation = m_gamelog.CreateAttribute("location");
							XmlAttribute xaDestination = m_gamelog.CreateAttribute("destination");
							xaType.Value = f.ProductionType.ToString();
							xaNumber.Value = f.AmountProduced.ToString();
							xaLocation.Value = f.CurrentTerritory.Name;
							xaDestination.Value = f.DestinationTerritory.Name;

							xeUnit.Attributes.Append(xaType);
							xeUnit.Attributes.Append(xaNumber);
							xeUnit.Attributes.Append(xaLocation);
							xeUnit.Attributes.Append(xaDestination);

							xePlayer.AppendChild(xeUnit);

							m_alteredTerritories[f.DestinationTerritory] = null;
							f.ExecuteProduction();
						}
					}
					
					xeProduction.AppendChild(xePlayer);
				}
			}

			m_xeCurrentTurn.AppendChild(xeProduction);
		}

		// TODO Uncontrolled Killer Satellites become the property of the new planetary owner

		public void NextTurn()
		{
			m_checkedActions.Clear();
			m_undoneActions.Clear();

			foreach(Player p in m_players)
			{
				if(p.Disabled)
				{
					if(p.TurnDisabled == TurnNumber - 1)
					{
						p.Disabled = false;
					}
				}
			}

			if(UpdateTerritory != null)
			{
				foreach(Territory t in m_alteredTerritories.Keys)
				{
					UpdateTerritory(t);
				}

			}

			m_alteredTerritories.Clear();
			

			if(m_turnNumber != 0)
			{
				m_map.AdvancePlanets();
			}			
			else
			{
				InitGamelog();
				LogInitialPlacements();
			}

			CheckDeadPlayers();

			if(!CheckVictory())
			{
				RollForInitiative();

				m_idxCurrentPlayer = 0;
				m_turnNumber++;

				m_phase = GamePhase.Movement;

				m_xeCurrentTurn = m_gamelog.CreateElement("Turn");
				XmlAttribute number = m_gamelog.CreateAttribute("number");
				XmlAttribute order = m_gamelog.CreateAttribute("order");
				number.Value = m_turnNumber.ToString();
				
				StringBuilder sb = new StringBuilder();
				foreach(Player p in m_currentPlayerOrder)
				{
					int idx = Array.IndexOf(m_players, p);
					sb.Append(idx);
				}
				order.Value = sb.ToString();

				m_xeCurrentTurn.Attributes.Append(number);
				m_xeCurrentTurn.Attributes.Append(order);
				m_xeTurns.AppendChild(m_xeCurrentTurn);

				m_xeCurrentMovement = m_gamelog.CreateElement("Movement");
				m_xeCurrentTurn.AppendChild(m_xeCurrentMovement);

				LogNextPlayer();
				
			}			
			else
			{
				if(StatusUpdate != null)
				{
					StatusUpdateEventArgs suea = new StatusUpdateEventArgs();
					suea.Player = m_winner;//(Player)m_currentPlayerOrder[m_idxCurrentPlayer];
				
					suea.StatusInfo = StatusInfo.GameOver;
					StatusUpdate(this, suea);
				}
			}
			m_gamelog.Save("gamelog.xml");
		}

		private bool CheckVictory()
		{
			bool gameOver = false;

			switch(m_options.WinningConditions)
			{
				case VictoryConditions.TotalAnnihilation:
				{
					gameOver = (m_players.Length == 1);
					
					if(gameOver)
					{
						m_winner = m_players[0];
					}
					break;
				}
				case VictoryConditions.OneEarthYear:
				{
					if(m_turnNumber == 8)
					{
						// Can't test against Player.NONE, because he "owns" all the 
						// space zones
						Player highest = m_players[0];

						foreach(Player p in m_players)
						{
							if(p.Territories.Count > highest.Territories.Count)
							{
								highest = p;
							}
						}

						gameOver = true;
						m_winner = highest;
					}
					break;
				}
			}
			return gameOver;
		}

		private void CheckDeadPlayers()
		{
			ArrayList al = new ArrayList(m_players);

			foreach(Player p in m_players)
			{
				if(p.Units.Count == 0 && p.Territories.Count == 0)
				{
					al.Remove(p);
				}
			}

			m_players = (Player[])al.ToArray(typeof(Player));

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

				LogNextPlayer();
			}

			if(!morePlayers)
			{
				// returns false if we don't go to the next phase, so to 
				// tell the listener that we're not advancing, needs to be true
				// Makes sense in a twisted way at the moment
				morePlayers = !CheckNextPhase();
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

		public bool CheckNextPhase()
		{
			bool doNextPhase = true;
			switch(m_phase)
			{
				case GamePhase.Setup:
				{
					foreach(Player p in m_currentPlayerOrder)
					{
						for(int i = 0; i < p.Units.Count && doNextPhase; i++)
						{
							Unit u = p.Units[i];
							if(u.CurrentTerritory == Territory.NONE)
							{
								doNextPhase = false;
								break;

							}
						}
					}

					if(doNextPhase)
					{
						m_phase = GamePhase.Movement;
					}
					break;
				}
				case GamePhase.Production:
				{
					m_phase = GamePhase.EndTurn;

					if(StatusUpdate != null)
					{
						StatusUpdateEventArgs suea = new StatusUpdateEventArgs();				
						suea.StatusInfo = StatusInfo.NextPhase;
						StatusUpdate(this, suea);
					}
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
			return doNextPhase;
		}

		public void PlaceUnits(UnitCollection uc, Territory t)
		{
			foreach(Unit u in uc)
			{
				u.CurrentTerritory = t;
			}

			if(TerritoryUnitsChanged != null)
			{
				TerritoryUnitsEventArgs tuea = new TerritoryUnitsEventArgs();
				tuea.Units = uc;
				tuea.Territory = t;
				tuea.Added = true;

				TerritoryUnitsChanged(this, tuea);
			}
		}

		public System.Xml.XmlDocument Gamelog
		{
			get { return this.m_gamelog; }
			set { this.m_gamelog = value; }
		}
	}
}
