using System;

namespace NoAlloq.Ordering
{
    /// <summary>
    ///     Extracts a pair of keys from a value, using a delegate for the 
    ///     second key.
    /// </summary>
    public struct ThenDelegateExtractor<TValue, TKey1, TExtractor1, TKey2>
            : IKeyExtractor<TValue, (TKey1, TKey2)>
        where TExtractor1 : IKeyExtractor<TValue, TKey1>
    {
        /// <summary> Extractor for the original key. </summary>
        private readonly TExtractor1 _extractor1;

        /// <summary> Extractor for the second part of the key. </summary>
        private readonly Func<TValue, TKey2> _extractor2;

        public ThenDelegateExtractor(
            TExtractor1 extractor1, 
            Func<TValue, TKey2> extractor2)
        {
            _extractor1 = extractor1;
            _extractor2 = extractor2 ?? throw new ArgumentNullException(nameof(extractor2));
        }

        public (TKey1, TKey2) Extract(TValue v) =>
            (_extractor1.Extract(v), _extractor2(v));
    }
}
