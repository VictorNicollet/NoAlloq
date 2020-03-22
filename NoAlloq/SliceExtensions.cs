using NoAlloq.Ordering;
using NoAlloq.Producers;
using System.Collections.Generic;

namespace NoAlloq
{
    public static class SliceExtensions
    {
        #region Force OrderingPlan into SpanEnumerable

        /// <summary>
        ///     Returns a new sequence that contains only the elements after a certain 
        ///     position.
        /// </summary>
        public static SpanEnumerable<TV, TV,
            SliceProducer<TV, TV,
                OrderingProducer<TV, TK, TC, TE>>>
            Slice<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan,
                int offset)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToEnumerable().Slice(offset);

        /// <summary>
        ///     Returns a new sequence that contains only the elements after a certain 
        ///     position, and only a limited number of those elements.
        /// </summary>
        public static SpanEnumerable<TV, TV,
            SliceProducer<TV, TV,
                OrderingProducer<TV, TK, TC, TE>>>
            Slice<TV, TK, TC, TE>(
                this OrderingPlan<TV, TK, TC, TE> plan,
                int offset,
                int count)
            where TC : IComparer<TK>
            where TE : struct, IKeyExtractor<TV, TK>
        =>
            plan.ToEnumerable().Slice(offset, count);

        #endregion
    }
}
