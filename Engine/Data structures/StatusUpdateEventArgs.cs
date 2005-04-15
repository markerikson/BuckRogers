using System;

namespace BuckRogers
{
	public enum StatusInfo
	{
		NextPlayer,
		NextPhase,
	};
	/// <summary>
	/// Summary description for StatusUpdateEventArgs.
	/// </summary>
	public class StatusUpdateEventArgs : System.EventArgs
	{
		private Player m_player;
		private StatusInfo m_statusInfo;

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
	}
}
