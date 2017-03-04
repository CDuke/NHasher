using System.Text;
using Xunit;

namespace NHasher.Tests
{
    public class MurmurHash32X32V3Tests
    {
        [Theory]

        [InlineData(2538058380, "", "28C2B6EB")]
        [InlineData(2538058380, "The quick brown fox jumps over the lazy dog", "CD26A82F")]
        [InlineData(2538058380, "The quick brown fox jumps over the lazy cog", "D49E9C8D")]
        [InlineData(2538058380, "THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG", "F58C7B63")]
        [InlineData(2538058380, "THE QUICK BROWN FOX JUMPS OVER THE LAZY COG", "36F2B821")]
        [InlineData(2538058380, "the quick brown fox jumps over the lazy dog", "1C2FC0ED")]
        [InlineData(2538058380, "the quick brown fox jumps over the lazy cog", "2B578197")]

        [InlineData(0, "", "00000000")]
        [InlineData(0x0, "The quick brown fox jumps over the lazy dog", "23F74F2E")]
        [InlineData(0x0, "The quick brown fox jumps over the lazy cog", "FC0082F0")]
        [InlineData(0x0, "THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG", "AEB01B2C")]
        [InlineData(0x0, "THE QUICK BROWN FOX JUMPS OVER THE LAZY COG", "8554B421")]
        [InlineData(0x0, "the quick brown fox jumps over the lazy dog", "FF62DE02")]
        [InlineData(0x0, "the quick brown fox jumps over the lazy cog", "413361BD")]
        [InlineData(0x0, "aaa", "B75FD0B4")]

        [InlineData(3314489979, "", "86991F82")]
        [InlineData(3314489979, "The quick brown fox jumps over the lazy dog", "D622B475")]
        [InlineData(3314489979, "The quick brown fox jumps over the lazy cog", "27757527")]
        [InlineData(3314489979, "THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG", "E06049F2")]
        [InlineData(3314489979, "THE QUICK BROWN FOX JUMPS OVER THE LAZY COG", "FC70A31E")]
        [InlineData(3314489979, "the quick brown fox jumps over the lazy dog", "60145614")]
        [InlineData(3314489979, "the quick brown fox jumps over the lazy cog", "FF92D2B6")]
        public void CheckHashes(uint seed, string input, string expectedHash)
        {
            using (var hasher = new MurmurHash32X32V3(seed))
            {
                var buffer = StringToBytes(input);
                var hash = hasher.ComputeHashAsString(buffer);
                Assert.Equal(expectedHash, hash);
            }
        }

        [Fact]
        public void CheckHashSize()
        {
            using (var hash = new MurmurHash32X32V3())
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
