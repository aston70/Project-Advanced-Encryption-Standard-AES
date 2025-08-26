using AES_Project_Domain;

namespace AES_Project_UnitTests
{

    [TestClass]
    public class KeyExpansion_Tests
    {

        [TestMethod]
        public void RotWord_ShouldRotateLeft()
        {
            byte[] input = { 0x01, 0x02, 0x03, 0x04 };
            byte[] expected = { 0x02, 0x03, 0x04, 0x01 };

            byte[] actual = KeyExpansion.RotWord(input);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SubWord_ShouldSubstituteBytes()
        {
            byte[] input = { 0x00, 0x01, 0x02, 0x03 };
            byte[] expected = {
                SBoxes.SBox[0x00],
                SBoxes.SBox[0x01],
                SBoxes.SBox[0x02],
                SBoxes.SBox[0x03]
            };

            byte[] actual = KeyExpansion.SubWord(input);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ExpandKey_AES128_ShouldReturnCorrectLength()
        {
            byte[] key128 = {
                0x2b,0x7e,0x15,0x16,
                0x28,0xae,0xd2,0xa6,
                0xab,0xf7,0x15,0x88,
                0x09,0xcf,0x4f,0x3c
            };

            byte[][] expanded = KeyExpansion.ExpandKey(key128, AesKeySize.AES128);

            // AES-128: 4 columns * (10 rounds + 1) = 44 words
            Assert.AreEqual(44, expanded.Length);

            // Each word should be 4 bytes
            foreach (var word in expanded)
            {
                Assert.AreEqual(4, word.Length);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExpandKey_InvalidKeyLength_ShouldThrow()
        {
            byte[] badKey = { 0x00, 0x01, 0x02 }; // too short
            KeyExpansion.ExpandKey(badKey, AesKeySize.AES128);
        }

    }
}
