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

        [Fact]
        public void ascending_then_descending()
        {
            var strings = new[] { "a", "b", "aa", "c", "cd" };

            strings.AsSpan()
                   .OrderBy(strings, s => s.Length)
                   .ThenByDescending(s => s[0])
                   .CopyInto(strings);

            Assert.Equal(new[] { "c", "b", "a", "cd", "aa" }, strings);
        }

        [Fact]
        public void ascending_then_ascending()
        {
            var strings = new[] { "a", "b", "aa", "c", "cd" };

            strings.AsSpan()
                   .OrderBy(strings, s => s.Length)
                   .ThenBy(s => s[0])
                   .CopyInto(strings);

            Assert.Equal(new[] { "a", "b", "c", "aa", "cd" }, strings);
        }

        [Fact]
        public void descending_then_ascending()
        {
            var strings = new[] { "a", "b", "aa", "c", "cd" };

            strings.AsSpan()
                   .OrderByDescending(strings, s => s.Length)
                   .ThenBy(s => s[0])
                   .CopyInto(strings);

            Assert.Equal(new[] { "aa", "cd", "a", "b", "c" }, strings);
        }

        [Fact]
        public void descending_then_descending()
        {
            var strings = new[] { "a", "b", "aa", "c", "cd" };

            strings.AsSpan()
                   .OrderByDescending(strings, s => s.Length)
                   .ThenByDescending(s => s[0])
                   .CopyInto(strings);

            Assert.Equal(new[] { "cd", "aa", "c", "b", "a" }, strings);
        }
    }
}
