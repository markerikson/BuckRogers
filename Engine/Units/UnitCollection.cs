using System;
using System.Collections;

namespace BuckRogers
{
	public class UnitCollection : CollectionBase//IList
	{
		//private ArrayList List;

		/** Creates new UnitCollection */
		public UnitCollection()
		{
			//List = new ArrayList();
		}

		public int AddUnit(Unit unit)
		{
			if(!List.Contains(unit))
			{
				return List.Add(unit);
			}
			
			////m_holder.notifyChanged();
			return -1;
		}

		public void AddAllUnits(UnitCollection collection)
		{
			//List.AddRange(collection.Units);
			for(int i = 0; i < collection.Count; i++)
			{
				Unit u = collection[i];
				if(!List.Contains(u))
				{
				List.Add(u);
				}
				
			}
			//m_holder.notifyChanged();
		}

		public void RemoveUnit(Unit unit)
		{
			List.Remove(unit);
		}

		public void RemoveAllUnits(UnitCollection units)
		{
			//List.removeAll(units);
			foreach(Unit unit in units.Units)
			{
				if(List.Contains(unit))
				{
					List.Remove(unit);
				}
				
			}
			//m_holder.notifyChanged();
		}
/*
		public int Count
		{
			get
			{
				return List.Count;
				
			}
		}
	*/ 

		public Unit this[int idx]
		{
			get
			{
				return (Unit)List[idx];
			}
			
		}

		public int NumUnits(UnitType ut, Player p, Territory t)
		{
			//UnitCollection uc = new UnitCollection();
			int count = 0;

			/*
			if (count == 0)
			{
				return uc;
			}
			 */

			for(int i = 0; i < List.Count; i++)
			{
				Unit current = (Unit)List[i];

				bool countUnit = false;

				if(current.Type == ut)
				{
					countUnit = true;
				}

				if(p != null && current.Owner == p)
				{
					countUnit = true;
				}

				if(t != null && current.CurrentTerritory == t)
				{
					countUnit = true;
				}

				if(countUnit)
				{
					count++;
				}

				/*
				if (uc.Count == count)
				{
					return uc;
				}
				 */
			}

			return count;
		}

		
		public int NumUnits(UnitType type)
		{
			return NumUnits(type, null, null);
		}

		public int NumUnits(Player p)
		{
			return NumUnits(UnitType.None, p, null);
		}

		public int NumUnits(Territory t)
		{
			return NumUnits(UnitType.None, null, t);
		}
		

		/*
		public boolean containsAll(Collection units)
		{
			return List.containsAll(units);
		}
		*/

		public bool ContainsUnit(Unit unit)
		{
			bool contains = false;

			for(int i = 0; i < List.Count; i++)
			{
				Unit u = (Unit)List[i];
				if(u == unit)
				{
					contains = true;
					break;
				}
			}

			return contains;
		}

		public Unit GetUnitByID(int id)
		{
			for(int i = 0; i < List.Count; i++)
			{
				Unit u = (Unit)List[i];
				if(u.ID == id)
				{
					return u;
				}
			}
			
			return null;
		}

		public UnitCollection GetUnits(int max)
		{
			return GetUnits(UnitType.None, null, null, max);
		}

		public UnitCollection GetUnits(UnitType ut, Player p, Territory t)
		{
			return GetUnits(ut, p, t, List.Count);
		}

		public UnitCollection GetUnits(UnitType ut, Player p, Territory t, int max)
		{
			UnitCollection uc = new UnitCollection();

			int numAdded = 0;
			if(List.Count < max)
			{
				max = List.Count;
			}
			for (int i = 0; i < List.Count && numAdded < max; i++)
			{
				Unit current = (Unit)List[i];

				bool addUnit = true;
				bool matchType = true;
				bool matchOwner = true;
				bool matchTerritory = true;

				
				if (ut != UnitType.None && current.Type != ut)
				{
					matchType = false;
				}

				if (p != null && current.Owner != p)
				{
					matchOwner = false;
				}

				if (t != null && current.CurrentTerritory != t)
				{
					matchTerritory = false;
				}

				addUnit = (matchType && matchOwner && matchTerritory);
				

				if (addUnit)
				{
					uc.AddUnit(current);
					numAdded++;
				}
			}

			return uc;
		}

		public UnitCollection GetUnits(Territory t)
		{
			return GetUnits(UnitType.None, null, t, List.Count);
		}

		public UnitCollection GetUnits(UnitType type)
		{
			return GetUnits(type, null, null, List.Count);
		}

		public UnitCollection GetUnits(Player p)
		{
			return GetUnits(UnitType.None, p, null, List.Count);
		}

		public UnitCollection GetUnits(Territory t, int max)
		{
			return GetUnits(UnitType.None, null, t, max);
		}

		public UnitCollection GetUnits(UnitType type, int max)
		{
			return GetUnits(type, null, null, max);
		}

		public UnitCollection GetUnits(Player p, int max)
		{
			return GetUnits(UnitType.None, p, null, max);
		}

		public UnitCollection GetCombatUnits()
		{
			UnitCollection uc = new UnitCollection();
			foreach(Unit u in List)
			{
				switch(u.Type)
				{
					case UnitType.Trooper:
					case UnitType.Gennie:
					case UnitType.Fighter:
					case UnitType.Battler:
					case UnitType.Transport:
					case UnitType.KillerSatellite:
					{
						uc.AddUnit(u);
						break;
					}
				}
			}

			return uc;
		}

		public UnitCollection GetUnitsWithMoves(int numMoves)
		{
			UnitCollection uc = new UnitCollection();

			foreach(Unit u in List)
			{
				if(u.MovesLeft == numMoves)
				{
					uc.AddUnit(u);
				}
			}

			return uc;
		}

		public UnitCollection GetUnitsWithMinMoves(int numMoves)
		{
			UnitCollection uc = new UnitCollection();

			foreach (Unit u in List)
			{
				if (u.MovesLeft >= numMoves)
				{
					uc.AddUnit(u);
				}
			}

			return uc;
		}

		/**
		* Returns a map of UnitType -> int.
		*/
		public Hashtable GetUnitTypeCount()
		{
			Hashtable units = new Hashtable();
			//Iterator iter = getData().getUnitTypeList().iterator();
			//while(iter.hasNext() )
			foreach(UnitType type in Enum.GetValues(typeof(UnitType)))
			{
				int count = NumUnits(type);
				if(count > 0)
				{
					units[type] = count;
				}
			}
			return units;
		}

		/**
		* Returns a map of UnitType -> int.
		* Only returns units for the specified player
		*/
		public Hashtable GetUnitTypeCount(Player id)
		{
			Hashtable count = new Hashtable();

			foreach(UnitType ut in Enum.GetValues(typeof(UnitType)))
			{
				count[ut] = 0;
			}

			//Iterator iter = List.iterator();
			//while(iter.hasNext() )
			foreach(Unit unit in List)
			{
				if(unit.Owner == id)
				{
					UnitType ut = unit.Type;
					int num = (int)count[ut];
					num++;
					count[ut] = num;

					/*
					int num = 1;
					if(!count.ContainsKey(ut))
					{
						count[ut] = num;
					}
					else
					{
						num = (int)count[ut];
						num++;
						count[ut] = num;
					}
					*/
				}					
			}

			
			return count;
		}


		/**
		* Passed a map of UnitType -> int
		* return a collection of units of each type up to max
		*/
		/*
		public Hashtable getUnits(Hashtable types)
		{
			Collection units = new ArrayList();
			Iterator iter = types.keySet().iterator();
			while(iter.hasNext() )
			{
				UnitType type = (UnitType) iter.next();
				units.addAll(getUnits(type, types.getInt(type)));
			}
			return units;
		}
		*/

		
		public bool Empty
		{
			get
			{
				return List.Count == 0;
			}
			
		}
		 

		public ArrayList Units
		{
			get
			{
				return new ArrayList(List);
			}			
		}

		/*
		public UnitCollection GetNonMatchingUnits(UnitType ut, Player p, Territory t, int max)
		{
			UnitCollection uc = new UnitCollection();

			foreach(Unit u in List)
			{
				if(u.Owner != player)
				{
					uc.AddUnit(u);
				}
			}
			
			return uc;
		}
		*/

		
		public UnitCollection GetNonMatchingUnits(UnitType ut, Player p, Territory t, int max)
		{
			UnitCollection uc = new UnitCollection();

			int numAdded = 0;
			if(List.Count < max)
			{
				max = List.Count;
			}
			for (int i = 0; i < List.Count && numAdded < max; i++)
			{
				Unit current = (Unit)List[i];

				bool addUnit = true;
				bool doesntMatchType = true;
				bool doesntMatchOwner = true;
				bool doesntMatchTerritory = true;

				
				if (ut != UnitType.None && current.Type == ut)
				{
					doesntMatchType = false;
				}

				if (p != null && current.Owner == p)
				{
					doesntMatchOwner = false;
				}

				if (t != null && current.CurrentTerritory == t)
				{
					doesntMatchTerritory = false;
				}

				addUnit = (doesntMatchType && doesntMatchOwner && doesntMatchTerritory);
				

				if (addUnit)
				{
					uc.AddUnit(current);
					numAdded++;
				}
			}

			return uc;
		}

		public UnitCollection GetNonMatchingUnits(Territory t)
		{
			return GetNonMatchingUnits(UnitType.None, null, t, List.Count);
		}

		public UnitCollection GetNonMatchingUnits(UnitType type)
		{
			return GetNonMatchingUnits(type, null, null, List.Count);
		}

		public UnitCollection GetNonMatchingUnits(Player p)
		{
			return GetNonMatchingUnits(UnitType.None, p, null, List.Count);
		}

		public UnitCollection GetNonMatchingUnits(Territory t, int max)
		{
			return GetNonMatchingUnits(UnitType.None, null, t, max);
		}

		public UnitCollection GetNonMatchingUnits(UnitType type, int max)
		{
			return GetNonMatchingUnits(type, null, null, max);
		}

		public UnitCollection GetNonMatchingUnits(Player p, int max)
		{
			return GetNonMatchingUnits(UnitType.None, p, null, max);
		}

		public UnitCollection GetTransportsWithContents(UnitType ut, int numTransportees)
		{
			UnitCollection uc = new UnitCollection();
			UnitCollection transports = this.GetUnits(UnitType.Transport);

			foreach(Transport tr in transports)
			{
				if(ut == UnitType.None && tr.Transportees.Count == 0)
				{
					uc.AddUnit(tr);
					continue;
				}

				bool transportMatches = true;

				if(tr.Transportees.Count != numTransportees)
				{
					transportMatches = false;
				}

				if(tr.Transportees.Count != 0 && tr.Transportees[0].Type != ut)
				{
					transportMatches = false;
				}

				if(transportMatches)
				{
					uc.AddUnit(tr);
				}
			}

			return uc;
		}

		public ArrayList GetUnitTerritories()
		{
			ArrayList al = new ArrayList();

			foreach(Unit u in List)
			{
				if(!al.Contains(u.CurrentTerritory))
				{
					al.Add(u.CurrentTerritory);
				}
			}

			return al;
		}

		public ArrayList GetPlayersWithUnits()
		{
			//note nulls are handled by PlayerID.NULL_PLAYERID
			//Hashtable ht = new Hashtable();
			ArrayList al = new ArrayList();

			foreach(Unit unit in List)
			{
				//if(!ht.ContainsKey(unit.Owner))
				if(!al.Contains(unit.Owner))
				{
					//ht[unit.Owner] = "";
					al.Add(unit.Owner);
				}
			}
			return al;
		}

		public Hashtable GetPlayerUnitCounts()
		{
			Hashtable count = new Hashtable();

			//Iterator iter = List.iterator();
			//while(iter.hasNext() )
			foreach(Unit unit in List)
			{
					Player player = unit.Owner;
					int num = 1;
					if(!count.ContainsKey(player))
					{
						count[player] = num;
					}
					else
					{
						num = (int)count[player];
						num++;
						count[player] = num;
					}
			}					
			return count;
		}

		public bool HasUnitsFromMultiplePlayers
		{
			get
			{
				return GetPlayersWithUnits().Count > 1;
			}
		}

	}
}