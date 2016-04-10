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
	public class GetPathWithoutFile {
		[Fact]
		public void WhenPathIsNull_ShouldReturnNull() {
			Assert.Null ( LinuxPath.GetPathWithoutFile ( null ) );
		}

		[Fact]
		public void WhenPathContainsFile_ShouldReturnPath ( ) {
			var fixture = new Fixture ( );
			var p1 = fixture.Create ( "path1-" );
			var p2 = fixture.Create ( "path2-" );
			var f1 = fixture.Create ( "file-" );
			var p = "/{0}/{1}/{2}".With ( p1, p2, f1 );
			var result = LinuxPath.GetPathWithoutFile ( p );
			Assert.Equal ( "/{0}/{1}/".With ( p1, p2 ), result );
		}
	}
}
