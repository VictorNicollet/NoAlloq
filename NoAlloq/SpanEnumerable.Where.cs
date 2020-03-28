using NoAlloq.Producers;
using System;

namespace NoAlloq
{
    public ref partial struct SpanEnumerable<TOut>
    {
        /// <remarks>
        ///     From an enumerable and a <see cref="Predicate{TOut}"/>.
        /// </remarks>
        public SpanEnumerable<TOut> Where(Predicate<TOut> predicate) =>
            new SpanEnumerable<TOut>(
                _input,
                new SecondaryWhereDelegateProducer<byte, TOut, IProducer<byte, TOut>>(
                    _producer,
                    predicate ?? throw new ArgumentNullException(nameof(predicate))),
                length: -1);
    }

    public ref partial struct SpanEnumerable<TIn, TOut, TProducer>
    {
        /// <remarks>
        ///     From an enumerable and a <see cref="Predicate{TOut}"/>.
        /// </remarks>
        public SpanEnumerable<TIn, TOut, SecondaryWhereDelegateProducer<TIn, TOut, TProducer>>
            Where(Predicate<TOut> predicate)
        =>
            new SpanEnumerable<TIn, TOut, SecondaryWhereDelegateProducer<TIn, TOut, TProducer>>(
                Input,
                new SecondaryWhereDelegateProducer<TIn, TOut, TProducer>(
                    Producer, 
                    predicate ?? throw new ArgumentNullException(nameof(predicate))),
                length: -1);
    }

    public ref partial struct SpanEnumerable<TOut, TProducer>
    {
        /// <remarks>
        ///     From an enumerable and a <see cref="Predicate{TOut}"/>.
        /// </remarks>
        public SpanEnumerable<TOut, SecondaryWhereDelegateProducer<TOut, TProducer>>
            Where(Predicate<TOut> predicate)
        =>
            new SpanEnumerable<TOut, SecondaryWhereDelegateProducer<TOut, TProducer>>(
                new SecondaryWhereDelegateProducer<TOut, TProducer>(
                    Producer,
                    predicate ?? throw new ArgumentNullException(nameof(predicate))),
                length: -1);
    }
}
