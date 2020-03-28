using System;
using Xunit;

namespace NoAlloq.Tests
{
    public sealed class first
    {
        public struct IsOdd : IMap<int, bool>
        {
            public bool Apply(int x) => x % 2 != 0;
        }

        [Fact]
        public void where_is_odd()
        {
            Span<int> values = stackalloc int[4] { 0, 1, 2, 3 };
            Assert.Equal(1, values.Where(new IsOdd()).First());
        }

        [Fact]
        public void where_is_odd_boxed()
        {
            Span<int> values = stackalloc int[4] { 0, 1, 2, 3 };
            Assert.Equal(1, values.Where(new IsOdd()).Box().First());
        }

        [Fact]
        public void where_is_odd_none()
        {
            Span<int> values = stackalloc int[4] { 0, 2, 4, 6 };

            try
            {
                var found = values.Where(new IsOdd()).First();
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
        public void where_is_odd_none_boxed()
        {
            Span<int> values = stackalloc int[4] { 0, 2, 4, 6 };

            try
            {
                var found = values.Where(new IsOdd()).Box().First();
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
            Assert.Equal(1, values.First());
        }

        [Fact]
        public void span_boxed()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            Assert.Equal(1, values.Box().First());
        }

        [Fact]
        public void span_empty()
        {
            Span<int> values = stackalloc int[0];

            try
            {
                values.First();
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
        public void span_empty_boxed()
        {
            Span<int> values = stackalloc int[0];

            try
            {
                values.Box().First();
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
            Assert.Equal(1, values.First());
        }

        [Fact]
        public void readonly_span_boxed()
        {
            ReadOnlySpan<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            Assert.Equal(1, values.Box().First());
        }

        [Fact]
        public void readonly_span_empty()
        {
            ReadOnlySpan<int> values = stackalloc int[0];

            try
            {
                values.First();
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
        public void readonly_span_empty_boxed()
        {
            ReadOnlySpan<int> values = stackalloc int[0];

            try
            {
                values.Box().First();
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
            Assert.Equal(1, values.First(v => v % 2 != 0));
        }

        [Fact]
        public void is_odd_delegate_boxed()
        {
            Span<int> values = stackalloc int[4] { 0, 1, 2, 3 };
            Assert.Equal(1, values.Box().First(v => v % 2 != 0));
        }

        [Fact]
        public void is_odd_delegate_empty()
        {
            Span<int> values = stackalloc int[4] { 0, 2, 4, 6 };
            try
            {
                values.First(v => v % 2 != 0);
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
        public void is_odd_delegate_empty_boxed()
        {
            Span<int> values = stackalloc int[4] { 0, 2, 4, 6 };
            try
            {
                values.Box().First(v => v % 2 != 0);
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
            Assert.Equal(1, values.First(v => v % 2 != 0));
        }

        [Fact]
        public void readonly_is_odd_delegate_boxed()
        {
            ReadOnlySpan<int> values = stackalloc int[4] { 0, 1, 2, 3 };
            Assert.Equal(1, values.Box().First(v => v % 2 != 0));
        }

        [Fact]
        public void readonly_is_odd_delegate_empty()
        {
            ReadOnlySpan<int> values = stackalloc int[4] { 0, 2, 4, 6 };
            try
            {
                values.First(v => v % 2 != 0);
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
        public void readonly_is_odd_delegate_empty_boxed()
        {
            ReadOnlySpan<int> values = stackalloc int[4] { 0, 2, 4, 6 };
            try
            {
                values.Box().First(v => v % 2 != 0);
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
            Assert.Equal(3, values.Select(v => v + 1).First(v => v % 2 != 0));
        }

        [Fact]
        public void plus_one_is_odd_boxed()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            Assert.Equal(3, values.Select(v => v + 1).Box().First(v => v % 2 != 0));
        }

        [Fact]
        public void range_even()
        {
            Assert.Equal(2, SpanEnumerable.Range(1, 30).First(v => v % 2 == 0));
        }

        [Fact]
        public void range_even_boxed()
        {
            Assert.Equal(2, SpanEnumerable.Range(1, 30).Box().First(v => v % 2 == 0));
        }

        [Fact]
        public void range()
        {
            Assert.Equal(1, SpanEnumerable.Range(1, 30).First());
        }

        [Fact]
        public void range_boxed()
        {
            Assert.Equal(1, SpanEnumerable.Range(1, 30).Box().First());
        }

        [Fact]
        public void range_empty()
        {
            try
            { 
                SpanEnumerable.Range(1, 0).First();
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
        public void range_empty_boxed()
        {
            try
            {
                SpanEnumerable.Range(1, 0).Box().First();
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
