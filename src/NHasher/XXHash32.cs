using System;
using System.Security.Cryptography;

namespace NHasher
{
    /// <summary>
    /// Implementation of 32 bit xxHash algoritm specified at <see href="https://github.com/Cyan4973/xxHash"/>.
    /// </summary>
    public sealed class XXHash32 : HashAlgorithm
    {
        private const int HashSizeBytes = 4;
        private const int BufferSize = 16;

        private const uint Prime1 = 2654435761U;
        private const uint Prime2 = 2246822519U;
        private const uint Prime3 = 3266489917U;
        private const uint Prime4 = 668265263U;
        private const uint Prime5 = 374761393U;

        private readonly uint _seed;

        private readonly byte[] _buffer;
        private uint _length;
        private uint _v1;
        private uint _v2;
        private uint _v3;
        private uint _v4;
        private int _bufUsed;

        /// <summary>
        /// Initializes a new instance of the <see cref="XXHash32"/> class.
        /// </summary>
        public XXHash32()
            : this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XXHash32"/> class, using the specified seed value.
        /// </summary>
        /// <param name="seed">A number used to calculate a starting value.</param>
        public XXHash32(uint seed)
        {
            _seed = seed;
            _buffer = new byte[BufferSize];
            Reset();
        }

        /// <inheritdoc cref="HashAlgorithm.HashSize"/>
        public override int HashSize => 64;

        /// <summary>
        /// Initializes an implementation of the <see cref="XXHash32"/> class.
        /// </summary>
        public override void Initialize()
        {
            Reset();
        }

        /// <inheritdoc cref="HashAlgorithm.HashCore"/>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            var n = cbSize - ibStart;
            _length += (uint)n;

            var r = BufferSize - _bufUsed;
            if (n < r)
            {
                Array.Copy(array, ibStart, _buffer, _bufUsed, n);
                _bufUsed += n;
                return;
            }

            // some data left from previous update
            var p = 0;
            if (_bufUsed > 0)
            {
                Array.Copy(array, ibStart, _buffer, _bufUsed, r);
                _v1 = Round(_v1, _buffer, 0);
                _v2 = Round(_v2, _buffer, 4);
                _v3 = Round(_v3, _buffer, 8);
                _v4 = Round(_v4, _buffer, 12);
                p = r;
                _bufUsed = 0;
            }

            if (p + BufferSize <= n)
            {
                var limit = n - BufferSize;
                do
                {
                    _v1 = Round(_v1, array, p);
                    p += 4;
                    _v2 = Round(_v2, array, p);
                    p += 4;
                    _v3 = Round(_v3, array, p);
                    p += 4;
                    _v4 = Round(_v4, array, p);
                    p += 4;
                }
                while (p <= limit);
            }

            var tailLength = n - p;
            if (tailLength > 0)
            {
                Array.Copy(array, p, _buffer, _bufUsed, tailLength);
                _bufUsed += tailLength;
            }
        }

        /// <inheritdoc cref="HashAlgorithm.HashFinal"/>
        protected override unsafe byte[] HashFinal()
        {
            uint h;

            if (_length >= BufferSize)
            {
                h = _v1.RotateLeft(1) + _v2.RotateLeft(7) + _v3.RotateLeft(12) + _v4.RotateLeft(18);
            }
            else
            {
                h = _v3 + Prime5;
            }

            h += _length;

            var p = 0;
            while (p + 4 <= _bufUsed)
            {
                h += _buffer.GetUInt32(p) * Prime3;
                h = h.RotateLeft(17) * Prime4;
                p += 4;
            }

            while (p < _bufUsed)
            {
                h += _buffer[p] * Prime5;
                h = h.RotateLeft(11) * Prime1;
                p++;
            }

            h ^= h >> 15;
            h *= Prime2;
            h ^= h >> 13;
            h *= Prime3;
            h ^= h >> 16;

            var hash = new byte[HashSizeBytes];
            fixed (byte* b = &hash[0])
            {
                *((uint*)b) = h;
            }

            return hash;
        }

        private void Reset()
        {
            _length = 0;
            _v1 = _seed + Prime1 + Prime2;
            _v2 = _seed + Prime2;
            _v3 = _seed;
            _v4 = _seed - Prime1;
            _bufUsed = 0;
        }

        private static uint Round(uint acc, byte[] buffer, int position)
        {
            var input = buffer.GetUInt32(position);
            return Round(acc, input);
        }

        private static uint Round(uint acc, uint input)
        {
            acc += input * Prime2;
            acc = acc.RotateLeft(13);
            acc *= Prime1;
            return acc;
        }
    }
}
