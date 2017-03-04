using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace NHasher
{
    /// <summary>
    /// Implementation of Adler32 hash algoritm specified at <see href="https://wikipedia.org/wiki/Adler-32"/>.
    /// </summary>
    public class Adler32 : HashAlgorithm
    {
        private const int HashSizeBytes = 4;

        // nmax is the largest n such that
        // 255 * n * (n+1) / 2 + (n+1) * (mod-1) <= 2^32-1.
        // It is mentioned in RFC 1950 (search for "5552").
        private const int NMax = 5552;

        // Mod is the largest prime that is less than 65536
        private const int Mod = 65521;
        private uint _h;

        /// <summary>
        /// Initializes a new instance of the <see cref="Adler32"/> class.
        /// </summary>
        public Adler32()
        {
            _h = 1;
        }

        /// <inheritdoc cref="HashAlgorithm.HashSize"/>
        public override int HashSize => 32;

        /// <summary>
        /// Initializes an implementation of the <see cref="Adler32"/> class.
        /// </summary>
        public override void Initialize()
        {
            _h = 1;
        }

        /// <inheritdoc cref="HashAlgorithm.HashCore"/>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            // split Adler-32 into component sums
            var sum1 = _h & 0xffff;
            var sum2 = (_h >> 16) & 0xffff;

            var pos = ibStart;
            var len = cbSize;

            // in case user likes doing a byte at a time, keep it fast
            if (len == 1)
            {
                sum1 += array[pos];
                if (sum1 >= Mod)
                {
                    sum1 -= Mod;
                }

                sum2 += sum1;
                if (sum2 >= Mod)
                {
                    sum2 -= Mod;
                }

                _h = MixFinal(sum1, sum2);
                return;
            }

            // in case short lengths are provided, keep it somewhat fast
            if (len < 16)
            {
                while (len-- > 0)
                {
                    sum1 += array[pos];
                    pos++;
                    sum2 += sum1;
                }

                if (sum1 >= Mod)
                {
                    sum1 -= Mod;
                }

                sum2 %= Mod;
                _h = MixFinal(sum1, sum2);
                return;
            }

            // do length NMax blocks -- requires just one modulo operation
            while (len >= NMax)
            {
                len -= NMax;
                var n = 347; // NMax / 16;
                do
                {
                    sum1 += array[pos];
                    sum2 += sum1;
                    sum1 += array[pos + 1];
                    sum2 += sum1;
                    sum1 += array[pos + 2];
                    sum2 += sum1;
                    sum1 += array[pos + 3];
                    sum2 += sum1;
                    sum1 += array[pos + 4];
                    sum2 += sum1;
                    sum1 += array[pos + 5];
                    sum2 += sum1;
                    sum1 += array[pos + 6];
                    sum2 += sum1;
                    sum1 += array[pos + 7];
                    sum2 += sum1;
                    sum1 += array[pos + 8];
                    sum2 += sum1;
                    sum1 += array[pos + 9];
                    sum2 += sum1;
                    sum1 += array[pos + 10];
                    sum2 += sum1;
                    sum1 += array[pos + 11];
                    sum2 += sum1;
                    sum1 += array[pos + 12];
                    sum2 += sum1;
                    sum1 += array[pos + 13];
                    sum2 += sum1;
                    sum1 += array[pos + 14];
                    sum2 += sum1;
                    sum1 += array[pos + 15];
                    sum2 += sum1;

                    pos += 16;
                    n--;
                }
                while (n > 0);

                sum1 %= Mod;
                sum2 %= Mod;
            }

            // do remaining bytes (less than NMAX, still just one modulo)
            // avoid modulos if none remaining
            if (len > 0)
            {
                while (len >= 16)
                {
                    len -= 16;

                    sum1 += array[pos];
                    sum2 += sum1;
                    sum1 += array[pos + 1];
                    sum2 += sum1;
                    sum1 += array[pos + 2];
                    sum2 += sum1;
                    sum1 += array[pos + 3];
                    sum2 += sum1;
                    sum1 += array[pos + 4];
                    sum2 += sum1;
                    sum1 += array[pos + 5];
                    sum2 += sum1;
                    sum1 += array[pos + 6];
                    sum2 += sum1;
                    sum1 += array[pos + 7];
                    sum2 += sum1;
                    sum1 += array[pos + 8];
                    sum2 += sum1;
                    sum1 += array[pos + 9];
                    sum2 += sum1;
                    sum1 += array[pos + 10];
                    sum2 += sum1;
                    sum1 += array[pos + 11];
                    sum2 += sum1;
                    sum1 += array[pos + 12];
                    sum2 += sum1;
                    sum1 += array[pos + 13];
                    sum2 += sum1;
                    sum1 += array[pos + 14];
                    sum2 += sum1;
                    sum1 += array[pos + 15];
                    sum2 += sum1;

                    pos += 16;
                }

                while (len-- > 0)
                {
                    sum1 += array[pos];
                    pos++;
                    sum2 += sum1;
                }

                sum1 %= Mod;
                sum2 %= Mod;
            }

            _h = MixFinal(sum1, sum2);
        }

        /// <inheritdoc cref="HashAlgorithm.HashFinal"/>
        protected override byte[] HashFinal()
        {
            unsafe
            {
                var hash = new byte[HashSizeBytes];
                fixed (byte* b = &hash[0])
                {
                    *((uint*)b) = _h;
                }

                return hash;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint MixFinal(uint sum1, uint sum2)
        {
            return sum1 | (sum2 << 16);
        }
    }
}
