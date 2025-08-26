using System.Security.Cryptography;

namespace AES_Project_Domain
{

    public class AES_Cipher
    {

        private readonly byte[][] _expandedKey;
        private readonly int _Nr; // number of rounds

        public AES_Cipher(byte[] key, AesKeySize keySize)
        {
            // Expand the key
            _expandedKey = KeyExpansion.ExpandKey(key, keySize);

            // Get the number of transformation rounds that convert the inputbased on key size:
            // 10 rounds for 128 - bit keys.
            // 12 rounds for 192 - bit keys.
            // 14 rounds for 256 - bit keys.
            _Nr = keySize switch
            {
                AesKeySize.AES128 => 10,
                AesKeySize.AES192 => 12,
                AesKeySize.AES256 => 14,
                _ => throw new ArgumentException("Invalid AES version")
            };
        }

        // Accept key as hex string
        public AES_Cipher(string hexKey, AesKeySize keySize)
            : this(Utility.ToByteArray(hexKey), keySize)
        {
            // Calls the other constructor
        }

        /// <summary>
        /// Encrypt a single 16-byte block.
        /// FIPS-197 Section 5.1
        /// </summary>
        public byte[] Cipher(byte[] input)
        {
            if (input.Length != 16) throw new ArgumentException("Block must be 16 bytes");

            // Convert to 4x4 array
            byte[,] state = new byte[4, 4];
            for (int i = 0; i < 16; i++)
                state[i % 4, i / 4] = input[i];

            AddRoundKey(state, 0);

            for (int round = 1; round < _Nr; round++)
            {
                SubBytes(state);
                ShiftRows(state);
                MixColumns(state);
                AddRoundKey(state, round);
            }

            // Final round
            SubBytes(state);
            ShiftRows(state);
            AddRoundKey(state, _Nr);

            // Convert state back to array
            byte[] output = new byte[16];
            for (int i = 0; i < 16; i++)
                output[i] = state[i % 4, i / 4];

            return output;
        }

        // Cipher method for string input
        public byte[] Cipher(string hexPlaintext)
        {
            byte[] plaintextBytes = Utility.ToByteArray(hexPlaintext);
            return Cipher(plaintextBytes);
        }

        /// <summary>
        /// Substitute byte using SBoxes.SBox
        /// FIPS-197 Section 5.1.1
        /// </summary>
        /// <param name="state"></param>
        private void SubBytes(byte[,] state)
        {
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    state[row, col] = SBoxes.SBox[state[row, col]];
                }
            }
        }

        /// <summary>
        /// Shift rows circularly to the left.
        /// FIPS-197 Section 5.1.2
        /// </summary>
        /// <param name="state"></param>
        private void ShiftRows(byte[,] state)
        {
            // Don't shift first row...
            for (int row = 1; row < 4; row++)
            {
                byte[] temp = new byte[4];
                for (int col = 0; col < 4; col++)
                {
                    temp[col] = state[row, (col + row) % 4];
                }
                for (int col = 0; col < 4; col++)
                {
                    state[row, col] = temp[col];
                }
            }
        }


        /// <summary>
        /// Treat each column in state as a four-term polynomial
        /// FIPS-197 Section 5.1.3
        /// </summary>
        /// <param name="state"></param>
        private void MixColumns(byte[,] state)
        {
            for (int col = 0; col < 4; col++)
            {
                byte s0 = state[0, col];
                byte s1 = state[1, col];
                byte s2 = state[2, col];
                byte s3 = state[3, col];

                state[0, col] = (byte)(FiniteField.GF_Multiply(s0, 2) ^
                                       FiniteField.GF_Multiply(s1, 3) ^
                                       s2 ^ s3);
                state[1, col] = (byte)(s0 ^
                                       FiniteField.GF_Multiply(s1, 2) ^
                                       FiniteField.GF_Multiply(s2, 3) ^
                                       s3);
                state[2, col] = (byte)(s0 ^ s1 ^
                                       FiniteField.GF_Multiply(s2, 2) ^
                                       FiniteField.GF_Multiply(s3, 3));
                state[3, col] = (byte)(FiniteField.GF_Multiply(s0, 3) ^
                                       s1 ^ s2 ^
                                       FiniteField.GF_Multiply(s3, 2));
            }
        }

        /// <summary>
        /// XOR state with round key.
        /// FIPS-197 Section 5.1.4
        /// </summary>
        /// <param name="state"></param>
        /// <param name="round"></param>
        private void AddRoundKey(byte[,] state, int round)
        {
            
            for (int col = 0; col < 4; col++)
            {
                for (int row = 0; row < 4; row++)
                {
                    state[row, col] ^= _expandedKey[round * 4 + col][row];
                }
            }
        }

    }

}