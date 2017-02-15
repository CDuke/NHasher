using System;
using System.Security.Cryptography;

namespace NHasher
{
    public class XXHash64 : HashAlgorithm
    {
        private const int HashSizeBytes = 8;
        private const int BufferSize = 32;

        private readonly ulong _seed;

        private const ulong Prime1 = 11400714785074694791UL;
        private const ulong Prime2 = 14029467366897019727UL;
        private const ulong Prime3 = 1609587929392839161UL;
        private const ulong Prime4 = 9650029242287828579UL;
        private const ulong Prime5 = 2870177450012600261UL;

        private ulong _length;
        private ulong _v1;
        private ulong _v2;
        private ulong _v3;
        private ulong _v4;
        private int _bufUsed;
        private readonly byte[] _buffer;

        public override int HashSize => 64;

        public XXHash64() : this(0) { }

        public XXHash64(ulong seed)
        {
            _seed = seed;
            _buffer = new byte[BufferSize];
            Reset();
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            var n = cbSize - ibStart;
            _length += (ulong)n;

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
                _v2 = Round(_v2, _buffer, 8);
                _v3 = Round(_v3, _buffer, 16);
                _v4 = Round(_v4, _buffer, 24);
                p = r;
                _bufUsed = 0;
            }

            if (p + BufferSize <= n)
            {
                var limit = n - 32;
                do
                {
                    _v1 = Round(_v1, array, p);
                    p += 8;
                    _v2 = Round(_v2, array, p);
                    p += 8;
                    _v3 = Round(_v3, array, p);
                    p += 8;
                    _v4 = Round(_v4, array, p);
                    p += 8;
                } while (p <= limit);
            }

            var tailLength = n - p;
            if (tailLength > 0)
            {
                Array.Copy(array, p, _buffer, _bufUsed, tailLength);
                _bufUsed += tailLength;
            }
        }

        protected override unsafe byte[] HashFinal()
        {
            ulong h;

            if (_length >= BufferSize)
            {
                h = _v1.RotateLeft(1) + _v2.RotateLeft(7) + _v3.RotateLeft(12) + _v4.RotateLeft(18);
                h = MergeRound(h, _v1);
                h = MergeRound(h, _v2);
                h = MergeRound(h, _v3);
                h = MergeRound(h, _v4);
            }
            else
            {
                h = _v3 + Prime5;
            }

            h += _length;

            var p = 0;
            while (p + 8 <= _bufUsed)
            {
                var k1 = Round(0, _buffer, p);
                h ^= k1;
                h = h.RotateLeft(27) * Prime1 + Prime4;
                p += 8;
            }

            if (p + 4 <= _bufUsed)
            {
                h ^= _buffer.GetUInt32(p) * Prime1;

                h = h.RotateLeft(23) * Prime2 + Prime3;
                p += 4;
            }

            while (p < _bufUsed)
            {
                h ^= _buffer[p] * Prime5;
                h = h.RotateLeft(11) * Prime1;
                p++;
            }

            h ^= h >> 33;
            h *= Prime2;
            h ^= h >> 29;
            h *= Prime3;
            h ^= h >> 32;

            var hash = new byte[HashSizeBytes];
            fixed (byte* b = &hash[0])
            {
                *((ulong*)b) = h;
            }

            return hash;
        }

        public override void Initialize()
        {
            Reset();
        }

        private void Reset()
        {
            _length = 0L;
            _v1 = _seed + Prime1 + Prime2;
            _v2 = _seed + Prime2;
            _v3 = _seed;
            _v4 = _seed - Prime1;
            _bufUsed = 0;
        }

        private static ulong Round(ulong acc, byte[] buffer, int position)
        {
            var input = buffer.GetUInt64(position);
            return Round(acc, input);
        }


        private static ulong MergeRound(ulong acc, ulong val)
        {
            val = Round(0, val);
            acc ^= val;
            acc = acc * Prime1 + Prime4;
            return acc;
        }

        private static ulong Round(ulong acc, ulong input)
        {
            acc += input * Prime2;
            acc = acc.RotateLeft(31);
            acc *= Prime1;
            return acc;
        }
    }
}