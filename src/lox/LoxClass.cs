using System;
using System.Collections.Generic;

namespace lox
{
    /// <summary>
    /// Represents a native lox class, its methods, and state.
    /// </summary>
    public class LoxClass : ILoxCallable
    {
        #region Constructors

        public LoxClass(string name,
                        LoxClass? superclass,
                        Dictionary<string, LoxFunction> methods)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Superclass = superclass;
            this.Methods = methods ?? throw new ArgumentNullException(nameof(methods));
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

        /// <summary>
        /// Get the (optional) superclass of this class.
        /// </summary>
        internal LoxClass? Superclass
        {
            get;
        }

        /// <summary>
        /// Get the methods defined on this class.
        /// </summary>
        internal IReadOnlyDictionary<string, LoxFunction> Methods
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
            LoxFunction? initializer = this.FindMethod("init");

            if(initializer != null)
            {
                return initializer.Arity();
            }

            return 0;
        }

        public object? Call(Interpreter interpreter,
                            List<object?> arguments,
                            in ReadOnlySpan<char> source)
        {
            LoxFunction? initializer = this.FindMethod("init");
            LoxInstance instance = new LoxInstance(this);

            if (initializer != null)
            {
                LoxFunction? boundInitializer = initializer.Bind(instance,
                                                                 source);
                boundInitializer.Call(interpreter,
                                      arguments,
                                      source);
            }

            return instance;
        }

        internal LoxFunction? FindMethod(string name)
        {
            this.Methods.TryGetValue(name,
                                     out LoxFunction? loxFunction);
            return loxFunction;
        }

        #endregion
    }
}