using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using static lox.Stmt;

namespace lox.test.parser
{
    [TestClass]
    public class ExpressionStmtTest : ParserTestBase
    {
        #region Instance Methods

        [TestMethod]
        public void CanDetectMissingSemicolons()
        {
            var source = @"
1 + 1;
2 + 1}";
            this.AssertParseErrors2(source,
                                    "Expect ';'");
        }

        [DataTestMethod]
        [DynamicData("GetTestStatements",
                     DynamicDataSourceType.Method)]
        public void CanParseExpressionStmts(string source,
                                            int expectedStatements)
        {
            var types = new List<Type>();

            for (int i = 0;
                 i < expectedStatements;
                 i++)
            {
                types.Add(typeof(Expression));
            }

            var stmts = this.AssertStmts(source,
                                         types.ToArray());

            foreach (var stmt in stmts)
            {
                Assert.IsNotNull(((Expression)stmt).expression);
            }
        }

        #endregion

        #region Class Methods

        private static IEnumerable<object[]> GetTestStatements()
        {
            yield return new object[]
                         {
                             @"
1 + 1;
""string"";
nil;",
                             3
                         };
        }

        #endregion
    }
}