using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test.interpreter
{
    [TestClass]
    public class IfInterpretationTest : InterpreterTestBase
    {
        #region Instance Methods

        [TestMethod]
        public void CanInterpretIfElseStatementsElseBranch()
        {
            var source = @"if( 1 == 0) { print ""hello""; } else { print ""world""; }";
            this.Interpret(source);
            this.AssertPrints("world");
        }

        [TestMethod]
        public void CanInterpretIfElseStatementsThenBranch()
        {
            var source = @"if( 1 == 1) { print ""hello""; } else { print ""world""; }";
            this.Interpret(source);
            this.AssertPrints("hello");
        }

        #endregion
    }
}