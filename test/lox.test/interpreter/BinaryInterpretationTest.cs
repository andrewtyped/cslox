using lox.constants;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test.interpreter
{
    [TestClass]
    public class BinaryInterpretationTest : InterpreterTestBase
    {
        [DataTestMethod]
        [DataRow("3 * 4", "12")]
        [DataRow("9 / 6", "1.5")]
        [DataRow("10 - 3", "7")]
        [DataRow("23 + 51", "74")]
        [DataRow("1 + 2 + 5.5", "8.5")]
        [DataRow("2 + 2 * 4", "10")]
        [DataRow("1 + 2 / 4 + 1", "2.5")]
        [DataRow("(1 + 2) / (4 + 1)", "0.6")]
        [DataRow("1 + nil", "nil")]
        [DataRow("nil + 1", "nil")]
        [DataRow("nil + nil", "nil")]
        public void CanInterpretArithmeticExpressions(string source,
                                                      string expectedPrint)
        {
            this.Interpret(source);
            this.AssertPrints(expectedPrint);
        }

        [DataTestMethod]
        [DataRow("\"Hello\" + \" world\"",
                 "Hello world")]
        [DataRow("nil + \" world\"",
                 "nil")]
        [DataRow("\"Hello\" + nil",
                 "nil")]
        public void CanInterpretStringConcatenationExpressions(string source,
                                                               string expectedValue)
        {
            this.Interpret(source);
            this.AssertPrints(expectedValue);
        }

        [DataTestMethod]
        [DataRow("1 > 2",
                 "False")]
        [DataRow("4 > 3",
                 "True")]
        [DataRow("4 > 4",
                 "False")]
        [DataRow("1 < 2",
                 "True")]
        [DataRow("4 < 3",
                 "False")]
        [DataRow("4 < 4",
                 "False")]
        [DataRow("1 >= 2",
                 "False")]
        [DataRow("4 >= 3",
                 "True")]
        [DataRow("4 >= 4",
                 "True")]
        [DataRow("1 <= 2",
                 "True")]
        [DataRow("4 <= 3",
                 "False")]
        [DataRow("4 <= 4",
                 "True")]
        [DataRow("nil > 1", "False")]
        [DataRow("nil < 1", "False")]
        [DataRow("nil >= 1", "False")]
        [DataRow("nil <= 1", "False")]
        [DataRow("1 > nil", "False")]
        [DataRow("1 < nil", "False")]
        [DataRow("1 >= nil", "False")]
        [DataRow("1 <= nil", "False")]
        [DataRow("nil > nil", "False")]
        [DataRow("nil < nil", "False")]
        [DataRow("nil >= nil", "False")]
        [DataRow("nil <= nil", "False")]
        public void CanInterpretComparisonExpressions(string source,
                                                      string expectedValue)
        {
            this.Interpret(source);
            this.AssertPrints(expectedValue);
        }

        [DataTestMethod]
        [DataRow("1 == 1", "True")]
        [DataRow("1 == 2", "False")]
        [DataRow("1 == \"1\"", "False")]
        [DataRow("1 == true", "False")]
        [DataRow("1 == false", "False")]
        [DataRow("1 == nil", "False")]
        [DataRow("nil == 1", "False")]
        [DataRow("1 != 1", "False")]
        [DataRow("1 != 2", "True")]
        [DataRow("1 != \"1\"", "True")]
        [DataRow("1 != true", "True")]
        [DataRow("1 != true", "True")]
        [DataRow("1 != nil", "True")]
        [DataRow("nil != 1", "True")]
        [DataRow("nil == nil", "True")]
        [DataRow("nil != nil", "False")]
        public void CanInterpretEqualityExpressions(string source,
                                                    string expectedValue)
        {
            this.Interpret(source);
            this.AssertPrints(expectedValue);
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