using System.Collections.Generic;

namespace NoAlloq.Ordering
{
    /// <summary> A lexicographic comparer for a pair. </summary>
    public struct PairComparer<TKey1, TKey2, TComparer1, TComparer2>
            : IComparer<(TKey1, TKey2)>
        where TComparer1 : IComparer<TKey1>
        where TComparer2 : IComparer<TKey2>
    {
        /// <summary> Comparer for the first item of the pair. </summary>
        private readonly TComparer1 _comparer1;

        /// <summary> Comparer for the second item of the pair. </summary>
        private readonly TComparer2 _comparer2;

        public PairComparer(TComparer1 comparer1, TComparer2 comparer2)
        {
            _comparer1 = comparer1;
            _comparer2 = comparer2;
        }

        public int Compare((TKey1, TKey2) x, (TKey1, TKey2) y)
        {
            var c1 = _comparer1.Compare(x.Item1, y.Item1);
            if (c1 != 0) return c1;

            return _comparer2.Compare(x.Item2, y.Item2);
        }
    }
}
