using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace lox.test.interpreter
{
    [TestClass]
    public class BlockInterpretationTest: InterpreterTestBase
    {
        [TestMethod]
        public void CanInterpretBlockStatements()
        {
            var source = @"
var a = ""global a"";
var b = ""global b"";
var c = ""global c"";
{
  var a = ""outer a"";
  var b = ""outer b"";
  {
    var a = ""inner a"";
    print a;
    print b;
    print c;
  }
  print a;
  print b;
  print c;
}
print a;
print b;
print c;
";
            this.Interpret(source);
            this.AssertPrints("inner a",
                              "outer b",
                              "global c",
                              "outer a",
                              "outer b",
                              "global c",
                              "global a",
                              "global b",
                              "global c");
        }
    }
}
