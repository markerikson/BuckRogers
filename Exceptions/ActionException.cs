using System;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for ActionException.
	/// </summary>
	public class ActionException : Exception
	{
		public ActionException(string message)
			: base(message)
		{
		}
	}
}
