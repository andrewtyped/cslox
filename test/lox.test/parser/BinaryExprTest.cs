using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test.parser
{
    [TestClass]
    public class BinaryExprTest : ParserTestBase
    {
        #region Instance Methods

        [DataTestMethod]
        [DataRow("1 * 2",
            "(* 1 2)")]
        [DataRow("3 / 4",
            "(/ 3 4)")]
        [DataRow("10 + 24",
            "(+ 10 24)")]
        [DataRow("10 - 2",
            "(- 10 2)")]
        [DataRow("1 + 3 * 2",
            "(+ 1 (* 3 2))")]
        [DataRow("1 + 3 * 2 / 5 - 10",
            "(- (+ 1 (/ (* 3 2) 5)) 10)")]
        public void CanParseBinaryArithmeticExpressions(string source,
                                                        string expectedAst)
        {
            this.AssertAst(source,
                           expectedAst);
        }

        [DataTestMethod]
        [DataRow("1 > 2",
            "(> 1 2)")]
        [DataRow("1 >= 2",
            "(>= 1 2)")]
        [DataRow("1 < 2",
            "(< 1 2)")]
        [DataRow("1 <= 2",
            "(<= 1 2)")]
        [DataRow("1 <= 2 > 3 >= 4",
            "(>= (> (<= 1 2) 3) 4)")]
        public void CanParseBinaryComparisonExpressions(string source,
                                                        string expectedAst)
        {
            this.AssertAst(source,
                           expectedAst);
        }

        [DataTestMethod]
        [DataRow("5 == 5",
            "(== 5 5)")]
        [DataRow("1 != 2",
            "(!= 1 2)")]
        [DataRow("1 != 2 == 2 != 3",
            "(!= (== (!= 1 2) 2) 3)")]
        public void CanParseBinaryEqualityExpressions(string source,
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