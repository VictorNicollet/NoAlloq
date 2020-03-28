using NoAlloq.Producers;
using System;

namespace NoAlloq
{
    public ref partial struct SpanEnumerable<TOut>
    {
        [Obsolete("Use '.Slice()' instead.", true)]
        public SpanEnumerable<TOut> Take(int count) =>
            throw new NotSupportedException();

        [Obsolete("Use '.Slice()' instead.", true)]
        public SpanEnumerable<TOut> Skip(int count) =>
            throw new NotSupportedException();

        /// <summary>
        ///     Returns a new sequence that contains only the elements after a certain 
        ///     position.
        /// </summary>
        public SpanEnumerable<TOut> Slice(int offset) =>
            new SpanEnumerable<TOut>(
                _input,
                new SliceProducer<byte, TOut, IProducer<byte,TOut>>(
                    _producer, offset, int.MaxValue),
                length:
                    LengthIfKnown >= offset ? LengthIfKnown - offset :
                    LengthIfKnown >= 0 ? 0 : -1);

        /// <summary>
        ///     Returns a new sequence that contains only the elements after a certain 
        ///     position, and only a limited number of those elements.
        /// </summary>
        public SpanEnumerable<TOut> Slice(int offset, int count) =>
            new SpanEnumerable<TOut>(
                _input,
                new SliceProducer<byte, TOut, IProducer<byte, TOut>>(
                    _producer, offset, count),
                length:
                    LengthIfKnown >= offset + count ? count :
                    LengthIfKnown >= offset ? LengthIfKnown - offset :
                    -1);
    }

    public ref partial struct SpanEnumerable<TIn, TOut, TProducer>
    {
        [Obsolete("Use '.Slice()' instead.", true)]
        public SpanEnumerable<TIn, TOut, SliceProducer<TIn, TOut, TProducer>>
            Take(int count)
        =>
            throw new NotSupportedException();

        [Obsolete("Use '.Slice()' instead.", true)]
        public SpanEnumerable<TIn, TOut, SliceProducer<TIn, TOut, TProducer>>
            Skip(int count)
        =>
            throw new NotSupportedException();

        /// <summary>
        ///     Returns a new sequence that contains only the elements after a certain 
        ///     position.
        /// </summary>
        public SpanEnumerable<TIn, TOut, SliceProducer<TIn, TOut, TProducer>>
            Slice(int offset)
        =>
            new SpanEnumerable<TIn, TOut, SliceProducer<TIn, TOut, TProducer>>(
                Input,
                new SliceProducer<TIn, TOut, TProducer>(
                    Producer, offset, int.MaxValue),
                length: 
                    LengthIfKnown >= offset ? LengthIfKnown - offset : 
                    LengthIfKnown >= 0 ? 0 : -1);

        /// <summary>
        ///     Returns a new sequence that contains only the elements after a certain 
        ///     position, and only a limited number of those elements.
        /// </summary>
        public SpanEnumerable<TIn, TOut, SliceProducer<TIn, TOut, TProducer>>
            Slice(int offset, int count)
        =>
            new SpanEnumerable<TIn, TOut, SliceProducer<TIn, TOut, TProducer>>(
                Input,
                new SliceProducer<TIn, TOut, TProducer>(
                    Producer, offset, count),
                length: 
                    LengthIfKnown >= offset + count ? count : 
                    LengthIfKnown >= offset ? LengthIfKnown - offset : 
                    -1);
    }

    public partial struct SpanEnumerable<TOut, TProducer>
    {
        [Obsolete("Use '.Slice()' instead.", true)]
        public SpanEnumerable<TOut, SliceProducer<TOut, TProducer>>
            Take(int count)
        =>
            throw new NotSupportedException();

        [Obsolete("Use '.Slice()' instead.", true)]
        public SpanEnumerable<TOut, SliceProducer<TOut, TProducer>>
            Skip(int count)
        =>
            throw new NotSupportedException();

        /// <summary>
        ///     Returns a new sequence that contains only the elements after a certain 
        ///     position, and only a limited number of those elements.
        /// </summary>
        public SpanEnumerable<TOut, SliceProducer<TOut, TProducer>>
            Slice(int offset, int count)
        =>
            new SpanEnumerable<TOut, SliceProducer< TOut, TProducer>>(
                new SliceProducer<TOut, TProducer>(
                    Producer, offset, count),
                length:
                    LengthIfKnown >= offset + count ? count :
                    LengthIfKnown >= offset ? LengthIfKnown - offset :
                    -1);

        /// <summary>
        ///     Returns a new sequence that contains only the elements after a certain 
        ///     position.
        /// </summary>
        public SpanEnumerable<TOut, SliceProducer<TOut, TProducer>>
            Slice(int offset)
        =>
            new SpanEnumerable<TOut, SliceProducer<TOut, TProducer>>(
                new SliceProducer<TOut, TProducer>(
                    Producer, offset, int.MaxValue),
                length:
                    LengthIfKnown >= offset ? LengthIfKnown - offset :
                    LengthIfKnown >= 0 ? 0 : -1);
    }
}
