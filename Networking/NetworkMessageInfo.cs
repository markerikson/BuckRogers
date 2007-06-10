using System;
using System.Collections.Generic;
using System.Text;

using BuckRogers;

namespace BuckRogers.Networking
{
	public class NetworkMessageInfo : IComparable
	{
		private int m_id;
		private bool m_acknowledged;
		private DateTime m_timeSent;
		private GameMessage m_messageType;
		private string m_messageText;

		public int ID
		{
			get { return m_id; }
			set { m_id = value; }
		}
		
		public string MessageText
		{
			get { return m_messageText; }
			set { m_messageText = value; }
		}

		public GameMessage MessageType
		{
			get { return m_messageType; }
			set { m_messageType = value; }
		}

		public DateTime TimeSent
		{
			get { return m_timeSent; }
			set { m_timeSent = value; }
		}

		public bool Acknowledged
		{
			get { return m_acknowledged; }
			set { m_acknowledged = value; }
		}



		#region IComparable Members

		public int CompareTo(object obj)
		{
			NetworkMessageInfo nmi = (NetworkMessageInfo)obj;

			return this.TimeSent.CompareTo(nmi.TimeSent);
		}

		#endregion
	}
}
