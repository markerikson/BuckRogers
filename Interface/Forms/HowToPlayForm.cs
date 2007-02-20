using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace BuckRogers.Interface
{
	public partial class HowToPlayForm : Form
	{
		public HowToPlayForm()
		{
			InitializeComponent();

			this.Icon = InterfaceUtility.GetApplicationIcon();

			textBox1.BackColor = Color.White;

			string textFile = "BuckRogers.Interface.Other.Resources.HowToPlay.txt";

			Assembly a = Assembly.GetExecutingAssembly();
			Stream stream =
				a.GetManifestResourceStream(textFile);
			StreamReader sr = new StreamReader(stream);

			textBox1.Text = sr.ReadToEnd();

			textBox1.SelectionStart = 0;
			textBox1.SelectionLength = 0;
			textBox1.Focus();
		}

		private void m_btnOK_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;

			this.Hide();
		}
	}
}