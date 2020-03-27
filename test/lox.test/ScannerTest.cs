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

        [TestMethod]
        public void ScannerHandlesOperators()
        {
            var source = "=!<>!===<=>=";

            this.AssertTokenSequence(source,
                                     (TokenType.EQUAL, "=", 1),
                                     (TokenType.BANG, "!", 1),
                                     (TokenType.LESS, "<", 1),
                                     (TokenType.GREATER, ">", 1),
                                     (TokenType.BANG_EQUAL, "!=", 1),
                                     (TokenType.EQUAL_EQUAL, "==", 1),
                                     (TokenType.LESS_EQUAL, "<=", 1),
                                     (TokenType.GREATER_EQUAL, ">=", 1));
        }

        [DataTestMethod]
        [DataRow(0,
                 TokenType.LEFT_PAREN,
                 "(",
                 1)]
        [DataRow(1,
                 TokenType.RIGHT_PAREN,
                 ")",
                 1)]
        [DataRow(2,
                 TokenType.LEFT_BRACE,
                 "{",
                 1)]
        [DataRow(3,
                 TokenType.RIGHT_BRACE,
                 "}",
                 1)]
        [DataRow(4,
                 TokenType.COMMA,
                 ",",
                 1)]
        [DataRow(5,
                 TokenType.DOT,
                 ".",
                 1)]
        [DataRow(6,
                 TokenType.MINUS,
                 "-",
                 1)]
        [DataRow(7,
                 TokenType.PLUS,
                 "+",
                 1)]
        [DataRow(8,
                 TokenType.SEMICOLON,
                 ";",
                 1)]
        [DataRow(9,
                 TokenType.STAR,
                 "*",
                 1)]
        public void ScannerHandlesSingleTokenCharacters(int tokenIndex,
                                                        TokenType expectedToken,
                                                        string expectedLexeme,
                                                        int expectedLine)
        {
            var source = "(){},.-+;*";
            var scanner = new Scanner(source);

            var tokens = scanner.ScanTokens();

            this.AssertTokenEquality(expectedToken,
                                     expectedLexeme,
                                     expectedLine,
                                     tokens[tokenIndex],
                                     source);
        }

        [TestMethod]
        public void ScannerHandlesUnrecognizedCharacters()
        {
            var source = "###";
            var scanner = new Scanner(source);
            var tokens = scanner.ScanTokens();

            Assert.AreEqual(1,
                            tokens.Count);
            this.AssertTokenEquality(TokenType.EOF,
                                     tokens[0]);
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
                            actualToken.GetLexeme(source)
                                       .ToString());
            Assert.AreEqual(expectedLiteral,
                            actualToken.Literal);
            Assert.AreEqual(expectedLine,
                            actualToken.Line);
        }

        private void AssertTokenSequence(string source,
                                         params (TokenType type, string lexeme, int line)[] expectedTokens)
        {
            var scanner = new Scanner(source);
            var tokens = scanner.ScanTokens();

            Assert.AreEqual(expectedTokens.Length,
                            tokens.Count - 1, //Ignore EOF
                            "Tokens length");

            for (int i = 0;
                 i < expectedTokens.Length;
                 i++)
            {
                var token = tokens[i];
                var expectedToken = expectedTokens[i];

                this.AssertTokenEquality(expectedToken.type,
                                         expectedToken.lexeme,
                                         expectedToken.line,
                                         token,
                                         source);
            }
        }

        #endregion
    }
}