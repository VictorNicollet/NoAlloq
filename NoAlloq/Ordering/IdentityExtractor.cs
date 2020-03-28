namespace NoAlloq.Ordering
{
    /// <summary> Extracts the value itself to act as a comparison key. </summary>
    public struct IdentityExtractor<TValue> : IKeyExtractor<TValue, TValue>
    {
        /// <see cref="IKeyExtractor{TValue, TKey}.Extract(TValue)"/>
        public TValue Extract(TValue v) => v;
    }

}
