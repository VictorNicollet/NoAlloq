using System;

namespace NoAlloq
{
    /// <summary> Maps an argument to a result. </summary>
    public interface IMap<in TIn, out TOut>
    {
        TOut Apply(TIn value);
    }
}
