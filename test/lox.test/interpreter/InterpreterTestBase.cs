using System;
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

        protected MockConsole Console;

        protected Interpreter Interpreter;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            this.Console = new MockConsole();
            this.Interpreter = new Interpreter(this.Console);
        }

        #region Instance Methods
        
        protected void AssertPrints(params string[] expectedTexts)
        {
            Assert.AreEqual(expectedTexts.Length,
                            this.Console.Writes.Count,
                            $"Console write count");

            for(int i = 0; i < expectedTexts.Length; i++)
            {
                Assert.AreEqual(expectedTexts[i],
                                this.Console.Writes[i],
                                $"Console text equality at entry {i}.");
            } 
        }

        protected void AssertRuntimeError(string source,
                                          TokenType op,
                                          string expectedError)
        {
            try
            {
                this.Interpret(source);

                Assert.IsNotNull(this.Interpreter.LastError,
                                 $"Expected source '{source}' to cause runtime error");

                RuntimeError error = this.Interpreter.LastError!;

                Assert.IsTrue(error.Message.Contains(expectedError),
                              $"Expected runtime error message '{error.Message}' to contain '{expectedError}'");
            }
            catch (RuntimeError runtimeError)
            {
                Assert.AreEqual(op,
                                runtimeError.Token.Type);
            }
        }

        protected void AssertVariable(string name, object? expectedValue)
        {
            var environment = this.Interpreter.Environment;
            var value = environment.Get(name);
            Assert.AreEqual(expectedValue,
                            value);
        }

        protected void Interpret(string source)
        {
            this.InterpretStmts(source);
        }

        protected void InterpretStmts(string source)
        {
            var stmts = this.Parse(source);

            var resolver = new Resolver(this.Interpreter);
            resolver.Resolve(stmts,
                             source);

            this.Interpreter.Interpret(stmts,
                                       source);
        }

        protected List<Stmt> Parse(string source)
        {
            var scanner = new Scanner();
            var parser = new Parser();

            var scannedTokens = scanner.ScanTokens(source);
            var stmts = parser.Parse(new ScannedSource(scannedTokens.ToArray(),
                                                       source));

            if (parser.ParseErrors.Any())
            {
                throw new AggregateException(parser.ParseErrors);
            }

            return stmts;
        }

        #endregion

        #region Nested type: MockConsole

        protected class MockConsole : IConsole
        {
            #region Instance Properties

            public List<string> Writes
            {
                get;
            } = new List<string>();

            #endregion

            #region Instance Methods

            public void Write(string text)
            {
                this.Writes.Add(text);
            }

            public void WriteLine(string text)
            {
                this.Writes.Add(text);
            }

            #endregion
        }

        #endregion
    }
}