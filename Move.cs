using System;
using System.Collections;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for Move.
	/// </summary>
	public class MoveAction : Action
	{
		
	
		public ArrayList Territories
		{
			get { return this.m_territories; }
			set { this.m_territories = value; }
		}

		
	
		private ArrayList m_territories;
		

		public MoveAction()
		{
			//
			// TODO: Add constructor logic here
			//
			m_territories = new ArrayList();
			
		}

		
	}
}
