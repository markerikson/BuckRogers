using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BuckRogers.Interface
{
	public partial class CombatForm2D : Form
	{
		private CombatControl m_combat;

		public CombatControl CombatDisplay
		{
			get { return m_combat; }
			set { m_combat = value; }
		}

		private GameController m_controller;
		private BattleController m_battleController;

		public CombatForm2D(GameController gc, BattleController bc, IconManager im)
		{
			InitializeComponent();

			m_controller = gc;
			m_battleController = bc;

			m_combat = new CombatControl(gc, bc, im);

			m_combat.Anchor =
				AnchorStyles.Bottom |
				AnchorStyles.Top |
				AnchorStyles.Left |
				AnchorStyles.Right;

			this.Controls.Add(m_combat);

			this.SetClientSizeCore(800, 600);

			m_combat.Bounds = ClientRectangle;
		}

		protected override void  OnShown(EventArgs e)
		{
 			 base.OnShown(e);

			 CombatDisplay.BeginCombat();
			 //MessageBox.Show("CombatForm2D shown");
		}

		
	}
}