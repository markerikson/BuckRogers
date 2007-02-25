using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BuckRogers.Interface
{
	public partial class YesNoForm : Form
	{
		public string LabelText
		{
			set
			{
				label1.Text = value;
			}
		}
		public YesNoForm()
		{
			InitializeComponent();

			pictureBox1.Image = SystemIcons.Question.ToBitmap();
		}

		private void m_btnYes_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Yes;
			this.Hide();
		}

		private void m_btnNo_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.No;
			this.Hide();
		}
	}
}