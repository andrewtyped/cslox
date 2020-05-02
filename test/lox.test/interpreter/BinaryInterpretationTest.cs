using lox.constants;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test.interpreter
{
    [TestClass]
    public class BinaryInterpretationTest : InterpreterTestBase
    {
        [DataTestMethod]
        [DataRow("3 * 4", 12d)]
        [DataRow("9 / 6", 1.5d)]
        [DataRow("10 - 3", 7d)]
        [DataRow("23 + 51", 74d)]
        [DataRow("1 + 2 + 5.5", 8.5d)]
        [DataRow("2 + 2 * 4", 10d)]
        [DataRow("1 + 2 / 4 + 1", 2.5d)]
        [DataRow("(1 + 2) / (4 + 1)", 0.6d)]
        [DataRow("1 + nil", null)]
        [DataRow("nil + 1", null)]
        [DataRow("nil + nil", null)]
        public void CanInterpretArithmeticExpressions(string source,
                                                      object expectedValue)
        {
            this.AssertExpression(source,
                                  expectedValue);
        }

        [DataTestMethod]
        [DataRow("\"Hello\" + \" world\"",
                 "Hello world")]
        [DataRow("nil + \" world\"",
                 null)]
        [DataRow("\"Hello\" + nil",
                 null)]
        public void CanInterpretStringConcatenationExpressions(string source,
                                                               object expectedValue)
        {
            this.AssertExpression(source,
                                  expectedValue);
        }

        [DataTestMethod]
        [DataRow("1 > 2",
                 false)]
        [DataRow("4 > 3",
                 true)]
        [DataRow("4 > 4",
                 false)]
        [DataRow("1 < 2",
                 true)]
        [DataRow("4 < 3",
                 false)]
        [DataRow("4 < 4",
                 false)]
        [DataRow("1 >= 2",
                 false)]
        [DataRow("4 >= 3",
                 true)]
        [DataRow("4 >= 4",
                 true)]
        [DataRow("1 <= 2",
                 true)]
        [DataRow("4 <= 3",
                 false)]
        [DataRow("4 <= 4",
                 true)]
        [DataRow("nil > 1", false)]
        [DataRow("nil < 1", false)]
        [DataRow("nil >= 1", false)]
        [DataRow("nil <= 1", false)]
        [DataRow("1 > nil", false)]
        [DataRow("1 < nil", false)]
        [DataRow("1 >= nil", false)]
        [DataRow("1 <= nil", false)]
        [DataRow("nil > nil", false)]
        [DataRow("nil < nil", false)]
        [DataRow("nil >= nil", false)]
        [DataRow("nil <= nil", false)]
        public void CanInterpretComparisonExpressions(string source,
                                                      object expectedValue)
        {
            this.AssertExpression(source,
                                  expectedValue);
        }

        [DataTestMethod]
        [DataRow("1 == 1", true)]
        [DataRow("1 == 2", false)]
        [DataRow("1 == \"1\"", false)]
        [DataRow("1 == true", false)]
        [DataRow("1 == false", false)]
        [DataRow("1 == nil", false)]
        [DataRow("nil == 1", false)]
        [DataRow("1 != 1", false)]
        [DataRow("1 != 2", true)]
        [DataRow("1 != \"1\"", true)]
        [DataRow("1 != true", true)]
        [DataRow("1 != true", true)]
        [DataRow("1 != nil", true)]
        [DataRow("nil != 1", true)]
        [DataRow("nil == nil", true)]
        [DataRow("nil != nil", false)]
        public void CanInterpretEqualityExpressions(string source,
                                                    object expectedValue)
        {
            this.AssertExpression(source,
                                  expectedValue);
        }

        [DataTestMethod]
        [DataRow("1 + true", TokenType.PLUS, "Operands must be two numbers or two strings.")]
        [DataRow("1 - true", TokenType.MINUS, "Operands must be numbers.")]
        [DataRow("1 * \"1\"", TokenType.STAR, "Operands must be numbers.")]
        [DataRow("1 / false", TokenType.SLASH, "Operands must be numbers.")]
        public void CanThrowRuntimeErrorsForInvalidArithmetic(string source, TokenType opType, string expectedError)
        {
            this.AssertRuntimeError(source,
                                    opType,
                                    expectedError);
        }

        [DataTestMethod]
        [DataRow("1 > true", TokenType.GREATER)]
        [DataRow("1 < true", TokenType.LESS)]
        [DataRow("1 >= true", TokenType.GREATER_EQUAL)]
        [DataRow("1 <= true", TokenType.LESS_EQUAL)]
        [DataRow("true > 1", TokenType.GREATER)]
        [DataRow("true < 1", TokenType.LESS)]
        [DataRow("true >= 1", TokenType.GREATER_EQUAL)]
        [DataRow("true <= 1", TokenType.LESS_EQUAL)]
        public void CanThrowRuntimeErrorForInvalidComparison(string source, TokenType opType)
        {
            this.AssertRuntimeError(source,
                                    opType,
                                    "Operands must be numbers.");
        }
    }
}