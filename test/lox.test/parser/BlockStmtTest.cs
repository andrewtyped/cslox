using Microsoft.VisualStudio.TestTools.UnitTesting;

using static lox.Stmt;

namespace lox.test.parser
{
    [TestClass]
    public class BlockStmtTest : ParserTestBase
    {
        #region Instance Methods

        [TestMethod]
        public void CanDetectUnbalancedBracesInBlockStatement()
        {
            var source = "{ var foo; foo = 1; print foo;";

            this.AssertParseErrors2(source,
                                    "Expect '}'");
        }

        [TestMethod]
        public void CanParseBlockStatements()
        {
            var source = "{ var foo; foo = 1; print foo; }";
            var blockStmt = this.AssertStmt<Block>(source);

            Assert.AreEqual(3,
                            blockStmt.statements.Count);
        }

        #endregion
    }
}