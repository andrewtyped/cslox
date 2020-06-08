using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test.interpreter
{
    [TestClass]
    public class SetInterpretationTest : InterpreterTestBase
    {
        #region Instance Methods

        [TestMethod]
        public void InterpretsSetExpression()
        {
            var source = @"
class foo {}
var fooInstance = foo();
fooInstance.bar = ""hello world"";
print fooInstance.bar;";

            this.Interpret(source);
            this.AssertPrints("hello world");
        }

        #endregion
    }
}