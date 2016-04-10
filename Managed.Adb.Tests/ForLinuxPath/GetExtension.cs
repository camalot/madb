using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Camalot.Common.Extensions;
using Managed.Adb.IO;
using Ploeh.AutoFixture;
using Xunit;

namespace Managed.Adb.Tests.ForLinuxPath {
	public class GetExtension {
		[Fact]
		public void WhenPathIsNull_ShouldReturnNull () {
			Assert.Null ( LinuxPath.GetExtension ( null ) );
		}

		[Fact]
		public void WhenPathContainsInvalidCharacter_ShouldThrowArgumentException () {
			var fixture = new Fixture ( );
			int errorCount = 0;
			for ( var x = 0; x < LinuxPathConsts.InvalidPathChars.Length; ++x ) {
				try {
					string result = LinuxPath.GetExtension ( "/some/path/{0}-{1}.ext".With ( LinuxPathConsts.InvalidPathChars[x], fixture.Create ( "file-" ) ) );
				} catch ( ArgumentException ) {
					errorCount++;
				}
			}

			Assert.Equal ( LinuxPathConsts.InvalidPathChars.Length, errorCount );
		}

		[Fact]
		public void WhenFilePathContainsExtension_ShouldReturnExtension () {
			var fixture = new Fixture ( );
			string result = LinuxPath.GetExtension ( "/some/path/{0}.ext".With (fixture.Create ( "file-" ) ) );
			Assert.Equal ( ".ext", result );
		}

		[Fact]
		public void WhenFilePathDoesNotContainsExtension_ShouldReturnEmpty ( ) {
			var fixture = new Fixture ( );
			string result = LinuxPath.GetExtension ( "/some/path/{0}".With ( fixture.Create ( "file-" ) ) );
			Assert.Equal ( string.Empty, result );
		}

		[Fact]
		public void WhenFilePathEndsWithDot_ShouldReturnEmpty ( ) {
			var fixture = new Fixture ( );
			string result = LinuxPath.GetExtension ( "/some/path/{0}.".With ( fixture.Create ( "file-" ) ) );
			Assert.Equal ( string.Empty, result );
		}

		[Fact]
		public void WhenPathDoesNotFile_ShouldReturnEmpty ( ) {
			var fixture = new Fixture ( );
			string result = LinuxPath.GetExtension ( "/some/path/{0}/".With ( fixture.Create ( "path-" ) ) );
			Assert.Equal ( string.Empty, result );
		}
	}
}
