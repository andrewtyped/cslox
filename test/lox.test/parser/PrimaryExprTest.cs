using Microsoft.VisualStudio.TestTools.UnitTesting;

using static lox.Expr;

namespace lox.test.parser
{
    [TestClass]
    public class PrimaryExprTest : ParserTestBase
    {
        #region Instance Methods

        [DataTestMethod]
        [DataRow("true", true)]
        [DataRow("false", false)]
        public void CanParseBooleanLiteralExpression(string boolean, bool expected)
        {
            var literalExpr = this.AssertExpr<Literal>(boolean);

            Assert.AreEqual(expected,
                            literalExpr!.value);
        }

        [TestMethod]
        public void CanParseNilExpression()
        {
            var literalExpr = this.AssertExpr<Literal>("nil");

            Assert.IsNull(literalExpr.value);
        }

        [TestMethod]
        public void CanParseNumericLiteralExpression()
        {
            var literalExpr = this.AssertExpr<Literal>("42");

            Assert.AreEqual(42d,
                            literalExpr!.value);
        }

        [TestMethod]
        public void CanParseStringLiteralExpression()
        {
            var @string = "\"My string\"";
            var literalExpr = this.AssertExpr<Literal>(@string);

            Assert.AreEqual("My string",
                            literalExpr!.value);
        }

        [TestMethod]
        public void ThrowsParseErrorIfNoValidExpressionDetected()
        {

            var source = "This is not valid lox";
            var scannedSource = this.Scan(source);
            var expr = this.parser.Parse(scannedSource);
            this.AssertParseErrors("Expect expression");
        }

        private ScannedSource ScanBoolean(bool boolean)
        {
            return this.Scan(boolean
                                 ? "true"
                                 : "false");
        }

        private ScannedSource ScanPrimary(double number)
        {
            return this.Scan(number.ToString());
        }

        private ScannedSource ScanString(string @string)
        {
            return this.Scan($@"""{@string}""");
        }

        #endregion
    }
}