using System;

namespace NoAlloq.Producers
{
    /// <summary>
    ///     Produces values into an output span from values in an 
    ///     input span.
    /// </summary>
    /// <see cref="SpanEnumerable"/>
    public interface IProducer<TIn, TOut>
    {
        /// <summary>
        ///     Attempt to fill <paramref name="output"/> with values, 
        ///     consuming as many values from <paramref name="input"/> as 
        ///     necessary. 
        /// </summary>
        /// <param name="input">
        ///     The input values. After the call, this span will be resized
        ///     to contain only the values that have not been consumed.
        /// </param>
        /// <param name="output">
        ///     The space where output values are written. After the call, 
        ///     this span will be resized to contain only the values that
        ///     have been written.
        /// </param>
        void Produce(ref ReadOnlySpan<TIn> input, ref Span<TOut> output);
    }

    /// <summary>
    ///     Produces values into an output span.
    /// </summary>
    /// <see cref="SpanEnumerable"/>
    public interface IProducer<TOut>
    {
        /// <summary>
        ///     Attempt to fill <paramref name="output"/> with values.
        /// </summary>
        /// <param name="output">
        ///     The space where output values are written. After the call, 
        ///     this span will be resized to contain only the values that
        ///     have been written.
        /// </param>
        void Produce(ref Span<TOut> output);
    }
}
