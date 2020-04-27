using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

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
}