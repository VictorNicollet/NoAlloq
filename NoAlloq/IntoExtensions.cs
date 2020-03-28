using NoAlloq.Ordering;
using NoAlloq.Producers;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NoAlloq
{
    public static class IntoExtensions
    {
        /// <summary>
        ///     Takes enough values from the sequence to fill the provided span. 
        /// </summary>
        /// <remarks>
        ///     A copy of the enumerable is taken, which means that (beyond any
        ///     side-effects of consuming values from it) the original 
        ///     enumerable is unchanged.
        /// </remarks>
        /// <returns>
        ///     Returns the span, maybe resized if there were not enough values 
        ///     in the enumerable.
        /// </returns>
        public static Span<TOut> TakeInto<TIn, TOut, TProducer>(
            this SpanEnumerable<TIn, TOut, TProducer> self,
            Span<TOut> output)
            where TProducer : IProducer<TIn, TOut>
        {
            self.Producer.Produce(ref self.Input, ref output);
            return output;
        }

        /// <summary>
        ///     Takes enough values from the sequence to fill the 
        ///     provided span. 
        /// </summary>
        /// <remarks>
        ///     A copy of the enumerable is taken, which means that (beyond any
        ///     side-effects of consuming values from it) the original 
        ///     enumerable is unchanged.
        /// </remarks>
        /// <returns>
        ///     Returns the span, maybe resized if 
        ///     there were not enough values in the kernel.
        /// </returns>
        public static Span<TOut> TakeInto<TOut, TProducer>(
            this SpanEnumerable<TOut, TProducer> self,
            Span<TOut> output)
            where TProducer : IProducer<TOut>
        {
            self.Producer.Produce(ref output);
            return output;
        }
        /// <summary>
        ///     Takes enough values from the sequence to fill the provided span. 
        /// </summary>
        /// <remarks>
        ///     A copy of the enumerable is taken, which means that (beyond any
        ///     side-effects of consuming values from it) the original 
        ///     enumerable is unchanged.
        /// </remarks>
        /// <returns>
        ///     Returns the span, maybe resized if there were not enough values 
        ///     in the enumerable.
        /// </returns>
        public static Span<TOut> TakeInto<TOut>(
            this SpanEnumerable<TOut> self,
            Span<TOut> output)
        =>
            self.ConsumeInto(output);
        
        /// <summary>
        ///     Takes values from the sequence to fill the 
        ///     provided span, throws if the span is not large enough to 
        ///     contain all elements from the sequence. 
        /// </summary>
        /// <returns>
        ///     Returns the span, maybe resized if 
        ///     there were not enough values in the enumerable.
        /// </returns>
        public static Span<TOut> CopyInto<TIn, TOut, TProducer>(
           this SpanEnumerable<TIn, TOut, TProducer> spanEnum,
           Span<TOut> output)
           where TProducer : IProducer<TIn, TOut>
        {
            var result = spanEnum.ConsumeInto(output);

            if (result.Length == output.Length)
            {
                TOut more = default;
                Span<TOut> moreSpan = MemoryMarshal.CreateSpan(ref more, 1);
                if (spanEnum.ConsumeInto(moreSpan).Length > 0)
                    throw new ArgumentException(
                        "Destination span is too small.",
                        nameof(output));
            }

            return result;
        }

        /// <summary>
        ///     Takes values from the sequence to fill the 
        ///     provided span, throws if the span is not large enough to 
        ///     contain all elements from the sequence. 
        /// </summary>
        /// <returns>
        ///     Returns the span, maybe resized if 
        ///     there were not enough values in the enumerable.
        /// </returns>
        public static Span<TOut> CopyInto<TOut, TProducer>(
           this SpanEnumerable<TOut, TProducer> spanEnum,
           Span<TOut> output)
           where TProducer : IProducer<TOut>
        {
            var result = spanEnum.ConsumeInto(output);

            if (result.Length == output.Length)
            {
                TOut more = default;
                Span<TOut> moreSpan = MemoryMarshal.CreateSpan(ref more, 1);
                if (spanEnum.ConsumeInto(moreSpan).Length > 0)
                    throw new ArgumentException(
                        "Destination span is too small.",
                        nameof(output));
            }

            return result;
        }

        /// <summary>
        ///     Takes values from the sequence to fill the 
        ///     provided span, throws if the span is not large enough to 
        ///     contain all elements from the sequence. 
        /// </summary>
        /// <returns>
        ///     Returns the span, maybe resized if 
        ///     there were not enough values in the enumerable.
        /// </returns>
        public static Span<TOut> CopyInto<TOut>(
           this SpanEnumerable<TOut> spanEnum,
           Span<TOut> output)
        {
            var result = spanEnum.ConsumeInto(output);

            if (result.Length == output.Length)
            {
                TOut more = default;
                Span<TOut> moreSpan = MemoryMarshal.CreateSpan(ref more, 1);
                if (spanEnum.ConsumeInto(moreSpan).Length > 0)
                    throw new ArgumentException(
                        "Destination span is too small.",
                        nameof(output));
            }

            return result;
        }

        /// <summary>
        ///     Copies all values from a span into a list.
        /// </summary>
        public static void CopyInto<T>(
           this ReadOnlySpan<T> span,
           ICollection<T> output)
        {
            foreach (var s in span)
                output.Add(s);
        }

        /// <summary>
        ///     Copies all values from a span into a list.
        /// </summary>
        public static void CopyInto<T>(this Span<T> span, ICollection<T> output) =>
            CopyInto((ReadOnlySpan<T>)span, output);

        /// <summary>
        ///     Takes values from the sequence to fill the 
        ///     provided array, throws if the array is not large enough to 
        ///     contain all elements from the sequence. 
        /// </summary>
        /// <returns>
        ///     Returns the span, maybe resized if 
        ///     there were not enough values in the enumerable.
        /// </returns>
        public static Span<TOut> CopyInto<TIn, TOut, TProducer>(
           this SpanEnumerable<TIn, TOut, TProducer> spanEnum,
           TOut[] output)
           where TProducer : IProducer<TIn, TOut>
        =>
            spanEnum.CopyInto(output.AsSpan());

        /// <summary>
        ///     Takes values from the sequence to fill the 
        ///     provided array, throws if the array is not large enough to 
        ///     contain all elements from the sequence. 
        /// </summary>
        /// <returns>
        ///     Returns the span, maybe resized if 
        ///     there were not enough values in the enumerable.
        /// </returns>
        public static Span<TOut> CopyInto<TOut, TProducer>(
            this SpanEnumerable<TOut, TProducer> spanEnum,
            TOut[] output)
            where TProducer : IProducer<TOut>
        =>
            spanEnum.CopyInto(output.AsSpan());

        /// <summary>
        ///     Takes values from the sequence to fill the 
        ///     provided array, throws if the array is not large enough to 
        ///     contain all elements from the sequence. 
        /// </summary>
        /// <returns>
        ///     Returns the span, maybe resized if 
        ///     there were not enough values in the enumerable.
        /// </returns>
        public static Span<TOut> CopyInto<TOut>(
           this SpanEnumerable<TOut> spanEnum,
           TOut[] output)
        =>
            spanEnum.CopyInto(output.AsSpan());

        /// <summary>
        ///     Takes values from the sequence to fill the provided collection.
        /// </summary>
        public static void CopyInto<TIn, TOut, TProducer>(
           this SpanEnumerable<TIn, TOut, TProducer> spanEnum,
           ICollection<TOut> output)
           where TProducer : IProducer<TIn, TOut>
        {
            TOut more = default;
            Span<TOut> moreSpan = MemoryMarshal.CreateSpan(ref more, 1);

            while (spanEnum.ConsumeInto(moreSpan).Length > 0)
                output.Add(more);
        }

        /// <summary>
        ///     Takes values from the sequence to fill the provided collection.
        /// </summary>
        public static void CopyInto<TOut, TProducer>(
           this SpanEnumerable<TOut, TProducer> spanEnum,
           ICollection<TOut> output)
           where TProducer : IProducer<TOut>
        {
            TOut more = default;
            Span<TOut> moreSpan = MemoryMarshal.CreateSpan(ref more, 1);

            while (spanEnum.ConsumeInto(moreSpan).Length > 0)
                output.Add(more);
        }

        /// <summary>
        ///     Takes values from the sequence to fill the provided collection.
        /// </summary>
        public static void CopyInto<TOut>(
           this SpanEnumerable<TOut> spanEnum,
           ICollection<TOut> output)
        {
            TOut more = default;
            Span<TOut> moreSpan = MemoryMarshal.CreateSpan(ref more, 1);

            while (spanEnum.ConsumeInto(moreSpan).Length > 0)
                output.Add(more);
        }

        /// <summary>
        ///     Takes values from the sequence to fill the provided dictionary.
        /// </summary>
        public static void CopyInto<TIn, TOut, TK, TV, TProducer>(
           this SpanEnumerable<TIn, TOut, TProducer> spanEnum,
           IDictionary<TK, TV> output,
           Func<TOut, TK> key,
           Func<TOut, TV> value)
           where TProducer : IProducer<TIn, TOut>
        {
            TOut more = default;
            Span<TOut> moreSpan = MemoryMarshal.CreateSpan(ref more, 1);

            while (spanEnum.ConsumeInto(moreSpan).Length > 0)
                output.Add(key(more), value(more));
        }

        /// <summary>
        ///     Takes values from the sequence to fill the provided dictionary.
        /// </summary>
        public static void CopyInto<TOut, TK, TV, TProducer>(
           this SpanEnumerable<TOut, TProducer> spanEnum,
           IDictionary<TK, TV> output,
           Func<TOut, TK> key,
           Func<TOut, TV> value)
           where TProducer : IProducer<TOut>
        {
            TOut more = default;
            Span<TOut> moreSpan = MemoryMarshal.CreateSpan(ref more, 1);

            while (spanEnum.ConsumeInto(moreSpan).Length > 0)
                output.Add(key(more), value(more));
        }

        /// <summary>
        ///     Takes values from the sequence to fill the provided dictionary.
        /// </summary>
        public static void CopyInto<TOut, TK, TV>(
           this SpanEnumerable<TOut> spanEnum,
           IDictionary<TK, TV> output,
           Func<TOut, TK> key,
           Func<TOut, TV> value)
        {
            TOut more = default;
            Span<TOut> moreSpan = MemoryMarshal.CreateSpan(ref more, 1);

            while (spanEnum.ConsumeInto(moreSpan).Length > 0)
                output.Add(key(more), value(more));
        }

        #region Force OrderingPlan into SpanEnumerable

        /// <summary>
        ///     Takes values from the sequence to fill the 
        ///     provided span, throws if the span is not large enough to 
        ///     contain all elements from the sequence. 
        /// </summary>
        /// <returns>
        ///     Returns the span, maybe resized if 
        ///     there were not enough values in the enumerable.
        /// </returns>
        public static void CopyInto<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan,
                Span<TV> output)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToEnumerable().CopyInto(output);

        /// <summary>
        ///     Takes values from the sequence to fill the 
        ///     provided span, throws if the span is not large enough to 
        ///     contain all elements from the sequence. 
        /// </summary>
        /// <returns>
        ///     Returns the span, maybe resized if 
        ///     there were not enough values in the enumerable.
        /// </returns>
        public static void CopyInto<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan,
                TV[] output)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToEnumerable().CopyInto(output);

        /// <summary>
        ///     Takes enough values from the sequence to fill the provided span. 
        /// </summary>
        /// <remarks>
        ///     A copy of the enumerable is taken, which means that (beyond any
        ///     side-effects of consuming values from it) the original 
        ///     enumerable is unchanged.
        /// </remarks>
        /// <returns>
        ///     Returns the span, maybe resized if there were not enough values 
        ///     in the enumerable.
        /// </returns>
        public static Span<TV> TakeInto<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan,
                Span<TV> output)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToEnumerable().TakeInto(output);

        /// <summary>
        ///     Takes values from the sequence to fill the provided collection.
        /// </summary>
        public static void CopyInto<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan,
                ICollection<TV> output)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToEnumerable().CopyInto(output);

        /// <summary>
        ///     Takes values from the sequence to fill the provided dictionary.
        /// </summary>
        public static void CopyInto<TV, TK, TC, TE, TDV, TDK>(
                this OrderingPlan<TV, TK, TC, TE> plan,
                IDictionary<TDK, TDV> output,
                Func<TV, TDK> keySelector,
                Func<TV, TDV> valueSelector)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToEnumerable().CopyInto(output, keySelector, valueSelector);

        #endregion
    }
}
