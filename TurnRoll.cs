using System;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for TurnRoll.
	/// </summary>
	public class TurnRoll : IComparable
	{
		private int m_roll;
		private Player m_player;

		public BuckRogers.Player Player
		{
			get { return this.m_player; }
			set { this.m_player = value; }
		}

		public int Roll
		{
			get { return this.m_roll; }
			set { this.m_roll = value; }
		}


		public TurnRoll()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#region IComparable Members

		public int CompareTo(object obj)
		{
			if(obj is TurnRoll)
			{
				TurnRoll roll = (TurnRoll) obj;

				return m_roll.CompareTo(roll.Roll);

			}
			return 0;
			
		}

		#endregion

		
	}
}
