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
	public class HasExtension {
		[Fact]
		public void WhenPathIsNull_ShouldReturnFalse () {
			var result = LinuxPath.HasExtension ( null );
			Assert.False ( result );
		}

		[Fact]
		public void WhenPathIsEmpty_ShouldReturnFalse ( ) {
			var result = LinuxPath.HasExtension ( string.Empty );
			Assert.False ( result );
		}

		[Fact]
		public void WhenPathDoesNotHaveFile_ShouldReturnFalse ( ) {
			var fixture = new Fixture ( );
			var path = "/path/to/file/";
			var result = LinuxPath.HasExtension ( path );
			Assert.False ( result );
		}

		[Fact]
		public void WhenPathHasExtension_ShouldReturnTrue ( ) {
			var fixture = new Fixture ( );
			var file = fixture.Create ( "file-" );
			var path = "/path/to/file/{0}.ext".With ( file );
			var result = LinuxPath.HasExtension ( path );
			Assert.True ( result );
		}

		[Fact]
		public void WhenPathDoesNotHaveExtension_ShouldReturnFalse ( ) {
			var fixture = new Fixture ( );
			var file = fixture.Create ( "file-" );
			var path = "/path/to/file/{0}".With ( file );
			var result = LinuxPath.HasExtension ( path );
			Assert.False ( result );
		}
	}
}
