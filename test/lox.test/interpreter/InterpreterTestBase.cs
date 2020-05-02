using System.Collections.Generic;
using System.Linq;
using lox.constants;
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
            var stmts = this.Parse(source);
            this.Interpreter.Interpret(stmts,
                                       source);

            return this.Interpreter.LastValue;
        }

        protected List<Stmt> Parse(string source)
        {
            var scanner = new Scanner();
            var parser = new Parser();

            var scannedTokens = scanner.ScanTokens(source);
            var stmts = parser.Parse(new ScannedSource(scannedTokens.ToArray(),
                                                       source));

            return stmts;
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


                Assert.IsNotNull(this.Interpreter.LastError,
                                 $"Expected source '{source}' to cause runtime error");

                RuntimeError error = this.Interpreter.LastError!;

                Assert.IsTrue(error.Message.Contains(expectedError),
                              $"Expected runtime error message '{error.Message}' to contain '{expectedError}'");
            }
            catch(RuntimeError runtimeError)
            {
                Assert.AreEqual(op,
                                runtimeError.Token.Type);
                
            }
        }

        #endregion
    }
}