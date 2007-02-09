using System;
using System.Drawing;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for TerritoryClickEventArgs.
	/// </summary>
	public class TerritoryEventArgs : System.EventArgs
	{
		private string m_territoryName;
		private Player m_owner;
		private System.Windows.Forms.MouseButtons m_button;
		private PointF m_point;
		private object m_tag;

		

		

		public TerritoryEventArgs()
		{

		}

		public TerritoryEventArgs(string s)
		{
			m_territoryName = s;
		}

		public string Name
		{
			get { return this.m_territoryName; }
			set { this.m_territoryName = value; }
		}

		public BuckRogers.Player Owner
		{
			get { return this.m_owner; }
			set { this.m_owner = value; }
		}

		public System.Windows.Forms.MouseButtons Button
		{
			get { return this.m_button; }
			set { this.m_button = value; }
		}

		public PointF PointClicked
		{
			get { return m_point; }
			set { m_point = value; }
		}

		public object Tag
		{
			get { return m_tag; }
			set { m_tag = value; }
		}

		
	}
}
