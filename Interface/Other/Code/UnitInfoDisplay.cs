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
	public class UnitInfoDisplay : IconInfo
	{
		private PComposite m_composite;
		private PlayerUnitDisplay m_pud;
		private PPath m_selection;
		private PPath m_iconCover;
		private PCanvas m_canvas;
		private PColorActivity m_activity;
		private bool m_selected;
		private bool m_dimmed;
		private bool m_updatingDeath;
		private bool m_resetting;

		private int m_numTotal;
		private int m_numCanShoot;
		private int m_numAlive;
		private int m_numDead;
		private int m_numCasualties;

		public string Tag;

		public int NumCasualties
		{
			get { return m_numCasualties; }
			set { m_numCasualties = value; }
		}

		public int NumDead
		{
			get { return m_numDead; }
			set { m_numDead = value; }
		}

		public int NumTotal
		{
			get { return m_numTotal; }
			set { m_numTotal = value; }
		}

		public int NumAlive
		{
			get { return m_numAlive; }
			set { m_numAlive = value; }
		}

		public int NumCanShoot
		{
			get { return m_numCanShoot; }
			set { m_numCanShoot = value; }
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

			m_iconCover = PPath.CreateRectangle(0, 0, 48, 48);

			Color fade = Color.FromArgb(100, 0, 0, 0);
			m_iconCover.Pen = new Pen(fade);
			m_iconCover.Brush = new SolidBrush(fade);

			m_iconCover.MouseUp += new PInputEventHandler(m_iconCover_MouseUp);
		}

		void m_iconCover_MouseUp(object sender, PInputEventArgs e)
		{
			string message = string.Format("Cover clicked:\nPlayer: {0}, Type: {1}\nHashcode: {2}",
											this.Player.Name, this.Type.ToString(), this.PlayerDisplay.GetHashCode());
			MessageBox.Show(message);
		}


		public bool Selected
		{
			get { return m_selected; }

			set
			{
				m_selected = value;

				if(m_numCanShoot == 0)
				{
					m_selected = false;
				}

				ToggleSelection();
			}
		}

		public void SetDeathCount(int total)
		{
			m_numDead = total;

			m_updatingDeath = true;

			UpdateUnitStatistics();

			m_updatingDeath = false;
		}

		public void SetUnitCount(int total)
		{
			m_numTotal = total;
			m_numAlive = total;
			m_numCanShoot = total;
			m_numDead = 0;

			ResetCasualties();

			UpdateUnitStatistics();
		}

		public void SetUnitCount(int total, int canShoot, int alive, int dead)
		{
			m_numTotal = total;
			m_numCanShoot = canShoot;
			m_numAlive = alive;
			m_numDead = dead;

			UpdateUnitStatistics();
		}

		public bool Shoot()
		{
			bool firedShot = false;

			if (m_numCanShoot > 0)
			{
				m_numCanShoot--;
				firedShot = true;
			}

			//UpdateUnitStatistics();

			return firedShot;
		}

		public bool KillOneUnit()
		{
			bool killedUnit = false;

			if (m_numAlive > 0)
			{
				m_numAlive--;

				m_numDead++;
				m_numCasualties++;

				killedUnit = true;
			}

			//UpdateUnitStatistics();

			return killedUnit;
		}

		public void ResetCasualties()
		{
			m_numCasualties = 0;
		}

		public void ResetUnitCounts()
		{
			m_numAlive = 0;
			m_numDead = 0;
			m_numCanShoot = 0;
			m_numTotal = 0;

			ResetCasualties();

			m_resetting = true;

			UpdateUnitStatistics();

			m_resetting = false;
		}

		private void ToggleSelection()
		{
			if (m_selected)
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
				if(m_activity != null)
				{
					m_activity.Terminate(TerminationBehavior.TerminateWithoutFinishing);
					m_canvas.Root.ActivityScheduler.RemoveActivity(m_activity);
					m_activity = null;
				}
				

				if(m_selection != null)
				{
					//m_composite.RemoveChild(m_selection);
					m_selection.RemoveFromParent();
				}
				
			}
		}

		public void UpdateUnitStatistics()
		{
			string stats = string.Format("A: {0}, S: {1}\nD: {2}, T: {3}", m_numAlive, m_numCanShoot, m_numDead, m_numTotal);
			this.Label.Text = stats;

			if(!m_resetting && (m_numTotal == 0 || (m_numAlive == 0 && m_updatingDeath)))
			{				
				DimUnitDisplay();
			}
			else
			{
				UnDimUnitDisplay();
			}
		}

		public void DimUnitDisplay()
		{
			if(m_dimmed)
			{
				return;
			}

			SolidBrush brush = (SolidBrush)Label.TextBrush;
			Color c = brush.Color;
			Color dimColor = Color.FromArgb(100, c.R, c.G, c.B);
			SolidBrush dimBrush = new SolidBrush(dimColor);
			Label.TextBrush = dimBrush;

			UpdateCoverLocation();

			//m_composite.AddChild(m_iconCover);
			m_canvas.Layer.AddChild(m_iconCover);
			

			m_dimmed = true;
		}

		public void UpdateCoverLocation()
		{
			m_iconCover.X = Icon.X;
			m_iconCover.Y = Icon.Y;

			m_iconCover.MoveToFront();
		}

		public void UnDimUnitDisplay()
		{
			if(!m_dimmed)
			{
				return;
			}

			//SolidBrush brush = (SolidBrush)Label.TextBrush;
			//Color c = brush.Color;
			//Color dimColor = Color.FromArgb(100, c.R, c.G, c.B);
			//SolidBrush dimBrush = Brushes.B
			Label.TextBrush = Brushes.Black;

			m_iconCover.RemoveFromParent();

			m_dimmed = false;
		}


		public void HideDisplay()
		{
			Composite.RemoveFromParent();
			m_iconCover.RemoveFromParent();
		}
	}
}
