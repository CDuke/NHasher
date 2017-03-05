using System.Security.Cryptography;

namespace NHasher
{
    /// <summary>
    /// Implementation of 32 bit FNVa (Fowler–Noll–Vo) hash algoritm specified at <see href="https://wikipedia.org/wiki/FNV"/>.
    /// </summary>
    public class Fnv32A : HashAlgorithm
    {
        private const int HashSizeBytes = 4;

        private const uint Prime = 0x01000193;
        private const uint OffsetBasis = 0x811c9dc5;

        private uint _h;

        /// <summary>
        /// Initializes a new instance of the <see cref="Fnv32A"/> class.
        /// </summary>
        public Fnv32A()
        {
            _h = OffsetBasis;
        }

        /// <inheritdoc cref="HashAlgorithm.HashSize"/>
        public override int HashSize => 32;

        /// <summary>
        /// Initializes an implementation of the <see cref="Fnv32A"/> class.
        /// </summary>
        public override void Initialize()
        {
            _h = OffsetBasis;
        }

        /// <inheritdoc cref="HashAlgorithm.HashCore"/>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            var result = _h;
            var end = ibStart + cbSize;
            for (var i = ibStart; i < end; i++)
            {
                result ^= array[i];
                result *= Prime;
            }

            _h = result;
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
    }
}
