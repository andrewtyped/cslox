using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test.interpreter
{
    [TestClass]
    public class PrintStatementInterpretationTest : InterpreterTestBase
    {
        #region Instance Methods

        [TestMethod]
        public void CanInterpretPrintStatements()
        {
            var source = @"
print ""hello"";
print ""world"";";

            this.Interpret(source);

            this.AssertPrints("hello",
                              "world");
        }

        #endregion
    }
}