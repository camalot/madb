using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managed.Adb.Logs {
	/// <summary>
	/// 
	/// </summary>
	public sealed class LogEntry {
		/// <summary>
		/// Gets or sets the length.
		/// </summary>
		/// <value>
		/// The length.
		/// </value>
		public int Length { get; set; }
		/// <summary>
		/// Gets or sets the process identifier.
		/// </summary>
		/// <value>
		/// The process identifier.
		/// </value>
		public int ProcessId { get; set; }
		/// <summary>
		/// Gets or sets the thread identifier.
		/// </summary>
		/// <value>
		/// The thread identifier.
		/// </value>
		public int ThreadId { get; set; }
		/// <summary>
		/// Gets or sets the time stamp.
		/// </summary>
		/// <value>
		/// The time stamp.
		/// </value>
		public DateTime TimeStamp { get; set; }
		/// <summary>
		/// Gets or sets the nano seconds.
		/// </summary>
		/// <value>
		/// The nano seconds.
		/// </value>
		public int NanoSeconds { get; set; }
		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		/// <value>
		/// The data.
		/// </value>
		public byte[] Data { get; set; }
	}
}
