using Microsoft.VisualStudio.TestTools.UnitTesting;

using static lox.Stmt;

namespace lox.test.parser
{
    [TestClass]
    public class WhileStmtTest : ParserTestBase
    {
        #region Instance Methods

        [TestMethod]
        public void CanDetectMissingLeftParenInWhileStatement()
        {
            var source = @"while true or false) { var foo = 1; }";

            this.AssertParseErrors2(source,
                                    "Expect '('",
                                    "Expect expression");
        }

        [TestMethod]
        public void CanDetectMissingRightParenInWhileStatement()
        {
            var source = @"while (true or false { var foo = 1; }";

            this.AssertParseErrors2(source,
                                    "Expect ')'",
                                    "Expect expression");
        }

        [TestMethod]
        public void CanParseWhileStatement()
        {
            var source = @"while(true or false) { var foo = 1; }";

            var whileStmt = this.AssertStmt<While>(source);
            this.AssertExpr<Expr.Logical>(whileStmt.condition);
            this.AssertStmt<Block>(whileStmt.statement);
        }

        #endregion
    }
}