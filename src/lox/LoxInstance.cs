using System;

namespace lox
{
    /// <summary>
    /// Represents a constructed instance of a lox class.
    /// </summary>
    public class LoxInstance
    {
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

        #endregion
    }
}