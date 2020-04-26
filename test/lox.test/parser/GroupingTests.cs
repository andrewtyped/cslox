using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test.parser
{
    [TestClass]
    public class GroupingTests : ParserTestBase
    {
        #region Instance Methods

        [TestMethod]
        public void CanDetectImbalancedParenthesesInGroupedExpressions()
        {
            var source = "1 + (2 + 2";
            var scannedSource = this.Scan(source);
            var expr = this.parser.Parse(scannedSource);
            this.AssertParseErrors(")");
        }

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