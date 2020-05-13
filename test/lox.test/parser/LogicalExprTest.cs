using lox.constants;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using static lox.Expr;

namespace lox.test.parser
{
    [TestClass]
    public class LogicalExprTest : ParserTestBase
    {
        #region Instance Methods

        [TestMethod]
        public void CanParseLogicalAndExpression()
        {
            var source = "true and 1 == 1";
            var expr = this.AssertExpr<Logical>(source);

            this.AssertExpr<Literal>(expr.left);
            this.AssertExpr<Binary>(expr.right);
            Assert.AreEqual(TokenType.AND,
                            expr.op.Type);
        }

        [TestMethod]
        public void CanParseLogicalOrExpression()
        {
            var source = "true or false";
            var expr = this.AssertExpr<Logical>(source);

            this.AssertExpr<Literal>(expr.left);
            this.AssertExpr<Literal>(expr.right);
            Assert.AreEqual(TokenType.OR,
                            expr.op.Type);
        }

        #endregion
    }
}