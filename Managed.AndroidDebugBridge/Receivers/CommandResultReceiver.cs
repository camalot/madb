using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managed.Adb {
	/// <summary>
	/// 
	/// </summary>
	public class CommandResultReceiver : MultiLineReceiver{
		/// <summary>
		/// Processes the new lines.
		/// </summary>
		/// <param name="lines">The lines.</param>
		protected override void ProcessNewLines ( string[] lines ) {
			var result = new StringBuilder ( );
			foreach ( string line in lines ) {
				if ( string.IsNullOrWhiteSpace ( line ) || line.StartsWith ( "#" ) || line.StartsWith ( "$" ) ) {
					continue;
				}

				result.AppendLine ( line );
			}

			this.Result = result.ToString ( ).Trim ( );
		}

		/// <summary>
		/// Gets the result.
		/// </summary>
		/// <value>
		/// The result.
		/// </value>
		public string Result { get; private set; }
	}
}
