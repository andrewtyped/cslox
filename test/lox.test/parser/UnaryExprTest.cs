using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

using static lox.constants.TokenType;
using static lox.Expr;

namespace lox.test.parser
{
    [TestClass]
    public class UnaryExprTest : ParserTestBase
    {
        [TestMethod]
        public void CanParseBangExpressions()
        {
            var source = "!42";
            var scannedSource = this.Scan(source);

            var unaryExpr = this.AssertExpr<Unary>(scannedSource);

            Assert.AreEqual(BANG,
                            unaryExpr.op.Type);

            var literalExpr = this.AssertExpr<Literal>(unaryExpr.right);

            Assert.AreEqual(42d,
                            literalExpr.value);
        }

        [TestMethod]
        public void CanParseNestedBangExpressions()
        {
            var source = "!!42";
            var scannedSource = this.Scan(source);

            var unaryExpr = this.AssertExpr<Unary>(scannedSource);

            Assert.AreEqual(BANG,
                            unaryExpr.op.Type);

            var nestedUnaryExpr = this.AssertExpr<Unary>(unaryExpr.right);

            Assert.AreEqual(BANG,
                            nestedUnaryExpr.op.Type);

            var literalExpr = this.AssertExpr<Literal>(nestedUnaryExpr.right);

            Assert.AreEqual(42d,
                            literalExpr.value);
        }

        [TestMethod]
        public void CanParseNegationExpressions()
        {
            var source = "-84";
            var scannedSource = this.Scan(source);

            var unaryExpr = this.AssertExpr<Unary>(scannedSource);

            Assert.AreEqual(MINUS,
                            unaryExpr.op.Type);

            var literalExpr = this.AssertExpr<Literal>(unaryExpr.right);

            Assert.AreEqual(84d,
                            literalExpr.value);
        }
    }
}
