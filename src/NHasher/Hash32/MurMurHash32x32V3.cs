using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace NHasher
{
    /// <summary>
    /// Implementation of 32 bit MurmurHash3 hash algoritm specified at <see href="https://github.com/aappleby/smhasher"/>.
    /// </summary>
    public class MurmurHash32X32V3 : HashAlgorithm
    {
        private const int HashSizeBytes = 4;
        private const uint C1 = 0xcc9e2d51;
        private const uint C2 = 0x1b873593;

        private readonly uint _seed;

        private uint _length;
        private uint _h1;

        /// <summary>
        /// Initializes a new instance of the <see cref="MurmurHash32X32V3"/> class.
        /// </summary>
        public MurmurHash32X32V3()
            : this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MurmurHash32X32V3"/> class, using the specified seed value.
        /// </summary>
        /// <param name="seed">A number used to calculate a starting value.</param>
        public MurmurHash32X32V3(int seed)
            : this((uint)seed)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MurmurHash32X32V3"/> class, using the specified seed value.
        /// </summary>
        /// <param name="seed">A number used to calculate a starting value.</param>
        public MurmurHash32X32V3(uint seed)
        {
            _seed = seed;
            _h1 = seed;
        }

        /// <inheritdoc cref="HashAlgorithm.HashSize"/>
        public override int HashSize => 32;

        /// <summary>
        /// Initializes an implementation of the <see cref="MurmurHash32X32V3"/> class.
        /// </summary>
        public override void Initialize()
        {
            _length = 0;
            _h1 = _seed;
        }

        /// <inheritdoc cref="HashAlgorithm.HashCore"/>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            var pos = ibStart;
            var remaining = (uint)cbSize;
            _length += remaining;

            unsafe
            {
                fixed (byte* b = array)
                {
                    while (remaining >= HashSizeBytes)
                    {
                        var k1 = HashExtensions.GetUInt32(b, pos);

                        pos += 4;
                        remaining -= HashSizeBytes;

                        _h1 ^= MixKey1(k1);
                        _h1 = _h1.RotateLeft(13);
                        _h1 = (_h1 * 5) + 0xe6546b64;
                    }

                    // if the input MOD 16 != 0
                    if (remaining > 0)
                    {
                        uint k1 = 0;

                        switch (remaining)
                        {
                            case 3:
                                k1 ^= (uint)b[pos + 2] << 16; // fall through
                                goto case 2;
                            case 2:
                                k1 ^= (uint)b[pos + 1] << 8; // fall through
                                goto case 1;
                            case 1:
                                k1 ^= (uint)b[pos]; // fall through
                                break;
                            default:
                                throw new Exception("Something went wrong with remaining bytes calculation.");
                        }

                        _h1 ^= MixKey1(k1);
                    }
                }
            }
        }

        /// <inheritdoc cref="HashAlgorithm.HashFinal"/>
        protected override byte[] HashFinal()
        {
            _h1 ^= _length;

            _h1 = MixFinal(_h1);

            unsafe
            {
                var hash = new byte[HashSizeBytes];
                fixed (byte* b = hash)
                {
                    *((uint*)b) = _h1;
                }

                return hash;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint MixKey1(uint k1)
        {
            k1 *= C1;
            k1 = k1.RotateLeft(15);
            k1 *= C2;
            return k1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint MixFinal(uint k)
        {
            // avalanche bits
            k ^= k >> 16;
            k *= 0x85ebca6b;
            k ^= k >> 13;
            k *= 0xc2b2ae35;
            k ^= k >> 16;
            return k;
        }
    }
}
