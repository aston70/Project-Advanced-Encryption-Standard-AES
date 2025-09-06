using AES_Project_Domain;
using System.Security.Cryptography;

namespace AES_Project
{

    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("AES Implementation (FIPS-197)");
            //Console.WriteLine();

            if (args.Length == 0)
            {
                // Run built-in Appendix C test vectors
                RunCipherAppendixCTests();
                RunInvCipherAppendixCTests();
                return;
            }

            // Otherwise, handle args
            if (args.Length < 3 || args.Length > 4)
            {
                Console.WriteLine("Usage: AES_Project_Console <AES128|AES192|AES256> <keyHexValue> <plaintextHexValue> [encrypt|decrypt]");
                Console.WriteLine("default mode: encrypt");
                return;
            }

            string keySizeStr = args[0];            
            string inputHex = args[1];
            string keyHex = args[2];
            string mode = args.Length == 4 ? args[3].ToLower() : "encrypt"; // default encrypt

            if (!Enum.TryParse<AesKeySize>(keySizeStr, true, out var keySize))
            {
                Console.WriteLine("Invalid key size. Use AES128, AES192, or AES256.");
                return;
            }

            try
            {
                OutputHeader(keySize, inputHex, keyHex);

                var aes = new AES_Cipher(keyHex, keySize);

                byte[] output;

                if (mode == "encrypt") {                    
                    output = aes.Cipher(inputHex, trace: true);
                    Console.WriteLine();
                    output = aes.InvCipher(output, trace: true);
                }
                else if (mode == "decrypt") {                    
                    output = aes.InvCipher(inputHex, trace: true);
                }
                else
                {
                    Console.WriteLine("Invalid mode. Use 'encrypt' or 'decrypt'.");
                    return;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

#if DEBUG
            Console.WriteLine("Paused in debug mode. Press Enter to continue...");
            Console.ReadLine();
#endif

        }

        static void OutputHeader(AesKeySize keySize, string plainText, string key)
        {
            var (Nk, Nr) = AES_Parameters.GetNkAndNrFromKeySize(keySize);
            string cipherType = AES_Parameters.GetCipherType(keySize);
            string displayKeySize = AES_Parameters.GetDisplayString(keySize);

            Console.WriteLine($"{cipherType}   {displayKeySize} (Nk={Nk}, Nr={Nr})");
            Console.WriteLine();
            Console.WriteLine($"PLAINTEXT:          {plainText}");
            Console.WriteLine($"KEY:                {key}");
            Console.WriteLine();
        }


        static void RunCipherAppendixCTests()
        {
            Console.WriteLine("Running AES Appendix C Cipher Test Vectors.");
            RunCipherTest(
                "AES-128",
                "000102030405060708090a0b0c0d0e0f", // key
                "00112233445566778899aabbccddeeff", // ciphertext input
                "69c4e0d86a7b0430d8cdb78070b4c55a", // expected plaintext
                AesKeySize.AES128
            );

            RunCipherTest(
                "AES-192",
                "000102030405060708090a0b0c0d0e0f1011121314151617",
                "00112233445566778899aabbccddeeff",
                "dda97ca4864cdfe06eaf70a0ec0d7191",
                AesKeySize.AES192
            );

            RunCipherTest(
                "AES-256",
                "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f",
                "00112233445566778899aabbccddeeff",
                "8ea2b7ca516745bfeafc49904b496089",
                AesKeySize.AES256
            );
        }

        static void RunInvCipherAppendixCTests()
        {
            Console.WriteLine("Running AES Appendix C Inverse Cipher Test Vectors.");

            RunInvCipherTest(
                "AES-128",
                "000102030405060708090a0b0c0d0e0f",  // key
                "69c4e0d86a7b0430d8cdb78070b4c55a",  // ciphertext input
                "00112233445566778899aabbccddeeff",  // expected plaintext
                AesKeySize.AES128
            );

            RunInvCipherTest(
                "AES-192",
                "000102030405060708090a0b0c0d0e0f1011121314151617",
                "dda97ca4864cdfe06eaf70a0ec0d7191",
                "00112233445566778899aabbccddeeff",
                AesKeySize.AES192
            );

            RunInvCipherTest(
                "AES-256",
                "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f",
                "8ea2b7ca516745bfeafc49904b496089",
                "00112233445566778899aabbccddeeff",
                AesKeySize.AES256
            );
        }


        static void RunCipherTest(string name, string keyHex, string plaintextHex, string expectedCipherHex, AesKeySize keySize)
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

        static void RunInvCipherTest(string name, string keyHex, string plaintextHex, string expectedCipherHex, AesKeySize keySize)
        {
            var aes = new AES_Cipher(keyHex, keySize);
            byte[] actual = aes.InvCipher(plaintextHex);

            string actualHex = BitConverter.ToString(actual).Replace("-", "");

            Console.WriteLine($"{name} Test:");
            Console.WriteLine($"  Key:       {keyHex}");
            Console.WriteLine($"  Plaintext: {plaintextHex}");
            Console.WriteLine($"  Output:    {actualHex}");
            Console.WriteLine($"  Expected:  {expectedCipherHex}");
            Console.WriteLine($"  Result:    {(string.Equals(actualHex, expectedCipherHex, StringComparison.OrdinalIgnoreCase) ? "PASS" : "FAIL")}");
            Console.WriteLine();
        }

    }

}