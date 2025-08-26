using AES_Project_Domain;

namespace AES_Project
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("AES Implementation (FIPS-197)");
            Console.WriteLine();

            if (args.Length == 0)
            {
                // Run built-in Appendix C test vectors
                RunAppendixCTests();
                return;
            }

            // Otherwise, handle args
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: AES_Project_Console <AES128|AES192|AES256> <keyHex> <plaintextHex>");
                return;
            }

            string keySizeStr = args[0];
            string keyHex = args[1];
            string plaintextHex = args[2];
            
            if (!Enum.TryParse<AesKeySize>(keySizeStr, true, out var keySize))
            {
                Console.WriteLine("Invalid key size. Use AES128, AES192, or AES256.");
                return;
            }

            try
            {
                var aes = new AES_Cipher(keyHex, keySize);
                byte[] ciphertext = aes.Cipher(plaintextHex);

                Console.WriteLine("Key:       " + keyHex);
                Console.WriteLine("Plaintext: " + plaintextHex);
                Console.WriteLine("Cipher:    " + BitConverter.ToString(ciphertext).Replace("-", "").ToLower());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

        }

        static void RunAppendixCTests()
        {
            RunTest(
                "AES-128",
                "000102030405060708090a0b0c0d0e0f",
                "00112233445566778899aabbccddeeff",
                "69c4e0d86a7b0430d8cdb78070b4c55a",
                AesKeySize.AES128
            );

            RunTest(
                "AES-192",
                "000102030405060708090a0b0c0d0e0f1011121314151617",
                "00112233445566778899aabbccddeeff",
                "dda97ca4864cdfe06eaf70a0ec0d7191",
                AesKeySize.AES192
            );

            RunTest(
                "AES-256",
                "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f",
                "00112233445566778899aabbccddeeff",
                "8ea2b7ca516745bfeafc49904b496089",
                AesKeySize.AES256
            );
        }

        static void RunTest(string name, string keyHex, string plaintextHex, string expectedCipherHex, AesKeySize keySize)
        {
            var aes = new AES_Cipher(keyHex, keySize);
            byte[] actual = aes.Cipher(plaintextHex);

            string actualHex = BitConverter.ToString(actual).Replace("-", "");

            Console.WriteLine($"{name} Test:");
            Console.WriteLine($"  Key:       {keyHex}");
            Console.WriteLine($"  Plaintext: {plaintextHex}");
            Console.WriteLine($"  Cipher:    {actualHex}");
            Console.WriteLine($"  Expected:  {expectedCipherHex}");
            Console.WriteLine($"  Result:    {(string.Equals(actualHex, expectedCipherHex, StringComparison.OrdinalIgnoreCase) ? "PASS" : "FAIL")}");
            Console.WriteLine();
        }

    }

}