using System;

namespace lox
{
    /// <summary>
    /// Stores scanned tokens alongside their source string for easy passage through
    /// the parser and easy lexeme retrieval.
    /// </summary>
    public ref struct ScannedSource
    {
        #region Constructors

        public ScannedSource(ReadOnlySpan<Token> tokens,
                             ReadOnlySpan<char> source)
        {
            this.Tokens = tokens;
            this.Source = source;
        }

        #endregion

        #region Instance Properties

        /// <summary>
        /// Get the fully sscanned source.
        /// </summary>
        public ReadOnlySpan<char> Source
        {
            get;
        }

        /// <summary>
        /// Get the tokens scanned from the source.
        /// </summary>
        public ReadOnlySpan<Token> Tokens
        {
            get;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Gets the lexeme for a token in <see cref="Tokens"/> from <see cref="Source"/>.
        /// </summary>
        /// <param name="token">The token to retrieve the lexeme for.</param>
        /// <returns>The lexeme representing <paramref name="token"/></returns>
        public ReadOnlySpan<char> GetLexeme(in Token token)
        {
            return token.GetLexeme(this.Source);
        }

        #endregion
    }
}