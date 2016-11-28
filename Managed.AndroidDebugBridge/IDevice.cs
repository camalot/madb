﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Net;

namespace Managed.Adb {

	/// <summary>
	/// 
	/// </summary>
	public interface IDevice {

		/// <summary>
		/// Occurs when device state changed.
		/// </summary>
		event EventHandler<EventArgs> StateChanged;
		/// <summary>
		/// Occurs when build info changed.
		/// </summary>
		event EventHandler<EventArgs> BuildInfoChanged;
		/// <summary>
		/// Occurs when client list changed.
		/// </summary>
		event EventHandler<EventArgs> ClientListChanged;

		/// <summary>
		/// Gets the file system.
		/// </summary>
		/// <value>
		/// The file system.
		/// </value>
		FileSystem FileSystem { get; }

		/// <summary>
		/// Gets the busy box.
		/// </summary>
		/// <value>
		/// The busy box.
		/// </value>
		BusyBox BusyBox { get; }

		/// <summary>
		/// Gets the serial number of the device.
		/// </summary>
		/// <value>The serial number.</value>
		string SerialNumber { get; }
		/// <summary>
		/// Gets the TCP endpoint defined when the transport is TCP.
		/// </summary>
		/// <value>
		/// The endpoint.
		/// </value>
		IPEndPoint Endpoint { get; }

		/// <summary>
		/// Gets the type of the transport used to connect to this device.
		/// </summary>
		/// <value>
		/// The type of the transport.
		/// </value>
		TransportType TransportType { get; }

		/// <summary>
		/// Returns the name of the AVD the emulator is running.
		/// <p/>
		/// This is only valid if {@link #isEmulator()} returns true.
		/// <p/>
		/// If the emulator is not running any AVD (for instance it's running from an Android source
		/// tree build), this method will return "<code>&lt;build&gt;</code>"
		/// @return the name of the AVD or  <code>null</code>
		///  if there isn't any.
		/// </summary>
		/// <value>The name of the avd.</value>
		string AvdName { get; set; }

		/// <summary>
		/// Gets the environment variables.
		/// </summary>
		/// <value>The environment variables.</value>
		Dictionary<string, string> EnvironmentVariables { get; }

		/// <summary>
		/// Gets the mount points.
		/// </summary>
		/// <value>The mount points.</value>
		Dictionary<string, MountPoint> MountPoints { get; }

		/// <summary>
		/// Gets the state.
		/// </summary>
		/// <value>The state.</value>
		/// Returns the state of the device.
		DeviceState State { get; }

		/// <summary>
		/// Returns the device properties. It contains the whole output of 'getprop'
		/// </summary>
		/// <value>The properties.</value>
		Dictionary<string, string> Properties { get; }

		/// <summary>
		/// Gets the property value.
		/// </summary>
		/// <param name="name">The name of the property.</param>
		/// <returns>
		/// the value or <code>null</code> if the property does not exist.
		/// </returns>
		string GetProperty ( string name );
		/// <summary>
		/// Gets the first property that exists in the array of property names.
		/// </summary>
		/// <param name="name">The array of property names.</param>
		/// <returns>
		/// the value or <code>null</code> if the property does not exist.
		/// </returns>
		string GetProperty ( params string[] name );

		/// <summary>
		/// Gets a value indicating whether the device is online.
		/// </summary>
		/// <value><c>true</c> if the device is online; otherwise, <c>false</c>.</value>
		bool IsOnline { get; }


		/// <summary>
		/// Gets a value indicating whether this device is emulator.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this device is emulator; otherwise, <c>false</c>.
		/// </value>
		bool IsEmulator { get; }

		/// <summary>
		/// Gets a value indicating whether this device is offline.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this device is offline; otherwise, <c>false</c>.
		/// </value>
		bool IsOffline { get; }


		/// <summary>
		/// Gets a value indicating whether this device is in boot loader mode.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this device is in boot loader mode; otherwise, <c>false</c>.
		/// </value>
		bool IsBootLoader { get; }

		/*
		 * Returns whether the {@link Device} has {@link Client}s.
		 */
		//bool HasClients { get; }

		/// <summary>
		/// Gets the list of clients
		/// </summary>
		List<IClient> Clients { get; }

		/*
		 * Returns a {@link Client} by its application name.
		 * @param applicationName the name of the application
		 * @return the <code>Client</code> object or <code>null</code> if no match was found.
		 */
		//Client GetClient(String applicationName);

		/// <summary>
		/// Returns a <see cref="SyncService" /> object to push / pull files to and from the device.
		/// </summary>
		/// <value>
		/// The synchronize service.
		/// </value>
		/// <exception cref="System.IO.IOException">Throws IOException if the connection with adb failed.</exception>
		/// <remarks>
		/// <code>null</code> if the SyncService couldn't be created. This can happen if adb
		/// refuse to open the connection because the {@link IDevice} is invalid (or got disconnected).
		/// </remarks>
		SyncService SyncService { get; }

		/// <summary>
		/// Returns a <see cref="FileListingService" /> for this device.
		/// </summary>
		/// <value>
		/// The file listing service.
		/// </value>
		FileListingService FileListingService { get; }


		/// <summary>
		/// Takes a screen shot of the device and returns it as a <see cref="RawImage"/>
		/// </summary>
		/// <value>The screenshot.</value>
		RawImage Screenshot { get; }

		/// <summary>
		/// Executes a shell command on the device, and sends the result to a receiver.
		/// </summary>
		/// <param name="command">The command to execute</param>
		/// <param name="receiver">The receiver object getting the result from the command.</param>
		void ExecuteShellCommand ( string command, IShellOutputReceiver receiver );
		/// <summary>
		/// Executes a shell command on the device, and sends the result to a receiver.
		/// </summary>
		/// <param name="command">The command to execute</param>
		/// <param name="receiver">The receiver object getting the result from the command.</param>
		/// <param name="maxTimeToOutputResponse">The max time to output response.</param>
		void ExecuteShellCommand ( string command, IShellOutputReceiver receiver, int maxTimeToOutputResponse );

		/// <summary>
		/// Executes a shell command on the device, and sends the result to a receiver.
		/// </summary>
		/// <param name="command">The command to execute</param>
		/// <param name="receiver">The receiver object getting the result from the command.</param>
		/// <param name="commandArgs">The command args.</param>
		void ExecuteShellCommand ( string command, IShellOutputReceiver receiver, params object[] commandArgs );

		/// <summary>
		/// Executes a shell command on the device, and sends the result to a receiver.
		/// </summary>
		/// <param name="command">The command to execute</param>
		/// <param name="receiver">The receiver object getting the result from the command.</param>
		/// <param name="maxTimeToOutputResponse">The max time to output response.</param>
		/// <param name="commandArgs">The command args.</param>
		void ExecuteShellCommand ( string command, IShellOutputReceiver receiver, int maxTimeToOutputResponse, params object[] commandArgs );


		/// <summary>
		/// Executes a shell command on the device as root, and sends the result to a receiver.
		/// </summary>
		/// <param name="command">The command to execute</param>
		/// <param name="receiver">The receiver object getting the result from the command.</param>
		/// <param name="commandArgs">The command args.</param>
		void ExecuteRootShellCommand ( string command, IShellOutputReceiver receiver, params object[] commandArgs );

		/// <summary>
		/// Executes a shell command on the device as root, and sends the result to a receiver.
		/// </summary>
		/// <param name="command">The command to execute</param>
		/// <param name="receiver">The receiver object getting the result from the command.</param>
		/// <param name="maxTimeToOutputResponse">The max time to output response.</param>
		/// <param name="commandArgs">The command args.</param>
		void ExecuteRootShellCommand ( string command, IShellOutputReceiver receiver, int maxTimeToOutputResponse, params object[] commandArgs );

		/// <summary>
		/// Executes a shell command on the device as root, and sends the result to a receiver.
		/// </summary>
		/// <param name="command">The command to execute</param>
		/// <param name="receiver">The receiver object getting the result from the command.</param>
		void ExecuteRootShellCommand ( string command, IShellOutputReceiver receiver );

		/// <summary>
		/// Executes a shell command on the device as root, and sends the result to a receiver.
		/// </summary>
		/// <param name="command">The command to execute</param>
		/// <param name="receiver">The receiver object getting the result from the command.</param>
		/// <param name="maxTimeToOutputResponse">The max time to output response.</param>
		void ExecuteRootShellCommand ( string command, IShellOutputReceiver receiver, int maxTimeToOutputResponse );
		/*
		 * Runs the event log service and outputs the event log to the {@link LogReceiver}.
		 * @param receiver the receiver to receive the event log entries.
		 * @throws IOException
		 */
		//void RunEventLogService(LogReceiver receiver);

		/*
		 * Runs the log service for the given log and outputs the log to the {@link LogReceiver}.
		 * @param logname the logname of the log to read from.
		 * @param receiver the receiver to receive the event log entries.
		 * @throws IOException
		 */
		//void RunLogService(String logname, LogReceiver receiver);
		
		/// <summary>
		/// Creates a port forwarding between a local and a remote port.
		/// </summary>
		/// <param name="localPort">the local port to forward</param>
		/// <param name="remotePort">the remote port.</param>
		/// <returns><code>true</code> if success.</returns>
		bool CreateForward(int localPort, int remotePort);
		
		/// <summary>
		/// Creates a reverse port forwarding between a local and a remote port.
		/// </summary>
		/// <param name="remotePort">the remote port to forward</param>
		/// <param name="localPort">the local port.</param>
		/// <returns><code>true</code> if success.</returns>
		bool CreateReverseForward(int remotePort, int localPort);

		/// <summary>
		/// Creates a reverse port forwarding between a local and a remote port.
		/// </summary>
		/// <param name="remotePort">the remote port to forward</param>
		/// <param name="localPort">the local port.</param>
		/// <returns><code>true</code> if success.</returns>
		bool CreateReverseForward ( int remotePort, int localPort );

		/// <summary>
		/// Removes a port forwarding between a local and a remote port.
		/// </summary>
		/// <param name="localPort"> the local port to forward</param>
		/// <returns><code>true</code> if success.</returns>
		bool RemoveForward ( int localPort );

		/*
		 * Returns the name of the client by pid or <code>null</code> if pid is unknown
		 * @param pid the pid of the client.
		 */
		//String GetClientName(int pid);

		/// <summary>
		/// Installs an Android application on device.
		/// This is a helper method that combines the syncPackageToDevice, installRemotePackage,
		/// and removePackage steps
		/// </summary>
		/// <param name="packageFilePath">the absolute file system path to file on local host to install</param>
		/// <param name="reinstall">set to <code>true</code>if re-install of app should be performed</param>
		void InstallPackage ( string packageFilePath, bool reinstall );

		/// <summary>
		/// Pushes a file to device
		/// </summary>
		/// <param name="localFilePath">the absolute path to file on local host</param>
		/// <returns>
		/// destination path on device for file
		/// </returns>
		/// <exception cref="System.IO.IOException">if fatal error occurred when pushing file</exception>
		string SyncPackageToDevice ( string localFilePath );
 
		/// <summary>
		/// Installs the application package that was pushed to a temporary location on the device.
		/// </summary>
		/// <param name="remoteFilePath">absolute file path to package file on device</param>
		/// <param name="reinstall">set to <code>true</code> if re-install of app should be performed</param>
		void InstallRemotePackage( string remoteFilePath, bool reinstall);


		/// <summary>
		/// Remove a file from device
		/// </summary>
		/// <param name="remoteFilePath">path on device of file to remove</param>
		/// <exception cref="System.IO.IOException">if file removal failed</exception>
		void RemoveRemotePackage(string remoteFilePath);

		/// <summary>
		/// Uninstall an package from the device.
		/// </summary>
		/// <param name="packageName">Name of the package.</param>
		/// <exception cref="System.IO.IOException"></exception>
		/// <exception cref="Exceptions.PackageInstallationException"></exception>
		void UninstallPackage (string packageName) ;

		/// <summary>
		/// Refreshes the environment variables.
		/// </summary>
		void RefreshEnvironmentVariables ( );

		/// <summary>
		/// Refreshes the mount points.
		/// </summary>
		void RefreshMountPoints ( );

		/// <summary>
		/// Refreshes the properties.
		/// </summary>
		void RefreshProperties ( );
	}
}
