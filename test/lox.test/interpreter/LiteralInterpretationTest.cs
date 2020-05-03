using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test.interpreter
{
    [TestClass]
    public class LiteralInterpretationTest : InterpreterTestBase
    {
        #region Instance Methods

        [DataTestMethod]
        [DataRow("\"Test\"",
                 "Test")]
        [DataRow("1",
                 "1")]
        [DataRow("nil",
                 "nil")]
        public void CanInterpretLiterals(string source,
                                         string expectedValue)
        {
            this.Interpret(source);
            this.AssertPrints(expectedValue);
        }

        #endregion
    }
}