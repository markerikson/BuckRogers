using System;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for Transport.
	/// </summary>
	public class Transport : Unit
	{
		private UnitCollection m_transportees;


		public Transport(Player owner)
			: base(owner, UnitType.Transport)
		{
			//
			// TODO: Add constructor logic here
			//
			Transportees = new UnitCollection();
		}

		public BuckRogers.UnitCollection Transportees
		{
			get { return this.m_transportees; }
			set { this.m_transportees = value; }
		}
	}
}
