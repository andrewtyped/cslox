using System;
using System.Collections.Generic;

namespace lox
{
    /// <summary>
    /// Represents a constructed instance of a lox class.
    /// </summary>
    public class LoxInstance
    {
        #region Fields

        private readonly Dictionary<string, object?> fields = new Dictionary<string, object?>();

        #endregion

        #region Constructors

        public LoxInstance(LoxClass @class)
        {
            this.Class = @class ?? throw new ArgumentNullException(nameof(@class));
        }

        #endregion

        #region Instance Properties

        /// <summary>
        /// Gets the class definition this instance implements.
        /// </summary>
        public LoxClass Class
        {
            get;
        }

        #endregion

        #region Instance Methods

        public override string ToString()
        {
            return $"{this.Class.Name} instance";
        }

        public object? Get(Token name,
                           in ReadOnlySpan<char> source)
        {
            string nameLexeme = name.GetLexeme(source)
                                    .ToString();

            if (this.fields.TryGetValue(nameLexeme,
                                        out object? value))
            {
                return value;
            }

            throw new RuntimeError(name,
                                   $"Undefined property {nameLexeme} on object {this.Class.Name}");
        }

        #endregion
    }
}