using lox.constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test
{
    [TestClass]
    public class LoxInstanceTest
    {
        #region Instance Methods

        [TestMethod]
        public void LoxInstanceStoresALoxClass()
        {
            var @class = new LoxClass("foo");
            var instance = new LoxInstance(@class);
            Assert.AreEqual(instance.Class,
                            @class);
        }

        [TestMethod]
        public void LoxInstanceToStringReturnsClassNameAndIndicatesInstance()
        {
            var @class = new LoxClass("foo");
            var instance = new LoxInstance(@class);
            Assert.AreEqual("foo instance",
                            instance.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(RuntimeError))]
        public void LoxInstanceThrowsRuntimeErrorWhenGettingUndefinedFieldValue()
        {
            var @class = new LoxClass("foo");
            var instance = new LoxInstance(@class);
            var source = "bar";
            var token = new Token(0,
                                  2,
                                  0,
                                  TokenType.IDENTIFIER);

            instance.Get(token,
                         source);
        }
        #endregion
    }
}