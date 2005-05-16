using System;
using System.Collections;
using System.Drawing;
using System.IO;

using BuckRogers;

namespace BuckRogers.Interface
{
	/// <summary>
	/// Summary description for IconManager.
	/// </summary>
	public class IconManager
	{
		private Hashtable m_territories;
		private Hashtable m_iconLocations;
		private int m_iconSetNumber;

		public IconManager()
		{
			m_territories = new Hashtable();
			m_iconLocations = new Hashtable();
		}

		public void LoadUnitIconLocations()
		{
			LoadUnitIconLocations(true, false);
		}

		public void LoadUnitIconLocations(bool nextSet, bool randomSet)
		{
			string filename = "iconlocations";

			if(nextSet)
			{
				m_iconSetNumber %= 5;
				m_iconSetNumber++;
			}
			else if(randomSet)
			{
				m_iconSetNumber = 0;
			}

			filename += m_iconSetNumber.ToString() + ".txt";

			StreamReader sr = new StreamReader(filename);

			string line = sr.ReadLine();
			int numTerritories = Int32.Parse(line);

			for(int i = 0; i < numTerritories; i++)
			{
				string territoryName = sr.ReadLine();

				int numIcons = 6;


				if(territoryName.EndsWith("Orbit") 
					&& !territoryName.StartsWith("Near") 
					&& !territoryName.StartsWith("Far"))
				{
					numIcons = 4;
				}
				PointF[] locations = new PointF[numIcons];
				for(int j = 0; j < numIcons; j++)
				{
					line = sr.ReadLine();
					string[] splitPoints = line.Split(new char[]{','});

					locations[j] = new PointF();
					locations[j].X = Int32.Parse(splitPoints[0]);
					locations[j].Y = Int32.Parse(splitPoints[1]);
				}

				m_iconLocations[territoryName] = locations;

			}

			sr.Close();
		}


		public void SetIconInfo(Territory t, Player p, UnitType ut, int numUnits)
		{

		}


		public PointF GetIconPosition(string territoryName, Player p, UnitType ut)
		{
			IconLocationInfo[] locations = (IconLocationInfo[])m_territories[territoryName];

			PointF location = new PointF();

			bool foundMatch = false;
			for(int i = 0; i < locations.Length; i++)
			{
				IconLocationInfo ili = locations[i];

				if(ili.Player == p && (ili.Type == ut))
				{
					location = ili.Location;
					foundMatch = true;
					break;
				}
			}

			if(!foundMatch)
			{
				location = GeneratePoint();
			}

			return location;
		}

		public PointF GeneratePoint()
		{
			return new PointF();
		}


		public System.Collections.Hashtable Territories
		{
			get { return this.m_territories; }
			set { this.m_territories = value; }
		}
	}

	public class IconLocationInfo
	{
		PointF m_location;
		Player m_player;
		UnitType m_unitType;

		public PointF Location
		{
			get { return this.m_location; }
			set { this.m_location = value; }
		}

		public BuckRogers.Player Player
		{
			get { return this.m_player; }
			set { this.m_player = value; }
		}

		public BuckRogers.UnitType Type
		{
			get { return this.m_unitType; }
			set { this.m_unitType = value; }
		}		
	}
}


