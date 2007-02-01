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

namespace BuckRogers.Interface
{
	public class PlayerUnitDisplay
	{
		private PComposite m_composite;
		private PointF m_location;
		private Player m_player;
		//private PPath m_cover;
		private PPath m_selection;
		private bool m_selected;
		private PCanvas m_canvas;
		PColorActivity m_activity;
		private Territory m_territory;
		private CombatControl m_combatDisplay;
		private bool m_displayed;

		public Territory Territory
		{
			get { return m_territory; }
			set { m_territory = value; }
		}

		private PText m_label;
		private PText m_secondaryLabel;

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
			m_secondaryLabel = new PText();
			m_combatDisplay = combatDisplay;
			m_canvas = canvas;
			m_composite = new PComposite();
			m_composite.Tag = this;
			m_uids = new Hashtable();

			Font f = m_secondaryLabel.Font;
			m_secondaryLabel.Font = new Font(f.FontFamily, f.Size - 2, FontStyle.Bold);
			CreateUIDs();

			// HACK We know this PUD will have HideDisplay() called early on,
			//		and we need the call to complete the first time
			//m_displayed = true;
		}

		/*
		public void UpdateUnitInfo()
		{

		}
		*/

		public void UpdateUnitCount(UnitType type, int total)//, int canShoot, int alive, int dead)
		{
			UnitInfoDisplay uid = (UnitInfoDisplay)m_uids[type];

			if(uid == null)
			{
				return;
			}

			uid.SetUnitCount(total);
		}

		public void UpdateUnitCount(UnitType type, int total, int canShoot, int alive, int dead)
		{
			UnitInfoDisplay uid = (UnitInfoDisplay)m_uids[type];

			if (uid == null)
			{
				return;
			}

			uid.SetUnitCount(total, canShoot, alive, dead);
		}



		public void UpdateDeathCount(UnitType type, int total)
		{
			UnitInfoDisplay uid = (UnitInfoDisplay)m_uids[type];

			if (uid == null)
			{
				return;
			}

			uid.SetDeathCount(total);
		}



		public int[] GetUnitCount(UnitType type)
		{
			return null;
		}

		public void LayoutChildren()
		{
			bool showSecondaryLabel = (m_secondaryLabel.Text != null);
			float headingBottom = 0;

			if (showSecondaryLabel)
			{
				m_location.Y -= 20;
			}

			m_label.X = m_location.X + (-1 * (m_label.Width / 2));
			m_label.Y = m_location.Y;
			headingBottom = m_label.Y + m_label.Height + 1;

			if (showSecondaryLabel)
			{
				m_secondaryLabel.X = m_location.X + (-1 * (m_secondaryLabel.Width / 2));
				m_secondaryLabel.Y = m_label.Y + m_label.Height;
				headingBottom = m_secondaryLabel.Y + m_secondaryLabel.Height;
			}

			float[] locations = { -128, -64, 0, 64 };

			for (int i = 0; i < m_types.Length; i++)
			{
				UnitType ut = m_types[i];
				UnitInfoDisplay uid = (UnitInfoDisplay)m_uids[ut];

				float height = headingBottom;

				if (i >= (m_types.Length / 2))
				{
					height += 82;
				}

				float location = m_location.X + locations[i % 4];
				uid.Icon.X = location;
				uid.Icon.Y = height;

				uid.UpdateCoverLocation();

				uid.Label.X = location;
				uid.Label.Y = height + 52;
			}

			if (showSecondaryLabel)
			{
				m_composite.AddChild(m_secondaryLabel);
			}
			else
			{
				m_secondaryLabel.RemoveFromParent();
			}
		}

		public void UpdatePlayerInfo()
		{
			UpdatePlayerInfo(null);
		}

		public void UpdatePlayerInfo(string territoryName)
		{
			Hashtable icons = m_iconManager.GetPlayerIcons(this.Player);

			foreach (UnitType ut in m_types)
			{
				UnitInfoDisplay uid = (UnitInfoDisplay)m_uids[ut];
				Bitmap b = (Bitmap)icons[ut];

				uid.Player = this.Player;
				uid.Icon.Image = b;

				int i = 42;
				int q = i;
			}

			m_label.Text = this.Player.Name;
			m_label.ConstrainHeightToTextHeight = true;
			m_label.ConstrainWidthToTextWidth = true;

			if (territoryName == null)
			{
				m_secondaryLabel.Text = null;
			}
			else
			{
				m_secondaryLabel.Text = territoryName;
				m_secondaryLabel.TextAlignment = StringAlignment.Center;
			}

			foreach (DictionaryEntry de in m_uids)
			{
				UnitInfoDisplay uid = (UnitInfoDisplay)de.Value;

				uid.Player = this.Player;
			}

			LayoutChildren();
		}

		private void CreateUIDs()
		{
			foreach (UnitType ut in m_types)
			{
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
				//m_canvas.Layer.AddChild(uid.Composite);

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
			
			if(!m_displayed)
			{
				return;
			}
			

			m_displayed = false;

			m_label.RemoveFromParent();
			m_secondaryLabel.RemoveFromParent();
			m_composite.RemoveFromParent();

			foreach (DictionaryEntry de in m_uids)
			{
				UnitInfoDisplay uid = (UnitInfoDisplay)de.Value;

				//uid.Composite.RemoveFromParent();
				uid.HideDisplay();
			}
		}

		public void ShowDisplay()
		{
			if(m_displayed)
			{
				return;
			}

			m_displayed = true;

			m_canvas.Layer.AddChild(m_composite);

			if(m_secondaryLabel.Text != null)
			{
				m_canvas.Layer.AddChild(m_secondaryLabel);
			}

			m_canvas.Layer.AddChild(m_label);

			foreach (DictionaryEntry de in m_uids)
			{
				UnitInfoDisplay uid = (UnitInfoDisplay)de.Value;

				m_canvas.Layer.AddChild(uid.Composite);
				uid.UpdateUnitStatistics();
			}
		}

		public void DimDisplay()
		{
			foreach (DictionaryEntry de in m_uids)
			{
				UnitInfoDisplay uid = (UnitInfoDisplay)de.Value;

				uid.DimUnitDisplay();
			}

			SolidBrush brush = (SolidBrush)m_label.TextBrush;
			Color c = brush.Color;
			Color dimColor = Color.FromArgb(100, c.R, c.G, c.B);
			SolidBrush dimBrush = new SolidBrush(dimColor);

			m_label.TextBrush = dimBrush;
			m_secondaryLabel.TextBrush = dimBrush;
		}

		public void UnDimDisplay()
		{
			foreach (DictionaryEntry de in m_uids)
			{
				UnitInfoDisplay uid = (UnitInfoDisplay)de.Value;

				//uid.UnDimUnitDisplay();
				uid.UpdateUnitStatistics();
			}

			m_label.TextBrush = Brushes.Black;
			m_secondaryLabel.TextBrush = Brushes.Black;

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

		public void ResetUnitCounts()
		{
			foreach (DictionaryEntry de in m_uids)
			{
				UnitInfoDisplay uid = (UnitInfoDisplay)de.Value;

				uid.ResetUnitCounts();
			}
		}

		public void ClearUnitSelection()
		{
			foreach (DictionaryEntry de in m_uids)
			{
				UnitInfoDisplay uid = (UnitInfoDisplay)de.Value;

				uid.Selected = false;
			}
		}

		public void DimDeadUnits()
		{
			foreach (DictionaryEntry de in m_uids)
			{
				UnitInfoDisplay uid = (UnitInfoDisplay)de.Value;

				if(uid.NumAlive == 0)
				{
					uid.DimUnitDisplay();
				}
			}
		}


	};
}
