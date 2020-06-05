using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lox.test.resolver
{
    [TestClass]
    public class ResolverTestBase
    {
        protected Resolver resolver;

        [TestInitialize]
        public void TestInitialize()
        {
            this.resolver = new Resolver(new Interpreter(new Mock<IConsole>().Object));
        }

        protected void Resolve(string source)
        {
            var scanner = new Scanner();
            var tokens = scanner.ScanTokens(source);
            var parser = new Parser();
            var statements = parser.Parse(new ScannedSource(new ReadOnlySpan<Token>(tokens.ToArray()),
                                                            source));

            if (parser.ParseErrors.Any())
            {
                string joinedErrors = string.Join(System.Environment.NewLine,
                                                  parser.ParseErrors.Select(err => err.Message));
                Assert.Fail(joinedErrors);
            }

            this.resolver.Resolve(statements,
                                  source);
        }
    }
}
