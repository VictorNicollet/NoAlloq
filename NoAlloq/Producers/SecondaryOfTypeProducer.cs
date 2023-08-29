using System;
using System.Runtime.InteropServices;

namespace NoAlloq.Producers
{
    /// <summary>
    ///     Produces an output value for each input value, by passing
    ///     the value through another producer, then type check result.
    /// </summary>
    public readonly struct SecondaryOfTypeProducer<TIn, TMid, TOut, TProducer> : IProducer<TIn, TOut>
        where TProducer : IProducer<TIn, TMid>
    {
        private readonly TProducer _inner;

        public SecondaryOfTypeProducer(
            TProducer inner)
        {
            _inner = inner;
        }

        public void Produce(ref ReadOnlySpan<TIn> input, ref Span<TOut> output)
        {
            // Create a temporary on-stack location for the middle values.
            TMid midOnStack = default;
            var mid = MemoryMarshal.CreateSpan(ref midOnStack, 1);

            var copied = 0;
            while (copied < output.Length)
            {
                _inner.Produce(ref input, ref mid);
                if (mid.Length == 0) break;

                if (midOnStack is TOut @out)
                {
                    output[copied++] = @out;
                }
            }

            output = output.Slice(0, copied);
        }
    }
}