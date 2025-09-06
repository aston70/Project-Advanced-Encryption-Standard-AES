namespace AES_Project_Domain
{
    public class Utility
    {

        public static byte[] ToByteArray(string hex)
        {
            if (hex.Length % 2 != 0) {
                throw new ArgumentException("Hex string must have an even length", nameof(hex));
            }

            byte[] bytes = new byte[hex.Length / 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }

            return bytes;
        }

        public static string ToHexString(byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", "").ToLowerInvariant();
        }

        public static byte[][] GetRoundKeys(byte[] key, AesKeySize keySize)
        {
            byte[][] w = KeyExpansion.ExpandKey(key, keySize);

            (int Nk, int Nr) = AES_Parameters.GetNkAndNrFromKeySize(keySize);
            int Nb = 4; // AES block size in words

            byte[][] roundKeys = new byte[Nr + 1][];
            for (int round = 0; round <= Nr; round++)
            {
                roundKeys[round] = new byte[16];
                for (int col = 0; col < Nb; col++)
                {
                    byte[] word = w[round * Nb + col];
                    for (int row = 0; row < Nb; row++)
                        roundKeys[round][col * Nb + row] = word[row];
                }
            }

            return roundKeys;
        }

    }

}