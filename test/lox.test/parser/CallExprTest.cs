using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using static lox.Expr;

namespace lox.test.parser
{
    [TestClass]
    public class CallExprTest : ParserTestBase
    {
        #region Instance Methods

        [TestMethod]
        public void CanParseNamedFunctionCall()
        {
            var source = "add()";
            var callExpr = this.AssertExpr<Call>(source);
            var callee = this.AssertExpr<Variable>(callExpr.callee);
            Assert.AreEqual("add",
                            callee.name.GetLexeme(source)
                                  .ToString());
            Assert.AreEqual(0,
                            callExpr.arguments.Count);
        }

        [TestMethod]
        public void CanParseNamedFunctionCallWithMultipleArguments()
        {
            var source = "add(1,2,3)";
            var callExpr = this.AssertExpr<Call>(source);
            var callee = this.AssertExpr<Variable>(callExpr.callee);
            Assert.AreEqual("add",
                            callee.name.GetLexeme(source)
                                  .ToString());
            Assert.AreEqual(3,
                            callExpr.arguments.Count);
            var firstArgument = this.AssertExpr<Literal>(callExpr.arguments[0]);

            Assert.AreEqual(1d,
                            firstArgument.value);

            var secondArgument = this.AssertExpr<Literal>(callExpr.arguments[1]);

            Assert.AreEqual(2d,
                            secondArgument.value);

            var thirdArgument = this.AssertExpr<Literal>(callExpr.arguments[2]);

            Assert.AreEqual(3d,
                            thirdArgument.value);
        }

        [TestMethod]
        public void CanParseNamedFunctionCallWithOneArgument()
        {
            var source = "add(1)";
            var callExpr = this.AssertExpr<Call>(source);
            var callee = this.AssertExpr<Variable>(callExpr.callee);
            Assert.AreEqual("add",
                            callee.name.GetLexeme(source)
                                  .ToString());
            Assert.AreEqual(1,
                            callExpr.arguments.Count);
            var firstArgument = this.AssertExpr<Literal>(callExpr.arguments[0]);

            Assert.AreEqual(1d,
                            firstArgument.value);
        }

        [TestMethod]
        public void LoxDoesNotSupportOver255ArgumentsInAFunction()
        {
            var sb = new StringBuilder();
            sb.Append("Add(");

            for (int i = 1;
                 i < 256;
                 i++)
            {
                sb.Append(i + ",");
            }

            sb.Append("256)");

            var source = sb.ToString();

            this.AssertParseErrors2(source,
                                    "255");
        }

        #endregion
    }
}