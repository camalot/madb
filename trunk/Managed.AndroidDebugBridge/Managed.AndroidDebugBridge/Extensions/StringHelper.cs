using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Camalot.Common.Extensions;

namespace Managed.Adb {
	/// <ignore>true</ignore>
	public static partial class ManagedAdbExtenstions {
		/// <summary>
		/// To the argument safe quoted string.
		/// </summary>
		/// <param name="s">The s.</param>
		/// <returns></returns>
		public static String ToArgumentSafe ( this String s ) {
			return "{0}{1}{0}".With ( s.Contains ( " " ) ? "\"" : String.Empty, s );
		}

		/// <summary>
		/// Gets the bytes from a string.
		/// </summary>
		/// <param name="str">The string.</param>
		/// <param name="encoding">The encoding.</param>
		/// <returns></returns>
		public static byte[] GetBytes ( this String str, String encoding ) {
			Encoding enc = Encoding.GetEncoding ( encoding );

			return enc.GetBytes ( str );
		}

		/// <summary>
		/// Gets the string from a byte array.
		/// </summary>
		/// <param name="bytes">The bytes.</param>
		/// <param name="encoding">The encoding.</param>
		/// <returns></returns>
		public static String GetString ( this byte[] bytes, String encoding ) {
			Encoding enc = Encoding.GetEncoding ( encoding );
			return bytes.GetString ( enc );
		}

		/// <summary>
		/// Gets the string from a byte array.
		/// </summary>
		/// <param name="bytes">The bytes.</param>
		/// <param name="index">The index.</param>
		/// <param name="count">The count.</param>
		/// <returns></returns>
		public static String GetString ( this byte[] bytes, int index, int count ) {
			return GetString ( bytes, index, count, Encoding.Default );
		}

		/// <summary>
		/// Gets the string from a byte array.
		/// </summary>
		/// <param name="bytes">The bytes.</param>
		/// <param name="index">The index.</param>
		/// <param name="count">The count.</param>
		/// <param name="encoding">The encoding.</param>
		/// <returns></returns>
		public static String GetString ( this byte[] bytes, int index, int count, Encoding encoding ) {
			return encoding.GetString ( bytes, index, count );
		}

		/// <summary>
		/// Gets the string from a byte array.
		/// </summary>
		/// <param name="bytes">The bytes.</param>
		/// <param name="index">The index.</param>
		/// <param name="count">The count.</param>
		/// <param name="encoding">The encoding.</param>
		/// <returns></returns>
		public static String GetString ( this byte[] bytes, int index, int count, String encoding ) {
			Encoding enc = Encoding.GetEncoding ( encoding );
			return GetString ( bytes, index, count, enc );
		}


	}
}
