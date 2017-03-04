using System.Runtime.CompilerServices;

namespace NHasher
{
    internal static class HashExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong RotateLeft(this ulong original, int bits)
        {
            return (original << bits) | (original >> (64 - bits));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateLeft(this uint original, int bits)
        {
            return (original << bits) | (original >> (32 - bits));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe ulong GetUInt64(this byte[] data, int position)
        {
            // we only read aligned longs, so a simple casting is enough
            fixed (byte* pbyte = &data[position])
            {
                return *((ulong*)pbyte);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe ulong GetUInt64(byte* data, int position)
        {
            // we only read aligned longs, so a simple casting is enough
            return *((ulong*)&data[position]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe uint GetUInt32(this byte[] data, int position)
        {
            // we only read aligned longs, so a simple casting is enough
            fixed (byte* pbyte = &data[position])
            {
                return *((uint*)pbyte);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe uint GetUInt32(byte* data, int position)
        {
            // we only read aligned longs, so a simple casting is enough
            return *((uint*)&data[position]);
        }
    }
}
