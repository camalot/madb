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
	public class Escape {
		[Fact]
		public void WhenPathIsNull_ShouldReturnThrowArgumentNullException () {
			Assert.Throws<ArgumentException>(() => {
				LinuxPath.Escape ( null );
			} );
		}

		[Fact]
		public void WhenPathIsEmpty_ShouldReturnThrowArgumentNullException ( ) {
			Assert.Throws<ArgumentException> ( ( ) => {
				LinuxPath.Escape ( string.Empty );
			} );
		}


		[Fact]
		public void WhenPathDoesNotContainCharactersToEscape_ShouldReturnPathUnchanged ( ) {
			var fixture = new Fixture ( );
			var p1 = fixture.Create ( "p1-" );
			var result = LinuxPath.Escape ( p1 );
			Assert.Equal ( p1, result );
		}

		[Fact]
		public void WhenPathContiansCharacterToEscape_ShouldReturnEscapedString ( ) {
			var chars = @"\()*+?""'#/".ToCharArray();
			var fixture = new Fixture ( );

			foreach ( var c in chars ) {
				var b = "before-";
				var a = "-after";
				var check = "{0}{1}{2}".With ( b, c, a);
				var result = LinuxPath.Escape ( check );
				Assert.Equal ( "{0}\\{1}{2}".With ( b, c, a ), result );
			}
		}
	}
}
