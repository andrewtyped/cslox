using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test.interpreter
{
    [TestClass]
    public class ExpressionStatementInterpretationTest : InterpreterTestBase
    {
        #region Instance Methods

        [TestMethod]
        public void CanInterpretMultipleExpressionStatements()
        {
            var source = @"
1 + 1;
""string"";
true == true;";

            var values = this.InterpretStmts(source);

            Assert.AreEqual(3,
                            values.Count);
            Assert.AreEqual(2d,
                            values[0]);
            Assert.AreEqual("string",
                            values[1]);
            Assert.AreEqual(true,
                            values[2]);
        }

        #endregion
    }
}