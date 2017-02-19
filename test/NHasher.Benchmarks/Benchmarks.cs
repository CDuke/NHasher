using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Order;

namespace NHasher.Benchmarks
{
    [ClrJob, CoreJob]
    [MemoryDiagnoser]
    public class Benchmarks
    {
        private const int N = 10000;
        private readonly byte[] _data;

        private readonly MurmurHash3X64L128 _murmurHash3X64L128 = new MurmurHash3X64L128();
        private readonly XXHash32 _xxHash32 = new XXHash32();
        private readonly XXHash64 _xxHash64 = new XXHash64();

        public Benchmarks()
        {
            _data = new byte[N];
            new Random(42).NextBytes(_data);
        }

        [Benchmark]
        public byte[] MurmurHash3X64L128()
        {
            return _murmurHash3X64L128.ComputeHash(_data);
        }

        [Benchmark]
        public byte[] XXHash32()
        {
            return _xxHash32.ComputeHash(_data);
        }

        [Benchmark]
        public byte[] XXHash64()
        {
            return _xxHash64.ComputeHash(_data);
        }
    }
}
