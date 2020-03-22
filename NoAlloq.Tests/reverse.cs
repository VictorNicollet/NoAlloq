using System;
using Xunit;

namespace NoAlloq.Tests
{
    public sealed class reverse
    {
        [Fact]
        public void times_two_delegate()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            Span<int> result = stackalloc int[6];

            result = values.Select(v => v * 2).ReverseInto(result);

            Assert.Equal(4, result.Length);

            for (var i = 0; i < result.Length; ++i)
                Assert.Equal(2 * (4 - i), result[i]);
        }

        [Fact]
        public void in_place()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 4 };

            var result = values.ReverseInPlace();

            Assert.Equal(4, result.Length);

            for (var i = 0; i < result.Length; ++i)
                Assert.Equal(4 - i, result[i]);
        }

        [Fact]
        public void range()
        {
            Span<int> result = stackalloc int[6];

            result = SpanEnumerable.Range(1, 6).ReverseInto(result);

            Assert.Equal(6, result.Length);

            for (var i = 0; i < result.Length; ++i)
                Assert.Equal(6 - i, result[i]);
        }
    }
}
