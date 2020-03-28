using System;
using Xunit;

namespace NoAlloq.Tests
{
    public sealed class count
    {
        [Fact]
        public void span()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            Assert.Equal(30, values.Count());
        }

        [Fact]
        public void span_boxed()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            Assert.Equal(30, values.Box().Count());
        }

        [Fact]
        public void span_select()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            Assert.Equal(30, values.Select(v => v * 2).Count());
        }

        [Fact]
        public void span_select_boxed()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            Assert.Equal(30, values.Select(v => v * 2).Box().Count());
        }

        [Fact]
        public void span_where()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            Assert.Equal(15, values.Where(v => v % 2 != 0).Count());
        }

        [Fact]
        public void span_where_boxed()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            Assert.Equal(15, values.Where(v => v % 2 != 0).Box().Count());
        }
        
        [Fact]
        public void readonly_span()
        {
            ReadOnlySpan<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            Assert.Equal(30, values.Count());
        }
        
        [Fact]
        public void readonly_span_boxed()
        {
            ReadOnlySpan<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            Assert.Equal(30, values.Box().Count());
        }

        [Fact]
        public void range()
        {
            Assert.Equal(30, SpanEnumerable.Range(1, 30).Count());
        }

        [Fact]
        public void range_boxed()
        {
            Assert.Equal(30, SpanEnumerable.Range(1, 30).Box().Count());
        }

        [Fact]
        public void range_where()
        {
            Assert.Equal(15, SpanEnumerable.Range(1, 30)
                .Where(p => p <= 15)
                .Count());
        }

        [Fact]
        public void range_where_box()
        {
            Assert.Equal(15, SpanEnumerable.Range(1, 30)
                .Where(p => p <= 15)
                .Box()
                .Count());
        }

        [Fact]
        public void range_odd()
        {
            Assert.Equal(15, SpanEnumerable.Range(1, 29).Count(v => v % 2 != 0));
        }

        [Fact]
        public void range_odd_boxed()
        {
            Assert.Equal(15, SpanEnumerable.Range(1, 29).Box().Count(v => v % 2 != 0));
        }

        [Fact]
        public void span_odd()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29,
            };

            Assert.Equal(15, values.Count(v => v % 2 != 0));
        }

        [Fact]
        public void span_odd_boxed()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29,
            };

            Assert.Equal(15, values.Box().Count(v => v % 2 != 0));
        }

        [Fact]
        public void span_where_odd()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29,
            };

            Assert.Equal(8, values.Where(v => v <= 15).Count(v => v % 2 != 0));
        }

        [Fact]
        public void span_where_odd_boxed()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29,
            };

            Assert.Equal(8, values.Where(v => v <= 15).Box().Count(v => v % 2 != 0));
        }

        [Fact]
        public void readonly_span_odd()
        {
            ReadOnlySpan<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 
            };

            Assert.Equal(15, values.Count(v => v % 2 != 0));
        }

        [Fact]
        public void readonly_span_odd_boxed()
        {
            ReadOnlySpan<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 
            };

            Assert.Equal(15, values.Box().Count(v => v % 2 != 0));
        }

        [Fact]
        public void span_true()
        {
            Span<bool> values = stackalloc bool[] { 
                true, false, true, true, false, false, true
            };

            Assert.Equal(4, values.CountTrue());
        }

        [Fact]
        public void span_true_boxed()
        {
            Span<bool> values = stackalloc bool[] { 
                true, false, true, true, false, false, true
            };

            Assert.Equal(4, values.Box().CountTrue());
        }

        [Fact]
        public void select_true()
        {
            Span<bool> values = stackalloc bool[] { 
                true, false, true, true, false, false, true
            };

            Assert.Equal(3, values.Select(b => !b).CountTrue());
        }

        [Fact]
        public void select_true_boxed()
        {
            Span<bool> values = stackalloc bool[] { 
                true, false, true, true, false, false, true
            };

            Assert.Equal(3, values.Select(b => !b).Box().CountTrue());
        }
        
        [Fact]
        public void readonly_span_true()
        {
            ReadOnlySpan<bool> values = stackalloc bool[] { 
                true, false, true, true, false, false, true
            };

            Assert.Equal(4, values.CountTrue());
        }
        
        [Fact]
        public void readonly_span_true_boxed()
        {
            ReadOnlySpan<bool> values = stackalloc bool[] { 
                true, false, true, true, false, false, true
            };

            Assert.Equal(4, values.Box().CountTrue());
        }

        [Fact]
        public void range_true()
        {
            Assert.Equal(10, SpanEnumerable.Range(1, 30)
                .Select(v => v % 3 == 0)
                .CountTrue());
        }

        [Fact]
        public void range_true_boxed()
        {
            Assert.Equal(10, SpanEnumerable.Range(1, 30)
                .Select(v => v % 3 == 0)
                .Box()
                .CountTrue());
        }
    }
}
