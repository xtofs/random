namespace console
{
    internal interface IUnboundedGenerator<T>
    {
        IRandom Generate(out T value);
    }
}
