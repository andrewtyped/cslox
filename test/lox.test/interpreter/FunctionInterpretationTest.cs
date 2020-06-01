using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test.interpreter
{
    [TestClass]
    public class FunctionInterpretationTest : InterpreterTestBase
    {
        #region Instance Methods

        [TestMethod]
        public void CanInterpretFunctionDeclaration()
        {
            var source = @"
fun sayName(first, last) {
    print ""Hi, "" + first + "" "" + last;
}

sayName(""Andrew"",""Barger"");
";

            this.Interpret(source);

            this.AssertPrints("Hi, Andrew Barger");
        }

        #endregion
    }
}