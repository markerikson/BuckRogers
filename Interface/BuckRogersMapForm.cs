using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Text;

using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.PiccoloX;
using UMD.HCIL.PiccoloX.Nodes;

namespace BuckRogers 
{
	public class BuckRogersMapForm : UMD.HCIL.PiccoloX.PForm 
	{
		private System.ComponentModel.IContainer components = null;

		private PNode[] m_meo;
		private PNode[] m_vo;
		private PNode[] m_eo;
		private PNode[] m_teo;
		private PNode[] m_mao;
		private PNode[] m_tmo;
		private PNode[] m_ao;
		private PNode[][] m_orbits;
		
		private int m_idxPlanets;
		private PClip[] m_planets;

		public BuckRogersMapForm() {
			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		public override void Initialize() {
			//PText text = new PText("Hello World");

			

			//int numNodes = 32;
			//int radius = 200;
			
			/*
			DrawNodeCircle("MeO", 2, 50);
			DrawNodeCircle("VO", 4, 100, true);
			DrawNodeCircle("MO", 16, 200);
			DrawNodeCircle("TMO", 32, 300);
			DrawNodeCircle("AO", 32, 400);
			*/

			/*
			DrawNodeCircle("Mercury Orbit: ", 2, 100);
			DrawNodeCircle("Venus Orbit: ", 4, 200, true);
			DrawNodeCircle("Earth Orbit", 8, 300);
			DrawNodeCircle("Trans-Earth Orbit: ", 16, 375);
			DrawNodeCircle("Mars Orbit: ", 16, 450);
			DrawNodeCircle("Trans-Mars Orbit: ", 32, 525);
			DrawNodeCircle("Asteroid Orbit: ", 32, 600);
			*/

			// replace standard layer with grid layer.
			PRoot root = Canvas.Root;
			PCamera camera = Canvas.Camera;
			root.RemoveChild(camera.GetLayer(0));
			camera.RemoveLayer(0);
			BlackLayer bl = new BlackLayer();
			root.AddChild(bl);
			camera.AddLayer(bl);
			//Canvas.Layer.Root.Brush = Brushes.Black;


			m_orbits = new PNode[7][];
			m_planets = new PClip[5];
			m_idxPlanets = 0;


			m_ao = new PNode[32];
			m_tmo = new PNode[32];
			m_mao = new PNode[16];
			m_teo = new PNode[16];
			m_eo = new PNode[8];
			m_vo = new PNode[4];
			m_meo = new PNode[2];

			m_orbits[0] = m_ao;
			m_orbits[1] = m_tmo;
			m_orbits[2] = m_mao;
			m_orbits[3]	= m_teo;
			m_orbits[4] = m_eo;
			m_orbits[5] = m_vo;
			m_orbits[6] = m_meo;

			int orbitRadiusUnit = 28;

			DrawNodeCircle(m_ao, "AO: ", 32, /*600*/ 36 * orbitRadiusUnit, Color.Gray, false, false, false);
			DrawNodeCircle(m_tmo, "TMO: ", 32, /*525*/  32 * orbitRadiusUnit, Color.White, false, true, true);
			DrawNodeCircle(m_mao,"MaO: ", 16, /*450*/ 28 * orbitRadiusUnit, Color.Red, false, true, false);
			DrawNodeCircle(m_teo, "TEO: ", 16, /*375*/ 24 * orbitRadiusUnit, Color.White, false, true, true);
			DrawNodeCircle(m_eo, "EO", 8, /*300*/ 19 * orbitRadiusUnit, Color.Blue, false, true, false);
			DrawNodeCircle(m_vo, "VO: ", 4, /*200*/ 12 * orbitRadiusUnit, Color.Green, true, true, false);
			DrawNodeCircle(m_meo, "MeO: ", 2, /*100*/ 5 * orbitRadiusUnit, Color.Yellow, false, true, false);
			
			
			PPath center = PPath.CreateEllipse(0, 0, 10f, 10f);
			center.MouseUp +=new PInputEventHandler(center_MouseUp);
			center.Brush = Brushes.White;
			RectangleF centerBounds = center.Bounds;

			int centerX = this.Bounds.Width / 2;
			int centerY = this.Bounds.Height / 2;
			centerBounds.X = -5 + centerX;
			centerBounds.Y = -5 + centerY;
			center.Bounds = centerBounds;
			
			Canvas.Layer.AddChild(center);
			
			

			DrawConnectors();

			AddPlanets();


			
			base.Initialize ();
		}

		private void AddPlanets()
		{
			#region Earth regions
			PointF[][] earthPoints = new PointF[][]
								{

									new PointF[] 
									{
										new PointF(0, 0), new PointF(160, 0), new PointF(160, 30f), 
										new PointF(0, 30f),
								},

									new PointF[] 
									{
										new PointF(0, 30f), new PointF(80, 30), new PointF(80, 65f), 
										new PointF(0, 65f),
								},

									new PointF[] 
									{
										new PointF(0, 65), new PointF(80, 65), new PointF(80, 115f), 
										new PointF(50, 115), new PointF(0, 85),
								},

									new PointF[] 
									{
										new PointF(0, 85), new PointF(50, 115), new PointF(70, 115), 
										new PointF(70, 160), new PointF(0, 160)
									},

									new PointF[] 
									{
										new PointF(70, 115), new PointF(160, 115), new PointF(160, 160), 
										new PointF(70, 160),
								},

									new PointF[] 
									{
										new PointF(160, 115), new PointF(80, 115), new PointF(80, 75), 
										new PointF(100, 75),new PointF(160, 45)
									},

									new PointF[] 
									{
										new PointF(160, 30), new PointF(80, 30), new PointF(80, 75), 
										new PointF(100, 75f), new PointF(160, 45)
									},
			};

			string[] earthNames = {"Independent Arcologies", "American Regency", "Urban Reservations", 
									  "Australian Development Facility", "Antarctic Testing Zone",
									  "African Regency", "Eurasian Regency"};

			#endregion
			
			#region Mercury regions
			PointF[][] mercuryPoints = new PointF[][]
								{

									new PointF[] 
									{
										new PointF(0, 0), new PointF(85, 85), new PointF(0, 120), 
									},

									new PointF[] 
									{
										new PointF(0, 0), new PointF(80, 80), new PointF(115, 0), 
									},

									new PointF[] 
									{
										new PointF(115, 0), new PointF(80, 80), new PointF(140, 140), 
										new PointF(140, 0),
									},

									new PointF[] 
									{
										new PointF(0, 120), new PointF(85, 85), new PointF(140, 140), 
										new PointF(0, 140), 
									},
								};
			string[] mercuryNames = {"Bach", "The Warrens", "Tolstoi", "Sobkou Plains"};

			#endregion

			#region Venus regions
			PointF[][] venusPoints = new PointF[][]
								{

									new PointF[] 
									{
										new PointF(0, 0), new PointF(65, 0), new PointF(65, 58.5f), 
										new PointF(0, 40f),
									},

									new PointF[] 
									{
										new PointF(65, 0), new PointF(65, 58.5f), new PointF(70, 60), 
										new PointF(160, 0),
									},

									new PointF[] 
									{
										new PointF(160, 0), new PointF(70, 60), new PointF(70, 90), 
										new PointF(160, 90),
									},

									new PointF[] 
									{
										new PointF(160, 90), new PointF(110, 90), new PointF(110, 160), 
										new PointF(160, 160), 
									},

									new PointF[] 
									{
										new PointF(110, 160), new PointF(110, 90), new PointF(70, 90), 
										new PointF(60, 98.5f), new PointF(60, 160),
									},

									new PointF[] 
									{
										new PointF(60, 160), new PointF(60, 98.5f), new PointF(0, 135), 
										new PointF(0, 160),
									},

									new PointF[] 
									{
										new PointF(0, 40), new PointF(65, 58.5f), new PointF(70, 60), 
										new PointF(70, 90), new PointF(60, 98.5f), new PointF(0, 135),
									},
								};

			string[] venusNames = {"Aerostates", "Mt. Maxwell", "Elysium,", 
									  "Wreckers", "Aphrodite",
									  "Beta Regio", "Lowlanders"};

			#endregion

			#region Mars regions
			PointF[][] marsPoints = new PointF[][]
								{
									new PointF[] 
									{
										new PointF(0, 0), new PointF(40, 0), new PointF(40, 150), 
										new PointF(0, 150),
									},

									new PointF[] 
									{
										new PointF(40, 0), new PointF(40, 40), new PointF(110, 40), 
										new PointF(110, 0),
									},

									new PointF[] 
									{
										new PointF(40, 40), new PointF(75, 40), new PointF(75, 150), 
										new PointF(40, 150),
									},

									new PointF[] 
									{
										new PointF(75, 40), new PointF(110, 40), new PointF(110, 150), 
										new PointF(75, 150), 
									},

									new PointF[] 
									{
										new PointF(110, 0), new PointF(150, 0), new PointF(150, 150), 
										new PointF(110, 150),
									},
								};

			string[] marsNames = {"Coprates Chasm", "Boreal Sea", "Ram HQ", 
									  "Arcadia", "Pavonis"};

			#endregion

			#region Moon regions
			PointF[][] moonPoints = new PointF[][]
								{

									new PointF[] 
									{
										new PointF(0, 0), new PointF(0, 10), new PointF(50, 50), 
										new PointF(110, 15), new PointF(110, 0)
									},

									new PointF[] 
									{
										new PointF(0, 10), new PointF(50, 50), new PointF(55, 55), 
										new PointF(50, 60), new PointF(0, 105)
									},

									new PointF[] 
									{
										new PointF(0, 110), new PointF(0, 105), new PointF(50, 60),  
										new PointF(110, 95), new PointF(110, 110), 
									},

									new PointF[] 
									{
										new PointF(110, 15), new PointF(50, 50), new PointF(55, 55), 
										new PointF(50, 60), new PointF(110, 95), 
									},

								};

			string[] moonNames = {"Moscoviense", "Tranquility", "Tycho","Farside"};

			#endregion

			float scaleFactor = 6.0f;
	
			#region Mercury orbit

			// 29cm wide, 27cm high
			PointF[][] mercuryNearOrbitPoints = new PointF[][]
			{
				/*
				new PointF[] 
				{
					new PointF(0, 0), new PointF(100, 16), new PointF(150, 20), 
					new PointF(200, 16), new PointF(270, 5), new PointF(285, 0),
					new PointF(290, 30), new PointF(275, 130), new PointF(285, 170),
					new PointF(255, 140), new PointF(60, 140), new PointF(0, 115),
				},
				*/
				new PointF[] 
				{
					new PointF(0, 15), new PointF(130, 40), new PointF(170, 40), new PointF(240, 0)
				},
				new PointF[] 
				{
					new PointF(240, 0), new PointF(260, -10), new PointF(275, 5), new PointF(270, 30)
				},
				new PointF[] 
				{
					new PointF(270, 30), new PointF(260, 70), new PointF(250, 150), new PointF(260, 200)
				},
				new PointF[] 
				{
					new PointF(260, 200), new PointF(255, 230), new PointF(240, 240), new PointF(220, 240)
				},
				new PointF[] 
				{
					new PointF(220, 240), new PointF(50, 240), new PointF(50, 240), new PointF(0, 155)
				},

			};

			PointF[][] mercuryFarOrbitPoints = new PointF[][]
			{
				new PointF[] 
				{
					new PointF(0, 325), new PointF(100, 300), new PointF(150, 285), new PointF(200, 285)
				},
				new PointF[] 
				{
					new PointF(200, 285), new PointF(250, 285), new PointF(270, 250), new PointF(260, 200)
				},
				new PointF[] 
				{
					new PointF(260, 200), new PointF(255, 230), new PointF(240, 240), new PointF(220, 240)
				},
				new PointF[] 
				{
					new PointF(220, 240), new PointF(50, 240), new PointF(50, 240),  new PointF(0, 155)
				},			
				
				
			};
			//string[] mercuryOrbitNames = {"Near Mercury Orbit", "Far Mercury Orbit"};

			#endregion
	
			#region Venus orbit
			
			PointF[][] venusNearOrbitPoints = new PointF[][]
			{
				new PointF[] 
				{
					new PointF(0, 45), new PointF(52.5f, 31.5f), new PointF(93, 21.5f), new PointF(140, 10)
				},
				new PointF[] 
				{
					new PointF(140, 10), new PointF(200, 10), new PointF(225, 30), new PointF(225, 60)
				},
				new PointF[] 
				{
					new PointF(225, 60), new PointF(230, 80), new PointF(230, 80), new PointF(225, 130)
				},
				new PointF[] 
				{
					new PointF(225, 130), new PointF(205, 170), new PointF(200, 190), new PointF(185, 230)
				},
				new PointF[]
				{
					new PointF(185, 230), new PointF(150, 230), new PointF(100, 230), new PointF(0, 230),
				},
			};

			PointF[][] venusFarOrbitPoints = new PointF[][]
			{
				new PointF[] 
				{
					new PointF(140, 10), new PointF(200, 0), new PointF(230, 0), new PointF(240, 10)
				},
				new PointF[] 
				{
					new PointF(240, 10), new PointF(270, 25), new PointF(265, 60), new PointF(260, 130)
				},
				new PointF[] 
				{
					new PointF(260, 130), new PointF(250, 170), new PointF(250, 190), new PointF(260, 230)
				},
				new PointF[] 
				{
					new PointF(260, 230), new PointF(230, 230), new PointF(220, 230), new PointF(185, 230)
				},
				new PointF[] 
				{
					new PointF(185, 230), new PointF(200, 190), new PointF(205, 170), new PointF(225, 130)
				},
				new PointF[] 
				{
					new PointF(225, 130), new PointF(230, 80), new PointF(230, 80), new PointF(225, 60)
				},
				new PointF[] 
				{	
					new PointF(225, 60), new PointF(225, 30), new PointF(200, 10), new PointF(140, 10)
				},
				
				
			};

			#endregion

			#region Earth/Moon orbit

			PointF[][] moonNearOrbitPoints = new PointF[][]
			{
				new PointF[] 
				{
						new PointF(10, 340), new PointF(0, 270), new PointF(0, 190), new PointF(20, 110)
				},
				new PointF[] 
				{
					new PointF(20, 110), new PointF(15, 150), new PointF(55, 150), new PointF(70, 160)
				},
				new PointF[] 
				{
					new PointF(70, 160), new PointF(95, 170), new PointF(105, 170), new PointF(110, 185)
				},
				new PointF[] 
				{
					new PointF(110, 185), new PointF(110, 200), new PointF(130, 210), new PointF(135, 240)
				},
				new PointF[] 
				{
					new PointF(135, 240), new PointF(150, 270), new PointF(145, 310), new PointF(155, 340)
				},
			};

			PointF[][] earthNearOrbitPoints = new PointF[][]
			{
				new PointF[] 
				{
					new PointF(155, 340), new PointF(145, 310), new PointF(150, 270), new PointF(135, 240)
				},
				new PointF[] 
				{
					new PointF(135, 240), new PointF(130, 210), new PointF(110, 200), new PointF(110, 185)
				},
				new PointF[] 
				{
					new PointF(110, 185), new PointF(110, 160), new PointF(130, 145), new PointF(150, 155)
				},
				new PointF[] 
				{
					new PointF(150, 155), new PointF(200, 170), new PointF(290, 180), new PointF(320, 155)
				},
				new PointF[]
				{
					new PointF(320, 155), new PointF(320, 170), new PointF(320, 300), new PointF(320, 340)
				},
			};

			PointF[][] earthFarOrbitPoints = new PointF[][]
			{
				new PointF[] 
				{
					new PointF(320, 155), new PointF(290, 180), new PointF(200, 170), new PointF(150, 155)
				},
				new PointF[] 
				{
					new PointF(150, 155), new PointF(130, 145), new PointF(110, 160), new PointF(110, 185)
				},
				new PointF[] 
				{
					new PointF(110, 185), new PointF(105, 170), new PointF(95, 170), new PointF(70, 160)
				},
				new PointF[] 
				{
					new PointF(70, 160), new PointF(55, 150), new PointF(15, 150), new PointF(20, 110)
				},
				new PointF[] 
				{
					new PointF(20, 110), new PointF(30, 90), new PointF(50, 30), new PointF(80, 45)
				},
				new PointF[] 
				{
					new PointF(80, 45), new PointF(170, 80), new PointF(280, 90), new PointF(320, 60)
				},
			};

			#endregion

			#region Mars orbit

			PointF[][] marsNearOrbitPoints = new PointF[][]
			{
				new PointF[] 
				{
					new PointF(320, 280), new PointF(280, 310), new PointF(170, 300), new PointF(80, 265)
				},
				new PointF[] 
				{
					new PointF(80, 265), new PointF(40, 230), new PointF(0, 150), new PointF(-20, 95)
				},
				new PointF[] 
				{
					new PointF(-20, 95), new PointF(0, 140), new PointF(50, 185), new PointF(80, 185)
				},
				new PointF[] 
				{
					new PointF(80, 185), new PointF(120, 185), new PointF(230, 70), new PointF(250, 55)
				},
				new PointF[] 
				{
					new PointF(250, 55), new PointF(260, 30), new PointF(270, 25), new PointF(280, 25)
				},
				new PointF[] 
				{
					new PointF(280, 25), new PointF(300, 25), new PointF(310, 25), new PointF(320, 25)
				},
				/*
				new PointF[] 
				{
					new PointF(-20, 95), new PointF(-20, 60), new PointF(-20, 30), new PointF(-20, 0)
				},
				new PointF[] 
				{
					new PointF(-20, 0), new PointF(0, 0), new PointF(300, 0), new PointF(320, 0)
				},
				*/
			};

			PointF[][] marsFarOrbitPoints = new PointF[][]
			{
				new PointF[] 
				{
					new PointF(-30, 25), new PointF(-30, 45), new PointF(-30, 70), new PointF(-20, 95)
				},
				new PointF[] 
				{
					new PointF(-20, 95), new PointF(0, 140), new PointF(50, 185), new PointF(80, 185)
				},
				new PointF[] 
				{
					new PointF(80, 185), new PointF(120, 185), new PointF(230, 70), new PointF(250, 55)
				},
				new PointF[] 
				{
					new PointF(250, 55), new PointF(260, 30), new PointF(270, 25), new PointF(280, 25)
				},
			};

			#endregion

			AddPlanet(70, mercuryPoints, mercuryNames, Brushes.Goldenrod, scaleFactor, -2400, -700);
			AddPlanet(80, venusPoints, venusNames, Brushes.LightGreen, scaleFactor, -2400, 1100);
			AddPlanet(80, earthPoints, earthNames, Brushes.LightBlue, scaleFactor, 2100, 1200);
			AddPlanet(55, moonPoints, moonNames, Brushes.LightGray, scaleFactor, 1300, 1400);
			AddPlanet(75, marsPoints, marsNames, Brushes.OrangeRed, scaleFactor, 2100, -450);

			DrawOrbit(mercuryNearOrbitPoints, Color.Yellow, 6.0f, -2500, -1000, true);
			DrawOrbit(mercuryFarOrbitPoints, Color.Yellow, 6.0f, -2500, -1000, true);

			DrawOrbit(venusNearOrbitPoints, Color.Green, 6.0F, -2500, 800, true);
			DrawOrbit(venusFarOrbitPoints, Color.Green, 6.0f, -2500, 800, true);

			DrawOrbit(moonNearOrbitPoints, Color.Blue, 6.0f, 1200, 150, true);
			DrawOrbit(earthNearOrbitPoints, Color.Blue, 6.0f, 1200, 150, true);
			DrawOrbit(earthFarOrbitPoints, Color.Blue, 6.0f, 1200, 150, true);

			DrawOrbit(marsNearOrbitPoints, Color.Red, 6.0f, 1200, -1200, true);
			DrawOrbit(marsFarOrbitPoints, Color.Red, 6.0f, 1200, -1200, true);

            for(int i = 0; i < m_planets.Length; i++)
			{
				m_planets[i].MoveToFront();
			}
			
		}

		private void AddPlanet(float radius, PointF[][] polygons, string[] names, Brush color, 
								float scaleFactor, int shiftX, int shiftY)
		{
			//float clipRadius = 80;
			float clipDiameter = 2 * radius;

			PClip planet = new PClip();
			planet.AddEllipse(0, 0, clipDiameter * scaleFactor, clipDiameter * scaleFactor);
			planet.X += shiftX;
			planet.Y += shiftY;

			Canvas.Layer.AddChild(planet);
			m_planets[m_idxPlanets] = planet;
			m_idxPlanets++;

			for(int i = 0; i < polygons.Length; i++)
			{
				PointF[] points = polygons[i];
				if(scaleFactor != 1.0f)
				{
					for(int j = 0; j < points.Length; j++)
					{
						points[j].X *= scaleFactor;
						points[j].Y *= scaleFactor;
					}
				}

				float centerX = 0.0f;
				float centerY = 0.0f;

				float totalX = 0.0f;
				float totalY = 0.0f;

				for(int j = 0; j < points.Length; j++)
				{
					totalX += points[j].X;
					totalY += points[j].Y;
				}

				centerX = totalX / points.Length;
				centerY = totalY / points.Length;


				PPath territory = PPath.CreatePolygon(points);
				territory.Brush = color;
				territory.Pen = Pens.White;

				PPath center = PPath.CreateEllipse(0, 0, 10, 10);
				center.Brush = Brushes.Red;

				center.X = centerX - (center.Width / 2) + shiftX;
				center.Y = centerY - (center.Width / 2) + shiftY;

				territory.AddChild(center);

				
				PText text = new PText(names[i]);
				text.X = centerX - (text.Width / 2) + shiftX;
				text.Y = centerY - (text.Height / 2) + shiftY;

				text.TextBrush = Brushes.Black;
				territory.AddChild(text);
				
				


				territory.X += shiftX;
				territory.Y += shiftY;

				planet.AddChild(territory);
			}
		}


		private void DrawNodeCircle(PNode[] nodes, string prefix, int numNodes, int radius, 
									Color color, bool drawConnector, bool isTransOrbit)
		{
			DrawNodeCircle(nodes, prefix, numNodes, radius, color, false, drawConnector, isTransOrbit);
		}

		private void DrawNodeCircle(PNode[] nodes, string prefix, int numNodes, int radius, 
									Color color, bool rotate45, bool drawConnector, bool isTransOrbit)
		{
			PPath circle;
			PText text;
			PComposite composite;
			float deg = (360 / (float)numNodes);

			int centerX = this.Bounds.Width / 2;
			int centerY = this.Bounds.Height / 2;

			PPath orbit = PPath.CreateEllipse(0f, 0f, 2 * radius, 2 * radius);
			RectangleF orbitBounds = orbit.Bounds;

			orbit.Brush = Brushes.Transparent;
			
			orbitBounds.X = centerX - radius;
			orbitBounds.Y = centerY - radius;
			orbit.Bounds = orbitBounds;

			if(!isTransOrbit)
			{
				orbit.Pen = new Pen(color);
			}
			
			Canvas.Layer.AddChild(orbit);

			//nodes = new PNode[numNodes];

			for(int i = 0; i < numNodes; i++)
			{
				
				composite = new PComposite();
				
				text = new PText(prefix + " " + i.ToString());
				text.TextBrush = Brushes.White;
				circle = PPath.CreateEllipse(0f, 0f, 10f, 10f);
				circle.Brush = new SolidBrush(color);//Brushes.Black;

				nodes[i] = circle;

				composite.MouseUp +=new UMD.HCIL.Piccolo.PInputEventHandler(text_Click);
				//composite.MouseEnter += new PInputEventHandler(composite_MouseEnter);
				//composite.MouseLeave += new PInputEventHandler(composite_MouseLeave);
				//text.Visible = false;

				float numDegrees = i * deg - 90;

				if(rotate45)
				{
					numDegrees += 45;
				}

				float x = -GetCos(numDegrees) * radius;
				float y = GetSin(numDegrees) * radius;

				

				RectangleF compositeBounds = composite.Bounds;
				compositeBounds.X = centerX + x;
				compositeBounds.Y = centerY + y;
				composite.Bounds = compositeBounds;

				composite.AddChild(text);
				composite.AddChild(circle);
				
				
				RectangleF textBounds = text.Bounds;
				textBounds.X = x + centerX - (textBounds.Width / 2);
				textBounds.Y = y - 20 + centerY;
				text.Bounds = textBounds;

				RectangleF circleBounds = circle.Bounds;				
				circleBounds.X = x - 5 + centerX;
				circleBounds.Y = y - 5 + centerY;				
				circle.Bounds = circleBounds;
				

				
				
				//Canvas.Layer.AddChild(text);
				//Canvas.Layer.AddChild(circle);
				Canvas.Layer.AddChild(composite);
			}
		}


		private void DrawOrbit(PointF[][] polygons, Color color, float scaleFactor, int shiftX, int shiftY, bool closeOrbit)
		{
			PPath orbit = new PPath();

			orbit.Pen = new Pen(color);
			orbit.Brush = Brushes.Black;

			for(int i = 0; i < polygons.Length; i++)
			{
				PointF[] shape = polygons[i];

				if(scaleFactor != 1.0f)
				{
					for(int j = 0; j < shape.Length; j++)
					{
						shape[j].X *= scaleFactor;
						shape[j].Y *= scaleFactor;


					}
				}
				
				orbit.AddBezier(shape[0].X, shape[0].Y, shape[1].X, shape[1].Y, shape[2].X, shape[2].Y, shape[3].X, shape[3].Y);
			}

			if(closeOrbit)
			{
				orbit.CloseFigure();
			}
			//orbit.X += shiftX;
			//orbit.Y += shiftY;
			orbit.OffsetX = shiftX;
			orbit.OffsetY = shiftY;

			orbit.MouseUp += new PInputEventHandler(orbit_MouseUp);

			Canvas.Layer.AddChild(orbit);

			
		}
		private void DrawConnectors()
		{
			for(int i = 1; i < m_orbits.Length; i++)
			{
				PNode[] orbit = m_orbits[i];
				PNode[] previousOrbit = m_orbits[i - 1];

				bool sameLength = (orbit.Length == previousOrbit.Length);

				if(!sameLength)
				{
					bool isMercury = (orbit.Length == 2);
					bool isVenus = (orbit.Length == 4);
					for(int j = 0; j < orbit.Length; j++)
					{
						int d1idx = 0;
						int d2idx = 0;
						
						if(isMercury)
						{
							/*
							d1idx = (j + 3) % 4;
							d2idx = (d1idx + 1) % 4;
							*/

							if(j == 0)
							{
								d1idx = 0;
								d2idx = 3;
							}
							else
							{
								d1idx = 1;
								d2idx = 2;
							}
						}
						else if(isVenus)
						{
							d1idx = (j * 2) + 1;
							d2idx = (d1idx + 1) % 8;
						}
						else
						{
							d1idx = 2 * j;
							d2idx = d1idx + 1;
						}
						

						/*
						if(j == 0)
						{
							d2idx = previousOrbit.Length - 1;
						}
						else
						{
							d2idx = 2 *j + 1;
						}
						*/

						PPath origin = (PPath)orbit[j];
						PPath destination1 = (PPath)previousOrbit[d1idx];
						PPath destination2 = (PPath)previousOrbit[d2idx];

						float startX = origin.X + 5;
						float startY = origin.Y + 5;

						float destination1X = destination1.X + 5;
						float destination1Y = destination1.Y + 5;

						float destination2X = destination2.X + 5;
						float destination2Y = destination2.Y + 5;

						PPath line1 = PPath.CreateLine(startX, startY, destination1X, destination1Y);
						PPath line2 = PPath.CreateLine(startX, startY, destination2X, destination2Y);

						line1.Pen = Pens.White;
						line2.Pen = Pens.White;

						Canvas.Layer.AddChild(line1);
						Canvas.Layer.AddChild(line2);

						origin.Parent.MoveToFront();
						destination1.Parent.MoveToFront();
						destination2.Parent.MoveToFront();
						//origin.MoveInFrontOf(line1);
						//destination1.MoveInFrontOf(line1);

						//origin.MoveInFrontOf(line2);
						//destination2.MoveInFrontOf(line2);
					}
				}
				else
				{
					for(int j = 0; j < orbit.Length; j++)
					{
						PPath origin = (PPath)orbit[j];
						PPath destination1 = (PPath)previousOrbit[j];

						float startX = origin.X + 5;
						float startY = origin.Y + 5;

						float destination1X = destination1.X + 5;
						float destination1Y = destination1.Y + 5;

						PPath line1 = PPath.CreateLine(startX, startY, destination1X, destination1Y);
						//PClip line1 = new PClip();
						//line1.AddLine(startX, startY, destination1X, destination1Y);

						line1.Pen = Pens.White;

						Canvas.Layer.AddChild(line1);
						//line1.MoveToBack();
						//origin.MoveInFrontOf(line1);
						//destination1.MoveInFrontOf(line1);
						origin.Parent.MoveToFront();
						destination1.Parent.MoveToFront();

					}
				}
				
			}
		}



		private static float GetSin(float degAngle)
		{
			return (float) Math.Sin(Math.PI * degAngle / 180);
		}
		private static float GetCos(float degAngle)
		{
			return (float) Math.Cos(Math.PI * degAngle / 180);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			// 
			// HelloWorldExample
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(392, 373);
			this.Location = new System.Drawing.Point(0, 0);
			this.Name = "Buck Rogers Interface Test";
			this.Text = "Buck Rogers Interface Test";

		}
		#endregion

		public static void Main(string[] args)
		{
			Application.Run(new BuckRogersMapForm());
		}

		private void text_Click(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
		{
			/*
			if(e.IsMouseEvent)
			{
				MessageBox.Show("IsMouseEvent");
			}
			else
			{
				MessageBox.Show("Not IsMouseEvent");
			}
			*/
			
			
			if(e.Button != MouseButtons.Left)
			{
				return;
			}
			//PText picked = (PText)e.PickedNode;
			PNode picked = e.PickedNode;
			
			if(picked is PComposite)
			{
				PComposite comp = picked as PComposite;

				/*
				PText text = null;
				foreach(PNode node in comp)
				{
					if(node is PText)
					{
						text = (PText)node;
						break;
					}
				}
				*/
				PPath path = null;
				foreach(PNode node in comp)
				{
					if(node is PPath)
					{
						path = (PPath)node;
						break;
					}
				}

				float scale = Canvas.Camera.ViewScale;
				MessageBox.Show("Scale: " + scale + ", x: " + path.Bounds.X + ", y: " + path.Bounds.Y);
			}
			
			
		}

		private void center_MouseUp(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
		{

			PNode picked = e.PickedNode;
			PPath path = (PPath)picked;
			MessageBox.Show("x: " + path.Bounds.X + ", y: " + path.Bounds.Y);
		}

		private void composite_MouseEnter(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
		{
			PComposite composite = (PComposite)e.PickedNode;

			PText text = null;

			foreach(PNode child in composite)
			{
				if(child is PText)
				{
					text = (PText)child;
					break;
				}
			}

			text.Visible = true;
		}

		private void composite_MouseLeave(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
		{
			PComposite composite = (PComposite)e.PickedNode;

			PText text = null;

			foreach(PNode child in composite)
			{
				if(child is PText)
				{
					text = (PText)child;
					break;
				}
			}

			text.Visible = false;
		}

		private void orbit_MouseUp(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
		{
			if(e.Button != MouseButtons.Left)
			{
				return;
			}
			PPath orbit = (PPath)e.PickedNode;

			PointF[] points = orbit.PathReference.PathPoints;

			StringBuilder sb = new StringBuilder();

			for(int i = 0; i < points.Length; i++)
			{
				sb.AppendFormat("{0}, {1}", points[i].X, points[i].Y);
				sb.Append("\r\n");
			}

			MessageBox.Show(sb.ToString());
		}
	}

	class BlackLayer : PLayer 
	{
		public BlackLayer() 
		{
		}
		protected override void Paint(PPaintContext paintContext) 
		{
			// make sure grid gets drawn on snap to grid boundaries. And 
			// expand a little to make sure that entire view is filled.
			/*
			float bx = (X - (X % gridSpacing)) - gridSpacing;
			float by = (Y - (Y % gridSpacing)) - gridSpacing;
			float rightBorder = X + Width + gridSpacing;
			float bottomBorder = Y + Height + gridSpacing;
			*/

			Graphics g = paintContext.Graphics;
			RectangleF clip = paintContext.LocalClip;

			g.FillRectangle(Brushes.Black, clip);
		}
	}
}