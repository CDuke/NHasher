using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace NHasher
{
    /// <summary>
    /// Implementation of 128 bit MurmurHash3 hash algoritm specified at <see href="https://github.com/aappleby/smhasher"/>.
    /// </summary>
    public sealed class MurmurHash128X64V3 : HashAlgorithm
    {
        private const int HashSizeBytes = 16;
        private const ulong C1 = 0x87c37b91114253d5L;
        private const ulong C2 = 0x4cf5ad432745937fL;

        private readonly ulong _seed;

        private ulong _length;
        private ulong _h1;
        private ulong _h2;

        /// <summary>
        /// Initializes a new instance of the <see cref="MurmurHash128X64V3"/> class.
        /// </summary>
        public MurmurHash128X64V3()
            : this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MurmurHash128X64V3"/> class, using the specified seed value.
        /// </summary>
        /// <param name="seed">A number used to calculate a starting value.</param>
        public MurmurHash128X64V3(int seed)
            : this((ulong)seed)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MurmurHash128X64V3"/> class, using the specified seed value.
        /// </summary>
        /// <param name="seed">A number used to calculate a starting value.</param>
        public MurmurHash128X64V3(uint seed)
            : this((ulong)seed)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MurmurHash128X64V3"/> class, using the specified seed value.
        /// </summary>
        /// <param name="seed">A number used to calculate a starting value.</param>
        public MurmurHash128X64V3(ulong seed)
        {
            _seed = seed;
            _h1 = seed;
            _h2 = seed;
        }

        /// <inheritdoc cref="HashAlgorithm.HashSize"/>
        public override int HashSize => 128;

        /// <summary>
        /// Initializes an implementation of the <see cref="MurmurHash128X64V3"/> class.
        /// </summary>
        public override void Initialize()
        {
            _length = 0L;
            _h1 = _seed;
            _h2 = _seed;
        }

        /// <inheritdoc cref="HashAlgorithm.HashCore"/>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            var pos = ibStart;
            var remaining = (ulong)cbSize;
            _length += remaining;

            unsafe
            {
                fixed (byte* b = array)
                {
                    while (remaining >= HashSizeBytes)
                    {
                        var k1 = HashExtensions.GetUInt64(b, pos);
                        var k2 = HashExtensions.GetUInt64(b, pos + 8);

                        pos += 16;
                        remaining -= HashSizeBytes;

                        _h1 ^= MixKey1(k1);
                        _h1 = _h1.RotateLeft(27);
                        _h1 += _h2;
                        _h1 = (_h1 * 5) + 0x52dce729;

                        _h2 ^= MixKey2(k2);
                        _h2 = _h2.RotateLeft(31);
                        _h2 += _h1;
                        _h2 = (_h2 * 5) + 0x38495ab5;
                    }

                    // if the input MOD 16 != 0
                    if (remaining > 0)
                    {
                        ulong k1 = 0;
                        ulong k2 = 0;

                        switch (remaining)
                        {
                            case 15:
                                k2 ^= (ulong)b[pos + 14] << 48; // fall through
                                goto case 14;
                            case 14:
                                k2 ^= (ulong)b[pos + 13] << 40; // fall through
                                goto case 13;
                            case 13:
                                k2 ^= (ulong)b[pos + 12] << 32; // fall through
                                goto case 12;
                            case 12:
                                k2 ^= (ulong)b[pos + 11] << 24; // fall through
                                goto case 11;
                            case 11:
                                k2 ^= (ulong)b[pos + 10] << 16; // fall through
                                goto case 10;
                            case 10:
                                k2 ^= (ulong)b[pos + 9] << 8; // fall through
                                goto case 9;
                            case 9:
                                k2 ^= (ulong)b[pos + 8]; // fall through
                                goto case 8;
                            case 8:
                                k1 ^= (ulong)b[pos + 7] << 56; // fall through
                                goto case 7;
                            case 7:
                                k1 ^= (ulong)b[pos + 6] << 48; // fall through
                                goto case 6;
                            case 6:
                                k1 ^= (ulong)b[pos + 5] << 40; // fall through
                                goto case 5;
                            case 5:
                                k1 ^= (ulong)b[pos + 4] << 32; // fall through
                                goto case 4;
                            case 4:
                                k1 ^= (ulong)b[pos + 3] << 24; // fall through
                                goto case 3;
                            case 3:
                                k1 ^= (ulong)b[pos + 2] << 16; // fall through
                                goto case 2;
                            case 2:
                                k1 ^= (ulong)b[pos + 1] << 8; // fall through
                                goto case 1;
                            case 1:
                                k1 ^= (ulong)b[pos]; // fall through
                                break;
                            default:
                                throw new Exception("Something went wrong with remaining bytes calculation.");
                        }

                        _h1 ^= MixKey1(k1);
                        _h2 ^= MixKey2(k2);
                    }
                }
            }
        }

        /// <inheritdoc cref="HashAlgorithm.HashFinal"/>
        protected override byte[] HashFinal()
        {
            _h1 ^= _length;
            _h2 ^= _length;

            _h1 += _h2;
            _h2 += _h1;

            _h1 = MixFinal(_h1);
            _h2 = MixFinal(_h2);

            _h1 += _h2;
            _h2 += _h1;

            unsafe
            {
                var hash = new byte[HashSizeBytes];
                fixed (byte* b = hash)
                {
                    *((ulong*)b) = _h1;
                    *((ulong*)(b + 8)) = _h2;
                }

                return hash;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong MixKey1(ulong k1)
        {
            k1 *= C1;
            k1 = k1.RotateLeft(31);
            k1 *= C2;
            return k1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong MixKey2(ulong k2)
        {
            k2 *= C2;
            k2 = k2.RotateLeft(33);
            k2 *= C1;
            return k2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong MixFinal(ulong k)
        {
            // avalanche bits
            k ^= k >> 33;
            k *= 0xff51afd7ed558ccdL;
            k ^= k >> 33;
            k *= 0xc4ceb9fe1a85ec53L;
            k ^= k >> 33;
            return k;
        }
    }
}
