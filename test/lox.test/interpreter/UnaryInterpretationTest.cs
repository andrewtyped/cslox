using lox.constants;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test.interpreter
{
    [TestClass]
    public class UnaryInterpretationTest : InterpreterTestBase
    {
        [DataTestMethod]
        [DataRow("-42", "-42")]
        [DataRow("!true", "False")]
        [DataRow("!false", "True")]
        [DataRow("!42", "False")]
        [DataRow("!\"A string\"", "False")]
        [DataRow("!nil", "True")]
        public void CanInterpretUnaryExpressionsAppliedToLiterals(string source,
                                                                  string expectedValue)
        {
            this.Interpret(source);
            this.AssertPrints(expectedValue);
        }

        [DataTestMethod]
        [DataRow("-(((30)))", "-30")]
        [DataRow("!(true)", "False")]
        [DataRow("!((false))", "True")]
        public void CanInterpretUnaryExpressionsAppliedToGroups(string source,
                                                                string expectedValue)
        {
            this.Interpret(source);
            this.AssertPrints(expectedValue);
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