using System;
using System.Collections.Generic;

namespace NoAlloq.Ordering
{
    /// <summary> An ordering plan on top of a span of values. </summary>
    public ref struct OrderingPlan<TValue, TKey, TComparer, TExtractor>
        where TExtractor : struct, IKeyExtractor<TValue, TKey>
        where TComparer : IComparer<TKey>
    {
        /// <summary> The key extractor for comparing values. </summary>
        private readonly TExtractor _extractor;

        /// <summary> The comparer for extracted keys. </summary>
        private readonly TComparer _comparer;

        /// <summary> The span of values being ordered. </summary>
        internal readonly Span<TValue> ToBeSorted;

        public OrderingPlan(
            TExtractor extractor, 
            TComparer comparer, 
            Span<TValue> toBeSorted)
        {
            _extractor = extractor;
            _comparer = comparer;
            ToBeSorted = toBeSorted;
        }

        /// <summary> The number of elements to be ordered. </summary>
        public int Length => ToBeSorted.Length;

        #region ThenBy & ThenByDescending

        /// <summary>
        ///     Performs a subsequent ordering of elements in ascending order.
        /// </summary>
        public OrderingPlan<
                TValue,
                (TKey, TKey2),
                PairComparer<TKey, TKey2, TComparer, TComparer2>,
                ThenDelegateExtractor<TValue, TKey, TExtractor, TKey2>>
            ThenBy<TKey2, TComparer2>(
                Func<TValue, TKey2> keySelector,
                TComparer2 comparer)
            where TComparer2 : IComparer<TKey2>
        =>
            new OrderingPlan<
                TValue,
                (TKey, TKey2),
                PairComparer<TKey, TKey2, TComparer, TComparer2>,
                ThenDelegateExtractor<TValue, TKey, TExtractor, TKey2>>(
                    new ThenDelegateExtractor<TValue, TKey, TExtractor, TKey2>(
                        _extractor, keySelector),
                    new PairComparer<TKey, TKey2, TComparer, TComparer2>(
                        _comparer, comparer),
                    ToBeSorted);

        /// <summary>
        ///     Performs a subsequent ordering of elements in ascending order.
        /// </summary>
        public OrderingPlan<
                TValue,
                (TKey, TKey2),
                PairComparer<TKey, TKey2, TComparer, Comparer<TKey2>>,
                ThenDelegateExtractor<TValue, TKey, TExtractor, TKey2>>
            ThenBy<TKey2>(
                Func<TValue, TKey2> keySelector)
        =>
            ThenBy(keySelector, Comparer<TKey2>.Default);

        /// <summary>
        ///     Performs a subsequent ordering of elements in ascending order.
        /// </summary>
        public OrderingPlan<
                TValue,
                (TKey, TKey2),
                PairComparer<TKey, TKey2, TComparer, 
                    DescendingComparer<TKey2, TComparer2>>,
                ThenDelegateExtractor<TValue, TKey, TExtractor, TKey2>>
            ThenByDescending<TKey2, TComparer2>(
                Func<TValue, TKey2> keySelector,
                TComparer2 comparer)
            where TComparer2 : IComparer<TKey2>
        =>
            new OrderingPlan<
                TValue,
                (TKey, TKey2),
                PairComparer<TKey, TKey2, TComparer, DescendingComparer<TKey2, TComparer2>>,
                ThenDelegateExtractor<TValue, TKey, TExtractor, TKey2>>(
                    new ThenDelegateExtractor<TValue, TKey, TExtractor, TKey2>(
                        _extractor, keySelector),
                    new PairComparer<TKey, TKey2, TComparer, DescendingComparer<TKey2, TComparer2>>(
                        _comparer, new DescendingComparer<TKey2, TComparer2>(comparer)),
                    ToBeSorted);

        /// <summary>
        ///     Performs a subsequent ordering of elements in ascending order.
        /// </summary>
        public OrderingPlan<
                TValue,
                (TKey, TKey2),
                PairComparer<TKey, TKey2, TComparer, DescendingComparer<TKey2, Comparer<TKey2>>>,
                ThenDelegateExtractor<TValue, TKey, TExtractor, TKey2>>
            ThenByDescending<TKey2>(
                Func<TValue, TKey2> keySelector)
        =>
            ThenByDescending(keySelector, Comparer<TKey2>.Default);

        #endregion

        /// <summary>
        ///     Construct a <see cref="SpanEnumerable{TIn, TOut, TProducer}"/> from 
        ///     this ordering plan.
        /// </summary>
        /// <remarks>
        ///     This is a necessary step before using other NoAlloq methods (Select, 
        ///     Where, Take, etc).
        /// </remarks>
        public SpanEnumerable<TValue, TValue, OrderingProducer<TValue, TKey, TComparer, TExtractor>>
            ToEnumerable()
        {
            SpanHeap.Make<TValue, TExtractor, TKey, TComparer>(
                ToBeSorted, _extractor, _comparer);

            return new SpanEnumerable<TValue, TValue, OrderingProducer<TValue, TKey, TComparer, TExtractor>>(
                ToBeSorted,
                new OrderingProducer<TValue, TKey, TComparer, TExtractor>(
                    _extractor, _comparer),
                length: ToBeSorted.Length);
        }

        /// <summary>
        ///     Returns an enumerator duck-typing-compatible with foreach.
        /// </summary>
        public SpanEnumerable<TValue, TValue, OrderingProducer<TValue, TKey, TComparer, TExtractor>>.Enumerator
            GetEnumerator() => ToEnumerable().GetEnumerator();
    }
}
