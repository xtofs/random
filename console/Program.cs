namespace console
{


    class Program
    {
        static void Main(string[] args)
        {
            IRandom random = new WELL512Random();
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
