using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test
{
    [TestClass]
    public class ScannerTest
    {
        [TestMethod]
        public void CanConstructScannerWithSource()
        {
            var scanner = new Scanner("test");

            Assert.AreEqual("test",
                            scanner.Source);
        }

        [TestMethod]
        public void ScannerProducesEmptyTokenList()
        {
            var scanner = new Scanner("test");
            var tokens = scanner.ScanTokens();

            Assert.AreEqual(0,
                            tokens.Count);
        }
    }
}
