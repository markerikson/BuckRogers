//#define DEBUGCOMBAT

#region using directives
using System;
using System.Drawing;
using System.Drawing.Design;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
using System.Threading;

using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.PiccoloX;
using UMD.HCIL.PiccoloX.Nodes;
using UMD.HCIL.PiccoloX.Components;

using BuckRogers;
using BuckRogers.Interface;
using BuckRogers.Networking;
#endregion

namespace BuckRogers.Interface
{

	public enum MapClickMode
	{
		Normal,
		Move,
		PlaceUnits,
		SelectTerritories,
		GameOver,
		Observation,
	}

	public class BuckRogersForm : System.Windows.Forms.Form, IMessageFilter
	{
		private static string m_versionString = "0.8.1a (Beta)";

		#region private members
		private IContainer components;
		private System.Windows.Forms.Button m_btnZoomIn;
		private System.Windows.Forms.Button m_btnZoomOut;
		private System.Windows.Forms.Button m_btnDefaultZoom;

		private GameController m_controller;
		private BattleController m_battleController;
		private BuckRogersClient m_gameClient;
		private ClientSideGameManager m_csgm;

		private ProductionForm m_productionForm;
		private CombatForm2D m_combatForm2;
		private HowToPlayForm m_howToPlay;
		private YesNoForm m_yesno;

		private MapControl m_map;
		private MapClickMode m_clickMode;

		private BuckRogers.Interface.MovePanel m_movePanel;
		private BuckRogers.Interface.PlacementPanel m_placementPanel;
		private BuckRogers.Interface.InformationPanel m_informationPanel;
		private BuckRogers.Interface.TerritoryPanel m_territoryPanel;

		private System.Windows.Forms.Button m_btnCenterCamera;
		private System.Windows.Forms.ComboBox m_cbCenterLocations;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabControl tabControl1;		
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.StatusBarPanel statusBarPanel1;
		private System.Windows.Forms.StatusBarPanel statusBarPanel2;
		private System.Windows.Forms.TabPage m_tpAction;
		private System.Windows.Forms.TabPage m_tpTerritory;
		private System.Windows.Forms.TabPage m_tpPlacement;		
		private System.Windows.Forms.MenuItem m_menuFileExit;
		private System.Windows.Forms.TabPage m_tpInformation;		
		private System.Windows.Forms.MenuItem m_menuFileSave;
		private System.Windows.Forms.MenuItem m_menuFileLoad;
		private MenuItem menuItem2;
		private MenuItem m_menuHelpAbout;
		private MenuItem m_menuHelpHow;
		private StatusBarPanel statusBarPanel3;
		private MenuItem m_menuHelpIconLegend;
		private MenuItem menuItem4;
		

		#endregion

		#region properties

		public static string VersionString
		{
			get { return BuckRogersForm.m_versionString; }
		}

		public BuckRogers.GameController GameController
		{
			get { return this.m_controller; }
			set { this.m_controller = value; }
		}

		public BuckRogers.BattleController BattleController
		{
			get { return this.m_battleController; }
			set { this.m_battleController = value; }
		}

		#endregion

		#region constructors

		public BuckRogersForm(GameOptions go, string loadFileName, BuckRogersClient client)
		{
			InitializeComponent();
			this.Icon = InterfaceUtility.GetApplicationIcon();
			m_gameClient = client;

			this.StartPosition = FormStartPosition.CenterScreen;
			Rectangle desktop = Screen.PrimaryScreen.WorkingArea;
			this.Size = new Size(desktop.Width - 80, desktop.Height - 80);			

			Initialize(go, loadFileName);			
		}

		#endregion

		#region initialization

		private void Initialize(GameOptions go, string loadFileName)
		{
			bool useTesting = go.OptionalRules["UseTestingSetup"];
			bool loadSaveGame = (loadFileName != string.Empty);
			ControllerTest ct = null;

			if(!loadSaveGame && useTesting)
			{
				ct = new ControllerTest();
				ct.Reinitialize = false;
				m_controller = ct.GameController;
				m_battleController = ct.BattleController;

				GameController.Options.OptionalRules = go.OptionalRules;
				//GameController.Options.SetupOptions = go.SetupOptions;
				GameController.Options.IncreasedProductionTurn = go.IncreasedProductionTurn;
				GameController.Options.NumTerritoriesNeeded = go.NumTerritoriesNeeded;
				GameController.Options.ProductionMultiplier = go.ProductionMultiplier;
				GameController.Options.WinningConditions = go.WinningConditions;
			}			
			else
			{
				m_controller = new GameController(go);
				m_battleController = new BattleController(m_controller);

				if(go.IsNetworkGame)
				{
					Player[] players = new Player[m_gameClient.Players.Count];
					for (int i = 0; i < m_gameClient.Players.Count; i++)
					{
						players[i] = (Player)m_gameClient.Players[i];
					}

					m_controller.SetPlayers(players);
				}
				
			}

			m_csgm = new ClientSideGameManager(m_gameClient, m_controller, m_battleController);


#if DEBUGCOMBAT
			m_battleController.AttacksAlwaysHit = true;
#endif

			InitControls();

			m_map.MovePanel = m_movePanel;
			m_map.IconManager.Controller = m_controller;
			m_map.IconManager.LoadUnitIconLocations(false, true);
			InitEvents();

			if(!loadSaveGame)
			{
				if(useTesting)
				{
					ct.TerritoryOwnerChanged += new EventHandler<TerritoryEventArgs>(m_map.SetTerritoryOwner); //new TerritoryOwnerChangedHandler(m_map.SetTerritoryOwner);
					m_map.IconManager.CreateIcons();
					ct.Init();
					m_controller.InitGamelog();

					tabControl1.TabPages.Clear();
					tabControl1.TabPages.Add(m_tpAction);
					tabControl1.TabPages.Add(m_tpTerritory);
					tabControl1.TabPages.Add(m_tpInformation);

					m_clickMode = MapClickMode.Move;
					m_movePanel.MoveMode = MoveMode.StartMove;

					GameController.Options.OptionalRules = go.OptionalRules;
					m_map.IconManager.CreateIcons();
					
					StartGame();

				}
				else
				{
					tabControl1.TabPages.Clear();
					tabControl1.TabPages.Add(m_tpPlacement);
					tabControl1.TabPages.Add(m_tpTerritory);

					m_map.IconManager.CreateIcons();
					m_map.IconManager.LoadUnitIconLocations(false, true);
					m_placementPanel.IconManager = m_map.IconManager;

					if(!go.IsNetworkGame)
					{
						m_controller.AssignTerritories();
						m_controller.CreateInitialUnits();
						m_controller.RollForInitiative(false);

						//m_placementPanel.RefreshPlayerOrder();
						//m_placementPanel.RefreshAvailableUnits();
						m_controller.InitGamelog();

						m_clickMode = MapClickMode.Normal;
						m_placementPanel.Initialize();

						

						statusBar1.Panels[0].Text = "Current player: " + m_controller.CurrentPlayer.Name;
					}
					else
					{
						m_clickMode = MapClickMode.Observation;
						statusBar1.Panels[0].Text = "Current player: N/A";
					}

					
					statusBar1.Panels[1].Text = "Placement";

					string victoryDescription = Utility.GetDescriptionOf(GameController.Options.WinningConditions);
					statusBar1.Panels[2].Text = "Victory Condition: " + victoryDescription;

					m_menuFileSave.Enabled = false;
				}
			}
			else
			{
				tabControl1.TabPages.Clear();
				tabControl1.TabPages.Add(m_tpAction);
				tabControl1.TabPages.Add(m_tpTerritory);
				tabControl1.TabPages.Add(m_tpInformation);
				LoadGame(loadFileName);
				m_map.IconManager.CreateIcons();
				//m_map.IconManager.LoadUnitIconLocations(false, true);
			}


			
		}

		

		private void InitControls()
		{
			m_movePanel.Height = m_tpAction.ClientSize.Height;

			m_map = new MapControl();
			m_map.Size = new Size(this.ClientSize.Width - tabControl1.Right - 4, this.ClientSize.Height - 52);
			m_map.TerritoryClicked +=new TerritoryClickedHandler(OnTerritoryClicked);
			m_map.Location = new Point(tabControl1.Right + 4, 0);
			m_map.ScrollControl.Size = m_map.ClientSize;
			m_map.Canvas.Size = m_map.ClientSize;
			this.Controls.Add(m_map);

			m_map.Anchor = 
				AnchorStyles.Bottom |
				AnchorStyles.Top |
				AnchorStyles.Left |
				AnchorStyles.Right;

			m_map.Canvas.Camera.ViewScale = 0.25f;

			Size viewSize = m_map.ScrollControl.ViewSize;

			int centerX = (int)(viewSize.Width / 2);
			int centerY = 0;

			Point ulCorner = new Point(centerX, centerY);
			ulCorner.X -= this.Width /  2;

			m_map.ScrollControl.ViewPosition = ulCorner;
			m_cbCenterLocations.SelectedIndex = 0;

			m_movePanel.MoveModeChanged += new EventHandler<MoveModeEventArgs>(OnMoveModeChanged);//new MoveModeChangedHandler(OnMoveModeChanged);
			m_placementPanel.MoveModeChanged += new EventHandler<MoveModeEventArgs>(OnMoveModeChanged);//new MoveModeChangedHandler(OnMoveModeChanged);

			m_movePanel.Map = m_map;

			m_movePanel.Controller = m_controller;			
			m_map.GameController = m_controller;
			m_placementPanel.GameController = m_controller;
			m_informationPanel.Controller = m_controller;

			m_placementPanel.GameManager = m_csgm;
			m_movePanel.GameManager = m_csgm;


			m_yesno = new YesNoForm();
			m_yesno.VisibleChanged += new EventHandler(OnYesNoFormVisibleChanged);

			m_map.PlacePlanetIcons();
		}

		private void InitEvents()
		{
			m_controller.TerritoryOwnerChanged += new EventHandler<TerritoryEventArgs>(m_map.SetTerritoryOwner); //new TerritoryOwnerChangedHandler(m_map.SetTerritoryOwner);
			m_battleController.TerritoryOwnerChanged += new EventHandler<TerritoryEventArgs>(m_map.SetTerritoryOwner); //TerritoryOwnerChangedHandler(m_map.SetTerritoryOwner);

			m_controller.StatusUpdate += new EventHandler<StatusUpdateEventArgs>(OnStatusUpdate); //StatusUpdateHandler(OnStatusUpdate);
			m_battleController.StatusUpdate += new EventHandler<StatusUpdateEventArgs>(OnStatusUpdate);

			m_controller.ActionAdded += new EventHandler<StatusUpdateEventArgs>(m_movePanel.AddActionToList);//new DisplayActionHandler(m_movePanel.AddActionToList);
			m_controller.ActionUndone += new EventHandler<StatusUpdateEventArgs>(m_movePanel.RemoveActionFromList);//new DisplayActionHandler(m_movePanel.RemoveActionFromList);
			m_controller.TerritoryUnitsChanged += new EventHandler<TerritoryUnitsEventArgs>(m_informationPanel.UpdateUnitInfo);//new TerritoryUnitsChangedHandler(m_informationPanel.UpdateUnitInfo);
			m_controller.TerritoryUnitsChanged += new EventHandler<TerritoryUnitsEventArgs>(m_map.UpdateUnitInfo);//new TerritoryUnitsChangedHandler(m_map.UpdateUnitInfo);

			m_battleController.UpdateTerritory += new EventHandler<StatusUpdateEventArgs>(m_map.IconManager.RefreshIcons);//new TerritoryUpdateHandler(m_map.IconManager.RefreshIcons);
			m_controller.UpdateTerritory += new EventHandler<StatusUpdateEventArgs>(m_map.IconManager.RefreshIcons);//new TerritoryUpdateHandler(m_map.IconManager.RefreshIcons);
			m_battleController.TerritoryUnitsChanged += new EventHandler<TerritoryUnitsEventArgs>(m_map.UpdateUnitInfo);//new TerritoryUnitsChangedHandler(m_map.UpdateUnitInfo);

			m_controller.PlayersCreated += new EventHandler<StatusUpdateEventArgs>(m_map.IconManager.CreateIcons);//new PlayersCreatedHandler(m_map.IconManager.CreateIcons);

			m_csgm.ClientUpdateMessage += new EventHandler<ClientUpdateEventArgs>(OnClientUpdateMessage);
			m_csgm.TransportLanded += new EventHandler<StatusUpdateEventArgs>(OnStatusUpdate);
		}

		#endregion

		#region plumbing

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.m_btnZoomIn = new System.Windows.Forms.Button();
			this.m_btnZoomOut = new System.Windows.Forms.Button();
			this.m_btnDefaultZoom = new System.Windows.Forms.Button();
			this.m_btnCenterCamera = new System.Windows.Forms.Button();
			this.m_cbCenterLocations = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.m_tpPlacement = new System.Windows.Forms.TabPage();
			this.m_placementPanel = new BuckRogers.Interface.PlacementPanel();
			this.m_tpAction = new System.Windows.Forms.TabPage();
			this.m_movePanel = new BuckRogers.Interface.MovePanel();
			this.m_tpTerritory = new System.Windows.Forms.TabPage();
			this.m_territoryPanel = new BuckRogers.Interface.TerritoryPanel();
			this.m_tpInformation = new System.Windows.Forms.TabPage();
			this.m_informationPanel = new BuckRogers.Interface.InformationPanel();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
			this.statusBarPanel2 = new System.Windows.Forms.StatusBarPanel();
			this.statusBarPanel3 = new System.Windows.Forms.StatusBarPanel();
			this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.m_menuFileSave = new System.Windows.Forms.MenuItem();
			this.m_menuFileLoad = new System.Windows.Forms.MenuItem();
			this.m_menuFileExit = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.m_menuHelpHow = new System.Windows.Forms.MenuItem();
			this.m_menuHelpAbout = new System.Windows.Forms.MenuItem();
			this.m_menuHelpIconLegend = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.tabControl1.SuspendLayout();
			this.m_tpPlacement.SuspendLayout();
			this.m_tpAction.SuspendLayout();
			this.m_tpTerritory.SuspendLayout();
			this.m_tpInformation.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel3)).BeginInit();
			this.SuspendLayout();
			// 
			// m_btnZoomIn
			// 
			this.m_btnZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_btnZoomIn.Location = new System.Drawing.Point(424, 648);
			this.m_btnZoomIn.Name = "m_btnZoomIn";
			this.m_btnZoomIn.Size = new System.Drawing.Size(75, 23);
			this.m_btnZoomIn.TabIndex = 0;
			this.m_btnZoomIn.Text = "Zoom In";
			this.m_btnZoomIn.Click += new System.EventHandler(this.m_btnZoomIn_Click);
			// 
			// m_btnZoomOut
			// 
			this.m_btnZoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_btnZoomOut.Location = new System.Drawing.Point(504, 648);
			this.m_btnZoomOut.Name = "m_btnZoomOut";
			this.m_btnZoomOut.Size = new System.Drawing.Size(75, 23);
			this.m_btnZoomOut.TabIndex = 1;
			this.m_btnZoomOut.Text = "Zoom Out";
			this.m_btnZoomOut.Click += new System.EventHandler(this.m_btnZoomOut_Click);
			// 
			// m_btnDefaultZoom
			// 
			this.m_btnDefaultZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_btnDefaultZoom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_btnDefaultZoom.Location = new System.Drawing.Point(584, 648);
			this.m_btnDefaultZoom.Name = "m_btnDefaultZoom";
			this.m_btnDefaultZoom.Size = new System.Drawing.Size(80, 23);
			this.m_btnDefaultZoom.TabIndex = 2;
			this.m_btnDefaultZoom.Text = "Default Zoom";
			this.m_btnDefaultZoom.Click += new System.EventHandler(this.m_btnDefaultZoom_Click);
			// 
			// m_btnCenterCamera
			// 
			this.m_btnCenterCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_btnCenterCamera.Location = new System.Drawing.Point(920, 648);
			this.m_btnCenterCamera.Name = "m_btnCenterCamera";
			this.m_btnCenterCamera.Size = new System.Drawing.Size(75, 23);
			this.m_btnCenterCamera.TabIndex = 3;
			this.m_btnCenterCamera.Text = "Center";
			this.m_btnCenterCamera.Click += new System.EventHandler(this.m_btnCenterCamera_Click);
			// 
			// m_cbCenterLocations
			// 
			this.m_cbCenterLocations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_cbCenterLocations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_cbCenterLocations.Items.AddRange(new object[] {
            "Sun",
            "Mercury",
            "Venus",
            "Earth",
            "Mars",
            "Asteroids"});
			this.m_cbCenterLocations.Location = new System.Drawing.Point(792, 648);
			this.m_cbCenterLocations.Name = "m_cbCenterLocations";
			this.m_cbCenterLocations.Size = new System.Drawing.Size(121, 21);
			this.m_cbCenterLocations.TabIndex = 4;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(688, 652);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 16);
			this.label1.TabIndex = 5;
			this.label1.Text = "Center camera on:";
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.tabControl1.Controls.Add(this.m_tpPlacement);
			this.tabControl1.Controls.Add(this.m_tpAction);
			this.tabControl1.Controls.Add(this.m_tpTerritory);
			this.tabControl1.Controls.Add(this.m_tpInformation);
			this.tabControl1.Location = new System.Drawing.Point(4, 8);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(240, 660);
			this.tabControl1.TabIndex = 7;
			// 
			// m_tpPlacement
			// 
			this.m_tpPlacement.Controls.Add(this.m_placementPanel);
			this.m_tpPlacement.Location = new System.Drawing.Point(4, 22);
			this.m_tpPlacement.Name = "m_tpPlacement";
			this.m_tpPlacement.Size = new System.Drawing.Size(232, 634);
			this.m_tpPlacement.TabIndex = 2;
			this.m_tpPlacement.Text = "Placement";
			// 
			// m_placementPanel
			// 
			this.m_placementPanel.GameController = null;
			this.m_placementPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_placementPanel.IconManager = null;
			this.m_placementPanel.Location = new System.Drawing.Point(0, 0);
			this.m_placementPanel.Name = "m_placementPanel";
			this.m_placementPanel.Size = new System.Drawing.Size(232, 634);
			this.m_placementPanel.TabIndex = 0;
			this.m_placementPanel.UnitKeys = new System.Windows.Forms.Keys[] {
        System.Windows.Forms.Keys.D1,
        System.Windows.Forms.Keys.D2,
        System.Windows.Forms.Keys.D3,
        System.Windows.Forms.Keys.D4,
        System.Windows.Forms.Keys.D5,
        System.Windows.Forms.Keys.D6};
			// 
			// m_tpAction
			// 
			this.m_tpAction.Controls.Add(this.m_movePanel);
			this.m_tpAction.Location = new System.Drawing.Point(4, 22);
			this.m_tpAction.Name = "m_tpAction";
			this.m_tpAction.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this.m_tpAction.Size = new System.Drawing.Size(232, 634);
			this.m_tpAction.TabIndex = 0;
			this.m_tpAction.Text = "Actions";
			// 
			// m_movePanel
			// 
			this.m_movePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.m_movePanel.Controller = null;
			this.m_movePanel.Location = new System.Drawing.Point(0, 5);
			this.m_movePanel.Map = null;
			this.m_movePanel.Name = "m_movePanel";
			this.m_movePanel.Padding = new System.Windows.Forms.Padding(0, 282, 0, 4);
			this.m_movePanel.Size = new System.Drawing.Size(236, 524);
			this.m_movePanel.TabIndex = 0;
			// 
			// m_tpTerritory
			// 
			this.m_tpTerritory.Controls.Add(this.m_territoryPanel);
			this.m_tpTerritory.Location = new System.Drawing.Point(4, 22);
			this.m_tpTerritory.Name = "m_tpTerritory";
			this.m_tpTerritory.Size = new System.Drawing.Size(232, 634);
			this.m_tpTerritory.TabIndex = 1;
			this.m_tpTerritory.Text = "Territory";
			// 
			// m_territoryPanel
			// 
			this.m_territoryPanel.Location = new System.Drawing.Point(0, 0);
			this.m_territoryPanel.Name = "m_territoryPanel";
			this.m_territoryPanel.Size = new System.Drawing.Size(240, 400);
			this.m_territoryPanel.TabIndex = 0;
			// 
			// m_tpInformation
			// 
			this.m_tpInformation.Controls.Add(this.m_informationPanel);
			this.m_tpInformation.Location = new System.Drawing.Point(4, 22);
			this.m_tpInformation.Name = "m_tpInformation";
			this.m_tpInformation.Size = new System.Drawing.Size(232, 634);
			this.m_tpInformation.TabIndex = 3;
			this.m_tpInformation.Text = "Information";
			// 
			// m_informationPanel
			// 
			this.m_informationPanel.Controller = null;
			this.m_informationPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_informationPanel.Location = new System.Drawing.Point(0, 0);
			this.m_informationPanel.Name = "m_informationPanel";
			this.m_informationPanel.Size = new System.Drawing.Size(232, 634);
			this.m_informationPanel.TabIndex = 0;
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 674);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarPanel1,
            this.statusBarPanel2,
            this.statusBarPanel3});
			this.statusBar1.ShowPanels = true;
			this.statusBar1.Size = new System.Drawing.Size(1016, 22);
			this.statusBar1.TabIndex = 8;
			this.statusBar1.Text = "statusBar1";
			// 
			// statusBarPanel1
			// 
			this.statusBarPanel1.Name = "statusBarPanel1";
			this.statusBarPanel1.Text = "statusBarPanel1";
			this.statusBarPanel1.Width = 200;
			// 
			// statusBarPanel2
			// 
			this.statusBarPanel2.Name = "statusBarPanel2";
			this.statusBarPanel2.Text = "statusBarPanel2";
			this.statusBarPanel2.Width = 120;
			// 
			// statusBarPanel3
			// 
			this.statusBarPanel3.Name = "statusBarPanel3";
			this.statusBarPanel3.Text = "Victory Condition:";
			this.statusBarPanel3.Width = 300;
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.m_menuFileSave,
            this.m_menuFileLoad,
            this.m_menuFileExit});
			this.menuItem1.Text = "File";
			// 
			// m_menuFileSave
			// 
			this.m_menuFileSave.Index = 0;
			this.m_menuFileSave.Text = "Save";
			this.m_menuFileSave.Click += new System.EventHandler(this.m_menuFileSave_Click);
			// 
			// m_menuFileLoad
			// 
			this.m_menuFileLoad.Index = 1;
			this.m_menuFileLoad.Text = "Load";
			this.m_menuFileLoad.Click += new System.EventHandler(this.m_menuFileLoad_Click);
			// 
			// m_menuFileExit
			// 
			this.m_menuFileExit.Index = 2;
			this.m_menuFileExit.Text = "Exit";
			this.m_menuFileExit.Click += new System.EventHandler(this.m_menuFileExit_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.m_menuHelpHow,
            this.m_menuHelpIconLegend,
            this.menuItem4,
            this.m_menuHelpAbout});
			this.menuItem2.Text = "Help";
			// 
			// m_menuHelpHow
			// 
			this.m_menuHelpHow.Index = 0;
			this.m_menuHelpHow.Text = "How To Play";
			this.m_menuHelpHow.Click += new System.EventHandler(this.m_menuHelpHow_Click);
			// 
			// m_menuHelpAbout
			// 
			this.m_menuHelpAbout.Index = 3;
			this.m_menuHelpAbout.Text = "About";
			this.m_menuHelpAbout.Click += new System.EventHandler(this.m_menuHelpAbout_Click);
			// 
			// m_menuHelpIconLegend
			// 
			this.m_menuHelpIconLegend.Index = 1;
			this.m_menuHelpIconLegend.Text = "Unit Icon Legend";
			this.m_menuHelpIconLegend.Click += new System.EventHandler(this.m_menuHelpIconLegend_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 2;
			this.menuItem4.Text = "-";
			// 
			// BuckRogersForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(1016, 696);
			this.Controls.Add(this.statusBar1);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.m_cbCenterLocations);
			this.Controls.Add(this.m_btnCenterCamera);
			this.Controls.Add(this.m_btnDefaultZoom);
			this.Controls.Add(this.m_btnZoomOut);
			this.Controls.Add(this.m_btnZoomIn);
			this.Menu = this.mainMenu1;
			this.Name = "BuckRogersForm";
			this.Text = "Buck Rogers: Battle for the 25th Century";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.BuckRogersForm_Closing);
			this.tabControl1.ResumeLayout(false);
			this.m_tpPlacement.ResumeLayout(false);
			this.m_tpAction.ResumeLayout(false);
			this.m_tpTerritory.ResumeLayout(false);
			this.m_tpInformation.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel3)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#endregion

		#region message handlers

		private void OnClientUpdateMessage(object sender, ClientUpdateEventArgs e)
		{
			switch(e.MessageType)
			{
				case GameMessage.InitialSetupInformation:
				{
					m_controller.InitGamelog();

					m_placementPanel.Initialize();
					break;
				}
				case GameMessage.PlacementPhaseEnded:
				{
					tabControl1.TabPages.Clear();
					tabControl1.TabPages.Add(m_tpAction);
					tabControl1.TabPages.Add(m_tpTerritory);
					tabControl1.TabPages.Add(m_tpInformation);

					break;
				}
				case GameMessage.CombatPhaseStarted:
				{
					m_menuFileSave.Enabled = false;
					statusBar1.Panels[0].Text = "Combat phase";

					if (m_combatForm2 == null)
					{
						m_combatForm2 = new CombatForm2D(m_csgm, m_controller, m_battleController, m_map.IconManager);
					}

					CombatPreviewForm cpf = new CombatPreviewForm();
					cpf.DisplayBattles(m_controller.Battles);
					cpf.ShowDialog();

					m_combatForm2.ShowDialog();
					break;
				}
			}
		}

		public void OnTerritoryClicked(object sender, TerritoryEventArgs tcea)
		{
			string name = tcea.Name;

			Territory t = m_controller.Map[name];

			if(t == null)
			{
				return;
			}

			bool isLeftButton = (tcea.Button == MouseButtons.Left);

			// If the user double-right-clicked, we're ALWAYS going to show the 
			// territory information panel
			if(!isLeftButton && tcea.DoubleClick)
			{
				m_territoryPanel.DisplayUnits(t, tcea);
				tabControl1.SelectedTab = m_tpTerritory;
				return;
			}

			// Otherwise, let the appropriate panel figure out how to deal with it.
			switch (m_clickMode)
			{
				case MapClickMode.Normal:
				case MapClickMode.GameOver:
				{
					m_territoryPanel.DisplayUnits(t, tcea);
					break;
				}
				case MapClickMode.Move:
				{
					m_movePanel.TerritoryClicked(t, tcea);					
					break;
				}
				case MapClickMode.PlaceUnits:
				{					
					m_placementPanel.TerritoryClicked(t, tcea);
					break;
				}
				case MapClickMode.Observation:
				{
					// for the moment, deliberately do nothing
					break;
				}
			}

		}

		public void OnMoveModeChanged(object sender, MoveModeEventArgs mmea)
		{
			switch (mmea.MoveMode)
			{
				case MoveMode.StartMove:
				case MoveMode.StartTransport:
				{
					m_clickMode = MapClickMode.Move;
					break;
				}
				case MoveMode.Finished:
				{
					m_clickMode = MapClickMode.Normal;
					break;
				}
				case MoveMode.StartPlacement:
				{
					m_clickMode = MapClickMode.PlaceUnits;
					break;
				}
				case MoveMode.None:
				{
					m_clickMode = MapClickMode.Observation;
					break;
				}
			}
		}

		private void OnStatusUpdate(object sender, StatusUpdateEventArgs suea)
		{
			bool result = true;
			switch (suea.StatusInfo)
			{
				case StatusInfo.NextPlayer:
				{
					statusBar1.Panels[0].Text = "Current player: " + suea.Player.Name;
					break;
				}
				case StatusInfo.NextPhase:
				{
					switch (m_controller.CurrentPhase)
					{
						case GamePhase.Movement:
						{
							//tabControl1.TabPages.Remove(m_tpPlacement);
							tabControl1.TabPages.Clear();
							tabControl1.TabPages.Add(m_tpAction);
							tabControl1.TabPages.Add(m_tpTerritory);
							tabControl1.TabPages.Add(m_tpInformation);

							m_movePanel.BeginMovement();

							m_menuFileSave.Enabled = true;

							StartGame();
							break;
						}
						case GamePhase.Combat:
						{
							m_menuFileSave.Enabled = false;
							//m_menuFileLoad.Enabled = false;
							statusBar1.Panels[0].Text = "Combat phase";
							//MessageBox.Show("Movement over, time for combat");

							/*
							if(m_combatForm == null)
							{
								m_combatForm = new CombatForm(m_controller, m_battleController);
							}
							*/

							if (m_combatForm2 == null)
							{
								m_combatForm2 = new CombatForm2D(m_csgm, m_controller, m_battleController, m_map.IconManager);
							}

							m_controller.FindBattles();

							if (m_controller.Battles.Count == 0)
							{
								//MessageBox.Show("No battles this turn - moving to production");
								m_controller.CheckNextPhase();

							}
							else
							{
								//m_combatForm.BeginCombat();
								//m_combatForm.ShowDialog();


								//m_combatForm2.CombatDisplay.BeginCombat();
								m_combatForm2.ShowDialog();

								m_controller.CheckNextPhase();
							}

							//break;
							goto case GamePhase.Production;
						}
						case GamePhase.Production:
						{
							if (m_productionForm == null)
							{
								m_productionForm = new ProductionForm(m_controller);
								m_productionForm.Owner = this;
							}

							m_productionForm.SetupProduction();
							m_productionForm.Show();

							bool visible = m_productionForm.Visible;

							tabControl1.TabPages.Remove(m_tpAction);

							break;
						}
						case GamePhase.EndTurn:
						{
							tabControl1.TabPages.Clear();

							tabControl1.TabPages.Add(m_tpAction);
							tabControl1.TabPages.Add(m_tpTerritory);
							m_controller.NextTurn();
							m_map.AdvancePlanets();
							statusBar1.Panels[0].Text = "Current player: " + m_controller.CurrentPlayer.Name;
							statusBar1.Panels[1].Text = "Turn: " + m_controller.TurnNumber.ToString();
							m_movePanel.RefreshPlayerOrder();
							m_informationPanel.RefreshAllInfo();

							m_movePanel.BeginMovement();

							m_menuFileSave.Enabled = true;
							//m_menuFileLoad.Enabled = true;
							break;
						}
					}



					break;
				}
				case StatusInfo.GameOver:
				{
					MessageBox.Show("The winner is " + suea.Player.Name, "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);

					m_movePanel.DisableMovePanel();
					m_clickMode = MapClickMode.GameOver;
					break;
				}
				case StatusInfo.TransportLanded:
				{
					Territory location = suea.Territories[0];
					string message = "You have loaded transports in " + location.Name + ".  Unload them?";
					DialogResult dr = MessageBox.Show(message, "Unload Transports?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

					result = (dr == DialogResult.Yes);
					suea.Result = result;
					break;
				}
			}

			return;
		}

		private void OnYesNoFormVisibleChanged(object sender, EventArgs e)
		{
			if (!m_yesno.Visible)
			{
				DialogResult dr = m_yesno.DialogResult;

				MessageBox.Show("Result: " + dr);
			}
		}

		#endregion

		#region form event handlers

		private void m_btnZoomIn_Click(object sender, System.EventArgs e)
		{
			m_map.ZoomIn();
		}

		private void m_btnZoomOut_Click(object sender, System.EventArgs e)
		{
			m_map.ZoomOut();
		}


		private void m_btnDefaultZoom_Click(object sender, System.EventArgs e)
		{
			m_map.DefaultZoom();

			/*
			m_yesno.TopMost = false;
			m_yesno.Owner = this;
			m_yesno.Show();
			*/
		}

		private void m_btnCenterCamera_Click(object sender, System.EventArgs e)
		{
			string target = (string)m_cbCenterLocations.SelectedItem;

			m_map.CenterMapOn(target);
		}

		

		

		private void button1_Click_1(object sender, System.EventArgs e)
		{
			Point p = m_map.ScrollControl.ViewPosition;
			UMD.HCIL.Piccolo.PCamera c =  m_map.Canvas.Camera;
			RectangleF bounds = c.Bounds;
			float zoom = c.ViewScale;

			//p.X += (int)(bounds.Width / 2);
			//p.Y += (int)(bounds.Height / 2);

			PointF local = c.ViewToLocal(p);
			PointF global = c.LocalToGlobal(local);

			PointF zoomed = global;
			zoomed.X /= zoom;
			zoomed.Y /= zoom;

			RectangleF localr = c.ViewToLocal(bounds);
			RectangleF globalr = c.LocalToGlobal(localr);
			


			StringBuilder sb = new StringBuilder();
			//sb.AppendFormat("UL - global: {0}, local: {1}", ulg, ulz);
			sb.AppendFormat("Bounds - top: {0}, left: {1}, , bottom: {2}, , right: {3}", 
							globalr.Top, globalr.Left, globalr.Bottom, globalr.Bottom);

			//string message = "Global: " + global.ToString() + "\r\nZoomed: " + zoomed.ToString();
			MessageBox.Show(sb.ToString());

		}

		private void m_menuFileExit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void BuckRogersForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			m_controller.SaveLog();
		}

		private void m_menuFileSave_Click(object sender, System.EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "Buck Rogers save games (*.xml)|*.xml";

			if(sfd.ShowDialog() == DialogResult.OK)
			{
				m_controller.SaveGame(sfd.FileName);
			}

		}

		private void m_menuFileLoad_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Buck Rogers save games (*.xml)|*.xml";

			if(ofd.ShowDialog() == DialogResult.OK)
			{
				LoadGame(ofd.FileName);
			}
		}

		private void m_menuHelpAbout_Click(object sender, EventArgs e)
		{
			AboutForm af = new AboutForm();
			af.Owner = this;

			af.ShowDialog();
		}

		private void m_menuHelpHow_Click(object sender, EventArgs e)
		{
			if (m_howToPlay == null)
			{
				m_howToPlay = new HowToPlayForm();
				m_howToPlay.StartPosition = FormStartPosition.Manual;

				int x = this.Bounds.X + (this.Width / 2) - (m_howToPlay.Width / 2);
				int y = this.Bounds.Y + (this.Height / 2) - (m_howToPlay.Height / 2);

				m_howToPlay.Location = new Point(x, y);
			}

			m_howToPlay.Show();


		}

		private void m_menuHelpIconLegend_Click(object sender, EventArgs e)
		{
			UnitIconLegendForm uilf = new UnitIconLegendForm(m_controller, m_map.IconManager);

			uilf.Initialize();
			uilf.AddItems();
			uilf.ShowDialog();
		}

		#endregion

		#region game load / start

		public void LoadGame(string filename)
		{
			m_movePanel.ResetMovementInfo();
			m_map.IconManager.ClearAllIcons();
			m_controller.LoadGame(filename);

			m_battleController.InitGameLog();

			m_clickMode = MapClickMode.Move;
			m_movePanel.MoveMode = MoveMode.StartMove;

			tabControl1.TabPages.Clear();
			tabControl1.TabPages.Add(m_tpAction);
			tabControl1.TabPages.Add(m_tpTerritory);
			tabControl1.TabPages.Add(m_tpInformation);

			m_movePanel.PanelEnabled= true;

			m_menuFileSave.Enabled = true;

			statusBar1.Panels[0].Text = "Current player: " + m_controller.CurrentPlayer.Name;
			statusBar1.Panels[1].Text = "Turn: " + m_controller.TurnNumber.ToString();

			string victoryDescription = Utility.GetDescriptionOf(GameController.Options.WinningConditions);
			statusBar1.Panels[2].Text = "Victory Condition: " + victoryDescription;
			
			m_movePanel.RefreshPlayerOrder();
			m_movePanel.BeginMovement();
			m_informationPanel.RefreshAllInfo();
			m_map.AdvancePlanets();

			m_movePanel.EnableMovePanel();
		}

		private void StartGame()
		{
			//m_controller.InitGamelog();
			m_controller.NextTurn();
			//m_battleController.Gamelog = m_controller.Gamelog;
			m_battleController.InitGameLog();
			//m_controller.LogInitialPlacements();

			statusBar1.Panels[0].Text = "Current player: " + m_controller.CurrentPlayer.Name;
			statusBar1.Panels[1].Text = "Turn: " + m_controller.TurnNumber.ToString();

			string victoryDescription = Utility.GetDescriptionOf(GameController.Options.WinningConditions);
			statusBar1.Panels[2].Text = "Victory Condition: " + victoryDescription;

			m_movePanel.RefreshPlayerOrder();
			m_informationPanel.RefreshAllInfo();
		}

		#endregion

		#region keypress handling

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			Application.AddMessageFilter(this);

			if (GameController.Options.IsNetworkGame)
			{
				m_gameClient.SendMessageToServer(GameMessage.ClientLoaded, string.Empty);
			}
		}
		#region IMessageFilter Members

		public bool PreFilterMessage(ref Message m)
		{
			const int WM_KEYUP = 0x101;
			if (m.Msg != WM_KEYUP)
			{
				return false;
			}

			//const int WM_KEYDOWN = 0x100;

			Keys keyCode = (Keys)(int)m.WParam & Keys.KeyCode;

			if(!GameController.Options.IsNetworkGame || ClientSideGameManager.IsActiveClient)
			{
				if (m_controller.CurrentPhase == GamePhase.Setup)
				{
					if (Array.IndexOf(m_placementPanel.UnitKeys, keyCode) != -1)
					{
						m_placementPanel.KeyPressed(keyCode);
						return true;
					}
				}
				else if (m_controller.CurrentPhase == GamePhase.Movement)
				{
					if (keyCode == Keys.Enter || keyCode == Keys.T)
					{
						m_movePanel.KeyPressed(keyCode);
						return true;
					}
				}
			}
			
			return false; 
		}

		#endregion

		#endregion
	}
}
