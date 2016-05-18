using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managed.Adb {
	/// <ignore>true</ignore>
	public static partial class ManagedAdbExtenstions {

		/// <summary>
		/// Bits the shift right.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="bits">The bits.</param>
		/// <returns></returns>
		public static Int32 BitShiftRight( this Int32 value, int bits ) {
			return (Int32)( (UInt32)value >> bits );
		}

		/// <summary>
		/// Bits the shift right.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="bits">The bits.</param>
		/// <returns></returns>
		public static Int16 BitShiftRight( this Int16 value, int bits ) {
			return (Int16)( (UInt16)value >> bits );
		}

		/// <summary>
		/// Bits the shift right.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="bits">The bits.</param>
		/// <returns></returns>
		public static Int64 BitShiftRight( this Int64 value, int bits ) {
			return (Int64)( (UInt64)value >> bits );
		}

		/// <summary>
		/// Converts the value to enum
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static T ToEnum<T> ( this int value ) {
			return (T)Enum.ToObject(typeof(T), value);
		}

		/// <summary>
		/// Converts the value to enum
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static T ToEnum<T> ( this uint value ) {
			return (T)Enum.ToObject ( typeof ( T ), value );
		}

		/// <summary>
		/// Converts the value to enum
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static T ToEnum<T> ( this short value ) {
			return (T)Enum.ToObject ( typeof ( T ), value );
		}

		/// <summary>
		/// Converts the value to enum
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static T ToEnum<T> ( this ushort value ) {
			return (T)Enum.ToObject ( typeof ( T ), value );
		}

		/// <summary>
		/// Converts the value to enum
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static T ToEnum<T> ( this long value ) {
			return (T)Enum.ToObject ( typeof ( T ), value );
		}

		/// <summary>
		/// Converts the value to enum
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static T ToEnum<T> ( this ulong value ) {
			return (T)Enum.ToObject ( typeof ( T ), value );
		}

		/// <summary>
		/// Converts the value to enum
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static T ToEnum<T> ( this byte value ){
			return (T)Enum.ToObject ( typeof ( T ), value );
		}

		/// <summary>
		/// Converts the value to enum
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static T ToEnum<T> ( this sbyte value ){
			return (T)Enum.ToObject ( typeof ( T ), value );
		}

		/// <summary>
		/// Reverses the byte array.
		/// </summary>
		/// <param name="inArray">The in array.</param>
		/// <returns></returns>
		public static byte[] ReverseByteArray ( this byte[] inArray ) {
			byte temp;
			int highCtr = inArray.Length - 1;

			for ( int ctr = 0; ctr < inArray.Length / 2; ctr++ ) {
				temp = inArray[ctr];
				inArray[ctr] = inArray[highCtr];
				inArray[highCtr] = temp;
				highCtr -= 1;
			}
			return inArray;
		}
	}
}
