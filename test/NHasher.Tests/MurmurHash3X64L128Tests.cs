using System;
using Xunit;

namespace NHasher.Tests
{
    public class MurmurHash3X64L128Tests
    {
        [Fact]
        public void EmptyBytes()
        {
            using (var hasher = new MurmurHash3X64L128())
            {
                var expectedHash = new byte[16];
                var hash = hasher.ComputeHash(Array.Empty<byte>());
                Assert.Equal(expectedHash, hash);
            }
        }
    }
}