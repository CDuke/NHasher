using System.Text;
using Xunit;

namespace NHasher.Tests
{
    public class MurmurHash128X32V3Tests
    {

        [Theory]

        [InlineData(2538058380, "", "A1D5BEF71C6A575B1C6A575B1C6A575B")]
        [InlineData(2538058380, "The quick brown fox jumps over the lazy dog", "5ED5D48A7161B84C9C3AA78E3E79B6CD")]
        [InlineData(2538058380, "The quick brown fox jumps over the lazy cog", "0E8829CCF91081107C6F5EBF3433E188")]
        [InlineData(2538058380, "THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG", "85C392715A1701C682DFBF4A14508190")]
        [InlineData(2538058380, "THE QUICK BROWN FOX JUMPS OVER THE LAZY COG", "98BB1B3C95194F656E79DE6D66243F09")]
        [InlineData(2538058380, "the quick brown fox jumps over the lazy dog", "6EC157F6790F9BEE9E2BE436C509F88A")]
        [InlineData(2538058380, "the quick brown fox jumps over the lazy cog", "BA7799E33BD19F1C3D49A292432788FE")]

        [InlineData(0, "", "00000000000000000000000000000000")]
        [InlineData(0x0, "The quick brown fox jumps over the lazy dog", "C383152F672CEEEC6CF67B5D2C1DE9E5")]
        [InlineData(0x0, "The quick brown fox jumps over the lazy cog", "8843D60E79E79A3E934503972FF3B349")]
        [InlineData(0x0, "THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG", "5CB9E26451952B2FCC493CB9FED0B4C9")]
        [InlineData(0x0, "THE QUICK BROWN FOX JUMPS OVER THE LAZY COG", "484F7F9A1B2A6202C3AB64C4F4DDBBAB")]
        [InlineData(0x0, "the quick brown fox jumps over the lazy dog", "FDB47DAF02170D403F6093539B388AF2")]
        [InlineData(0x0, "the quick brown fox jumps over the lazy cog", "FBF39633637C9E70EFCE1DF157359E42")]
        [InlineData(0x0, "aaaaaaa", "A7387719286D290FCA19A398CA19A398")]
        [InlineData(0x0, "aaaaaaaaaaaaaaa", "78FA8D8D331A81A560DFB090484C863A")]

        [InlineData(3314489979, "", "3C5B306A10B5611510B5611510B56115")]
        [InlineData(3314489979, "The quick brown fox jumps over the lazy dog", "4F3BA94A618F373A2B66976AA38FF8E8")]
        [InlineData(3314489979, "The quick brown fox jumps over the lazy cog", "086525191FEE0A51BCD01CAAF5B988FD")]
        [InlineData(3314489979, "THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG", "2B9A5CC8C8EC392164AE9207E0428B18")]
        [InlineData(3314489979, "THE QUICK BROWN FOX JUMPS OVER THE LAZY COG", "1FC7B642B0AC04D7BB65238419CBC824")]
        [InlineData(3314489979, "the quick brown fox jumps over the lazy dog", "A0A355F5E26816115CCF78A1925D1B4E")]
        [InlineData(3314489979, "the quick brown fox jumps over the lazy cog", "6C22B9AA4BBA167C809D3E087B1B9D46")]
        public void CheckHashes(uint seed, string input, string expectedHash)
        {
            using (var hasher = new MurmurHash128X32V3(seed))
            {
                var buffer = StringToBytes(input);
                var hash = hasher.ComputeHashAsString(buffer);
                Assert.Equal(expectedHash, hash);
            }
        }

        [Fact]
        public void CheckHashSize()
        {
            using (var hash = new MurmurHash128X32V3())
            {
                Assert.Equal(128, hash.HashSize);
            }
        }
        private static byte[] StringToBytes(string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }
    }
}
