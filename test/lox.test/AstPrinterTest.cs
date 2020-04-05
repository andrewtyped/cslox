using lox.constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using static lox.Expr;

namespace lox.test
{
    [TestClass]
    public class AstPrinterTest
    {
        private AstPrinter astPrinter = new AstPrinter();
        
        [TestInitialize]
        public void TestInitialize()
        {
            this.astPrinter = new AstPrinter();
        }

        [DataTestMethod]
        [DataRow(1, "1")]
        [DataRow("hello world","hello world")]
        [DataRow(null, "nil")]
        public void CanPrintLiteralExpr(object? literalValue, string expectedPrint)
        {
            var literal = new Literal(literalValue);

            this.AssertPrint(literal,
                             expectedPrint,
                             expectedPrint);
        }

        [TestMethod]
        public void CanPrintGroupingExpr()
        {
            var group = new Grouping(new Literal(1));

            this.AssertPrint(group,
                             "(1)",
                             "(group 1)");
        }

        [TestMethod]
        public void CanPrintBinaryExpr()
        {
            var source = "3 + 1";
            var binary = new Binary(new Literal(3),
                                    new Token(2,
                                              3,
                                              1,
                                              TokenType.PLUS),
                                    new Literal(1));

            this.AssertPrint(binary,
                             source,
                             "(+ 3 1)");
        }

        [TestMethod]
        public void CanPrintUnaryExpression()
        {
            var source = "-1";
            var unary = new Unary(new Token(0,
                                            1,
                                            1,
                                            TokenType.MINUS),
                                  new Literal(1));
            this.AssertPrint(unary,
                             source,
                             "(- 1)");
        }

        [TestMethod]
        public void CanPrintDeepExpression()
        {
            var source = "-123 * (45.67)";
            var expr = new Binary(new Unary(new Token(0,
                                                      1,
                                                      1,
                                                      TokenType.MINUS),
                                            new Literal(123)),
                                  new Token(5,
                                            6,
                                            1,
                                            TokenType.STAR),
                                  new Grouping(new Literal(45.67)));

            this.AssertPrint(expr,
                             source,
                             "(* (- 123) (group 45.67))");
        }

        private void AssertPrint(Expr expr,string source, string expectedPrint)
        {
            var actualPrint = this.astPrinter!.Print(expr,
                                                     source);
            Assert.AreEqual(expectedPrint,
                            actualPrint);
        }
    }
}
