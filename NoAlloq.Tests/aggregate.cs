using System;
using Xunit;

namespace NoAlloq.Tests
{
    public sealed class aggregate
    {
        [Fact]
        public void aggregate3_span()
        { 
            Span<int> values = stackalloc int[] { 1, 2, 4, 8, 16 };
            Assert.Equal(33, values.Aggregate(1,
                (a, n) =>
                {
                    Assert.Equal(a, n);
                    return a + n;
                },
                a => a + 1));
        }

        [Fact]
        public void aggregate3_span_boxed()
        {
            Span<int> values = stackalloc int[] { 1, 2, 4, 8, 16 };
            Assert.Equal(33, values.Box().Aggregate(1,
                (a, n) =>
                {
                    Assert.Equal(a, n);
                    return a + n;
                },
                a => a + 1));
        }

        [Fact]
        public void aggregate3_readonly_span()
        {
            ReadOnlySpan<int> values = stackalloc int[] { 1, 2, 4, 8, 16 };
            Assert.Equal(33, values.Aggregate(1,
                (a, n) =>
                {
                    Assert.Equal(a, n);
                    return a + n;
                },
                a => a + 1));
        }

        [Fact]
        public void aggregate3_readonly_span_boxed()
        {
            ReadOnlySpan<int> values = stackalloc int[] { 1, 2, 4, 8, 16 };
            Assert.Equal(33, values.Box().Aggregate(1,
                (a, n) =>
                {
                    Assert.Equal(a, n);
                    return a + n;
                },
                a => a + 1));
        }

        [Fact]
        public void aggregate3_range()
        {
            Assert.Equal(33, SpanEnumerable.Range(0, 5).Select(i => 1 << i)
                .Aggregate(1,
                    (a, n) =>
                    {
                        Assert.Equal(a, n);
                        return a + n;
                    },
                    a => a + 1));
        }

        [Fact]
        public void aggregate3_range_boxed()
        {
            Assert.Equal(33, SpanEnumerable.Range(0, 5).Select(i => 1 << i)
                .Box().Aggregate(1,
                    (a, n) =>
                    {
                        Assert.Equal(a, n);
                        return a + n;
                    },
                    a => a + 1));
        }

        [Fact]
        public void aggregate3_select()
        {
            Span<int> values = stackalloc int[] { 0, 1, 2, 3, 4 };
            Assert.Equal(33, values.Select(i => 1 << i).Aggregate(1,
                (a, n) =>
                {
                    Assert.Equal(a, n);
                    return a + n;
                },
                a => a + 1));
        }

        [Fact]
        public void aggregate3_select_boxed()
        {
            Span<int> values = stackalloc int[] { 0, 1, 2, 3, 4 };
            Assert.Equal(33, values.Select(i => 1 << i).Box().Aggregate(1,
                (a, n) =>
                {
                    Assert.Equal(a, n);
                    return a + n;
                },
                a => a + 1));
        }

        [Fact]
        public void aggregate2_span()
        {
            Span<int> values = stackalloc int[] { 1, 2, 4, 8, 16 };
            Assert.Equal(32, values.Aggregate(1,
                (a, n) =>
                {
                    Assert.Equal(a, n);
                    return a + n;
                }));
        }

        [Fact]
        public void aggregate2_span_boxed()
        {
            Span<int> values = stackalloc int[] { 1, 2, 4, 8, 16 };
            Assert.Equal(32, values.Box().Aggregate(1,
                (a, n) =>
                {
                    Assert.Equal(a, n);
                    return a + n;
                }));
        }

        [Fact]
        public void aggregate2_readonly_span()
        {
            ReadOnlySpan<int> values = stackalloc int[] { 1, 2, 4, 8, 16 };
            Assert.Equal(32, values.Aggregate(1,
                (a, n) =>
                {
                    Assert.Equal(a, n);
                    return a + n;
                }));
        }

        [Fact]
        public void aggregate2_readonly_span_boxed()
        {
            ReadOnlySpan<int> values = stackalloc int[] { 1, 2, 4, 8, 16 };
            Assert.Equal(32, values.Box().Aggregate(1,
                (a, n) =>
                {
                    Assert.Equal(a, n);
                    return a + n;
                }));
        }

        [Fact]
        public void aggregate2_range()
        {
            Assert.Equal(32, SpanEnumerable.Range(0, 5).Select(i => 1 << i)
                .Aggregate(1,
                    (a, n) =>
                    {
                        Assert.Equal(a, n);
                        return a + n;
                    }));
        }

        [Fact]
        public void aggregate2_range_boxed()
        {
            Assert.Equal(32, SpanEnumerable.Range(0, 5).Select(i => 1 << i)
                .Box().Aggregate(1,
                    (a, n) =>
                    {
                        Assert.Equal(a, n);
                        return a + n;
                    }));
        }

        [Fact]
        public void aggregate2_select()
        {
            Span<int> values = stackalloc int[] { 0, 1, 2, 3, 4 };
            Assert.Equal(32, values.Select(i => 1 << i).Aggregate(1,
                (a, n) =>
                {
                    Assert.Equal(a, n);
                    return a + n;
                }));
        }

        [Fact]
        public void aggregate2_select_boxed()
        {
            Span<int> values = stackalloc int[] { 0, 1, 2, 3, 4 };
            Assert.Equal(32, values.Select(i => 1 << i).Box().Aggregate(1,
                (a, n) =>
                {
                    Assert.Equal(a, n);
                    return a + n;
                }));
        }

        [Fact]
        public void aggregate1_span()
        {
            Span<int> values = stackalloc int[] { 1, 1, 2, 4, 8, 16 };
            Assert.Equal(32, values.Aggregate(
                (a, n) =>
                {
                    Assert.Equal(a, n);
                    return a + n;
                }));
        }

        [Fact]
        public void aggregate1_span_boxed()
        {
            Span<int> values = stackalloc int[] { 1, 1, 2, 4, 8, 16 };
            Assert.Equal(32, values.Box().Aggregate(
                (a, n) =>
                {
                    Assert.Equal(a, n);
                    return a + n;
                }));
        }

        [Fact]
        public void aggregate1_readonly_span()
        {
            ReadOnlySpan<int> values = stackalloc int[] { 1, 1, 2, 4, 8, 16 };
            Assert.Equal(32, values.Aggregate(
                (a, n) =>
                {
                    Assert.Equal(a, n);
                    return a + n;
                }));
        }

        [Fact]
        public void aggregate1_readonly_span_boxed()
        {
            ReadOnlySpan<int> values = stackalloc int[] { 1, 1, 2, 4, 8, 16 };
            Assert.Equal(32, values.Box().Aggregate(
                (a, n) =>
                {
                    Assert.Equal(a, n);
                    return a + n;
                }));
        }

        [Fact]
        public void aggregate1_range()
        {
            Assert.Equal(32, SpanEnumerable.Range(-1, 6).Select(i => 1 << Math.Max(0, i))
                .Aggregate(
                    (a, n) =>
                    {
                        Assert.Equal(a, n);
                        return a + n;
                    }));
        }

        [Fact]
        public void aggregate1_range_boxed()
        {
            Assert.Equal(32, SpanEnumerable.Range(-1, 6).Select(i => 1 << Math.Max(0, i))
                .Box().Aggregate(
                    (a, n) =>
                    {
                        Assert.Equal(a, n);
                        return a + n;
                    }));
        }

        [Fact]
        public void aggregate1_select()
        {
            Span<int> values = stackalloc int[] { 0,  0, 1, 2, 3, 4 };
            Assert.Equal(32, values.Select(i => 1 << i).Aggregate(
                (a, n) =>
                {
                    Assert.Equal(a, n);
                    return a + n;
                }));
        }

        [Fact]
        public void aggregate1_select_boxed()
        {
            Span<int> values = stackalloc int[] { 0, 0, 1, 2, 3, 4 };
            Assert.Equal(32, values.Select(i => 1 << i).Box().Aggregate(
                (a, n) =>
                {
                    Assert.Equal(a, n);
                    return a + n;
                }));
        }
    }
}
