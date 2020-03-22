using NoAlloq.Producers;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NoAlloq.Ordering
{
    /// <summary>
    ///     Produces values in the order specified by the key-extractor and 
    ///     key-comparer. The input span is mutable and contains values stored
    ///     as a min-heap with its root at `input.Length - 1`, which means 
    ///     extracting values frees input cells by ascending index.
    /// </summary>
    public struct OrderingProducer<TValue, TKey, TComparer, TExtractor> : IProducer<TValue, TValue>
        where TExtractor : struct, IKeyExtractor<TValue, TKey>
        where TComparer : IComparer<TKey>
    {
        /// <summary> Used to extract the comparable keys from the values. </summary>
        private readonly TExtractor _extractor;

        /// <summary> Used to compare the keys. </summary>
        private readonly TComparer _comparer;

        public OrderingProducer(TExtractor extractor, TComparer comparer)
        {
            _extractor = extractor;
            _comparer = comparer;
        }

        /// <see cref="IProducer{TIn, TOut}.Produce"/>
        public void Produce(ref ReadOnlySpan<TValue> input, ref Span<TValue> output)
        {
            // We cheat: we know that our input is not really read-only,
            // because we provided a writable span in OrderingPlan.ToEnumerable()
            // which is being passed back to us here.
            //
            // We _could_ get rid of this cast by distinguising SpanEnumerable
            // with a Span<TInput> or a ReadOnlySpan<TInput>, but this is probably 
            // too much effort.

            var writableInput =
                MemoryMarshal.CreateSpan(
                    ref MemoryMarshal.GetReference(input),
                    input.Length);

            output = RealProduce(writableInput, output);
            input = input.Slice(output.Length);
        }

        private Span<TValue> RealProduce(Span<TValue> input, Span<TValue> output)
        {
            var count = Math.Min(input.Length, output.Length);

            for (var i = 0; i < count; ++i)
                output[i] = SpanHeap.ExtractMin<TValue, TExtractor, TKey, TComparer>(
                    ref input, _extractor, _comparer);

            return output.Slice(0, count);
        }
    }
}
