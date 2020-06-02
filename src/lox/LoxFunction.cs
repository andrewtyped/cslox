using System;
using System.Collections.Generic;

using static lox.Stmt;

namespace lox
{
    /// <summary>
    /// Represents a user-defined lox function.
    /// </summary>
    public class LoxFunction : ILoxCallable
    {
        #region Fields

        private readonly Function declaration;

        private readonly Environment closure;

        private readonly string functionName;

        #endregion

        #region Constructors

        public LoxFunction(in ReadOnlySpan<char> source,
                           Function declaration,
                           Environment closure)
        {
            if (declaration == null)
            {
                throw new ArgumentNullException(paramName: nameof(declaration));
            }

            this.declaration = declaration;
            this.closure = closure ?? throw new ArgumentNullException(nameof(closure));

            this.functionName = declaration.name.GetLexeme(source)
                                           .ToString();
        }

        #endregion

        #region Instance Methods

        public override string ToString()
        {
            return $"<fn {this.functionName}>";
        }

        public int Arity() => this.declaration.parameters.Count;

        public object? Call(Interpreter interpreter,
                            List<object?> arguments,
                            in ReadOnlySpan<char> source)
        {
            Environment environment = new Environment(this.closure);

            for (int i = 0;
                 i < arguments.Count;
                 i++)
            {
                environment.Define(source,
                                   this.declaration.parameters[i],
                                   arguments[i]);
            }

            interpreter.ExecuteBlock(this.declaration.body,
                                     environment,
                                     source);

            return null;
        }

        #endregion
    }
}