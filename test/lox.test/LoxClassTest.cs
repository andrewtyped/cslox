﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var @class = this.GetLoxClass();
            Assert.AreEqual("foo",
                            @class.Name);
        }

        [TestMethod]
        public void LoxClassHasASuperclass()
        {
            var @class = this.GetLoxClass();
            Assert.AreEqual("bar",
                            @class.Superclass!.Name);
        }

        [TestMethod]
        public void LoxClassHasMethods()
        {
            var function = new LoxFunction("source",
                                           new Stmt.Function(new Token(),
                                                             new List<Token>(),
                                                             new List<Stmt>()),
                                           new Environment(),
                                           false);
            var methodMap = new Dictionary<string, LoxFunction>
                            {
                                ["bar"] = function
                            };
            var @class = new LoxClass("foo",
                                      null,
                                      methodMap);

            Assert.AreSame(function,
                           @class.Methods["bar"]);
        }

        [TestMethod]
        public void LoxClassCanFindMethods()
        {
            var function = new LoxFunction("source",
                                           new Stmt.Function(new Token(),
                                                             new List<Token>(),
                                                             new List<Stmt>()),
                                           new Environment(),
                                           false);
            var methodMap = new Dictionary<string, LoxFunction>
                            {
                                ["bar"] = function
                            };
            var @class = new LoxClass("foo",
                                      null,
                                      methodMap);

            var method = @class.FindMethod("bar");

            Assert.AreSame(method,
                           @class.Methods["bar"]);
        }

        [TestMethod]
        public void LoxClassToStringIsLoxClassName()
        {
            var @class = this.GetLoxClass();
            Assert.AreEqual("foo",
                            @class.ToString());
        }

        [TestMethod]
        public void LoxClassHasArity0()
        {
            var @class = this.GetLoxClass();
            Assert.AreEqual(0,
                            @class.Arity());
        }

        private LoxClass GetLoxClass()
        {
            var superclass = new LoxClass("bar",
                                          null,
                                          new Dictionary<string, LoxFunction>());
            return new LoxClass("foo",
                                superclass,
                                new Dictionary<string, LoxFunction>());
        }
    }
}
