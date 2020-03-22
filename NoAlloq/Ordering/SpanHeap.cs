using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NoAlloq.Ordering
{
    /// <summary>
    ///     Implementation of a min-heap inside a `Span`. The root of the heap is
    ///     at position `span.Length - 1`.
    /// </summary>
    internal static class SpanHeap
    {
        /// <summary> The index of the root cell in the heap. </summary>
        private const int Root = 1;

        /// <summary> The index of the parent of a heap cell. </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Parent(int cell) => cell / 2;

        /// <summary> The index of the left sub-cell of a heap cell. </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Left(int cell) => cell * 2;

        /// <summary> The index of the left sub-cell of a heap cell. </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Right(int cell) => cell * 2 + 1;

        /// <summary>
        ///     A reference to the contents of the cell at the specified 
        ///     index.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ref T Cell<T>(Span<T> cells, int index) =>
            ref cells[cells.Length - index];

        /// <summary> The largest cell index in the heap. </summary>
        /// <remarks>
        ///     This is *not* the cell that contains the maximum value !
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int MaxCell<T>(Span<T> cells) =>
            cells.Length;

        /// <summary> Heap sort sifting. </summary>
        internal static void Sift<TValue, TExtractor, TKey, TComparer>(
                Span<TValue> heap,
                int index, 
                TExtractor extractor,
                TComparer comparer)
            where TExtractor : struct, IKeyExtractor<TValue, TKey>
            where TComparer : IComparer<TKey>
        {
            var max = MaxCell(heap);

            var sift = index;
            ref var siftCell = ref Cell(heap, sift);
            var siftKey = extractor.Extract(siftCell);

            // Sift down the node at 'sift'
            while (Left(sift) <= max)
            {
                ref var leftCell = ref Cell(heap, Left(sift));
                var leftKey = extractor.Extract(leftCell);

                ref var rightCell = ref leftCell;
                var rightKey = leftKey;

                if (Right(sift) <= max)
                {
                    rightCell = ref Cell(heap, Right(sift));
                    rightKey = extractor.Extract(rightCell);
                }

                var swapWithLeft = comparer.Compare(siftKey, leftKey) > 0;
                var swapWithRight = comparer.Compare(siftKey, rightKey) > 0;

                if (swapWithLeft && swapWithRight)
                {
                    // Both left and right are candidates, eliminate one.
                    if (comparer.Compare(leftKey, rightKey) > 0)
                    {
                        swapWithLeft = false;
                    }
                    else
                    {
                        swapWithRight = false;
                    }
                }

                if (swapWithLeft)
                {
                    // Swap with left and recurse down.
                    var temp = leftCell;
                    leftCell = siftCell;
                    siftCell = temp;

                    siftCell = ref leftCell;
                    sift = Left(sift);
                }
                else if (swapWithRight)
                {
                    // Swap with right and recurse down.
                    var temp = rightCell;
                    rightCell = siftCell;
                    siftCell = temp;

                    siftCell = ref rightCell;
                    sift = Right(sift);
                }
                else
                {
                    // No need to swap, sifting is done.
                    break;
                }
            }
        }

        internal static TValue ExtractMin<TValue, TExtractor, TKey, TComparer>(
                ref Span<TValue> heap, 
                TExtractor extractor, 
                TComparer comparer)
            where TExtractor : struct, IKeyExtractor<TValue, TKey>
            where TComparer : IComparer<TKey>
        {
            ref var root = ref Cell(heap, Root);

            // Extract the root of the min-heap and replace it with the highest
            // remaining cell.
            var min = root;
            root = Cell(heap, MaxCell(heap));
            heap = heap.Slice(1);
            
            if (heap.Length > 0)
                // Sift down the new root to its appropriate position.
                Sift<TValue, TExtractor, TKey, TComparer>(
                    heap, Root, extractor, comparer);

            return min;
        }

        internal static void Make<TValue, TExtractor, TKey, TComparer>(
                Span<TValue> heap, 
                TExtractor extractor, 
                TComparer comparer)
            where TExtractor : struct, IKeyExtractor<TValue, TKey>
            where TComparer : IComparer<TKey>
        {
            var max = MaxCell(heap);

            for (var i = Parent(max); i >= Root; --i)
                Sift<TValue, TExtractor, TKey, TComparer>(
                    heap, i, extractor, comparer);
        }
    }
}
