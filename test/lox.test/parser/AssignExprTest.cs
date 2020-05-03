using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

using static lox.Expr;

namespace lox.test.parser
{
    [TestClass]
    public class AssignExprTest : ParserTestBase
    {
        [TestMethod]
        public void CanParseAssignExpressions()
        {
            var source = @"foo = ""bar""";

            var expr = this.AssertExpr<Assign>(source);

            Assert.AreEqual("foo",
                            expr.name.GetLexeme(source)
                                .ToString());
            this.AssertExpr<Literal>(expr.value);
        }

        [TestMethod]
        public void CanDetectInvalidAssignmentTargets()
        {
            var source = @"(a) = 3;";
            this.AssertParseErrors2(source,
                                    "Invalid assignment target.");
        }
    }
}
