using System;

namespace lox
{
    /// <summary>
    /// Represents a lox error that occurs while interpreting an expression.
    /// </summary>
    public class RuntimeError : Exception
    {
        #region Constructors

        public RuntimeError(Token token,
                            string message)
            : base(message)
        {
            this.Token = token;
        }

        #endregion

        #region Instance Properties

        /// <summary>
        /// Get the token at which the error occurred.
        /// </summary>
        public Token Token
        {
            get;
        }

        #endregion
    }
}