using NoAlloq.Ordering;
using NoAlloq.Producers;
using System;
using System.Collections.Generic;

namespace NoAlloq
{
    public static class ToAllocExtensions
    {
        /// <summary>
        ///     Create an array containing all elements from the input 
        ///     sequence.
        /// </summary>
        public static TOut[] ToArray<TIn, TOut, TProducer>(
            this SpanEnumerable<TIn, TOut, TProducer> spanEnum)
            where TProducer : IProducer<TIn, TOut>
        {
            if (spanEnum.KnownLength)
            {
                var array = new TOut[spanEnum.Length];
                spanEnum.ConsumeInto(array);
                return array;
            }
            else
            {
                return spanEnum.ToList().ToArray();
            }
        }

        /// <summary>
        ///     Create an array containing all elements from the input 
        ///     sequence.
        /// </summary>
        public static TOut[] ToArray<TOut, TProducer>(
            this SpanEnumerable<TOut, TProducer> spanEnum)
            where TProducer : IProducer<TOut>
        {
            if (spanEnum.KnownLength)
            {
                var array = new TOut[spanEnum.Length];
                spanEnum.ConsumeInto(array);
                return array;
            }
            else
            {
                return spanEnum.ToList().ToArray();
            }
        }

        /// <summary>
        ///     Create an array containing all elements from the input 
        ///     sequence.
        /// </summary>
        public static TOut[] ToArray<TOut>(this SpanEnumerable<TOut> spanEnum)
        {
            if (spanEnum.KnownLength)
            {
                var array = new TOut[spanEnum.Length];
                spanEnum.ConsumeInto(array);
                return array;
            }
            else
            {
                return spanEnum.ToList().ToArray();
            }
        }

        /// <summary>
        ///     Create a hash set containing all elements from the input 
        ///     sequence.
        /// </summary>
        public static HashSet<TOut> ToHashSet<TOut, TProducer>(
            this SpanEnumerable<TOut, TProducer> spanEnum)
            where TProducer : IProducer<TOut>
        {
            var set = new HashSet<TOut>();
            spanEnum.CopyInto(set);
            return set;
        }

        /// <summary>
        ///     Create a hash set containing all elements from the input 
        ///     sequence.
        /// </summary>
        public static HashSet<TOut> ToHashSet<TIn, TOut, TProducer>(
            this SpanEnumerable<TIn, TOut, TProducer> spanEnum)
            where TProducer : IProducer<TIn, TOut>
        {
            var set = new HashSet<TOut>();
            spanEnum.CopyInto(set);
            return set;
        }

        /// <summary>
        ///     Create a hash set containing all elements from the input 
        ///     sequence.
        /// </summary>
        public static HashSet<TOut> ToHashSet<TOut>(
            this SpanEnumerable<TOut> spanEnum)
        {
            var set = new HashSet<TOut>();
            spanEnum.CopyInto(set);
            return set;
        }

        /// <summary>
        ///     Create a list containing all elements from the input 
        ///     sequence.
        /// </summary>
        public static List<TOut> ToList<TOut, TProducer>(
            this SpanEnumerable<TOut, TProducer> spanEnum)
            where TProducer : IProducer<TOut>
        {
            var list = spanEnum.KnownLength
                ? new List<TOut>(spanEnum.Length)
                : new List<TOut>();

            spanEnum.CopyInto(list);

            return list;
        }

        /// <summary>
        ///     Create a list containing all elements from the input 
        ///     sequence.
        /// </summary>
        public static List<TOut> ToList<TOut>(this SpanEnumerable<TOut> spanEnum)
        {
            var list = spanEnum.KnownLength
                ? new List<TOut>(spanEnum.Length)
                : new List<TOut>();

            spanEnum.CopyInto(list);

            return list;
        }

        /// <summary>
        ///     Create a list containing all elements from the input 
        ///     sequence.
        /// </summary>
        public static List<TOut> ToList<TIn, TOut, TProducer>(
            this SpanEnumerable<TIn, TOut, TProducer> spanEnum)
            where TProducer : IProducer<TIn, TOut>
        {
            var list = spanEnum.KnownLength
                ? new List<TOut>(spanEnum.Length)
                : new List<TOut>();

            spanEnum.CopyInto(list);

            return list;
        }

        /// <summary>
        ///     Create a dictionary containing all elements from the input 
        ///     sequence.
        /// </summary>
        public static Dictionary<TK, TV> ToDictionary<TOut, TK, TV, TProducer>(
            this SpanEnumerable<TOut, TProducer> spanEnum,
            Func<TOut, TK> key,
            Func<TOut, TV> value)
            where TProducer : IProducer<TOut>
        {
            var dict = spanEnum.KnownLength
                ? new Dictionary<TK, TV>(spanEnum.Length)
                : new Dictionary<TK, TV>();

            spanEnum.CopyInto(dict, key, value);

            return dict;
        }

        /// <summary>
        ///     Create a dictionary containing all elements from the input 
        ///     sequence.
        /// </summary>
        public static Dictionary<TK, TV> ToDictionary<TOut, TK, TV>(
            this SpanEnumerable<TOut> spanEnum,
            Func<TOut, TK> key,
            Func<TOut, TV> value)
        {
            var dict = spanEnum.KnownLength
                ? new Dictionary<TK, TV>(spanEnum.Length)
                : new Dictionary<TK, TV>();

            spanEnum.CopyInto(dict, key, value);

            return dict;
        }

        /// <summary>
        ///     Create a dictionary containing all elements from the input 
        ///     sequence.
        /// </summary>
        public static Dictionary<TK, TV> ToDictionary<TIn, TOut, TK, TV, TProducer>(
            this SpanEnumerable<TIn, TOut, TProducer> spanEnum,
            Func<TOut, TK> key,
            Func<TOut, TV> value)
            where TProducer : IProducer<TIn, TOut>
        {
            var dict = spanEnum.KnownLength
                ? new Dictionary<TK, TV>(spanEnum.Length)
                : new Dictionary<TK, TV>();

            spanEnum.CopyInto(dict, key, value);

            return dict;
        }

        #region Force OrderingPlan into SpanEnumerable

        /// <summary>
        ///     Create an array containing all elements from the input 
        ///     sequence.
        /// </summary>
        public static TV[] ToArray<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToEnumerable().ToArray();

        /// <summary>
        ///     Create a list containing all elements from the input 
        ///     sequence.
        /// </summary>
        public static List<TV> ToList<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToEnumerable().ToList();

        /// <summary>
        ///     Create a hash set containing all elements from the input 
        ///     sequence.
        /// </summary>
        public static HashSet<TV> ToHashSet<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToEnumerable().ToHashSet();

        /// <summary>
        ///     Create a dictionary containing all elements from the input 
        ///     sequence.
        /// </summary>
        public static Dictionary<TDK, TDV> ToDictionary<TV, TK, TC, TE, TDK, TDV>(
                this OrderingPlan<TV, TK, TC, TE> plan,
                Func<TV, TDK> keySelector,
                Func<TV, TDV> valueSelector)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToEnumerable().ToDictionary(keySelector, valueSelector);

        #endregion
    }
}
