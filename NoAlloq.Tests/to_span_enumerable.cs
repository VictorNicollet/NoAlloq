using System;
using System.Linq;
using Xunit;

namespace NoAlloq.Tests
{
    public sealed class to_span_enumerable
    {
        [Fact]
        public void copy_into()
        {
            Span<int> values = stackalloc int[30];

            Enumerable.Repeat(1337, 30).ToSpanEnumerable().CopyInto(values);

            for (var i = 0; i < 30; ++i)
                Assert.Equal(1337, values[i]);
        }

        [Fact]
        public void copy_into_boxed()
        {
            Span<int> values = stackalloc int[30];

            Enumerable.Repeat(1337, 30).ToSpanEnumerable().Box().CopyInto(values);

            for (var i = 0; i < 30; ++i)
                Assert.Equal(1337, values[i]);
        }

        [Fact]
        public void unknown_length()
        {
            Assert.False(Enumerable.Repeat(1337, 30).ToSpanEnumerable().KnownLength);
        }

        [Fact]
        public void known_length()
        {
            var e = Enumerable.Repeat(1337, 30).ToList().ToSpanEnumerable();

            Assert.True(e.KnownLength);
            Assert.Equal(30, e.Length);
        }
    }
}
