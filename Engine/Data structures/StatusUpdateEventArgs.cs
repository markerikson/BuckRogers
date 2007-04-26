using System;

namespace BuckRogers
{
	public enum StatusInfo
	{
		NextPlayer,
		NextPhase,
		FactoryConquered,
		SabotageResult,
		LeaderKilled,
		TransportLanded,
		PlayerKilled,
		GameOver,
	};
	/// <summary>
	/// Summary description for StatusUpdateEventArgs.
	/// </summary>
	public class StatusUpdateEventArgs : System.EventArgs
	{
		private Player m_player;
		private Territory m_territory;
		private StatusInfo m_statusInfo;
		private bool m_result;
		private bool m_isLocal;		

		public StatusUpdateEventArgs()
		{
			
		}

		public BuckRogers.Player Player
		{
			get { return this.m_player; }
			set { this.m_player = value; }
		}

		public BuckRogers.StatusInfo StatusInfo
		{
			get { return this.m_statusInfo; }
			set { this.m_statusInfo = value; }
		}

		public BuckRogers.Territory Territory
		{
			get { return this.m_territory; }
			set { this.m_territory = value; }
		}

		public bool Result
		{
			get { return this.m_result; }
			set { this.m_result = value; }
		}

		public bool IsLocal
		{
			get { return m_isLocal; }
			set { m_isLocal = value; }
		}
	}
}
