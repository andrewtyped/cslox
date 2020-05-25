using System;
using System.Collections.Generic;

namespace lox
{
    /// <summary>
    /// Represents an expression that can be invoked as a function in lox.
    /// </summary>
    public interface ILoxCallable
    {
        #region Instance Methods

        int Arity();

        object? Call(Interpreter interpreter,
                     List<object?> arguments,
                     in ReadOnlySpan<char> source);

        #endregion
    }
}