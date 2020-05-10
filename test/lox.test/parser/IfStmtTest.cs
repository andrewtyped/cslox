using Microsoft.VisualStudio.TestTools.UnitTesting;

using static lox.Expr;
using static lox.Stmt;

namespace lox.test.parser
{
    [TestClass]
    public class IfStmtTest : ParserTestBase
    {
        #region Instance Methods

        [TestMethod]
        public void CanParseIfStatement()
        {
            var source = "if (true) { true; }";
            var stmt = this.AssertStmt<If>(source);

            this.AssertExpr<Literal>(stmt.condition);
            this.AssertStmt<Block>(stmt.thenBranch);
            Assert.IsNull(stmt.elseBranch);
        }

        [TestMethod]
        public void CanParseIfElseStatement()
        {
            var source = "if (true) { true; } else { print \"hello world\"; }";
            var stmt = this.AssertStmt<If>(source);

            this.AssertExpr<Literal>(stmt.condition);
            this.AssertStmt<Block>(stmt.thenBranch);
            this.AssertStmt<Block>(stmt.elseBranch!);
        }

        #endregion
    }
}