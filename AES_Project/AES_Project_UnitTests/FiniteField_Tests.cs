using AES_Project_Domain;

namespace AES_Project_UnitTests
{

    [TestClass]
    public class FiniteField_Tests
    {

        [TestMethod]
        public void Test_GFAdd()
        {
            // GF addition is XOR
            Assert.AreEqual(0x52, FiniteField.GF_Add(0x57, 0x05)); // example: 0x57 ^ 0x05 = 0x52
            Assert.AreEqual(0xFF, FiniteField.GF_Add(0xAA, 0x55)); // 0xAA ^ 0x55 = 0xFF
            Assert.AreEqual(0x00, FiniteField.GF_Add(0xFF, 0xFF)); // 0xFF ^ 0xFF = 0x00
        }

        [TestMethod]
        public void Test_GF_XTime()
        {
            Assert.AreEqual(0xAE, FiniteField.GF_XTime(0x57)); // FIPS-197 example                                                               
            Assert.AreEqual(0x01, FiniteField.GF_XTime(0x8D)); // MSB set, reduction applied: 0x8D << 1 = 0x1A, XOR 0x1B -> 0x01            
            Assert.AreEqual(0x2A, FiniteField.GF_XTime(0x15)); // MSB not set, just shift left: 0x15 << 1 = 0x2A
        }

        [TestMethod]
        public void Test_GF_Multiply()
        {
            Assert.AreEqual(0xFE, FiniteField.GF_Multiply(0x57, 0x13)); // FIPS-197 example
            Assert.AreEqual(0xC1, FiniteField.GF_Multiply(0x57, 0x83)); // FIPS-197 example
            Assert.AreEqual(0x00, FiniteField.GF_Multiply(0x57, 0x00)); // multiply by 0 = 0
            Assert.AreEqual(0x57, FiniteField.GF_Multiply(0x57, 0x01)); // multiply by 1 = original
        }

    }

}