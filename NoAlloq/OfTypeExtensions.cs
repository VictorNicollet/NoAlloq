using System;
using NoAlloq.Producers;

namespace NoAlloq
{
    public static class OfTypeExtensions
    {
        public static SpanEnumerable<TIn, TOut, OfTypeProducer<TIn, TOut>>
            OfType<TIn, TOut>(this Span<TIn> span)
            => OfType<TIn, TOut>((ReadOnlySpan<TIn>)span);

        public static SpanEnumerable<TIn, TOut, OfTypeProducer<TIn, TOut>>
            OfType<TIn, TOut>(this ReadOnlySpan<TIn> span)
            =>
                new SpanEnumerable<TIn, TOut, OfTypeProducer<TIn, TOut>>(
                    span,
                    new OfTypeProducer<TIn, TOut>(),
                    -1);
    }
}