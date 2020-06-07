﻿using System;
using System.Collections.Generic;

namespace lox
{
    /// <summary>
    /// Represents a native lox class, its methods, and state.
    /// </summary>
    public class LoxClass : ILoxCallable
    {
        #region Constructors

        public LoxClass(string name)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        #endregion

        #region Instance Properties

        /// <summary>
        /// Get the name of the lox class.
        /// </summary>
        public string Name
        {
            get;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Returns the lox class's name.
        /// </summary>
        public override string ToString()
        {
            return this.Name;
        }

        public int Arity()
        {
            return 0;
        }

        public object? Call(Interpreter interpreter,
                            List<object?> arguments,
                            in ReadOnlySpan<char> source)
        {
            LoxInstance instance = new LoxInstance(this);
            return instance;
        }

        #endregion
    }
}