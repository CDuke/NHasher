using System.Text;
using Xunit;

namespace NHasher.Tests
{
    public class Fnv64ATests
    {
        [Theory]
        [InlineData(0xcbf29ce484222325, "")]
        [InlineData(0xaf63dc4c8601ec8c, "a")]
        [InlineData(0xaf63df4c8601f1a5, "b")]
        [InlineData(0x85944171f73967e8, "foobar")]
        [InlineData(0x38d1334144987bf4, "Hello, world!")]
        public void CheckHashes(ulong expectedHash, string input)
        {
            using (var hasher = new Fnv64A())
            {
                var buffer = StringToBytes(input);
                var hash = hasher.ComputeHashAsUInt64(buffer);
                Assert.Equal(expectedHash, hash);
            }
        }

        [Fact]
        public void CheckHashSize()
        {
            using (var hash = new Fnv64A())
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
