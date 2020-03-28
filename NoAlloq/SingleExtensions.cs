using NoAlloq.Producers;
using System;
using System.Runtime.InteropServices;

namespace NoAlloq
{
    public static class SingleExtensions
    {
        /// <summary> The single element of a sequence. </summary>
        public static TOut Single<TIn, TOut, TProducer>(
            this SpanEnumerable<TIn, TOut, TProducer> spanEnum)
            where TProducer : struct, IProducer<TIn, TOut>
        {
            TOut onStack = default;
            Span<TOut> outOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            if (spanEnum.ConsumeInto(outOnStack).Length == 0)
                throw new InvalidOperationException("The input sequence is empty.");
            
            var single = onStack;
            
            if (spanEnum.ConsumeInto(outOnStack).Length != 0)
                throw new InvalidOperationException("The input sequence contains more than one element.");

            return single;
        }

        /// <summary> The single element of a sequence. </summary>
        public static TOut Single<TOut>(
            this SpanEnumerable<TOut> spanEnum)
        {
            TOut onStack = default;
            Span<TOut> outOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            if (spanEnum.ConsumeInto(outOnStack).Length == 0)
                throw new InvalidOperationException("The input sequence is empty.");

            var single = onStack;

            if (spanEnum.ConsumeInto(outOnStack).Length != 0)
                throw new InvalidOperationException("The input sequence contains more than one element.");

            return single;
        }

        /// <summary> The single element of a sequence. </summary>
        public static T Single<T>(this ReadOnlySpan<T> span)
        {
            if (span.Length == 0)
                throw new InvalidOperationException("The input sequence is empty.");

            if (span.Length > 1)
                throw new InvalidOperationException("The input sequence contains more than one element.");

            return span[0];
        }

        /// <summary> The single element of a sequence. </summary>
        public static T Single<T>(this Span<T> span) =>
            Single((ReadOnlySpan<T>)span);

        /// <summary> 
        ///     The single element of a sequence that satisfies a predicate. 
        /// </summary>
        public static TOut Single<TIn, TOut, TProducer>(
            this SpanEnumerable<TIn, TOut, TProducer> spanEnum,
            Predicate<TOut> predicate)
            where TProducer : struct, IProducer<TIn, TOut>
        =>
            spanEnum.Where(predicate).Single();

        /// <summary> 
        ///     The single element of a sequence that satisfies a predicate. 
        /// </summary>
        public static TOut Single<TOut>(
            this SpanEnumerable<TOut> spanEnum,
            Predicate<TOut> predicate)
        =>
            spanEnum.Where(predicate).Single();

        /// <summary> 
        ///     The single element of a sequence that satisfies a predicate.
        /// </summary>
        public static T Single<T>(
            this ReadOnlySpan<T> span,
            Predicate<T> predicate)
        {
            var wasFound = false;
            T found = default;

            foreach (var s in span)
            {
                if (!predicate(s))
                    continue;
                
                if (wasFound)
                    throw new InvalidOperationException("The input sequence contains more than one element.");

                wasFound = true;
                found = s;
            }

            if (wasFound) 
                return found;

            throw new InvalidOperationException("The input sequence is empty.");
        }

        /// <summary> 
        ///     The single element of a sequence that satisfies a predicate.
        /// </summary>
        public static T Single<T>(this Span<T> span, Predicate<T> predicate) =>
            Single((ReadOnlySpan<T>)span, predicate);
    }
}
