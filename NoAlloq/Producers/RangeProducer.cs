using System;

namespace NoAlloq.Producers
{
    public struct RangeProducer : IProducer<int>
    {
        /// <summary> The next value to be returned. </summary>
        private int _next;

        /// <summary>
        ///     The last value that will be returned. Once <see cref="_next"/>
        ///     is greated, no more values are returned. 
        /// </summary>
        private readonly int _last;

        public RangeProducer(int next, int last)
        {
            _next = next;
            _last = last;
        }

        /// <see cref="IProducer{TOut}.Produce"/>
        public void Produce(ref Span<int> output)
        {
            var count = 0;
            while (_next <= _last && count < output.Length) 
                output[count++] = _next++;

            output = output.Slice(0, count);
        }
    }
}
