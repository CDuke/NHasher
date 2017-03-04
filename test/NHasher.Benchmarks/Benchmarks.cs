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

        private readonly MurmurHash128X64V3 _murmurHash128X64V3 = new MurmurHash128X64V3();
        private readonly MurmurHash128X32V3 _murmurHash128X32V3 = new MurmurHash128X32V3();
        private readonly MurmurHash32X32V3 _murmurHash32X32V3 = new MurmurHash32X32V3();
        private readonly XXHash32 _xxHash32 = new XXHash32();
        private readonly XXHash64 _xxHash64 = new XXHash64();
        private readonly Adler32 _adler32 = new Adler32();

        [Params(4, 11, 25, 100, 1000, 10000)]
        public int PayloadLength { get; set; }

        [Setup]
        public void SetupData()
        {
            _data = new byte[PayloadLength];
            new Random(42).NextBytes(_data);
        }

        [Benchmark]
        public byte[] MurmurHash128X64V3() => _murmurHash128X64V3.ComputeHash(_data);

        [Benchmark]
        public byte[] MurmurHash128X32V3() => _murmurHash128X32V3.ComputeHash(_data);

        [Benchmark]
        public byte[] MurmurHash32X32V3() => _murmurHash32X32V3.ComputeHash(_data);

        [Benchmark]
        public byte[] XXHash32() => _xxHash32.ComputeHash(_data);

        [Benchmark]
        public byte[] XXHash64() => _xxHash64.ComputeHash(_data);

        [Benchmark]
        public byte[] Adler32() => _adler32.ComputeHash(_data);
    }
}
