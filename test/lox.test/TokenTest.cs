using Microsoft.VisualStudio.TestTools.UnitTesting;

using static lox.constants.TokenType;

namespace lox.test
{
    [TestClass]
    public class TokenTest
    {
        #region Instance Methods

        [TestMethod]
        public void CanConstructNonLiteralToken()
        {
            var token = new Token(1,
                                  3,
                                  1,
                                  FOR);

            Assert.AreEqual(1,
                            token.LexemeStart);
            Assert.AreEqual(3,
                            token.LexemeEnd);
            Assert.AreEqual(1,
                            token.Line);
            Assert.AreEqual(FOR,
                            token.Type);
        }

        [TestMethod]
        public void CanGetLexemeFromTokenWithSource()
        {
            var token = new Token(20,
                                  23,
                                  1,
                                  FOR);

            Assert.AreEqual("for",
                            token.GetLexeme("all the wine is all for me")
                                 .ToString());
        }

        [TestMethod]
        public void CanConstructLiteralToken()
        {
            var token = new Token(1,
                                  10,
                                  2,
                                  "My string",
                                  STRING);

            Assert.AreEqual(1,
                            token.LexemeStart);
            Assert.AreEqual(10,
                            token.LexemeEnd);
            Assert.AreEqual(2,
                            token.Line);
            Assert.AreEqual("My string",
                            token.Literal);
            Assert.AreEqual(STRING,
                            token.Type);
        }

        #endregion
    }
}