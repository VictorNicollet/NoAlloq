using System;
using System.Collections.Generic;

namespace NoAlloq.Ordering
{
    /// <summary> Extracts comparable keys from values. </summary>
    /// <remarks> 
    ///     Used for constructing comparisons in an order-by.
    /// </remarks>
    public interface IKeyExtractor<TValue, TKey>
    {
        /// <summary> The key to use when comparing a value. </summary>
        TKey Extract(TValue v);
    }

    /// <summary> Extracts the value itself to act as a comparison key. </summary>
    public struct IdentityExtractor<TValue> : IKeyExtractor<TValue, TValue>
    {
        /// <see cref="IKeyExtractor{TValue, TKey}.Extract(TValue)"/>
        public TValue Extract(TValue v) => v;
    }

    public struct DelegateExtractor<TValue, TKey> : IKeyExtractor<TValue, TKey>
    {
        private Func<TValue, TKey> _delegate;

        public DelegateExtractor(Func<TValue, TKey> d)
        {
            _delegate = d ?? throw new ArgumentNullException(nameof(d));
        }

        /// <see cref="IKeyExtractor{TValue, TKey}.Extract(TValue)"/>
        public TKey Extract(TValue v) => _delegate(v);
    }

    public struct DescendingComparer<TKey, TComparer> : IComparer<TKey>
        where TComparer : IComparer<TKey>
    {
        /// <summary> The ascending comparer. </summary>
        private readonly TComparer _inner;

        public DescendingComparer(TComparer inner)
        {
            _inner = inner;
        }

        /// <see cref="IComparer{T}.Compare(T, T)"/>
        public int Compare(TKey x, TKey y) =>
            _inner.Compare(y, x);
    }

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
    }

}
