using System;

namespace BuckRogers.Interface
{
	/// <summary>
	/// Summary description for TransportEventArgs.
	/// </summary>
	public class TransportInfoEventArgs : System.EventArgs
	{
		private string m_message;

		public TransportInfoEventArgs()
		{
		}

		public string Message
		{
			get { return this.m_message; }
			set { this.m_message = value; }
		}
	}
}
