using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test.interpreter
{
    [TestClass]
    public class LiteralInterpretationTest : InterpreterTestBase
    {
        #region Instance Methods

        [DataTestMethod]
        [DataRow("\"Test\"", "Test")]
        [DataRow("1", 1d)]
        [DataRow("nil", null)]
        public void CanInterpretLiterals(string source, object expectedValue)
        {
            var value = this.Interpret(source);
            Assert.AreEqual(expectedValue,
                            value);
        }

        #endregion
    }
}