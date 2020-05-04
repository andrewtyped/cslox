using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test.interpreter
{
    [TestClass]
    public class AssignInterpretationTest : InterpreterTestBase
    {
        #region Instance Methods

        [TestMethod]
        public void CanInterpretAssignExpressions()
        {
            var source = @"var foo; foo = 2;";
            this.Interpret(source);
            this.AssertVariable("foo",
                                2d);
        }

        [TestMethod]
        public void CanInterpretChainedAssignExpressions()
        {
            var source = @"var foo; var bar; foo = bar = 2;";
            this.Interpret(source);
            this.AssertVariable("foo",
                                2d);
            this.AssertVariable("bar",
                                2d);
        }

        #endregion
    }
}