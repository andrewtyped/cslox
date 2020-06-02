using System;
using System.Collections.Generic;
using System.Linq;

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
            this.astPrinter = new AstPrinter();
        }

        protected virtual void AssertAst(string source,
                                         string expectedAst)
        {
            var scannedSource = this.Scan(source);
            var stmts = this.parser.Parse(scannedSource);
            var actualAst = this.astPrinter.Print(stmts,
                                                  scannedSource.Source);

            Assert.AreEqual(expectedAst,
                            actualAst.Trim());
        }

        protected T AssertExpr<T>(string source)
            where T : Expr
        {
            var scannedSource = this.Scan(source);
            return this.AssertExpr<T>(scannedSource);
        }

        protected T AssertExpr<T>(ScannedSource source)
            where T : Expr
        {
            var stmts = this.parser.Parse(source);

            return stmts.Single() switch
            {
                Stmt.Expression expr => this.AssertExpr<T>(expr.expression),
                Stmt.Print print => this.AssertExpr<T>(print.expression),
                Stmt.Return @return => this.AssertExpr<T>(@return.value),
                _ => throw new InvalidOperationException($"Unrecognized stmt type ")
            };
        }

        protected T AssertExpr<T>(Expr? expr)
            where T : Expr
        {
            Assert.IsNotNull(expr);

            var expectedExpr = expr as T;

            Assert.IsNotNull(expectedExpr,
                             $"Expected expr to be of type {typeof(T)} but was {expr!.GetType()}");

            return expectedExpr!;
        }

        protected void AssertParseErrors2(string source,
                                         params string[] containsMessages)
        {
            this.Parse(source);
            this.AssertParseErrors(containsMessages);
        }

        protected void AssertParseErrors(params string[] containsMessages)
        {
            var parseErrors = this.parser.ParseErrors.ToList();

            Assert.AreEqual(containsMessages.Length,
                            parseErrors.Count);

            for (int i = 0;
                 i < containsMessages.Length;
                 i++)
            {
                var parseError = parseErrors[i];
                var containsMessage = containsMessages[i];
                Assert.IsTrue(parseError.Message.Contains(containsMessage),
                              $"Expected parse error '{parseError.Message}' to contain text '{containsMessage}'");
            }
        }

        protected T AssertStmt<T>(string source)
            where T : Stmt
        {
            var scannedSource = this.Scan(source);
            var stmts = this.parser.Parse(scannedSource);

            Assert.AreEqual(1,
                            stmts.Count,
                            "Expected a single statement");
            Assert.AreEqual(typeof(T),
                            stmts[0]
                                .GetType());
            return (T)stmts[0];
        }

        protected T AssertStmt<T>(Stmt statement)
            where T : Stmt
        {
            Assert.AreEqual(typeof(T),
                            statement
                                .GetType());
            return (T)statement;
        }

        protected List<Stmt> AssertStmts(string source,
                                         params Type[] expectedSmtTypes)
        {
            var scannedSource = this.Scan(source);
            var stmts = this.parser.Parse(scannedSource);

            Assert.AreEqual(expectedSmtTypes.Length,
                            stmts.Count,
                            "Stmts length");

            for (int i = 0;
                 i < stmts.Count;
                 i++)
            {
                Assert.AreEqual(expectedSmtTypes[i],
                                stmts[i]
                                    .GetType(),
                                $"Stmt type at index {i}");
            }

            return stmts;
        }

        protected List<Stmt> Parse(string source)
        {
            var scannedSource = this.Scan(source);
            var stmts = this.parser.Parse(scannedSource);
            return stmts;
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

        protected AstPrinter astPrinter;
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    }
}