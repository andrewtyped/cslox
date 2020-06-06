using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test.interpreter
{
    [TestClass]
    public class VariableInterpretationTest : InterpreterTestBase
    {
        #region Instance Methods

        [TestMethod]
        public void CanInterpretVariableDeclarationWithInitializer()
        {
            var source = @"var foo = ""hello"";";

            this.Interpret(source);

            this.AssertVariable("foo",
                                "hello");
        }

        [TestMethod]
        public void CanInterpretVariableRedeclarationWithInitializer()
        {
            var source = @"var foo = ""hello""; var foo = ""world"";";

            this.Interpret(source);

            this.AssertVariable("foo",
                                "world");
        }

        [TestMethod]
        public void CanInterpretVariableDeclarationWithoutInitializer()
        {
            var source = "var foo;";

            this.Interpret(source);

            this.AssertVariable("foo",
                                null);
        }

        [TestMethod]
        public void CanInterpretVariableRedeclarationWithoutInitializer()
        {
            var source = "var foo; var foo;";

            this.Interpret(source);

            this.AssertVariable("foo",
                                null);
        }

        [TestMethod]
        public void CanInterpretMultipleVariableDeclarations()
        {
            var source = @"var foo = ""bar""; var baz = 2; var bat = nil;";

            this.Interpret(source);

            this.AssertVariable("foo",
                                "bar");
            this.AssertVariable("baz",
                                2d);
            this.AssertVariable("bat",
                                null);
        }

        [TestMethod]
        public void CanInterpretVariableExpression()
        {
            var source = @"var foo = 1; print foo;";

            this.Interpret(source);

            this.AssertPrints("1");
        }

        [TestMethod]
        public void CanDeclareAVariableTwiceInGlobalScope()
        {
            var source = @"var foo = 1; var foo = 2;";
            this.Interpret(source);
            this.AssertVariable("foo",
                                2d);
        }

        [TestMethod]
        public void CannotDeclareAVariableTwiceInTheSameLocalScope()
        {
            var source = @"{var foo = 1; var foo = 2;}";
            this.AssertResolutionError(source,
                                       "Variable 'foo' is already declared in this scope.");
        }

        #endregion
    }
}