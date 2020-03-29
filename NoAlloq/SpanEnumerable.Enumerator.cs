using NoAlloq.Producers;
using System;
using System.Runtime.InteropServices;

namespace NoAlloq
{
    public ref partial struct SpanEnumerable<TOut>
    {
        /// <summary>
        ///     An enumerator, duck-typing-compatible with foreach.
        /// </summary>
        public ref struct Enumerator
        {
            private SpanEnumerable<TOut> _enum;

            public TOut Current { get; private set; }

            public Enumerator(SpanEnumerable<TOut> e)
            {
                _enum = e;
                Current = default;
            }

            public bool MoveNext()
            {
                TOut onStack = default;
                Span<TOut> span = MemoryMarshal.CreateSpan(ref onStack, 1);
                if (_enum.ConsumeInto(span).Length == 0)
                    return false;

                Current = onStack;
                return true;
            }
        }

        /// <summary>
        ///     Returns an enumerator duck-typing-compatible with foreach.
        /// </summary>
        public Enumerator GetEnumerator() => new Enumerator(this);
    }

    public ref partial struct SpanEnumerable<TIn, TOut, TProducer>
    {
        /// <summary>
        ///     An enumerator, duck-typing-compatible with foreach.
        /// </summary>
        public ref struct Enumerator
        {
            private SpanEnumerable<TIn, TOut, TProducer> _enum;

            public TOut Current { get; private set; }

            public Enumerator(SpanEnumerable<TIn, TOut, TProducer> e)
            {
                _enum = e;
                Current = default;
            }

            public bool MoveNext()
            {
                TOut onStack = default;
                Span<TOut> span = MemoryMarshal.CreateSpan(ref onStack, 1);
                if (_enum.ConsumeInto(span).Length == 0)
                    return false;

                Current = onStack;
                return true;
            }
        }

        /// <summary>
        ///     Returns an enumerator duck-typing-compatible with foreach.
        /// </summary>
        public Enumerator GetEnumerator() => new Enumerator(this);
    }

    public ref partial struct SpanEnumerable<TOut, TProducer>
    {
        /// <summary>
        ///     An enumerator, duck-typing-compatible with foreach.
        /// </summary>
        public ref struct Enumerator
        {
            private SpanEnumerable<TOut, TProducer> _enum;

            public TOut Current { get; private set; }

            public Enumerator(SpanEnumerable<TOut, TProducer> e)
            {
                _enum = e;
                Current = default;
            }

            public bool MoveNext()
            {
                TOut onStack = default;
                Span<TOut> span = MemoryMarshal.CreateSpan(ref onStack, 1);
                if (_enum.ConsumeInto(span).Length == 0)
                    return false;

                Current = onStack;
                return true;
            }
        }

        /// <summary>
        ///     Returns an enumerator duck-typing-compatible with foreach.
        /// </summary>
        public Enumerator GetEnumerator() => new Enumerator(this);
    }
}
