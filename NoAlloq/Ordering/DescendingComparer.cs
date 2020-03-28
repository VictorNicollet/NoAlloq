using System.Collections.Generic;

namespace NoAlloq.Ordering
{
    public struct DescendingComparer<TKey, TComparer> : IComparer<TKey>
        where TComparer : IComparer<TKey>
    {
        /// <summary> The ascending comparer. </summary>
        private readonly TComparer _inner;

        public DescendingComparer(TComparer inner)
        {
            _inner = inner;
        }

        /// <see cref="IComparer{T}.Compare(T, T)"/>
        public int Compare(TKey x, TKey y) =>
            _inner.Compare(y, x);
    }

}
