using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using BuckRogers;

namespace BuckRogers.Interface
{
	public partial class UnitIconLegendForm : Form
	{
		private IconManager m_iconManager;
		private GameController m_gc;

		public UnitIconLegendForm(GameController gc, IconManager iconManager)
		{
			m_iconManager = iconManager;
			m_gc = gc;

			InitializeComponent();

			int LVM_SETICONSPACING = 4149;
			int width = 70;
			int height = 80;
			int iconSpacing = Utility.MakeLong(width, height);

			SendMessage(listView1.Handle, LVM_SETICONSPACING, 0, iconSpacing);

			listView1.BackColor = Color.White;
		}

		public void Initialize()
		{
			Hashtable icons = m_iconManager.GetPlayerIcons(m_gc.Players[0]);

			ImageList il = new ImageList();
			il.ImageSize = new Size(48, 48);

			foreach (UnitType ut in Enum.GetValues(typeof(UnitType)))
			{
				if (!icons.ContainsKey(ut))
				{
					continue;
				}

				Bitmap b = (Bitmap)icons[ut];
				il.Images.Add(ut.ToString(), b);
			}

			listView1.LargeImageList = il;
		}

		public void AddItems()
		{
			Hashtable icons = m_iconManager.GetPlayerIcons(m_gc.Players[0]);

			ImageList il = new ImageList();
			il.ImageSize = new Size(48, 48);

			foreach (UnitType ut in Enum.GetValues(typeof(UnitType)))
			{
				if (!icons.ContainsKey(ut))
				{
					continue;
				}

				ListViewItem lvi = new ListViewItem();

				lvi.Text = ut.ToString();
				lvi.ImageKey = ut.ToString();
				lvi.Tag = ut;

				listView1.Items.Add(lvi);
			}
		}

		[DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int SendMessage(IntPtr hwnd, int msg, int wParam, int lParam);

		private void OK_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem lvi in listView1.Items)
			{
				Image image = listView1.LargeImageList.Images[lvi.ImageKey];

				int i = 42;
				int q = i;
			}

			this.DialogResult = DialogResult.OK;
		}
	}
}