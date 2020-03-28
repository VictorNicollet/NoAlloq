using System;
using System.Collections.Generic;
using NoAlloq.Ordering;
using NoAlloq.Producers;

namespace NoAlloq
{
    public static class WhereExtensions
    {
        /// <remarks>
        ///     From <see cref="Span{T}"/> and an arbitrary <see cref="IMap{TI, TO}"/>.
        /// </remarks>
        public static SpanEnumerable<T, T, WhereProducer<T, TMap>>
           Where<T, TMap>(
               this Span<T> input,
               TMap map) where TMap : struct, IMap<T, bool>
        =>
            Where((ReadOnlySpan<T>)input, map);

        /// <remarks>
        ///     From <see cref="ReadOnlySpan{T}"/> and an arbitrary <see cref="IMap{TI, TO}"/>.
        /// </remarks>
        public static SpanEnumerable<T, T, WhereProducer<T, TMap>>
            Where<T, TMap>(
                this ReadOnlySpan<T> input,
                TMap map) where TMap : struct, IMap<T, bool>
        =>
            new SpanEnumerable<T, T, WhereProducer<T, TMap>>(
                input,
                new WhereProducer<T, TMap>(map),
                length: -1);

        /// <remarks>
        ///     From <see cref="Span{T}"/> and an arbitrary predicate.
        /// </remarks>
        public static SpanEnumerable<T, T, WhereDelegateProducer<T>>
           Where<T>(
               this Span<T> input,
               Predicate<T> predicate)
        =>
            Where((ReadOnlySpan<T>)input, predicate);

        /// <remarks>
        ///     From <see cref="ReadOnlySpan{T}"/> and an arbitrary 
        ///     predicate.
        /// </remarks>
        public static SpanEnumerable<T, T, WhereDelegateProducer<T>>
            Where<T>(
                this ReadOnlySpan<T> input,
                Predicate<T> predicate)
        =>
            new SpanEnumerable<T, T, WhereDelegateProducer<T>>(
                input,
                new WhereDelegateProducer<T>(
                    predicate ?? throw new ArgumentNullException()),
                length: -1);

        #region Force OrderingPlan into SpanEnumerable

        // Without type inference, is sadness.

        public static SpanEnumerable<TV, TV,
            SecondaryWhereDelegateProducer<TV, TV, 
                OrderingProducer<TV, TK, TC, TE>>>
            Where<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan,
                Predicate<TV> predicate)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToEnumerable().Where(predicate);

        #endregion
    }
}
