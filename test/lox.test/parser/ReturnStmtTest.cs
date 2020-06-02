using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Text;

using static lox.Expr;
using static lox.Stmt;

namespace lox.test.parser
{
    [TestClass]
    public class ReturnStmtTest : ParserTestBase
    {
        [TestMethod]
        public void CanParseReturnStmtWithoutValue()
        {
            var source = @"return;";

            var @return = this.AssertStmt<Return>(source);

            Assert.AreEqual("return",
                            @return.keyword.GetLexeme(source)
                                   .ToString());
            Assert.IsNull(@return.value);
        }

        [TestMethod]
        public void CanParseReturnStmtWithValue()
        {
            var source = @"return 1;";

            var @return = this.AssertStmt<Return>(source);

            Assert.AreEqual("return",
                            @return.keyword.GetLexeme(source)
                                   .ToString());

            var literal = this.AssertExpr<Literal>(source);

            Assert.AreEqual(1d,
                            literal.value);
        }
    }
}