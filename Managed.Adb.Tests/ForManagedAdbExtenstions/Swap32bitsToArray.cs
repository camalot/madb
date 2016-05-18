using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Xunit;

namespace Managed.Adb.Tests.ForManagedAdbExtenstions {
	public class Swap32bitsToArray {
		[Fact]
		public void WhenValueIsPositive_ShouldReturnSwapped() {
			var fixture = new Fixture ( );
			var value = fixture.Create<int> ( );
			var offset = fixture.Create<int> ( );
			var dest = new byte[offset + 4];
			Assert.True ( offset >= 0 );

			ManagedAdbExtenstions.Swap32bitsToArray ( value, dest, offset );
			Assert.True ( dest.Length == offset + 4 );
			Assert.True ( dest[offset] == (byte)( value & 0x000000FF ) );
			Assert.True ( dest[offset + 1] == (byte)( value & 0x0000FF00 ) >> 8 );
			Assert.True ( dest[offset + 2] == (byte)( value & 0x00FF0000 ) >> 16 );
			Assert.True ( dest[offset + 3] == (byte)( value & 0xFF000000 ) >> 24 );
		}

		[Fact]
		public void WhenValueIsZero_ShouldReturnSwapped ( ) {
			var fixture = new Fixture ( );
			var value = 0;
			var offset = fixture.Create<int> ( );
			var dest = new byte[offset + 4];
			Assert.True ( offset >= 0 );

			ManagedAdbExtenstions.Swap32bitsToArray ( value, dest, offset );
			Assert.True ( dest.Length == offset + 4 );
			Assert.True ( dest[offset] == (byte)( value & 0x000000FF ) );
			Assert.True ( dest[offset + 1] == (byte)( value & 0x0000FF00 ) >> 8 );
			Assert.True ( dest[offset + 2] == (byte)( value & 0x00FF0000 ) >> 16 );
			Assert.True ( dest[offset + 3] == (byte)( value & 0xFF000000 ) >> 24 );
		}
		
	}
}
