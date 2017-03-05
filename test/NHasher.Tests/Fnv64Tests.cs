using System.Text;
using Xunit;

namespace NHasher.Tests
{
    public class Fnv64Tests
    {
        [Theory]
        [InlineData(0xcbf29ce484222325, "")]
        [InlineData(0xaf63bd4c8601b7be, "a")]
        [InlineData(0xaf63bd4c8601b7bd, "b")]
        [InlineData(0x340d8765a4dda9c2, "foobar")]
        [InlineData(7285062107457560934, "Hello, world!")]
        public void CheckHashes(ulong expectedHash, string input)
        {
            using (var hasher = new Fnv64())
            {
                var buffer = StringToBytes(input);
                var hash = hasher.ComputeHashAsUInt64(buffer);
                Assert.Equal(expectedHash, hash);
            }
        }

        [Fact]
        public void CheckHashSize()
        {
            using (var hash = new Fnv64())
            {
                Assert.Equal(64, hash.HashSize);
            }
        }

        private static byte[] StringToBytes(string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }
    }
}
