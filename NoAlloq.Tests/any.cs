using System;
using Xunit;

namespace NoAlloq.Tests
{
    public sealed class any
    {
        [Fact]
        public void span()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            Assert.True(values.Any());
        }

        [Fact]
        public void span_empty()
        {
            Span<int> values = stackalloc int[0];

            Assert.False(values.Any());
        }

        [Fact]
        public void span_select()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            Assert.True(values.Select(v => v * 2).Any());
        }

                [Fact]
        public void span_select_empty()
        {
            Span<int> values = stackalloc int[0];

            Assert.False(values.Select(v => v * 2).Any());
        }

        [Fact]
        public void span_where()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            Assert.True(values.Where(v => v % 2 != 0).Any());
        }

        [Fact]
        public void span_where_empty()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
            };

            Assert.False(values.Where(v => false).Any());
        }
        
        [Fact]
        public void readonly_span()
        {
            ReadOnlySpan<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            Assert.True(values.Any());
        }
        
        [Fact]
        public void readonly_span_empty()
        {
            ReadOnlySpan<int> values = stackalloc int[0];

            Assert.False(values.Any());
        }

        [Fact]
        public void range()
        {
            Assert.True(SpanEnumerable.Range(1, 30).Any());
        }

        [Fact]
        public void range_empty()
        {
            Assert.False(SpanEnumerable.Range(1, 0).Any());
        }

        [Fact]
        public void range_where()
        {
            Assert.True(SpanEnumerable.Range(1, 30)
                .Where(p => p <= 15)
                .Any());
        }

        [Fact]
        public void range_where_empty()
        {
            Assert.False(SpanEnumerable.Range(1, 30)
                .Where(p => p <= 0)
                .Any());
        }

        [Fact]
        public void range_odd()
        {
            Assert.True(SpanEnumerable.Range(1, 29).Any(v => v % 2 != 0));
        }

        [Fact]
        public void range_none()
        {
            Assert.False(SpanEnumerable.Range(1, 29).Any(v => v > 30));
        }

        [Fact]
        public void span_odd()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29,
            };

            Assert.True(values.Any(v => v % 2 != 0));
        }

        [Fact]
        public void span_odd_none()
        {
            Span<int> values = stackalloc int[] { 
                 2,  4,  6,  8,  10,
            };

            Assert.False(values.Any(v => v % 2 != 0));
        }

        [Fact]
        public void span_where_odd()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29,
            };

            Assert.True(values.Where(v => v <= 15).Any(v => v % 2 != 0));
        }
        
        [Fact]
        public void span_where_odd_none()
        {
            Span<int> values = stackalloc int[] { 
                 2,  4,  6,  8,  10
            };

            Assert.False(values.Where(v => v <= 15).Any(v => v % 2 != 0));
        }

        [Fact]
        public void readonly_span_odd()
        {
            ReadOnlySpan<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 
            };

            Assert.True(values.Any(v => v % 2 != 0));
        }

        [Fact]
        public void span_true()
        {
            Span<bool> values = stackalloc bool[] { 
                true, false, true, true, false, false, true
            };

            Assert.True(values.AnyTrue());
        }

        [Fact]
        public void span_none_true()
        {
            Span<bool> values = stackalloc bool[] { 
                false, false, false
            };

            Assert.False(values.AnyTrue());
        }

        [Fact]
        public void select_true()
        {
            Span<bool> values = stackalloc bool[] { 
                true, false, true, true, false, false, true
            };

            Assert.True(values.Select(b => !b).AnyTrue());
        }

        [Fact]
        public void select_none_true()
        {
            Span<bool> values = stackalloc bool[] { 
                true, false, true, true, false, false, true
            };

            Assert.False(values.Select(b => false).AnyTrue());
        }
        
        [Fact]
        public void readonly_span_true()
        {
            ReadOnlySpan<bool> values = stackalloc bool[] { 
                true, false, true, true, false, false, true
            };

            Assert.True(values.AnyTrue());
        }

        [Fact]
        public void range_true()
        {
            Assert.True(SpanEnumerable.Range(1, 30)
                .Select(v => v % 3 == 0)
                .AnyTrue());
        }

        [Fact]
        public void range_none_true()
        {
            Assert.False(SpanEnumerable.Range(1, 30)
                .Select(v => v < 0)
                .AnyTrue());
        }
    }
}
