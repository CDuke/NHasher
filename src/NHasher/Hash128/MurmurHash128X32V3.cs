using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace NHasher
{
    /// <summary>
    /// Implementation of 128 bit MurmurHash3 hash algoritm specified at <see href="https://github.com/aappleby/smhasher"/>.
    /// </summary>
    public class MurmurHash128X32V3 : HashAlgorithm
    {
        private const int HashSizeBytes = 16;
        private const uint C1 = 0x239b961b;
        private const uint C2 = 0xab0e9789;
        private const uint C3 = 0x38b34ae5;
        private const uint C4 = 0xa1e38b93;

        private readonly uint _seed;

        private uint _length;
        private uint _h1;
        private uint _h2;
        private uint _h3;
        private uint _h4;

        /// <summary>
        /// Initializes a new instance of the <see cref="MurmurHash128X32V3"/> class.
        /// </summary>
        public MurmurHash128X32V3()
            : this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MurmurHash128X32V3"/> class, using the specified seed value.
        /// </summary>
        /// <param name="seed">A number used to calculate a starting value.</param>
        public MurmurHash128X32V3(int seed)
            : this((uint)seed)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MurmurHash128X32V3"/> class, using the specified seed value.
        /// </summary>
        /// <param name="seed">A number used to calculate a starting value.</param>
        public MurmurHash128X32V3(uint seed)
        {
            _seed = seed;
            _h1 = seed;
            _h2 = seed;
            _h3 = seed;
            _h4 = seed;
        }

        /// <inheritdoc cref="HashAlgorithm.HashSize"/>
        public override int HashSize => 128;

        /// <summary>
        /// Initializes an implementation of the <see cref="MurmurHash128X32V3"/> class.
        /// </summary>
        public override void Initialize()
        {
            _length = 0;
            _h1 = _seed;
            _h2 = _seed;
            _h3 = _seed;
            _h4 = _seed;
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
                        var k2 = HashExtensions.GetUInt32(b, pos + 4);
                        var k3 = HashExtensions.GetUInt32(b, pos + 8);
                        var k4 = HashExtensions.GetUInt32(b, pos + 12);

                        pos += 16;
                        remaining -= HashSizeBytes;

                        _h1 ^= MixKey1(k1);
                        _h1 = _h1.RotateLeft(19);
                        _h1 += _h2;
                        _h1 = (_h1 * 5) + 0x561ccd1b;

                        _h2 ^= MixKey2(k2);
                        _h2 = _h2.RotateLeft(17);
                        _h2 += _h3;
                        _h2 = (_h2 * 5) + 0x0bcaa747;

                        _h3 ^= MixKey3(k3);
                        _h3 = _h3.RotateLeft(15);
                        _h3 += _h4;
                        _h3 = (_h3 * 5) + 0x96cd1c35;

                        _h4 ^= MixKey4(k4);
                        _h4 = _h4.RotateLeft(13);
                        _h4 += _h1;
                        _h4 = (_h4 * 5) + 0x32ac3b17;
                    }

                    // if the input MOD 16 != 0
                    if (remaining > 0)
                    {
                        uint k1 = 0;
                        uint k2 = 0;
                        uint k3 = 0;
                        uint k4 = 0;

                        switch (remaining)
                        {
                            case 15:
                                k4 ^= (uint)b[pos + 14] << 16; // fall through
                                goto case 14;
                            case 14:
                                k4 ^= (uint)b[pos + 13] << 8; // fall through
                                goto case 13;
                            case 13:
                                k4 ^= (uint)b[pos + 12]; // fall through
                                goto case 12;
                            case 12:
                                k3 ^= (uint)b[pos + 11] << 24; // fall through
                                goto case 11;
                            case 11:
                                k3 ^= (uint)b[pos + 10] << 16; // fall through
                                goto case 10;
                            case 10:
                                k3 ^= (uint)b[pos + 9] << 8; // fall through
                                goto case 9;
                            case 9:
                                k3 ^= (uint)b[pos + 8]; // fall through
                                goto case 8;
                            case 8:
                                k2 ^= (uint)b[pos + 7] << 24; // fall through
                                goto case 7;
                            case 7:
                                k2 ^= (uint)b[pos + 6] << 16; // fall through
                                goto case 6;
                            case 6:
                                k2 ^= (uint)b[pos + 5] << 8; // fall through
                                goto case 5;
                            case 5:
                                k2 ^= (uint)b[pos + 4]; // fall through
                                goto case 4;
                            case 4:
                                k1 ^= (uint)b[pos + 3] << 24; // fall through
                                goto case 3;
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
                        _h2 ^= MixKey2(k2);
                        _h3 ^= MixKey3(k3);
                        _h4 ^= MixKey4(k4);
                    }
                }
            }
        }

        /// <inheritdoc cref="HashAlgorithm.HashFinal"/>
        protected override byte[] HashFinal()
        {
            _h1 ^= _length;
            _h2 ^= _length;
            _h3 ^= _length;
            _h4 ^= _length;

            _h1 += _h2 + _h3 + _h4;
            _h2 += _h1;
            _h3 += _h1;
            _h4 += _h1;

            _h1 = MixFinal(_h1);
            _h2 = MixFinal(_h2);
            _h3 = MixFinal(_h3);
            _h4 = MixFinal(_h4);

            _h1 += _h2 + _h3 + _h4;
            _h2 += _h1;
            _h3 += _h1;
            _h4 += _h1;

            unsafe
            {
                var hash = new byte[HashSizeBytes];
                fixed (byte* b = hash)
                {
                    *((uint*)b) = _h1;
                    *((uint*)(b + 4)) = _h2;
                    *((uint*)(b + 8)) = _h3;
                    *((uint*)(b + 12)) = _h4;
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
        private static uint MixKey2(uint k2)
        {
            k2 *= C2;
            k2 = k2.RotateLeft(16);
            k2 *= C3;
            return k2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint MixKey3(uint k3)
        {
            k3 *= C3;
            k3 = k3.RotateLeft(17);
            k3 *= C4;
            return k3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint MixKey4(uint k4)
        {
            k4 *= C4;
            k4 = k4.RotateLeft(18);
            k4 *= C1;
            return k4;
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
