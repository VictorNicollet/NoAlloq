using System;

namespace NoAlloq.Producers
{
    /// <summary>
    ///     Copy values from the input to the output, when the predicate
    ///     returns true for them.
    /// </summary>
    /// <remarks>
    ///     Slightly slower than <see cref="WhereProducer{T, TMap}"/>
    /// </remarks>
    public struct WhereDelegateProducer<T> : IProducer<T, T>
    {
        private readonly Predicate<T> _predicate;

        public WhereDelegateProducer(Predicate<T> predicate)
        {
            _predicate = predicate;
        }

        public void Produce(ref ReadOnlySpan<T> input, ref Span<T> output)
        {
            int i = 0, o = 0;
            for (; i < input.Length && o < output.Length; ++i)
                if (_predicate(input[i]))
                    output[o++] = input[i];

            input = input.Slice(i);
            output = output.Slice(0, o);
        }
    }
}
