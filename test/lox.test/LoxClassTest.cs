using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}
