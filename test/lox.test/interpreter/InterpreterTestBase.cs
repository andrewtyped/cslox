using System.Linq;
using lox.constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using static lox.constants.TokenType;

namespace lox.test.interpreter
{
    [TestClass]
    public class InterpreterTestBase
    {
        #region Fields

        protected Interpreter Interpreter = new Interpreter();

        #endregion

        #region Instance Methods

        protected object? Interpret(string source)
        {
            var expr = this.Parse(source);
            var value = expr.Accept(this.Interpreter,
                                    source);

            return value;
        }

        protected Expr Parse(string source)
        {
            var scanner = new Scanner();
            var parser = new Parser();

            var scannedTokens = scanner.ScanTokens(source);
            var expr = parser.Parse(new ScannedSource(scannedTokens.ToArray(),
                                                      source));

            return expr;
        }

        protected void AssertExpression(string source,
                                        object expectedValue)
        {
            var value = this.Interpret(source);
            Assert.AreEqual(expectedValue,
                            value);
        }

        protected void AssertRuntimeError(string source,
                                          TokenType op,
                                          string expectedError)
        {
            try
            {
                var value = this.Interpret(source);
                Assert.Fail($"Expected source '{source}' to cause runtime error");
            }
            catch(RuntimeError runtimeError)
            {
                Assert.AreEqual(op,
                                runtimeError.Token.Type);
                Assert.IsTrue(runtimeError.Message.Contains(expectedError),
                              $"Expected runtime error message '{runtimeError.Message}' to contain '{expectedError}'");
            }
        }

        #endregion
    }

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

    [TestClass]
    public class GroupingInterpretaionTest : InterpreterTestBase 
    {
        [DataTestMethod]
        [DataRow("(1)",
            1d)]
        [DataRow("((42))",
            42d)]
        [DataRow("((((((((100))))))))",
            100d)]
        public void CanInterpretGroupedLiteralExpressions(string source,
                                                          object expectedValue)
        {
            var value = this.Interpret(source);
            Assert.AreEqual(expectedValue,
                            value);
        }
    }

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
                                    MINUS,
                                    "Operand must be a number.");
        }

    }

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
        [DataRow("1 + true", PLUS, "Operands must be two numbers or two strings.")]
        [DataRow("1 - true", MINUS, "Operands must be numbers.")]
        [DataRow("1 * \"1\"", STAR, "Operands must be numbers.")]
        [DataRow("1 / false", SLASH, "Operands must be numbers.")]
        public void CanThrowRuntimeErrorsForInvalidArithmetic(string source, TokenType opType, string expectedError)
        {
            this.AssertRuntimeError(source,
                                    opType,
                                    expectedError);
        }

        [DataTestMethod]
        [DataRow("1 > true", GREATER)]
        [DataRow("1 < true", LESS)]
        [DataRow("1 >= true", GREATER_EQUAL)]
        [DataRow("1 <= true", LESS_EQUAL)]
        [DataRow("true > 1", GREATER)]
        [DataRow("true < 1", LESS)]
        [DataRow("true >= 1", GREATER_EQUAL)]
        [DataRow("true <= 1", LESS_EQUAL)]
        public void CanThrowRuntimeErrorForInvalidComparison(string source, TokenType opType)
        {
            this.AssertRuntimeError(source,
                                    opType,
                                    "Operands must be numbers.");
        }
    }
}