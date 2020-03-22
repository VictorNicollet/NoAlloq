using System;

namespace NoAlloq.Producers
{
    /// <summary>
    ///     Copy values from the input to the output, when the predicate
    ///     <see cref="IMap{TIn, TOut}"/> returns true for them.
    /// </summary>
    public struct WhereProducer<T, TMap> : IProducer<T, T>
        where TMap : struct, IMap<T, bool>
    {
        private readonly TMap _predicate;

        public WhereProducer(TMap predicate)
        {
            _predicate = predicate;
        }

        public void Produce(ref ReadOnlySpan<T> input, ref Span<T> output)
        {
            int i = 0, o = 0;
            for (; i < input.Length && o < output.Length; ++i)
                if (_predicate.Apply(input[i]))
                    output[o++] = input[i];

            input = input.Slice(i);
            output = output.Slice(0, o);
        }
    }
}
