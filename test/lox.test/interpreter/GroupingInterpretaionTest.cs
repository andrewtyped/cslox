using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test.interpreter
{
    [TestClass]
    public class GroupingInterpretaionTest : InterpreterTestBase
    {
        #region Instance Methods

        [DataTestMethod]
        [DataRow("(1)",
                 "1")]
        [DataRow("((42))",
                 "42")]
        [DataRow("((((((((100))))))))",
                 "100")]
        public void CanInterpretGroupedLiteralExpressions(string source,
                                                          string expectedValue)
        {
            this.Interpret(source);
            this.AssertPrints(expectedValue);
        }

        #endregion
    }
}