using NoAlloq.Ordering;
using NoAlloq.Producers;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NoAlloq
{
    public static class AggregateExtensions
    {
        #region Aggregate(enum, seed, func, resultSelector)

        public static TResult Aggregate<TIn, TSource, TProducer, TAccum, TResult>(
                this SpanEnumerable<TIn, TSource, TProducer> enumerable,
                TAccum seed,
                Func<TAccum, TSource, TAccum> func,
                Func<TAccum, TResult> resultSelector)
            where TProducer : IProducer<TIn, TSource>
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            TSource onStack = default;
            Span<TSource> spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            while (enumerable.ConsumeInto(spanOnStack).Length > 0)
                seed = func(seed, onStack);

            return resultSelector(seed);
        }

        public static TResult Aggregate<TSource, TProducer, TAccum, TResult>(
                this SpanEnumerable<TSource, TProducer> enumerable,
                TAccum seed,
                Func<TAccum, TSource, TAccum> func,
                Func<TAccum, TResult> resultSelector)
            where TProducer : IProducer<TSource>
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            TSource onStack = default;
            Span<TSource> spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            while (enumerable.ConsumeInto(spanOnStack).Length > 0)
                seed = func(seed, onStack);

            return resultSelector(seed);
        }

        public static TResult Aggregate<TSource, TAccum, TResult>(
            this SpanEnumerable<TSource> enumerable,
            TAccum seed,
            Func<TAccum, TSource, TAccum> func,
            Func<TAccum, TResult> resultSelector)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            TSource onStack = default;
            Span<TSource> spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            while (enumerable.ConsumeInto(spanOnStack).Length > 0)
                seed = func(seed, onStack);

            return resultSelector(seed);
        }

        public static TResult Aggregate<TSource, TAccum, TResult>(
            this ReadOnlySpan<TSource> enumerable,
            TAccum seed,
            Func<TAccum, TSource, TAccum> func,
            Func<TAccum, TResult> resultSelector)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            foreach (var s in enumerable)
                seed = func(seed, s);

            return resultSelector(seed);
        }

        public static TResult Aggregate<TSource, TAccum, TResult>(
            this Span<TSource> enumerable,
            TAccum seed,
            Func<TAccum, TSource, TAccum> func,
            Func<TAccum, TResult> resultSelector)
        =>
            Aggregate((ReadOnlySpan<TSource>)enumerable, seed, func, resultSelector);

        public static TResult Aggregate<TSource, TK, TC, TE, TAccum, TResult>(
                this OrderingPlan<TSource, TK, TC, TE> plan,
                TAccum seed,
                Func<TAccum, TSource, TAccum> func,
                Func<TAccum, TResult> resultSelector)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TSource, TK>
        =>
            plan.ToEnumerable().Aggregate(seed, func, resultSelector);

        #endregion Aggregate(enum, seed, func, resultSelector)

        #region Aggregate(enum, seed, func)

        public static TAccum Aggregate<TIn, TSource, TProducer, TAccum>(
                this SpanEnumerable<TIn, TSource, TProducer> enumerable,
                TAccum seed,
                Func<TAccum, TSource, TAccum> func)
            where TProducer : IProducer<TIn, TSource>
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

         
            TSource onStack = default;
            Span<TSource> spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            while (enumerable.ConsumeInto(spanOnStack).Length > 0)
                seed = func(seed, onStack);

            return seed;
        }

        public static TAccum Aggregate<TSource, TProducer, TAccum>(
                this SpanEnumerable<TSource, TProducer> enumerable,
                TAccum seed,
                Func<TAccum, TSource, TAccum> func)
            where TProducer : IProducer<TSource>
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            TSource onStack = default;
            Span<TSource> spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            while (enumerable.ConsumeInto(spanOnStack).Length > 0)
                seed = func(seed, onStack);

            return seed;
        }

        public static TAccum Aggregate<TSource, TAccum>(
            this SpanEnumerable<TSource> enumerable,
            TAccum seed,
            Func<TAccum, TSource, TAccum> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            TSource onStack = default;
            Span<TSource> spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            while (enumerable.ConsumeInto(spanOnStack).Length > 0)
                seed = func(seed, onStack);

            return seed;
        }

        public static TAccum Aggregate<TSource, TAccum>(
            this ReadOnlySpan<TSource> enumerable,
            TAccum seed,
            Func<TAccum, TSource, TAccum> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            foreach (var s in enumerable)
                seed = func(seed, s);

            return seed;
        }

        public static TAccum Aggregate<TSource, TAccum>(
            this Span<TSource> enumerable,
            TAccum seed,
            Func<TAccum, TSource, TAccum> func)
        =>
            Aggregate((ReadOnlySpan<TSource>)enumerable, seed, func);

        public static TAccum Aggregate<TSource, TK, TC, TE, TAccum>(
                this OrderingPlan<TSource, TK, TC, TE> plan,
                TAccum seed,
                Func<TAccum, TSource, TAccum> func)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TSource, TK>
        =>
            plan.ToEnumerable().Aggregate(seed, func);

        #endregion Aggregate(enum, seed, func)

        #region Aggregate(enum, func)

        public static TSource Aggregate<TIn, TSource, TProducer>(
                this SpanEnumerable<TIn, TSource, TProducer> enumerable,
                Func<TSource, TSource, TSource> func)
            where TProducer : IProducer<TIn, TSource>
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            TSource onStack = default;
            Span<TSource> spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            if (enumerable.ConsumeInto(spanOnStack).Length == 0)
                throw new InvalidOperationException(
                    "Source contains no elements.");

            var seed = onStack;
            while (enumerable.ConsumeInto(spanOnStack).Length > 0)
                seed = func(seed, onStack);

            return seed;
        }

        public static TSource Aggregate<TSource, TProducer>(
                this SpanEnumerable<TSource, TProducer> enumerable,
                Func<TSource, TSource, TSource> func)
            where TProducer : IProducer<TSource>
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            TSource onStack = default;
            Span<TSource> spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            if (enumerable.ConsumeInto(spanOnStack).Length == 0)
                throw new InvalidOperationException(
                    "Source contains no elements.");

            var seed = onStack;
            while (enumerable.ConsumeInto(spanOnStack).Length > 0)
                seed = func(seed, onStack);

            return seed;
        }

        public static TSource Aggregate<TSource>(
            this SpanEnumerable<TSource> enumerable,
            Func<TSource, TSource, TSource> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            TSource onStack = default;
            Span<TSource> spanOnStack = MemoryMarshal.CreateSpan(ref onStack, 1);

            if (enumerable.ConsumeInto(spanOnStack).Length == 0)
                throw new InvalidOperationException(
                    "Source contains no elements.");

            var seed = onStack;
            while (enumerable.ConsumeInto(spanOnStack).Length > 0)
                seed = func(seed, onStack);

            return seed;
        }

        public static TSource Aggregate<TSource>(
            this ReadOnlySpan<TSource> span,
            Func<TSource, TSource, TSource> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            if (span.Length == 0)
                throw new InvalidOperationException("Source contains no elements.");

            var seed = span[0];
            for (var i = 1; i < span.Length; ++i)
                seed = func(seed, span[i]);

            return seed;
        }

        public static TSource Aggregate<TSource>(
            this Span<TSource> span,
            Func<TSource, TSource, TSource> func)
        =>
            Aggregate((ReadOnlySpan<TSource>)span, func);

        public static TSource Aggregate<TSource, TK, TC, TE>(
                this OrderingPlan<TSource, TK, TC, TE> plan,
                Func<TSource, TSource, TSource> func)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TSource, TK>
        =>
            plan.ToEnumerable().Aggregate(func);

        #endregion Aggregate(enum, func)
    }
}
