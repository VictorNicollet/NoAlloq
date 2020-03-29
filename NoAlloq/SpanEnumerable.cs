using NoAlloq.Producers;
using System;
using System.Collections.Generic;

namespace NoAlloq
{
    /// <summary> 
    ///     An enumerable that uses boxing (and therefore, memory allocations)
    ///     to hide away all types except its output type.
    /// </summary>
    /// <see cref="SpanEnumerable{TOut, TProducer}"/>
    /// <see cref="SpanEnumerable{TIn, TOut, TProducer}"/>
    /// <see cref="BoxExtensions"/>
    public ref partial struct SpanEnumerable<TOut>
    {
        /// <summary> The input bytes. </summary>
        /// <remarks>
        ///     This is a single input span, cast to bytes to hide 
        ///     its type. If the original enumerable did not have an input,
        ///     this will be an empty span.
        /// </remarks>
        /// <remarks>
        ///     Not read-only because it is mutated by <see cref="ConsumeInto"/>.
        /// </remarks>
        private ReadOnlySpan<byte> _input;

        /// <summary> A *boxed* producer. </summary>
        private readonly IProducer<byte, TOut> _producer;

        /// <summary> The length of this sequence, -1 if unknown. </summary>
        /// <remarks>
        ///     Not read-only because it is mutated by <see cref="ConsumeInto"/>.
        /// </remarks>
        private int LengthIfKnown;

        public SpanEnumerable(
            ReadOnlySpan<byte> input, 
            IProducer<byte, TOut> producer, 
            int length)
        {
            _input = input;
            _producer = producer ?? throw new ArgumentNullException(nameof(producer));
            LengthIfKnown = length;
        }

        /// <summary> Whether the length of this sequence is known. </summary>
        public bool KnownLength => LengthIfKnown >= 0;

        /// <summary> The length of this sequence, if known. </summary>
        public int Length =>
            KnownLength ? LengthIfKnown :
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
            _producer.Produce(ref _input, ref output);

            if (LengthIfKnown >= 0)
                LengthIfKnown -= output.Length;

            return output;
        }

        public SpanEnumerable<TOut> Box() => 
            // Just smile and wave, boys. Smile and wave...
            this;
    }

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
        internal int LengthIfKnown;

        internal SpanEnumerable(TProducer producer, int length)
        {
            Producer = producer;
            LengthIfKnown = length;
        }

        /// <summary> Whether the length of this sequence is known. </summary>
        public bool KnownLength => LengthIfKnown >= 0;

        /// <summary> The length of this sequence, if known. </summary>
        public int Length =>
            KnownLength ? LengthIfKnown :
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

            if (LengthIfKnown >= 0)
                LengthIfKnown -= output.Length;

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
        internal int LengthIfKnown;

        /// <summary> Whether the length of this sequence is known. </summary>
        public bool KnownLength => LengthIfKnown >= 0;

        /// <summary> The length of this sequence, if known. </summary>
        public int Length =>
            KnownLength ? LengthIfKnown :
            throw new InvalidOperationException("Sequence does not have a known length");

        internal SpanEnumerable(ReadOnlySpan<TIn> input, TProducer producer, int length)
        {
            Input = input;
            Producer = producer;
            LengthIfKnown = length;
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
            
            if (LengthIfKnown >= 0) 
                LengthIfKnown -= output.Length;
            
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

        /// <summary>
        ///     Generates a sequence that contains one repeated value.
        /// </summary>
        public static SpanEnumerable<T, RepeatProducer<T>> Repeat<T>(T value, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(count),
                    count,
                    "Repeat count is out of bounds.");

            return new SpanEnumerable<T, RepeatProducer<T>>(
                new RepeatProducer<T>(count, value),
                length: count);
        }

        /// <summary> Converts a normal enumerable to a span-enumerable. </summary>
        public static SpanEnumerable<T, EnumerableProducer<T>> ToSpanEnumerable<T>(
            this IEnumerable<T> source)
        =>
            new SpanEnumerable<T, EnumerableProducer<T>>(
                new EnumerableProducer<T>(source.GetEnumerator()),
                length: 
                    source is ICollection<T> collection 
                    ? collection.Count
                    : -1);
    }
}
