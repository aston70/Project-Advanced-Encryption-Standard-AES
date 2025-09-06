using System.Security.Cryptography;
using System.Text;

namespace AES_Project_Domain
{

    public class AES_Cipher
    {

        private readonly byte[][] _expandedKey;
        private readonly int _Nr; // number of rounds
        private readonly byte[][] _roundKeys;


        public AES_Cipher(byte[] key, AesKeySize keySize)
        {
            // Expand the key
            _expandedKey = KeyExpansion.ExpandKey(key, keySize);

            // Convert the expanded key words into 16-byte round keys.
            // Each round key corresponds to one round of AES transformations.
            _roundKeys = Utility.GetRoundKeys(key, keySize);

            var (_, Nr) = AES_Parameters.GetNkAndNrFromKeySize(keySize);
            _Nr = Nr;
        }

        // Accept key as hex string
        public AES_Cipher(string hexKey, AesKeySize keySize)
            : this(Utility.ToByteArray(hexKey), keySize)
        {
            // Calls the other constructor
        }

        #region "public cipher calls"

        /// <summary>
        /// Encrypt a single 16-byte block.
        /// FIPS-197 Section 5.1
        /// </summary>
        public byte[] Cipher(byte[] input, bool trace = false)
        {
            if (input.Length != 16) throw new ArgumentException("Block must be 16 bytes");

            Console.WriteLine("CIPHER (ENCRYPT):");

            // Convert to 4x4 array
            byte[,] state = new byte[4, 4];
            for (int i = 0; i < 16; i++)
                state[i % 4, i / 4] = input[i];

            TraceRound(trace, 0, "input", state); // initial input
            AddRoundKey(state, 0);
            TraceRound(trace, 0, "k_sch", _roundKeys[0]);  // first round key

            for (int round = 1; round < _Nr; round++)
            {
                TraceRound(trace, round,"start", state);
                SubBytes(state);

                TraceRound(trace, round, "s_box", state);
                ShiftRows(state);

                TraceRound(trace, round, "s_row", state);
                MixColumns(state);

                TraceRound(trace, round, "m_col", state);
                AddRoundKey(state, round);

                TraceRound(trace, round, "k_sch", _roundKeys[round]);
            }

            // Final round
            TraceRound(trace, _Nr, "start", state);
            SubBytes(state);
            TraceRound(trace, _Nr, "s_box", state);
            ShiftRows(state);
            TraceRound(trace, _Nr, "s_row", state);
            AddRoundKey(state, _Nr);
            TraceRound(trace, _Nr, "k_sch", _roundKeys[_Nr]);

            // Convert state back to array
            byte[] output = new byte[16];
            for (int i = 0; i < 16; i++)
                output[i] = state[i % 4, i / 4];

            // Trace the final output
            TraceRound(trace, _Nr, "output", output);

            return output;
        }

        // Cipher method for string input
        public byte[] Cipher(string hexPlaintext, bool trace = false)
        {
            byte[] plaintextBytes = Utility.ToByteArray(hexPlaintext);
            return Cipher(plaintextBytes, trace);
        }

        #endregion

        #region "public inverse cipher calls"

        public byte[] InvCipher(byte[] input, bool trace = false)
        {
            if (input.Length != 16) throw new ArgumentException("Block must be 16 bytes");

            Console.WriteLine("INVERSE CIPHER (DECRYPT):");

            byte[,] state = new byte[4, 4];
            for (int i = 0; i < 16; i++)
                state[i % 4, i / 4] = input[i];

            // Initial trace
            TraceRound(trace, 0, "iinput", input);
            AddRoundKey(state, _Nr);
            TraceRound(trace, 0, "ik_sch", _roundKeys[_Nr]);

            for (int round = _Nr - 1; round >= 1; round--)
            {
                TraceRound(trace, _Nr - round, "istart", state);

                InvShiftRows(state);
                TraceRound(trace, _Nr - round, "is_row", state);

                InvSubBytes(state);
                TraceRound(trace, _Nr - round, "is_box", state);
                TraceRound(trace, _Nr - round, "ik_sch", _roundKeys[round]);

                AddRoundKey(state, round);
                TraceRound(trace, _Nr - round, "ik_add", state);

                InvMixColumns(state);
            }

            // Final round
            TraceRound(trace, _Nr, "istart", state);
            InvShiftRows(state);
            TraceRound(trace, _Nr, "is_row", state);

            InvSubBytes(state);
            TraceRound(trace, _Nr, "is_box", state);

            TraceRound(trace, _Nr, "ik_sch", _roundKeys[0]);
            AddRoundKey(state, 0);

            byte[] output = new byte[16];
            for (int i = 0; i < 16; i++)
                output[i] = state[i % 4, i / 4];

            TraceRound(trace, _Nr, "ioutput", output);

            return output;
        }

        // Inverse Cipher method for string input
        public byte[] InvCipher(string hexPlaintext, bool trace = false)
        {
            byte[] plaintextBytes = Utility.ToByteArray(hexPlaintext);
            return InvCipher(plaintextBytes, trace);
        }

        private static string StateToHex(byte[,] state)
        {
            var sb = new StringBuilder(32);
            for (int col = 0; col < 4; col++)
                for (int row = 0; row < 4; row++)
                    sb.Append(state[row, col].ToString("x2"));
            return sb.ToString();
        }

        #endregion

        #region "Cipher Steps"

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

        #endregion

        #region "Inverse Cipher Steps"

        /// <summary>
        /// Inverse Substitute byte using SBoxes.InverseSBox
        /// Section 5.3.2 of FIPS-197
        /// </summary>
        /// <param name="state"></param>
        private void InvSubBytes(byte[,] state)
        {
            for (int row = 0; row < 4; row++)
                for (int col = 0; col < 4; col++)
                    state[row, col] = SBoxes.InverseSBox[state[row, col]];
        }

        /// <summary>
        /// Inverse Shift rows circularly to the right.
        /// Section 5.3.1 of FIPS-197
        /// </summary>
        /// <param name="state"></param>
        private void InvShiftRows(byte[,] state)
        {

            for (int row = 1; row < 4; row++)
            {
                byte[] temp = new byte[4];
                for (int col = 0; col < 4; col++)
                    temp[col] = state[row, (col - row + 4) % 4];
                for (int col = 0; col < 4; col++)
                    state[row, col] = temp[col];
            }

        }

        /// <summary>
        /// Inverse Mix Columns
        /// Section 5.3.3 of FIPS-197
        /// </summary>
        /// <param name="state"></param>
        private void InvMixColumns(byte[,] state)
        {

            for (int col = 0; col < 4; col++)
            {
                byte s0 = state[0, col];
                byte s1 = state[1, col];
                byte s2 = state[2, col];
                byte s3 = state[3, col];

                state[0, col] = (byte)(FiniteField.GF_Multiply(s0, 0x0e) ^
                                       FiniteField.GF_Multiply(s1, 0x0b) ^
                                       FiniteField.GF_Multiply(s2, 0x0d) ^
                                       FiniteField.GF_Multiply(s3, 0x09));

                state[1, col] = (byte)(FiniteField.GF_Multiply(s0, 0x09) ^
                                       FiniteField.GF_Multiply(s1, 0x0e) ^
                                       FiniteField.GF_Multiply(s2, 0x0b) ^
                                       FiniteField.GF_Multiply(s3, 0x0d));

                state[2, col] = (byte)(FiniteField.GF_Multiply(s0, 0x0d) ^
                                       FiniteField.GF_Multiply(s1, 0x09) ^
                                       FiniteField.GF_Multiply(s2, 0x0e) ^
                                       FiniteField.GF_Multiply(s3, 0x0b));

                state[3, col] = (byte)(FiniteField.GF_Multiply(s0, 0x0b) ^
                                       FiniteField.GF_Multiply(s1, 0x0d) ^
                                       FiniteField.GF_Multiply(s2, 0x09) ^
                                       FiniteField.GF_Multiply(s3, 0x0e));
            }

        }

        #endregion

        /// <summary>
        /// XOR state with round key.
        /// FIPS-197 Section 5.1.4
        /// FIPS-197 Section 5.3.4 (Inverse is the same, bc it uses XOR)
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

        private void TraceRound(bool trace, int round, string label, byte[] data)
        {
            if (!trace || data == null) return;

            string roundStr = round < 10 ? $" {round}" : round.ToString();
            string hex = BitConverter.ToString(data).Replace("-", "").ToLower();
            Console.WriteLine($"round[{roundStr}].{label,-8}     {hex}");
        }

        private void TraceRound(bool trace, int round, string label, byte[,] state)
        {
            if (!trace || state == null) return;

            string roundStr = round < 10 ? $" {round}" : round.ToString(); // pad single digits
            string hex = StateToHex(state);
            Console.WriteLine($"round[{roundStr}].{label,-8}     {hex}");
        }

        //private void TraceState(bool trace, string label, byte[,] state)
        //{
        //    if (!trace || state == null) return;

        //    Console.WriteLine($"{label}     {StateToHex(state)}");
        //}


    }

}