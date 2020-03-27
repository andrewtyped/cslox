using System;

using lox.constants;

namespace lox
{
    /// <summary>
    /// Represents a unit of Lox syntax.
    /// </summary>
    public struct Token
    {
        #region Constructors

        ///// <summary>
        ///// Create a non-literal token.
        ///// </summary>
        ///// <param name="lexeme">The text of the token.</param>
        ///// <param name="line">The line on which the token appears.</param>
        ///// <param name="type">The type of token represented by the lexeme.</param>
        //public Token(ReadOnlySpan<char> lexeme,
        //             int line,
        //             TokenType type)
        //{
        //    this.Lexeme = lexeme;
        //    this.Line = line;
        //    this.Type = type;
        //    this.Literal = null;
        //}

        ///// <summary>
        ///// Create a literal token.
        ///// </summary>
        ///// <param name="lexeme">The text of the token.</param>
        ///// <param name="line">The line on which the token appears.</param>
        ///// <param name="literal">The literal value of the token (string, number, identifier).</param>
        ///// <param name="type">The type of token represented by the lexeme.</param>
        //public Token(ReadOnlySpan<char> lexeme,
        //             int line,
        //             object literal,
        //             TokenType type)
        //{
        //    this.Lexeme = lexeme;
        //    this.Line = line;
        //    this.Literal = literal;
        //    this.Type = type;
        //}

        /// <summary>
        /// Create a non-literal token.
        /// </summary>
        /// <param name="lexemeStart">The start character index of the lexeme text in the source code</param>
        /// <param name="lexemeEnd">The end character index of the lexeme text in the source code.</param>
        /// <param name="line">The line on which the token appears.</param>
        /// <param name="type">The type of token represented by the lexeme.</param>
        public Token(int lexemeStart,
                     int lexemeEnd,
                     int line,
                     TokenType type)
        {
            this.LexemeStart = lexemeStart;
            this.LexemeEnd = lexemeEnd;
            this.Line = line;
            this.Type = type;
            this.Literal = null;
        }

        /// <summary>
        /// Create a literal token.
        /// </summary>
        /// <param name="lexemeStart">The start character index of the lexeme text in the source code</param>
        /// <param name="lexemeEnd">The end character index of the lexeme text in the source code.</param>
        /// <param name="line">The line on which the token appears.</param>
        /// <param name="literal">The literal value of the token (string, number, identifier).</param>
        /// <param name="type">The type of token represented by the lexeme.</param>
        public Token(int lexemeStart,
                     int lexemeEnd,
                     int line,
                     object literal,
                     TokenType type)
        {
            this.LexemeStart = lexemeStart;
            this.LexemeEnd = lexemeEnd;
            this.Line = line;
            this.Literal = literal;
            this.Type = type;
        }

        #endregion

        #region Instance Properties

        /// <summary>
        /// Get the end character index of the lexeme text in source code.
        /// </summary>
        public int LexemeEnd
        {
            get;
        }

        ///// <summary>
        ///// Get the text of the token as it appeared in source code.
       // /// </summary>
        //public ReadOnlySpan<char> Lexeme
        //{
        //get;
        //}

        /// <summary>
        /// Get the start index of the lexeme text in source code.
        /// </summary>
        public int LexemeStart
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

        #region Instance Methods

        public ReadOnlySpan<char> GetLexeme(in ReadOnlySpan<char> source)
        {
            return source.Slice(this.LexemeStart,
                                this.LexemeEnd - this.LexemeStart);
        }

        public string ToString(in ReadOnlySpan<char> source)
        {
            return $"{this.Type} {this.GetLexeme(source).ToString()} {this.Literal ?? ""}";
        }

        #endregion
    }
}