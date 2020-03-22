using System;

namespace NoAlloq.Producers
{
    /// <summary> Filters a span-enumerable. </summary>
    public struct SecondaryWhereDelegateProducer<TIn, TOut, TProducer>
        : IProducer<TIn, TOut>
        where TProducer : IProducer<TIn, TOut>
    {
        private TProducer _inner;

        private readonly Predicate<TOut> _predicate;

        public SecondaryWhereDelegateProducer(
            TProducer inner,
            Predicate<TOut> predicate)
        {
            _inner = inner;
            _predicate = predicate;
        }

        public void Produce(ref ReadOnlySpan<TIn> input, ref Span<TOut> output)
        {
            var copied = 0;
            while (copied < output.Length)
            {
                // Use the part of the span where there are no values yet
                // to extract as many values from the inner predicate as
                // possible.
                var outSpan = output.Slice(copied);

                _inner.Produce(ref input, ref outSpan);

                for (var i = 0; i < outSpan.Length; ++i)
                    if (_predicate(outSpan[i]))
                        output[copied++] = outSpan[i];

                if (outSpan.Length < output.Length) 
                    break;
            }

            output = output.Slice(0, copied);
        }
    }

    /// <summary>
    ///     Produces an output value for each input value, by passing 
    ///     the value through another producer, then applying a delegate
    ///     to the result.
    /// </summary>
    public struct SecondaryWhereDelegateProducer<TOut, TProducer>
        : IProducer<TOut>
        where TProducer : IProducer<TOut>
    {
        private TProducer _inner;

        private readonly Predicate<TOut> _predicate;

        public SecondaryWhereDelegateProducer(
            TProducer inner,
            Predicate<TOut> predicate)
        {
            _inner = inner;
            _predicate = predicate;
        }

        public void Produce(ref Span<TOut> output)
        {
            var copied = 0;
            while (copied < output.Length)
            {
                // Use the part of the span where there are no values yet
                // to extract as many values from the inner predicate as
                // possible.
                var outSpan = output.Slice(copied);

                _inner.Produce(ref outSpan);

                for (var i = 0; i < outSpan.Length; ++i)
                    if (_predicate(outSpan[i]))
                        output[copied++] = outSpan[i];

                if (outSpan.Length < output.Length - copied)
                    break;
            }

            output = output.Slice(0, copied);
        }
    }
}
