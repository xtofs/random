using System;
using System.Collections.Generic;
using System.Linq;

namespace console
{
    public class Counter<T>
    {
        private readonly Dictionary<T, int> _counter = new Dictionary<T, int>();

        public int this[T key]
        {
            get => _counter.TryGetValue(key, out var value) ? value : 0;
            set => _counter[key] = 1 + this[key];
        }

        public int Total => _counter.Values.Sum();

        public IReadOnlyDictionary<T, int> Counts => _counter;
    }

    public static class CounterExtensions
    {
        public static (long, double, double) Stats(this Counter<double> counter)
        {
            // https://en.wikipedia.org/wiki/Standard_deviation#Identities_and_mathematical_properties

            var (cnt, sum, ssq) = counter.Counts.Aggregate((0L, .0, .0), (s, p) =>
                (s.Item1 + p.Value, s.Item2 + p.Key * p.Value, s.Item3 + Square(p.Key * p.Value)));

            return (cnt, sum / cnt, Math.Sqrt(ssq / cnt - Square(sum / cnt)));
        }

        public static (long, double, double) Stats(this Counter<int> counter)
        {
            var (cnt, sum, ssq) = counter.Counts.Aggregate((0L, 0L, 0L), (s, p) =>
                (s.Item1 + p.Value, s.Item2 + p.Key * p.Value, s.Item3 + Square(p.Key * p.Value)));

            // var count = counter.Counts.Sum(p => p.Value);
            // var avg = counter.Counts.Sum(p => p.Key * p.Value) / count;
            // var std = Math.Sqrt(counter.Counts.Sum(p => Square(p.Key * p.Value - avg)) / count);

            return (cnt, sum / cnt, Math.Sqrt((double)ssq / cnt - Square((double)sum / cnt)));
        }

        private static double Square(double v) => v * v;

        private static long Square(long v) => v * v;
    }
}
