using lox.constants;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using static lox.constants.TokenType;

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

            this.AssertTokenEquality(EOF,
                                     "",
                                     1,
                                     tokens[0],
                                     source);
        }

        [TestMethod]
        public void ScannerHandlesComments()
        {
            var source = "//This is a comment and symbols to not parse / * ( )";
            var scanner = new Scanner(source);
            var tokens = scanner.ScanTokens();

            Assert.AreEqual(1,
                            tokens.Count);
            this.AssertTokenEquality(EOF,
                                     tokens[0]);
        }

        [TestMethod]
        public void ScannerHandlesIntegerNumbers()
        {
            var source = "012 34 6789";
            this.AssertLiteralTokenSequence(source,
                                            (NUMBER, "012", 12d, 1),
                                            (NUMBER, "34", 34d, 1),
                                            (NUMBER, "6789", 6789d, 1));
        }

        [TestMethod]
        public void ScannerHandlesMultilineString()
        {
            var source = "\"This is a string.\n It uses two lines.\"";

            this.AssertLiteralTokenSequence(source,
                                            (STRING, "\"This is a string.\n It uses two lines.\"", "This is a string.\n It uses two lines.", 2));
        }

        [TestMethod]
        public void ScannerHandlesOperators()
        {
            var source = "=!<>!===<=>=/";

            this.AssertTokenSequence(source,
                                     (EQUAL, "=", 1),
                                     (BANG, "!", 1),
                                     (LESS, "<", 1),
                                     (GREATER, ">", 1),
                                     (BANG_EQUAL, "!=", 1),
                                     (EQUAL_EQUAL, "==", 1),
                                     (LESS_EQUAL, "<=", 1),
                                     (GREATER_EQUAL, ">=", 1),
                                     (SLASH, "/", 1));
        }

        [TestMethod]
        public void ScannerHandlesRealNumbers()
        {
            var source = "012.12 34.4598 6789.1";
            this.AssertLiteralTokenSequence(source,
                                            (NUMBER, "012.12", 12.12d, 1),
                                            (NUMBER, "34.4598", 34.4598d, 1),
                                            (NUMBER, "6789.1", 6789.1d, 1));
        }

        [TestMethod]
        public void ScannerHandlesSingleTokenCharacters()
        {
            var source = "(){},.-+;*";

            this.AssertTokenSequence(source,
                                     (LEFT_PAREN, "(", 1),
                                     (RIGHT_PAREN, ")", 1),
                                     (LEFT_BRACE, "{", 1),
                                     (RIGHT_BRACE, "}", 1),
                                     (COMMA, ",", 1),
                                     (DOT, ".", 1),
                                     (MINUS, "-", 1),
                                     (PLUS, "+", 1),
                                     (SEMICOLON, ";", 1),
                                     (STAR, "*", 1));
        }

        [TestMethod]
        public void ScannerHandlesStrings()
        {
            var source = "\"This is a string\"\"Another string.\"";

            this.AssertLiteralTokenSequence(source,
                                            (STRING, "\"This is a string\"", "This is a string", 1),
                                            (STRING, "\"Another string.\"", "Another string.", 1));
        }

        [TestMethod]
        public void ScannerHandlesUnrecognizedCharacters()
        {
            var source = "###";
            var scanner = new Scanner(source);
            var tokens = scanner.ScanTokens();

            Assert.AreEqual(1,
                            tokens.Count);
            this.AssertTokenEquality(EOF,
                                     tokens[0]);
        }

        [TestMethod]
        public void ScannerHandlesWhitespace()
        {
            var source = "  \r\n\n\t\t  ";
            var scanner = new Scanner(source);
            var tokens = scanner.ScanTokens();

            Assert.AreEqual(1,
                            tokens.Count);
            this.AssertTokenEquality(EOF,
                                     "",
                                     3,
                                     tokens[0],
                                     source);
        }

        private void AssertLiteralTokenSequence(string source,
                                                params (TokenType type, string lexeme, object literal, int line)[] expectedTokens)
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
                                         expectedToken.literal,
                                         expectedToken.line,
                                         token,
                                         source);
            }
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