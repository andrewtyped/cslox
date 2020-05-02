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

            var values = this.InterpretStmts(source);

            Assert.AreEqual(0,
                            values.Count);
            Assert.AreEqual("hello",
                            this.Console.Writes[0]);
            Assert.AreEqual("world",
                            this.Console.Writes[1]);
        }

        #endregion
    }
}