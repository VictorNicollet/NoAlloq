using System;
using System.Linq;
using Xunit;

namespace NoAlloq.Tests
{
    public class of_type
    {
        public interface IData
        {
            int Id { get; }
        }

        class Data1 : IData
        {
            public int Id { get; set; }
        }

        class Data2 : IData
        {
            public int Id { get; set; }
        }

        private readonly IData[] _testArray =
        {
            new Data1 { Id = 10 },
            new Data1 { Id = 10 },
            new Data1 { Id = 1 },
            new Data2 { Id = 2 },
            new Data1 { Id = 1 },
            new Data2 { Id = 2 },
        };


        [Fact]
        public void of_type_span()
        {
            var withSpanLinq = _testArray.AsSpan().OfType<IData, Data1>().ToArray();
            var withLinq = _testArray.OfType<Data1>().ToArray();

            Assert.Equal(withSpanLinq, withLinq);
        }

        [Fact]
        public void of_type_span_then_where()
        {
            var withSpanLinq = _testArray.AsSpan().OfType<IData, Data1>().Where(d => d.Id == 1).ToArray();
            var withLinq = _testArray.OfType<Data1>().Where(d => d.Id == 1).ToArray();

            Assert.Equal(withSpanLinq, withLinq);
        }

        [Fact]
        public void of_type_after_where()
        {
            var withSpanLinq = _testArray.AsSpan().Where(d => d.Id == 1).OfType<Data1>().ToArray();
            var withLinq = _testArray.Where(d => d.Id == 1).OfType<Data1>().ToArray();

            Assert.Equal(withSpanLinq, withLinq);
        }

        [Fact]
        public void of_type_after_select_select()
        {
            var withSpanLinq = _testArray.AsSpan().Select(d => d.Id).Select(i => i.ToString()).OfType<string>()
                .ToArray();
            var withLinq = _testArray.Select(d => d.Id).Select(i => i.ToString()).OfType<string>().ToArray();

            Assert.Equal(withSpanLinq, withLinq);
        }

        [Fact]
        public void of_type_span_with_slice_and_select()
        {
            var withSpanLinq = _testArray.AsSpan().OfType<IData, Data1>().Slice(2, 2).Select(d => d.Id * 10).ToArray();
            var withLinq = _testArray.OfType<Data1>().Skip(2).Take(2).Select(d => d.Id * 10).ToArray();

            Assert.Equal(withSpanLinq, withLinq);
        }

        [Fact]
        public void of_type_then_order_by()
        {
            var backing = new Data1[_testArray.Length];
            var withSpanLinq = _testArray.AsSpan().OfType<IData, Data1>().OrderBy(backing, data => data.Id).ToArray();

            var withLinq = _testArray.OfType<Data1>().OrderBy(data => data.Id).ToArray();

            for (int i = 0; i < withSpanLinq.Length; i++)
            {
                var fromSpanLinq = withSpanLinq[i];
                var fromLinq = withLinq[i];

                Assert.Equal(fromSpanLinq.Id, fromLinq.Id);
            }
        }
    }
}