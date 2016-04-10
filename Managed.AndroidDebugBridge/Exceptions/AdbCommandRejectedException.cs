using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managed.Adb.Exceptions {

	/// <summary>
	/// Exception thrown when adb refuses a command.
	/// </summary>
	public class AdbCommandRejectedException : AdbException {
		/// <summary>
		/// Initializes a new instance of the <see cref="AdbCommandRejectedException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public AdbCommandRejectedException ( String message )
			: base ( message ) {
			IsDeviceOffline = message.Equals ( "device offline" );
			WasErrorDuringDeviceSelection = false;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="AdbCommandRejectedException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="errorDuringDeviceSelection">if set to <c>true</c> [error during device selection].</param>
		public AdbCommandRejectedException ( String message, bool errorDuringDeviceSelection )
			: base ( message ) {
			WasErrorDuringDeviceSelection = errorDuringDeviceSelection;
			IsDeviceOffline = message.Equals ( "device offline" );
		}
		/// <summary>
		/// Gets a value indicating whether this instance is device offline.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is device offline; otherwise, <c>false</c>.
		/// </value>
		public bool IsDeviceOffline { get; private set; }
		/// <summary>
		/// Gets a value indicating whether [was error during device selection].
		/// </summary>
		/// <value>
		/// <c>true</c> if [was error during device selection]; otherwise, <c>false</c>.
		/// </value>
		public bool WasErrorDuringDeviceSelection { get; private set; }
	}
}
