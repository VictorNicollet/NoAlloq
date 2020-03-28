using NoAlloq.Ordering;
using NoAlloq.Producers;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NoAlloq
{
    public static class CountExtensions
    {
        /// <summary> The number of elements in a sequence. </summary>
        public static int Count<TIn, TOut, TProducer>(
            this SpanEnumerable<TIn, TOut, TProducer> spanEnum)
            where TProducer : struct, IProducer<TIn, TOut>
        {
            if (spanEnum.KnownLength) 
                return spanEnum.Length;

            var count = 0;

            TOut onStack = default;
            var spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            while (spanEnum.ConsumeInto(spanOnStack).Length > 0)
                ++count;

            return count;
        }

        /// <summary> The number of elements in a sequence. </summary>
        public static int Count<TOut, TProducer>(
            this SpanEnumerable<TOut, TProducer> spanEnum)
            where TProducer : struct, IProducer<TOut>
        {
            if (spanEnum.KnownLength)
                return spanEnum.Length;

            var count = 0;

            TOut onStack = default;
            var spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            while (spanEnum.ConsumeInto(spanOnStack).Length > 0)
                ++count;

            return count;
        }

        /// <summary> The number of elements in a sequence. </summary>
        public static int Count<TOut>(this SpanEnumerable<TOut> spanEnum)
        {
            if (spanEnum.KnownLength)
                return spanEnum.Length;

            var count = 0;

            TOut onStack = default;
            var spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            while (spanEnum.ConsumeInto(spanOnStack).Length > 0)
                ++count;

            return count;
        }

        /// <summary> The number of elements in a sequence. </summary>
        public static int Count<T>(this ReadOnlySpan<T> span) => span.Length;

        /// <summary> The number of elements in a sequence. </summary>
        public static int Count<T>(this Span<T> span) => span.Length;

        /// <summary> The number of true elements in a boolean sequence. </summary>
        public static int CountTrue<TIn, TProducer>(
            this SpanEnumerable<TIn, bool, TProducer> spanEnum)
            where TProducer : struct, IProducer<TIn, bool>
        {
            var count = 0;

            long longOnStack = default;
            Span<bool> onStack =
                MemoryMarshal.Cast<long, bool>(
                    MemoryMarshal.CreateSpan(ref longOnStack, 1));

            while (true)
            {
                onStack = spanEnum.ConsumeInto(onStack);
                if (onStack.Length == 0) break;

                count += onStack.CountTrue();
            }

            return count;
        }

        /// <summary> The number of true elements in a boolean sequence. </summary>
        public static int CountTrue<TProducer>(
            this SpanEnumerable<bool, TProducer> spanEnum)
            where TProducer : struct, IProducer<bool>
        {
            var count = 0;

            long longOnStack = default;
            Span<bool> onStack =
                MemoryMarshal.Cast<long, bool>(
                    MemoryMarshal.CreateSpan(ref longOnStack, 1));

            while (true)
            {
                onStack = spanEnum.ConsumeInto(onStack);
                if (onStack.Length == 0) break;

                count += onStack.CountTrue();
            }

            return count;
        }

        /// <summary> The number of true elements in a boolean sequence. </summary>
        public static int CountTrue(this SpanEnumerable<bool> spanEnum)
        {
            var count = 0;

            long longOnStack = default;
            Span<bool> onStack =
                MemoryMarshal.Cast<long, bool>(
                    MemoryMarshal.CreateSpan(ref longOnStack, 1));

            while (true)
            {
                onStack = spanEnum.ConsumeInto(onStack);
                if (onStack.Length == 0) break;

                count += onStack.CountTrue();
            }

            return count;
        }

        /// <summary> The number of elements in a boolean sequence. </summary>
        public static int CountTrue(this ReadOnlySpan<bool> span)
        {
            var count = 0;
            foreach (var b in span)
                if (b)
                    ++count;

            return count;
        }

        /// <summary> The number of true elements in a boolean sequence. </summary>
        public static int CountTrue(this Span<bool> span) => 
            CountTrue((ReadOnlySpan<bool>)span);

        /// <summary> 
        ///     The number of elements in a sequence for which the provided 
        ///     predicate returns true.
        /// </summary>
        public static int Count<TIn, TOut, TProducer>(
            this SpanEnumerable<TIn, TOut, TProducer> spanEnum,
            Predicate<TOut> predicate)
            where TProducer : struct, IProducer<TIn, TOut>
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var count = 0;

            TOut onStack = default;
            var spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            while (spanEnum.ConsumeInto(spanOnStack).Length > 0)
                if (predicate(onStack))
                    ++count;

            return count;
        }

        /// <summary> 
        ///     The number of elements in a sequence for which the provided 
        ///     predicate returns true.
        /// </summary>
        public static int Count<TOut, TProducer>(
            this SpanEnumerable<TOut, TProducer> spanEnum,
            Predicate<TOut> predicate)
            where TProducer : struct, IProducer<TOut>
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var count = 0;

            TOut onStack = default;
            var spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            while (spanEnum.ConsumeInto(spanOnStack).Length > 0)
                if (predicate(onStack))
                    ++count;

            return count;
        }

        /// <summary> 
        ///     The number of elements in a sequence for which the provided 
        ///     predicate returns true.
        /// </summary>
        public static int Count<TOut>(
            this SpanEnumerable<TOut> spanEnum,
            Predicate<TOut> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var count = 0;

            TOut onStack = default;
            var spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            while (spanEnum.ConsumeInto(spanOnStack).Length > 0)
                if (predicate(onStack))
                    ++count;

            return count;
        }

        /// <summary> 
        ///     The number of elements in a sequence for which the provided 
        ///     predicate returns true.
        /// </summary>
        public static int Count<T>(
            this ReadOnlySpan<T> span,
            Predicate<T> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var count = 0;
            foreach (var s in span)
                if (predicate(s))
                    ++count;

            return count;
        }

        /// <summary> 
        ///     The number of elements in a sequence for which the provided 
        ///     predicate returns true.
        /// </summary>
        public static int Count<T>(this Span<T> span, Predicate<T> predicate) =>
            Count((ReadOnlySpan<T>)span, predicate);

        #region Force OrderingPlan into SpanEnumerable
        
        /// <summary> 
        ///     The number of elements in a sequence for which the provided 
        ///     predicate returns true.
        /// </summary>
        public static int Count<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan,
                Predicate<TV> predicate)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToBeSorted.Count(predicate); // Don't need to sort in order to count

        /// <summary> The number of true elements in a boolean sequence. </summary>
        public static int CountTrue<TK, TC, TE>(
                this OrderingPlan<bool, TK, TC, TE> plan)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<bool, TK>
        =>
            plan.ToBeSorted.CountTrue(); // Don't need to sort in order to count

        /// <summary> The number of elements in a sequence. </summary>
        public static int Count<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.Length; // Don't need to sort in order to count

        #endregion
    }
}
