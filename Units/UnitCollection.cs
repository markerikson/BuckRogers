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
				List.Add(collection[i]);
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
				List.Remove(unit);
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

				if(current.UnitType == ut)
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

		public UnitCollection GetUnits(UnitType ut, Player p, Territory t)
		{
			UnitCollection uc = new UnitCollection();

			for (int i = 0; i < List.Count; i++)
			{
				Unit current = (Unit)List[i];

				bool addUnit = true;
				bool matchType = true;
				bool matchOwner = true;
				bool matchTerritory = true;

				
				if (ut != UnitType.None && current.UnitType != ut)
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
				}
			}

			return uc;
		}

		public UnitCollection GetUnits(Territory t)
		{
			return GetUnits(UnitType.None, null, t);
		}

		public UnitCollection GetUnits(UnitType type)
		{
			return GetUnits(type, null, null);
		}

		public UnitCollection GetUnits(Player p)
		{
			return GetUnits(UnitType.None, p, null);
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

			//Iterator iter = List.iterator();
			//while(iter.hasNext() )
			foreach(Unit unit in List)
			{
				if(unit.Owner == id)
				{
					UnitType ut = unit.UnitType;
					int num = 0;
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

		public UnitCollection GetOtherPlayersUnits(Player player)
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

		public Hashtable GetPlayersWithUnits()
		{
			//note nulls are handled by PlayerID.NULL_PLAYERID
			Hashtable ht = new Hashtable();

			foreach(Unit unit in List)
			{
				if(!ht.ContainsKey(unit.Owner))
				{
					ht[unit.Owner] = "";
				}
			}
			return ht;
		}

		public Hashtable GetPlayerUnitCounts()
		{
			Hashtable count = new Hashtable();

			//Iterator iter = List.iterator();
			//while(iter.hasNext() )
			foreach(Unit unit in List)
			{
					Player player = unit.Owner;
					int num = 0;
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