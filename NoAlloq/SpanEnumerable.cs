using NoAlloq.Producers;
using System;

namespace NoAlloq
{
    public partial struct SpanEnumerable<TOut, TProducer>
        where TProducer : IProducer<TOut>
    {
        /// <summary> The producer used to produce values. </summary>
        /// <remarks>
        ///     Not read-only because it is mutated by <see cref="ConsumeInto"/>.
        /// </remarks>
        internal TProducer Producer;

        /// <summary> The length of this sequence, -1 if unknown. </summary>
        /// <remarks>
        ///     Not read-only because it is mutated by <see cref="ConsumeInto"/>.
        /// </remarks>
        private int _length;

        internal SpanEnumerable(TProducer producer, int length)
        {
            Producer = producer;
            _length = length;
        }

        /// <summary> Whether the length of this sequence is known. </summary>
        public bool KnownLength => _length >= 0;

        /// <summary> The length of this sequence, if known. </summary>
        public int Length =>
            KnownLength ? _length :
            throw new InvalidOperationException("Sequence does not have a known length");

        /// <summary>
        ///     Takes enough values from the sequence to fill the provided span, 
        ///     consuming them from the enumerable.
        /// </summary>
        /// <remarks>
        ///     Unlike <c>TakeInto</c>, repeatedly calling this method on the 
        ///     enumerable will yield different results.
        /// </remarks>
        internal Span<TOut> ConsumeInto(Span<TOut> output)
        {
            Producer.Produce(ref output);

            if (_length >= 0)
                _length -= output.Length;

            return output;
        }

    }

    public ref partial struct SpanEnumerable<TIn, TOut, TProducer>
        where TProducer : IProducer<TIn, TOut>
    {
        /// <summary> The input data for this sequence. </summary>
        /// <remarks>
        ///     Not read-only because it is mutated by <see cref="ConsumeInto"/>.
        /// </remarks>
        internal ReadOnlySpan<TIn> Input;

        /// <summary>
        ///     The producer used to transform values from the input span into
        ///     output values.
        /// </summary>
        /// <remarks>
        ///     Not read-only because it is mutated by <see cref="ConsumeInto"/>.
        /// </remarks>
        internal TProducer Producer;

        /// <summary> The length of this sequence, -1 if unknown. </summary>
        /// <remarks>
        ///     Not read-only because it is mutated by <see cref="ConsumeInto"/>.
        /// </remarks>
        private int _length;

        /// <summary> Whether the length of this sequence is known. </summary>
        public bool KnownLength => _length >= 0;

        /// <summary> The length of this sequence, if known. </summary>
        public int Length =>
            KnownLength ? _length :
            throw new InvalidOperationException("Sequence does not have a known length");

        internal SpanEnumerable(ReadOnlySpan<TIn> input, TProducer producer, int length)
        {
            Input = input;
            Producer = producer;
            _length = length;
        }

        /// <summary>
        ///     Takes enough values from the sequence to fill the provided span, 
        ///     consuming them from the enumerable.
        /// </summary>
        /// <remarks>
        ///     Unlike <c>TakeInto</c>, repeatedly calling this method on the 
        ///     enumerable will yield different results.
        /// </remarks>
        public Span<TOut> ConsumeInto(Span<TOut> output)
        {
            Producer.Produce(ref Input, ref output);
            
            if (_length >= 0) 
                _length -= output.Length;
            
            return output;
        }
    }

    public static partial class SpanEnumerable
    {
        /// <summary> Create a kernel from a read-only span.  </summary>
        internal static SpanEnumerable<T, T, IdentityProducer<T>> Of<T>(ReadOnlySpan<T> span) =>
            new SpanEnumerable<T, T, IdentityProducer<T>>(
                span, new IdentityProducer<T>(), span.Length);

        /// <summary> Create a kernel from a span.  </summary>
        internal static SpanEnumerable<T, T, IdentityProducer<T>> Of<T>(Span<T> span) =>
            new SpanEnumerable<T, T, IdentityProducer<T>>(
                span, new IdentityProducer<T>(), span.Length);

        /// <summary>
        ///     Generates a sequence of integral numbers within a specified range.
        /// </summary>
        /// <param name="start"> The first integer in the sequence. </param>
        /// <param name="count"> The number of integers in the range. </param>
        public static SpanEnumerable<int, RangeProducer> Range(int start, int count)
        {
            if (count < 0 || int.MaxValue - count < start)
                throw new ArgumentOutOfRangeException(
                    nameof(count), 
                    count, 
                    "Range count is out of bounds.");

            return new SpanEnumerable<int, RangeProducer>(
                new RangeProducer(start, start + count - 1), 
                start + count - 1);
        }
    }
}
