using System;
using System.Collections.Generic;
using System.Text;

namespace lox
{
    /// <summary>
    /// Represents a value returned from a Lox function. Implemented as an exception to conveniently unwind
    /// the call stack to the function's call site.
    /// </summary>
    public class ReturnValue : Exception
    {
        public ReturnValue(object? value)
        {
            this.Value = value;
        }

        public object? Value
        {
            get;
        }
    }
}