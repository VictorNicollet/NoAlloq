using System;
using Xunit;

namespace NoAlloq.Tests
{
    public sealed class single
    {
        public struct IsOdd : IMap<int, bool>
        {
            public bool Apply(int x) => x % 2 != 0;
        }

        [Fact]
        public void where_is_odd()
        {
            Span<int> values = stackalloc int[3] { 0, 1, 2 };
            Assert.Equal(1, values.Where(new IsOdd()).Single());
        }

        [Fact]
        public void where_is_odd_boxed()
        {
            Span<int> values = stackalloc int[3] { 0, 1, 2 };
            Assert.Equal(1, values.Where(new IsOdd()).Box().Single());
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
        public void where_is_odd_two()
        {
            Span<int> values = stackalloc int[4] { 0, 1, 2, 3 };

            try
            {
                var found = values.Where(new IsOdd()).Single();
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
                var found = values.Where(new IsOdd()).Box().Single();
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
            Assert.Equal(1, values.Single());
        }

        [Fact]
        public void span_boxed()
        {
            Span<int> values = stackalloc int[1] { 1 };
            Assert.Equal(1, values.Box().Single());
        }

        [Fact]
        public void span_empty()
        {
            Span<int> values = stackalloc int[0];

            try
            {
                values.Single();
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
                values.Box().Single();
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
        public void span_two()
        {
            Span<int> values = stackalloc int[2] { 1, 2 };

            try
            {
                values.Single();
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
                values.Box().Single();
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
            Assert.Equal(1, values.Single());
        }

        [Fact]
        public void readonly_span_boxed()
        {
            ReadOnlySpan<int> values = stackalloc int[1] { 1 };
            Assert.Equal(1, values.Box().Single());
        }

        [Fact]
        public void readonly_span_empty()
        {
            ReadOnlySpan<int> values = stackalloc int[0];

            try
            {
                values.Single();
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
                values.Box().Single();
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
        public void readonly_span_two()
        {
            ReadOnlySpan<int> values = stackalloc int[2] { 1, 2 };

            try
            {
                values.Single();
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
                values.Box().Single();
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
            Assert.Equal(1, values.Single(v => v % 2 != 0));
        }

        [Fact]
        public void is_odd_delegate_boxed()
        {
            Span<int> values = stackalloc int[3] { 0, 1, 2 };
            Assert.Equal(1, values.Box().Single(v => v % 2 != 0));
        }

        [Fact]
        public void is_odd_delegate_empty()
        {
            Span<int> values = stackalloc int[4] { 0, 2, 4, 6 };
            try
            {
                values.Single(v => v % 2 != 0);
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
        public void is_odd_delegate_empty_boxe()
        {
            Span<int> values = stackalloc int[4] { 0, 2, 4, 6 };
            try
            {
                values.Box().Single(v => v % 2 != 0);
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
        public void is_odd_delegate_two()
        {
            Span<int> values = stackalloc int[4] { 0, 1, 2, 3 };
            try
            {
                values.Single(v => v % 2 != 0);
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
                values.Box().Single(v => v % 2 != 0);
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
            Assert.Equal(1, values.Single(v => v % 2 != 0));
        }

        [Fact]
        public void readonly_is_odd_delegate_boxed()
        {
            ReadOnlySpan<int> values = stackalloc int[3] { 0, 1, 2 };
            Assert.Equal(1, values.Box().Single(v => v % 2 != 0));
        }

        [Fact]
        public void readonly_is_odd_delegate_empty()
        {
            ReadOnlySpan<int> values = stackalloc int[4] { 0, 2, 4, 6 };
            try
            {
                values.Single(v => v % 2 != 0);
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
                values.Box().Single(v => v % 2 != 0);
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
        public void readonly_is_odd_delegate_two()
        {
            ReadOnlySpan<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            try
            {
                values.Single(v => v % 2 != 0);
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
                values.Box().Single(v => v % 2 != 0);
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
            Assert.Equal(3, values.Select(v => v + 1).Single(v => v % 2 != 0));
        }

        [Fact]
        public void plus_one_is_odd_boxed()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 5 };
            Assert.Equal(3, values.Select(v => v + 1).Box().Single(v => v % 2 != 0));
        }
    }
}
