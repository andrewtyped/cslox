using System;
using System.Collections.Generic;

namespace lox
{
    /// <summary>
    /// Represents a container for Lox variables and their values during runtime.
    /// </summary>
    public class Environment
    {
        #region Fields

        private readonly Dictionary<string, object?> values = new Dictionary<string, object?>();

        #endregion

        #region Instance Methods

        public void Define(in ReadOnlySpan<char> source,
                           Token token,
                           object? value)
        {
            this.values[token.GetLexeme(source)
                             .ToString()] = value;
        }

        public object? Get(in ReadOnlySpan<char> source,
                           Token token)
        {
            if (this.values.TryGetValue(token.GetLexeme(source)
                                             .ToString(),
                                        out object? value))
            {
                return value;
            }

            throw new RuntimeError(token,
                                   $"Undefined variable '{token.GetLexeme(source).ToString()}'");
        }

        /// <summary>
        /// For test purposes only. Get a variable's value by referencing its name directly.
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        internal object? Get(string variable)
        {
            if (this.values.TryGetValue(variable,
                                        out object? value))
            {
                return value;
            }

            throw new ArgumentException($"Undefined variable {variable}");
        }

        #endregion
    }
}