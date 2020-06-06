using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test.interpreter
{
    [TestClass]
    public class ClosureInterpretationTest : InterpreterTestBase
    {
        #region Instance Methods

        [TestMethod]
        public void LocalFunctionCapturesVariablesFromOuterFunction()
        {
            var source = @"
fun makeCounter() {
    var i = 0;
    fun count() {
        i = i + 1;
        print i;
    }

    return count;
}

var counter = makeCounter();
counter();
counter();
counter();";

            this.Interpret(source);
            this.AssertPrints("1",
                              "2",
                              "3");
        }

        [TestMethod]
        public void ClosuresCaptureVariablesAtTheScopeTheyWereDefined()
        {
            var source = @"var a = ""global"";
    {
        fun showA() {
            print a;
        }

        showA();
        var a = ""block"";
        showA();
    }";

            this.Interpret(source);
            this.AssertPrints("global",
                              "global");
        }

        #endregion
    }
}