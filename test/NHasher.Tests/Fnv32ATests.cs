using System.Text;
using Xunit;

namespace NHasher.Tests
{
    public class Fnv32ATests
    {
        [Theory]
        [InlineData(0x811c9dc5, "")]
        [InlineData(0xe40c292c, "a")]
        [InlineData(0xe70c2de5, "b")]
        [InlineData(0xbf9cf968, "foobar")]
        [InlineData(0xed90f094, "Hello, world!")]
        public void CheckHashes(uint expectedHash, string input)
        {
            using (var hasher = new Fnv32A())
            {
                var buffer = StringToBytes(input);
                var hash = hasher.ComputeHashAsUInt32(buffer);
                Assert.Equal(expectedHash, hash);
            }
        }

        [Fact]
        public void CheckHashSize()
        {
            using (var hash = new Fnv32A())
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
