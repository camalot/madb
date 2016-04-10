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
	public class GetDirectoryNameTests {
		[Fact]
		public void WhenPathIsNull_ShouldReturnNull ( ) {
			var result = LinuxPath.GetDirectoryName ( null );
			Assert.Null ( result );
		}


		[Fact]
		public void WhenPathContainsInvalidCharacters_ShouldThrowException ( ) {
			int errorCount = 0;
			for ( var x = 0; x < LinuxPathConsts.InvalidPathChars.Length; ++x ) {
				try {
					string result = LinuxPath.GetDirectoryName ( "/my-invalid-path/f-{0}".With ( LinuxPathConsts.InvalidPathChars[x] ) );
				} catch ( ArgumentException ) {
					errorCount++;
				}
			}

			Assert.Equal ( LinuxPathConsts.InvalidPathChars.Length, errorCount );
		}

		[Fact]
		public void WhenPathLengthIs1_ShouldReturnRelativePath ( ) {
			string result = LinuxPath.GetDirectoryName ( "a" );
			Assert.Equal ( "./a/", result );
		}

		[Fact]
		public void WhenPathLengthIsRoot_ShouldReturnRootPath ( ) {
			string result = LinuxPath.GetDirectoryName ( "/" );
			Assert.Equal ( "/", result );
		}

		[Fact]
		public void WhenPathRelative_ShouldReturnRelativePath ( ) {
			var fixture = new Fixture ( );
			var p = fixture.Create ( "path" );
			string result = LinuxPath.GetDirectoryName ( p );
			Assert.Equal ( "./{0}/".With ( p ), result );
		}

		[Fact]
		public void WhenPathRelativeWithFileWithExtension_ShouldReturnDirectoryRelativePath ( ) {
			var fixture = new Fixture ( );
			var p = fixture.Create ( "path" );
			var pf = "{0}/file.ext".With ( p );
			string result = LinuxPath.GetDirectoryName ( pf );
			Assert.Equal ( "./{0}/".With ( p ), result );
		}


		[Fact]
		public void WhenPathRelativeWithFileWithoutExtension_ShouldReturnDirectoryRelativePath ( ) {
			var fixture = new Fixture ( );
			var p = fixture.Create ( "path" );
			var pf = "{0}/file".With ( p );
			string result = LinuxPath.GetDirectoryName ( pf );
			Assert.Equal ( "./{0}/".With ( p ), result );
		}

		[Fact]
		public void WhenPathRootedWithFileWithExtension_ShouldReturnDirectoryRootedPath ( ) {
			var fixture = new Fixture ( );
			var p = fixture.Create ( "path" );
			var pf = "/{0}/file.ext".With ( p );
			string result = LinuxPath.GetDirectoryName ( pf );
			Assert.Equal ( "/{0}/".With ( p ), result );
		}


		[Fact]
		public void WhenPathRootedWithFileWithoutExtension_ShouldReturnDirectoryRootedPath ( ) {
			var fixture = new Fixture ( );
			var p = fixture.Create ( "path" );
			var pf = "/{0}/file".With ( p );
			string result = LinuxPath.GetDirectoryName ( pf );
			Assert.Equal ( "/{0}/".With ( p ), result );
		}

		[Fact]
		public void WhenPathRooted_ShouldReturnRootPath ( ) {
			var fixture = new Fixture ( );
			var p = "/{0}/".With ( fixture.Create ( "path" ) );
			string result = LinuxPath.GetDirectoryName ( p );
			Assert.Equal ( p, result );
		}

		[Fact]
		public void WhenPathIsLevels_ShouldReturnRootPath ( ) {
			var fixture = new Fixture ( );
			var pname = fixture.Create ( "path" );
			var p = "/some/path/deep/{0}/".With ( pname );
			string result = LinuxPath.GetDirectoryName ( p );
			Assert.Equal ( "{0}".With ( p ), result );
		}
	}
}
