using System.Collections.Generic;

namespace lox
{
    /// <summary>
    /// Converts a string of Lox code to a list of tokens.
    /// </summary>
    public class Scanner
    {
        #region Constructors

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="source">The Lox code to tokenize.</param>
        public Scanner(string source) => this.Source = source;

        #endregion

        #region Instance Properties

        /// <summary>
        /// Gets the Lox code to tokenize.
        /// </summary>
        public string Source
        {
            get;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Reads the content of <see cref="Source"/> to produce a list of Lox tokens.
        /// </summary>
        /// <returns>The list of tokens read from <see cref="Source"/></returns>
        public IReadOnlyList<Token> ScanTokens()
        {
            var tokens = new List<Token>();

            return tokens;
        }

        #endregion
    }
}