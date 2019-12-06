namespace console
{
    internal interface IBoundedGenerator<TRandom, T> where TRandom : IRandom
    {
        TRandom Generate(T lower, T upper, out T value);
    }
}
