using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test.interpreter
{
    [TestClass]
    public class WhileInterpretationTest : InterpreterTestBase
    {
        #region Instance Methods

        [TestMethod]
        public void CanInterpretWhileStatements()
        {
            var source = @"
var i = 0;

while( i < 3)
{
  print i;
  i = i + 1;
}
";
            this.Interpret(source);
            this.AssertPrints("0",
                              "1",
                              "2");
        }

        [TestMethod]
        public void WhileBodyCanExecuteZeroTimesWithFalseCondition()
        {
            var source = @"
var i = 0;

while(false)
{
  print i;
  i = i + 1;
}
";
            this.AssertPrints();
        }

        #endregion
    }
}