namespace console
{
    internal interface IUnboundedGenerator<TRandom, T> where TRandom : IRandom
    {
        TRandom Generate(out T value);
    }
}
