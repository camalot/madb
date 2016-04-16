using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Managed.Adb.IO;
using Xunit;

namespace Managed.Adb.Tests.ForIOLinuxPath {
	public class GetInvalidPathChars {
		[Fact]
		public void WhenGetttingInvalidPathChars_ShouldEqualExpected ( ) {
			var result = LinuxPath.GetInvalidPathChars ( );
			Assert.Equal ( LinuxPathConsts.InvalidPathChars, result );
		}



	}
}
