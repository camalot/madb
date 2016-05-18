using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Camalot.Common.Extensions;
using Managed.Adb.IO;
using Ploeh.AutoFixture;
using Xunit;

namespace Managed.Adb.Tests.ForIOLinuxPath {
	public class Quote {
		[Fact]
		public void WhenValueIsNull_ShouldReturnNull ( ) {
			var result = LinuxPath.Quote ( null );
			Assert.Null ( result );
		}

		[Fact] 
		public void WhenValueIsEmpty_ShouldReturnEmpty () {
			var result = LinuxPath.Quote ( string.Empty );
			Assert.Equal ( string.Empty, result );
		}
		[Fact] 
		public void WhenValueHasSpace_ShouldReturnQuotedValue() {
			var fixture = new Fixture ( );

			var val = fixture.Create ( "value " );
			var result = LinuxPath.Quote ( val );
			Assert.Equal ( "\"{0}\"".With ( val ), result );
		}

		[Fact]
		public void WhenValueDoesNotHaveSpace_ShouldReturnValue ( ) {
			var fixture = new Fixture ( );

			var val = fixture.Create ( "value-" );
			var result = LinuxPath.Quote ( val );
			Assert.Equal ( val , result );
		}
	}
}
