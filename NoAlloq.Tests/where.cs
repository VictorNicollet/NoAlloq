using System;
using Xunit;

namespace NoAlloq.Tests
{
    public sealed class where
    {
        public struct IsOdd : IMap<int, bool>
        {
            public bool Apply(int x) => x % 2 != 0;
        }

        [Fact]
        public void is_odd()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            var array = values.Where(new IsOdd()).ToArray();

            Assert.Equal(new[] { 1, 3 }, array);
        }

        [Fact]
        public void is_odd_delegate()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            var array = values.Where(v => v % 2 != 0).ToArray();

            Assert.Equal(new[] { 1, 3 }, array);
        }

        [Fact]
        public void plus_one_is_odd()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            var array = values.Select(v => v + 1).Where(v => v % 2 != 0).ToArray();

            Assert.Equal(new[] { 3, 5 }, array);
        }
    }
}
