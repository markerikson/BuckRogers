#region using directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.Piccolo.Activities;
using UMD.HCIL.PiccoloX;
using UMD.HCIL.PiccoloX.Activities;
using UMD.HCIL.PiccoloX.Nodes;
using UMD.HCIL.PiccoloX.Components;
using UMD.HCIL.Piccolo.Event;

using BuckRogers;
using BuckRogers.Networking;

#endregion

#region algorithm comments
/*
Each player's display is a PComposite that looks like:

PComposite:
 - PText (name)
 - PComposite * 8
   - PImage (icon)
   - PText (status)
   
Sequence:

* Click on unit composite
	- Current player's composite?
		- Yes: Anything selected?
			- Yes: Selected unit clicked?
				- Yes: Deselect unit
				- No: Select other unit
			- No: Select this unit
		- No: Attacking unit selected?
			- Yes: Left click or right click?
				- Left: attack once
				- Right: attack until no attackers left 
						 or all defenders dead
			- No: Ignore

Note:	It's another edge case, but it would be possible to have a battler around Earth or Venus, 
		all territories owned by enemies, and have the enemies invading each other.  That would 
		result in more than 7 separate territory/enemy combos.  So, I'm thinking of just having 
		"right arrow" and "left arrow" nodes that would allow you to "turn the page".  In fact, 
		that might work better than having 8 sets on screen at once!  Limit it to six, just page
		through any extras!  And of course, the odds of this happening are slim, but it's worth it
		(yay for flexible design!).
		
  
  
 */

#endregion

namespace BuckRogers.Interface
{
	public partial class CombatControl : UserControl
	{
		#region private members

		private ClientSideGameManager m_csgm;
		private GameController m_controller;
		private BattleController m_battleController;
		private IconManager m_iconManager;

		private static Object m_locker = new Object();

		private PCanvas m_canvas;
		private PScrollableControl m_scroller;

		private PText m_lblBattleLocation;
		private PText m_lblBattleType;
		private PText m_lblBattlesLeft;

		private PText m_lblNextPlayers;
		private PText m_lblPrevPlayers;

		private PText m_tooltip;

		//private PlayerUnitDisplay[] m_displays;
		private Hashlist m_displays;
		private Hashlist m_availableDisplays;
		private PlayerUnitDisplay m_currentPUD;
		private ArrayList m_displayedResults;
		private ArrayList m_availableResults;
		private UnitInfoDisplay m_selectedUID;
		private PointF[] m_resultLocations;

		private PointF[] m_playerLocationsNormal;
		private PointF[] m_playerLocationsExtended;

		private int m_displayedPage;
		private int m_numPages;

		private Player m_player;

		private int m_numAttacks;

		private ArrayList m_combatMessages;
		private ArrayList m_bombingTargets;
		private ArrayList m_playerTerritoryNames;

		#endregion

		#region constructor

		public CombatControl(ClientSideGameManager csgm, GameController gc, BattleController bc, IconManager im)
		{
			InitializeComponent();

			m_csgm = csgm;
			m_controller = gc;
			m_battleController = bc;
			m_iconManager = im;

			m_csgm.ClientUpdateMessage += new EventHandler<ClientUpdateEventArgs>(OnClientUpdateMessage);

			m_combatMessages = new ArrayList();

			m_canvas = new PCanvas();
			PRoot r = new ScreenshotRoot();
			PLayer l = new PLayer();
			PCamera c = new PCamera();

			r.AddChild(c);
			r.AddChild(l);
			c.AddLayer(l);

			this.SuspendLayout();

			m_scroller = new RefreshingScrollableControl(m_canvas);
			m_scroller.Scrollable = false;
			this.Controls.Add(m_scroller);

			m_scroller.Anchor =
				AnchorStyles.Bottom |
				AnchorStyles.Top |
				AnchorStyles.Left |
				AnchorStyles.Right;

			m_scroller.Bounds = this.ClientRectangle;

			this.ResumeLayout(false);

			m_canvas.Camera = c;

			PBasicInputEventHandler tipEventHandler = new PBasicInputEventHandler();
			tipEventHandler.MouseMove = new MouseMoveDelegate(MouseMoveHandler);
			c.AddInputEventListener(tipEventHandler);

			m_tooltip = new PText();
			m_tooltip.TextBrush = Brushes.Black;
			m_tooltip.Brush = Brushes.Aquamarine;
			m_tooltip.Pickable = false;
			Font font = m_tooltip.Font;
			m_tooltip.Font = new Font(font.Name, font.SizeInPoints - 2, FontStyle.Bold);
			c.AddChild(m_tooltip);

			PlayerUnitDisplay.IconManager = im;

			m_canvas.AnimatingRenderQuality = RenderQuality.HighQuality;
			m_canvas.InteractingRenderQuality = RenderQuality.HighQuality;

			CreateInitialDisplays();

			m_playerLocationsNormal = new PointF[]{	new PointF ( 400, 2 ), 
													new PointF ( 400, 408 ), 
													new PointF ( 675, 85 ), 
													new PointF ( 130, 325 ), 
													new PointF ( 675, 325 ), 
													new PointF ( 130, 85 ), 
													
												};

			m_playerLocationsExtended = new PointF[]{	new PointF ( 400, 2 ), 
													new PointF ( 400, 408 ), 
													new PointF ( 680, 42 ), 
													new PointF ( 130, 416 ), 
													new PointF ( 675, 229 ),
													new PointF ( 130, 229 ), 
													new PointF ( 675, 416 ), 
													new PointF ( 130, 42 ), 
												};

			m_displayedResults = new ArrayList();
			m_availableResults = new ArrayList();

			m_resultLocations = new PointF[10];

			for (int i = 0; i < 10; i++)
			{
				m_resultLocations[i] = new PointF(268, 195 + (i * 20));

				CombatMessage cm = new CombatMessage(m_canvas);
				m_availableResults.Add(cm);	
			}

			m_numAttacks = 0;

			m_lblBattlesLeft = new PText();
			m_lblBattleType = new PText();
			m_lblBattleLocation = new PText();

			PText lbl1 = new PText();
			PText lbl2 = new PText();
			PText lbl3 = new PText();

			lbl1.Text = "Battle Location ";
			lbl2.Text = "Battle Type:";
			lbl3.Text = "Battles Left:";

			Font f = lbl1.Font;

			f = new Font(f, FontStyle.Bold);

			lbl1.Font = f;
			lbl2.Font = f;
			lbl3.Font = f;

			lbl1.X = 10;
			lbl1.Y = 10;

			lbl2.X = 560;
			lbl2.Y = 10;

			lbl3.X = 560;
			lbl3.Y = 25;


			m_lblBattleLocation.X = 10;
			m_lblBattleLocation.Y = 25;

			m_lblBattleType.X = 660;
			m_lblBattleType.Y = 10;

			m_lblBattlesLeft.X = 660;
			m_lblBattlesLeft.Y = 25;

			m_canvas.Layer.AddChild(lbl1);
			m_canvas.Layer.AddChild(lbl2);
			m_canvas.Layer.AddChild(lbl3);

			m_canvas.Layer.AddChild(m_lblBattleLocation);
			m_canvas.Layer.AddChild(m_lblBattleType);
			m_canvas.Layer.AddChild(m_lblBattlesLeft);

			m_lblBattleLocation.Text = "Australian Development Facility";
			m_lblBattleType.Text = "Killer Satellite";
			m_lblBattlesLeft.Text = "42";

			// Unicode characters for "black right pointer" and "black left pointer"
			m_lblNextPlayers = new PText("More Targets \u25BA");
			m_lblPrevPlayers = new PText("\u25C4 Previous Targets");

			m_lblNextPlayers.X = 675 + (-1 * (m_lblNextPlayers.Width / 2));
			m_lblNextPlayers.Y = 550;

			m_lblPrevPlayers.X = 130 + (-1 * (m_lblPrevPlayers.Width / 2));
			m_lblPrevPlayers.Y = 550;

			m_lblNextPlayers.MouseEnter += new PInputEventHandler(OnLabelMouseEnter);
			m_lblNextPlayers.MouseLeave += new PInputEventHandler(OnLabelMouseLeave);

			m_lblPrevPlayers.MouseEnter += new PInputEventHandler(OnLabelMouseEnter);
			m_lblPrevPlayers.MouseLeave += new PInputEventHandler(OnLabelMouseLeave);

			m_lblNextPlayers.MouseUp += new PInputEventHandler(OnPageButtonClicked);
			m_lblPrevPlayers.MouseUp += new PInputEventHandler(OnPageButtonClicked);

			//m_battleController.UnitsToDisplay += new DisplayUnitsHandler(DisplayUnits);
			m_battleController.UnitsToDisplay += new EventHandler<DisplayUnitsEventArgs>(DisplayUnits);
			m_battleController.StatusUpdate += new EventHandler<StatusUpdateEventArgs>(OnBattleControllerStatusUpdate);//new StatusUpdateHandler(OnBattleControllerStatusUpdate);
			m_battleController.BattleStatusUpdated += new EventHandler<StatusUpdateEventArgs>(OnBattleStatusUpdated);//new BattleStatusUpdateHandler(OnBattleStatusUpdated);
		}

		

		#endregion

		#region event handlers

		void OnClientUpdateMessage(object sender, ClientUpdateEventArgs e)
		{
			switch(e.MessageType)
			{
				default:
					break;
			}
		}

		private void OnLabelMouseEnter(object sender, PInputEventArgs e)
		{
			if (!(sender is PText))
			{
				return;
			}

			PText text = sender as PText;

			Font f = text.Font;
			text.Font = new Font(f.Name, f.SizeInPoints, FontStyle.Bold);
		}

		private void OnLabelMouseLeave(object sender, PInputEventArgs e)
		{
			if (!(sender is PText))
			{
				return;
			}

			PText text = sender as PText;

			Font f = text.Font;
			text.Font = new Font(f.Name, f.SizeInPoints, FontStyle.Regular);
		}

		void OnPageButtonClicked(object sender, PInputEventArgs e)
		{
			PText button = sender as PText;

			if(button == m_lblNextPlayers)
			{
				DisplayPage(m_displayedPage + 1);
			}
			else if(button == m_lblPrevPlayers)
			{
				DisplayPage(m_displayedPage - 1);
			}
		}

		public void NodeClicked(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
		{
			PNode picked = (PNode)e.PickedNode;

			object obj = picked.Tag;

			if (obj is UnitInfoDisplay)
			{
				UnitInfoDisplay clickedUID = (UnitInfoDisplay)obj;
				PlayerUnitDisplay pud = clickedUID.PlayerDisplay;

				if (pud.Player == m_player)
				{
					Hashtable uids = pud.UIDs;

					// "Anything selected?"
					foreach (DictionaryEntry de in uids)
					{
						UnitInfoDisplay uid = (UnitInfoDisplay)de.Value;

						if (uid.Selected)
						{
							m_selectedUID = uid;
							break;
						}
					}

					// Nothing should be selected
					if (clickedUID == m_selectedUID)
					{
						m_selectedUID = null;
					}
					else
					{
						// De-select the selected icon and save the clicked icon for later
						if (m_selectedUID != null)
						{
							m_selectedUID.Selected = !m_selectedUID.Selected;
						}

						m_selectedUID = clickedUID;
					}

					clickedUID.Selected = !clickedUID.Selected;

				}
				else
				{
					ArrayList messages = new ArrayList();

					if (m_selectedUID != null)
					{
						int numAttacks = 0;

						if (e.Button == MouseButtons.Left)
						{
							m_numAttacks++;

							numAttacks = 1;
						}
						else if (e.Button == MouseButtons.Right)
						{
							for (int i = 0; i < 5; i++)
							{
								m_numAttacks++;
							}

							numAttacks = 5;
						}

						DoAttack(clickedUID, numAttacks);

						UpdateToolTip(e);
					}
					else
					{
					}
				}
			}
			else if (obj is PlayerUnitDisplay)
			{
				PlayerUnitDisplay pud = (PlayerUnitDisplay)obj;

				MessageBox.Show("Clicked player: " + pud.Player.Name);
			}
		}

		public void MouseMoveHandler(object sender, PInputEventArgs e)
		{
			UpdateToolTip(e);
		}

		public void UpdateToolTip(PInputEventArgs e)
		{
			PNode n = (PNode)e.InputManager.MouseOver.PickedNode;

			String tooltipString = null;
			UnitInfoDisplay info = null;

			if (n.Tag is IconInfo)
			{
				info = (UnitInfoDisplay)n.Tag;

				tooltipString = string.Format("Type: {0}\nAlive: {1}\nDead: {2}\nCan shoot: {3}\nTotal: {4}",
											info.Type, info.NumAlive, info.NumDead, info.NumCanShoot, info.NumTotal);
			}

			if (tooltipString == null || info.NumTotal == 0)
			{
				m_tooltip.Visible = false;
				return;
			}
			else
			{
				PointF p = e.CanvasPosition;

				p = e.Path.CanvasToLocal(p, m_canvas.Camera);

				if (m_tooltip.Text != tooltipString)
				{
					m_tooltip.Text = tooltipString;

					if (m_tooltip.Bounds.Width > 350)
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

				float x = info.Icon.X + 60;
				float y = info.Icon.Y;

				m_tooltip.SetOffset((int)x, (int)y);
				m_tooltip.Visible = true;

				RectangleF tipBounds = m_tooltip.Bounds;
				m_tooltip.RepaintFrom(tipBounds, m_tooltip);
			}
		}

		#endregion

		#region player displays

		private void CreateInitialDisplays()
		{
			m_availableDisplays = new Hashlist();
			m_displays = new Hashlist();

			// Start with the initial six displays
			for (int i = 0; i < 6; i++)
			{
				CreateDisplay(i);
			}

			for (int i = 0; i < m_availableDisplays.Count; i++)
			{
				PlayerUnitDisplay pud = (PlayerUnitDisplay)m_availableDisplays[i];
				pud.HideDisplay();
			}
		}

		private void CreateDisplay(int playerIndex)
		{
			UnitType[] types = {UnitType.Trooper, UnitType.Gennie, UnitType.Leader, UnitType.Factory, 
								UnitType.Fighter, UnitType.Battler, UnitType.Transport, UnitType.KillerSatellite};

			Hashtable uids = new Hashtable();

			PlayerUnitDisplay pud = new PlayerUnitDisplay(this, m_canvas);
			m_availableDisplays.Add(pud.GetHashCode(), pud);
			pud.Composite.Tag = pud;

			PText name = new PText();
			pud.Label = name;
			Font f = name.Font;
			name.Font = new Font(f.Name, f.SizeInPoints + 3, FontStyle.Bold);

			pud.Composite.AddChild(name);
		}



		private void CreatePlayerDisplays()
		{
			// Never more than six displays being shown at a time
			//m_displays = new PlayerUnitDisplay[6];   //m_controller.Players.Length];
			m_displays = new Hashlist();

			UnitType[] types = {UnitType.Trooper, UnitType.Gennie, UnitType.Leader, UnitType.Factory, 
								UnitType.Fighter, UnitType.Battler, UnitType.Transport, UnitType.KillerSatellite};

			for (int i = 0; i < 6; i++)
			{
				// Make sure all the players' icons get grabbed
				Player p = m_controller.Players[i];

				Hashtable uids = new Hashtable();

				PlayerUnitDisplay pud = new PlayerUnitDisplay(this, m_canvas);
				m_displays.Add(i, pud);
				pud.Composite.Tag = pud;

				PText name = new PText(p.Name);
				pud.Label = name;
				Font f = name.Font;
				name.Font = new Font(f.Name, f.SizeInPoints + 3, FontStyle.Bold);

				pud.Composite.AddChild(name);
			}
		}

		private void UpdatePlayerDisplays()
		{
			switch(m_battleController.CurrentBattle.Type)
			{
				case BattleType.Normal:
				case BattleType.KillerSatellite:
				{
					for(int i = 0; i < m_displays.Count; i++)
					{
						PlayerUnitDisplay pud = (PlayerUnitDisplay)m_displays[i];

						pud.Location = m_playerLocationsNormal[i];
						pud.LayoutChildren();
						pud.ShowDisplay();
					}

					m_lblNextPlayers.RemoveFromParent();
					m_lblPrevPlayers.RemoveFromParent();

					break;
				}
				case BattleType.Bombing:
				{
					int numOtherDisplays = m_displays.Count - 1;
					m_numPages = numOtherDisplays / 5;
					m_numPages += (numOtherDisplays % 5 > 0) ? 1 : 0;
					m_displayedPage = 1;

					PlayerUnitDisplay bomberPUD = (PlayerUnitDisplay)m_displays[0];

					bomberPUD.Location = m_playerLocationsNormal[0];
					bomberPUD.LayoutChildren();
					bomberPUD.ShowDisplay();

					DisplayPage(1);

					break;
				}
			}
		}

		private void DisplayPage(int pageNum)
		{
			int pageIndex = pageNum - 1;
			int displayIndex = 0;

			displayIndex = (pageIndex * 5) + 1;

			int highestIndexForPage = (pageIndex + 1) * 5;
			int maxDisplayIndex = Math.Min(highestIndexForPage, m_displays.Count - 1);

			for (int i = 1; i < displayIndex; i++)
			{
				PlayerUnitDisplay pud = (PlayerUnitDisplay)m_displays[i];
				pud.HideDisplay();
			}

			int locationIndex = 1;
			for (int i = displayIndex; i <= maxDisplayIndex; i++)
			{
				PlayerUnitDisplay pud = (PlayerUnitDisplay)m_displays[i];

				pud.Location = m_playerLocationsNormal[locationIndex];
				pud.LayoutChildren();
				pud.ShowDisplay();

				locationIndex++;
			}

			for (int i = maxDisplayIndex + 1; i < m_displays.Count; i++)
			{
				PlayerUnitDisplay pud = (PlayerUnitDisplay)m_displays[i];
				pud.HideDisplay();
			}

			m_displayedPage = pageNum;

			if(m_displayedPage < m_numPages)
			{
				m_canvas.Layer.AddChild(m_lblNextPlayers);
			}
			else
			{
				m_lblNextPlayers.RemoveFromParent();
			}

			if(m_displayedPage > 1)
			{
				m_canvas.Layer.AddChild(m_lblPrevPlayers);
			}
			else
			{
				m_lblPrevPlayers.RemoveFromParent();
			}
		}

		public void DisplayUnits(object sender, DisplayUnitsEventArgs duea)
		{
			PlayerUnitDisplay pud = GetPlayerUnitDisplay(duea.Player, duea.Territory);

			// Based on the new setup, should only ever display one 
			// player's info at a time

			Hashtable counts = duea.Units.GetUnitTypeCount();

			bool displayingDeadUnits = (duea.Category == DisplayCategory.DeadUnits);
			bool nonCombatUnits = (duea.Category == DisplayCategory.NonCombatUnits);

			foreach (DictionaryEntry de in counts)
			{
				UnitType ut = (UnitType)de.Key;
				int count = (int)de.Value;

				if (!displayingDeadUnits)
				{
					if (!nonCombatUnits)
					{
						pud.UpdateUnitCount(ut, count);
					}
					else
					{
						pud.UpdateUnitCount(ut, count, 0, count, 0);
					}
				}
				else
				{
					pud.UpdateDeathCount(ut, count);
				}
			}

			pud.ShowDisplay();
		}

		public PlayerUnitDisplay GetPlayerUnitDisplay(Player p, Territory t)
		{
			PlayerUnitDisplay pud = null;

			string identifier = p.Name + " - " + t.Name;

			if (m_displays.ContainsKey(identifier))
			{
				pud = (PlayerUnitDisplay)m_displays[identifier];
			}
			else
			{
				if (m_availableDisplays.Count == 0)
				{
					// Initialize it with the first player.  Doesn't matter,
					// cause it'll be replaced soon anyway.
					CreateDisplay(0);
				}

				pud = (PlayerUnitDisplay)m_availableDisplays[0];
				m_availableDisplays.RemoveAt(0);

				pud.Player = p;
				pud.Territory = t;
				PointF location;

				if (m_displays.Count >= 6)
				{
					int index = (m_displays.Count - 1) % 6;
					location = m_playerLocationsNormal[index];
				}
				else
				{
					location = m_playerLocationsNormal[m_displays.Count];
				}

				pud.Location = location;

				// Only need to display territory names if it's a bombing attack
				// and this isn't the first player
				if ((m_battleController.CurrentBattle.Type == BattleType.Bombing)
					&& m_displays.Count > 0)
				{
					pud.UpdatePlayerInfo(t.Name);
				}
				else
				{
					pud.UpdatePlayerInfo();
				}

				m_displays.Add(identifier, pud);
			}

			return pud;
		}

		#endregion

		#region combat functions

		private void DoAttack(UnitInfoDisplay defendingUID, int numAttacks)
		{
			CombatResult cr = null;

			UnitInfoDisplay attackingUID;
			
			if(m_battleController.CurrentBattle.Type == BattleType.KillerSatellite)
			{
				PlayerUnitDisplay pud = (PlayerUnitDisplay)m_displays[0];
				attackingUID = (UnitInfoDisplay)pud.UIDs[UnitType.KillerSatellite];
			}
			else
			{
				attackingUID = m_selectedUID;
			}

			switch (m_battleController.CurrentBattle.Type)
			{
				case BattleType.KillerSatellite:
				{
					cr = m_battleController.DoKillerSatelliteCombat(m_battleController.CurrentBattle);
					break;
				}
				case BattleType.Bombing:
				{
					CombatInfo ci = SetUpCombat(attackingUID, defendingUID, numAttacks);

					if(ci == null)
					{
						return;
					}

					ci.Type = BattleType.Bombing;

					UnitCollection leaders = m_battleController.CurrentBattle.Territory.Units.GetUnits(UnitType.Leader);
					ci.AttackingLeader = (leaders.GetUnits(m_battleController.CurrentPlayer).Count > 0);

					cr = m_battleController.DoBombingCombat(ci);
					break;
				}
				case BattleType.Normal:
				{
					CombatInfo ci = null;
					try
					{
						ci = SetUpCombat(attackingUID, defendingUID, numAttacks);

						if (ci == null)
						{
							if(defendingUID.NumAlive == 0)
							{
								MessageBox.Show("All units of this type are dead!", "I See Dead Units",
												MessageBoxButtons.OK, MessageBoxIcon.Information);
							}
							return;
						}

						ci.Type = BattleType.Normal;

						UnitCollection leaders = m_battleController.CurrentBattle.Territory.Units.GetUnits(UnitType.Leader);
						ci.AttackingLeader = (leaders.GetUnits(m_battleController.CurrentPlayer).Count > 0);

						cr = m_battleController.ExecuteCombat(ci);

					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
						return;
					}

					break;
				}
			}

			if (cr != null)
			{
				// should only be true if it's a Killer Satellite attack
				
				bool killerSatellite = (defendingUID == null);
				ArrayList m_satelliteUIDs = new ArrayList();

				// Show messages here
				foreach(AttackResult ar in cr.AttackResults)
				{
					AddAttackMessage(ar.Attacker.Owner, ar.Defender.Owner, ar.Attacker.Type, 
									ar.Defender.Type, ar.Roll, ar.Hit);

					attackingUID.Shoot();

					if (killerSatellite)
					{
						PlayerUnitDisplay defendingPUD = GetPlayerUnitDisplay(ar.Defender.Owner, m_battleController.CurrentBattle.Territory);
						defendingUID = (UnitInfoDisplay)defendingPUD.UIDs[ar.Defender.Type];
						m_satelliteUIDs.Add(defendingUID);
					}

					if(ar.Hit)
					{
						defendingUID.KillOneUnit();
					}
				}

				attackingUID.UpdateUnitStatistics();

				if(!killerSatellite)
				{
					defendingUID.UpdateUnitStatistics();
				}
				
				else
				{
					foreach(UnitInfoDisplay uid in m_satelliteUIDs)
					{
						uid.UpdateUnitStatistics();
					}
				}						
	
				m_battleController.LastResult = cr;		
				m_battleController.ProcessAttackResults();
			}
		}

		private UnitCollection GetRequestedUnits(Player p, UnitType ut, Territory territory, bool attacking, int numAttacks)
		{
			UnitCollection allMatchingUnits = new UnitCollection();

			UnitCollection matches = null;
			if(attacking)
			{
				allMatchingUnits = m_battleController.CurrentUnused.GetUnits(ut, p, territory, numAttacks);
				matches = allMatchingUnits.GetUnits(numAttacks);
			}
			else
			{
				UnitCollection playerUnits = m_battleController.SurvivingUnits.GetUnits(p);
				if (ut == UnitType.Transport)
				{
					int numTransports = playerUnits.GetUnits(UnitType.Transport).Count;
					UnitCollection otherUnits = playerUnits.GetNonMatchingUnits(UnitType.Transport);
					UnitCollection otherCombatUnits = otherUnits.GetCombatUnits();

					if (otherCombatUnits.Count > 0)
					{
						throw new Exception("Can't attack transports if other units are still alive");
					}
				}

				UnitCollection matchesType = playerUnits.GetUnits(ut);
				matchesType.RemoveAllUnits(allMatchingUnits);

				Territory t = m_controller.Map[territory.Name];
				matches = matchesType.GetUnits(t, numAttacks);

				allMatchingUnits.AddAllUnits(matches);
			}

			return allMatchingUnits;
		}

		private CombatInfo SetUpCombat(UnitInfoDisplay attackingUID, UnitInfoDisplay defendingUID, int numAttacks)
		{
			CombatInfo ci = new CombatInfo();

			UnitCollection attackers = null;
			UnitCollection defenders = null;

			try
			{						
				attackers = GetRequestedUnits(m_selectedUID.Player, attackingUID.Type, m_selectedUID.PlayerDisplay.Territory, true, numAttacks);
				ci.Attackers.AddAllUnits(attackers);
				
				defenders = GetRequestedUnits(defendingUID.Player, defendingUID.Type, defendingUID.PlayerDisplay.Territory, false, numAttacks);
				ci.Defenders.AddAllUnits(defenders);

			}
			catch(Exception ex)
			{
				if(attackers != null)
				{
					m_battleController.CurrentUnused.AddAllUnits(attackers);
				}
				
				throw ex;
			}

			if (attackers.Count == 0 || defenders.Count == 0)
			{
				return null;
			}

			return ci;
		}

		#endregion

		#region combat message functions

		// FIXME Rapid clicking sometimes puts one message at the top, out of order
		public void AddAttackMessage(Player attacker, Player defender, UnitType attackType, 
							UnitType defendType, int roll, bool hit)
		{
			int numDisplayedResults = m_displayedResults.Count;

			CombatMessage cm;
			if (numDisplayedResults == 10)
			{
				cm = (CombatMessage)m_displayedResults[0];
				m_displayedResults.RemoveAt(0);
				cm.HideMessage();

				ScrollMessagesUpward();
			}
			else
			{
				int numAvailableResults = m_availableResults.Count;
				cm = (CombatMessage)m_availableResults[numAvailableResults - 1];
				m_availableResults.RemoveAt(m_availableResults.Count - 1);
			}

			cm.SetDetails(attacker, defender, attackType, defendType, roll, hit);

			PointF location = m_resultLocations[m_displayedResults.Count];
			cm.DisplayMessage(location);

			m_displayedResults.Add(cm);
		}

		public void AddAttackMessages(ArrayList messages)
		{
			lock(m_locker)
			{
				for (int i = 0; i < messages.Count; i++)
				{
					string message = (string)messages[i];

					PText label;

					int numDisplayedResults = m_displayedResults.Count;

					if (numDisplayedResults == 10)
					{
						label = (PText)m_displayedResults[0];
						m_displayedResults.RemoveAt(0);
						m_canvas.Layer.RemoveChild(label);

						ScrollMessagesUpward();
					}
					else
					{
						int numAvailableResults = m_availableResults.Count;
						label = (PText)m_availableResults[numAvailableResults - 1];
						m_availableResults.RemoveAt(m_availableResults.Count - 1);
					}

					label.Text = message;
					PointF location = m_resultLocations[m_displayedResults.Count];
					label.X = location.X;
					label.Y = location.Y;

					if(message.Contains("Hit!"))
					{
						label.TextBrush = Brushes.Red;
					}
					else
					{
						label.TextBrush = Brushes.Black;
					}

					m_canvas.Layer.AddChild(label);
					m_displayedResults.Add(label);
				}
			}
		}

		private void ScrollMessagesUpward()
		{
			for(int i = 0; i < m_displayedResults.Count; i++)
			{
				CombatMessage cm = (CombatMessage)m_displayedResults[i];
				PointF location = m_resultLocations[i];

				if(i % 2 == 0)
				{
					//cm.DimMessage();
				}
				else
				{
					//cm.UnDimMessage();
				}

				cm.AnimateMessageTo(location);

				//RectangleF dest = new RectangleF(location.X, location.Y, label.Width, label.Height);
				//PNode.PNodeBoundsActivity pna = new PNode.PNodeBoundsActivity(label, dest, 500);
				//m_canvas.Root.ActivityScheduler.AddActivity(pna, true);
			}
		}

		public void ClearMessages()
		{
			for(int i = 0; i < m_displayedResults.Count; i++)
			{
				CombatMessage cm = (CombatMessage)m_displayedResults[i];
				cm.HideMessage();
			}

			m_availableResults.AddRange(m_displayedResults);
			m_displayedResults.Clear();
		}

		#endregion

		#region combat turn / update stuff

		public void BeginCombat()
		{
			m_battleController.Battles = m_controller.Battles;

			ResetDisplay();

			m_displayedPage = 0;

			m_battleController.LogNewTurn();

			m_csgm.ReadyToBeginCombat();

			m_battleController.NextBattle();
		}

		public void UpdateCombatInformation()
		{
			ResetDisplay();

			m_lblBattleType.Text = m_battleController.CurrentBattle.Type.ToString();
			m_lblBattlesLeft.Text = m_battleController.Battles.Count.ToString();

			Territory t = m_battleController.CurrentBattle.Territory;
			string location = string.Format("{0}\n{1}", t.System.Name, t.Name);
			m_lblBattleLocation.Text = location;

			if(m_battleController.CurrentBattle.Type == BattleType.Bombing)
			{
				m_bombingTargets = new ArrayList();
				m_playerTerritoryNames = new ArrayList();

				Player p = (Player)m_battleController.BattleOrder[0];

				PlayerUnitDisplay pud = (PlayerUnitDisplay)m_displays[0];
				pud.Player = p;
				pud.ShowDisplay();
			}
			else
			{
				for (int i = 0; i < m_battleController.BattleOrder.Count; i++)
				{
					Player p = (Player)m_battleController.BattleOrder[i];

					PlayerUnitDisplay pud = (PlayerUnitDisplay)m_displays[i];
					pud.Player = p;
					pud.ShowDisplay();
				}
			}
		}

		public void ResetDisplay()
		{
			foreach (PlayerUnitDisplay pud in m_displays)
			{
				m_availableDisplays.Add(pud.GetHashCode(), pud);
			}

			m_displays.Clear();

			foreach (PlayerUnitDisplay pud in m_availableDisplays)
			{
				pud.Selected = false;
				pud.ResetUnitCounts();
				pud.HideDisplay();

			}
		}

		#endregion

		#region battle status updates

		void OnBattleStatusUpdated(object sender, StatusUpdateEventArgs suea)//BattleStatus status)
		{
			BattleStatus status = suea.BattleStatus;
			switch (status)
			{
				case BattleStatus.BattleReady:
				{
					UpdatePlayerDisplays();
					ClearMessages();

					m_player = m_battleController.CurrentPlayer;
					m_currentPUD = (PlayerUnitDisplay)m_displays[0];
					m_currentPUD.Selected = true;

					Territory t = m_battleController.CurrentBattle.Territory;
					string systemName = string.Empty;

					if (t.System == OrbitalSystem.NONE)
					{
						systemName = t.Orbit.Name;
					}
					else
					{
						systemName = t.System.Name;
					}

					string battleLocation = string.Format("System: {0}\nTerritory: {1}",
														systemName, t.Name);

					m_lblBattleLocation.Text = battleLocation;
					m_lblBattlesLeft.Text = m_battleController.Battles.Count.ToString();
					m_lblBattleType.Text = m_battleController.CurrentBattle.Type.ToString();

					if (m_battleController.CurrentBattle.Type == BattleType.KillerSatellite)
					{
						MessageBox.Show("Killer Satellite preparing to fire...");
						DoAttack(null, 0);
					}
					break;
				}
				case BattleStatus.RoundComplete:
				{
					m_currentPUD.ClearUnitSelection();
					m_currentPUD.Selected = false;
					m_selectedUID = null;

					MessageBox.Show("Combat round complete.  Click OK to continue.", "Round Finished",
									MessageBoxButtons.OK);
					ClearMessages();

					if (m_battleController.NextPlayer())
					{
						m_player = m_battleController.CurrentPlayer;
						string territoryName = m_battleController.CurrentBattle.Territory.Name;

						string pudID = m_player.Name + " - " + territoryName;
						m_currentPUD = (PlayerUnitDisplay)m_displays[pudID];
						m_currentPUD.Selected = true;
					}
					else
					{
						goto case BattleStatus.BattleComplete;
					}

					break;
				}
				case BattleStatus.BattleComplete:
				{
					MessageBox.Show("Battle finished.  Click OK to continue.", "Battle Finished",
									MessageBoxButtons.OK);

					ResetDisplay();

					if (!m_battleController.NextBattle())
					{
						ClearMessages();
						m_battleController.CombatComplete();
						MessageBox.Show("All battles finished");
						Form parent = (Form)this.Parent;
						parent.DialogResult = DialogResult.OK;
					}
					break;
				}
			}
		}

		private void OnBattleControllerStatusUpdate(object sender, StatusUpdateEventArgs suea)
		{
			bool result = true;

			switch (suea.StatusInfo)
			{
				case StatusInfo.FactoryConquered:
				{
					Territory location = suea.Territories[0];
					string message = location.Name + " has been conquered and a factory is about to be captured.  Sabotage the factory?";
					DialogResult dr = MessageBox.Show(message, "Sabotage Factory?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

					result = (dr == DialogResult.Yes);
					suea.Result = result;
					break;
				}
				case StatusInfo.SabotageResult:
				{
					string message = String.Empty;

					if (suea.Result)
					{
						message = "Sabotage successful!  The factory was destroyed";
					}
					else
					{
						message = "Sabotage failed.  The factory is still intact.";
					}

					MessageBox.Show(message, "Sabotage Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
					break;
				}
				case StatusInfo.LeaderKilled:
				{
					string message = suea.Player.Name + "'s leader has been killed!";
					MessageBox.Show(message, "Leader Killed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					break;
				}
				// TODO Um... why isn't this being used anywhere?
				case StatusInfo.PlayerKilled:
				{
					PlayerUnitDisplay pud = GetPlayerUnitDisplay(suea.Player, suea.Territories[0]);

					pud.DimDisplay();
					break;
				}
			}

			return;
		}

		#endregion

	}

	#region PulseTarget
	class PulseTarget : PColorActivity.Target
	{
		PPath node;
		public PulseTarget(PPath node)
		{
			this.node = node;
		}

		public Color Color
		{
			get
			{
				return ((Pen)node.Pen).Color;
			}
			set
			{
				node.Pen = new Pen(value);
			}
		}
	}

	#endregion


}


