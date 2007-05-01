#region using directives
using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

using BuckRogers;
#endregion

namespace ISquared.Debugging
{
	#region comments
	/// <summary>
	/// A lightweight logging class for Compact Framework applications.
	/// </summary>
	/// <remarks>
	/// <para>
	/// You can simply call the static member <see cref="Log">TextTraceListener.Log</see>. There is no requirement for prior intitialization
	/// or object instantiation.
	/// </para>
	/// <para>
	/// Log messages are prepended with a time stamp of the form hh:mm:ss.cc. The "hours" component is
	/// in 24-hour clock format. Log messages are written to a file with a name of the form prefix-yymmdd-log.txt
	/// where "prefix" is the value of a static TextTraceListener property and yymmdd is the date that the message
	/// was written. Log message files are written to the current user's preferred (according to .NET) 
	/// temporary directory. Log message files are subject to "housekeeping" once a day. Housekeeping removes
	/// all but the most recent seven log files with the current prefix.
	/// </para>
	/// <para>
	/// You can change the prefix used to name log files. See <see cref="Prefix">Prefix</see>.
	/// You can change the file flushing behaviour. See <see cref="FlushType">FlushType</see>.
	/// </para>
	/// <example>
	/// </example>
	/// </remarks>
	#endregion
	public class TextTraceListener : TraceListener, IDisposable
	{
		#region members
		const string DefaultPrefix  = "Buck Rogers Log";
		const int    RetentionCount = 7; // Number of logs to keep when housekeeping

		private static bool         enabled   = true;
		private static LogFlushType flushType = LogFlushType.AutoClose;
		private static string       prefix; // initialized in static constructor
		private static string       logDir; // ditto

		static string   logfile_re_pattern;
		static Regex    logfile_re;
		static DateTime nextHousekeeping  = DateTime.MinValue;

		static StreamWriter writer = null;
		private static TextTraceListener listener = null;
		#endregion

		#region Constructors / setup
		public static void InstallListener()
		{
			if(listener == null)
			{
				listener = new TextTraceListener();
			}

			Debug.Listeners.Add(listener);

		}

		static TextTraceListener()
		{
			try
			{
				Prefix = DefaultPrefix;
				
				logDir = @"D:\Temp\";
				if (!Directory.Exists(logDir))
				{
					logDir = Path.GetTempPath();
				}
				int i = 42;
				int q = i;
			}
			catch (Exception)
			{
				logDir = @"\";
			}
		}

		/// <summary>
		/// This class is "static". Prevent instantiation.
		/// </summary>
		private TextTraceListener()
		{
		}
		#endregion

		#region properties
		/// <summary>
		/// Gets or sets a value indicating whether or not to perform operations
		/// on log files, including the writing of log messages.
		/// </summary>
		/// <value><b>true</b> if operations should be performed on log files; otherwise <b>false</b>. The default is <b>true</b>. </value>
		public static bool Enabled
		{
			get
			{
				return enabled;
			}
			set
			{
				enabled = value;
			}
		}

		/// <summary>
		/// Gets or sets the prefix used to name log files.
		/// </summary>
		/// <value>Any string containing characters valid in a filename. The default is "ox".</value>
		/// <remarks>
		/// Log files names are of the form <i>prefix</i>-<i>yymmdd</i>i>-log.txt, where <i>yymmdd</i> is the current date.
		/// </remarks>
		public static string Prefix
		{
			get
			{
				return prefix;
			}
			set
			{
				prefix             = value;
				logfile_re_pattern = String.Format(@".*{0}-\d{{6}}-log.txt$", prefix);
				logfile_re         = new Regex(logfile_re_pattern, RegexOptions.Compiled);
			}
		}

		/// <summary>
		/// Gets the full pathname of the log file currently in use.
		/// </summary>
		/// <value>The full pathname of the log file currently in use.</value>
		/// <remarks>
		/// This property is not needed for logging, or for resetting or deleting logs. 
		/// It is provided for callers who  may wish to read the contents of the current log file.
		/// </remarks>
		public static string LogPath
		{
			get
			{
				return String.Format(@"{0}{1}-{2:yyMMdd}-log.txt", logDir, Prefix, DateTime.Now);
			}
		}

		/// <summary>
		/// Gets or sets the log file flushing method.
		/// </summary>
		/// <value>The log file flushing method - AutoClose, AutoFlush or Manual.</value>
		/// <remarks>
		/// <para>
		/// This property controls when log buffers are actually written to the file system, and 
		/// whether or not the log file is kept open by TextTraceListener.</para>
		/// <para>
		/// The default method is AutoClose. AutoClose is easiest to use and most robust flush method. It is also the slowest.
		/// The following characteristics will help you choose the most appropriate method. Timings are taken from tests
		/// on an iPAQ 2200.
		/// </para>
		/// <list type="table">
		/// <listheader>
		/// <term>Method</term>
		/// <description>Characteristics</description>
		/// </listheader>
		/// <item><term>AutoClose</term><description>Slowest. About 10.5 ms. Most robust. Messages never lost. Log file closed except when a message is being written.</description></item>
		/// <item><term>AutoFlush</term><description>About 4.5 ms. Messages never lost. Log file kept open, limiting access from other applications.</description></item>
		/// <item><term>Manual</term><description>Fastest. About 4.0 ms. Least robust. Messages may be lost unless caller flushes or closes at appropriate times. Log file kept open, limiting access from other applications.</description></item>
		/// </list>
		/// </remarks>
		public static LogFlushType FlushType
		{
			get
			{
				return flushType;
			}
			set
			{
				flushType = value;
			}
		}

		/// <summary>
		/// A convenience property that compensates for the absence of
		/// Enum.GetNames() in the Compact Framework.
		/// </summary>
		/// <exclude />
		public static LogFlushType[] FlushTypes
		{
			get
			{
				return new LogFlushType[]{LogFlushType.AutoClose, LogFlushType.AutoFlush, LogFlushType.Manual};
			}
		}
		#endregion

		#region Management functions
		/// <summary>
		/// Flushes the log stream if it is open.
		/// </summary>
		/// <remarks>This method is useful only if <see cref="FlushType">FlushType</see> is Manual. 
		/// Otherwise it will have no effect.</remarks>
		public override void Flush()
		{
			if (!Enabled)
				return;

			if (writer != null)
				writer.Flush();
		}

		/// <summary>
		/// Closes the log stream if it is open.
		/// </summary>
		/// <remarks>This method is useful only if <see cref="FlushType">FlushType</see> is AutoFlush or Manual.
		/// Otherwise (i.e. AutoClose) it will have no effect.</remarks>
		public override void Close()
		{
			if (!Enabled)
				return;

			if (writer != null)
			{
				writer.Close();
				writer = null;
			}
			
		}

		/// <summary>
		/// Deletes the current log file and restarts the log with a "Log file has been reset" message.
		/// </summary>
		/// <remarks>
		///  The net result is that the log file is recreated with this single message. All
		/// previous messages in this log file are lost.
		/// </remarks>
		public static void Reset()
		{
			if (!Enabled || listener == null)
				return;

			listener.Delete();
			listener.Log("Log file reset under program control");
		}

		/// <summary>
		/// Deletes the current log file.
		/// </summary>
		/// <remarks> All previous messages in this log file are lost.</remarks>
		public void Delete()
		{
			if (!Enabled)
				return;

			string logPath = LogPath;

			Close();
			File.Delete(logPath);
		}

		private void Open()
		{
			if (!Enabled)
				return;

			if (writer == null)
				writer = new StreamWriter(LogPath, true);
		}

		private static void Housekeeping()
		{
			if (!Enabled)
				return;

			// Remove all but the most recent 7 log files. 
			string[] paths = Directory.GetFiles(logDir);
			ArrayList logfiles = new ArrayList();

			foreach (string path in paths)
			{
				if (logfile_re.IsMatch(path))
					logfiles.Add(new Logfile(path));
			}

			logfiles.Sort();

			for (int i = RetentionCount; i < logfiles.Count; i++)
				((Logfile)logfiles[i]).Delete();
		}
		#endregion

		#region Logging functions
		/// <summary>
		/// Log a message. Syntax as for String.Format.
		/// A time stamp, including hundredths of a second, is
		/// prepended.
		/// </summary>
		/// <param name="format">A String containing zero or more format items.</param>
		/// <param name="arg">An object array containg zero or more items to format.</param>
		/// <example>This example shows how to call the Log method.
		/// <code>
		/// class ConsoleApp
		/// {
		///		[STAThread]
		///		static void Main(string[] args)
		///		{
		///			TextTraceListener.Log("ConsoleApp starting, {0}.", "hello");
		///		}
		/// }
		/// </code>
		/// <para>
		/// The log file would be named something like ox-041130-log.txt. The log file would be in the preferred 
		/// temporary files directory. Normally the name of this directory is the value of the environment variable
		/// TEMP. The message logged in this example would look something like
		/// </para>
		/// <code>
		/// 11:36:01.47 ConsoleApp starting, hello.
		/// </code>
		/// </example>
		public void Log(string format, params object[] arg)
		{
			if (!Enabled)
				return;

			string logPath = LogPath;

			try
			{
				if (DateTime.Now > nextHousekeeping)
				{
					Housekeeping();
					nextHousekeeping = DateTime.Now.AddDays(1); // once every 24 hours
				}

				Open();

				DateTime now = HiResDateTime.Now;
				writer.Write("{0:HH:mm:ss.ff} ", now);
				writer.WriteLine(format, arg);

				if (TextTraceListener.FlushType == LogFlushType.AutoFlush)
					Flush();
				else if (TextTraceListener.FlushType == LogFlushType.AutoClose)
					Close();
			}
			catch (Exception)
			{
				// Not much we can do. Can't log it.
				// Some apps may want an event to be raised here.
				if (writer != null)
				{
					writer.Close();
					writer = null;
				}
			}
		}

		public override void Write(string message)
		{
			Log(message);
		}
		
		public override void WriteLine(string message)
		{
			string msg = String.Format("{0}\n", message);
			Log(message);
		}
		#endregion

		#region IDisposable Members

		new public void Dispose()
		{
			Close();
		}

		#endregion

		#region Helpers
		/// <summary>
		/// A class that encapsulates log files for sorting (descending)
		/// </summary>
		class Logfile : IComparable
		{
			string   path;

			public Logfile(string path)
			{
				this.path = path;
			}

			public void Delete()
			{
				File.Delete(path);
			}

			#region IComparable Members

			// Sort descending, i.e. most recent dates first
			public int CompareTo(object obj)
			{
				return ((Logfile) obj).path.CompareTo(path);
			}

			#endregion
		}

		/// <summary>
		/// An enumeration of the ways that log files can be flushed.
		/// </summary>
		public enum LogFlushType
		{
			/// <summary>
			/// The log file is opened and closed for each message.
			/// </summary>
			AutoClose,
			/// <summary>
			/// The the log file is kept open and is flushed after each message is written.
			/// </summary>
			AutoFlush,
			/// <summary>
			/// The log file is kept open. Flushing is the responsibility of the client.
			/// </summary>
			Manual
		}
		#endregion

	}
}
