using System.Text;
using Xunit;

namespace NHasher.Tests
{
    public class Adler32Tests
    {
        [Theory]
        [InlineData(0x00000001, "")]
        [InlineData(0x00620062, "a")]
        [InlineData(0x012600c4, "ab")]
        [InlineData(0x024d0127, "abc")]
        [InlineData(0x03d8018b, "abcd")]
        [InlineData(0x05c801f0, "abcde")]
        [InlineData(0x081e0256,"abcdef")]
        [InlineData(0x0adb02bd, "abcdefg")]
        [InlineData(0x0e000325, "abcdefgh")]
        [InlineData(0x118e038e, "abcdefghi")]
        [InlineData(0x158603f8, "abcdefghij")]
        [InlineData(0x3f090f02, "Discard medicine more than two years old.")]
        [InlineData(0x46d81477, "He who has a shady past knows that nice guys finish last.")]
        [InlineData(0x40ee0ee1, "I wouldn't marry him with a ten foot pole.")]
        [InlineData(0x16661315, "Free! Free!/A trip/to Mars/for 900/empty jars/Burma Shave")]
        [InlineData(0x5b2e1480, "The days of the digital watch are numbered.  -Tom Stoppard")]
        [InlineData(0x8c3c09ea, "Nepal premier won't resign.")]
        [InlineData(0x45ac18fd, "For every action there is an equal and opposite government program.")]
        [InlineData(0x53c61462, "His money is twice tainted: 'taint yours and 'taint mine.")]
        [InlineData(0x7e511e63, "There is no reason for any individual to have a computer in their home. -Ken Olsen, 1977")]
        [InlineData(0xe4801a6a, "It's a tiny change to the code and not completely disgusting. - Bob Manchek")]
        [InlineData(0x61b507df, "size:  a.out:  bad magic")]
        [InlineData(0xb8631171, "The major problem is with sendmail.  -Mark Horton")]
        [InlineData(0x8b5e1904, "Give me a rock, paper and scissors and I will move the world.  CCFestoon")]
        [InlineData(0x7cc6102b, "If the enemy is within range, then so are you.")]
        [InlineData(0x700318e7, "It's well we cannot hear the screams/That we create in others' dreams.")]
        [InlineData(0x1e601747, "You remind me of a TV show, but that's all right: I watch it anyway.")]
        [InlineData(0xb55b0b09, "C is as portable as Stonehedge!!")]
        [InlineData(0x39111dd0, "Even if I could be Shakespeare, I think I should still choose to be Faraday. - A. Huxley")]
        [InlineData(0x91dd304f, "The fugacity of a constituent in a mixture of gases at a given temperature is proportional to its mole fraction.  Lewis-Randall Rule")]
        [InlineData(0x2e5d1316, "How can you write a big system without C++?  -Paul Glick")]
        [InlineData(0xd0201df6, "'Invariant assertions' is the most elegant programming technique!  -Tom Szymanski")]
        public void CheckHashes(uint expectedHash, string input)
        {
            using (var hasher = new Adler32())
            {
                var buffer = StringToBytes(input);
                var hash = hasher.ComputeHashAsUInt32(buffer);
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

        private static byte[] StringToBytes(string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }
    }
}
