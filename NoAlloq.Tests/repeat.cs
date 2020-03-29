using System;
using Xunit;

namespace NoAlloq.Tests
{
    public sealed class repeat
    {
        [Fact]
        public void copy_into()
        {
            Span<int> values = stackalloc int[30];

            SpanEnumerable.Repeat(1337, 30).CopyInto(values);

            for (var i = 0; i < 30; ++i)
                Assert.Equal(1337, values[i]);
        }

        [Fact]
        public void negative()
        {
            try
            {
                _ = SpanEnumerable.Repeat(1, -1);
                Assert.True(false);
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
    }
}
