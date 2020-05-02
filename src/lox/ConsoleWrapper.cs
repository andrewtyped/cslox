using System;

namespace lox
{
    /// <summary>
    /// Abstracts the console for testability.
    /// </summary>
    public interface IConsole
    {
        #region Instance Methods

        void Write(string text);

        void WriteLine(string text);

        #endregion
    }

    /// <summary>
    /// Passes through to <see cref="Console"/>
    /// </summary>
    public class ConsoleWrapper : IConsole
    {
        #region Instance Methods

        public void Write(string text)
        {
            Console.Write(text);
        }

        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        #endregion
    }
}