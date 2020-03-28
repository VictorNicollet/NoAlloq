using System;

namespace NoAlloq.Ordering
{
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

}
