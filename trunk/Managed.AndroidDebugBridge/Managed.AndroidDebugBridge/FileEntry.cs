﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Managed.Adb.IO;
using MoreLinq;

namespace Managed.Adb {
	/// <summary>
	/// Represents a file entry on a device
	/// </summary>
	public class FileEntry {

		/// <summary>
		/// Finds the file entry, or creates an empty FileEntry if it does not exist.
		/// </summary>
		/// <param name="device">The device.</param>
		/// <param name="path">The path.</param>
		/// <remarks>This does not create the FileEntry on disk. It only creates the FileEntry object.</remarks>
		/// <returns></returns>
		public static FileEntry FindOrCreate ( Device device, String path ) {
			device.ThrowIfNull ( "device" );
			path.ThrowIfNullOrEmpty ( "path" );

			if ( !device.IsOffline ) {
				try {
					return device.FileListingService.FindFileEntry ( path );
				} catch ( FileNotFoundException ) {
					var fe = new FileEntry ( device, path );
					fe.Create ( );
					return fe;
				}
			} else {
				throw new IOException ( "Device is not online" );
			}
		}

		public static FileEntry CreateNoPermissions ( Device device, String path ) {
			var fe = new FileEntry ( device, path );
			fe.Permissions = new FilePermissions ( );
			return fe;
		}

		/// <summary>
		/// Gets a file entry from the specified path on the device.
		/// </summary>
		/// <param name="device">The device to check</param>
		/// <param name="path">the path to check</param>
		/// <exception cref="IOException">If the device is not connected.</exception>
		/// <exception cref="ArgumentNullException">If the device or path is null.</exception>
		/// <exception cref="FileNotFoundException">If the entrty is not found.</exception>
		/// <returns></returns>
		public static FileEntry Find ( Device device, String path ) {
			device.ThrowIfNull ( "device" );
			path.ThrowIfNullOrEmpty ( "path" );

			if ( !device.IsOffline ) {
				return device.FileListingService.FindFileEntry ( path );
			} else {
				throw new IOException ( "Device is not online" );
			}
		}



		/// <summary>
		/// Creates a new file entry.
		/// </summary>
		/// <param name="device">The device.</param>
		/// <param name="parent">parent entry or null if entry is root</param>
		/// <param name="name">name of the entry.</param>
		/// <param name="type">entry type.</param>
		/// <param name="isRoot">if set to <c>true</c> [is root].</param>
		internal FileEntry ( Device device, FileEntry parent, String name, FileListingService.FileTypes type, bool isRoot ) {
			this.FetchTime = 0;
			this.Parent = parent;
			this.Name = name;
			this.Type = type;
			this.IsRoot = isRoot;
			Children = new List<FileEntry> ( );
			CheckAppPackageStatus ( );
			this.Exists = true;
			this.Device = device;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FileEntry"/> class.
		/// </summary>
		/// <param name="device">The device.</param>
		/// <param name="path">The path.</param>
		internal FileEntry ( Device device, String path ) {
			this.FetchTime = 0;
			this.Parent = null;
			bool isDir = path.EndsWith ( new String ( LinuxPath.DirectorySeparatorChar, 1 ) );
			this.Name = isDir ? LinuxPath.GetDirectoryName ( path ) : LinuxPath.GetFileName ( path );
			this.IsRoot = path.Length == 1 && path[0] == LinuxPath.DirectorySeparatorChar;
			this.Type = isDir ? FileListingService.FileTypes.Directory : FileListingService.FileTypes.File;
			this.Size = 0;
			this.Children = new List<FileEntry> ( );
			this.Exists = false;
			this.Device = device;
		}


		/// <summary>
		/// Gets the device associated with this file entry.
		/// </summary>
		public Device Device { get; private set; }
		/// <summary>
		/// Gets the parent.
		/// </summary>
		public FileEntry Parent { get; private set; }
		/// <summary>
		/// Gets the name.
		/// </summary>
		public String Name { get; private set; }
		/// <summary>
		/// Gets or sets the name of the link.
		/// </summary>
		/// <value>
		/// The name of the link.
		/// </value>
		public String LinkName { get; set; }
		/// <summary>
		/// Gets or sets the info.
		/// </summary>
		/// <value>
		/// The info.
		/// </value>
		public String Info { get; set; }
		/// <summary>
		/// Gets or sets the permissions.
		/// </summary>
		/// <value>
		/// The permissions.
		/// </value>
		public FilePermissions Permissions { get; set; }
		/// <summary>
		/// Gets or sets the size.
		/// </summary>
		/// <value>
		/// The size.
		/// </value>
		public long Size { get; set; }
		/// <summary>
		/// Gets or sets the date.
		/// </summary>
		/// <value>
		/// The date.
		/// </value>
		public DateTime Date { get; set; }
		/// <summary>
		/// Gets or sets the owner.
		/// </summary>
		/// <value>
		/// The owner.
		/// </value>
		public String Owner { get; set; }
		/// <summary>
		/// Gets or sets the group.
		/// </summary>
		/// <value>
		/// The group.
		/// </value>
		public String Group { get; set; }
		/// <summary>
		/// Gets the type.
		/// </summary>
		public FileListingService.FileTypes Type { get; private set; }
		/// <summary>
		/// Gets a value indicating whether this instance is application package.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is application package; otherwise, <c>false</c>.
		/// </value>
		public bool IsApplicationPackage { get; private set; }
		/// <summary>
		/// Gets a value indicating whether this instance is root.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is root; otherwise, <c>false</c>.
		/// </value>
		public bool IsRoot { get; private set; }
		/// <summary>
		/// Gets a value indicating whether this instance is executable.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is executable; otherwise, <c>false</c>.
		/// </value>
		public bool IsExecutable { get; internal set; }
		/// <summary>
		/// Gets or sets the children.
		/// </summary>
		/// <remarks>Note that this is not populated by default. Use the FileListingService.GetChildren to populate it.</remarks>
		/// <value>
		/// The children.
		/// </value>
		public List<FileEntry> Children { get; set; }
		/// <summary>
		/// Gets a value indicating whether this instance is link.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is link; otherwise, <c>false</c>.
		/// </value>
		public bool IsLink {
			get {
				return Type == FileListingService.FileTypes.Link || Type == FileListingService.FileTypes.DirectoryLink;
			}
		}



		/// <summary>
		/// Gets a value indicating whether this <see cref="FileEntry"/> is exists.
		/// </summary>
		/// <value>
		///   <c>true</c> if exists; otherwise, <c>false</c>.
		/// </value>
		public bool Exists { get; private set; }


		/// <summary>
		/// Indicates whether the entry content has been fetched yet, or not.
		/// </summary>
		public long FetchTime { get; set; }

		internal bool NeedFetch {
			get {
				if ( FetchTime == 0 ) {
					return true;
				}

				long current = DateTime.Now.CurrentTimeMillis ( );
				if ( current - FetchTime > FileListingService.REFRESH_TEST ) {
					return true;
				}

				return false;
			}
		}

		/// <summary>
		/// Creates the file entry, if it does not exist.
		/// </summary>
		/// <returns></returns>
		public bool Create ( ) {
			try {
				if ( this.Exists ) {
					return true;
				}
				var fe = this.Device.FileSystem.Create ( this );
				return fe.Exists;
			} catch {
				return false;
			}
		}

		/// <summary>
		/// Deletes the file entry from the file system.
		/// </summary>
		/// <returns></returns>
		public bool Delete ( ) {
			try {
				this.Device.FileSystem.Delete ( this.FullEscapedPath );
				return true;
			} catch {
				return false;
			}
		}

		/// <summary>
		/// Adds a child file entry
		/// </summary>
		/// <param name="child">The child file entry</param>
		public void AddChild ( FileEntry child ) {
			Children.Add ( child );
		}


		/// <summary>
		/// Returns the child FileEntry matching the name.
		/// This uses the cached children list.
		/// </summary>
		/// <param name="name">the name of the child to return.</param>
		/// <return>the FileEntry matching the name or null.</return>
		public FileEntry FindChild ( String name ) {
			var entry = Children.Where ( e => String.Compare ( e.Name, name, false ) == 0 ).FirstOrDefault ( );

			return entry;
		}


		/// <summary>
		/// Gets a value indicating whether this instance is directory.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is directory; otherwise, <c>false</c>.
		/// </value>
		public bool IsDirectory {
			get {
				return this.Type == FileListingService.FileTypes.Directory || Type == FileListingService.FileTypes.DirectoryLink;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is application file name.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is application file name; otherwise, <c>false</c>.
		/// </value>
		public bool IsApplicationFileName {
			get {
				Regex regex = new Regex ( FileListingService.APK_FILE_PATTERN, RegexOptions.Compiled );
				return regex.IsMatch ( this.Name );
			}
		}

		/// <summary>
		/// Gets the full path of the entry.
		/// </summary>
		public String FullPath {
			get {
				if ( IsRoot ) {
					return FileListingService.FILE_ROOT;
				}

				StringBuilder pathBuilder = new StringBuilder ( );
				FillPathBuilder ( pathBuilder, false );

				if ( IsDirectory && pathBuilder[pathBuilder.Length - 1] != LinuxPath.DirectorySeparatorChar ) {
					pathBuilder.Append ( LinuxPath.DirectorySeparatorChar );
				}

				return pathBuilder.ToString ( );
			}
		}

		/// <summary>
		/// Gets the full resolved path.
		/// </summary>
		public String FullResolvedPath {
			get {
				if ( IsRoot ) {
					return FileListingService.FILE_ROOT;
				}
				return Device.FileSystem.ResolveLink ( this.FullPath );
			}
		}

		/// <summary>
		/// Gets the fully escaped path of the entry. This path is safe to use in a shell command line.
		/// </summary>
		public String FullEscapedPath {
			get {
				StringBuilder pathBuilder = new StringBuilder ( );
				FillPathBuilder ( pathBuilder, true, true );

				return pathBuilder.ToString ( );
			}
		}

		/// <summary>
		/// Gets the path as a list of segments.
		/// </summary>
		public String[] PathSegments {
			get {
				var list = new List<String> ( );
				FillPathSegments ( list );

				return list.ToArray ( );
			}
		}


		/// <summary>
		/// Sets the internal app package status flag. This checks whether the entry is in an app
		/// directory like /data/app or /system/app or /sd/app or /sd-ext/app. The last two are common for
		/// apps2sd on rooted phones
		/// </summary>
		private void CheckAppPackageStatus ( ) {
			IsApplicationPackage = false;

			String[] segments = PathSegments;
			if ( this.Type == FileListingService.FileTypes.File && segments.Length == 3 && IsApplicationFileName ) {
				IsApplicationPackage = String.Compare ( FileListingService.DIRECTORY_APP, segments[1], false ) == 0 &&
						( String.Compare ( FileListingService.DIRECTORY_SYSTEM, segments[0], false ) == 0 ||
						String.Compare ( FileListingService.DIRECTORY_DATA, segments[0], false ) == 0 ||
						String.Compare ( FileListingService.DIRECTORY_SD, segments[0], false ) == 0 ||
						String.Compare ( FileListingService.DIRECTORY_SDEXT, segments[0], false ) == 0 );
			}
		}

		/// <summary>
		/// Recursively fills the segment list with the full path.
		/// </summary>
		/// <param name="list">The list of segments to fill.</param>
		protected void FillPathSegments ( List<String> list ) {
			if ( IsRoot ) {
				return;
			}

			if ( Parent != null ) {
				Parent.FillPathSegments ( list );
			}

			list.Add ( Name );
		}

		/// <summary>
		/// Recursively fills the pathBuilder with the full path
		/// </summary>
		/// <param name="pathBuilder">a StringBuilder used to create the path.</param>
		/// <param name="escapePath">Whether the path need to be escaped for consumption by a shell command line.</param>
		/// <param name="resolveLinks">if set to <c>true</c> [resolve links].</param>
		protected void FillPathBuilder ( StringBuilder pathBuilder, bool escapePath, bool resolveLinks ) {
			if ( IsRoot ) {
				return;
			}

			if ( Parent != null ) {
				Parent.FillPathBuilder ( pathBuilder, escapePath, resolveLinks );
			}

			String n = resolveLinks && !String.IsNullOrEmpty ( LinkName ) ? LinkName : Name;

			if ( n[0] != LinuxPath.DirectorySeparatorChar ) {
				pathBuilder.Append ( LinuxPath.DirectorySeparatorChar );
			}

			pathBuilder.Append ( escapePath ? LinuxPath.Quote ( n ) : n );
		}

		/// <summary>
		/// Fills the path builder.
		/// </summary>
		/// <param name="pathBuilder">The path builder.</param>
		/// <param name="escapePath">if set to <c>true</c> [escape path].</param>
		protected void FillPathBuilder ( StringBuilder pathBuilder, bool escapePath ) {
			FillPathBuilder ( pathBuilder, escapePath, false );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString ( ) {
			return this.FullResolvedPath;
		}

		/// <summary>
		/// 
		/// </summary>
		public class FileEntryComparer : Comparer<FileEntry> {

			/// <summary>
			/// Compares the specified x.
			/// </summary>
			/// <param name="x">The x.</param>
			/// <param name="y">The y.</param>
			/// <returns></returns>
			public override int Compare ( FileEntry x, FileEntry y ) {
				return x.Name.CompareTo ( y.Name );
			}
		}
	}
}
