using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test.parser
{
    [TestClass]
    public class GroupingTests : ParserTestBase
    {
        #region Instance Methods

        [DataTestMethod]
        [DataRow("1 + (3 * 2)",
            "(+ 1 (group (* 3 2)))")]
        public void CanParseGroupedBinaryExpressions(string source,
                                                     string expectedAst)
        {
            this.AssertAst(source,
                           expectedAst);
        }

        #endregion
    }
}