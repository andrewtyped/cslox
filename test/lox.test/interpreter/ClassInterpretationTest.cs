﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        [TestMethod]
        public void CannotUseThisOutsideAClass()
        {
            var source = "print this;";
            this.AssertResolutionError(source,
                                       "Cannot use 'this' outside of a class.");
        }

        [TestMethod]
        public void CanUseConstructorToInitializeClass()
        {
            var source = @"
class Person {
  init() {
    this.name = ""Andrew"";
  }
}

var person = Person();
print person.name;
";
            this.Interpret(source);
            this.AssertPrints("Andrew");
        }

        [TestMethod]
        public void CanUseConstructorWithParametersToInitializeClass()
        {
            var source = @"
class Person {
  init(name) {
    this.name = name;
  }
}

var person = Person(""Andrew"");
print person.name;
";
            this.Interpret(source);
            this.AssertPrints("Andrew");
        }

        [TestMethod]
        public void InvokingInitReturnsInstance()
        {
            var source = @"
class Person {
  init() {}
}

var person = Person();
var person2 = person.init();
print person2;
";
            this.Interpret(source);
            this.AssertPrints("Person instance");
        }

        [TestMethod]
        public void CannotReturnFromInit()
        {
            var source = @"
class Person {
  init() {
    return ""hello"";
  }
}
";
            this.AssertResolutionError(source,
                                       "Return statements are only allowed in functions and methods.");
        }
    }
}
