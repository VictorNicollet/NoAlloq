using System;
using Xunit;

namespace NoAlloq.Tests
{
    public sealed class order_by
    {
        [Fact]
        public void ascending()
        {
            Span<int> number = stackalloc int[10] {
                3, 1, 5, 7, 2, 8, 10, 9, 4, 6
            };

            number.OrderByValue(number).CopyInto(number);

            for (var i = 0; i < 10; ++i)
                Assert.Equal(i + 1, number[i]);
        }

        [Fact]
        public void ascending_negative()
        {
            Span<int> number = stackalloc int[10] {
                3, 1, 5, 7, 2, 8, 10, 9, 4, 6
            };

            number.OrderBy(number, n => -n).CopyInto(number);

            for (var i = 0; i < 10; ++i)
                Assert.Equal(10 - i, number[i]);
        }
        [Fact]
        public void descending()
        {
            Span<int> number = stackalloc int[10] {
                3, 1, 5, 7, 2, 8, 10, 9, 4, 6
            };

            number.OrderByValueDescending(number).CopyInto(number);

            for (var i = 0; i < 10; ++i)
                Assert.Equal(10 - i, number[i]);
        }

        [Fact]
        public void descending_negative()
        {
            Span<int> number = stackalloc int[10] {
                3, 1, 5, 7, 2, 8, 10, 9, 4, 6
            };

            number.OrderByDescending(number, n => -n).CopyInto(number);

            for (var i = 0; i < 10; ++i)
                Assert.Equal(i + 1, number[i]);
        }
    }
}
