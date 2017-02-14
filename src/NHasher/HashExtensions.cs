namespace NHasher
{
    internal static class HashExtensions
    {
        public static ulong RotateLeft(this ulong original, int bits)
        {
            return (original << bits) | (original >> (64 - bits));
        }

        internal static unsafe ulong GetUInt64(this byte[] data, int position)
        {
            // we only read aligned longs, so a simple casting is enough
            fixed (byte* pbyte = &data[position])
            {
                return *((ulong*)pbyte);
            }
        }

        internal static unsafe uint GetUInt32(this byte[] data, int position)
        {
            // we only read aligned longs, so a simple casting is enough
            fixed (byte* pbyte = &data[position])
            {
                return *((uint*)pbyte);
            }
        }
    }
}