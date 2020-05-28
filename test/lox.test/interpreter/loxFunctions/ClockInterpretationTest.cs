using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test.interpreter.loxFunctions
{
    [TestClass]
    public class ClockInterpretationTest : InterpreterTestBase
    {
        #region Instance Methods

        [TestMethod]
        public void CanCallClockFunction()
        {
            var source = "print clock();";
            this.Interpret(source);

            Assert.AreEqual(1,
                            this.Console.Writes.Count,
                            "Write count");

            var clock = double.Parse(this.Console.Writes[0]);

            Assert.IsTrue(clock > 0.0d,
                          "Expected clock() to return a non-zero double");
        }

        #endregion
    }
}