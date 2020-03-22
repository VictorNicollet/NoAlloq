using System;

namespace NoAlloq.Producers
{
    /// <summary> Copies data from a span to the next. </summary>
    public struct IdentityProducer<T> : IProducer<T, T>
    {
        public void Produce(ref ReadOnlySpan<T> input, ref Span<T> output)
        {
            var max = Math.Min(input.Length, output.Length);

            input.Slice(0, max).CopyTo(output);

            input = input.Slice(max);
            output = output.Slice(0, max);
        }
    }
}
