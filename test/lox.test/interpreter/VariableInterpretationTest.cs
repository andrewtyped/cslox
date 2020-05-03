﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        #endregion
    }
}