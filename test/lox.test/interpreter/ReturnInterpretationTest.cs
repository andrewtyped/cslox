using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Text;

namespace lox.test.interpreter
{
    [TestClass]
    public class ReturnInterpretationTest : InterpreterTestBase
    {
        [TestMethod]
        public void CanInterpretReturnStatementWithoutValue()
        {
            var source = @"
fun unreachable() {
    return;
    print ""hello world"";
}

print unreachable();";

            this.Interpret(source);
            this.AssertPrints("nil");
        }

        [TestMethod]
        public void CanInterpretReturnStatementWithValue()
        {
            var source = @"
fun add(num1, num2) {
    return num1 + num2;
}

print add(1, 2);";

            this.Interpret(source);
            this.AssertPrints("3");
        }

        [TestMethod]
        public void FunctionWithoutReturnReturnsNil()
        {
            var source = @"
fun nop() {
    var goesNowhere = 1 + 1;
}

print nop();";

            this.Interpret(source);
            this.AssertPrints("nil");
        }

        [TestMethod]
        public void FunctionWithEmptyReturnReturnsNil()
        {
            var source = @"
fun nop() {
    return;

    print ""I'm nopping"";
}

print nop();";

            this.Interpret(source);
            this.AssertPrints("nil");
        }

        [TestMethod]
        public void CannotReturnWhenNotInAFunction()
        {
            var source = "return 1;";
            this.AssertResolutionError(source,
                                       "Return statements are only allowed in functions and methods.");
        }
    }
}