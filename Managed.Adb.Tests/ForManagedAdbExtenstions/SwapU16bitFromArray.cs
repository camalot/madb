using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Xunit;

namespace Managed.Adb.Tests.ForManagedAdbExtenstions {
	public class SwapU16bitFromArray {
		[Fact]
		public void WhenValueIsPositive_ShouldReturnInteger ( ) {
			var fixture = new Fixture ( );
			var value = fixture.Create<int> ( );
			var offset = fixture.Create<int> ( );
			var dest = new byte[offset + 4];
			Assert.True ( offset >= 0 );
			ManagedAdbExtenstions.Swap32bitsToArray ( value, dest, offset );

			var result = ManagedAdbExtenstions.SwapU16bitFromArray ( dest, offset );
			Assert.Equal ( value, result );
		}


		[Fact]
		public void WhenValueIsZero_ShouldReturnInteger ( ) {
			var fixture = new Fixture ( );
			var value = 0;
			var offset = fixture.Create<int> ( );
			var dest = new byte[offset + 4];
			Assert.True ( offset >= 0 );
			ManagedAdbExtenstions.Swap32bitsToArray ( value, dest, offset );

			var result = ManagedAdbExtenstions.SwapU16bitFromArray ( dest, offset );
			Assert.Equal ( value, result );
		}
	}
}
