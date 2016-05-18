using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Managed.Adb {
	/// <summary>
	/// 
	/// </summary>
	[Flags]
		public enum ClientChangeState {
		/// <summary>
		/// The name
		/// </summary>
		Name = 0x0001,
		/// <summary>
		/// The debugger status
		/// </summary>
		DebuggerStatus = 0x0002,
		/// <summary>
		/// The port
		/// </summary>
		Port = 0x0004,
		/// <summary>
		/// The thread mode
		/// </summary>
		ThreadMode = 0x0008,
		/// <summary>
		/// The thread data
		/// </summary>
		ThreadData = 0x0010,
		/// <summary>
		/// The heap mode
		/// </summary>
		HeapMode = 0x0020,
		/// <summary>
		/// The heap data
		/// </summary>
		HeapData = 0x0040,
		/// <summary>
		/// The native heap data
		/// </summary>
		NativeHeapData = 0x0080,
		/// <summary>
		/// The thread stack trace
		/// </summary>
		ThreadStackTrace = 0x0100,
		/// <summary>
		/// The heap allocations
		/// </summary>
		HeapAllocations = 0x0200,
		/// <summary>
		/// The heap allocation status
		/// </summary>
		HeapAllocationStatus = 0x0400,
		/// <summary>
		/// The method profiling status
		/// </summary>
		MethodProfilingStatus = 0x0800,
		/// <summary>
		/// The information
		/// </summary>
		Info = Name | DebuggerStatus | Port,
		}

	/// <summary>
	/// 
	/// </summary>
	public enum ClientConnectionState {
		/// <summary>
		/// The initialize
		/// </summary>
		Init = 1,
		/// <summary>
		/// The not JDWP
		/// </summary>
		NotJDWP = 2,
		/// <summary>
		/// The await shake
		/// </summary>
		AwaitShake = 10,
		/// <summary>
		/// The need DDM packet
		/// </summary>
		NeedDDMPacket = 11,
		/// <summary>
		/// The not DDM
		/// </summary>
		NotDDM = 12,
		/// <summary>
		/// The ready
		/// </summary>
		Ready = 13,
		/// <summary>
		/// The error
		/// </summary>
		Error = 20,
		/// <summary>
		/// The disconnected
		/// </summary>
		Disconnected = 21,
		}

	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Managed.Adb.IPacketConsumer" />
	public interface IClient : IPacketConsumer {


		/// <summary>
		/// Gets the state of the connection.
		/// </summary>
		/// <value>
		/// The state of the connection.
		/// </value>
		ClientConnectionState ConnectionState { get; }
		/// <summary>
		/// Gets the state of the change.
		/// </summary>
		/// <value>
		/// The state of the change.
		/// </value>
		ClientChangeState ChangeState { get; }
		/// <summary>
		/// Gets or sets the channel.
		/// </summary>
		/// <value>
		/// The channel.
		/// </value>
		Socket Channel { get; set; }

		/// <summary>
		/// Gets the device.
		/// </summary>
		/// <value>
		/// The device.
		/// </value>
		IDevice Device { get; }
		/// <summary>
		/// Gets the device implementation.
		/// </summary>
		/// <value>
		/// The device implementation.
		/// </value>
		Device DeviceImplementation { get; }
		/// <summary>
		/// Gets the debugger listen port.
		/// </summary>
		/// <value>
		/// The debugger listen port.
		/// </value>
		int DebuggerListenPort { get; }
		/// <summary>
		/// Gets a value indicating whether this instance is DDM aware.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is DDM aware; otherwise, <c>false</c>.
		/// </value>
		bool IsDdmAware { get; }
		/// <summary>
		/// Gets a value indicating whether this instance is debugger attached.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is debugger attached; otherwise, <c>false</c>.
		/// </value>
		bool IsDebuggerAttached { get; }
		/// <summary>
		/// Gets the debugger.
		/// </summary>
		/// <value>
		/// The debugger.
		/// </value>
		Debugger Debugger { get; }
		/// <summary>
		/// Gets the client data.
		/// </summary>
		/// <value>
		/// The client data.
		/// </value>
		ClientData ClientData { get; }
		/// <summary>
		/// Gets or sets a value indicating whether this instance is thread update enabled.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is thread update enabled; otherwise, <c>false</c>.
		/// </value>
		bool IsThreadUpdateEnabled { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether this instance is heap update enabled.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is heap update enabled; otherwise, <c>false</c>.
		/// </value>
		bool IsHeapUpdateEnabled { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether this instance is selected client.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is selected client; otherwise, <c>false</c>.
		/// </value>
		bool IsSelectedClient { get; set; }
		/// <summary>
		/// Returns true if ... is valid.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
		/// </value>
		bool IsValid { get; }

		/// <summary>
		/// Executes the garbage collector.
		/// </summary>
		void ExecuteGarbageCollector ( );
		/// <summary>
		/// Dumps the hprof.
		/// </summary>
		void DumpHprof ( );
		/// <summary>
		/// Toggles the method profiling.
		/// </summary>
		void ToggleMethodProfiling ( );
		/// <summary>
		/// Requests the method profiling status.
		/// </summary>
		void RequestMethodProfilingStatus ( );
		/// <summary>
		/// Requests the thread update.
		/// </summary>
		void RequestThreadUpdate ( );
		/// <summary>
		/// Requests the thread stack trace.
		/// </summary>
		/// <param name="threadID">The thread identifier.</param>
		void RequestThreadStackTrace ( int threadID );
		/// <summary>
		/// Requests the native heap information.
		/// </summary>
		/// <returns></returns>
		bool RequestNativeHeapInformation ( );
		/// <summary>
		/// Enables the allocation tracker.
		/// </summary>
		/// <param name="enable">if set to <c>true</c> [enable].</param>
		void EnableAllocationTracker ( bool enable );
		/// <summary>
		/// Requests the allocation status.
		/// </summary>
		void RequestAllocationStatus ( );
		/// <summary>
		/// Requests the allocation details.
		/// </summary>
		void RequestAllocationDetails ( );
		/// <summary>
		/// Kills this instance.
		/// </summary>
		void Kill ( );
		// TODO: Define Selector
		/// <summary>
		/// Registers the specified selector.
		/// </summary>
		/// <param name="selector">The selector.</param>
		void Register ( /*Selector*/ Object selector );
		/// <summary>
		/// Listens for debugger.
		/// </summary>
		/// <param name="listenPort">The listen port.</param>
		void ListenForDebugger ( int listenPort );
		// TODO: JdwpPacket
		/// <summary>
		/// Sends the and consume.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <param name="replyHandler">The reply handler.</param>
		void SendAndConsume ( /*JdwpPacket*/ Object packet, ChunkHandler replyHandler );
		/// <summary>
		/// Adds the request identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="handler">The handler.</param>
		void AddRequestId ( int id, ChunkHandler handler );
		/// <summary>
		/// Removes the request identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		void RemoveRequestId ( int id );
		/// <summary>
		/// Determines whether [is response to us] [the specified identifier].
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		ChunkHandler IsResponseToUs ( int id );
		// TODO: JdwpPacket
		/// <summary>
		/// Packets the failed.
		/// </summary>
		/// <param name="packet">The packet.</param>
		void PacketFailed ( /*JdwpPacket*/ Object packet );
		/// <summary>
		/// DDMs the seen.
		/// </summary>
		/// <returns></returns>
		bool DdmSeen ( );
		/// <summary>
		/// Closes the specified notify.
		/// </summary>
		/// <param name="notify">if set to <c>true</c> [notify].</param>
		void Close ( bool notify );
		/// <summary>
		/// Updates the specified change mask.
		/// </summary>
		/// <param name="changeMask">The change mask.</param>
		void Update ( ClientChangeMask changeMask );
	}
}
