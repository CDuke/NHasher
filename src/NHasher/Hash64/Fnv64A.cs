using System.Security.Cryptography;

namespace NHasher
{
    /// <summary>
    /// Implementation of 64 bit FNV (Fowler–Noll–Vo) hash algoritm specified at <see href="https://wikipedia.org/wiki/FNV"/>.
    /// </summary>
    public class Fnv64A : HashAlgorithm
    {
        private const int HashSizeBytes = 8;

        private const ulong Prime = 0x0100000001b3;
        private const ulong OffsetBasis = 0xcbf29ce484222325;

        private ulong _h;

        /// <summary>
        /// Initializes a new instance of the <see cref="Fnv64A"/> class.
        /// </summary>
        public Fnv64A()
        {
            _h = OffsetBasis;
        }

        /// <inheritdoc cref="HashAlgorithm.HashSize"/>
        public override int HashSize => 64;

        /// <summary>
        /// Initializes an implementation of the <see cref="Fnv64A"/> class.
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
                    *((ulong*)b) = _h;
                }

                return hash;
            }
        }
    }
}
