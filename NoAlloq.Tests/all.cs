using System;
using Xunit;

namespace NoAlloq.Tests
{
    public sealed class all
    {
        [Fact]
        public void range_odd()
        {
            Assert.False(SpanEnumerable.Range(1, 29).All(v => v % 2 != 0));
        }

        [Fact]
        public void range_odd_boxed()
        {
            Assert.False(SpanEnumerable.Range(1, 29).Box().All(v => v % 2 != 0));
        }

        [Fact]
        public void range_all()
        {
            Assert.True(SpanEnumerable.Range(1, 29).All(v => v <= 30));
        }

        [Fact]
        public void range_all_boxed()
        {
            Assert.True(SpanEnumerable.Range(1, 29).Box().All(v => v <= 30));
        }

        [Fact]
        public void span_odd()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29,
            };

            Assert.False(values.All(v => v % 2 != 0));
        }

        [Fact]
        public void span_odd_boxed()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29,
            };

            Assert.False(values.Box().All(v => v % 2 != 0));
        }

        [Fact]
        public void span_odd_none()
        {
            Span<int> values = stackalloc int[] { 
                 2,  4,  6,  8,  10,
            };

            Assert.True(values.All(v => v % 2 == 0));
        }

        [Fact]
        public void span_odd_none_boxed()
        {
            Span<int> values = stackalloc int[] { 
                 2,  4,  6,  8,  10,
            };

            Assert.True(values.Box().All(v => v % 2 == 0));
        }

        [Fact]
        public void span_where_odd()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29,
            };

            Assert.False(values.Where(v => v <= 15).All(v => v % 2 != 0));
        }

        [Fact]
        public void span_where_odd_boxed()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29,
            };

            Assert.False(values.Where(v => v <= 15).Box().All(v => v % 2 != 0));
        }
        
        [Fact]
        public void span_where_odd_none()
        {
            Span<int> values = stackalloc int[] { 
                 2,  4,  6,  8,  10
            };

            Assert.True(values.Where(v => v <= 15).All(v => v % 2 == 0));
        }
        
        [Fact]
        public void span_where_odd_none_boxed()
        {
            Span<int> values = stackalloc int[] { 
                 2,  4,  6,  8,  10
            };

            Assert.True(values.Where(v => v <= 15).Box().All(v => v % 2 == 0));
        }

        [Fact]
        public void readonly_span_odd()
        {
            ReadOnlySpan<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 
            };

            Assert.False(values.All(v => v % 2 != 0));
        }

        [Fact]
        public void readonly_span_odd_boxed()
        {
            ReadOnlySpan<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 
            };

            Assert.False(values.Box().All(v => v % 2 != 0));
        }

        [Fact]
        public void span_true()
        {
            Span<bool> values = stackalloc bool[] { 
                true, false, true, true, false, false, true
            };

            Assert.False(values.AllTrue());
        }

        [Fact]
        public void span_true_boxed()
        {
            Span<bool> values = stackalloc bool[] { 
                true, false, true, true, false, false, true
            };

            Assert.False(values.Box().AllTrue());
        }

        [Fact]
        public void span_all_true()
        {
            Span<bool> values = stackalloc bool[] { 
                true, true, true
            };

            Assert.True(values.AllTrue());
        }

        [Fact]
        public void span_all_true_boxed()
        {
            Span<bool> values = stackalloc bool[] { 
                true, true, true
            };

            Assert.True(values.Box().AllTrue());
        }

        [Fact]
        public void select_all_true()
        {
            Span<bool> values = stackalloc bool[] { 
                true, false, true, true, false, false, true
            };

            Assert.True(values.Select(b => true).AllTrue());
        }

        [Fact]
        public void select_all_true_boxed()
        {
            Span<bool> values = stackalloc bool[] { 
                true, false, true, true, false, false, true
            };

            Assert.True(values.Select(b => true).Box().AllTrue());
        }

        [Fact]
        public void select_true()
        {
            Span<bool> values = stackalloc bool[] { 
                true, false, true, true, false, false, true
            };

            Assert.False(values.Select(b => !b).AllTrue());
        }

        [Fact]
        public void select_true_boxed()
        {
            Span<bool> values = stackalloc bool[] { 
                true, false, true, true, false, false, true
            };

            Assert.False(values.Select(b => !b).Box().AllTrue());
        }
        
        [Fact]
        public void readonly_span_true()
        {
            ReadOnlySpan<bool> values = stackalloc bool[] { 
                true, false, true, true, false, false, true
            };

            Assert.False(values.AllTrue());
        }
        
        [Fact]
        public void readonly_span_true_boxed()
        {
            ReadOnlySpan<bool> values = stackalloc bool[] { 
                true, false, true, true, false, false, true
            };

            Assert.False(values.Box().AllTrue());
        }

        [Fact]
        public void range_true()
        {
            Assert.False(SpanEnumerable.Range(1, 30)
                .Select(v => v % 3 == 0)
                .AllTrue());
        }

        [Fact]
        public void range_true_boxed()
        {
            Assert.False(SpanEnumerable.Range(1, 30)
                .Select(v => v % 3 == 0)
                .Box()
                .AllTrue());
        }

        [Fact]
        public void range_all_true()
        {
            Assert.True(SpanEnumerable.Range(1, 30)
                .Select(v => v >= 0)
                .AllTrue());
        }

        [Fact]
        public void range_all_true_boxed()
        {
            Assert.True(SpanEnumerable.Range(1, 30)
                .Select(v => v >= 0)
                .Box()
                .AllTrue());
        }
    }
}
