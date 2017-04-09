using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managed.Adb {
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="System.EventArgs" />
	public class DeviceEventArgs : EventArgs {

		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceEventArgs"/> class.
		/// </summary>
		/// <param name="device">The device.</param>
		public DeviceEventArgs ( string device, DeviceState state ) {
			this.Device = device;
			this.State = state;
		}

		/// <summary>
		/// Gets the device.
		/// </summary>
		/// <value>The device.</value>
		public string Device { get; private set; }
		public DeviceState State { get; set; } = DeviceState.Unknown;
	}
}
