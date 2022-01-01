using System;
namespace Sanchez.Match
{
    public delegate bool ActivePattern<TInput, TOutput>(TInput input, out TOutput output);
    public delegate Optional<TOutput> OptionalPattern<TInput, TOutput>(TInput input);
}
