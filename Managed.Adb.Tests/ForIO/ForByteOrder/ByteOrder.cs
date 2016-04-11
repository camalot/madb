using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Managed.Adb.IO;
using Ploeh.AutoFixture;
using Xunit;

namespace Managed.Adb.Tests.ForIO.ForByteOrder {
	public class ByteOrder {
		[Fact]
		public void WhenBigEdianName_ShouldReturnBigEdian ( ) {
			var fixture = new Fixture ( );

			var be = IO.ByteOrder.BIG_ENDIAN;
			Assert.Equal ( be.Name, "BIG_ENDIAN" );
		}

		[Fact]
		public void WhenBigEdianToString_ShouldReturnBigEdian ( ) {
			var fixture = new Fixture ( );

			var be = IO.ByteOrder.BIG_ENDIAN;
			Assert.Equal ( be.ToString(), "BIG_ENDIAN" );
		}


		[Fact]
		public void WhenLittleEdianName_ShouldReturnBigEdian ( ) {
			var fixture = new Fixture ( );

			var be = IO.ByteOrder.LITTLE_ENDIAN;
			Assert.Equal ( be.Name, "LITTLE_ENDIAN" );
		}

		[Fact]
		public void WhenLittleEdianToString_ShouldReturnBigEdian ( ) {
			var fixture = new Fixture ( );

			var be = IO.ByteOrder.LITTLE_ENDIAN;
			Assert.Equal ( be.ToString ( ), "LITTLE_ENDIAN" );
		}

	}
}
