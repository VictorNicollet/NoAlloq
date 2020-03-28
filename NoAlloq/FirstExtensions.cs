using NoAlloq.Ordering;
using NoAlloq.Producers;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NoAlloq
{
    public static class FirstExtensions
    {
        #region First

        /// <summary> The first element of a sequence. </summary>
        public static TOut First<TIn, TOut, TProducer>(
            this SpanEnumerable<TIn, TOut, TProducer> spanEnum)
            where TProducer : struct, IProducer<TIn, TOut>
        {
            TOut onStack = default;
            Span<TOut> outOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            return spanEnum.ConsumeInto(outOnStack).First();
        }

        /// <summary> The first element of a sequence. </summary>
        public static TOut First<TOut, TProducer>(
            this SpanEnumerable<TOut, TProducer> spanEnum)
            where TProducer : struct, IProducer<TOut>
        {
            TOut onStack = default;
            Span<TOut> outOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            return spanEnum.ConsumeInto(outOnStack).First();
        }

        /// <summary> The first element of a sequence. </summary>
        public static TOut First<TOut>(
            this SpanEnumerable<TOut> spanEnum)
        {
            TOut onStack = default;
            Span<TOut> outOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            return spanEnum.ConsumeInto(outOnStack).First();
        }

        /// <summary> The first element of a sequence. </summary>
        public static T First<T>(this ReadOnlySpan<T> span)
        {
            if (span.Length == 0)
                throw new InvalidOperationException("The input sequence is empty.");

            return span[0];
        }

        /// <summary> The first element of a sequence. </summary>
        public static T First<T>(this Span<T> span) =>
            First((ReadOnlySpan<T>)span);

        /// <summary> 
        ///     The first element of a sequence that satisfies a predicate. 
        /// </summary>
        public static TOut First<TIn, TOut, TProducer>(
            this SpanEnumerable<TIn, TOut, TProducer> spanEnum,
            Predicate<TOut> predicate)
            where TProducer : struct, IProducer<TIn, TOut>
        =>
            spanEnum.Where(predicate).First();

        /// <summary> 
        ///     The first element of a sequence that satisfies a predicate. 
        /// </summary>
        public static TOut First<TOut, TProducer>(
            this SpanEnumerable<TOut, TProducer> spanEnum,
            Predicate<TOut> predicate)
            where TProducer : struct, IProducer<TOut>
        =>
            spanEnum.Where(predicate).First();

        /// <summary> 
        ///     The first element of a sequence that satisfies a predicate. 
        /// </summary>
        public static TOut First<TOut>(
            this SpanEnumerable<TOut> spanEnum,
            Predicate<TOut> predicate)
        =>
            spanEnum.Where(predicate).First();

        /// <summary> 
        ///     The first element of a sequence that satisfies a predicate.
        /// </summary>
        public static T First<T>(
            this ReadOnlySpan<T> span,
            Predicate<T> predicate)
        {
            foreach (var s in span)
                if (predicate(s))
                    return s;

            throw new InvalidOperationException("The input sequence is empty.");
        }

        /// <summary> 
        ///     The first element of a sequence that satisfies a predicate.
        /// </summary>
        public static T First<T>(this Span<T> span, Predicate<T> predicate) =>
            First((ReadOnlySpan<T>)span, predicate);

        #endregion

        #region FirstOrDefault

        /// <summary> The first element of a sequence. </summary>
        public static TOut FirstOrDefault<TIn, TOut, TProducer>(
            this SpanEnumerable<TIn, TOut, TProducer> spanEnum)
            where TProducer : struct, IProducer<TIn, TOut>
        {
            TOut onStack = default;
            Span<TOut> outOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            return spanEnum.ConsumeInto(outOnStack).FirstOrDefault();
        }

        /// <summary> The first element of a sequence. </summary>
        public static TOut FirstOrDefault<TOut, TProducer>(
            this SpanEnumerable<TOut, TProducer> spanEnum)
            where TProducer : struct, IProducer<TOut>
        {
            TOut onStack = default;
            Span<TOut> outOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            return spanEnum.ConsumeInto(outOnStack).FirstOrDefault();
        }

        /// <summary> The first element of a sequence. </summary>
        public static TOut FirstOrDefault<TOut>(this SpanEnumerable<TOut> spanEnum)
        {
            TOut onStack = default;
            Span<TOut> outOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            return spanEnum.ConsumeInto(outOnStack).FirstOrDefault();
        }

        /// <summary> The first element of a sequence. </summary>
        public static T FirstOrDefault<T>(this ReadOnlySpan<T> span) => 
            span.Length == 0 ? (default) : span[0];

        /// <summary> The first element of a sequence. </summary>
        public static T FirstOrDefault<T>(this Span<T> span) =>
            FirstOrDefault((ReadOnlySpan<T>)span);

        /// <summary> 
        ///     The first element of a sequence that satisfies a predicate. 
        /// </summary>
        public static TOut FirstOrDefault<TIn, TOut, TProducer>(
            this SpanEnumerable<TIn, TOut, TProducer> spanEnum,
            Predicate<TOut> predicate)
            where TProducer : struct, IProducer<TIn, TOut>
        =>
            spanEnum.Where(predicate).FirstOrDefault();

        /// <summary> 
        ///     The first element of a sequence that satisfies a predicate. 
        /// </summary>
        public static TOut FirstOrDefault<TOut, TProducer>(
            this SpanEnumerable<TOut, TProducer> spanEnum,
            Predicate<TOut> predicate)
            where TProducer : struct, IProducer<TOut>
        =>
            spanEnum.Where(predicate).FirstOrDefault();

        /// <summary> 
        ///     The first element of a sequence that satisfies a predicate. 
        /// </summary>
        public static TOut FirstOrDefault<TOut>(
            this SpanEnumerable<TOut> spanEnum,
            Predicate<TOut> predicate)
        =>
            spanEnum.Where(predicate).FirstOrDefault();

        /// <summary> 
        ///     The first element of a sequence that satisfies a predicate.
        /// </summary>
        public static T FirstOrDefault<T>(
            this ReadOnlySpan<T> span,
            Predicate<T> predicate)
        {
            foreach (var s in span)
                if (predicate(s))
                    return s;

            return default;
        }

        /// <summary> 
        ///     The first element of a sequence that satisfies a predicate.
        /// </summary>
        public static T FirstOrDefault<T>(this Span<T> span, Predicate<T> predicate) =>
            FirstOrDefault((ReadOnlySpan<T>)span, predicate);

        #endregion

        #region Force OrderingPlan into SpanEnumerable

        /// <summary> 
        ///     The first element of a sequence that satisfies a predicate.
        /// </summary>
        public static TV FirstOrDefault<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan,
                Predicate<TV> predicate)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToEnumerable().FirstOrDefault(predicate);

        /// <summary> 
        ///     The first element of a sequence that satisfies a predicate.
        /// </summary>
        public static TV First<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan,
                Predicate<TV> predicate)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToEnumerable().First(predicate);

        /// <summary> The first element of a sequence. </summary>
        public static TV FirstOrDefault<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToEnumerable().FirstOrDefault();

        /// <summary> The first element of a sequence. </summary>
        public static TV First<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToEnumerable().First();

        #endregion
    }
}
