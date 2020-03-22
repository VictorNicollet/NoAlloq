using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoAlloq.Tests.Benchmark
{
    [SimpleJob(RuntimeMoniker.CoreRt31), RankColumn, MemoryDiagnoser]
    public class Sum
    {
        [Benchmark]
        public void Manual()
        {
            Span<int> values = stackalloc int[] {
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            Sum(values);

            void Sum(Span<int> v)
            {
                int accum = 0;
                for (var i = 0; i < v.Length; ++i)
                    accum += v[i] * 2;
            }
        }

        [Benchmark]
        public void WithStruct()
        {
            Span<int> values = stackalloc int[] {
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            var sum = values
                .Select<int, int, select.Times>(new select.Times(2))
                .Sum();
        }

        [Benchmark]
        public void WithDelegate()
        {
            Span<int> values = stackalloc int[] {
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            var sum = values.Sum(x => x * 2);
        }

        [Benchmark(Baseline = true)]
        public void WithLinq()
        {
            var values = new int[] {
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            var sum = System.Linq.Enumerable.Sum(values, v => v * 2);
        }
    }
}
