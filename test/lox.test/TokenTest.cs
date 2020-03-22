using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace lox.test
{
    [TestClass]
    public class TokenTest
    {
        [TestMethod]
        public void CanConstructToken()
        {
            var token = new Token();
        }
    }
}
