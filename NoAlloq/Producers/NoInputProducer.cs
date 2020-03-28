using System;

namespace NoAlloq.Producers
{
    /// <summary>
    ///     Wraps an <see cref="IProducer{TOut}"/> as a 
    ///     <see cref="IProducer{TIn, TOut}"/>
    /// </summary>
    /// <remarks>
    ///     Used to wrap a <see cref="SpanEnumerable{TOut, TProducer}"/> 
    ///     as a <see cref="SpanEnumerable{TOut}"/>
    /// </remarks>
    public struct NoInputProducer<TOut> : IProducer<byte, TOut>
    {
        /// <summary> Inner producer. </summary>
        /// <remarks>
        ///     Not readonly because it can be mutated by <see cref="Produce"/>
        /// </remarks>
        private IProducer<TOut> _inner;

        public NoInputProducer(IProducer<TOut> inner) =>
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));

        public void Produce(ref ReadOnlySpan<byte> input, ref Span<TOut> output) =>
            _inner.Produce(ref output);
    }
}
