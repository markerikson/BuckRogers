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

		//private PointF m_location;

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

			if (m_hit)
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


			m_lblHit.X = location.X;
			m_lblHit.Y = location.Y;

			m_lblAttackingType.X = location.X + 58;
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

		public void DimMessage()
		{
			foreach (PText label in m_nodes)
			{
				SolidBrush brush = (SolidBrush)label.TextBrush;
				Color c = brush.Color;
				Color dimColor = Color.FromArgb(50, c.R, c.G, c.B);
				SolidBrush dimBrush = new SolidBrush(dimColor);
				label.TextBrush = dimBrush;
			}
		}

		public void UnDimMessage()
		{
			foreach (PText label in m_nodes)
			{
				SolidBrush brush = (SolidBrush)label.TextBrush;
				Color c = brush.Color;
				Color normalColor = Color.FromArgb(255, c.R, c.G, c.B);
				SolidBrush normalBrush = new SolidBrush(normalColor);
				label.TextBrush = normalBrush;
			}
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
}
