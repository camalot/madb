using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managed.Adb {
	/// <summary>
	/// Centralized point to provide a IDebugPortProvider to ddmlib.
	/// </summary>
	public class DebugPortManager {
		/// <summary>
		/// no static port
		/// </summary>
		public const int NO_STATIC_PORT = -1;

		/// <summary>
		/// Initializes a new instance of the <see cref="DebugPortManager"/> class.
		/// </summary>
		public DebugPortManager ( ) {

		}

		/// <summary>
		/// Gets or sets the IDebugPortProvider that will be used when a new Client requests
		/// </summary>
		public IDebugPortProvider Provider { get; set; }

		/// <summary>
		/// The _instance
		/// </summary>
		private static DebugPortManager _instance;
		/// <summary>
		/// Returns an instance of the debug port manager
		/// </summary>
		public DebugPortManager Instance {
			get {
				if ( _instance == null ) {
					_instance = new DebugPortManager ( );
				}
				return _instance;
			}
		}
	}
}
