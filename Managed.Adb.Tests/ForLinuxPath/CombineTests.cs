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
	public class CombineTests {
		[Fact]
		public void When2ArgsAndPath1IsNull_ShouldThrowArgumentNullException ( ) {
			var fixture = new Fixture ( );
			Assert.Throws<ArgumentNullException> ( ( ) => {
				var result = LinuxPath.Combine ( null, fixture.Create ( "path2" ) );
			} );
		}
		[Fact]
		public void When2ArgsAndPath2IsNull_ShouldThrowArgumentNullException ( ) {
			var fixture = new Fixture ( );
			Assert.Throws<ArgumentNullException> ( ( ) => {
				var result = LinuxPath.Combine ( fixture.Create ( "path1" ), null );
			} );
		}

		[Fact]
		public void When2ArgsAndPath1IsEmpty_ShouldReturnRelativePath2 ( ) {
			var fixture = new Fixture ( );
			var p2 = fixture.Create ( "path2-" );
			var result = LinuxPath.Combine ( string.Empty, p2 );
			Assert.Equal ( "./{0}/".With ( p2 ), result );
		}

		[Fact]
		public void When2ArgsAndPath1IsEmptyAndPath2IsRooted_ShouldReturnRootedPath2 ( ) {
			var fixture = new Fixture ( );
			var p2 = fixture.Create ( "/path2-" );
			var result = LinuxPath.Combine ( string.Empty, p2 );
			Assert.Equal ( "/{0}/".With ( p2 ).REReplace ( "//", "/" ), result );
		}

		[Fact]
		public void When2ArgsAndPath2IsEmpty_ShouldReturnRelativePath1 ( ) {
			var fixture = new Fixture ( );
			var p1 = fixture.Create ( "path1-" );
			var p2 = string.Empty;
			var result = LinuxPath.Combine ( p1, p2 );
			Assert.Equal ( "./{0}/".With ( p1 ), result );
		}

		[Fact]
		public void When2ArgsAndPath2IsEmptyAndPath2IsRooted_ShouldReturnRootedPath1 ( ) {
			var fixture = new Fixture ( );
			var p1 = fixture.Create ( "/path1-" );
			var p2 = string.Empty;
			var result = LinuxPath.Combine ( p1, p2 );
			Assert.Equal ( "/{0}/".With ( p1 ).REReplace ( "//", "/" ), result );
		}

		[Fact]
		public void When2ArgsAndPath1ContainsInvalidCharacter_ShouldThrowArgumentException ( ) {
			var fixture = new Fixture ( );
			int errorCount = 0;
			for ( var x = 0; x < LinuxPathConsts.InvalidPathChars.Length; ++x ) {
				try {
					string result = LinuxPath.Combine ( "/{0}-{1}".With ( LinuxPathConsts.InvalidPathChars[x], fixture.Create ( "path1" ) ), fixture.Create ( "path2" ) );
				} catch ( ArgumentException ) {
					errorCount++;
				}
			}

			Assert.Equal ( LinuxPathConsts.InvalidPathChars.Length, errorCount );

		}

		[Fact]
		public void When2ArgsAndPath2ContainsInvalidCharacter_ShouldThrowArgumentException ( ) {
			var fixture = new Fixture ( );
			int errorCount = 0;
			for ( var x = 0; x < LinuxPathConsts.InvalidPathChars.Length; ++x ) {
				try {
					string result = LinuxPath.Combine ( "/{0}".With ( fixture.Create ( "path1" ) ), "{0}-{1}".With ( fixture.Create ( "path2" ), LinuxPathConsts.InvalidPathChars[x] ) );
				} catch ( ArgumentException ) {
					errorCount++;
				}
			}

			Assert.Equal ( LinuxPathConsts.InvalidPathChars.Length, errorCount );

		}

		[Fact]
		public void When2Args_ShouldReturnCombined ( ) {
			var fixture = new Fixture ( );
			var p1 = fixture.Create ( "/path1" );
			var p2 = fixture.Create ( "path2" );
			var result = LinuxPath.Combine ( p1, p2 );
			Assert.Equal ( "{0}/{1}".With ( p1, p2 ), result );
		}

		[Fact]
		public void When2ArgsAndPathsHaveSeparators_ShouldReturnCombined ( ) {
			var fixture = new Fixture ( );
			var p1 = "{0}/".With ( fixture.Create ( "/path1" ) );
			var p2 = fixture.Create ( "path2" );
			var result = LinuxPath.Combine ( p1, p2 );
			Assert.Equal ( "{0}/{1}".With ( p1, p2 ).REReplace ( "//", "/" ), result );
		}

		[Fact]
		public void When2ArgsAndPath2IsRooted_ShouldReturnPath2 ( ) {
			var fixture = new Fixture ( );
			var p1 = fixture.Create ( "/path1-" );
			var p2 = fixture.Create ( "/path2-" );
			var result = LinuxPath.Combine ( p1, p2 );
			Assert.Equal ( "{0}/".With ( p2 ), result );
		}

		[Fact]
		public void When3ArgsAndPath1IsNull_ShouldThrowArgumentNullException ( ) {
			var fixture = new Fixture ( );
			Assert.Throws<ArgumentNullException> ( ( ) => {
				var result = LinuxPath.Combine ( null, fixture.Create ( "path2" ), fixture.Create ( "path3" ) );
			} );
		}

		[Fact]
		public void When3ArgsAndPath2IsNull_ShouldThrowArgumentNullException ( ) {
			var fixture = new Fixture ( );
			Assert.Throws<ArgumentNullException> ( ( ) => {
				var result = LinuxPath.Combine ( fixture.Create ( "path1" ), null, fixture.Create ( "path3" ) );
			} );
		}

		[Fact]
		public void When3ArgsAndPath3IsNull_ShouldThrowArgumentNullException ( ) {
			var fixture = new Fixture ( );
			Assert.Throws<ArgumentNullException> ( ( ) => {
				var result = LinuxPath.Combine ( fixture.Create ( "path1" ), fixture.Create ( "path2" ), null );
			} );
		}

		[Fact]
		public void When3ArgsAndPath1ContainsInvalidCharacter_ShouldThrowArgumentException ( ) {
			var fixture = new Fixture ( );
			int errorCount = 0;
			for ( var x = 0; x < LinuxPathConsts.InvalidPathChars.Length; ++x ) {
				try {
					string result = LinuxPath.Combine ( "/{0}-{1}".With ( LinuxPathConsts.InvalidPathChars[x], fixture.Create ( "path1" ) ), fixture.Create ( "path2" ) );
				} catch ( ArgumentException ) {
					errorCount++;
				}
			}

			Assert.Equal ( LinuxPathConsts.InvalidPathChars.Length, errorCount );

		}

		[Fact]
		public void When3ArgsAndPath2ContainsInvalidCharacter_ShouldThrowArgumentException ( ) {
			var fixture = new Fixture ( );
			int errorCount = 0;
			for ( var x = 0; x < LinuxPathConsts.InvalidPathChars.Length; ++x ) {
				try {
					string result = LinuxPath.Combine ( "/{0}".With ( fixture.Create ( "path1" ) ), "{0}-{1}".With ( fixture.Create ( "path2" ), LinuxPathConsts.InvalidPathChars[x] ) );
				} catch ( ArgumentException ) {
					errorCount++;
				}
			}

			Assert.Equal ( LinuxPathConsts.InvalidPathChars.Length, errorCount );

		}

		[Fact]
		public void When3ArgsAndPath3ContainsInvalidCharacter_ShouldThrowArgumentException ( ) {
			var fixture = new Fixture ( );
			int errorCount = 0;
			for ( var x = 0; x < LinuxPathConsts.InvalidPathChars.Length; ++x ) {
				try {
					string result = LinuxPath.Combine ( "/{0}".With ( fixture.Create ( "path1" ) ), fixture.Create ( "path2" ), "{0}-{1}".With ( fixture.Create ( "path3" ), LinuxPathConsts.InvalidPathChars[x] ) );
				} catch ( ArgumentException ) {
					errorCount++;
				}
			}

			Assert.Equal ( LinuxPathConsts.InvalidPathChars.Length, errorCount );

		}

		[Fact]
		public void When3Args_ShouldReturnCombined ( ) {
			var fixture = new Fixture ( );
			var p1 = fixture.Create ( "/path1" );
			var p2 = fixture.Create ( "path2" );
			var p3 = fixture.Create ( "path3" );
			var result = LinuxPath.Combine ( p1, p2, p3 );
			Assert.Equal ( "{0}/{1}/{2}".With ( p1, p2, p3 ), result );
		}

		[Fact]
		public void When4ArgsAndPath1IsNull_ShouldThrowArgumentNullException ( ) {
			var fixture = new Fixture ( );
			Assert.Throws<ArgumentNullException> ( ( ) => {
				var result = LinuxPath.Combine ( null, fixture.Create ( "path2" ), fixture.Create ( "path3" ), fixture.Create ( "path4" ) );
			} );
		}

		[Fact]
		public void When4ArgsAndPath2IsNull_ShouldThrowArgumentNullException ( ) {
			var fixture = new Fixture ( );
			Assert.Throws<ArgumentNullException> ( ( ) => {
				var result = LinuxPath.Combine ( fixture.Create ( "path1" ), null, fixture.Create ( "path3" ), fixture.Create ( "path4" ) );
			} );
		}

		[Fact]
		public void When4ArgsAndPath3IsNull_ShouldThrowArgumentNullException ( ) {
			var fixture = new Fixture ( );
			Assert.Throws<ArgumentNullException> ( ( ) => {
				var result = LinuxPath.Combine ( fixture.Create ( "path1" ), fixture.Create ( "path2" ), null, fixture.Create ( "path4" ) );
			} );
		}
		[Fact]
		public void When4ArgsAndPath4IsNull_ShouldThrowArgumentNullException ( ) {
			var fixture = new Fixture ( );
			Assert.Throws<ArgumentNullException> ( ( ) => {
				var result = LinuxPath.Combine ( fixture.Create ( "path1" ), fixture.Create ( "path2" ), fixture.Create ( "path3" ), null );
			} );
		}

		[Fact]
		public void When4ArgsAndPath1ContainsInvalidCharacter_ShouldThrowArgumentException ( ) {
			var fixture = new Fixture ( );
			int errorCount = 0;
			for ( var x = 0; x < LinuxPathConsts.InvalidPathChars.Length; ++x ) {
				try {
					string result = LinuxPath.Combine ( "/{0}-{1}".With ( LinuxPathConsts.InvalidPathChars[x], fixture.Create ( "path1" ) ), fixture.Create ( "path2" ) );
				} catch ( ArgumentException ) {
					errorCount++;
				}
			}

			Assert.Equal ( LinuxPathConsts.InvalidPathChars.Length, errorCount );

		}

		[Fact]
		public void When4ArgsAndPath2ContainsInvalidCharacter_ShouldThrowArgumentException ( ) {
			var fixture = new Fixture ( );
			int errorCount = 0;
			for ( var x = 0; x < LinuxPathConsts.InvalidPathChars.Length; ++x ) {
				try {
					string result = LinuxPath.Combine ( "/{0}".With ( fixture.Create ( "path1" ) ), "{0}-{1}".With ( fixture.Create ( "path2" ), LinuxPathConsts.InvalidPathChars[x] ) );
				} catch ( ArgumentException ) {
					errorCount++;
				}
			}

			Assert.Equal ( LinuxPathConsts.InvalidPathChars.Length, errorCount );

		}

		[Fact]
		public void When4ArgsAndPath3ContainsInvalidCharacter_ShouldThrowArgumentException ( ) {
			var fixture = new Fixture ( );
			int errorCount = 0;
			for ( var x = 0; x < LinuxPathConsts.InvalidPathChars.Length; ++x ) {
				try {
					string result = LinuxPath.Combine ( "/{0}".With ( fixture.Create ( "path1" ) ), fixture.Create ( "path2" ), "{0}-{1}".With ( fixture.Create ( "path3" ), LinuxPathConsts.InvalidPathChars[x] ) );
				} catch ( ArgumentException ) {
					errorCount++;
				}
			}

			Assert.Equal ( LinuxPathConsts.InvalidPathChars.Length, errorCount );

		}

		[Fact]
		public void When4ArgsAndPath4ContainsInvalidCharacter_ShouldThrowArgumentException ( ) {
			var fixture = new Fixture ( );
			int errorCount = 0;
			for ( var x = 0; x < LinuxPathConsts.InvalidPathChars.Length; ++x ) {
				try {
					string result = LinuxPath.Combine ( "/{0}".With ( fixture.Create ( "path1" ) ), fixture.Create ( "path2" ), fixture.Create ( "path3" ), "{0}-{1}".With ( fixture.Create ( "path4" ), LinuxPathConsts.InvalidPathChars[x] ) );
				} catch ( ArgumentException ) {
					errorCount++;
				}
			}

			Assert.Equal ( LinuxPathConsts.InvalidPathChars.Length, errorCount );

		}

		[Fact]
		public void When4Args_ShouldReturnCombined ( ) {
			var fixture = new Fixture ( );
			var p1 = fixture.Create ( "/path1" );
			var p2 = fixture.Create ( "path2/" );
			var p3 = fixture.Create ( "./path3" );
			var p4 = fixture.Create ( "path4/" );
			var result = LinuxPath.Combine ( p1, p2, p3, p4 );
			Assert.Equal ( "{0}/{1}/{2}/{3}".With ( p1, p2, p3, p4 ), result );
		}

		[Fact]
		public void WhenArgsArray_ShouldReturnCombined ( ) {
			var fixture = new Fixture ( );
			var p1 = fixture.Create ( "/path1" );
			var p2 = fixture.Create ( "path2" );
			var p3 = fixture.Create ( "path3" );
			var p4 = fixture.Create ( "path4" );
			var p5 = fixture.Create ( "path5" );
			var result = LinuxPath.Combine ( p1, p2, p3, p4, p5 );
			var expected = "{0}/{1}/{2}/{3}/{4}".With ( p1, p2, p3, p4, p5 );
			Assert.Equal ( expected, result );
		}

		[Fact]
		public void WhenArgsArrayIsNull_ShouldThrowArgumentNullException ( ) {
			var fixture = new Fixture ( );
			string[] args = null;
			Assert.Throws<ArgumentNullException> ( ( ) => {
				LinuxPath.Combine ( args );
			} );
		}

		[Fact]
		public void WhenArgsArrayContainsNullItem_ShouldThrowArgumentNullException ( ) {
			var fixture = new Fixture ( );
			var p1 = fixture.Create ( "/path1" );
			string p2 = null;
			var p3 = fixture.Create ( "path3" );
			var p4 = fixture.Create ( "path4" );
			var p5 = fixture.Create ( "path5" );
			Assert.Throws<ArgumentNullException> ( ( ) => {
				LinuxPath.Combine ( p1, p2, p3, p4, p5 );
			} );
			
		}


	}
}
