namespace AES_Project_Domain
{

    public class AES_Parameters
    {

        /// <summary>
        /// Returns as touple the number of transformation rounds (Nr) and the number of 32-bit words
        /// in the cipher key (Nk) based on the AES key size:
        /// - AES-128: Nk = 4, Nr = 10
        /// - AES-192: Nk = 6, Nr = 12
        /// - AES-256: Nk = 8, Nr = 14
        /// </summary>
        /// <param name="keySize"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static (int Nk, int Nr) GetNkAndNrFromKeySize(AesKeySize keySize)
        {
            return keySize switch
            {
                AesKeySize.AES128 => (4, 10),
                AesKeySize.AES192 => (6, 12),
                AesKeySize.AES256 => (8, 14),
                _ => throw new ArgumentOutOfRangeException(nameof(keySize), "Unsupported AES key size")
            };
        }

        public static string GetDisplayString(AesKeySize keySize)
        {
            string str = keySize.ToString();
            // Insert dash between "AES" and number
            if (str.StartsWith("AES") && str.Length > 3)
                str = str.Insert(3, "-");
            return str;
        }

        public static string GetCipherType(AesKeySize keySize)
        {
            return keySize switch
            {
                AesKeySize.AES128 => "C.1",
                AesKeySize.AES192 => "C.2",
                AesKeySize.AES256 => "C.3",
                _ => throw new ArgumentOutOfRangeException(nameof(keySize), "Unsupported AES key size")
            };
        }

    }

}