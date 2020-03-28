using NoAlloq.Ordering;
using NoAlloq.Producers;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NoAlloq
{
    public static class LastExtensions
    {
        #region Last

        /// <summary> The last element of a sequence. </summary>
        public static TOut Last<TIn, TOut, TProducer>(
            this SpanEnumerable<TIn, TOut, TProducer> spanEnum)
            where TProducer : struct, IProducer<TIn, TOut>
        {
            TOut onStack = default;
            Span<TOut> outOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            if (spanEnum.ConsumeInto(outOnStack).Length == 0)
                throw new InvalidOperationException("The input sequence is empty.");

            var last = onStack;
            while (spanEnum.ConsumeInto(outOnStack).Length > 0)
                last = onStack;

            return last;
        }

        /// <summary> The last element of a sequence. </summary>
        public static TOut Last<TOut, TProducer>(
            this SpanEnumerable<TOut, TProducer> spanEnum)
            where TProducer : struct, IProducer<TOut>
        {
            TOut onStack = default;
            Span<TOut> outOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            if (spanEnum.ConsumeInto(outOnStack).Length == 0)
                throw new InvalidOperationException("The input sequence is empty.");

            var last = onStack;
            while (spanEnum.ConsumeInto(outOnStack).Length > 0)
                last = onStack;

            return last;
        }

        /// <summary> The last element of a sequence. </summary>
        public static TOut Last<TOut>(this SpanEnumerable<TOut> spanEnum)
        {
            TOut onStack = default;
            Span<TOut> outOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            if (spanEnum.ConsumeInto(outOnStack).Length == 0)
                throw new InvalidOperationException("The input sequence is empty.");

            var last = onStack;
            while (spanEnum.ConsumeInto(outOnStack).Length > 0)
                last = onStack;

            return last;
        }

        /// <summary> The last element of a sequence. </summary>
        public static T Last<T>(this ReadOnlySpan<T> span)
        {
            if (span.Length == 0)
                throw new InvalidOperationException("The input sequence is empty.");

            return span[span.Length - 1];
        }

        /// <summary> The last element of a sequence. </summary>
        public static T Last<T>(this Span<T> span) =>
            Last((ReadOnlySpan<T>)span);

        /// <summary> 
        ///     The first element of a sequence that satisfies a predicate. 
        /// </summary>
        public static TOut Last<TIn, TOut, TProducer>(
            this SpanEnumerable<TIn, TOut, TProducer> spanEnum,
            Predicate<TOut> predicate)
            where TProducer : struct, IProducer<TIn, TOut>
        =>
            spanEnum.Where(predicate).Last();

        /// <summary> 
        ///     The first element of a sequence that satisfies a predicate. 
        /// </summary>
        public static TOut Last<TOut, TProducer>(
            this SpanEnumerable<TOut, TProducer> spanEnum,
            Predicate<TOut> predicate)
            where TProducer : struct, IProducer<TOut>
        =>
            spanEnum.Where(predicate).Last();

        /// <summary> 
        ///     The first element of a sequence that satisfies a predicate. 
        /// </summary>
        public static TOut Last<TOut>(
            this SpanEnumerable<TOut> spanEnum,
            Predicate<TOut> predicate)
        =>
            spanEnum.Where(predicate).Last();

        /// <summary> 
        ///     The first element of a sequence that satisfies a predicate.
        /// </summary>
        public static T Last<T>(
            this ReadOnlySpan<T> span,
            Predicate<T> predicate)
        {
            for (var i = span.Length - 1; i >= 0; --i)
                if (predicate(span[i]))
                    return span[i];

            throw new InvalidOperationException("The input sequence is empty.");
        }

        /// <summary> 
        ///     The first element of a sequence that satisfies a predicate.
        /// </summary>
        public static T Last<T>(this Span<T> span, Predicate<T> predicate) =>
            Last((ReadOnlySpan<T>)span, predicate);

        #endregion

        #region LastOrDefault

        /// <summary> The last element of a sequence. </summary>
        public static TOut LastOrDefault<TIn, TOut, TProducer>(
            this SpanEnumerable<TIn, TOut, TProducer> spanEnum)
            where TProducer : struct, IProducer<TIn, TOut>
        {
            TOut onStack = default;
            Span<TOut> outOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            var last = onStack;
            while (spanEnum.ConsumeInto(outOnStack).Length > 0)
                last = onStack;

            return last;
        }

        /// <summary> The last element of a sequence. </summary>
        public static TOut LastOrDefault<TOut, TProducer>(
            this SpanEnumerable<TOut, TProducer> spanEnum)
            where TProducer : struct, IProducer<TOut>
        {
            TOut onStack = default;
            Span<TOut> outOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            var last = onStack;
            while (spanEnum.ConsumeInto(outOnStack).Length > 0)
                last = onStack;

            return last;
        }

        /// <summary> The last element of a sequence. </summary>
        public static TOut LastOrDefault<TOut>(
            this SpanEnumerable<TOut> spanEnum)
        {
            TOut onStack = default;
            Span<TOut> outOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            var last = onStack;
            while (spanEnum.ConsumeInto(outOnStack).Length > 0)
                last = onStack;

            return last;
        }

        /// <summary> The last element of a sequence. </summary>
        public static T LastOrDefault<T>(this ReadOnlySpan<T> span) => 
            span.Length == 0 ? (default) : span[span.Length - 1];

        /// <summary> The last element of a sequence. </summary>
        public static T LastOrDefault<T>(this Span<T> span) =>
            LastOrDefault((ReadOnlySpan<T>)span);

        /// <summary> 
        ///     The last element of a sequence that satisfies a predicate. 
        /// </summary>
        public static TOut LastOrDefault<TIn, TOut, TProducer>(
            this SpanEnumerable<TIn, TOut, TProducer> spanEnum,
            Predicate<TOut> predicate)
            where TProducer : struct, IProducer<TIn, TOut>
        =>
            spanEnum.Where(predicate).LastOrDefault();

        /// <summary> 
        ///     The last element of a sequence that satisfies a predicate. 
        /// </summary>
        public static TOut LastOrDefault<TOut, TProducer>(
            this SpanEnumerable<TOut, TProducer> spanEnum,
            Predicate<TOut> predicate)
            where TProducer : struct, IProducer<TOut>
        =>
            spanEnum.Where(predicate).LastOrDefault();

        /// <summary> 
        ///     The last element of a sequence that satisfies a predicate. 
        /// </summary>
        public static TOut LastOrDefault<TOut>(
            this SpanEnumerable<TOut> spanEnum,
            Predicate<TOut> predicate)
        =>
            spanEnum.Where(predicate).LastOrDefault();

        /// <summary> 
        ///     The last element of a sequence that satisfies a predicate.
        /// </summary>
        public static T LastOrDefault<T>(
            this ReadOnlySpan<T> span,
            Predicate<T> predicate)
        {
            for (var i = span.Length - 1; i >= 0; --i)
                if (predicate(span[i]))
                    return span[i];

            return default;
        }

        /// <summary> 
        ///     The last element of a sequence that satisfies a predicate.
        /// </summary>
        public static T LastOrDefault<T>(this Span<T> span, Predicate<T> predicate) =>
            LastOrDefault((ReadOnlySpan<T>)span, predicate);

        #endregion

        #region Force OrderingPlan into SpanEnumerable

        /// <summary> 
        ///     The last element of a sequence that satisfies a predicate.
        /// </summary>
        public static TV LastOrDefault<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan,
                Predicate<TV> predicate)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToEnumerable().LastOrDefault(predicate);

        /// <summary> 
        ///     The last element of a sequence that satisfies a predicate.
        /// </summary>
        public static TV Last<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan,
                Predicate<TV> predicate)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToEnumerable().Last(predicate);

        /// <summary> The last element of a sequence. </summary>
        public static TV LastOrDefault<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToEnumerable().LastOrDefault();

        /// <summary> The last element of a sequence. </summary>
        public static TV Last<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToEnumerable().Last();

        #endregion
    }
}
