using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace lox.test.interpreter
{
    [TestClass]
    public class ClassInterpretationTest: InterpreterTestBase
    {
        [TestMethod]
        public void CanDeclareClassAndPrintItsName()
        {
            var source = @"
class foo { 
  hello(){} 
  world(){} 
} 
print foo;";

            this.Interpret(source);

            this.AssertPrints("foo");
        }
    }
}
