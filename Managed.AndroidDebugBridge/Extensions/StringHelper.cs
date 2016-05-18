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
		/// Gets the bytes from a string.
		/// </summary>
		/// <param name="str">The string.</param>
		/// <param name="encoding">The encoding.</param>
		/// <returns></returns>
		public static byte[] GetBytes ( this string str, string encoding ) {
			Encoding enc = Encoding.GetEncoding ( encoding );

			return enc.GetBytes ( str );
		}
		/// <summary>
		/// Gets the string from a byte array.
		/// </summary>
		/// <param name="bytes">The bytes.</param>
		/// <returns></returns>
		public static string GetString ( this byte[] bytes ) {
			return GetString ( bytes, Encoding.Default );
		}
		/// <summary>
		/// Gets the string from a byte array.
		/// </summary>
		/// <param name="bytes">The bytes.</param>
		/// <param name="encoding">The encoding.</param>
		/// <returns></returns>
		public static string GetString ( this byte[] bytes, Encoding encoding ) {
			return encoding.GetString ( bytes, 0, bytes.Length );
		}
		/// <summary>
		/// Gets the string from a byte array.
		/// </summary>
		/// <param name="bytes">The bytes.</param>
		/// <param name="encoding">The encoding.</param>
		/// <returns></returns>
		public static string GetString ( this byte[] bytes, string encoding ) {
			Encoding enc = Encoding.GetEncoding ( encoding );
			return GetString ( bytes, enc );
		}
		/// <summary>
		/// Gets the string from a byte array.
		/// </summary>
		/// <param name="bytes">The bytes.</param>
		/// <param name="index">The index.</param>
		/// <param name="count">The count.</param>
		/// <returns></returns>
		public static string GetString ( this byte[] bytes, int index, int count ) {
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
		public static string GetString ( this byte[] bytes, int index, int count, Encoding encoding ) {
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
		public static string GetString ( this byte[] bytes, int index, int count, string encoding ) {
			Encoding enc = Encoding.GetEncoding ( encoding );
			return GetString ( bytes, index, count, enc );
		}


	}
}
