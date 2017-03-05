using System.Text;
using Xunit;

namespace NHasher.Tests
{
    public class Fnv32Tests
    {
        [Theory]
        [InlineData(0x811c9dc5, "")]
        [InlineData(0x050c5d7e, "a")]
        [InlineData(0x050c5d7d, "b")]
        [InlineData(0x31f0b262, "foobar")]
        [InlineData(0xe84ead66, "Hello, world!")]
        public void CheckHashes(uint expectedHash, string input)
        {
            using (var hasher = new Fnv32())
            {
                var buffer = StringToBytes(input);
                var hash = hasher.ComputeHashAsUInt32(buffer);
                Assert.Equal(expectedHash, hash);
            }
        }

        [Fact]
        public void CheckHashSize()
        {
            using (var hash = new Fnv32())
            {
                Assert.Equal(32, hash.HashSize);
            }
        }

        private static byte[] StringToBytes(string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }
    }
}
