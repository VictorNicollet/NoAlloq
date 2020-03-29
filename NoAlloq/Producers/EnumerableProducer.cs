using System;
using System.Collections.Generic;

namespace NoAlloq.Producers
{
    /// <summary> Produces values from an enumerator. </summary>
    public struct EnumerableProducer<T> : IProducer<T>
    {
        /// <summary> The number of values left to produce. </summary>
        private readonly IEnumerator<T> _source;

        public EnumerableProducer(IEnumerator<T> source) =>
            _source = source;
        
        /// <see cref="IProducer{TOut}.Produce"/>
        public void Produce(ref Span<T> output)
        {
            var count = 0;
            while (count < output.Length && _source.MoveNext())
                output[count++] = _source.Current;

            output = output.Slice(0, count);
        }
    }
}
