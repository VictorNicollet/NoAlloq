using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;

namespace NoAlloq.Tests.Benchmark
{
    [SimpleJob(RuntimeMoniker.CoreRt31), RankColumn, MemoryDiagnoser]
    public class Select
    {
        [Benchmark]
        public int Manual()
        {
            Span<int> values = stackalloc int[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            MultiplyByTwo(values);

            void MultiplyByTwo(Span<int> v)
            {
                for (var i = 0; i < v.Length; ++i)
                    v[i] *= 2;
            }

            return values[7];
        }

        [Benchmark]
        public int WithStruct()
        {
            Span<int> values = stackalloc int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            return values
                .Select<int, int, select.Times>(new select.Times(2))
                .CopyInto(values)
                .Length;
        }

        [Benchmark]
        public int WithDelegate()
        {
            Span<int> values = stackalloc int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            return values
                .Select(v => v * 2)
                .CopyInto(values)
                .Length;
        }

        [Benchmark(Baseline = true)]
        public int[] WithLinq()
        {
            var values = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            return System.Linq.Enumerable.ToArray(
                System.Linq.Enumerable.Select(values, x => x * 2));
        }
    }
}
