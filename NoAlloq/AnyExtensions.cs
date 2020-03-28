using NoAlloq.Ordering;
using NoAlloq.Producers;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NoAlloq
{
    public static class AnyExtensions
    {
        /// <summary> Whether a sequence is non-empty. </summary>
        public static bool Any<TIn, TOut, TProducer>(
            this SpanEnumerable<TIn, TOut, TProducer> spanEnum)
            where TProducer : struct, IProducer<TIn, TOut>
        {
            if (spanEnum.KnownLength) 
                return spanEnum.Length > 0;

            TOut onStack = default;
            var spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            return spanEnum.ConsumeInto(spanOnStack).Length > 0;
        }

        /// <summary> Whether a sequence is non-empty. </summary>
        public static bool Any<TOut, TProducer>(
            this SpanEnumerable<TOut, TProducer> spanEnum)
            where TProducer : struct, IProducer<TOut>
        {
            if (spanEnum.KnownLength)
                return spanEnum.Length > 0;

            TOut onStack = default;
            var spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            return spanEnum.ConsumeInto(spanOnStack).Length > 0;
        }

        /// <summary> Whether a sequence is non-empty. </summary>
        public static bool Any<TOut>(this SpanEnumerable<TOut> spanEnum)
        {
            if (spanEnum.KnownLength)
                return spanEnum.Length > 0;

            TOut onStack = default;
            var spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            return spanEnum.ConsumeInto(spanOnStack).Length > 0;
        }

        /// <summary> Whether a sequence is non-empty. </summary>
        public static bool Any<T>(this ReadOnlySpan<T> span) => span.Length > 0;

        /// <summary> Whether a sequence is non-empty. </summary>
        public static bool Any<T>(this Span<T> span) => span.Length > 0;

        /// <summary> Whether a sequence contains 'true'. </summary>
        public static bool AnyTrue<TIn, TProducer>(
            this SpanEnumerable<TIn, bool, TProducer> spanEnum)
            where TProducer : struct, IProducer<TIn, bool>
        {
            bool onStack = default;
            Span<bool> spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            while (true)
            {
                spanOnStack = spanEnum.ConsumeInto(spanOnStack);
                if (spanOnStack.Length == 0) return false;
                if (onStack) return true;
            }
        }

        /// <summary> Whether a sequence contains 'true'. </summary>
        public static bool AnyTrue<TProducer>(
            this SpanEnumerable<bool, TProducer> spanEnum)
            where TProducer : struct, IProducer<bool>
        {
            bool onStack = default;
            Span<bool> spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            while (true)
            {
                spanOnStack = spanEnum.ConsumeInto(spanOnStack);
                if (spanOnStack.Length == 0) return false;
                if (onStack) return true;
            }
        }

        /// <summary> Whether a sequence contains 'true'. </summary>
        public static bool AnyTrue(this SpanEnumerable<bool> spanEnum)
        {
            bool onStack = default;
            Span<bool> spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            while (true)
            {
                spanOnStack = spanEnum.ConsumeInto(spanOnStack);
                if (spanOnStack.Length == 0) return false;
                if (onStack) return true;
            }
        }

        /// <summary> Whether a sequence contains 'true'. </summary>
        public static bool AnyTrue(this ReadOnlySpan<bool> span)
        {
            foreach (var b in span)
                if (b)
                    return true;

            return false;
        }

        /// <summary> Whether a sequence contains 'true'. </summary>
        public static bool AnyTrue(this Span<bool> span) => 
            AnyTrue((ReadOnlySpan<bool>)span);

        /// <summary> 
        ///     Whether the predicate returns true for any element
        ///     in a sequence.
        /// </summary>
        public static bool Any<TIn, TOut, TProducer>(
            this SpanEnumerable<TIn, TOut, TProducer> spanEnum,
            Predicate<TOut> predicate)
            where TProducer : struct, IProducer<TIn, TOut>
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            TOut onStack = default;
            var spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            while (spanEnum.ConsumeInto(spanOnStack).Length > 0)
                if (predicate(onStack))
                    return true;

            return false;
        }

        /// <summary> 
        ///     Whether the predicate returns true for any element
        ///     in a sequence.
        /// </summary>
        public static bool Any<TOut, TProducer>(
            this SpanEnumerable<TOut, TProducer> spanEnum,
            Predicate<TOut> predicate)
            where TProducer : struct, IProducer<TOut>
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            TOut onStack = default;
            var spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            while (spanEnum.ConsumeInto(spanOnStack).Length > 0)
                if (predicate(onStack))
                    return true;

            return false;
        }

        /// <summary> 
        ///     Whether the predicate returns true for any element
        ///     in a sequence.
        /// </summary>
        public static bool Any<TOut>(
            this SpanEnumerable<TOut> spanEnum,
            Predicate<TOut> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            TOut onStack = default;
            var spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            while (spanEnum.ConsumeInto(spanOnStack).Length > 0)
                if (predicate(onStack))
                    return true;

            return false;
        }

        /// <summary> 
        ///     Whether the predicate returns true for any element
        ///     in a sequence.
        /// </summary>
        public static bool Any<T>(
            this ReadOnlySpan<T> span,
            Predicate<T> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            foreach (var s in span)
                if (predicate(s))
                    return true;

            return false;
        }

        /// <summary> 
        ///     Whether the predicate returns true for any element
        ///     in a sequence.
        /// </summary>
        public static bool Any<T>(this Span<T> span, Predicate<T> predicate) =>
            Any((ReadOnlySpan<T>)span, predicate);

        #region Force OrderingPlan into SpanEnumerable

        /// <summary> Whether a sequence is non-empty. </summary>
        public static bool Any<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan,
                Predicate<TV> predicate)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToBeSorted.Any(predicate); // Don't need to sort in order to check

        /// <summary> Whether a sequence contains 'true'. </summary>
        public static bool AnyTrue<TK, TC, TE>(
                this OrderingPlan<bool, TK, TC, TE> plan)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<bool, TK>
        =>
            plan.ToBeSorted.AnyTrue(); // Don't need to sort in order to check

        /// <summary> 
        ///     Whether the predicate returns true for any element
        ///     in a sequence.
        /// </summary>
        public static bool Any<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.Length > 0; // Don't need to sort in order to check

        #endregion 
    }
}
