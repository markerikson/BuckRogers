using System;
using System.Collections;

namespace BuckRogers
{

	public delegate void DisplayUnitsHandler(object sender, DisplayUnitsEventArgs e);

	


	/// <summary>
	/// Summary description for BattleController.
	/// </summary>
	public class BattleController
	{
		public event DisplayUnitsHandler UnitsToDisplay;
		
		private int[,] m_combatTable = new int[,]	{	{6, 8, 7, NOTPOSSIBLE, 6, NOTPOSSIBLE, 3}, // Trooper
														{5, 6, 6, NOTPOSSIBLE, 5, NOTPOSSIBLE, 2}, // Gennie
														{7, 7, 6, 8, 3, 7, 3}, // Fighter
														{7, 7, 4, 6, 4, 6, NOTPOSSIBLE}, // Battler
														{9, 10, 8, 10, 6, 10, 9}, // Transport
														{NOTPOSSIBLE, NOTPOSSIBLE, 6, 7, 5, NOTPOSSIBLE, NOTPOSSIBLE}, // Killer Satellite
														{8, 9, 9, NOTPOSSIBLE, 7, NOTPOSSIBLE, NOTPOSSIBLE}, // Control marker
													};

		private const int NOTPOSSIBLE = 99;
		private Hashlist m_battles;
		private GameController m_controller;
		private ArrayList m_playerOrder;
		private BattleInfo m_currentBattle;
		private Hashtable m_survivingUnits;
		private Player m_currentPlayer;
		private UnitCollection m_currentUnused;
		private CombatResult m_turnResult;
		private CombatResult m_cumulativeResult;
		private CombatResult m_lastResult;

		public BattleController(GameController gc)
		{
			m_controller = gc;
		}

		private void CheckPlayerOrder()
		{
			m_playerOrder = new ArrayList();
			ArrayList al;
			
			if(m_currentBattle.Type == BattleType.Bombing)
			{
				al = m_controller.GetBombingTargets(m_currentBattle.Territory, m_currentBattle.Player).GetPlayersWithUnits();
				al.Add(m_currentBattle.Player);
			}
			else
			{
				al = m_currentBattle.Territory.Units.GetPlayersWithUnits();
			}

			foreach(Player p in m_controller.PlayerOrder)
			{
				if(al.Contains(p))
				{
					m_playerOrder.Add(p);
					m_survivingUnits[p] = new UnitCollection();
				}
			}

			switch(m_currentBattle.Type)
			{
				case BattleType.KillerSatellite:
				case BattleType.Bombing:
				{
					Player p = m_currentBattle.Player;
					m_playerOrder.Remove(p);
					m_playerOrder.Insert(0, p);
					break;
				}
			}
		}

		public bool NextBattle()
		{
			if(m_battles != null && m_battles.Count > 0)
			{
				m_currentBattle = (BattleInfo)m_battles[0];
				m_battles.Remove(m_currentBattle.ToString());

				/*
				m_labBattleType.Text = m_currentBattle.Type.ToString();				
				m_labLocation.Text = m_currentBattle.Territory.Name;
				m_labBattlesLeft.Text = m_battleController.Battles.Count.ToString();

				m_btnContinue.Enabled = false;
				m_btnNextPlayer.Enabled = false;
				m_btnNextBattle.Enabled = false;
				m_btnAttack.Enabled = false;

				m_lvAttUsed.Items.Clear();
				m_lvAttUnused.Items.Clear();
				m_lvAttackers.Items.Clear();
				m_lvDefenders.Items.Clear();
				m_lvEnemyLive.Items.Clear();
				m_lvEnemyDead.Items.Clear();
				*/

				m_cumulativeResult = new CombatResult();
				m_turnResult = new CombatResult();				
				m_survivingUnits = new Hashtable();

				CheckPlayerOrder();

				/*
				m_lbCurrentPlayer.Items.Clear();

				foreach(Player p in m_playerOrder)
				{
					m_lbCurrentPlayer.Items.Add(p.Name);
				}
				*/

				m_currentPlayer = (Player)m_playerOrder[0];
				//m_lbCurrentPlayer.SelectedIndex = 0;
				//m_labCurrentPlayer.Text = m_currentPlayer.Name;
				m_currentUnused = new UnitCollection();
				
				Territory t = m_currentBattle.Territory;

				switch(m_currentBattle.Type)
				{
					case BattleType.KillerSatellite:
					{
						UnitCollection satellites = t.Units.GetUnits(UnitType.KillerSatellite);
						m_currentUnused.AddAllUnits(satellites);
						UnitCollection defenders = t.Units.GetNonMatchingUnits(m_currentBattle.Player);

						UnitCollection uc = null;

						uc = (UnitCollection)m_survivingUnits[m_currentPlayer];
						uc.AddAllUnits(satellites);

						foreach(Player p in defenders.GetPlayersWithUnits())
						{
							uc = (UnitCollection)m_survivingUnits[p];
							uc.AddAllUnits(defenders.GetUnits(p));
						}

						if(UnitsToDisplay != null)
						{
							//AddUnitsToListView(satellites, m_lvAttackers, false);
							//AddUnitsToListView(defenders, m_lvDefenders, true);
							DisplayUnitsEventArgs duea = new DisplayUnitsEventArgs();
							duea.Category = DisplayCategory.Attackers;
							duea.Units = satellites;
							UnitsToDisplay(this, duea);

							duea = new DisplayUnitsEventArgs();
							duea.Category = DisplayCategory.Defenders;
							duea.Units = defenders;
							UnitsToDisplay(this, duea);
						}
						

						//EnableAttack();

						break;
					}
					case BattleType.Bombing:
					{
						UnitCollection attackers = t.Units.GetUnits(UnitType.Battler, m_currentPlayer, null);//).GetUnits(UnitType.Battler);
						UnitCollection defenders = m_controller.GetBombingTargets(t, m_currentPlayer);

						m_currentUnused.AddAllUnits(attackers);

						UnitCollection uc = null;

						uc = (UnitCollection)m_survivingUnits[m_currentPlayer];
						uc.AddAllUnits(attackers);

						foreach(Player p in defenders.GetPlayersWithUnits())
						{
							uc = (UnitCollection)m_survivingUnits[p];
							UnitCollection playerUnits = defenders.GetUnits(p);
							uc.AddAllUnits(playerUnits);
						}

						/*
						AddUnitsToListView(attackers, m_lvAttUnused, false);
						AddUnitsToListView(defenders, m_lvEnemyLive, true);
						*/
						if(UnitsToDisplay != null)
						{
							DisplayUnitsEventArgs duea = new DisplayUnitsEventArgs();
							duea.Category = DisplayCategory.UnusedAttackers;
							duea.Units = attackers;
							UnitsToDisplay(this, duea);

							duea = new DisplayUnitsEventArgs();
							duea.Category = DisplayCategory.SurvivingDefenders;
							duea.Units = defenders;
							UnitsToDisplay(this, duea);
						}
						break;
					}
					case BattleType.Normal:
					{
						foreach(Player p in m_playerOrder)
						{
							UnitCollection uc = (UnitCollection)m_survivingUnits[p];
							UnitCollection playerUnits = t.Units.GetUnits(p);
							uc.AddAllUnits(playerUnits.GetCombatUnits());
						}

						UpdateUnusedUnits();
						DisplayUnits();
						break;
					}
				}
				return true;
			}

			return false;
		}

		public void DisplayUnits()
		{
			//m_lvAttUnused.Items.Clear();
			//m_lvEnemyLive.Items.Clear();

			//AddUnitsToListView(m_currentUnused, m_lvAttUnused, false);

			if(UnitsToDisplay != null)
			{
				DisplayUnitsEventArgs duea = new DisplayUnitsEventArgs();
				duea.Category = DisplayCategory.UnusedAttackers;
				duea.Units = m_currentUnused;

				UnitsToDisplay(this, duea);
			}

			DisplaySurvivingEnemies();			
		}

		public void DisplaySurvivingEnemies()
		{
			foreach(Player p in m_playerOrder)
			{
				if(p == m_currentPlayer)
				{
					continue;
				}

				UnitCollection enemySurvivors = (UnitCollection)m_survivingUnits[p];


				//AddUnitsToListView(enemySurvivors, m_lvEnemyLive, true);
				if(UnitsToDisplay != null)
				{
					DisplayUnitsEventArgs duea = new DisplayUnitsEventArgs();
					duea.Category = DisplayCategory.SurvivingDefenders;
					duea.Units = enemySurvivors;
					
					UnitsToDisplay(this, duea);
				}
			}
		}

		public void UpdateUnusedUnits()
		{
			m_currentUnused.Clear();
			UnitCollection survivors = (UnitCollection)m_survivingUnits[m_currentPlayer];

			// if there's anything here, it's been killed this turn and can still shoot
			UnitCollection casualties = m_turnResult.Casualties.GetUnits(m_currentPlayer);

			m_currentUnused.AddAllUnits(survivors);
			m_currentUnused.AddAllUnits(casualties);

			//m_labCurrentPlayer.Text = m_currentPlayer.Name;
			//m_lbCurrentPlayer.SelectedIndex = m_playerOrder.IndexOf(m_currentPlayer);
		}

		
		// Returns true if there is another turn after this one
		public bool NextTurn()
		{
			foreach(Unit u in m_turnResult.Casualties)
			{
				u.Destroy();
			}

			m_currentUnused.RemoveAllUnits(m_turnResult.Casualties);

			if( (m_currentBattle.Type == BattleType.KillerSatellite)
				|| (m_currentBattle.Type == BattleType.Bombing))
			{
				m_playerOrder.Clear();
				return false;
			}
			else
			{
				Territory t = m_currentBattle.Territory;
				ArrayList playersToRemove = new ArrayList();
				foreach(Player p in m_playerOrder)
				{
					UnitCollection uc = (UnitCollection)m_survivingUnits[p];

					if(uc.Count == 0)
					{
						playersToRemove.Add(p);
					}				
				}

				foreach(Player p in playersToRemove)
				{
					m_playerOrder.Remove(p);
				}

				// TODO Could just clear out everything instead...
				m_turnResult = new CombatResult();

				// only one player left?
				return (m_playerOrder.Count > 1);
			}
			
		}


		#region Combat functions

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

				int roll = Utility.RollD10();

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
			}

			cr.UnusedAttackers.AddAllUnits(ci.Attackers);
			cr.Survivors.AddAllUnits(ci.Defenders);
			
			return cr;
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

		#endregion


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

		public System.Collections.Hashtable SurvivingUnits
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

		public BuckRogers.CombatResult LastResult
		{
			get { return this.m_lastResult; }
			set { this.m_lastResult = value; }
		}

		public BuckRogers.CombatResult TurnResult
		{
			get { return this.m_turnResult; }
			set { this.m_turnResult = value; }
		}
	}
}
