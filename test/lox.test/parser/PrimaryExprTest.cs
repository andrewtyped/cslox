using Microsoft.VisualStudio.TestTools.UnitTesting;

using static lox.Expr;

namespace lox.test.parser
{
    [TestClass]
    public class PrimaryExprTest : ParserTestBase
    {
        #region Instance Methods

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void CanParseBooleanLiteralExpression(bool boolean)
        {
            var scannedSource = this.ScanBoolean(boolean);
            var expr = this.parser.Parse(scannedSource);

            var literalExpr = expr as Literal;

            Assert.IsNotNull(literalExpr);
            Assert.AreEqual(boolean,
                            literalExpr!.value);
        }

        [TestMethod]
        public void CanParseNilExpression()
        {
            var scannedSource = this.Scan("nil");
            var expr = this.parser.Parse(scannedSource);

            var literalExpr = expr as Literal;

            Assert.IsNotNull(literalExpr);
            Assert.IsNull(literalExpr!.value);
        }

        [TestMethod]
        public void CanParseNumericLiteralExpression()
        {
            double number = 42;
            var scannedSource = this.ScanPrimary(number);
            var expr = this.parser.Parse(scannedSource);

            var literalExpr = expr as Literal;

            Assert.IsNotNull(literalExpr);
            Assert.AreEqual(number,
                            literalExpr!.value);
        }

        [TestMethod]
        public void CanParseStringLiteralExpression()
        {
            var @string = "My string";
            var scannedSource = this.ScanString(@string);
            var expr = this.parser.Parse(scannedSource);

            var literalExpr = expr as Literal;

            Assert.IsNotNull(literalExpr);
            Assert.AreEqual(@string,
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