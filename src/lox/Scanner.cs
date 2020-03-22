using System;
using System.Collections.Generic;
using System.Text;

namespace lox
{
    /// <summary>
    /// Converts a string of Lox code to a list of tokens.
    /// </summary>
    public class Scanner
    {
        /// <summary>
        /// Gets the Lox code to tokenize.
        /// </summary>
        public string Source
        {
            get;
        }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="source">The Lox code to tokenize.</param>
        public Scanner(string source) => this.Source = source;
    }
}
