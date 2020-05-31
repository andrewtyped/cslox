using System.Text;

using lox.constants;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using static lox.Stmt;

namespace lox.test.parser
{
    [TestClass]
    public class FunctionStmtTest : ParserTestBase
    {
        #region Instance Methods

        [TestMethod]
        public void CannotParseFunctionWithTooManyParameters()
        {
            var sb = new StringBuilder();
            sb.Append("fun tooManyParams(param0");

            for (int i = 1;
                 i <= Limits.MaxArguments;
                 i++)
            {
                sb.Append($",param{i}");
            }

            sb.Append("){}");
            var source = sb.ToString();

            this.AssertParseErrors2(source,
                                    $"{Limits.MaxArguments}");
        }

        [TestMethod]
        public void CanParseFunctionDeclarationWithManyParameters()
        {
            var source = @"
fun myPrint (text, text2, text3) {
    print text;
    print text2;
    print text3;
}
";
            var functionStmt = this.AssertStmt<Function>(source);
            Assert.AreEqual("myPrint",
                            functionStmt.name.GetLexeme(source)
                                        .ToString(),
                            "Function name equality");
            Assert.AreEqual(3,
                            functionStmt.parameters.Count,
                            "Parameter count");
            Assert.AreEqual("text",
                            functionStmt.parameters[0]
                                        .GetLexeme(source)
                                        .ToString(),
                            "Parameter name");
            Assert.AreEqual("text2",
                            functionStmt.parameters[1]
                                        .GetLexeme(source)
                                        .ToString(),
                            "Parameter name");
            Assert.AreEqual("text3",
                            functionStmt.parameters[2]
                                        .GetLexeme(source)
                                        .ToString(),
                            "Parameter name");
            Assert.AreEqual(3,
                            functionStmt.body.Count,
                            "Body statements count");
        }

        [TestMethod]
        public void CanParseFunctionDeclarationWithNoParameters()
        {
            var source = @"
fun helloWorld() {
    print ""hello world"";
}
";
            var functionStmt = this.AssertStmt<Function>(source);
            Assert.AreEqual("helloWorld",
                            functionStmt.name.GetLexeme(source)
                                        .ToString(),
                            "Function name equality");
            Assert.AreEqual(0,
                            functionStmt.parameters.Count,
                            "Parameter count");
            Assert.AreEqual(1,
                            functionStmt.body.Count,
                            "Body statements count");
        }

        [TestMethod]
        public void CanParseFunctionDeclarationWithOneParameter()
        {
            var source = @"
fun myPrint (text) {
    print text;
}
";
            var functionStmt = this.AssertStmt<Function>(source);
            Assert.AreEqual("myPrint",
                            functionStmt.name.GetLexeme(source)
                                        .ToString(),
                            "Function name equality");
            Assert.AreEqual(1,
                            functionStmt.parameters.Count,
                            "Parameter count");
            Assert.AreEqual("text",
                            functionStmt.parameters[0]
                                        .GetLexeme(source)
                                        .ToString(),
                            "Parameter name");
            Assert.AreEqual(1,
                            functionStmt.body.Count,
                            "Body statements count");
        }

        #endregion
    }
}