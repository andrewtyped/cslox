using lox.constants;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test.interpreter
{
    [TestClass]
    public class GetInterpretationTest : InterpreterTestBase
    {
        #region Instance Methods

        [TestMethod]
        public void CannotInterpretGetExpressionForUndefinedProperty()
        {
            var source = @"
class foo {}
print foo().bar;
";

            this.AssertRuntimeError(source,
                                    TokenType.IDENTIFIER,
                                    "Undefined property bar on object foo");
        }

        #endregion
    }
}