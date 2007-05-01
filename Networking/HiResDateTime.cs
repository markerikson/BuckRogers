#region using directives
using System;
using System.Runtime.InteropServices;
#endregion

namespace ISquared.Debugging
{
	#region comments
	/// <summary>
	/// A class providing DateTime objects with millisecond resolution, for
	/// Compact Framework applications.
	/// </summary>
	/// <remarks>
	/// Uses Win32 <see href="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/sysinfo/base/gettickcount.asp">GetTickCount</see> to provide Compact Framework DateTime objects with milliseconds.
	/// The need arises from certain Compact Framework (1.1) limitations. The Win32 APIs 
	/// <see href="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/sysinfo/base/getsystemtime.asp">GetSystemTime</see> and
	/// <see href="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/sysinfo/base/getlocaltime.asp">GetLocalTime</see> always return
	/// 0 milliseconds, as does DateTime.Now. <see href="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/sysinfo/base/getsystemtimeadjustment.asp">GetSystemTimeAdjustment</see>
	/// is not implemented in the Compact Framework.
	/// </remarks>
	#endregion
	public class HiResDateTime
	{
		[DllImport("kernel32.dll")]
		private static extern int GetTickCount();

		private static DateTime referenceDateTime = DateTime.Now;
		private static int      referenceMillis   = MillisSinceBoot;

		/// <summary>
		/// This class is "static". Prevent instantiation.
		/// </summary>
		private HiResDateTime()
		{
		}

		/// <summary>
		/// Current DateTime, with milliseconds. 
		/// </summary>
		/// <remarks>
		/// The milliseconds part does not necessarily reflect any absolute time 
		/// understood by the Pocket PC. However, the TimeSpan between two DateTimes
		/// will be a reasonably precise reflection of the elapsed time in milliseconds.
		/// </remarks>
		public static DateTime Now
		{
			get
			{
				return referenceDateTime.AddMilliseconds(MillisSinceBoot - referenceMillis);
			}
		}

		private static int MillisSinceBoot
		{
			get
			{
				return GetTickCount();
			}
		}
	}
}
