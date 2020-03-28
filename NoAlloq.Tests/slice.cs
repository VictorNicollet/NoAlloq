using System;
using Xunit;

namespace NoAlloq.Tests
{
    public sealed class slice
    {
        [Fact]
        public void take_two()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            var array = values.Select(v => v * 2).Slice(0, 2).ToArray();

            Assert.Equal(new[] { 2, 4 }, array);
        }

        [Fact]
        public void take_two_boxed()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            var array = values.Select(v => v * 2).Box().Slice(0, 2).ToArray();

            Assert.Equal(new[] { 2, 4 }, array);
        }

        [Fact]
        public void skip_one_take_two()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            var array = values.Select(v => v * 2).Slice(1, 2).ToArray();

            Assert.Equal(new[] { 4, 6 }, array);
        }

        [Fact]
        public void skip_one_take_two_boxed()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            var array = values.Select(v => v * 2).Box().Slice(1, 2).ToArray();

            Assert.Equal(new[] { 4, 6 }, array);
        }

        [Fact]
        public void skip_all()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            var array = values.Select(v => v * 2).Slice(4, 2).ToArray();

            Assert.Empty(array);
        }

        [Fact]
        public void skip_all_boxed()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            var array = values.Select(v => v * 2).Box().Slice(4, 2).ToArray();

            Assert.Empty(array);
        }

        [Fact]
        public void skip_one_take_all()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            var array = values.Select(v => v * 2).Slice(1, 4).ToArray();

            Assert.Equal(new[] { 4, 6, 8 }, array);
        }

        [Fact]
        public void skip_one_take_all_boxed()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            var array = values.Select(v => v * 2).Box().Slice(1, 4).ToArray();

            Assert.Equal(new[] { 4, 6, 8 }, array);
        }
    }
}
