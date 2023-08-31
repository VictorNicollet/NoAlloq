using System;

namespace NoAlloq.Producers
{
    public readonly struct OfTypeProducer<TIn, TOut> : IProducer<TIn, TOut>
    {
        public void Produce(ref ReadOnlySpan<TIn> input, ref Span<TOut> output)
        {
            int i = 0, o = 0;
            for (; i < input.Length && o < output.Length; ++i)
                if (input[i] is TOut @out)
                    output[o++] = @out;

            input = input.Slice(i);
            output = output.Slice(0, o);
        }
    }
}