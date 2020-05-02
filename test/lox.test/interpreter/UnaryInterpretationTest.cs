using lox.constants;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test.interpreter
{
    [TestClass]
    public class UnaryInterpretationTest : InterpreterTestBase
    {
        [DataTestMethod]
        [DataRow("-42", -42d)]
        [DataRow("!true", false)]
        [DataRow("!false", true)]
        [DataRow("!42", false)]
        [DataRow("!\"A string\"", false)]
        [DataRow("!nil", true)]
        public void CanInterpretUnaryExpressionsAppliedToLiterals(string source,
                                                                  object expectedValue)
        {
            var value = this.Interpret(source);
            Assert.AreEqual(expectedValue,
                            value);
        }

        [DataTestMethod]
        [DataRow("-(((30)))", -30d)]
        [DataRow("!(true)", false)]
        [DataRow("!((false))", true)]
        public void CanInterpretUnaryExpressionsAppliedToGroups(string source,
                                                                object expectedValue)
        {
            var value = this.Interpret(source);
            Assert.AreEqual(expectedValue,
                            value);
        }

        [DataTestMethod]
        [DataRow("-true")]
        [DataRow("-false")]
        [DataRow("-\"1\"")]
        public void CanThrowRuntimeErrorForInvalidUnaryNegation(string source)
        {
            this.AssertRuntimeError(source,
                                    TokenType.MINUS,
                                    "Operand must be a number.");
        }

    }
}