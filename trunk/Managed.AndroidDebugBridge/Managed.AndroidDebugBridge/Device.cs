﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using Managed.Adb.Exceptions;
using Managed.Adb.IO;
using Managed.Adb.Logs;
using System.Net;

namespace Managed.Adb {
	/// <summary>
	/// 
	/// </summary>
	public enum DeviceState {
		/// <summary>
		/// 
		/// </summary>
		Recovery,
		/// <summary>
		/// 
		/// </summary>
		BootLoader,
		/// <summary>
		/// 
		/// </summary>
		Offline,
		/// <summary>
		/// 
		/// </summary>
		Online,
		/// <summary>
		/// 
		/// </summary>
		Download,
		/// <summary>
		/// 
		/// </summary>
		Unknown
	}

	/// <summary>
	/// 
	/// </summary>
	public sealed class Device : IDevice {
		public const String MNT_EXTERNAL_STORAGE = "EXTERNAL_STORAGE";
		public const String MNT_DATA = "ANDROID_DATA";
		public const String MNT_ROOT = "ANDROID_ROOT";

		/// <summary>
		/// Occurs when [state changed].
		/// </summary>
		public event EventHandler<EventArgs> StateChanged;
		/// <summary>
		/// Occurs when [build info changed].
		/// </summary>
		public event EventHandler<EventArgs> BuildInfoChanged;
		/// <summary>
		/// Occurs when [client list changed].
		/// </summary>
		public event EventHandler<EventArgs> ClientListChanged;

		/// <summary>
		/// 
		/// </summary>
		public const String TEMP_DIRECTORY_FOR_INSTALL = "/storage/sdcard0/tmp/";

		/// <summary>
		/// 
		/// </summary>
		public const String PROP_BUILD_VERSION = "ro.build.version.release";

		/// <summary>
		/// 
		/// </summary>
		public const String PROP_BUILD_API_LEVEL = "ro.build.version.sdk";

		/// <summary>
		/// 
		/// </summary>
		public const String PROP_BUILD_CODENAME = "ro.build.version.codename";

		/// <summary>
		/// 
		/// </summary>
		public const String PROP_DEBUGGABLE = "ro.debuggable";

		/// <summary>
		/// Serial number of the first connected emulator. 
		/// </summary>
		public const String FIRST_EMULATOR_SN = "emulator-5554";

		/** @deprecated Use {@link #PROP_BUILD_API_LEVEL}. */
		[Obsolete("Use PROP_BUILD_API_LEVEL")]
		public const String PROP_BUILD_VERSION_NUMBER = PROP_BUILD_API_LEVEL;

		/// <summary>
		///  Emulator Serial Number regexp.
		/// </summary>
		private const String RE_EMULATOR_SN = @"emulator-(\d+)";

		/// <summary>
		/// Device list info regex
		/// </summary>
		/// <workitem>21136</workitem>
		private const String RE_DEVICELIST_INFO = @"^([a-z0-9_-]+(?:\s?[\.a-z0-9_-]+)?(?:\:\d{1,})?)\s+(device|offline|unknown|bootloader|recovery|download|host)(?:(?:\s+product:([\S]+)\s+model\:([\S]+)\s+device\:([\S]+))|(?:\s+features:\S+))?$";
		/// <summary>
		/// Tag
		/// </summary>
		private const String LOG_TAG = "Device";

		private const int BATTERY_TIMEOUT = 2 * 1000; //2 seconds
		private const int GETPROP_TIMEOUT = 2 * 1000; //2 seconds
		private const int INSTALL_TIMEOUT = 2 * 60 * 1000; // 2 minutes
		/// <summary>
		/// 
		/// </summary>
		private string _avdName;
		private IPEndPoint _endpoint;
		private bool _canSU = false;
		private BatteryInfo _lastBatteryInfo = null;
		private DateTime _lastBatteryCheckTime = DateTime.MinValue;

		/// <summary>
		/// Initializes a new instance of the <see cref="Device"/> class.
		/// </summary>
		/// <param name="serial">The serial.</param>
		/// <param name="state">The state.</param>
		/// <param name="model">The model.</param>
		/// <param name="product">The product.</param>
		/// <param name="device">The device.</param>
		public Device(string serial, DeviceState state, string model, string product, string device) {
			this.SerialNumber = serial;
			this.State = state;
			MountPoints = new Dictionary<String, MountPoint>();
			Properties = new Dictionary<string, string>();
			EnvironmentVariables = new Dictionary<string, string>();
			Clients = new List<IClient>();
			FileSystem = new FileSystem(this);
			BusyBox = new BusyBox(this);

			Model = model;
			Product = product;
			DeviceProperty = device;

			RetrieveDeviceInfo();
		}

		/// <summary>
		/// Retrieves the device info.
		/// </summary>
		private void RetrieveDeviceInfo() {
			RefreshMountPoints();
			RefreshEnvironmentVariables();
			RefreshProperties();
		}

		/// <summary>
		/// Get the device state from the string value
		/// </summary>
		/// <param name="state">The device state string</param>
		/// <returns></returns>
		private static DeviceState GetStateFromString(String state) {
			String tstate = state;

			if(String.Compare(state, "device", false) == 0) {
				tstate = "online";
			}

			if(Enum.IsDefined(typeof(DeviceState), tstate)) {
				return (DeviceState)Enum.Parse(typeof(DeviceState), tstate, true);
			} else {
				foreach(var fi in typeof(DeviceState).GetFields()) {
					if(string.Compare(fi.Name, tstate, true) == 0) {
						return (DeviceState)fi.GetValue(null);
					}
				}
			}

			return DeviceState.Unknown;
		}


		/// <summary>
		/// Create a device from Adb Device list data
		/// </summary>
		/// <param name="data">the line data for the device</param>
		/// <returns></returns>
		public static Device CreateFromAdbData(String data) {
			Regex re = new Regex(RE_DEVICELIST_INFO, RegexOptions.Compiled | RegexOptions.IgnoreCase);
			Match m = re.Match(data);
			if(m.Success) {
				if ( m.Groups[2].Value != "host" ) {
					return new Device ( m.Groups[1].Value, GetStateFromString ( m.Groups[2].Value ), m.Groups[4].Value, m.Groups[3].Value, m.Groups[5].Value );
				}
				return null;
			} else {
				throw new ArgumentException("Invalid device list data");
			}
		}

		/// <summary>
		/// Determines whether this device can perform a backup.
		/// </summary>
		/// <returns>
		///   <c>true</c> if this device can perform a backup; otherwise, <c>false</c>.
		/// </returns>
		public bool CanBackup() {
			return this.FileSystem.Exists("/system/bin/bu");
		}

		/// <summary>
		/// Backups this device.
		/// </summary>
		public void Backup() {
			AdbHelper.Instance.Backup(AndroidDebugBridge.SocketAddress);
		}

		/// <summary>
		/// Determines whether this instance can use the SU shell.
		/// </summary>
		/// <returns>
		///   <c>true</c> if this instance can use the SU shell; otherwise, <c>false</c>.
		/// </returns>
		public bool CanSU() {
			if(_canSU) {
				return _canSU;
			}

			try {
				// workitem: 16822
				// this now checks if permission was denied and accounts for that. 
				// The nulloutput receiver is fine here because it doesn't need to send the output anywhere,
				// the execute command can still handle the output with the null output receiver.
				this.ExecuteRootShellCommand("echo \\\"I can haz root\\\"", NullOutputReceiver.Instance);
				_canSU = true;
			} catch(PermissionDeniedException) {
				_canSU = false;
			} catch(FileNotFoundException) {
				_canSU = false;
			}

			return _canSU;
		}

		/// <summary>
		/// Gets or sets the client monitoring socket.
		/// </summary>
		/// <value>
		/// The client monitoring socket.
		/// </value>
		public Socket ClientMonitoringSocket { get; set; }

		/// <summary>
		/// Gets the device serial number
		/// </summary>
		public String SerialNumber { get; private set; }

		/// <summary>
		/// Gets the TCP endpoint defined when the transport is TCP.
		/// </summary>
		/// <value>
		/// The endpoint.
		/// </value>
		public IPEndPoint Endpoint { get; private set; }

		public TransportType TransportType { get; private set; }


		/// <summary>
		/// Gets or sets the Avd name.
		/// </summary>
		public String AvdName {
			get { return _avdName; }
			set {
				if(!IsEmulator) {
					throw new ArgumentException("Cannot set the AVD name of the device is not an emulator");
				}
				_avdName = value;
			}
		}


		/// <summary>
		/// Gets the product.
		/// </summary>
		/// <value>
		/// The product.
		/// </value>
		public string Product { get; private set; }
		/// <summary>
		/// Gets the model.
		/// </summary>
		/// <value>
		/// The model.
		/// </value>
		public string Model { get; private set; }
		/// <summary>
		/// Gets the device.
		/// </summary>
		/// <value>
		/// The device identifier.
		/// </value>
		public string DeviceProperty { get; private set; }

		/// <summary>
		/// Gets the device state
		/// </summary>
		public DeviceState State { get; internal set; }

		/// <summary>
		/// Gets the device mount points.
		/// </summary>
		public Dictionary<String, MountPoint> MountPoints { get; set; }


		/// <summary>
		/// Returns the device properties. It contains the whole output of 'getprop'
		/// </summary>
		/// <value>The properties.</value>
		public Dictionary<String, String> Properties { get; private set; }

		/// <summary>
		/// Gets the environment variables.
		/// </summary>
		/// <value>The environment variables.</value>
		public Dictionary<String, String> EnvironmentVariables { get; private set; }

		/// <summary>
		/// Gets the property value.
		/// </summary>
		/// <param name="name">The name of the property.</param>
		/// <returns>
		/// the value or <code>null</code> if the property does not exist.
		/// </returns>
		public String GetProperty(String name) {
			return GetProperty(new String[] { name });
		}

		/// <summary>
		/// Gets the first property that exists in the array of property names.
		/// </summary>
		/// <param name="name">The array of property names.</param>
		/// <returns></returns>
		public String GetProperty(params String[] name) {
			foreach(var item in name) {
				if(Properties.ContainsKey(item)) {
					return Properties[item];
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the file system for this device.
		/// </summary>
		public FileSystem FileSystem { get; private set; }

		/// <summary>
		/// Gets the busy box object for this device.
		/// </summary>
		public BusyBox BusyBox { get; private set; }

		/// <summary>
		/// Gets a value indicating whether the device is online.
		/// </summary>
		/// <value><c>true</c> if the device is online; otherwise, <c>false</c>.</value>
		public bool IsOnline {
			get {
				return State == DeviceState.Online;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this device is emulator.
		/// </summary>
		/// <value><c>true</c> if this device is emulator; otherwise, <c>false</c>.</value>
		public bool IsEmulator {
			get {
				return Regex.Match(SerialNumber, RE_EMULATOR_SN).Success;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this device is offline.
		/// </summary>
		/// <value><c>true</c> if this device is offline; otherwise, <c>false</c>.</value>
		public bool IsOffline {
			get {
				return State == DeviceState.Offline;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this device is in boot loader mode.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this device is in boot loader mode; otherwise, <c>false</c>.
		/// </value>
		public bool IsBootLoader {
			get {
				return State == DeviceState.BootLoader;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is recovery.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is recovery; otherwise, <c>false</c>.
		/// </value>
		public bool IsRecovery {
			get { return State == DeviceState.Recovery; }
		}

		/// <summary>
		/// Remounts the mount point.
		/// </summary>
		/// <param name="mnt">The mount point.</param>
		/// <param name="readOnly">if set to <c>true</c> the mount poine will be set to read-only.</param>
		public void RemountMountPoint(MountPoint mnt, bool readOnly) {
			String command = String.Format("mount -o {0},remount -t {1} {2} {3}", readOnly ? "ro" : "rw", mnt.FileSystem, mnt.Block, mnt.Name);
			this.ExecuteShellCommand(command, NullOutputReceiver.Instance);
			RefreshMountPoints();
		}

		/// <summary>
		/// Remounts the mount point.
		/// </summary>
		/// <param name="mountPoint">the mount point</param>
		/// <param name="readOnly">if set to <c>true</c> the mount poine will be set to read-only.</param>
		/// <exception cref="IOException">Throws if the mount point does not exist.</exception>
		public void RemountMountPoint(String mountPoint, bool readOnly) {
			if(MountPoints.ContainsKey(mountPoint)) {
				MountPoint mnt = MountPoints[mountPoint];
				RemountMountPoint(mnt, readOnly);
			} else {
				throw new IOException("Invalid mount point");
			}
		}


		/// <summary>
		/// Refreshes the mount points.
		/// </summary>
		public void RefreshMountPoints() {
			if(!IsOffline) {
				try {
					this.ExecuteShellCommand(MountPointReceiver.MOUNT_COMMAND, new MountPointReceiver(this));
				} catch(AdbException) {

				}
			}
		}

		/// <summary>
		/// Refreshes the environment variables.
		/// </summary>
		public void RefreshEnvironmentVariables() {
			if(!IsOffline) {
				try {
					this.ExecuteShellCommand(EnvironmentVariablesReceiver.ENV_COMMAND, new EnvironmentVariablesReceiver(this));
				} catch(AdbException) {

				}
			}
		}

		/// <summary>
		/// Refreshes the properties.
		/// </summary>
		public void RefreshProperties() {
			if(!IsOffline) {
				try {
					this.ExecuteShellCommand(GetPropReceiver.GETPROP_COMMAND, new GetPropReceiver(this));
				} catch(AdbException aex) {
					Log.w(LOG_TAG, aex);
				}
			}
		}

		/// <summary>
		/// Reboots the device in to the specified state
		/// </summary>
		/// <param name="into">The reboot state</param>
		public void Reboot(String into) {
			AdbHelper.Instance.Reboot(into, AndroidDebugBridge.SocketAddress, this);
		}

		/// <summary>
		/// Reboots the device in to the specified state
		/// </summary>
		public void Reboot() {
			Reboot(String.Empty);
		}

		/// <summary>
		/// Gets the battery level.
		/// </summary>
		/// <returns></returns>
		public BatteryInfo GetBatteryInfo() {
			return GetBatteryInfo(5 * 60 * 1000);
		}

		/// <summary>
		/// Gets the battery level.
		/// </summary>
		/// <param name="freshness">The freshness.</param>
		/// <returns></returns>
		public BatteryInfo GetBatteryInfo(long freshness) {
			if(_lastBatteryInfo != null
								&& this._lastBatteryCheckTime > (DateTime.Now.AddMilliseconds(-freshness))) {
				return _lastBatteryInfo;
			}
			var receiver = new BatteryReceiver();
			ExecuteShellCommand("dumpsys battery", receiver, BATTERY_TIMEOUT);
			_lastBatteryInfo = receiver.BatteryInfo;
			_lastBatteryCheckTime = DateTime.Now;
			return _lastBatteryInfo;
		}

		/// <summary>
		/// Gets a value indicating whether this instance has clients.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance has clients; otherwise, <c>false</c>.
		/// </value>
		public bool HasClients {
			get {
				return Clients.Count > 0;
			}
		}


		/// <summary>
		/// Gets the list of clients
		/// </summary>
		public List<IClient> Clients { get; private set; }

		/// <summary>
		/// Returns a <see cref="SyncService"/> object to push / pull files to and from the device.
		/// </summary>
		/// <value></value>
		/// <remarks>
		/// 	<code>null</code> if the SyncService couldn't be created. This can happen if adb
		/// refuse to open the connection because the {@link IDevice} is invalid (or got disconnected).
		/// </remarks>
		/// <exception cref="IOException">Throws IOException if the connection with adb failed.</exception>
		public SyncService SyncService {
			get {
				SyncService syncService = new SyncService(AndroidDebugBridge.SocketAddress, this);
				if(syncService.Open()) {
					return syncService;
				}

				return null;
			}
		}

		/// <summary>
		/// Returns a <see cref="PackageManager"/> for this device.
		/// </summary>
		public PackageManager PackageManager {
			get {
				return new PackageManager(this);
			}
		}

		/// <summary>
		/// Returns a <see cref="FileListingService"/> for this device.
		/// </summary>
		/// <value></value>
		public FileListingService FileListingService {
			get {
				return new FileListingService(this);
			}
		}

		/// <summary>
		/// Takes a screen shot of the device and returns it as a <see cref="RawImage"/>
		/// </summary>
		/// <value>The screenshot.</value>
		public RawImage Screenshot {
			get {
				return AdbHelper.Instance.GetFrameBuffer(AndroidDebugBridge.SocketAddress, this);
			}
		}

		/// <summary>
		/// Executes a shell command on the device, and sends the result to a receiver.
		/// </summary>
		/// <param name="command">The command to execute</param>
		/// <param name="receiver">The receiver object getting the result from the command.</param>
		public void ExecuteShellCommand(String command, IShellOutputReceiver receiver) {
			ExecuteShellCommand(command, receiver, new object[] { });
		}

		/// <summary>
		/// Executes a shell command on the device, and sends the result to a receiver.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="receiver">The receiver.</param>
		/// <param name="timeout">The timeout.</param>
		public void ExecuteShellCommand(String command, IShellOutputReceiver receiver, int timeout) {
			ExecuteShellCommand(command, receiver, new object[] { });
		}


		/// <summary>
		/// Executes a shell command on the device, and sends the result to a receiver.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="receiver">The receiver.</param>
		/// <param name="commandArgs">The command args.</param>
		public void ExecuteShellCommand(String command, IShellOutputReceiver receiver, params object[] commandArgs) {
			AdbHelper.Instance.ExecuteRemoteCommand(AndroidDebugBridge.SocketAddress, string.Format(command, commandArgs), this, receiver);
		}

		/// <summary>
		/// Executes a shell command on the device, and sends the result to a receiver.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="receiver">The receiver.</param>
		/// <param name="timeout">The timeout.</param>
		/// <param name="commandArgs">The command args.</param>
		public void ExecuteShellCommand(String command, IShellOutputReceiver receiver, int timeout, params object[] commandArgs) {
			AdbHelper.Instance.ExecuteRemoteCommand(AndroidDebugBridge.SocketAddress, string.Format(command, commandArgs), this, receiver);
		}


		/// <summary>
		/// Executes a shell command on the device as root, and sends the result to a receiver.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="receiver">The receiver.</param>
		public void ExecuteRootShellCommand(String command, IShellOutputReceiver receiver, int timeout) {
			ExecuteRootShellCommand(command, receiver, timeout, new object[] { });
		}

		/// <summary>
		/// Executes a shell command on the device as root, and sends the result to a receiver.
		/// </summary>
		/// <param name="command">The command to execute</param>
		/// <param name="receiver">The receiver object getting the result from the command.</param>
		public void ExecuteRootShellCommand(String command, IShellOutputReceiver receiver) {
			ExecuteRootShellCommand(command, receiver, int.MaxValue);
		}

		/// <summary>
		/// Executes a root shell command on the device, and sends the result to a receiver.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="receiver">The receiver.</param>
		/// <param name="commandArgs">The command args.</param>
		public void ExecuteRootShellCommand(String command, IShellOutputReceiver receiver, params object[] commandArgs) {
			ExecuteRootShellCommand(command, receiver, int.MaxValue, commandArgs);
		}

		/// <summary>
		/// Executes a root shell command on the device, and sends the result to a receiver.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="receiver">The receiver.</param>
		/// <param name="timeout">The timeout.</param>
		/// <param name="commandArgs">The command args.</param>
		public void ExecuteRootShellCommand(String command, IShellOutputReceiver receiver, int timeout, params object[] commandArgs) {
			AdbHelper.Instance.ExecuteRemoteRootCommand(AndroidDebugBridge.SocketAddress, string.Format(command, commandArgs), this, receiver, timeout);
		}


		/// <summary>
		/// Runs the event log service.
		/// </summary>
		/// <param name="receiver">The receiver.</param>
		public void RunEventLogService(LogReceiver receiver) {
			AdbHelper.Instance.RunEventLogService(AndroidDebugBridge.SocketAddress, this, receiver);
		}

		/// <summary>
		/// Runs the log service.
		/// </summary>
		/// <param name="logname">The logname.</param>
		/// <param name="receiver">The receiver.</param>
		public void RunLogService(String logname, LogReceiver receiver) {
			AdbHelper.Instance.RunLogService(AndroidDebugBridge.SocketAddress, this, logname, receiver);
		}

		/// <summary>
		/// Creates a port forwarding between a local and a remote port.
		/// </summary>
		/// <param name="localPort">the local port to forward</param>
		/// <param name="remotePort">the remote port.</param>
		/// <returns><code>true</code> if success.</returns>
		public bool CreateForward(int localPort, int remotePort) {
			try {
				return AdbHelper.Instance.CreateForward(AndroidDebugBridge.SocketAddress, this, localPort, remotePort);
			} catch(IOException e) {
				Log.w("ddms", e);
				return false;
			}
		}

		/// <summary>
		/// Removes a port forwarding between a local and a remote port.
		/// </summary>
		/// <param name="localPort">the local port to forward</param>
		/// <param name="remotePort">the remote port.</param>
		/// <returns><code>true</code> if success.</returns>
		public bool RemoveForward(int localPort) {
			try {
				return AdbHelper.Instance.RemoveForward(AndroidDebugBridge.SocketAddress, this, localPort);
			} catch(IOException e) {
				Log.w("ddms", e);
				return false;
			}
		}

		/*
		public String GetClientName ( int pid ) {
			lock ( ClientList ) {
				foreach ( Client c in ClientList ) {
					if ( c.ClientData ( ).Pid == pid ) {
						return c.ClientData.ClientDescription;
					}
				}
			}

			return null;
		}

		DeviceMonitor Monitor { get; private set; }

		void AddClient ( Client client ) {
			lock ( ClientList ) {
				ClientList.Add ( client );
			}
		}

		List<Client> ClientList { get; private set; }

		bool HasClient ( int pid ) {
			lock ( ClientList ) {
				foreach ( Client client in ClientList ) {
					if ( client.ClientData.Pid == pid ) {
						return true;
					}
				}
			}

			return false;
		}

		void ClearClientList ( ) {
			lock ( ClientList ) {
				ClientList.Clear ( );
			}
		}
		
		SocketChannel ClientMonitoringSocket { get; set; }

		void RemoveClient ( Client client, bool notify ) {
			Monitor.AddPortToAvailableList ( client.DebuggerListenPort );
			lock ( ClientList ) {
				ClientList.Remove ( client );
			}
			if ( notify ) {
				Monitor.Server.DeviceChanged ( this, CHANGE_CLIENT_LIST );
			}
		}

		void Update ( int changeMask ) {
			Monitor.Server.DeviceChanged ( this, changeMask );
		}

		void Update ( Client client, int changeMask ) {
			Monitor.Server.ClientChanged ( client, changeMask );
		}
*/

		/// <summary>
		/// Installs an Android application on device.
		/// This is a helper method that combines the syncPackageToDevice, installRemotePackage,
		/// and removePackage steps
		/// </summary>
		/// <param name="packageFilePath">the absolute file system path to file on local host to install</param>
		/// <param name="reinstall">set to <code>true</code>if re-install of app should be performed</param>
		public void InstallPackage(String packageFilePath, bool reinstall) {
			String remoteFilePath = SyncPackageToDevice(packageFilePath);
			InstallRemotePackage(remoteFilePath, reinstall);
			RemoveRemotePackage(remoteFilePath);
		}

		/// <summary>
		/// Pushes a file to device
		/// </summary>
		/// <param name="localFilePath">the absolute path to file on local host</param>
		/// <returns>destination path on device for file</returns>
		/// <exception cref="IOException">if fatal error occurred when pushing file</exception>
		public String SyncPackageToDevice(String localFilePath) {
			try {
				String packageFileName = Path.GetFileName(localFilePath);
				// only root has access to /data/local/tmp/... not sure how adb does it then...
				// workitem: 16823
				// workitem: 19711
				String remoteFilePath = LinuxPath.Combine(TEMP_DIRECTORY_FOR_INSTALL, packageFileName);
				Log.d(packageFileName, String.Format("Uploading {0} onto device '{1}'", packageFileName, SerialNumber));

				SyncService sync = SyncService;
				if(sync != null) {
					String message = String.Format("Uploading file onto device '{0}'", SerialNumber);
					Log.d(LOG_TAG, message);
					SyncResult result = sync.PushFile(localFilePath, remoteFilePath, SyncService.NullProgressMonitor);

					if(result.Code != ErrorCodeHelper.RESULT_OK) {
						throw new IOException(String.Format("Unable to upload file: {0}", result.Message));
					}
				} else {
					throw new IOException("Unable to open sync connection!");
				}
				return remoteFilePath;
			} catch(IOException e) {
				Log.e(LOG_TAG, String.Format("Unable to open sync connection! reason: {0}", e.Message));
				throw;
			}
		}

		/// <summary>
		/// Installs the application package that was pushed to a temporary location on the device.
		/// </summary>
		/// <param name="remoteFilePath">absolute file path to package file on device</param>
		/// <param name="reinstall">set to <code>true</code> if re-install of app should be performed</param>
		public void InstallRemotePackage(String remoteFilePath, bool reinstall) {
			InstallReceiver receiver = new InstallReceiver();
			FileEntry entry = FileListingService.FindFileEntry(remoteFilePath);
			String cmd = String.Format("pm install {1}{0}", entry.FullEscapedPath, reinstall ? "-r " : String.Empty);
			ExecuteShellCommand(cmd, receiver);

			if(!String.IsNullOrEmpty(receiver.ErrorMessage)) {
				throw new PackageInstallationException(receiver.ErrorMessage);
			}
		}


		/// <summary>
		/// Remove a file from device
		/// </summary>
		/// <param name="remoteFilePath">path on device of file to remove</param>
		/// <exception cref="IOException">if file removal failed</exception>
		public void RemoveRemotePackage(String remoteFilePath) {
			// now we delete the app we sync'ed
			try {
				ExecuteShellCommand("rm " + remoteFilePath, NullOutputReceiver.Instance);
			} catch(IOException e) {
				Log.e(LOG_TAG, String.Format("Failed to delete temporary package: {0}", e.Message));
				throw e;
			}
		}

		/// <summary>
		/// Uninstall an package from the device.
		/// </summary>
		/// <param name="packageName">Name of the package.</param>
		/// <exception cref="IOException"></exception>
		///   
		/// <exception cref="PackageInstallException"></exception>
		public void UninstallPackage(String packageName) {
			InstallReceiver receiver = new InstallReceiver();
			ExecuteShellCommand(String.Format("pm uninstall {0}", packageName), receiver);
			if(!String.IsNullOrEmpty(receiver.ErrorMessage)) {
				throw new PackageInstallationException(receiver.ErrorMessage);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:StateChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		internal void OnStateChanged(EventArgs e) {
			if(this.StateChanged != null) {
				this.StateChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:BuildInfoChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		internal void OnBuildInfoChanged(EventArgs e) {
			if(this.BuildInfoChanged != null) {
				this.BuildInfoChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:ClientListChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		internal void OnClientListChanged(EventArgs e) {
			if(this.ClientListChanged != null) {
				this.ClientListChanged(this, e);
			}
		}
	}
}