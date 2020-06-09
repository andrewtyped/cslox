using lox.constants;
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
        public bool IsInitializer
        {
            get;
        }

        #region Fields

        private readonly Function declaration;

        private readonly Environment closure;

        private readonly string functionName;

        private static readonly Token thisToken = new Token(0,
                                                            4,
                                                            0,
                                                            TokenType.THIS);

        #endregion

        #region Constructors

        public LoxFunction(in ReadOnlySpan<char> source,
                           Function declaration,
                           Environment closure,
                           bool isInitializer)
        {
            if (declaration == null)
            {
                throw new ArgumentNullException(paramName: nameof(declaration));
            }

            this.IsInitializer = isInitializer;

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

        public LoxFunction Bind(LoxInstance instance,
                                in ReadOnlySpan<char> source)
        {
            Environment environment = new Environment(this.closure);
            environment.Define("this",
                               thisToken,
                               instance);
            return new LoxFunction(source,
                                   this.declaration,
                                   environment,
                                   this.IsInitializer);
        }

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

            if (this.IsInitializer)
            {
                return closure.GetAt("this",
                                     thisToken,
                                     0);
            }
            return null;
        }

        #endregion
    }
}