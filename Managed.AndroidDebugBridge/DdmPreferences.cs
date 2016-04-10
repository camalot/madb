using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managed.Adb {
	/// <summary>
	/// 
	/// </summary>
	/// <ignore>true</ignore>
	public static class DdmPreferences {
		/** Default value for thread update flag upon client connection. */
		public const bool DEFAULT_INITIAL_THREAD_UPDATE = false;
		/** Default value for heap update flag upon client connection. */
		public const bool DEFAULT_INITIAL_HEAP_UPDATE = false;
		/** Default value for the selected client debug port */
		public const int DEFAULT_SELECTED_DEBUG_PORT = 8700;
		/** Default value for the debug port base */
		public const int DEFAULT_DEBUG_PORT_BASE = 8600;
		/** Default value for the logcat {@link LogLevel} */
		public static readonly LogLevel.LogLevelInfo DEFAULT_LOG_LEVEL = Managed.Adb.LogLevel.Error;
		/** Default timeout values for adb connection (milliseconds) */
		public const int DEFAULT_TIMEOUT = 5000; // standard delay, in ms

		private static int _selectedDebugPort;
		private static LogLevel.LogLevelInfo _logLevel;

		/// <summary>
		/// Initializes the <see cref="DdmPreferences"/> class.
		/// </summary>
		static DdmPreferences ( ) {
			Timeout = DEFAULT_TIMEOUT;
			LogLevel = DEFAULT_LOG_LEVEL;
			SelectedDebugPort = DEFAULT_SELECTED_DEBUG_PORT;
			DebugPortBase = DEFAULT_DEBUG_PORT_BASE;
			InitialThreadUpdate = DEFAULT_INITIAL_THREAD_UPDATE;
			InitialHeapUpdate = DEFAULT_INITIAL_HEAP_UPDATE;
		}

		/// <summary>
		/// Gets or sets the timeout.
		/// </summary>
		/// <value>
		/// The timeout.
		/// </value>
		public static int Timeout { get; set; }
		/// <summary>
		/// Gets or sets the log level.
		/// </summary>
		/// <value>
		/// The log level.
		/// </value>
		public static LogLevel.LogLevelInfo LogLevel {
			get {
				return _logLevel;
			}
			set {
				_logLevel = value;
				Log.Level = value ;
			}
		}
		/// <summary>
		/// Gets or sets the debug port base.
		/// </summary>
		/// <value>
		/// The debug port base.
		/// </value>
		public static int DebugPortBase { get; set; }
		/// <summary>
		/// Gets or sets the selected debug port.
		/// </summary>
		/// <value>
		/// The selected debug port.
		/// </value>
		public static int SelectedDebugPort {
			get {
				return _selectedDebugPort;
			}
			set {
				_selectedDebugPort = value;

				MonitorThread monitorThread = MonitorThread.Instance;
				if ( monitorThread != null ) {
					monitorThread.SetDebugSelectedPort ( value );
				}
			}
		}
		/// <summary>
		/// Gets or sets a value indicating whether [initial thread update].
		/// </summary>
		/// <value>
		///   <c>true</c> if [initial thread update]; otherwise, <c>false</c>.
		/// </value>
		public static bool InitialThreadUpdate { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether [initial heap update].
		/// </summary>
		/// <value>
		///   <c>true</c> if [initial heap update]; otherwise, <c>false</c>.
		/// </value>
		public static bool InitialHeapUpdate { get; set; }
	}
}
