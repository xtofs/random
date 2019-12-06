using System;

namespace console
{



    public class WELL512Random : IRandom,
        IBoundedGenerator<int>,
        IBoundedGenerator<uint>,
        IBoundedGenerator<double>,
        IUnboundedGenerator<bool>
    {
        #region custom RNG implementation

        // http://www.iro.umontreal.ca/~panneton/well/WELL512a.c
        const int R = 16;
        const int M1 = 13;
        const int M2 = 9;

        private static uint MAT0NEG(int t, uint v) => (v ^ (v << (-(t))));
        private static uint MAT0POS(int t, uint v) => (v ^ (v >> t));

        private static uint MAT3NEG(int t, uint v) => (v << (-(t)));
        private static uint MAT4NEG(int t, uint b, uint v) => (v ^ ((v << (-(t))) & b));

        private uint V0 => state[index];
        private uint VM1 => state[(index + M1) & 0x0000000fU];
        private uint VM2 => state[(index + M2) & 0x0000000fU];
        private uint VRm1 => state[(index + 15) & 0x0000000fU];

        // private uint newV0 { set => STATE[(state_i + 15) & 0x0000000fU] = value; }
        // private uint newV1 { get => STATE[state_i]; set => STATE[state_i] = value; }
        const double FACT = 2.32830643653869628906e-10;

        private readonly uint index;
        private readonly uint[] state;

        public WELL512Random() : this(0, new uint[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }) { }

        private WELL512Random(uint i, uint[] init)
        {
            index = i;
            state = new uint[R];
            for (int j = 0; j < R; j++)
            {
                state[j] = init[j];
            }
        }

        private (uint, WELL512Random) NextUInt32()
        {
            var z0 = state[(index + 15) & 0x0000000fU];
            var z1 = MAT0NEG(-16, V0) ^ MAT0NEG(-15, VM1);
            var z2 = MAT0POS(11, VM2);

            // newV1 = z1 ^ z2;
            // newV0 = MAT0NEG(-2, z0) ^ MAT0NEG(-18, z1) ^ MAT3NEG(-28, z2) ^ MAT4NEG(-5, 0xda442d24U, newV1);
            var copy = new uint[R];
            Array.Copy(state, copy, R);
            copy[index] = z1 ^ z2;
            copy[(index + 15) & 0x0000000fU] = MAT0NEG(-2, z0) ^ MAT0NEG(-18, z1) ^ MAT3NEG(-28, z2) ^ MAT4NEG(-5, 0xda442d24U, copy[index]);
            var copy_i = (index + 15) & 0x0000000fU;
            return (copy[index], new WELL512Random(copy_i, copy));
        }
        #endregion

        #region IRandom
        public bool CanGenerate<T>(bool bounded)
        {
            return bounded ? this is IUnboundedGenerator<T> : this is IBoundedGenerator<T>;
        }

        public IRandom Next<T>(out T value)
        {
            if (this is IUnboundedGenerator<T> generator)
                return generator.Generate(out value);

            throw new NotImplementedException();
        }

        public IRandom Next<T>(T lower, T upper, out T value)
        {
            if (this is IBoundedGenerator<T> generator)
                return generator.Generate(lower, upper, out value);

            throw new NotImplementedException();
        }

        #endregion

        #region I...Generators

        public IRandom Generate(uint lower, uint upper, out uint value)
        {
            var (n, next) = this.NextUInt32();
            value = (n % (upper - lower)) + lower;
            return next;
        }

        public IRandom Generate(int lower, int upper, out int value)
        {
            var next = Generate((uint)lower, (uint)upper, out uint u);
            value = (int)u;
            return next;
        }

        public IRandom Generate(double lower, double upper, out double value)
        {
            var (n, next) = NextUInt32();
            value = ((double)n) * FACT;
            value = value * (upper - lower) + lower;
            return next;
        }

        public IRandom Generate(out bool value)
        {
            var (n, next) = this.NextUInt32();
            value = (n & 1) == 0;
            return next;
        }
        #endregion
    }
}
