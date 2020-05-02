using Microsoft.VisualStudio.TestTools.UnitTesting;

using static lox.Stmt;

namespace lox.test.parser
{
    [TestClass]
    public class PrintStmtTest : ParserTestBase
    {
        #region Instance Methods

        [TestMethod]
        public void CanParsePrintStatements()
        {
            var source = @"
print ""hello"";
print ""world"";";

            var printStmtType = typeof(Print);

            this.AssertStmts(source,
                             printStmtType,
                             printStmtType);
        }

        #endregion
    }
}