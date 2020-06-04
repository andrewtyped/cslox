using lox.constants;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test.interpreter
{
    [TestClass]
    public class ResolveTest : InterpreterTestBase
    {
        #region Instance Methods

        [TestMethod]
        public void ResolveStoresExpressionDepth()
        {
            var expr = new Expr.Binary(new Expr.Literal(1),
                                       new Token(0,
                                                 1,
                                                 0,
                                                 TokenType.PLUS),
                                       new Expr.Literal(2));
            this.Interpreter.Resolve(expr,
                                     1);

            Assert.AreEqual(1,
                            this.Interpreter.Locals.Count);
            Assert.AreEqual(1,
                            this.Interpreter.Locals[expr]);
        }

        #endregion
    }
}