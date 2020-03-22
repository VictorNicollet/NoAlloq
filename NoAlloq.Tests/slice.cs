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
    }
}
