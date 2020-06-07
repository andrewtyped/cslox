using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

using static lox.Expr;

namespace lox.test.parser
{
    [TestClass]
    public class GetExprTest : ParserTestBase
    {
        [TestMethod]
        public void CanParseGetExpr()
        {
            var source = "foo.bar";
            var getExpr = this.AssertExpr<Get>(source);
            Assert.AreEqual("bar",
                            getExpr.name.GetLexeme(source)
                                   .ToString());
            var varExpr = this.AssertExpr<Variable>(getExpr.@object);
            Assert.AreEqual("foo",
                            varExpr.name.GetLexeme(source)
                                   .ToString());
        }
    }
}
