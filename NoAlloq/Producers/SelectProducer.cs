using System;

namespace NoAlloq.Producers
{
    /// <summary>
    ///     Produces an output value for each input value, by applying
    ///     an <see cref="IMap{TIn, TOut}"/> to that value.
    /// </summary>
    public struct SelectProducer<TIn, TOut, TMap> : IProducer<TIn, TOut>
        where TMap : struct, IMap<TIn, TOut>
    {
        private readonly TMap _map;

        public SelectProducer(TMap map)
        {
            _map = map;
        }

        public void Produce(ref ReadOnlySpan<TIn> input, ref Span<TOut> output)
        {
            var max = Math.Min(input.Length, output.Length);
            for (var i = 0; i < max; ++i)
                output[i] = _map.Apply(input[i]);

            input = input.Slice(max);
            output = output.Slice(0, max);
        }
    }
}
