using System;

namespace lox
{
    /// <summary>
    /// Used to indicate a syntax error discovered during parsing.
    /// </summary>
    public class ParseError : Exception
    {
        #region Constructors

        public ParseError()
        {
        }

        public ParseError(string message)
            : base(message)
        {
        }

        public ParseError(string message,
                          Exception innerException)
            : base(message,
                   innerException)
        {
        }

        #endregion
    }
}