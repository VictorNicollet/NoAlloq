using System;
using Xunit;

namespace NoAlloq.Tests
{
    public sealed class sum
    {
        [Fact]
        public void select_times_two_delegate()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            var sum = values.Select(v => v * 2).Sum();

            Assert.Equal(930, sum);
        }

        [Fact]
        public void select_times_two_delegate_boxed()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            var sum = values.Select(v => v * 2).Box().Sum();

            Assert.Equal(930, sum);
        }

        [Fact]
        public void times_two_delegate()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            var sum = values.Sum(v => v * 2);

            Assert.Equal(930, sum);
        }
        
        [Fact]
        public void times_two_delegate_boxed()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            var sum = values.Box().Sum(v => v * 2);

            Assert.Equal(930, sum);
        }
        
        [Fact]
        public void readonly_ints()
        {
            ReadOnlySpan<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            var sum = values.Sum();

            Assert.Equal(465, sum);
        }
        
        [Fact]
        public void readonly_ints_boxed()
        {
            ReadOnlySpan<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            var sum = values.Box().Sum();

            Assert.Equal(465, sum);
        }
        
        [Fact]
        public void ints()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            var sum = values.Sum();

            Assert.Equal(465, sum);
        }
        
        [Fact]
        public void ints_boxed()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            var sum = values.Box().Sum();

            Assert.Equal(465, sum);
        }

        [Fact]
        public void select_times_two_delegate_float()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            var sum = values.Select(v => v * 2.0f).Sum();

            Assert.Equal(930.0f, sum);
        }

        [Fact]
        public void select_times_two_delegate_float_boxed()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            var sum = values.Select(v => v * 2.0f).Box().Sum();

            Assert.Equal(930.0f, sum);
        }

        [Fact]
        public void times_two_delegate_float()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            var sum = values.Sum(v => v * 2.0f);

            Assert.Equal(930.0f, sum);
        }

        [Fact]
        public void times_two_delegate_float_boxed()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            var sum = values.Box().Sum(v => v * 2.0f);

            Assert.Equal(930.0f, sum);
        }

        [Fact]
        public void readonly_times_two_delegate_float()
        {
            ReadOnlySpan<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            var sum = values.Sum(v => v * 2.0f);

            Assert.Equal(930.0f, sum);
        }

        [Fact]
        public void readonly_times_two_delegate_float_boxed()
        {
            ReadOnlySpan<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            var sum = values.Box().Sum(v => v * 2.0f);

            Assert.Equal(930.0f, sum);
        }
        
        [Fact]
        public void floats()
        {
            Span<float> values = stackalloc float[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            var sum = values.Sum();

            Assert.Equal(465.0f, sum);
        }
        
        [Fact]
        public void floats_boxed()
        {
            Span<float> values = stackalloc float[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            var sum = values.Box().Sum();

            Assert.Equal(465.0f, sum);
        }

        [Fact]
        public void range_ints()
        {
            var sum = SpanEnumerable.Range(1, 30).Sum();
            Assert.Equal(465, sum);
        }

        [Fact]
        public void range_ints_boxed()
        {
            var sum = SpanEnumerable.Range(1, 30).Box().Sum();
            Assert.Equal(465, sum);
        }

        [Fact]
        public void range_floats()
        {
            var sum = SpanEnumerable.Range(1, 30).Select(i => (float)i).Sum();
            Assert.Equal(465.0f, sum);
        }

        [Fact]
        public void range_floats_boxed()
        {
            var sum = SpanEnumerable.Range(1, 30).Select(i => (float)i).Box().Sum();
            Assert.Equal(465.0f, sum);
        }

        [Fact]
        public void range_doubles()
        {
            var sum = SpanEnumerable.Range(1, 30).Select(i => (double)i).Sum();
            Assert.Equal(465.0, sum);
        }

        [Fact]
        public void range_doubles_boxed()
        {
            var sum = SpanEnumerable.Range(1, 30).Select(i => (double)i).Box().Sum();
            Assert.Equal(465.0, sum);
        }

        [Fact]
        public void doubles()
        {
            Span<double> values = stackalloc double[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            var sum = values.Sum();

            Assert.Equal(465.0, sum);
        }

        [Fact]
        public void doubles_boxed()
        {
            Span<double> values = stackalloc double[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            var sum = values.Box().Sum();

            Assert.Equal(465.0, sum);
        }

        [Fact]
        public void times_two_delegate_double()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            var sum = values.Select(v => v * 2.0).Sum();

            Assert.Equal(930.0, sum);
        }

        [Fact]
        public void times_two_delegate_double_boxed()
        {
            Span<int> values = stackalloc int[] { 
                 1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            };

            var sum = values.Select(v => v * 2.0).Sum();

            Assert.Equal(930.0, sum);
        }
    }
}
