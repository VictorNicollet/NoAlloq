using System;
using Xunit;

namespace NoAlloq.Tests
{
    public sealed class range
    {
        [Fact]
        public void copy_into()
        {
            Span<int> values = stackalloc int[30];

            SpanEnumerable.Range(1, 30).CopyInto(values);

            for (var i = 0; i < 30; ++i)
                Assert.Equal(i + 1, values[i]);
        }

        [Fact]
        public void copy_into_boxed()
        {
            Span<int> values = stackalloc int[30];

            SpanEnumerable.Range(1, 30).Box().CopyInto(values);

            for (var i = 0; i < 30; ++i)
                Assert.Equal(i + 1, values[i]);
        }

        [Fact]
        public void where_odd_copy_into()
        {
            Span<int> values = stackalloc int[15];

            SpanEnumerable.Range(1, 30).Where(v => v % 2 != 0).CopyInto(values);

            for (var i = 0; i < 15; ++i)
                Assert.Equal(2 * i + 1, values[i]);
        }

        [Fact]
        public void where_odd_copy_into_boxed()
        {
            Span<int> values = stackalloc int[15];

            SpanEnumerable.Range(1, 30).Where(v => v % 2 != 0).Box().CopyInto(values);

            for (var i = 0; i < 15; ++i)
                Assert.Equal(2 * i + 1, values[i]);
        }

        [Fact]
        public void take_copy_into()
        {
            Span<int> values = stackalloc int[25];

            SpanEnumerable.Range(1, 30).Slice(5).CopyInto(values);

            for (var i = 5; i < 30; ++i)
                Assert.Equal(i + 1, values[i - 5]);
        }

        [Fact]
        public void take_copy_into_boxed()
        {
            Span<int> values = stackalloc int[25];

            SpanEnumerable.Range(1, 30).Slice(5).Box().CopyInto(values);

            for (var i = 5; i < 30; ++i)
                Assert.Equal(i + 1, values[i - 5]);
        }

        [Fact]
        public void slice_copy_into()
        {
            Span<int> values = stackalloc int[10];

            SpanEnumerable.Range(1, 30).Slice(5, 10).CopyInto(values);

            for (var i = 5; i < 15; ++i)
                Assert.Equal(i + 1, values[i - 5]);
        }

        [Fact]
        public void slice_copy_into_boxed()
        {
            Span<int> values = stackalloc int[10];

            SpanEnumerable.Range(1, 30).Slice(5, 10).Box().CopyInto(values);

            for (var i = 5; i < 15; ++i)
                Assert.Equal(i + 1, values[i - 5]);
        }

        [Fact]
        public void select_copy_into()
        {
            Span<int> values = stackalloc int[30];

            SpanEnumerable.Range(1, 30).Select(i => i * 2).CopyInto(values);

            for (var i = 0; i < 30; ++i)
                Assert.Equal((i + 1) * 2, values[i]);
        }

        [Fact]
        public void select_copy_into_boxed()
        {
            Span<int> values = stackalloc int[30];

            SpanEnumerable.Range(1, 30).Select(i => i * 2).Box().CopyInto(values);

            for (var i = 0; i < 30; ++i)
                Assert.Equal((i + 1) * 2, values[i]);
        }


    }
}
