using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managed.Adb.Exceptions {
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Managed.Adb.Exceptions.AdbException" />
	public class ShellCommandUnresponsiveException : AdbException {
		/// <summary>
		/// Initializes a new instance of the <see cref="ShellCommandUnresponsiveException"/> class.
		/// </summary>
		public ShellCommandUnresponsiveException ( ) : base("The shell command has become unresponsive"){

		}
	}
}
