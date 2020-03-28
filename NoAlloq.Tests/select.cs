using System;
using Xunit;

namespace NoAlloq.Tests
{
    public sealed class select
    {
        public struct Times : IMap<int, int>
        {
            public int Operand;

            public Times(int operand)
            {
                Operand = operand;
            }

            public int Apply(int x) => x * Operand;
        }

        [Fact]
        public void times_two()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            var array = values.Select<int, int, Times>(new Times(2)).ToArray();

            for (var i = 0; i < values.Length; ++i)
                Assert.Equal(values[i] * 2, array[i]);
        }

        [Fact]
        public void times_two_delegate()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            var array = values.Select(v => v * 2).ToArray();

            for (var i = 0; i < values.Length; ++i)
                Assert.Equal(values[i] * 2, array[i]);
        }

        [Fact]
        public void times_two_delegate_boxed()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            var array = values.Box().Select(v => v * 2).ToArray();

            for (var i = 0; i < values.Length; ++i)
                Assert.Equal(values[i] * 2, array[i]);
        }

        [Fact]
        public void times_two_twice_delegate()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            var array = values.Select(v => v * 2).Select(v => v * 2).ToArray();

            for (var i = 0; i < values.Length; ++i)
                Assert.Equal(values[i] * 4, array[i]);
        }

        [Fact]
        public void times_two_twice_delegate_boxed()
        {
            Span<int> values = stackalloc int[4] { 1, 2, 3, 4 };
            var array = values.Select(v => v * 2).Box().Select(v => v * 2).ToArray();

            for (var i = 0; i < values.Length; ++i)
                Assert.Equal(values[i] * 4, array[i]);
        }
    }
}
