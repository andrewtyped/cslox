using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test.interpreter
{
    [TestClass]
    public class LogicalInterpretationTest : InterpreterTestBase
    {
        #region Instance Methods

        [TestMethod]
        public void InterpretsChainsOfAndExpressions()
        {
            var source = "print true and true and true and true and false;";
            this.Interpret(source);
            this.AssertPrints("False");
        }

        [TestMethod]
        public void InterpretsChainsOfOrExpressions()
        {
            var source = "print false or false or false or false or true;";
            this.Interpret(source);
            this.AssertPrints("True");
        }

        [TestMethod]
        public void InterpretsFalseLogicAndExpressions()
        {
            var source = "print 1 > 0 and 5 > 10;";
            this.Interpret(source);
            this.AssertPrints("False");
        }

        [TestMethod]
        public void InterpretsFalseLogicOrExpressions()
        {
            var source = "print 1 == 0 or 2 > 3;";
            this.Interpret(source);
            this.AssertPrints("False");
        }

        [TestMethod]
        public void InterpretsTrueLogicAndExpressions()
        {
            var source = "print 1 > 0 and 5 < 10;";
            this.Interpret(source);
            this.AssertPrints("True");
        }

        [TestMethod]
        public void InterpretsTrueLogicOrExpressions()
        {
            var source = "print true or false;";
            this.Interpret(source);
            this.AssertPrints("True");
        }

        [TestMethod]
        public void LogicAndReturnsFalsyLeftValue()
        {
            var source = "print nil and 1;";
            this.Interpret(source);
            this.AssertPrints("nil");
        }

        [TestMethod]
        public void LogicAndReturnsFalsyRightValue()
        {
            var source = "print true and nil;";
            this.Interpret(source);
            this.AssertPrints("nil");
        }

        [TestMethod]
        public void LogicAndReturnsTruthyRightValue()
        {
            var source = "print 1 and 2;";
            this.Interpret(source);
            this.AssertPrints("2");
        }

        [TestMethod]
        public void LogicAndShortCircuitsAfterSeeingFalseExpression()
        {
            var source = "print false and 1 / 0;";
            this.Interpret(source);
            this.AssertPrints("False");
        }

        [TestMethod]
        public void LogicOrReturnsFalsyRightValue()
        {
            var source = "print false or nil;";
            this.Interpret(source);
            this.AssertPrints("nil");
        }

        [TestMethod]
        public void LogicOrReturnsTruthyLeftValue()
        {
            var source = "print 1 or true;";
            this.Interpret(source);
            this.AssertPrints("1");
        }

        [TestMethod]
        public void LogicOrReturnsTruthyRightValue()
        {
            var source = "print false or 1;";
            this.Interpret(source);
            this.AssertPrints("1");
        }

        [TestMethod]
        public void LogicOrShortCircuitsAfterSeeingTrueExpression()
        {
            var source = "print true or 1 / 0;";
            this.Interpret(source);
            this.AssertPrints("True");
        }

        #endregion
    }
}