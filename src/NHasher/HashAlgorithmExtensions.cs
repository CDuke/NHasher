using System.IO;
using System.Security.Cryptography;

namespace NHasher
{
    public static class HashAlgorithmExtensions
    {
        public static string ComputeHashAsString(this HashAlgorithm hashAlgorithm, byte[] buffer)
        {
            var hash = hashAlgorithm.ComputeHash(buffer);
            return HashToString(hash);
        }

        public static string ComputeHashAsString(this HashAlgorithm hashAlgorithm, Stream inputStream)
        {
            var hash = hashAlgorithm.ComputeHash(inputStream);
            return HashToString(hash);
        }

        public static string ComputeHashAsString(this HashAlgorithm hashAlgorithm, byte[] buffer, int offset, int count)
        {
            var hash = hashAlgorithm.ComputeHash(buffer, offset, count);
            return HashToString(hash);
        }

        private static string HashToString(byte[] hash)
        {
            var length1 = hash.Length * 2;
            var chArray = new char[length1];
            var num1 = 0;
            var index = 0;
            while (index < length1)
            {
                var num2 = hash[num1++];
                chArray[index] = GetHexValue(num2 / 16);
                chArray[index + 1] = GetHexValue(num2 % 16);
                index += 2;
            }
            return new string(chArray, 0, length1);
        }

        private static char GetHexValue(int i)
        {
            if (i < 10)
                return (char)(i + 48);
            return (char)(i - 10 + 65);
        }
    }
}