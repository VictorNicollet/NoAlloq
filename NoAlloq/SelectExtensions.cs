using System;
using System.Collections.Generic;
using NoAlloq.Ordering;
using NoAlloq.Producers;

namespace NoAlloq
{
    public static class SelectExtensions
    {
        /// <remarks>
        ///     From <see cref="Span{T}"/> and an arbitrary <see cref="IMap{TI, TO}"/>.
        /// </remarks>
        public static SpanEnumerable<TIn, TOut, SelectProducer<TIn, TOut, TMap>>
           Select<TIn, TOut, TMap>(
               this Span<TIn> input,
               TMap map) where TMap : struct, IMap<TIn, TOut>
        =>
            Select<TIn, TOut, TMap>((ReadOnlySpan<TIn>)input, map);

        /// <remarks>
        ///     From <see cref="ReadOnlySpan{T}"/> and an arbitrary <see cref="IMap{TI, TO}"/>.
        /// </remarks>
        public static SpanEnumerable<TIn, TOut, SelectProducer<TIn, TOut, TMap>> 
            Select<TIn, TOut, TMap>(
                this ReadOnlySpan<TIn> input,
                TMap map) where TMap : struct, IMap<TIn, TOut>
        =>
            new SpanEnumerable<TIn, TOut, SelectProducer<TIn, TOut, TMap>>(
                input,
                new SelectProducer<TIn, TOut, TMap>(map),
                length: input.Length);

        /// <remarks>
        ///     From <see cref="Span{T}"/> and a <see cref="Func{TI, TO}"/>.
        /// </remarks>
        public static SpanEnumerable<TIn, TOut, SelectDelegateProducer<TIn, TOut>>
           Select<TIn, TOut>(
               this Span<TIn> input,
               Func<TIn, TOut> map)
        =>
            Select((ReadOnlySpan<TIn>)input, map);

        /// <remarks>
        ///     From <see cref="ReadOnlySpan{T}"/> and a <see cref="Func{TI, TO}"/>.
        /// </remarks>
        public static SpanEnumerable<TIn, TOut, SelectDelegateProducer<TIn, TOut>>
            Select<TIn, TOut>(
                this ReadOnlySpan<TIn> input,
                Func<TIn, TOut> map)
        =>
            new SpanEnumerable<TIn, TOut, SelectDelegateProducer<TIn, TOut>>(
                input,
                new SelectDelegateProducer<TIn, TOut>(
                    map ?? throw new ArgumentNullException(nameof(map))),
                length: input.Length);

        #region Force OrderingPlan into SpanEnumerable

        // An offering to the C# type system.

        public static SpanEnumerable<TV, TOut, 
            SecondarySelectDelegateProducer<TV, TV, TOut, 
                OrderingProducer<TV, TK, TC, TE>>> 
            Select<TV, TK, TC, TE, TOut>(
                this OrderingPlan<TV, TK, TC, TE> plan,
                Func<TV, TOut> d)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToEnumerable().Select(d); 

        #endregion
    }
}
