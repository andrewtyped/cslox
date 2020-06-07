using Microsoft.VisualStudio.TestTools.UnitTesting;

using static lox.Expr;

namespace lox.test.parser
{
    [TestClass]
    public class SetExprTest : ParserTestBase
    {
        #region Instance Methods

        [TestMethod]
        public void CanParseSetExpression()
        {
            var source = "foo.bar = 1";
            var setExpr = this.AssertExpr<Set>(source);

            Assert.AreEqual("bar",
                            setExpr.name.GetLexeme(source)
                                   .ToString());
            var varExpr = this.AssertExpr<Variable>(setExpr.@object);
            Assert.AreEqual("foo",
                            varExpr.name.GetLexeme(source)
                                   .ToString());
            var literalExpr = this.AssertExpr<Literal>(setExpr.value);
            Assert.AreEqual(1d,
                            literalExpr.value);
        }

        #endregion
    }
}