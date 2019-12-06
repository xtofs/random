using System;

namespace console
{
    class Program
    {
        static void Main(string[] args)
        {
            var counter1 = new Counter<int>();
            var counter2 = new Counter<double>();

            IRandom random = new WELL512Random();
            for (int i = 0; i < 10_000; i++)
            {
                random = random.Next<bool>(out var b);
                if (b)
                {
                    random = random.Next<int>(0, 200, out var n);
                    // System.Console.WriteLine(n);
                    counter1[n]++;
                }
                else
                {
                    random = random.Next<double>(0.0, 1.0, out var d);
                    // System.Console.WriteLine(d);
                    counter2[d]++;
                }
            }

            Console.WriteLine(counter1.Stats());
            Console.WriteLine(counter2.Stats());
        }
    }
}
