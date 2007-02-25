using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Data;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Reflection;

using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.PiccoloX;
using UMD.HCIL.PiccoloX.Activities;
using UMD.HCIL.PiccoloX.Nodes;
using UMD.HCIL.PiccoloX.Components;
using UMD.HCIL.Piccolo.Event;


/*
#if DIRECT3D

using UMD.HCIL.PiccoloDirect3D;
using UMD.HCIL.PiccoloDirect3D.Nodes;
using UMD.HCIL.PiccoloDirect3D.Util;


using PCamera = UMD.HCIL.PiccoloDirect3D.P3Camera;
using PCanvas = UMD.HCIL.PiccoloDirect3D.P3Canvas;
using PForm = UMD.HCIL.PiccoloDirect3D.P3Form;
using PNode = UMD.HCIL.PiccoloDirect3D.P3Node;
using PImage = UMD.HCIL.PiccoloDirect3D.Nodes.P3Image;
using PPath = UMD.HCIL.PiccoloDirect3D.Nodes.P3Path;
using PText = UMD.HCIL.PiccoloDirect3D.Nodes.P3Text;
using PComposite = UMD.HCIL.PiccoloDirect3D.Util.P3Composite;
//using PPaintContext = UMD.HCIL.PiccoloDirect3D.Util.P3PaintContext;

#endif
*/

using GpcWrapper;

namespace BuckRogers.Interface
{

	public delegate void TerritoryClickedHandler(object sender, TerritoryEventArgs e);

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

		private PText m_tooltip;

		private GameController m_controller;

		private PComposite m_iconMercury;
		private PComposite m_iconVenus;
		private PComposite m_iconEarth;
		private PComposite m_iconMars;
		private PComposite[] m_iconAsteroids;

		private Hashtable m_territoryMarkers;
		private Hashtable m_territories;
		private Hashtable m_orbitOffsets;
		private Hashtable m_territoryCenters;

		private ArrayList m_pathArrows;
		private ArrayList m_currentPathTerritories;
		
		//private int m_idxPlanets;
		private PClip[] m_planets;

		private float[] m_zoomFactors;

		private PCanvas m_canvas;
		private RefreshingScrollableControl m_scroller;
		private IconManager m_iconManager;

		private ArrayList m_movementIcons;

		private Timer m_timer;
		private bool m_dragOccurredDuringClick;
		private RectangleF m_doubleClickRectangle;
		private bool m_isFirstClick;
		private PInputEventArgs m_clickInfo;
		private int m_clickMilliseconds;

		private MovePanel m_movePanel;


		public ArrayList MovementIcons
		{
			get { return m_movementIcons; }
			set { m_movementIcons = value; }
		}

		public bool PathArrowsDisplayed
		{
			get
			{
				return m_pathArrows.Count > 0;
			}
		}

		public MovePanel MovePanel
		{
			set
			{
				m_movePanel = value;
			}
		}



		public MapControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			m_iconManager = new IconManager(this);

			m_territoryMarkers = new Hashtable();
			m_territories = new Hashtable();
			m_orbitOffsets = new Hashtable();
			m_territoryCenters = new Hashtable();

			m_movementIcons = new ArrayList();
			m_pathArrows = new ArrayList();
			m_currentPathTerritories = new ArrayList();

			m_timer = new Timer();
			m_timer.Tick += new EventHandler(OnClickTimerTick);
			m_timer.Interval = 50;

			m_isFirstClick = true;

			m_zoomFactors = new float[]{0.1f, 0.175f, 0.25f, 0.5f, 0.75f, 1.0f, 1.5f, 2.0f, 3.0f, 4.0f, 5.0f};

			m_canvas = new PCanvas();
			PRoot r = new ScreenshotRoot();
			PLayer l = new BlackLayer();
			//PLayer l = new PLayer();
			l.Brush = Brushes.Black;
			PCamera c = new PCamera();
		
			r.AddChild(c); 
			r.AddChild(l); 
			c.AddLayer(l);

			Canvas.Camera = c;
			Canvas.PanEventHandler = new PPanEventHandler();
			Canvas.ZoomEventHandler = new PZoomEventHandler();

			Canvas.PanEventHandler.Drag += new DragDelegate(OnDragAction);
			Canvas.ZoomEventHandler.Drag += new DragDelegate(OnDragAction);

			Canvas.PanEventHandler.MinDragStartDistance = 0.5f;
			Canvas.ZoomEventHandler.MinDragStartDistance = 0.5f;

			m_tooltip = new PText();
			m_tooltip.TextBrush = Brushes.White;
			m_tooltip.Pickable = false;
			Font font = m_tooltip.Font;
			m_tooltip.Font = new Font(font.Name, font.SizeInPoints + 8, FontStyle.Bold);
			c.AddChild(m_tooltip);

			PBasicInputEventHandler tipEventHandler = new PBasicInputEventHandler();
			tipEventHandler.MouseMove = new MouseMoveDelegate(MouseMoveHandler);
			c.AddInputEventListener(tipEventHandler);
			
			m_canvas.Focus();

			m_scroller = new RefreshingScrollableControl(m_canvas);
			m_scroller.Scrollable = true;

			m_scroller.MouseWheel += new MouseEventHandler(m_scroller_MouseWheel);
			m_canvas.MouseWheel += new MouseEventHandler(m_scroller_MouseWheel);

			this.SuspendLayout();

			this.Controls.Add(m_scroller);

			m_scroller.Anchor = 
				AnchorStyles.Bottom |
				AnchorStyles.Top |
				AnchorStyles.Left |
				AnchorStyles.Right;

			this.ResumeLayout(false);

			m_orbits = new PNode[7][];
			m_planets = new PClip[5];
			//m_idxPlanets = 0;

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
			//center.MouseUp +=new PInputEventHandler(center_MouseUp);
			center.Brush = Brushes.White;
			RectangleF centerBounds = center.Bounds;

			int centerX = this.Bounds.Width / 2;
			int centerY = this.Bounds.Height / 2;
			centerBounds.X = -5 + centerX;
			centerBounds.Y = -5 + centerY;
			center.Bounds = centerBounds;
			
			Canvas.Layer.AddChild(center);

			DrawConnectors();
			AddPlanets2();

			Canvas.AnimatingRenderQuality = RenderQuality.HighQuality;
			Canvas.InteractingRenderQuality = RenderQuality.HighQuality;

			string[] orbitNames = {"Near Mercury Orbit", "Far Mercury Orbit", "Near Venus Orbit", 
									"Far Venus Orbit","Near Moon Orbit","Near Earth Orbit", 
									"Far Earth/Moon Orbit", "Near Mars Orbit", "Far Mars Orbit"};
			PointF[] orbitCenters = new PointF[]{		new PointF(-1750, 120 ), 
														new PointF(-2070, 475 ), 
														new PointF(-1545, 1140 ), 
														new PointF(-1225, 1200 ), 
														new PointF( 1330, 1160 ), 										
														new PointF( 1865, 1155), 
														new PointF( 1910, 705), 
														new PointF( 1610, 60), 
														new PointF( 1910, -845) 
													};

			for(int i = 0; i < orbitNames.Length; i++)
			{
				m_territoryCenters[orbitNames[i]] = orbitCenters[i];
			}

		}

		void m_scroller_MouseWheel(object sender, MouseEventArgs e)
		{
			//throw new Exception("The method or operation is not implemented.");
			float scaleFactor = 1.0f;

			if(e.Delta > 0)
			{
				scaleFactor = 1.5f;
			}
			else
			{
				scaleFactor = 0.67f;
			}

			CenterZoomedMap(true, scaleFactor, e.X, e.Y);
			
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

		private void LoadTerritoryPolygons()
		{
			Assembly a = Assembly.GetExecutingAssembly();
			Stream stream =
				a.GetManifestResourceStream("BuckRogers.Interface.Other.Resources.territories.txt");
			StreamReader sr = new StreamReader(stream);//"territories.txt");

			string line = sr.ReadLine();
			int numTerritories = Int32.Parse(line);

			char[] pointSeparators = new char[]{','};

			for(int i = 0; i < numTerritories; i++)
			{
				string territoryName = sr.ReadLine();
				string sNumContours = sr.ReadLine();
				int numContours = Int32.Parse(sNumContours);

				Polygon poly = new Polygon();

				for(int j = 0; j < numContours; j++)
				{
					line = sr.ReadLine();

					string[] contourInfo = line.Split(new char[]{':'});

					int numPoints = Int32.Parse(contourInfo[1]);
					bool isHole = Boolean.Parse(contourInfo[2]);

					VertexList points = new VertexList();
					points.Vertex = new Vertex[numPoints];
					points.NofVertices = numPoints;
					for(int k = 0; k < numPoints; k++)
					{
						line = sr.ReadLine();

						string[] splitPoints = line.Split(pointSeparators);

						points.Vertex[k] = new Vertex(Int32.Parse(splitPoints[0]), Int32.Parse(splitPoints[1]));
					}

					poly.AddContour(points, isHole);
				}

				PPath path = new PPath(poly.ToGraphicsPath());
				m_territories[territoryName] = path;				
			}
			sr.Close();
		}
		

		private void AddPlanets2()
		{
			LoadTerritoryPolygons();
			float scaleFactor = 6.0f;

			#region Territory centers

			PointF[] mercuryCenters = new PointF[] {new PointF(27.41f, 67.41f), new PointF(64.1f, 25.74f),
													   new PointF(105f, 67f), new PointF(75f, 110f)}; 

			PointF[] venusCenters = new PointF[] {new PointF(50f, 38f), new PointF(85f, 28f),
													 new PointF(114f, 65f), new PointF(130f, 110f),
													 new PointF(83f, 120f), new PointF(47f, 110f), new PointF(35f, 79f)}; 

			PointF[] moonCenters = new PointF[] {new PointF(53f, 20f), new PointF(75f, 55f),
													new PointF(53f, 90f), new PointF(30f, 55f)}; 

			PointF[] earthCenters = new PointF[] {new PointF(79f, 14f), new PointF(105f, 50f),
													 new PointF(105f, 90f), new PointF(95f, 125f),
													 new PointF(48f, 125f), new PointF(50f, 75f), new PointF(45f, 45f)}; 


			
			PointF[] marsCenters = new PointF[] {new PointF(75f, 19f), new PointF(129f, 74f),
													new PointF(91.5f, 94f), new PointF(56.5f, 94f),
													new PointF(19f, 74f)};
			#endregion

			#region Territory Names

			string[] mercuryNames = {"Bach", "The Warrens", "Tolstoi", "Sobkou Plains"};

			string[] venusNames = {"Aerostates", "Mt. Maxwell", "Elysium", 
									  "Wreckers", "Aphrodite",
									  "Beta Regio", "Lowlanders"};

			string[] earthNames = {"Independent Arcologies", "Eurasian Regency", "African Regency", 
									  "Antarctic Testing Zone",  "Australian Development Facility", 
									  "Urban Reservations",	"American Regency"};

			string[] moonNames = {"Moscoviense", "Farside", "Tycho", "Tranquility" };

			string[] marsNames = { "Boreal Sea", "Pavonis", "Arcadia", 
									 "Ram HQ", "Coprates Chasm"};
			#endregion


			DrawPlanetaryOrbit2("Near Mercury Orbit", Color.Yellow, scaleFactor, -2700, -1100, true);
			DrawPlanetaryOrbit2("Far Mercury Orbit", Color.Yellow, scaleFactor, -2700, -1100, true);

			DrawPlanetaryOrbit2("Near Venus Orbit", Color.Green, scaleFactor, -2700, 700, true);
			DrawPlanetaryOrbit2("Far Venus Orbit", Color.Green, scaleFactor, -2700, 700, true);

			DrawPlanetaryOrbit2("Near Moon Orbit", Color.Blue, scaleFactor, 1000, 50, true);
			DrawPlanetaryOrbit2("Near Earth Orbit", Color.Blue, scaleFactor, 1000, 50, true);
			DrawPlanetaryOrbit2("Far Earth/Moon Orbit", Color.Blue, scaleFactor, 1000, 50, true);

			DrawPlanetaryOrbit2("Near Mars Orbit", Color.Red, scaleFactor, 1000, -1300, true);
			DrawPlanetaryOrbit2("Far Mars Orbit", Color.Red, scaleFactor, 1000, -1300, true);
			
			
			DrawAsteroid2(scaleFactor, "Ceres", Color.Gray, -200, 1150);
			DrawAsteroid2(scaleFactor, "Pallas", Color.Gray, 450, 1000);
			DrawAsteroid2(scaleFactor, "Psyche", Color.LightSteelBlue, 500, 1350);
			DrawAsteroid2(scaleFactor, "Thule", Color.MediumSlateBlue, 500, 1650);
			DrawAsteroid2(scaleFactor, "Fortuna", Color.Goldenrod, 0, 1700);
			DrawAsteroid2(scaleFactor, "Vesta", Color.Gainsboro, -500, 1650);
			DrawAsteroid2(scaleFactor, "Juno", Color.LightSteelBlue, -1000, 1650);
			DrawAsteroid2(scaleFactor, "Hygeia", Color.Gold, -800, 1300);
			DrawAsteroid2(scaleFactor, "Aurora", Color.LightGray, -800, 1000);
			
			AddPlanet2(mercuryNames, mercuryCenters, Brushes.Goldenrod, scaleFactor, -2600, -800);
			AddPlanet2(venusNames, venusCenters, Brushes.LightGreen, scaleFactor, -2600, 1000);
			AddPlanet2(earthNames, earthCenters, Brushes.LightBlue, scaleFactor, 1900, 1100);
			AddPlanet2(moonNames, moonCenters, Brushes.LightGray, scaleFactor, 1100, 1300);
			AddPlanet2(marsNames, marsCenters, Brushes.OrangeRed, scaleFactor, 1800, -550);

			object[][] satelliteInfo = new object[][]{ new object[]{"Hielo", Color.Yellow, -1600, -350},
														 new object[]{"Mariposas", Color.Yellow, -1700, -750},
														 new object[]{"Deimos", Color.Gray, 1100, -1000},
														 new object[]{"L-4 Colony", Color.CornflowerBlue, 1350, 600},
														 new object[]{"L-5 Colony", Color.CornflowerBlue, 2100, 650},
			};

			for(int i = 0; i < satelliteInfo.Length; i++)
			{
				object[] info = satelliteInfo[i];
				string name = (string)info[0];
				Color color = (Color)info[1];
				PPath path = (PPath)m_territories[name];
				path.Brush = new SolidBrush(color);
				int x = (int)info[2];
				int y = (int)info[3];

				PComposite composite = new PComposite();
				composite.Tag = name;
				composite.AddChild(path);
				Canvas.Layer.AddChild(composite);

				DrawLabelAndOwner2(path, name, x, y);

				composite.MouseUp += new PInputEventHandler(OnTerritoryMouseUp);
			}

			PPath elevator = (PPath)m_territories["Space Elevator"];
			elevator.Brush = Brushes.DarkRed;
			PComposite elevatorParent = new PComposite();
			elevatorParent.AddChild(elevator);
			elevatorParent.Tag = "Space Elevator";
			Canvas.Layer.AddChild(elevatorParent);

			elevatorParent.MouseUp += new PInputEventHandler(OnTerritoryMouseUp);

			DrawLabelAndOwner2(elevator, "Space Elevator", 2200, -1000);
			PNode elevatorLabel = (PNode)elevator[0];
			PNode elevatorMarker = (PNode)elevator[1];
			elevatorLabel.X = 2377;
			elevatorLabel.Y = -834;
			elevatorMarker.X = 2449;
			elevatorMarker.Y = -805;
		}
	

		private void AddPlanet2(string[] territoryNames, PointF[] territoryCenters, Brush color, 
			float scaleFactor, int shiftX, int shiftY)
		{
			for(int i = 0; i < territoryNames.Length; i++)
			{
				PPath territory = (PPath)m_territories[territoryNames[i]];

				territory.Brush = color;
				territory.Pen = Pens.White;

				float centerX = territoryCenters[i].X;
				float centerY = territoryCenters[i].Y;
				centerX *= scaleFactor;
				centerY *= scaleFactor;

				PPath center = PPath.CreateEllipse(0, 0, 40, 40);
				center.Brush = Brushes.White;
				Pen p = new Pen(Color.Black, 3.0f);
				center.Pen = p;
				center.Tag = territoryNames[i];
				m_territoryMarkers[territoryNames[i]] = center;

				PointF unshiftedCenter = new PointF();
				unshiftedCenter.X = centerX - (center.Width / 2);
				unshiftedCenter.Y = centerY;// - (center.Height / 2);

				center.X = unshiftedCenter.X + shiftX;
				center.Y = unshiftedCenter.Y + shiftY;

				m_territoryCenters[territoryNames[i]] = new PointF(center.X, center.Y);

				string label = territoryNames[i];
				
				PText text = new PText(label);
				Font f = text.Font;
				text.Font = new Font(f.Name, f.SizeInPoints + 6, FontStyle.Bold);

				if(text.Bounds.Width > 280)
				{
					RectangleF bounds = text.Bounds;
					bounds.Width = 280;
					text.ConstrainWidthToTextWidth = false;
					text.TextAlignment = StringAlignment.Center;
					text.Bounds = bounds;
				}
				text.X = centerX - (text.Width / 2) + shiftX;
				text.Y = centerY - (text.Height) + shiftY;
				text.Tag = label;
				text.TextBrush = Brushes.Black;

				PComposite compTerritory = new PComposite();
				compTerritory.Tag = label;
				compTerritory.AddChild(territory);
				compTerritory.AddChild(text);
				compTerritory.AddChild(center);

				compTerritory.MouseUp += new PInputEventHandler(OnTerritoryMouseUp);
				Canvas.Layer.AddChild(compTerritory);
			}
		}

		void OnTerritoryMouseUp(object sender, PInputEventArgs e)
		{
			if(m_dragOccurredDuringClick)
			{
				m_isFirstClick = true;
				m_dragOccurredDuringClick = false;
				return;
			}

			if (m_isFirstClick)
			{
				m_isFirstClick = false;

				// Determine the location and size of the double click 
				// rectangle area to draw around the cursor point.
				m_doubleClickRectangle = new RectangleF(
					e.Position.X - (SystemInformation.DoubleClickSize.Width / 2),
					e.Position.Y - (SystemInformation.DoubleClickSize.Height / 2),
					SystemInformation.DoubleClickSize.Width,
					SystemInformation.DoubleClickSize.Height);
				//Invalidate();

				m_clickInfo = e;

				// Start the double click timer.
				m_timer.Start();
			}

			// This is the second mouse click.
			else
			{
				// Verify that the mouse click is within the double click
				// rectangle and is within the system-defined double 
				// click period.
				
				if (m_doubleClickRectangle.Contains(e.Position) &&
					m_clickMilliseconds < 350)//SystemInformation.DoubleClickTime)
				{
					m_timer.Stop();

					//m_isDoubleClick = true;
					m_isFirstClick = true;
					m_clickMilliseconds = 0;

					UserClickedMouse(true, e);	
				}
			}
		}

		private void UserClickedMouse(bool doubleClicked, PInputEventArgs e)
		{
			/*
			if(doubleClicked)
			{
				m_doubleClicks++;
			}
			else
			{
				m_singleClicks++;
			}

			UpdateToolTip(e);	
			*/

			// make sure we only handle actual clicks



			if (Canvas.PanEventHandler.Dragging
				|| Canvas.ZoomEventHandler.Dragging)
			{
				return;
			}

			PNode picked = (PNode)e.PickedNode;

			string territoryName = (string)picked.Tag;

			Territory t = m_controller.Map[territoryName];
			PPath territoryPath = (PPath)m_territories[territoryName];

			if (territoryName == null)
			{
				return;
			}

			if (TerritoryClicked != null)
			{
				TerritoryEventArgs tcea = new TerritoryEventArgs(territoryName);
				tcea.Button = e.Button;
				tcea.Tag = e;
				tcea.DoubleClick = doubleClicked;
				tcea.Modifiers = e.Modifiers;

				TerritoryClicked(this, tcea);
			}			
		}

		void OnDragAction(object sender, PInputEventArgs e)
		{
			m_dragOccurredDuringClick = true;
		}

		void OnClickTimerTick(object sender, EventArgs e)
		{
			m_clickMilliseconds += m_timer.Interval;

            // The timer has reached the double click time limit.

			int timeout = 350;//SystemInformation.DoubleClickTime;
			if (m_clickMilliseconds >= timeout)
			{
				m_timer.Stop();

				m_isFirstClick = true;
				m_clickMilliseconds = 0;

				UserClickedMouse(false, m_clickInfo);
			}
		}

		private void DrawAsteroid2(float scaleFactor, string name, Color color, int shiftX, int shiftY)
		{
			PPath asteroid = (PPath)m_territories[name];
			string orbitName = name + " Orbit";
			PPath orbit = (PPath)m_territories[orbitName];

			m_territoryCenters[orbitName] = PUtil.CenterOfRectangle(orbit.FullBounds);

			PComposite asteroidParent = new PComposite();
			asteroidParent.Tag = name;
			asteroidParent.AddChild(asteroid);
			Canvas.Layer.AddChild(asteroidParent);

			PComposite orbitParent = new PComposite();
			orbitParent.Tag = orbitName;
			orbitParent.AddChild(orbit);
			Canvas.Layer.AddChild(orbitParent);


			DrawLabelAndOwner2(asteroid, name, shiftX, shiftY);

			asteroid.Brush = new SolidBrush(color);
			asteroid.Pen = Pens.White;

			orbit.Brush = Brushes.Black;
			orbit.Pen = Pens.White;

			orbitParent.MouseUp += new PInputEventHandler(OnTerritoryMouseUp);
			asteroidParent.MouseUp += new PInputEventHandler(OnTerritoryMouseUp);

		}
		
		
		private void DrawPlanetaryOrbit2(string name, Color color, float scaleFactor, int shiftX, int shiftY, bool closeOrbit)
		{
			PPath orbit = (PPath)m_territories[name];

			PComposite parent = new PComposite();
			parent.Tag = name;
			parent.AddChild(orbit);
			Canvas.Layer.AddChild(parent);

			orbit.Pen = new Pen(color);
			orbit.Brush = Brushes.Black;

			orbit.OffsetX += shiftX;
			orbit.OffsetY += shiftY;

			m_orbitOffsets[name] = new PointF(shiftX, shiftY);

			parent.MouseUp += new PInputEventHandler(OnTerritoryMouseUp);
		}

		private void DrawLabelAndOwner2(PPath parent, string name, int shiftX, int shiftY)
		{
			PPath center = PPath.CreateEllipse(0, 0, 40, 40);
			center.Brush = Brushes.White;
			Pen p = new Pen(Color.Black, 3.0f);
			center.Pen = p;
			center.Tag = name;

			float centerX = parent.Width / 2;
			float centerY = parent.Height / 2;

			PointF unshiftedCenter = new PointF();
			unshiftedCenter.X = centerX - (center.Width / 2);
			unshiftedCenter.Y = centerY;

			center.X = unshiftedCenter.X + shiftX;
			center.Y = unshiftedCenter.Y + shiftY;

			m_territoryCenters[name] = new PointF(center.X, center.Y);

			PText text = new PText(name);//names[i]);

			text.TextBrush = Brushes.Black;
			Font f = text.Font;
			text.Font = new Font(f.Name, f.SizeInPoints + 6, FontStyle.Bold);
			text.X = centerX - (text.Width / 2) + shiftX;
			text.Y = centerY - (text.Height) + shiftY;
			text.Tag = name;

			m_territoryMarkers[name] = center;

			parent.AddChild(text);
			parent.AddChild(center);
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


			for(int i = 0; i < numNodes; i++)
			{
				composite = new PComposite();
				
				text = new PText(prefix + " " + i.ToString());
				text.TextBrush = Brushes.White;
				circle = PPath.CreateEllipse(0f, 0f, 10f, 10f);
				circle.Brush = new SolidBrush(color);//Brushes.Black;
				circle.Pen = Pens.Black;

				circle.Tag = orbitName + " Orbit: " + i.ToString();

				nodes[i] = circle;

				composite.MouseUp += new UMD.HCIL.Piccolo.PInputEventHandler(OnTerritoryMouseUp);

				float numDegrees = i * deg - 90;

				if(rotate45)
				{
					numDegrees += 45;
				}

				float x = -Utility.GetCos(numDegrees) * radius;
				float y = Utility.GetSin(numDegrees) * radius;

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

				string tag = orbitName + " Orbit: " + i.ToString();

				m_territoryCenters[tag] = PUtil.CenterOfRectangle(circleBounds);

				text.Tag = tag;
				circle.Tag = tag;
				composite.Tag = tag;

				m_territories[tag] = circle;
				
				Canvas.Layer.AddChild(composite);
			}
		}


		/*
		private void DrawPlanetaryOrbit(PointF[][] polygons, string name, Color color, float scaleFactor, int shiftX, int shiftY, bool closeOrbit)
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

			orbit.OffsetX = shiftY;
			orbit.OffsetY = shiftY;

			orbit.Tag = name;

			orbit.MouseUp +=new UMD.HCIL.Piccolo.PInputEventHandler(text_Click);

			Canvas.Layer.AddChild(orbit);

			
		}
		*/
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

						line1.Pen = Pens.White;

						Canvas.Layer.AddChild(line1);
						origin.Parent.MoveToFront();
						destination1.Parent.MoveToFront();
					}
				}
			}
		}


		public void MouseMoveHandler(object sender, PInputEventArgs e) 
		{
			UpdateMovementIcons(e);
			UpdateToolTip(e);

			if(m_movePanel.MoveMode == MoveMode.StartMove
				&& m_movePanel.CurrentMoveTerritories.Count > 0)
			{
				UpdateMovementArrows(e);
			}
			//Canvas.Refresh();
		}

		private void UpdateMovementArrows(PInputEventArgs e)
		{
			PNode n = (PNode)e.InputManager.MouseOver.PickedNode;

			if (!(n.Tag is string))
			{
				ClearPathArrows();
				return;
			}

			ArrayList currentMoveTerritories = m_movePanel.CurrentMoveTerritories;
			Territory startingTerritory = (Territory)currentMoveTerritories[0];

			string territoryName = n.Tag as string;

			// if this is somehow a territory we don't have info on, or if
			// we're just starting the move and we're over the starting territory, don't
			// bother drawing any path arrows
			if(!m_territoryCenters.ContainsKey(territoryName)
				|| (currentMoveTerritories.Count == 1 
					&& territoryName == startingTerritory.Name) )
			{
				ClearPathArrows();
				return;
			}			

			ArrayList newPathTerritories = new ArrayList();

			newPathTerritories.AddRange(currentMoveTerritories);

			Territory lastTerritoryAdded = (Territory)currentMoveTerritories[currentMoveTerritories.Count - 1];
			Territory hoveredTerritory = m_controller.Map[territoryName];

			ArrayList shortestPathTerritories = m_controller.Map.Graph.ShortestPath(lastTerritoryAdded, hoveredTerritory);

			newPathTerritories.AddRange(shortestPathTerritories);
			
			bool identicalPath = true;

			if(newPathTerritories.Count == m_currentPathTerritories.Count)
			{
				for(int i = 0; i < newPathTerritories.Count; i++)
				{
					Territory t1 = (Territory)newPathTerritories[i];
					Territory t2 = (Territory)m_currentPathTerritories[i];

					if(t1 != t2)
					{
						identicalPath = false;
						break;
					}
				}
			}
			else
			{
				identicalPath = false;
			}
			
			if(identicalPath)
			{
				// We should already be showing this path.  Don't do anything
				return;
			}
			else
			{
				ClearPathArrows();
				ShowPathArrows(newPathTerritories);

				m_currentPathTerritories.Clear();
				m_currentPathTerritories.AddRange(newPathTerritories);
			}

		}

		public void UpdateMovementIcons(PInputEventArgs e)
		{
			//foreach(IconInfo info in m_movementIcons)
			//{
				//info.Icon.X += e.Delta.Width;
				//info.Label.X += e.Delta.Width;

				//info.Icon.Y += e.Delta.Height;
				//info.Label.Y += e.Delta.Height;

				PointF p = e.CanvasPosition;

				p = e.Path.CanvasToLocal(p, Canvas.Camera);

				for(int i = 0; i < m_movementIcons.Count; i++)
				{
					IconInfo info = (IconInfo)m_movementIcons[i];

					float x = p.X + 8 + (i * 30);

					info.Icon.Y = p.Y * 2;
					info.Icon.X = x * 2;

					info.Label.Y = (p.Y * 2 + 50);
					//info.Label.X = ((x + 18 - (info.Label.Width / 2)) * 2);
					info.Label.X = info.Icon.X;

				}
			//}
		}

		public void UpdateToolTip(PInputEventArgs e) 
		{
			PNode n = (PNode)e.InputManager.MouseOver.PickedNode;

			String tooltipString = null;
			
			if(n.Tag is IconInfo)
			{
				n = e.InputManager.MouseOver.NextPickedNode;
			}
			else if(n.Tag is string)
			{
				tooltipString = (string)n.Tag;
			}


			if(tooltipString != null)
			{
				int idx = tooltipString.IndexOf(':');
				if(idx != -1)
				{
					string[] split = tooltipString.Split(new char[]{':'});
					split[1].Trim();

					int orbitIndex = Int32.Parse(split[1]);

					tooltipString = m_controller.Map.GetPlanetTag(split[0], orbitIndex);

					
				}
			}


			if(tooltipString == null)
			{
				m_tooltip.Visible = false;
				return;
			}
			else
			{
				PointF p = e.CanvasPosition;

				p = e.Path.CanvasToLocal(p, Canvas.Camera);

				//tooltipString = string.Format("{0}\nsingle clicks: {1}\ndouble clicks: {2}",
				//									tooltipString, m_singleClicks, m_doubleClicks);
				
				if(m_tooltip.Text != tooltipString)
				{
					m_tooltip.Text = tooltipString;

					if(m_tooltip.Bounds.Width > 350)
					{
						RectangleF bounds = m_tooltip.Bounds;
						bounds.Width = 300;
						m_tooltip.ConstrainWidthToTextWidth = false;
						m_tooltip.TextAlignment = StringAlignment.Center;
						m_tooltip.Bounds = bounds;
					}
					else
					{
						m_tooltip.ConstrainWidthToTextWidth = true;
						m_tooltip.TextAlignment = StringAlignment.Near;
					}
				}
				
				float x = p.X - (m_tooltip.Width / 2);
				float y = p.Y - m_tooltip.Height - 8;

				m_tooltip.SetOffset((int)x, (int)y);
				m_tooltip.Visible = true;
				
				RectangleF tipBounds = m_tooltip.Bounds;
				m_tooltip.RepaintFrom(tipBounds, m_tooltip);
			}			
		}

		private void text_Click(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
		{
			// make sure we only handle actual clicks
			if(Canvas.PanEventHandler.Dragging
				|| Canvas.ZoomEventHandler.Dragging)
			{
				return;
			}

			PNode picked = (PNode)e.PickedNode;
			
			string territoryName = (string)picked.Tag;

			if(territoryName == null)
			{
				return;
			}

			//Stack s = e.Path.NodeStackReference;
			
			
			
			if(TerritoryClicked != null)
			{
				TerritoryEventArgs tcea = new TerritoryEventArgs(territoryName);
				tcea.Button = e.Button;
				tcea.Tag = e;

				TerritoryClicked(this, tcea);
			}			
		}

		/*
		private void center_MouseUp(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
		{
			PNode picked = (PNode)e.PickedNode;
			PPath path = (PPath)picked;
			MessageBox.Show("x: " + path.Bounds.X + ", y: " + path.Bounds.Y);
		}
		*/


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

		public void ZoomIn()
		{
			/*
			// Save the details from before the zoom
			float currentZoom = Canvas.Camera.ViewScale;
			Point originalPosition = ScrollControl.ViewPosition;
			Size originalSize = ScrollControl.ViewSize;

			float previousZoom = 0.0f;
			float newZoom = 0.0f;
			bool foundZoom = false;

			// Find the next smallest zoom factor
			foreach(float zoom in m_zoomFactors)
			{
				if(zoom > currentZoom)
				{
					newZoom = zoom;
					foundZoom = true;
					break;
				}
				previousZoom = zoom;
			}

			// Change the zoom if necessary
			if(foundZoom)
			{
				//Canvas.Camera.ViewScale = newZoom;
			}
		
			//CenterZoomedMap(newZoom, currentZoom, originalPosition, originalSize);
			*/
			CenterZoomedMap(true, 1.5f);
		}

		public void ZoomOut()
		{
			/*
			// Save the details from before the zoom
			Point originalPosition = ScrollControl.ViewPosition;
			Size originalSize = ScrollControl.ViewSize;
			float currentZoom = Canvas.Camera.ViewScale;

			float newZoom = 0.0f;
			bool foundZoom = false;

			// Find the next largest zoom factor
			for(int i = m_zoomFactors.Length - 1; i >= 0; i--)
			{
				float zoom = m_zoomFactors[i];

				if(currentZoom > zoom)
				{			
					newZoom = zoom;
					foundZoom = true;
					break;
				}
			}

			if(!foundZoom)
			{
				newZoom = currentZoom;
			}

			*/
			CenterZoomedMap(false, 0.67f);
		}

		public void DefaultZoom()
		{
			Canvas.Camera.ViewScale = 0.25f;
		}

		private void CenterZoomedMap(bool zoomIn, float scaleFactor, float x, float y)
		{
			Canvas.Camera.ScaleViewBy(scaleFactor, x, y);
			Canvas.Refresh();
		}

		private void CenterZoomedMap(bool zoomIn, float scaleFactor)
		{
			PointF boundsCenter = PUtil.CenterOfRectangle(Canvas.Camera.Bounds);
			PointF actualCenter = Canvas.Camera.LocalToView(boundsCenter);
			CenterZoomedMap(zoomIn, scaleFactor, actualCenter.X, actualCenter.Y);
		}

		public void CenterMapOn(string target)
		{
			PointF targetLocation = new Point();
			Size viewSize = ScrollControl.ViewSize;
			float zoom = Canvas.Camera.ViewScale;
			float originalWidth = viewSize.Width / zoom;
			float originalHeight = viewSize.Height / zoom;

			RectangleF cameraBounds = Canvas.Camera.UnionOfChildrenBounds;

			switch(target)
			{
				case "Sun":
				{
					targetLocation.X = 2700;
					targetLocation.Y = 1300;
					break;
				}
				case "Mercury":
				{
					targetLocation.X = 400;
					targetLocation.Y = 710;
					break;
				}
				case "Venus":
				{
					targetLocation.X = 500;
					targetLocation.Y = 2700;
					break;
				}
				case "Earth":
				{
					targetLocation.X = 4900;
					targetLocation.Y = 2640;
					break;
				}
				case "Mars":
				{
					targetLocation.X = 4700;
					targetLocation.Y = 1025;
					break;
				}
				case "Asteroids":
				{
					targetLocation.X = 2600;
					targetLocation.Y = 2600;
					break;
				}
			}

			targetLocation.X *= zoom;
			targetLocation.Y *= zoom;

			PointF local = Canvas.Camera.GlobalToLocal(targetLocation);
			PointF view = Canvas.Camera.LocalToView(local);
			Point ulCorner = new Point((int)targetLocation.X, (int)targetLocation.Y);
			
			RectangleF bounds = Canvas.Camera.Bounds;
			ulCorner.X -= (int)(bounds.Width / 2);
			ulCorner.Y -= (int)(bounds.Height / 2);

			
			if(ulCorner.X < 0)
			{
				ulCorner.X = 0;
			}

			if(ulCorner.Y < 0)
			{
				ulCorner.Y = 0;
			}
			

			if(ulCorner.X + bounds.Width > viewSize.Width)
			{
				ulCorner.X = (int)(viewSize.Width - bounds.Width);
			}

			if(ulCorner.Y + bounds.Height > viewSize.Height)
			{
				ulCorner.Y = (int)(viewSize.Height - bounds.Height);
			}
			
			Point oldPosition = ScrollControl.ViewPosition;
			ScrollControl.ViewPosition = ulCorner;
			Canvas.Refresh();
		}

		public void SetTerritoryOwner(object sender, TerritoryEventArgs tea)
		{
			PPath marker = (PPath)m_territoryMarkers[tea.Name];

			if(marker != null)
			{
				marker.Brush = new SolidBrush(tea.Owner.Color);
			}
		}

		public void PlacePlanetIcons()
		{
			int iconRadius = 50;
			m_iconMercury = new PComposite();
			PPath mercCircle = PPath.CreateEllipse(0, 0, iconRadius, iconRadius);
			
			mercCircle.Brush = new HatchBrush(HatchStyle.DiagonalCross, Color.Black, Color.Yellow);
			mercCircle.Pen = Pens.White;
			PText mercName = new PText("Mercury");
			mercName.TextBrush = new SolidBrush(Color.White);
			mercName.X = iconRadius - (mercName.Width / 2);
			mercName.Y = iconRadius + mercName.Height + 4;
			m_iconMercury.AddChild(mercCircle);
			m_iconMercury.AddChild(mercName);
			m_iconMercury.Tag = "Mercury";

			m_iconVenus = new PComposite();
			PPath venusCircle = PPath.CreateEllipse(0, 0, iconRadius, iconRadius);
			venusCircle.Brush = new HatchBrush(HatchStyle.DiagonalCross, Color.Black, Color.Green);
			venusCircle.Pen = Pens.White;
			PText venusName = new PText("Venus");
			venusName.TextBrush = new SolidBrush(Color.White);
			venusName.X = iconRadius - (venusName.Width / 2);
			venusName.Y = iconRadius + venusName.Height + 4;
			m_iconVenus.AddChild(venusCircle);
			m_iconVenus.AddChild(venusName);
			m_iconVenus.Tag = "Venus";
			

			m_iconEarth = new PComposite();
			PPath earthCircle = PPath.CreateEllipse(0, 0, iconRadius, iconRadius);
			earthCircle.Brush = new HatchBrush(HatchStyle.DiagonalCross, Color.Black, Color.Blue);
			earthCircle.Pen = Pens.White;
			PText earthName = new PText("Earth");
			earthName.TextBrush = new SolidBrush(Color.White);
			earthName.X = iconRadius - (earthName.Width / 2);
			earthName.Y = iconRadius + earthName.Height + 4;
			m_iconEarth.AddChild(earthCircle);
			m_iconEarth.AddChild(earthName);
			m_iconEarth.Tag = "Earth";

			m_iconMars = new PComposite();
			PPath marsCircle = PPath.CreateEllipse(0, 0, iconRadius, iconRadius);
			marsCircle.Brush = new HatchBrush(HatchStyle.DiagonalCross, Color.Black, Color.Red);
			marsCircle.Pen = Pens.White;
			PText marsName = new PText("Mars");
			marsName.TextBrush = new SolidBrush(Color.White);
			marsName.X = iconRadius - (marsName.Width / 2);
			marsName.Y = iconRadius + marsName.Height + 4;
			m_iconMars.AddChild(marsCircle);
			m_iconMars.AddChild(marsName);
			m_iconMars.Tag = "Mars";
			

			Canvas.Layer.AddChild(m_iconMercury);
			Canvas.Layer.AddChild(m_iconVenus);
			Canvas.Layer.AddChild(m_iconEarth);
			Canvas.Layer.AddChild(m_iconMars);

			m_iconAsteroids = new PComposite[9];
			string[] asteroidNames = {"Ceres", "Pallas", "Psyche", "Thule", "Fortuna", "Vesta", "Juno", "Hygeia", "Aurora"};
			for(int i = 0; i < 9; i++)
			{
				m_iconAsteroids[i] = new PComposite();
				PPath asteroidCircle = PPath.CreateEllipse(0, 0, iconRadius, iconRadius);
				asteroidCircle.Brush = new HatchBrush(HatchStyle.DiagonalCross, Color.Black, Color.Gray);
				asteroidCircle.Pen = Pens.White;
				m_iconAsteroids[i].AddChild(asteroidCircle);
				PText asteroidName = new PText(asteroidNames[i]);
				asteroidName.TextBrush = new SolidBrush(Color.White);
				asteroidName.X = iconRadius - (asteroidName.Width / 2);
				asteroidName.Y = iconRadius + asteroidName.Height + 4;

				m_iconAsteroids[i].AddChild(asteroidCircle);
				m_iconAsteroids[i].AddChild(asteroidName);
				m_iconAsteroids[i].Tag = asteroidNames[i];
				Canvas.Layer.AddChild(m_iconAsteroids[i]);

			}

			AdvancePlanets();
		}

		public void AdvancePlanets()
		{
			object[][] groups = {	new object[]{"Mercury", m_meo, m_iconMercury},
									new object[]{"Venus", m_vo, m_iconVenus}, 
									new object[]{"Earth", m_eo, m_iconEarth},
									new object[]{"Mars", m_mao, m_iconMars},
							   };

			for(int i = 0; i < groups.Length; i++)
			{
				string name = (string)groups[i][0];
				OrbitalSystem os = (OrbitalSystem)m_controller.Map.Planets[name];
				PNode[] orbit = (PNode[])groups[i][ 1];
				PNode node = (PNode)orbit[os.CurrentOrbitIndex].AllNodes[0];
				PNode composite = (PNode)groups[i][ 2];
				PNode icon = (PNode)composite[0];
				PNode text = (PNode)composite[1];

				icon.CenterFullBoundsOnPoint(node.X + node.Width / 2, node.Y + node.Height / 2);
				text.CenterFullBoundsOnPoint(node.X + node.Width / 2, node.Y + 40 + node.Height / 2);
				node.Parent.MoveInFrontOf(composite);

			}

			string[] asteroidNames = {"Ceres", "Pallas", "Psyche", "Thule", "Fortuna", "Vesta", "Juno", "Hygeia", "Aurora"};
			for(int i = 0; i < m_iconAsteroids.Length; i++)
			{
				OrbitalSystem asteroid = (OrbitalSystem)m_controller.Map.Planets[asteroidNames[i]];
				PNode node = (PNode)m_ao[asteroid.CurrentOrbitIndex].AllNodes[0];
				PComposite composite = (PComposite)m_iconAsteroids[i];
				PNode icon = (PNode)composite[0];
				PNode text = (PNode)composite[1];

				icon.CenterFullBoundsOnPoint(node.X + node.Width / 2, node.Y + node.Height / 2);
				text.CenterFullBoundsOnPoint(node.X + node.Width / 2, node.Y + 40 + node.Height / 2);
				node.Parent.MoveInFrontOf(composite);

				node.Parent.MoveInFrontOf(m_iconAsteroids[i]);
			}
			
		}

		public void UpdateUnitInfo(object sender, TerritoryUnitsEventArgs tuea)
		{
			ArrayList players = tuea.Units.GetPlayersWithUnits();

			foreach(Player p in players)
			{
				UnitCollection uc = tuea.Units.GetUnits(p);
				//uc = uc.GetUnits(tuea.Territory);
				Hashtable ht = uc.GetUnitTypeCount();

				foreach(UnitType ut in ht.Keys)
				{
					m_iconManager.UpdateIconInfo(tuea.Territory, p, ut);
				}
			}

			Canvas.Refresh();
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
			set { this.m_canvas = (PCanvas)value; }
		}

		public RefreshingScrollableControl ScrollControl
		{
			get { return this.m_scroller; }
			set { this.m_scroller = value; }
		}

		public BuckRogers.GameController GameController
		{
			get { return this.m_controller; }
			set { this.m_controller = value; }
		}

		public BuckRogers.Interface.IconManager IconManager
		{
			get { return this.m_iconManager; }
			set { this.m_iconManager = value; }
		}

		public System.Collections.Hashtable Territories
		{
			get { return this.m_territories; }
			set { this.m_territories = value; }
		}


		public void ShowPathArrows(ArrayList al)
		{
			if(al.Count == 1)
			{
				return;
			}

			for(int i = 0; i < al.Count - 1; i++)
			{
				PPath line = new PPath();
				Pen newPen = (Pen)line.Pen.Clone();
				line.Pen = newPen;

				line.Pen.Color = Color.Red;
				line.Pen.StartCap = LineCap.RoundAnchor;
				line.Pen.EndCap = LineCap.Custom;
				line.Pen.CustomEndCap = new AdjustableArrowCap(3, 4, true);
				line.Pen.Width = 10;


				Territory startingTerritory = (Territory)al[i];
				Territory endingTerritory = (Territory)al[i + 1];

				if( (startingTerritory.IsSolarTerritory && !endingTerritory.IsSolarTerritory)
					|| (!startingTerritory.IsSolarTerritory && endingTerritory.IsSolarTerritory))
				{
					line.Pen.DashStyle = DashStyle.Dash;
					line.Pen.DashOffset = 75;
					line.Pen.Width = 7.5f;
				}

				PointF startingPoint = (PointF)m_territoryCenters[startingTerritory.Name];
				PointF endingPoint = (PointF)m_territoryCenters[endingTerritory.Name];

				line.AddLine(startingPoint.X, startingPoint.Y, endingPoint.X, endingPoint.Y);

				line.Pickable = false;

				Canvas.Layer.AddChild(line);

				m_pathArrows.Add(line);
			}
		}

		public void ClearPathArrows()
		{
			foreach(PPath line in m_pathArrows)
			{
				line.RemoveFromParent();
			}

			m_pathArrows.Clear();
		}
	}

	class BlackLayer : PLayer 
	{
		public BlackLayer() 
		{
		}


		protected override void Paint(PPaintContext paintContext) 
		{
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
			
			ScaleAndDraw(g, displayRect);

			return b;
		}
	}
}
