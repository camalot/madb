using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Camalot.Common.Extensions;

namespace Managed.Adb.IO {
	/// <summary>
	/// Just like System.IO.Path, except it is geared for Linux
	/// </summary>
	public static class LinuxPath {

		/// <summary>
		/// Pattern to escape filenames for shell command consumption.
		/// </summary>
		private const string ESCAPEPATTERN = @"([\\\(\)*+?""'\#/])";

		/// <summary>
		/// The directory separator character
		/// </summary>
		public static readonly char DirectorySeparatorChar = '/';
		/// <summary>
		/// The directory separator alternate character
		/// </summary>
		public static readonly char AltDirectorySeparatorChar = '/';
		/// <summary>
		/// Invalid characters for a file name
		/// </summary>
		private static readonly char[] InvalidFileNameChars = new char[] {
						'"', '<', '>', '|', '\0', '\x0001', '\x0002', '\x0003', '\x0004', '\x0005', '\x0006', '\a', '\b', '\t', '\n', '\v',
						'\f', '\r', '\x000e', '\x000f', '\x0010', '\x0011', '\x0012', '\x0013', '\x0014', '\x0015', '\x0016', '\x0017', '\x0018', '\x0019', '\x001a', '\x001b',
						'\x001c', '\x001d', '\x001e', '\x001f', '*', '?', '\\', '/'
				 };
		/// <summary>
		/// The max length of a directory path
		/// </summary>
		internal const int MAX_DIRECTORY_PATH = 0xf8;
		/// <summary>
		/// the max length of a file path
		/// </summary>
		internal const int MAX_PATH = 260;
		/// <summary>
		/// the max length of a file path
		/// </summary>
		internal static readonly int MaxPath = 260;
		/// <summary>
		/// The character used to separate multiple paths
		/// </summary>
		public static readonly char PathSeparator = ';';
		/// <summary>
		/// Invalid characters for a file/directory path
		/// </summary>
		private static readonly char[] RealInvalidPathChars = new char[] {
						'"', '<', '>', '|', '\0', '\x0001', '\x0002', '\x0003', '\x0004', '\x0005', '\x0006', '\a', '\b', '\t', '\n', '\v',
						'\f', '\r', '\x000e', '\x000f', '\x0010', '\x0011', '\x0012', '\x0013', '\x0014', '\x0015', '\x0016', '\x0017', '\x0018', '\x0019', '\x001a', '\x001b',
						'\x001c', '\x001d', '\x001e', '\x001f'
				 };

		/// <summary>Changes the extension of a path string.</summary>
		/// <returns>A string containing the modified path information.On Windows-based desktop platforms, if path is null or an empty string (""), the path information is returned unmodified. If extension is null, the returned string contains the specified path with its extension removed. If path has no extension, and extension is not null, the returned path string contains extension appended to the end of path.</returns>
		/// <param name="extension">The new extension (with a leading period). Specify null to remove an existing extension from path. </param>
		/// <param name="path">The path information to modify. The path cannot contain any of the characters defined in <see cref="F:System.IO.Path.InvalidPathChars"></see>. </param>
		/// <exception cref="T:System.ArgumentException">The path contains one or more of the invalid characters defined in <see cref="F:System.IO.Path.InvalidPathChars"></see>, or contains a wildcard character. </exception>
		/// <filterpriority>1</filterpriority>
		public static string ChangeExtension ( string path, string extension ) {
			if ( path == null ) {
				return null;
			}
			CheckInvalidPathChars ( path );
			string str = path;
			int length = path.Length;
			while ( --length >= 0 ) {
				char ch = path[length];
				if ( ch == '.' ) {
					str = path.Substring ( 0, length );
					break;
				}
				if ( IsDirectorySeparator(ch) ) {
					break;
				}
			}
			if ( string.IsNullOrWhiteSpace ( extension ) || path.Length == 0 ) {
				return path;
			}
			if ( extension.Length > 0 && extension[0] != '.' ) {
				str = str + ".";
			}

			return ( str + extension );
		}

		/// <summary>
		/// Checks the invalid path chars.
		/// </summary>
		/// <param name="path">The path.</param>
		internal static void CheckInvalidPathChars ( string path ) {
			for ( int i = 0; i < path.Length; i++ ) {
				int num2 = path[i];
				if ( ( ( num2 == 0x22 ) || ( num2 == 60 ) ) || ( ( ( num2 == 0x3e ) || ( num2 == 0x7c ) ) || ( num2 < 0x20 ) ) ) {
					throw new ArgumentException ( "Path contains invalid characters" );
				}
			}
		}

		/// <summary>
		/// Combine the specified paths to form one path
		/// </summary>
		/// <param name="paths">The paths.</param>
		/// <returns></returns>
		public static string Combine ( params string[] paths ) {
			if ( paths == null ) {
				throw new ArgumentNullException ( "paths" );
			}
			int capacity = 0;
			int num2 = 0;
			for ( int i = 0; i < paths.Length; i++ ) {
				if ( paths[i] == null ) {
					throw new ArgumentNullException ( "paths" );
				}
				if ( paths[i].Length != 0 ) {
					CheckInvalidPathChars ( paths[i] );
					if ( IsPathRooted ( paths[i] ) ) {
						num2 = i;
						capacity = paths[i].Length;
					} else {
						capacity += paths[i].Length;
					}
					char ch = paths[i][paths[i].Length - 1];
					if ( !IsDirectorySeparator(ch) ) {
						capacity++;
					}
				}
			}
			var builder = new StringBuilder ( capacity );
			for ( int j = num2; j < paths.Length; j++ ) {
				if ( paths[j].Length != 0 ) {
					if ( builder.Length == 0 ) {
						builder.Append ( FixupPath ( paths[j] ) );
					} else {
						char ch2 = builder[builder.Length - 1];
						if ( !IsDirectorySeparator(ch2) ) {
							builder.Append ( DirectorySeparatorChar );
						}
						builder.Append ( paths[j] );
					}
				}
			}
			return builder.ToString ( );

		}


		/// <summary>Returns the directory information for the specified path string.</summary>
		/// <returns>A <see cref="T:System.String"></see> containing directory information for path, or null if path denotes a root directory, is the empty string (""), or is null. Returns <see cref="F:System.String.Empty"></see> if path does not contain directory information.</returns>
		/// <param name="path">The path of a file or directory. </param>
		/// <exception cref="T:System.ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces, or contains a wildcard character. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The path parameter is longer than the system-defined maximum length.</exception>
		/// <filterpriority>1</filterpriority>
		public static string GetDirectoryName ( string path ) {
			if ( path != null ) {
				CheckInvalidPathChars ( path );
				var tpath = path;
				if ( EndsWithDirectorySeparator(tpath) ) {
					return FixupPath ( tpath );
				} else {
					var end = tpath.LastIndexOfAny ( new char[] { DirectorySeparatorChar, AltDirectorySeparatorChar } );
					end = end < 0 ? tpath.Length : end;
					return FixupPath ( tpath.Substring ( 0, end ) );
				}
			}
			return null;
		}

		/// <summary>
		/// Gets the path without file name.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		public static string GetPathWithoutFile ( string path ) {
			if ( path != null ) {
				CheckInvalidPathChars ( path );
				string tfilename = GetFileName ( path );
				string tpath = path.Substring ( 0, path.Length - tfilename.Length );
				tpath = FixupPath ( tpath );
				return tpath.Substring ( 0, tpath.LastIndexOfAny ( new char[] { DirectorySeparatorChar, AltDirectorySeparatorChar } ) + 1 );
			}
			return null;
		}

		/// <summary>
		/// Fixups the path.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		private static string FixupPath ( string path ) {
			string sb = path;
			sb = sb.Replace ( System.IO.Path.DirectorySeparatorChar, DirectorySeparatorChar );


			if ( !StartsWithDirectorySeparator(sb) ) {
				sb = string.Format ( ".{0}{1}", DirectorySeparatorChar, sb );
			}

			if ( !EndsWithDirectorySeparator(sb) ) {
				sb = string.Format ( "{0}{1}", sb, DirectorySeparatorChar );
			}

			sb = sb.Replace ( "//", new byte[] { (byte)DirectorySeparatorChar }.GetString ( ) );

			return sb;
		}

		/// <summary>Returns the extension of the specified path string.</summary>
		/// <returns>A <see cref="T:System.String"></see> containing the extension of the specified path (including the "."), null, or <see cref="F:System.String.Empty"></see>. If path is null, GetExtension returns null. If path does not have extension information, GetExtension returns Empty.</returns>
		/// <param name="path">The path string from which to get the extension. </param>
		/// <exception cref="T:System.ArgumentException">path contains one or more of the invalid characters defined in <see cref="F:System.IO.Path.InvalidPathChars"></see>, or contains a wildcard character. </exception>
		/// <filterpriority>1</filterpriority>
		public static string GetExtension ( string path ) {
			if ( path == null ) {
				return null;
			}
			CheckInvalidPathChars ( path );
			int length = path.Length;
			int startIndex = length;
			while ( --startIndex >= 0 ) {
				char ch = path[startIndex];
				if ( ch == '.' ) {
					if ( startIndex != ( length - 1 ) ) {
						return path.Substring ( startIndex, length - startIndex );
					}
					return string.Empty;
				}
				if ( IsDirectorySeparator ( ch ) ) {
					break;
				}
			}
			return string.Empty;
		}

		/// <summary>Returns the file name and extension of the specified path string.</summary>
		/// <returns>A <see cref="T:System.String"></see> consisting of the characters after the last directory character in path. If the last character of path is a directory or volume separator character, this method returns <see cref="F:System.String.Empty"></see>. If path is null, this method returns null.</returns>
		/// <param name="path">The path string from which to obtain the file name and extension. </param>
		/// <exception cref="T:System.ArgumentException">path contains one or more of the invalid characters defined in <see cref="F:System.IO.Path.InvalidPathChars"></see>, or contains a wildcard character. </exception>
		/// <filterpriority>1</filterpriority>
		public static string GetFileName ( string path ) {
			if ( path != null ) {
				CheckInvalidPathChars ( path );
				int length = path.Length;
				int num2 = length;
				while ( --num2 >= 0 ) {
					char ch = path[num2];
					if ( IsDirectorySeparator ( ch ) ) {
						return path.Substring ( num2 + 1, ( length - num2 ) - 1 );
					}
				}
			}
			return path;
		}

		/// <summary>Returns the file name of the specified path string without the extension.</summary>
		/// <returns>A <see cref="T:System.String"></see> containing the string returned by <see cref="M:System.IO.Path.GetFileName(System.String)"></see>, minus the last period (.) and all characters following it.</returns>
		/// <param name="path">The path of the file. </param>
		/// <exception cref="T:System.ArgumentException">path contains one or more of the invalid characters defined in <see cref="F:System.IO.Path.InvalidPathChars"></see>, or contains a wildcard character. </exception>
		/// <filterpriority>1</filterpriority>
		public static string GetFileNameWithoutExtension ( string path ) {
			path = GetFileName ( path );
			if ( path == null ) {
				return null;
			}
			int length = path.LastIndexOf ( '.' );
			if ( length == -1 ) {
				return path;
			}
			return path.Substring ( 0, length );
		}


		/// <summary>Gets an array containing the characters that are not allowed in file names.</summary>
		/// <returns>An array containing the characters that are not allowed in file names.</returns>
		public static char[] GetInvalidFileNameChars ( ) {
			return (char[])InvalidFileNameChars.Clone ( );
		}

		/// <summary>Gets an array containing the characters that are not allowed in path names.</summary>
		/// <returns>An array containing the characters that are not allowed in path names.</returns>
		public static char[] GetInvalidPathChars ( ) {
			return (char[])RealInvalidPathChars.Clone ( );
		}

		/// <summary>Gets the root directory information of the specified path.</summary>
		/// <returns>A string containing the root directory of path, such as "C:\", or null if path is null, or an empty string if path does not contain root directory information.</returns>
		/// <param name="path">The path from which to obtain root directory information. </param>
		/// <exception cref="T:System.ArgumentException">path contains one or more of the invalid characters defined in <see cref="F:System.IO.Path.InvalidPathChars"></see>, or contains a wildcard character.-or- <see cref="F:System.String.Empty"></see> was passed to path. </exception>
		/// <filterpriority>1</filterpriority>
		public static string GetPathRoot ( string path ) {
			if ( path == null ) {
				return null;
			}
			path = FixupPath ( path );
			return path.Substring ( 0, GetRootLength ( path ) );
		}

		/// <summary>
		/// Gets the length of the root.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		internal static int GetRootLength ( string path ) {
			CheckInvalidPathChars ( path );
			int num = 0;
			int length = path.Length;
			if ( ( length >= 1 ) && IsDirectorySeparator ( path[0] ) ) {
				num = 1;
				if ( ( length >= 2 ) && IsDirectorySeparator ( path[1] ) ) {
					num = 2;
					int num3 = 2;
					while ( ( num < length ) && !IsDirectorySeparator(path[num] ) || ( --num3 > 0 ) ) {
						num++;
					}
				}
				return num;
			}
			if ( ( length >= 2 ) ) {
				num = 2;
				if ( ( length >= 3 ) && IsDirectorySeparator ( path[2] ) ) {
					num++;
				}
			}
			return num;
		}

		/// <summary>Determines whether a path includes a file name extension.</summary>
		/// <returns>true if the characters that follow the last directory separator (\\ or /) or volume separator (:) in the path include a period (.) followed by one or more characters; otherwise, false.</returns>
		/// <param name="path">The path to search for an extension. </param>
		/// <exception cref="T:System.ArgumentException">path contains one or more of the invalid characters defined in <see cref="F:System.IO.Path.InvalidPathChars"></see>, or contains a wildcard character. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool HasExtension ( string path ) {
			if ( path != null ) {
				CheckInvalidPathChars ( path );
				if ( EndsWithDirectorySeparator ( path ) ) {
					return false;
				}

				int length = path.Length;
				while ( --length >= 0 ) {
					char ch = path[length];
					if ( ch == '.' ) {
						return ( length != ( path.Length - 1 ) );
					}
					if ( IsDirectorySeparator ( ch ) ) {
						break;
					}
				}
			}
			return false;
		}

		internal static bool IsDirectorySeparator ( char c ) {
			if ( c != DirectorySeparatorChar ) {
				return ( c == AltDirectorySeparatorChar );
			}
			return true;
		}

		internal static bool EndsWithDirectorySeparator ( string path ) {
			return path.EndsWith ( new string ( new char[] { DirectorySeparatorChar } ) ) || path.EndsWith ( new string ( new char[] { AltDirectorySeparatorChar } ) );
		}

		internal static bool StartsWithDirectorySeparator ( string path ) {
			return path.StartsWith ( new string ( new char[] { DirectorySeparatorChar } ) ) || path.StartsWith ( new string ( new char[] { AltDirectorySeparatorChar } ) );
		}

		/// <summary>Gets a value indicating whether the specified path string contains absolute or relative path information.</summary>
		/// <returns>true if path contains an absolute path; otherwise, false.</returns>
		/// <param name="path">The path to test. </param>
		/// <exception cref="T:System.ArgumentException">path contains one or more of the invalid characters defined in <see cref="F:System.IO.Path.InvalidPathChars"></see>, or contains a wildcard character. </exception>
		/// <filterpriority>1</filterpriority>
		public static bool IsPathRooted ( string path ) {
			if ( path != null ) {
				CheckInvalidPathChars ( path );
				int length = path.Length;
				if ( ( length >= 1 && IsDirectorySeparator ( path[0] ) ) ||
					( length == 1 ) ) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Returns an escaped version of the entry name.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">When path is null.</exception>
		public static string Escape ( string path ) {
			return path.Require ( ).REReplace(ESCAPEPATTERN, new MatchEvaluator ( delegate ( Match m) {
				return m.Result ( "\\$1" );
			} ) );
		}

		/// <summary>
		/// Quotes the specified path.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		public static string Quote ( string path ) {
			if ( string.IsNullOrEmpty(path) ) {
				return path;
			}

			if(path.IsMatch(@"\s")) {
				return "\"{0}\"".With ( path );
			} else {
				return path;
			}
		}


	}
}
