using System;
using System.Runtime.InteropServices;

namespace NoAlloq.Producers
{
    /// <summary>
    ///     Wraps a producer, hiding its input type by casting from 
    ///     a `Span{byte}` to a `Span{TIn}` on the fly.    
    /// </summary>
    /// <remarks>
    ///     Used to wrap a <see cref="SpanEnumerable{TIn, TOut, TProducer}"/>
    ///     as a <see cref="SpanEnumerable{TOut}"/>.
    /// </remarks>
    internal struct FromBytesProducer<TIn, TOut> : IProducer<byte, TOut> 
        where TIn : unmanaged
    {
        /// <summary> Inner producer. </summary>
        /// <remarks>
        ///     Not readonly because it can be mutated by <see cref="Produce"/>
        /// </remarks>
        private IProducer<TIn, TOut> _inner;

        public FromBytesProducer(IProducer<TIn, TOut> inner) =>
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));

        public void Produce(ref ReadOnlySpan<byte> input, ref Span<TOut> output)
        {
            var cast = MemoryMarshal.Cast<byte, TIn>(input);
            _inner.Produce(ref cast, ref output);
            input = MemoryMarshal.Cast<TIn, byte>(cast);
        }
    }
}
