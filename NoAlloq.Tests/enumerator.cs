using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NoAlloq.Tests
{
    /// <summary>
    ///     Tests whether span enumerables can be used in <c>foreach</c> loops.
    /// </summary>
    public sealed class enumerator
    {
        [Fact]
        public void range()
        {
            var l = new List<int>();

            foreach (var e in SpanEnumerable.Range(1, 10))
                l.Add(e);

            Assert.Equal(
                new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                l.ToArray());
        }

        [Fact]
        public void range_boxed()
        {
            var l = new List<int>();

            foreach (var e in SpanEnumerable.Range(1, 10).Box())
                l.Add(e);

            Assert.Equal(
                new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                l.ToArray());
        }

        [Fact]
        public void select()
        {
            var l = new List<int>();

            ReadOnlySpan<int> span = stackalloc int[] { 1, 2, 3, 4 };

            foreach (var e in span.Select(x => x * 2))
                l.Add(e);

            Assert.Equal(
                new[] { 2, 4, 6, 8 },
                l.ToArray());
        }

        [Fact]
        public void order_by()
        {
            var l = new List<int>();

            Span<int> span = stackalloc int[] { 4, 1, 3, 2 };

            foreach (var e in span.OrderByValue(span))
                l.Add(e);

            Assert.Equal(
                new[] { 1, 2, 3, 4 },
                l.ToArray());
        }
    }
}
