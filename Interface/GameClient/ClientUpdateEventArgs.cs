using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BuckRogers;
using BuckRogers.Networking;

namespace BuckRogers.Networking
{
	public class ClientUpdateEventArgs : EventArgs
	{
		private GameMessage m_networkMessage;		
		private string m_messageText;		
		private List<Player> m_players;

		public List<Player> Players
		{
			get { return m_players; }
			set { m_players = value; }
		}

		public GameMessage MessageType
		{
			get { return m_networkMessage; }
			set { m_networkMessage = value; }
		}

		public string MessageText
		{
			get { return m_messageText; }
			set { m_messageText = value; }
		}

		public ClientUpdateEventArgs()
		{
			m_players = new List<Player>();
		}

	}
}