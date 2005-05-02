using System;

namespace BuckRogers.Interface
{

	public enum MoveMode
	{
		StartMove,
		StartTransport,
		StartPlacement,
		Finished,
	}
	/// <summary>
	/// Summary description for MoveModeEventArgs.
	/// </summary>
	public class MoveModeEventArgs : System.EventArgs
	{
		private MoveMode m_moveMode;

		public MoveModeEventArgs()
		{
			
		}

		public MoveMode MoveMode
		{
			get { return this.m_moveMode; }
			set { this.m_moveMode = value; }
		}
	}
}
