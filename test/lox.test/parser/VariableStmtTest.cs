using Microsoft.VisualStudio.TestTools.UnitTesting;

using static lox.Stmt;

namespace lox.test.parser
{
    [TestClass]
    public class VariableStmtTest : ParserTestBase
    {
        #region Instance Methods

        [TestMethod]
        public void CanParseVariableStmtWithoutAssignment()
        {
            var source = @"var foo;";

            var stmt = this.AssertStmt<Var>(source);

            Assert.AreEqual("foo",
                            stmt.name.GetLexeme(source)
                                .ToString());
            Assert.IsNull(stmt.initializer);
        }

        #endregion
    }
}