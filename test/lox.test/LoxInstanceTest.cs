using lox.constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace lox.test
{
    [TestClass]
    public class LoxInstanceTest
    {
        #region Instance Methods

        [TestMethod]
        public void LoxInstanceStoresALoxClass()
        {
            var @class = this.GetLoxClass();
            var instance = new LoxInstance(@class);
            Assert.AreEqual(instance.Class,
                            @class);
        }

        [TestMethod]
        public void LoxInstanceToStringReturnsClassNameAndIndicatesInstance()
        {
            var @class = this.GetLoxClass();
            var instance = new LoxInstance(@class);
            Assert.AreEqual("foo instance",
                            instance.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(RuntimeError))]
        public void LoxInstanceThrowsRuntimeErrorWhenGettingUndefinedFieldValue()
        {
            var @class = this.GetLoxClass();
            var instance = new LoxInstance(@class);
            var source = "bar";
            var token = new Token(0,
                                  2,
                                  0,
                                  TokenType.IDENTIFIER);

            instance.Get(token,
                         source);
        }

        [TestMethod]
        public void LoxInstanceSetsFieldValue()
        {
            var @class = this.GetLoxClass();
            var instance = new LoxInstance(@class);
            var source = "bar";
            var token = new Token(0,
                                  2,
                                  0,
                                  TokenType.IDENTIFIER);

            instance.Set(token,
                         1d,
                         source);

            var value = instance.Get(token,
                                     source);
            Assert.AreEqual(1d,
                            value);

        }

        private LoxClass GetLoxClass()
        {
            return new LoxClass("foo",
                                new Dictionary<string, LoxFunction>());
        } 

        #endregion
    }
}