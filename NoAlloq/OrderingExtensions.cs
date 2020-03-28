using System;
using System.Collections.Generic;
using NoAlloq.Ordering;
using NoAlloq.Producers;

namespace NoAlloq
{
    public static class OrderingExtensions
    {
        /// <summary>
        ///     If two spans reference different memory areas, copy from the first
        ///     to the second.
        /// </summary>
        private static void CopyIfNotSame<T>(ReadOnlySpan<T> a, Span<T> b)
        {
            if (a.Length > 0)
                if (!b.Overlaps(a, out var offset) || offset != 0)
                    a.CopyTo(b);
        }

        #region ReadOnlySpan<T> ordering

        /// <summary>
        ///     Prepare to order the contents of a span by the specified 
        ///     extractor and comparer.
        /// </summary>
        internal static OrderingPlan<TValue, TKey, TComparer, TExtractor> 
            InternalOrderBy<TKey, TValue, TComparer, TExtractor>(
                this ReadOnlySpan<TValue> input,
                Span<TValue> backing,
                TComparer comparer,
                TExtractor extractor)
            where TExtractor : struct, IKeyExtractor<TValue, TKey>
            where TComparer : IComparer<TKey>
        {
            if (backing.Length < input.Length)
                throw new ArgumentOutOfRangeException(
                    nameof(backing),
                    backing.Length,
                    $"Need a backing span of length {input.Length}.");

            backing = backing.Slice(0, input.Length);

            CopyIfNotSame(input, backing);

            return new OrderingPlan<TValue, TKey, TComparer, TExtractor>(
                extractor, comparer, backing);
        }

        /// <summary>
        ///     Orders a span using the default comparer for the contents
        ///     of the span.
        /// </summary>
        public static OrderingPlan<T, T, Comparer<T>, IdentityExtractor<T>> OrderByValue<T>(
            this ReadOnlySpan<T> input,
            Span<T> backing)
        =>
            InternalOrderBy<T, T, Comparer<T>, IdentityExtractor<T>>(
                input,
                backing,
                Comparer<T>.Default,
                new IdentityExtractor<T>());

        /// <summary>
        ///     Orders a span using the provided comparer for the contents
        ///     of the span.
        /// </summary>
        public static OrderingPlan<T, T, IComparer<T>, IdentityExtractor<T>> OrderByValue<T>(
            this ReadOnlySpan<T> input,
            Span<T> backing,
            IComparer<T> comparer)
        =>
            InternalOrderBy<T, T, IComparer<T>, IdentityExtractor<T>>(
                input,
                backing,
                comparer,
                new IdentityExtractor<T>());

        /// <summary>
        ///     Orders a span using the default comparer for a key extracted from
        ///     each value by an extractor method.
        /// </summary>
        public static OrderingPlan<TValue, TKey, Comparer<TKey>, DelegateExtractor<TValue, TKey>> 
            OrderBy<TValue, TKey>(
                this ReadOnlySpan<TValue> input,
                Span<TValue> backing,
                Func<TValue, TKey> keySelector)
        =>
            InternalOrderBy<TKey, TValue, Comparer<TKey>, DelegateExtractor<TValue, TKey>>(
                input,
                backing,
                Comparer<TKey>.Default,
                new DelegateExtractor<TValue, TKey>(
                    keySelector ?? throw new ArgumentNullException(nameof(keySelector))));

        /// <summary>
        ///     Orders a span using the provided comparer for a key extracted from
        ///     each value by an extractor method.
        /// </summary>
        public static OrderingPlan<TValue, TKey, IComparer<TKey>, DelegateExtractor<TValue, TKey>> 
            OrderBy<TValue, TKey>(
                this ReadOnlySpan<TValue> input,
                Span<TValue> backing,
                Func<TValue, TKey> keySelector,
                IComparer<TKey> comparer)
        =>
            InternalOrderBy<TKey, TValue, IComparer<TKey>, DelegateExtractor<TValue, TKey>>(
                input,
                backing,
                comparer,
                new DelegateExtractor<TValue, TKey>(
                    keySelector ?? throw new ArgumentNullException(nameof(keySelector))));

        /// <summary>
        ///     Orders a span using the default comparer for the contents
        ///     of the span.
        /// </summary>
        public static OrderingPlan<T, T, DescendingComparer<T, Comparer<T>>, IdentityExtractor<T>> 
            OrderByValueDescending<T>(
                this ReadOnlySpan<T> input,
                Span<T> backing)
        =>
            InternalOrderBy<T, T, DescendingComparer<T, Comparer<T>>, IdentityExtractor<T>>(
                input,
                backing,
                new DescendingComparer<T, Comparer<T>>(Comparer<T>.Default),
                new IdentityExtractor<T>());

        /// <summary>
        ///     Orders a span using the provided comparer for the contents
        ///     of the span.
        /// </summary>
        public static OrderingPlan<T, T, DescendingComparer<T, IComparer<T>>, IdentityExtractor<T>> 
            OrderByValueDescending<T>(
                this ReadOnlySpan<T> input,
                Span<T> backing,
                IComparer<T> comparer)
        =>
            InternalOrderBy<T, T, DescendingComparer<T, IComparer<T>>, IdentityExtractor<T>>(
                input,
                backing,
                new DescendingComparer<T, IComparer<T>>(comparer),
                new IdentityExtractor<T>());

        /// <summary>
        ///     Orders a span using the default comparer for a key extracted from
        ///     each value by an extractor method.
        /// </summary>
        public static OrderingPlan<TValue, TKey, DescendingComparer<TKey, Comparer<TKey>>, DelegateExtractor<TValue, TKey>>
            OrderByDescending<TValue, TKey>(
                this ReadOnlySpan<TValue> input,
                Span<TValue> backing,
                Func<TValue, TKey> keySelector)
        =>
            InternalOrderBy<TKey, TValue, DescendingComparer<TKey, Comparer<TKey>>, DelegateExtractor<TValue, TKey>>(
                input,
                backing,
                new DescendingComparer<TKey, Comparer<TKey>>(Comparer<TKey>.Default),
                new DelegateExtractor<TValue, TKey>(
                    keySelector ?? throw new ArgumentNullException(nameof(keySelector))));

        /// <summary>
        ///     Orders a span using the provided comparer for a key extracted from
        ///     each value by an extractor method.
        /// </summary>
        public static OrderingPlan<TValue, TKey, DescendingComparer<TKey, IComparer<TKey>>, DelegateExtractor<TValue, TKey>>
            OrderByDescending<TValue, TKey>(
                this ReadOnlySpan<TValue> input,
                Span<TValue> backing,
                Func<TValue, TKey> keySelector,
                IComparer<TKey> comparer)
        =>
            InternalOrderBy<TKey, TValue, DescendingComparer<TKey, IComparer<TKey>>, DelegateExtractor<TValue, TKey>>(
                input,
                backing,
                new DescendingComparer<TKey, IComparer<TKey>>(comparer),
                new DelegateExtractor<TValue, TKey>(
                    keySelector ?? throw new ArgumentNullException(nameof(keySelector))));

        /// <summary>
        ///     Orders a span using the default comparer for the contents
        ///     of the span.
        /// </summary>
        public static OrderingPlan<T, T, Comparer<T>, IdentityExtractor<T>> OrderByValue<T>(
            this Span<T> input,
            Span<T> backing)
        =>
            InternalOrderBy<T, T, Comparer<T>, IdentityExtractor<T>>(
                input,
                backing,
                Comparer<T>.Default,
                new IdentityExtractor<T>());

        /// <summary>
        ///     Orders a span using the provided comparer for the contents
        ///     of the span.
        /// </summary>
        public static OrderingPlan<T, T, IComparer<T>, IdentityExtractor<T>> OrderByValue<T>(
            this Span<T> input,
            Span<T> backing,
            IComparer<T> comparer)
        =>
            InternalOrderBy<T, T, IComparer<T>, IdentityExtractor<T>>(
                input,
                backing,
                comparer,
                new IdentityExtractor<T>());

        /// <summary>
        ///     Orders a span using the default comparer for a key extracted from
        ///     each value by an extractor method.
        /// </summary>
        public static OrderingPlan<TValue, TKey, Comparer<TKey>, DelegateExtractor<TValue, TKey>>
            OrderBy<TValue, TKey>(
                this Span<TValue> input,
                Span<TValue> backing,
                Func<TValue, TKey> keySelector)
        =>
            InternalOrderBy<TKey, TValue, Comparer<TKey>, DelegateExtractor<TValue, TKey>>(
                input,
                backing,
                Comparer<TKey>.Default,
                new DelegateExtractor<TValue, TKey>(
                    keySelector ?? throw new ArgumentNullException(nameof(keySelector))));

        /// <summary>
        ///     Orders a span using the provided comparer for a key extracted from
        ///     each value by an extractor method.
        /// </summary>
        public static OrderingPlan<TValue, TKey, IComparer<TKey>, DelegateExtractor<TValue, TKey>>
            OrderBy<TValue, TKey>(
                this Span<TValue> input,
                Span<TValue> backing,
                Func<TValue, TKey> keySelector,
                IComparer<TKey> comparer)
        =>
            InternalOrderBy<TKey, TValue, IComparer<TKey>, DelegateExtractor<TValue, TKey>>(
                input,
                backing,
                comparer,
                new DelegateExtractor<TValue, TKey>(
                    keySelector ?? throw new ArgumentNullException(nameof(keySelector))));

        /// <summary>
        ///     Orders a span using the default comparer for the contents
        ///     of the span.
        /// </summary>
        public static OrderingPlan<T, T, DescendingComparer<T, Comparer<T>>, IdentityExtractor<T>>
            OrderByValueDescending<T>(
                this Span<T> input,
                Span<T> backing)
        =>
            InternalOrderBy<T, T, DescendingComparer<T, Comparer<T>>, IdentityExtractor<T>>(
                input,
                backing,
                new DescendingComparer<T, Comparer<T>>(Comparer<T>.Default),
                new IdentityExtractor<T>());

        /// <summary>
        ///     Orders a span using the provided comparer for the contents
        ///     of the span.
        /// </summary>
        public static OrderingPlan<T, T, DescendingComparer<T, IComparer<T>>, IdentityExtractor<T>>
            OrderByValueDescending<T>(
                this Span<T> input,
                Span<T> backing,
                IComparer<T> comparer)
        =>
            InternalOrderBy<T, T, DescendingComparer<T, IComparer<T>>, IdentityExtractor<T>>(
                input,
                backing,
                new DescendingComparer<T, IComparer<T>>(comparer),
                new IdentityExtractor<T>());

        /// <summary>
        ///     Orders a span using the default comparer for a key extracted from
        ///     each value by an extractor method.
        /// </summary>
        public static OrderingPlan<TValue, TKey, DescendingComparer<TKey, Comparer<TKey>>, DelegateExtractor<TValue, TKey>>
            OrderByDescending<TValue, TKey>(
                this Span<TValue> input,
                Span<TValue> backing,
                Func<TValue, TKey> keySelector)
        =>
            InternalOrderBy<TKey, TValue, DescendingComparer<TKey, Comparer<TKey>>, DelegateExtractor<TValue, TKey>>(
                input,
                backing,
                new DescendingComparer<TKey, Comparer<TKey>>(Comparer<TKey>.Default),
                new DelegateExtractor<TValue, TKey>(
                    keySelector ?? throw new ArgumentNullException(nameof(keySelector))));

        /// <summary>
        ///     Orders a span using the provided comparer for a key extracted from
        ///     each value by an extractor method.
        /// </summary>
        public static OrderingPlan<TValue, TKey, DescendingComparer<TKey, IComparer<TKey>>, DelegateExtractor<TValue, TKey>>
            OrderByDescending<TValue, TKey>(
                this Span<TValue> input,
                Span<TValue> backing,
                Func<TValue, TKey> keySelector,
                IComparer<TKey> comparer)
        =>
            InternalOrderBy<TKey, TValue, DescendingComparer<TKey, IComparer<TKey>>, DelegateExtractor<TValue, TKey>>(
                input,
                backing,
                new DescendingComparer<TKey, IComparer<TKey>>(comparer),
                new DelegateExtractor<TValue, TKey>(
                    keySelector ?? throw new ArgumentNullException(nameof(keySelector))));

        #endregion

        #region SpanEnumerable<TIn, TOut, TProducer> ordering

        /// <summary>
        ///     Prepare to order the contents of a span by the specified 
        ///     extractor and comparer.
        /// </summary>
        internal static OrderingPlan<TValue, TKey, TComparer, TExtractor>
            InternalOrderBy<TKey, TIn, TProducer, TValue, TComparer, TExtractor>(
                this SpanEnumerable<TIn, TValue, TProducer> input,
                Span<TValue> backing,
                TComparer comparer,
                TExtractor extractor)
            where TExtractor : struct, IKeyExtractor<TValue, TKey>
            where TComparer : IComparer<TKey>
            where TProducer : IProducer<TIn, TValue>
        {
            input.CopyInto(backing);
            return new OrderingPlan<TValue, TKey, TComparer, TExtractor>(
                extractor, comparer, backing);
        }

        /// <summary>
        ///     Orders a span using the default comparer for the contents
        ///     of the span.
        /// </summary>
        public static OrderingPlan<T, T, Comparer<T>, IdentityExtractor<T>> OrderByValue<TIn, T, TProducer>(
                this SpanEnumerable<TIn, T, TProducer> input,
                Span<T> backing)
            where TProducer : IProducer<TIn, T>
        =>
            InternalOrderBy<T, TIn, TProducer, T, Comparer<T>, IdentityExtractor<T>>(
                input,
                backing,
                Comparer<T>.Default,
                new IdentityExtractor<T>());

        /// <summary>
        ///     Orders a span using the provided comparer for the contents
        ///     of the span.
        /// </summary>
        public static OrderingPlan<T, T, IComparer<T>, IdentityExtractor<T>> OrderByValue<TIn, T, TProducer>(
                this SpanEnumerable<TIn, T, TProducer> input,
                Span<T> backing,
                IComparer<T> comparer)
            where TProducer : IProducer<TIn, T>
        =>
            InternalOrderBy<T, TIn, TProducer, T, IComparer<T>, IdentityExtractor<T>>(
                input,
                backing,
                comparer,
                new IdentityExtractor<T>());

        /// <summary>
        ///     Orders a span using the default comparer for a key extracted from
        ///     each value by an extractor method.
        /// </summary>
        public static OrderingPlan<TValue, TKey, Comparer<TKey>, DelegateExtractor<TValue, TKey>>
            OrderBy<TValue, TKey, TIn, TProducer>(
                this SpanEnumerable<TIn, TValue, TProducer> input,
                Span<TValue> backing,
                Func<TValue, TKey> keySelector)
            where TProducer : IProducer<TIn, TValue>
        =>
            InternalOrderBy<TKey, TIn, TProducer, TValue, Comparer<TKey>, DelegateExtractor<TValue, TKey>>(
                input,
                backing,
                Comparer<TKey>.Default,
                new DelegateExtractor<TValue, TKey>(
                    keySelector ?? throw new ArgumentNullException(nameof(keySelector))));

        /// <summary>
        ///     Orders a span using the provided comparer for a key extracted from
        ///     each value by an extractor method.
        /// </summary>
        public static OrderingPlan<TValue, TKey, IComparer<TKey>, DelegateExtractor<TValue, TKey>>
            OrderBy<TValue, TKey, TIn, TProducer>(
                this SpanEnumerable<TIn, TValue, TProducer> input,
                Span<TValue> backing,
                Func<TValue, TKey> keySelector,
                IComparer<TKey> comparer)
            where TProducer : IProducer<TIn, TValue>
        =>
            InternalOrderBy<TKey, TIn, TProducer, TValue, IComparer<TKey>, DelegateExtractor<TValue, TKey>>(
                input,
                backing,
                comparer,
                new DelegateExtractor<TValue, TKey>(
                    keySelector ?? throw new ArgumentNullException(nameof(keySelector))));

        /// <summary>
        ///     Orders a span using the default comparer for the contents
        ///     of the span.
        /// </summary>
        public static OrderingPlan<T, T, DescendingComparer<T, Comparer<T>>, IdentityExtractor<T>>
            OrderByValueDescending<TIn, T, TProducer>(
                this SpanEnumerable<TIn, T, TProducer> input,
                Span<T> backing)
            where TProducer : IProducer<TIn, T>
        =>
            InternalOrderBy<T, TIn, TProducer, T, DescendingComparer<T, Comparer<T>>, IdentityExtractor<T>>(
                input,
                backing,
                new DescendingComparer<T, Comparer<T>>(Comparer<T>.Default),
                new IdentityExtractor<T>());

        /// <summary>
        ///     Orders a span using the provided comparer for the contents
        ///     of the span.
        /// </summary>
        public static OrderingPlan<T, T, DescendingComparer<T, IComparer<T>>, IdentityExtractor<T>>
            OrderByValueDescending<TIn, T, TProducer>(
                this SpanEnumerable<TIn, T, TProducer> input,
                Span<T> backing,
                IComparer<T> comparer)
            where TProducer : IProducer<TIn, T>
        =>
            InternalOrderBy<T, TIn, TProducer, T, DescendingComparer<T, IComparer<T>>, IdentityExtractor<T>>(
                input,
                backing,
                new DescendingComparer<T, IComparer<T>>(comparer),
                new IdentityExtractor<T>());

        /// <summary>
        ///     Orders a span using the default comparer for a key extracted from
        ///     each value by an extractor method.
        /// </summary>
        public static OrderingPlan<TValue, TKey, DescendingComparer<TKey, Comparer<TKey>>, DelegateExtractor<TValue, TKey>>
            OrderByDescending<TIn, TValue, TKey, TProducer>(
                this SpanEnumerable<TIn, TValue, TProducer> input,
                Span<TValue> backing,
                Func<TValue, TKey> keySelector)
            where TProducer : IProducer<TIn, TValue>
        =>
            InternalOrderBy<TKey, TIn, TProducer, TValue, DescendingComparer<TKey, Comparer<TKey>>, DelegateExtractor<TValue, TKey>>(
                input,
                backing,
                new DescendingComparer<TKey, Comparer<TKey>>(Comparer<TKey>.Default),
                new DelegateExtractor<TValue, TKey>(
                    keySelector ?? throw new ArgumentNullException(nameof(keySelector))));

        /// <summary>
        ///     Orders a span using the provided comparer for a key extracted from
        ///     each value by an extractor method.
        /// </summary>
        public static OrderingPlan<TValue, TKey, DescendingComparer<TKey, IComparer<TKey>>, DelegateExtractor<TValue, TKey>>
            OrderByDescending<TIn, TValue, TKey, TProducer>(
                this SpanEnumerable<TIn, TValue, TProducer> input,
                Span<TValue> backing,
                Func<TValue, TKey> keySelector,
                IComparer<TKey> comparer)
            where TProducer : IProducer<TIn, TValue>
        =>
            InternalOrderBy<TKey, TIn, TProducer, TValue, DescendingComparer<TKey, IComparer<TKey>>, DelegateExtractor<TValue, TKey>>(
                input,
                backing,
                new DescendingComparer<TKey, IComparer<TKey>>(comparer),
                new DelegateExtractor<TValue, TKey>(
                    keySelector ?? throw new ArgumentNullException(nameof(keySelector))));

        #endregion

        #region SpanEnumerable<T, TProducer> ordering

        /// <summary>
        ///     Prepare to order the contents of a span by the specified 
        ///     extractor and comparer.
        /// </summary>
        internal static OrderingPlan<TValue, TKey, TComparer, TExtractor>
            InternalOrderBy<TKey, TProducer, TValue, TComparer, TExtractor>(
                this SpanEnumerable<TValue, TProducer> input,
                Span<TValue> backing,
                TComparer comparer,
                TExtractor extractor)
            where TExtractor : struct, IKeyExtractor<TValue, TKey>
            where TComparer : IComparer<TKey>
            where TProducer : IProducer<TValue>
        {
            input.CopyInto(backing);
            return new OrderingPlan<TValue, TKey, TComparer, TExtractor>(
                extractor, comparer, backing);
        }

        /// <summary>
        ///     Orders a span using the default comparer for the contents
        ///     of the span.
        /// </summary>
        public static OrderingPlan<T, T, Comparer<T>, IdentityExtractor<T>> OrderByValue<T, TProducer>(
                this SpanEnumerable<T, TProducer> input,
                Span<T> backing)
            where TProducer : IProducer<T>
        =>
            InternalOrderBy<T, TProducer, T, Comparer<T>, IdentityExtractor<T>>(
                input,
                backing,
                Comparer<T>.Default,
                new IdentityExtractor<T>());

        /// <summary>
        ///     Orders a span using the provided comparer for the contents
        ///     of the span.
        /// </summary>
        public static OrderingPlan<T, T, IComparer<T>, IdentityExtractor<T>> OrderByValue<T, TProducer>(
                this SpanEnumerable<T, TProducer> input,
                Span<T> backing,
                IComparer<T> comparer)
            where TProducer : IProducer<T>
        =>
            InternalOrderBy<T, TProducer, T, IComparer<T>, IdentityExtractor<T>>(
                input,
                backing,
                comparer,
                new IdentityExtractor<T>());

        /// <summary>
        ///     Orders a span using the default comparer for a key extracted from
        ///     each value by an extractor method.
        /// </summary>
        public static OrderingPlan<TValue, TKey, Comparer<TKey>, DelegateExtractor<TValue, TKey>>
            OrderBy<TValue, TKey, TProducer>(
                this SpanEnumerable<TValue, TProducer> input,
                Span<TValue> backing,
                Func<TValue, TKey> keySelector)
            where TProducer : IProducer<TValue>
        =>
            InternalOrderBy<TKey, TProducer, TValue, Comparer<TKey>, DelegateExtractor<TValue, TKey>>(
                input,
                backing,
                Comparer<TKey>.Default,
                new DelegateExtractor<TValue, TKey>(
                    keySelector ?? throw new ArgumentNullException(nameof(keySelector))));

        /// <summary>
        ///     Orders a span using the provided comparer for a key extracted from
        ///     each value by an extractor method.
        /// </summary>
        public static OrderingPlan<TValue, TKey, IComparer<TKey>, DelegateExtractor<TValue, TKey>>
            OrderBy<TValue, TKey, TProducer>(
                this SpanEnumerable<TValue, TProducer> input,
                Span<TValue> backing,
                Func<TValue, TKey> keySelector,
                IComparer<TKey> comparer)
            where TProducer : IProducer<TValue>
        =>
            InternalOrderBy<TKey, TProducer, TValue, IComparer<TKey>, DelegateExtractor<TValue, TKey>>(
                input,
                backing,
                comparer,
                new DelegateExtractor<TValue, TKey>(
                    keySelector ?? throw new ArgumentNullException(nameof(keySelector))));

        /// <summary>
        ///     Orders a span using the default comparer for the contents
        ///     of the span.
        /// </summary>
        public static OrderingPlan<T, T, DescendingComparer<T, Comparer<T>>, IdentityExtractor<T>>
            OrderByValueDescending<T, TProducer>(
                this SpanEnumerable<T, TProducer> input,
                Span<T> backing)
            where TProducer : IProducer<T>
        =>
            InternalOrderBy<T, TProducer, T, DescendingComparer<T, Comparer<T>>, IdentityExtractor<T>>(
                input,
                backing,
                new DescendingComparer<T, Comparer<T>>(Comparer<T>.Default),
                new IdentityExtractor<T>());

        /// <summary>
        ///     Orders a span using the provided comparer for the contents
        ///     of the span.
        /// </summary>
        public static OrderingPlan<T, T, DescendingComparer<T, IComparer<T>>, IdentityExtractor<T>>
            OrderByValueDescending<T, TProducer>(
                this SpanEnumerable<T, TProducer> input,
                Span<T> backing,
                IComparer<T> comparer)
            where TProducer : IProducer<T>
        =>
            InternalOrderBy<T, TProducer, T, DescendingComparer<T, IComparer<T>>, IdentityExtractor<T>>(
                input,
                backing,
                new DescendingComparer<T, IComparer<T>>(comparer),
                new IdentityExtractor<T>());

        /// <summary>
        ///     Orders a span using the default comparer for a key extracted from
        ///     each value by an extractor method.
        /// </summary>
        public static OrderingPlan<TValue, TKey, DescendingComparer<TKey, Comparer<TKey>>, DelegateExtractor<TValue, TKey>>
            OrderByDescending<TValue, TKey, TProducer>(
                this SpanEnumerable<TValue, TProducer> input,
                Span<TValue> backing,
                Func<TValue, TKey> keySelector)
            where TProducer : IProducer<TValue>
        =>
            InternalOrderBy<TKey, TProducer, TValue, DescendingComparer<TKey, Comparer<TKey>>, DelegateExtractor<TValue, TKey>>(
                input,
                backing,
                new DescendingComparer<TKey, Comparer<TKey>>(Comparer<TKey>.Default),
                new DelegateExtractor<TValue, TKey>(
                    keySelector ?? throw new ArgumentNullException(nameof(keySelector))));

        /// <summary>
        ///     Orders a span using the provided comparer for a key extracted from
        ///     each value by an extractor method.
        /// </summary>
        public static OrderingPlan<TValue, TKey, DescendingComparer<TKey, IComparer<TKey>>, DelegateExtractor<TValue, TKey>>
            OrderByDescending<TValue, TKey, TProducer>(
                this SpanEnumerable<TValue, TProducer> input,
                Span<TValue> backing,
                Func<TValue, TKey> keySelector,
                IComparer<TKey> comparer)
            where TProducer : IProducer<TValue>
        =>
            InternalOrderBy<TKey, TProducer, TValue, DescendingComparer<TKey, IComparer<TKey>>, DelegateExtractor<TValue, TKey>>(
                input,
                backing,
                new DescendingComparer<TKey, IComparer<TKey>>(comparer),
                new DelegateExtractor<TValue, TKey>(
                    keySelector ?? throw new ArgumentNullException(nameof(keySelector))));

        #endregion

        #region SpanEnumerable<T> ordering

        /// <summary>
        ///     Prepare to order the contents of a span by the specified 
        ///     extractor and comparer.
        /// </summary>
        internal static OrderingPlan<TValue, TKey, TComparer, TExtractor>
            InternalOrderBy<TKey, TValue, TComparer, TExtractor>(
                this SpanEnumerable<TValue> input,
                Span<TValue> backing,
                TComparer comparer,
                TExtractor extractor)
            where TExtractor : struct, IKeyExtractor<TValue, TKey>
            where TComparer : IComparer<TKey>
        {
            input.CopyInto(backing);
            return new OrderingPlan<TValue, TKey, TComparer, TExtractor>(
                extractor, comparer, backing);
        }

        /// <summary>
        ///     Orders a span using the default comparer for the contents
        ///     of the span.
        /// </summary>
        public static OrderingPlan<T, T, Comparer<T>, IdentityExtractor<T>> OrderByValue<T>(
            this SpanEnumerable<T> input,
            Span<T> backing)
        =>
            InternalOrderBy<T, T, Comparer<T>, IdentityExtractor<T>>(
                input,
                backing,
                Comparer<T>.Default,
                new IdentityExtractor<T>());

        /// <summary>
        ///     Orders a span using the provided comparer for the contents
        ///     of the span.
        /// </summary>
        public static OrderingPlan<T, T, IComparer<T>, IdentityExtractor<T>> OrderByValue<T>(
            this SpanEnumerable<T> input,
            Span<T> backing,
            IComparer<T> comparer)
        =>
            InternalOrderBy<T, T, IComparer<T>, IdentityExtractor<T>>(
                input,
                backing,
                comparer,
                new IdentityExtractor<T>());

        /// <summary>
        ///     Orders a span using the default comparer for a key extracted from
        ///     each value by an extractor method.
        /// </summary>
        public static OrderingPlan<TValue, TKey, Comparer<TKey>, DelegateExtractor<TValue, TKey>>
            OrderBy<TValue, TKey>(
                this SpanEnumerable<TValue> input,
                Span<TValue> backing,
                Func<TValue, TKey> keySelector)
        =>
            InternalOrderBy<TKey, TValue, Comparer<TKey>, DelegateExtractor<TValue, TKey>>(
                input,
                backing,
                Comparer<TKey>.Default,
                new DelegateExtractor<TValue, TKey>(
                    keySelector ?? throw new ArgumentNullException(nameof(keySelector))));

        /// <summary>
        ///     Orders a span using the provided comparer for a key extracted from
        ///     each value by an extractor method.
        /// </summary>
        public static OrderingPlan<TValue, TKey, IComparer<TKey>, DelegateExtractor<TValue, TKey>>
            OrderBy<TValue, TKey>(
                this SpanEnumerable<TValue> input,
                Span<TValue> backing,
                Func<TValue, TKey> keySelector,
                IComparer<TKey> comparer)
        =>
            InternalOrderBy<TKey, TValue, IComparer<TKey>, DelegateExtractor<TValue, TKey>>(
                input,
                backing,
                comparer,
                new DelegateExtractor<TValue, TKey>(
                    keySelector ?? throw new ArgumentNullException(nameof(keySelector))));

        /// <summary>
        ///     Orders a span using the default comparer for the contents
        ///     of the span.
        /// </summary>
        public static OrderingPlan<T, T, DescendingComparer<T, Comparer<T>>, IdentityExtractor<T>>
            OrderByValueDescending<T>(
                this SpanEnumerable<T> input,
                Span<T> backing)
        =>
            InternalOrderBy<T, T, DescendingComparer<T, Comparer<T>>, IdentityExtractor<T>>(
                input,
                backing,
                new DescendingComparer<T, Comparer<T>>(Comparer<T>.Default),
                new IdentityExtractor<T>());

        /// <summary>
        ///     Orders a span using the provided comparer for the contents
        ///     of the span.
        /// </summary>
        public static OrderingPlan<T, T, DescendingComparer<T, IComparer<T>>, IdentityExtractor<T>>
            OrderByValueDescending<T>(
                this SpanEnumerable<T> input,
                Span<T> backing,
                IComparer<T> comparer)
        =>
            InternalOrderBy<T, T, DescendingComparer<T, IComparer<T>>, IdentityExtractor<T>>(
                input,
                backing,
                new DescendingComparer<T, IComparer<T>>(comparer),
                new IdentityExtractor<T>());

        /// <summary>
        ///     Orders a span using the default comparer for a key extracted from
        ///     each value by an extractor method.
        /// </summary>
        public static OrderingPlan<TValue, TKey, DescendingComparer<TKey, Comparer<TKey>>, DelegateExtractor<TValue, TKey>>
            OrderByDescending<TValue, TKey>(
                this SpanEnumerable<TValue> input,
                Span<TValue> backing,
                Func<TValue, TKey> keySelector)
        =>
            InternalOrderBy<TKey, TValue, DescendingComparer<TKey, Comparer<TKey>>, DelegateExtractor<TValue, TKey>>(
                input,
                backing,
                new DescendingComparer<TKey, Comparer<TKey>>(Comparer<TKey>.Default),
                new DelegateExtractor<TValue, TKey>(
                    keySelector ?? throw new ArgumentNullException(nameof(keySelector))));

        /// <summary>
        ///     Orders a span using the provided comparer for a key extracted from
        ///     each value by an extractor method.
        /// </summary>
        public static OrderingPlan<TValue, TKey, DescendingComparer<TKey, IComparer<TKey>>, DelegateExtractor<TValue, TKey>>
            OrderByDescending<TValue, TKey>(
                this SpanEnumerable<TValue> input,
                Span<TValue> backing,
                Func<TValue, TKey> keySelector,
                IComparer<TKey> comparer)
        =>
            InternalOrderBy<TKey, TValue, DescendingComparer<TKey, IComparer<TKey>>, DelegateExtractor<TValue, TKey>>(
                input,
                backing,
                new DescendingComparer<TKey, IComparer<TKey>>(comparer),
                new DelegateExtractor<TValue, TKey>(
                    keySelector ?? throw new ArgumentNullException(nameof(keySelector))));

        #endregion
    }
}
