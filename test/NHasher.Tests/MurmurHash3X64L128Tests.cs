﻿using System.Text;
using Xunit;

namespace NHasher.Tests
{
    public class MurmurHash3X64L128Tests
    {

        [Theory]

        [InlineData(0x9747b28c, "", "B3BBAA1D8A202B397A9502E38F60B093")]
        [InlineData(0x9747b28c, "The quick brown fox jumps over the lazy dog", "213163D23B7F8A73E516C07E727345F9")]
        [InlineData(0x9747b28c, "The quick brown fox jumps over the lazy cog", "94618270B057CDB83CF873585B456F55")]
        [InlineData(0x9747b28c, "THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG", "6C8AD027E39089788AD2CB67789CA4CF")]
        [InlineData(0x9747b28c, "THE QUICK BROWN FOX JUMPS OVER THE LAZY COG", "DD904D9A52D2FB5E71EAD0546624BEA0")]
        [InlineData(0x9747b28c, "the quick brown fox jumps over the lazy dog", "A8FA6851BD2C21CDF33E80C8968B74D0")]
        [InlineData(0x9747b28c, "the quick brown fox jumps over the lazy cog", "714C9A5ADD16AA271F90A72183FD2BE0")]

        [InlineData(0, "", "00000000000000000000000000000000")]
        [InlineData(0x0, "The quick brown fox jumps over the lazy dog", "6C1B07BC7BBC4BE347939AC4A93C437A")]
        [InlineData(0x0, "The quick brown fox jumps over the lazy cog", "9A2685FF70A98C653E5C8EA6EAE3FE43")]
        [InlineData(0x0, "THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG", "C9FB0A32011820A64B3C7A60B06C3982")]
        [InlineData(0x0, "THE QUICK BROWN FOX JUMPS OVER THE LAZY COG", "24B0E694C86C766A6C8FD44492BB010B")]
        [InlineData(0x0, "the quick brown fox jumps over the lazy dog", "B386ADE2FEE9E4BC7F4B6E4074E3E20A")]
        [InlineData(0x0, "the quick brown fox jumps over the lazy cog", "3222507256FE092F24D124BB1E8D7586")]

        [InlineData(0xc58f1a7b, "", "6E4718DF51C270B92DAD702FF62D055F")]
        [InlineData(0xc58f1a7b, "The quick brown fox jumps over the lazy dog", "FF9D0CD2EE401FAC26F5EFDE525C9338")]
        [InlineData(0xc58f1a7b, "The quick brown fox jumps over the lazy cog", "8C935C5B843839F964B24F7AD58BBCCD")]
        [InlineData(0xc58f1a7b, "THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG", "69E483F3D9F149F2F480CE3A0527FE34")]
        [InlineData(0xc58f1a7b, "THE QUICK BROWN FOX JUMPS OVER THE LAZY COG", "D81BB41CBA1C1F083E81A8624EE4F16F")]
        [InlineData(0xc58f1a7b, "the quick brown fox jumps over the lazy dog", "02F78A1B0296EC885CC5692EC8430864")]
        [InlineData(0xc58f1a7b, "the quick brown fox jumps over the lazy cog", "48517672B6B419924084009F9F6D7381")]
        public void CheckHashes(uint seed, string input, string expectedHash)
        {
            using (var hasher = new MurmurHash3X64L128(seed))
            {
                var buffer = StringToBytes(input);
                var hash = hasher.ComputeHashAsString(buffer);
                Assert.Equal(expectedHash, hash);
            }
        }

        private static byte[] StringToBytes(string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }
    }
}