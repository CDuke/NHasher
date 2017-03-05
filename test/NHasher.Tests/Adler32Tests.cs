using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace NHasher.Tests
{
    public class Adler32Tests
    {
        [Theory]
        [MemberData(nameof(GetTestData))]
        public void CheckHashes(uint expectedHash, string input)
        {
            using (var hasher = new Adler32())
            {
                var buffer = StringToBytes(input);
                var hash = hasher.ComputeHashAsUInt32(buffer);
                Assert.Equal(expectedHash, hash);
            }
        }

        [Theory]
        [MemberData(nameof(GetTestDataArray))]
        public void CheckHashes(uint expectedHash, byte[] input)
        {
            using (var hasher = new Adler32())
            {
                var hash = hasher.ComputeHashAsUInt32(input);
                Assert.Equal(expectedHash, hash);
            }
        }

        [Fact]
        public void CheckHashSize()
        {
            using (var hash = new Adler32())
            {
                Assert.Equal(32, hash.HashSize);
            }
        }

        private static IEnumerable<object[]> GetTestData()
        {
            return new[]
            {
                new object[] { 0x00000001, "" },
                new object[] { 0x00620062, "a" },
                new object[] { 0x012600c4, "ab" },
                new object[] { 0x024d0127, "abc" },
                new object[] { 0x03d8018b, "abcd" },
                new object[] { 0x05c801f0, "abcde" },
                new object[] { 0x081e0256, "abcdef" },
                new object[] { 0x0adb02bd, "abcdefg" },
                new object[] { 0x0e000325, "abcdefgh" },
                new object[] { 0x118e038e, "abcdefghi" },
                new object[] { 0x158603f8, "abcdefghij" },
                new object[] { 0x3f090f02, "Discard medicine more than two years old." },
                new object[] { 0x46d81477, "He who has a shady past knows that nice guys finish last." },
                new object[] { 0x40ee0ee1, "I wouldn't marry him with a ten foot pole." },
                new object[] { 0x16661315, "Free! Free!/A trip/to Mars/for 900/empty jars/Burma Shave" },
                new object[] { 0x5b2e1480, "The days of the digital watch are numbered.  -Tom Stoppard" },
                new object[] { 0x8c3c09ea, "Nepal premier won't resign." },
                new object[] { 0x45ac18fd, "For every action there is an equal and opposite government program." },
                new object[] { 0x53c61462, "His money is twice tainted: 'taint yours and 'taint mine." },
                new object[] { 0x7e511e63, "There is no reason for any individual to have a computer in their home. -Ken Olsen, 1977" },
                new object[] { 0xe4801a6a, "It's a tiny change to the code and not completely disgusting. - Bob Manchek" },
                new object[] { 0x61b507df, "size:  a.out:  bad magic" },
                new object[] { 0xb8631171, "The major problem is with sendmail.  -Mark Horton" },
                new object[] { 0x8b5e1904, "Give me a rock, paper and scissors and I will move the world.  CCFestoon" },
                new object[] { 0x7cc6102b, "If the enemy is within range, then so are you." },
                new object[] { 0x700318e7, "It's well we cannot hear the screams/That we create in others' dreams." },
                new object[] { 0x1e601747, "You remind me of a TV show, but that's all right: I watch it anyway." },
                new object[] { 0xb55b0b09, "C is as portable as Stonehedge!!" },
                new object[] { 0x39111dd0, "Even if I could be Shakespeare, I think I should still choose to be Faraday. - A. Huxley" },
                new object[] { 0x91dd304f, "The fugacity of a constituent in a mixture of gases at a given temperature is proportional to its mole fraction.  Lewis-Randall Rule" },
                new object[] { 0x2e5d1316, "How can you write a big system without C++?  -Paul Glick" },
                new object[] { 0xd0201df6, "'Invariant assertions' is the most elegant programming technique!  -Tom Szymanski" },
                new object[] { 0x86af0001, new string('\x00', (int)Math.Pow(10, 5)) },
                new object[] { 0x79660b4d, new string('a', (int)Math.Pow(10, 5)) },
                new object[] { 0x110588ee, CreateString("ABCDEFGHIJKLMNOPQRSTUVWXYZ", (int)Math.Pow(10, 4)) },
            };
        }

        private static IEnumerable<object[]> GetTestDataArray()
        {
            return new[]
            {
                new object[] { 0x211297c8, CreateTestByteArray(5548, '8')},
                new object[] { 0xbaa198c8, CreateTestByteArray(5549, '9')},
                new object[] { 0x553499be, CreateTestByteArray(5550, '0')},
                new object[] { 0xf0c19abe, CreateTestByteArray(5551, '1')},
                new object[] { 0x8d5c9bbe, CreateTestByteArray(5552, '2')},
                new object[] { 0x2af69cbe, CreateTestByteArray(5553, '3')},
                new object[] { 0xc9809dbe, CreateTestByteArray(5554, '4')},
                new object[] { 0x69189ebe, CreateTestByteArray(5555, '5')},
            };
        }

        private static byte[] CreateTestByteArray(int length, char lastByte)
        {
            return Enumerable.Range(0, length).Select(_ => byte.MaxValue).Concat(new[] {(byte) lastByte}).ToArray();
        }

        private static string CreateString(string s, int count)
        {
            var sb = new StringBuilder(s.Length * count);
            for (var i = 0; i < count; i++)
            {
                sb.Append(s);
            }

            return sb.ToString();
        }

        private static byte[] StringToBytes(string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }
    }
}
