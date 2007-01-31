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
 */

namespace BuckRogers.Interface
{
	public partial class CombatControl : UserControl
	{
		private GameController m_controller;
		private BattleController m_battleController;
		private IconManager m_iconManager;

		private static Object m_locker = new Object();

		private PCanvas m_canvas;
		private PScrollableControl m_scroller;

		private PText m_lblBattleLocation;
		private PText m_lblBattleType;
		private PText m_lblBattlesLeft;

		private PlayerUnitDisplay[] m_displays;
		//private Hashlist m_displays;
		//private PText[] m_results;
		private ArrayList m_displayedResults;
		private ArrayList m_availableResults;
		private UnitInfoDisplay m_selectedUID;
		private PointF[] m_resultLocations;

		private PointF[] m_playerLocationsNormal;
		private PointF[] m_playerLocationsExtended;

		private Player m_player;

		private int m_numAttacks;

		private ArrayList m_combatMessages;


		public CombatControl(GameController gc, BattleController bc, IconManager im)
		{
			InitializeComponent();

			m_combatMessages = new ArrayList();

			//m_playerUIDs = new Hashtable();

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

			m_controller = gc;
			m_battleController = bc;
			m_iconManager = im;

			PlayerUnitDisplay.IconManager = im;

			//PText label = new PText("Testing");
			//m_canvas.Layer.AddChild(label);

			m_canvas.AnimatingRenderQuality = RenderQuality.HighQuality;
			m_canvas.InteractingRenderQuality = RenderQuality.HighQuality;

			CreatePlayerDisplays();

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


			int numDisplaysToShow = 6; // m_displays.Length
			PointF[] locations = m_playerLocationsNormal; //m_playerLocationsExtended;
			for(int i = 0; i < numDisplaysToShow; i++)
			{
				PlayerUnitDisplay pud = (PlayerUnitDisplay)m_displays[i];
				pud.Location = locations[i];//new PointF(200, 50);
				pud.LayoutChildren();
				pud.Player = m_controller.Players[i % m_controller.Players.Length];
				pud.UpdatePlayerInfo(i.ToString());
				//m_canvas.Layer.AddChild(pud.Composite);

				pud.ShowDisplay();

				//DecoratorGroup dg = new DecoratorGroup();
				//dg.AddChild(pud.Composite);
				//dg.Pen = new Pen(pud.Player.Color);

				//m_canvas.Layer.AddChild(dg);

				if(pud.Player.Name == "Mark")
				{
					m_player = pud.Player;
				}

				/*
				if(pud.Player.Name == "Chris" ||
					pud.Player.Name == "Jake")
				{
					pud.HideDisplay(m_canvas);
				}
				*/				
			}

			for (int i = numDisplaysToShow; i < m_displays.Length; i++)
			{
				m_displays[i].HideDisplay();
			}

				m_displayedResults = new ArrayList(); //new PText[10];
			m_availableResults = new ArrayList();

			m_resultLocations = new PointF[10];

			for (int i = 0; i < 10; i++)
			{
				m_resultLocations[i] = new PointF(268, 200 + (i * 20));

				CombatMessage cm = new CombatMessage(m_canvas);
				m_availableResults.Add(cm);


				/*
				PText label = new PText();
				m_availableResults.Add(label);

				Font f = label.Font;
				label.Font = new Font(f.Name, f.SizeInPoints - 2, FontStyle.Bold);				

				label.Text = "Testing " + i;
				label.X = m_resultLocations[i].X;
				label.Y = m_resultLocations[i].Y;

				m_canvas.Layer.AddChild(label);
				*/				
			}

			m_numAttacks = 0;

			m_lblBattlesLeft = new PText();
			m_lblBattleType = new PText();
			m_lblBattleLocation = new PText();

			PText lbl1 = new PText();
			PText lbl2 = new PText();
			PText lbl3 = new PText();

			lbl1.Text = "Battle Location: ";
			lbl2.Text = "Battle Type:";
			lbl3.Text = "Battles Left:";

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

			/*
			CombatMessage cm = new CombatMessage(m_canvas);

			cm.SetDetails(m_controller.Players[0], m_controller.Players[1], UnitType.Trooper, UnitType.Gennie, 8, true);
			cm.DisplayMessage(new PointF(268, 200));

			CombatMessage cm2 = new CombatMessage(m_canvas);

			cm2.SetDetails(m_controller.Players[3], m_controller.Players[4], UnitType.Fighter, UnitType.KillerSatellite, 3, false);
			cm2.DisplayMessage(new PointF(268, 280));

			m_combatMessages.Add(cm);
			m_combatMessages.Add(cm2);
			*/
			
		}

		

		private void CreatePlayerDisplays()
		{
			// Worse-case scenario: 1 attacker, 7 defenders (bombing Earth or Venus)
			m_displays = new PlayerUnitDisplay[8];   //m_controller.Players.Length];
			//m_displays = new Hashlist();

			UnitType[] types = {UnitType.Trooper, UnitType.Gennie, UnitType.Leader, UnitType.Factory, 
								UnitType.Fighter, UnitType.Battler, UnitType.Transport, UnitType.KillerSatellite};

			//foreach(Player p in m_game.Players)
			//for (int i = 0; i < m_controller.Players.Length; i++)
			for (int i = 0; i < 8; i++)
			{
				// Make sure all the players' icons get grabbed
				Player p = m_controller.Players[i % m_controller.Players.Length];
				

				Hashtable uids = new Hashtable();

				PlayerUnitDisplay pud = new PlayerUnitDisplay(this, m_canvas);
				m_displays[i] = pud;
				//m_displays.Add(p, pud);
				//pud.Player = p;
				pud.Composite.Tag = pud;
				//pud.Canvas = m_canvas;

				PText name = new PText(p.Name);
				pud.Label = name;
				Font f = name.Font;
				name.Font = new Font(f.Name, f.SizeInPoints + 3, FontStyle.Bold);

				pud.Composite.AddChild(name);


				//pud.UIDs = uids;

				/*
				if(!PlayerUnitDisplay.PlayerUIDs.ContainsKey(p))
				{
					Hashtable icons = m_iconManager.GetPlayerIcons(p);

					foreach (UnitType ut in types)
					{
						Bitmap b = (Bitmap)icons[ut];

						UnitInfoDisplay uid = new UnitInfoDisplay(m_canvas);

						uid.Composite = new PComposite();
						uid.Icon = new PImage(b);
						uid.Label = new PText("T: 0, S: 0\nA: 0, D: 0");
						uid.Label.TextBrush = Brushes.Black;
						f = uid.Label.Font;
						uid.Label.Font = new Font(f.Name, f.SizeInPoints - 4, FontStyle.Bold);

						uid.Composite.AddChild(uid.Icon);
						uid.Composite.AddChild(uid.Label);
						m_canvas.Layer.AddChild(uid.Composite);
						//displayNode.AddChild(uid.Composite);
						//displayNode.AddChild(uid.Icon);
						//displayNode.AddChild(uid.Label);

						uid.Type = ut;
						uid.Composite.Tag = uid;

						uid.PlayerDisplay = pud;
						uid.Player = pud.Player;
						uid.Composite.MouseUp += new UMD.HCIL.Piccolo.PInputEventHandler(NodeClicked);

						uids[ut] = uid;
					}

					PlayerUnitDisplay.PlayerUIDs[p] = uids;
				}
				
				*/
				
				//displayNode.MouseUp += new UMD.HCIL.Piccolo.PInputEventHandler(NodeClicked);
			}
		}


		public void NodeClicked(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
		{
			PNode picked = (PNode)e.PickedNode;

			object obj = picked.Tag;

			

			if(obj is UnitInfoDisplay)
			{
				UnitInfoDisplay clickedUID = (UnitInfoDisplay)obj;
				PlayerUnitDisplay pud = clickedUID.PlayerDisplay;
				//MessageBox.Show("Clicked type: " + name);

				// "Current player's composite?"
				//if(pud.Player == m_battle.CurrentPlayer)

				/*
				if(!m_battle.BattleOrder.Contains(pud.Player))
				{
					return;
				}
				*/


				if(pud.Player == m_player)
				{
					Hashtable uids = pud.UIDs;
					//UnitInfoDisplay selectedUID = null;
					
					// "Anything selected?"
					foreach(DictionaryEntry de in uids)
					{
						UnitInfoDisplay uid = (UnitInfoDisplay)de.Value;

						if(uid.Selected)
						{
							m_selectedUID = uid;
							break;
						}
					}

					if (clickedUID == m_selectedUID)
					{
						m_selectedUID = null;	
					}
					else
					{
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

					if(m_selectedUID != null)
					{
						int numAttacks = 0;

						if(e.Button == MouseButtons.Left)
						{
							m_numAttacks++;
							//MessageBox.Show("Attacking once");

							/*
							string message = string.Format(//"Attacking one of {0}'s {1}s with one of {2}'s {3}s",
															"{2}'s {3} attacks one of {0}'s {1}s ({4})",
															clickedUID.Player.Name, clickedUID.Type.ToString(),
															m_selectedUID.Player.Name, m_selectedUID.Type.ToString(), 
															m_numAttacks);
							MessageBox.Show(message);

							messages.Add(message);
							*/

							numAttacks = 1;
						}
						else if(e.Button == MouseButtons.Right)
						{
							//MessageBox.Show("Attacking with a full stack");

							for(int i = 0; i < 7; i++)
							{
								m_numAttacks++;
								/*
								string message = string.Format(//"Attacking {0}'s {1}s with all of {2}'s {3}s",
															"{2}'s {3}s all attack {0}'s {1}s ({4})",
															clickedUID.Player.Name, clickedUID.Type.ToString(),
															m_selectedUID.Player.Name, m_selectedUID.Type.ToString(),
															m_numAttacks);
								messages.Add(message);
								*/
							}
							
							//MessageBox.Show(message);

							numAttacks = 7;
						}

						//AddAttackMessages(messages);
						Random r = new Random();

						for(int i = 0; i < numAttacks; i++)
						{
							int roll = r.Next(1, 11);
							bool hit = (roll > 6);
							AddAttackMessage(m_selectedUID.Player, clickedUID.Player, m_selectedUID.Type,
											clickedUID.Type, roll, hit);
						}
					}
					else
					{
						//CombatMessage cm2 = (CombatMessage)m_combatMessages[1];
						//cm2.AnimateMessageTo(new PointF(268, 220));
					}
				}

				//bool selected = uid.Selected;
				//uid.Selected = !selected;

				
			}
			else if(obj is PlayerUnitDisplay)
			{
				PlayerUnitDisplay pud = (PlayerUnitDisplay)obj;

				MessageBox.Show("Clicked player: " + pud.Player.Name);
			}		
		}

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
				//m_canvas.Layer.RemoveChild(label);
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
				//PText label = (PText)m_displayedResults[i];

				CombatMessage cm = (CombatMessage)m_displayedResults[i];
				PointF location = m_resultLocations[i];
				//label.AnimateToBounds(location.X, location.Y, label.Width, label.Height, 500);

				cm.AnimateMessageTo(location);

				//RectangleF dest = new RectangleF(location.X, location.Y, label.Width, label.Height);
				//PNode.PNodeBoundsActivity pna = new PNode.PNodeBoundsActivity(label, dest, 500);
				//m_canvas.Root.ActivityScheduler.AddActivity(pna, true);
			}
		}

		public void ClearMessages()
		{
			for (int i = 0; i < m_displayedResults.Count; i++)
			{
				PText label = (PText)m_displayedResults[i];
				label.RemoveFromParent();
			}

			m_availableResults.AddRange(m_displayedResults);
			m_displayedResults.Clear();
		}

		public void BeginCombat()
		{
			m_battleController.Battles = m_controller.Battles;

			m_battleController.LogNewTurn();
			m_battleController.NextBattle();

			//UpdateCombatInformation();
		}

		public void UpdateCombatInformation()
		{
			ResetDisplay();

			m_lblBattleType.Text = m_battleController.CurrentBattle.Type.ToString();
			m_lblBattlesLeft.Text = m_battleController.Battles.Count.ToString();

			Territory t = m_battleController.CurrentBattle.Territory;
			string location = string.Format("{0}\n{1}", t.System.Name, t.Name);
			m_lblBattleLocation.Text = location;

			foreach (Player p in m_battleController.BattleOrder)
			{
				//m_lbCurrentPlayer.Items.Add(p.Name);
				for(int i = 0; i < m_displays.Length; i++)
				{
					PlayerUnitDisplay pud = (PlayerUnitDisplay)m_displays[i];
					if(pud.Player == p)
					{
						pud.ShowDisplay();
						break;
					}
				}
			}

		}

		public void ResetDisplay()
		{
			foreach(PlayerUnitDisplay pud in m_displays)
			{
				pud.HideDisplay();				
			}
		}
	}

	public class PlayerUnitDisplay
	{
		private PComposite m_composite;
		private PointF m_location;
		private Player m_player;
		private PPath m_cover;
		private PPath m_selection;
		private bool m_selected;
		private PCanvas m_canvas;
		PColorActivity m_activity;
		private Territory m_territory;
		private CombatControl m_combatDisplay;

		public Territory Territory
		{
			get { return m_territory; }
			set { m_territory = value; }
		}

		private PText m_label;		

		private Hashtable m_uids;
		private static Hashtable m_playerUIDs = new Hashtable();
		private static IconManager m_iconManager;

		

		

		private static UnitType[] m_types = {UnitType.Trooper, UnitType.Gennie, UnitType.Leader, UnitType.Factory, 
								UnitType.Fighter, UnitType.Battler, UnitType.Transport, UnitType.KillerSatellite};


		public static Hashtable PlayerUIDs
		{
			get { return PlayerUnitDisplay.m_playerUIDs; }
			set { PlayerUnitDisplay.m_playerUIDs = value; }
		}

		public static IconManager IconManager
		{
			get { return PlayerUnitDisplay.m_iconManager; }
			set { PlayerUnitDisplay.m_iconManager = value; }
		}

		public bool Selected
		{
			get { return m_selected; }

			set
			{
				m_selected = value;

				ToggleSelection();
			}
		}

		public PCanvas Canvas
		{
			get { return m_canvas; }
			set { m_canvas = value; }
		}

		public Player Player
		{
			get { return m_player; }
			set { m_player = value; }
		}


		public Hashtable UIDs
		{
			get { return m_uids; }
			set { m_uids = value; }
		}

		public PComposite Composite
		{
			get { return m_composite; }
			set { m_composite = value; }
		}

		public PText Label
		{
			get { return m_label; }
			set { m_label = value; }
		}

		public PointF Location
		{
			get { return m_location; }
			set { m_location = value; }
		}


		public PlayerUnitDisplay(CombatControl combatDisplay, PCanvas canvas)
		{
			m_combatDisplay = combatDisplay;
			m_canvas = canvas;
			m_composite = new PComposite();
			m_composite.Tag = this;
			m_uids = new Hashtable();

			CreateUIDs();
		}

		public void UpdateUnitCount(UnitType type, int total, int canShoot, int alive, int dead)
		{

		}

		public int[] GetUnitCount(UnitType type)
		{
			return null;
		}

		public void LayoutChildren()
		{
			m_label.X = m_location.X + (-1 * (m_label.Width / 2));
			m_label.Y = m_location.Y;

			float[] locations = { -128, -64, 0, 64 };

			for (int i = 0; i < m_types.Length; i++)
			{
				UnitType ut = m_types[i];
				UnitInfoDisplay uid = (UnitInfoDisplay)m_uids[ut];

				float height = m_label.Y + m_label.Height + 1;

				if(i >= (m_types.Length / 2))
				{
					height += 82;
				}

				float location = m_location.X + locations[i % 4];
				uid.Icon.X = location;
				uid.Icon.Y = height;

				uid.Label.X = location;
				uid.Label.Y = height + 52;				
			}

			float top = m_label.Y;

			UnitInfoDisplay upperLeftUID = (UnitInfoDisplay)m_uids[m_types[0]];
			float left = upperLeftUID.Icon.X;

			UnitInfoDisplay lowerRightUID = (UnitInfoDisplay)m_uids[m_types[m_types.Length - 1]];
			float right = lowerRightUID.Label.Bounds.Right;
			float bottom = lowerRightUID.Label.Bounds.Bottom;

			//RectangleF bounds = new RectangleF(left, top, right - left, bottom - top);
			m_cover = PPath.CreateRectangle(left, top, right - left, bottom - top);
			m_cover.Brush = Brushes.White;
			m_cover.Pen = Pens.White;
		}

		public void UpdatePlayerInfo()
		{
			UpdatePlayerInfo(null);
		}

		public void UpdatePlayerInfo(string territoryName)
		{
			//Hashtable uids = (Hashtable)PlayerUnitDisplay.PlayerUIDs[this.Player];
			Hashtable icons = m_iconManager.GetPlayerIcons(this.Player);

			foreach(UnitType ut in m_types)
			{
				UnitInfoDisplay uid = (UnitInfoDisplay)m_uids[ut];
				Bitmap b = (Bitmap)icons[ut];

				uid.Player = this.Player;
				uid.Icon.Image = b;

				int i = 42;
				int q = i;
			}

			if(territoryName == null)
			{
				m_label.Text = this.Player.Name;
			}
			else
			{
				m_label.Text = string.Format("{0} ({1})", this.Player.Name, territoryName);
			}

			//m_label.X = m_location.X + (-1 * (m_label.Width / 2));	
			LayoutChildren();
		}

		private void CreateUIDs()
		{
			foreach (UnitType ut in m_types)
			{
				//Bitmap b = (Bitmap)icons[ut];
				Bitmap b = new Bitmap(48, 48);

				UnitInfoDisplay uid = new UnitInfoDisplay(m_canvas);

				uid.Composite = new PComposite();
				uid.Icon = new PImage(b);
				uid.Label = new PText("T: 25, S: 15\nA: 18, D: 7");
				uid.Label.TextBrush = Brushes.Black;
				Font f = uid.Label.Font;
				uid.Label.Font = new Font(f.Name, f.SizeInPoints - 4, FontStyle.Bold);

				uid.Composite.AddChild(uid.Icon);
				uid.Composite.AddChild(uid.Label);
				m_canvas.Layer.AddChild(uid.Composite);
				//displayNode.AddChild(uid.Composite);
				//displayNode.AddChild(uid.Icon);
				//displayNode.AddChild(uid.Label);

				uid.Type = ut;
				uid.Composite.Tag = uid;

				uid.PlayerDisplay = this;
				uid.Player = this.Player;
				uid.Composite.MouseUp += new UMD.HCIL.Piccolo.PInputEventHandler(m_combatDisplay.NodeClicked);

				m_uids[ut] = uid;
			}
		}

		public void HideDisplay()
		{
			//Canvas.Layer.AddChild(m_cover);

			m_label.RemoveFromParent();
			m_composite.RemoveFromParent();

			
			foreach(DictionaryEntry de in m_uids)
			{
				UnitInfoDisplay uid = (UnitInfoDisplay)de.Value;

				uid.Composite.RemoveFromParent();
				//uid.Icon.RemoveFromParent();
				//uid.Label.RemoveFromParent();
			}
		}

		public void ShowDisplay()
		{
			//Canvas.Layer.RemoveChild(m_cover);

			//m_canvas.Layer.AddChild(m_label);
			m_canvas.Layer.AddChild(m_composite);

			foreach (DictionaryEntry de in m_uids)
			{
				UnitInfoDisplay uid = (UnitInfoDisplay)de.Value;

				m_canvas.Layer.AddChild(uid.Composite);
				//m_canvas.Layer.AddChild(uid.Icon);
				//m_canvas.Layer.AddChild(uid.Label);
			}
		}

		private void ToggleSelection()
		{
			if (m_selected)
			{
				if (m_selection == null)
				{
					m_selection = PPath.CreateRectangle(m_label.X, m_label.Y, m_label.Width, m_label.Height);
					m_selection.Brush = Brushes.Transparent;

				}

				m_selection.Pen = Pens.Red;

				m_activity = new PColorActivity(1000, 0, 5000, ActivityMode.SourceToDestinationToSource,
																		new PulseTarget(m_selection), Color.Black);

				Canvas.Root.ActivityScheduler.AddActivity(m_activity);
				m_composite.AddChild(m_selection);
			}
			else
			{
				m_canvas.Root.ActivityScheduler.RemoveActivity(m_activity);
				m_activity = null;

				m_composite.RemoveChild(m_selection);
			}
		}


	};
	
	public class UnitInfoDisplay : IconInfo
	{
		private PComposite m_composite;
		private PlayerUnitDisplay m_pud;
		private PPath m_selection;
		private PCanvas m_canvas;
		private PColorActivity m_activity;
		private bool m_selected;

		private int m_numTotal;
		private int m_numCanShoot;
		private int m_numAlive;
		private int m_numDead;

		public bool Selected
		{
			get { return m_selected; }

			set 
			{ 
				m_selected = value;

				ToggleSelection();
			}
		}

		private void ToggleSelection()
		{
			if(m_selected)
			{
				if (m_selection == null)
				{
					m_selection = PPath.CreateRectangle(Icon.X, Icon.Y, Icon.Width, Icon.Height);
					m_selection.Brush = Brushes.Transparent;
					
				}

				m_selection.Pen = Pens.Red;

				m_activity = new PColorActivity(1000, 0, 5000, ActivityMode.SourceToDestinationToSource,
																		new PulseTarget(m_selection), Color.Black);

				m_canvas.Root.ActivityScheduler.AddActivity(m_activity);
				m_composite.AddChild(m_selection);
			}
			else
			{
				m_canvas.Root.ActivityScheduler.RemoveActivity(m_activity);
				m_activity = null;

				m_composite.RemoveChild(m_selection);
			}			
		}

		public PlayerUnitDisplay PlayerDisplay
		{
			get { return m_pud; }
			set { m_pud = value; }
		}

		public PComposite Composite
		{
			get { return m_composite; }
			set { m_composite = value; }
		}


		public UnitInfoDisplay(PCanvas canvas)
		{
			m_canvas = canvas;
		}
	}

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

	public class CombatMessage
	{
		private Player m_attacker;
		private Player m_defender;
		private UnitType m_attackingType;
		private UnitType m_defendingType;
		private int m_roll;
		private bool m_hit;

		PNodeList m_nodes;

		private PCanvas m_canvas;

		private PText m_lblArrow;
		private PText m_lblAttackingType;
		private PText m_lblDefendingType;
		//private PText m_lblRoll;
		private PText m_lblHit;

		private PointF m_location;

		public CombatMessage(PCanvas canvas)
		{
			m_canvas = canvas;

			m_lblArrow = new PText();
			m_lblAttackingType = new PText();
			m_lblDefendingType = new PText();
			//m_lblRoll = new PText();
			m_lblHit = new PText();

			m_lblHit.ConstrainHeightToTextHeight = true;
			m_lblHit.ConstrainWidthToTextWidth = true;

			m_lblAttackingType.ConstrainHeightToTextHeight = true;
			m_lblAttackingType.ConstrainWidthToTextWidth = true;

			m_lblArrow.ConstrainHeightToTextHeight = true;
			m_lblArrow.ConstrainWidthToTextWidth = true;

			m_lblDefendingType.ConstrainHeightToTextHeight = true;
			m_lblDefendingType.ConstrainWidthToTextWidth = true;

			Font f = m_lblArrow.Font;
			Font replacementFont = new Font(f.Name, f.SizeInPoints - 2, FontStyle.Bold);
			m_lblArrow.Font = replacementFont;
			m_lblAttackingType.Font = replacementFont;
			m_lblDefendingType.Font = replacementFont;
			m_lblHit.Font = replacementFont;


			m_nodes = new PNodeList();

			m_nodes.Add(m_lblArrow);
			m_nodes.Add(m_lblAttackingType);
			m_nodes.Add(m_lblDefendingType);
			//m_nodes.Add(m_lblRoll);
			m_nodes.Add(m_lblHit);
		}

		public void SetDetails(Player attacker, Player defender, UnitType attackType, 
							UnitType defendType, int roll, bool hit)
		{
			m_attacker = attacker;
			m_defender = defender;
			m_attackingType = attackType;
			m_defendingType = defendType;
			m_roll = roll;
			m_hit = hit;
		}

		public void DisplayMessage(PointF location)
		{
			
			if(m_hit)
			{
				m_lblHit.Text = string.Format("Hit! ({0})", m_roll);//"Hit!";
				m_lblHit.TextBrush = Brushes.Red;
			}
			else
			{
				m_lblHit.Text = string.Format("Miss ({0})", m_roll);//"Miss";
				m_lblHit.TextBrush = Brushes.Black;
			}

			m_lblArrow.Text = "attacking";

			m_lblAttackingType.Text = Utility.GetDescriptionOf(m_attackingType);//m_attackingType.ToString();
			m_lblAttackingType.TextBrush = new SolidBrush(m_attacker.Color);

			m_lblDefendingType.Text = Utility.GetDescriptionOf(m_defendingType); //m_defendingType.ToString();
			m_lblDefendingType.TextBrush = new SolidBrush(m_defender.Color);

			


			//m_lblRoll.X = location.X;
			//m_lblRoll.Y = location.Y;

			//m_lblHit.X = m_lblRoll.Bounds.Right + 6;
			m_lblHit.X = location.X;
			m_lblHit.Y = location.Y;

			m_lblAttackingType.X = location.X + 58;//m_lblHit.Bounds.Right + 3;
			m_lblAttackingType.Y = location.Y;

			m_lblArrow.X = m_lblAttackingType.Bounds.Right + 1;
			m_lblArrow.Y = location.Y;

			m_lblDefendingType.X = m_lblArrow.Bounds.Right + 1;
			m_lblDefendingType.Y = location.Y;

			m_canvas.Layer.AddChildren(m_nodes);
		}

		public void HideMessage()
		{
			m_canvas.Layer.RemoveChildren(m_nodes);
		}

		public void AnimateMessageTo(PointF newLocation)
		{
			PNode.PNodeBoundsActivity[] pna = new PNode.PNodeBoundsActivity[4];

			RectangleF dest = new RectangleF(m_lblHit.X, newLocation.Y, m_lblHit.Width, m_lblHit.Height);
			pna[0] = new PNode.PNodeBoundsActivity(m_lblHit, dest, 500);

			dest = new RectangleF(m_lblAttackingType.X, newLocation.Y, m_lblAttackingType.Width, m_lblAttackingType.Height);
			pna[1] = new PNode.PNodeBoundsActivity(m_lblAttackingType, dest, 500);

			dest = new RectangleF(m_lblArrow.X, newLocation.Y, m_lblArrow.Width, m_lblArrow.Height);
			pna[2] = new PNode.PNodeBoundsActivity(m_lblArrow, dest, 500);

			dest = new RectangleF(m_lblDefendingType.X, newLocation.Y, m_lblDefendingType.Width, m_lblDefendingType.Height);
			pna[3] = new PNode.PNodeBoundsActivity(m_lblDefendingType, dest, 500);

			for (int i = 0; i < 4; i++)
			{
				m_canvas.Root.ActivityScheduler.AddActivity(pna[i], false);
			}
				
		}



	}

	/*
	class DecoratorGroup : PNode
	{
		readonly static int INDENT = 10;

		RectangleF cachedChildBounds = RectangleF.Empty;
		RectangleF comparisonBounds = RectangleF.Empty;

		private Pen m_pen;

		public Pen Pen
		{
			get { return m_pen; }
			set { m_pen = value; }
		}

		protected override void Paint(PPaintContext paintContext)
		{
			if (Brush != null)
			{
				Graphics g = paintContext.Graphics;

				RectangleF bounds = UnionOfChildrenBounds;
				bounds = new RectangleF(bounds.X - INDENT, bounds.Y - INDENT, bounds.Width + 2 * INDENT, bounds.Height + 2 * INDENT);
				g.FillRectangle(Brush, bounds);
				//g.DrawRectangle(m_pen, bounds.X, bounds.Y, bounds.Width, bounds.Height);
			}
		}

		public override RectangleF ComputeFullBounds()
		{
			RectangleF result = UnionOfChildrenBounds;
			cachedChildBounds = result;
			result = new RectangleF(result.X - INDENT, result.Y - INDENT, result.Width + 2 * INDENT, result.Height + 2 * INDENT);
			result = LocalToParent(result);
			return result;
		}

		protected override bool ValidateFullBounds()
		{
			comparisonBounds = UnionOfChildrenBounds;

			if (!cachedChildBounds.Equals(comparisonBounds))
			{
				PaintInvalid = true;
			}
			return base.ValidateFullBounds();
		}
	}
	*/
}


