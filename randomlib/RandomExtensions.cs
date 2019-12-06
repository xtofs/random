namespace console
{
    public static class RandomExtensions
    {

        public static IRandom Next(this IRandom rand, int lower, int upper, out int value)
        {
            var r = rand.Next<int>(out value);
            value = value % (upper - lower) + lower;
            return r;
        }

        public static IRandom Next(this IRandom rand, double lower, double upper, out double value)
        {
            var r = rand.Next<double>(out value);
            value = value * (upper - lower) + lower;
            return r;
        }
    }
}