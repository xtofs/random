namespace console
{
    public interface IRandom
    {
        bool CanGenerate<T>(bool bounded);
        IRandom Next<T>(out T value);
        IRandom Next<T>(T lower, T upper, out T value);
    }
}
