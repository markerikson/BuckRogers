using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace BuckRogers.Interface
{
	/// <summary>
	/// Summary description for TerritoryPanel.
	/// </summary>
	public class TerritoryPanel : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.ListView m_lvUnits;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label m_labTerritoryName;
		private System.Windows.Forms.Label m_labTerritoryOwner;
		private System.Windows.Forms.Label label3;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TerritoryPanel()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.m_lvUnits = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.label1 = new System.Windows.Forms.Label();
			this.m_labTerritoryName = new System.Windows.Forms.Label();
			this.m_labTerritoryOwner = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_lvUnits
			// 
			this.m_lvUnits.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.columnHeader1,
																						this.columnHeader2,
																						this.columnHeader3});
			this.m_lvUnits.Location = new System.Drawing.Point(0, 64);
			this.m_lvUnits.Name = "m_lvUnits";
			this.m_lvUnits.Size = new System.Drawing.Size(232, 240);
			this.m_lvUnits.TabIndex = 0;
			this.m_lvUnits.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Player";
			this.columnHeader1.Width = 80;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Type";
			this.columnHeader2.Width = 80;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Count";
			this.columnHeader3.Width = 52;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(4, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 20);
			this.label1.TabIndex = 1;
			this.label1.Text = "Name:";
			// 
			// m_labTerritoryName
			// 
			this.m_labTerritoryName.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_labTerritoryName.Location = new System.Drawing.Point(68, 4);
			this.m_labTerritoryName.Name = "m_labTerritoryName";
			this.m_labTerritoryName.Size = new System.Drawing.Size(168, 36);
			this.m_labTerritoryName.TabIndex = 2;
			this.m_labTerritoryName.Text = "Australian Development Facility";
			// 
			// m_labTerritoryOwner
			// 
			this.m_labTerritoryOwner.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_labTerritoryOwner.Location = new System.Drawing.Point(68, 44);
			this.m_labTerritoryOwner.Name = "m_labTerritoryOwner";
			this.m_labTerritoryOwner.Size = new System.Drawing.Size(168, 20);
			this.m_labTerritoryOwner.TabIndex = 4;
			this.m_labTerritoryOwner.Text = "Mark";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(4, 44);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 20);
			this.label3.TabIndex = 3;
			this.label3.Text = "Owner:";
			// 
			// TerritoryPanel
			// 
			this.Controls.Add(this.m_labTerritoryOwner);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.m_labTerritoryName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.m_lvUnits);
			this.Name = "TerritoryPanel";
			this.Size = new System.Drawing.Size(240, 400);
			this.ResumeLayout(false);

		}
		#endregion

		public void DisplayUnits(Territory t)
		{
			m_lvUnits.Items.Clear();

			m_labTerritoryName.Text = t.Name;
			m_labTerritoryOwner.Text = t.Owner.Name;

			foreach(Player p in t.Units.GetPlayersWithUnits())
			{
				UnitCollection uc = t.Units.GetUnits(p);

				foreach(DictionaryEntry de in uc.GetUnitTypeCount())
				{
					ListViewItem lvi = new ListViewItem();
					UnitType ut = (UnitType)de.Key;
					int numUnits = (int)de.Value;

					lvi.Text = p.Name;
					lvi.SubItems.Add(ut.ToString());
					lvi.SubItems.Add(numUnits.ToString());

					m_lvUnits.Items.Add(lvi);
				}
			}
		}
	}
}
