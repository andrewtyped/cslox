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

        [TestMethod]
        public void CanCreateInstanceOfClass()
        {
            var source = @"
class foo { 
  hello(){} 
  world(){} 
}
var fooInstance = foo();
print fooInstance;";

            this.Interpret(source);
            this.AssertPrints("foo instance");
        }

        [TestMethod]
        public void CanAccessClassMethod()
        {
            var source = @"
class foo { 
  hello() {
    return ""world"";
  } 
}
var fooInstance = foo();
print fooInstance.hello();";

            this.Interpret(source);
            this.AssertPrints("world");
        }

        [TestMethod]
        public void CanUseThisToAccessInstanceState()
        {
            var source = @"
class Person {
  hello(){
    print this.name;
  }
}

var person1 = Person();
person1.name = ""MyNameIs"";
var person2 = Person();
person2.name = ""Slim Shady"";
person1.hello();
person2.hello();
";
            this.Interpret(source);
            this.AssertPrints("MyNameIs",
                              "Slim Shady");
        }
    }
}
