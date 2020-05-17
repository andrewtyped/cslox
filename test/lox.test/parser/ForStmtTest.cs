using Microsoft.VisualStudio.TestTools.UnitTesting;

using static lox.Expr;
using static lox.Stmt;

namespace lox.test.parser
{
    [TestClass]
    public class ForStmtTest : ParserTestBase
    {
        #region Instance Methods

        [TestMethod]
        public void CanParseForStmt()
        {
            var source = @"
for(var i = 0; i < 10; i = i + 1) {
    print i;
}";
            var stmts = this.Parse(source);

            Assert.AreEqual(1,
                            stmts.Count);

            var block = this.AssertStmt<Block>(stmts[0]);

            Assert.AreEqual(2,
                            block.statements.Count);

            this.AssertStmt<Var>(block.statements[0]);
            var @while = this.AssertStmt<While>(block.statements[1]);

            this.AssertExpr<Binary>(@while.condition);

            var whileBody = this.AssertStmt<Block>(@while.statement);

            Assert.AreEqual(2,
                            whileBody.statements.Count);

            var whileBlock = this.AssertStmt<Block>(whileBody.statements[0]);
            this.AssertStmt<Print>(whileBlock.statements[0]);
            var increment = this.AssertStmt<Expression>(whileBody.statements[1]);
            this.AssertExpr<Assign>(increment.expression);
            this.AssertExpr<Assign>(increment.expression);
        }

        #endregion
    }
}