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

        protected override void AssertAst(string source, string expectedAst)
        {
            //Expression statements with no semicolon are parsed as print statements so
            //simple REPL statements don't need semicolons.
            base.AssertAst(source, $"(print {expectedAst})");
        }

        #endregion
    }
}