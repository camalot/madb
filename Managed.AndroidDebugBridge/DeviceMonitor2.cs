using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Managed.Adb {
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Managed.Adb.IDeviceMonitor" />
	public class DeviceMonitor2 : IDeviceMonitor {
		private const string TAG = nameof ( DeviceMonitor2 );
		/// <summary>
		/// Occurs when [device state changed].
		/// </summary>
		public event EventHandler<DeviceEventArgs> DeviceStateChanged;
		/// <summary>
		/// Occurs when [connected].
		/// </summary>
		public event EventHandler<DeviceEventArgs> Connected;
		/// <summary>
		/// Occurs when [disconnected].
		/// </summary>
		public event EventHandler<DeviceEventArgs> Disconnected;

		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceMonitor2"/> class.
		/// </summary>
		/// <param name="bridge">The bridge.</param>
		public DeviceMonitor2 ( AndroidDebugBridge bridge, IDevice device ) {
			Bridge = bridge;
			Device = device;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceMonitor2"/> class.
		/// </summary>
		/// <param name="bridge">The bridge.</param>
		/// <param name="device">The device.</param>
		public DeviceMonitor2 ( AndroidDebugBridge bridge, string device ) {
			Bridge = bridge;
			Device = bridge.Devices.First(m => m.SerialNumber == device);
		}

		private AndroidDebugBridge Bridge { get; set; }
		public IDevice Device { get; private set; }

		public bool HasExited { get; private set; }

		public DeviceState State { get; set; } = DeviceState.Unknown;

		public void Restart ( ) {
			Stop ( );
			Start ( );
		}

		private Timer Timer { get; set; }

		/// <summary>
		/// Starts this instance.
		/// </summary>
		public void Start ( ) {
			HasExited = false;
			if ( Timer == null ) {
				Timer = new Timer ( device => {
					var pstate = State;
					State = Bridge.GetDeviceState ( (device as IDevice).SerialNumber );
					if ( pstate != State ) {
						if ( State == DeviceState.Online || State == DeviceState.Recovery || State == DeviceState.Device ) {
							if ( this.Connected != null ) {
								Log.v ( TAG, $"Connected: {State}" );
								this.Connected ( this, new DeviceEventArgs ( Device.SerialNumber, State ) );
							}
						} else {
							if ( this.Disconnected != null ) {
								Log.v ( TAG, $"Disconnected: {State}" );
								this.Disconnected ( this, new DeviceEventArgs ( Device.SerialNumber, State ) );
							}
						}

						if ( DeviceStateChanged != null ) {
							Log.v ( TAG, $"State Changed: {State}" );
							this.DeviceStateChanged ( this, new DeviceEventArgs ( Device.SerialNumber, State ) );
						}
					}
				}, this.Device, 0, 1000 );
			}
		}

		/// <summary>
		/// Stops this instance.
		/// </summary>
		public void Stop ( ) {
			if ( Timer != null ) {
				Timer.Change ( Timeout.Infinite, Timeout.Infinite );
				Timer = null;

				State = DeviceState.Offline;
				if ( this.Disconnected != null ) {
					this.Disconnected ( this, new DeviceEventArgs ( Device.SerialNumber, State ) );
				}
				if ( DeviceStateChanged != null ) {
					Log.v ( TAG, $"State Changed: {State}" );
					this.DeviceStateChanged ( this, new DeviceEventArgs ( Device.SerialNumber, State ) );
				}
				Log.v (TAG, "Stop" );
				HasExited = true;
			}
		}
	}
}
