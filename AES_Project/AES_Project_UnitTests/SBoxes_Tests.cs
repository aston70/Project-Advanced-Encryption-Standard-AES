using AES_Project_Domain;

namespace AES_Project_UnitTests
{

    /// <summary>
    /// Tests for AES S-boxes. Just simple tests like making sure byte arrays are the correct length, etc.
    /// </summary>
    [TestClass]
    public class SBoxes_Tests
    {

        [TestMethod]
        public void SBox_Length_Is256()
        {
            const int expectedLength = 256;
            int actualLength = SBoxes.SBox.Length;
            Assert.AreEqual(expectedLength, actualLength);
        }

        [TestMethod]
        public void InvSBox_Length_Is256()
        {
            const int expectedLength = 256;
            int actualLength = SBoxes.InverseSBox.Length;
            Assert.AreEqual(expectedLength, actualLength);
        }

        [TestMethod]
        public void SBox_Known_Values()
        {
            Assert.AreEqual(0x63, SBoxes.SBox[0x00]);
            Assert.AreEqual(0x7c, SBoxes.SBox[0x01]);
            Assert.AreEqual(0x16, SBoxes.SBox[0xff]);
        }

        [TestMethod]
        public void InvSBox_Known_Values()
        {
            Assert.AreEqual(0x52, SBoxes.InverseSBox[0x00]);
            Assert.AreEqual(0x09, SBoxes.InverseSBox[0x01]);
            Assert.AreEqual(0x7d, SBoxes.InverseSBox[0xff]);
        }

    }

}