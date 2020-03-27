using lox.constants;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test
{
    [TestClass]
    public class ScannerTest
    {
        #region Instance Methods

        [TestMethod]
        public void CanConstructScannerWithSource()
        {
            var scanner = new Scanner("test");

            Assert.AreEqual("test",
                            scanner.Source);
        }

        [TestMethod]
        public void ScannerDetectsEndOfFile()
        {
            var source = "";
            var scanner = new Scanner(source);
            var tokens = scanner.ScanTokens();

            Assert.AreEqual(1,
                            tokens.Count);

            this.AssertTokenEquality(TokenType.EOF,
                                     "",
                                     1,
                                     tokens[0],
                                     source);
        }

        private void AssertTokenEquality(TokenType expectedTokenType,
                                         Token actualToken)
        {
            Assert.AreEqual(expectedTokenType,
                            actualToken.Type);
        }

        private void AssertTokenEquality(TokenType expectedTokenType,
                                         string expectedLexeme,
                                         int expectedLine,
                                         Token actualToken,
                                         string source)
        {
            this.AssertTokenEquality(expectedTokenType,
                                     expectedLexeme,
                                     null,
                                     expectedLine,
                                     actualToken,
                                     source);
        }

        private void AssertTokenEquality(TokenType expectedTokenType,
                                         string expectedLexeme,
                                         object? expectedLiteral,
                                         int expectedLine,
                                         Token actualToken,
                                         string source)
        {
            Assert.AreEqual(expectedTokenType,
                            actualToken.Type);
            Assert.AreEqual(expectedLexeme,
                            actualToken.GetLexeme(source).ToString());
            Assert.AreEqual(expectedLiteral,
                            actualToken.Literal);
            Assert.AreEqual(expectedLine,
                            actualToken.Line);
        }

        #endregion
    }
}