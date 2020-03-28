using System;
using Xunit;

namespace NoAlloq.Tests
{
    public sealed class first_or_default
    {
        public struct IsOdd : IMap<int, bool>
        {
            public bool Apply(int x) => x % 2 != 0;
        }

        [Fact]
        public void where_is_odd()
        {
            Span<int> values = stackalloc int[4] { 0, 1, 2, 3 };
            Assert.Equal(1, values.Where(new IsOdd()).FirstOrDefault());
        }

        [Fact]
        public void where_is_odd_boxed()
        {
            Span<int> values = stackalloc int[4] { 0, 1, 2, 3 };
            Assert.Equal(1, values.Where(new IsOdd()).Box().FirstOrDefault());
        }

        [Fact]
        public void where_is_odd_none()
        {
            Span<int> values = stackalloc int[4] { 2, 4, 6, 8 };
            Assert.Equal(0, values.Where(new IsOdd()).FirstOrDefault());
        }

        [Fact]
        public void where_is_odd_none_boxed()
        {
            Span<int> values = stackalloc int[4] { 2, 4, 6, 8 };
            Assert.Equal(0, values.Where(new IsOdd()).Box().FirstOrDefault());
        }

        [Fact]
        public void span()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            Assert.Equal(1, values.FirstOrDefault());
        }

        [Fact]
        public void span_boxed()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            Assert.Equal(1, values.Box().FirstOrDefault());
        }

        [Fact]
        public void span_empty()
        {
            Span<int> values = stackalloc int[0];
            Assert.Equal(0, values.FirstOrDefault());
        }

        [Fact]
        public void span_empty_box()
        {
            Span<int> values = stackalloc int[0];
            Assert.Equal(0, values.Box().FirstOrDefault());
        }

        [Fact]
        public void readonly_span()
        {
            ReadOnlySpan<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            Assert.Equal(1, values.FirstOrDefault());
        }

        [Fact]
        public void readonly_span_boxed()
        {
            ReadOnlySpan<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            Assert.Equal(1, values.Box().FirstOrDefault());
        }

        [Fact]
        public void readonly_span_empty()
        {
            ReadOnlySpan<int> values = stackalloc int[0];
            Assert.Equal(0, values.FirstOrDefault());
        }

        [Fact]
        public void readonly_span_empty_boxed()
        {
            ReadOnlySpan<int> values = stackalloc int[0];
            Assert.Equal(0, values.Box().FirstOrDefault());
        }

        [Fact]
        public void is_odd_delegate()
        {
            Span<int> values = stackalloc int[4] { 0, 1, 2, 3 };
            Assert.Equal(1, values.FirstOrDefault(v => v % 2 != 0));
        }

        [Fact]
        public void is_odd_delegate_boxed()
        {
            Span<int> values = stackalloc int[4] { 0, 1, 2, 3 };
            Assert.Equal(1, values.Box().FirstOrDefault(v => v % 2 != 0));
        }

        [Fact]
        public void is_odd_delegate_empty()
        {
            Span<int> values = stackalloc int[4] { 2, 4, 6, 8 };
            Assert.Equal(0, values.FirstOrDefault(v => v % 2 != 0));
        }

        [Fact]
        public void is_odd_delegate_empty_boxed()
        {
            Span<int> values = stackalloc int[4] { 2, 4, 6, 8 };
            Assert.Equal(0, values.Box().FirstOrDefault(v => v % 2 != 0));
        }

        [Fact]
        public void readonly_is_odd_delegate()
        {
            ReadOnlySpan<int> values = stackalloc int[4] { 0, 1, 2, 3 };
            Assert.Equal(1, values.FirstOrDefault(v => v % 2 != 0));
        }

        [Fact]
        public void readonly_is_odd_delegate_boxed()
        {
            ReadOnlySpan<int> values = stackalloc int[4] { 0, 1, 2, 3 };
            Assert.Equal(1, values.Box().FirstOrDefault(v => v % 2 != 0));
        }

        [Fact]
        public void readonly_is_odd_delegate_empty()
        {
            ReadOnlySpan<int> values = stackalloc int[4] { 2, 4, 6, 8 };
            Assert.Equal(0, values.FirstOrDefault(v => v % 2 != 0));
        }

        [Fact]
        public void readonly_is_odd_delegate_empty_boxed()
        {
            ReadOnlySpan<int> values = stackalloc int[4] { 2, 4, 6, 8 };
            Assert.Equal(0, values.Box().FirstOrDefault(v => v % 2 != 0));
        }

        [Fact]
        public void plus_one_is_odd()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            Assert.Equal(3, values.Select(v => v + 1).FirstOrDefault(v => v % 2 != 0));
        }

        [Fact]
        public void plus_one_is_odd_boxed()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            Assert.Equal(3, values.Select(v => v + 1).Box().FirstOrDefault(v => v % 2 != 0));
        }

        [Fact]
        public void range_even()
        {
            Assert.Equal(2, SpanEnumerable.Range(1, 30).FirstOrDefault(v => v % 2 == 0));
        }

        [Fact]
        public void range_even_boxed()
        {
            Assert.Equal(2, SpanEnumerable.Range(1, 30).Box().FirstOrDefault(v => v % 2 == 0));
        }

        [Fact]
        public void range()
        {
            Assert.Equal(1, SpanEnumerable.Range(1, 30).FirstOrDefault());
        }

        [Fact]
        public void range_boxed()
        {
            Assert.Equal(1, SpanEnumerable.Range(1, 30).Box().FirstOrDefault());
        }

        [Fact]
        public void range_empty()
        {
            Assert.Equal(0, SpanEnumerable.Range(1, 0).FirstOrDefault());
        }

        [Fact]
        public void range_empty_boxed()
        {
            Assert.Equal(0, SpanEnumerable.Range(1, 0).Box().FirstOrDefault());
        }
    }
}
