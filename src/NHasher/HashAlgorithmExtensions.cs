using System.IO;
using System.Security.Cryptography;

namespace NHasher
{
    /// <summary>
    /// Additional method for <see cref="HashAlgorithm"/> class.
    /// </summary>
    public static class HashAlgorithmExtensions
    {
        /// <summary>
        /// Compute the hash value for the specified byte array and return string hash representation.
        /// </summary>
        /// <param name="hashAlgorithm"><see cref="HashAlgorithm"/> instance.</param>
        /// <param name="buffer">The input to compute the hash code for.</param>
        /// <returns>String hash representation.</returns>
        public static string ComputeHashAsString(this HashAlgorithm hashAlgorithm, byte[] buffer)
        {
            var hash = hashAlgorithm.ComputeHash(buffer);
            return HashToString(hash);
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="Stream"/> object and return string hash representation.
        /// </summary>
        /// <param name="hashAlgorithm"><see cref="HashAlgorithm"/> instance.</param>
        /// <param name="inputStream">The input to compute the hash code for.</param>
        /// <returns>String hash representation.</returns>
        public static string ComputeHashAsString(this HashAlgorithm hashAlgorithm, Stream inputStream)
        {
            var hash = hashAlgorithm.ComputeHash(inputStream);
            return HashToString(hash);
        }

        /// <summary>
        /// Computes the hash value for the specified region of the specified byte array and return string hash representation.
        /// </summary>
        /// <param name="hashAlgorithm"><see cref="HashAlgorithm"/> instance.</param>
        /// <param name="buffer">The input to compute the hash code for.</param>
        /// <param name="offset">The offset into the byte array from which to begin using data.</param>
        /// <param name="count">The number of bytes in the array to use as data.</param>
        /// <returns>String hash representation.</returns>
        public static string ComputeHashAsString(this HashAlgorithm hashAlgorithm, byte[] buffer, int offset, int count)
        {
            var hash = hashAlgorithm.ComputeHash(buffer, offset, count);
            return HashToString(hash);
        }

        private static string HashToString(byte[] hash)
        {
            var length1 = hash.Length * 2;
            var charHashArray = new char[length1];
            var num1 = 0;
            var index = 0;
            while (index < length1)
            {
                var num2 = hash[num1++];
                charHashArray[index] = GetHexValue(num2 / 16);
                charHashArray[index + 1] = GetHexValue(num2 % 16);
                index += 2;
            }

            return new string(charHashArray, 0, length1);
        }

        private static char GetHexValue(int i)
        {
            if (i < 10)
            {
                return (char)(i + 48);
            }

            return (char)(i - 10 + 65);
        }
    }
}
