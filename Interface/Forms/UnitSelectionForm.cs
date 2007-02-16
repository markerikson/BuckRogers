using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BuckRogers.Interface
{
	public partial class UnitSelectionForm : Form
	{
		private int m_maxUnits = 15;

		public int TotalFighters
		{
			get
			{
				int numFighters = (int)m_nudFighters.Value;
				return numFighters;
			}
		}

		public int TotalTransports
		{
			get
			{
				int numTransports = (int)m_nudTransports.Value;
				return numTransports;
			}
		}

		public int TotalTroopers
		{
			get
			{
				int numTroopers = (int)m_nudTroopers.Value;
				return numTroopers;
			}
		}

		public int TotalGennies
		{
			get
			{
				int numGennies = (int)m_nudGennies.Value;
				return numGennies;
			}
		}

		public int TotalUnitsLeft
		{
			get
			{
				int numUnitsLeft = m_maxUnits - TotalFighters - TotalTransports - TotalTroopers - TotalGennies;
				return numUnitsLeft;
			}
		}

		public UnitSelectionForm()
		{
			Initialize();
		}

		public UnitSelectionForm(int maxUnits)
		{
			m_maxUnits = maxUnits;

			Initialize();
		}

		private void Initialize()
		{
			InitializeComponent();

			UpdateTotals();
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			m_labUnitsLeft.Text = TotalUnitsLeft.ToString();
		}

		private void ItemValueChanged(object sender, EventArgs e)
		{
			UpdateTotals();
		}

		private void UpdateTotals()
		{
			m_nudFighters.Maximum = TotalUnitsLeft + TotalFighters;
			m_nudGennies.Maximum = TotalUnitsLeft + TotalGennies;
			m_nudTroopers.Maximum = TotalUnitsLeft + TotalTroopers;
			m_nudTransports.Maximum = TotalUnitsLeft + TotalTransports;

			m_labUnitsLeft.Text = TotalUnitsLeft.ToString();
		}

		private void m_btnOK_Click(object sender, EventArgs e)
		{
			if(TotalUnitsLeft > 0)
			{
				MessageBox.Show("You have not selected all your units!", "Selection Unfinished",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else
			{
				this.DialogResult = DialogResult.OK;
			}
		}

		private void m_btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		
	}
}