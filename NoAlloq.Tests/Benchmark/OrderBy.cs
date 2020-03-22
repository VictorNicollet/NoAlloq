using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoAlloq.Tests.Benchmark
{
    [SimpleJob(RuntimeMoniker.CoreRt31), RankColumn, MemoryDiagnoser]
    public class OrderBy
    {
        [Benchmark]
        public void WithNoAlloq()
        {
            Span<int> values = stackalloc int[] {
                 91, 38, 29, 38, 81, 55, 17, 10, 40, 33,
                 19, 61, 85, 25, 41, 31, 28, 12, 93, 67
            };

            var first = values
                .OrderByValue(values)
                .Slice(0, 5)
                .CopyInto(values);
        }


        [Benchmark(Baseline = true)]
        public void WithLinq()
        {
            var values = new int[] {
                 91, 38, 29, 38, 81, 55, 17, 10, 40, 33,
                 19, 61, 85, 25, 41, 31, 28, 12, 93, 67
            };

            var sum = Enumerable.OrderBy(values, v => v)
                .Take(5)
                .ToArray();
        }
    }
}
