using System;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for BattleController.
	/// </summary>
	public class BattleController
	{
		private int[,] m_combatTable = new int[,]	{	{6, 8, 7, NOTPOSSIBLE, 6, NOTPOSSIBLE, 3}, // Trooper
														{5, 6, 6, NOTPOSSIBLE, 5, NOTPOSSIBLE, 2}, // Gennie
														{7, 7, 6, 8, 3, 7, 3}, // Fighter
														{7, 7, 4, 6, 4, 6, NOTPOSSIBLE}, // Battler
														{9, 10, 8, 10, 6, 10, 9}, // Transport
														{NOTPOSSIBLE, NOTPOSSIBLE, 6, 7, 5, NOTPOSSIBLE, NOTPOSSIBLE}, // Killer Satellite
														{8, 9, 9, NOTPOSSIBLE, 7, NOTPOSSIBLE, NOTPOSSIBLE}, // Control marker
													};

		private const int NOTPOSSIBLE = 99;

		public BattleController()
		{
			//
			// TODO: Add constructor logic here
			//
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

				// TODO Maybe raise an event here that can pass the attack details back to the interface?

				
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
	}
}
