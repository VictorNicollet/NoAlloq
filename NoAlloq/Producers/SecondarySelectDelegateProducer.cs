using System;
using System.Runtime.InteropServices;

namespace NoAlloq.Producers
{
    /// <summary>
    ///     Produces an output value for each input value, by passing 
    ///     the value through another producer, then applying a delegate
    ///     to the result.
    /// </summary>
    public struct SecondarySelectDelegateProducer<TIn, TMid, TOut, TProducer>
        : IProducer<TIn, TOut>
        where TProducer : IProducer<TIn, TMid>
    {
        private TProducer _inner;

        private readonly Func<TMid, TOut> _delegate;

        public SecondarySelectDelegateProducer(
            TProducer inner,
            Func<TMid, TOut> d)
        {
            _inner = inner; 
            _delegate = d;
        }

        public void Produce(ref ReadOnlySpan<TIn> input, ref Span<TOut> output)
        {
            // Create a temporary on-stack location for the middle values.
            TMid midOnStack = default;
            var mid = MemoryMarshal.CreateSpan(ref midOnStack, 1);

            var copied = 0;
            while (copied < output.Length)
            {
                _inner.Produce(ref input, ref mid);
                if (mid.Length == 0) break;

                output[copied++] = _delegate(midOnStack);
            }

            output = output.Slice(0, copied);
        }
    }


    /// <summary>
    ///     Produces an output value for each input value, by passing 
    ///     the value through another producer, then applying a delegate
    ///     to the result.
    /// </summary>
    public struct SecondarySelectDelegateProducer<TIn, TOut, TProducer> : IProducer<TOut>
        where TProducer : IProducer<TIn>
    {
        private TProducer _inner;

        private readonly Func<TIn, TOut> _delegate;

        public SecondarySelectDelegateProducer(
            TProducer inner,
            Func<TIn, TOut> d)
        {
            _inner = inner;
            _delegate = d;
        }

        public void Produce(ref Span<TOut> output)
        {
            // Create a temporary on-stack location for the middle values.
            TIn midOnStack = default;
            var mid = MemoryMarshal.CreateSpan(ref midOnStack, 1);

            var copied = 0;
            while (copied < output.Length)
            {
                _inner.Produce(ref mid);
                if (mid.Length == 0) break;

                output[copied++] = _delegate(midOnStack);
            }

            output = output.Slice(0, copied);
        }
    }
}
