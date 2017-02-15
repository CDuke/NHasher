using System;
using System.Collections.Generic;
using Xunit;

namespace NHasher.Tests
{
    public class XXHash32Tests
    {
        [Theory]
        [MemberData(nameof(GetTestData))]
        public void CheckHashes(uint seed, byte[] input, int count, ulong expectedHash)
        {
            using (var hasher = new XXHash32(seed))
            {
                var hash = hasher.ComputeHash(input, 0, count);
                var hashInt = BitConverter.ToUInt32(hash, 0);
                Assert.Equal(expectedHash, hashInt);
            }
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
                new object[] {0, Array.Empty<byte>(), 0, 0x02CC5D05},
                new object[] {prime, Array.Empty<byte>(), 0, 0x36B78AE7},
                new object[] {0, sanityBuffer, 1, 0xB85CBEE5},
                new object[] {prime, sanityBuffer, 1, 0xD5845D64},
                new object[] {0, sanityBuffer, 14, 0xE5AA0AB4},
                new object[] {prime, sanityBuffer, 14, 0x4481951D},
                new object[] {0, sanityBuffer, sanityBufferSize, 0x1F1AA412},
                new object[] {prime, sanityBuffer, sanityBufferSize, 0x498EC8E2 }
            };
        }
    }
}