using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Managed.Adb {
	/// <ignore>true</ignore>
	public static partial class ManagedAdbExtenstions {
		/// <summary>
		/// Gets EPOCH time
		/// </summary>
		public static readonly DateTime Epoch = new DateTime ( 1970, 1, 1 );

		/// <summary>
		/// Gets epoch time.
		/// </summary>
		/// <param name="dt">The dt.</param>
		/// <returns></returns>
		public static DateTime GetEpoch ( this DateTime dt ) {
			return Epoch;
		}

		/// <summary>
		/// Currents the time millis.
		/// </summary>
		/// <param name="dateTime">The date time.</param>
		/// <returns></returns>
		public static long CurrentTimeMillis ( this DateTime dateTime ) {
			return (long)( dateTime.ToUniversalTime() - Epoch ).TotalMilliseconds;
		}
	}
}
