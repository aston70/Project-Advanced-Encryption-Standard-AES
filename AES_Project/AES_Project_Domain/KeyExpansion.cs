namespace AES_Project_Domain
{

    // Key expansion class for AES.
    // Section 5.2 of FIPS-197
    public static class KeyExpansion
    {

        // Round constants
        private static readonly byte[] Rcon = new byte[]
        {
            0x00, // dummy value for 0 index
            0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x1B, 0x36
        };

        /// <summary>
        /// Performs the cyclic operation (left rotate by 1 byte) on a 4-byte word.
        /// Section 5.2
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static byte[] RotWord(byte[] word)
        {
            if (word.Length != 4)
            { 
                throw new ArgumentException("4 bytes only");
            }

            return new byte[] { word[1], word[2], word[3], word[0] };
        }

        /// <summary>
        /// Applies the S-box substitution to each byte of a 4-byte word.
        /// Section 5.2
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static byte[] SubWord(byte[] word)
        {
            if (word.Length != 4)
            {
                throw new ArgumentException("4 bytes only"); 
            }

            return new byte[]
            {
                SBoxes.SBox[word[0]],
                SBoxes.SBox[word[1]],
                SBoxes.SBox[word[2]],
                SBoxes.SBox[word[3]]
            };
        }

        /// <summary>
        /// Expands the cipher key into the key schedule.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keySize">AES128, AES192, or AES256</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static byte[][] ExpandKey(byte[] key, AesKeySize keySize)
        {

            (int Nk, int Nr) = AES_Parameters.GetNkAndNrFromKeySize(keySize);

            if (key.Length != Nk * 4) throw new ArgumentException("Key length mismatch");

            int Nb = 4;
            int totalWords = Nb * (Nr + 1);
            byte[][] w = new byte[totalWords][];

            // first Nk words = original key
            for (int i = 0; i < Nk; i++)
            {
                w[i] = new byte[4];
                Array.Copy(key, i * 4, w[i], 0, 4);
            }

            for (int i = Nk; i < totalWords; i++)
            {
                byte[] temp = new byte[4];
                Array.Copy(w[i - 1], temp, 4);

                if (i % Nk == 0)
                {
                    temp = SubWord(RotWord(temp));
                    temp[0] ^= Rcon[i / Nk];
                }
                else if (Nk > 6 && i % Nk == 4) // for AES-256 extra SubWord
                {
                    temp = SubWord(temp);
                }

                w[i] = new byte[4];
                for (int j = 0; j < 4; j++)
                    w[i][j] = (byte)(w[i - Nk][j] ^ temp[j]);
            }

            return w;
        }

    }

}