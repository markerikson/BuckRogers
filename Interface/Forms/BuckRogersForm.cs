using System;
using System.Drawing;
using System.Drawing.Design;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;

using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.PiccoloX;
using UMD.HCIL.PiccoloX.Nodes;
using UMD.HCIL.PiccoloX.Components;

using BuckRogers;
using BuckRogers.Interface;

namespace BuckRogers
{

	public enum MapClickMode
	{
		Normal,
		Move,
		PlaceUnits,
		SelectTerritories,
	}

	/// <summary>
	/// Summary description for BuckRogersForm.
	/// </summary>
	public class BuckRogersForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button m_btnZoomIn;
		private System.Windows.Forms.Button m_btnZoomOut;
		private System.Windows.Forms.Button m_btnDefaultZoom;

		private GameController m_controller;
		private BattleController m_battleController;

		private CombatForm m_combatForm;
		private ProductionForm m_productionForm;

		private MapControl m_map;
		private MapClickMode m_clickMode;
		private System.Windows.Forms.Button m_btnCenterCamera;
		private System.Windows.Forms.ComboBox m_cbCenterLocations;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabControl tabControl1;
		private BuckRogers.Interface.MovePanel m_movePanel;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.StatusBarPanel statusBarPanel1;
		private System.Windows.Forms.StatusBarPanel statusBarPanel2;
		private System.Windows.Forms.TabPage m_tpAction;
		private System.Windows.Forms.TabPage m_tpTerritory;
		private System.Windows.Forms.TabPage m_tpPlacement;
		private BuckRogers.Interface.PlacementPanel m_placementPanel;
		private System.Windows.Forms.MenuItem m_menuFileExit;
		private System.Windows.Forms.TabPage m_tpInformation;
		private BuckRogers.Interface.InformationPanel m_informationPanel;
		private System.Windows.Forms.Button button1;
		private BuckRogers.Interface.TerritoryPanel m_territoryPanel;
		

		public BuckRogersForm()
		{
			InitializeComponent();

			ControllerTest ct = new ControllerTest();
			ct.Reinitialize = false;
			m_controller = ct.GameController;
			m_battleController = ct.BattleController;

			InitControls();
			InitEvents();

			ct.TerritoryOwnerChanged += new TerritoryOwnerChangedHandler(m_map.SetTerritoryOwner);
			ct.Init();

			tabControl1.TabPages.Clear();
			tabControl1.TabPages.Add(m_tpAction);
			tabControl1.TabPages.Add(m_tpTerritory);

			m_clickMode = MapClickMode.Normal;
			StartGame();

		}

		public BuckRogersForm(GameOptions go)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			bool useTesting = go.OptionalRules["UseTestingSetup"];
			ControllerTest ct = null;

			if(useTesting)
			{
				ct = new ControllerTest();
				ct.Reinitialize = false;
				m_controller = ct.GameController;
				m_battleController = ct.BattleController;
			}
			else
			{
				m_controller = new GameController(go);
				m_battleController = new BattleController(m_controller);
			}
			

			InitControls();
			InitEvents();

			if(useTesting)
			{
				ct.TerritoryOwnerChanged += new TerritoryOwnerChangedHandler(m_map.SetTerritoryOwner);
				ct.Init();

				tabControl1.TabPages.Clear();
				tabControl1.TabPages.Add(m_tpAction);
				tabControl1.TabPages.Add(m_tpTerritory);
				tabControl1.TabPages.Add(m_tpInformation);

				m_clickMode = MapClickMode.Normal;

				GameController.Options.OptionalRules = go.OptionalRules;
				StartGame();

			}
			else
			{
				tabControl1.TabPages.Clear();
				tabControl1.TabPages.Add(m_tpPlacement);
				tabControl1.TabPages.Add(m_tpTerritory);

				m_controller.AssignTerritories();
				m_controller.CreateInitialUnits();
				m_controller.RollForInitiative(false);

				m_clickMode = MapClickMode.Normal;
			
				m_placementPanel.RefreshPlayerOrder();

				statusBar1.Panels[0].Text = "Current player: " + m_controller.CurrentPlayer.Name;
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

			m_movePanel.MoveModeChanged += new MoveModeChangedHandler(OnMoveModeChanged);
			m_placementPanel.MoveModeChanged += new MoveModeChangedHandler(OnMoveModeChanged);

			m_movePanel.Controller = m_controller;
			m_map.GameController = m_controller;
			m_placementPanel.Controller = m_controller;
			m_informationPanel.Controller = m_controller;

			m_map.PlacePlanetIcons();
		}


		private void InitEvents()
		{
			m_controller.TerritoryOwnerChanged += new TerritoryOwnerChangedHandler(m_map.SetTerritoryOwner);
			m_battleController.TerritoryOwnerChanged += new TerritoryOwnerChangedHandler(m_map.SetTerritoryOwner);

			m_controller.StatusUpdate += new StatusUpdateHandler(OnStatusUpdate);
			m_battleController.StatusUpdate += new StatusUpdateHandler(OnStatusUpdate);

			m_controller.ActionAdded += new DisplayActionHandler(m_movePanel.AddActionToList);
		}

		private void StartGame()
		{
			//m_controller.InitGamelog();
			m_controller.NextTurn();
			//m_controller.LogInitialPlacements();

			statusBar1.Panels[0].Text = "Current player: " + m_controller.CurrentPlayer.Name;
			statusBar1.Panels[1].Text = "Turn: " + m_controller.TurnNumber.ToString();
			
			m_movePanel.RefreshPlayerOrder();			
			m_informationPanel.RefreshAllInfo();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.m_menuFileExit = new System.Windows.Forms.MenuItem();
			this.button1 = new System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.m_tpPlacement.SuspendLayout();
			this.m_tpAction.SuspendLayout();
			this.m_tpTerritory.SuspendLayout();
			this.m_tpInformation.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).BeginInit();
			this.SuspendLayout();
			// 
			// m_btnZoomIn
			// 
			this.m_btnZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_btnZoomIn.Location = new System.Drawing.Point(424, 648);
			this.m_btnZoomIn.Name = "m_btnZoomIn";
			this.m_btnZoomIn.TabIndex = 0;
			this.m_btnZoomIn.Text = "Zoom In";
			this.m_btnZoomIn.Click += new System.EventHandler(this.m_btnZoomIn_Click);
			// 
			// m_btnZoomOut
			// 
			this.m_btnZoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_btnZoomOut.Location = new System.Drawing.Point(504, 648);
			this.m_btnZoomOut.Name = "m_btnZoomOut";
			this.m_btnZoomOut.TabIndex = 1;
			this.m_btnZoomOut.Text = "Zoom Out";
			this.m_btnZoomOut.Click += new System.EventHandler(this.m_btnZoomOut_Click);
			// 
			// m_btnDefaultZoom
			// 
			this.m_btnDefaultZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_btnDefaultZoom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
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
			this.m_placementPanel.Controller = null;
			this.m_placementPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_placementPanel.Location = new System.Drawing.Point(0, 0);
			this.m_placementPanel.Name = "m_placementPanel";
			this.m_placementPanel.Size = new System.Drawing.Size(232, 634);
			this.m_placementPanel.TabIndex = 0;
			// 
			// m_tpAction
			// 
			this.m_tpAction.Controls.Add(this.m_movePanel);
			this.m_tpAction.DockPadding.Top = 10;
			this.m_tpAction.Location = new System.Drawing.Point(4, 22);
			this.m_tpAction.Name = "m_tpAction";
			this.m_tpAction.Size = new System.Drawing.Size(232, 634);
			this.m_tpAction.TabIndex = 0;
			this.m_tpAction.Text = "Actions";
			// 
			// m_movePanel
			// 
			this.m_movePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.m_movePanel.Controller = null;
			this.m_movePanel.DockPadding.Bottom = 4;
			this.m_movePanel.DockPadding.Top = 282;
			this.m_movePanel.Location = new System.Drawing.Point(0, 5);
			this.m_movePanel.Name = "m_movePanel";
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
																						  this.statusBarPanel2});
			this.statusBar1.ShowPanels = true;
			this.statusBar1.Size = new System.Drawing.Size(1016, 22);
			this.statusBar1.TabIndex = 8;
			this.statusBar1.Text = "statusBar1";
			// 
			// statusBarPanel1
			// 
			this.statusBarPanel1.Text = "statusBarPanel1";
			this.statusBarPanel1.Width = 200;
			// 
			// statusBarPanel2
			// 
			this.statusBarPanel2.Text = "statusBarPanel2";
			this.statusBarPanel2.Width = 120;
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.m_menuFileExit});
			this.menuItem1.Text = "File";
			// 
			// m_menuFileExit
			// 
			this.m_menuFileExit.Index = 0;
			this.m_menuFileExit.Text = "Exit";
			this.m_menuFileExit.Click += new System.EventHandler(this.m_menuFileExit_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(284, 648);
			this.button1.Name = "button1";
			this.button1.TabIndex = 9;
			this.button1.Text = "button1";
			this.button1.Click += new System.EventHandler(this.button1_Click_1);
			// 
			// BuckRogersForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(1016, 696);
			this.Controls.Add(this.button1);
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
			this.Text = "BuckRogersForm";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.BuckRogersForm_Closing);
			this.Load += new System.EventHandler(this.BuckRogersForm_Load);
			this.tabControl1.ResumeLayout(false);
			this.m_tpPlacement.ResumeLayout(false);
			this.m_tpAction.ResumeLayout(false);
			this.m_tpTerritory.ResumeLayout(false);
			this.m_tpInformation.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/*
		[STAThread]
		public static void Main(string[] args)
		{
			Application.Run(new BuckRogersForm());
		}
		*/

		public void OnTerritoryClicked(object sender, TerritoryEventArgs tcea)
		{
			string name = tcea.Name;

			Territory t = m_controller.Map[name];

			if(tcea.Button == MouseButtons.Right)
			{
				tabControl1.SelectedTab = m_tpTerritory;
				m_territoryPanel.DisplayUnits(t);
			}

			else if(t != null)
			{
				switch(m_clickMode)
				{
					case MapClickMode.Normal:
					{
						m_territoryPanel.DisplayUnits(t);
						break;
					}
					case MapClickMode.Move:
					{
						m_movePanel.TerritoryClicked(t);
						break;
					}
					case MapClickMode.PlaceUnits:
					{
						m_placementPanel.TerritoryClicked(t);
						break;
					}
				}

				//MessageBox.Show("Territory: " + name + ", owner: " + t.Owner.Name);
			}

			//MessageBox.Show("Territory clicked: " + name);
		}

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
		}

		private void m_btnCenterCamera_Click(object sender, System.EventArgs e)
		{
			string target = (string)m_cbCenterLocations.SelectedItem;

			m_map.CenterMapOn(target);
		}

		public void OnMoveModeChanged(object sender, MoveModeEventArgs mmea)
		{
			switch(mmea.MoveMode)
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
			}
		}

		private void BuckRogersForm_Load(object sender, System.EventArgs e)
		{
		
		}

		private bool OnStatusUpdate(object sender, StatusUpdateEventArgs suea)
		{
			bool result = true;
			switch(suea.StatusInfo)
			{
				case StatusInfo.NextPlayer:
				{
					statusBar1.Panels[0].Text = "Current player: " + suea.Player.Name;
					break;
				}
				case StatusInfo.NextPhase:
				{
					switch(m_controller.CurrentPhase)
					{
						case GamePhase.Movement:
						{
							//tabControl1.TabPages.Remove(m_tpPlacement);
							tabControl1.TabPages.Clear();
							tabControl1.TabPages.Add(m_tpAction);
							tabControl1.TabPages.Add(m_tpTerritory);
							tabControl1.TabPages.Add(m_tpInformation);

							StartGame();
							break;
						}
						case GamePhase.Combat:
						{
							statusBar1.Panels[0].Text = "Combat phase";
							//MessageBox.Show("Movement over, time for combat");
							if(m_combatForm == null)
							{
								m_combatForm = new CombatForm(m_controller, m_battleController);
							}

							m_controller.FindBattles();

							if(m_controller.Battles.Count == 0)
							{
								MessageBox.Show("No battles this turn - moving to production");
								m_controller.CheckNextPhase();
								
							}
							else
							{
								m_combatForm.BeginCombat();
								m_combatForm.ShowDialog();
							}
							
							goto case GamePhase.Production;
						}
						case GamePhase.Production:
						{
							if(m_productionForm == null)
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
							break;
						}
					}
					
					

					break;
				}
				case StatusInfo.GameOver:
				{
					MessageBox.Show("The winner is " + suea.Player.Name, "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
					break;
				}
				case StatusInfo.TransportLanded:
				{
					string message = "You have loaded transports in " + suea.Territory.Name + ".  Unload them?";
					DialogResult dr = MessageBox.Show(message, "Unload Transports?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

					return dr == DialogResult.Yes;
				}				
			}		
	
			return result;
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
	}
}
