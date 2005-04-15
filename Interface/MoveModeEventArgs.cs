using System;

namespace BuckRogers.Interface
{

	public enum MoveMode
	{
		StartMove,
		StartTransport,
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
			//
			// TODO: Add constructor logic here
			//
			
		}

		public MoveMode MoveMode
		{
			get { return this.m_moveMode; }
			set { this.m_moveMode = value; }
		}
	}
}
