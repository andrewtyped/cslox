using System;
using System.Collections.Generic;

namespace lox.loxFunctions
{
    /// <summary>
    /// Native lox function returning the elapsed time since the Unix epoch in seconds.
    /// </summary>
    public class Clock : ILoxCallable
    {
        private readonly IDateTimeOffsetProvider dateTimeOffsetProvider;

        public Clock(IDateTimeOffsetProvider dateTimeOffsetProvider)
        {
            this.dateTimeOffsetProvider = dateTimeOffsetProvider ?? throw new ArgumentNullException(nameof(dateTimeOffsetProvider));
        }

        #region Instance Methods

        public override string ToString() => "<native fn>";

        public int Arity() => 0;

        public object? Call(Interpreter interpreter,
                            List<object?> arguments,
                            in ReadOnlySpan<char> source)
        {
            return this.dateTimeOffsetProvider.UtcNow()
                       .ToUnixTimeMilliseconds()
                   / 1000.0d;
        }

        #endregion
    }

    /// <summary>
    /// Exposes operations for getting the current time.
    /// </summary>
    public interface IDateTimeOffsetProvider
    {
        #region Instance Methods

        /// <summary>
        /// Gets the current time in UTC.
        /// </summary>
        DateTimeOffset UtcNow();

        #endregion
    }

    /// <summary>
    /// Default implementation of <see cref="IDateTimeOffsetProvider"/>
    /// which utilitizes <see cref="System.DateTimeOffset"/>.
    /// </summary>
    public class DateTimeOffsetProvider : IDateTimeOffsetProvider
    {
        #region Instance Methods

        public DateTimeOffset UtcNow() => DateTimeOffset.UtcNow;

        #endregion
    }
}