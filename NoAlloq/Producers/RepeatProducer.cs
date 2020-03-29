using System;

namespace NoAlloq.Producers
{
    public struct RepeatProducer<T> : IProducer<T>
    {
        /// <summary> The number of values left to produce. </summary>
        private int _count;

        /// <summary> The value to repeat. </summary>
        private readonly T _toRepeat;

        public RepeatProducer(int count, T toRepeat)
        {
            _count = count;
            _toRepeat = toRepeat;
        }

        /// <see cref="IProducer{TOut}.Produce"/>
        public void Produce(ref Span<T> output)
        {
            var count = 0;
            while (_count > 0 && count < output.Length)
            {
                output[count++] = _toRepeat;
                --_count;
            }

            output = output.Slice(0, count);
        }
    }
}
