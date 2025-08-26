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

    }

}