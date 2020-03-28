using NoAlloq.Producers;
using System;
using System.Runtime.InteropServices;

namespace NoAlloq
{
    /// <summary>
    ///     Boxing consists in hiding the implementation of a span-enumerable 
    ///     inside a <see cref="SpanEnumerable{T}"/>, using boxing to hide 
    ///     the implementation of <see cref="IProducer{TOut}"/> and 
    ///     <see cref="IProducer{TIn, TOut}"/> behind the interface, and using
    ///     <see cref="MemoryMarshal.Cast"/> to hide the type of the input span
    ///     (if any) as a span-of-bytes. 
    /// </summary>
    public static class BoxExtensions
    {
        /// <summary> Box a span as a <see cref="SpanEnumerable{T}"/> </summary>
        public static SpanEnumerable<T> Box<T>(this ReadOnlySpan<T> span)
            where T : struct
        =>
            new SpanEnumerable<T>(
                MemoryMarshal.Cast<T, byte>(span),
                new ByteIdentityProducer<T>(),
                span.Length);

        /// <summary> Box a span as a <see cref="SpanEnumerable{T}"/> </summary>
        public static SpanEnumerable<T> Box<T>(this Span<T> span) where T : struct =>
            Box((ReadOnlySpan<T>)span);

        /// <summary>
        ///     Boxing to hide the <typeparamref name="TProducer"/> type.
        /// </summary>
        public static SpanEnumerable<TOut> Box<TOut, TProducer>(
                this SpanEnumerable<TOut, TProducer> producerEnumerable)
            where TProducer : IProducer<TOut>
        =>
            new SpanEnumerable<TOut>(
                Array.Empty<byte>(),
                new NoInputProducer<TOut>(producerEnumerable.Producer),
                producerEnumerable.LengthIfKnown);

        /// <summary>
        ///     Boxing to hide the <typeparamref name="TProducer"/> type.
        /// </summary>
        public static SpanEnumerable<TOut> Box<TIn, TOut, TProducer>(
                this SpanEnumerable<TIn, TOut, TProducer> producerEnumerable)
            where TIn : unmanaged
            where TProducer : IProducer<TIn, TOut>
        =>
            new SpanEnumerable<TOut>(
                MemoryMarshal.Cast<TIn, byte>(producerEnumerable.Input),
                new FromBytesProducer<TIn, TOut>(producerEnumerable.Producer),
                producerEnumerable.LengthIfKnown);
    }
}
