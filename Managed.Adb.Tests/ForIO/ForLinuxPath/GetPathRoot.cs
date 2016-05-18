using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Managed.Adb.IO;
using Ploeh.AutoFixture;
using Xunit;

namespace Managed.Adb.Tests.ForIOLinuxPath {
	public class GetPathRoot {
		[Fact]
		public void WhenPathIsNull_ShouldReturnNull () {
			var result = LinuxPath.GetPathRoot ( null );
			Assert.Null ( result );
		}

		[Fact]
		public void WhenPathEmpty_ShouldReturnRelativeCurrentPath ( ) {
			var result = LinuxPath.GetPathRoot ( string.Empty );
			Assert.Equal ( "./", result );
		}

		[Fact]
		public void WhenPathIsRooted_ShouldReturnRootPath () {
			var fixture = new Fixture ( );
			var path = fixture.Create ( "/path-" );
			var result = LinuxPath.GetPathRoot ( path );
			Assert.Equal ( "/", result );
		}

		[Fact]
		public void WhenPathIsRelative_ShouldReturnRelativeCurrentPath ( ) {
			var fixture = new Fixture ( );
			var path = fixture.Create ( "path-" );
			var result = LinuxPath.GetPathRoot ( path );
			Assert.Equal ( "./", result );
		}

	}


}
