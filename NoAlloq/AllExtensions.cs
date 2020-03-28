using NoAlloq.Ordering;
using NoAlloq.Producers;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NoAlloq
{
    public static class AllExtensions
    {
        /// <summary> Whether a sequence contains only 'true'. </summary>
        public static bool AllTrue<TIn, TProducer>(
            this SpanEnumerable<TIn, bool, TProducer> spanEnum)
            where TProducer : struct, IProducer<TIn, bool>
        {
            bool onStack = default;
            Span<bool> spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            while (true)
            {
                spanOnStack = spanEnum.ConsumeInto(spanOnStack);
                if (spanOnStack.Length == 0) return true;
                if (!onStack) return false;
            }
        }

        /// <summary> Whether a sequence contains only 'true'. </summary>
        public static bool AllTrue<TProducer>(
            this SpanEnumerable<bool, TProducer> spanEnum)
            where TProducer : struct, IProducer<bool>
        {
            bool onStack = default;
            Span<bool> spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            while (true)
            {
                spanOnStack = spanEnum.ConsumeInto(spanOnStack);
                if (spanOnStack.Length == 0) return true;
                if (!onStack) return false;
            }
        }

        /// <summary> Whether a sequence contains only 'true'. </summary>
        public static bool AllTrue(this SpanEnumerable<bool> spanEnum)
        {
            bool onStack = default;
            Span<bool> spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            while (true)
            {
                spanOnStack = spanEnum.ConsumeInto(spanOnStack);
                if (spanOnStack.Length == 0) return true;
                if (!onStack) return false;
            }
        }

        /// <summary> Whether a sequence contains only 'true'. </summary>
        public static bool AllTrue(this ReadOnlySpan<bool> span)
        {
            foreach (var b in span)
                if (!b)
                    return false;

            return true;
        }

        /// <summary> Whether a sequence contains only 'true'. </summary>
        public static bool AllTrue(this Span<bool> span) => 
            AllTrue((ReadOnlySpan<bool>)span);

        /// <summary> 
        ///     Whether the predicate returns true for all elements
        ///     in a sequence.
        /// </summary>
        public static bool All<TIn, TOut, TProducer>(
            this SpanEnumerable<TIn, TOut, TProducer> spanEnum,
            Predicate<TOut> predicate)
            where TProducer : struct, IProducer<TIn, TOut>
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            TOut onStack = default;
            var spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            while (spanEnum.ConsumeInto(spanOnStack).Length > 0)
                if (!predicate(onStack))
                    return false;

            return true;
        }

        /// <summary> 
        ///     Whether the predicate returns true for all elements
        ///     in a sequence.
        /// </summary>
        public static bool All<TOut, TProducer>(
            this SpanEnumerable<TOut, TProducer> spanEnum,
            Predicate<TOut> predicate)
            where TProducer : struct, IProducer<TOut>
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            TOut onStack = default;
            var spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            while (spanEnum.ConsumeInto(spanOnStack).Length > 0)
                if (!predicate(onStack))
                    return false;

            return true;
        }

        /// <summary> 
        ///     Whether the predicate returns true for all elements
        ///     in a sequence.
        /// </summary>
        public static bool All<TOut>(
            this SpanEnumerable<TOut> spanEnum,
            Predicate<TOut> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            TOut onStack = default;
            var spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            while (spanEnum.ConsumeInto(spanOnStack).Length > 0)
                if (!predicate(onStack))
                    return false;

            return true;
        }

        /// <summary> 
        ///     Whether the predicate returns true for all elements
        ///     in a sequence.
        /// </summary>
        public static bool All<T>(
            this ReadOnlySpan<T> span,
            Predicate<T> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            foreach (var s in span)
                if (!predicate(s))
                    return false;

            return true;
        }

        /// <summary> 
        ///     Whether the predicate returns true for all elements
        ///     in a sequence.
        /// </summary>
        public static bool All<T>(this Span<T> span, Predicate<T> predicate) =>
            All((ReadOnlySpan<T>)span, predicate);

        #region Force OrderingPlan into SpanEnumerable

        /// <summary> 
        ///     Whether the predicate returns true for all elements
        ///     in a sequence.
        /// </summary>
        public static bool All<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan,
                Predicate<TV> predicate)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToBeSorted.All(predicate); // Don't need to sort in order to check

        /// <summary> Whether a sequence contains only 'true'. </summary>
        public static bool AllTrue<TK, TC, TE>(
                this OrderingPlan<bool, TK, TC, TE> plan)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<bool, TK>
        =>
            plan.ToBeSorted.AllTrue(); // Don't need to sort in order to check

        #endregion 
    }
}
