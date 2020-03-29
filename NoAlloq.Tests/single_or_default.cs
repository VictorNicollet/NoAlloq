using System;
using Xunit;

namespace NoAlloq.Tests
{
    public sealed class single_or_default
    {
        public struct IsOdd : IMap<int, bool>
        {
            public bool Apply(int x) => x % 2 != 0;
        }

        [Fact]
        public void where_is_odd()
        {
            Span<int> values = stackalloc int[3] { 0, 1, 2 };
            Assert.Equal(1, values.Where(new IsOdd()).SingleOrDefault());
        }

        [Fact]
        public void where_is_odd_boxed()
        {
            Span<int> values = stackalloc int[3] { 0, 1, 2 };
            Assert.Equal(1, values.Where(new IsOdd()).Box().SingleOrDefault());
        }

        [Fact]
        public void where_is_odd_none()
        {
            Span<int> values = stackalloc int[4] { 0, 2, 4, 6 };
            Assert.Equal(0, values.Where(new IsOdd()).SingleOrDefault());
        }

        [Fact]
        public void where_is_odd_none_boxed()
        {
            Span<int> values = stackalloc int[4] { 0, 2, 4, 6 };
            Assert.Equal(0, values.Where(new IsOdd()).Box().SingleOrDefault());
        }

        [Fact]
        public void where_is_odd_two()
        {
            Span<int> values = stackalloc int[4] { 0, 1, 2, 3 };

            try
            {
                var found = values.Where(new IsOdd()).SingleOrDefault();
                Assert.True(false);
            }
            catch (InvalidOperationException e)
            {
                Assert.Equal(
                    "The input sequence contains more than one element.",
                    e.Message);
            }
        }

        [Fact]
        public void where_is_odd_two_boxed()
        {
            Span<int> values = stackalloc int[4] { 0, 1, 2, 3 };

            try
            {
                var found = values.Where(new IsOdd()).Box().SingleOrDefault();
                Assert.True(false);
            }
            catch (InvalidOperationException e)
            {
                Assert.Equal(
                    "The input sequence contains more than one element.",
                    e.Message);
            }
        }

        [Fact]
        public void span()
        {
            Span<int> values = stackalloc int[1] { 1 };
            Assert.Equal(1, values.SingleOrDefault());
        }

        [Fact]
        public void span_boxed()
        {
            Span<int> values = stackalloc int[1] { 1 };
            Assert.Equal(1, values.Box().SingleOrDefault());
        }

        [Fact]
        public void span_empty()
        {
            Span<int> values = stackalloc int[0];
            Assert.Equal(0, values.SingleOrDefault());
        }

        [Fact]
        public void span_empty_boxed()
        {
            Span<int> values = stackalloc int[0];
            Assert.Equal(0, values.Box().SingleOrDefault());
        }

        [Fact]
        public void span_two()
        {
            Span<int> values = stackalloc int[2] { 1, 2 };

            try
            {
                values.SingleOrDefault();
                Assert.True(false);
            }
            catch (InvalidOperationException e)
            {
                Assert.Equal(
                    "The input sequence contains more than one element.",
                    e.Message);
            }
        }

        [Fact]
        public void span_two_boxed()
        {
            Span<int> values = stackalloc int[2] { 1, 2 };

            try
            {
                values.Box().SingleOrDefault();
                Assert.True(false);
            }
            catch (InvalidOperationException e)
            {
                Assert.Equal(
                    "The input sequence contains more than one element.",
                    e.Message);
            }
        }

        [Fact]
        public void readonly_span()
        {
            ReadOnlySpan<int> values = stackalloc int[1] { 1 };
            Assert.Equal(1, values.SingleOrDefault());
        }

        [Fact]
        public void readonly_span_boxed()
        {
            ReadOnlySpan<int> values = stackalloc int[1] { 1 };
            Assert.Equal(1, values.Box().SingleOrDefault());
        }

        [Fact]
        public void readonly_span_empty()
        {
            ReadOnlySpan<int> values = stackalloc int[0];
            Assert.Equal(0, values.SingleOrDefault());
        }

        [Fact]
        public void readonly_span_empty_boxed()
        {
            ReadOnlySpan<int> values = stackalloc int[0];
            Assert.Equal(0, values.Box().SingleOrDefault());
        }

        [Fact]
        public void readonly_span_two()
        {
            ReadOnlySpan<int> values = stackalloc int[2] { 1, 2 };

            try
            {
                values.SingleOrDefault();
                Assert.True(false);
            }
            catch (InvalidOperationException e)
            {
                Assert.Equal(
                    "The input sequence contains more than one element.",
                    e.Message);
            }
        }

        [Fact]
        public void readonly_span_two_boxed()
        {
            ReadOnlySpan<int> values = stackalloc int[2] { 1, 2 };

            try
            {
                values.Box().SingleOrDefault();
                Assert.True(false);
            }
            catch (InvalidOperationException e)
            {
                Assert.Equal(
                    "The input sequence contains more than one element.",
                    e.Message);
            }
        }

        [Fact]
        public void orderby()
        {
            Span<int> values = stackalloc int[] { 1 };
            Assert.Equal(1, values.OrderByValue(values).SingleOrDefault());
        }

        [Fact]
        public void orderby_empty()
        {
            Span<int> values = stackalloc int[] { };
            Assert.Equal(0, values.OrderByValue(values).SingleOrDefault());
        }

        [Fact]
        public void orderby_two()
        {
            Span<int> values = stackalloc int[] { 2, 1 };
            try
            {
                _ = values.OrderByValue(values).SingleOrDefault();
                Assert.True(false);
            }
            catch (InvalidOperationException e)
            {
                Assert.Equal(
                    "The input sequence contains more than one element.",
                    e.Message);
            }
        }

        [Fact]
        public void orderby_odd()
        {
            Span<int> values = stackalloc int[] { 3, 2 };
            Assert.Equal(3, values.OrderByValue(values).SingleOrDefault(v => v % 2 != 0));
        }

        [Fact]
        public void orderby_odd_empty()
        {
            Span<int> values = stackalloc int[] { 2 };
            Assert.Equal(0, values.OrderByValue(values)
                .SingleOrDefault(v => v % 2 != 0));
        }

        [Fact]
        public void orderby_odd_two()
        {
            Span<int> values = stackalloc int[] { 3, 2, 1 };
            try
            {
                _ = values.OrderByValue(values).SingleOrDefault(v => v % 2 != 0);
                Assert.True(false);
            }
            catch (InvalidOperationException e)
            {
                Assert.Equal(
                    "The input sequence contains more than one element.",
                    e.Message);
            }
        }

        [Fact]
        public void is_odd_delegate()
        {
            Span<int> values = stackalloc int[3] { 0, 1, 2 };
            Assert.Equal(1, values.SingleOrDefault(v => v % 2 != 0));
        }

        [Fact]
        public void is_odd_delegate_boxed()
        {
            Span<int> values = stackalloc int[3] { 0, 1, 2 };
            Assert.Equal(1, values.Box().SingleOrDefault(v => v % 2 != 0));
        }

        [Fact]
        public void is_odd_delegate_empty()
        {
            Span<int> values = stackalloc int[4] { 0, 2, 4, 6 };
            Assert.Equal(0, values.SingleOrDefault(v => v % 2 != 0));
        }

        [Fact]
        public void is_odd_delegate_empty_boxed()
        {
            Span<int> values = stackalloc int[4] { 0, 2, 4, 6 };
            Assert.Equal(0, values.Box().SingleOrDefault(v => v % 2 != 0));
        }

        [Fact]
        public void is_odd_delegate_two()
        {
            Span<int> values = stackalloc int[4] { 0, 1, 2, 3 };
            try
            {
                values.SingleOrDefault(v => v % 2 != 0);
                Assert.True(false);
            }
            catch (InvalidOperationException e)
            {
                Assert.Equal(
                    "The input sequence contains more than one element.",
                    e.Message);
            }
        }

        [Fact]
        public void is_odd_delegate_two_boxed()
        {
            Span<int> values = stackalloc int[4] { 0, 1, 2, 3 };
            try
            {
                values.Box().SingleOrDefault(v => v % 2 != 0);
                Assert.True(false);
            }
            catch (InvalidOperationException e)
            {
                Assert.Equal(
                    "The input sequence contains more than one element.",
                    e.Message);
            }
        }

        [Fact]
        public void readonly_is_odd_delegate()
        {
            ReadOnlySpan<int> values = stackalloc int[3] { 0, 1, 2 };
            Assert.Equal(1, values.SingleOrDefault(v => v % 2 != 0));
        }

        [Fact]
        public void readonly_is_odd_delegate_boxed()
        {
            ReadOnlySpan<int> values = stackalloc int[3] { 0, 1, 2 };
            Assert.Equal(1, values.Box().SingleOrDefault(v => v % 2 != 0));
        }

        [Fact]
        public void readonly_is_odd_delegate_empty()
        {
            ReadOnlySpan<int> values = stackalloc int[4] { 0, 2, 4, 6 };
            Assert.Equal(0, values.SingleOrDefault(v => v % 2 != 0));
        }

        [Fact]
        public void readonly_is_odd_delegate_empty_boxed()
        {
            ReadOnlySpan<int> values = stackalloc int[4] { 0, 2, 4, 6 };
            Assert.Equal(0, values.Box().SingleOrDefault(v => v % 2 != 0));
        }

        [Fact]
        public void readonly_is_odd_delegate_two()
        {
            ReadOnlySpan<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            try
            {
                values.SingleOrDefault(v => v % 2 != 0);
                Assert.True(false);
            }
            catch (InvalidOperationException e)
            {
                Assert.Equal(
                    "The input sequence contains more than one element.",
                    e.Message);
            }
        }

        [Fact]
        public void readonly_is_odd_delegate_two_boxed()
        {
            ReadOnlySpan<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            try
            {
                values.Box().SingleOrDefault(v => v % 2 != 0);
                Assert.True(false);
            }
            catch (InvalidOperationException e)
            {
                Assert.Equal(
                    "The input sequence contains more than one element.",
                    e.Message);
            }
        }

        [Fact]
        public void plus_one_is_odd()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 5 };
            Assert.Equal(3, values.Select(v => v + 1).SingleOrDefault(v => v % 2 != 0));
        }

        [Fact]
        public void plus_one_is_odd_boxed()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 5 };
            Assert.Equal(3, values.Select(v => v + 1).Box().SingleOrDefault(v => v % 2 != 0));
        }

        [Fact]
        public void range()
        {
            Assert.Equal(5, SpanEnumerable.Range(5, 1).SingleOrDefault());
        }

        [Fact]
        public void range_boxed()
        {
            Assert.Equal(5, SpanEnumerable.Range(5, 1).Box().SingleOrDefault());
        }

        [Fact]
        public void range_empty()
        {
            Assert.Equal(0, SpanEnumerable.Range(5, 0).SingleOrDefault());
        }

        [Fact]
        public void range_empty_boxed()
        {
            Assert.Equal(0, SpanEnumerable.Range(5, 0).SingleOrDefault());
        }

        [Fact]
        public void range_two()
        {
            try
            {
                _ = SpanEnumerable.Range(5, 2).SingleOrDefault();
                Assert.True(false);
            }
            catch (InvalidOperationException e)
            {
                Assert.Equal(
                    "The input sequence contains more than one element.",
                    e.Message);
            }
        }

        [Fact]
        public void range_two_boxed()
        {
            try
            {
                _ = SpanEnumerable.Range(5, 2).Box().SingleOrDefault();
                Assert.True(false);
            }
            catch (InvalidOperationException e)
            {
                Assert.Equal(
                    "The input sequence contains more than one element.",
                    e.Message);
            }
        }

        [Fact]
        public void range_odd()
        {
            Assert.Equal(5, SpanEnumerable.Range(4, 3).SingleOrDefault(v => v % 2 != 0));
        }

        [Fact]
        public void range_odd_boxed()
        {
            Assert.Equal(5, SpanEnumerable.Range(4, 3).Box()
                .SingleOrDefault(v => v % 2 != 0));
        }

        [Fact]
        public void range_odd_empty()
        {
            Assert.Equal(0, SpanEnumerable.Range(4, 1)
                .SingleOrDefault(v => v % 2 != 0));
        }

        [Fact]
        public void range_odd_empty_boxed()
        {
            Assert.Equal(0, SpanEnumerable.Range(4, 1).Box()
                .SingleOrDefault(v => v % 2 != 0));
        }

        [Fact]
        public void range_odd_two()
        {
            try
            {
                _ = SpanEnumerable.Range(5, 3).SingleOrDefault(v => v % 2 != 0);
                Assert.True(false);
            }
            catch (InvalidOperationException e)
            {
                Assert.Equal(
                    "The input sequence contains more than one element.",
                    e.Message);
            }
        }

        [Fact]
        public void range_odd_two_boxed()
        {
            try
            {
                _ = SpanEnumerable.Range(5, 3).Box()
                    .SingleOrDefault(v => v % 2 != 0);
                Assert.True(false);
            }
            catch (InvalidOperationException e)
            {
                Assert.Equal(
                    "The input sequence contains more than one element.",
                    e.Message);
            }
        }
    }
}
