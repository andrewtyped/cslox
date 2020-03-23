using lox.constants;

namespace lox
{
    /// <summary>
    /// Represents a unit of Lox syntax.
    /// </summary>
    public sealed class Token
    {
        #region Constructors

        /// <summary>
        /// Create a non-literal token.
        /// </summary>
        /// <param name="lexeme">The text of the token.</param>
        /// <param name="line">The line on which the token appears.</param>
        /// <param name="type">The type of token represented by the lexeme.</param>
        public Token(string lexeme,
                     int line,
                     TokenType type)
        {
            this.Lexeme = lexeme;
            this.Line = line;
            this.Type = type;
        }

        /// <summary>
        /// Create a literal token.
        /// </summary>
        /// <param name="lexeme">The text of the token.</param>
        /// <param name="line">The line on which the token appears.</param>
        /// <param name="literal">The literal value of the token (string, number, identifier).</param>
        /// <param name="type">The type of token represented by the lexeme.</param>
        public Token(string lexeme,
                     int line,
                     object literal,
                     TokenType type)
        {
            this.Lexeme = lexeme;
            this.Line = line;
            this.Literal = literal;
            this.Type = type;
        }

        #endregion

        #region Instance Properties

        /// <summary>
        /// Get the text of the token as it appeared in source code.
        /// </summary>
        public string Lexeme
        {
            get;
        }

        /// <summary>
        /// Get the line number in source code where the token appeared.
        /// </summary>
        public int Line
        {
            get;
        }

        /// <summary>
        /// Get the literal value of the token, if any. Null if the token type is not Number or String.
        /// </summary>
        public object? Literal
        {
            get;
        }

        /// <summary>
        /// Get the type code of the token.
        /// </summary>
        public TokenType Type
        {
            get;
        }

        #endregion
    }
}