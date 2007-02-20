using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BuckRogers.Interface
{
	public partial class NumberEntryForm : Form
	{
		public NumberEntryForm()
		{
			InitializeComponent();

			this.Icon = InterfaceUtility.GetApplicationIcon();

			//m_labText.Height = 40;
		}

		public int NumberValue
		{
			get
			{
				return (int)m_nudNumber.Value;
			}
		}

		public void Initialize(string text, int min, int max, int value)
		{
			m_labText.Text = text;
			m_nudNumber.Minimum = min;
			m_nudNumber.Maximum = max;
			m_nudNumber.Value = value;
		}

		private void m_btnOK_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		private void m_btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}
	}
}