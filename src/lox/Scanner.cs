using System.Collections.Generic;
using static lox.constants.TokenType;

namespace lox
{
    /// <summary>
    /// Converts a string of Lox code to a list of tokens.
    /// </summary>
    public class Scanner
    {
        #region Fields

        private readonly List<Token> tokens = new List<Token>();

        private int current = 0;

        private int line = 1;

        private int start;

        #endregion

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
            if (this.tokens.Count > 0)
            {
                return this.tokens;
            }

            while (!this.IsAtEnd())
            {
                //TODO Section 4.4
                this.start = this.current;
                this.ScanToken();
            }

            tokens.Add(new Token("",
                                 1,
                                 EOF));

            return this.tokens;
        }

        private void ScanToken()
        {
            this.current++;
        }

        private bool IsAtEnd() => this.current >= this.Source.Length;

        #endregion
    }
}