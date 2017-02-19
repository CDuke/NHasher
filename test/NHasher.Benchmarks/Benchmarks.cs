using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;

namespace NHasher.Benchmarks
{
    [ClrJob, CoreJob]
    [MemoryDiagnoser]
    public class Benchmarks
    {
        private byte[] _data;

        private readonly MurmurHash3X64L128 _murmurHash3X64L128 = new MurmurHash3X64L128();
        private readonly XXHash32 _xxHash32 = new XXHash32();
        private readonly XXHash64 _xxHash64 = new XXHash64();

        [Params(4, 11, 25, 100, 1000, 10000)]
        public int PayloadLength { get; set; }

        [Setup]
        public void SetupData()
        {
            _data = new byte[PayloadLength];
            new Random(42).NextBytes(_data);
        }

        [Benchmark]
        public byte[] MurmurHash3X64L128() => _murmurHash3X64L128.ComputeHash(_data);

        [Benchmark]
        public byte[] XXHash32() => _xxHash32.ComputeHash(_data);

        [Benchmark]
        public byte[] XXHash64() => _xxHash64.ComputeHash(_data);
    }
}
