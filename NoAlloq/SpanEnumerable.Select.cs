using NoAlloq.Producers;
using System;

namespace NoAlloq
{
    public ref partial struct SpanEnumerable<TOut>
    {
        /// <remarks>
        ///     From an enumerable and a <see cref="Func{TI, TO}"/>.
        /// </remarks>
        public SpanEnumerable<TOut2> Select<TOut2>(Func<TOut, TOut2> map) =>
            new SpanEnumerable<TOut2>(
                _input,
                new SecondarySelectDelegateProducer<byte, TOut, TOut2, IProducer<byte, TOut>>(
                    _producer, map),
                length: LengthIfKnown);
    }

    public ref partial struct SpanEnumerable<TIn, TOut, TProducer>
    {
        /// <remarks>
        ///     From an enumerable and a <see cref="Func{TI, TO}"/>.
        /// </remarks>
        public SpanEnumerable<TIn, TOut2, SecondarySelectDelegateProducer<TIn, TOut, TOut2, TProducer>>
            Select<TOut2>(Func<TOut, TOut2> map)
        =>
            new SpanEnumerable<TIn, TOut2, SecondarySelectDelegateProducer<TIn, TOut, TOut2, TProducer>>(
                Input,
                new SecondarySelectDelegateProducer<TIn, TOut, TOut2, TProducer>(
                    Producer, map),
                length: LengthIfKnown);
    }

    public partial struct SpanEnumerable<TOut, TProducer>
    {
        /// <remarks>
        ///     From an enumerable and a <see cref="Func{TI, TO}"/>.
        /// </remarks>
        public SpanEnumerable<TOut2, SecondarySelectDelegateProducer<TOut, TOut2, TProducer>>
            Select<TOut2>(Func<TOut, TOut2> map)
        =>
            new SpanEnumerable<TOut2, SecondarySelectDelegateProducer<TOut, TOut2, TProducer>>(
                new SecondarySelectDelegateProducer<TOut, TOut2, TProducer>(
                    Producer, map),
                length: LengthIfKnown);
    }
}
