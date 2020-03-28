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

}
