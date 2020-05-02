using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test.interpreter
{
    [TestClass]
    public class GroupingInterpretaionTest : InterpreterTestBase 
    {
        [DataTestMethod]
        [DataRow("(1)",
                 1d)]
        [DataRow("((42))",
                 42d)]
        [DataRow("((((((((100))))))))",
                 100d)]
        public void CanInterpretGroupedLiteralExpressions(string source,
                                                          object expectedValue)
        {
            var value = this.Interpret(source);
            Assert.AreEqual(expectedValue,
                            value);
        }
    }
}