namespace console
{
    public interface IRandom
    {
        bool CanGenerate<T>(bool bounded);
        IRandom Next<T>(out T value);
        IRandom Next<T>(T lower, T upper, out T value);
    }


    class Program
    {
        static void Main(string[] args)
        {
            IRandom random = new BetterRandom();
            for (int i = 0; i < 100; i++)
            {
                random = random.Next<bool>(out var b);
                if (b)
                {
                    random = random.Next<int>(0, 100, out var n);
                    System.Console.WriteLine(n);
                }
                else
                {
                    random = random.Next<double>(0.0, 1.0, out var d);
                    System.Console.WriteLine(d);
                }
            }
        }
    }
}
