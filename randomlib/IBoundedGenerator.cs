namespace console
{
    internal interface IBoundedGenerator<T>
    {
        IRandom Generate(T lower, T upper, out T value);
    }
}
