using System;
using System.Drawing;
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
		private System.Windows.Forms.TabPage m_pgAction;
		private System.Windows.Forms.TabPage m_pgTerritory;
		private BuckRogers.Interface.MovePanel m_movePanel;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.StatusBarPanel statusBarPanel1;
		private System.Windows.Forms.StatusBarPanel statusBarPanel2;
		private System.Windows.Forms.Button button1;
		private BuckRogers.Interface.TerritoryPanel m_territoryPanel;


		public BuckRogersForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			m_clickMode = MapClickMode.Normal;

			m_movePanel.Height = m_pgAction.ClientSize.Height;

			m_map = new MapControl();

			ControllerTest ct = new ControllerTest();
			ct.Reinitialize = false;
			m_controller = ct.GameController;
			m_battleController = ct.BattleController;

		

			m_controller.TerritoryOwnerChanged += new TerritoryOwnerChangedHandler(m_map.SetTerritoryOwner);
			m_battleController.TerritoryOwnerChanged += new TerritoryOwnerChangedHandler(m_map.SetTerritoryOwner);
			m_controller.StatusUpdate += new StatusUpdateHandler(OnStatusUpdate);
			m_battleController.StatusUpdate += new StatusUpdateHandler(OnStatusUpdate);
			ct.TerritoryOwnerChanged += new TerritoryOwnerChangedHandler(m_map.SetTerritoryOwner);

			ct.Init();

			m_movePanel.Controller = m_controller;

			
			
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
			int centerY = 0;//(int)(viewSize.Height / 2);

			//Point center = new Point(, );

			Point ulCorner = new Point(centerX, centerY);
			ulCorner.X -= this.Width /  2;
			//ulCorner.Y -= this.Height / 2;

			m_map.ScrollControl.ViewPosition = ulCorner;
			m_cbCenterLocations.SelectedIndex = 0;


			m_movePanel.MoveModeChanged += new MoveModeChangedHandler(OnMoveModeChanged);

			m_controller.NextTurn();

			/*
			statusBar1.Panels.Add("Foo");
			statusBar1.Panels.Add("Bar");

			statusBar1.Panels[0].Width = 120;
			statusBar1.Panels[1].Width = 100;
			*/
			
			statusBar1.Panels[0].Text = "Current player: " + m_controller.CurrentPlayer.Name;
			statusBar1.Panels[1].Text = "Turn: " + m_controller.TurnNumber.ToString();

			m_movePanel.RefreshPlayerOrder();

			m_map.GameController = m_controller;
			m_map.PlacePlanetIcons();
			
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
			this.m_pgAction = new System.Windows.Forms.TabPage();
			this.m_movePanel = new BuckRogers.Interface.MovePanel();
			this.m_pgTerritory = new System.Windows.Forms.TabPage();
			this.m_territoryPanel = new BuckRogers.Interface.TerritoryPanel();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
			this.statusBarPanel2 = new System.Windows.Forms.StatusBarPanel();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.button1 = new System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.m_pgAction.SuspendLayout();
			this.m_pgTerritory.SuspendLayout();
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
			this.tabControl1.Controls.Add(this.m_pgAction);
			this.tabControl1.Controls.Add(this.m_pgTerritory);
			this.tabControl1.Location = new System.Drawing.Point(4, 44);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(240, 624);
			this.tabControl1.TabIndex = 7;
			// 
			// m_pgAction
			// 
			this.m_pgAction.Controls.Add(this.m_movePanel);
			this.m_pgAction.DockPadding.Top = 10;
			this.m_pgAction.Location = new System.Drawing.Point(4, 22);
			this.m_pgAction.Name = "m_pgAction";
			this.m_pgAction.Size = new System.Drawing.Size(232, 598);
			this.m_pgAction.TabIndex = 0;
			this.m_pgAction.Text = "Actions";
			// 
			// m_movePanel
			// 
			this.m_movePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.m_movePanel.Controller = null;
			this.m_movePanel.DockPadding.Bottom = 4;
			this.m_movePanel.DockPadding.Top = 282;
			this.m_movePanel.Location = new System.Drawing.Point(0, 5);
			this.m_movePanel.Name = "m_movePanel";
			this.m_movePanel.Size = new System.Drawing.Size(236, 488);
			this.m_movePanel.TabIndex = 0;
			// 
			// m_pgTerritory
			// 
			this.m_pgTerritory.Controls.Add(this.m_territoryPanel);
			this.m_pgTerritory.Location = new System.Drawing.Point(4, 22);
			this.m_pgTerritory.Name = "m_pgTerritory";
			this.m_pgTerritory.Size = new System.Drawing.Size(232, 598);
			this.m_pgTerritory.TabIndex = 1;
			this.m_pgTerritory.Text = "Territory";
			// 
			// m_territoryPanel
			// 
			this.m_territoryPanel.Location = new System.Drawing.Point(0, 0);
			this.m_territoryPanel.Name = "m_territoryPanel";
			this.m_territoryPanel.Size = new System.Drawing.Size(240, 400);
			this.m_territoryPanel.TabIndex = 0;
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
																					  this.menuItem2});
			this.menuItem1.Text = "File";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 0;
			this.menuItem2.Text = "Exit";
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
			this.Load += new System.EventHandler(this.BuckRogersForm_Load);
			this.tabControl1.ResumeLayout(false);
			this.m_pgAction.ResumeLayout(false);
			this.m_pgTerritory.ResumeLayout(false);
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

			if(t != null)
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
			//MessageBox.Show(m_map.ScrollControl.ViewPosition.ToString());
			//return;

			string target = (string)m_cbCenterLocations.SelectedItem;

			PointF targetLocation = new Point();
			Size viewSize = m_map.ScrollControl.ViewSize;
			float zoom = m_map.Canvas.Camera.ViewScale;
			float originalWidth = viewSize.Width / zoom;
			float originalHeight = viewSize.Height / zoom;

			switch(target)
			{
				case "Sun":
				{
					targetLocation.X = 0.52f * viewSize.Width;
					targetLocation.Y = 0.34f * viewSize.Height;//0.0f;
					break;
				}
				case "Mercury":
				{
					targetLocation.X = viewSize.Width * 0.085f;//0;//-2180;
					targetLocation.Y = viewSize.Height * 0.222f;//-380;
					break;
				}
				case "Venus":
				{
					break;
				}
				case "Earth":
				{
					break;
				}
				case "Mars":
				{
					break;
				}
				case "Asteroids":
				{
					break;
				}
			}

			


			//targetLocation.X *= zoom;
			//targetLocation.Y *= zoom;

			//Size mapSize = m_map.ClientSize;
			//targetLocation.X -= mapSize.Width / 2;
			//targetLocation.Y -= mapSize.Height / 2;

			Point ulCorner = new Point((int)targetLocation.X, (int)targetLocation.Y);

			PointF transformedPoint = m_map.Canvas.Camera.ViewToLocal(ulCorner);
			ulCorner.X -= m_map.Width /  2;
			ulCorner.Y -= m_map.Height / 2;

			m_map.ScrollControl.ViewPosition = ulCorner;// new Point((int)transformedPoint.X, (int)transformedPoint.Y);

			//PointF local = m_map.Canvas.Camera.GlobalToLocal(targetLocation);
			//PointF view = m_map.Canvas.Camera.LocalToView(targetLocation);
			//m_map.Canvas.Camera.ScaleViewBy(0.999999f, view.X, view.Y);
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
					statusBar1.Panels[0].Text = "Combat phase";
					//MessageBox.Show("Movement over, time for combat");

					switch(m_controller.CurrentPhase)
					{
						case GamePhase.Combat:
						{
							if(m_combatForm == null)
							{
								m_combatForm = new CombatForm(m_controller, m_battleController);
							}

							m_controller.FindBattles();

							if(m_controller.Battles.Count == 0)
							{
								MessageBox.Show("No battles this turn - moving to production");
								m_controller.NextPhase();
								
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
							}

							m_productionForm.SetupProduction();
							m_productionForm.ShowDialog();

							m_controller.NextTurn();
							m_map.AdvancePlanets();
							statusBar1.Panels[0].Text = "Current player: " + m_controller.CurrentPlayer.Name;
							statusBar1.Panels[1].Text = "Turn: " + m_controller.TurnNumber.ToString();
							break;
						}
					}
					
					

					break;
				}
				
			}		
	
			return result;
		}

		private void button1_Click_1(object sender, System.EventArgs e)
		{
			int i = 42;
			int q = i;
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
