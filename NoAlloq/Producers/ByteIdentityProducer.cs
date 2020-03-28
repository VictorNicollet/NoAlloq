using System;
using System.Runtime.InteropServices;

namespace NoAlloq.Producers
{
    /// <summary> Copies data from a byte span to another span. </summary>
    public struct ByteIdentityProducer<T> : IProducer<byte, T>
        where T : struct
    {
        public void Produce(ref ReadOnlySpan<byte> input, ref Span<T> output)
        {
            var cast = MemoryMarshal.Cast<byte, T>(input);
            var max = Math.Min(cast.Length, output.Length);

            cast.Slice(0, max).CopyTo(output);

            input = MemoryMarshal.Cast<T, byte>(cast.Slice(max));
            output = output.Slice(0, max);
        }
    }
}
