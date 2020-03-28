using NoAlloq.Ordering;
using NoAlloq.Producers;
using System;
using System.Collections.Generic;

namespace NoAlloq
{
    public static class ReverseExtensions
    {
        /// <summary> Copy a sequence into a span, in reverse. </summary>
        public static Span<T> ReverseInto<T>(
            this ReadOnlySpan<T> self, 
            Span<T> output)
        {
            if (output.Length < self.Length)
                throw new ArgumentOutOfRangeException(
                    nameof(output),
                    output.Length,
                    $"Need an output span of length {self.Length}.");

            output = output.Slice(0, self.Length);

            for (var i = 0; i < output.Length; ++i)
                output[i] = self[self.Length - 1 - i];

            return output;
        }

        /// <summary> Copy a sequence into a span, in reverse. </summary>
        public static Span<T> ReverseInto<T>(
            this Span<T> self,
            Span<T> output)
        =>
            ReverseInto((ReadOnlySpan<T>)self, output);

        /// <summary> Copy a sequence into a span, in reverse. </summary>
        public static Span<TOut> ReverseInto<TOut, TProducer>(
            this SpanEnumerable<TOut, TProducer> spanEnum,
            Span<TOut> output)
            where TProducer : struct, IProducer<TOut>
        =>
            spanEnum.CopyInto(output).ReverseInPlace();

        /// <summary> Copy a sequence into a span, in reverse. </summary>
        public static Span<TOut> ReverseInto<TIn, TOut, TProducer>(
            this SpanEnumerable<TIn, TOut, TProducer> spanEnum,
            Span<TOut> output)
            where TProducer : struct, IProducer<TIn, TOut>
        =>
            spanEnum.CopyInto(output).ReverseInPlace();

        /// <summary> Copy a sequence into a span, in reverse. </summary>
        public static Span<TOut> ReverseInto<TOut>(
            this SpanEnumerable<TOut> spanEnum,
            Span<TOut> output)
        =>
            spanEnum.CopyInto(output).ReverseInPlace();

        /// <summary>
        ///     Reverse the contents of a span in place.
        /// </summary>
        public static Span<T> ReverseInPlace<T>(this Span<T> span)
        {
            span.Reverse();
            return span;
        }

        #region Force OrderingPlan into SpanEnumerable

        /// <summary> 
        ///     The number of elements in a sequence for which the provided 
        ///     predicate returns true.
        /// </summary>
        public static Span<TV> ReverseInto<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan,
                Span<TV> output)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToEnumerable().ReverseInto(output);

        #endregion
    }
}
