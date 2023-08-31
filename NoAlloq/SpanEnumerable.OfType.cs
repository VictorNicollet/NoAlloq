using NoAlloq.Producers;
using System;

namespace NoAlloq
{
    public ref partial struct SpanEnumerable<TIn, TOut, TProducer>
    {
        public SpanEnumerable<TIn, TOut2, SecondaryOfTypeProducer<TIn, TOut, TOut2, TProducer>>
            OfType<TOut2>()
        =>
            new SpanEnumerable<TIn, TOut2, SecondaryOfTypeProducer<TIn, TOut, TOut2, TProducer>>(
                Input,
                new SecondaryOfTypeProducer<TIn, TOut, TOut2, TProducer>(
                    Producer),
                length: LengthIfKnown);
    }
}
