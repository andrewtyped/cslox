using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace lox.test
{
    [TestClass]
    public class LoxClassTest
    {
        [TestMethod]
        public void LoxClassHasAName()
        {
            var @class = new LoxClass("foo");
            Assert.AreEqual("foo",
                            @class.Name);
        }

        [TestMethod]
        public void LoxClassToStringIsLoxClassName()
        {
            var @class = new LoxClass("foo");
            Assert.AreEqual("foo",
                            @class.ToString());
        }

        [TestMethod]
        public void LoxClassHasArity0()
        {
            var @class = new LoxClass("foo");
            Assert.AreEqual(0,
                            @class.Arity());
        }
    }
}
