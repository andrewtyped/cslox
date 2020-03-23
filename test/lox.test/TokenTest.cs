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
            var token = new Token("for",
                                  1,
                                  FOR);

            Assert.AreEqual("for",
                            token.Lexeme);
            Assert.AreEqual(1,
                            token.Line);
            Assert.AreEqual(FOR,
                            token.Type);
        }

        [TestMethod]
        public void CanConstructLiteralToken()
        {
            var token = new Token("My string",
                                  2,
                                  "My string",
                                  STRING);

            Assert.AreEqual("My string",
                            token.Lexeme);
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