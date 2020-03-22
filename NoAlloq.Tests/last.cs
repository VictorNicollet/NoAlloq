using System;
using Xunit;

namespace NoAlloq.Tests
{
    public sealed class last
    {
        public struct IsOdd : IMap<int, bool>
        {
            public bool Apply(int x) => x % 2 != 0;
        }

        [Fact]
        public void where_is_odd()
        {
            Span<int> values = stackalloc int[4] { 0, 1, 2, 3 };
            Assert.Equal(3, values.Where(new IsOdd()).Last());
        }

        [Fact]
        public void where_is_odd_none()
        {
            Span<int> values = stackalloc int[4] { 0, 2, 4, 6 };

            try
            {
                var found = values.Where(new IsOdd()).Last();
                Assert.True(false);
            }
            catch (InvalidOperationException e)
            {
                Assert.Equal(
                    "The input sequence is empty.",
                    e.Message);
            }
        }

        [Fact]
        public void span()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            Assert.Equal(4, values.Last());
        }

        [Fact]
        public void span_empty()
        {
            Span<int> values = stackalloc int[0];

            try
            {
                values.Last();
                Assert.True(false);
            }
            catch (InvalidOperationException e)
            {
                Assert.Equal(
                    "The input sequence is empty.",
                    e.Message);
            }
        }

        [Fact]
        public void readonly_span()
        {
            ReadOnlySpan<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            Assert.Equal(4, values.Last());
        }

        [Fact]
        public void readonly_span_empty()
        {
            ReadOnlySpan<int> values = stackalloc int[0];

            try
            {
                values.Last();
                Assert.True(false);
            }
            catch (InvalidOperationException e)
            {
                Assert.Equal(
                    "The input sequence is empty.",
                    e.Message);
            }
        }

        [Fact]
        public void is_odd_delegate()
        {
            Span<int> values = stackalloc int[4] { 0, 1, 2, 3 };
            Assert.Equal(3, values.Last(v => v % 2 != 0));
        }

        [Fact]
        public void is_odd_delegate_empty()
        {
            Span<int> values = stackalloc int[4] { 0, 2, 4, 6 };
            try
            {
                values.Last(v => v % 2 != 0);
                Assert.True(false);
            }
            catch (InvalidOperationException e)
            {
                Assert.Equal(
                    "The input sequence is empty.",
                    e.Message);
            }
        }

        [Fact]
        public void readonly_is_odd_delegate()
        {
            ReadOnlySpan<int> values = stackalloc int[4] { 0, 1, 2, 3 };
            Assert.Equal(3, values.Last(v => v % 2 != 0));
        }

        [Fact]
        public void readonly_is_odd_delegate_empty()
        {
            ReadOnlySpan<int> values = stackalloc int[4] { 0, 2, 4, 6 };
            try
            {
                values.Last(v => v % 2 != 0);
                Assert.True(false);
            }
            catch (InvalidOperationException e)
            {
                Assert.Equal(
                    "The input sequence is empty.",
                    e.Message);
            }
        }

        [Fact]
        public void plus_one_is_odd()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            Assert.Equal(5, values.Select(v => v + 1).Last(v => v % 2 != 0));
        }

        [Fact]
        public void range_odd()
        {
            Assert.Equal(29, SpanEnumerable.Range(1, 30).Last(v => v % 2 != 0));
        }

        [Fact]
        public void range()
        {
            Assert.Equal(30, SpanEnumerable.Range(1, 30).Last());
        }

        [Fact]
        public void range_empty()
        {
            try
            {
                SpanEnumerable.Range(1, 0).Last();
                Assert.True(false);
            }
            catch (InvalidOperationException e)
            {
                Assert.Equal(
                    "The input sequence is empty.",
                    e.Message);
            }
        }
    }
}
