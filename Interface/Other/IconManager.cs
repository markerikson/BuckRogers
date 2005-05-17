using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.PiccoloX;
using UMD.HCIL.PiccoloX.Activities;
using UMD.HCIL.PiccoloX.Nodes;
using UMD.HCIL.PiccoloX.Components;
using UMD.HCIL.Piccolo.Event;

using BuckRogers;
using GpcWrapper;

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
		private MapControl m_map;

		public IconManager(MapControl map)
		{
			m_map = map;
			m_territories = new Hashtable();
			m_iconLocations = new Hashtable();
			m_icons = new Hashtable();
		}

		public void CreateIcons()
		{
			string[] typeAbbreviations = {"To", "Ge", "Fi", "Ba", "Tr", "Ks", "Ma", "Fa", "Le"};
			UnitType[] types = {UnitType.Trooper, UnitType.Gennie, UnitType.Fighter, UnitType.Battler, 
								UnitType.Transport, UnitType.KillerSatellite, UnitType.Marker,
								UnitType.Factory, UnitType.Leader};
			Font f = new Font("Arial",	19, FontStyle.Bold,	GraphicsUnit.Point);

			for(int i = 0; i < m_controller.Players.Length; i++)
			{
				Player p = m_controller.Players[i];
				Hashtable ht = new Hashtable();

				Brush brush = new SolidBrush(p.Color);

				Bitmap[] icons = new Bitmap[typeAbbreviations.Length];

				for(int j = 0; j < typeAbbreviations.Length; j++)
				{	
					string name = typeAbbreviations[j];
					icons[j] = new Bitmap(48, 48);
					Graphics g = Graphics.FromImage(icons[j]);

					g.FillRectangle(Brushes.White, 0, 0, 48, 48);
					g.DrawRectangle(Pens.Black, 0, 0, 47, 47);

					g.SmoothingMode = SmoothingMode.AntiAlias;
					StringFormat sf = new StringFormat();
					sf.Alignment = StringAlignment.Center;
					sf.LineAlignment = StringAlignment.Center;
					RectangleF rect = new RectangleF(1, 1, 46, 46);
					
					g.DrawString(name, f, brush, rect, sf);
					
					ht[types[j]] = icons[j];
					
				}

				m_icons[p.Name] = ht;
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
				
				//PointF[] locations = new PointF[numIcons];
				//IconInfo[] icons = new IconInfo[numIcons];
				ArrayList al = new ArrayList();
				for(int j = 0; j < numIcons; j++)
				{
					line = sr.ReadLine();
					string[] splitPoints = line.Split(new char[]{','});

					IconInfo info = new IconInfo();
					info.Original = true;
					info.Used = false;
					info.Location = new PointF(Int32.Parse(splitPoints[0]), Int32.Parse(splitPoints[1]));

					al.Add(info);
				}

				m_iconLocations[territoryName] = al;

			}

			sr.Close();
		}


		public void SetIconInfo(Territory t, Player p, UnitType ut, int numUnits)
		{
			IconInfo info = GetIconInfo(t, p, ut);			
			info.Used = true;
			info.Type = ut;
			info.Player = p;


			info.Label.Text = numUnits.ToString();

			m_map.Canvas.Refresh();
		}

		public void RemoveIcon(Territory t, Player p, UnitType ut)
		{
			IconInfo info = GetIconInfo(t, p, ut);

			if(info.Label != null)
			{
				PNode parent = info.Label.Parent;
				parent.RemoveFromParent();
				info.Label.RemoveFromParent();
				info.Icon.RemoveFromParent();
			}


			info.Used = false;
			info.Icon = null;
			info.Label = null;
			info.Type = UnitType.None;
			info.Player = Player.NONE;

			m_map.Canvas.Refresh();
		}


		public IconInfo GetIconInfo(Territory t, Player p, UnitType ut)
		{
			ArrayList locations = (ArrayList)m_iconLocations[t.Name];

			if(locations != null)
			{
				PointF location = new PointF();

				bool foundMatch = false;
				for(int i = 0; i < locations.Count; i++)
				{
					IconInfo info = (IconInfo)locations[i];

					if(info.Player == p && (info.Type == ut))
					{
						location = info.Location;
						foundMatch = true;
						
						return info;
					}
				}

				if(!foundMatch)
				{

					IconInfo info = null;

					for(int i = 0; i < locations.Count; i++)
					{
						IconInfo newInfo = (IconInfo)locations[i];
						if(!newInfo.Used)
						{
							info = newInfo;							
							break;
						}
					}

					if(info == null)
					{
						info = new IconInfo();
						info.Original = false;
						if(t.IsSolarTerritory)
						{
							PPath node = (PPath)m_map.Territories[t.Name];
							PointF nodeCenter = PUtil.CenterOfRectangle(node.Bounds);
							info.Location = GenerateSolarPoint(0, true, nodeCenter);
						}
						else
						{
							info.Location = GenerateTerritoryPoint(t);
						}						

						locations.Add(info);
					}					

					info.Player = p;
					info.Type = ut;
					InitializeIcon(info, t);
					
					return info;
				
				}
			}
			// must be a previously unused solar point, because all ground/local 
			// territories are already filled in, and so are solar points once they're used
			else
			{
				ArrayList al = new ArrayList();

				PPath node = (PPath)m_map.Territories[t.Name];
				PointF nodeCenter = PUtil.CenterOfRectangle(node.Bounds);

				for(int i = 0; i < 6; i++)
				{
					PointF location = GenerateSolarPoint(i, false, nodeCenter);

					IconInfo info = new IconInfo();
					info.Original = true;
					info.Used = false;
					info.Location = location;

					al.Add(info);
				}

				m_iconLocations[t.Name] = al;

				IconInfo returnInfo = (IconInfo)al[0];

				returnInfo.Player = p;
				returnInfo.Type = ut;
				InitializeIcon(returnInfo, t);
				return returnInfo;
			}
			
			return null;
		}

		private void InitializeIcon(IconInfo info, Territory t)
		{
			Hashtable ht = (Hashtable)m_icons[info.Player.Name];
			Bitmap b = (Bitmap)ht[info.Type];

			info.Icon = new PImage();
			info.Icon.Image = b;
			info.Icon.X = info.Location.X;
			info.Icon.Y = info.Location.Y;

			info.Label = new PText("w");
			Font f = info.Label.Font;
			info.Label.Font = new Font(f.Name, f.SizeInPoints + 6, FontStyle.Bold);

			if(t.Type == TerritoryType.Space)
			{
				info.Label.TextBrush = Brushes.White;
			}
			info.Label.X = info.Location.X + 24 - (info.Label.Width / 2);

			if(t.IsSolarTerritory)
			{
				info.Icon.X -= 24;
				info.Label.X -= 24;
			}
			info.Label.Y = info.Location.Y + 48 + 2;
					
			PComposite composite = new PComposite();
			composite.AddChild(info.Icon);
			composite.AddChild(info.Label);

			PPath territory = (PPath)m_map.Territories[t.Name];
			territory.Parent.AddChild(composite);

			info.Icon.MoveToFront();
			info.Label.MoveToFront();

		}

		// By this time, we don't care whether it overlaps or not
		public PointF GenerateTerritoryPoint(Territory t)
		{
			bool inTerritory = false;

			PPath territoryPath = (PPath)m_map.Territories[t.Name];
			RectangleF territoryBounds = territoryPath.Bounds;
			Polygon polyTerritory = PathToPolygon(territoryPath);
			PointF[] territoryPoints = PolygonToPoints(polyTerritory);

			SizeF iconSize = new SizeF(48, 80);

			PointF point = new PointF();

			while(!inTerritory)
			{
				point = new PointF();
				point.X = Utility.Twister2.Next((int)(territoryBounds.Left + 1), (int)(territoryBounds.Right - iconSize.Width - 1));
				point.Y = Utility.Twister2.Next((int)(territoryBounds.Top + 1), (int)(territoryBounds.Bottom - iconSize.Height - 1));
				
				inTerritory = PointInPolygon(territoryPoints, point);
			}

			return point;
		}

		private Polygon PathToPolygon(PPath path)
		{
			GraphicsPath gp = path.PathReference;
			gp.Flatten(new Matrix(), 0.1f);

			Polygon poly = new Polygon(gp);

			return poly;
		}

		private PointF[] PolygonToPoints(Polygon polyTerritory)
		{
			ArrayList al = new ArrayList();

			for(int i = 0; i < polyTerritory.Contour.Length; i++)
			{
				VertexList vertices = polyTerritory.Contour[i];
				
				for(int j = 0; j < vertices.Vertex.Length; j++)
				{
					Vertex v = vertices.Vertex[j];
					al.Add(new PointF((float)v.X, (float)v.Y));
				}
			}

			PointF[] territoryPoints = (PointF[])al.ToArray(typeof(PointF));

			return territoryPoints;
		}

		private bool PointInPolygon(PointF[] polygon, PointF p)
		{
			int i, j = 0;
			bool c = false;
			float x = p.X;
			float y = p.Y;
			for (i = 0, j = polygon.Length-1; i < polygon.Length; j = i++) 
			{
				if ((((polygon[i].Y <= y) && (y < polygon[j].Y)) ||
					((polygon[j].Y <= y) && (y < polygon[i].Y))) &&
					(x < (polygon[j].X - polygon[i].X) * (y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
					c = !c;
			}
			return c;
		}

		public PointF GenerateSolarPoint(int iconNumber, bool random, PointF offset)
		{
			float numDegrees = 0;
			float radius = 60;

			if(!random)
			{
				// multiply the nth position times number
				// of degrees of separation for six icons,
				// then rotate left by 45 degrees
				numDegrees = iconNumber * 60 - 60;
				//numDegrees += 45;
			}
			else
			{
				numDegrees = Utility.Twister2.Next(0, 360);
			}

			float x = -Utility.GetCos(numDegrees) * radius;
			float y = Utility.GetSin(numDegrees) * radius;

			// center the icon, hopefully
			//x -= 24;

			x += offset.X;
			y += offset.Y;

			y -= 24;

			return new PointF(x, y);
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

	public class IconInfo
	{
		private PointF m_location;
		private Player m_player;
		private UnitType m_unitType;
		private PText m_label;
		private PImage m_icon;
		private bool m_original;
		private bool m_used;

		public IconInfo()
		{
			m_original = false;
			m_unitType = UnitType.None;
			m_player = Player.NONE;
			m_used = false;
		}

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

		public PImage Icon
		{
			get { return this.m_icon; }
			set { this.m_icon = value; }
		}

		public PText Label
		{
			get { return this.m_label; }
			set { this.m_label = value; }
		}

		public bool Original
		{
			get { return this.m_original; }
			set { this.m_original = value; }
		}

		public bool Used
		{
			get { return this.m_used; }
			set { this.m_used = value; }
		}		
	}
}


