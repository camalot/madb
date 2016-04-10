using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Managed.Adb.IO;
using Xunit;

namespace Managed.Adb.Tests.ForLinuxPath {
	public class GetInvalidFileNameChars {
		[Fact]
		public void WhenGetttingInvalidFileNameChars_ShouldEqualExpected () {
			var result = LinuxPath.GetInvalidFileNameChars ( );
			Assert.Equal ( LinuxPathConsts.InvalidFileNameChars, result );
		}
	}
}
