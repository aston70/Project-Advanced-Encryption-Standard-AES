namespace AES_Project_Domain
{

    /// <summary>
    /// Finite field arithmetic in GF(2^8).
    /// Addition, xtime, etc. Section 4 of FIPS-197.
    /// </summary>
    public static class FiniteField
    {

        private const byte Irreducible = 0x1B;

        /// <summary>
        /// Addition in GF(2^8) is just XOR.
        /// Section 4.1 of FIPS-197
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static byte GF_Add(byte a, byte b)
        {
            return (byte)(a ^ b);
        }

        /// <summary>
        /// Multiply by x (i.e., {02}) in GF(2^8).
        /// Section 4.2.1 of FIPS-197
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static byte GF_XTime(byte a)
        {
            return (byte)((a << 1) ^ ((a & 0x80) != 0 ? Irreducible : 0x00));
        }

        /// <summary>
        /// Multiplication in GF(2^8).
        /// Section 4.2.1 of FIPS-197
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Product of a and b</returns>
        public static byte GF_Multiply(byte a, byte b)
        {
            byte p = 0;
            byte hiBitSet;
            for (int i = 0; i < 8; i++)
            {
                if ((b & 1) != 0)
                {
                    p ^= a;
                }
                hiBitSet = (byte)(a & 0x80);
                a <<= 1;
                if (hiBitSet != 0)
                {
                    a ^= 0x1b;
                }
                b >>= 1;
            }
            return p;
        }

    }

}