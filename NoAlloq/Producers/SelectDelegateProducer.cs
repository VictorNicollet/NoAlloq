using System;

namespace NoAlloq.Producers
{
    /// <summary>
    ///     Produces an output value for each input value, by 
    ///     applying a delegate to that value.
    /// </summary>
    /// <remarks>
    ///     Slightly slower than <see cref="SelectProducer{TIn, TOut, TMap}"/>
    /// </remarks>
    public struct SelectDelegateProducer<TIn, TOut> : IProducer<TIn, TOut>
    {
        private readonly Func<TIn, TOut> _delegate;

        public SelectDelegateProducer(Func<TIn, TOut> d)
        {
            _delegate = d;
        }

        public void Produce(ref ReadOnlySpan<TIn> input, ref Span<TOut> output)
        {
            var max = Math.Min(input.Length, output.Length);
            for (var i = 0; i < max; ++i)
                output[i] = _delegate(input[i]);

            input = input.Slice(max);
            output = output.Slice(0, max);
        }
    }
}
