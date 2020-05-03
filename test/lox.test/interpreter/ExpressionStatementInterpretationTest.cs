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

            this.Interpret(source);
            this.AssertPrints();
        }

        #endregion
    }
}