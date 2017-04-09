using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managed.Adb {
	/// <summary>
	/// 
	/// </summary>
	public interface IDeviceMonitor {

		event EventHandler<DeviceEventArgs> DeviceStateChanged;
		/// <summary>
		/// Occurs when [connected].
		/// </summary>
		event EventHandler<DeviceEventArgs> Connected;
		/// <summary>
		/// Occurs when [disconnected].
		/// </summary>
		event EventHandler<DeviceEventArgs> Disconnected;

		IDevice Device { get; }
		bool HasExited { get; }
		DeviceState State { get; set; }
		void Start ( );
		void Stop ( );
		void Restart ( );
	}

}
