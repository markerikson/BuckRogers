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
	public partial class AboutForm : Form
	{
		public AboutForm()
		{
			InitializeComponent();

			this.Icon = InterfaceUtility.GetApplicationIcon();

			Assembly asm = Assembly.GetExecutingAssembly();
			string isquaredLogo = "BuckRogers.Interface.Other.Graphics.ISquared logo.png";
			Stream stream = asm.GetManifestResourceStream(isquaredLogo);
			Bitmap bmp = new Bitmap(stream);
			pictureBox1.Image = bmp;



			string buckLogo = "BuckRogers.Interface.Other.Graphics.Buck Rogers Logo (small).png";
			Stream stream2 = asm.GetManifestResourceStream(buckLogo);
			Bitmap bmp2 = new Bitmap(stream2);
			pictureBox2.Image = bmp2;

			AssemblyName name = asm.GetName();
			Version version = name.Version;

			DateTime buildTime = Utility.RetrieveLinkerTimestamp();

			string buildDateString = buildTime.ToString("yyyy-MM-dd");
			string buildTimeString = buildTime.ToString("h:mm tt");

			string versionLabelText = string.Format("Version {0}\nBuild {1}\nBuilt on {2} at {3}",
													BuckRogersForm.VersionString, version.ToString(), 
													buildDateString, buildTimeString);

			m_lblVersion.Text = versionLabelText;
		}

		private void m_btnOK_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string address = linkLabel1.Text;

			System.Diagnostics.Process.Start(address);
		}
	}
}