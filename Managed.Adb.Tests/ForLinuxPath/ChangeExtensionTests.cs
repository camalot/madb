using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Camalot.Common.Extensions;
using Managed.Adb.IO;
using Xunit;

namespace Managed.Adb.Tests.ForLinuxPath {
	public class ChangeExtensionTests {

		[Fact]
		public void WhenPathIsNull_ShouldReturnNull () {
			string result = LinuxPath.ChangeExtension ( null, "ext" );
			Assert.Null ( result );
		}

		/// <summary>
		/// Whens the path contains invalid character_ should throw argument exception.
		/// </summary>
		[Fact]
		public void WhenPathContainsInvalidCharacter_ShouldThrowArgumentException ( ) {
			int errorCount = 0;
			for ( var x = 0; x < LinuxPathConsts.InvalidChars.Length; ++x  ) {
				try {
					string result = LinuxPath.ChangeExtension ("/my-invalid-path/f-{0}".With ( LinuxPathConsts.InvalidChars[x]), "ext" );
				} catch (ArgumentException ) {
					errorCount++;
				}
			}

			Assert.Equal ( LinuxPathConsts.InvalidChars.Length, errorCount );
		}

		[Fact]
		public void WhenPathLengthIsZero_ShouldReturnEmpty () {
			string result = LinuxPath.ChangeExtension ( string.Empty, "ext" );
			Assert.Equal ( string.Empty, result );
		}

		[Fact]
		public void WhenExtensionIsEmpty_ShouldReturnPath ( ) {
			string result = LinuxPath.ChangeExtension ( "/my-path/to/file.ext1", string.Empty );
			Assert.Equal ( "/my-path/to/file.ext1", result );
		}

		[Fact]
		public void WhenExtensionDoesNotStartWithDot_ShouldReturnChangedExtensionWithDot () {
			string result = LinuxPath.ChangeExtension ( "/my-path/to/file.ext1", "ext2" );
			Assert.Equal ( "/my-path/to/file.ext2", result );
		}

		[Fact]
		public void WhenExtensionStartsWithDot_ShouldReturnChangedExtension ( ) {
			string result = LinuxPath.ChangeExtension ( "/my-path/to/file.ext1", ".ext2" );
			Assert.Equal ( "/my-path/to/file.ext2", result );
		}

		[Fact]
		public void WhenPathHasNoExtension_ShouldReturnPathWithExtension ( ) {
			string result = LinuxPath.ChangeExtension ( "/my-path/to/file", ".ext2" );
			Assert.Equal ( "/my-path/to/file.ext2", result );
		}
	}
}
