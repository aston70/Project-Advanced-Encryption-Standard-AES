using AES_Project_Domain;

namespace AES_Project_UnitTests
{
    [TestClass]
    public sealed class AES_Cipher_Tests
    {

        /// <summary>
        /// Appendix C.1 Test Vector from FIPS-197
        /// </summary>
        [TestMethod]
        public void AES128_AppendixC_Vector()
        {
            string keyHex = "000102030405060708090a0b0c0d0e0f";
            string plaintextHex = "00112233445566778899aabbccddeeff";
            string expectedCipherHex = "69c4e0d86a7b0430d8cdb78070b4c55a";

            var aes = new AES_Cipher(keyHex, AesKeySize.AES128);

            byte[] actualCipher = aes.Cipher(plaintextHex);

            CollectionAssert.AreEqual(Utility.ToByteArray(expectedCipherHex), actualCipher);
        }

        /// <summary>
        /// Appendix C.2 Test Vector from FIPS-197
        /// </summary>
        [TestMethod]
        public void AES192_AppendixC_Vector()
        {
            string keyHex = "000102030405060708090a0b0c0d0e0f1011121314151617";
            string plaintextHex = "00112233445566778899aabbccddeeff";
            string expectedCipherHex = "dda97ca4864cdfe06eaf70a0ec0d7191";

            var aes = new AES_Cipher(keyHex, AesKeySize.AES192);
            byte[] actualCipher = aes.Cipher(plaintextHex);

            CollectionAssert.AreEqual(Utility.ToByteArray(expectedCipherHex), actualCipher);
        }

        /// <summary>
        /// Appendix C.3 Test Vector from FIPS-197
        /// </summary>
        [TestMethod]
        public void AES256_AppendixC_Vector()
        {
            string keyHex = "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f";
            string plaintextHex = "00112233445566778899aabbccddeeff";
            string expectedCipherHex = "8ea2b7ca516745bfeafc49904b496089";

            var aes = new AES_Cipher(keyHex, AesKeySize.AES256);
            byte[] actualCipher = aes.Cipher(plaintextHex);

            CollectionAssert.AreEqual(Utility.ToByteArray(expectedCipherHex), actualCipher);
        }

    }

}