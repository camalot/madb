using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Camalot.Common.Extensions;
using MoreLinq;

namespace Managed.Adb {
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Managed.Adb.MultiLineReceiver" />
	internal sealed class LinkResoverReceiver : MultiLineReceiver {
		/// <summary>
		/// Initializes a new instance of the <see cref="LinkResoverReceiver"/> class.
		/// </summary>
		public LinkResoverReceiver ( ) {

		}

		/// <summary>
		/// Gets or sets the resolved path.
		/// </summary>
		/// <value>
		/// The resolved path.
		/// </value>
		public string ResolvedPath { get; set; }

		/// <summary>
		/// Processes the new lines.
		/// </summary>
		/// <param name="lines">The lines.</param>
		protected override void ProcessNewLines ( string[] lines ) {
			// all we care about is a line with '->'
			var regex = @"->\s+([^$]+)";
			foreach ( var line in lines.Where ( l => l.IsMatch(regex) ) ) {
				ResolvedPath = line.Match ( regex ).Groups[1].Value;
			}

		}
	}
}
