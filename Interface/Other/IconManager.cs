using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
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
		private Hashtable m_icons;
		private int m_iconSetNumber;
		private GameController m_controller;

		public IconManager()
		{
			m_territories = new Hashtable();
			m_iconLocations = new Hashtable();
			m_icons = new Hashtable();
		}

		public void CreateIcons()
		{
			string[] types = {"To", "Ge", "Fi", "Ba", "Tr", "Ks", "Ma", "Fa", "Le"};
			Font f = new Font("Arial",	19, FontStyle.Bold,	GraphicsUnit.Point);

			for(int i = 0; i < m_controller.Players.Length; i++)
			{
				Player p = m_controller.Players[i];
				Brush brush = new SolidBrush(p.Color);

				Bitmap[] icons = new Bitmap[types.Length];

				for(int j = 0; j < types.Length; j++)
				{	
					string name = types[j];
					icons[j] = new Bitmap(48, 48);
					Graphics g = Graphics.FromImage(icons[j]);

					g.FillRectangle(Brushes.White, 0, 0, 48, 48);
					g.DrawRectangle(Pens.Black, 0, 0, 47, 47);

					g.SmoothingMode = SmoothingMode.AntiAlias;
					StringFormat sf = new StringFormat();
					sf.Alignment = StringAlignment.Center;
					sf.LineAlignment = StringAlignment.Center;
					RectangleF rect = new RectangleF(1, 1, 46, 46);
					//SizeF size = g.MeasureString(name, f, )
					
					/*
					PointF location = new PointF();

					if(size.Width > 48)
					{
						location.X = 1;
					}
					else
					{
						float difference = 48 - size.Width;
						location.X = difference / 2;
					}

					if(size.Height > 48)
					{
						location.Y = 1;
					}
					else
					{
						float difference = 48 - size.Height;
						location.Y = difference / 2;
					}
					*/
					
					g.DrawString(name, f, brush, rect, sf);

					//icons[j].Save(p.Name + name + ".png", ImageFormat.Png);

				}

				m_icons[p.Name] = icons;
			}
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
				m_iconSetNumber = Utility.Twister2.Next(1,5);
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

		public BuckRogers.GameController Controller
		{
			get { return this.m_controller; }
			set { this.m_controller = value; }
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


