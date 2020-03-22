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
    }
}
