using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NHasher.Tests
{
    public class XXHash64Tests
    {
        [Theory]
        [InlineData(0, "", 0xEF46DB3751D8E999)]
        [InlineData(0, "a", 0xd24ec4f1a98c6e5b)]
        [InlineData(0, "as", 0x1c330fb2d66be179)]
        [InlineData(0, "asd", 0x631c37ce72a97393)]
        [InlineData(0, "asdf", 0x415872f599cea71e)]
        [InlineData(0, "abcdefghijklmnopqrstuvwxyz012345", 0xbf2cd639b4143b80)]
        [InlineData(0, "abcdefghijklmnopqrstuvwxyz0123456789", 0x64f23ecf1609b766)]
        [InlineData(0, "Call me Ishmael. Some years ago--never mind how long precisely-", 0x02a2e85470d6fd96)]
        [InlineData(2654435761, "", 0xAC75FDA2929B17EF)]
        public void CheckHashesString(uint seed, string input, ulong expectedHash)
        {
            using (var hasher = new XXHash64(seed))
            {
                var buffer = StringToBytes(input);
                var hash = hasher.ComputeHash(buffer);
                var hashInt = BitConverter.ToUInt64(hash, 0);
                Assert.Equal(expectedHash, hashInt);
            }
        }

		[Theory]
		[MemberData(nameof(GetTestData))]
        public void CheckHashesArray(uint seed, byte[] input, int count, ulong expectedHash)
        {
            using (var hasher = new XXHash64(seed))
            {
                var hash = hasher.ComputeHash(input, 0, count);
                var hashInt = BitConverter.ToUInt64(hash, 0);
                Assert.Equal(expectedHash, hashInt);
            }
        }

        [Fact]
        public void CheckHashSize()
        {
            using (var hash = new XXHash64())
            {
                Assert.Equal(64, hash.HashSize);
            }
        }

        private static byte[] StringToBytes(string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }

        private static IEnumerable<object[]> GetTestData()
        {
            const uint prime = 2654435761;
            const int sanityBufferSize = 101;
            var sanityBuffer = new byte[101];

            var byteGen = prime;
            for (var i = 0; i < sanityBufferSize; i++)
            {
                sanityBuffer[i] = (byte)(byteGen >> 24);
                byteGen *= byteGen;
            }
            return new[]
            {
                new object[] {0, sanityBuffer, 1, 0x4FCE394CC88952D8},
                new object[] {prime, sanityBuffer, 1, 0x739840CB819FA723},
                new object[] {0, sanityBuffer, 14, 0xCFFA8DB881BC3A3D},
                new object[] {prime, sanityBuffer, 14, 0x5B9611585EFCC9CB},
                new object[] {0, sanityBuffer, sanityBufferSize, 0x0EAB543384F878AD},
                new object[] {prime, sanityBuffer, sanityBufferSize, 0xCAA65939306F1E21}
            };
        }
    }
}