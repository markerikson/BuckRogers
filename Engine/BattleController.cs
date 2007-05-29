using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace BuckRogers
{
	#region delegates

	//public delegate void DisplayUnitsHandler(object sender, DisplayUnitsEventArgs e);
	public delegate void TerritoryUpdateHandler(Territory t);
	public delegate void BattleStatusUpdateHandler(BattleStatus status);

	#endregion

	#region BattleStatus enum
	public enum BattleStatus
	{
		None,
		Setup,
		AttackComplete,
		PlayerTurnFinished,
		BattleComplete,
		BattleReady,
	}

	#endregion

	public class BattleController
	{
		#region events
		public event EventHandler<StatusUpdateEventArgs> StatusUpdate = delegate { };
		public event EventHandler<TerritoryEventArgs> TerritoryOwnerChanged = delegate { };
		public event EventHandler<TerritoryUnitsEventArgs> TerritoryUnitsChanged = delegate { };
		public event EventHandler<DisplayUnitsEventArgs> UnitsToDisplay = delegate { };

		public event EventHandler<StatusUpdateEventArgs> BattleStatusUpdated = delegate { };
		public event EventHandler<StatusUpdateEventArgs> UpdateTerritory = delegate { };

		//public event DisplayUnitsHandler UnitsToDisplay;
		//public event TerritoryOwnerChangedHandler TerritoryOwnerChanged;
		//public StatusUpdateHandler StatusUpdate;
		//public TerritoryUpdateHandler UpdateTerritory;
		//public event TerritoryUnitsChangedHandler TerritoryUnitsChanged;
		//public event BattleStatusUpdateHandler BattleStatusUpdated;

		#endregion

		#region private members

		private int[,] m_combatTable = new int[,]	{	{6, 8, 7, NOTPOSSIBLE, 6, NOTPOSSIBLE, 3}, // Trooper
														{5, 6, 6, NOTPOSSIBLE, 5, NOTPOSSIBLE, 2}, // Gennie
														{7, 7, 6, 8, 3, 7, 3}, // Fighter
														{7, 7, 4, 6, 4, 6, NOTPOSSIBLE}, // Battler
														{9, 10, 8, 10, 6, 10, 9}, // Transport
														{NOTPOSSIBLE, NOTPOSSIBLE, 6, 7, 5, NOTPOSSIBLE, NOTPOSSIBLE}, // Killer Satellite
														{8, 9, 9, NOTPOSSIBLE, 7, NOTPOSSIBLE, NOTPOSSIBLE}, // Control marker
													};

		private const int NOTPOSSIBLE = 99;

		private GameController m_controller;

		private Hashlist m_battles;

		private UnitCollection m_survivingUnits;
		private UnitCollection m_currentUnused;
		private UnitCollection m_capturedFactories;

		private ArrayList m_playerOrder;

		private BattleInfo m_currentBattle;
		
		private Player m_currentPlayer;
		
		private CombatResult m_turnResult;
		private CombatResult m_cumulativeResult;
		//private CombatResult m_lastResult;
		private BattleStatus m_status;

		private XmlDocument m_gamelog;

		/*
		private XmlNode m_xnTurns;
		private XmlElement m_xeCurrentTurn;
		private XmlElement m_xeBattles;
		private XmlElement m_xeCurrentBattle;
		*/

		private int m_numRolls;
		private bool m_attacksAlwaysHit;

		#endregion

		#region properties

		public bool AttacksAlwaysHit
		{
			get { return m_attacksAlwaysHit; }
			set { m_attacksAlwaysHit = value; }
		}

		public BuckRogers.Hashlist Battles
		{
			get { return this.m_battles; }
			set { this.m_battles = value; }
		}

		public BuckRogers.BattleInfo CurrentBattle
		{
			get { return this.m_currentBattle; }
			set { this.m_currentBattle = value; }
		}

		public System.Collections.ArrayList BattleOrder
		{
			get { return this.m_playerOrder; }
			set { this.m_playerOrder = value; }
		}

		public UnitCollection SurvivingUnits
		{
			get { return this.m_survivingUnits; }
			set { this.m_survivingUnits = value; }
		}

		public BuckRogers.Player CurrentPlayer
		{
			get { return this.m_currentPlayer; }
			set { this.m_currentPlayer = value; }
		}

		public BuckRogers.UnitCollection CurrentUnused
		{
			get { return this.m_currentUnused; }
			set { this.m_currentUnused = value; }
		}

		public UnitCollection CapturedFactories
		{
			get { return m_capturedFactories; }
			set { m_capturedFactories = value; }
		}

		/*
		public BuckRogers.CombatResult LastResult
		{
			get { return this.m_lastResult; }
			set { this.m_lastResult = value; }
		}
		*/

		public BuckRogers.CombatResult TurnResult
		{
			get { return this.m_turnResult; }
			set { this.m_turnResult = value; }
		}

		public BuckRogers.CombatResult BattleResult
		{
			get { return this.m_cumulativeResult; }
			set { this.m_cumulativeResult = value; }
		}

		public BuckRogers.BattleStatus Status
		{
			get { return this.m_status; }
			set { this.m_status = value; }
		}

		public System.Xml.XmlDocument Gamelog
		{
			get { return this.m_gamelog; }
			set { this.m_gamelog = value; }
		}

		#endregion

		#region constructor

		public BattleController(GameController gc)
		{
			m_controller = gc;
			m_cumulativeResult = new CombatResult();

			m_currentBattle = null;
			m_battles = null;
		}

		#endregion

		#region gamelog

		public void InitGameLog()
		{
			// also need to update the m_xeCurrentTurn each turn

			/*
			m_gamelog = m_controller.Gamelog;
			m_xnTurns = m_gamelog.GetElementsByTagName("Turns")[0];
			*/
		}

		public void LogNewTurn()
		{
			/*
			XmlNode lastChild = m_xnTurns.LastChild;
			m_xeCurrentTurn = (XmlElement)lastChild;
			m_xeBattles = m_gamelog.CreateElement("Battles");
			m_xeCurrentTurn.AppendChild(m_xeBattles);
			*/
		}

		#endregion

		#region CheckPlayerOrder
		private void CheckPlayerOrder()
		{
			m_playerOrder = new ArrayList();
			
			
			if(m_currentBattle.Type == BattleType.Bombing
				|| m_currentBattle.Type == BattleType.KillerSatellite)
			{
				//al = m_controller.GetBombingTargets(m_currentBattle.Territory, m_currentBattle.Player).GetPlayersWithUnits();
				m_playerOrder.Add(m_currentBattle.Player);

				return;
			}
			else
			{
				ArrayList al = m_currentBattle.Territory.Units.GetPlayersWithUnits();

				foreach (Player p in m_controller.PlayerOrder)
				{
					if (al.Contains(p))
					{
						UnitCollection uc = m_currentBattle.Territory.Units.GetUnits(p);
						UnitCollection combatUnits = uc.GetCombatUnits();

						if (combatUnits.Count == 0)
						{
							Hashtable ht = uc.GetUnitTypeCount();

							if (ht.ContainsKey(UnitType.Leader))
							{
								Unit leader = uc.GetUnits(UnitType.Leader)[0];
								p.Disabled = true;

								if (StatusUpdate != null)
								{
									StatusUpdateEventArgs suea = new StatusUpdateEventArgs();
									suea.StatusInfo = StatusInfo.LeaderKilled;
									suea.Player = leader.Owner;

									StatusUpdate(this, suea);
								}
							}
						}
						else
						{
							m_playerOrder.Add(p);
							//m_survivingUnits[p] = new UnitCollection();
						}						
					}					
				}
			}

			if (m_playerOrder.Count == 1)
			{
				m_status = BattleStatus.BattleComplete;
				return;
			}
		}

		#endregion

		#region NextBattle
		public bool NextBattle()
		{
			FinalizeCurrentBattle();

			return StartNextBattle();
		}

		public bool FinalizeCurrentBattle()
		{
			bool sabotagedFactories = false;

			CleanUpCasualties();

			if (m_currentBattle != null)
			{
				/*
				XmlAttribute xaNumRolls = m_gamelog.CreateAttribute("rolls");
				xaNumRolls.Value = m_numRolls.ToString();
				m_xeCurrentBattle.Attributes.Append(xaNumRolls);
				*/
			}

			// if this is not the first battle, check to see if the territory changed owners
			if (m_currentBattle != null && m_currentBattle.Territory.Type != TerritoryType.Space)
			{
				sabotagedFactories = CheckForCapturedFactories();
			}

			return sabotagedFactories;
		}

		public void FinalizeCurrentBattle2()
		{
			FinalizeCurrentBattle2(null);
		}

		public void FinalizeCurrentBattle2(Dictionary<Factory, bool> sabotageResults)
		{
			// if this is not the first battle, check to see if the territory changed owners
			if(m_currentBattle != null && m_currentBattle.Territory.Type != TerritoryType.Space)
			{
				UpdateTerritoryOwnership(sabotageResults);

			}

			RemoveUnneededBattles();
		}

		public void RemoveUnneededBattles()
		{
			bool activeBattle = (m_currentBattle != null);
			bool satelliteBattle = false;
			if(activeBattle)
			{
				satelliteBattle = (m_currentBattle.Type == BattleType.KillerSatellite);
			}

			//if(m_currentBattle != null && m_currentBattle.Type == BattleType.KillerSatellite)
			if(satelliteBattle)
			{
				List<BattleInfo> otherBattles = new List<BattleInfo>();

				foreach(BattleInfo bi in m_battles)
				{
					if(bi.Territory == m_currentBattle.Territory)
					{
						otherBattles.Add(bi);
					}
				}

				foreach(BattleInfo bi in otherBattles)
				{
					ArrayList playersLeftInTerritory = bi.Territory.Units.GetPlayersWithUnits();

					if(playersLeftInTerritory.Count == 1)
					{
						m_battles.Remove(bi.ToString());
					}
				}
			}
		}

		public void UpdateTerritoryOwnership(Dictionary<Factory, bool> sabotageResults)
		{
			if(m_currentBattle == null || m_currentBattle.Territory.Type != TerritoryType.Ground)
			{
				return;
			}

			if(sabotageResults != null)
			{
				ArrayList remainingPlayers = m_currentBattle.Territory.Units.GetPlayersWithUnits();

				Player factoryOwner = m_capturedFactories[0].Owner;
				remainingPlayers.Remove(factoryOwner);

				// should only be one remaining player at this point
				Player territoryOwner = (Player)remainingPlayers[0];

				foreach(Factory f in m_capturedFactories)
				{
					bool factoryDestroyed = sabotageResults[f];

					if(factoryDestroyed)
					{
						f.Destroy();
					}
					else
					{
						f.Owner = territoryOwner;
					}
				}

			}


			ArrayList playersLeft = m_currentBattle.Territory.Units.GetPlayersWithUnits();

			if(playersLeft.Count > 1)
			{
				throw new Exception("Shouldn't be more than one player left after a battle");
			}

			if(playersLeft.Count == 1)
			{
				Player p = (Player)playersLeft[0];
				Player owner = m_currentBattle.Territory.Owner;

				if(p != owner)
				{
					m_currentBattle.Territory.Owner = p;

					if(TerritoryOwnerChanged != null)
					{
						TerritoryEventArgs tea = new TerritoryEventArgs();
						tea.Name = m_currentBattle.Territory.Name;
						tea.Owner = p;

						TerritoryOwnerChanged(this, tea);
					}
				}
			}



			if(UpdateTerritory != null)
			{
				StatusUpdateEventArgs suea = new StatusUpdateEventArgs();
				suea.StatusInfo = StatusInfo.UpdateTerritory;
				//suea.Territory = m_currentBattle.Territory;
				suea.Territories.Add(m_currentBattle.Territory);

				EventsHelper.Fire(UpdateTerritory, this, suea);
				//UpdateTerritory(m_currentBattle.Territory);
			}
		}

		public bool CheckForCapturedFactories()
		{
			bool sabotagedFactories = false;

			m_capturedFactories.Clear();

			ArrayList playersLeft = m_currentBattle.Territory.Units.GetPlayersWithUnits();

			// Check for captured factories

			if(playersLeft.Count == 2)
			{
				UnitCollection factories = m_currentBattle.Territory.Units.GetUnits(UnitType.Factory);

				if(factories.Count > 0)
				{
					m_capturedFactories.AddAllUnits(factories);

					StatusUpdateEventArgs suea = new StatusUpdateEventArgs();
					suea.Territories.Add(m_currentBattle.Territory);

					suea.StatusInfo = StatusInfo.FactoryConquered;
					suea.Units.AddAllUnits(factories);

					EventsHelper.Fire(StatusUpdate, this, suea);

					sabotagedFactories = suea.Result;
				}
			}

			return sabotagedFactories;
		}

		public Dictionary<Factory, bool> SabotageCapturedFactories()
		{
			Dictionary<Factory, bool> sabotageResults = new Dictionary<Factory, bool>();

			foreach(Factory f in m_capturedFactories)
			{
				int roll = Utility.RollD10();
				bool factoryDestroyed = (roll >= 7);

				sabotageResults[f] = factoryDestroyed;
			}

			return sabotageResults;
		}
		

		public void CleanUpCasualties()
		{
			if(m_cumulativeResult.Casualties.Count > 0)
			{
				if(CurrentBattle.Type == BattleType.Bombing)
				{
					if(TerritoryUnitsChanged != null)
					{
						ArrayList hurtPlayers = m_cumulativeResult.Casualties.GetPlayersWithUnits();

						foreach(Player p in hurtPlayers)
						{
							UnitCollection playerUnits = m_cumulativeResult.Casualties.GetUnits(p);
							ArrayList territories = playerUnits.GetUnitTerritories();

							foreach(Territory t in territories)
							{
								UnitCollection territoryUnits = playerUnits.GetUnits(t);

								TerritoryUnitsEventArgs tuea = new TerritoryUnitsEventArgs();
								tuea.Units = new UnitCollection();
								tuea.Units.AddAllUnits(playerUnits);
								tuea.Territory = t;
								tuea.Added = false;
								tuea.Player = p;

								foreach(Unit u in territoryUnits)
								{
									u.CurrentTerritory = Territory.NONE;
								}

								TerritoryUnitsChanged(this, tuea);
							}
						}
					}
				}
				else
				{
					foreach(Unit u in m_cumulativeResult.Casualties)
					{
						u.CurrentTerritory = Territory.NONE;
					}

					// FIXME This doesn't work right with bombing!

					if(TerritoryUnitsChanged != null)
					{
						TerritoryUnitsEventArgs tuea = new TerritoryUnitsEventArgs();
						tuea.Units = new UnitCollection();
						tuea.Units.AddAllUnits(m_cumulativeResult.Casualties);
						tuea.Territory = m_currentBattle.Territory;
						tuea.Added = false;
						tuea.Player = m_currentBattle.Player;

						TerritoryUnitsChanged(this, tuea);
					}
				}

				//XmlElement xeUnits = m_gamelog.CreateElement("Casualties");

				ArrayList players = m_cumulativeResult.Casualties.GetPlayersWithUnits();

				foreach(Player p in players)
				{
					UnitCollection playerCasualties = m_cumulativeResult.Casualties.GetUnits(p);

					/*
					XmlElement xePlayer = m_gamelog.CreateElement("Player");
					XmlAttribute xaPlayerName = m_gamelog.CreateAttribute("name");
					xaPlayerName.Value = p.Name;
					xePlayer.Attributes.Append(xaPlayerName);
					*/

					foreach(Unit u in playerCasualties)
					{
						/*
						XmlElement xeUnit = m_gamelog.CreateElement("Unit");
						XmlAttribute xaType = m_gamelog.CreateAttribute("type");
						XmlAttribute xaUnitID = m_gamelog.CreateAttribute("id");
						xaType.Value = u.Type.ToString();
						xaUnitID.Value = u.ID.ToString();
						
						xeUnit.Attributes.Append(xaType);
						xeUnit.Attributes.Append(xaUnitID);
						
						xePlayer.AppendChild(xeUnit);
						*/

						u.Destroy();
					}

					//xeUnits.AppendChild(xePlayer);
				}

				//m_xeCurrentBattle.AppendChild(xeUnits);
			}
		}

		public bool StartNextBattle()
		{
			bool startedNextBattle = false;

			if (m_battles != null && m_battles.Count > 0)
			{
				m_currentBattle = (BattleInfo)m_battles[0];
				m_battles.Remove(m_currentBattle.ToString());

				m_cumulativeResult = new CombatResult();
				m_turnResult = new CombatResult();
				m_survivingUnits = new UnitCollection();

				/*
				m_xeCurrentBattle = m_gamelog.CreateElement("Battle");
				m_xeBattles.AppendChild(m_xeCurrentBattle);

				XmlAttribute xaBattleType = m_gamelog.CreateAttribute("type");
				xaBattleType.Value = m_currentBattle.Type.ToString();
				XmlAttribute xaLocation = m_gamelog.CreateAttribute("territory");
				xaLocation.Value = m_currentBattle.Territory.Name;
				m_xeCurrentBattle.Attributes.Append(xaBattleType);
				m_xeCurrentBattle.Attributes.Append(xaLocation);
				*/

				m_status = BattleStatus.Setup;

				CheckPlayerOrder();

				//if(m_playerOrder.Count == 1 || m_status == BattleStatus.BattleComplete)

				// If the status was reset during CheckPlayerOrder, then it was probably a 
				// KillerSatellite battle where all the attacking forces were destroyed.
				// In that case, we need to remove the current battle and start the process over.
				if (m_status == BattleStatus.BattleComplete)
				{
					startedNextBattle = true;
				}
				else
				{
					m_currentPlayer = (Player)m_playerOrder[0];
					m_currentUnused = new UnitCollection();

					Territory t = m_currentBattle.Territory;
					m_numRolls = 0;

					switch (m_currentBattle.Type)
					{
						case BattleType.Normal:
						{
							UnitCollection combatUnits = t.Units.GetCombatUnits();
							m_survivingUnits.AddAllUnits(combatUnits);

							UpdateUnusedUnits();
							DisplayUnitsByTerritory(m_survivingUnits);

							UnitCollection nonCombatUnits = new UnitCollection();
							UnitCollection leaders = t.Units.GetUnits(UnitType.Leader);
							UnitCollection factories = t.Units.GetUnits(UnitType.Factory);

							nonCombatUnits.AddAllUnits(leaders);
							nonCombatUnits.AddAllUnits(factories);

							DisplayUnitsByTerritory(nonCombatUnits, DisplayCategory.NonCombatUnits);

							break;
						}
						case BattleType.KillerSatellite:
						{
							UnitCollection satellites = t.Units.GetUnits(UnitType.KillerSatellite);
							m_currentUnused.AddAllUnits(satellites);
							UnitCollection defenders = t.Units.GetNonMatchingUnits(m_currentBattle.Player);

							m_survivingUnits.AddAllUnits(satellites);
							m_survivingUnits.AddAllUnits(defenders);

							DisplayUnitsByTerritory(satellites);
							DisplayUnitsByTerritory(defenders);

							break;
						}
						case BattleType.Bombing:
						{
							UnitCollection attackers = t.Units.GetUnits(UnitType.Battler, m_currentPlayer, null);//).GetUnits(UnitType.Battler);
							UnitCollection defenders = m_controller.GetBombingTargets(t, m_currentPlayer);

							m_currentUnused.AddAllUnits(attackers);

							m_survivingUnits.AddAllUnits(attackers);
							m_survivingUnits.AddAllUnits(defenders);

							DisplayUnitsByTerritory(attackers);
							DisplayUnitsByTerritory(defenders);

							break;
						}

					}

					m_status = BattleStatus.BattleReady;

					if (BattleStatusUpdated != null)
					{
						StatusUpdateEventArgs suea = new StatusUpdateEventArgs();
						suea.StatusInfo = StatusInfo.BattleStatusUpdated;
						suea.BattleStatus = m_status;

						EventsHelper.Fire(BattleStatusUpdated, this, suea);
					}

					startedNextBattle = true;
				}
			}

			if (!startedNextBattle)
			{
				m_currentBattle = null;
			}

			return startedNextBattle;
		}

		

		#endregion

		#region round functions

		public void ProcessAttackResults(CombatResult cr)
		{
			TurnResult.Casualties.AddAllUnits(cr.Casualties);

			m_survivingUnits.RemoveAllUnits(cr.Casualties);

			CurrentUnused.AddAllUnits(cr.UnusedAttackers);
			CurrentUnused.RemoveAllUnits(cr.UsedAttackers);

			StatusUpdateEventArgs suea = new StatusUpdateEventArgs();
			suea.StatusInfo = StatusInfo.BattleStatusUpdated;
			suea.BattleStatus = BattleStatus.AttackComplete;
			suea.CombatResult = cr;

			EventsHelper.Fire(BattleStatusUpdated, this, suea);


			UnitCollection nonPlayerSurvivors = m_survivingUnits.GetNonMatchingUnits(m_currentPlayer);

			if(nonPlayerSurvivors.Count == 0)
			{
				if(m_playerOrder.IndexOf(m_currentPlayer) == m_playerOrder.Count - 1)
				{
					ProcessTurnResults();
					m_status = BattleStatus.BattleComplete;
				}
				else
				{
					m_status = BattleStatus.PlayerTurnFinished;
				}
			}
			else if(CurrentUnused.Count == 0)
			{
				m_status = BattleStatus.PlayerTurnFinished;
			}
			else
			{
				m_status = BattleStatus.AttackComplete;
			}

			if(m_status != BattleStatus.AttackComplete)
			{
				suea.StatusInfo = StatusInfo.BattleStatusUpdated;
				suea.BattleStatus = m_status;
				suea.CombatResult = cr;

				EventsHelper.Fire(BattleStatusUpdated, this, suea);
			}
		}

		public bool NextPlayer()
		{
			bool anotherPlayer = false;
			switch(CurrentBattle.Type)
			{
				// only one turn / player for these types
				case BattleType.KillerSatellite:
				case BattleType.Bombing:
				{
					NextTurn();
					m_status = BattleStatus.BattleComplete;
					break;
				}
				case BattleType.Normal:
				{
					int index = BattleOrder.IndexOf(m_currentPlayer);

					bool anotherTurn = false;

					if( (index == (BattleOrder.Count - 1)))
					{
						anotherTurn = NextTurn();
						if(anotherTurn)
						{
							m_currentPlayer = (Player)BattleOrder[0];
							UpdateUnusedUnits();
							DisplayUnitsByTerritory(m_survivingUnits);
							m_status = BattleStatus.Setup;
							anotherPlayer = true;
						}
					}
					else
					{
						m_currentPlayer = (Player)BattleOrder[index + 1];
						UpdateUnusedUnits();
						m_status = BattleStatus.Setup;
						anotherPlayer = true;
					}

					break;
				}

			}

			return anotherPlayer;
		}

		public void NextRound()
		{
			m_status = BattleStatus.Setup;
		}

		#endregion

		#region display functions

		public void DisplayUnits()
		{
			if(UnitsToDisplay != null)
			{
				DisplayUnitsEventArgs duea = new DisplayUnitsEventArgs();
				duea.Category = DisplayCategory.UnusedAttackers;
				duea.Units = m_currentUnused;

				UnitsToDisplay(this, duea);
			}

			DisplaySurvivingEnemies();		
		}

		public void DisplayUnitsByTerritory(UnitCollection uc)
		{
			DisplayUnitsByTerritory(uc, DisplayCategory.UnusedAttackers);
		}

		public void DisplayUnitsByTerritory(UnitCollection uc, DisplayCategory category)
		{
			ArrayList players = uc.GetPlayersWithUnits();
			
			foreach(Player p in m_controller.PlayerOrder)//players)
			{
				UnitCollection playerUnits = uc.GetUnits(p);
				if(playerUnits.Count == 0)
				{
					continue;
				}

				ArrayList playerTerritories = uc.GetUnitTerritories();

				foreach(Territory t in playerTerritories)
				{
					UnitCollection territoryUnits = playerUnits.GetUnits(t);

					if(territoryUnits.Count == 0)
					{
						continue;
					}

					if (UnitsToDisplay != null)
					{
						DisplayUnitsEventArgs duea = new DisplayUnitsEventArgs();
						duea.Territory = t;
						duea.Category = category;
						duea.Units = territoryUnits;
						duea.Player = p;
						

						UnitsToDisplay(this, duea);
					}
				}
			}
			
		}

		public void DisplaySurvivingEnemies()
		{
			UnitCollection enemySurvivors = m_survivingUnits.GetNonMatchingUnits(m_currentPlayer);

			if(UnitsToDisplay != null)
			{
				DisplayUnitsEventArgs duea = new DisplayUnitsEventArgs();
				duea.Category = DisplayCategory.SurvivingDefenders;
				duea.Units = enemySurvivors;
					
				UnitsToDisplay(this, duea);
			}

		}

		public void UpdateUnusedUnits()
		{
			m_currentUnused.Clear();
			UnitCollection survivors = m_survivingUnits.GetUnits(m_currentPlayer);//(UnitCollection)m_survivingUnits[m_currentPlayer];

			// if there's anything here, it's been killed this turn and can still shoot
			UnitCollection casualties = m_turnResult.Casualties.GetUnits(m_currentPlayer);

			m_currentUnused.AddAllUnits(survivors);
			m_currentUnused.AddAllUnits(casualties);
		}

		#endregion

		#region end of turn functions

		public void ProcessTurnResults()
		{
			foreach(Unit u in m_turnResult.Casualties)
			{
				m_cumulativeResult.Casualties.AddUnit(u);
			}

			m_currentUnused.RemoveAllUnits(m_turnResult.Casualties);

			Territory t = m_currentBattle.Territory;
			ArrayList playersToRemove = new ArrayList();

			foreach(Player p in m_playerOrder)
			{
				UnitCollection uc = m_survivingUnits.GetUnits(p);//(UnitCollection)m_survivingUnits[p];

				if(uc.Count == 0)
				{
					playersToRemove.Add(p);
				}				
			}

			foreach(Player p in playersToRemove)
			{
				UnitCollection leaders = t.Units.GetUnits(UnitType.Leader, p, null);

				// TODO Actually disable the player for a turn
				if( (leaders.Count > 0) && (t.Units.GetNonMatchingUnits(p).Count > 0))
				{
					Unit leader = leaders[0];
					m_cumulativeResult.Casualties.AddUnit(leader);
					p.Disabled = true;
					p.TurnDisabled = m_controller.TurnNumber;
					//leader.Destroy();

					if(StatusUpdate != null)
					{
						StatusUpdateEventArgs suea = new StatusUpdateEventArgs();
						suea.StatusInfo = StatusInfo.LeaderKilled;
						suea.Player = leader.Owner;

						StatusUpdate(this, suea);
					}
				}



				m_playerOrder.Remove(p);
			}
		}

		
		// Returns true if there is another turn after this one
		public bool NextTurn()
		{
			ProcessTurnResults();

			DisplayUnitsByTerritory(m_cumulativeResult.Casualties, DisplayCategory.DeadUnits);

			bool anotherTurn = true;
			if( (m_currentBattle.Type == BattleType.KillerSatellite)
				|| (m_currentBattle.Type == BattleType.Bombing))
			{
				m_playerOrder.Clear();
				anotherTurn = false;
			}
			else
			{
				// TODO Could just clear out everything instead...
				m_turnResult = new CombatResult();

				// only one player left?
				anotherTurn = (m_playerOrder.Count > 1);
			}

			if(anotherTurn)
			{
				m_status = BattleStatus.PlayerTurnFinished;
			}
			else
			{
				m_status = BattleStatus.BattleComplete;
			}

			return anotherTurn;
			
		}

		public void CombatComplete()
		{
			m_cumulativeResult.Casualties.Clear();

			m_currentBattle = null;
		}

		#endregion

		#region Combat functions

		public CombatResult DoCombat(CombatInfo ci)
		{
			return ExecuteCombat(ci);

			/*
			StatusUpdateEventArgs suea = new StatusUpdateEventArgs();
			suea.StatusInfo = StatusInfo.BattleStatusUpdated;
			suea.BattleStatus = BattleStatus.AttackComplete;
			suea.CombatResult = cr;

			EventsHelper.Fire(BattleStatusUpdated, this, suea);
			*/
		}

		private CombatResult ExecuteCombat(CombatInfo ci)
		{
			CombatResult cr = new CombatResult();

			// it's basically a while loop - no incrementing of i, since this
			// needs to go until all attackers are done or all defenders are destroyed, 
			// and one unit will be removed from ci.Attackers each time through
			while(ci.Attackers.Count > 0)
			{
				Unit attacker = (Unit)ci.Attackers[0];
				Unit defender = (Unit)ci.Defenders[0];

				int toHit = NOTPOSSIBLE;
				if(!(ci.Type == BattleType.Bombing))
				{
					toHit = m_combatTable[(int)attacker.Type, (int)defender.Type];
				}
				else
				{
					toHit = 7;
				}
				
				if(toHit == NOTPOSSIBLE)
				{
					throw new Exception("Can't attack a " + defender.Type + " with a " + attacker.Type);
				}

				int roll = Utility.RollD10();
				m_numRolls++;

				if(ci.AttackingLeader)
				{
					roll += 2;
				}

				if(roll > 10)
				{
					roll = 10;
				}

				bool attackHit = (roll >= toHit);

				if(m_attacksAlwaysHit)
				{
					attackHit = true;
				}

				AttackResult ar = new AttackResult();
				ar.Attacker = attacker;
				ar.Defender = defender;
				ar.Roll = roll;
				ar.Hit = attackHit;
				ar.Leader = ci.AttackingLeader;

				cr.AttackResults.Add(ar);

				if(ci.Type == BattleType.KillerSatellite)
				{
					ci.Defenders.RemoveUnit(defender);

					if(attackHit)
					{
						cr.Casualties.AddUnit(defender);
					}
					else
					{
						cr.Survivors.AddUnit(defender);
					}

					if(ci.Defenders.Count == 0)
					{
						ci.Attackers.RemoveUnit(attacker);

						if (m_currentUnused.ContainsUnit(attacker))
						{
							m_currentUnused.RemoveUnit(attacker);
						}

						cr.UsedAttackers.AddUnit(attacker);
					}
				}
				else
				{
					ci.Attackers.RemoveUnit(attacker);

					if (m_currentUnused.ContainsUnit(attacker))
					{
						m_currentUnused.RemoveUnit(attacker);
					}

					cr.UsedAttackers.AddUnit(attacker);

					if (attackHit)
					{
						ci.Defenders.RemoveUnit(defender);
						cr.Casualties.AddUnit(defender);

						if (ci.Defenders.Count == 0)
						{
							break;
						}
					}
				}				
			}

			cr.Survivors.AddAllUnits(ci.Defenders);
			cr.UnusedAttackers.AddAllUnits(ci.Attackers);
			
			
			m_status = BattleStatus.AttackComplete;
			return cr;
		}
		#endregion

	}
}
