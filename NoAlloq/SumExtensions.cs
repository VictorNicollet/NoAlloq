using NoAlloq.Ordering;
using NoAlloq.Producers;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;

namespace NoAlloq
{
    public static class SumExtensions
    {
        /// <summary> The sum of a sequence of int. </summary>
        public static int Sum<TIn, TProducer>(
            this SpanEnumerable<TIn, int, TProducer> spanEnum)
            where TProducer : struct, IProducer<TIn, int>
        {
            var vector = spanEnum.VectorSum();
            var accum = vector[0];
            for (var i = 1; i < Vector<int>.Count; ++i)
                accum += vector[i];

            return accum;
        }

        /// <summary> The sum of a sequence of int. </summary>
        public static int Sum<TProducer>(
            this SpanEnumerable<int, TProducer> spanEnum)
            where TProducer : struct, IProducer<int>
        {
            var vector = spanEnum.VectorSum();
            var accum = vector[0];
            for (var i = 1; i < Vector<int>.Count; ++i)
                accum += vector[i];

            return accum;
        }

        /// <summary> The sum of a sequence of int. </summary>
        public static int Sum(this SpanEnumerable<int> spanEnum)
        {
            var vector = spanEnum.VectorSum();
            var accum = vector[0];
            for (var i = 1; i < Vector<int>.Count; ++i)
                accum += vector[i];

            return accum;
        }

        /// <summary> The sum of a sequence of int. </summary>
        public static int Sum(this ReadOnlySpan<int> span)
        {
            var vector = span.VectorSum();
            var accum = vector[0];
            for (var i = 1; i < Vector<int>.Count; ++i)
                accum += vector[i];

            return accum;
        }

        /// <summary> The sum of a sequence of int. </summary>
        public static int Sum(this Span<int> span) =>
            Sum((ReadOnlySpan<int>)span);

        /// <summary> 
        ///     The sum of a sequence, with a delegate returning an integer 
        ///     for each element of the sequence.
        /// </summary>
        public static int Sum<TIn, TOut, TProducer>(
            this SpanEnumerable<TIn, TOut, TProducer> spanEnum,
            Func<TOut, int> getValue)
            where TProducer : struct, IProducer<TIn, TOut>
        =>
            spanEnum.Select(getValue).Sum();

        /// <summary> 
        ///     The sum of a sequence, with a delegate returning an integer 
        ///     for each element of the sequence.
        /// </summary>
        public static int Sum<TOut>(
            this SpanEnumerable<TOut> spanEnum,
            Func<TOut, int> getValue)
        =>
            spanEnum.Select(getValue).Sum();

        /// <summary> 
        ///     The sum of a sequence, with a delegate returning an integer 
        ///     for each element of the sequence.
        /// </summary>
        public static int Sum<T>(
            this ReadOnlySpan<T> span,
            Func<T, int> getValue) 
        =>
            span.Select(getValue).Sum();

        /// <summary> 
        ///     The sum of a sequence, with a delegate returning an integer 
        ///     for each element of the sequence.
        /// </summary>
        public static int Sum<T>(
            this Span<T> span,
            Func<T, int> getValue) 
        =>
            span.Select(getValue).Sum();

        /// <summary> The sum of a sequence of float. </summary>
        public static float Sum<TIn, TProducer>(
            this SpanEnumerable<TIn, float, TProducer> spanEnum)
            where TProducer : struct, IProducer<TIn, float>
        {
            var vector = spanEnum.VectorSum();
            var accum = vector[0];
            for (var i = 1; i < Vector<float>.Count; ++i)
                accum += vector[i];

            return accum;
        }

        /// <summary> The sum of a sequence of float. </summary>
        public static float Sum<TProducer>(
            this SpanEnumerable<float, TProducer> spanEnum)
            where TProducer : struct, IProducer<float>
        {
            var vector = spanEnum.VectorSum();
            var accum = vector[0];
            for (var i = 1; i < Vector<float>.Count; ++i)
                accum += vector[i];

            return accum;
        }

        /// <summary> The sum of a sequence of float. </summary>
        public static float Sum(this SpanEnumerable<float> spanEnum)
        {
            var vector = spanEnum.VectorSum();
            var accum = vector[0];
            for (var i = 1; i < Vector<float>.Count; ++i)
                accum += vector[i];

            return accum;
        }

        /// <summary> The sum of a sequence of float. </summary>
        public static float Sum(this ReadOnlySpan<float> span)
        {
            var vector = span.VectorSum();
            var accum = vector[0];
            for (var i = 1; i < Vector<float>.Count; ++i)
                accum += vector[i];

            return accum;
        }

        /// <summary> The sum of a sequence of float. </summary>
        public static float Sum(this Span<float> span) =>
            Sum((ReadOnlySpan<float>)span);

        /// <summary> 
        ///     The sum of a sequence, with a delegate returning a float 
        ///     for each element of the sequence.
        /// </summary>
        public static float Sum<TIn, TOut, TProducer>(
            this SpanEnumerable<TIn, TOut, TProducer> spanEnum,
            Func<TOut, float> getValue)
            where TProducer : struct, IProducer<TIn, TOut>
        =>
            spanEnum.Select(getValue).Sum();

        /// <summary> 
        ///     The sum of a sequence, with a delegate returning a float 
        ///     for each element of the sequence.
        /// </summary>
        public static float Sum<TOut>(
            this SpanEnumerable<TOut> spanEnum,
            Func<TOut, float> getValue)
        =>
            spanEnum.Select(getValue).Sum();

        /// <summary> 
        ///     The sum of a sequence, with a delegate returning a float
        ///     for each element of the sequence.
        /// </summary>
        public static float Sum<T>(
            this ReadOnlySpan<T> span,
            Func<T, float> getValue)
        =>
            span.Select(getValue).Sum();

        /// <summary> 
        ///     The sum of a sequence, with a delegate returning a float
        ///     for each element of the sequence.
        /// </summary>
        public static float Sum<T>(
            this Span<T> span,
            Func<T, float> getValue)
        =>
            span.Select(getValue).Sum();

        /// <summary> The sum of a sequence of double. </summary>
        public static double Sum<TIn, TProducer>(
            this SpanEnumerable<TIn, double, TProducer> spanEnum)
            where TProducer : struct, IProducer<TIn, double>
        {
            var vector = spanEnum.VectorSum();
            var accum = vector[0];
            for (var i = 1; i < Vector<double>.Count; ++i)
                accum += vector[i];

            return accum;
        }

        /// <summary> The sum of a sequence of double. </summary>
        public static double Sum<TProducer>(
            this SpanEnumerable<double, TProducer> spanEnum)
            where TProducer : struct, IProducer<double>
        {
            var vector = spanEnum.VectorSum();
            var accum = vector[0];
            for (var i = 1; i < Vector<double>.Count; ++i)
                accum += vector[i];

            return accum;
        }

        /// <summary> The sum of a sequence of double. </summary>
        public static double Sum(this SpanEnumerable<double> spanEnum)
        {
            var vector = spanEnum.VectorSum();
            var accum = vector[0];
            for (var i = 1; i < Vector<double>.Count; ++i)
                accum += vector[i];

            return accum;
        }

        /// <summary> The sum of a sequence of double. </summary>
        public static double Sum(this ReadOnlySpan<double> span)
        {
            var vector = span.VectorSum();
            var accum = vector[0];
            for (var i = 1; i < Vector<double>.Count; ++i)
                accum += vector[i];

            return accum;
        }

        /// <summary> The sum of a sequence of double. </summary>
        public static double Sum(this Span<double> span) =>
            Sum((ReadOnlySpan<double>)span);

        /// <summary> 
        ///     The sum of a sequence, with a delegate returning a float 
        ///     for each element of the sequence.
        /// </summary>
        public static double Sum<TIn, TOut, TProducer>(
            this SpanEnumerable<TIn, TOut, TProducer> spanEnum,
            Func<TOut, double> getValue)
            where TProducer : struct, IProducer<TIn, TOut>
        =>
            spanEnum.Select(getValue).Sum();

        /// <summary> 
        ///     The sum of a sequence, with a delegate returning a float 
        ///     for each element of the sequence.
        /// </summary>
        public static double Sum<TOut>(
            this SpanEnumerable<TOut> spanEnum,
            Func<TOut, double> getValue)
        =>
            spanEnum.Select(getValue).Sum();

        /// <summary> 
        ///     The sum of a sequence, with a delegate returning a float
        ///     for each element of the sequence.
        /// </summary>
        public static double Sum<T>(
            this ReadOnlySpan<T> span,
            Func<T, double> getValue)
        =>
            span.Select(getValue).Sum();

        /// <summary> 
        ///     The sum of a sequence, with a delegate returning a float
        ///     for each element of the sequence.
        /// </summary>
        public static double Sum<T>(
            this Span<T> span,
            Func<T, double> getValue)
        =>
            span.Select(getValue).Sum();

        /// <summary>
        ///     SIMD sum of an arbitrary type supported by <see cref="Vector{T}"/>.
        /// </summary>
        private static Vector<TOut> VectorSum<TIn, TOut, TProducer>(
            this SpanEnumerable<TIn, TOut, TProducer> spanEnum) 
            where TProducer : struct, IProducer<TIn, TOut> where TOut : struct
        {
            var accum = Vector<TOut>.Zero;
            var each  = Vector<TOut>.Zero;
            
            var spans = 
                MemoryMarshal.Cast<Vector<TOut>, TOut>(
                    MemoryMarshal.CreateSpan(ref each, 1));

            var read = spans;

            while (read.Length == spans.Length)
            {
                read = spanEnum.ConsumeInto(spans);

                for (var i = read.Length; i < spans.Length; ++i)
                    spans[i] = default;

                accum = Vector.Add(accum, each);
            }

            return accum; 
        }

        /// <summary>
        ///     SIMD sum of an arbitrary type supported by <see cref="Vector{T}"/>.
        /// </summary>
        private static Vector<TOut> VectorSum<TOut, TProducer>(
            this SpanEnumerable<TOut, TProducer> spanEnum)
            where TProducer : struct, IProducer<TOut> 
            where TOut : struct
        {
            var accum = Vector<TOut>.Zero;
            var each = Vector<TOut>.Zero;

            var spans =
                MemoryMarshal.Cast<Vector<TOut>, TOut>(
                    MemoryMarshal.CreateSpan(ref each, 1));

            var read = spans;

            while (read.Length == spans.Length)
            {
                read = spanEnum.ConsumeInto(spans);

                for (var i = read.Length; i < spans.Length; ++i)
                    spans[i] = default;

                accum = Vector.Add(accum, each);
            }

            return accum;
        }

        /// <summary>
        ///     SIMD sum of an arbitrary type supported by <see cref="Vector{T}"/>.
        /// </summary>
        private static Vector<TOut> VectorSum<TOut>(
            this SpanEnumerable<TOut> spanEnum)
            where TOut : struct
        {
            var accum = Vector<TOut>.Zero;
            var each = Vector<TOut>.Zero;

            var spans =
                MemoryMarshal.Cast<Vector<TOut>, TOut>(
                    MemoryMarshal.CreateSpan(ref each, 1));

            var read = spans;

            while (read.Length == spans.Length)
            {
                read = spanEnum.ConsumeInto(spans);

                for (var i = read.Length; i < spans.Length; ++i)
                    spans[i] = default;

                accum = Vector.Add(accum, each);
            }

            return accum;
        }

        /// <summary>
        ///     SIMD sum of an arbitrary type supported by <see cref="Vector{T}"/>.
        /// </summary>
        private static Vector<T> VectorSum<T>(
            this ReadOnlySpan<T> span) where T : unmanaged
        {
            var accum = Vector<T>.Zero;

            var vectors = MemoryMarshal.Cast<T, Vector<T>>(span);

            foreach (var each in vectors)
                accum = Vector.Add(accum, each);

            var read = MemoryMarshal.Cast<Vector<T>, T>(vectors).Length;

            if (read < span.Length)
            {
                Span<T> final = stackalloc T[Vector<T>.Count];
                span.Slice(read).CopyTo(final);

                accum = Vector.Add(
                    accum,
                    MemoryMarshal.Cast<T, Vector<T>>(final)[0]);
            }

            return accum;
        }

        #region Force OrderingPlan into SpanEnumerable

        /// <summary> The sum of a sequence of int. </summary>
        public static int Sum<TK, TC, TE>(
                this OrderingPlan<int, TK, TC, TE> plan)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<int, TK>
        =>
            plan.ToEnumerable().Sum();

        /// <summary> The sum of a sequence of float. </summary>
        public static float Sum<TK, TC, TE>(
                this OrderingPlan<float, TK, TC, TE> plan)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<float, TK>
        =>
            plan.ToEnumerable().Sum();

        /// <summary> The sum of a sequence of double. </summary>
        public static double Sum<TK, TC, TE>(
                this OrderingPlan<double, TK, TC, TE> plan)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<double, TK>
        =>
            plan.ToEnumerable().Sum();

        /// <summary> 
        ///     The sum of a sequence, with a delegate returning an int
        ///     for each element of the sequence.
        /// </summary>
        public static int Sum<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan,
                Func<TV, int> getValue)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToEnumerable().Sum(getValue);

        /// <summary> 
        ///     The sum of a sequence, with a delegate returning a float
        ///     for each element of the sequence.
        /// </summary>
        public static float Sum<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan,
                Func<TV, float> getValue)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToEnumerable().Sum(getValue);

        /// <summary> 
        ///     The sum of a sequence, with a delegate returning a double
        ///     for each element of the sequence.
        /// </summary>
        public static double Sum<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan,
                Func<TV, double> getValue)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToEnumerable().Sum(getValue);

        #endregion
    }
}
