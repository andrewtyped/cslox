using Microsoft.VisualStudio.TestTools.UnitTesting;

using static lox.Expr;
using static lox.Stmt;

namespace lox.test.parser
{
    [TestClass]
    public class ClassStmtTest : ParserTestBase
    {
        #region Instance Methods

        [TestMethod]
        public void CanParseClassDeclaration()
        {
            var source = @"
class foo {
  hello() {}

  world() {}
}
";
            var @class = this.AssertStmt<Class>(source);
            Assert.AreEqual("foo",
                            @class.name.GetLexeme(source)
                                  .ToString());
            Assert.AreEqual(2,
                            @class.methods.Count);
        }

        [TestMethod]
        public void CanParseSuperClassDeclaration()
        {
            var source = @"
class foo < bar {
  hello() {}

  world() {}
}
";
            var @class = this.AssertStmt<Class>(source);
            Assert.AreEqual("foo",
                            @class.name.GetLexeme(source)
                                  .ToString());
            Assert.AreEqual(2,
                            @class.methods.Count);

            var superclass = this.AssertExpr<Variable>(@class.superclass);

            Assert.AreEqual("bar",
                            superclass.name.GetLexeme(source)
                                      .ToString());
        }

        #endregion
    }
}