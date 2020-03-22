using System;

namespace NoAlloq.Producers
{
    /// <summary>
    ///     Produces a slice of an enumerable represented by an inner
    ///     <see cref="ISpanEnumerable{TOut}"/>
    /// </summary>
    public struct SliceProducer<TOut, TProducer> : IProducer<TOut>
        where TProducer : IProducer<TOut>
    {
        private TProducer _inner;

        private int _skip;

        private int _take;

        public SliceProducer(TProducer inner, int skip, int take)
        {
            _inner = inner;
            _skip = skip;
            _take = take;
        }

        /// <see cref="IProducer{TOut}.Produce"/>
        public void Produce(ref Span<TOut> output)
        {
            while (_skip > 0)
            {
                var throwaway = output;
                if (throwaway.Length > _skip)
                    throwaway = throwaway.Slice(0, _skip);

                _inner.Produce(ref throwaway);

                _skip -= throwaway.Length;
                if (throwaway.Length == 0) return;
            }

            if (_take < output.Length)
                output = output.Slice(0, _take);

            _take -= output.Length;

            _inner.Produce(ref output);
        }
    }

    /// <summary>
    ///     Produces a slice of an enumerable represented by an inner
    ///     <see cref="IProducer{TIn, TOut}"/>
    /// </summary>
    public struct SliceProducer<TIn, TOut, TProducer> : IProducer<TIn, TOut>
        where TProducer : IProducer<TIn, TOut>
    {
        private readonly TProducer _inner;

        /// <summary> How many items remain to be skipped. </summary>
        private int _skip;

        /// <summary> How many items remain to be skipped. </summary>
        private int _take;

        public SliceProducer(TProducer inner, int skip, int take)
        {
            _inner = inner;
            _skip = skip;
            _take = take;
        }

        public void Produce(ref ReadOnlySpan<TIn> input, ref Span<TOut> output)
        {
            while (_skip > 0)
            {
                var throwaway = output;
                if (throwaway.Length > _skip)
                    throwaway = throwaway.Slice(0, _skip);

                _inner.Produce(ref input, ref throwaway);

                _skip -= throwaway.Length;
                if (throwaway.Length == 0) return;
            }

            if (_take < output.Length)
                output = output.Slice(0, _take);

            _take -= output.Length;

            _inner.Produce(ref input, ref output);
        }
    }
}
