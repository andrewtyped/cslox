﻿using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test.parser
{
    [TestClass]
    public class ParserTestBase
    {
        #region Instance Methods

        [TestInitialize]
        public void TestInitialize()
        {
            this.scanner = new Scanner();
            this.parser = new Parser();
        }

        protected ScannedSource Scan(string source)
        {
            var tokens = this.scanner.ScanTokens(source);
            return new ScannedSource(tokens.ToArray(),
                                     source);
        }

        #endregion

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        protected Scanner scanner;

        protected Parser parser;
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    }
}