using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Windows.Forms;
using System.Text;

using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.PiccoloX;
using UMD.HCIL.PiccoloX.Nodes;
using UMD.HCIL.PiccoloX.Components;
using UMD.HCIL.Piccolo.Event;

using BuckRogers.Interface;

namespace BuckRogers
{

	public delegate void TerritoryClickedHandler(object sender, TerritoryClickEventArgs e);

	/// <summary>
	/// Summary description for MapControl.
	/// </summary>
	public class MapControl : System.Windows.Forms.Control
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public event TerritoryClickedHandler TerritoryClicked;

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

		private PCanvas m_canvas;
		private PScrollableControl m_scroller;

		public MapControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			m_canvas = new PCanvas();

			PRoot r = new ScreenshotRoot();
			PLayer l = new BlackLayer();
			PCamera c = new PCamera();
		
			r.AddChild(c); 
			r.AddChild(l); 
			c.AddLayer(l);

			Canvas.Camera = c;

			Canvas.PanEventHandler = new PPanEventHandler();
			Canvas.ZoomEventHandler = new PZoomEventHandler();

			m_canvas.Focus();

			m_scroller = new PScrollableControl(m_canvas);
			m_scroller.Scrollable = true;

			this.SuspendLayout();

			//m_canvas.Size = ClientSize;
			//m_scroller.Size = ClientSize;
			//m_scroller.Location = new Point(0, 0);
			this.Controls.Add(m_scroller);

			m_scroller.Anchor = 
				AnchorStyles.Bottom |
				AnchorStyles.Top |
				AnchorStyles.Left |
				AnchorStyles.Right;

			this.ResumeLayout(false);


			

			/*
			//PRoot root = Canvas.Root;
			
			//root.RemoveChild(camera.GetLayer(0));
			//camera.RemoveLayer(0);
			//BlackLayer bl = new BlackLayer();

			Canvas.Root.AddChild(bl);
			Canvas.Camera.AddLayer(bl);
			//root.AddChild(bl);
			//camera.AddLayer(bl);
			*/

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

			DrawSolarOrbit(m_ao, "AO: ", "Asteroid", 32, /*600*/ 36 * orbitRadiusUnit, Color.Gray, false, false, false);
			DrawSolarOrbit(m_tmo, "TMO: ", "Trans-Mars", 32, /*525*/  32 * orbitRadiusUnit, Color.White, false, true, true);
			DrawSolarOrbit(m_mao,"MaO: ", "Mars", 16, /*450*/ 28 * orbitRadiusUnit, Color.Red, false, true, false);
			DrawSolarOrbit(m_teo, "TEO: ", "Trans-Earth", 16, /*375*/ 24 * orbitRadiusUnit, Color.White, false, true, true);
			DrawSolarOrbit(m_eo, "EO",  "Earth", 8,/*300*/ 19 * orbitRadiusUnit, Color.Blue, false, true, false);
			DrawSolarOrbit(m_vo, "VO: ", "Venus", 4, /*200*/ 12 * orbitRadiusUnit, Color.Green, true, true, false);
			DrawSolarOrbit(m_meo, "MeO: ", "Mercury", 2, /*100*/ 5 * orbitRadiusUnit, Color.Yellow, false, true, false);
			
			
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
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

		
		private void AddPlanets()
		{
			float scaleFactor = 6.0f;

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
					new PointF(110, 15), new PointF(50, 50), new PointF(55, 55), 
					new PointF(50, 60), new PointF(110, 95), 
				},

				new PointF[] 
				{
					new PointF(0, 110), new PointF(0, 105), new PointF(50, 60),  
					new PointF(110, 95), new PointF(110, 110), 
				},

				new PointF[] 
				{
					new PointF(0, 10), new PointF(50, 50), new PointF(55, 55), 
					new PointF(50, 60), new PointF(0, 105)
				},
			};

			string[] moonNames = {"Moscoviense", "Farside", "Tycho", "Tranquility" };

			#endregion

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
					new PointF(160, 30), new PointF(80, 30), new PointF(80, 75), 
					new PointF(100, 75f), new PointF(160, 45)
				},

				new PointF[] 
				{
					new PointF(160, 115), new PointF(80, 115), new PointF(80, 75), 
					new PointF(100, 75),new PointF(160, 45)
				},

				new PointF[] 
				{
					new PointF(70, 115), new PointF(160, 115), new PointF(160, 160), 
					new PointF(70, 160),
				},

				new PointF[] 
				{
					new PointF(0, 85), new PointF(50, 115), new PointF(70, 115), 
					new PointF(70, 160), new PointF(0, 160)
				},

				new PointF[] 
				{
					new PointF(0, 65), new PointF(80, 65), new PointF(80, 115f), 
					new PointF(50, 115), new PointF(0, 85),
				},
	
				new PointF[] 
				{
					new PointF(0, 30f), new PointF(80, 30), new PointF(80, 65f), 
					new PointF(0, 65f),
				},

				

				
			};

			string[] earthNames = {"Independent Arcologies", "Eurasian Regency", "African Regency", 
									"Antarctic Testing Zone",  "Australian Development Facility", 
									"Urban Reservations",	"American Regency"};

			#endregion

			#region Mars regions
			PointF[][] marsPoints = new PointF[][]
			{
				

				new PointF[] 
				{
					new PointF(40, 0), new PointF(40, 40), new PointF(110, 40), 
					new PointF(110, 0),
				},

				new PointF[] 
				{
					new PointF(110, 0), new PointF(150, 0), new PointF(150, 150), 
					new PointF(110, 150),
				},

				new PointF[] 
				{
					new PointF(75, 40), new PointF(110, 40), new PointF(110, 150), 
					new PointF(75, 150), 
				},

				new PointF[] 
				{
					new PointF(40, 40), new PointF(75, 40), new PointF(75, 150), 
					new PointF(40, 150),
				},

				new PointF[] 
				{
					new PointF(0, 0), new PointF(40, 0), new PointF(40, 150), 
					new PointF(0, 150),
				},
			};

			string[] marsNames = { "Boreal Sea", "Pavonis", "Arcadia", 
									"Ram HQ", "Coprates Chasm"};

			#endregion
			
	
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

			#region Territory centers

			PointF[] mercuryCenters = new PointF[] {new PointF(27.41f, 67.41f), new PointF(64.1f, 25.74f),
												  new PointF(105f, 67f), new PointF(75f, 110f)}; 

			PointF[] venusCenters = new PointF[] {new PointF(38f, 30f), new PointF(85f, 28f),
													 new PointF(114f, 65f), new PointF(130f, 110f),
												new PointF(83f, 120f), new PointF(40f, 125f), new PointF(35f, 79f)}; 

			PointF[] moonCenters = new PointF[] {new PointF(53f, 20f), new PointF(80f, 55f),
												new PointF(53f, 90f), new PointF(25f, 55f)}; 

			PointF[] earthCenters = new PointF[] {new PointF(79f, 14f), new PointF(105f, 50f),
													 new PointF(105f, 90f), new PointF(95f, 130f),
													 new PointF(40f, 120f), new PointF(45f, 85f), new PointF(45f, 45f)}; 


			
			PointF[] marsCenters = new PointF[] {new PointF(75f, 19f), new PointF(129f, 74f),
													 new PointF(91.5f, 94f), new PointF(56.5f, 94f),
													 new PointF(19f, 74f)};
			#endregion

			#region Draw planets and planetary orbits
			AddPlanet(70, mercuryPoints, mercuryNames, mercuryCenters, Brushes.Goldenrod, scaleFactor, -2600, -800);
			AddPlanet(80, venusPoints, venusNames, venusCenters, Brushes.LightGreen, scaleFactor, -2600, 1000);
			AddPlanet(80, earthPoints, earthNames, earthCenters, Brushes.LightBlue, scaleFactor, 1900, 1100);
			AddPlanet(55, moonPoints, moonNames, moonCenters, Brushes.LightGray, scaleFactor, 1100, 1300);
			AddPlanet(75, marsPoints, marsNames, marsCenters, Brushes.OrangeRed, scaleFactor, 1800, -550);

			DrawPlanetaryOrbit(mercuryNearOrbitPoints, Color.Yellow, scaleFactor, -2700, -1100, true);
			DrawPlanetaryOrbit(mercuryFarOrbitPoints, Color.Yellow, scaleFactor, -2700, -1100, true);

			DrawPlanetaryOrbit(venusNearOrbitPoints, Color.Green, scaleFactor, -2700, 700, true);
			DrawPlanetaryOrbit(venusFarOrbitPoints, Color.Green, scaleFactor, -2700, 700, true);

			DrawPlanetaryOrbit(moonNearOrbitPoints, Color.Blue, scaleFactor, 1000, 50, true);
			DrawPlanetaryOrbit(earthNearOrbitPoints, Color.Blue, scaleFactor, 1000, 50, true);
			DrawPlanetaryOrbit(earthFarOrbitPoints, Color.Blue, scaleFactor, 1000, 50, true);

			DrawPlanetaryOrbit(marsNearOrbitPoints, Color.Red, scaleFactor, 1000, -1300, true);
			DrawPlanetaryOrbit(marsFarOrbitPoints, Color.Red, scaleFactor, 1000, -1300, true);

			DrawAsteroid(75, 75, scaleFactor, "Ceres", Color.Gray, 30, 40, -200, 1150);
			DrawAsteroid(65, 55, scaleFactor, "Pallas", Color.Gray, -20, 20, 450, 1000);
			DrawAsteroid(70, 45, scaleFactor, "Psyche", Color.LightSteelBlue, 40, 10, 500, 1350);
			DrawAsteroid(70, 60, scaleFactor, "Thule", Color.MediumSlateBlue, 35, 30, 500, 1650);
			DrawAsteroid(60, 60, scaleFactor, "Fortuna", Color.Goldenrod, 40, 25, 0, 1700);
			DrawAsteroid(60, 70, scaleFactor, "Vesta", Color.Gainsboro, 20, -15, -500, 1650);
			DrawAsteroid(60, 60, scaleFactor, "Juno", Color.LightSteelBlue, 30, 30, -1000, 1650);
			DrawAsteroid(60, 60, scaleFactor, "Hygeia", Color.Gold, -25, 15, -800, 1300);
			DrawAsteroid(85, 50, scaleFactor, "Aurora", Color.LightGray, -10, -5, -800, 1000);

			// Hielo and Mariposas
			DrawObject(60, 60, scaleFactor, "Hielo", Color.Yellow, -1600, -350);
			DrawObject(85, 35, scaleFactor, "Mariposas", Color.Yellow, -1700, -750);

			// Deimos
			DrawObject(100, 100, scaleFactor, "Deimos", Color.Gray, 1100, -1000);

			// L4 and L5 colonies
			DrawObject(65, 50, scaleFactor, "L4 Colony", Color.CornflowerBlue, 1350, 600);
			DrawObject(80, 50, scaleFactor, "L5 Colony", Color.CornflowerBlue, 2100, 650);

			#endregion

			

			for(int i = 0; i < m_planets.Length; i++)
			{
				m_planets[i].MoveToFront();
			}

			PPath upperElevator = DrawObject(90, 65, scaleFactor, "", Color.DarkRed, 2200, -1000, false);
			PPath lowerElevator = DrawObject(40, 75, scaleFactor, "", Color.DarkRed, 2450, -800, false);

			PComposite elevator = new PComposite();
			elevator.AddChild(upperElevator);
			elevator.AddChild(lowerElevator);

			elevator.MouseUp +=new UMD.HCIL.Piccolo.PInputEventHandler(text_Click);

			DrawLabelAndOwner(upperElevator, "Space Elevator", 2200, -1000);

			Canvas.Layer.AddChild(elevator);
			
		}

		private void DrawAsteroid(float width, float height, float scaleFactor, string name, Color color, 
			int orbitX, int orbitY, int shiftX, int shiftY)
		{
			PPath asteroid = DrawObject(width, height, scaleFactor, name, color, shiftX, shiftY);


			float centerX = (width / 2) * scaleFactor;
			float centerY = (height / 2) * scaleFactor;

			float orbitDiameter = scaleFactor * 40;
			PPath orbit = PPath.CreateEllipse(0, 0, orbitDiameter, orbitDiameter);

			orbit.Brush = Brushes.Black;
			orbit.Pen = Pens.White;

			orbit.X += shiftX + (orbitX * scaleFactor);
			orbit.Y += shiftY + (orbitY * scaleFactor);

			
			
			Canvas.Layer.AddChild(orbit);

		}

		private PPath DrawObject(float width, float height, float scaleFactor, string name, Color color, int shiftX, int shiftY)
		{
			return DrawObject(width, height, scaleFactor, name, color, shiftX, shiftY, true);
		}
		private PPath DrawObject(float width, float height, float scaleFactor, string name, Color color, int shiftX, int shiftY, bool addChild)
		{
			width *= scaleFactor;
			height *= scaleFactor;
			PPath obj = PPath.CreateEllipse(0, 0, width, height);

			obj.Pen = new Pen(color);
			obj.Brush = new SolidBrush(color);

			obj.X += shiftX;
			obj.Y += shiftY;

			

			if(addChild)
			{
				DrawLabelAndOwner(obj, name, shiftX, shiftY);

				obj.MouseUp +=new UMD.HCIL.Piccolo.PInputEventHandler(text_Click);
				Canvas.Layer.AddChild(obj);
			}
			
			return obj;
		}

		private void DrawLabelAndOwner(PPath parent, string name, int shiftX, int shiftY)
		{
			PPath center = PPath.CreateEllipse(0, 0, 20, 20);
			center.Brush = Brushes.White;
			Pen p = new Pen(Color.Black, 3.0f);
			center.Pen = p;

			float centerX = parent.Width / 2;
			float centerY = parent.Height / 2;

			PointF unshiftedCenter = new PointF();
			unshiftedCenter.X = centerX - (center.Width / 2);
			unshiftedCenter.Y = centerY;

			center.X = unshiftedCenter.X + shiftX;
			center.Y = unshiftedCenter.Y + shiftY;

			PText text = new PText(name);//names[i]);
			text.X = centerX - (text.Width / 2) + shiftX;
			text.Y = centerY - (text.Height) + shiftY;
			text.TextBrush = Brushes.Black;

			parent.AddChild(center);
			parent.AddChild(text);
		}

		private void AddPlanet(float radius, PointF[][] polygons, string[] names, PointF[] territoryCenters, 
								Brush color, float scaleFactor, int shiftX, int shiftY)
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


			
			/*
			cVertexList clc = new cVertexList();
			float deg = 1.0f;

			for(int i = 0; i < 360; i++)
			{
				
				float numDegrees = i * deg - 90;

				float x = -GetCos(numDegrees) * radius;
				float y = GetSin(numDegrees) * radius;
				clc.SetVertex((int)x, (int)y);
			}

			*/

			/*
			PolyDefault pdc = new PolyDefault();
			float deg = 1.0f;
			for(int i = 0; i < 360; i++)
			{
				
				float numDegrees = i * deg - 90;

				float x = -GetCos(numDegrees) * radius;
				float y = GetSin(numDegrees) * radius;
				pdc.add(x, y);
			}
			*/



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

				/*
				ConvConv cc = new ConvConv();
				cVertexList clt = new cVertexList();
				
				foreach(PointF p in points)
				{
					clt.SetVertex((int)p.X, (int)p.Y);
				}

				cc.Start(clt, clc);

				cVertexList intersectedPolygon = cc.IntersectedPolygon;

				for(int j = 0; j < intersectedPolygon.Count; j++)//points.Length; j++)
				{
					cPointi circlePoint = intersectedPolygon.GetElement(j).v;
					totalX += circlePoint.x;//.points[j].X;
					totalY += circlePoint.y;//points[j].Y;
				}

				centerX = totalX / intersectedPolygon.Count;//points.Length;
				centerY = totalY / intersectedPolygon.Count;//points.Length;
				*/

				/*
				PolyDefault pdt = new PolyDefault();
				foreach(PointF p in points)
				{
					pdt.add(p);
				}

				Poly result = Clip.intersection(pdc, pdt);

				for(int j = 0; j < result.NumPoints; j++)
				{
					totalX += (float)result.getX(j);
					totalY += (float)result.getY(j);
				}

				centerX = totalX / result.NumPoints;
				centerY = totalY / result.NumPoints;
				*/

				/*
				for(int j = 0; j < points.Length; j++)
				{
					totalX += points[j].X;
					totalY += points[j].Y;
				}
				*/

				//centerX = totalX / points.Length;
				//centerY = totalY / points.Length;
				
				centerX = territoryCenters[i].X;
				centerY = territoryCenters[i].Y;

				centerX *= scaleFactor;
				centerY *= scaleFactor;

				PPath territory = PPath.CreatePolygon(points);
				territory.Brush = color;
				territory.Pen = Pens.White;


				PPath center = PPath.CreateEllipse(0, 0, 20, 20);
				center.Brush = Brushes.White;
				Pen p = new Pen(Color.Black, 3.0f);
				center.Pen = p;

				PointF unshiftedCenter = new PointF();
				unshiftedCenter.X = centerX - (center.Width / 2);
				unshiftedCenter.Y = centerY;// - (center.Height / 2);

				center.X = unshiftedCenter.X + shiftX;
				center.Y = unshiftedCenter.Y + shiftY;

				//center.X = centerX - (center.Width / 2) + shiftX;
				//center.Y = centerY - (center.Width / 2) + shiftY;

				territory.AddChild(center);

				string label = names[i];// + "\r\n(x: " + (unshiftedCenter.X / scaleFactor)  + ", y: " + (unshiftedCenter.Y / scaleFactor) + ")";
				
				PText text = new PText(label);//names[i]);
				text.X = centerX - (text.Width / 2) + shiftX;
				text.Y = centerY - (text.Height) + shiftY;

				text.TextBrush = Brushes.Black;
				territory.AddChild(text);
				
				territory.MouseUp +=new UMD.HCIL.Piccolo.PInputEventHandler(text_Click);


				territory.X += shiftX;
				territory.Y += shiftY;

				planet.AddChild(territory);
			}
		}


		private void DrawSolarOrbit(PNode[] nodes, string prefix, string orbitName, int numNodes, int radius, 
			Color color, bool drawConnector, bool isTransOrbit)
		{
			DrawSolarOrbit(nodes, prefix, orbitName, numNodes, radius, color, false, drawConnector, isTransOrbit);
		}

		private void DrawSolarOrbit(PNode[] nodes, string prefix, string orbitName, int numNodes, int radius, 
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

				circle.Tag = orbitName + " Orbit: " + i.ToString();

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

				circle.AddChild(text);
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


		private void DrawPlanetaryOrbit(PointF[][] polygons, Color color, float scaleFactor, int shiftX, int shiftY, bool closeOrbit)
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

			//orbit.MouseUp += new PInputEventHandler(orbit_MouseUp);

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

		private void text_Click(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
		{
			if(e.Button != MouseButtons.Left)
			{
				return;
			}
			//PText picked = (PText)e.PickedNode;
			PNode picked = e.PickedNode;

			PPath path = null;
			
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
				
				foreach(PNode node in comp)
				{
					if(node is PPath)
					{
						path = (PPath)node;
						break;
					}
				}
			}
			else if(picked is PPath)
			{
				path = (PPath)picked;
			}

			if(path == null)
			{
				return;
			}

			string territoryName = null;

			if(path.Tag != null)
			{
				territoryName = (string)path.Tag;
			}
			else
			{
				PText text = null;

				foreach(PNode node in path)
				{
					if(node is PText)
					{
						text = (PText)node;
						break;
					}
				}

				if(text == null)
				{
					return;
				}

				//MessageBox.Show(text.Text);

				territoryName = text.Text;
			}
			
			if(TerritoryClicked != null)
			{
				TerritoryClickEventArgs tcea = new TerritoryClickEventArgs(territoryName);

				TerritoryClicked(this, tcea);
			}
				/*
				float scale = Canvas.Camera.ViewScale;
				MessageBox.Show("Scale: " + scale + ", x: " + path.Bounds.X + ", y: " + path.Bounds.Y);
				*/
			
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

		public void DrawScreenshot()
		{
			float oldScale = m_canvas.Camera.ViewScale;
			m_canvas.Camera.ViewScale = 1.0f;

			ScreenshotRoot sr = (ScreenshotRoot)m_canvas.Camera.Root;

			RectangleF shotSize = new RectangleF(0, 0, m_scroller.ViewSize.Width, m_scroller.ViewSize.Height);
			Bitmap b = sr.Screenshot(shotSize);

			b.Save("c:\\temp\\brmap.png", ImageFormat.Png);
		}

		public UMD.HCIL.Piccolo.PCanvas Canvas
		{
			get { return this.m_canvas; }
			set { this.m_canvas = value; }
		}

		public UMD.HCIL.PiccoloX.Components.PScrollableControl ScrollControl
		{
			get { return this.m_scroller; }
			set { this.m_scroller = value; }
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

	class ScreenshotRoot : PRoot
	{
		public Bitmap Screenshot(RectangleF displayRect)
		{
			Bitmap b = new Bitmap((int)displayRect.Width, (int)displayRect.Height);
			Graphics g = Graphics.FromImage(b);
			
			ScaleAndDraw(g, this.UnionOfChildrenBounds, displayRect);

			return b;
		}
	}
}
